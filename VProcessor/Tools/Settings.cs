namespace VProcessor.Tools
{
    public class VpConsts
    {
        public const int ControlMemorySize = 100;
        public const int FlashMemorySize = 32;
        public const string ControlMemoryLocation = "Software\\ControlMemory.vpo";
        public const string FlashMemoryLocation = "Assembly\\UserMemory.vpo";
        public const string UserSettingsLocation = "Gui\\UserSettings.txt";
        public const int RandomAccessMemorySize = 128;
        public const int RandomAccessMemorySpeed = 4;
        public const int RandomAccessMemorySpread = 2;
        public const int LedBoardCount = 4;
        public const int ClockSpeed = 1000;
        public const long ClockTime = 1 / ClockSpeed;
        public const int VectoredInterruptControllerAddress = RandomAccessMemorySize;
        public const int VectoredInterruptControllerSize = 32;
    }
}
