using System;
using VProcessor.Common;

namespace VProcessor.Hardware.Components
{
    public class Brancher
    {
        private Register Nzcv;      

        public Brancher()
            : this(new Register()) {}

        public Brancher(Register status)
        {
            this.Nzcv = status;
        }

        public Register GetNzcv()
        {
            return this.Nzcv;
        }

        public void SetNzcv(Register reg)
        {
            this.Nzcv = new Register(reg);
        }

        public Boolean Branch(UInt32 code)
        {
            return this.Branch((BranchCode)code);
        }

        public Boolean Branch(BranchCode code)
        {
            //https://www.cs.tcd.ie/John.Waldron/3d1/branches.pdf
            switch(code)
            {
                case BranchCode.B:
                    return true;
                case BranchCode.BEQ:
                    return Nzcv.BitMatch(2, 1);
                case BranchCode.BNE:
                    return Nzcv.BitMatch(2, 0);
                case BranchCode.BGT:
                    return Nzcv.BitMatch(2, 0) && (Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 0) );
                case BranchCode.BLT:
                    return Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 1);
                case BranchCode.BGE:
                    return Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 0);
                case BranchCode.BLE:
                    return Nzcv.BitMatch(2, 1) || (Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 1) );
                case BranchCode.BVS:
                    return Nzcv.BitMatch(0, 1);
                case BranchCode.BVC:
                    return Nzcv.BitMatch(0, 0);
                case BranchCode.BCS:
                    return Nzcv.BitMatch(1, 1);
                case BranchCode.BCC:
                    return Nzcv.BitMatch(1, 0);
                case BranchCode.BNS:
                    return Nzcv.BitMatch(3, 1);
                case BranchCode.BNC:
                    return Nzcv.BitMatch(3, 0);
                case BranchCode.BHI:
                    return Nzcv.BitMatch(1, 1) && Nzcv.BitMatch(2, 0);
                case BranchCode.BLS:
                    return Nzcv.BitMatch(1, 0) || Nzcv.BitMatch(2, 1);
            }
            return false;
        }
    }
}
