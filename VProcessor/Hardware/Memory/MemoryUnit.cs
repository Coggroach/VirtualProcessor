namespace VProcessor.Hardware.Memory
{

    public class MemoryUnit<T>
    {
        private uint _register;
        private IMemory<T> _memory;
        public MemoryUnit(IMemory<T> m)
        {
            Init();
            _memory = m;
        } 

        private void Init() => _register = 0;

        public void Reset() => Init();

        public void SetMemory(IMemory<T> m) => _memory = m;

        public T GetMemory() => _memory.GetMemory(_register);

        public IMemory<T> GetMemoryChunk() => _memory;

        public uint GetRegister() => _register;

        public void SetRegister(uint value) => _register = value;

        public void Increment() => _register++;

        public void Add(uint i) => _register += i;

        public static MemoryUnit<T> operator ++(MemoryUnit<T> memory)
        {
            memory.Increment();
            return memory;
        }

        public static MemoryUnit<T> operator +(MemoryUnit<T> memory, uint i)
        {
            memory.Add(i);
            return memory;
        }

    }
}
