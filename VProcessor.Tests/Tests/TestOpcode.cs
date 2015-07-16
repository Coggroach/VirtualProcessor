using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;
using VProcessor.Tools;
using System.Collections;
using VProcessor.Common;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestOpcode
    {
        [TestMethod]
        public void TestCodeTable()
        {
            Assert.IsTrue(Opcode.GetCodeIndexer("ADD") == Opcode.ADD);
            Assert.IsTrue(Opcode.GetCodeType("ADD") == 0x15);
        }

        [TestMethod]
        public void TestAdd()
        {
            var reg = TestHelper.GetRegisterFromDatapath(12, 14, Opcode.ADD);
            Assert.IsTrue(12 + 14 == reg);
        }

        [TestMethod]
        public void TestSub()
        {
            const UInt32 a = 12;
            const UInt32 b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.SUB);
            Assert.IsTrue(a + ~b + 1 == reg);
        }

        [TestMethod]
        public void TestMul()
        {
            const UInt32 a = 12;
            const UInt32 b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.MUL);
            Assert.IsTrue(a * b == reg);
        }

        [TestMethod]
        public void TestLsr()
        {
            const UInt32 a = 12;
            const UInt32 b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.LSR);
            Assert.IsTrue(b >> 1 == reg);
        }

        [TestMethod]
        public void TestLsl()
        {
            const UInt32 a = 12;
            const UInt32 b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.LSL);
            Assert.IsTrue(b << 1 == reg);
        }

        [TestMethod]
        public void TestRor()
        {
            const UInt32 a = 12;
            const UInt32 b = 0xF;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.ROR);
            Assert.IsTrue(0x80000007 == reg);
        }

        [TestMethod]
        public void TestRol()
        {
            const UInt32 a = 12;
            const UInt32 b = 0x80000001;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.ROL);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestAnd()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.AND);
            Assert.IsTrue(12 == reg);
        }

        [TestMethod]
        public void TestOrr()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.ORR);
            Assert.IsTrue(15 == reg);
        }

        [TestMethod]
        public void TestEor()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.EOR);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestBic()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.BIC);
            Assert.IsTrue(0 == reg);
        }

        [TestMethod]
        public void TestAdc()
        {
            const UInt32 a = 0x80000000;
            const UInt32 b = 0x80000030;

            var datapath = TestHelper.CreateDatapath(a, b);
            datapath.FunctionUnit(Opcode.ADD, 1);
            datapath.FunctionUnit(Opcode.ADC, 1);

            var reg = datapath.GetRegister();

            Assert.IsTrue(0x80000061 == reg);
        }

        [TestMethod]
        public void TestRsb()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.RSB);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestRsc()
        {
            const UInt32 a = 12;
            const UInt32 b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.RSC);
            Assert.IsTrue(2 == reg);
        }

        [TestMethod]
        public void TestOpcodeAddressMapping()
        {
            var control = new VPFile(Settings.ControlMemoryLocation);
            var assembler = new Assembler();
            var memoryChunk = assembler.Compile64(control, Settings.ControlMemorySize);

            var opcodes = Opcode.GetCodeTable();

            foreach(String code in opcodes.Keys)
            {
                var address = Opcode.GetCodeAddress(code);
                var value = Opcode.GetCodeIndexer(code);
                var memory = memoryChunk.GetMemory(address);

                memory >>= 16;
                memory &= 0xFFFFF;

                var compare = (Int32) memory == value;
                if (!compare) Logger.Instance().Log("Code: " + code + " Address: " + address + " Internal: " + value + "External: " + memory);
                Assert.IsTrue(compare);
            }
        }
    }
}
