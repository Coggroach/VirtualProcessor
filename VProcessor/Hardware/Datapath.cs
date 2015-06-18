﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class Datapath
    {
        private static readonly Byte RegisterFileSize = (Byte) Math.Log(UInt16.MaxValue, 2);
        private static readonly Byte RegisterContentsSize = (Byte) (RegisterFileSize*2);
        private static readonly Byte ChannelOutSize = 2;

        private Int32[] registers;
        private Byte[] channels;
        private Byte nzcv;

        public Datapath()
        {
            this.registers = new Int32[RegisterFileSize];
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

        public void SetRegister(Byte register, Int32 value)
        {
            this.registers[register] = value;
        }

        public Int32 GetRegister(Byte channel)
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

        public void FunctionUnit(Byte code, Byte destination)
        {
            this.registers[destination] = this.FunctionUnit(code);
        }
        
        private Int32 Shifter(Int32 b, Int32 direction)
        {
            return direction == 0 ? b << 1 : b >> 1;
        }
        
        private Int32 FunctionUnit(Byte code)
        {
            var result = 0;
            var a = this.registers[this.channels[0]];
            var b = this.registers[this.channels[1]];
            switch (code)
            {
                case AssemblyTable.LDR:
                    result = a;
                    break;
                case AssemblyTable.INC:
                    result = this.RippleAdder(a, 0, 1);
                    break;
                case AssemblyTable.ADD:
                    result = this.RippleAdder(a, b);
                    break;
                case AssemblyTable.ADDI:
                    result = this.RippleAdder(a, b, 1);
                    break;
                case AssemblyTable.SUBD:
                    result = this.RippleAdder(a, ~b);
                    break;
                case AssemblyTable.SUB:
                    result = this.RippleAdder(a, ~b, 1);
                    break;
            }

            if (result == 0)
                this.nzcv |= 1 << 2;
            if (result < 0)
                this.nzcv |= 1 << 3;

            this.nzcv &= 0x0F;

            return result;
        }

        private Int32 RippleAdder(Int32 a, Int32 b, Int32 cIn = 0)
        {
            a += cIn;
            do
            {
                var and = a & b;
                var xor = a ^ b;
                and <<= 1;

                a = and;
                b = xor;
            } while (a != 0);
            return b;
        }
    }
}