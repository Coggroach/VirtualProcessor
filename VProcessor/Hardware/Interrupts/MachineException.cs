using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Interrupts
{
    public class MachineException : Exception
    {
        public MachineException() 
            : base() { }

        public MachineException(String s)
            : base(s) { }
    }
}
