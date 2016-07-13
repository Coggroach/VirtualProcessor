using System.Collections.Generic;
using VProcessor.Tools;

namespace VProcessor.Common
{
    public enum Opcode
    {
        //http://simplemachines.it/doc/arm_inst.pdf
        Nul = 0,

        //Data Movement Flags
        Ldr = 0,
        Ldrc = 0xD,        
        Str = 0xE,

        //Arithmetic Flags
        Inc = 1,
        Add = 2,
        Addi = 3,
        Adc = 4,
        Subd = 5,
        Sub = 6,
        Sbc = 7,
        Dec = 8,
        Rsb = 9,
        Rsc = 0xA,

        //Multiplication Flags
        Mul = 0xB,
        Mla = 0xC,

        //Logical Flags
        And = 0x10,
        Eor = 0x15,
        Orr = 0x12,
        Bic = 0x13,

        //Data Movement
        Mov = 0x14,
        Mnv = 0x15,

        //Shifting Flags
        Rol = 0x18,
        Ror = 0x19,
        Lsl = 0x1A,
        Lsr = 0x1B,

        //Comparison Flags
        Tst = 0x1C,
        Teq = 0x1D,
        Cmn = 0x1E,
        Cmp = 0x1F,

        Ldpc = 0x80,
        Stpc = 0x40,

        Mod = 0x100,
        Eoi = 0x2000,
        Irq = 0
    }

    public enum BranchCode
    {
        //Branch Flags
        B = 0x0,
        Beq = 0x1,
        Bne = 0x2,
        Bcs = 0x3,
        Bcc = 0x4,
        Bns = 0x5,
        Bnc = 0x6,
        Bvs = 0x7,
        Bvc = 0x8,
        Bhi = 0x9,
        Bls = 0xA,
        Bge = 0xB,
        Blt = 0xC,
        Bgt = 0xD,
        Ble = 0xE
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
            public int Code { get; private set; }
            public byte Type { get; private set; }
            public int Address { get; private set; }

            public OpcodeRegistryElement(int code, int address, byte type)
            {
                Code = code;
                Address = address;
                Type = type;
            }
        }

        enum OpcodeTypeFlag
        {
            Constant = 1,
            Number = 3,
            Register = 1            
        }

        private readonly Dictionary<string, OpcodeRegistryElement> _codeTable;
        private int _currentAddress;
        private int _lastType;
        private readonly Logger _logger;    
        
        private const int FirstAddress = 1;

        private OpcodeRegistryElement GetCodeTable(string code)
        {
            return _codeTable[code];
        }

        public bool IsValidCode(string code)
        {
            return _codeTable.ContainsKey(code);
        }

        public int GetCode(string code)
        {
            return _codeTable[code].Code;
        }

        public byte GetCodeType(string code)
        {
            return _codeTable[code].Type;
        }

        public int GetCodeAddress(string code)
        {
            return _codeTable[code].Address;
        }

        public Dictionary<string, OpcodeRegistryElement> GetCodeTable()
        {
            return _codeTable;
        }

        public void Add(string name, Opcode value, byte type)
        {
            Add(name, (int)value, type);
        }

        public void Add(string name, Opcode value, int address, byte type)
        {
            Add(name, (int)value, address, type);
        }

        public void Add(string name, BranchCode value, byte type)
        {
            Add(name, (int)value, type);
        }

        public void Add(string name, BranchCode value, int address, byte type)
        {
            Add(name, (int)value, address, type);
        }

        public void Add(string name, int value, int address, byte type)
        {
            _currentAddress += address;
            _codeTable.Add(name, new OpcodeRegistryElement(value, _currentAddress, type));
            _lastType = type & 7;
            _logger.Log(name + ":" + _currentAddress + " : " + GetNextAddressSize());
        }

        public void Add(string name, int value, byte type)
        {
            Add(name, value, GetNextAddressSize(), type);
        }

        private int GetNextAddressSize()
        {
            switch(_lastType)
            {                
                case 1:                    
                    return (int)OpcodeTypeFlag.Constant;
                case 2:
                    return (int)OpcodeTypeFlag.Number;
                case 3:
                    return (int)OpcodeTypeFlag.Number + (int)OpcodeTypeFlag.Constant;
                case 4:
                    return (int)OpcodeTypeFlag.Register;
                case 5:
                    return (int)OpcodeTypeFlag.Register + (int)OpcodeTypeFlag.Constant;
                case 6:
                    return (int)OpcodeTypeFlag.Register + (int)OpcodeTypeFlag.Number;
                case 7:
                    return (int)OpcodeTypeFlag.Register + (int)OpcodeTypeFlag.Number + (int)OpcodeTypeFlag.Constant;
                default:
                    return 0;
            }
        }

        public OpcodeRegistry()
        {
            _codeTable = new Dictionary<string, OpcodeRegistryElement>();
            _logger = new Logger("LoggerControl.txt", false);            
            _currentAddress = FirstAddress;
            _lastType = 0;
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
                        
            Add("LDR", Opcode.Ldr, 0x27);
            Add("ADD", Opcode.Add, 0x15);
            Add("INC", Opcode.Inc, 0x24);
            Add("ADDI", Opcode.Addi, 0x15);            
            Add("MOV", Opcode.Mov, 0x25);
            Add("MNV", Opcode.Mnv, 0x25);
            Add("CMP", Opcode.Cmp, 0x25);
            Add("CMN", Opcode.Cmn, 0x25);
            Add("TST", Opcode.Tst, 0x25);
            Add("TEQ", Opcode.Teq, 0x25);

            Add("AND", Opcode.And, 0x15);
            Add("EOR", Opcode.Eor, 0x15);
            Add("ORR", Opcode.Orr, 0x15);
            Add("BIC", Opcode.Bic, 0x15);

            Add("ROL", Opcode.Rol, 0x15);
            Add("ROR", Opcode.Ror, 0x15);
            Add("LSL", Opcode.Lsl, 0x15);
            Add("LSR", Opcode.Lsr, 0x15); 

            Add("B", BranchCode.B, 0x31);
            Add("BEQ", BranchCode.Beq, 0x31);
            Add("BNE", BranchCode.Bne, 0x31);
            Add("BCS", BranchCode.Bcs, 0x31);
            Add("BCC", BranchCode.Bcc, 0x31);
            Add("BNS", BranchCode.Bns, 0x31);
            Add("BNC", BranchCode.Bnc, 0x31);
            Add("BVS", BranchCode.Bvs, 0x31);
            Add("BVC", BranchCode.Bvc, 0x31);
            Add("BHI", BranchCode.Bhi, 0x31);
            Add("BLS", BranchCode.Bls, 0x31);
            Add("BGE", BranchCode.Bge, 0x31);
            Add("BLT", BranchCode.Blt, 0x31);
            Add("BGT", BranchCode.Bgt, 0x31);
            Add("BLE", BranchCode.Ble, 0x31);

            Add("MUL", Opcode.Mul, 0x15);
            Add("MLA", Opcode.Mla, 0x15);

            Add("ADC", Opcode.Adc, 0x15);
            Add("SUBD", Opcode.Subd, 0x15);
            Add("SUB", Opcode.Sub, 0x15);
            Add("SBC", Opcode.Sbc, 0x15);
            Add("DEC", Opcode.Dec, 0x24);
            Add("RSB", Opcode.Rsb, 0x15);          
            Add("RSC", Opcode.Rsc, 0x15);

            Add("STR", Opcode.Str, 0x54);
            Add("LDRST", Opcode.Ldr, 0x54); //3 Lines

            Add("LDM", Opcode.Nul, 3, 0x80);
            Add("STM", Opcode.Nul, 0x80);

            Add("LDPC", Opcode.Ldpc, 0x84);
            Add("STPC", Opcode.Stpc, 0x84);

            Add("BX", Opcode.Nul, 0x80);
            Add("^", Opcode.Nul, 0x80);

            Add("MOD", Opcode.Mod, 0x86);
            Add("EOI", Opcode.Eoi, 0x81);
            Add("IRQ", Opcode.Irq, 0x82);            
        }
    }
}
