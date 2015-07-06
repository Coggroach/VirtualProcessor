using System;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;
     
    public class Processor
    {
        private const Int32 ControlMemorySize = 64;
        private const Int32 UserMemorySize = 32;
        private const String ControlMemoryLocation = "Software\\ControlMemory.txt";
        private const String UserMemoryLocation = "Software\\UserMemory.txt";

        private Datapath datapath;
        private MemoryUnit controlMemory;
        private MemoryUnit userMemory;
        private Brancher branchControl;
        private UInt64 instructionReg;
        public Processor()
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit(ControlMemorySize, ControlMemoryLocation);
            this.userMemory = new MemoryUnit(UserMemorySize, UserMemoryLocation);
            this.branchControl = new Brancher();
            this.instructionReg = this.userMemory.GetMemory();
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
            return this.userMemory.GetRegister();
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
            return this.userMemory;
        }

        public void Restart()
        {
            this.Refresh();
            this.datapath.StartUp();
            this.instructionReg = this.userMemory.GetMemory();
        }

        public void Refresh()
        {
            this.userMemory.StartUp();
            this.controlMemory.StartUp();
        }
        
        public void Tick()
        { 
            //Split Ir into Opcode, Channel A and B, Destination           

            var opcode = (UInt32) BitHelper.BitExtract(this.instructionReg, 16, 0xFFFF);
            var dest = (Byte)(this.controlMemory.BitExtractMemory(2) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 8, 0xF));
            var srcA = (Byte)(this.controlMemory.BitExtractMemory(1) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 4, 0xF));
            var srcB = (Byte)(this.controlMemory.BitExtractMemory(0) == 1 ? 0xF : BitHelper.BitExtract(this.instructionReg, 0, 0xF)); 
            var halfMem = (UInt32) BitHelper.BitExtract(this.instructionReg, 0, 0xF); 

            //Split Control into Parts
            var ld = (Byte)(this.controlMemory.BitExtractMemory(3));            // 3
            var pc = this.controlMemory.BitExtractMemory(4, 3);                 // 5:4
            var lCar = this.controlMemory.BitExtractMemory(6, 3);               // 7:6
            var branch = this.controlMemory.BitExtractMemory(8, 0xF) + Opcode.B_BASE;      // 11:8
            var il = this.controlMemory.BitExtractMemory(12);
            var constIn = this.controlMemory.BitExtractMemory(13) == 1;       
            var dataIn = this.controlMemory.BitExtractMemory(14) == 1;          
            var updateBranch = this.controlMemory.BitExtractMemory(15);             

            var fs = (Byte)(this.controlMemory.BitExtractMemory(16, 0x1F));   

            var na = this.controlMemory.BitExtractMemory(48, 0xFFFF);            // 63:48   
            
            //Set up Datapath
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.SetConstIn(halfMem);
            if (dataIn && ld == 1) this.datapath.SetRegister(dest, (UInt32)this.userMemory.GetMemory());
            else this.datapath.FunctionUnit(fs, dest, ld, constIn);

            //Update Branch
            if (updateBranch == 1)
                this.branchControl.Nzcv = this.datapath.GetNzcv();

            //Set up CAR
            var muxCar = (lCar & 2) == 2 ? opcode : na;
            if(this.branchControl.Branch(branch) && (lCar & 1) == 0) 
                this.controlMemory.SetRegister(muxCar);
            else 
                this.controlMemory++;

            //Set up PC
            if((pc & 1) == 1)
                this.userMemory++;
            else if((pc & 2) == 2)
                this.userMemory += this.userMemory.BitExtractMemory(0, 0xFFF);

            //Set up IR
            if (il == 1) this.instructionReg = this.userMemory.GetMemory();
        }

    }
}
