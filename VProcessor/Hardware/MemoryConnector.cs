using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware
{
    public class MemoryConnector
    {
        public const Byte Idle = 0;
        public const Byte Store = 1;
        public const Byte Fetch = 2;
        public const Byte Received = 3;

        public Int32 Address { get; set; }
        public UInt32 Value { get; set; }
        public Byte Command { get; set; }

        public MemoryConnector(Int32 a, UInt32 v, Byte c)
        {
            this.Set(a, v, c);
        }

        public MemoryConnector()
        {
            this.Flush();
        }

        public void Flush()
        {
            this.Set(0, 0, 0);
        }

        public void Set(Int32 a, UInt32 v, Byte c)
        {
            this.Address = a;
            this.Value = v;
            this.Command = c;
        }    
    }
}
