using System;
using System.Globalization;
using System.IO;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;
    using VProcessor.Software.Assembly;

    public class MemoryUnit
    {
        private UInt32 register;
        private Memory memory;
        public MemoryUnit(Memory m)
        {
            this.Init();
            this.memory = m;
        } 

        private void Init()
        {
            this.register = 0;
        }

        public void Reset()
        {
            this.Init();            
        }
      
        public void SetMemory(Memory m)
        {
            this.memory = m;
        }

        public UInt64 GetMemory()
        {
            return this.memory.GetMemory(this.register);
        }

        public Boolean BitMatchMemory(Byte bitPos, Byte matchBit, UInt32 mask = 1)
        {
            return BitHelper.BitMatch(this.GetMemory(), bitPos, matchBit, mask);
        }

        public Boolean BitMatchRegister(Byte bitPos, Byte matchBit, UInt32 mask = 1)
        {
            return BitHelper.BitMatch(this.register, bitPos, matchBit, mask);
        }

        public UInt32 BitExtractMemory(Byte bitPos, UInt32 mask = 1)
        {
            return (UInt32)BitHelper.BitExtract(this.GetMemory(), bitPos, mask);
        }

        public UInt32 BitExtractRegister(Byte bitPos, UInt32 mask = 1)
        {
            return (UInt32) BitHelper.BitExtract(this.register, bitPos, mask);
        }       

        public UInt32 GetRegister()
        {
            return this.register;
        }
        
        public void SetRegister(UInt32 value)
        {
            this.register = value;
        }

        public void Increment()
        {
            this.register++;
        }

        public void Add(UInt32 i)
        {
            this.register += i;
        }

        public static MemoryUnit operator ++(MemoryUnit memory)
        {
            memory.Increment();
            return memory;
        }

        public static MemoryUnit operator +(MemoryUnit memory, UInt32 i)
        {
            memory.Add(i);
            return memory;
        }

    }
}
