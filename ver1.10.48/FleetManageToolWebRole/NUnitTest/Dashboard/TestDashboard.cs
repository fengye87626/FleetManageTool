using System;
using NUnit.Framework;
using System.Web.Mvc;
using FleetManageToolWebRole.Controllers;

namespace FleetManageToolWebRole.NUnitTest.Dashboard
{
    [TestFixture]
    public class TestDashboard
    {
        [Test]
        public void TestControllerHomeMethod()
        {
            DashboardController dashboard = new DashboardController();
            var viewdata = (ViewResult)dashboard.Home();
            
            Assert.AreEqual(4, viewdata.ViewBag.DiverVehicle);
            Assert.AreEqual(2, viewdata.ViewBag.StopVehicle);
            Assert.AreEqual(3, viewdata.ViewBag.BreakVehicle);
            Assert.AreEqual(3, viewdata.ViewBag.AlertVehicle);
            Assert.AreEqual(3, viewdata.ViewBag.MissedVehicle);
            Assert.AreEqual(0, viewdata.ViewBag.VehiclesInfo);
            Assert.AreEqual("Delivery Van 1", viewdata.ViewBag.VehiclesName);
            Assert.AreEqual("Hilton Downtown", viewdata.ViewBag.VehiclesLocaltion);
            Assert.AreEqual("40分钟", viewdata.ViewBag.VehiclesEngineTime);
            Assert.AreEqual("OK", viewdata.ViewBag.VehiclesEngine);
            Assert.AreEqual("有故障", viewdata.ViewBag.VehiclesHealth);
            Assert.AreEqual("~/Content/Home/images/u142_normal.png", viewdata.ViewBag.VehicleLogoUrl);
            Assert.AreEqual(2, viewdata.ViewBag.StopVehicle);
        }
        [Test]
        public void TestGetGroups()
        {
            //DashboardController dashboard = new DashboardController();
            //string viewdata = dashboard.GetGroups();

            //Assert.AreEqual("group1,group2", viewdata);
        }

        [Test]
        public void TestGetVehicles()
        {
            //DashboardController dashaboard = new DashboardController();
            //string expected = "";//todo
            //string group = "";//todo
            //string actual = dashaboard.GetVehicles(group);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetTripLog()
        {
            DashboardController dashaboard = new DashboardController();
            string expected = "";//todo
            string vehicle = "";//todo
            JsonResult actual = dashaboard.GetTripLog(vehicle);
            Assert.AreEqual(expected, actual);
        }
    }
}