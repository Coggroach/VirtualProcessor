using System;

namespace VProcessor.Hardware
{
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
        public Processor()
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit(ControlMemorySize, ControlMemoryLocation);
            this.userMemory = new MemoryUnit(UserMemorySize, UserMemoryLocation);
            this.branchControl = new Brancher();
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

        public MemoryUnit GetUserMemory()
        {
            return this.userMemory;
        }

        public void Refresh()
        {
            this.userMemory.StartUp();
            this.controlMemory.StartUp();
        }
        
        public void Tick()
        {
            // UMemory 16:4:4:4:4
            // Car:Unassigned:Dest:SrcA:SrcB
            // CMemory 16:3:2:1:5:2:3
            // Na:Unassigned:LoadCar:LoadReg:FS:PC:TD:TA:TB
            //Split Ir into Opcode, Channel A and B, Destination
            
            var opcode = this.userMemory.BitExtractMemory(16, 0xFFFF);
            var dest = (Byte) (this.controlMemory.BitExtractMemory(2) == 1 ? 0xF : this.userMemory.BitExtractMemory(8, 0xF)); 
            var srcA = (Byte) (this.controlMemory.BitExtractMemory(1) == 1 ? 0xF : this.userMemory.BitExtractMemory(4, 0xF));
            var srcB = (Byte) (this.controlMemory.BitExtractMemory(0) == 1 ? 0xF : this.userMemory.BitExtractMemory(0, 0xF));
            var halfMem = this.userMemory.BitExtractMemory(16, 0xFF);

            //Split Control into Parts
            var ld = (Byte)(this.controlMemory.BitExtractMemory(3));            // 3
            var pc = this.controlMemory.BitExtractMemory(4, 3);                 // 5:4
            var lCar = this.controlMemory.BitExtractMemory(6, 3);               // 7:6
            var branch = this.controlMemory.BitExtractMemory(8, 0xF) + Opcode.B_BASE;      // 11:8
            var fs = (Byte) (this.controlMemory.BitExtractMemory(12, 0x1F));    // 16:12
            var constIn = this.controlMemory.BitExtractMemory(17) == 1;         // 17
            var dataIn = this.controlMemory.BitExtractMemory(18) == 1;          // 18
            var updateBranch = this.controlMemory.BitExtractMemory(19);         // 19       
            
            var na = this.controlMemory.BitExtractMemory(48, 0xFFFF);            // 63:48
            
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.SetConstIn(halfMem);
            if (dataIn && ld == 1) this.datapath.SetRegister(dest, (UInt32)this.userMemory.GetMemory());
            else this.datapath.FunctionUnit(ld, fs, dest, constIn);
            
            var muxCar = (lCar & 2) == 2 ? na : opcode;

            if(updateBranch == 1)
                this.branchControl.Nzcv = this.datapath.GetNzcv();

            if(this.branchControl.Branch(branch) && (lCar & 1) == 0) 
                this.controlMemory.SetRegister(muxCar);
            else 
                this.controlMemory++;

            if((pc & 3) == 3)
                this.userMemory--;
            else if((pc & 1) == 1)
                this.userMemory++;
            else if((pc & 2) == 2)
                this.userMemory += this.userMemory.BitExtractMemory(0, 0xFFF);
        }

    }
}
