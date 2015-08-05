using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Interfacing
{
    public class Channel
    {
        public const Byte Idle = 0;
        public const Byte Process = 1;

        private List<IPacket> channel;
        public Byte Status { get; set; }

        public Channel()
        {
            this.channel = new List<IPacket>();
            this.Status = Idle;
        }

        public void Push(IPacket c)
        {
            if (c == null) return;
            this.channel.Add(c);
            this.Status = Process;
        }

        public IPacket Pop()
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
