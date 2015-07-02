using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;

namespace VProcessor.Tests.Software.Assembly
{
    [TestClass]
    public class TestAssemblyConverter
    {
        [TestMethod]
        public void TestConverter_ValidInput()
        {
            const String input = "ADD r0, r1, r2";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = AssemblyCompiler.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput);
        }

        [TestMethod]
        public void TestConverter_ValidInput_ExtraWhitespace()
        {
            const String input = "  ADD  r0, r1,  r2  ";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = AssemblyCompiler.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput);
        }

        [TestMethod]
        public void TestConverter_ValidInput_ExtraTabs()
        {
            const String input = "      ADD     r0,     r1,     r2  ";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = AssemblyCompiler.Convert(input);

            Assert.AreEqual(theoOutput, pracOutput);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestConverter_InvalidInput()
        {
            const String input = "DD r0, r1, r2";
            const UInt64 theoOutput = (Opcode.ADD << 16) | 0x0012;

            var pracOutput = AssemblyCompiler.Convert(input);

            Assert.AreNotEqual(theoOutput, pracOutput);
        }
    }
}
