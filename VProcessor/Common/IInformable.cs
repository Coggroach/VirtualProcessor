﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Memory;

namespace VProcessor.Common
{
    interface IInformable
    {
        UInt32[] GetRegisters();
        Byte GetStatusRegister();
        UInt32 GetProgramCounter();
        UInt32 GetControlAddressRegister();
        Register GetInstructionRegister();
        MemoryUnit<UInt32> GetFlashMemory();
        MemoryUnit<UInt64> GetControlMemory();
    }
}
