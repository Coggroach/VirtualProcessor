using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Components
{
    class RegisterFile : IDatapath
    {
        private UInt32[] registers;

        public RegisterFile()
        {
            this.registers = new UInt32[Datapath.RegisterFileSize];
        }

        public void Reset()
        {
            for (var i = 0; i < registers.Length; i++)
                this.registers[i] = 0;
        }

        public void SetRegister(Byte register, UInt32 value)
        {
            this.registers[register] = value;
        }

        public UInt32[] GetRegisters()
        {
            return this.registers;
        }

        public UInt32 GetRegister(Byte register)
        {
            return this.registers[register];
        }
    }
}
