using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VProcessor.Tests.Hardware
{
    using VProcessor.Hardware;
    using VProcessor.Software.Assembly;
    using VProcessor.Common;
    using VProcessor.Hardware.Memory;

    [TestClass]
    public class TestMemoryUnit
    {
        [TestMethod]
        public void TestStartUp_ControlMemory()
        {
            this.TestStartUp("Res\\TestControlMemory.vpo");
        }

        [TestMethod]
        public void TestStartUp_UserMemory()
        {
            this.TestStartUp("Res\\TestUserMemory.vpo");
        }

        internal void TestStartUp(String path)
        {            
            var file = new VPFile(path);
            var compiler = new Assembler();

            var memChunk = compiler.Compile32(file, 4);
            var memory = new MemoryUnit<UInt32>(memChunk);

            Assert.AreNotEqual((UInt32)0, memory.GetMemory());
            memory++;
            Assert.AreEqual((UInt32)0, memory.GetMemory());
        }
    }
}
