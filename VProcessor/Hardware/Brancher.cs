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
            }
            return false;
        }
        
        private Boolean Parse(String s)
        {
            var neg = 1;
            if(s.Contains("!"))
                neg = 0;
            var codes = new[] {"N", "Z", "C", "V"};
            var i = codes.Where(code => if(s.Contains(code)) { break; } i++);
            
            return BitMatch(i, neg);
        }
    }
}
