using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VProcessor.Tests.Hardware
{
    using VProcessor.Hardware;

    [TestClass]
    public class TestMemoryUnit
    {
        [TestMethod]
        public void TestStartUp()
        {
            var memory = new MemoryUnit(4, "Software\\TestControlMemory.txt");
            Assert.AreNotEqual((UInt32) 0, memory.GetMemory());
            memory++;
            Assert.AreEqual((UInt32) 0, memory.GetMemory());
        }
    }
}
