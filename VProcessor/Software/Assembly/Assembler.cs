using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VProcessor.Hardware;

namespace VProcessor.Software.Assembly
{
    public class Assembler : IAssembler
    {
        private const String GeneralRoot = @"^[\w][\w]+[,][rR][\d]+[,][rR][\d]+[,]";
        private const String DataRoot = @"^[\w][\w]+[,][rR][\d]+[,]";
        private const String BranchRoot = @"^[\w]+[,][\d]+";
        private const String ReferenceRoot = @"^[\d]+$";
        private const String RegisterStem = @"[rR][\d]+";
        private const String ConstNumberStem = @"[#][\d]+";
        private const String FullNumberStem = @"[=][\d]+";


        public Memory64 Compile64(SFile file, Int32 size)
        {
            var lines = file.GetString().Split(';');
            var memory = new Memory64(size);
            var mode = file.GetMode();

            for (var i = 0; i < memory.GetLength(); i++)
            {
                try
                {
                    memory.SetMemory(i, ParseValue64(lines[i], mode));
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is IndexOutOfRangeException)
                    {
                        memory.SetMemory(i, 0);
                        continue;
                    }
                    throw;
                }
            }
            return memory;
        }

        public Memory32 Compile32(SFile file, Int32 size)
        {
            var lines = file.GetString().Split(SFile.Delimiter);
            var memory = new Memory32(size);
            var mode = file.GetMode();
            var index = 0;

            for (var i = 0; i < memory.GetLength(); i++, index++)
            {
                try
                {
                    UInt32[] array = ParseValue32(lines[index], mode);
                    for (var j = 0; j < array.Length; j++)
                        memory.SetMemory(i + j, array[j]);
                    i += array.Length - 1;
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is IndexOutOfRangeException)
                    {
                        memory.SetMemory(i, 0);
                        continue;
                    }
                    throw;
                }
            }
            return memory;
        }

        public static UInt32[] Convert(String s)
        {
            var table = CreatePropertyTable(s);
            var stem = (Int32)table["Stem"];

            var array = new UInt32[
                (stem & 2) == 2 ? (Int32)table["Contains"] + 1 : 1];
            for (var i = 0; i < array.Length; i++)
                array[i] = 0;

            var parts = (String[])table["Code"];
            var type = (Int32)table["Type"];

            var address = Opcode.GetCodeAddress(parts[0].ToUpper());

            for (var i = 1; i <= 3 - type; i++)
                array[0] |= (UInt32)(GetRegisterCode(parts[i]) << ((3 - i) * 4));

            var lastElement = parts.Length - 1;
            if (Regex.Match(parts[lastElement], RegisterStem).Success && (stem & 4) == 4)
            {
                array[0] |= GetRegisterCode(parts[lastElement]);
                if ((stem & 2) == 2)
                    address += 3;
            }
            else if (Regex.Match(parts[lastElement], ConstNumberStem).Success && (stem & 1) == 1)
            {
                array[0] |= GetConstantNumberCode(parts[lastElement]);
                if ((stem & 2) == 2)
                    address += 3;
                if ((stem & 4) == 4)
                    address += 1;
            }
            else if (Regex.Match(parts[lastElement], FullNumberStem).Success && (stem & 2) == 2)
                array[1] |= (UInt32)GetFullNumberCode(parts[lastElement]);

            array[0] |= (UInt32)(address << 16);

            return array;
        }

        private static Hashtable CreatePropertyTable(String s)
        {
            var table = new Hashtable();
            var line = CleanUp(s);
            var parts = line.Split(',');

            var type = Opcode.GetCodeType(parts[0].ToUpper());

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

            return (String[])list.ToArray();
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
            if (s.Contains("#0x"))
                return Byte.Parse(s.Replace("#0x", ""), NumberStyles.HexNumber);

            return (Byte)(Byte.Parse(s.Replace("#", "")) & 0xF);
        }

        private static Int32 GetFullNumberCode(String s)
        {
            if (s.Contains("=0x"))
                return Int32.Parse(s.Replace("=0x", ""), NumberStyles.HexNumber);

            return Int32.Parse(s.Replace("=", ""));
        }

        private static String CleanUp(String s)
        {
            return new Regex(@"[ ]{2,}", RegexOptions.None).Replace(s.Replace("\t", " "), @" ").Trim().Replace(" ", ",").Replace(",,,", ",").Replace(",,", ",");
        }

        private static UInt64 ParseValue64(String s, Int32 mode)
        {
            switch (mode)
            {
                case SFile.Hexadecimal:
                    return UInt64.Parse(s.Replace(" ", ""), NumberStyles.HexNumber);
                case SFile.Decimal:
                    return UInt64.Parse(s, NumberStyles.Number);
                default:
                    return 0;
            }
        }

        private static UInt32[] ParseValue32(String s, Int32 mode)
        {
            switch (mode)
            {
                case SFile.Hexadecimal:
                    return new UInt32[] { UInt32.Parse(s.Replace(" ", ""), NumberStyles.HexNumber) };
                case SFile.Decimal:
                    return new UInt32[] { UInt32.Parse(s, NumberStyles.Number) };
                case SFile.Assembly:
                    return Convert(s);
                default:
                    return new UInt32[] { 0 };
            }
        }
    }
}
