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
            throw new NotImplementedException();
        }

        public void SetRegister(byte register, uint value)
        {
            throw new NotImplementedException();
        }
        
        public uint[] GetRegisters()
        {
            throw new NotImplementedException();
        }

        public uint GetRegister(byte register)
        {
            throw new NotImplementedException();
        }
    }
}
