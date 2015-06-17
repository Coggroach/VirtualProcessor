using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class AssemblyTable
    {
        public const Int32 ADD = 2;
        public const Int32 LDR = 0;
        public const Int32 INC = 1;
        public const Int32 ADDI = 3;
        public const Int32 SUBD = 4;
        public const Int32 SUB = 5;

        private static Hashtable codeTable;

        public static Int32 GetCode(String code)
        {
            return (Int32) codeTable[code];
        }

        static AssemblyTable()
        {
            codeTable.Add("ADD", ADD);
            codeTable.Add("LDR", LDR);
            codeTable.Add("INC", INC);
            codeTable.Add("ADDI", ADDI);
            codeTable.Add("SUBD", SUBD);
            codeTable.Add("SUB", SUB);
        }
    }
}
