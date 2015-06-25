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
        private MemoryUnit controlMemory;
        private MemoryUnit userMemory;
        private UInt32 instructionRegister;
        private Brancher branchControl;
        public Processor()
        {
            this.datapath = new Datapath();
            this.controlMemory = new MemoryUnit(ControlMemorySize, ControlMemoryLocation);
            this.userMemory = new MemoryUnit(UserMemorySize, UserMemoryLocation);
            this.instructionRegister = 0;
            this.branchControl = new Brancher();
        }

        public void Tick()
        {
            this.instructionRegister = userMemory.GetMemory();
            //Split Ir into Opcode, Channel A and B, Destination
            var car = controlMemory.GetRegister();
            var pc = userMemory.GetRegister();
            var control = controlMemory.GetMemory();
            //Split Control into Parts
            //MORE HARD PARST
        }

    }
}
