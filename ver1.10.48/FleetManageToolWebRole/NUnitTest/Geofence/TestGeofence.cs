using System;
using NUnit.Framework;
using System.Web.Mvc;
using FleetManageToolWebRole.Controllers;

namespace FleetManageToolWebRole.NUnitTest.Geofence
{
    [TestFixture]
    public class TestGeofence
    {
        [Test]
        public void TestControllerHomeMethod()
        {
            GeoFenceController geofence = new GeoFenceController();
            double lat = 0;
            double lng = 0;
            var viewdata = (ViewResult)geofence.Landing();

            Assert.AreEqual("Home", viewdata.ViewBag.GeofenceName);
            Assert.AreEqual("123 Argyle Street, Boston", viewdata.ViewBag.GeofenceLocation);
            Assert.AreEqual("Delivery Van 1, Delivery Van 2, Van 3, Ford Echo 1995, Blue Honda Element, Van 4", viewdata.ViewBag.GeofenceVehicles);
            Assert.AreEqual("激活", viewdata.ViewBag.GeofenceStatus);
        }
    }
}