using System;
using System.Globalization;
using System.IO;

namespace VProcessor.Hardware
{
    using VProcessor.Tools;

    public class MemoryUnit
    {
        private UInt32 register;
        private readonly UInt64[] memory;
        private readonly String path;
        public MemoryUnit(Int32 i, String s)
        {
            this.memory = new UInt64[i];
            this.path = s;
            this.StartUp();
        }
        public void StartUp()
        {
            this.register = 0;
            if (!File.Exists(this.path)) return;
            using (var reader = File.OpenText(this.path))
            {
                for (var i = 0; i < this.memory.Length; i++)
                {
                    var input = reader.ReadLine();
                    if (!String.IsNullOrWhiteSpace(input))
                        this.memory[i] = UInt64.Parse(input.Replace(" ", ""), NumberStyles.HexNumber);
                    else
                        this.memory[i] = 0;
                }
            }
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

        public UInt64 GetMemory()
        {
            return this.memory[this.register];
        }

        public Int32 GetLength()
        {
            return this.memory.Length;
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

        public void Decrement()
        {
            this.register--;
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

        public static MemoryUnit operator --(MemoryUnit memory)
        {
            memory.Decrement();
            return memory;
        }

        public static MemoryUnit operator +(MemoryUnit memory, UInt32 i)
        {
            memory.Add(i);
            return memory;
        }

    }
}
