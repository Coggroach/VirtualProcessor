using System.Collections.Generic;

namespace VProcessor.Hardware.Interfacing
{
    public class Channel
    {
        public const byte Idle = 0;
        public const byte Process = 1;

        private readonly List<IPacket> _channel;
        public byte Status { get; set; }

        public Channel()
        {
            _channel = new List<IPacket>();
            Status = Idle;
        }

        public void Push(IPacket c)
        {
            if (c == null) return;
            _channel.Add(c);
            Status = Process;
        }

        public IPacket Pop()
        {
            if (IsEmpty())
            {
                Status = Idle;
                return null;
            }

            var component = _channel[0];
            _channel.RemoveAt(0);

            if (IsEmpty()) Status = Idle;

            return component;
        }

        public bool IsEmpty()
        {
            return _channel.Count == 0;
        }
    }
}
