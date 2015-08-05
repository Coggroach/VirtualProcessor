using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VProcessor.Hardware.Components;

namespace VProcessor.Hardware.Interrupts
{
    public class Interrupt
    {
        public UInt32 Address { get; set; }
        public DatapathMode Mode { get; set; }
        public Boolean Enable { get; set; }
        public Boolean Accepting { get; set; }
        
        public Interrupt()
        {
            this.Address = 0;
            this.Mode = DatapathMode.System;
            this.Enable = false;
            this.Accepting = false;
        }
    }
}
