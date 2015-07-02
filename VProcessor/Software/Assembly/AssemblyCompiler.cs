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
    public class AssemblyCompiler
    {
        private const String GeneralRoot = @"^[\w][\w]+[,][rR][\d]+[,][rR][\d]+[,]";
        private const String DataRoot = @"^[\w][\w]+[,][rR][\d]+[,]";
        private const String BranchRoot = @"^[\w]+[,][\d]+";
        private const String ReferenceRoot = @"^[\d]+$";
        private const String RegisterStem = @"[rR][\d]+";
        private const String ConstNumberStem = @"[#][\d]+";
        private const String FullNumberStem = @"[=][\d]+";

        private static UInt64 ConvertGeneral(String s)
        {
            var split = s.Split(',');
            var convert = 0;

            convert |= Opcode.GetCodeIndexer(split[0]) << 16;
            for (var i = 1; i <= 2; i++)
            convert |= (GetRegisterCode(split[i]) << ((3 - i) * 4));

            if (Regex.Match(split[3], ConstNumberStem).Success)
                convert |= GetConstantNumberCode(split[3]);
            else if (Regex.Match(split[3], RegisterStem).Success)
                convert |= GetRegisterCode(split[3]);

            return (UInt64) convert;
        }

        private static UInt64 ConvertData(String s)
        {
            var split = s.Split(',');
            var convert = 0;

            convert |= Opcode.GetCodeIndexer(split[0]) << 16;
            for (var i = 1; i <= 2; i++)
                convert |= (GetRegisterCode(split[i]) << ((3 - i) * 4));

            if (Regex.Match(split[3], ConstNumberStem).Success)
                convert |= GetConstantNumberCode(split[3]);
            else if (Regex.Match(split[3], RegisterStem).Success)
                convert |= GetRegisterCode(split[3]);
            else if (Regex.Match(split[3], FullNumberStem).Success)
                convert |= GetFullNumberCode(split[3]);

            return (UInt64)convert;
        }

        private static UInt64 ConvertBranch(String s)
        {
            var split = s.Split(',');
            var convert = 0;

            convert |= Opcode.GetCodeIndexer(split[0]) << 16;
            convert |= Int32.Parse(split[1]);

            return (UInt64)convert;
        }

        private static String CleanUp(String s)
        {
            return new Regex(@"[ ]{2,}", RegexOptions.None).Replace(s.Replace("\t", " "), @" ").Trim().Replace(" ", ",").Replace(",,,", ",").Replace(",,", ",");
        }

        public static UInt64 Convert(String s)
        {
            //LDR r0,=0x1010
            //ADD r3, r3, r1
            //MOV r0,#0
            //BEQ branch
            //branch

            var line = CleanUp(s);

            if (Regex.Match(line, GeneralRoot).Success)
                return ConvertGeneral(line);
            if (Regex.Match(line, DataRoot).Success)
                return ConvertData(line);
            if (Regex.Match(line, BranchRoot).Success)
                return ConvertBranch(line);
            if (Regex.Match(line, ReferenceRoot).Success)
                throw CompilerException("Unimplemented References");

            throw CompilerException("Invalid Input");
        }

        private static Exception CompilerException(String p)
        {
            throw new Exception(p);
        }

        private static Byte GetRegisterCode(String s)
        {
            return Byte.Parse(s.ToLower().Replace("r", ""));
        }

        private static Byte GetConstantNumberCode(String s)
        {
            return Byte.Parse(s.Replace("#", ""));
        }

        private static Int32 GetFullNumberCode(String s)
        {
            return Int32.Parse(s.Replace("=", ""));
        }
    }
}
