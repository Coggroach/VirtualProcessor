using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Memory
{
    public enum MemoryDualChannelRequest
    {
        None = 0,
        Pull = 1,
        Complete = 2,
        Push = 3
    }

    public class MemoryDualChannel
    {
        private MemoryChannel input;
        private MemoryChannel output;

        public Byte Status;
        public MemoryDualChannelRequest MemoryPullRequest { get; set; }

        public MemoryDualChannel()
        {
            this.input = new MemoryChannel();
            this.output = new MemoryChannel();
            this.Status = MemoryChannel.Idle;
        }

        private void UpdateStatus()
        {
            this.Status = (Byte)(this.input.Status | this.output.Status);
        }

        public void PushInput(MemoryChannelPacket packet)
        {
            this.input.Push(packet);
            this.UpdateStatus();
        }

        public void PushOutput(MemoryChannelPacket packet)
        {
            this.output.Push(packet);
            this.UpdateStatus();
        }

        public MemoryChannelPacket PopInput()
        {
            var pop =  this.input.Pop();
            this.UpdateStatus();
            return (MemoryChannelPacket) pop;
        }

        public MemoryChannelPacket PopOutput()
        {
            var pop = this.output.Pop();
            this.UpdateStatus();
            return (MemoryChannelPacket) pop;
        }
    }
}
