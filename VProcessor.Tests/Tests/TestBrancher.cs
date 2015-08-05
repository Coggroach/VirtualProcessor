using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;
using VProcessor.Hardware.Components;
using VProcessor.Common;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestBrancher
    {
        [TestMethod]
        public void TestBeq()
        {
            var brancher = new Brancher();
            brancher.SetNzcv(new Register(4));
            Assert.IsTrue(brancher.Branch(BranchCode.BEQ));
        }

        [TestMethod]
        public void TestBcs()
        {
            var brancher = new Brancher();
            brancher.SetNzcv(new Register(2));
            Assert.IsTrue(brancher.Branch(BranchCode.BCS));
        }

        [TestMethod]
        public void TestBns()
        {
            var brancher = new Brancher();
            brancher.SetNzcv(new Register(8));
            Assert.IsTrue(brancher.Branch(BranchCode.BNS));
        }

        [TestMethod]
        public void TestBvs()
        {
            var brancher = new Brancher();
            brancher.SetNzcv(new Register(1));
            Assert.IsTrue(brancher.Branch(BranchCode.BVS));
        }

        [TestMethod]
        public void TestBhi()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 12);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsTrue(brancher.Branch(BranchCode.BHI));
        }

        [TestMethod]
        public void TestBge_WhenFalse()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsFalse(brancher.Branch(BranchCode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsTrue(brancher.Branch(BranchCode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsTrue(brancher.Branch(BranchCode.BGE));
        }

        [TestMethod]
        public void TestBgt_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsFalse(brancher.Branch(BranchCode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsTrue(brancher.Branch(BranchCode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenLessThan()
        {
            var brancher = new Brancher();
            var datapath = TestHelper.CreateDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.SetNzcv(datapath.GetStatusRegister());
            Assert.IsFalse(brancher.Branch(BranchCode.BGT));
        }
    }
}
