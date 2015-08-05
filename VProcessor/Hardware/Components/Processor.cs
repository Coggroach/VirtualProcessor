using System;
using VProcessor.Tools;
using VProcessor.Hardware.Memory;
using VProcessor.Common;
using VProcessor.Hardware.Interrupts;

namespace VProcessor.Hardware.Components
{    
    public class Processor : IInformable
    {
        private Datapath datapath;
        private MemoryUnit<UInt64> controlMemory;
        private MemoryUnit<UInt32> flashMemory;
        private Brancher branchControl;
        private Register instruction;
        private Register status;
        private MemoryDualChannel memoryChannel;
        private InterruptChannel interruptChannel;
        private Register interrupt;
        private Decoder decoder;

        public MemoryChannelPacket ChannelPacket { get; set; }

        public Processor(Memory64 control, Memory32 flash)
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit<UInt64>(control);
            this.flashMemory = new MemoryUnit<UInt32>(flash);
            this.status = new Register();
            this.interrupt = new Register();
            this.decoder = new Decoder();
            this.instruction = new Register(this.flashMemory.GetMemory());
            this.branchControl = new Brancher(this.instruction);            
            this.memoryChannel = new MemoryDualChannel();
            this.interruptChannel = new InterruptChannel();
        }

        public MemoryDualChannel GetMemoryDualChannel()
        {
            return this.memoryChannel;
        }

        public InterruptChannel GetInterruptChannel()
        {
            return this.interruptChannel;
        }

        public UInt32[] GetRegisters()
        {
            return this.datapath.GetRegisters();
        }

        public Byte GetStatusRegister()
        {
            return (Byte) this.branchControl.GetNzcv().Value;
        }

        public UInt32 GetProgramCounter()
        {
            return this.flashMemory.GetRegister();
        }

        public UInt32 GetControlAddressRegister()
        {
            return this.controlMemory.GetRegister();
        }

        public MemoryUnit<UInt64> GetControlMemory()
        {
            return this.controlMemory;
        }

        public Register GetInstructionRegister()
        {
            return this.instruction;
        }

        public MemoryUnit<UInt32> GetFlashMemory()
        {
            return this.flashMemory;
        }

        public void Reset(Memory32 flash)
        {
            this.flashMemory.SetMemory(flash);
            this.Reset();
        }

        public void Reset()
        {
            this.flashMemory.Reset();
            this.controlMemory.Reset();
            this.datapath.Reset();
            this.instruction.Value = this.flashMemory.GetMemory();
            this.status = new Register();
        }

        public Boolean HasTerminated()
        {
            return this.instruction.Value == 0;
        }
    
        public void SetInstructionRegister(UInt32 i)
        {
            this.instruction.Value = i;
        }
                
        public void Tick()
        {
            this.decoder.Memory = this.controlMemory.GetMemory();
            this.decoder.Decode(this.instruction);
            
            //Set up Datapath
            this.datapath.SetChannel(Datapath.ChannelA, this.decoder.ChannelA);
            this.datapath.SetChannel(Datapath.ChannelB, this.decoder.ChannelB);
            this.datapath.SetChannel(Datapath.ChannelD, this.decoder.ChannelD);
            this.datapath.SetConstIn(this.decoder.ConstantIn, this.decoder.Constant);

            //Change Mode
            if (this.decoder.Mode != 0) 
                this.datapath.SetMode(this.decoder.Mode);  
        
            //Check if Interrupt Polled
            var interrupt = ((InterruptPacket)this.interruptChannel.Pop());
            if(interrupt != null)
            {
                if(interrupt.Request == InterruptPacketRequest.IRQ)
                {
                    this.interrupt.Value = this.flashMemory.GetRegister();
                    this.datapath.SetMode(Datapath.Interupt);
                    this.ChannelPacket = new MemoryChannelPacket()
                    {
                        Value = 0,
                        Address = VPConsts.VectoredInterruptControllerAddress,
                        Offset = (Int32) interrupt.Address
                    };
                    this.memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.Push;
                    this.memoryChannel.PushOutput(this.ChannelPacket);
                    this.controlMemory.SetRegister((UInt32) OpcodeRegistry.Instance.GetCodeAddress("IRQ"));
                    return;
                }
            }

            //Check if Interrupt Complete
            if(this.decoder.EndOfInterrupt)            
                this.flashMemory.SetRegister(this.interrupt.Value);            

            //Move Data in Datapath
            var dataOut = (UInt32) 0;
            if (this.decoder.DataIn && this.decoder.LoadRegister) 
                this.datapath.SetRegister(this.decoder.ChannelD, (UInt32)this.flashMemory.GetMemory());
            else if (this.memoryChannel.MemoryPullRequest == MemoryDualChannelRequest.Complete && this.decoder.LoadRegister)
            {
                this.datapath.SetRegister(this.decoder.ChannelD, this.ChannelPacket.Value);
                this.memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.None;
            }
            else if (this.decoder.LoadPc && this.decoder.LoadRegister)
                this.datapath.SetRegister(this.decoder.ChannelD, this.flashMemory.GetRegister());
            else 
                dataOut = this.datapath.FunctionUnit(this.decoder.FunctionCode, this.decoder.LoadRegister);

            //Set up CAR
            var muxCar = (this.decoder.CarControl & 2) == 2 ? this.decoder.Opcode : this.decoder.NextAddress;
            if (this.decoder.MemoryInterrupt)
            {
                if (this.memoryChannel.MemoryPullRequest == MemoryDualChannelRequest.Complete)
                    this.controlMemory++;
            }
            else if ((this.decoder.CarControl & 1) == 0)
                this.controlMemory.SetRegister(muxCar);
            else
                this.controlMemory++;


            //Moving Data to RAM
            if (this.decoder.MemoryIn ^ this.decoder.MemoryOut && this.memoryChannel.MemoryPullRequest != MemoryDualChannelRequest.Complete)
            {
                this.ChannelPacket = new MemoryChannelPacket()
                {
                    Value = dataOut,
                    Address = (Int32)this.datapath.GetRegister(Datapath.ChannelA),
                    Offset = (Int32)this.datapath.GetRegister(Datapath.ChannelB)
                };
                if (this.decoder.MemoryOut) 
                    this.memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.Push;
                if (this.decoder.MemoryIn) 
                    this.memoryChannel.MemoryPullRequest = MemoryDualChannelRequest.Pull;
                this.memoryChannel.PushOutput(this.ChannelPacket);
            }
            
            //Set up PC            
            if (this.decoder.StorePc && !this.decoder.LoadPc)            
                this.flashMemory.SetRegister(dataOut);            
            else if ((this.decoder.ProgramControl & 2) == 2 && this.branchControl.Branch(this.decoder.FunctionCode))
            {
                var extract = (UInt32)BitHelper.BitExtract(this.instruction.Value, 0, 0xFFFF);
                this.flashMemory += BitHelper.Negate4Bits(extract);
            }
            else if ((this.decoder.ProgramControl & 1) == 1)
                this.flashMemory++;

            //Update Branch
            if (this.decoder.BranchUpdate == 1) this.status = new Register(this.datapath.GetStatusRegister(), 0xF);

            //Set up IR
            if (this.decoder.StoreInstruction == 1) this.instruction.Value = this.flashMemory.GetMemory();
        }        
    }
}
