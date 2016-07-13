namespace VProcessor.Hardware.Components
{
    public class Register
    {
        public uint Value { get; set; }

        public Register()
        {
            Value = 0;
        }

        public Register(uint b)
        {
            Value = b;
        }

        public Register(Register reg)
        {
            Value = reg.Value;
        }

        public Register(Register reg, uint mask)
        {
            Value &= ~mask;
            var value = reg.Value & mask;
            Value |= value;
        }

        public bool BitMatch(byte bitPos, byte matchBit)
        {
            return ((Value >> bitPos) & 1) == matchBit;
        }

        public void SetBit(byte bitPos)
        {
            Value |= (byte) (1 << bitPos);
        }

        public void ClrBit(byte bitPos)
        {
            Value &= (uint)~(1 << bitPos);
        }

        public void BitSet(byte bitPos, bool value)
        {
            if(value)
                SetBit(bitPos);
            else ClrBit(bitPos);
        }

        public uint GetBit(byte bitPos)
        {
            return (Value >> bitPos) & 1;
        }

        public void Mask(byte mask = 0xF)
        {
            Value &= mask;
        }        
    }
}
