using System;

namespace VProcessor.Hardware.Interrupts
{
    public class MachineException : Exception
    {
        public MachineException()
        { }

        public MachineException(string s)
            : base(s) { }
    }
}
