using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Peripherals
{
    public class Timer : IPeripheral
    {
        private Int32 timer;
        private Int32 limit;

        public Timer()
        {
            this.timer = 0;
            this.limit = 20; //Later From Memory Connection
        }

        public void Tick()
        {
            this.timer++;
        }

        public bool Trigger()
        {
            var trigger = this.timer >= this.limit;
            this.timer = 0;
            return trigger;
        }
    }
}
