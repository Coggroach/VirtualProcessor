using VProcessor.Hardware.Components;
using VProcessor.Hardware.Memory;

namespace VProcessor.Common
{
    interface IInformable
    {
        uint[] GetRegisters();
        byte GetStatusRegister();
        uint GetProgramCounter();
        uint GetControlAddressRegister();
        Register GetInstructionRegister();
        MemoryUnit<uint> GetFlashMemory();
        MemoryUnit<ulong> GetControlMemory();
        bool HasTerminated();
    }
}
