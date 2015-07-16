using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannel
    {
        public const Byte Idle = 0;        
        public const Byte Process = 1;

        private List<MemoryChannelPacket> channel;
        public Byte Status { get; set; } 

        public MemoryChannel()
        {
            this.channel = new List<MemoryChannelPacket>();
            this.Status = Idle;
        }

        public void Push(Int32 a, UInt32 v, Int32 o)
        {
            this.Push(new MemoryChannelPacket
            {
                Address = a,
                Value = v,
                Offset = o
            });
        }

        public void Push(MemoryChannelPacket c)
        {
            this.channel.Add(c);
            this.Status = Process;
        }

        public MemoryChannelPacket Pop()
        {
            if (this.IsEmpty())
            {
                this.Status = Idle;
                return null;
            }

            var component = this.channel[0];
            this.channel.RemoveAt(0);

            if (this.IsEmpty()) this.Status = Idle;

            return component;
        }

        public Boolean IsEmpty()
        {
            return this.channel.Count == 0;
        }
    }
}
