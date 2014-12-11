using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Filters;
using FleetManageToolWebRole.DB_interface;
//caoyandong-operating
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Util;
using FleetManageTool.Models.page;
using FleetManageToolWebRole.Models.page;
using Newtonsoft.Json;
using FleetManageToolWebRole.Models.Constant;
using System.Threading;
using System.Globalization;
using Resource.String;
using System;
using System.Text;
using BaiduWGSPoint;

namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class ExportController : Controller
    {
        public ActionResult HealthReportView()
        {
            try
            {
                DebugLog.Debug("SettingController HealthReportView Start");
                String timezone = Request["timezone"];
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;

                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                //get current date
                DateTime nowDate = DateTime.Today.AddDays(1);
                int temp = 0 - DateTime.Today.Day;
                DateTime firstDate = nowDate.AddDays(temp);
                
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<HealthReport> healthList = getWarnInfoInMonth_fakedata();
                    String healthJson = JsonConvert.SerializeObject(healthList);
                    ViewBag.initDate = nowDate;
                    ViewBag.healthList = healthJson;
                }
                else
                {
                    List<HealthReport> healthList = getWarnInfoInMonth(firstDate, nowDate, timezone);
                    String healthJson = JsonConvert.SerializeObject(healthList);
                    ViewBag.initDate = nowDate;
                    ViewBag.healthList = healthJson;
                }
                DebugLog.Debug("SettingController HealthReportView End");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug("异常信息：" + e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        public ActionResult TripReportView()
        {
            try
            {
                DebugLog.Debug("SettingController TripReportView Start");
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;
                String timezone = Request["timezone"];
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                //get current date
                DateTime nowDate = DateTime.Today.AddDays(1);
                int temp = 0 - DateTime.Today.Day;
                DateTime firstDate = nowDate.AddDays(temp);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<TripReport> tripList = getTripInfoInMonth_fakedata();
                    String tripJson = JsonConvert.SerializeObject(tripList);
                    ViewBag.initDate = nowDate;
                    ViewBag.tripList = tripJson;

                }
                else
                {
                    List<TripReport> tripList = getTripInfoInMonth(firstDate, nowDate, timezone);
                    String tripJson = JsonConvert.SerializeObject(tripList);
                    ViewBag.initDate = nowDate;
                    ViewBag.tripList = tripJson;
                }
                DebugLog.Debug("SettingController TripReportView Start");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.StackTrace);
            }
        }

        // GET: /Export/HealthReport
        
        public JsonResult HealthMonthReport(string startTime, string endTime, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController HealthMonthReport Start");
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<HealthReport> MonthList = getWarnInfoInMonth_fakedata();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController HealthMonthReport Start");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> dic = getHealthInfoInMonthForPage(startdate, enddate.AddDays(1), intkey, timezone);
                    DebugLog.Debug("SettingController HealthMonthReport Start");
                    return Json(dic, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.StackTrace);
            }
        }
        // GET: /Export/UtilizationReport
        
        public JsonResult UtilizationMonthReport(string startTime, string endTime, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController UtilizationMonthReport Start");
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<UtilizationReport> MonthList = getUtilizationInMonth_fakedata();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController UtilizationMonthReport Start");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //get vehicles data by date
                    Dictionary<string, object> dic = getUtilizationInfoInMonthForPage(startdate, enddate.AddDays(1), intkey, timezone);
                    DebugLog.Debug("SettingController UtilizationMonthReport Start");
                    return Json(dic, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.StackTrace);
            }
        }
        // Export Health Report.
        
        public void ExportHealthReport()
        {
            try
            {
                DebugLog.Debug("SettingController ExportHealthReport Start");
                Encoding codestyle = null;
                string[] lang = Request.UserLanguages;
                if (lang[0].ToUpper().Equals("ZH-CN"))
                {
                    codestyle = Encoding.GetEncoding("gb2312");
                }
                else
                {
                    codestyle = Encoding.UTF8;
                }
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                String startTime = Request["startTime"];
                String endTime = Request["endTime"];
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);

                String timezone = Request["timezone"];
                List<HealthReport> MonthList = new List<HealthReport>();
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    MonthList = getWarnInfoInMonth_fakedata_CSV();
                }
                else
                {
                    MonthList = getWarnInfoInMonth_CSV(startdate, enddate.AddDays(1), timezone);
                }
                //output
                String now = Request["date"];//DateTime.Now.ToString("yyyyMMddHHmmss");
                String filename = "HealthReport" + now + ".csv";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, codestyle));
                //write file stream
                System.IO.MemoryStream output = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, codestyle);
                writer.Write(TripConstant.ExportGroupName + ",");
                writer.Write(TripConstant.ExportVehicleName + ",");
                writer.Write(TripConstant.ExportVehicleLicence + ",");
                writer.Write(TripConstant.ExportAlerttime + ",");
                writer.Write(TripConstant.ExportDetailInfo);
                writer.WriteLine();
                writer.Flush();
                int length = (int)output.Position;
                output.Position = 0;
                byte[] bytes_title = new byte[output.Length];
                output.Read(bytes_title, 0, length);
                Response.BinaryWrite(bytes_title);
                Response.Flush();
                output.Position = 0;
                foreach (HealthReport i in MonthList)
                {
                    writer.Write('"' + i.group.Trim() + '"' + ",");
                    writer.Write('"' + i.name.Trim() + '"' + ",");
                    writer.Write('"' + i.licence.Trim() + '"' + ",");
                    writer.Write('"' + i.warnningtime.Trim() + '"' + ",");
                    writer.Write('"' + i.warninginfo.Trim() + '"');
                    writer.WriteLine();
                    writer.Flush();
                    length = (int)output.Position;
                    output.Position = 0;
                    byte[] bytes = new byte[length];
                    //bytes = output.GetBuffer();
                    output.Read(bytes, 0, length);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    output.Position = 0;
                }
                output.Close();
                writer.Close();
                Response.End();
                //caoyandong-Operating
                OperatorLog.log(OperateType.REPORT, "HealthReport", Session["companyID"].ToString());
                DebugLog.Debug("SettingController ExportHealthReport End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        //Export Fuel Report.
        
        public void ExportFuelReport()
        {
            try
            {
                DebugLog.Debug("SettingController ExportFuelReport Start");
                Encoding codestyle = null;
                string[] lang = Request.UserLanguages;
                if (lang[0].ToUpper().Equals("ZH-CN"))
                {
                    codestyle = Encoding.GetEncoding("gb2312");
                }
                else
                {
                    codestyle = Encoding.UTF8;
                }
                //chenyangwen 2014/3/8
                String startTime = Request["startTime"];
                String endTime = Request["endTime"];
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                String timezoneStr = Request["timezone"];
                int timezone = int.Parse(timezoneStr);
                //get vehicles data by date
                List<FuelReport> MonthList = new List<FuelReport>();
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    MonthList = getFuelInfoInMonth_CSV();
                }
                else
                {
                    MonthList = getFuelInfoInMonth_CSVRealData(startdate, enddate);
                }
                //output
                String now = Request["date"];//DateTime.Now.ToString("yyyyMMddHHmmss");
                String filename = "FuelReport" + now + ".csv";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, codestyle));

                //write file stream
                System.IO.MemoryStream output = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, codestyle);
                writer.Write(TripConstant.ExportGroupName + ",");
                writer.Write(TripConstant.ExportVehicleName + ",");
                writer.Write(TripConstant.ExportVehicleLicence + ",");
                writer.Write(TripConstant.ExportDate + ",");
                writer.Write(TripConstant.ExportDrivingtime + ",");
                writer.Write(TripConstant.ExportDistance + ",");
                writer.Write(TripConstant.ExportGallonpermiles + ",");
                writer.Write(TripConstant.ExportGallons);
                writer.WriteLine();
                writer.Flush();
                int length = (int)output.Position;
                output.Position = 0;
                byte[] bytes_title = new byte[output.Length];
                output.Read(bytes_title, 0, length);
                Response.BinaryWrite(bytes_title);
                Response.Flush();
                output.Position = 0;
                foreach (FuelReport i in MonthList)
                {
                    writer.Write('"' + i.group.Trim() + '"' + ",");
                    writer.Write('"' + i.name.Trim() + '"' + ",");
                    writer.Write('"' + i.licence.Trim() + '"' + ",");
                    writer.Write('"' + i.date.Trim() + '"' + ",");
                    writer.Write('"' + i.driveTime.Trim() + '"' + ",");
                    writer.Write('"' + i.miles.Trim() + '"' + ",");
                    writer.Write('"' + i.gallonsPerMile.Trim() + '"' + ",");
                    writer.Write('"' + i.gallonsAll.Trim() + '"');
                    writer.WriteLine();
                    writer.Flush();
                    length = (int)output.Position;
                    output.Position = 0;
                    byte[] bytes = new byte[length];
                    output.Read(bytes, 0, length);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    output.Position = 0;
                }
                output.Close();
                writer.Close();
                Response.End();
                //caoyandong-Operating
                OperatorLog.log(OperateType.REPORT, "FuelReport", Session["companyID"].ToString());
                DebugLog.Debug("SettingController ExportFuelReport End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // Export Utilization Report.
        
        public void ExportUtilizationReport()
        {
            try
            {
                DebugLog.Debug("SettingController ExportUtilizationReport Start");
                Encoding codestyle = null;
                string[] lang = Request.UserLanguages;
                if (lang[0].ToUpper().Equals("ZH-CN"))
                {
                    codestyle = Encoding.GetEncoding("gb2312");
                }
                else
                {
                    codestyle = Encoding.UTF8;
                }
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                String starttime = Request["startTime"];
                String endtime = Request["endTime"];
                DateTime startDate = DateTime.Parse(starttime);
                DateTime endDate = DateTime.Parse(endtime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                String timezone = Request["timezone"];
                List<TripReport> MonthList = new List<TripReport>();
                List<UtilizationReport> MonthList_fake = new List<UtilizationReport>();
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    MonthList_fake = getUtilizationInMonth_fakedata_CSV();
                }
                else
                {
                    MonthList = getUtilizationInMonth_CSV(startdate, enddate.AddDays(1), timezone);
                }
                //output
                String now = Request["date"];//DateTime.Now.ToString("yyyyMMddHHmmss");
                String filename = "UtilizationReport" + now + ".csv";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, codestyle));
                //write file stream
                System.IO.MemoryStream output = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, codestyle);
                writer.Write(TripConstant.ExportGroupName + ",");
                writer.Write(TripConstant.ExportVehicleName + ",");
                writer.Write(TripConstant.ExportVehicleLicence + ",");
                writer.Write(TripConstant.ExportDate + ",");
                writer.Write(TripConstant.ExportEngineontime + ",");
                writer.Write(TripConstant.ExportIdletime + ",");
                writer.Write(TripConstant.ExportUtilization);
                writer.WriteLine();
                writer.Flush();
                int length = (int)output.Position;
                output.Position = 0;
                byte[] bytes_title = new byte[output.Length];
                output.Read(bytes_title, 0, length);
                Response.BinaryWrite(bytes_title);
                Response.Flush();
                output.Position = 0;
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    foreach (UtilizationReport i in MonthList_fake)
                    {
                        writer.Write('"' + i.group.Trim() + '"' + ",");
                        writer.Write('"' + i.name.Trim() + '"' + ",");
                        writer.Write('"' + i.licence.Trim() + '"' + ",");
                        writer.Write('"' + i.date.Trim() + '"' + ",");
                        writer.Write('"' + i.engineontime.Trim() + '"' + ",");
                        writer.Write('"' + i.tripidle.Trim() + '"' + ",");
                        writer.Write('"' + i.utilization.Trim() + '"');
                        writer.WriteLine();
                        writer.Flush();
                        length = (int)output.Position;
                        output.Position = 0;
                        byte[] bytes = new byte[length];
                        //bytes = output.GetBuffer();
                        output.Read(bytes, 0, length);
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                        output.Position = 0;
                    }
                }
                else
                {
                    foreach (TripReport i in MonthList)
                    {
                        writer.Write('"' + i.group.Trim() + '"' + ",");
                        writer.Write('"' + i.name.Trim() + '"' + ",");
                        writer.Write('"' + i.licence.Trim() + '"' + ",");
                        writer.Write('"' + i.date.Trim() + '"' + ",");
                        writer.Write('"' + i.drivingTime.Trim() + '"' + ",");
                        writer.Write('"' + i.idleTime.Trim() + '"' + ",");
                        writer.Write('"' + i.utilization.Trim() + '"');
                        writer.WriteLine();
                        writer.Flush();
                        length = (int)output.Position;
                        output.Position = 0;
                        byte[] bytes = new byte[length];
                        output.Read(bytes, 0, length);
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                        output.Position = 0;
                    }
                }
                output.Close();
                writer.Close();
                Response.End();
                //caoyandong-Operating
                OperatorLog.log(OperateType.REPORT, "UtilizationReport", Session["companyID"].ToString());
                //return File(output, "text/comma-separated-values", filename);
                DebugLog.Debug("SettingController ExportUtilizationReport End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<HealthReport> getWarnInfoInMonth(DateTime startdate, DateTime enddate, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getWarnInfoInMonth Start");
                List<HealthReport> healthList = new List<HealthReport>();
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                else
                {
                    String companyid = Session["companyID"].ToString();
                    AppendVehicleHealthInfo(healthList, companyid, startdate, enddate, timezone);
                    DebugLog.Debug("SettingController getWarnInfoInMonth End");
                    return healthList;
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }

        }
        
        private List<HealthReport> getWarnInfoInMonth_CSV(DateTime startdate, DateTime enddate, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getWarnInfoInMonth_CSV Start");
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                else
                {
                    List<HealthReport> healthList = new List<HealthReport>();
                    String companyid = Session["companyID"].ToString();
                    VehicleWarnningInfoForCSV(healthList, companyid, startdate, enddate, timezone);
                    DebugLog.Debug("SettingController getWarnInfoInMonth_CSV End");
                    return healthList;
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // get data in Month.
        
        private List<TripReport> getUtilizationInMonth(DateTime startdate, DateTime enddate, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getUtilizationInMonth Start");
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                List<TripReport> utilizationList = new List<TripReport>();
                String companyid = Session["companyID"].ToString();
                AppendVehicleUtilizationInfo(utilizationList, companyid, startdate, enddate, timezone);
                DebugLog.Debug("SettingController getUtilizationInMonth End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }


        }
        
        private List<TripReport> getUtilizationInMonth_CSV(DateTime startdate, DateTime enddate, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getUtilizationInMonth_CSV Start");
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                String companyid = Session["companyID"].ToString();
                List<TripReport> utilizationList = new List<TripReport>();
                VehicleUtilizationInfoForCSV(utilizationList, companyid, startdate, enddate, timezone);
                DebugLog.Debug("SettingController getUtilizationInMonth_CSV End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }

        }
        
        public ActionResult FuelReportView()
        {
            try
            {
                DebugLog.Debug("SettingController FuelReportView Start");
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;

                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                DateTime nowDate = DateTime.Today.AddDays(1);
                int temp = 0 - DateTime.Today.Day;
                DateTime firstDate = nowDate.AddDays(temp);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<FuelReport> healthList = getFuelInfoInMonth();
                    String healthJson = JsonConvert.SerializeObject(healthList);
                    ViewBag.initDate = nowDate;
                    ViewBag.healthList = healthJson;
                }
                else
                {
                    List<FuelReport> healthList = getFuelInfoInMonth_realdata(firstDate, nowDate);
                    String healthJson = JsonConvert.SerializeObject(healthList);
                    ViewBag.initDate = nowDate;
                    ViewBag.healthList = healthJson;
                }
                DebugLog.Debug("SettingController FuelReportView End");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<FuelReport> getFuelInfoInMonth_realdata(DateTime startdate, DateTime enddate)
        {
            List<FuelReport> healthList = new List<FuelReport>();
            return healthList;
        }
        // get data in Month.
        
        private List<FuelReport> getFuelInfoInMonth()
        {
            try
            {
                DebugLog.Debug("SettingController getFuelInfoInMonth Start");
                List<FuelReport> healthList = new List<FuelReport>();
                String companyid = (String)Session["companyID"];
                //get date from db
                for (int i = 0; i < 3; i++)
                {
                    FuelReport temp = new FuelReport();
                    temp.id = i + 1;
                    temp.name = "奔驰 " + temp.id;
                    temp.date = "2014-03-22";
                    temp.licence = "辽A1111" + i;
                    temp.driveTime = "02: 30: 00";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                {
                    FuelReport temp = new FuelReport();
                    temp.id = 4;
                    temp.name = "小计 ";
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.driveTime = "07: 30: 00";
                    temp.miles = "450.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "18.0";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                for (int i = 0; i < 2; i++)
                {
                    FuelReport temp = new FuelReport();
                    temp.id = i + 5;
                    temp.name = "宝马 " + temp.id;
                    temp.date = "2014-03-23";
                    temp.licence = "辽A1112" + i;
                    temp.driveTime = "02: 30: 00";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }

                {
                    FuelReport temp = new FuelReport();
                    temp.id = 7;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.driveTime = "05: 00: 00";
                    temp.miles = "300.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "12.0";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    FuelReport temp = new FuelReport();
                    temp.id = 8;
                    temp.name = "大众 1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11131";
                    temp.driveTime = "02: 30: 00";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = TripConstant.ExportNogroup;
                    healthList.Add(temp);
                }
                DebugLog.Debug("SettingController getFuelInfoInMonth End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }

        }
        
        private List<FuelReport> getFuelInfoInMonth_CSVRealData(DateTime startdate, DateTime enddate)
        {
            List<FuelReport> healthList = new List<FuelReport>();
            return healthList;
        }
        
        private List<FuelReport> getFuelInfoInMonth_CSV()
        {
            try
            {
                DebugLog.Debug("SettingController getFuelInfoInMonth_CSV Start");
                List<FuelReport> healthList = new List<FuelReport>();
                String companyid = (String)Session["companyID"];
                //get date from db
                for (int i = 0; i < 3; i++)
                {
                    FuelReport temp = new FuelReport();
                    temp.id = i + 1;
                    temp.name = "奔驰 " + temp.id;
                    temp.date = "2014-03-22";
                    temp.licence = "辽A1111" + i;
                    temp.driveTime = "2.5";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                for (int i = 0; i < 2; i++)
                {
                    FuelReport temp = new FuelReport();
                    temp.id = i + 4;
                    temp.name = "宝马 " + temp.id;
                    temp.date = "2014-03-23";
                    temp.licence = "辽A1112" + i;
                    temp.driveTime = "2.5";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    FuelReport temp = new FuelReport();
                    temp.id = 6;
                    temp.name = "大众 1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11131";
                    temp.driveTime = "2.5";
                    temp.miles = "150.0";
                    temp.gallonsPerMile = "4.0";
                    temp.gallonsAll = "6.0";
                    temp.group = TripConstant.ExportNogroup;
                    healthList.Add(temp);
                }
                DebugLog.Debug("SettingController getFuelInfoInMonth_CSV End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        //get fuel data in Month
        
        public JsonResult FuelMonthReport(string startTime, string endTime, string intkey)
        {
            try
            {
                DebugLog.Debug("SettingController FuelMonthReport Start");
                DateTime startDate = DateTime.Parse(startTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime endDate = DateTime.Parse(endTime);
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime enddate = DateTime.Parse(endStr);
                //get vehicles data by date

                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<FuelReport> MonthList = getFuelInfoInMonth();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController FuelMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<FuelReport> MonthList = getFuelInfoInMonth_realdata(startdate, enddate);
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController FuelMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // get trip data in Month.
        
        private List<TripReport> getTripInfoInMonth(DateTime startdate, DateTime enddate, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonth Start");
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                String companyid = Session["companyID"].ToString();
                List<TripReport> tripList = new List<TripReport>();
                AppendVehicleTripInfo(tripList, companyid, startdate, enddate, timezone);
                DebugLog.Debug("SettingController getTripInfoInMonth End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private Dictionary<string, object> getTripInfoInMonthForPage(DateTime startdate, DateTime enddate, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonthForPage Start");
                List<TripReport> tripList = new List<TripReport>();
                tripList = getTripInfoInMonth(startdate, enddate, timezone);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                int PageNum = 0;
                int CurrentPageNum = int.Parse(intkey);
                if (tripList.Count() % Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) == 0)
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]);
                }
                else
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) + 1;
                }
                dic.Add("pagecount", PageNum);
                List<TripReport> tripListForPage = new List<TripReport>();
                if (CurrentPageNum <= PageNum)
                {
                    if (CurrentPageNum != PageNum)
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < CurrentPageNum * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); ++i)
                        {
                            tripListForPage.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < tripList.Count(); ++i)
                        {
                            tripListForPage.Add(tripList[i]);
                        }
                    }
                }
                dic.Add("dataList", tripListForPage);
                DebugLog.Debug("SettingController getTripInfoInMonthForPage End");
                return dic;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<TripReport> getTripInfoInMonth_CSV(DateTime startdate, DateTime enddate, int itimezone)
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonth_CSV Start");
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                String companyid = Session["companyID"].ToString();
                List<TripReport> tripList = new List<TripReport>();
                VehicleTripInfoForCSV(tripList, companyid, startdate, enddate, itimezone);
                DebugLog.Debug("SettingController getTripInfoInMonth_CSV End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public JsonResult getTripInfo_DB(string startTime, string endTime, int itimezone)
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfo_DB Start");
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                String companyid = Session["companyID"].ToString();
                //List<TripReport> tripList = new List<TripReport>();
                int totalnum = 0;
                totalnum = GetTotalTrips(companyid, startdate, endDate.AddDays(1), itimezone);
                //VehicleTripInfoForCSV(tripList, companyid, startdate, endDate, itimezone);
                Dictionary<string, object> infonumber = new Dictionary<string, object>();
                if (totalnum % 1000 == 0)
                {
                    infonumber.Add("totalnumber", totalnum / 1000);
                }
                else
                {
                    infonumber.Add("totalnumber", totalnum / 1000 + 1);
                }
                DebugLog.Debug("result " + totalnum);
                DebugLog.Debug("Controler getTripInfo_DB end");
                DebugLog.Debug("SettingController getTripInfo_DB End");
                return Json(infonumber, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public JsonResult getTripInfoInMonth_DB(string startTime, string endTime, int itimezone, int itotalnumber, int icurrentnumber)
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonth_DB Start");
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                if (null == startdate || null == startdate)
                {
                    return null;
                }
                String companyid = Session["companyID"].ToString();
                List<TripReport> tripList = new List<TripReport>();
                VehicleTripInfoForAddress(tripList, companyid, startdate, enddate.AddDays(1), itimezone);
                List<TripReport> tripListOnce = new List<TripReport>();
                if (icurrentnumber <= itotalnumber)
                {
                    if (icurrentnumber != itotalnumber)
                    {
                        for (int i = (icurrentnumber - 1) * 1000; i < icurrentnumber * 1000; ++i)
                        {
                            tripListOnce.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (icurrentnumber - 1) * 1000; i < tripList.Count(); ++i)
                        {
                            tripListOnce.Add(tripList[i]);
                        }
                    }
                }
                DebugLog.Debug("result " + tripList.Count());
                DebugLog.Debug("Controler getTripInfoInMonth_DB End");
                return Json(tripListOnce, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // Search trip data in Month.
        // GET: /Export/TripMonthReport
        
        public JsonResult TripMonthReport(string startTime, string endTime, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController TripMonthReport Start");
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime start = DateTime.Parse(startStr);
                DateTime end = DateTime.Parse(endStr);

                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<TripReport> MonthList = getTripInfoInMonth_fakedata();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController TripMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> dic = getTripInfoInMonthForPage(start, end.AddDays(1), intkey, timezone);
                    DebugLog.Debug("SettingController TripMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // Export Trip Report.
        
        public void ExportTripReport()
        {
            try
            {
                DebugLog.Debug("SettingController ExportTripReport Start");
                Encoding codestyle = null;
                string[] lang = Request.UserLanguages;
                if (lang[0].ToUpper().Equals("ZH-CN"))
                {
                    codestyle = Encoding.GetEncoding("gb2312");
                }
                else
                {
                    codestyle = Encoding.UTF8;
                }
                //chenyangwen 2014/3/8
                String startTime = Request["startTime"];
                String endTime = Request["endTime"];
                DateTime startDate = DateTime.Parse(startTime);
                DateTime endDate = DateTime.Parse(endTime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);

                List<TripReport> MonthList = new List<TripReport>();
                String timezoneStr = Request["timezone"];
                int timezone = int.Parse(timezoneStr);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    MonthList = getTripInfoInMonth_fakedata_CSV();

                }
                else
                {
                    MonthList = getTripInfoInMonth_CSV(startdate, enddate.AddDays(1), timezone);
                }
                //output
                String now = Request["date"];//DateTime.Now.ToString("yyyyMMddHHmmss");
                String filename = "TripReport" + now + ".csv";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, codestyle));
                //write file stream
                System.IO.MemoryStream output = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, codestyle);
                writer.Write(TripConstant.ExportGroupName + ",");
                writer.Write(TripConstant.ExportVehicleName + ",");
                writer.Write(TripConstant.ExportVehicleLicence + ",");
                writer.Write(TripConstant.ExportLeavetime + ",");
                writer.Write(ihpleD_String_cn.export_leavelocation_province + ",");
                writer.Write(ihpleD_String_cn.export_leavelocation_city + ",");
                writer.Write(ihpleD_String_cn.export_leavelocation_district + ",");
                writer.Write(ihpleD_String_cn.export_leavelocation_street + ",");
                writer.Write(TripConstant.ExportArrivaltime + ",");
                writer.Write(ihpleD_String_cn.export_arrivallocation_province + ",");
                writer.Write(ihpleD_String_cn.export_arrivallocation_city + ",");
                writer.Write(ihpleD_String_cn.export_arrivallocation_district + ",");
                writer.Write(ihpleD_String_cn.export_arrivallocation_street + ",");
                writer.Write(TripConstant.ExportTripdistance + ",");
                writer.Write(TripConstant.ExportTriptime + ",");
                writer.Write(TripConstant.ExportTripidletime + ",");
                writer.Write(TripConstant.ExportTripidlerate + ",");
                writer.Write(TripConstant.ExportAvespeed);
                writer.WriteLine();
                writer.Flush();
                int length = (int)output.Position;
                output.Position = 0;
                byte[] bytes_title = new byte[output.Length];

                //bytes_title = output.GetBuffer();
                output.Read(bytes_title, 0, length);
                Response.BinaryWrite(bytes_title);
                Response.Flush();
                output.Position = 0;
                foreach (TripReport i in MonthList)
                {
                    writer.Write('"' + i.group.Trim() + '"' + ",");
                    writer.Write('"' + i.name.Trim() + '"' + ",");
                    writer.Write('"' + i.licence.Trim() + '"' + ",");
                    writer.Write('"' + i.leavetime.Trim() + '"' + ",");
                    writer.Write('"' + i.leavelocation_province.Trim() + '"' + ",");
                    writer.Write('"' + i.leavelocation_city.Trim() + '"' + ",");
                    writer.Write('"' + i.leavelocation_district.Trim() + '"' + ",");
                    writer.Write('"' + i.leavelocation_street.Trim() + '"' + ",");
                    writer.Write('"' + i.arrivaltime.Trim() + '"' + ",");
                    writer.Write('"' + i.arrivallocation_province.Trim() + '"' + ",");
                    writer.Write('"' + i.arrivallocation_city.Trim() + '"' + ",");
                    writer.Write('"' + i.arrivallocation_district.Trim() + '"' + ",");
                    writer.Write('"' + i.arrivallocation_street.Trim() + '"' + ",");
                    writer.Write('"' + i.tripdistance.Trim() + '"' + ",");
                    writer.Write('"' + i.triptime.Trim() + '"' + ",");
                    writer.Write('"' + i.tripidletime.Trim() + '"' + ",");
                    writer.Write('"' + i.tripidlerate.Trim() + '"' + ",");
                    writer.Write('"' + i.tripavespeed.Trim() + '"');
                    writer.WriteLine();
                    writer.Flush();
                    length = (int)output.Position;
                    output.Position = 0;
                    byte[] bytes = new byte[length];
                    //bytes = output.GetBuffer();
                    output.Read(bytes, 0, length);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    output.Position = 0;
                }
                output.Close();
                writer.Close();
                Response.End();
                //caoyandong-Operating
                OperatorLog.log(OperateType.REPORT, "TripReport", Session["companyID"].ToString());
                DebugLog.Debug("SettingController ExportTripReport End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        
        public ActionResult ReportIndex()
        {
            try
            {
                DebugLog.Debug("SettingController ReportIndex Start");
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                DebugLog.Debug("SettingController ReportIndex End");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public ActionResult UtilizationReportView()
        {
            try
            {
                DebugLog.Debug("SettingController UtilizationReportView Start");
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;
                String timezone = Request["timezone"];
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                //get current date
                DateTime nowDate = DateTime.Today.AddDays(1);
                int temp = 0 - DateTime.Today.Day;
                DateTime firstDate = nowDate.AddDays(temp);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<UtilizationReport> utilizationList = getUtilizationInMonth_fakedata();
                    String utilizationJson = JsonConvert.SerializeObject(utilizationList);
                    ViewBag.initDate = nowDate;
                    ViewBag.utilizationList = utilizationJson;
                }
                else
                {
                    List<TripReport> utilizationList = getUtilizationInMonth(firstDate, nowDate, timezone);
                    String utilizationJson = JsonConvert.SerializeObject(utilizationList);
                    ViewBag.initDate = nowDate;
                    ViewBag.utilizationList = utilizationJson;
                }
                DebugLog.Debug("SettingController UtilizationReportView End");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public ActionResult HistoryReportView()
        {
            try
            {
                DebugLog.Debug("SettingController HistoryReportView Start");
                //mabiao js多语言
                Language language = new Language();
                string language_string = language.LanguageScript();
                ViewBag.Language = language_string;
                String timezone = Request["timezone"];
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                //get current date
                DateTime nowDate = DateTime.Today.AddDays(1);
                int temp = 0 - DateTime.Today.Day;
                DateTime firstDate = nowDate.AddDays(temp);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<HistoryReport> historyList = getHistoryInfoInMonth_fakedata();
                    String historyJson = JsonConvert.SerializeObject(historyList);
                    ViewBag.initDate = nowDate;
                    ViewBag.historyList = historyJson;
                }
                else
                {
                    List<HistoryReport> historyList = getHistoryInfoInMonth(firstDate, nowDate, timezone);
                    String historyJson = JsonConvert.SerializeObject(historyList);
                    ViewBag.initDate = nowDate;
                    ViewBag.historyList = historyJson;

                }
                DebugLog.Debug("SettingController HistoryReportView End");
                return View();
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<HistoryReport> getHistoryInfoInMonth(DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController getHistoryInfoInMonth Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                List<HistoryReport> historyList = new List<HistoryReport>();
                String companyid = Session["companyID"].ToString();
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehiclenoOBU = new List<Vehicle>();
                vehiclenoOBU = db.GetHistoryVehiclesByCompannyID(companyid);
                int itimezone = int.Parse(timezone);
                int vehicleOBUNum = 0;
                InitializationVehicleName(vehiclenoOBU);
                vehiclenoOBU.Sort((x, y) => x.name.CompareTo(y.name));
                if (vehiclenoOBU.Count() != 0)
                {
                    for (vehicleOBUNum = 0; vehicleOBUNum < vehiclenoOBU.Count(); ++vehicleOBUNum)
                    {
                        List<Trip> temp = new List<Trip>();
                        temp = db.GetTripByVehicleID(vehiclenoOBU[vehicleOBUNum].pkid);
                        if (temp.Count() == 0)
                        {
                            continue;
                        }
                        else
                        {
                            for (int i = 0; i < temp.Count(); ++i)
                            {
                                if (temp[i].startTime == null)
                                {
                                    temp[i].startTime = DateTime.Parse("0001-01-01 00:00:01");
                                }
                                if (temp[i].endtime == null)
                                {
                                    temp[i].endtime = DateTime.Parse("0001-01-01 00:00:01");
                                }
                            }
                            temp.Sort((x, y) => y.endtime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.endtime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                            temp.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                            if ((temp[0].startTime.Value.AddHours(itimezone) >= startdate && temp[0].startTime.Value.AddHours(itimezone) <= enddate)
                            || (temp[0].endtime.Value.AddHours(itimezone) >= startdate && temp[0].endtime.Value.AddHours(itimezone) <= enddate))
                            {
                                HistoryReport historylist = new HistoryReport();
                                if (vehiclenoOBU[vehicleOBUNum].name == null)
                                {
                                    historylist.name = "";
                                }
                                else
                                {
                                    historylist.name = vehiclenoOBU[vehicleOBUNum].name.Trim();
                                }
                                if (vehiclenoOBU[vehicleOBUNum].licence == null)
                                {
                                    historylist.licence = "";
                                }
                                else
                                {
                                    historylist.licence = vehiclenoOBU[vehicleOBUNum].licence.Trim();
                                }
                                List<string> hisStrArrval = new List<string>();
                                List<string> hisStrLeave = new List<string>();
                                if (temp[0].endlocation == null || temp[0].endlocation == "")
                                {
                                    historylist.arrivallocation_province = "";
                                    historylist.arrivallocation_city = "";
                                    historylist.arrivallocation_district = "";
                                    historylist.arrivallocation_street = "";
                                }
                                else
                                {
                                    SetAddressInfo(temp[0].endlocation, hisStrArrval);
                                    historylist.arrivallocation_province = hisStrArrval[0];
                                    historylist.arrivallocation_city = hisStrArrval[1];
                                    historylist.arrivallocation_district = hisStrArrval[2];
                                    historylist.arrivallocation_street = hisStrArrval[3];
                                }

                                if (temp[0].startlocation == null || temp[0].startlocation == "")
                                {
                                    historylist.leavelocation_province = "";
                                    historylist.leavelocation_city = "";
                                    historylist.leavelocation_district = "";
                                    historylist.leavelocation_street = "";
                                }
                                else
                                {
                                    SetAddressInfo(temp[0].startlocation, hisStrLeave);
                                    historylist.leavelocation_province = hisStrLeave[0];
                                    historylist.leavelocation_city = hisStrLeave[1];
                                    historylist.leavelocation_district = hisStrLeave[2];
                                    historylist.leavelocation_street = hisStrLeave[3];
                                }

                                if (temp[0].endtime != DateTime.Parse("0001-01-01 00:00:01"))
                                {
                                    historylist.time = ((DateTime)(temp[0].endtime)).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    historylist.location_province = historylist.arrivallocation_province;
                                    historylist.location_city = historylist.arrivallocation_city;
                                    historylist.location_district = historylist.arrivallocation_district;
                                    historylist.location_street = historylist.arrivallocation_street;
                                    historylist.location = temp[0].endlocation;
                                    if (null == temp[0].endlocationLng)
                                    {
                                        historylist.locationlng = 0;
                                    }
                                    else
                                    {
                                        historylist.locationlng = (double)temp[0].endlocationLng;
                                    }
                                    if (null == temp[0].endlocationLat)
                                    {
                                        historylist.locationlat = 0;
                                    }
                                    else
                                    {
                                        historylist.locationlat = (double)temp[0].endlocationLat;
                                    }
                                }
                                else
                                {
                                    historylist.time = ((DateTime)(temp[0].startTime)).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    historylist.location_province = historylist.leavelocation_province;
                                    historylist.location_city = historylist.leavelocation_city;
                                    historylist.location_district = historylist.leavelocation_district;
                                    historylist.location_street = historylist.leavelocation_street;
                                    historylist.location = temp[0].startlocation;
                                    if (null == temp[0].startlocationLng)
                                    {
                                        historylist.locationlng = 0;
                                    }
                                    else
                                    {
                                        historylist.locationlng = (double)temp[0].startlocationLng;
                                    }
                                    if (null == temp[0].startlocationLat)
                                    {
                                        historylist.locationlat = 0;
                                    }
                                    else
                                    {
                                        historylist.locationlat = (double)temp[0].startlocationLat;
                                    }
                                }
                                historylist.guid = temp[0].guid;
                                if (temp[0].endlocationLat == null)
                                {
                                    historylist.endlocctionlat = 0;
                                }
                                else
                                {
                                    historylist.endlocctionlat = (double)temp[0].endlocationLat;
                                }
                                if (temp[0].endlocationLat == null)
                                {
                                    historylist.endlocctionlng = 0;
                                }
                                else
                                {
                                    historylist.endlocctionlng = (double)temp[0].endlocationLng;
                                }

                                if (temp[0].startlocationLat == null)
                                {
                                    historylist.startlocctionlat = 0;
                                }
                                else
                                {
                                    historylist.startlocctionlat = (double)temp[0].startlocationLat;
                                }
                                if (temp[0].startlocationLng == null)
                                {
                                    historylist.startlocctionlng = 0;
                                }
                                else
                                {
                                    historylist.startlocctionlng = (double)temp[0].startlocationLng;
                                }
                                if (temp[0].isLastFlag == null)
                                {
                                    historylist.isLastFlag = 0;
                                }
                                else
                                {
                                    historylist.isLastFlag = (int)temp[0].isLastFlag;
                                }
                                if (temp[0].isFirstFlag == null)
                                {
                                    historylist.isFirstFlag = 0;
                                }
                                else
                                {
                                    historylist.isFirstFlag = (int)temp[0].isFirstFlag;
                                }
                                historyList.Add(historylist);
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController getHistoryInfoInMonth End");
                return historyList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public JsonResult getHistoryInfoInMonth_DB(string startTime, string endTime, string timezone)
        {
            DebugLog.Debug("SettingController getHistoryInfoInMonth_DB Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                String starttime = Request["startTime"];
                String endtime = Request["endTime"];
                DateTime startDate = DateTime.Parse(starttime);
                DateTime endDate = DateTime.Parse(endtime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                if (null == startdate || null == enddate)
                {
                    return null;
                }
                List<HistoryReport> historyList = new List<HistoryReport>();
                String companyid = Session["companyID"].ToString();
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehiclenoOBU = new List<Vehicle>();
                vehiclenoOBU = db.GetHistoryVehiclesByCompannyID(companyid);
                int itimezone = Int32.Parse(timezone);
                int vehicleOBUNum = 0;
                InitializationVehicleName(vehiclenoOBU);
                vehiclenoOBU.Sort((x, y) => x.name.CompareTo(y.name));
                if (vehiclenoOBU.Count() != 0)
                {
                    for (vehicleOBUNum = 0; vehicleOBUNum < vehiclenoOBU.Count(); ++vehicleOBUNum)
                    {
                        List<Trip> temp = new List<Trip>();
                        temp = db.GetTripByVehicleID(vehiclenoOBU[vehicleOBUNum].pkid);
                        if (temp.Count() == 0)
                        {
                            continue;
                        }
                        else
                        {
                            for (int i = 0; i < temp.Count(); ++i)
                            {
                                if (temp[i].startTime == null)
                                {
                                    temp[i].startTime = DateTime.Parse("0001-01-01 00:00:01");
                                }
                                if (temp[i].endtime == null)
                                {
                                    temp[i].endtime = DateTime.Parse("0001-01-01 00:00:01");
                                }
                            }
                            temp.Sort((x, y) => y.endtime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.endtime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                            temp.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                            if ((temp[0].startTime.Value.AddHours(itimezone) >= startdate && temp[0].startTime.Value.AddHours(itimezone) <= enddate)
                            || (temp[0].endtime.Value.AddHours(itimezone) >= startdate && temp[0].endtime.Value.AddHours(itimezone) <= enddate))
                            {
                                HistoryReport historylist = new HistoryReport();
                                if (vehiclenoOBU[vehicleOBUNum].name == null)
                                {
                                    historylist.name = "";
                                }
                                else
                                {
                                    historylist.name = vehiclenoOBU[vehicleOBUNum].name.Trim();
                                }
                                if (vehiclenoOBU[vehicleOBUNum].licence == null)
                                {
                                    historylist.licence = "";
                                }
                                else
                                {
                                    historylist.licence = vehiclenoOBU[vehicleOBUNum].licence.Trim();
                                }
                                List<string> hisStrArrval = new List<string>();
                                List<string> hisStrLeave = new List<string>();
                                if (temp[0].endlocation == null || temp[0].endlocation == "")
                                {
                                    historylist.arrivallocation_province = "";
                                    historylist.arrivallocation_city = "";
                                    historylist.arrivallocation_district = "";
                                    historylist.arrivallocation_street = "";
                                }
                                else
                                {
                                    SetAddressInfo(temp[0].endlocation, hisStrArrval);
                                    historylist.arrivallocation_province = hisStrArrval[0];
                                    historylist.arrivallocation_city = hisStrArrval[1];
                                    historylist.arrivallocation_district = hisStrArrval[2];
                                    historylist.arrivallocation_street = hisStrArrval[3];
                                }
                                if (temp[0].startlocation == null || temp[0].startlocation == "")
                                {
                                    historylist.leavelocation_province = "";
                                    historylist.leavelocation_city = "";
                                    historylist.leavelocation_district = "";
                                    historylist.leavelocation_street = "";
                                }
                                else
                                {
                                    SetAddressInfo(temp[0].startlocation, hisStrLeave);
                                    historylist.leavelocation_province = hisStrLeave[0];
                                    historylist.leavelocation_city = hisStrLeave[1];
                                    historylist.leavelocation_district = hisStrLeave[2];
                                    historylist.leavelocation_street = hisStrLeave[3];
                                }
                                if (temp[0].endtime != DateTime.Parse("0001-01-01 00:00:01"))
                                {
                                    historylist.time = ((DateTime)(temp[0].endtime)).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    historylist.location_province = historylist.arrivallocation_province;
                                    historylist.location_city = historylist.arrivallocation_city;
                                    historylist.location_district = historylist.arrivallocation_district;
                                    historylist.location_street = historylist.arrivallocation_street;
                                    historylist.location = temp[0].endlocation;
                                }
                                else
                                {
                                    historylist.time = ((DateTime)(temp[0].startTime)).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    historylist.location_province = historylist.leavelocation_province;
                                    historylist.location_city = historylist.leavelocation_city;
                                    historylist.location_district = historylist.leavelocation_district;
                                    historylist.location_street = historylist.leavelocation_street;
                                    historylist.location = temp[0].startlocation;
                                }
                                historylist.guid = temp[0].guid;
                                if (temp[0].endlocationLat == null)
                                {
                                    historylist.endlocctionlat = 0;
                                }
                                else
                                {
                                    historylist.endlocctionlat = (double)temp[0].endlocationLat;
                                }
                                if (temp[0].endlocationLat == null)
                                {
                                    historylist.endlocctionlng = 0;
                                }
                                else
                                {
                                    historylist.endlocctionlng = (double)temp[0].endlocationLng;
                                }

                                if (temp[0].startlocationLat == null)
                                {
                                    historylist.startlocctionlat = 0;
                                }
                                else
                                {
                                    historylist.startlocctionlat = (double)temp[0].startlocationLat;
                                }
                                if (temp[0].startlocationLng == null)
                                {
                                    historylist.startlocctionlng = 0;
                                }
                                else
                                {
                                    historylist.startlocctionlng = (double)temp[0].startlocationLng;
                                }
                                if (temp[0].isLastFlag == null)
                                {
                                    historylist.isLastFlag = 0;
                                }
                                else
                                {
                                    historylist.isLastFlag = (int)temp[0].isLastFlag;
                                }
                                if (temp[0].isFirstFlag == null)
                                {
                                    historylist.isFirstFlag = 0;
                                }
                                else
                                {
                                    historylist.isFirstFlag = (int)temp[0].isFirstFlag;
                                }

                                GeoPointDTO point = new GeoPointDTO();
                                point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(historylist.endlocctionlng, historylist.endlocctionlat);
                                historylist.endlocctionlng = point.longitude;
                                historylist.endlocctionlat = point.latitude;
                                point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(historylist.startlocctionlng, historylist.startlocctionlat);
                                historylist.startlocctionlng = point.longitude;
                                historylist.startlocctionlat = point.latitude;

                                historyList.Add(historylist);
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController getHistoryInfoInMonth_DB End");
                return Json(historyList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private Dictionary<string, object> getHistoryInfoInMonthForPage(DateTime startdate, DateTime enddate, string intkey, string timezone)
        {
            DebugLog.Debug("SettingController getHistoryInfoInMonthForPage Start");
            try
            {
                List<HistoryReport> tripList = new List<HistoryReport>();
                tripList = getHistoryInfoInMonth(startdate, enddate, timezone);

                Dictionary<string, object> dic = new Dictionary<string, object>();

                int PageNum = 0;
                int CurrentPageNum = int.Parse(intkey);
                if (tripList.Count() % Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) == 0)
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]);
                }
                else
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) + 1;
                }
                dic.Add("pagecount", PageNum);
                List<HistoryReport> tripListForPage = new List<HistoryReport>();
                if (CurrentPageNum <= PageNum)
                {
                    if (CurrentPageNum != PageNum)
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < CurrentPageNum * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); ++i)
                        {
                            tripListForPage.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (CurrentPageNum - 1) * 10; i < tripList.Count(); ++i)
                        {
                            tripListForPage.Add(tripList[i]);
                        }
                    }
                }
                dic.Add("dataList", tripListForPage);
                DebugLog.Debug("SettingController getHistoryInfoInMonthForPage Start");
                return dic;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public JsonResult HistoryMonthReport(string startTime, string endTime, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController HistoryMonthReport Start");
                String starttime = Request["startTime"];
                String endtime = Request["endTime"];
                DateTime startDate = DateTime.Parse(starttime);
                DateTime endDate = DateTime.Parse(endtime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    List<HistoryReport> MonthList = getHistoryInfoInMonth_fakedata();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("pagecount", 1);
                    dic.Add("dataList", MonthList);
                    DebugLog.Debug("SettingController HistoryMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> dic = getHistoryInfoInMonthForPage(startdate, enddate.AddDays(1), intkey, timezone);
                    DebugLog.Debug("SettingController HistoryMonthReport End");
                    return Json(dic, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        // Export History Report.
        
        public void ExportHistoryReport()
        {
            try
            {
                DebugLog.Debug("SettingController ExportHistoryReport Start");
                Encoding codestyle = null;
                string[] lang = Request.UserLanguages;
                if (lang[0].ToUpper().Equals("ZH-CN"))
                {
                    codestyle = Encoding.GetEncoding("gb2312");
                }
                else
                {
                    codestyle = Encoding.UTF8;
                }
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                String starttime = Request["startTime"];
                String endtime = Request["endTime"];
                DateTime startDate = DateTime.Parse(starttime);
                DateTime endDate = DateTime.Parse(endtime);
                String startStr = startDate.ToString("yyyy/MM/dd HH:mm:ss");
                String endStr = endDate.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime startdate = DateTime.Parse(startStr);
                DateTime enddate = DateTime.Parse(endStr);
                String timezone = Request["timezone"];
                List<HistoryReport> MonthList = new List<HistoryReport>();
                //get vehicles data by date
                if (Session["companyID"].ToString() == "ABCSoft" || Session["companyID"].ToString() == "ihpleD")
                {
                    MonthList = getHistoryInfoInMonth_fakedata();
                }
                else
                {
                    MonthList = getHistoryInfoInMonth(startdate, enddate.AddDays(1), timezone);
                }
                //output
                String now = Request["date"];//DateTime.Now.ToString("yyyyMMddHHmmss");
                String filename = "HistoryReport" + now + ".csv";
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(filename, codestyle));
                //write file stream
                System.IO.MemoryStream output = new System.IO.MemoryStream();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, codestyle);
                writer.Write(TripConstant.ExportVehicleName + ",");
                writer.Write(TripConstant.ExportVehicleLicence + ",");
                writer.Write(TripConstant.ExportVehicleLastTime + ",");
                writer.Write(ihpleD_String_cn.export_lastestlocation_province + ",");
                writer.Write(ihpleD_String_cn.export_lastestlocation_city + ",");
                writer.Write(ihpleD_String_cn.export_lastestlocation_district + ",");
                writer.Write(ihpleD_String_cn.export_lastestlocation_street);
                writer.WriteLine();
                writer.Flush();
                int length = (int)output.Position;
                output.Position = 0;
                byte[] bytes_title = new byte[output.Length];
                output.Read(bytes_title, 0, length);
                Response.BinaryWrite(bytes_title);
                Response.Flush();
                output.Position = 0;
                foreach (HistoryReport i in MonthList)
                {
                    writer.Write('"' + i.name.Trim() + '"' + ",");
                    writer.Write('"' + i.licence.Trim() + '"' + ",");
                    writer.Write('"' + i.time.Trim() + '"' + ",");
                    writer.Write('"' + i.location_province.Trim() + '"' + ",");
                    writer.Write('"' + i.location_city.Trim() + '"' + ",");
                    writer.Write('"' + i.location_district.Trim() + '"' + ",");
                    writer.Write('"' + i.location_street.Trim() + '"');
                    writer.WriteLine();
                    writer.Flush();
                    length = (int)output.Position;
                    output.Position = 0;
                    byte[] bytes = new byte[length];
                    //bytes = output.GetBuffer();
                    output.Read(bytes, 0, length);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    output.Position = 0;
                }
                output.Close();
                writer.Close();
                Response.End();
                DebugLog.Debug("SettingController ExportHistoryReport End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        //ABCSoft假数据
        
        private List<HealthReport> getWarnInfoInMonth_fakedata()
        {
            try
            {
                DebugLog.Debug("SettingController getWarnInfoInMonth_fakedata Start");
                List<HealthReport> healthList = new List<HealthReport>();
                String companyid = Session["companyID"].ToString();

                //get all vehicle.
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehicleList = new List<Vehicle>();
                vehicleList = db.GetTenantVehiclesByCompannyID(companyid);
                //get date from db
                for (int i = 0; i < 3; i++)
                {
                    HealthReport temp = new HealthReport();
                    temp.id = i + 1;
                    //int j = i + 1;
                    temp.name = "奔驰" + temp.id;
                    temp.date = "2014-03-22";
                    temp.licence = "辽A1111" + temp.id;
                    temp.speed = "1";
                    temp.round = "1";
                    temp.engine = "1";
                    temp.shake = "1";
                    temp.warninginfo = "";
                    temp.warningtype = "";
                    temp.warnningtime = "";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 4;
                    //int j = i + 1;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.speed = "3";
                    temp.round = "3";
                    temp.engine = "3";
                    temp.shake = "3";
                    temp.warninginfo = "";
                    temp.warningtype = "";
                    temp.warnningtime = "";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                for (int i = 0; i < 2; i++)
                {
                    HealthReport temp = new HealthReport();
                    temp.id = i + 5;
                    //int j = i + 1;
                    temp.name = "宝马" + temp.id;
                    temp.date = "2014-03-22";
                    temp.licence = "辽A1112" + temp.id;
                    temp.speed = "2";
                    temp.round = "0";
                    temp.engine = "1";
                    temp.shake = "1";
                    temp.warninginfo = "";
                    temp.warningtype = "";
                    temp.warnningtime = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 8;
                    //int j = i + 1;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.speed = "4";
                    temp.round = "0";
                    temp.engine = "2";
                    temp.shake = "2";
                    temp.warninginfo = "";
                    temp.warningtype = "";
                    temp.warnningtime = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 9;
                    //int j = i + 1;
                    temp.name = "大众1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A1113" + temp.id;
                    temp.speed = "2";
                    temp.round = "0";
                    temp.engine = "1";
                    temp.shake = "1";
                    temp.warninginfo = "";
                    temp.warningtype = "";
                    temp.warnningtime = "";
                    temp.group = TripConstant.ExportNogroup;
                    healthList.Add(temp);
                }
                DebugLog.Debug("SettingController getWarnInfoInMonth_fakedata End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<HealthReport> getWarnInfoInMonth_fakedata_CSV()
        {
            try
            {
                DebugLog.Debug("SettingController getWarnInfoInMonth_fakedata_CSV Start");
                List<HealthReport> healthList = new List<HealthReport>();
                String companyid = Session["companyID"].ToString();

                //get all vehicle.
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehicleList = new List<Vehicle>();
                vehicleList = db.GetTenantVehiclesByCompannyID(companyid);
                //get date from db
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 1;
                    //int j = i + 1;
                    temp.name = "奔驰1";
                    temp.warnningtime = "2014-03-22 00:00:00";
                    temp.licence = "辽A11111";
                    temp.warningtype = "超速报警";
                    temp.warninginfo = "时速报警: 80公里/小时";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 2;
                    //int j = i + 1;
                    temp.name = "奔驰1";
                    temp.warnningtime = "2014-03-22 00:30:00";
                    temp.licence = "辽A11111";
                    temp.warningtype = "超速报警";
                    temp.warninginfo = "时速报警: 80公里/小时";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "顺丰";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 3;
                    temp.name = "宝马1 ";
                    temp.warnningtime = "2014-03-22 00:00:00";
                    temp.licence = "辽A11112";
                    temp.warningtype = "超速报警";
                    temp.warninginfo = "时速报警: 80公里/小时";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }

                {
                    HealthReport temp = new HealthReport();
                    temp.id = 4;
                    temp.name = "宝马1 ";
                    temp.warnningtime = "2014-03-22 00:20:00";
                    temp.licence = "辽A11112";
                    temp.warningtype = "超转速报警";
                    temp.warninginfo = "转速超过4000RPM";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 5;
                    temp.name = "宝马1 ";
                    temp.warnningtime = "2014-03-22 01:30:00";
                    temp.licence = "辽A11112";
                    temp.warningtype = "引擎报警";
                    temp.warninginfo = "引擎灯亮";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                {
                    HealthReport temp = new HealthReport();
                    temp.id = 6;
                    temp.name = "宝马1 ";
                    temp.warnningtime = "2014-03-22 01:40:00";
                    temp.licence = "辽A11112";
                    temp.warningtype = "震动报警";
                    temp.warninginfo = "震动强度3G";
                    temp.speed = "";
                    temp.round = "";
                    temp.engine = "";
                    temp.shake = "";
                    temp.group = "圆通";
                    healthList.Add(temp);
                }
                DebugLog.Debug("SettingController getWarnInfoInMonth_fakedata_CSV End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<UtilizationReport> getUtilizationInMonth_fakedata()
        {
            try
            {
                DebugLog.Debug("SettingController getUtilizationInMonth_fakedata Start");
                List<UtilizationReport> utilizationList = new List<UtilizationReport>();
                String companyid = (String)Session["companyID"];
                //get date from db
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 1;
                    temp.name = "奔驰1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11111";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 2;
                    temp.name = "奔驰2 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11112";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 3;
                    temp.name = "奔驰3 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11113";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 4;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.drivingTime = "07: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 5;
                    temp.name = "宝马1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11121";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "圆通";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 6;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = "圆通";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 7;
                    temp.name = "大众1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11131";
                    temp.drivingTime = "02: 30: 00";
                    temp.utilization = "31.25%";
                    temp.engineontime = "";
                    temp.tripidle = "";
                    temp.group = TripConstant.ExportNogroup;
                    utilizationList.Add(temp);
                }
                DebugLog.Debug("SettingController getUtilizationInMonth_fakedata End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<UtilizationReport> getUtilizationInMonth_fakedata_CSV()
        {
            try
            {
                DebugLog.Debug("SettingController getUtilizationInMonth_fakedata_CSV Start");
                List<UtilizationReport> utilizationList = new List<UtilizationReport>();
                String companyid = (String)Session["companyID"];
                //get date from db
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 1;
                    temp.name = "奔驰1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11111";
                    temp.drivingTime = "";
                    temp.engineontime = "2.5";
                    temp.tripidle = "0.25";
                    temp.utilization = "10.00";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 2;
                    temp.name = "奔驰2 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11112";
                    temp.drivingTime = "";
                    temp.engineontime = "2.5";
                    temp.tripidle = "0.25";
                    temp.utilization = "10.00";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 3;
                    temp.name = "奔驰3 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11113";
                    temp.drivingTime = "";
                    temp.engineontime = "2.5";
                    temp.tripidle = "0.25";
                    temp.utilization = "10.00";
                    temp.group = "顺丰";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 4;
                    temp.name = "宝马1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11121";
                    temp.drivingTime = "";
                    temp.engineontime = "2.5";
                    temp.tripidle = "0.25";
                    temp.utilization = "10.00";
                    temp.group = "圆通";
                    utilizationList.Add(temp);
                }
                {
                    UtilizationReport temp = new UtilizationReport();
                    temp.id = 5;
                    temp.name = "大众1 ";
                    temp.date = "2014-03-25";
                    temp.licence = "辽A11131";
                    temp.drivingTime = "";
                    temp.engineontime = "2.5";
                    temp.tripidle = "0.25";
                    temp.utilization = "10.00"; ;
                    temp.group = TripConstant.ExportNogroup;
                    utilizationList.Add(temp);
                }
                DebugLog.Debug("SettingController getUtilizationInMonth_fakedata_CSV End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<TripReport> getTripInfoInMonth_fakedata()
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonth_fakedata Start");
                List<TripReport> tripList = new List<TripReport>();
                String companyid = Session["companyID"].ToString();
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehicleList = new List<Vehicle>();
                vehicleList = db.GetTenantVehiclesByCompannyID(companyid);
                //get date from db
                {
                    TripReport temp = new TripReport();
                    temp.id = 1;
                    temp.name = "奔驰1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11111";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "顺丰";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 2;
                    temp.name = "奔驰2";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11112";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "顺丰";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 3;
                    temp.name = "奔驰3";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11113";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "顺丰";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 4;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.drivingTime = "07: 30: 00";
                    temp.idleTime = "00: 45: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 6;
                    temp.distance = "300.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "顺丰";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 5;
                    temp.name = "宝马1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11121";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "圆通";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 6;
                    temp.name = "大众1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11131";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "圆通";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 7;
                    temp.name = TripConstant.ExportSum;
                    temp.date = "2014-03";
                    temp.licence = "";
                    temp.drivingTime = "05: 00: 00";
                    temp.idleTime = "00: 30: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 4;
                    temp.distance = "200.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = "圆通";
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 8;
                    temp.name = "奇瑞1";
                    temp.date = "2014-03-22";
                    temp.licence = "辽A11151";
                    temp.drivingTime = "02: 30: 00";
                    temp.idleTime = "00: 15: 00";
                    temp.idleDivide = "10.00%";
                    temp.tripCnt = 2;
                    temp.distance = "100.0";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.tripdistance = "";
                    temp.group = TripConstant.ExportNogroup;
                    temp.triptime = "";
                    temp.tripidletime = "";
                    temp.tripidlerate = "";
                    temp.tripavespeed = "";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                DebugLog.Debug("SettingController getTripInfoInMonth_fakedata End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<TripReport> getTripInfoInMonth_fakedata_CSV()
        {
            try
            {
                DebugLog.Debug("SettingController getTripInfoInMonth_fakedata_CSV Start");
                List<TripReport> tripList = new List<TripReport>();
                String companyid = Session["companyID"].ToString();
                VehicleDBInterface db = new VehicleDBInterface();
                List<Vehicle> vehicleList = new List<Vehicle>();
                vehicleList = db.GetTenantVehiclesByCompannyID(companyid);
                //get date from db
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 1;
                    temp.name = "奔驰1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11111";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "顺丰";
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 2;
                    temp.name = "奔驰1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11111";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "顺丰";
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 3;
                    temp.name = "奔驰1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11111";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "顺丰";
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 4;
                    temp.name = "奔驰2";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11121";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "顺丰";
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 5;
                    temp.name = "宝马1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11121";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "20";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "圆通";
                    temp.triptime = "50";
                    temp.tripidletime = "10";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 6;
                    temp.name = "宝马1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11131";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = "圆通";
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 7;
                    temp.name = "大众1";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11141";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = TripConstant.ExportNogroup;
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                {
                    TripReport temp = new TripReport();
                    //temp.id = i + 1;
                    //temp.name = "货车 " + j;
                    temp.id = 8;
                    temp.name = "大众2";
                    //temp.date = startTime.AddDays(i).ToString("yyyy/MM/dd HH:mm");//显示当前时间
                    temp.licence = "辽A11151";
                    temp.leavetime = "2014-03-22 10:10:10";
                    temp.leavelocation_province = "北京市";
                    temp.leavelocation_city = "北京市";
                    temp.leavelocation_district = "海淀区";
                    temp.leavelocation_street = "新建宫门路19号";
                    temp.arrivaltime = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "北京市";
                    temp.arrivallocation_city = "北京市";
                    temp.arrivallocation_district = "东城区";
                    temp.arrivallocation_street = "东长安街";
                    temp.tripdistance = "10";
                    temp.drivingTime = "";
                    temp.idleTime = "";
                    temp.idleDivide = "";
                    temp.tripCnt = -1;
                    temp.distance = "";
                    temp.group = TripConstant.ExportNogroup;
                    temp.triptime = "25";
                    temp.tripidletime = "5";
                    temp.tripidlerate = "20";
                    temp.tripavespeed = "30";
                    temp.utilization = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.guid = "";
                    tripList.Add(temp);
                }
                DebugLog.Debug("SettingController getTripInfoInMonth_fakedata_CSV End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private List<HistoryReport> getHistoryInfoInMonth_fakedata()
        {
            try
            {
                DebugLog.Debug("SettingController getHistoryInfoInMonth_fakedata Start");
                List<HistoryReport> historyList = new List<HistoryReport>();
                String companyid = (String)Session["companyID"];
                //get date from db
                {
                    HistoryReport temp = new HistoryReport();
                    temp.id = 1;
                    temp.name = "奔驰4";
                    temp.licence = "辽A11111";
                    temp.time = "2014-03-22 10:10:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.guid = "";
                    temp.location_province = "北京市";
                    temp.location_city = "北京市";
                    temp.location_district = "东城区";
                    temp.location_street = "东长安街";
                    historyList.Add(temp);
                }
                {
                    HistoryReport temp = new HistoryReport();
                    temp.id = 2;
                    temp.name = "红旗1";
                    temp.licence = "辽A11112";
                    temp.time = "2014-03-29 16:50:10";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.guid = "";
                    temp.location_province = "北京市";
                    temp.location_city = "北京市";
                    temp.location_district = "东城区";
                    temp.location_street = "东长安街";
                    historyList.Add(temp);
                }
                {
                    HistoryReport temp = new HistoryReport();
                    temp.id = 3;
                    temp.name = "红旗2";
                    temp.licence = "辽A11113";
                    temp.time = "2014-03-27 20:13:20";
                    temp.arrivallocation_province = "";
                    temp.arrivallocation_city = "";
                    temp.arrivallocation_district = "";
                    temp.arrivallocation_street = "";
                    temp.endlocctionlat = 0;
                    temp.endlocctionlng = 0;
                    temp.leavelocation_province = "";
                    temp.leavelocation_city = "";
                    temp.leavelocation_district = "";
                    temp.leavelocation_street = "";
                    temp.startlocctionlat = 0;
                    temp.startlocctionlng = 0;
                    temp.guid = "";
                    temp.location_province = "北京市";
                    temp.location_city = "北京市";
                    temp.location_district = "东城区";
                    temp.location_street = "东长安街";
                    historyList.Add(temp);
                }
                DebugLog.Debug("SettingController getHistoryInfoInMonth_fakedata End");
                return historyList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //页面显示VehicleHealthInfo
        public List<HealthReport> AppendVehicleHealthInfo(List<HealthReport> healthList, string companyid, DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController AppendVehicleHealthInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                int iNumForID = 0;
                int itimezone = int.Parse(timezone);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    iNumForID = AddVehicleInGroupWarnningInfo(groupList, healthList, vehiclehasOBU, vehicleList, startdate, enddate, iNumForID, iGroupNum, itimezone);
                }
                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupWarnningInfo(vehicleAllList, healthList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, iNumForID, itimezone);
                DebugLog.Debug("SettingController AppendVehicleHealthInfo End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //初始化未命名的车辆
        public void InitializationVehicleName(List<Vehicle> vehicleList)
        {
            try
            {
                DebugLog.Debug("SettingController InitializationVehicleName Start");
                for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                {
                    if (null == vehicleList[iVehicleNum].name || vehicleList[iVehicleNum].name.Trim() == "")
                    {
                        vehicleList[iVehicleNum].name = "";
                    }
                }
                DebugLog.Debug("SettingController InitializationVehicleName End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //处理分组中的车辆信息
        public int AddVehicleInGroupWarnningInfo(List<VehicleGroup> groupList, List<HealthReport> healthList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iNumForID, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupWarnningInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return iNumForID;
                }
                else
                {
                    //车辆当日统计
                    int SpeedAlertCount = 0;
                    int MottionAlertCount = 0;
                    int HighprmAlertCount = 0;
                    int EngineAlertCount = 0;
                    //分组月份统计
                    int SpeedAlertCountOfGroup = 0;
                    int MottionAlertCountOfGroup = 0;
                    int HighprmAlertCountOfGroup = 0;
                    int EngineAlertCountOfGroup = 0;

                    int GroupFlag = 0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Alert> alert = db.GetAlertByVehicleIDandTimeforReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Alert> alertlist = alert.FindAll(x => (x.TriggeredDateTime >= d.AddDays(-1)
                                    && x.TriggeredDateTime <= d)
                                    );
                                if (alertlist.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    GroupFlag = 1;
                                    alertlist.Sort((x, y) => y.TriggeredDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.TriggeredDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    HealthReport temp = new HealthReport();
                                    temp.name = vehicleList[iVehicleNum].name.Trim();

                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence.Trim();
                                    }
                                    temp.date = (((DateTime)alertlist[0].TriggeredDateTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    for (int j = 0; j < alertlist.Count(); ++j)
                                    {
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Speed
                                            || alertlist[j].AlertType.Trim() == "SPEED"
                                            || alertlist[j].AlertType.Trim() == "Speed")
                                        {
                                            ++SpeedAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Rpm
                                            || alertlist[j].AlertType.Trim() == TripConstant.RpmWithEmpty
                                            || alertlist[j].AlertType.Trim() == "EngineRPM"
                                            || alertlist[j].AlertType.Trim() == "RPM"
                                            || alertlist[j].AlertType.Trim() == "Rpm"
                                            || alertlist[j].AlertType.Trim() == "Engine Rpm"
                                            || alertlist[j].AlertType.Trim() == "EngineRpm"
                                            )
                                        {
                                            ++HighprmAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Motion
                                            || alertlist[j].AlertType.Trim() == "MOTION"
                                            || alertlist[j].AlertType.Trim() == "Motion"
                                            )
                                        {
                                            ++MottionAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Engine)
                                        {
                                            ++EngineAlertCount;
                                        }
                                    }
                                    temp.speed = SpeedAlertCount + "";
                                    temp.round = HighprmAlertCount + "";
                                    temp.shake = MottionAlertCount + "";
                                    temp.engine = EngineAlertCount + "";
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iNumForID;
                                    temp.warninginfo = "";
                                    temp.warningtype = "";
                                    temp.warnningtime = "";

                                    healthList.Add(temp);

                                    SpeedAlertCountOfGroup += SpeedAlertCount;
                                    HighprmAlertCountOfGroup += HighprmAlertCount;
                                    MottionAlertCountOfGroup += MottionAlertCount;
                                    EngineAlertCountOfGroup += EngineAlertCount;

                                    SpeedAlertCount = 0;
                                    MottionAlertCount = 0;
                                    HighprmAlertCount = 0;
                                    EngineAlertCount = 0;

                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                    if (GroupFlag == 1)
                    {
                        HealthReport GroupTemp = new HealthReport();
                        GroupTemp.name = TripConstant.ExportSum;
                        GroupTemp.date = "";
                        GroupTemp.licence = "";
                        GroupTemp.speed = SpeedAlertCountOfGroup + "";
                        GroupTemp.round = HighprmAlertCountOfGroup + "";
                        GroupTemp.shake = MottionAlertCountOfGroup + "";
                        GroupTemp.engine = EngineAlertCountOfGroup + "";
                        GroupTemp.group = groupList[iGroupNum].name.Trim();
                        GroupTemp.id = iNumForID;
                        GroupTemp.warninginfo = "";
                        GroupTemp.warningtype = "";
                        GroupTemp.warnningtime = "";
                        healthList.Add(GroupTemp);
                        ++iNumForID;
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupWarnningInfo End");
                return iNumForID;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //处理未分组的车辆信息
        public void AddVehicleNotInGroupWarnningInfo(List<Vehicle> vehicleAllList, List<HealthReport> healthList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, int iNumForID, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupWarnningInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }
                if (vehiclenogroup.Count() == 0)
                {
                    return;
                }
                else
                {
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));
                    //车辆当日统计
                    int SpeedAlertCount = 0;
                    int MottionAlertCount = 0;
                    int HighprmAlertCount = 0;
                    int EngineAlertCount = 0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Alert> alert = db.GetAlertByVehicleIDandTimeforReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Alert> alertlist = alert.FindAll(x => (x.TriggeredDateTime >= d.AddDays(-1)
                                    && x.TriggeredDateTime <= d)
                                    );
                                if (alertlist.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    alertlist.Sort((x, y) => y.TriggeredDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.TriggeredDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    HealthReport temp = new HealthReport();
                                    temp.name = vehiclenogroup[iVehicleNum].name.Trim();

                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence.Trim();
                                    }
                                    temp.date = (((DateTime)alertlist[0].TriggeredDateTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    for (int j = 0; j < alertlist.Count(); ++j)
                                    {
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Speed
                                            || alertlist[j].AlertType.Trim() == "SPEED"
                                            || alertlist[j].AlertType.Trim() == "Speed")
                                        {
                                            ++SpeedAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Rpm
                                            || alertlist[j].AlertType.Trim() == TripConstant.RpmWithEmpty
                                            || alertlist[j].AlertType.Trim() == "EngineRPM"
                                            || alertlist[j].AlertType.Trim() == "RPM"
                                            || alertlist[j].AlertType.Trim() == "Rpm"
                                            || alertlist[j].AlertType.Trim() == "Engine Rpm"
                                            || alertlist[j].AlertType.Trim() == "EngineRpm"
                                            )
                                        {
                                            ++HighprmAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Motion
                                            || alertlist[j].AlertType.Trim() == "MOTION"
                                            || alertlist[j].AlertType.Trim() == "Motion"
                                            )
                                        {
                                            ++MottionAlertCount;
                                        }
                                        if (alertlist[j].AlertType.Trim() == TripConstant.Engine)
                                        {
                                            ++EngineAlertCount;
                                        }
                                    }
                                    temp.speed = SpeedAlertCount + "";
                                    temp.round = HighprmAlertCount + "";
                                    temp.shake = MottionAlertCount + "";
                                    temp.engine = EngineAlertCount + "";
                                    temp.group = ihpleD_String_cn.export_nogroup;
                                    temp.id = iNumForID;
                                    temp.warninginfo = "";
                                    temp.warningtype = "";
                                    temp.warnningtime = "";

                                    healthList.Add(temp);

                                    SpeedAlertCount = 0;
                                    MottionAlertCount = 0;
                                    HighprmAlertCount = 0;
                                    EngineAlertCount = 0;

                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupWarnningInfo End");
                return;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //导出VehicleHealthInfo
        public List<HealthReport> VehicleWarnningInfoForCSV(List<HealthReport> healthList, string companyid, DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController VehicleWarnningInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                int itimezone = int.Parse(timezone);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    AddVehicleInGroupWarnningInfoForCSV(groupList, healthList, vehiclehasOBU, vehicleList, startdate, enddate, iGroupNum, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupWarnningInfoForCSV(vehicleAllList, healthList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, groupList, itimezone);
                DebugLog.Debug("SettingController VehicleWarnningInfoForCSV End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //处理分组中的车辆信息
        public List<HealthReport> AddVehicleInGroupWarnningInfoForCSV(List<VehicleGroup> groupList, List<HealthReport> healthList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupWarnningInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return healthList;
                }
                else
                {
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Alert> alertlist = db.GetAlertByVehicleIDandTimeforReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (alertlist.Count() == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = alertlist.Count - 1; i >= 0; --i)
                                {
                                    string value = "";
                                    dynamic obj = JsonConvert.DeserializeObject(alertlist[i].Value.Trim());
                                    if (obj["LimitValue"] != null)
                                    {
                                        value = obj["LimitValue"].Value.ToString();
                                    }
                                    else
                                    {
                                        value = TripConstant.ExportUndefine;
                                    }
                                    HealthReport temp = new HealthReport();
                                    temp.name = vehicleList[iVehicleNum].name.Trim();
                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence.Trim();
                                    }
                                    temp.date = ((DateTime)alertlist[i].TriggeredDateTime.Value.AddHours(itimezone)).ToString("yyyy-MM-dd");

                                    if (alertlist[i].AlertType.Trim() == TripConstant.Speed
                                        || alertlist[i].AlertType.Trim() == "SPEED"
                                        || alertlist[i].AlertType.Trim() == "Speed")
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.SpeedAlertInfo;
                                            temp.warninginfo = TripConstant.SpeedAlert + value + TripConstant.SpeedUnit;
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.SpeedAlertInfo;
                                            temp.warninginfo = TripConstant.SpeedAlert + value;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Rpm
                                        || alertlist[i].AlertType.Trim() == TripConstant.RpmWithEmpty
                                        || alertlist[i].AlertType.Trim() == "EngineRPM"
                                        || alertlist[i].AlertType.Trim() == "RPM"
                                        || alertlist[i].AlertType.Trim() == "Rpm"
                                        || alertlist[i].AlertType.Trim() == "Engine Rpm"
                                        || alertlist[i].AlertType.Trim() == "EngineRpm"
                                        )
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.RPMAlertInfo;
                                            temp.warninginfo = TripConstant.EngineRPM + value + "RPM";
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.RPMAlertInfo;
                                            temp.warninginfo = TripConstant.EngineRPM + value;
                                        }
                                        if (obj["DurationThreshold"] != null)
                                        {
                                            temp.warninginfo += ";" + TripConstant.ExportDurationThreshold + obj["DurationThreshold"].Value.ToString() + TripConstant.ExportSecond;
                                        }
                                        else
                                        {
                                            temp.warninginfo += ";" + TripConstant.ExportDurationThreshold + TripConstant.ExportUndefine;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Motion
                                        || alertlist[i].AlertType.Trim() == "MOTION"
                                        || alertlist[i].AlertType.Trim() == "Motion"
                                        )
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.MotionAlertInfo;
                                            temp.warninginfo = TripConstant.MotionLevel + value + "G";
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.MotionAlertInfo;
                                            temp.warninginfo = TripConstant.MotionLevel + value;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Engine)
                                    {
                                        temp.warningtype = TripConstant.EngineAlertInfo;
                                        temp.warninginfo = TripConstant.EngineAlertDetail;
                                    }
                                    temp.speed = "";
                                    temp.round = "";
                                    temp.shake = "";
                                    temp.engine = "";
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iGroupNum;
                                    temp.warnningtime = alertlist[i].TriggeredDateTime.Value.AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss") + "";
                                    if (temp.warningtype != "" && temp.warningtype != null)
                                    {
                                        healthList.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupWarnningInfoForCSV End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        //处理未分组的车辆信息
        public List<HealthReport> AddVehicleNotInGroupWarnningInfoForCSV(List<Vehicle> vehicleAllList, List<HealthReport> healthList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, List<VehicleGroup> groupList, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupWarnningInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }
                if (vehiclenogroup.Count() == 0)
                {
                    return healthList;
                }
                else
                {
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Alert> alertlist = db.GetAlertByVehicleIDandTimeforReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (alertlist.Count() == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = alertlist.Count - 1; i >= 0; --i)
                                {
                                    string value = "";
                                    dynamic obj = JsonConvert.DeserializeObject(alertlist[i].Value.Trim());
                                    if (obj["LimitValue"] != null)
                                    {
                                        value = obj["LimitValue"].Value.ToString();
                                    }
                                    else
                                    {
                                        value = TripConstant.ExportUndefine;
                                    }
                                    HealthReport temp = new HealthReport();
                                    temp.name = vehiclenogroup[iVehicleNum].name.Trim();
                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence.Trim();
                                    }
                                    temp.date = ((DateTime)alertlist[i].TriggeredDateTime.Value.AddHours(itimezone)).ToString("yyyy-MM-dd");

                                    if (alertlist[i].AlertType.Trim() == TripConstant.Speed
                                        || alertlist[i].AlertType.Trim() == "SPEED"
                                        || alertlist[i].AlertType.Trim() == "Speed")
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.SpeedAlertInfo;
                                            temp.warninginfo = TripConstant.SpeedAlert + value + TripConstant.SpeedUnit;
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.SpeedAlertInfo;
                                            temp.warninginfo = TripConstant.SpeedAlert + value;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Rpm
                                        || alertlist[i].AlertType.Trim() == TripConstant.RpmWithEmpty
                                        || alertlist[i].AlertType.Trim() == "EngineRPM"
                                        || alertlist[i].AlertType.Trim() == "RPM"
                                        || alertlist[i].AlertType.Trim() == "Rpm"
                                        || alertlist[i].AlertType.Trim() == "Engine Rpm"
                                        || alertlist[i].AlertType.Trim() == "EngineRpm"
                                        )
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.RPMAlertInfo;
                                            temp.warninginfo = TripConstant.EngineRPM + value + "RPM";
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.RPMAlertInfo;
                                            temp.warninginfo = TripConstant.EngineRPM + value;
                                        }
                                        if (obj["DurationThreshold"] != null)
                                        {
                                            temp.warninginfo += ";" + TripConstant.ExportDurationThreshold + obj["DurationThreshold"].Value.ToString() + TripConstant.ExportSecond;
                                        }
                                        else
                                        {
                                            temp.warninginfo += ";" + TripConstant.ExportDurationThreshold + TripConstant.ExportUndefine;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Motion
                                        || alertlist[i].AlertType.Trim() == "MOTION"
                                        || alertlist[i].AlertType.Trim() == "Motion"
                                        )
                                    {
                                        if (obj["LimitValue"] != null)
                                        {
                                            temp.warningtype = TripConstant.MotionAlertInfo;
                                            temp.warninginfo = TripConstant.MotionLevel + value + "G";
                                        }
                                        else
                                        {
                                            temp.warningtype = TripConstant.MotionAlertInfo;
                                            temp.warninginfo = TripConstant.MotionLevel + value;
                                        }
                                    }
                                    if (alertlist[i].AlertType.Trim() == TripConstant.Engine)
                                    {
                                        temp.warningtype = TripConstant.EngineAlertInfo;
                                        temp.warninginfo = TripConstant.EngineAlertDetail;
                                    }
                                    temp.speed = "";
                                    temp.round = "";
                                    temp.shake = "";
                                    temp.engine = "";
                                    temp.group = ihpleD_String_cn.export_nogroup;
                                    temp.id = groupList.Count;
                                    temp.warnningtime = alertlist[i].TriggeredDateTime.Value.AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss") + "";
                                    if (temp.warningtype != "" && temp.warningtype != null)
                                    {
                                        healthList.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupWarnningInfoForCSV End");
                return healthList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AppendVehicleUtilizationInfo(List<TripReport> utilizationList, string companyid, DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController AppendVehicleUtilizationInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                int iNumForID = 0;
                int itimezone = int.Parse(timezone);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    iNumForID = AddVehicleInGroupUtilizationInfo(groupList, utilizationList, vehiclehasOBU, vehicleList, startdate, enddate, iNumForID, iGroupNum, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupUtilizationInfo(vehicleAllList, utilizationList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, iNumForID, itimezone);

                DebugLog.Debug("SettingController AppendVehicleUtilizationInfo End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public int AddVehicleInGroupUtilizationInfo(List<VehicleGroup> groupList, List<TripReport> utilizationList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iNumForID, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupUtilizationInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return iNumForID;
                }
                else
                {
                    VehicleDBInterface db = new VehicleDBInterface();
                    double Idle = 0.0;
                    double Distance = 0.0;

                    double GroupIdle = 0.0;
                    double GroupDistance = 0.0;
                    TimeSpan tempGroup = new TimeSpan();
                    int TripNum = 0;
                    int GroupFlag = 0;
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >= d.AddDays(-1)
                                    && x.startTime <= d)
                                    );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    GroupFlag = 1;
                                    ++TripNum;
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehicleList[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence.Trim();
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                        Distance += (double)tripDay[j].distance;
                                    }

                                    SetDrivingTimeForPage(temp, tempT.Hours, tempT.Minutes, tempT.Seconds);
                                    SetIdleTimeForPage(temp, ((int)Idle) / 3600, ((int)Idle) / 60 - ((int)Idle) / 3600 * 60, ((int)Idle) % 60);

                                    temp.idleDivide = "0%";
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = Distance + "";
                                    temp.arrivallocation_province = "";
                                    temp.arrivallocation_city = "";
                                    temp.arrivallocation_district = "";
                                    temp.arrivallocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iNumForID;
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = Math.Round(((double)(tempT.TotalSeconds) / (double)(8 * 3600) * 100), 2) + "%";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    utilizationList.Add(temp);

                                    tempGroup += tempT;
                                    GroupDistance += Distance;
                                    GroupIdle += Idle;

                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                    if (GroupFlag == 1)
                    {
                        TripReport GroupTemp = new TripReport();
                        GroupTemp.name = TripConstant.ExportSum;
                        GroupTemp.date = "";
                        GroupTemp.licence = "";

                        SetDrivingTimeForPage(GroupTemp, tempGroup.Hours, tempGroup.Minutes, tempGroup.Seconds);
                        SetIdleTimeForPage(GroupTemp, ((int)GroupIdle) / 3600, ((int)GroupIdle) / 60 - ((int)GroupIdle) / 3600 * 60, ((int)GroupIdle) % 60);
                        GroupTemp.tripidlerate = "";
                        GroupTemp.tripCnt = TripNum;
                        GroupTemp.distance = GroupDistance + "";
                        GroupTemp.arrivallocation_province = "";
                        GroupTemp.arrivallocation_city = "";
                        GroupTemp.arrivallocation_district = "";
                        GroupTemp.arrivallocation_street = "";
                        GroupTemp.arrivaltime = "2014-03-22 10:10:10";
                        GroupTemp.leavelocation_province = "";
                        GroupTemp.leavelocation_city = "";
                        GroupTemp.leavelocation_district = "";
                        GroupTemp.leavelocation_street = "";
                        GroupTemp.leavetime = "2014-03-22 10:10:10";
                        GroupTemp.tripdistance = "";
                        GroupTemp.tripidlerate = "";
                        GroupTemp.tripidletime = "";
                        GroupTemp.tripavespeed = "";
                        GroupTemp.group = groupList[iGroupNum].name.Trim();
                        GroupTemp.id = iNumForID;
                        GroupTemp.idleDivide = "0%";
                        GroupTemp.triptime = "";
                        GroupTemp.utilization = Math.Round(((double)(tempGroup.TotalSeconds) / (double)(8 * 3600 * TripNum) * 100), 2) + "%";
                        GroupTemp.isFirstFlag = 0;
                        GroupTemp.isLastFlag = 0;
                        utilizationList.Add(GroupTemp);
                        ++iNumForID;
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupUtilizationInfo End");
                return iNumForID;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public void AddVehicleNotInGroupUtilizationInfo(List<Vehicle> vehicleAllList, List<TripReport> utilizationList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, int iNumForID, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupUtilizationInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }

                if (vehiclenogroup.Count() != 0)
                {
                    VehicleDBInterface db = new VehicleDBInterface();
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));

                    double Idle = 0.0;
                    double Distance = 0.0;
                    int TripNum = 0;

                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >= d.AddDays(-1)
                                    && x.startTime <= d)
                                    );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    ++TripNum;
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehiclenogroup[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence.Trim();
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                        Distance += (double)tripDay[j].distance;
                                    }

                                    SetDrivingTimeForPage(temp, tempT.Hours, tempT.Minutes, tempT.Seconds);
                                    SetIdleTimeForPage(temp, ((int)Idle) / 3600, ((int)Idle) / 60 - ((int)Idle) / 3600 * 60, ((int)Idle) % 60);

                                    temp.idleDivide = "0%";
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = Distance + "";
                                    temp.arrivallocation_province = "";
                                    temp.arrivallocation_city = "";
                                    temp.arrivallocation_district = "";
                                    temp.arrivallocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = ihpleD_String_cn.export_nogroup;
                                    temp.id = iNumForID;
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = Math.Round(((double)(tempT.TotalSeconds) / (double)(8 * 3600) * 100), 2) + "%";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    utilizationList.Add(temp);

                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupUtilizationInfo End");
                return;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private void SetDrivingTimeForPage(TripReport temp, int DrivingHours, int DrivingMins, int DrivingSecs)
        {
            
            string strHour;
            string strMin;
            string strSec;
            if (DrivingHours < 10)
            {
                strHour = "0" + DrivingHours.ToString();
            }
            else
            {
                strHour = DrivingHours.ToString();
            }
            if (DrivingMins < 10)
            {
                strMin = "0" + DrivingMins.ToString();
            }
            else
            {
                strMin = DrivingMins.ToString();
            }
            if (DrivingSecs < 10)
            {
                strSec = "0" + DrivingSecs.ToString();
            }
            else
            {
                strSec = DrivingSecs.ToString();
            }
            temp.drivingTime = strHour + ": " + strMin + ": " + strSec;
                
        }
        
        private void SetIdleTimeForPage(TripReport temp, int IdleHours, int IdleMins, int IdleSecs)
        {
            
            string strHour;
            string strMin;
            string strSec;
            if (IdleHours < 10)
            {
                strHour = "0" + IdleHours.ToString();
            }
            else
            {
                strHour = IdleHours.ToString();
            }
            if (IdleMins < 10)
            {
                strMin = "0" + IdleMins.ToString();
            }
            else
            {
                strMin = IdleMins.ToString();
            }
            if (IdleSecs < 10)
            {
                strSec = "0" + IdleSecs.ToString();
            }
            else
            {
                strSec = IdleSecs.ToString();
            }
            temp.idleTime = strHour + ": " + strMin + ": " + strSec;
               
        }
        
        public List<TripReport> VehicleUtilizationInfoForCSV(List<TripReport> utilizationList, string companyid, DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController VehicleUtilizationInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                int itimezone = int.Parse(timezone);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    AddVehicleInGroupUtilizationInfoForCSV(groupList, utilizationList, vehiclehasOBU, vehicleList, startdate, enddate, iGroupNum, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupUtilizationInfoForCSV(vehicleAllList, utilizationList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, groupList, itimezone);
                DebugLog.Debug("SettingController VehicleUtilizationInfoForCSV End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AddVehicleInGroupUtilizationInfoForCSV(List<VehicleGroup> groupList, List<TripReport> utilizationList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupUtilizationInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return utilizationList;
                }
                else
                {
                    //取得选择的时间以及当月天数
                    double Idle = 0.0;
                    double Distance = 0.0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >= d.AddDays(-1)
                                    && x.startTime <= d)
                                    );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehicleList[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence;
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                    }
                                    temp.drivingTime = Math.Round(((double)(tempT.TotalSeconds) / (double)(3600)), 2) + "";
                                    temp.idleTime = Math.Round(((double)(Idle) / (double)(3600)), 2) + "";
                                    temp.idleDivide = "0%";
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = Distance + "";
                                    temp.arrivallocation_province = "";
                                    temp.arrivallocation_city = "";
                                    temp.arrivallocation_district = "";
                                    temp.arrivallocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iGroupNum;
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = Math.Round(((double)(tempT.TotalSeconds) / (double)(8 * 3600) * 100), 2) + "";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    utilizationList.Add(temp);
                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupUtilizationInfoForCSV End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AddVehicleNotInGroupUtilizationInfoForCSV(List<Vehicle> vehicleAllList, List<TripReport> utilizationList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, List<VehicleGroup> groupList, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupUtilizationInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }
                if (vehiclenogroup.Count() == 0)
                {
                    return utilizationList;
                }
                else
                {
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));
                    double Idle = 0.0;
                    double Distance = 0.0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >= d.AddDays(-1)
                                    && x.startTime <= d)
                                    );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    List<Trip> triplist = new List<Trip>();
                                    triplist = db.GetTripByVehicleID(vehiclenogroup[iVehicleNum].pkid);
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehiclenogroup[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence;
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                        Distance += (double)tripDay[j].distance;
                                    }
                                    temp.drivingTime = Math.Round(((double)(tempT.TotalSeconds) / (double)(3600)), 2) + "";
                                    temp.idleTime = Math.Round(((double)(Idle) / (double)(3600)), 2) + "";
                                    temp.idleDivide = "0%";
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = Distance + "";
                                    temp.arrivallocation_province = "";
                                    temp.arrivallocation_city = "";
                                    temp.arrivallocation_district = "";
                                    temp.arrivallocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = TripConstant.ExportNogroup;
                                    temp.id = groupList.Count();
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = Math.Round(((double)(tempT.TotalSeconds) / (double)(8 * 3600) * 100), 2) + "";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    utilizationList.Add(temp);
                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupUtilizationInfoForCSV End");
                return utilizationList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AppendVehicleTripInfo(List<TripReport> tripList, string companyid, DateTime startdate, DateTime enddate, string timezone)
        {
            DebugLog.Debug("SettingController AppendVehicleTripInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                int iNumForID = 0;
                int itimezone = int.Parse(timezone);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    iNumForID = AddVehicleInGroupTripInfo(groupList, tripList, vehiclehasOBU, vehicleList, startdate, enddate, iNumForID, iGroupNum, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupTripInfo(vehicleAllList, tripList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, iNumForID, itimezone);
                DebugLog.Debug("SettingController AppendVehicleTripInfo End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public int AddVehicleInGroupTripInfo(List<VehicleGroup> groupList, List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iNumForID, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupTripInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return iNumForID;
                }
                else
                {
                    VehicleDBInterface db = new VehicleDBInterface();

                    double Idle = 0.0;
                    double Distance = 0.0;

                    double GroupIdle = 0.0;
                    double GroupDistance = 0.0;
                    TimeSpan tempGroup = new TimeSpan();

                    int TripNum = 0;
                    int GroupFlag = 0;
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >=d.AddDays(-1)
                                    && x.startTime <= d)
                                    );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    GroupFlag = 1;
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehicleList[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");

                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence.Trim();
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                        Distance += (double)tripDay[j].distance;
                                    }
                                    tempGroup += tempT;
                                    SetDrivingTimeForPage(temp, tempT.Hours + tempT.Days * 24, tempT.Minutes, tempT.Seconds);
                                    SetIdleTimeForPage(temp, ((int)Idle) / 3600, ((int)Idle) / 60 - ((int)Idle) / 3600 * 60, ((int)Idle) % 60);
                                    if (tempT.TotalSeconds == 0)
                                    {
                                        temp.idleDivide = "0%";
                                    }
                                    else
                                    {
                                        temp.idleDivide = Math.Round(((double)(Idle) / (double)(tempT.TotalSeconds) * 100), 2) + "%";
                                    }
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = String.Format("{0:N1}", Math.Round(Distance, 1)) + "";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iNumForID;
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = "";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    tripList.Add(temp);

                                    TripNum += temp.tripCnt;
                                    GroupDistance += Distance;
                                    GroupIdle += Idle;

                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                    if (GroupFlag == 1)
                    {
                        TripReport GroupTemp = new TripReport();
                        GroupTemp.name = TripConstant.ExportSum;
                        GroupTemp.date = "";
                        GroupTemp.licence = "";
                        SetDrivingTimeForPage(GroupTemp, tempGroup.Hours + tempGroup.Days * 24, tempGroup.Minutes, tempGroup.Seconds);
                        SetIdleTimeForPage(GroupTemp, ((int)GroupIdle) / 3600, ((int)GroupIdle) / 60 - ((int)GroupIdle) / 3600 * 60, ((int)GroupIdle) % 60);
                        GroupTemp.tripidlerate = "";
                        GroupTemp.tripCnt = TripNum;
                        GroupTemp.distance = String.Format("{0:N1}", Math.Round(GroupDistance, 1)) + "";
                        GroupTemp.arrivallocation_province = "";
                        GroupTemp.arrivallocation_city = "";
                        GroupTemp.arrivallocation_district = "";
                        GroupTemp.arrivallocation_street = "";
                        GroupTemp.arrivaltime = "2014-03-22 10:10:10";
                        GroupTemp.leavelocation_province = "";
                        GroupTemp.leavelocation_city = "";
                        GroupTemp.leavelocation_district = "";
                        GroupTemp.leavelocation_street = "";
                        GroupTemp.leavetime = "2014-03-22 10:10:10";
                        GroupTemp.tripdistance = "";
                        GroupTemp.tripidlerate = "";
                        GroupTemp.tripidletime = "";
                        GroupTemp.tripavespeed = "";
                        GroupTemp.group = groupList[iGroupNum].name.Trim(); ;
                        GroupTemp.id = iNumForID;
                        if (tempGroup.TotalSeconds == 0)
                        {
                            GroupTemp.idleDivide = "0%";
                        }
                        else
                        {
                            GroupTemp.idleDivide = Math.Round(((double)(GroupIdle) / (double)(tempGroup.TotalSeconds) * 100), 2) + "%";
                        }
                        GroupTemp.triptime = "";
                        GroupTemp.utilization = "";
                        GroupTemp.isFirstFlag = 0;
                        GroupTemp.isLastFlag = 0;
                        tripList.Add(GroupTemp);
                        ++iNumForID;
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupTripInfo End");
                return iNumForID;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public void AddVehicleNotInGroupTripInfo(List<Vehicle> vehicleAllList, List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, int iNumForID, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfo Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }

                if (vehiclenogroup.Count() != 0)
                {
                    VehicleDBInterface db = new VehicleDBInterface();
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));

                    double Idle = 0.0;
                    double Distance = 0.0;

                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            int daygap = ((TimeSpan)(enddate - startdate)).Days;
                            int igap = 1;
                            for (DateTime d = enddate.AddHours(0 - itimezone); d > startdate.AddHours(0 - itimezone); )
                            {
                                List<Trip> tripDay = trip.FindAll(x => (x.startTime >= d.AddDays(-1)
                                   && x.startTime <= d)
                                   );
                                if (tripDay.Count() == 0)
                                {
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                    continue;
                                }
                                else
                                {
                                    tripDay.Sort((x, y) => y.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss").CompareTo(x.startTime.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    temp.name = vehiclenogroup[iVehicleNum].name.Trim();
                                    temp.date = (((DateTime)tripDay[0].startTime).AddHours(itimezone)).ToString("yyyy-MM-dd");

                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence.Trim();
                                    }
                                    for (int j = 0; j < tripDay.Count(); ++j)
                                    {
                                        tempT += (DateTime)tripDay[j].endtime - (DateTime)tripDay[j].startTime;
                                        if (tripDay[j].IdleTime == null)
                                        {
                                            tripDay[j].IdleTime = 0;
                                        }
                                        if (tripDay[j].distance == null)
                                        {
                                            tripDay[j].distance = 0;
                                        }
                                        Idle += (double)tripDay[j].IdleTime;
                                        Distance += (double)tripDay[j].distance;
                                    }
                                    SetDrivingTimeForPage(temp, tempT.Hours + tempT.Days * 24, tempT.Minutes, tempT.Seconds);
                                    SetIdleTimeForPage(temp, ((int)Idle) / 3600, ((int)Idle) / 60 - ((int)Idle) / 3600 * 60, ((int)Idle) % 60);
                                    if (tempT.TotalSeconds == 0)
                                    {
                                        temp.idleDivide = "0%";
                                    }
                                    else
                                    {
                                        temp.idleDivide = Math.Round(((double)(Idle) / (double)(tempT.TotalSeconds) * 100), 2) + "%";
                                    }
                                    temp.tripCnt = tripDay.Count();
                                    temp.distance = String.Format("{0:N1}", Math.Round(Distance, 1)) + "";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.arrivaltime = "2014-03-22 10:10:10";
                                    temp.leavelocation_province = "";
                                    temp.leavelocation_city = "";
                                    temp.leavelocation_district = "";
                                    temp.leavelocation_street = "";
                                    temp.leavetime = "2014-03-22 10:10:10";
                                    temp.tripdistance = "";
                                    temp.tripidlerate = "";
                                    temp.tripidletime = "";
                                    temp.tripavespeed = "";
                                    temp.group = ihpleD_String_cn.export_nogroup;
                                    temp.id = iNumForID;
                                    temp.triptime = "";
                                    temp.startlocctionlat = 0;
                                    temp.startlocctionlng = 0;
                                    temp.endlocctionlat = 0;
                                    temp.endlocctionlng = 0;
                                    temp.guid = "";
                                    temp.utilization = "";
                                    temp.isFirstFlag = 0;
                                    temp.isLastFlag = 0;
                                    tripList.Add(temp);

                                    Idle = 0.0;
                                    Distance = 0.0;
                                    d = enddate.AddDays(0 - igap).AddHours(0 - itimezone);
                                    ++igap;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfo End");
                return;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> VehicleTripInfoForCSV(List<TripReport> tripList, string companyid, DateTime startdate, DateTime enddate, int itimezone)
        {
            DebugLog.Debug("SettingController VehicleTripInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);

                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    AddVehicleInGroupTripInfoForCSV(groupList, tripList, vehiclehasOBU, vehicleList, startdate, enddate, iGroupNum, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupTripInfoForCSV(vehicleAllList, tripList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, groupList, itimezone);
                DebugLog.Debug("SettingController VehicleTripInfoForCSV End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AddVehicleInGroupTripInfoForCSV(List<VehicleGroup> groupList, List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int iGroupNum, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupTripInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return tripList;
                }
                else
                {
                    //取得选择的时间以及当月天数
                    double Distance = 0.0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    List<string> listStrTripTemp = new List<string>();
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> triplist = new List<Trip>();
                            triplist = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (triplist.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = triplist.Count - 1; i >= 0; --i)
                                {
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    {
                                        temp.name = vehicleList[iVehicleNum].name.Trim();
                                    }
                                    temp.date = ((DateTime)triplist[i].startTime.Value.AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehicleList[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehicleList[iVehicleNum].licence;
                                    }
                                    tempT = (DateTime)triplist[i].endtime - (DateTime)triplist[i].startTime;
                                    if (triplist[i].IdleTime == null)
                                    {
                                        triplist[i].IdleTime = 0;
                                    }
                                    if (triplist[i].distance == null)
                                    {
                                        triplist[i].distance = 0;
                                    }

                                    temp.drivingTime = Math.Round(((double)(tempT.TotalSeconds) / (double)(3600) * 60), 2) + "";

                                    temp.idleTime = Math.Round(((double)(triplist[i].IdleTime) / (double)(3600) * 60), 2) + "";
                                    if (tempT.TotalSeconds == 0)
                                    {
                                        temp.idleDivide = "0";
                                    }
                                    else
                                    {
                                        temp.idleDivide = Math.Round(((double)(triplist[i].IdleTime) / (double)(tempT.TotalSeconds) * 100), 2) + "";
                                    }
                                    temp.tripCnt = triplist.Count();
                                    temp.distance = Math.Round(Distance, 1) + "";
                                    //List<string> listStrTripArrival = new List<string>();
                                    //List<string> listStrTripLeave = new List<string>();
                                    listStrTripTemp.Clear();
                                    if (triplist[i].endlocation == null)
                                    {
                                        SetAddressInfo(String.Empty, listStrTripTemp);
                                    }
                                    else
                                    {
                                        SetAddressInfo(triplist[i].endlocation, listStrTripTemp);
                                    }
                                    temp.arrivallocation_province = listStrTripTemp[0];
                                    temp.arrivallocation_city = listStrTripTemp[1];
                                    temp.arrivallocation_district = listStrTripTemp[2];
                                    temp.arrivallocation_street = listStrTripTemp[3];

                                    listStrTripTemp.Clear();
                                    temp.arrivaltime = ((DateTime)triplist[i].endtime).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    if (triplist[i].startlocation == null)
                                    {
                                        SetAddressInfo(String.Empty, listStrTripTemp);
                                    }
                                    else
                                    {
                                        SetAddressInfo(triplist[i].startlocation, listStrTripTemp);
                                    }
                                    temp.leavelocation_province = listStrTripTemp[0];
                                    temp.leavelocation_city = listStrTripTemp[1];
                                    temp.leavelocation_district = listStrTripTemp[2];
                                    temp.leavelocation_street = listStrTripTemp[3];
                                    temp.leavetime = ((DateTime)triplist[i].startTime).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    temp.tripdistance = String.Format("{0:N1}", triplist[i].distance);
                                    if (triplist[i].startlocationLat == null)
                                    {
                                        temp.startlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlat = (double)triplist[i].startlocationLat;
                                    }
                                    if (triplist[i].startlocationLng == null)
                                    {
                                        temp.startlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlng = (double)triplist[i].startlocationLng;
                                    }
                                    if (triplist[i].endlocationLat == null)
                                    {
                                        temp.endlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlat = (double)triplist[i].endlocationLat;
                                    }
                                    if (triplist[i].endlocationLng == null)
                                    {
                                        temp.endlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlng = (double)triplist[i].endlocationLng;
                                    }
                                    if (triplist[i].guid == null)
                                    {
                                        temp.guid = "";
                                    }
                                    else
                                    {
                                        temp.guid = triplist[i].guid;
                                    }
                                    temp.tripidlerate = temp.idleDivide;
                                    temp.tripidletime = temp.idleTime;
                                    if ((tempT.TotalSeconds - triplist[i].IdleTime) == 0)
                                    {
                                        temp.tripavespeed = "0";
                                    }
                                    else
                                    {
                                        temp.tripavespeed = Math.Round(((double)triplist[i].distance / ((double)(tempT.TotalSeconds - triplist[i].IdleTime) / 3600)), 2) + "";
                                    }
                                    temp.group = groupList[iGroupNum].name.Trim();
                                    temp.id = iGroupNum;
                                    temp.triptime = temp.drivingTime;
                                    temp.utilization = "";
                                    if (null == triplist[i].isFirstFlag)
                                    {
                                        temp.isFirstFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isFirstFlag = (int)triplist[i].isFirstFlag;
                                    }
                                    if (null == triplist[i].isLastFlag)
                                    {
                                        temp.isLastFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isLastFlag = (int)triplist[i].isLastFlag;
                                    }
                                    tripList.Add(temp);

                                    Distance = 0.0;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupTripInfoForCSV End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        public List<TripReport> AddVehicleNotInGroupTripInfoForCSV(List<Vehicle> vehicleAllList, List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, List<VehicleGroup> groupList, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfoForCSV Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }
                if (vehiclenogroup.Count() == 0)
                {
                    return tripList;
                }
                else
                {
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));
                    double Distance = 0.0;
                    VehicleDBInterface db = new VehicleDBInterface();
                    List<string> listStrTripTemp = new List<string>();
                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> triplist = new List<Trip>();
                            triplist = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (triplist.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = triplist.Count - 1; i >= 0; --i)
                                {
                                    TripReport temp = new TripReport();
                                    TimeSpan tempT = new TimeSpan();
                                    {
                                        temp.name = vehiclenogroup[iVehicleNum].name.Trim();
                                    }
                                    temp.date = ((DateTime)triplist[i].startTime.Value.AddHours(itimezone)).ToString("yyyy-MM-dd");
                                    if (vehiclenogroup[iVehicleNum].licence == null)
                                    {
                                        temp.licence = "";
                                    }
                                    else
                                    {
                                        temp.licence = vehiclenogroup[iVehicleNum].licence;
                                    }
                                    tempT = (DateTime)triplist[i].endtime - (DateTime)triplist[i].startTime;
                                    if (triplist[i].IdleTime == null)
                                    {
                                        triplist[i].IdleTime = 0;
                                    }
                                    if (triplist[i].distance == null)
                                    {
                                        triplist[i].distance = 0;
                                    }

                                    temp.drivingTime = Math.Round(((double)(tempT.TotalSeconds) / (double)(3600) * 60), 2) + "";

                                    temp.idleTime = Math.Round(((double)(triplist[i].IdleTime) / (double)(3600) * 60), 2) + "";
                                    if (tempT.TotalSeconds == 0)
                                    {
                                        temp.idleDivide = "0";
                                    }
                                    else
                                    {
                                        temp.idleDivide = Math.Round(((double)(triplist[i].IdleTime) / (double)(tempT.TotalSeconds) * 100), 2) + "";
                                    }
                                    temp.tripCnt = triplist.Count();
                                    temp.distance = Math.Round(Distance, 1) + "";
                                    //List<string> listStrTripArrival = new List<string>();
                                    //List<string> listStrTripLeave = new List<string>();
                                    listStrTripTemp.Clear();
                                    if (triplist[i].endlocation == null)
                                    {
                                        SetAddressInfo(String.Empty, listStrTripTemp);
                                    }
                                    else
                                    {
                                        SetAddressInfo(triplist[i].endlocation, listStrTripTemp);
                                    }
                                    temp.arrivallocation_province = listStrTripTemp[0];
                                    temp.arrivallocation_city = listStrTripTemp[1];
                                    temp.arrivallocation_district = listStrTripTemp[2];
                                    temp.arrivallocation_street = listStrTripTemp[3];
                                    temp.arrivaltime = ((DateTime)triplist[i].endtime).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    listStrTripTemp.Clear();
                                    if (triplist[i].startlocation == null)
                                    {
                                        SetAddressInfo(String.Empty, listStrTripTemp);
                                    }
                                    else
                                    {
                                        SetAddressInfo(triplist[i].startlocation, listStrTripTemp);
                                    }
                                    temp.leavelocation_province = listStrTripTemp[0];
                                    temp.leavelocation_city = listStrTripTemp[1];
                                    temp.leavelocation_district = listStrTripTemp[2];
                                    temp.leavelocation_street = listStrTripTemp[3];
                                    temp.leavetime = ((DateTime)triplist[i].startTime).AddHours(itimezone).ToString("yyyy-MM-dd HH:mm:ss");
                                    temp.tripdistance = String.Format("{0:N1}", triplist[i].distance);
                                    if (triplist[i].startlocationLat == null)
                                    {
                                        temp.startlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlat = (double)triplist[i].startlocationLat;
                                    }
                                    if (triplist[i].startlocationLng == null)
                                    {
                                        temp.startlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlng = (double)triplist[i].startlocationLng;
                                    }
                                    if (triplist[i].endlocationLat == null)
                                    {
                                        temp.endlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlat = (double)triplist[i].endlocationLat;
                                    }
                                    if (triplist[i].endlocationLng == null)
                                    {
                                        temp.endlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlng = (double)triplist[i].endlocationLng;
                                    }
                                    if (triplist[i].guid == null)
                                    {
                                        temp.guid = "";
                                    }
                                    else
                                    {
                                        temp.guid = triplist[i].guid;
                                    }
                                    temp.tripidlerate = temp.idleDivide;
                                    temp.tripidletime = temp.idleTime;
                                    if ((tempT.TotalSeconds - triplist[i].IdleTime) == 0)
                                    {
                                        temp.tripavespeed = "0";
                                    }
                                    else
                                    {
                                        temp.tripavespeed = Math.Round(((double)triplist[i].distance / ((double)(tempT.TotalSeconds - triplist[i].IdleTime) / 3600)), 2) + "";
                                    }
                                    temp.group = ihpleD_String_cn.export_nogroup;
                                    temp.id = groupList.Count(); ;
                                    temp.triptime = temp.drivingTime;
                                    temp.utilization = "";
                                    if (null == triplist[i].isFirstFlag)
                                    {
                                        temp.isFirstFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isFirstFlag = (int)triplist[i].isFirstFlag;
                                    }
                                    if (null == triplist[i].isLastFlag)
                                    {
                                        temp.isLastFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isLastFlag = (int)triplist[i].isLastFlag;
                                    }
                                    tripList.Add(temp);

                                    Distance = 0.0;
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfoForCSV End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        
        private Dictionary<string, object> getFuelInfoInMonthForPage(DateTime startdate, DateTime enddate, string intkey)
        {
            try
            {
                DebugLog.Debug("SettingController getFuelInfoInMonthForPage Start");
                List<FuelReport> tripList = new List<FuelReport>();
                tripList = getFuelInfoInMonth();

                Dictionary<string, object> dic = new Dictionary<string, object>();

                int PageNum = 0;
                int CurrentPageNum = int.Parse(intkey);
                if (tripList.Count() % Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) == 0)
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]);
                }
                else
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) + 1;
                }
                dic.Add("pagecount", PageNum);
                List<FuelReport> fuelListForPage = new List<FuelReport>();
                if (CurrentPageNum <= PageNum)
                {
                    if (CurrentPageNum != PageNum)
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < CurrentPageNum * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); ++i)
                        {
                            fuelListForPage.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < tripList.Count(); ++i)
                        {
                            fuelListForPage.Add(tripList[i]);
                        }
                    }
                }
                dic.Add("dataList", fuelListForPage);
                DebugLog.Debug("SettingController getFuelInfoInMonthForPage End");
                return dic;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        private Dictionary<string, object> getHealthInfoInMonthForPage(DateTime startdate, DateTime enddate, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getHealthInfoInMonthForPage Start");
                List<HealthReport> tripList = new List<HealthReport>();
                tripList = getWarnInfoInMonth(startdate, enddate, timezone);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                int PageNum = 0;
                int CurrentPageNum = int.Parse(intkey);
                if (tripList.Count() % Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) == 0)
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]);
                }
                else
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) + 1;
                }
                dic.Add("pagecount", PageNum);
                List<HealthReport> healthListForPage = new List<HealthReport>();
                if (CurrentPageNum <= PageNum)
                {
                    if (CurrentPageNum != PageNum)
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < CurrentPageNum * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); ++i)
                        {
                            healthListForPage.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < tripList.Count(); ++i)
                        {
                            healthListForPage.Add(tripList[i]);
                        }
                    }
                }
                dic.Add("dataList", healthListForPage);
                DebugLog.Debug("SettingController getHealthInfoInMonthForPage End");
                return dic;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        private Dictionary<string, object> getUtilizationInfoInMonthForPage(DateTime startdate, DateTime enddate, string intkey, string timezone)
        {
            try
            {
                DebugLog.Debug("SettingController getUtilizationInfoInMonthForPage Start");
                List<TripReport> tripList = new List<TripReport>();
                tripList = getUtilizationInMonth(startdate, enddate, timezone);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                int PageNum = 0;
                int CurrentPageNum = int.Parse(intkey);
                if (tripList.Count() % Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) == 0)
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]);
                }
                else
                {
                    PageNum = tripList.Count() / Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]) + 1;
                }
                dic.Add("pagecount", PageNum);
                List<TripReport> utilizationListForPage = new List<TripReport>();
                if (CurrentPageNum <= PageNum)
                {
                    if (CurrentPageNum != PageNum)
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < CurrentPageNum * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); ++i)
                        {
                            utilizationListForPage.Add(tripList[i]);
                        }
                    }
                    else
                    {
                        for (int i = (CurrentPageNum - 1) * Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["CountNumForExport"]); i < tripList.Count(); ++i)
                        {
                            utilizationListForPage.Add(tripList[i]);
                        }
                    }
                }
                dic.Add("dataList", utilizationListForPage);
                DebugLog.Debug("SettingController getUtilizationInfoInMonthForPage End");
                return dic;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        public void WriteLocationToDB(String start, String end)
        {
            try
            {
                DebugLog.Debug("SettingController WriteLocationToDB Start");
                string[] strStart = start.Split('+');
                string[] strEnd = end.Split('+');
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                vehicleInterface.WriteLocationToDBReport(strStart, strEnd);
                DebugLog.Debug("SettingController WriteLocationToDB End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        private void SetAddressInfo(String str, List<string> liststring)
        {
            //DebugLog.Debug("SettingController SetAddressInfo Start");
            string province = "";
            string city = "";
            string district = "";
            string street = "";

            string[] add = str.Split(',');
            switch (add.Length)
            {
                case 1:
                    province = ihpleD_String_cn.page_report_unknownaddress;
                    city = ihpleD_String_cn.page_report_unknownaddress;
                    district = ihpleD_String_cn.page_report_unknownaddress;
                    street = ihpleD_String_cn.page_report_unknownaddress;
                    break;
                case 3:
                    if (add[0].IndexOf("*") != -1)
                    {
                        province = add[0];
                        city = "*" + add[1];
                        district = "*" + add[2];
                        street = "";
                    }
                    else
                    {
                        province = add[0];
                        city = add[1];
                        district = add[2];
                        street = "";
                    }
                    break;
                case 4:
                    if (add[0].IndexOf("*") != -1)
                    {
                        province = add[0];
                        city = "*" + add[1];
                        district = "*" + add[2];
                        street = "*" + add[3]; ;
                    }
                    else
                    {
                        province = add[0];
                        city = add[1];
                        district = add[2];
                        street = add[3];
                    }
                    break;
                case 5:
                    if (add[0].IndexOf("*") != -1)
                    {
                        province = add[0];
                        city = "*" + add[1];
                        district = "*" + add[2];
                        street = "*" + add[3] + add[4];
                    }
                    else
                    {
                        province = add[0];
                        city = add[1];
                        district = add[2];
                        street = add[3] + add[4];
                    }
                    break;
                default:
                    province = ihpleD_String_cn.page_report_unknownaddress;
                    city = ihpleD_String_cn.page_report_unknownaddress;
                    district = ihpleD_String_cn.page_report_unknownaddress;
                    street = ihpleD_String_cn.page_report_unknownaddress;
                    break;
            }

            liststring.Add(province);
            liststring.Add(city);
            liststring.Add(district);
            liststring.Add(street);
            //DebugLog.Debug("SettingController SetAddressInfo End");
        }

        public int GetTotalTrips(string companyid, DateTime startdate, DateTime enddate,int itimezone)
        {
            DebugLog.Debug("SettingController GetTotalTrips Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int totalnum = 0;

                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);
                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    List<Trip> trip = new List<Trip>();
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count; ++iVehicleNum)
                    {
                        trip = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                        totalnum += trip.Count;
                    }
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                List<Trip> trip_g = new List<Trip>();

                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }

                for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count; ++iVehicleNum)
                {
                    trip_g = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                    totalnum += trip_g.Count;
                }
                DebugLog.Debug("SettingController GetTotalTrips End");
                return totalnum;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        public List<TripReport> VehicleTripInfoForAddress(List<TripReport> tripList, string companyid, DateTime startdate, DateTime enddate, int itimezone)
        {
            DebugLog.Debug("SettingController VehicleTripInfoForAddress Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                VehicleDBInterface db = new VehicleDBInterface();
                //有分组的车辆的处理
                List<Vehicle> vehicleList = new List<Vehicle>();
                List<VehicleGroup> groupList = new List<VehicleGroup>();
                //取得组
                groupList = db.GetGroupsByCompannyID(companyid);
                //对分组按照名称排序
                groupList.Sort((x, y) => x.name.CompareTo(y.name));
                //取得插有OBU的车辆
                List<Vehicle> vehiclehasOBU = new List<Vehicle>();
                vehiclehasOBU = db.GetTenantVehiclesByCompannyID(companyid);

                for (int iGroupNum = 0; iGroupNum < groupList.Count(); ++iGroupNum)
                {
                    //通过分组ID取得车辆
                    vehicleList = db.GetGroupVehiclesByGroupId(groupList[iGroupNum].pkid);
                    InitializationVehicleName(vehicleList);
                    vehicleList.Sort((x, y) => x.name.CompareTo(y.name));
                    AddVehicleInGroupTripInfoForAddress(tripList, vehiclehasOBU, vehicleList, startdate, enddate, itimezone);
                }

                //无分组的车辆的处理
                List<Vehicle> vehicleAllList = new List<Vehicle>();
                vehicleAllList = db.GetTenantVehiclesByCompannyID(companyid);
                List<Vehicle> vehicleingroupList = new List<Vehicle>();
                vehicleingroupList = db.GetGroupVehicles();
                List<Vehicle> vehiclenogroup = new List<Vehicle>();
                AddVehicleNotInGroupTripInfoForAddress(vehicleAllList, tripList, vehiclehasOBU, vehicleingroupList, startdate, enddate, vehiclenogroup, groupList, itimezone);
                DebugLog.Debug("SettingController VehicleTripInfoForAddress End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
        public List<TripReport> AddVehicleInGroupTripInfoForAddress(List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleList, DateTime startdate, DateTime enddate, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleInGroupTripInfoForAddress Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                if (vehicleList.Count() == 0)
                {
                    return tripList;
                }
                else
                {
                    //取得选择的时间以及当月天数
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehicleList.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehicleList.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> triplist = new List<Trip>();
                            triplist = db.GetTripsByVehicleIDAnd2TimeForReport(vehicleList[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (triplist.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = triplist.Count - 1; i >= 0; --i)
                                {
                                    TripReport temp = new TripReport();

                                    if (triplist[i].startlocationLat == null)
                                    {
                                        temp.startlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlat = (double)triplist[i].startlocationLat;
                                    }
                                    if (triplist[i].startlocationLng == null)
                                    {
                                        temp.startlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlng = (double)triplist[i].startlocationLng;
                                    }
                                    if (triplist[i].endlocationLat == null)
                                    {
                                        temp.endlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlat = (double)triplist[i].endlocationLat;
                                    }
                                    if (triplist[i].endlocationLng == null)
                                    {
                                        temp.endlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlng = (double)triplist[i].endlocationLng;
                                    }
                                    if (triplist[i].guid == null)
                                    {
                                        temp.guid = "";
                                    }
                                    else
                                    {
                                        temp.guid = triplist[i].guid;
                                    }
                                    if (null == triplist[i].isFirstFlag)
                                    {
                                        temp.isFirstFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isFirstFlag = (int)triplist[i].isFirstFlag;
                                    }
                                    if (null == triplist[i].isLastFlag)
                                    {
                                        temp.isLastFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isLastFlag = (int)triplist[i].isLastFlag;
                                    }
                                    if (triplist[i].endlocation == null || triplist[i].endlocation == "")
                                    {
                                        temp.arrivallocation_province = "";
                                    }
                                    else
                                    {
                                        temp.arrivallocation_province = triplist[i].endlocation.Trim();
                                    }
                                    if (triplist[i].startlocation == null || triplist[i].startlocation == "")
                                    {
                                        temp.leavelocation_province = "";
                                    }
                                    else
                                    {
                                        temp.leavelocation_province = triplist[i].startlocation.Trim();
                                    }

                                    GeoPointDTO point = new GeoPointDTO();
                                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(temp.endlocctionlng, temp.endlocctionlat);
                                    temp.endlocctionlng = point.longitude;
                                    temp.endlocctionlat = point.latitude;
                                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(temp.startlocctionlng, temp.startlocctionlat);
                                    temp.startlocctionlng = point.longitude;
                                    temp.startlocctionlat = point.latitude;
                                    tripList.Add(temp);
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleInGroupTripInfoForAddress End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        public List<TripReport> AddVehicleNotInGroupTripInfoForAddress(List<Vehicle> vehicleAllList, List<TripReport> tripList, List<Vehicle> vehiclehasOBU, List<Vehicle> vehicleingroupList, DateTime startdate, DateTime enddate, List<Vehicle> vehiclenogroup, List<VehicleGroup> groupList, int itimezone)
        {
            DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfoForAddress Start");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN");
            try
            {
                int iGroupAll = 0;
                int iGroupInGroup = 0;
                for (iGroupAll = 0; iGroupAll < vehicleAllList.Count(); ++iGroupAll)
                {
                    for (iGroupInGroup = 0; iGroupInGroup < vehicleingroupList.Count(); ++iGroupInGroup)
                    {
                        if (vehicleAllList[iGroupAll].pkid == vehicleingroupList[iGroupInGroup].pkid)
                        {
                            break;
                        }
                    }
                    if (iGroupInGroup == vehicleingroupList.Count())
                    {
                        vehiclenogroup.Add(vehicleAllList[iGroupAll]);
                    }
                }
                if (vehiclenogroup.Count() == 0)
                {
                    return tripList;
                }
                else
                {
                    InitializationVehicleName(vehiclenogroup);
                    vehiclenogroup.Sort((x, y) => x.name.CompareTo(y.name));
                    VehicleDBInterface db = new VehicleDBInterface();
                    for (int iVehicleNum = 0; iVehicleNum < vehiclenogroup.Count(); ++iVehicleNum)
                    {
                        if (null != vehiclehasOBU.Find(t => t.pkid == vehiclenogroup.ElementAt(iVehicleNum).pkid))
                        {
                            List<Trip> triplist = new List<Trip>();
                            triplist = db.GetTripsByVehicleIDAnd2TimeForReport(vehiclenogroup[iVehicleNum].pkid, startdate.AddHours(0 - itimezone), enddate.AddHours(0 - itimezone));
                            if (triplist.Count == 0)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = triplist.Count - 1; i >= 0; --i)
                                {
                                    TripReport temp = new TripReport();

                                    if (triplist[i].startlocationLat == null)
                                    {
                                        temp.startlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlat = (double)triplist[i].startlocationLat;
                                    }
                                    if (triplist[i].startlocationLng == null)
                                    {
                                        temp.startlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.startlocctionlng = (double)triplist[i].startlocationLng;
                                    }
                                    if (triplist[i].endlocationLat == null)
                                    {
                                        temp.endlocctionlat = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlat = (double)triplist[i].endlocationLat;
                                    }
                                    if (triplist[i].endlocationLng == null)
                                    {
                                        temp.endlocctionlng = 0;
                                    }
                                    else
                                    {
                                        temp.endlocctionlng = (double)triplist[i].endlocationLng;
                                    }
                                    if (triplist[i].guid == null)
                                    {
                                        temp.guid = "";
                                    }
                                    else
                                    {
                                        temp.guid = triplist[i].guid;
                                    }
                                    
                                    if (null == triplist[i].isFirstFlag)
                                    {
                                        temp.isFirstFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isFirstFlag = (int)triplist[i].isFirstFlag;
                                    }
                                    if (null == triplist[i].isLastFlag)
                                    {
                                        temp.isLastFlag = 0;
                                    }
                                    else
                                    {
                                        temp.isLastFlag = (int)triplist[i].isLastFlag;
                                    }
                                    if (triplist[i].endlocation == null || triplist[i].endlocation == "")
                                    {
                                        temp.arrivallocation_province = "";
                                    }
                                    else
                                    {
                                        temp.arrivallocation_province = triplist[i].endlocation.Trim();
                                    }
                                    if (triplist[i].startlocation == null || triplist[i].startlocation == "")
                                    {
                                        temp.leavelocation_province = "";
                                    }
                                    else
                                    {
                                        temp.leavelocation_province = triplist[i].startlocation.Trim();
                                    }

                                    GeoPointDTO point = new GeoPointDTO();
                                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(temp.endlocctionlng, temp.endlocctionlat);
                                    temp.endlocctionlng = point.longitude;
                                    temp.endlocctionlat = point.latitude;
                                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(temp.startlocctionlng, temp.startlocctionlat);
                                    temp.startlocctionlng = point.longitude;
                                    temp.startlocctionlat = point.latitude;

                                    tripList.Add(temp);
                                }
                            }
                        }
                    }
                }
                DebugLog.Debug("SettingController AddVehicleNotInGroupTripInfoForAddress End");
                return tripList;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }
    }
}
