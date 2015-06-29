using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VProcessor.Hardware
{
    public class MemoryUnit
    {
        private UInt32 register;
        private UInt32[] memory;
        private String path;
        public MemoryUnit(int i, String s)
        {
            this.memory = new UInt32[i];
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
                            this.memory[i] = UInt32.Parse(input);
                        else
                            this.memory[i] = 0;                        

                    }                  
                }
            }
        }
        
        public Boolean BitMatch(Byte bitPos, Byte matchBit, Byte mask = 1)
        {
            return (this.register >> bitPos) & mask == matchBit;
        }

        public UInt32 GetMemory()
        {
            return this.memory[this.register];
        }

        public UInt32 GetRegister()
        {
            return this.register;
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
