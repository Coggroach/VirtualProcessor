﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Common;
using VProcessor.Software;

namespace VProcessor.Tests.Tests
{
    [TestClass]
    public class TestAssembler
    {
        [TestMethod]
        public void Test_ValidInput_Type1()
        {
            const string input = "ADD r0, r1, r2";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraWhitespace()
        {
            const string input = "  ADD  r0, r1,  r2  ";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type1_ExtraTabs()
        {
            const string input = "      ADD     r0,     r1,     r2  ";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_InvalidInput()
        {
            const string input = "DD r0, r1, r2";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("ADD") << 16) | 0x0012;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreNotEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2()
        {
            const string input = "LDR r0, =14";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("LDR") << 16) | 0x0000;
            const ulong theOutput2 = 14;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraWhitespace()
        {
            const string input = "  LDR    r1 ,   =18 ";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("LDR") << 16) | 0x0100;
            const ulong theOutput2 = 18;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ExtraTabs()
        {
            const string input = "    LDR        r1 ,            =18         ";
            var theoOutput = (uint)(OpcodeRegistry.Instance.GetCodeAddress("LDR") << 16) | 0x0100;
            const ulong theOutput2 = 18;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
            Assert.AreEqual(theOutput2, pracOutput[1]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_SingleElement()
        {
            const string input = "MOV r1, #1";
            var theoOutput = (uint)((OpcodeRegistry.Instance.GetCodeAddress("MOV") + 1) << 16) | 0x0101;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_ValidInput_Type2_ConstantTooLarge()
        {
            const string input = "MOV r1, #21";
            var theoOutput = (uint)((OpcodeRegistry.Instance.GetCodeAddress("MOV") + 1) << 16) | 0x0105;

            var pracOutput = new Assembler().ConvertLine32(input);

            Assert.AreEqual(theoOutput, pracOutput[0]);
        }

        [TestMethod]
        public void Test_Compiler()
        {
            const int length = 8;

            var compiler = new Assembler();

            var testFile = new VpFile("Res\\TestAssembly.vps", VpFile.Assembly);
            var expectedFile = new VpFile("Res\\TestAssemblyCode.vpo");
            
            var testMemory = compiler.Compile32(testFile, length);
            var expectedMemory = compiler.Compile32(expectedFile, length);

            Assert.AreEqual(length, testMemory.Length);
            for (var i = 0; i < length; i++)
                Assert.AreEqual(expectedMemory.GetMemory(i), testMemory.GetMemory(i));            
        }

        [TestMethod]
        public void Test_Mov_Registers()
        {
            const int length = 3;

            var compiler = new Assembler();

            var file = new VpFile("Res\\TempAssembly.vps", VpFile.Assembly);

            file.SetString("MOV r0, r2");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);
            Assert.AreEqual((uint) 0x000B0002, memory.GetMemory(0));
        }

        [TestMethod]
        public void Test_BranchRegistering()
        {
            const int length = 12;

            var compiler = new Assembler();

            var file = new VpFile("Res\\TempAssembly.vps", VpFile.Assembly);

            file.SetString("CMP r0, #1; BEQ branch; LDR r0, =14; branch; ADD r0, r0, #1");
            file.Save();
            file.Load();

            compiler.Compile32(file, length);            
        }

        [TestMethod]
        public void Test_Str_WithConstant()
        {
            const int length = 12;

            var compiler = new Assembler();

            var file = new VpFile("Res\\TempAssembly.vps", VpFile.Assembly);

            file.SetString("STR r0, [r1, #1]");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);
            Assert.AreNotEqual(0, memory.GetMemory(0));
        }

        [TestMethod]
        public void Test_Ldr_WithAddress()
        {
            const int length = 12;

            var compiler = new Assembler();

            var file = new VpFile("Res\\TempAssembly.vps", VpFile.Assembly);

            file.SetString("LDR r0, =branch; ADD r0, r0, #1; branch");
            file.Save();
            file.Load();

            var memory = compiler.Compile32(file, length);
            Assert.IsTrue(3 == memory.GetMemory(1));
        }
    }
}
