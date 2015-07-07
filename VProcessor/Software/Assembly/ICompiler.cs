using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Software.Assembly
{
    using VProcessor.Hardware;

    interface ICompiler
    {
        Memory Compile(SFile file, Int32 size);
    }
}
