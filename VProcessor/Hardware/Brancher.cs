using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return (Nzcv >> bitPos) & 1 == matchBit;
        }

        public Boolean Branch(Int32 code)
        {
            //https://www.cs.tcd.ie/John.Waldron/3d1/branches.pdf
            switch(code)
            {
                case Opcode.B:
                    return true;
                case Opcode.BEQ:
                    return BitMatch(2, 1);
                case Opcode.BNE:
                    return BitMatch(2, 0);
                case Opcode.BGT:
                    return BitMatch(2, 1) && ( BitMatch(3, 1) ^ BitMatch(0, 0) );
                case Opcode.BLT:
                    return BitMatch(3, 1) ^ BitMatch(0, 1);
                case Opcode.BGE:
                    return BitMatch(3, 1) ^ BitMatch(0, 0);
                case Opcode.BLE:
                    return BitMatch(2, 1) || ( BitMatch(3, 1) ^ BitMatch(0, 1) );
                case Opcode.BVS:
                    return BitMatch(0, 1);
                case Opcode.BVC:
                    return BitMatch(0, 0);
                case Opcode.BCS:
                    return BitMatch(1, 1);
                case Opcode.BCC:
                    return BitMatch(1, 0);
                case Opcode.BNS:
                    return BitMatch(3, 1);
                case Opcode.BNC:
                    return BitMatch(3, 0);
                case Opcode.BHI:
                    return BitMatch(1, 1) && BitMatch(2, 0);
                case Opcode.BLS:
                    return BitMatch(1, 0) || BitMatch(2, 1);
            }
            return false;
        }
    }
}
