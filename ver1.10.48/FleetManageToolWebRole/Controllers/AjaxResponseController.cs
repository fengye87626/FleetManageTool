using FleetManageTool.Models.page;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Filters;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AjaxResponseController : Controller
    {

        public JsonResult GetVehicleInformation(long groupID)
        {
            DebugLog.Debug("DashboardController GetVehicleInformation Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            FleetInfoFetcher fleetInfo = new FleetInfoFetcher();
            int timeZone = 0;
            if (null != Session["TimeZone"])
            {
                timeZone = Int32.Parse(Session["TimeZone"].ToString());
            }
            DebugLog.Debug("DashboardController GetVehicleInformation Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(fleetInfo.GetVehicleInfo(groupID, Session["companyID"].ToString(), timeZone), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroups()
        {
            DebugLog.Debug("DashBoardController GetGroups Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            DebugLog.Debug("DashBoardController GetGroups Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicles(int groupID)
        {
            DebugLog.Debug("DashBoardController GetVehicles Performance Start " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<Vehicle> vehicles = new List<Vehicle>();
            if (-1 == groupID)
            {
                vehicles = dbInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());
            }
            else
            {
                vehicles = dbInterface.GetGroupVehiclesByGroupId(groupID);
            }
            //mabiao  20140506
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
            vehicles.Sort((x, y) => x.name.Trim().CompareTo(y.name.Trim()));
            DebugLog.Debug("DashBoardController GetVehicles Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(vehicles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrips(string vehicleID, string timeZone)
        {
            DebugLog.Debug("DashBoardController GetTrips Performance Start " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            try
            {
                DateTime time = DateTime.Now.ToUniversalTime();
                TripLogFetcher tripInterface = new TripLogFetcher();
                List<TripLog> triplogs = tripInterface.GetTrips(int.Parse(vehicleID), time, true, Session["companyID"].ToString(), int.Parse(timeZone));
                DebugLog.Debug("DashBoardController GetTrips Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                return Json(triplogs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                DebugLog.Debug("DashBoardController GetTrips Performance Exception " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                return null;
            }
        }

        public JsonResult GetVehicleInformationInVehicle()
        {
            DebugLog.Debug("VehiclesController GetVehicleInformation Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            FleetInfoFetcher fleetInfo = new FleetInfoFetcher();
            int timeZone = 0;
            if (null != Session["TimeZone"])
            {
                timeZone = Int32.Parse(Session["TimeZone"].ToString());
            }
            DebugLog.Debug("VehiclesController GetVehicleInformation Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(fleetInfo.GetVehicleListInfo(Session["companyID"].ToString(), timeZone), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsInVehicle()
        {
            DebugLog.Debug("VehiclesController GetGroups Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            VehicleGroup temp = new VehicleGroup();
            temp.pkid = -2;
            groups.Add(temp);
            DebugLog.Debug("VehiclesController GetGroups Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        private DateTime ChangeTimeToUniversalTime(string dateTime, string timeZone)
        {
            DateTime time = new DateTime();
            string symbol = null;
            if (int.Parse(timeZone) >= 0)
            {
                symbol = "+";
            }
            else
            {
                symbol = "-";
            }
            time = Convert.ToDateTime(dateTime.Substring(0, 4) + "-" +
                                                    dateTime.Substring(4, 2) + "-" +
                                                    dateTime.Substring(6, 2) + " " +
                                                    dateTime.Substring(8, 2) + ":" +
                                                    dateTime.Substring(10, 2) + ":" +
                                                    dateTime.Substring(12, 2) + symbol + "0" + timeZone + ":00").ToUniversalTime();

            return time;
        }

        public ActionResult DrawImage(long vehicleID, string type)
        {
            try
            {
                LogoDBInterface logoInterface = new LogoDBInterface();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                TenantDBInterface tenantInterface = new TenantDBInterface();
                long logoID = 0;
                if ("vehicleLogo".Equals(type))
                {
                    logoID = vehicleInterface.GetLogoIDByVehicleID(vehicleID);
                }
                var bytes = logoInterface.GetLogoDataByLogoID(logoID);
                if (bytes != null)
                {
                    return File(bytes, @"image/jpeg");
                }
                else
                {

                    FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Images/vehicle.png", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(file);
                    binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    bytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                    return File(bytes, @"image/jpeg");
                }
            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
            }
            return null;
        }

        public JsonResult GetVehicleAlertsOfOnePage(long vehicleId, int pageNum, string month)
        {
            Dictionary<string, object> alerts = new Dictionary<string, object>();
            AlertFetcher alertIF = new AlertFetcher();
            int timeZone = 0;
            if (null != Session["TimeZone"])
            {
                timeZone = Int32.Parse(Session["TimeZone"].ToString());
            }
            List<VehicleAlert> vehicleAlerts = alertIF.GetVehicleAlertInfo(Session["companyID"].ToString(), vehicleId, month, pageNum, timeZone);
            if (null != vehicleAlerts)
            {
                vehicleAlerts.Sort((y, x) => x.alertTime.CompareTo(y.alertTime));/*fengpan 20140324 #775*/
            }
            alerts.Add("alerts", vehicleAlerts);
            alerts.Add("alertsCount", GetVehicleAlertsPageCount(vehicleId, month));
            return Json(alerts, JsonRequestBehavior.AllowGet);
        }

        public int GetVehicleAlertsPageCount(long vehicleId, string month)
        {
            if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
            {
                return 1;
            }
            else
            {
                AlertFetcher alertFetcher = new AlertFetcher();
                int timeZone = 0;
                if (null != Session["TimeZone"])
                {
                    timeZone = Int32.Parse(Session["TimeZone"].ToString());
                }
                return alertFetcher.GetVehicleAlertNum(Session["companyID"].ToString(), vehicleId, alertFetcher.GetDateTimeMonthFirstDay(DateTime.Parse(month)), alertFetcher.GetDateTimeMonthLastDay(DateTime.Parse(month)), timeZone);
            }
        }

        public String GetFuelLog()
        {
            String view = null;
            view = "[{\"date\":\"2014-03-27\",\"since\":\"0\",\"L\":\"0\",\"cost\":\"0\",\"Mile_Cost\":\"0\",}," +
                    "{\"date\":\"2014-03-26\",\"since\":\"0\",\"L\":\"0\",\"cost\":\"0\",\"Mile_Cost\":\"0\",}," +
                    "{\"date\":\"2014-03-25\",\"since\":\"0\",\"L\":\"0\",\"cost\":\"0\",\"Mile_Cost\":\"0\",}," +
                    "{\"date\":\"2014-03-24\",\"since\":\"0\",\"L\":\"0\",\"cost\":\"0\",\"Mile_Cost\":\"0\",}" +
                   "]";
            return view;
        }

        public JsonResult GetCurrentVehicleInfo(long vehicleID)
        {
            try
            {
                VehicleInfo vehicleInfo = null;
                VehicleDBInterface vehicleDbInterface = new VehicleDBInterface();
                Models.Vehicle vehicleDB = vehicleDbInterface.GetVehicleByID(vehicleID);
                FleetInfoFetcher fleetFetcher = new FleetInfoFetcher();
                int timeZone = 0;
                if (null != Session["TimeZone"])
                {
                    timeZone = Int32.Parse(Session["TimeZone"].ToString());
                }
                var fleetInfo = fleetFetcher.GetInfoFromCache(Session["companyID"].ToString(), -1, timeZone, false);
                if (null == fleetInfo || null == fleetInfo.allVehicle)
                {
                    //..
                }
                else
                {
                    vehicleInfo = fleetInfo.allVehicle.Find(t => t.primarykey == vehicleID);
                }
                if (null == vehicleInfo || null == vehicleDB)
                {
                    return null;
                }
                else
                {
                    vehicleInfo.alerts = null;
                    return Json(vehicleInfo, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                DebugLog.Debug("[VehiclesContrller] GetCurrentVehicleInfo() Exception: " + e.Message);
                return null;
            }
        }

        [LogFilter]
        //detail 画面中 列表风格、地图风格的triplog都取这个接口的数据
        public JsonResult GetTripsInVehicle(string vehicleID, string nearTime, string oldTime, string timeZone)
        {
            DebugLog.Debug("VehiclesController GetTrips Performance Start " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            try
            {
                DateTime ntime = new DateTime();
                Boolean flag = false;
                TripLogFetcher tripInterface = new TripLogFetcher();

                if (null == nearTime || nearTime.Equals(""))
                {
                    flag = true;
                    ntime = DateTime.Now.ToUniversalTime();
                }
                else
                {
                    ntime = ChangeTimeToUniversalTime(nearTime, timeZone);
                }

                if (null == oldTime || oldTime.Equals(""))
                {
                    List<TripLog> triplogs = tripInterface.GetTrips(int.Parse(vehicleID), ntime, flag, Session["companyID"].ToString(), int.Parse(timeZone));
                    DebugLog.Debug("VehiclesController GetTrips Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                    return Json(triplogs, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DateTime otime = ChangeTimeToUniversalTime(oldTime, timeZone);
                    List<TripLog> triplogs = tripInterface.GetTrips(int.Parse(vehicleID), ntime, otime, flag, Session["companyID"].ToString(), int.Parse(timeZone));
                    DebugLog.Debug("VehiclesController GetTrips Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                    return Json(triplogs, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                DebugLog.Debug("VehiclesController GetTrips Performance Exception " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                return null;
            }
        }

        public JsonResult GetGeofenceAlertsOfOnePage(long vehicleId, int pageNum, string month)
        {
            Dictionary<string, object> alerts = new Dictionary<string, object>();
            AlertFetcher alertIF = new AlertFetcher();
            List<GeofenceAlert> geoAlerts = alertIF.GetVehicleGeofenceAlertInfo(Session["companyID"].ToString(), vehicleId, pageNum, month, Int32.Parse(Session["TimeZone"].ToString()));
            if (null != geoAlerts)
            {
                geoAlerts.Sort((y, x) => (x.alertTime.CompareTo(y.alertTime)));/*fengpan 20140324 #775*/
            }
            alerts.Add("alerts", geoAlerts);
            alerts.Add("alertsCount", GetGeofenceAlertsPageCount(vehicleId, month));
            return Json(alerts, JsonRequestBehavior.AllowGet);
        }

        public int GetGeofenceAlertsPageCount(long vehicleId, string month)
        {
            if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
            {
                return 1;
            }
            else
            {
                AlertFetcher alertFetcher = new AlertFetcher();
                return alertFetcher.GetGeofenceAlertNum(Session["companyID"].ToString(), vehicleId, alertFetcher.GetDateTimeMonthFirstDay(DateTime.Parse(month)), alertFetcher.GetDateTimeMonthLastDay(DateTime.Parse(month)), Int32.Parse(Session["TimeZone"].ToString()));
            }
        }

    }
}
