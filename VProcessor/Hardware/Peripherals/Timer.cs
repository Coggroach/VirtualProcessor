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
        private UInt32 limit;
        private Boolean enable;

        public Timer()
        {
            this.timer = 0;
            this.enable = false;
            this.limit = 20;
        }

        public void Tick()
        {
            if(this.enable)
                this.timer++;
        }

        public bool Trigger()
        {
            var trigger = this.timer >= this.limit && this.enable;
            this.timer = 0;
            return trigger;
        }

        public void Reset()
        {
            this.limit = 0;
            this.enable = false;
        }

        public UInt32 GetMemory(int index)
        {
            return this.GetMemory((UInt32)index);
        }

        public UInt32 GetMemory(uint index)
        {
            if (index == 0)
                return (UInt32)(this.enable ? 1 : 0);
            else
                return this.limit;
        }

        public void SetMemory(int index, UInt32 value)
        {
            if (index == 0)
                this.enable = value != 0;
            else
                this.limit = value;
        }

        public int Length
        {
            get { return 2; }
        }

        public bool HasMemory
        {
            get { return true; }
        }
    }
}
