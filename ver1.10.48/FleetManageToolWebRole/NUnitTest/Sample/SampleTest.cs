using System;
using NUnit.Framework;
using FleetManageToolWebRole.Controllers;

namespace FleetManageToolWebRole.NUnitTest.Dashboard
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void TestHomeControllerMethod()
        {
            HomeController home = new HomeController();
            int a = 1;
            int b = 2;
            Assert.AreEqual(home.TestNUnit(a, b), 3);
        }
    }
}