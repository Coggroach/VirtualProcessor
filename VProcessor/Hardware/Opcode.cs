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
        
        public const Int32 B   = 0x10000;
        public const Int32 BEQ = 0x20000;
        public const Int32 BNE = 0x30000;
        public const Int32 BCS = 0x40000;
        public const Int32 BCC = 0x50000;
        public const Int32 BNS = 0x60000;
        public const Int32 BNC = 0x70000;
        public const Int32 BVS = 0x80000;
        public const Int32 BVC = 0x90000;
        public const Int32 BHI = 0xA0000;
        public const Int32 BLS = 0xB0000;
        public const Int32 BGE = 0xC0000;
        public const Int32 BLT = 0xD0000;
        public const Int32 BGT = 0xE0000;
        public const Int32 BLE = 0xF0000;

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
            codeTable.Add("B", B);
            codeTable.Add("BEQ", BEQ);
            codeTable.Add("BNE", BNE);
            codeTable.Add("BCS", BCS);
            codeTable.Add("BCC", BCC);
            codeTable.Add("BNS", BNS);
            codeTable.Add("BNC", BNC);
            codeTable.Add("BVS", BVS);
            codeTable.Add("BVC", BVC);
            codeTable.Add("BHI", BHI);
            codeTable.Add("BLS", BLS);
            codeTable.Add("BGE", BGE);
            codeTable.Add("BLT", BLT);
            codeTable.Add("BGT", BGT);
            codeTable.Add("BLE", BLE);
        }
    }
}
