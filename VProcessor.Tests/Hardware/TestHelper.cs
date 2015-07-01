using System;
using VProcessor.Hardware;

namespace VProcessor.Tests.Hardware
{
    public class TestHelper
    {
        public static Datapath CreateDatapath(UInt32 a, UInt32 b)
        {
            var datapath = new Datapath();

            const Byte reg0 = 0;
            const Byte reg1 = 1;

            datapath.SetRegister(reg0, a);
            datapath.SetRegister(reg1, b);

            datapath.SetChannel(0, reg0);
            datapath.SetChannel(1, reg1);
            return datapath;
        }

        public static UInt32 GetRegisterFromDatapath(UInt32 a, UInt32 b, Byte code)
        {
            var datapath = CreateDatapath(a, b);
            datapath.FunctionUnit(code, 0, 1);
            return datapath.GetRegister();
        }
    }
}
