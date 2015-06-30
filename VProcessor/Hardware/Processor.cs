using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Processor
    {
        private const Int32 ControlMemorySize = 64;
        private const Int32 UserMemorySize = 32;
        private const String ControlMemoryLocation = "Software\\ControlMemory.txt";
        private const String UserMemoryLocation = "Software\\UserMemory.txt";

        private Datapath datapath;
        private MemoryUnit<UInt64> controlMemory;
        private MemoryUnit<UInt32> userMemory;
        private Brancher branchControl;
        public Processor()
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit<UInt64>(ControlMemorySize, ControlMemoryLocation);
            this.userMemory = new MemoryUnit<UInt32>(UserMemorySize, UserMemoryLocation);
            this.branchControl = new Brancher();
        }

        public void Tick()
        {
            // UMemory 16:4:4:4:4
            // Car:Unassigned:Dest:SrcA:SrcB
            // CMemory 16:3:2:1:5:2:3
            // Na:Unassigned:LoadCar:LoadReg:FS:PC:TD:TA:TB
            //Split Ir into Opcode, Channel A and B, Destination
            
            var opcode = this.userMemory.BitExtractMemory(16, 0xFFFF);
            var dest = this.controlMemory.BitExtractMemory(2) == 1 ? 0xF : this.userMemory.BitExtractMemory(8, 0xF); 
            var srcA = this.controlMemory.BitExtractMemory(1) == 1 ? 0xF : this.userMemory.BitExtractMemory(4, 0xF);
            var srcB = this.controlMemory.BitExtractMemory(0) == 1 ? 0xF : this.userMemory.BitExtractMemory(0, 0xF);
            
            //Split Control into Parts
            var pc = this.controlMemory.BitExtractMemory(3, 3);     // 4:3
            var fs = this.controlMemory.BitExtractMemory(5, 0x1F);  // 9:5
            var ld = this.controlMemory.BitExtractMemory(10);       // 10
            var lCar = this.controlMemory.BitExtractMemory(11, 3);  // 12:11
            var branch = this.controlMemory.BitExtractMemory(13, 0xF);  // 16:13
            var na = this.controlMemory.BitExtractMemory(48, 0xFFFF);   // 63:48
            
            this.datapath.SetChannel(0, srcA);
            this.datapath.SetChannel(1, srcB);
            this.datapath.FunctionUnit(ld, fs, dest);
            
            var muxCar = (lCar & 2) == 2 ? na : opcode;
            
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
