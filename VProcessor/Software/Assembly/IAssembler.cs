using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Software.Assembly
{
    using VProcessor.Hardware;

    interface IAssembler
    {
        Memory64 Compile64(SFile file, Int32 size);
        Memory32 Compile32(SFile file, Int32 size);
    }
}
