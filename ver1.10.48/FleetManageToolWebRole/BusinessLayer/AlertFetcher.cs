using FleetManageTool.Models.Common;
using FleetManageTool.Models.page;
using FleetManageTool.WebAPI;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Models.page;
using FleetManageToolWebRole.Util;
using Microsoft.ApplicationServer.Caching;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Resource.String;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class AlertFetcher
    {
        //获取一辆车的电子围栏警告信息
        public List<GeofenceAlert> GetVehicleGeofenceAlertInfo(string companyID, long vehicleID, int pageNum, string month, int timeZone)
        {
            DebugLog.Debug("[AlertFetcher] GetVehicleGeofenceAlertInfo() para(companyID=" + companyID + ";vehicleID=" + vehicleID + ";pageNum=" + pageNum + ";month=" + month + ") Start");
            List<GeofenceAlert> geoAlerts = new List<GeofenceAlert>();
            DateTime startDay = GetDateTimeMonthFirstDay(DateTime.Parse(month));
            DateTime endDay = GetDateTimeMonthLastDay(DateTime.Parse(month));
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                DateTime monthDate = DateTime.Parse(month);
                if (null == monthDate)
                {
                    DebugLog.Debug("[AlertFetcher] GetVehicleGeofenceAlertInfo() End (return null List<GeofenceAlert> becauseOf null month)");
                    return new List<GeofenceAlert>();
                }
                try
                {
                    DateTime startTime = GetDateTimeMonthFirstDay(monthDate);
                    DateTime endTime = GetDateTimeMonthLastDay(monthDate);
                    for (int i = 0; i < 20; i++)
                    {
                        GeofenceAlert temp = new GeofenceAlert();
                        temp.alertTime = startTime.AddDays(i).ToUniversalTime();//显示当前时间/*fengpan 20140324 #832*/
                        if (0 == i % 5)
                        {
                            temp.alertInfo = "进入";
                            temp.geofenceName = "沈阳浑南";
                            //temp.locationName = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                        }
                        else if (1 == i % 5)
                        {
                            temp.alertInfo = "离开";
                            temp.geofenceName = "沈阳浑南";
                            //temp.locationName = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                        }
                        else if (2 == i % 5)
                        {
                            temp.alertInfo = "进入";
                            temp.geofenceName = "沈阳浑南";
                            //temp.locationName = @"沈阳市浑南新区沈营路新秀街东软";
                        }
                        else if (3 == i % 5)
                        {
                            temp.alertInfo = "离开";
                            temp.geofenceName = "沈阳浑南";
                            //temp.locationName = "沈阳市浑南新区沈营路新秀街东软";
                        }
                        else if (4 == i % 5)
                        {
                            temp.alertInfo = "离开";
                            temp.geofenceName = "家";
                            //temp.locationName = "奥体中心";
                        }
                        else
                        {
                            temp.alertInfo = "";
                            temp.geofenceName = "";
                            //temp.locationName = "";
                        }
                        geoAlerts.Add(temp);
                    }
                    DebugLog.Debug("[AlertFetcher] GetVehicleGeofenceAlertInfo() End[geoAlerts.Count=" + geoAlerts.Count + "]");
                    return geoAlerts;
                }
                catch (Exception e)
                {
                    DebugLog.Debug(e.Message);
                    throw new Exception(e.Message);
                }
            }
            else
            {
                try
                {
                    geoAlerts = GetGeofenceAlertInfoFromCacheOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["VehicleDetailOnePageCount"]), timeZone);
                    DebugLog.Debug("[AlertFetcher] GetVehicleGeofenceAlertInfo() End[geoAlerts.Count=" + geoAlerts.Count + "]");
                    return geoAlerts;
                }
                catch (Exception e)
                {
                    DebugLog.Debug(e.Message);
                    Console.WriteLine(e.Message);
                    return geoAlerts;
                }
            }
        }
        public int GetGeofenceAlertNum(string companyID, long vehicleID, DateTime startDay, DateTime endDay, int timeZone)
        {
            int ret = 0;
            FleetInfo fleet = new FleetInfo();
            FleetInfoFetcher fleetInfoFetcher = new FleetInfoFetcher();
            fleet = fleetInfoFetcher.GetInfoFromCache(companyID, -1, timeZone,false);
            VehicleInfo vehicle = fleet.allVehicle.Find(t => (t.primarykey == vehicleID));
            if (null == vehicle)
            {
                vehicle = new VehicleInfo();
                vehicle.alerts = new List<Models.API.Alert>();
                //return new List<VehicleAlert>();
            }
            AlertConfigurationInfo alertConfigs = GetAlertConfigInfoFromDB(companyID);
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            if (null != vehicle.alerts)
            {
                vehicle.alerts = vehicle.alerts.FindAll(t => AlertConfigurationConstant.Geo1.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo2.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo3.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo4.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo5.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo6.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_1.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_2.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_3.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_4.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_5.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_6.Equals(t.AlertType));
                vehicle.alerts.Sort((x, y) => y.TriggeredDateTime.CompareTo(x.TriggeredDateTime));
                if (0 >= vehicle.alerts.Count || vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime > endDay.ToUniversalTime())
                {
                    return vehicleDB.getGeoAlertCount(vehicleID, startDay, endDay);
                }
            }
            int numInDB = 0;
            int numInCache = 0;
            if (null != vehicle.alerts)
            {
                if (vehicle.alerts.Count > 0)
                {
                    numInCache = vehicle.alerts.Count;
                    numInDB = vehicleDB.getGeoAlertCount(vehicleID, startDay, vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime.ToUniversalTime());
                }
                else
                {
                    numInDB = vehicleDB.getGeoAlertCount(vehicleID, startDay, endDay);
                }
            }
            else
            {
                numInDB = vehicleDB.getGeoAlertCount(vehicleID, startDay,endDay);
            }
            ret = numInCache + numInDB;
            return ret;
        }
        //从Cache中获取GeofenceAlert历史信息中的一页
        public List<GeofenceAlert> GetGeofenceAlertInfoFromCacheOfOnePage(string companyID, long vehicleID, DateTime startDay, DateTime endDay, int pageNum, int pageSize, int timeZone)
        {
            DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() para( companyID=" + companyID + ";vehicleID=" + vehicleID + ";pageNum=" + pageNum + " ) Start");
            List<GeofenceAlert> geoAlerts = new List<GeofenceAlert>();
            try
            {
                FleetInfo fleet = new FleetInfo();
                FleetInfoFetcher fleetInfoFetcher = new FleetInfoFetcher();
                fleet = fleetInfoFetcher.GetInfoFromCache(companyID, -1, timeZone,false);
                VehicleInfo vehicle = fleet.allVehicle.Find(t => (t.primarykey == vehicleID));
                if (null == vehicle)
                {
                    vehicle = new VehicleInfo();
                    //return new List<VehicleAlert>();
                }
                if (null != vehicle.alerts)
                {
                    vehicle.alerts = vehicle.alerts.FindAll(t => AlertConfigurationConstant.Geo1.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo2.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo3.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo4.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo5.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Geo6.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_1.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_2.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_3.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_4.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_5.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.GEO_6.Equals(t.AlertType) );
                    vehicle.alerts.Sort((x, y) => y.TriggeredDateTime.CompareTo(x.TriggeredDateTime));
                    if (0 >= vehicle.alerts.Count || vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime > endDay.ToUniversalTime())
                    {
                        return GetGeofenceAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, 10);
                    }
                    for (int i = (pageNum - 1) * pageSize; i < pageNum * pageSize && i < vehicle.alerts.Count; i++)
                    {
                        GeofenceAlert temp = new GeofenceAlert();
                        TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                        temp.alertTime = ((DateTime)vehicle.alerts.ElementAt(i).TriggeredDateTime).Add(backTimeZone).ToUniversalTime();
                        //json串 转为对象
                        dynamic alertObj = JsonConvert.DeserializeObject(vehicle.alerts.ElementAt(i).Details.ToString());
                        if (null != alertObj["Name"])
                        {
                            temp.geofenceName = alertObj["Name"].Value.ToString();
                        }
                        //temp.locationName = geoDB.GetGeofenceLocationInfoByGeofenceName(alertObj["Name"].Value.ToString());
                        if (null != alertObj["Event"])
                        {
                            if ("Entered".Equals(alertObj["Event"].Value))
                            {
                                temp.alertInfo = ihpleD_String_cn.page_vehicles_Entered;
                            }
                            else if ("Exited".Equals(alertObj["Event"].Value))
                            {
                                temp.alertInfo = ihpleD_String_cn.page_vehicles_Exited;
                            }
                        }
                        geoAlerts.Add(temp);
                    }
                    if (geoAlerts.Count < pageSize)
                    {
                        geoAlerts.AddRange(GetGeofenceAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime.ToUniversalTime(), pageNum, pageSize,vehicle.alerts.Count));
                        //geoAlerts.Reverse();
                        //geoAlerts = geoAlerts.Take(10 - (pageNum * pageSize - vehicle.alerts.Count - geoAlerts.Count)).ToList();
                        geoAlerts = geoAlerts.Take(pageSize).ToList();
                    }
                }
                else
                {
                    geoAlerts = GetGeofenceAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, 10);
                }
                DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() End[geoAlerts.Count=" + geoAlerts.Count + "]");
                return geoAlerts;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() End[return null List<GeofenceAlert> becauseOf Exception]");
                return new List<GeofenceAlert>();
            }
        }
        //从DB中获取GeofenceAlert历史信息中的一页
        public List<GeofenceAlert> GetGeofenceAlertInfoFromDBOfOnePage(string companyID, long vehicleID,  DateTime startDay, DateTime endDay,int pageNum, int pageSize, int cacheCount = 0)
        {
            DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() para( companyID=" + companyID + ";vehicleID=" + vehicleID + ";pageNum=" + pageNum + " ) Start");
            List<GeofenceAlert> geoAlerts = new List<GeofenceAlert>();
            try
            {
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                List<Models.Alert> alerts = vehicleDB.getGeoAlertPage(vehicleID, startDay.ToUniversalTime(), endDay.ToUniversalTime(), pageNum, pageSize,cacheCount);
                GeofenceDBInterface geoDB = new GeofenceDBInterface();
                for (int i = 0; i < alerts.Count; i++)
                {
                    GeofenceAlert temp = new GeofenceAlert();
                    //AlertConfigurationInfo alertConfigs = GetAlertConfigInfoFromDB(companyID);
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    temp.alertTime = ((DateTime)alerts.ElementAt(i).TriggeredDateTime).Add(backTimeZone).ToUniversalTime();
                    //json串 转为对象
                    dynamic alertObj = JsonConvert.DeserializeObject(alerts.ElementAt(i).Value);
                    if (null != alertObj["Name"])
                    {
                        temp.geofenceName = alertObj["Name"].Value.ToString();
                    }
                    //temp.locationName = geoDB.GetGeofenceLocationInfoByGeofenceName(alertObj["Name"].Value.ToString());
                    if (null != alertObj["Event"])
                    {
                        if ("Entered".Equals(alertObj["Event"].Value))
                        {
                            temp.alertInfo = ihpleD_String_cn.page_vehicles_Entered;
                        }
                        else if ("Exited".Equals(alertObj["Event"].Value))
                        {
                            temp.alertInfo = ihpleD_String_cn.page_vehicles_Exited;
                        }
                    }
                    geoAlerts.Add(temp);
                }
                DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() End[geoAlerts.Count=" + geoAlerts.Count + "]");
                return geoAlerts;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] GetGeofenceAlertInfoFromDBOfOnePage() End[return null List<GeofenceAlert> becauseOf Exception]");
                return new List<GeofenceAlert>();
            }
        }
        //获取一辆车的健康警告信息
        public List<VehicleAlert> GetVehicleAlertInfo(string companyID, long vehicleID, string month, int pageNum, int timeZone)
        {
            DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() para(companyID=" + companyID + ";vehicleID=" + vehicleID + ";month=" + month + ";pageNum=" + pageNum + ") Start");
            DateTime startDay = GetDateTimeMonthFirstDay(DateTime.Parse(month));
            DateTime endDay = GetDateTimeMonthLastDay(DateTime.Parse(month));

            List<VehicleAlert> vehicleAlerts = new List<VehicleAlert>();
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                DateTime monthDate = DateTime.Parse(month);
                if (null == monthDate)
                {
                    DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() End (return vehicleAlerts.Count=" + vehicleAlerts.Count + ")");
                    return vehicleAlerts;
                }
                try
                {
                    DateTime startTime = GetDateTimeMonthFirstDay(monthDate);
                    DateTime endTime = GetDateTimeMonthLastDay(monthDate);
                    for (int i = 0; i < 20; i++)
                    {
                        VehicleAlert temp = new VehicleAlert();
                        temp.detail = new VehicleAlertDetail();
                        temp.alertTime = startTime.AddDays(i).ToUniversalTime();//显示当前时间/*fengpan 20140324 #832*/
                        if (0 == i % 5)
                        {
                            temp.detail.alertInfo = "75";
                            temp.alertType = AlertType.SPEEDALERT;
                        }
                        else if (1 == i % 5)
                        {
                            temp.detail.alertInfo = "75";
                            temp.alertType = AlertType.SPEEDALERT;
                        }
                        else if (2 == i % 5)
                        {
                            temp.detail.alertInfo = "3";
                            temp.alertType = AlertType.MOTIONALERT;
                        }
                        else if (3 == i % 5)
                        {
                            temp.detail.alertInfo = "4000";
                            temp.detail.duration = "5";
                            temp.alertType = AlertType.HIGHPRMALERT;
                        }
                        else if (4 == i % 5)
                        {
                            temp.detail.alertInfo = "4000";
                            temp.detail.duration = "5";
                            temp.alertType = AlertType.HIGHPRMALERT;
                        }
                        else
                        {
                            temp.detail.alertInfo = "";
                            temp.alertType = AlertType.NOALERT;
                        }
                        vehicleAlerts.Add(temp);
                    }
                    DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() End[vehicleAlerts.Count=" + vehicleAlerts.Count + "]");
                    return vehicleAlerts;
                }
                catch (Exception e)
                {
                    DebugLog.Debug(e.Message);
                    DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() End[return null List<VehicleAlert> becauseOf Exception]");
                    return new List<VehicleAlert>();
                }
            }
            else
            {
                try
                {
                    //从Cache中获取
                    vehicleAlerts = GetVehicleAlertInfoOfOnePageFromCache(companyID, vehicleID, startDay, endDay, pageNum, Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["VehicleDetailOnePageCount"]), timeZone);
                    //从DB中获取
                    //vehicleAlerts = GetVehicleAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, 10, timeZone);
                    DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() End[vehicleAlerts.Count=" + vehicleAlerts.Count + "]");
                    return vehicleAlerts;
                }
                catch (Exception e)
                {
                    DebugLog.Debug(e.Message);
                    Console.WriteLine(e.Message);
                    DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfo() End[return null List<VehicleAlert> becauseOf Exception]");
                    return new List<VehicleAlert>();
                }
            }
        }

        public int GetVehicleAlertNum(string companyID, long vehicleID, DateTime startDay, DateTime endDay, int timeZone)
        {
            int ret = 0;
            FleetInfo fleet = new FleetInfo();
            FleetInfoFetcher fleetInfoFetcher = new FleetInfoFetcher();
            fleet = fleetInfoFetcher.GetInfoFromCache(companyID, -1, timeZone,false);
            VehicleInfo vehicle = fleet.allVehicle.Find(t => (t.primarykey == vehicleID));
            if (null == vehicle)
            {
                vehicle = new VehicleInfo();
                //return new List<VehicleAlert>();
            }
            AlertConfigurationInfo alertConfigs = GetAlertConfigInfoFromDB(companyID);
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            if (null != vehicle.alerts)
            {
                vehicle.alerts = vehicle.alerts.FindAll(t => AlertConfigurationConstant.Speed.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.Motion.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.Rpm.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.Speed_cn.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.Motion_cn.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.Rpm_cn.Equals(t.AlertType) ||
                                                            AlertConfigurationConstant.EngineRpm.Equals(t.AlertType));
                vehicle.alerts.Sort((x, y) => y.TriggeredDateTime.CompareTo(x.TriggeredDateTime));
                if (0 >= vehicle.alerts.Count || vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime > endDay.ToUniversalTime())
                {
                    return vehicleDB.getAlertCount(vehicleID, startDay, endDay);
                }
            }
            int numInDB = 0;
            int numInCahce = 0;
            if (null != vehicle.alerts)
            {
                if (vehicle.alerts.Count > 0)
                {
                    numInCahce = vehicle.alerts.Count;
                    numInDB = vehicleDB.getAlertCount(vehicleID, startDay, vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime.ToUniversalTime());
                }
                else 
                {
                    numInDB = vehicleDB.getAlertCount(vehicleID, startDay, endDay);
                }
            }
            else
            { 
                numInDB = vehicleDB.getAlertCount(vehicleID, startDay, endDay);
            }
            ret = numInCahce + numInDB;
            return ret;
        }
        /// <summary>
        /// 从cache中获取第一页车辆alert。
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        public List<VehicleAlert> GetVehicleAlertInfoOfOnePageFromCache(string companyID, long vehicleID, DateTime startDay, DateTime endDay, int pageNum, int pageSize, int timeZone)
        {
            DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfoFromCache() para[companyID=" + companyID + ",vehicleID=" + vehicleID + "] Start");
            List<VehicleAlert> vehicleAlerts = new List<VehicleAlert>();
            try
            {
                FleetInfo fleet = new FleetInfo();
                FleetInfoFetcher fleetInfoFetcher = new FleetInfoFetcher();
                fleet = fleetInfoFetcher.GetInfoFromCache(companyID, -1, timeZone,false);
                VehicleInfo vehicle = fleet.allVehicle.Find(t => (t.primarykey == vehicleID));
                if (null == vehicle)
                {
                    vehicle = new VehicleInfo();
                    //return new List<VehicleAlert>();
                }
                AlertConfigurationInfo alertConfigs = GetAlertConfigInfoFromDB(companyID);
                if (null != vehicle.alerts)
                {
                    vehicle.alerts = vehicle.alerts.FindAll(t => AlertConfigurationConstant.Speed.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Motion.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Rpm.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Speed_cn.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Motion_cn.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.Rpm_cn.Equals(t.AlertType) ||
                                                                AlertConfigurationConstant.EngineRpm.Equals(t.AlertType));
                    vehicle.alerts.Sort((x, y) => y.TriggeredDateTime.CompareTo(x.TriggeredDateTime));
                    if (0 >= vehicle.alerts.Count || vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime > endDay.ToUniversalTime())
                    {
                        return GetVehicleAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, 10, timeZone);
                    }
                    for (int i = (pageNum - 1) * pageSize; i < pageNum * pageSize && i < vehicle.alerts.Count; i++)
                    {
                        VehicleAlert temp = new VehicleAlert();
                        temp.detail = new VehicleAlertDetail();
                        temp.detail.alertInfo = vehicle.alerts.ElementAt(i).Details.LimitValue;
                        temp.alertTime = vehicle.alerts.ElementAt(i).TriggeredDateTime.ToUniversalTime();
                        if (AlertConfigurationConstant.Speed.Equals(vehicle.alerts.ElementAt(i).AlertType) ||
                            AlertConfigurationConstant.Speed_cn.Equals(vehicle.alerts.ElementAt(i).AlertType))
                        {
                            temp.alertType = AlertType.SPEEDALERT;
                            //temp.alertInfo = alertConfigs.speed;
                        }
                        else if (AlertConfigurationConstant.Motion.Equals(vehicle.alerts.ElementAt(i).AlertType) ||
                            AlertConfigurationConstant.Motion_cn.Equals(vehicle.alerts.ElementAt(i).AlertType))
                        {
                            temp.alertType = AlertType.MOTIONALERT;
                            //temp.alertInfo = alertConfigs.motion;
                        }
                        else if (AlertConfigurationConstant.Rpm.Equals(vehicle.alerts.ElementAt(i).AlertType) ||
                            AlertConfigurationConstant.Rpm_cn.Equals(vehicle.alerts.ElementAt(i).AlertType) ||
                            AlertConfigurationConstant.EngineRpm.Equals(vehicle.alerts.ElementAt(i).AlertType))
                        {
                            temp.alertType = AlertType.HIGHPRMALERT;
                            temp.detail.duration = vehicle.alerts.ElementAt(i).Details.DurationThreshold;
                            //temp.alertInfo = alertConfigs.rpm;
                        }
                        else
                        {
                            continue;
                        }
                        vehicleAlerts.Add(temp);
                    }
                    if (vehicleAlerts.Count < pageSize)
                    {
                        vehicleAlerts.AddRange(GetVehicleAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, vehicle.alerts.ElementAt(vehicle.alerts.Count - 1).TriggeredDateTime.ToUniversalTime(), pageNum, pageSize, timeZone, vehicle.alerts.Count));
                        //vehicleAlerts.Reverse();
                        //vehicleAlerts = vehicleAlerts.Take(10 - (pageNum * pageSize - vehicle.alerts.Count - vehicleAlerts.Count)).ToList();
                        vehicleAlerts = vehicleAlerts.Take(pageSize).ToList();
                    }
                }
                else 
                {
                    vehicleAlerts = GetVehicleAlertInfoFromDBOfOnePage(companyID, vehicleID, startDay, endDay, pageNum, 10, timeZone);
                }
                DebugLog.Debug("AlertFetcher GetVehicleAlertInfoFromCache() End");
                return vehicleAlerts;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
                return new List<VehicleAlert>();
            }
        }

        //从DB中获取车辆alert历史信息中的一页
        public List<VehicleAlert> GetVehicleAlertInfoFromDBOfOnePage(string companyID, long vehicleID, DateTime startDay, DateTime endDay, int pageNum, int pageSize ,int timeZone, int cacheCount = 0 )
        {
            DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfoFromDBOfOnePage() para( companyID=" + companyID + ";vehicleID=" + vehicleID + ";pageNum=" + pageNum + " ) Start");
            List<VehicleAlert> vehicleAlerts = new List<VehicleAlert>();
            try
            {
                VehicleDBInterface alertDB = new VehicleDBInterface();
                List<Models.Alert> alerts = alertDB.getAlertPage(vehicleID, startDay, endDay, pageNum, pageSize, cacheCount);
                DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfoFromDBOfOnePage()  call -> alertDB.getAlertPage(vehicleID="+vehicleID+",startDay="+startDay.ToShortDateString()+",endDay="+endDay.ToShortDateString()+",pageNum="+pageNum+",pageSize="+pageSize+")[return alerts.Count=" + alerts.Count + "]");
                for (int i = 0; i < alerts.Count; i++)
                {
                    VehicleAlert temp = new VehicleAlert();
                    temp.detail = new VehicleAlertDetail();
                    AlertConfigurationInfo alertConfigs = GetAlertConfigInfoFromDB(companyID);
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    temp.alertTime = ((DateTime)alerts.ElementAt(i).TriggeredDateTime).Add(backTimeZone).ToUniversalTime();
                    dynamic alertObj = JsonConvert.DeserializeObject(alerts.ElementAt(i).Value);
                    if (null == alertObj["LimitValue"])
                    {
                        temp.detail.alertInfo = null;
                    }
                    else
                    {
                        temp.detail.alertInfo = alertObj["LimitValue"].Value.ToString();
                    }
                    switch (alerts.ElementAt(i).AlertType)
                    {
                        case AlertConfigurationConstant.Speed: 
                            temp.alertType = AlertType.SPEEDALERT; 
                            //temp.alertInfo = alertConfigs.speed; 
                            break;
                        case AlertConfigurationConstant.Speed_cn:
                            temp.alertType = AlertType.SPEEDALERT;
                            //temp.alertInfo = alertConfigs.speed; 
                            break;
                        case AlertConfigurationConstant.Rpm: 
                            temp.alertType = AlertType.HIGHPRMALERT; 
                            if (null != alertObj["DurationThreshold"])
                            {
                                temp.detail.duration = alertObj["DurationThreshold"].Value.ToString();
                            }
                            //temp.alertInfo = alertConfigs.rpm; 
                            break;
                        case AlertConfigurationConstant.Rpm_cn:
                            temp.alertType = AlertType.HIGHPRMALERT;
                            if (null != alertObj["DurationThreshold"])
                            {
                                temp.detail.duration = alertObj["DurationThreshold"].Value.ToString();
                            }
                            //temp.alertInfo = alertConfigs.rpm; 
                            break;
                        case AlertConfigurationConstant.EngineRpm:
                            temp.alertType = AlertType.HIGHPRMALERT;
                            if (null != alertObj["DurationThreshold"])
                            {
                                temp.detail.duration = alertObj["DurationThreshold"].Value.ToString();
                            }
                            //temp.alertInfo = alertConfigs.rpm; 
                            break;
                        case AlertConfigurationConstant.Motion:
                            temp.alertType = AlertType.MOTIONALERT;
                            //temp.alertInfo = alertConfigs.motion;
                            break;
                        case AlertConfigurationConstant.Motion_cn:
                            temp.alertType = AlertType.MOTIONALERT;
                            //temp.alertInfo = alertConfigs.motion;
                            break;
                        default: 
                            break;
                    }
                    vehicleAlerts.Add(temp);
                }
                DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfoFromDBOfOnePage() End[vehicleAlerts.Conut=" + vehicleAlerts.Count + "]");
                return vehicleAlerts;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] GetVehicleAlertInfoFromDBOfOnePage() End[return null List<VehicleAlert> becauseOf Exception]");
                return new List<VehicleAlert>();
            }
        }
        //获取报警配置from DB
        public AlertConfigurationInfo GetAlertConfigInfoFromDB(string companyId)
        {
            DebugLog.Debug("[AlertFetcher] GetAlertConfigInfoFromDB() para( companyID=" + companyId + ") Start");
            AlertConfigurationInfo alertConfigs = new AlertConfigurationInfo();
            try
            {
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                List<Models.Parameter> parameters = alertConfigDB.GetAlertThresholdsByCompanyID(companyId);
                alertConfigs.speed = parameters.Find(t => t.ParameterType.Equals(AlertConfigurationConstant.SPEED_THRESHOLD)).value;
                alertConfigs.motion = parameters.Find(t => t.ParameterType.Equals(AlertConfigurationConstant.MOTION_THRESHOLD)).value;
                alertConfigs.rpm = parameters.Find(t => t.ParameterType.Equals(AlertConfigurationConstant.RPM_THRESHOLD)).value;
                alertConfigs.rpmDuration = parameters.Find(t => t.ParameterType.Equals(AlertConfigurationConstant.RPM_DURATION_THRESHOLD)).value;
                if (null == alertConfigs.speed) alertConfigs.speed = "";
                if (null == alertConfigs.motion) alertConfigs.motion = "";
                if (null == alertConfigs.rpm) alertConfigs.rpm = "";
                if (null == alertConfigs.rpmDuration) alertConfigs.rpmDuration = "";
                DebugLog.Debug("[AlertFetcher] GetAlertConfigInfoFromDB() End(return alertConfigs)[speed=" + alertConfigs.speed + ",rpm=" + alertConfigs.rpm + ",rpmDuring=" + alertConfigs.rpmDuration + ",motion=" + alertConfigs.motion + "]");
                return alertConfigs;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] GetAlertConfigInfoFromDB() End(return alertConfigs)[return null AlertConfigurationInfo becauseOf Exception]");
                return new AlertConfigurationInfo();
            }
        }

        //设置报警配置from API
        public bool SetAlertConfigInfo(string companyId, List<Models.AlertConfiguration> alertConfigurationDBs, int speedThreshold, int rpmThreshold, int rpmTimeThreshold, string motionThreshold, bool setGeoConfigFlag)
        {
            DebugLog.Debug("[AlertFetcher] SetAlertConfigInfo(string companyID) para( companyID=" + companyId + ";speedThreshold=" + speedThreshold + ";rpmThreshold=" + rpmThreshold + ";rpmTimeThreshold=" + rpmTimeThreshold + ";motionThreshold=" + motionThreshold + ") Start");
            Models.AlertConfiguration alertConfigs = new Models.AlertConfiguration();
            try
            {
                AlertConfigurationInfo alertConfigsDB = GetAlertConfigInfoFromDB(companyId);//从数据库获取配置信息
                IHalClient client = HalClient.GetInstance();
                HalLink link = new HalLink() { Href = FleetManageToolWebRole.Models.API.URI.ALERTCONFIGURATIONS, IsTemplated = true };
                VehicleDBInterface VehicleDB = new VehicleDBInterface();
                List<Models.Vehicle> vehicles = VehicleDB.GetTenantVehiclesByCompannyID(companyId);
#if false
                IEnumerable<Task<IHalResult>> tasksQuery =
                    from vehicle in vehicles select ProcessClient(vehicle, client, speedThreshold, rpmThreshold, rpmTimeThreshold, motionThreshold);

                // Use ToArray to execute the query and start the download tasks.
                Task<IHalResult>[] configurationTasks = tasksQuery.ToArray();
#else
                List<Task<IHalResult>> configurationTasks = new List<Task<IHalResult>>();
                List<Models.API.AlertConfigurationParameter> alertParameters = new List<Models.API.AlertConfigurationParameter>();
                Models.API.AlertConfigurationParameter speedAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = AlertConfigurationConstant.SPEED_THRESHOLD, ParameterColumnName = "LimitValue", Value = speedThreshold.ToString() };
                Models.API.AlertConfigurationParameter motionAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = AlertConfigurationConstant.MOTION_THRESHOLD, ParameterColumnName = "LimitValue", Value = motionThreshold.ToString() };
                Models.API.AlertConfigurationParameter rpmAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = AlertConfigurationConstant.RPM_THRESHOLD, ParameterColumnName = "LimitValue", Value = rpmThreshold.ToString() };
                Models.API.AlertConfigurationParameter rpmDurationAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = AlertConfigurationConstant.RPM_DURATION_THRESHOLD, ParameterColumnName = "DurationThreshold", Value = rpmTimeThreshold.ToString() };
                //List<Models.API.AlertConfigurationNotification> notifications = new List<Models.API.AlertConfigurationNotification>();
                //Models.API.AlertConfigurationNotification notification = new Models.API.AlertConfigurationNotification() { NotificationType = "EMAIL", Value = "feng.p@ABCSoft.com" };
                alertParameters.Add(speedAlertParameter);
                alertParameters.Add(motionAlertParameter);
                alertParameters.Add(rpmAlertParameter);
                alertParameters.Add(rpmDurationAlertParameter);

                /********************************/
                Models.Parameter para = new Models.Parameter();
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                for (int i = 0; i < alertParameters.Count; i++)
                {
                    para.pkid = alertConfigDB.GetParameterIDByTenantID(companyId, alertParameters.ElementAt(i).ParameterType);
                    para.value = alertParameters.ElementAt(i).Value;
                    alertConfigDB.UpdateAlertThreshold(companyId, para);
                }
                /********************************/

                foreach (Models.Vehicle vehicle in vehicles)
                {
                    AlertConfigurationDBInterface alertconfigDB = new AlertConfigurationDBInterface();
                    //notifications
                    //notifications.Add(notification);
                    //parameters
                    if (Int32.Parse(alertConfigsDB.speed) != speedThreshold)
                    {
                        List<Models.API.AlertConfigurationParameter> speedAlertParameters = new List<Models.API.AlertConfigurationParameter>();
                        speedAlertParameters.Add(speedAlertParameter);
                        Models.Vehicle_AlertConfiguration speedAlertConfiguration = alertconfigDB.GetVehicleAlertConfigurationByVehicleID(vehicle.pkid, AlertConfigurationConstant.HighSpeed);
                        Task<IHalResult> speedTask = CreatConfigurationTask(alertConfigurationDBs, speedAlertParameters, speedAlertConfiguration);
                        //IHalResult speedResult = speedTask.Result;
                        configurationTasks.Add(speedTask);
                    }

                    if (Int32.Parse(alertConfigsDB.rpm) != rpmThreshold || Int32.Parse(alertConfigsDB.rpmDuration) != rpmTimeThreshold)
                    {
                        List<Models.API.AlertConfigurationParameter> rpmAlertParameters = new List<Models.API.AlertConfigurationParameter>();
                        rpmAlertParameters.Add(rpmAlertParameter);
                        rpmAlertParameters.Add(rpmDurationAlertParameter);
                        Models.Vehicle_AlertConfiguration rpmAlertConfiguration = alertconfigDB.GetVehicleAlertConfigurationByVehicleID(vehicle.pkid, AlertConfigurationConstant.HighRpm);
                        Task<IHalResult> rpmTask = CreatConfigurationTask(alertConfigurationDBs, rpmAlertParameters, rpmAlertConfiguration);
                        //IHalResult rpmResult = rpmTask.Result;
                        configurationTasks.Add(rpmTask);
                    }

                    if (!motionThreshold.Equals(alertConfigsDB.motion))
                    {
                        List<Models.API.AlertConfigurationParameter> motionAlertParameters = new List<Models.API.AlertConfigurationParameter>();
                        motionAlertParameters.Add(motionAlertParameter);
                        Models.Vehicle_AlertConfiguration motionAlertConfiguration = alertconfigDB.GetVehicleAlertConfigurationByVehicleID(vehicle.pkid, AlertConfigurationConstant.HighMotion);
                        Task<IHalResult> motionTask = CreatConfigurationTask(alertConfigurationDBs, motionAlertParameters, motionAlertConfiguration);
                        //IHalResult motionResult = motionTask.Result;
                        configurationTasks.Add(motionTask);
                    }

                    if (setGeoConfigFlag)
                    {
                        if (null != alertconfigDB.GetGeofenceConfigurationsByVehicleID(vehicle.pkid, AlertConfigurationConstant.GeoFence))
                        {
                            List<Task<IHalResult>> geoTasks = new List<Task<IHalResult>>();
                            geoTasks = CreatGeofenceTasks(alertConfigurationDBs, vehicle);
                            configurationTasks.AddRange(geoTasks);
                        }
                    }
					System.Threading.Thread.Sleep(1000);
                }
#endif
                //int insertToDBFlag = 1;

                //foreach (IHalResult result in Task.WhenAll(configurationTasks).Result)
                //{
                //    if (result.Success.Equals(true))
                //    {
                //        //...
                //        insertToDBFlag = 1;
                //        break;
                //    }
                //}
                DebugLog.Debug("[AlertFetcher] SetAlertConfigInfo() creatTasks end");
                IHalResult[] iHalResult = Task.WhenAll(configurationTasks).Result;
                for (int i = 0; i < iHalResult.Length; i++)
                {
                    DebugLog.Debug("[AlertFetcher] SetAlertConfigInfo() iHalResult[" + i + "].Success = " + iHalResult[i].Success);
                }
                //if (!setGeoConfigFlag)
                //{
                    //Models.Parameter para = new Models.Parameter();
                    //AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                    //for (int i = 0; i < alertParameters.Count; i++)
                    //{
                    //    para.pkid = alertConfigDB.GetParameterIDByTenantID(companyId, alertParameters.ElementAt(i).ParameterType);
                    //    para.value = alertParameters.ElementAt(i).Value;
                    //    alertConfigDB.UpdateAlertThreshold(companyId, para);
                    //}
                //}
                DebugLog.Debug("[AlertFetcher] SetAlertConfigInfo(string companyID) End[return true]");
                //if (insertToDBFlag == 0)
                //{
                //    return false;
                //}
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] SetAlertConfigInfo(string companyID) End[return false Exception ：]" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建geofenceTasks
        /// </summary>
        /// <param name="alertConfigurationDBs"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public List<Task<IHalResult>> CreatGeofenceTasks(List<Models.AlertConfiguration> alertConfigurationDBs, Models.Vehicle vehicle)
        {
            DebugLog.Debug("[AlertFetcher] CreatGeofenceTasks start");
            try
            {
                List<Task<IHalResult>> geoTasks = new List<Task<IHalResult>>();
                List<Models.API.AlertConfigurationParameter> geoAlertParameters = new List<Models.API.AlertConfigurationParameter>();
                Models.API.AlertConfigurationParameter para1 = new Models.API.AlertConfigurationParameter() { ParameterType = "ENTER_GEOFENCE", Value = "true" };
                Models.API.AlertConfigurationParameter para2 = new Models.API.AlertConfigurationParameter() { ParameterType = "EXIT_GEOFENCE", Value = "true" };
                geoAlertParameters.Add(para1);
                geoAlertParameters.Add(para2);
                AlertConfigurationDBInterface alertconfigDB = new AlertConfigurationDBInterface();
                List<Models.Vehicle_AlertConfiguration> geoAlertConfigurations = alertconfigDB.GetGeofenceConfigurationsByVehicleID(vehicle.pkid, AlertConfigurationConstant.GeoFence);
                foreach (Models.Vehicle_AlertConfiguration geoAlertConfigurationTemp in geoAlertConfigurations)
                {
                    geoTasks.Add(CreatConfigurationTask(alertConfigurationDBs, geoAlertParameters,geoAlertConfigurationTemp));
                }
                DebugLog.Debug("[AlertFetcher] CreatGeofenceTasks end geoTasks.Count="+geoTasks.Count+"");
                return geoTasks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DebugLog.Debug("[AlertFetcher] CreatGeofenceTasks  Exception" + e.Message);
                return new List<Task<IHalResult>>();
            }
        }

        /// <summary>
        /// 创建tasks
        /// </summary>
        /// <param name="alertConfigurationDBs"></param>
        /// <param name="parameters"></param>
        /// <param name="alertConfiguration"></param>
        /// <returns></returns>
        public Task<IHalResult> CreatConfigurationTask(List<Models.AlertConfiguration> alertConfigurationDBs,List<Models.API.AlertConfigurationParameter> parameters,Models.Vehicle_AlertConfiguration alertConfiguration)
        {
            DebugLog.Debug("[AlertFetcher] CreatConfigurationTask paras[alertConfiguration.category=" + alertConfiguration.category + "] start");
            try
            {
                IHalClient client = HalClient.GetInstance();
                HalLink link = new HalLink() { Href = FleetManageToolWebRole.Models.API.URI.ALERTCONFIGURATIONS, IsTemplated = true };
                Dictionary<string, object> alertParameters = new Dictionary<string, object>();
                List<Models.API.AlertConfigurationNotification> notifications = new List<Models.API.AlertConfigurationNotification>();
                List<Models.Notification> notificationDBs = new List<Models.Notification>();
                foreach (Models.Notification notificationDb in alertConfigurationDBs.ElementAt(0).Notification.ToList())
                {
                    if (null == notificationDb.value || "" == notificationDb.value)
                    {
                        continue;
                    }
                    else
                    {
                        Models.API.AlertConfigurationNotification notification = new Models.API.AlertConfigurationNotification() { NotificationType = notificationDb.type, Value = notificationDb.value };
                        notification.Value = notification.Value.ToLower();
                        notifications.Add(notification);
                    }
                }
                alertParameters["ID"] = alertConfiguration.alertConfigurationGuID;
                alertParameters["Notifications"] = notifications;
                alertParameters["Parameters"] = parameters;
                alertParameters["State"] = "ENABLED";
                DebugLog.Debug("[AlertFetcher] CreatConfigurationTask End");
                return client.Put(link, alertParameters);
            }
            catch (Exception e)
            {
                DebugLog.Debug("[AlertFetcher] CreatConfigurationTask Exception" + e.Message);
                return null;
            }
        }


#if false
        //设置一辆车的alertconfiguration
        private Task<IHalResult> ProcessClient(Models.Vehicle vehielce, HalClient client, int speedThreshold, int rpmThreshold, int rpmTimeThreshold, float motionThreshold)
        {
            DebugLog.Debug("[AlertFetcher] ProcessClient(string companyID) Start");
            try
            {
                AlertConfigurationDBInterface alertconfigDB = new AlertConfigurationDBInterface();
                Models.Vehicle_AlertConfiguration alertConfiguration = alertconfigDB.GetVehicleAlertConfigurationByVehicleID(vehielce.pkid,"");
                HalLink link = new HalLink() { Href = FleetManageToolWebRole.Models.API.URI.VEHICLEALERTCONFIGURATIONS, IsTemplated = true };
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["ID"] = alertConfiguration.alertConfigurationGuID;
                parameters["ID"] = "6b109235-b4d1-41ec-8e10-d6158f7814a6";
                //notifications
                List<Models.API.AlertConfigurationNotification> notifications = new List<Models.API.AlertConfigurationNotification>();
                Models.API.AlertConfigurationNotification notification = new Models.API.AlertConfigurationNotification() { NotificationType = "EMAIL", Value = "yiu@lixar.com" };
                notifications.Add(notification);
                //parameters
                List<Models.API.AlertConfigurationParameter> alertParameters = new List<Models.API.AlertConfigurationParameter>();
                Models.API.AlertConfigurationParameter speedAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = "SpeedThreshold", Value = speedThreshold.ToString() };
                Models.API.AlertConfigurationParameter motionAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = "MotionThreshold", Value = motionThreshold.ToString() };
                Models.API.AlertConfigurationParameter rpmAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = "RpmThreshold", Value = rpmThreshold.ToString() };
                Models.API.AlertConfigurationParameter rpmDurationAlertParameter = new Models.API.AlertConfigurationParameter { ParameterType = "RpmDurationThreshold", Value = rpmTimeThreshold.ToString() };
                alertParameters.Add(speedAlertParameter);
                //alertParameters.Add(motionAlertParameter);
                //alertParameters.Add(rpmAlertParameter);
                //alertParameters.Add(rpmDurationAlertParameter);
                //parameters["Notifications"] = notifications;
                parameters["Parameters"] = alertParameters;

                Task<IHalResult> task = client.Put(link, parameters);
                return task;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                Console.WriteLine(e.Message);
            }
            return null;
        } 
#endif

        // 获取指定日期的月份第一天
        // <param name="dateTime"></param>
        public DateTime GetDateTimeMonthFirstDay(DateTime dateTime)
        {
            DebugLog.Debug("[AlertFetcher] GetDateTimeMonthFirstDay(DateTime dateTime) Start");
            DebugLog.Debug("para(dateTime.ToString():" + dateTime.ToString("yyyy-MM-dd") + ")");
            try
            {
                if (dateTime == null)
                {
                    dateTime = DateTime.Now.ToUniversalTime();
                }
                DebugLog.Debug("[AlertFetcher] GetDateTimeMonthFirstDay(DateTime dateTime) End");
                return new DateTime(dateTime.Year, dateTime.Month, 1);
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                return new DateTime();
            }
        }

        // 获取指定月份最后一天
        // <param name="dateTime"></param>
        public DateTime GetDateTimeMonthLastDay(DateTime dateTime)
        {
            DebugLog.Debug("[AlertFetcher] GetDateTimeMonthLastDay(DateTime dateTime) Start");
            DebugLog.Debug("para(dateTime.ToString():" + dateTime.ToString("yyyy-MM-dd") + ")");
            try
            {
                int day = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                DebugLog.Debug("[AlertFetcher] GetDateTimeMonthLastDay(DateTime dateTime) End");
                return new DateTime(dateTime.Year, dateTime.Month, day);
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                return new DateTime();
            }
        }
    }
}