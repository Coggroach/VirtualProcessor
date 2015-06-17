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

            datapath.SetRegister(a, 17);
            datapath.SetRegister(b, 12);
            datapath.SetChannel(0, a);
            datapath.SetChannel(1, b);
            datapath.FunctionUnit(3, 2);

            //r0 = 17, r1 = 12, r2 = 17 + 12 + 1 = 30
            //NZCV = 0001
            
            Assert.AreEqual(1, datapath.GetNzcv());
            Assert.AreEqual(17, datapath.GetRegister(0));
            Assert.AreEqual(12, datapath.GetRegister(1));
            datapath.SetChannel(0, 2);
            Assert.AreEqual(30, datapath.GetRegister(0));
            Assert.AreEqual(1, datapath.GetNzcv());
        }

        [TestMethod]
        public void TestNegativeFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, 17);
            datapath.SetChannel(0, 0);
            datapath.SetChannel(1, 1);
            datapath.FunctionUnit(AssemblyTable.SUB, 2);
            Assert.IsTrue(8 <= datapath.GetNzcv());
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
            Assert.IsTrue(4 <= datapath.GetNzcv());
        }

        [TestMethod]
        public void TestCarryFlag()
        {
            var datapath = new Datapath();
            datapath.SetRegister(1, Int32.MaxValue);
            datapath.SetRegister(0, Int32.MaxValue);
            datapath.FunctionUnit(AssemblyTable.SUB, 0);
            Console.WriteLine(datapath.GetRegister(0));
            Assert.IsTrue(3 <= datapath.GetNzcv());
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
