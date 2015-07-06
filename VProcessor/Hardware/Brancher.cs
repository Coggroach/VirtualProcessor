using System;

namespace VProcessor.Hardware
{
    public class Brancher
    {
        //Branch Flags
        public const Int32 B   = 0x1;
        public const Int32 BEQ = 0x2;
        public const Int32 BNE = 0x3;
        public const Int32 BCS = 0x4;
        public const Int32 BCC = 0x5;
        public const Int32 BNS = 0x6;
        public const Int32 BNC = 0x7;
        public const Int32 BVS = 0x8;
        public const Int32 BVC = 0x9;
        public const Int32 BHI = 0xA;
        public const Int32 BLS = 0xB;
        public const Int32 BGE = 0xC;
        public const Int32 BLT = 0xD;
        public const Int32 BGT = 0xE;
        public const Int32 BLE = 0xF;

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
                case B:
                    return true;
                case BEQ:
                    return this.BitMatch(2, 1);
                case BNE:
                    return this.BitMatch(2, 0);
                case BGT:
                    return this.BitMatch(2, 0) && (this.BitMatch(3, 1) ^ this.BitMatch(0, 0) );
                case BLT:
                    return this.BitMatch(3, 1) ^ this.BitMatch(0, 1);
                case BGE:
                    return this.BitMatch(3, 1) ^ this.BitMatch(0, 0);
                case BLE:
                    return this.BitMatch(2, 1) || (this.BitMatch(3, 1) ^ this.BitMatch(0, 1) );
                case BVS:
                    return this.BitMatch(0, 1);
                case BVC:
                    return this.BitMatch(0, 0);
                case BCS:
                    return this.BitMatch(1, 1);
                case BCC:
                    return this.BitMatch(1, 0);
                case BNS:
                    return this.BitMatch(3, 1);
                case BNC:
                    return this.BitMatch(3, 0);
                case BHI:
                    return this.BitMatch(1, 1) && this.BitMatch(2, 0);
                case BLS:
                    return this.BitMatch(1, 0) || this.BitMatch(2, 1);
            }
            return false;
        }
    }
}
