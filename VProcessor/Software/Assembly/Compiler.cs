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
    public class Compiler : ICompiler
    {
        public Memory64 Compile64(SFile file, Int32 size)
        {
            var lines = file.GetString().Split(';');
            var memory = new Memory64(size);
            var mode = file.GetMode();

            for (var i = 0; i < memory.GetLength(); i++)
            {
                var input = (i < lines.Length) ? lines[i] : "";
                if (!String.IsNullOrWhiteSpace(input))
                    memory.SetMemory(i, ParseValue64(input, mode));
                else
                    memory.SetMemory(i, 0);
            }
            return memory;
        }

        public Memory32 Compile32(SFile file, Int32 size)
        {
            var lines = file.GetString().Split('\n');
            var memory = new Memory32(size);
            var mode = file.GetMode();

            for (var i = 0; i < memory.GetLength(); i++)
            {
                var input = (i < lines.Length) ? lines[i] : "";
                if (!String.IsNullOrWhiteSpace(input))
                {
                    UInt32[] array = ParseValue32(input, mode);
                    for(var j = 0; j < array.Length; j++, i++)
                        memory.SetMemory(i, array[j]);
                }                    
                else
                    memory.SetMemory(i, 0);
            }
            return memory;
        }

        private UInt64 ParseValue64(String s, Int32 mode)
        {
            switch(mode)
            {
                case SFile.Hexadecimal:
                    return UInt64.Parse(s.Replace(" ", ""), NumberStyles.HexNumber);
                case SFile.Decimal:
                    return UInt64.Parse(s, NumberStyles.Number);
                default:
                    return 0;
            }
        }

        private UInt32[] ParseValue32(String s, Int32 mode)
        {
            switch (mode)
            {
                case SFile.Hexadecimal:
                    return new UInt32[] {UInt32.Parse(s.Replace(" ", ""), NumberStyles.HexNumber)};
                case SFile.Decimal:
                    return new UInt32[] {UInt32.Parse(s, NumberStyles.Number)};
                case SFile.Assembly:
                    return CompilerHelper.Convert(s);
                default:
                    return new UInt32[] {0};
            }
        }
    }
}
