using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VProcessor.Hardware.Memory;
using VProcessor.Common;
using VProcessor.Tools;

namespace VProcessor.Software.Assembly
{
    public interface IAssembler
    {
        Memory64 Compile64(VPFile file, Int32 size);
        Memory32 Compile32(VPFile file, Int32 size);
    }

    public class Assembler : IAssembler, IDisposable
    {
        #region Regexs
        private const String GeneralRoot = @"^[\w][\w]+[,][rR][\d]+[,][rR][\d]+[,]";
        private const String DataRoot = @"^[\w][\w]+[,][rR][\d]+[,]";
        private const String BranchRoot = @"^[\w]+[,][\d]+";
        private const String ReferenceRoot = @"^[\d]+$";
        private const String RegisterStem = @"[rR][\d]+";
        private const String ConstNumberStem = @"[#][-\d]+";
        private const String FullNumberStem = @"[=][-\d]+";
        private const String AddressStem = @"[\[][\w,#]+[\]]";
        #endregion

        #region Attributes
        private Hashtable BranchLookup;
        private Hashtable BranchRegistry;
        private Int32 AssemblyLine;
        private Int32 MachineLine;
        private Hashtable KeywordLookup;
        private List<UInt32> CurrentLines;
        #endregion

        public Assembler()
        {
            this.BranchLookup = new Hashtable();
            this.BranchRegistry = new Hashtable();
            this.AssemblyLine = 0;
            this.MachineLine = 0;
            this.KeywordLookup = new Hashtable();
            this.InitKeywordLookup();
            this.CurrentLines = new List<UInt32>();
        }

        public void InitKeywordLookup()
        {
            this.KeywordLookup.Add("sp", "r14");
            this.KeywordLookup.Add("lr", "r13");            
            this.KeywordLookup.Add("rt", "r15");
            this.KeywordLookup.Add("System", "#0");
            this.KeywordLookup.Add("Interupt", "#1");
            this.KeywordLookup.Add("Previlaged", "#2");
            this.KeywordLookup.Add("UnPrevilaged", "#3");
        }

        #region Exposed Compile Methods
        public Memory64 Compile64(VPFile file, Int32 size)
        {
            var lines = file.GetString().Split(VPFile.Delimiter);
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

        public Memory32 Compile32(VPFile file, Int32 size)
        {
            var lines = file.GetString().Split(VPFile.Delimiter);
            var memory = new Memory32(size);
            var mode = file.GetMode();
            UInt32[] array = null;
            for (this.MachineLine = 0, this.AssemblyLine = 0; 
                this.MachineLine < memory.GetLength(); 
                this.MachineLine++, this.AssemblyLine++)
            {
                try
                {
                    array = ParseValue32(lines[this.AssemblyLine], mode);
                    if (array == null)
                    {
                        this.MachineLine--;
                        continue;
                    }
                                         
                    for (var j = 0; j < array.Length; j++)
                        memory.SetMemory(this.MachineLine + j, array[j]);
                    this.MachineLine += array.Length - 1;
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is IndexOutOfRangeException)
                    {
                        memory.SetMemory(this.MachineLine, 0);
                        continue;
                    }
                    throw;
                }
            }

            this.LinkBranches();
            this.PostLinkBranches(memory);
            this.Dispose();

            return memory;
        }
        #endregion 

        #region CompoundLines
        private UInt32[] PCCompoundLine(String[] parts, String mode)
        {            
            var upperCode = parts[0].ToUpper();

            var movLine = "MOV ";

            movLine += mode == "STPC" ? "rt" : parts[1];
            movLine += ", ";
            movLine += mode == "LDPC" ? "rt" : parts[1];

            List<UInt32> lines = new List<UInt32>();

            var code = (UInt32) Opcode.GetCodeAddress(upperCode) << 16;

            if (mode == "LDPC")
                lines.Add(code);
            lines.AddRange(this.ConvertLine32(movLine));
            if (mode == "STPC")
                lines.Add(code);

            return lines.ToArray();
        }

        private UInt32[] MemoryCompoundLine(String s, String cmd, String cmd2)
        {
            List<UInt32> lines = new List<UInt32>();
            List<String> assemblies = new List<String>();

            var mode = s.Contains('!');
            var match = Regex.Matches(this.CleanUp(s), RegisterStem);

            if (mode && cmd == "STR")
                assemblies.Add("MOV r15, #0");
            var incrementReg = (mode) ? "r14" : "r15";
            foreach (Capture capture in match)
            {
                var value = (String)capture.Value;

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
            foreach (String assemblyLine in assemblies)
            {
                var lineConversion = this.ConvertLine32(assemblyLine);
                if(lineConversion != null)
                    lines.AddRange(lineConversion);
            }
            return lines.ToArray();
        }

        private UInt32[] SetModeLine(String[] parts)
        {
            var array = new UInt32[1];
            var addressOffset = GetConstantNumberCode(parts[1]);
            var address = (UInt32)Opcode.GetCodeAddress(parts[0].ToUpper()) + addressOffset;

            array[0] |= address << 16;
            return array;
        }

        private UInt32[] SubroutineCompoundLine(String[] parts, String type)
        {
            if (parts.Length <= 0)
                return null;

            List<UInt32> list = new List<UInt32>();

            if(parts[0] == type)
            {
                switch(type)
                {
                    case "BX":
                        list.AddRange(this.ConvertLine32("LDPC lr"));
                        list.AddRange(this.ConvertLine32("ADD lr, lr, #4"));
                        list.AddRange(this.ConvertLine32("B " + parts[1], 3));
                        break;
                    case "^":
                        list.AddRange(this.ConvertLine32("STPC lr"));
                        break;
                }
            }
            return list.ToArray();
        }
        #endregion

        #region ConvertLine32
        public UInt32[] ConvertLine32(String s, Int32 machineBranchOffset = 0)
        {
            var line = CleanUp(s);
            var table = CreatePropertyTable(line);

            if(table == null)
            {                
                this.RegisterBranch(line);
                return null;
            }

            var stem = (Int32)table["Stem"];
            var parts = (String[])table["Code"];
            var type = (Int32)table["Type"];
            var lastElement = parts.Length - 1;
            var index = 0;
            var upperCode = parts[0].ToUpper();

            if((type & 8) == 8)
            {
                switch(upperCode)
                {
                    case "LDM":
                        return this.MemoryCompoundLine(s, "LDRST", "SUB");
                    case "STM":
                        return this.MemoryCompoundLine(s, "STR", "ADD");
                    case "LDPC":
                        return this.PCCompoundLine(parts, "LDPC");
                    case "STPC":
                        return this.PCCompoundLine(parts, "STPC");
                    case "BX":
                        return this.SubroutineCompoundLine(parts, "BX");
                    case "^":
                        return this.SubroutineCompoundLine(parts, "^");
                    case "MOD":
                        return this.SetModeLine(parts);
                }
            }

            var array = new UInt32[
                ((stem & 2) == 2 || (type & 4) == 4 ) ? (Int32)table["Contains"] + 1 : 1];

            if (Regex.Match(parts[lastElement], ConstNumberStem).Success && (type & 4) == 4)
            {
                array[index] = this.ConvertLine32("MOV r15," + parts[lastElement])[0];
                parts[lastElement] = "r15";
                index++;
            }

            if((type & 5) == 5 && parts.Length < 4)
            {
                array[index] = this.ConvertLine32("MOV r15,#0")[0];
                var newArray = array.ToList<UInt32>();
                newArray.Add(0);
                array = newArray.ToArray();
                var newParts = parts.ToList<String>();
                newParts.Add("r15");                
                parts = newParts.ToArray();
                lastElement++;
                index++;                
            }
            
            if (type == 3)
            {
                this.LinkBranch(parts[1], machineBranchOffset);
                array[index] |= 1;
            }
                
            
            var address = Opcode.GetCodeAddress(upperCode);

            for (var i = 1; i <= 3 - (type & 3); i++)
                array[index] |= (UInt32)(GetRegisterCode(parts[i]) << ((3 - i) * 4));

            
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
                array[index ^ 1] |= (UInt32)GetFullNumberCode(parts[lastElement]);

            array[index] |= (UInt32)(address << 16);

            return array;
        }
        #endregion

        #region Branch Registry
        private void RegisterBranch(String branchName, Int32 offset = 0)
        {
            if(!this.BranchRegistry.ContainsKey(branchName))
            {
                var map = new Hashtable();

                map.Add("MachineLine", this.MachineLine + offset);
                map.Add("AssemblyLine", this.AssemblyLine);

                this.BranchRegistry.Add(branchName, map);
            }
        }

        private void LinkBranch(String branchName, Int32 offset = 0)
        {
            var map = new Hashtable();

            map.Add("LinkMachineLine", this.MachineLine + offset);
            map.Add("LinkAssemblyLine", this.AssemblyLine);
            
            map.Add("BranchKey", branchName);

            this.BranchLookup.Add(branchName, map);
        }

        private void LinkBranches()
        {
            foreach(DictionaryEntry dictionary in this.BranchLookup)
            {
                var lookup = (Hashtable)dictionary.Value;
                lookup.Add("BranchRegistryKey", this.BranchRegistry.ContainsKey(lookup["BranchKey"]));                    
            }
        }

        private void PostLinkBranches(Memory32 memory)
        {
            foreach (DictionaryEntry dictionary in this.BranchLookup)
            {
                var lookup = (Hashtable) dictionary.Value;
                if ((Boolean)(lookup["BranchRegistryKey"]))
                {
                    var regLine = (Int32) ((Hashtable) this.BranchRegistry[lookup["BranchKey"]])["MachineLine"];
                    var linkLine = (Int32) lookup["LinkMachineLine"];

                    var difference = BitHelper.Subtract(regLine, linkLine);
                    var currentValue = memory.GetMemory(linkLine);

                    currentValue &= 0xFFFF0000;
                    currentValue |= difference;

                    memory.SetMemory(linkLine, currentValue);
                }                
            }
        }
        #endregion

        #region PropertyTable
        private static Hashtable CreatePropertyTable(String line)
        {
            var table = new Hashtable();
            var parts = line.Split(',');
            var upperCode = parts[0].ToUpper();

            if(!Opcode.IsValidCode(parts[0].ToUpper()))
                return null;

            var type = Opcode.GetCodeType(upperCode);

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
        #endregion 

        #region Parsing 
        private static Byte GetRegisterCode(String s)
        {
            return Byte.Parse(s.ToLower().Replace("r", ""));
        }

        private static Byte GetConstantNumberCode(String s)
        {
            if (s.Contains("#0x"))
                return (Byte)(Byte.Parse(s.Replace("#0x", ""), NumberStyles.HexNumber) & 0xF);

            if (s.Contains("#-"))
                return (Byte)(BitHelper.Negate(Int32.Parse(s.Replace("#", ""), NumberStyles.Number)) & 0xF);

            return (Byte)(Byte.Parse(s.Replace("#", "")) & 0xF);
        }

        private static Int32 GetFullNumberCode(String s)
        {
            if (s.Contains("=0x"))
                return Int32.Parse(s.Replace("=0x", ""), NumberStyles.HexNumber);

            if (s.Contains("=-"))
                return (Int32)BitHelper.Negate(Int32.Parse(s.Replace("=", ""), NumberStyles.Number));

            return Int32.Parse(s.Replace("=", ""));
        }

        private String CleanUp(String s)
        {
            var keyword = s;
            foreach(DictionaryEntry entry in this.KeywordLookup)
            {
                keyword = Regex.Replace(keyword, (String) entry.Key, (String) entry.Value, RegexOptions.IgnoreCase);
            }
            return Regex.Replace(keyword.Replace("\t", " "), @"[ ]{2,}", @" ").Trim().Replace(" ", ",").Replace("[", "").Replace("]", "").Replace(",,,", ",").Replace(",,", ",");
        }

        private static UInt64 ParseValue64(String s, Int32 mode)
        {
            switch (mode)
            {
                case VPFile.Hexadecimal:
                    return UInt64.Parse(s.Replace(" ", ""), NumberStyles.HexNumber);
                case VPFile.Decimal:
                    return UInt64.Parse(s, NumberStyles.Number);
                default:
                    return 0;
            }
        }

        private UInt32[] ParseValue32(String s, Int32 mode)
        {
            switch (mode)
            {
                case VPFile.Hexadecimal:
                    return new UInt32[] { UInt32.Parse(s.Replace(" ", ""), NumberStyles.HexNumber) };
                case VPFile.Decimal:
                    return new UInt32[] { UInt32.Parse(s, NumberStyles.Number) };
                case VPFile.Assembly:
                    return this.ConvertLine32(s);
                default:
                    return new UInt32[] { 0 };
            }
        }
        #endregion

        #region Dispose
        private static Exception CompilerException(String p)
        {
            throw new Exception(p);
        }

        public void Dispose()
        {
            this.BranchLookup.Clear();
            this.BranchRegistry.Clear();
            this.MachineLine = 0;
            this.AssemblyLine = 0;
        }
        #endregion
    }
}
