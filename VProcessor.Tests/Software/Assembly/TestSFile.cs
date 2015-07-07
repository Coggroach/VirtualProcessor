using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Software.Assembly;

namespace VProcessor.Tests.Software.Assembly
{
    [TestClass]
    public class TestSFile
    {
        [TestMethod]
        public void TestLoad()
        {
            var file = new SFile("Software\\TestControlMemory.txt");
           
            var s = file.GetString();

            Assert.AreEqual("156247", s);
        }

        [TestMethod]
        public void TestSave()
        {
            var file = new SFile("Software\\TestControlMemory.txt");
            const String test = "Jarrod";

            file.SetString(test);
            file.Save();
            file.Load();

            var result = file.GetString();

            Assert.AreEqual(test, result);
        }
    }
}
