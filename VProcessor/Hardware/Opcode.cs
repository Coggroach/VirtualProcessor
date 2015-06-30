using System;
using System.Collections;

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

        public const Int32 LSL = 0xA;
        public const Int32 LSR = 0xB;
        public const Int32 CMP = 0xF;
        
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

        private static readonly Hashtable CodeTable;

        public static Int32 GetCode(String code)
        {
            return (Int32) CodeTable[code];
        }

        static Opcode()
        {
            CodeTable = new Hashtable
            {
                {"ADD", ADD},
                {"LDR", LDR},
                {"INC", INC},
                {"ADDI", ADDI},
                {"SUBD", SUBD},
                {"SUB", SUB},
                {"DEC", DEC},
                {"LSL", LSL},
                {"LSR", LSR},
                {"B", B},
                {"BEQ", BEQ},
                {"BNE", BNE},
                {"BCS", BCS},
                {"BCC", BCC},
                {"BNS", BNS},
                {"BNC", BNC},
                {"BVS", BVS},
                {"BVC", BVC},
                {"BHI", BHI},
                {"BLS", BLS},
                {"BGE", BGE},
                {"BLT", BLT},
                {"BGT", BGT},
                {"BLE", BLE}
            };
        }
    }
}
