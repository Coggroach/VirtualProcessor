using VProcessor.Tools;

namespace VProcessor.Hardware.Peripherals
{
    public class LedBoard : IPeripheral
    {
        private readonly bool[] _leds;

        public LedBoard()
        {
            _leds = new bool[VpConsts.LedBoardCount];
        }

        public bool GetColor(int index)
        {
            return _leds[index];
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
            for (var i = 0; i < _leds.Length; i++)
                _leds[i] = false;
        }

        public uint GetMemory(int index)
        {
            return GetMemory((uint)index);
        }

        public uint GetMemory(uint index)
        {
            return (uint)(_leds[index] ? 1 : 0);
        }

        public void SetMemory(int index, uint value)
        {
            _leds[index] = value != 0;
        }

        public int Length => VpConsts.LedBoardCount;

        public bool HasMemory => true;
    }
}
