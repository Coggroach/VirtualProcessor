using System;
using System.Globalization;
using System.IO;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;
    using VProcessor.Software.Assembly;

    public class MemoryUnit<T>
    {
        private UInt32 register;
        private Memory<T> memory;
        public MemoryUnit(Memory<T> m)
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
      
        public void SetMemory(Memory<T> m)
        {
            this.memory = m;
        }

        public T GetMemory()
        {
            return this.memory.GetMemory(this.register);
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

        public static MemoryUnit<T> operator ++(MemoryUnit<T> memory)
        {
            memory.Increment();
            return memory;
        }

        public static MemoryUnit<T> operator +(MemoryUnit<T> memory, UInt32 i)
        {
            memory.Add(i);
            return memory;
        }

    }
}
