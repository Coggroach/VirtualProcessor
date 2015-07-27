using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannel : Channel
    {
        public MemoryChannel() : base() { }

        public void Push(Int32 a, UInt32 v, Int32 o)
        {
            this.Push(new MemoryChannelPacket
            {
                Address = a,
                Value = v,
                Offset = o
            });
        }       
    }
}
