using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Memory
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

        public Int32 Length
        {
            get { return this.memory.Length; }
        }    

        public static Memory32 operator +(Memory32 a, Memory32 b)
        {
            var length = a.Length + b.Length;
            var mem32 = new Memory32(length);

            for(var i = 0; i < a.Length; i++)
                mem32.SetMemory(i, a.GetMemory(i));
            for (var i = 0; i < b.Length; i++)
                mem32.SetMemory(i + a.Length, b.GetMemory(i));
            return mem32;
        }


        public bool HasMemory
        {
            get { return true; }
        }
    }
}
