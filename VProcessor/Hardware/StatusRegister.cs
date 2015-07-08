using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class StatusRegister
    {
        public Byte Nzcv { get; set; }

        public StatusRegister()
        {
            this.Nzcv = 0;
        }

        public Boolean BitMatch(Byte bitPos, Byte matchBit)
        {
            return ((this.Nzcv >> bitPos) & 1) == matchBit;
        }

        public void SetBit(Byte bitPos)
        {
            this.Nzcv |= (Byte) (1 << bitPos);
        }

        public void ClrBit(Byte bitPos)
        {
            this.Nzcv &= (Byte)~(1 << bitPos);
        }

        public void BitSet(Byte bitPos, Boolean value)
        {
            if(value)
                SetBit(bitPos);
            else ClrBit(bitPos);
        }

        public Byte GetBit(Byte bitPos)
        {
            return (Byte) ((this.Nzcv >> bitPos) & 1);
        }

        public void Mask(Byte mask = 0xF)
        {
            this.Nzcv &= mask;
        }        
    }
}
