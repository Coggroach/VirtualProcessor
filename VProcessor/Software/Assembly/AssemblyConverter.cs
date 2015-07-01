using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VProcessor.Hardware;

namespace VProcessor.Software.Assembly
{
    class AssemblyConverter
    {
        private const String CommandRoot = @"[\w][\w][\w][:][rR][\d][,]";
        private const String RegisterRoot = @"[rR][\d]";
        private const String BranchRoot = @"[\w][\w][\w][:]";
        private const String RegisterCompound = RegisterRoot + @"[,]";


        static AssemblyConverter()
        {
            
        }

        public static UInt64 Convert(String s)
        {
            //LDR r0,=0x1010
            //ADD r3, r3, r1
            //MOV r0,#0
            //BEQ branch
            //branch
            
            var line = s.Trim().Replace(" ", ",").Replace("\t", ",").Replace(",,", ",");
            var split = line.Split(',');

            if (!Opcode.GetCodeTable().Contains(split[0]))
                return UInt64.MinValue;

            return 0;
        }

        private static Byte GetRegisterCode(String s)
        {
            return Byte.Parse(s.Replace("r", ""));
        }

    }
}
