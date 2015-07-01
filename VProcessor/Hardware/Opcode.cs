using System;
using System.Collections;

namespace VProcessor.Hardware
{
    public class Opcode
    {
        //http://simplemachines.it/doc/arm_inst.pdf
        //General Flags
        public const Int32 LDR = 0;

        //Arithmetic Flags
        public const Int32 INC = 1;
        public const Int32 ADD = 2;
        public const Int32 ADDI = 3;
        public const Int32 ADC = 4;
        public const Int32 SUBD = 5;
        public const Int32 SUB = 6;
        public const Int32 SBC = 7;
        public const Int32 DEC = 8;
        public const Int32 RSB = 9;
        public const Int32 RSC = 0xA;

        //Multiplication Flags
        public const Int32 MUL = 0xB;
        public const Int32 MLA = 0xC;

        //Logical Flags
        public const Int32 AND = 0x10;
        public const Int32 EOR = 0x11;
        public const Int32 ORR = 0x12;
        public const Int32 BIC = 0x13;

        //Data Movement
        public const Int32 MOV = 0x14;
        public const Int32 MNV = 0x15;

        //Shifting Flags
        public const Int32 ROL = 0x18;
        public const Int32 ROR = 0x19;
        public const Int32 LSL = 0x1A;
        public const Int32 LSR = 0x1B;

        //Comparison Flags
        public const Int32 TST = 0x1C;
        public const Int32 TEQ = 0x1D;
        public const Int32 CMN = 0x1E;
        public const Int32 CMP = 0x1F;
        
        //Branch Flags
        public const Int32 B   = 0x21;
        public const Int32 BEQ = 0x22;
        public const Int32 BNE = 0x23;
        public const Int32 BCS = 0x24;
        public const Int32 BCC = 0x25;
        public const Int32 BNS = 0x26;
        public const Int32 BNC = 0x27;
        public const Int32 BVS = 0x28;
        public const Int32 BVC = 0x29;
        public const Int32 BHI = 0x2A;
        public const Int32 BLS = 0x2B;
        public const Int32 BGE = 0x2C;
        public const Int32 BLT = 0x2D;
        public const Int32 BGT = 0x2E;
        public const Int32 BLE = 0x2F;

        private static readonly Hashtable CodeTable;

        public static Int32 GetCode(String code)
        {
            return (Int32) CodeTable[code];
        }

        public static Hashtable GetCodeTable()
        {
            return CodeTable;
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
