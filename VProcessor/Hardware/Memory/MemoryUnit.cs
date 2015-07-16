using System;
using System.Globalization;
using System.IO;

namespace VProcessor.Hardware.Memory
{
    using VProcessor.Tools;
    using VProcessor.Software.Assembly;

    public class MemoryUnit<T>
    {
        private UInt32 register;
        private IMemory<T> memory;
        public MemoryUnit(IMemory<T> m)
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
      
        public void SetMemory(IMemory<T> m)
        {
            this.memory = m;
        }

        public T GetMemory()
        {
            return this.memory.GetMemory(this.register);
        }

        public IMemory<T> GetMemoryChunk()
        {
            return this.memory;
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
