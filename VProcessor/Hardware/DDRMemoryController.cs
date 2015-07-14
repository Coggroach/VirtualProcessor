using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Tools;

namespace VProcessor.Hardware
{
    public class DDRMemoryController
    {
        private Memory32 memory;

        public DDRMemoryController()
        {
            this.memory = new Memory32(Settings.RandomAccessMemorySize);
        }

        public UInt32 Read(Int32 address, Int32 offset = 0)
        {
            return this.memory.GetMemory(address + offset);
        }

        public void Write(Int32 address, UInt32 value, Int32 offset = 0)
        {
            this.memory.SetMemory(address + offset, value);
        }
    }
}
