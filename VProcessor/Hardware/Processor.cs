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
            var pc = this.controlMemory.BitExtractMemory(3, 3);              // 4:3
            var fs = (Byte) (this.controlMemory.BitExtractMemory(5, 0x1F));  // 9:5
            var ld = (Byte) (this.controlMemory.BitExtractMemory(10));       // 10
            var lCar = this.controlMemory.BitExtractMemory(11, 3);          // 12:11
            var branch = this.controlMemory.BitExtractMemory(13, 0xF);      // 16:13
            var updateBranch = this.controlMemory.BitExtractMemory(14);     // 14       
            var constIn = this.controlMemory.BitExtractMemory(15) == 1;          // 15
            var na = this.controlMemory.BitExtractMemory(48, 0xFFFF);       // 63:48
            
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.SetConstIn(halfMem);
            this.datapath.FunctionUnit(ld, fs, dest, constIn);
            
            var muxCar = (lCar & 2) == 2 ? na : opcode;

            if(updateBranch == 1)
                this.branchControl.Nzcv = this.datapath.GetNzcv();

            if(this.branchControl.Branch(branch) && (lCar & 1) == 1) 
                this.controlMemory.SetRegister(muxCar);
            else 
                this.controlMemory++;
            
            if((pc & 1) == 1)
                this.userMemory++;
            if((pc & 2) == 2)
                this.userMemory += this.userMemory.BitExtractMemory(0, 0xFFF);
        }

    }
}
