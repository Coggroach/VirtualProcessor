using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessor.Hardware.Interfacing
{
    interface IConnectable
    {
        void Send(IPacket packet);
        IPacket Receive();
        void Connect(Bus bus);
    }
}
