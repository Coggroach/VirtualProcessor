using VProcessor.Common;
using VProcessor.Hardware.Interrupts;
using VProcessor.Hardware.Memory;
using VProcessor.Tools;

namespace VProcessor.Hardware.Components
{    
    public class Processor : IInformable
    {
        private readonly Datapath _datapath;
        private MemoryUnit<ulong> _controlMemory;
        private MemoryUnit<uint> _flashMemory;
        private readonly Brancher _branchControl;
        private readonly Register _instruction;
        private Register _status;
        private readonly MemoryDualChannel _memoryChannel;
        private readonly InterruptChannel _interruptChannel;
        private Interrupt _interrupt;
        private readonly Decoder _decoder;

        public MemoryChannelPacket ChannelPacket { get; set; }

        public Processor(Memory64 control, Memory32 flash)
        {
            _datapath = new Datapath();
            _controlMemory = new MemoryUnit<ulong>(control);
            _flashMemory = new MemoryUnit<uint>(flash);
            _status = new Register();
            _interrupt = new Interrupt();
            _decoder = new Decoder();
            _instruction = new Register(_flashMemory.GetMemory());
            _branchControl = new Brancher(_instruction);            
            _memoryChannel = new MemoryDualChannel();
            _interruptChannel = new InterruptChannel();
        }

        public MemoryDualChannel GetMemoryDualChannel()
        {
            return _memoryChannel;
        }

        public InterruptChannel GetInterruptChannel()
        {
            return _interruptChannel;
        }

        public uint[] GetRegisters()
        {
            return _datapath.GetRegisters();
        }

        public byte GetStatusRegister()
        {
            return (byte) _branchControl.GetNzcv().Value;
        }

        public uint GetProgramCounter()
        {
            return _flashMemory.GetRegister();
        }

        public uint GetControlAddressRegister()
        {
            return _controlMemory.GetRegister();
        }

        public MemoryUnit<ulong> GetControlMemory()
        {
            return _controlMemory;
        }

        public Register GetInstructionRegister()
        {
            return _instruction;
        }

        public MemoryUnit<uint> GetFlashMemory()
        {
            return _flashMemory;
        }

        public void Reset(Memory32 flash)
        {
            _flashMemory.SetMemory(flash);
            Reset();
        }

        public void Reset()
        {
            _flashMemory.Reset();
            _controlMemory.Reset();
            _datapath.Reset();
            _instruction.Value = _flashMemory.GetMemory();
            _status = new Register();
            _interrupt = new Interrupt();
        }

        public bool HasTerminated()
        {
            return _instruction.Value == 0;
        }
    
        public void SetInstructionRegister(uint i)
        {
            _instruction.Value = i;
        }
                
        public void Tick()
        {
            _decoder.Memory = _controlMemory.GetMemory();
            _decoder.Decode(_instruction);

            //Check if Allowed to Execute
            if(_decoder.ExecutionMode)// == this.datapath.GetMode())
            {
                var logic = _decoder.Mode == (byte)_datapath.GetMode()
                    || _datapath.ValidMode((DatapathMode)_decoder.Mode);
                if (!logic)
                    throw new MachineException("Invalid Mode to Execute Command");
            }
            
            //Set up Datapath
            _datapath.SetChannel(Datapath.ChannelA, _decoder.ChannelA);
            _datapath.SetChannel(Datapath.ChannelB, _decoder.ChannelB);
            _datapath.SetChannel(Datapath.ChannelD, _decoder.ChannelD);
            _datapath.SetConstIn(_decoder.ConstantIn, _decoder.Constant);

            //Change Mode
            if (_decoder.Mode != 0) 
                _datapath.SetMode((DatapathMode)_decoder.Mode);  
        
            //Check if Interrupt Polled
            var interrupt = ((InterruptPacket)_interruptChannel.Pop());
            if(interrupt != null && _interrupt.Enable && _interrupt.Accepting)
            {
                if(interrupt.Request == InterruptPacketRequest.Irq)
                {
                    _interrupt.Address = _flashMemory.GetRegister();
                    _interrupt.Accepting = false;
                    _interrupt.Mode = _datapath.GetMode();
                    _datapath.SetMode(DatapathMode.Interupt);
                    _flashMemory.SetRegister(interrupt.Address);
                    _controlMemory.SetRegister(0);
                    _instruction.Value = _flashMemory.GetMemory();
                    return;
                }
            }

            //Check if Interrupt Complete
            if (_decoder.EndOfInterrupt && _interrupt.Enable)
            {                
                _flashMemory.SetRegister(_interrupt.Address);
                _interrupt.Accepting = true;
                _datapath.SetMode(_interrupt.Mode);
            }
            if (_decoder.EndOfInterrupt && !_interrupt.Enable)
            {
                _interrupt.Enable = _interrupt.Accepting = true;
                _flashMemory++;
            }                

            //Move Data in Datapath
            var dataOut = (uint) 0;
            if (_decoder.DataIn && _decoder.LoadRegister) 
                _datapath.SetRegister(_decoder.ChannelD, _flashMemory.GetMemory());
            else if (_memoryChannel.MemoryPullRequest == MemoryDualChannelRequest.Complete && _decoder.LoadRegister)
            {
                _datapath.SetRegister(_decoder.ChannelD, ChannelPacket.Value);
                _memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.None;
            }
            else if (_decoder.LoadPc && _decoder.LoadRegister)
                _datapath.SetRegister(_decoder.ChannelD, _flashMemory.GetRegister());
            else 
                dataOut = _datapath.FunctionUnit(_decoder.FunctionCode, _decoder.LoadRegister);

            //Set up CAR
            var muxCar = (_decoder.CarControl & 2) == 2 ? _decoder.Opcode : _decoder.NextAddress;
            if (_decoder.MemoryInterrupt)
            {
                if (_memoryChannel.MemoryPullRequest == MemoryDualChannelRequest.Complete)
                    _controlMemory++;
            }
            else if ((_decoder.CarControl & 1) == 0)
                _controlMemory.SetRegister(muxCar);
            else
                _controlMemory++;


            //Moving Data to RAM
            if (_decoder.MemoryIn ^ _decoder.MemoryOut && _memoryChannel.MemoryPullRequest != MemoryDualChannelRequest.Complete)
            {
                ChannelPacket = new MemoryChannelPacket
                {
                    Value = dataOut,
                    Address = (int)_datapath.GetRegister(),
                    Offset = (int)_datapath.GetRegister(Datapath.ChannelB)
                };
                if (_decoder.MemoryOut) 
                    _memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.Push;
                if (_decoder.MemoryIn) 
                    _memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.Pull;
                _memoryChannel.PushOutput(ChannelPacket);
            }
            
            //Set up PC            
            if (_decoder.StorePc && !_decoder.LoadPc)            
                _flashMemory.SetRegister(dataOut);            
            else if ((_decoder.ProgramControl & 2) == 2 && _branchControl.Branch(_decoder.FunctionCode))
            {
                var extract = BitHelper.BitExtract(_instruction.Value, 0, 0xFFFF);
                _flashMemory += BitHelper.Negate4Bits(extract);
            }
            else if ((_decoder.ProgramControl & 1) == 1)
                _flashMemory++;

            //Update Branch
            if (_decoder.BranchUpdate == 1) _status = new Register(_datapath.GetStatusRegister(), 0xF);

            //Set up IR
            if (_decoder.StoreInstruction == 1) _instruction.Value = _flashMemory.GetMemory();
        }        
    }
}
