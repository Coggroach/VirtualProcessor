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

namespace VProcessor.Hardware
{
    public class Machine : IInformable
    {
        private Processor processor;
        private MemoryBus bus;
        private MemoryController controller;
        private IAssembler assembler;

        public Machine() : this(new Assembler())
        {

        }

        public Machine(IAssembler assembler)
        {
            this.assembler = assembler;
            this.processor = new Processor(
                this.assembler.Compile64(new VPFile(Settings.ControlMemoryLocation), Settings.ControlMemorySize), 
                this.assembler.Compile32(new VPFile(Settings.FlashMemoryLocation), Settings.FlashMemorySize));
            this.controller = new MemoryController();
            this.bus = new MemoryBus(this.processor, this.controller);
        }

        public void Tick()
        {
            this.processor.Tick();
            this.bus.Tick();            
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

        public UInt32 GetInstructionRegister()
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
