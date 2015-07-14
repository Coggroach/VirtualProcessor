using System;
using System.Collections;
using System.Collections.Generic;

namespace VProcessor.Hardware
{
    public class Opcode
    {
        //http://simplemachines.it/doc/arm_inst.pdf
        //Data Movement Flags
        public const Int32 LDR = 0;
        public const Int32 LDRC = 0xD;        
        public const Int32 STR = 0xE;

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
        public const Int32 B   = 0x0;
        public const Int32 BEQ = 0x1;
        public const Int32 BNE = 0x2;
        public const Int32 BCS = 0x3;
        public const Int32 BCC = 0x4;
        public const Int32 BNS = 0x5;
        public const Int32 BNC = 0x6;
        public const Int32 BVS = 0x7;
        public const Int32 BVC = 0x8;
        public const Int32 BHI = 0x9;
        public const Int32 BLS = 0xA;
        public const Int32 BGE = 0xB;
        public const Int32 BLT = 0xC;
        public const Int32 BGT = 0xD;
        public const Int32 BLE = 0xE;

        private static readonly Hashtable CodeTable;
        private static Int32 CurrentAddress;
        private static Int32 LastType;

        private const Int32 K = 1;
        private const Int32 F = 3;
        private const Int32 R = 1;
        private const Int32 A = 3;
        
        private const Int32 FirstAddress = 1;

        public static Hashtable GetCode(String code)
        {
            return (Hashtable) CodeTable[code];
        }

        public static Boolean IsValidCode(String code)
        {
            return CodeTable.ContainsKey(code);
        }

        public static Int32 GetCodeIndexer(String code)
        {
            return (Int32)GetCode(code)["Value"];
        }

        public static Byte GetCodeType(String code)
        {
            return (Byte)GetCode(code)["Type"];
        }

        public static Int32 GetCodeAddress(String code)
        {
            return (Int32)GetCode(code)["Address"];
        }

        public static Hashtable GetCodeTable()
        {
            return CodeTable;
        }

        public static void Add(String name, Int32 value, Byte typeAndConstants)
        {            
            CurrentAddress += GetNextAddress();
            CodeTable.Add(name, new Hashtable()
            {
                {"Address", CurrentAddress},
                {"Value", value},
                {"Type", typeAndConstants}
            });
            LastType = typeAndConstants & 7;            
        }

        public static void Add(String name, Int32 value, Int32 address, Byte typeAndConstants)
        {
            CurrentAddress = address;
            CodeTable.Add(name, new Hashtable()
            {
                {"Address", CurrentAddress},
                {"Value", value},
                {"Type", typeAndConstants}
            });
            LastType = typeAndConstants & 7;
        }

        private static Int32 GetNextAddress()
        {
            switch(LastType)
            {                
                case 1:                    
                    return K;
                case 2:
                    return F;
                case 3:
                    return F + K;
                case 4:
                    return R;
                case 5:
                    return R + K;
                case 6:
                    return R + F;
                case 7:
                    return R + F + K;
                default:
                    return 0;
            }
        }

        static Opcode()
        {
            CodeTable = new Hashtable();
            CurrentAddress = FirstAddress;
            LastType = 0;
            //Type 1: ADD rx, ry, C
            //Type 2: LDR rx, C
            //Type 3: BEQ C
            
            //001 - 1 : K   - 1
            //010 - 2 : F   - 3
            //011 - 3 : FK  - 4
            //100 - 4 : R   - 1
            //101 - 5 : RK  - 2
            //110 - 6 : RF  - 4
            //111 - 7 : RFK - 5    
                        
            Add("LDR", LDR, 0x27);
            Add("ADD", ADD, 0x15);
            Add("INC", INC, 0x24);
            Add("ADDI", ADDI, 0x15);            
            Add("MOV", MOV, 0x25);
            Add("MNV", MNV, 0x25);
            Add("CMP", CMP, 0x25);
            Add("CMN", CMN, 0x25);
            Add("TST", TST, 0x25);
            Add("TEQ", TEQ, 0x25);

            Add("AND", AND, 0x15);
            Add("EOR", EOR, 0x15);
            Add("ORR", ORR, 0x15);
            Add("BIC", BIC, 0x15);

            Add("LSL", LSL, 0x15);
            Add("LSR", LSR, 0x15);
            Add("ROL", ROL, 0x15);
            Add("ROR", ROR, 0x15);

            Add("B", B, 0x31);
            Add("BEQ", BEQ, 0x31);
            Add("BNE", BNE, 0x31);
            Add("BCS", BCS, 0x31);
            Add("BCC", BCC, 0x31);
            Add("BNS", BNS, 0x31);
            Add("BNC", BNC, 0x31);
            Add("BVS", BVS, 0x31);
            Add("BVC", BVC, 0x31);
            Add("BHI", BHI, 0x31);
            Add("BLS", BLS, 0x31);
            Add("BGE", BGE, 0x31);
            Add("BLT", BLT, 0x31);
            Add("BGT", BGT, 0x31);
            Add("BLE", BLE, 0x31);

            Add("MUL", MUL, 0x15);
            Add("MLA", MLA, 0x15);

            Add("ADC", ADC, 0x15);
            Add("SUBD", SUBD, 0x15);
            Add("SUB", SUB, 0x15);
            Add("SBC", SBC, 0x15);
            Add("DEC", DEC, 0x24);
            Add("RSB", RSB, 0x15);
            Add("RSC", RSC, 0x15);

            Add("STR", STR, 0x15);
            Add("LDRST", LDR, CurrentAddress + A, 0x15);
        }
    }
}
