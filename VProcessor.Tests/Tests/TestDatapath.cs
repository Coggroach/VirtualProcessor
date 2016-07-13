using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Common;
using VProcessor.Hardware.Components;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestDatapath
    {
        [TestMethod]
        public void TestRegisterFunctionUnit()
        {
            var datapath = new Datapath();

            const byte regA = 3;
            const byte regB = 5;

            const uint i = 162;
            const uint j = 94;

            datapath.SetRegister(regA, i);
            datapath.SetRegister(regB, j);
            datapath.SetChannel(0, regA);
            datapath.SetChannel(1, regB);
            datapath.SetChannel(Datapath.ChannelD, 2);
            datapath.FunctionUnit(Opcode.Add, 1);
            
            Assert.AreEqual(i, datapath.GetRegister(0));
            Assert.AreEqual(j, datapath.GetRegister(1));
            datapath.SetChannel(0, 2);
            var reg = datapath.GetRegister(0);
            const uint outReg = i + j;
            Assert.AreEqual(outReg, reg);
            var nzcv = datapath.GetStatusRegister().Value;
            Assert.AreEqual((uint) 0, nzcv);
        }

        [TestMethod]
        public void TestNegativeFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, 17);
            datapath.SetChannel(0, 0);
            datapath.SetChannel(1, 1);
            datapath.SetChannel(Datapath.ChannelD, 2);
            datapath.FunctionUnit(Opcode.Sub, 1);
            datapath.GetStatusRegister().Mask(8);
            var nzcv = datapath.GetStatusRegister().Value;
            Assert.IsTrue(8 == nzcv);
        }

        [TestMethod]
        public void TestZeroFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, 17);
            datapath.SetRegister(0, 17);
            datapath.SetChannel(0, 0);
            datapath.SetChannel(1, 1);
            datapath.SetChannel(Datapath.ChannelD, 2);
            datapath.FunctionUnit(Opcode.Sub, 1);
            datapath.GetStatusRegister().Mask(4);
            var nzcv = datapath.GetStatusRegister().Value;
            Assert.IsTrue(4 == nzcv);
        }

        [TestMethod]
        public void TestCarryFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, uint.MaxValue);
            datapath.SetRegister(0, uint.MaxValue);
            datapath.SetChannel(Datapath.ChannelD, 0);
            datapath.FunctionUnit(Opcode.Add, 1);
            datapath.GetStatusRegister().Mask(2);
            var nzcv = datapath.GetStatusRegister().Value;
            var reg = datapath.GetRegister();
            Assert.IsTrue(2 == nzcv);
            Assert.IsTrue(uint.MaxValue << 1 == reg);
        }

        [TestMethod]
        public void TestOverflowFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, int.MaxValue);
            datapath.SetRegister(0, int.MaxValue);
            datapath.SetChannel(Datapath.ChannelD, 0);
            datapath.FunctionUnit(Opcode.Add, 1);
            datapath.GetStatusRegister().Mask(1);
            var nzcv = datapath.GetStatusRegister().Value;
            Assert.IsTrue(1 == nzcv);
        }

        [TestMethod]
        public void TestRegister()
        {
            var datapath = new Datapath();
            const uint data = 17;
            datapath.SetRegister(0, data);
            datapath.SetChannel(0, 0);
            Assert.AreEqual(data, datapath.GetRegister(0));
        }

        [TestMethod]
        public void TestChannel()
        {
            var datapath = new Datapath();
            datapath.SetChannel(0, 1);
            Assert.AreEqual(1, datapath.GetChannel(0));
        }

        [TestMethod]
        public void TestMode()
        {
            var datapath = new Datapath();
            datapath.SetMode(DatapathMode.Interupt);
            Assert.IsTrue(datapath.GetMode() == DatapathMode.Interupt);
            datapath.SetMode(DatapathMode.Previlaged);
            Assert.IsTrue(datapath.GetMode() == DatapathMode.Previlaged);
            datapath.SetMode(DatapathMode.UnPrevilaged);
            Assert.IsTrue(datapath.GetMode() == DatapathMode.UnPrevilaged);
            datapath.SetMode(DatapathMode.System);
            Assert.IsFalse(datapath.GetMode() == DatapathMode.System);
        }
    }
}
