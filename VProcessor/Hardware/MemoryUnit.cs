using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VProcessor.Hardware
{
    public class MemoryUnit <T> where T : UInt32, UInt64
    {
        private T register;
        private T[] memory;
        private String path;
        public MemoryUnit(int i, String s)
        {
            this.memory = new T[i];
            this.path = s;
            this.StartUp();
        }
        public void StartUp()
        {
            this.register = 0;
            if(File.Exists(this.path))
            {
                using (StreamReader reader = File.OpenText(this.path))
                {
                    String input = "";
                    for (int i = 0; i < memory.Length; i++)
                    {
                        if ((input = reader.ReadLine()) != null)
                            this.memory[i] = T.Parse(input);
                        else
                            this.memory[i] = 0;                        

                    }                  
                }
            }
        }
        
        private Boolean BitMatch<T>(T value, Byte bitPos, Byte matchBit, Byte mask = 1)
        {
            return (value >> bitPos) & mask == matchBit;
        }
        
        public Boolean BitMatchMemory(Byte bitPos, Byte matchBit, Byte mask = 1)
        {
            return this.BitMatch(this.GetMemory(), bitPos, matchBit, mask);
        }
        
        public Boolean BitMatchRegister(Byte bitPos, Byte matchBit, Byte mask = 1)
        {
            return this.BitMatch(this.register, bitPos, matchBit, mask);
        }
        
        private T BitExtract<T>(T value, Byte bitPos, Byte mask = 1)
        {
            return (value >> bitPos) & mask;
        }
        
        public T BitExtractMemory(Byte bitPos, Byte mask = 1)
        {
            return this.BitExtract(this.GetMemory(), bitPos, mask);
        }
        
        public T BitExtractRegister(Byte bitPos, Byte mask = 1)
        {
            return this.BitExtract(this.register, bitPos, mask);
        }

        public T GetMemory()
        {
            return this.memory[this.register];
        }

        public T GetRegister()
        {
            return this.register;
        }

        public void Increment()
        {
            this.register++;
        }

        public void Add<T>(T i)
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
