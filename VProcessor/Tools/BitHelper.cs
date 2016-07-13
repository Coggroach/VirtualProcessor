using System;

namespace VProcessor.Tools
{
    public class BitHelper
    {
        public static bool BitMatch(ulong value, byte bitPos, byte matchBit, uint mask = 1)
        {
            return (value >> bitPos & mask) == matchBit;
        }

        public static ulong BitExtract(ulong value, byte bitPos, uint mask = 1)
        {
            return (value >> bitPos) & mask;
        }

        public static bool BitMatch(uint value, byte bitPos, byte matchBit, uint mask = 1)
        {
            return ((value >> bitPos) & mask) == matchBit;
        }

        public static uint BitExtract(uint value, byte bitPos, uint mask = 1)
        {
            return (value >> bitPos) & mask;
        }

        public static bool MatchMask(byte value, byte mask)
        {
            return (value & mask) == mask;
        }

        public static uint Negate4Bits(uint extract)
        {
             if(extract >= 0x8000)
                    extract = ~(~extract ^ 0xFFFF0000);

            return extract;
        }

        public static uint Negate(int value)
        {
            return ~(uint)Math.Abs(value) + 1;
        }

        public static uint Subtract(int i, int j)
        {
            var result = i - j;
            uint extract = 0;
            if (result < 0)
                extract = (~(uint)Math.Abs(result) + 1);
            else
                extract = (uint)result;

            return extract & 0xFFFF;
        }
    }
}
