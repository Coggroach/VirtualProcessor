using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Common;
using VProcessor.Software;
using VProcessor.Tools;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestOpcode
    {
        [TestMethod]
        public void TestCodeTable()
        {
            Assert.IsTrue(OpcodeRegistry.Instance.GetCode("ADD") == (int)Opcode.Add);
            Assert.IsTrue(OpcodeRegistry.Instance.GetCodeType("ADD") == 0x15);
        }

        [TestMethod]
        public void TestAdd()
        {
            var reg = TestHelper.GetRegisterFromDatapath(12, 14, Opcode.Add);
            Assert.IsTrue(12 + 14 == reg);
        }

        [TestMethod]
        public void TestSub()
        {
            const uint a = 12;
            const uint b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Sub);
            Assert.IsTrue(a + ~b + 1 == reg);
        }

        [TestMethod]
        public void TestMul()
        {
            const uint a = 12;
            const uint b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Mul);
            Assert.IsTrue(a * b == reg);
        }

        [TestMethod]
        public void TestLsr()
        {
            const uint a = 12;
            const uint b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Lsr);
            Assert.IsTrue(b >> 1 == reg);
        }

        [TestMethod]
        public void TestLsl()
        {
            const uint a = 12;
            const uint b = 14;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Lsl);
            Assert.IsTrue(b << 1 == reg);
        }

        [TestMethod]
        public void TestRor()
        {
            const uint a = 12;
            const uint b = 0xF;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Ror);
            Assert.IsTrue(0x80000007 == reg);
        }

        [TestMethod]
        public void TestRol()
        {
            const uint a = 12;
            const uint b = 0x80000001;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Rol);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestAnd()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.And);
            Assert.IsTrue(12 == reg);
        }

        [TestMethod]
        public void TestOrr()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Orr);
            Assert.IsTrue(15 == reg);
        }

        [TestMethod]
        public void TestEor()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Eor);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestBic()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Bic);
            Assert.IsTrue(0 == reg);
        }

        [TestMethod]
        public void TestAdc()
        {
            const uint a = 0x80000000;
            const uint b = 0x80000030;

            var datapath = TestHelper.CreateDatapath(a, b);
            datapath.FunctionUnit((byte)Opcode.Add, 1);
            datapath.FunctionUnit((byte)Opcode.Adc, 1);

            var reg = datapath.GetRegister();

            Assert.IsTrue(0x80000061 == reg);
        }

        [TestMethod]
        public void TestRsb()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Rsb);
            Assert.IsTrue(3 == reg);
        }

        [TestMethod]
        public void TestRsc()
        {
            const uint a = 12;
            const uint b = 15;
            var reg = TestHelper.GetRegisterFromDatapath(a, b, Opcode.Rsc);
            Assert.IsTrue(2 == reg);
        }

        [TestMethod]
        public void TestOpcodeAddressMapping()
        {
            var control = new VpFile(VpConsts.ControlMemoryLocation);
            var assembler = new Assembler();
            var memoryChunk = assembler.Compile64(control, VpConsts.ControlMemorySize);

            var opcodes = OpcodeRegistry.Instance.GetCodeTable();

            foreach(string code in opcodes.Keys)
            {
                var address = OpcodeRegistry.Instance.GetCodeAddress(code);
                var value = OpcodeRegistry.Instance.GetCode(code);
                var memory = memoryChunk.GetMemory(address);
                var type = OpcodeRegistry.Instance.GetCodeType(code);

                if ((type & 0xF) == 0)
                    continue;

                memory >>= 16;
                memory &= 0xFFFFF;

                var compare = (int) memory == value;
                if (!compare) Logger.Instance().Log("Code: " + code + " Address: " + address + " Internal: " + value + "External: " + memory);
                Assert.IsTrue(compare);
            }
        }
    }
}
