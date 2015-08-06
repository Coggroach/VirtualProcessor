using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Tools;

namespace VProcessor.Hardware.Peripherals
{
    public class LEDBoard : IPeripheral
    {
        private Boolean[] leds;

        public LEDBoard()
        {
            this.leds = new Boolean[VPConsts.LEDBoardCount];
        }

        public Boolean GetColor(Int32 index)
        {
            return leds[index];
        }

        public bool Trigger()
        {
            return false;
        }

        public void Tick()
        {
            
        }

        public void Reset()
        {
            for (var i = 0; i < leds.Length; i++)
                this.leds[i] = false;
        }

        public UInt32 GetMemory(int index)
        {
            return this.GetMemory((UInt32)index);
        }

        public UInt32 GetMemory(uint index)
        {
            return (UInt32)(this.leds[index] ? 1 : 0);
        }

        public void SetMemory(int index, uint value)
        {
            this.leds[index] = value != 0;
        }

        public int Length
        {
            get { return VPConsts.LEDBoardCount; }
        }

        public bool HasMemory
        {
            get { return true; }
        }
    }
}
