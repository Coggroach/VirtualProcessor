using VProcessor.Hardware.Interfacing;

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
        private readonly MemoryChannel _input;
        private readonly MemoryChannel _output;

        public byte Status;
        public MemoryDualChannelRequest MemoryPullRequest { get; set; }

        public MemoryDualChannel()
        {
            _input = new MemoryChannel();
            _output = new MemoryChannel();
            Status = Channel.Idle;
        }

        private void UpdateStatus() => Status = (byte)(_input.Status | _output.Status);

        public void PushInput(MemoryChannelPacket packet)
        {
            _input.Push(packet);
            UpdateStatus();
        }

        public void PushOutput(MemoryChannelPacket packet)
        {
            _output.Push(packet);
            UpdateStatus();
        }

        public MemoryChannelPacket PopInput()
        {
            var pop =  _input.Pop();
            UpdateStatus();
            return (MemoryChannelPacket) pop;
        }

        public MemoryChannelPacket PopOutput()
        {
            var pop = _output.Pop();
            UpdateStatus();
            return (MemoryChannelPacket) pop;
        }
    }
}
