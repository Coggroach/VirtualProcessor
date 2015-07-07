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
        public const Int32 LDRC = 0xD;

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
        public const Int32 B_BASE = B;

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

        public static Int32 GetCodeAddress(String code)
        {
            return (Int32) ((Hashtable)CodeTable[code])["Address"];
        }

        public static Hashtable GetCodeTable()
        {
            return CodeTable;
        }

        public static void Add(String name, Int32 value, Int32 address, Byte typeAndConstants)
        {
            CodeTable.Add(name, new Hashtable()
            {
                {"Address", address},
                {"Value", value},
                {"Type", typeAndConstants}
            });
        }

        static Opcode()
        {
            CodeTable = new Hashtable();
                        
            Add("LDR", LDR, 1, 0x26);
            Add("LDRR", LDR, 4, 0x26);
            Add("LDRC", LDRC, 5, 0x26);
            Add("ADD", ADD, 6, 0x15);
            Add("INC", INC, 8, 0x24);
            Add("ADDI", ADDI, 9, 0x15);            
            Add("MOV", MOV, 11, 0x25);
            Add("MNV", MNV, 13, 0x25);
            Add("CMP", CMP, 15, 0x25);
            Add("CMN", CMN, 17, 0x25);
            Add("TST", TST, 19, 0x25);
            Add("TEQ", TEQ, 21, 0x25);

            Add("AND", AND, 23, 0x15);
            Add("EOR", EOR, 25, 0x15);
            Add("ORR", ORR, 27, 0x15);
            Add("BIC", BIC, 29, 0x15);

            Add("LSL", LSL, 31, 0x15);
            Add("LSR", LSR, 33, 0x15);
            Add("ROL", ROL, 35, 0x15);
            Add("ROR", ROR, 37, 0x15);

            Add("B", B, 39, 0x32);
            Add("BEQ", BEQ, 40, 0x32);
            Add("BNE", BNE, 41, 0x32);
            Add("BCS", BCS, 42, 0x32);
            Add("BCC", BCC, 43, 0x32);
            Add("BNS", BNS, 44, 0x32);
            Add("BNC", BNC, 45, 0x32);
            Add("BVS", BVS, 46, 0x32);
            Add("BVC", BVC, 47, 0x32);
            Add("BHI", BHI, 48, 0x32);
            Add("BLS", BLS, 49, 0x32);
            Add("BGE", BGE, 50, 0x32);
            Add("BLT", BLT, 51, 0x32);
            Add("BGT", BGT, 52, 0x32);
            Add("BLE", BLE, 53, 0x32);

            Add("MUL", MUL, 54, 0x15);
            Add("MLA", MLA, 56, 0x15);

            Add("ADC", ADC, 58, 0x15);
            Add("SUBD", SUBD, 60, 0x15);
            Add("SUB", SUB, 62, 0x15);
            Add("SBC", SBC, 64, 0x15);
            Add("DEC", DEC, 66, 0x24);
            Add("RSB", RSB, 68, 0x15);
            Add("RSC", RSC, 70, 0x15);
        }
    }
}
