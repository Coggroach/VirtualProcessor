using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Memory;
using VProcessor.Tools;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptController : ITickable
    {
        private Register irr;
        private Register isr;
        private Register imr;

        private Boolean waiting;

        private Channel interruptChannel;
        
        //0000 0000
        public UInt32 RequestInput { get; set; }        

        public InterruptController(InterruptChannel interrupt)
        {
            this.irr = new Register();
            this.isr = new Register();
            this.imr = new Register();
            this.waiting = true;
            this.interruptChannel = interrupt;
            this.RequestInput = 0;
        }

        public void Request(UInt32 irq)
        {
            if ((irq & this.imr.Value) != irq)
                return;

            this.irr.Value |= irq;


            if (irq != 0)
                this.waiting = false;
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
            this.waiting = true;

            return new InterruptPacket()
            {
                Address = bitPos,
                Request = InterruptPacketRequest.IRQ
            };
        }

        public void Tick()
        {
            this.Request(this.RequestInput);
            if (!this.waiting)
                this.interruptChannel.Push(this.CreateInterruptRequest());            
        }
    }
}
