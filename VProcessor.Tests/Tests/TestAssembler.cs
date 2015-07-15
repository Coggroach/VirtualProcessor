using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;

namespace VProcessor.Tests.Software.Assembly
{
    [TestClass]
    public class TestAssembler
    {
        [TestMethod]
        public void Test_ValidInput_Type1()
        {
            const String input = "ADD r0, r1, r2";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraWhitespace()
        {
            const String input = "  ADD  r0, r1,  r2  ";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraTabs()
        {
            const String input = "      ADD     r0,     r1,     r2  ";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_InvalidInput()
        {
            const String input = "DD r0, r1, r2";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreNotEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2()
        {
            const String input = "LDR r0, =14";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("LDR") << 16) | 0x0000;
            const UInt64 theOutput2 = 14;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraWhitespace()
        {
            const String input = "  LDR    r1 ,   =18 ";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("LDR") << 16) | 0x0100;
            const UInt64 theOutput2 = 18;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraTabs()
        {
            const String input = "    LDR        r1 ,            =18         ";
            var theoOutput = (UInt32)(Opcode.GetCodeAddress("LDR") << 16) | 0x0100;
            const UInt64 theOutput2 = 18;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_SingleElement()
        {
            const String input = "MOV r1, #1";
            var theoOutput = (UInt32)((Opcode.GetCodeAddress("MOV") + 1) << 16) | 0x0101;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ConstantTooLarge()
        {
            const String input = "MOV r1, #21";
            var theoOutput = (UInt32)((Opcode.GetCodeAddress("MOV") + 1) << 16) | 0x0105;

            var pracOutput = new Assembler().Convert(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_Compiler()
        {
            const Int32 length = 8;

            var compiler = new Assembler();

            var testFile = new VPFile("Res\\TestAssembly.vps", VPFile.Assembly);
            var expectedFile = new VPFile("Res\\TestAssemblyCode.vpo", VPFile.Hexadecimal);
            
            var testMemory = compiler.Compile32(testFile, length);
            var expectedMemory = compiler.Compile32(expectedFile, length);

            Assert.AreEqual(length, testMemory.GetLength());
            for (var i = 0; i < length; i++)
                Assert.AreEqual(expectedMemory.GetMemory(i), testMemory.GetMemory(i));            
        }

        [TestMethod]
        public void Test_Mov_Registers()
        {
            const Int32 length = 3;

            var compiler = new Assembler();

            var file = new VPFile("Res\\TempAssembly.vps", VPFile.Assembly);

            file.SetString("MOV r0, r2");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);
            Assert.AreEqual((UInt32) 0x000B0002, memory.GetMemory(0));
        }

        [TestMethod]
        public void Test_BranchRegistering()
        {
            const Int32 length = 12;

            var compiler = new Assembler();

            var file = new VPFile("Res\\TempAssembly.vps", VPFile.Assembly);

            file.SetString("CMP r0, #1; BEQ branch; LDR r0, =14; branch; ADD r0, r0, #1");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);            
        }

        [TestMethod]
        public void Test_Str_WithConstant()
        {
            const Int32 length = 12;

            var compiler = new Assembler();

            var file = new VPFile("Res\\TempAssembly.vps", VPFile.Assembly);

            file.SetString("STR r0, [r1, #1]");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);
            Assert.AreNotEqual(0, memory.GetMemory(0));
        }
    }
}
