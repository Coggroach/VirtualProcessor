using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using VProcessor.Common;
using VProcessor.Hardware.Memory;
using VProcessor.Tools;

namespace VProcessor.Software
{
    public interface IAssembler
    {
        Memory64 Compile64(VpFile file, int size);
        Memory32 Compile32(VpFile file, int size);
    }

    public class Assembler : IAssembler, IDisposable
    {
        #region Regexs
        private const string GeneralRoot = @"^[\w][\w]+[,][rR][\d]+[,][rR][\d]+[,]";
        private const string DataRoot = @"^[\w][\w]+[,][rR][\d]+[,]";
        private const string BranchRoot = @"^[\w]+[,][\d]+";
        private const string ReferenceRoot = @"^[\d]+$";
        private const string RegisterStem = @"[rR][\d]+";
        private const string ConstNumberStem = @"[#][-\d]+";
        private const string FullNumberStem = @"[=][-\d]+";
        private const string AddressStem = @"[\[][\w,#]+[\]]";
        private const string FullBranchStem = @"[=][\w]+";
        #endregion

        #region Attributes
        private readonly Hashtable _branchLookup;
        private readonly Hashtable _branchRegistry;
        private int _assemblyLine;
        private int _machineLine;
        public Hashtable KeywordLookup;
        private List<uint> _currentLines;
        #endregion

        public Assembler()
        {
            _branchLookup = new Hashtable();
            _branchRegistry = new Hashtable();
            _assemblyLine = 0;
            _machineLine = 0;
            KeywordLookup = new Hashtable();
            InitKeywordLookup();
            _currentLines = new List<uint>();
        }

        public void InitKeywordLookup()
        {
            KeywordLookup.Add("sp", "r14");
            KeywordLookup.Add("lr", "r13");            
            KeywordLookup.Add("rt", "r15");
            KeywordLookup.Add("System", "#0");
            KeywordLookup.Add("Interrupt", "#1");
            KeywordLookup.Add("Previlaged", "#2");
            KeywordLookup.Add("UnPrevilaged", "#3");
        }

        #region Exposed Compile Methods
        public Memory64 Compile64(VpFile file, int size)
        {
            var lines = file.GetString().Split(VpFile.Delimiter);
            var memory = new Memory64(size);
            var mode = file.GetMode();

            for (var i = 0; i < memory.Length; i++)
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

        public Memory32 Compile32(VpFile file, int size)
        {
            var lines = file.GetString().Split(VpFile.Delimiter);
            var memory = new Memory32(size);
            var mode = file.GetMode();
            for (_machineLine = 0, _assemblyLine = 0; 
                _machineLine < memory.Length; 
                _machineLine++, _assemblyLine++)
            {
                try
                {
                    var array = ParseValue32(lines[_assemblyLine], mode);
                    if (array == null)
                    {
                        _machineLine--;
                        continue;
                    }
                                         
                    for (var j = 0; j < array.Length; j++)
                        memory.SetMemory(_machineLine + j, array[j]);
                    _machineLine += array.Length - 1;
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is IndexOutOfRangeException)
                    {
                        memory.SetMemory(_machineLine, 0);
                        continue;
                    }
                    throw;
                }
            }

            LinkBranches();
            PostLinkBranches(memory);
            Dispose();

            return memory;
        }
        #endregion 

        #region CompoundLines
        private uint[] PcCompoundLine(string[] parts, string mode)
        {            
            var upperCode = parts[0].ToUpper();

            var movLine = "MOV ";

            movLine += mode == "STPC" ? "rt" : parts[1];
            movLine += ", ";
            movLine += mode == "LDPC" ? "rt" : parts[1];

            List<uint> lines = new List<uint>();

            var code = (uint)OpcodeRegistry.Instance.GetCodeAddress(upperCode) << 16;

            if (mode == "LDPC")
                lines.Add(code);
            lines.AddRange(ConvertLine32(movLine));
            if (mode == "STPC")
                lines.Add(code);

            return lines.ToArray();
        }

        private uint[] MemoryCompoundLine(string s, string cmd, string cmd2)
        {
            List<uint> lines = new List<uint>();
            List<string> assemblies = new List<string>();

            var mode = s.Contains('!');
            var match = Regex.Matches(CleanUp(s), RegisterStem);

            if (mode && cmd == "STR")
                assemblies.Add("MOV r15, #0");
            var incrementReg = (mode) ? "r14" : "r15";
            foreach (Capture capture in match)
            {
                var value = capture.Value;

                if (value == "r15" || value == "r14") continue;

                if (mode && cmd == "LDRST")
                {
                    assemblies.Add("MOV r15, #0");
                    assemblies.Add(cmd2 + " " + incrementReg + ", " + incrementReg + ", #1");
                }
                assemblies.Add(cmd + " " + value + ", [r14, r15]");

                if (mode && cmd == "STR")
                    assemblies.Add(cmd2 + " " + incrementReg + ", " + incrementReg + ", #1");
            }
            foreach (string assemblyLine in assemblies)
            {
                var lineConversion = ConvertLine32(assemblyLine);
                if(lineConversion != null)
                    lines.AddRange(lineConversion);
            }
            return lines.ToArray();
        }

        private uint[] SetModeLine(string[] parts)
        {
            var array = new uint[1];
            var addressOffset = GetConstantNumberCode(parts[1]);
            var address = (uint)OpcodeRegistry.Instance.GetCodeAddress(parts[0].ToUpper()) + addressOffset;

            if (addressOffset > 3)
                return null;

            array[0] |= address << 16;
            return array;
        }

        private uint[] SubroutineCompoundLine(string[] parts, string type)
        {
            if (parts.Length <= 0)
                return null;

            List<uint> list = new List<uint>();

            if(parts[0] == type)
            {
                switch(type)
                {
                    case "BX":
                        list.AddRange(ConvertLine32("LDPC lr"));
                        list.AddRange(ConvertLine32("ADD lr, lr, #4"));
                        list.AddRange(ConvertLine32("B " + parts[1], 3));
                        break;
                    case "^":
                        list.AddRange(ConvertLine32("STPC lr"));
                        break;
                }
            }
            return list.ToArray();
        }

        public uint[] DefaultCompoundLine(string[] parts)
        {
            var array = new uint[1];
            array[0] |= (uint)OpcodeRegistry.Instance.GetCodeAddress(parts[0].ToUpper()) << 16;
            return array;
        }

        #endregion

        #region ConvertLine32
        public uint[] ConvertLine32(string s, int machineBranchOffset = 0)
        {
            var line = CleanUp(s);
            var table = CreatePropertyTable(line);

            if(table == null)
            {                
                RegisterBranch(line);
                return null;
            }

            var stem = (int)table["Stem"];
            var parts = (string[])table["Code"];
            var type = (int)table["Type"];
            var lastElement = parts.Length - 1;
            var index = 0;
            var upperCode = parts[0].ToUpper();

            if((type & 8) == 8)
            {
                switch(upperCode)
                {
                    case "LDM":
                        return MemoryCompoundLine(s, "LDRST", "SUB");
                    case "STM":
                        return MemoryCompoundLine(s, "STR", "ADD");
                    case "LDPC":
                        return PcCompoundLine(parts, "LDPC");
                    case "STPC":
                        return PcCompoundLine(parts, "STPC");
                    case "BX":
                        return SubroutineCompoundLine(parts, "BX");
                    case "^":
                        return SubroutineCompoundLine(parts, "^");
                    case "MOD":
                        return SetModeLine(parts);
                    default:
                        return DefaultCompoundLine(parts);
                }
            }

            var array = new uint[
                ((stem & 2) == 2 || (type & 4) == 4 ) ? (int)table["Contains"] + 1 : 1];

            if (Regex.Match(parts[lastElement], ConstNumberStem).Success && (type & 4) == 4)
            {
                array[index] = ConvertLine32("MOV r15," + parts[lastElement])[0];
                parts[lastElement] = "r15";
                index++;
            }

            if((type & 5) == 5 && parts.Length < 4)
            {
                array[index] = ConvertLine32("MOV r15,#0")[0];
                var newArray = array.ToList();
                newArray.Add(0);
                array = newArray.ToArray();
                var newParts = parts.ToList();
                newParts.Add("r15");                
                parts = newParts.ToArray();
                lastElement++;
                index++;                
            }
            
            if (type == 3)
            {
                LinkBranch(parts[1], machineBranchOffset);
                array[index] |= 1;
            }


            var address = OpcodeRegistry.Instance.GetCodeAddress(upperCode);

            for (var i = 1; i <= 3 - (type & 3); i++)
                array[index] |= (uint)(GetRegisterCode(parts[i]) << ((3 - i) * 4));


            if (Regex.Match(parts[lastElement], RegisterStem).Success && (stem & 4) == 4)
            {
                array[index] |= GetRegisterCode(parts[lastElement]);
                if ((stem & 2) == 2)
                    address += 3;
            }
            else if (Regex.Match(parts[lastElement], ConstNumberStem).Success && (stem & 1) == 1)
            {
                array[index] |= GetConstantNumberCode(parts[lastElement]);
                if ((stem & 2) == 2)
                    address += 3;
                if ((stem & 4) == 4)
                    address += 1;
            }
            else if (Regex.Match(parts[lastElement], FullNumberStem).Success && (stem & 2) == 2)
                array[index ^ 1] |= (uint)GetFullNumberCode(parts[lastElement]);
            else if (Regex.Match(parts[lastElement], FullBranchStem).Success && (stem & 2) == 2)
                LinkBranch(parts[lastElement].Replace("=", ""), 1, true);

            array[index] |= (uint)(address << 16);

            return array;
        }
        #endregion

        #region Branch Registry
        private void RegisterBranch(string branchName, int offset = 0)
        {
            if(!_branchRegistry.ContainsKey(branchName))
            {
                var map = new Hashtable();

                map.Add("MachineLine", _machineLine + offset);
                map.Add("AssemblyLine", _assemblyLine);

                _branchRegistry.Add(branchName, map);
            }
        }

        private void LinkBranch(string branchName, int offset = 0, bool pointer = false)
        {
            var map = new Hashtable();

            map.Add("LinkMachineLine", _machineLine + offset);
            map.Add("LinkAssemblyLine", _assemblyLine);
            map.Add("Pointer", pointer);
            map.Add("BranchKey", branchName);

            _branchLookup.Add(branchName, map);
        }

        private void LinkBranches()
        {
            foreach(DictionaryEntry dictionary in _branchLookup)
            {
                var lookup = (Hashtable)dictionary.Value;
                lookup.Add("BranchRegistryKey", _branchRegistry.ContainsKey(lookup["BranchKey"]));                    
            }
        }

        private void PostLinkBranches(Memory32 memory)
        {
            foreach (DictionaryEntry dictionary in _branchLookup)
            {
                var lookup = (Hashtable) dictionary.Value;
                if ((bool)(lookup["BranchRegistryKey"]))
                {
                    var regLine = (int) ((Hashtable) _branchRegistry[lookup["BranchKey"]])["MachineLine"];
                    var linkLine = (int) lookup["LinkMachineLine"];
                    var pointer = (bool)lookup["Pointer"];

                    var difference = pointer ? (uint) regLine : BitHelper.Subtract(regLine, linkLine);
                    var currentValue = memory.GetMemory(linkLine);

                    currentValue &= 0xFFFF0000;
                    currentValue |= difference;

                    memory.SetMemory(linkLine, currentValue);
                }                
            }
        }
        #endregion

        #region PropertyTable
        private static Hashtable CreatePropertyTable(string line)
        {
            var table = new Hashtable();
            var parts = line.Split(',');
            var upperCode = parts[0].ToUpper();

            if (!OpcodeRegistry.Instance.IsValidCode(upperCode))
                return null;

            var type = OpcodeRegistry.Instance.GetCodeType(upperCode);

            table.Add("Code", parts);
            table.Add("Type", (type & 0xF0) >> 4);
            table.Add("Stem", type & 0x0F);
            table.Add("Contains", GetStemSymbols(line).Count);            

            return table;
        }

        private static IList GetStemSymbols(string s)
        {
            var list = new ArrayList();
            if (s.Contains("#"))
                list.Add("#");
            if (s.Contains("="))
                list.Add("=");
            return list;
        }

        private static IEnumerable<string> GetStem(int b)
        {
            var list = new ArrayList();

            if ((b & 0x4) == 0x4)
                list.Add(RegisterStem);
            if ((b & 0x2) == 0x2)
                list.Add(FullNumberStem);
            if ((b & 0x1) == 0x1)
                list.Add(ConstNumberStem);

            return (string[])list.ToArray();
        }

        private static string GetRoot(int b)
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
        #endregion 

        #region Parsing
        private static byte GetRegisterCode(string s)
        {
            return byte.Parse(s.ToLower().Replace("r", ""));
        }

        private static byte GetConstantNumberCode(string s)
        {
            if (s.Contains("#0x"))
                return (byte)(byte.Parse(s.Replace("#0x", ""), NumberStyles.HexNumber) & 0xF);

            if (s.Contains("#-"))
                return (byte)(BitHelper.Negate(int.Parse(s.Replace("#", ""), NumberStyles.Number)) & 0xF);

            return (byte)(byte.Parse(s.Replace("#", "")) & 0xF);
        }

        private static int GetFullNumberCode(string s)
        {
            if (s.Contains("=0x"))
                return int.Parse(s.Replace("=0x", ""), NumberStyles.HexNumber);

            if (s.Contains("=-"))
                return (int)BitHelper.Negate(int.Parse(s.Replace("=", ""), NumberStyles.Number));

            return int.Parse(s.Replace("=", ""));
        }

        private string CleanUp(string s)
        {
            var keyword = s;
            foreach(DictionaryEntry entry in KeywordLookup)
            {
                keyword = Regex.Replace(keyword, (string) entry.Key, (string) entry.Value, RegexOptions.IgnoreCase);
            }
            return Regex.Replace(keyword.Replace("\t", " "), @"[ ]{2,}", @" ").Trim().Replace(" ", ",").Replace("[", "").Replace("]", "").Replace(",,,", ",").Replace(",,", ",");
        }

        private static ulong ParseValue64(string s, int mode)
        {
            switch (mode)
            {
                case VpFile.Hexadecimal:
                    return ulong.Parse(s.Replace(" ", ""), NumberStyles.HexNumber);
                case VpFile.Decimal:
                    return ulong.Parse(s, NumberStyles.Number);
                default:
                    return 0;
            }
        }

        private uint[] ParseValue32(string s, int mode)
        {
            switch (mode)
            {
                case VpFile.Hexadecimal:
                    return new[] { uint.Parse(s.Replace(" ", ""), NumberStyles.HexNumber) };
                case VpFile.Decimal:
                    return new[] { uint.Parse(s, NumberStyles.Number) };
                case VpFile.Assembly:
                    return ConvertLine32(s);
                default:
                    return new uint[] { 0 };
            }
        }
        #endregion

        #region Dispose
        private static Exception CompilerException(string p)
        {
            throw new Exception(p);
        }

        public void Dispose()
        {
            _branchLookup.Clear();
            _branchRegistry.Clear();
            _machineLine = 0;
            _assemblyLine = 0;
        }
        #endregion
    }
}
