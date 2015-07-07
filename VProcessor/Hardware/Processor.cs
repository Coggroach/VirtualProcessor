using System;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;
    using VProcessor.Software.Assembly;
     
    public class Processor
    {
        private Datapath datapath;
        private MemoryUnit controlMemory;
        private MemoryUnit flashMemory;
        private Brancher branchControl;
        private UInt64 instructionReg;
        public Processor(Memory control, Memory flash)
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit(control);
            this.flashMemory = new MemoryUnit(flash);
            this.branchControl = new Brancher();
            this.instructionReg = this.flashMemory.GetMemory();
        }

        public UInt32[] GetRegisters()
        {
            return this.datapath.GetRegisters();
        }

        public Byte GetNzcv()
        {
            return this.branchControl.Nzcv;
        }

        public UInt32 GetProgramCounter()
        {
            return this.flashMemory.GetRegister();
        }

        public UInt32 GetControlAddressRegister()
        {
            return this.controlMemory.GetRegister();
        }

        public MemoryUnit GetControlMemory()
        {
            return this.controlMemory;
        }

        public UInt64 GetInstructionRegister()
        {
            return this.instructionReg;
        }

        public MemoryUnit GetUserMemory()
        {
            return this.flashMemory;
        }

        public void Reset(Memory flash)
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
            this.branchControl.Nzcv = 0;
        }
                
        public void Tick()
        { 
            //Split Ir into Opcode, Channel A and B, Destination  
            var opcode = (UInt32) BitHelper.BitExtract(this.instructionReg, 16, 0xFFFF);
            var dest = (Byte)(this.controlMemory.BitExtractMemory(2) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 8, 0xF));
            var srcA = (Byte)(this.controlMemory.BitExtractMemory(1) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 4, 0xF));
            var srcB = (Byte)(this.controlMemory.BitExtractMemory(0) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 0, 0xF)); 
            var cnst = (UInt32) BitHelper.BitExtract(this.instructionReg, 0, 0xF); 

            //Split Control into Parts
            var Lr = (Byte)(this.controlMemory.BitExtractMemory(3));            // 3
            var Pc = this.controlMemory.BitExtractMemory(4, 3);                 // 5:4
            var Cion = this.controlMemory.BitExtractMemory(6, 3);               // 7:6
            var Bx = this.controlMemory.BitExtractMemory(8, 0xF) + Opcode.B_BASE;      // 11:8
            var IL = this.controlMemory.BitExtractMemory(12);
            var Cin = this.controlMemory.BitExtractMemory(13) == 1;       
            var Din = this.controlMemory.BitExtractMemory(14) == 1;          
            var Bu = this.controlMemory.BitExtractMemory(15);             

            var fs = (Byte)this.controlMemory.BitExtractMemory(16, 0x1F);
            
            var na = this.controlMemory.BitExtractMemory(48, 0xFFFF);            // 63:48   
            
            //Set up Datapath
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.SetConstIn(cnst);
            if (Din && Lr == 1) this.datapath.SetRegister(dest, (UInt32)this.flashMemory.GetMemory());
            else this.datapath.FunctionUnit(fs, dest, Lr, Cin);

            //Update Branch
            if (Bu == 1)
                this.branchControl.Nzcv = this.datapath.GetNzcv();
            
            //Set up CAR
            var muxCar = (Cion & 2) == 2 ? opcode : na;
            if((Cion & 1) == 0) 
                this.controlMemory.SetRegister(muxCar);
            else
                this.controlMemory++;

            //Set up PC
            if ((Pc & 2) == 2 && this.branchControl.Branch(Bx))
                this.flashMemory += this.flashMemory.BitExtractMemory(0, 0xFFFF);
            else if((Pc & 1) == 1)
                this.flashMemory++;            

            //Set up IR
            if (IL == 1) this.instructionReg = this.flashMemory.GetMemory();
        }

    }
}
