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

        public Color GetColor(Int32 index)
        {
            return this.GetColor(leds[index]);
        }

        private Color GetColor(Boolean state)
        {
            return state ? Color.Green : Color.Red;
        }

        public bool Trigger()
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public uint GetMemory(int index)
        {
            throw new NotImplementedException();
        }

        public uint GetMemory(uint index)
        {
            throw new NotImplementedException();
        }

        public void SetMemory(int index, uint value)
        {
            throw new NotImplementedException();
        }

        public int Length
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasMemory
        {
            get { throw new NotImplementedException(); }
        }
    }
}
