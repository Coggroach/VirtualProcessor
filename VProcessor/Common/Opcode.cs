using System;
using System.Collections;
using System.Collections.Generic;
using VProcessor.Tools;

namespace VProcessor.Common
{
    public enum Opcode : int
    {
        //http://simplemachines.it/doc/arm_inst.pdf
        NUL = 0,

        //Data Movement Flags
        LDR = 0,
        LDRC = 0xD,        
        STR = 0xE,

        //Arithmetic Flags
        INC = 1,
        ADD = 2,
        ADDI = 3,
        ADC = 4,
        SUBD = 5,
        SUB = 6,
        SBC = 7,
        DEC = 8,
        RSB = 9,
        RSC = 0xA,

        //Multiplication Flags
        MUL = 0xB,
        MLA = 0xC,

        //Logical Flags
        AND = 0x10,
        EOR = 0x15,
        ORR = 0x12,
        BIC = 0x13,

        //Data Movement
        MOV = 0x14,
        MNV = 0x15,

        //Shifting Flags
        ROL = 0x18,
        ROR = 0x19,
        LSL = 0x1A,
        LSR = 0x1B,

        //Comparison Flags
        TST = 0x1C,
        TEQ = 0x1D,
        CMN = 0x1E,
        CMP = 0x1F,

        LDPC = 0x80,
        STPC = 0x40,

        MOD = 0x100,
        EOI = 0x2000,
        IRQ = 0
    }

    public enum BranchCode : int
    {
        //Branch Flags
        B = 0x0,
        BEQ = 0x1,
        BNE = 0x2,
        BCS = 0x3,
        BCC = 0x4,
        BNS = 0x5,
        BNC = 0x6,
        BVS = 0x7,
        BVC = 0x8,
        BHI = 0x9,
        BLS = 0xA,
        BGE = 0xB,
        BLT = 0xC,
        BGT = 0xD,
        BLE = 0xE
    }

    public class OpcodeRegistry
    {
        public static OpcodeRegistry Instance { get; private set; }

        static OpcodeRegistry()
        {
            Instance = new OpcodeRegistry();
        }

        public class OpcodeRegistryElement
        {
            public Int32 Code { get; private set; }
            public Byte Type { get; private set; }
            public Int32 Address { get; private set; }

            public OpcodeRegistryElement(Int32 code, Int32 address, Byte type)
            {
                this.Code = code;
                this.Address = address;
                this.Type = type;
            }
        }

        enum OpcodeTypeFlag : int
        {
            Constant = 1,
            Number = 3,
            Register = 1            
        }

        private readonly Dictionary<String, OpcodeRegistryElement> CodeTable;
        private Int32 CurrentAddress;
        private Int32 LastType;
        private Logger Logger;    
        
        private const Int32 FirstAddress = 1;

        private OpcodeRegistryElement GetCodeTable(String code)
        {
            return CodeTable[code];
        }

        public Boolean IsValidCode(String code)
        {
            return CodeTable.ContainsKey(code);
        }

        public Int32 GetCode(String code)
        {
            return CodeTable[code].Code;
        }

        public Byte GetCodeType(String code)
        {
            return CodeTable[code].Type;
        }

        public Int32 GetCodeAddress(String code)
        {
            return CodeTable[code].Address;
        }

        public Dictionary<String, OpcodeRegistryElement> GetCodeTable()
        {
            return CodeTable;
        }

        public void Add(String name, Opcode value, Byte type)
        {
            this.Add(name, (Int32)value, type);
        }

        public void Add(String name, Opcode value, Int32 address, Byte type)
        {
            this.Add(name, (Int32)value, address, type);
        }

        public void Add(String name, BranchCode value, Byte type)
        {
            this.Add(name, (Int32)value, type);
        }

        public void Add(String name, BranchCode value, Int32 address, Byte type)
        {
            this.Add(name, (Int32)value, address, type);
        }

        public void Add(String name, Int32 value, Int32 address, Byte type)
        {
            CurrentAddress += address;
            CodeTable.Add(name, new OpcodeRegistryElement(value, CurrentAddress, type));
            LastType = type & 7;
            Logger.Log(name + ":" + CurrentAddress + " : " + GetNextAddressSize());
        }

        public void Add(String name, Int32 value, Byte type)
        {
            Add(name, value, GetNextAddressSize(), type);
        }

        private Int32 GetNextAddressSize()
        {
            switch(LastType)
            {                
                case 1:                    
                    return (Int32)OpcodeTypeFlag.Constant;
                case 2:
                    return (Int32)OpcodeTypeFlag.Number;
                case 3:
                    return (Int32)OpcodeTypeFlag.Number + (Int32)OpcodeTypeFlag.Constant;
                case 4:
                    return (Int32)OpcodeTypeFlag.Register;
                case 5:
                    return (Int32)OpcodeTypeFlag.Register + (Int32)OpcodeTypeFlag.Constant;
                case 6:
                    return (Int32)OpcodeTypeFlag.Register + (Int32)OpcodeTypeFlag.Number;
                case 7:
                    return (Int32)OpcodeTypeFlag.Register + (Int32)OpcodeTypeFlag.Number + (Int32)OpcodeTypeFlag.Constant;
                default:
                    return 0;
            }
        }

        public OpcodeRegistry()
        {
            CodeTable = new Dictionary<String, OpcodeRegistryElement>();
            Logger = new Logger("LoggerControl.txt", false);            
            CurrentAddress = FirstAddress;
            LastType = 0;
            //Type 1: ADD rx, ry, C
            //Type 2: LDR rx, C
            //Type 3: BEQ C
            //Type 4: LDPC
            //Type 5: STR rx, [ry, C]
            //Type 7: INC rx
            //Type 8: Compound Command.
            
            //001 - 1 : Constant   - 1
            //010 - 2 : Number   - 3
            //011 - 3 : FK  - 4
            //100 - 4 : Register   - 1
            //101 - 5 : RK  - 2
            //110 - 6 : RF  - 4
            //111 - 7 : RFK - 5    
                        
            Add("LDR", Opcode.LDR, 0x27);
            Add("ADD", Opcode.ADD, 0x15);
            Add("INC", Opcode.INC, 0x24);
            Add("ADDI", Opcode.ADDI, 0x15);            
            Add("MOV", Opcode.MOV, 0x25);
            Add("MNV", Opcode.MNV, 0x25);
            Add("CMP", Opcode.CMP, 0x25);
            Add("CMN", Opcode.CMN, 0x25);
            Add("TST", Opcode.TST, 0x25);
            Add("TEQ", Opcode.TEQ, 0x25);

            Add("AND", Opcode.AND, 0x15);
            Add("EOR", Opcode.EOR, 0x15);
            Add("ORR", Opcode.ORR, 0x15);
            Add("BIC", Opcode.BIC, 0x15);

            Add("ROL", Opcode.ROL, 0x15);
            Add("ROR", Opcode.ROR, 0x15);
            Add("LSL", Opcode.LSL, 0x15);
            Add("LSR", Opcode.LSR, 0x15); 

            Add("B", BranchCode.B, 0x31);
            Add("BEQ", BranchCode.BEQ, 0x31);
            Add("BNE", BranchCode.BNE, 0x31);
            Add("BCS", BranchCode.BCS, 0x31);
            Add("BCC", BranchCode.BCC, 0x31);
            Add("BNS", BranchCode.BNS, 0x31);
            Add("BNC", BranchCode.BNC, 0x31);
            Add("BVS", BranchCode.BVS, 0x31);
            Add("BVC", BranchCode.BVC, 0x31);
            Add("BHI", BranchCode.BHI, 0x31);
            Add("BLS", BranchCode.BLS, 0x31);
            Add("BGE", BranchCode.BGE, 0x31);
            Add("BLT", BranchCode.BLT, 0x31);
            Add("BGT", BranchCode.BGT, 0x31);
            Add("BLE", BranchCode.BLE, 0x31);

            Add("MUL", Opcode.MUL, 0x15);
            Add("MLA", Opcode.MLA, 0x15);

            Add("ADC", Opcode.ADC, 0x15);
            Add("SUBD", Opcode.SUBD, 0x15);
            Add("SUB", Opcode.SUB, 0x15);
            Add("SBC", Opcode.SBC, 0x15);
            Add("DEC", Opcode.DEC, 0x24);
            Add("RSB", Opcode.RSB, 0x15);          
            Add("RSC", Opcode.RSC, 0x15);

            Add("STR", Opcode.STR, 0x54);
            Add("LDRST", Opcode.LDR, 0x54); //3 Lines

            Add("LDM", Opcode.NUL, 3, 0x80);
            Add("STM", Opcode.NUL, 0x80);

            Add("LDPC", Opcode.LDPC, 0x84);
            Add("STPC", Opcode.STPC, 0x84);

            Add("BX", Opcode.NUL, 0x80);
            Add("^", Opcode.NUL, 0x80);

            Add("MOD", Opcode.MOD, 0x86);
            Add("EOI", Opcode.EOI, 0x81);
            Add("IRQ", Opcode.IRQ, 0x82);            
        }
    }
}
