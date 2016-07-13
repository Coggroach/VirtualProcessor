using VProcessor.Hardware.Components;

namespace VProcessor.Hardware.Interrupts
{
    public class Interrupt
    {
        public uint Address { get; set; }
        public DatapathMode Mode { get; set; }
        public bool Enable { get; set; }
        public bool Accepting { get; set; }
        
        public Interrupt()
        {
            Address = 0;
            Mode = DatapathMode.System;
            Enable = false;
            Accepting = false;
        }
    }
}
