using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannelPacket : IPacket
    {
        public Int32 Address { get; set; }
        public Int32 Offset { get; set; }
        public UInt32 Value { get; set; }     
    }
}
