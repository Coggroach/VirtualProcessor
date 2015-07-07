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
        public Memory Compile(SFile file, Int32 size)
        {
            var lines = file.GetString().Split('\n');
            var memory = new Memory(size);
            var mode = file.GetMode();

            for (var i = 0; i < memory.GetLength(); i++)
            {
                var input = (i < lines.Length) ? lines[i] : "";
                if (!String.IsNullOrWhiteSpace(input))
                    memory.SetMemory(i, ParseValue(input, mode));
                else
                    memory.SetMemory(i, 0);
            }
            return memory;
        }

        private UInt64 ParseValue(String s, Int32 mode)
        {
            switch(mode)
            {
                case SFile.Hexadecimal:
                    return UInt64.Parse(s.Replace(" ", ""), NumberStyles.HexNumber);
                case SFile.Decimal:
                    return UInt64.Parse(s, NumberStyles.Number);
                case SFile.Assembly:
                    return 0; //TODO
                default:
                    return 0;
            }
        }
    }
}
