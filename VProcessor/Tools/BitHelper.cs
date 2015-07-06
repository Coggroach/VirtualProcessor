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
    }
}
