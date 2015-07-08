using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VProcessor.Tests.Hardware
{
    using VProcessor.Hardware;

    [TestClass]
    public class TestDatapath
    {
        [TestMethod]
        public void TestRegisterFunctionUnit()
        {
            var datapath = new Datapath();

            const Byte regA = 3;
            const Byte regB = 5;

            const UInt32 i = 162;
            const UInt32 j = 94;

            datapath.SetRegister(regA, i);
            datapath.SetRegister(regB, j);
            datapath.SetChannel(0, regA);
            datapath.SetChannel(1, regB);
            datapath.FunctionUnit(Opcode.ADD, 2, 1);
            
            Assert.AreEqual(i, datapath.GetRegister(0));
            Assert.AreEqual(j, datapath.GetRegister(1));
            datapath.SetChannel(0, 2);
            var reg = datapath.GetRegister(0);
            const UInt32 outReg = i + j;
            Assert.AreEqual(outReg, reg);
            var nzcv = datapath.GetStatusRegister().Nzcv;
            Assert.AreEqual(0, nzcv);
        }

        [TestMethod]
        public void TestNegativeFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, 17);
            datapath.SetChannel(0, 0);
            datapath.SetChannel(1, 1);
            datapath.FunctionUnit(Opcode.SUB, 2, 1);
            datapath.GetStatusRegister().Mask(8);
            var nzcv = datapath.GetStatusRegister().Nzcv;
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
            datapath.FunctionUnit(Opcode.SUB, 2, 1);
            datapath.GetStatusRegister().Mask(4);
            var nzcv = datapath.GetStatusRegister().Nzcv;
            Assert.IsTrue(4 == nzcv);
        }

        [TestMethod]
        public void TestCarryFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, UInt32.MaxValue);
            datapath.SetRegister(0, UInt32.MaxValue);
            datapath.FunctionUnit(Opcode.ADD, 0, 1);
            datapath.GetStatusRegister().Mask(2);
            var nzcv = datapath.GetStatusRegister().Nzcv;
            var reg = datapath.GetRegister();
            Assert.IsTrue(2 == nzcv);
            Assert.IsTrue(UInt32.MaxValue << 1 == reg);
        }

        [TestMethod]
        public void TestOverflowFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, Int32.MaxValue);
            datapath.SetRegister(0, Int32.MaxValue);
            datapath.FunctionUnit(Opcode.ADD, 0, 1);
            datapath.GetStatusRegister().Mask(1);
            var nzcv = datapath.GetStatusRegister().Nzcv;
            Assert.IsTrue(1 == nzcv);
        }

        [TestMethod]
        public void TestRegister()
        {
            var datapath = new Datapath();
            const UInt32 data = 17;
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
    }
}
