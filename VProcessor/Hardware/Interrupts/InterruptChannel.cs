using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptChannel : Channel
    {
        public InterruptChannel() 
            : base() {}

        public void Push(UInt32 a, InterruptPacketRequest irq)
        {
            this.Push(new InterruptPacket
            {
                Address = a,
                Request = irq
            });
        }       
    }
}
