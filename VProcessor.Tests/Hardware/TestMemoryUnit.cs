using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VProcessor.Tests.Hardware
{
    using VProcessor.Hardware;
    using VProcessor.Software.Assembly;

    [TestClass]
    public class TestMemoryUnit
    {
        [TestMethod]
        public void TestStartUp_ControlMemory()
        {
            this.TestStartUp("Software\\TestControlMemory.txt");
        }

        [TestMethod]
        public void TestStartUp_UserMemory()
        {
            this.TestStartUp("Software\\TestUserMemory.txt");
        }

        internal void TestStartUp(String path)
        {            
            var file = new SFile(path);
            var compiler = new Compiler();

            var memChunk = compiler.Compile(file, 4);
            var memory = new MemoryUnit(memChunk);

            Assert.AreNotEqual((UInt32)0, memory.GetMemory());
            memory++;
            Assert.AreEqual((UInt32)0, memory.GetMemory());
        }
    }
}
