using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Interfacing
{
    abstract class Bus
    {
        protected IConnectable connect1;
        protected IConnectable connect2;

        public void Connect(IConnectable con1, IConnectable con2)
        {
            this.connect1 = con1;
            this.connect2 = con2;

            this.connect1.Connect(this);
            this.connect2.Connect(this);
        }        
    }
}
