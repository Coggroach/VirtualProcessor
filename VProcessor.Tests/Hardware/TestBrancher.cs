using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestBrancher
    {
        [TestMethod]
        public void TestBeq()
        {
            var brancher = new Brancher();
            brancher.Nzcv.Nzcv = 4;
            Assert.IsTrue(brancher.Branch(Opcode.BEQ));
        }

        [TestMethod]
        public void TestBcs()
        {
            var brancher = new Brancher();
            brancher.Nzcv.Nzcv = 2;
            Assert.IsTrue(brancher.Branch(Opcode.BCS));
        }

        [TestMethod]
        public void TestBns()
        {
            var brancher = new Brancher();
            brancher.Nzcv.Nzcv = 8;
            Assert.IsTrue(brancher.Branch(Opcode.BNS));
        }

        [TestMethod]
        public void TestBvs()
        {
            var brancher = new Brancher();
            brancher.Nzcv.Nzcv = 1;
            Assert.IsTrue(brancher.Branch(Opcode.BVS));
        }

        [TestMethod]
        public void TestBhi()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 12);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsTrue(brancher.Branch(Opcode.BHI));
        }

        [TestMethod]
        public void TestBge_WhenFalse()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsFalse(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsTrue(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsTrue(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBgt_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsFalse(brancher.Branch(Opcode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsTrue(brancher.Branch(Opcode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenLessThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetStatusRegister();
            Assert.IsFalse(brancher.Branch(Opcode.BGT));
        }
    }
}
