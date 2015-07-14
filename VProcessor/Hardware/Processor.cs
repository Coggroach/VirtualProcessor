using System;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;
    using VProcessor.Software.Assembly;
     
    public class Processor : IInformable
    {
        private Datapath datapath;
        private MemoryUnit<UInt64> controlMemory;
        private MemoryUnit<UInt32> flashMemory;
        private Brancher branchControl;
        private UInt32 instructionReg;
        private MemoryConnector connector;

        public Processor(Memory64 control, Memory32 flash)
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit<UInt64>(control);
            this.flashMemory = new MemoryUnit<UInt32>(flash);
            this.branchControl = new Brancher();
            this.instructionReg = this.flashMemory.GetMemory();
            this.connector = new MemoryConnector();
        }

        public MemoryConnector GetMemoryConnector()
        {
            return this.connector;
        }

        private void SetMemoryCommand(Boolean Min, Boolean Mout)
        {
            if (Min && !Mout)
                this.connector.Command = MemoryConnector.Fetch;
            else if (!Min && Mout)
                this.connector.Command = MemoryConnector.Store;
            else
                this.connector.Command = MemoryConnector.Idle;
        }

        public UInt32[] GetRegisters()
        {
            return this.datapath.GetRegisters();
        }

        public Byte GetStatusRegister()
        {
            return this.branchControl.GetNzcv().Nzcv;
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

        public UInt32 GetInstructionRegister()
        {
            return this.instructionReg;
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
            this.instructionReg = this.flashMemory.GetMemory();
            this.branchControl.SetNzcv(new StatusRegister());
        }

        public Boolean HasTerminated()
        {
            return this.instructionReg == 0;
        }
    
        public void SetInstructionRegister(UInt32 i)
        {
            this.instructionReg = i;
        }
                
        public void Tick()
        { 
            //Split Ir into Opcode, Channel A and B, Destination  
            var opcode = (UInt32) BitHelper.BitExtract(this.instructionReg, 16, 0xFFFF);
            var dest = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 2, 1) ? 0xF : BitHelper.BitExtract(this.instructionReg, 8, 0xF));
            var srcA = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 1, 1) ? 0xF : BitHelper.BitExtract(this.instructionReg, 4, 0xF));
            var srcB = (Byte)(BitHelper.BitMatch(this.controlMemory.GetMemory(), 0, 1) ? 0xF : BitHelper.BitExtract(this.instructionReg, 0, 0xF)); 
            var cnst = (UInt32) BitHelper.BitExtract(this.instructionReg, 0, 0xF); 

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

            var na = (UInt32)BitHelper.BitExtract(this.controlMemory.GetMemory(), 48, 0xFFFF); // 63:48   
            
            //Set up Datapath
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.SetConstIn(cnst);

            //Move Data in Datapath
            if (Din && Lr == 1) this.datapath.SetRegister(dest, (UInt32)this.flashMemory.GetMemory());
            else if (this.connector.Command == MemoryConnector.Received && Lr == 1) this.datapath.SetRegister(dest, this.connector.Value);
            else this.datapath.FunctionUnit(fs, dest, Lr, Cin);

            if (this.connector.Command == MemoryConnector.Store)
                this.connector.Value = this.datapath.GetRegister(0);                
            
            this.connector.Address = (Int32)this.datapath.GetRegister(1);
            this.SetMemoryCommand(Min, Mout);

            //Set up CAR
            var muxCar = (Cion & 2) == 2 ? opcode : na;
            if (Cmem)
            {
                if (this.connector.Command == MemoryConnector.Received)
                {
                    this.controlMemory++;
                    this.connector.Flush();
                }
                //else wait
            }
            else if ((Cion & 1) == 0)
                this.controlMemory.SetRegister(muxCar);
            else
                this.controlMemory++;

            //Set up PC
            if ((Pc & 2) == 2 && this.branchControl.Branch(fs))
            {
                var extract = (UInt32) BitHelper.BitExtract(this.instructionReg, 0, 0xFFFF);

                if(extract >= 0x8000)
                    extract = ~(~extract ^ 0xFFFF0000);

                this.flashMemory += extract;
            }
            else if ((Pc & 1) == 1)
                this.flashMemory++;

            //Update Branch
            if (Bu == 1)
                this.branchControl.SetNzcv(this.datapath.GetStatusRegister());

            //Set up IR
            if (IL == 1) this.instructionReg = this.flashMemory.GetMemory();
        }

    }
}
