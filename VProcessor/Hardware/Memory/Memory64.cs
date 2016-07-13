namespace VProcessor.Hardware.Memory
{
    public class Memory64 : IMemory<ulong>
    {
        private readonly ulong[] _memory;
        private readonly int _length;

        public Memory64(int i)
        {
            _memory = new ulong[i];
            _length = i;
        }

        public void Reset()
        {
            for (var i = 0; i < _memory.Length; i++)
                _memory[i] = 0;
        }

        public ulong GetMemory(int index) => _memory[index % _length];

        public ulong GetMemory(uint index) => _memory[index % _length];

        public void SetMemory(int index, ulong value) => _memory[index % _length] = value;

        public int Length => _memory.Length;

        public bool HasMemory => true;
    }
}
