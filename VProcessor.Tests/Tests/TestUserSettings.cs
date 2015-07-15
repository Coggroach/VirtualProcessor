using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VProcessor.Gui;

namespace VProcessor.Tests.Tests
{
    [TestClass]
    public class TestUserSettings
    {
        [TestMethod]
        public void TestLoading()
        {
           var settings = new UserSettings();

           settings.Save();
        }
    }
}
