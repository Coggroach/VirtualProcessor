using System;

namespace VProcessor.Hardware
{
    public class Brancher
    {
        private Register Nzcv;      
        public Brancher()
        {
            this.Nzcv = new Register();
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
            //https://www.cs.tcd.ie/John.Waldron/3d1/branches.pdf
            switch(code)
            {
                case Opcode.B:
                    return true;
                case Opcode.BEQ:
                    return Nzcv.BitMatch(2, 1);
                case Opcode.BNE:
                    return Nzcv.BitMatch(2, 0);
                case Opcode.BGT:
                    return Nzcv.BitMatch(2, 0) && (Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 0) );
                case Opcode.BLT:
                    return Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 1);
                case Opcode.BGE:
                    return Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 0);
                case Opcode.BLE:
                    return Nzcv.BitMatch(2, 1) || (Nzcv.BitMatch(3, 1) ^ Nzcv.BitMatch(0, 1) );
                case Opcode.BVS:
                    return Nzcv.BitMatch(0, 1);
                case Opcode.BVC:
                    return Nzcv.BitMatch(0, 0);
                case Opcode.BCS:
                    return Nzcv.BitMatch(1, 1);
                case Opcode.BCC:
                    return Nzcv.BitMatch(1, 0);
                case Opcode.BNS:
                    return Nzcv.BitMatch(3, 1);
                case Opcode.BNC:
                    return Nzcv.BitMatch(3, 0);
                case Opcode.BHI:
                    return Nzcv.BitMatch(1, 1) && Nzcv.BitMatch(2, 0);
                case Opcode.BLS:
                    return Nzcv.BitMatch(1, 0) || Nzcv.BitMatch(2, 1);
            }
            return false;
        }
    }
}
