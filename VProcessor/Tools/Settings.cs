using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Tools
{
    public class Settings
    {
        public const Int32 ControlMemorySize = 100;
        public const Int32 FlashMemorySize = 32;
        public const String ControlMemoryLocation = "Software\\ControlMemory.vpo";
        public const String FlashMemoryLocation = "Assembly\\UserMemory.vpo";
        public const String UserSettingsLocation = "Gui\\UserSettings.txt";
        public const Int32 RandomAccessMemorySize = 128;
        public const Int32 RandomAccessMemorySpeed = 4;
        public const Int32 RandomAccessMemorySpread = 2;
    }
}
