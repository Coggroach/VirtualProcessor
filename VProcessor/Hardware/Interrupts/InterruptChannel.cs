using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Interrupts
{
    public class InterruptChannel : Channel
    {
        public void Push(uint a, InterruptPacketRequest irq)
        {
            Push(new InterruptPacket
            {
                Address = a,
                Request = irq
            });
        }       
    }
}
