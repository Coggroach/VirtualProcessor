namespace VProcessor.Hardware.Peripherals
{
    public class Timer : IPeripheral
    {
        private int _timer;
        private uint _limit;
        private bool _enable;

        public Timer()
        {
            _timer = 0;
            _enable = false;
            _limit = 20;
        }

        public void Tick()
        {
            if (Trigger())
                _timer = 0;
            if(_enable)
                _timer++;      
        }

        public bool Trigger() => _timer >= _limit && _enable;

        public void Reset()
        {
            _limit = 0;
            _enable = false;
        }

        public uint GetMemory(int index) => GetMemory((uint)index);

        public uint GetMemory(uint index)
        {
            if (index == 0)
                return (uint)(_enable ? 1 : 0);
            return _limit;
        }

        public void SetMemory(int index, uint value)
        {
            if (index == 0)
                _enable = value != 0;
            else
                _limit = value;
        }

        public int Length => 2;

        public bool HasMemory => true;
    }
}
