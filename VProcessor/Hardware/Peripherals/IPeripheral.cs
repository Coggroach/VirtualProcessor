using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Memory;

namespace VProcessor.Hardware.Peripherals
{
    public interface IPeripheral : ITickable, IMemory<uint>
    {
        bool Trigger();
    }
}
