using System;
using VProcessor.Tools;
using VProcessor.Software.Assembly;
using VProcessor.Hardware.Memory;
using VProcessor.Common;

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
        private MemoryDualChannel channel;

        public MemoryChannelPacket ChannelPacket { get; set; }

        public Processor(Memory64 control, Memory32 flash)
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit<UInt64>(control);
            this.flashMemory = new MemoryUnit<UInt32>(flash);
            this.status = new Register();
            this.instruction = new Register(this.flashMemory.GetMemory());
            this.branchControl = new Brancher(this.instruction);            
            this.channel = new MemoryDualChannel();
        }

        public MemoryDualChannel GetMemoryDualChannel()
        {
            return this.channel;
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
            //Split Ir into Opcode, Channel A and B, Destination  
            var opcode = (UInt32)BitHelper.BitExtract(this.instruction.Value, 16, 0xFFFF);
            var dest = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 2, 1) ? 0xF : BitHelper.BitExtract(this.instruction.Value, 8, 0xF));
            var srcA = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 1, 1) ? 0xF : BitHelper.BitExtract(this.instruction.Value, 4, 0xF));
            var srcB = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 0, 1) ? 0xF : BitHelper.BitExtract(this.instruction.Value, 0, 0xF));
            var cnst = (UInt32)BitHelper.BitExtract(this.instruction.Value, 0, 0xF); 

            //Split Control into Parts
            var Lr = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 3));            // 3
            var Pc = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 4, 3));         // 5:4
            var Cion = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 6, 3));       // 7:6

            var Cmem = BitHelper.BitMatch(this.controlMemory.GetMemory(), 8, 1);
            var Min = BitHelper.BitMatch(this.controlMemory.GetMemory(), 9, 1);
            var Mout = BitHelper.BitMatch(this.controlMemory.GetMemory(), 10, 1);
           
            var IL = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 12));
            var Cin = BitHelper.BitMatch(this.controlMemory.GetMemory(), 13, 1);
            var Din = BitHelper.BitMatch(this.controlMemory.GetMemory(), 14, 1);
            var Bu = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 15));

            var fs = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 16, 0x3F));
            var stpc = BitHelper.BitMatch(this.controlMemory.GetMemory(), 22, 1);
            var ldpc = BitHelper.BitMatch(this.controlMemory.GetMemory(), 23, 1);

            var mode = (Byte)(BitHelper.BitExtract(this.controlMemory.GetMemory(), 24, 0xF));

            var na = (UInt32)BitHelper.BitExtract(this.controlMemory.GetMemory(), 48, 0xFFFF); // 63:48   
            
            //Set up Datapath
            this.datapath.SetChannel(Datapath.ChannelA, srcA);
            this.datapath.SetChannel(Datapath.ChannelB, srcB);
            this.datapath.SetChannel(Datapath.ChannelD, dest);
            this.datapath.SetConstIn(Cin, cnst);

            //Change Mode
            if(mode != 0) this.datapath.SetMode(mode);
            


            //Move Data in Datapath
            var dataOut = (UInt32) 0;
            if (Din && Lr == 1) this.datapath.SetRegister(dest, (UInt32)this.flashMemory.GetMemory());
            else if (this.channel.MemoryPullRequest == MemoryDualChannelRequest.Complete && Lr == 1)
            {
                this.datapath.SetRegister(dest, this.ChannelPacket.Value);
                this.channel.MemoryPullRequest = MemoryDualChannelRequest.None;
            }
            else if (ldpc && Lr == 1)
                this.datapath.SetRegister(dest, this.flashMemory.GetRegister());
            else dataOut = this.datapath.FunctionUnit(fs, Lr);

            //Set up CAR
            var muxCar = (Cion & 2) == 2 ? opcode : na;
            if (Cmem)
            {
                if (this.channel.MemoryPullRequest == MemoryDualChannelRequest.Complete)
                    this.controlMemory++;
            }
            else if ((Cion & 1) == 0)
                this.controlMemory.SetRegister(muxCar);
            else
                this.controlMemory++;


            //Moving Data to RAM
            if (Min ^ Mout && this.channel.MemoryPullRequest != MemoryDualChannelRequest.Complete)
            {
                this.ChannelPacket = new MemoryChannelPacket()
                {
                    Value = dataOut,
                    Address = (Int32)this.datapath.GetRegister(Datapath.ChannelA),
                    Offset = (Int32)this.datapath.GetRegister(Datapath.ChannelB)
                };
                if (Mout) this.channel.MemoryPullRequest = MemoryDualChannelRequest.Push;
                if (Min) this.channel.MemoryPullRequest = MemoryDualChannelRequest.Pull;
                this.channel.PushOutput(this.ChannelPacket);
            }
            
            //Set up PC
            if(stpc && !ldpc)            
                this.flashMemory.SetRegister(dataOut);            
            else if ((Pc & 2) == 2 && this.branchControl.Branch(fs))
            {
                var extract = (UInt32)BitHelper.BitExtract(this.instruction.Value, 0, 0xFFFF);

                this.flashMemory += BitHelper.Negate4Bits(extract);
            }
            else if ((Pc & 1) == 1)
                this.flashMemory++;

            //Update Branch
            if (Bu == 1) this.status = new Register(this.datapath.GetStatusRegister(), 0xF);

            //Set up IR
            if (IL == 1) this.instruction.Value = this.flashMemory.GetMemory();
        }
    }
}
