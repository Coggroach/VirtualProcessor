using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Memory
    {
        private readonly UInt64[] memory;       

        public Memory(Int32 i)
        {
            this.memory = new UInt64[i];
        }

        public void Reset()
        {
            for (var i = 0; i < this.memory.Length; i++)
                this.memory[i] = 0;
        }

        public UInt64 GetMemory(Int32 index)
        {
            return this.memory[index];
        }

        public UInt64 GetMemory(UInt32 index)
        {
            return this.memory[index];
        }

        public void SetMemory(Int32 index, UInt64 value)
        {
            this.memory[index] = value;
        }

        public Int32 GetLength()
        {
            return this.memory.Length;
        }       
    }
}
