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
            var brancher = new Brancher {Nzcv = 4};
            Assert.IsTrue(brancher.Branch(Opcode.BEQ));
        }

        [TestMethod]
        public void TestBcs()
        {
            var brancher = new Brancher {Nzcv =  2};
            Assert.IsTrue(brancher.Branch(Opcode.BCS));
        }

        [TestMethod]
        public void TestBns()
        {
            var brancher = new Brancher { Nzcv = 8 };
            Assert.IsTrue(brancher.Branch(Opcode.BNS));
        }

        [TestMethod]
        public void TestBvs()
        {
            var brancher = new Brancher { Nzcv = 1 };
            Assert.IsTrue(brancher.Branch(Opcode.BVS));
        }

        [TestMethod]
        public void TestBhi()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(14, 12);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsTrue(brancher.Branch(Opcode.BHI));
        }

        private static Datapath SetupDatapath(UInt32 a, UInt32 b)
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

        [TestMethod]
        public void TestBge_WhenFalse()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);
            
            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsFalse(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsTrue(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBge_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsTrue(brancher.Branch(Opcode.BGE));
        }

        [TestMethod]
        public void TestBgt_WhenEqual()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(17, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsFalse(brancher.Branch(Opcode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenGreaterThan()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(18, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsTrue(brancher.Branch(Opcode.BGT));
        }

        [TestMethod]
        public void TestBgt_WhenLessThan()
        {
            var brancher = new Brancher();
            var datapath = SetupDatapath(14, 17);

            datapath.FunctionUnit(Opcode.CMP);

            brancher.Nzcv = datapath.GetNzcv();
            Assert.IsFalse(brancher.Branch(Opcode.BGT));
        }
    }
}
