using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VProcessor.Tests.Hardware
{
    using VProcessor.Hardware;

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
            var memory = new MemoryUnit(4, path);
            Assert.AreNotEqual((UInt32)0, memory.GetMemory());
            memory++;
            Assert.AreEqual((UInt32)0, memory.GetMemory());
        }
    }
}
