using System;
using VProcessor.Common;
using VProcessor.Hardware.Interfacing;

namespace VProcessor.Hardware.Components
{
    [Flags]
    public enum DatapathMode : byte
    {
        System = 1,
        Interupt = 2,
        Previlaged = 4,
        UnPrevilaged = 8
    }

    public class Datapath : IDatapath
    {
        #region Constants
        public static readonly byte RegisterFileSize = (byte) Math.Ceiling(Math.Log(ushort.MaxValue, 2));
        public static readonly byte RegisterContentsSize = (byte) (RegisterFileSize*2);
        private const byte ChannelOutSize = 3;

        public const byte ChannelA = 0;
        public const byte ChannelB = 1;
        public const byte ChannelD = 2;
        public const byte Modes = 4;
        #endregion

        #region Attributes
        private readonly RegisterFile[] _registers;
        private DatapathMode _mode;
        private readonly byte[] _channels;
        private uint _constIn;
        private bool _constEnable;
        private Register _nzcv;

        private byte ModeIndex => ParseMode(_mode);

        #endregion

        public Datapath()
        {
            _registers = new RegisterFile[Modes];
            _channels = new byte[ChannelOutSize];
            _constIn = 0;
            _mode = DatapathMode.System;
            Reset();
        }

        public void Reset()
        {
            for (var i = 0; i < _registers.Length; i++)
            {
                _registers[i] = new RegisterFile();
                _registers[i].Reset();
            }            
            for (var i = 0; i < _channels.Length; i++)
                _channels[i] = 0;
            _nzcv = new Register();
            _mode = DatapathMode.System;
            _constEnable = false;
            _constIn = 0;
        }

        #region Channel Functions
        public void SetChannel(byte channel, byte value)
        {
            _channels[channel] = value;
        }

        private void SetChannelD(uint value)
        {
            _registers[ModeIndex].SetRegister(_channels[ChannelD], value);
        }

        public byte GetChannel(byte channel)
        {
            return _channels[channel];
        }
        #endregion

        #region Register Functions
        public void SetConstIn(bool b, uint i)
        {
            _constIn = i;
            _constEnable = b;
        }

        public void SetRegister(byte register, uint value)
        {
            _registers[ModeIndex].SetRegister(register, value);
        }

        public uint[] GetRegisters()
        {
            return _registers[ModeIndex].GetRegisters();
        }

        public uint GetRegister(byte channel = 0)
        {
            return (channel == ChannelB && _constEnable) ? _constIn : _registers[ModeIndex].GetRegister(_channels[channel]);
        }
        #endregion

        #region StatusRegister
        public void SetStatusRegister(Register reg)
        {
            _nzcv = reg;
        }

        public Register GetStatusRegister()
        {
            return _nzcv;
        }
        #endregion

        #region Mode
        public void SetMode(DatapathMode mode)
        {
            if (ValidMode(mode))
                _mode = mode;
        }

        public DatapathMode GetMode()
        {
            return _mode;
        }

        public bool ValidMode(DatapathMode destMode)
        {
            if (_mode == DatapathMode.System || _mode == DatapathMode.Interupt)
                return true;
            if (_mode == DatapathMode.Previlaged && destMode != DatapathMode.System)
                return true;
            return _mode == destMode;
        }

        private byte ParseMode(DatapathMode mode)
        {
            return (byte)Math.Log((byte)mode, 2);
        }
        #endregion

        #region FunctionUnit
        public uint FunctionUnit(byte code, bool load = false)
        {
            var a = GetRegister();
            var b = GetRegister(ChannelB);
            var d = GetRegister(ChannelD);
            var f = FunctionUnit(code, d, a, b);

            if (load)
                SetChannelD(f);
            return f;
        }

        public uint FunctionUnit(Opcode code, bool load = false)
        {
            return FunctionUnit((byte)code, load);
        }

        public uint FunctionUnit(byte code, byte load)
        {
            return FunctionUnit(code, load == 1);
        }

        public uint FunctionUnit(Opcode code, byte load)
        {
            return FunctionUnit((byte)code, load);
        }

        private uint Shifter(uint b, uint direction, bool barrel = false)
        {
            switch (direction)
            {
                case 0:
                {
                    var c = 1 & b;                    

                    _nzcv.BitSet(0, b > int.MaxValue);
                    _nzcv.BitSet(1, c == 1);

                    return barrel ? ((b >> 1) | (c << 31)): b >> 1;
                }
                default:
                {
                    var c = (0x80000000 & b) >> 30;

                    _nzcv.BitSet(0, b >= int.MaxValue);
                    _nzcv.BitSet(1, c == 2);
                    
                    return barrel ? (b << 1) | (c >> 1) : b << 1;
                }
            }
        }

        private uint FunctionUnit(byte code, uint d, uint a, uint b)
        {
            uint result = 0;
            switch ((Opcode) code)
            {
                //Data Movement
                case Opcode.Ldr:
                    result = a;
                    break;
                case Opcode.Ldrc:
                    result = b;
                    break;
                case Opcode.Mov:
                    result = b;
                    break;
                case Opcode.Str:
                    result = d;
                    break;                

                //Arithmetic
                case Opcode.Inc:
                    result = RippleAdder(a, 0, 1);
                    break;
                case Opcode.Add:
                    result = RippleAdder(a, b);
                    break;
                case Opcode.Addi:
                    result = RippleAdder(a, b, 1);
                    break;
                case Opcode.Subd:
                    result = RippleAdder(a, ~b);
                    break;
                case Opcode.Sub:
                    result = RippleAdder(a, ~b, 1);
                    break;
                case Opcode.Dec:
                    result = RippleAdder(a, uint.MaxValue - 1, 1);
                    break;
                case Opcode.Adc:
                    result = RippleAdder(a, b, _nzcv.GetBit(1));
                    break;
                case Opcode.Rsb:
                    result = RippleAdder(b, ~a, 1);
                    break;
                case Opcode.Rsc:
                    result = RippleAdder(b, ~a, _nzcv.GetBit(1));
                    break;
                case Opcode.Sbc:
                    result = RippleAdder(a, ~b, _nzcv.GetBit(1));
                    break;

                //Multiplication
                case Opcode.Mul:
                    result = BoothMultiplier(a, b);
                    break;

                //Shifting
                case Opcode.Lsl:
                    result = Shifter(b, 1);
                    break;
                case Opcode.Lsr:
                    result = Shifter(b, 0);
                    break;
                case Opcode.Rol:
                    result = Shifter(b, 1, true);
                    break;
                case Opcode.Ror:
                    result = Shifter(b, 0, true);
                    break;

                //Logical
                case Opcode.And:
                    result = a & b;
                    break;
                case Opcode.Eor:
                    result = a ^ b;
                    break;
                case Opcode.Orr:
                    result = a | b;
                    break;
                case Opcode.Bic:
                    result = a & (~b);
                    break;

                //Comparisons
                case Opcode.Cmp:
                    result = RippleAdder(d, ~b, 1);
                    break;
                case Opcode.Cmn:
                    result = RippleAdder(d, b);
                    break;
                case Opcode.Teq:
                    result = a ^ b;
                    break;
                case Opcode.Tst:
                    result = a & b;
                    break;

            }
            _nzcv.BitSet(2, result == 0);
            _nzcv.BitSet(3, result > int.MaxValue);
            _nzcv.Mask();

            return result;
        }

        private uint BoothMultiplier(uint a, uint b)
        {   
            _nzcv.BitSet(1, (int)(a * b) <= 0);
            _nzcv.BitSet(0, IsNegative(a) && IsNegative(b));

            return a*b;
        }

        private uint RippleAdder(uint a, uint b, uint cIn = 0)
        {
            var sum = a + b + cIn;
            _nzcv.BitSet(1, CarryOut(a, b, cIn));
            _nzcv.BitSet(0, Overflow(sum, a, b));

            return sum;
        }

        private static bool Overflow(uint sum, uint a, uint b)
        {
            return IsPositive(a) && IsPositive(b) && IsNegative(sum);
        }

        private static bool IsPositive(uint a)
        {
            return a <= int.MaxValue;
        }

        private static bool IsNegative(uint a)
        {
            return a > int.MaxValue;
        }

        private static bool CarryOut(uint a, uint b, uint c)
        {
            var longA = (ulong) a;
            var longB = (ulong) b;
            var longC = (ulong) c;

            return longA + longB + longC > uint.MaxValue;
        }
        #endregion
    }
}
