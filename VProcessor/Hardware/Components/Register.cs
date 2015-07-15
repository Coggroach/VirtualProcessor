using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Register
    {
        public UInt32 Value { get; set; }

        public Register()
        {
            this.Value = 0;
        }

        public Register(UInt32 b)
        {
            this.Value = b;
        }

        public Register(Register reg)
        {
            this.Value = reg.Value;
        }

        public Boolean BitMatch(Byte bitPos, Byte matchBit)
        {
            return ((this.Value >> bitPos) & 1) == matchBit;
        }

        public void SetBit(Byte bitPos)
        {
            this.Value |= (Byte) (1 << bitPos);
        }

        public void ClrBit(Byte bitPos)
        {
            this.Value &= (UInt32)~(1 << bitPos);
        }

        public void BitSet(Byte bitPos, Boolean value)
        {
            if(value)
                SetBit(bitPos);
            else ClrBit(bitPos);
        }

        public UInt32 GetBit(Byte bitPos)
        {
            return (UInt32)((this.Value >> bitPos) & 1);
        }

        public void Mask(Byte mask = 0xF)
        {
            this.Value &= mask;
        }        
    }
}
