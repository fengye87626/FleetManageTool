using FleetManageTool.Models.page;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageToolWebRole.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.NUnitTest.BusinessLayer
{
    [TestFixture]
    public class TestTripLogFetcher
    {
        [Test]
        public void TestGetTrips()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            long vehicleID = 0;//
            DateTime time = Convert.ToDateTime("2014-3-26 16:25:00");
            Boolean flag = true;
            List<TripLog> trips = tripFetcher.GetTrips(vehicleID, time, flag, "xxx",8);
        }

        [Test]
        public void TestGetTripsFromCache()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            long vehicleID = 0;//
            DateTime time = Convert.ToDateTime("2014-3-26 16:25:00");
            Boolean flag = true;
            List<Alert> alertsFromApi = new List<Alert>();
            List<TripLog> trips = tripFetcher.GetTripsFromCache(vehicleID, time, flag, "xx", alertsFromApi);
        }

        [Test]
        public void TestPickUpTripByTime()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            long vehicleID = 0;
            DateTime startTime = Convert.ToDateTime("2014-03-12 12:00:00");
            List<Models.API.Trip> tripsApi = null;
            Boolean flag = true;
            List<Alert> alertsFromApi = new List<Alert>();
            List<TripLog> trips = tripFetcher.PickUpTripByTime(vehicleID, startTime, tripsApi, flag, alertsFromApi);
        }

        [Test]
        public void TestGetTripsFromDB()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            long vehicleID = 0;
            DateTime startTime = Convert.ToDateTime("2014-03-12 12:00:00");
            int count = 15;
            List<Alert> alertsFromApi = new List<Alert>();
            List<TripLog> trips = tripFetcher.GetTripsFromDB(vehicleID, startTime, count, 8, alertsFromApi);
        }

        [Test]
        public void TestPickUpAlertsInfo()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            TripLog tripLog = null;
            List<Alert> alerts = null;
            TripLog trips = tripFetcher.PickUpAlertsInfo(tripLog, alerts);
        }

        [Test]
        public void TestGetTripDetail()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            string GUID = null;
            TripLogDetail tripDetail = tripFetcher.GetTripDetail(GUID,8);
        }

        [Test]
        public void TestGetTripFromApi()
        {
            TripLogFetcher tripFetcher = new TripLogFetcher();
            string GUID = null;
            Models.API.Trip tripDetail = tripFetcher.GetTripFromApi(GUID);
        }
    }
}