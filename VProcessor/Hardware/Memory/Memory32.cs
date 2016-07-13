namespace VProcessor.Hardware.Memory
{
    public class Memory32 : IMemory<uint>
    {
        private readonly uint[] _memory;       

        public Memory32(int i)
        {
            _memory = new uint[i];
        }

        public void Reset()
        {
            for (var i = 0; i < _memory.Length; i++)
                _memory[i] = 0;
        }

        public uint GetMemory(int index) => _memory[index];

        public uint GetMemory(uint index) => _memory[index];

        public void SetMemory(int index, uint value) => _memory[index] = value;

        public int Length => _memory.Length;

        public static Memory32 operator +(Memory32 a, Memory32 b)
        {
            var length = a.Length + b.Length;
            var mem32 = new Memory32(length);

            for(var i = 0; i < a.Length; i++)
                mem32.SetMemory(i, a.GetMemory(i));
            for (var i = 0; i < b.Length; i++)
                mem32.SetMemory(i + a.Length, b.GetMemory(i));
            return mem32;
        }

        public bool HasMemory => true;
    }
}
