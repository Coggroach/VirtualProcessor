using VProcessor.Common;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Interrupts;
using VProcessor.Hardware.Memory;
using VProcessor.Hardware.Peripherals;
using VProcessor.Software;
using VProcessor.Tools;

namespace VProcessor.Hardware
{
    public class Machine : IInformable
    {
        private readonly Processor _processor;
        private readonly MemoryController _memory;
        private readonly InterruptController _interrupts;
        private readonly IAssembler _assembler;

        public Machine() 
            : this(new Assembler()) { }

        public Machine(IAssembler assembler)
        {
            _assembler = assembler;
            _processor = new Processor(
                _assembler.Compile64(new VpFile(VpConsts.ControlMemoryLocation), VpConsts.ControlMemorySize), 
                _assembler.Compile32(new VpFile(VpConsts.FlashMemoryLocation), VpConsts.FlashMemorySize));
            _interrupts = new InterruptController(_processor.GetInterruptChannel());
            _memory = new MemoryController(_processor.GetMemoryDualChannel());
            _memory.RegisterMappedMemory(_interrupts);
        }
        
        public void Tick()
        {
            _processor.Tick();
            _memory.Tick();
            _interrupts.Tick();
        }

        public void RegisterPeripheral(IPeripheral peripheral)
        {            
            _interrupts.RegisterPeripheral(peripheral);
            if (peripheral.HasMemory)
                _memory.RegisterMappedMemory(peripheral);
        }
       
        public bool HasTerminated() => _processor.HasTerminated();

        public void Reset() => _processor.Reset();

        public void Reset(Memory32 m) => _processor.Reset(m);

        public uint[] GetRegisters() => _processor.GetRegisters();

        public byte GetStatusRegister() => _processor.GetStatusRegister();

        public uint GetProgramCounter() => _processor.GetProgramCounter();

        public uint GetControlAddressRegister() => _processor.GetControlAddressRegister();

        public Register GetInstructionRegister() => _processor.GetInstructionRegister();

        public MemoryUnit<uint> GetFlashMemory() => _processor.GetFlashMemory();

        public MemoryUnit<ulong> GetControlMemory() => _processor.GetControlMemory();
    }
}
