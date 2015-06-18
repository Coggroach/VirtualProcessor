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

            const Byte a = 3;
            const Byte b = 5;

            datapath.SetRegister(a, 162);
            datapath.SetRegister(b, 94);
            datapath.SetChannel(0, a);
            datapath.SetChannel(1, b);
            datapath.FunctionUnit(AssemblyTable.ADD, 2);
            
            Assert.AreEqual(162, datapath.GetRegister(0));
            Assert.AreEqual(94, datapath.GetRegister(1));
            datapath.SetChannel(0, 2);
            var reg = datapath.GetRegister(0);
            Assert.AreEqual(256, reg);
            var nzcv = datapath.GetNzcv();
            Assert.AreEqual(0, nzcv);
        }

        [TestMethod]
        public void TestNegativeFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, 17);
            datapath.SetChannel(0, 0);
            datapath.SetChannel(1, 1);
            datapath.FunctionUnit(AssemblyTable.SUB, 2);
            var nzcv = (datapath.GetNzcv() & 0x08);
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
            datapath.FunctionUnit(AssemblyTable.SUB, 2);
            var nzcv = (datapath.GetNzcv() & 0x04);
            Assert.IsTrue(4 == nzcv);
        }

        [TestMethod]
        public void TestCarryFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, Int32.MinValue);
            datapath.SetRegister(0, Int32.MinValue);
            datapath.FunctionUnit(AssemblyTable.ADD, 0);
            var nzcv = (datapath.GetNzcv() & 2);
            Assert.IsTrue(2 == nzcv);
        }

        [TestMethod]
        public void TestOverflowFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, Int32.MaxValue);
            datapath.SetRegister(0, Int32.MaxValue);
            datapath.FunctionUnit(AssemblyTable.SUB, 0);
            var nzcv = (datapath.GetNzcv() & 0x01);
            Assert.IsTrue(1 == nzcv);
        }

        [TestMethod]
        public void TestRegister()
        {
            var datapath = new Datapath();
            datapath.SetRegister(0, 17);
            datapath.SetChannel(0, 0);
            Assert.AreEqual(17, datapath.GetRegister(0));
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
