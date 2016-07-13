namespace VProcessor.Hardware.Interfacing
{
    public interface IDatapath
    {
        void Reset();
        void SetRegister(byte register, uint value);        
        uint[] GetRegisters();
        uint GetRegister(byte register);
    }
}
