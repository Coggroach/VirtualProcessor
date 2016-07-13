using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Common;
using VProcessor.Hardware.Memory;
using VProcessor.Software;

namespace VProcessor.Tests.Hardware
{
    [TestClass]
    public class TestMemoryUnit
    {
        [TestMethod]
        public void TestStartUp_ControlMemory()
        {
            TestStartUp("Res\\TestControlMemory.vpo");
        }

        [TestMethod]
        public void TestStartUp_UserMemory()
        {
            TestStartUp("Res\\TestUserMemory.vpo");
        }

        internal void TestStartUp(string path)
        {            
            var file = new VpFile(path);
            var compiler = new Assembler();

            var memChunk = compiler.Compile32(file, 4);
            var memory = new MemoryUnit<uint>(memChunk);

            Assert.AreNotEqual((uint)0, memory.GetMemory());
            memory++;
            Assert.AreEqual((uint)0, memory.GetMemory());
        }
    }
}
