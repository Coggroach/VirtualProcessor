using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public interface Memory<T>
    {
        void Reset();
        T GetMemory(Int32 index);
        T GetMemory(UInt32 index);
        void SetMemory(Int32 index, T value);
        Int32 GetLength();
    }
}
