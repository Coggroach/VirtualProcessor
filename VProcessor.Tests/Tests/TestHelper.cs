using System;
using VProcessor.Common;
using VProcessor.Hardware.Components;

namespace VProcessor.Tests.Hardware
{
    public class TestHelper
    {
        public static Datapath CreateDatapath(uint a, uint b)
        {
            var datapath = new Datapath();

            var reg0 = (byte)0;
            var reg1 = (byte)1;
            var reg2 = (byte)0;

            datapath.SetRegister(reg0, a);
            datapath.SetRegister(reg1, b);

            datapath.SetChannel(Datapath.ChannelA, reg0);
            datapath.SetChannel(Datapath.ChannelB, reg1);
            datapath.SetChannel(Datapath.ChannelD, reg2);
            return datapath;
        }

        public static uint GetRegisterFromDatapath(uint a, uint b, Opcode code)
        {
            var datapath = CreateDatapath(a, b);
            datapath.FunctionUnit((byte)code, 1);
            return datapath.GetRegister();
        }
    }
}
