using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;

namespace VProcessor.Hardware.Interfacing
{
    public interface IDatapath
    {
        void Reset();
        void SetRegister(Byte register, UInt32 value);        
        UInt32[] GetRegisters();
        UInt32 GetRegister(Byte register);
    }
}
