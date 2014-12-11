using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Filters;
using FleetManageTool.Models.page;
using FleetManageToolWebRole.Util;
using System.Threading;
namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class DashboardController : Controller
    {
        //进去主画面，根据所给vins和时区获取主画面显示数据
        public ActionResult Home()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");
            CacheConfig.CacheSetting(Response);
            ViewBag.companyID = Session["companyID"];
            ViewBag.CountPerShortPage = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ShortCountNumForDashboard"]);
            ViewBag.CountPerTallPage = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["TallCountNumForDashboard"]); 

            ViewBag.lat = Session["latForSearch"];
            ViewBag.lng = Session["lngForSearch"];
            ViewBag.strSelect = Session["strSelectForSearch"];
            //Add by LiYing starat
            ViewBag.strSelectName = Session["strSelectForSearchName"];
            //Add by LiYing end
            ViewBag.zoom = Session["zoomForSearch"];
            ViewBag.showType = Session["showTypeForSearch"];
            CleanSearchPara();
           
            return View();
        }

        //获取车型 fengpan 20140304
        //chenyangwen 20140611 #1357
        [SessionTimeoutTimerFilter]
        public JsonResult GetVehicleInformation(long groupID) {
            DebugLog.Debug("DashboardController GetVehicleInformation Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            FleetInfoFetcher fleetInfo = new FleetInfoFetcher();
            if (null == Session["TimeZone"])
            {
                Session["TimeZone"] = 0;
            }
            DebugLog.Debug("DashboardController GetVehicleInformation Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(fleetInfo.GetVehicleInfo(groupID, Session["companyID"].ToString(), Int32.Parse(Session["TimeZone"].ToString())), JsonRequestBehavior.AllowGet);
        }
        //从数据库获取所有的车组显示在下拉列表中
        //chenyangwen 20140611 #1357
        [SessionTimeoutTimerFilter]
        public JsonResult GetGroups()
        {
            DebugLog.Debug("DashBoardController GetGroups Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            DebugLog.Debug("DashBoardController GetGroups Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        //根据车族获取车辆信息
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
            vehicles.Sort((x,y) => x.name.Trim().CompareTo(y.name.Trim()));
            DebugLog.Debug("DashBoardController GetVehicles Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(vehicles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTripLog(String Vehicle)
        {
            try
            {
                TripLogFetcher tripInterface = new TripLogFetcher();
                List<TripLog> triplogs = new List<TripLog>();
                List<TripLog> triplogback = new List<TripLog>();
                long VehicleID = int.Parse(Vehicle);
                triplogs = tripInterface.GetTripLogs(VehicleID, null);

                int height = 0;

                foreach (TripLog triplogTemp in triplogs)
                {
                    height += TripLogAddHeight(triplogTemp);

                    if (height > 715)
                    {
                        break;
                    }

                    triplogback.Add(triplogTemp);

                }
                return Json(triplogback, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //mabiao 一个节点需要增加的高度
        //20140312
        //When connect to API.this function need to delete
        public int TripLogAddHeight(TripLog triplog)
        {
            int addheight = 0;
            int length = 0;
            switch (triplog.type)
            {
                case "Driving": addheight = 40;
                    break;
                case "Final": addheight = 50;
                    break;
                case "Day": addheight = 55;
                    break;
                case "Normal":
                    length = 0;
                    addheight = 70;
                    if (0 == triplog.healthStatus)
                    {
                        length++;
                    }
                    if (triplog.alerts.Count > 0)
                    {
                        length++;
                    }
                    if (length > 1)
                    {
                        addheight += (length - 1) * 20;
                    }
                    break;
                case "Trailer": addheight = 70;
                    break;
            }
            return addheight;
        }

        //chenyangwen 20140611 #1357
        [SessionTimeoutTimerFilter]
        public JsonResult GetTrips(string vehicleID,string timeZone)
        {
            DebugLog.Debug("DashBoardController GetTrips Performance Start " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            try
            {
                DateTime time = DateTime.Now.ToUniversalTime();
                TripLogFetcher tripInterface = new TripLogFetcher();
                List<TripLog> triplogs = tripInterface.GetTrips(int.Parse(vehicleID), time, true, Session["companyID"].ToString(),int.Parse(timeZone));
                DebugLog.Debug("DashBoardController GetTrips Performance End " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                return Json(triplogs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                DebugLog.Debug("DashBoardController GetTrips Performance Exception " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
                return null;
            }
        }
        public void SearchPara(String zoom, String showType, String lat, String lng, String strSelect, String strSelectID) 
        { 
            Session["zoomForSearch"] = zoom;
            Session["showTypeForSearch"] = showType;
            Session["latForSearch"] = lat;
            Session["lngForSearch"] = lng;
         //   Session["strSelectForSearch"] = strSelect;
            Session["strSelectForSearch"] = strSelectID;
            Session["strSelectForSearchName"] = strSelect;
            return ;
        }
        public void CleanSearchPara() 
        {
            Session["zoomForSearch"] = null;
            Session["showTypeForSearch"] = null;
            Session["latForSearch"] = null;
            Session["lngForSearch"] = null;
            Session["strSelectForSearch"] = null;
            Session["strSelectForSearchName"] = null;
            return ;
        }
        //20140404 mabiao 
        //trip地点写入DB
        public void WriteLocationToDB(string tripGUID, string flag, string address)
        {
            VehicleDBInterface vehicleInterface = new VehicleDBInterface();
            Boolean flagLocation = true;
            if (flag.Equals("start"))
            {
                flagLocation = true;
            }
            else if (flag.Equals("end"))
            {
                flagLocation = false;
            }
            else
            {
                return;
            }
            vehicleInterface.WriteLocationToDB(tripGUID, flagLocation, address);
        }

        //保存时区信息到session
        public void SaveTimeZone(int timeZone /*,string date*/)
        {
            DebugLog.Debug("DashboardController SaveTimeZone Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            DebugLog.Debug("[dashboard controller]SaveTimeZone  paras:[timeZone = " + timeZone + "]");
            //DateTime now = DateTime.Parse(date);
            Session["TimeZone"] = timeZone;
            if (null == Session["NowDate"])
            {
                //Session["NowDate"] = now;
            }
            DebugLog.Debug("DashboardController SaveTimeZone Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            //return;
        }
        
        /// <summary>
        /// 保存所选车辆到session
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        public string SaveChooseVehicle(long vehicleID) 
        {
            Session["ChooseVehicleID"] = vehicleID;
            return "OK";
        }
    }
}
