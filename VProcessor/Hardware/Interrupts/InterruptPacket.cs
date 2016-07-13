using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptPacket : IPacket
    {
        public uint Address { get; set; }
        public InterruptPacketRequest Request { get; set; }
    }

    public enum InterruptPacketRequest
    {
        Eoi = 2,
        Irq = 1,
        Idle = 0
    }
}
