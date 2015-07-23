using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Components
{
    public class InterruptController
    {
        private Register irr;
        private Register isr;
        private Register imr;

        public InterruptController()
        {
            this.irr = new Register();
            this.isr = new Register();
            this.imr = new Register();
        }
    }
}
