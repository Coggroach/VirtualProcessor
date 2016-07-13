using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannelPacket : IPacket
    {
        public int Address { get; set; }
        public int Offset { get; set; }
        public uint Value { get; set; }             
    }
}
