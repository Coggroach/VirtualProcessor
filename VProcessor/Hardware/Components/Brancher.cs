using System;
using VProcessor.Common;

namespace VProcessor.Hardware.Components
{
    public class Brancher
    {
        private Register _nzcv;      

        public Brancher()
            : this(new Register()) {}

        public Brancher(Register status)
        {
            _nzcv = status;
        }

        public Register GetNzcv()
        {
            return _nzcv;
        }

        public void SetNzcv(Register reg)
        {
            _nzcv = new Register(reg);
        }

        public bool Branch(uint code)
        {
            return Branch((BranchCode)code);
        }

        public bool Branch(BranchCode code)
        {
            //https://www.cs.tcd.ie/John.Waldron/3d1/branches.pdf
            switch(code)
            {
                case BranchCode.B:
                    return true;
                case BranchCode.Beq:
                    return _nzcv.BitMatch(2, 1);
                case BranchCode.Bne:
                    return _nzcv.BitMatch(2, 0);
                case BranchCode.Bgt:
                    return _nzcv.BitMatch(2, 0) && (_nzcv.BitMatch(3, 1) ^ _nzcv.BitMatch(0, 0) );
                case BranchCode.Blt:
                    return _nzcv.BitMatch(3, 1) ^ _nzcv.BitMatch(0, 1);
                case BranchCode.Bge:
                    return _nzcv.BitMatch(3, 1) ^ _nzcv.BitMatch(0, 0);
                case BranchCode.Ble:
                    return _nzcv.BitMatch(2, 1) || (_nzcv.BitMatch(3, 1) ^ _nzcv.BitMatch(0, 1) );
                case BranchCode.Bvs:
                    return _nzcv.BitMatch(0, 1);
                case BranchCode.Bvc:
                    return _nzcv.BitMatch(0, 0);
                case BranchCode.Bcs:
                    return _nzcv.BitMatch(1, 1);
                case BranchCode.Bcc:
                    return _nzcv.BitMatch(1, 0);
                case BranchCode.Bns:
                    return _nzcv.BitMatch(3, 1);
                case BranchCode.Bnc:
                    return _nzcv.BitMatch(3, 0);
                case BranchCode.Bhi:
                    return _nzcv.BitMatch(1, 1) && _nzcv.BitMatch(2, 0);
                case BranchCode.Bls:
                    return _nzcv.BitMatch(1, 0) || _nzcv.BitMatch(2, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}
