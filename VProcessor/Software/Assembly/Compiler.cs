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
                try
                {
                    memory.SetMemory(i, ParseValue64(lines[i], mode));
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is IndexOutOfRangeException )
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
