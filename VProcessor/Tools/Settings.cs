using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Tools
{
    public class Settings
    {
        public const Int32 ControlMemorySize = 64;
        public const Int32 FlashMemorySize = 32;
        public const String ControlMemoryLocation = "Software\\ControlMemory.txt";
        public const String FlashMemoryLocation = "Software\\UserMemory.txt";
    }
}
