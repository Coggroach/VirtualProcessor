using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Common;

namespace VProcessor.Hardware.Components
{
    public class Datapath
    {
        public static readonly Byte RegisterFileSize = (Byte) Math.Ceiling(Math.Log(UInt16.MaxValue, 2));
        private static readonly Byte RegisterContentsSize = (Byte) (RegisterFileSize*2);
        private const Byte ChannelOutSize = 3;
        public const Byte ChannelA = 0;
        public const Byte ChannelB = 1;
        public const Byte ChannelD = 2;

        private UInt32[] registers;
        private Byte[] channels;
        private UInt32 constIn;
        private Boolean constEnable;
        private Register nzcv;
        public Datapath()
        {
            this.registers = new UInt32[RegisterFileSize];
            this.channels = new Byte[ChannelOutSize];
            this.constIn = 0;
            this.Reset();
        }

        public void Reset()
        {
            for (var i = 0; i < this.registers.Length; i++)
                this.registers[i] = 0;
            for (var i = 0; i < this.channels.Length; i++)
                this.channels[i] = 0;
            this.nzcv = new Register();
        }

        public void SetChannel(Byte channel, Byte value)
        {
            this.channels[channel] = value;
        }

        public void SetRegister(Byte register, UInt32 value)
        {
            this.registers[register] = value;
        }

        public void SetConstIn(Boolean b, UInt32 i)
        {
            this.constIn = i;
            this.constEnable = b;
        }

        public UInt32[] GetRegisters()
        {
            return this.registers;
        }

        public UInt32 GetRegister(Byte channel = 0)
        {
            return (channel == ChannelB && this.constEnable) ? this.constIn : this.registers[this.channels[channel]];
        }

        private void SetChannelD(UInt32 value)
        {
            this.registers[this.channels[ChannelD]] = value;
        }

        public Byte GetChannel(Byte channel)
        {
            return this.channels[channel];
        }

        public void SetStatusRegister(Register reg)
        {
            this.nzcv = reg;
        }

        public Register GetStatusRegister()
        {
            return this.nzcv;
        }
                
        public UInt32 FunctionUnit(Byte code, Byte load = 0)
        {
            var a = this.GetRegister(ChannelA);
            var b = this.GetRegister(ChannelB);
            var d = this.GetRegister(ChannelD);
            var f = this.FunctionUnit(code, d, a, b);

            if (load == 1)
                this.SetChannelD(f);
            return f;
        }

        private UInt32 Shifter(UInt32 b, UInt32 direction, Boolean barrel = false)
        {
            switch (direction)
            {
                case 0:
                {
                    var c = 1 & b;                    

                    this.nzcv.BitSet(0, b > Int32.MaxValue);
                    this.nzcv.BitSet(1, c == 1);

                    return barrel ? ((b >> 1) | (c << 31)): b >> 1;
                }
                default:
                {
                    var c = (0x80000000 & b) >> 30;
                    var v = b >= Int32.MaxValue ? 1 : 0;

                    this.nzcv.BitSet(0, b >= Int32.MaxValue);
                    this.nzcv.BitSet(1, c == 2);
                    
                    return barrel ? ((b << 1) | (c >> 1)) : b << 1;
                }
            }
        }

        private UInt32 FunctionUnit(Byte code, UInt32 d, UInt32 a, UInt32 b)
        {
            UInt32 result = 0;
            switch (code)
            {
                //Data Movement
                case Opcode.LDR:
                    result = a;
                    break;
                case Opcode.LDRC:
                    result = b;
                    break;
                case Opcode.MOV:
                    result = b;
                    break;
                case Opcode.STR:
                    result = d;
                    break;                

                //Arithmetic
                case Opcode.INC:
                    result = this.RippleAdder(a, 0, 1);
                    break;
                case Opcode.ADD:
                    result = this.RippleAdder(a, b);
                    break;
                case Opcode.ADDI:
                    result = this.RippleAdder(a, b, 1);
                    break;
                case Opcode.SUBD:
                    result = this.RippleAdder(a, ~b);
                    break;
                case Opcode.SUB:
                    result = this.RippleAdder(a, ~b, 1);
                    break;
                case Opcode.DEC:
                    result = this.RippleAdder(a, UInt32.MaxValue - 1, 1);
                    break;
                case Opcode.ADC:
                    result = this.RippleAdder(a, b, (UInt32) this.nzcv.GetBit(1));
                    break;
                case Opcode.RSB:
                    result = this.RippleAdder(b, ~a, 1);
                    break;
                case Opcode.RSC:
                    result = this.RippleAdder(b, ~a, (UInt32) this.nzcv.GetBit(1));
                    break;
                case Opcode.SBC:
                    result = this.RippleAdder(a, ~b, (UInt32) this.nzcv.GetBit(1));
                    break;

                //Multiplication
                case Opcode.MUL:
                    result = this.BoothMultiplier(a, b);
                    break;

                //Shifting
                case Opcode.LSL:
                    result = this.Shifter(b, 1);
                    break;
                case Opcode.LSR:
                    result = this.Shifter(b, 0);
                    break;
                case Opcode.ROL:
                    result = this.Shifter(b, 1, true);
                    break;
                case Opcode.ROR:
                    result = this.Shifter(b, 0, true);
                    break;

                //Logical
                case Opcode.AND:
                    result = a & b;
                    break;
                case Opcode.EOR:
                    result = a ^ b;
                    break;
                case Opcode.ORR:
                    result = a | b;
                    break;
                case Opcode.BIC:
                    result = a & (~b);
                    break;

                //Comparisons
                case Opcode.CMP:
                    result = this.RippleAdder(d, ~b, 1);
                    break;
                case Opcode.CMN:
                    result = this.RippleAdder(d, b);
                    break;
                case Opcode.TEQ:
                    result = a ^ b;
                    break;
                case Opcode.TST:
                    result = a & b;
                    break;

            }
            this.nzcv.BitSet(2, result == 0);
            this.nzcv.BitSet(3, result > Int32.MaxValue);
            this.nzcv.Mask();

            return result;
        }

        private UInt32 BoothMultiplier(UInt32 a, UInt32 b)
        {   
            this.nzcv.BitSet(1, (Int32)(a * b) <= 0);
            this.nzcv.BitSet(0, IsNegative(a) && IsNegative(b));

            return a*b;
        }

        private UInt32 RippleAdder(UInt32 a, UInt32 b, UInt32 cIn = 0)
        {
            var sum = a + b + cIn;
            this.nzcv.BitSet(1, CarryOut(a, b, cIn));
            this.nzcv.BitSet(0, Overflow(sum, a, b));

            return sum;
        }

        private static Boolean Overflow(UInt32 sum, UInt32 a, UInt32 b)
        {
            return IsPositive(a) && IsPositive(b) && IsNegative(sum);
        }

        private static Boolean IsPositive(UInt32 a)
        {
            return a >= 0 && a <= Int32.MaxValue;
        }

        private static Boolean IsNegative(UInt32 a)
        {
            return a > Int32.MaxValue;
        }

        private static Boolean CarryOut(UInt32 a, UInt32 b, UInt32 c)
        {
            var longA = (UInt64) a;
            var longB = (UInt64) b;
            var longC = (UInt64) c;

            return longA + longB + longC > UInt32.MaxValue;
        }
    }
}
