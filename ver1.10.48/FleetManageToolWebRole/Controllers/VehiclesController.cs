using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Filters;
using Microsoft.ApplicationServer.Caching;
using System.Runtime.Serialization;
using System.Collections;
using FleetManageTool.Models.page;
//caoyandong-Operating
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Util;
using BaiduWGSPoint;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Models.API;
using FleetManageTool.WebAPI;
using System.Threading;
using FleetManageTool.WebAPI.Exceptions;

namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class VehiclesController : Controller
    {
        /*fengpan*/
        [LogFilter]
        public ActionResult Index(int tabNum=0)
        {
            //chenyangwen 20140609 #1172
            if (tabNum >= CommonConstant.PAGE_VEHICLE_TAB_NUM)
            {
                return RedirectToAction("Index", "Error");
            }

            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            ViewBag.tabNum = tabNum;
            DateTime now = DateTime.Now;
            ViewBag.Update_Time = now.ToString("yyyy年MM月dd日 HH:mm");//显示当前时间
            return View();
        }
        /*fengpan*/

        [RoleFilter]
        [LogFilter]
        [SessionFilter]
        public string AddVehicle(string regkey, string esn)
        {
            try
            {
                //Add by liying Start
                //判断是否存在
                //查询新建表，ESN和KEY是否存在且匹配，若匹配，则进行下面已有步骤，否则返回
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                int count = vehicleInterface.CheckOBUAndRegKeyMatch(esn, regkey);
                if (count > 0)
                {
                    //Add by liying End
                    Device device = APIUtil.ValidateObu(esn, regkey);
                    if (null != device)
                    {
                        TenantDBInterface tenantdbInterface = new TenantDBInterface();
                        //             VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                        HalLink customerLink = device.Links.Find(t => t.Rel == "customer");
                        if (null != customerLink)
                        {
                            string customerid = customerLink.Href.Substring(customerLink.Href.LastIndexOf("/") + 1);
                            DebugLog.Debug("VehiclesController AddVehicle Run customerid = " + customerid);
                            //if (!tenantdbInterface.IsCustomerRegistered(customerid))
                            if (!tenantdbInterface.isOBUExist(esn, regkey))
                            {
                                long tenantID = tenantdbInterface.GetTenantIDByCompanyID(Session["companyID"].ToString());
                                DebugLog.Debug("VehiclesController AddVehicle Run tenantID = " + tenantID);
                                List<Device> devices = APIUtil.GetDevicesOfCustomer(customerid);
                                long obuid = -1;
                                foreach (Device deviceTemp in devices)
                                {
                                    //DB相关的处理
                                    //chenyangwen 20140520 #1569
                                    if (0 < vehicleInterface.CheckOBUAndRegKeyMatch(deviceTemp.LabelId, deviceTemp.RegistrationNumber))
                                    {
                                        Obu obu = new Obu();
                                        obu.regkey = deviceTemp.RegistrationNumber;
                                        obu.id = deviceTemp.LabelId;
                                        obu.guid = deviceTemp.Id;
                                        obu.tenantid = tenantID;
                                        obu.idtype = deviceTemp.LabelIdType;
                                        obu.status = "Active";
                                        obuid = vehicleInterface.AddOBU(obu);

                                    //Add by LiYing start
                                    vehicleInterface.updateOBUStatus(obu.id, obu.regkey);
                                    //Add by LiYing end
                                }
                        	}

                            Models.Customer customer = new Models.Customer() { guid = customerid, obuid = obuid, tenantid = tenantID };
                            tenantdbInterface.AddACustomer(customer);
                        	DebugLog.Debug("VehiclesController AddVehicle End return OK");
                            return "OK";
                        }
                        else
                        {
                            return "Regist";
                        }
                    }
                }
            }
            DebugLog.Debug("VehiclesController AddVehicle End return NG");
            return "NG";
            }
            catch (DBException dbException)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, dbException.Message);
                return "error";
            }
            catch (HalException halException)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, halException.Message);
                return "error";
            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
                return "error";
            }
        }

        //获取车型 fengpan 20140304
        [LogFilter]
        public JsonResult GetVehicleInformation()
        {
            DebugLog.Debug("VehiclesController GetVehicleInformation Performance Start" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            FleetInfoFetcher fleetInfo = new FleetInfoFetcher();
            if (null == Session["TimeZone"])
            {
                Session["TimeZone"] = 0;
            }
            DebugLog.Debug("VehiclesController GetVehicleInformation Performance End" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:ff"));
            return Json(fleetInfo.GetVehicleListInfo(Session["companyID"].ToString(),Int32.Parse(Session["TimeZone"].ToString())), JsonRequestBehavior.AllowGet);
        }
        //获取分组信息
        [LogFilter]
        public JsonResult GetGroups()
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
        /*fengpan*///fengpan #508 20140304
        [LogFilter]
        public string Edit_vehicle(string VehicleID) {
            Session["setting_vehicleID"] = VehicleID;
            Session["setting_tabNum"] = 2;
            return "OK";
        }
        [LogFilter]
        public ActionResult Detail(long VehicleID)
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            if (null == Session["nowUser"])
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Vehicle_id = VehicleID;
            /*string vin = "2G2WP552571160925";
            int timeZone = 8;*/
            /*mabiao*/
            /*chenyangwen*/
			String companyID = Session["companyID"].ToString();

            if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
            {
	            VehicleDBInterface dbInterface = new VehicleDBInterface();
	            //mabiao
	            Models.Vehicle vehicleTemp = dbInterface.GetVehicleByID(VehicleID);
	            if (null != vehicleTemp)
	            {
	                //chenyangwen 2014/3/1
                    if (null != vehicleTemp.name)
                    {
                        ViewBag.VehicleName = vehicleTemp.name.Trim(); //vehicleTemp.name;
                    }
                    if (null != vehicleTemp.info)
                    {
                        ViewBag.VehicleInfo = vehicleTemp.info.Trim();
                    }
                    if (null != vehicleTemp.vin)
                    {
                        ViewBag.VehicleVin = vehicleTemp.vin.Trim();
                    }
                    if (null != vehicleTemp.drivername)
                    {
                        ViewBag.driverName = vehicleTemp.drivername.Trim();
                    }
	                //chenyangwen 2014/3/1
	            }
	            //modified by caoyandong
	            else
	            {
	                return RedirectToAction("Index", "Error");
	            }
	            //modified by caoyandong
				/*mabiao*/
	            ViewBag.Logo_Name = "logo";
	            ViewBag.Update_Time = "2013年6月29日 13:28";
	            ViewBag.Logo_Url = "~/Content/Common/images/title_logo.png";
	            ViewBag.VehicleLogo = "~/Content/vehicle/images/vehicle_detail_vehiclelogo.png";
	            ViewBag.ESN = "6C4C729D";
	            ViewBag.RegistrationKey = "59e5-ad61";
	            ViewBag.VehicleOdometer = "255,999";
	            ViewBag.VehicleFuel = 24;
	            ViewBag.VehicleEngine = "ON";
                ViewBag.VehicleHealth = "Alert";
	            ViewBag.VehicleBattery = 12;
                ViewBag.VehicleMisState = 0;
                ViewBag.alertVehicle = "OK";
                FleetInfoFetcher fleetInfoFetcher = new FleetInfoFetcher();
                VehicleInfo vehicle = new VehicleInfo();
                FleetInfo fleet = fleetInfoFetcher.GetVehicleInfo(-1, companyID, Int32.Parse(Session["TimeZone"].ToString()));
                if (null != fleet)
                {
                    if (null != fleet.allVehicle)
                    {
                        vehicle = fleet.allVehicle.Find(t => t.primarykey == VehicleID);
                    }
                }
                ViewBag.VehicleLocationLongtitude = vehicle.location.longitude;
                ViewBag.VehicleLocationLatitude = vehicle.location.latitude;
                return View();
            }
			else
			{
				//API 交互 chenyangwen 20140328
                CacheService service = new CacheService();
				FleetInfoFetcher fleetFetcher = new FleetInfoFetcher();
				if (null == Session["TimeZone"])
				{
					Session["TimeZone"] = 0;
				}
				var fleetInfo = fleetFetcher.GetInfoFromCache(companyID, -1, Int32.Parse(Session["TimeZone"].ToString()),false);
				VehicleInfo vehicle = new VehicleInfo();
				VehicleDBInterface vehicleDbInterface = new VehicleDBInterface();
				Models.Vehicle vehicleDB = vehicleDbInterface.GetVehicleByID(VehicleID);
				if (null == fleetInfo)
				{
					vehicle = ConvertVehicle(vehicleDB);
				}
				else
				{
					vehicle = ((FleetInfo)fleetInfo).allVehicle.Find(v => v.primarykey == VehicleID);
				}

                if (!(null == vehicle && null == vehicleDB))
				{
                    if (null == vehicle)
                    {
                        return View();
                    }
                    //telephone zhangbo add
                    if (null == vehicle.telephone)
                    {
                        ViewBag.telephone = "";
                    }
                    else
                    {
                        ViewBag.telephone = vehicle.telephone.Trim();
                    }
				
                    ViewBag.VehicleMisState = vehicle.misState;
                    if (vehicle.alertType == AlertType.NOALERT)
                    {
                        ViewBag.alertVehicle = "OK";
                    }
                    else
                    {
                        ViewBag.alertVehicle = "Alert";
                    }
					if (null == vehicle.name)
					{
						ViewBag.VehicleName = "";
					}
					else
					{
						ViewBag.VehicleName = vehicle.name.Trim();
					}

					if (null == vehicle.Info)
					{
						ViewBag.VehicleInfo = "";
					}
					else
					{
						ViewBag.VehicleInfo = vehicle.Info.Trim();
					}
					ViewBag.VehicleVin = vehicle.vin;
					if (null == vehicle.driver)
					{
						ViewBag.driverName = "";
					}
					else
					{
						ViewBag.driverName = vehicle.driver.Trim();
					}
					Obu obu = vehicleDbInterface.GetOBUByVehicleId(vehicle.primarykey);
					if (null != obu)
					{
						ViewBag.ESN = obu.id;
						ViewBag.RegistrationKey = obu.regkey;
					}
					ViewBag.Vehicle_id = VehicleID;
					if (null == vehicle.speed)
					{
						ViewBag.VehicleSpeed = null;
					}
					else
					{
						ViewBag.VehicleSpeed = vehicle.speed;
					}
					AlertFetcher alertFetcher = new AlertFetcher();
					AlertConfigurationInfo alertConfigs = alertFetcher.GetAlertConfigInfoFromDB(companyID);
                    if (null != alertConfigs.speed && "" != alertConfigs.speed)
                    {
                        ViewBag.VehicleSpeedThreshold = Int32.Parse(alertConfigs.speed);
                    }
					ViewBag.VehicleOdometer = vehicle.odometer;
					ViewBag.VehicleFuel = vehicle.fuel ;
					if (EngineStatus.ENGINEON == vehicle.engineStatus)
					{
						ViewBag.VehicleEngine = "ON";
					}else{
						ViewBag.VehicleEngine = "OFF";
					}
					if (HealthStatus.ENGINELIGHTON == vehicle.healthStatus)
					{
						ViewBag.VehicleHealth = "Alert";
					}else{
						ViewBag.VehicleHealth = "OK";
					}
					ViewBag.VehicleBattery = vehicle.battery;
					//GeoPointDTO point = new GeoPointDTO();
					//point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(vehicle.location.longitude, vehicle.location.latitude);
					ViewBag.VehicleLocationLongtitude = vehicle.location.longitude;
					ViewBag.VehicleLocationLatitude = vehicle.location.latitude;
				}
				else
				{
					return RedirectToAction("Index", "Error");
				}
		    }
	    //API 交互 chenyangwen 20140328
            return View();
        }

        [LogFilter]
        //chenyangwen 20140611 #1357
        [SessionTimeoutTimerFilter]
        public JsonResult GetCurrentVehicleInfo(long vehicleID)
        {
            try
            {
                VehicleInfo vehicleInfo = null;
                VehicleDBInterface vehicleDbInterface = new VehicleDBInterface();
                Models.Vehicle vehicleDB = vehicleDbInterface.GetVehicleByID(vehicleID);
                FleetInfoFetcher fleetFetcher = new FleetInfoFetcher();
                if (null == Session["TimeZone"])
                {
                    Session["TimeZone"] = 0;
                }
                var fleetInfo = fleetFetcher.GetInfoFromCache(Session["companyID"].ToString(), -1, Int32.Parse(Session["TimeZone"].ToString()),false);
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
        private VehicleInfo ConvertVehicle(Models.Vehicle vehicle)
        {
            return null;
        }

        [LogFilter]
        public JsonResult GetGeofenceAlertsOfOnePage(long vehicleId, int pageNum, string month)
        {
            Dictionary<string,object> alerts = new Dictionary<string,object>();
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
        [LogFilter]
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

        [LogFilter]
        public JsonResult GetVehicleAlertsOfOnePage(long vehicleId, int pageNum, string month)
        {
            Dictionary<string, object> alerts = new Dictionary<string, object>();
            AlertFetcher alertIF = new AlertFetcher();
            if (null == Session["TimeZone"])
            {
                Session["TimeZone"] = 0;
            }
            List<VehicleAlert> vehicleAlerts = alertIF.GetVehicleAlertInfo(Session["companyID"].ToString(), vehicleId, month, pageNum, Int32.Parse(Session["TimeZone"].ToString()));
            if (null != vehicleAlerts)
            {
                vehicleAlerts.Sort((y, x) => x.alertTime.CompareTo(y.alertTime));/*fengpan 20140324 #775*/
            }
            alerts.Add("alerts", vehicleAlerts);
            alerts.Add("alertsCount", GetVehicleAlertsPageCount(vehicleId, month));
            return Json(alerts, JsonRequestBehavior.AllowGet);
        }
        [LogFilter]
        public int GetVehicleAlertsPageCount(long vehicleId, string month)
        {
            if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
            {
                return 1;
            }
            else
            {
                AlertFetcher alertFetcher = new AlertFetcher();
                return alertFetcher.GetVehicleAlertNum(Session["companyID"].ToString(), vehicleId, alertFetcher.GetDateTimeMonthFirstDay(DateTime.Parse(month)), alertFetcher.GetDateTimeMonthLastDay(DateTime.Parse(month)), Int32.Parse(Session["TimeZone"].ToString()));
            }
        }
        /******** fengpan 20140305**********/


        /*********mabiao trip log **********/
        [LogFilter]
        public JsonResult GetTripLog(String vehicle, String type,String starttime,String endtime)
        {
            long VehicleID = int.Parse(vehicle);

            TripLog lasttrip = new TripLog();
            lasttrip.type = type;
            //lasttrip.StartTime = starttime;
            //lasttrip.EndTime = endtime;

            TripLogFetcher tripInterface = new TripLogFetcher();
            List<TripLog> triplogs = new List<TripLog>();
            List<TripLog> triplogback = new List<TripLog>();
            triplogs = tripInterface.GetTripLogs(VehicleID, lasttrip);

            int height = 0;

            foreach (TripLog triplogTemp in triplogs)
            {
                height += TripLogAddHeight(triplogTemp);

                if (height > 710 && triplogback.Count > 0)    //第一个节点特别长 也得显示出来 防止出现空白
                {
                    break;
                }

                triplogback.Add(triplogTemp);

            }
            return Json(triplogback, JsonRequestBehavior.AllowGet);
        }

        //mabiao 一个节点需要增加的高度
        //20140312
        [LogFilter]
        public int TripLogAddHeight(TripLog triplog)
        {
            int addheight = 0;
            int length = 0;
            switch (triplog.type)
            {
                case "Driving": addheight = 70;
                    break;
                case "Final": addheight = 30;
                    break;
                case "Day": addheight = 55;
                    break;
                case "Normal":
                    length = 0;
                    addheight = 100;
                    if (0 == triplog.healthStatus)
                    {
                        length++;
                    }
                    length += triplog.alerts.Count;
                    length += triplog.geofenceInfo.Count;
                    if (length > 4)
                    {
                        addheight += (length - 4) * 16;
                    }
                    break;
                case "Trailer": addheight = 100;
                    break;
            }
            return addheight;
        }

        [LogFilter]
        public JsonResult GetTripLogList(String Vehicle, String type, String starttime, String endtime)
        {
            long VehicleID = int.Parse(Vehicle);
            TripLog lasttrip = new TripLog();
            lasttrip.type = type;
            /* lasttrip.StartTime = starttime;
            lasttrip.EndTime = endtime;*/

            TripLogFetcher triplogInterface = new TripLogFetcher();
            List<TripLog> triploglist = new List<TripLog>();
            triploglist = triplogInterface.GetTripLogLists(VehicleID, lasttrip);
            return Json(triploglist, JsonRequestBehavior.AllowGet);
        }

        [LogFilter]
        public JsonResult GetTripLogMore(String vehicle, String endtime)
        {

            TripLogFetcher TripLogApi = new TripLogFetcher();


            TripLogDetail TripLogDetail = new TripLogDetail();
            TripLogDetail = TripLogApi.GetTripLogDetail(vehicle, endtime);
            return Json(TripLogDetail, JsonRequestBehavior.AllowGet);
        }
        [LogFilter]
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

        [LogFilter]
        //detail 画面中 列表风格、地图风格的triplog都取这个接口的数据
        public JsonResult GetTrips(string vehicleID, string nearTime, string oldTime, string timeZone)
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

        [LogFilter]
        public JsonResult GetTripDetail(string tripGUID,string timeZone)
        {
            try
            {
                TripLogFetcher tripFetcher = new TripLogFetcher();
                TripLogDetail tripDetail = tripFetcher.GetTripDetail(tripGUID,int.Parse(timeZone));
                return Json(tripDetail, JsonRequestBehavior.AllowGet); 
            }
            catch
            {
                return null;
            }
        }

        [LogFilter]
        public Boolean ScanAndGetDTCByApi(string tripGUID)
        {
            return true;
        }

        [LogFilter]
        public Boolean GetStatusOfDTC(string tripGUID)
        {
            return true;
        }

        [LogFilter]
        public JsonResult GetCodeOfDTC(string tripGUID)
        {
            List<string> codes = new List<string>();
            codes.Add("P0100");
            codes.Add("P0105");
            return Json(codes, JsonRequestBehavior.AllowGet); 
        }

        [LogFilter]
        public void DTCThreadStart(long vehicleID)
        {
            DTCFetcher DTCInterface = new DTCFetcher();
            DTCInterface.vehicleID = vehicleID;
            Thread threadCustomer = new Thread(() => DTCInterface.VehicleDTCScan());
            threadCustomer.Start();
            DebugLog.Debug(" DTCInterface.VehicleDTCScan Start");
             
        }

        [LogFilter]
        public JsonResult GetDtcFromDB(long vehicleID)
        {
            DiagnosticDBInterface dtcInterface = new DiagnosticDBInterface();
            List<Diagnostic> result = dtcInterface.GetDiagnosticByVehicleId(vehicleID);
            if (null != result)
            {
                TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                foreach (Diagnostic resultTemp in result)
                {
                    if (null != resultTemp.lastReadDate)
                    {
                        resultTemp.lastReadDate = resultTemp.lastReadDate.Add(backTimeZone).ToUniversalTime();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [LogFilter]
        public bool ClearDeviceDiagnostic(long vehicleID)
        {
            DTCFetcher DTCInterface = new DTCFetcher();
            DTCInterface.vehicleID = vehicleID;
            return DTCInterface.ClearDeviceDiagnostic();
        }

        [LogFilter]
        public bool ClearServerDiagnostic(long vehicleID)
        {
            DTCFetcher DTCInterface = new DTCFetcher();
            DTCInterface.vehicleID = vehicleID;
            return DTCInterface.ClearServerDiagnostic();
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
    }
}
