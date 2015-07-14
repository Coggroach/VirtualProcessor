using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Memory32 : IMemory<UInt32>
    {
        private readonly UInt32[] memory;       

        public Memory32(Int32 i)
        {
            this.memory = new UInt32[i];
        }

        public void Reset()
        {
            for (var i = 0; i < this.memory.Length; i++)
                this.memory[i] = 0;
        }

        public UInt32 GetMemory(Int32 index)
        {
            return this.memory[index];
        }

        public UInt32 GetMemory(UInt32 index)
        {
            return this.memory[index];
        }

        public void SetMemory(Int32 index, UInt32 value)
        {
            this.memory[index] = value;
        }

        public Int32 GetLength()
        {
            return this.memory.Length;
        }    
    }
}
