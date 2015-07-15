using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Tools;

namespace VProcessor.Hardware
{
    public class MemoryController
    {
        private Memory32 memory;

        public MemoryController()
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

        public Memory32 GetMemory()
        {
            return this.memory;
        }
    }
}
