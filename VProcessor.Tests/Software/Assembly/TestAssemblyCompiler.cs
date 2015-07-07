using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;

namespace VProcessor.Tests.Software.Assembly
{
    [TestClass]
    public class TestAssemblyCompiler
    {
        [TestMethod]
        public void Test_ValidInput_Type1()
        {
            const String input = "ADD r0, r1, r2";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraWhitespace()
        {
            const String input = "  ADD  r0, r1,  r2  ";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraTabs()
        {
            const String input = "      ADD     r0,     r1,     r2  ";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_InvalidInput()
        {
            const String input = "DD r0, r1, r2";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreNotEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2()
        {
            const String input = "LDR r0, =14";
            const UInt64 theoOutput = (Opcode.LDR << 16) | 0x0000;
            const UInt64 theOutput2 = 14;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraWhitespace()
        {
            const String input = "  LDR    r1 ,   =18 ";
            const UInt64 theoOutput = (Opcode.LDR << 16) | 0x0100;
            const UInt64 theOutput2 = 18;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraTabs()
        {
            const String input = "    LDR        r1 ,            =18         ";
            const UInt64 theoOutput = (Opcode.LDR << 16) | 0x0100;
            const UInt64 theOutput2 = 18;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_SingleElement()
        {
            const String input = "MOV r1, #1";
            const UInt64 theoOutput = (Opcode.MOV << 16) | 0x0101;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ConstantTooLarge()
        {
            const String input = "MOV r1, #21";
            const UInt64 theoOutput = (Opcode.MOV << 16) | 0x0105;

            var pracOutput = CompilerHelper.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }
    }
}
