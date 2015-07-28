using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Peripherals
{
    public class Timer : IPeripheral, ITickable
    {
        private Int32 timer;

        public Timer()
        {

        }

        public void Tick()
        {
            this.timer++;
        }
    }
}
