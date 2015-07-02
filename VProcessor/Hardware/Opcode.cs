using System;
using System.Collections;
using System.Collections.Generic;

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
        public const Int32 EOR = 0x15;
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

        public static Hashtable GetCode(String code)
        {
            return (Hashtable) CodeTable[code];
        }

        public static Int32 GetCodeIndexer(String code)
        {
            return (Int32) ((Hashtable) CodeTable[code])["Value"];
        }

        public static Byte GetCodeType(String code)
        {
            return (Byte) ((Hashtable) CodeTable[code])["Type"];
        }

        public static Hashtable GetCodeTable()
        {
            return CodeTable;
        }

        public static void Add(String name, Int32 value, Byte typeAndConstants)
        {
            CodeTable.Add(name, new Hashtable()
            {
                {"Value", value},
                {"Type", typeAndConstants}
            });
        }

        static Opcode()
        {
            CodeTable = new Hashtable();
            //Name CarIndex 4:4 -> Type:C_F_R_CR_FR_CFR
            //1_2_4_5_6_7
            Add("ADD", ADD, 0x15);
            Add("LDR", LDR, 0x26);
            Add("INC", INC, 0x24);
            Add("ADDI", ADDI, 0x15);
            Add("ADC", ADC, 0x15);
            Add("SUBD", SUBD, 0x15);
            Add("SUB", SUB, 0x15);
            Add("SBC", SBC, 0x15);
            Add("DEC", DEC, 0x24);
            Add("B", B, 0x32);
            Add("BEQ", BEQ, 0x32);
            Add("BNE", BNE, 0x32);
            Add("BCS", BCS, 0x32);
            Add("BCC", BCC, 0x32);
            Add("BNS", BNS, 0x32);
            Add("BNC", BNC, 0x32);
            Add("BVS", BVS, 0x32);
            Add("BVC", BVC, 0x32);
            Add("BHI", BHI, 0x32);
            Add("BLS", BLS, 0x32);
            Add("BGE", BGE, 0x32);
            Add("BLT", BLT, 0x32);
            Add("BGT", BGT, 0x32);
            Add("BLE", BLE, 0x32);
            Add("LSL", LSL, 0x15);
            Add("LSR", LSR, 0x15);
            Add("ROL", ROL, 0x15);
            Add("ROR", ROR, 0x15);
            Add("TST", TST, 0x25);
            Add("TEQ", TEQ, 0x25);
            Add("CMN", CMN, 0x25);
            Add("CMP", CMP, 0x25);
            Add("RSB", RSB, 0x15);
            Add("RSC", RSC, 0x15);
            Add("MUL", MUL, 0x15);
            Add("MLA", MLA, 0x15);
            Add("AND", AND, 0x15);
            Add("EOR", EOR, 0x15);
            Add("ORR", ORR, 0x15);
            Add("BIC", BIC, 0x15);
            Add("MOV", MOV, 0x25);
            Add("MNV", MNV, 0x25);
        }
    }
}
