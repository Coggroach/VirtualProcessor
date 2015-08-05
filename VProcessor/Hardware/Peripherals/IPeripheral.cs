using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Peripherals
{
    public interface IPeripheral : ITickable
    {
        Boolean Trigger();
    }
}
