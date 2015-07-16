using System;
using VProcessor.Hardware;
using VProcessor.Hardware.Components;

namespace VProcessor.Tests.Hardware
{
    public class TestHelper
    {
        public static Datapath CreateDatapath(UInt32 a, UInt32 b)
        {
            var datapath = new Datapath();

            var reg0 = (Byte)0;
            var reg1 = (Byte)1;
            var reg2 = (Byte)0;

            datapath.SetRegister(reg0, a);
            datapath.SetRegister(reg1, b);

            datapath.SetChannel(Datapath.ChannelA, reg0);
            datapath.SetChannel(Datapath.ChannelB, reg1);
            datapath.SetChannel(Datapath.ChannelD, reg2);
            return datapath;
        }

        public static UInt32 GetRegisterFromDatapath(UInt32 a, UInt32 b, Byte code)
        {
            var datapath = CreateDatapath(a, b);
            datapath.FunctionUnit(code, 1);
            return datapath.GetRegister();
        }
    }
}
