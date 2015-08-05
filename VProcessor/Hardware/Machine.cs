using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Software.Assembly;
using VProcessor.Tools;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Memory;
using VProcessor.Common;
using VProcessor.Hardware.Interrupts;
using VProcessor.Hardware.Peripherals;

namespace VProcessor.Hardware
{
    public class Machine : IInformable
    {
        private Processor processor;
        private MemoryController memory;
        private InterruptController interrupts;
        private IAssembler assembler;

        public Machine() 
            : this(new Assembler()) { }

        public Machine(IAssembler assembler)
        {
            this.assembler = assembler;
            this.processor = new Processor(
                this.assembler.Compile64(new VPFile(VPConsts.ControlMemoryLocation), VPConsts.ControlMemorySize), 
                this.assembler.Compile32(new VPFile(VPConsts.FlashMemoryLocation), VPConsts.FlashMemorySize));
            this.interrupts = new InterruptController(this.processor.GetInterruptChannel());
            this.memory = new MemoryController(this.processor.GetMemoryDualChannel());
        }

        public void Tick()
        {
            this.processor.Tick();
            this.memory.Tick();
            this.interrupts.Tick();
        }

        public void RegisterPeripheral(IPeripheral peripheral)
        {            
            this.interrupts.RegisterPeripheral(peripheral);
        }
       
        public Boolean HasTerminated()
        {
            return this.processor.HasTerminated();
        }

        public void Reset()
        {
            this.processor.Reset();
        }

        public void Reset(Memory32 m)
        {
            this.processor.Reset(m);
        }

        public UInt32[] GetRegisters()
        {
            return this.processor.GetRegisters();
        }

        public Byte GetStatusRegister()
        {
            return this.processor.GetStatusRegister();
        }

        public UInt32 GetProgramCounter()
        {
            return this.processor.GetProgramCounter();
        }

        public UInt32 GetControlAddressRegister()
        {
            return this.processor.GetControlAddressRegister();
        }

        public Register GetInstructionRegister()
        {
            return this.processor.GetInstructionRegister();
        }

        public MemoryUnit<UInt32> GetFlashMemory()
        {
            return this.processor.GetFlashMemory();
        }

        public MemoryUnit<UInt64> GetControlMemory()
        {
            return this.processor.GetControlMemory();
        }
    }
}
