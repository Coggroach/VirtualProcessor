using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Tools
{
    public class BitHelper
    {
        public static Boolean BitMatch(UInt64 value, Byte bitPos, Byte matchBit, UInt32 mask = 1)
        {
            return ((value >> bitPos) & mask) == matchBit;
        }

        public static UInt64 BitExtract(UInt64 value, Byte bitPos, UInt32 mask = 1)
        {
            return (value >> bitPos) & mask;
        }

        public static Boolean BitMatch(UInt32 value, Byte bitPos, Byte matchBit, UInt32 mask = 1)
        {
            return ((value >> bitPos) & mask) == matchBit;
        }

        public static UInt32 BitExtract(UInt32 value, Byte bitPos, UInt32 mask = 1)
        {
            return (value >> bitPos) & mask;
        }

        public static UInt32 Negate4Bits(UInt32 extract)
        {
             if(extract >= 0x8000)
                    extract = ~(~extract ^ 0xFFFF0000);

            return extract;
        }

        public static UInt32 Negate(Int32 value)
        {
            return ~(UInt32)Math.Abs(value) + 1;
        }

        public static UInt32 Subtract(Int32 i, Int32 j)
        {
            var result = i - j;
            UInt32 extract = 0;
            if (result < 0)
                extract = (~(UInt32)Math.Abs(result) + 1);
            else
                extract = (UInt32)result;

            return extract & 0xFFFF;
        }
    }
}
