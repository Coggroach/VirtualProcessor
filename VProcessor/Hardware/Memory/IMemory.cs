namespace VProcessor.Hardware.Memory
{
    public interface IMemory<T>
    {
        void Reset();
        T GetMemory(int index);
        T GetMemory(uint index);
        void SetMemory(int index, T value);
        int Length { get; }
        bool HasMemory { get; }
    }
}
