using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Memory
{
    public class Memory64 : IMemory<UInt64>
    {
        private readonly UInt64[] memory;
        private readonly Int32 length;

        public Memory64(Int32 i)
        {
            this.memory = new UInt64[i];
            this.length = i;
        }

        public void Reset()
        {
            for (var i = 0; i < this.memory.Length; i++)
                this.memory[i] = 0;
        }

        public UInt64 GetMemory(Int32 index)
        {
            return this.memory[index % this.length];
        }

        public UInt64 GetMemory(UInt32 index)
        {
            return this.memory[index % this.length];
        }

        public void SetMemory(Int32 index, UInt64 value)
        {
            this.memory[index % this.length] = value;
        }

        public Int32 GetLength()
        {
            return this.memory.Length;
        }       
    }
}
