using System;

namespace VProcessor.Hardware
{
    public class Brancher
    {
        public Byte Nzcv { get; set; }       
        public Brancher()
        {
            this.Nzcv = 0;
        }
        
        private Boolean BitMatch(Byte bitPos, Byte matchBit)
        {
            return ((this.Nzcv >> bitPos) & 1) == matchBit;
        }

        public Boolean Branch(UInt32 code)
        {
            //https://www.cs.tcd.ie/John.Waldron/3d1/branches.pdf
            switch(code)
            {
                case Opcode.B:
                    return true;
                case Opcode.BEQ:
                    return this.BitMatch(2, 1);
                case Opcode.BNE:
                    return this.BitMatch(2, 0);
                case Opcode.BGT:
                    return this.BitMatch(2, 0) && (this.BitMatch(3, 1) ^ this.BitMatch(0, 0) );
                case Opcode.BLT:
                    return this.BitMatch(3, 1) ^ this.BitMatch(0, 1);
                case Opcode.BGE:
                    return this.BitMatch(3, 1) ^ this.BitMatch(0, 0);
                case Opcode.BLE:
                    return this.BitMatch(2, 1) || (this.BitMatch(3, 1) ^ this.BitMatch(0, 1) );
                case Opcode.BVS:
                    return this.BitMatch(0, 1);
                case Opcode.BVC:
                    return this.BitMatch(0, 0);
                case Opcode.BCS:
                    return this.BitMatch(1, 1);
                case Opcode.BCC:
                    return this.BitMatch(1, 0);
                case Opcode.BNS:
                    return this.BitMatch(3, 1);
                case Opcode.BNC:
                    return this.BitMatch(3, 0);
                case Opcode.BHI:
                    return this.BitMatch(1, 1) && this.BitMatch(2, 0);
                case Opcode.BLS:
                    return this.BitMatch(1, 0) || this.BitMatch(2, 1);
            }
            return false;
        }
    }
}
