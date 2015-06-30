using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Datapath
    {
        public static readonly Byte RegisterFileSize = (Byte) Math.Log(UInt16.MaxValue, 2);
        private static readonly Byte RegisterContentsSize = (Byte) (RegisterFileSize*2);
        private static readonly Byte ChannelOutSize = 2;

        private UInt32[] registers;
        private Byte[] channels;
        private Byte nzcv;
        public Datapath()
        {
            this.registers = new UInt32[RegisterFileSize];
            this.channels = new Byte[ChannelOutSize];
            this.StartUp();
        }

        public void StartUp()
        {
            for (var i = 0; i < this.registers.Length; i++)
                this.registers[i] = 0;
            for (var i = 0; i < this.channels.Length; i++)
                this.channels[i] = 0;
            this.nzcv = 0;
        }

        public void SetChannel(Byte channel, Byte value)
        {
            this.channels[channel] = value;
        }

        public void SetRegister(Byte register, UInt32 value)
        {
            this.registers[register] = value;
        }

        public UInt32[] GetRegisters()
        {
            return this.registers;
        }

        public UInt32 GetRegister(Byte channel)
        {
            return this.registers[this.channels[channel]];
        }

        public Byte GetChannel(Byte channel)
        {
            return this.channels[channel];
        }

        public Byte GetNzcv()
        {
            return this.nzcv;
        }

        public void FunctionUnit(Byte code, Byte destination = 0, Byte load = 0)
        {
            var a = this.registers[this.channels[0]];
            var b = this.registers[this.channels[1]];
            var f = this.FunctionUnit(code, a, b);

            if (load != 1) return;
            this.registers[destination] = f;
        }

        private UInt32 Shifter(UInt32 b, UInt32 direction)
        {
            return direction == 0 ? b << 1 : b >> 1;
        }

        private UInt32 FunctionUnit(Byte code, UInt32 a, UInt32 b)
        {
            UInt32 result = 0;
            switch (code)
            {
                case Opcode.LDR:
                    result = a;
                    break;
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
                case Opcode.CMP:
                    result = this.RippleAdder(a, ~b, 1);
                    break;
            }

            if (result == 0)
                this.nzcv |= 1 << 2;
            if (result > Int32.MaxValue)
                this.nzcv |= 1 << 3;

            this.nzcv &= 0x0F;

            return result;
        }

        private UInt32 RippleAdder(UInt32 a, UInt32 b, UInt32 cIn = 0)
        {
            var cOut = 0;
            if ((Int32) a - (b + cIn) <= 0)
                cOut = 1;
            var v = 0;
            if (a >= Int32.MaxValue && (b + cIn) >= Int32.MaxValue)
                v = 1;

            this.nzcv |= (Byte) (v);
            this.nzcv |= (Byte)(cOut << 1);

            return a + b + cIn;
        }
    }
}
