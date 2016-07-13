using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Memory
{
    public class MemoryChannel : Channel
    {
        public void Push(int a, uint v, int o)
        {
            Push(new MemoryChannelPacket
            {
                Address = a,
                Value = v,
                Offset = o
            });
        }       
    }
}
