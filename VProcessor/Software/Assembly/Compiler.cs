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
    public class Compiler
    {
        private const String GeneralRoot = @"^[\w][\w]+[,][rR][\d]+[,][rR][\d]+[,]";
        private const String DataRoot = @"^[\w][\w]+[,][rR][\d]+[,]";
        private const String BranchRoot = @"^[\w]+[,][\d]+";
        private const String ReferenceRoot = @"^[\d]+$";
        private const String RegisterStem = @"[rR][\d]+";
        private const String ConstNumberStem = @"[#][\d]+";
        private const String FullNumberStem = @"[=][\d]+";


        public static UInt64[] ConvertWithType(String s)
        {
            var table = CreatePropertyTable(s);
            var stem = (Int32)table["Stem"];

            var array = new UInt64[
                (stem & 2) == 2 ? (Int32) table["Contains"] + 1 : 1];
            for (var i = 0; i < array.Length; i++)
                array[i] = 0;
            
            var parts = (String[]) table["Code"];
            var type = (Int32) table["Type"];

            array[0] |= (UInt64) (Opcode.GetCodeIndexer(parts[0]) << 16);
            for (var i = 1; i <= 3 - type; i++)
                array[0] |= (UInt64) (GetRegisterCode(parts[i]) << ((3 - i) * 4));

            var lastElement = parts.Length - 1;
            if (Regex.Match(parts[lastElement], RegisterStem).Success)
                array[0] |= GetRegisterCode(parts[lastElement]);
            if (Regex.Match(parts[lastElement], ConstNumberStem).Success)
                array[0] |= GetConstantNumberCode(parts[lastElement]);
            if (Regex.Match(parts[lastElement], FullNumberStem).Success)
                array[1] |= (UInt64) GetFullNumberCode(parts[lastElement]);

            return array;
        }

        public static Hashtable CreatePropertyTable(String s)
        {
            var table = new Hashtable();
            var line = CleanUp(s);
            var parts = line.Split(',');

            var type = Opcode.GetCodeType(parts[0]);

            table.Add("Code", parts);
            table.Add("Type", (type & 0xF0) >> 4);
            table.Add("Stem", type & 0x0F);
            table.Add("Contains", GetStemSymbols(line).Count);
            
            return table;
        }

        private static IList GetStemSymbols(String s)
        {
            var list = new ArrayList();
            if (s.Contains("#"))
                list.Add("#");
            if (s.Contains("="))
                list.Add("=");
            return list;
        }

        private static IEnumerable<String> GetStem(Int32 b)
        {
            var list = new ArrayList();

            if ((b & 0x4) == 0x4)
                list.Add(RegisterStem);
            if ((b & 0x2) == 0x2)
                list.Add(FullNumberStem);
            if ((b & 0x1) == 0x1)
                list.Add(ConstNumberStem);

            return (String[]) list.ToArray();
        }

        private static String GetRoot(Int32 b)
        {
            switch (b)
            {
                case 0x10:
                    return GeneralRoot;
                case 0x20:
                    return DataRoot;
                case 0x30:
                    return BranchRoot;
                default:
                    return ReferenceRoot;
            }
        }

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
            return (Byte) (Byte.Parse(s.Replace("#", "")) & 0xF);
        }

        private static Int32 GetFullNumberCode(String s)
        {
            return Int32.Parse(s.Replace("=", ""));
        }
    }
}
