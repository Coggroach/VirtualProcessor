using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptController : ITickable
    {
        private Register irr;
        private Register isr;
        private Register imr;

        private UInt32[] addresses;
        private Boolean waiting;

        public InterruptPacket Packet { get; set; }
        public UInt32 RequestId { get; set; }

        private const Byte Int32Size = 32;

        public InterruptController()
        {
            this.irr = new Register();
            this.isr = new Register();
            this.imr = new Register();
            this.addresses = new UInt32[Int32Size];
            this.waiting = false;
        }

        public void SetAddress(Byte index, UInt32 value)
        {
            this.addresses[index] = value;
        }

        public void Request(UInt32 irq)
        {
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

            if (value == 0) return Int32Size + 1;

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

            if (bitPos > Int32Size)
                return null;

            this.irr.ClrBit(bitPos);
            this.isr.SetBit(bitPos);
            this.Mask();          

            return new InterruptPacket()
            {
                Address = this.addresses[bitPos],
                Request = InterruptPacketRequest.IRQ
            };
        }

        public void Tick()
        {
            this.Request(this.RequestId);
            if(!this.waiting)
               this.Packet = this.CreateInterruptRequest();                       
        }
    }
}
