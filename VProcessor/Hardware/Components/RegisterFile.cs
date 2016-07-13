using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Components
{
    class RegisterFile : IDatapath
    {
        private readonly uint[] _registers;

        public RegisterFile()
        {
            _registers = new uint[Datapath.RegisterFileSize];
        }

        public void Reset()
        {
            for (var i = 0; i < _registers.Length; i++)
                _registers[i] = 0;
        }

        public void SetRegister(byte register, uint value)
        {
            _registers[register] = value;
        }

        public uint[] GetRegisters()
        {
            return _registers;
        }

        public uint GetRegister(byte register)
        {
            return _registers[register];
        }
    }
}
