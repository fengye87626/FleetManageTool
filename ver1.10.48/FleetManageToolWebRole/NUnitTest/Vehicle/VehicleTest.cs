using System;
using NUnit.Framework;
using FleetManageToolWebRole.Controllers;
using System.Web.Mvc;
using FleetManageTool.Models.page;

namespace FleetManageToolWebRole.NUnitTest.Vehicles
{
    [TestFixture]
    //public class SampleTest
    public class VehiclesTest
    {
        [Test]
        public void TestVehiclesControllerIndexMethod()
        {
            VehiclesController vehicle = new VehiclesController();
            int tabNum = 0;
            ActionResult expected = new ViewResult();//todo
            ActionResult actual = vehicle.Index(tabNum);
            Assert.AreEqual(expected, actual);
            //Assert.IsInstanceOfType(actual, typeof(ViewResult));
        }
        [Test]
        public void TestVehiclesControllerGetTripLogMethod()
        {
            //VehiclesController TripLog = new VehiclesController();
            //string expected = "";//todo
            //string vehicle = "";//todo
            //string triplog_element = "";//todo
            //string actual = TripLog.GetTripLog(vehicle,triplog_element);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestVehiclesControllerGetTripLogListMethod()
        {
            VehiclesController TripLog = new VehiclesController();
            string expected = "";//todo
            string vehicle = "";//todo
            string type = "";//todo
            string starttime = "";//todo
            string endtime = "";//todo
            JsonResult actual = TripLog.GetTripLogList(vehicle,type,starttime,endtime);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestVehiclesControllerGetTripLogMoreMethod()
        {
            VehiclesController TripLog = new VehiclesController();
            string expected = "";//todo
            JsonResult actual = TripLog.GetTripLogMore("1","0000");
            Assert.AreEqual(expected, actual);
        }
    }
}