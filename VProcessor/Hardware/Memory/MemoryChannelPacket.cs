using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannelPacket
    {
        public Int32 Address { get; set; }
        public Int32 Offset { get; set; }
        public UInt32 Value { get; set; }     
    }
}
