using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Memory;
using VProcessor.Hardware.Peripherals;
using VProcessor.Tools;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptController : ITickable
    {
        private Register irr;
        private Register isr;
        private Register imr;

        private Channel interruptChannel;
        private IList<IPeripheral> peripherals;       

        public InterruptController(InterruptChannel interrupt)
        {
            this.irr = new Register();
            this.isr = new Register();
            this.imr = new Register();
            this.interruptChannel = interrupt;
            this.peripherals = new List<IPeripheral>();
        }

        public void Request(UInt32 irq)
        {
            if ((irq & this.imr.Value) != irq)
                return;

            this.irr.Value |= irq;
        }

        private void Service(Byte bitPos)
        {
            this.isr.ClrBit(bitPos);
        }

        private Byte RetrieveNextRequest()
        {
            Byte i = 0;
            var value = this.irr.Value;

            if (value == 0) return VPConsts.VectoredInterruptControllerAddress + 1;

            while(value > 1)
            {
                value >>= 1;
                i++;
            }
            return i;
        }

        private void Mask()
        {
            this.imr.Value = ~(this.irr.Value | this.isr.Value);
        }

        public InterruptPacket CreateInterruptRequest()
        {
            var bitPos = RetrieveNextRequest();

            if (bitPos > VPConsts.VectoredInterruptControllerAddress)
                return null;

            this.irr.ClrBit(bitPos);
            this.isr.SetBit(bitPos);
            this.Mask();

            return new InterruptPacket()
            {
                Address = bitPos,
                Request = InterruptPacketRequest.IRQ
            };
        }

        public void RegisterPeripheral(IPeripheral peripheral)
        {
            this.peripherals.Add(peripheral);
        }

        private UInt32 RequestInput()
        {
            var request = (UInt32) 0;
            for(var i = 0; i < this.peripherals.Count; i++)
            {
                if (this.peripherals[i].Trigger())
                    request |= (UInt32) (1 << i);
                //Needs to Have IDs in Future
            }
            return request;
        }

        public void Tick()
        {
            foreach (var peri in this.peripherals)
                peri.Tick();
            this.Request(this.RequestInput());
            this.interruptChannel.Push(this.CreateInterruptRequest());        
        }
    }
}
