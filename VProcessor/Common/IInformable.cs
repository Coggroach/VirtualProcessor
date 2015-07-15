using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    interface IInformable
    {
        UInt32[] GetRegisters();
        Byte GetStatusRegister();
        UInt32 GetProgramCounter();
        UInt32 GetControlAddressRegister();
        UInt32 GetInstructionRegister();
        MemoryUnit<UInt32> GetFlashMemory();
        MemoryUnit<UInt64> GetControlMemory();
    }
}
