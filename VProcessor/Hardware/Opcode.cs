using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Opcode
    {
        public const Int32 LDR = 0;
        public const Int32 INC = 1;
        public const Int32 ADD = 2;
        public const Int32 ADDI = 3;
        public const Int32 SUBD = 4;
        public const Int32 SUB = 5;
        public const Int32 DEC = 6;

        public const Int32 LSL = 10;
        public const Int32 LSR = 11;

        private static Hashtable codeTable;

        public static Int32 GetCode(String code)
        {
            return (Int32) codeTable[code];
        }

        static Opcode()
        {
            codeTable.Add("ADD", ADD);
            codeTable.Add("LDR", LDR);
            codeTable.Add("INC", INC);
            codeTable.Add("ADDI", ADDI);
            codeTable.Add("SUBD", SUBD);
            codeTable.Add("SUB", SUB);
            codeTable.Add("DEC", DEC);
            codeTable.Add("LSL", LSL);
            codeTable.Add("LSR", LSR);
        }
    }
}
