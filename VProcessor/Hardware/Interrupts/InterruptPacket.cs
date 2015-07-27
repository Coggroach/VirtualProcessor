using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptPacket : IPacket
    {
        public UInt32 Address { get; set; }
        public InterruptPacketRequest Request { get; set; }
    }

    public enum InterruptPacketRequest
    {
        EOI = 2,
        IRQ = 1,
        Idle = 0
    }
}
