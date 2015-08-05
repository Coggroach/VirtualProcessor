using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VProcessor.Hardware.Interfacing;
using VProcessor.Hardware.Memory;

namespace VProcessor.Hardware.Peripherals
{
    public interface IPeripheral : ITickable, IMemory<UInt32>
    {
        Boolean Trigger();
    }
}
