using BaiduWGSPoint;
using FleetManageTool.Models.Common;
using FleetManageTool.Models.page;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Exceptions;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.API;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Util;
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class FleetInfoFetcher
    {
        //获取车型
        public FleetInfo GetVehicleInfo(long groupID, string companyID, int timeZone)
        {
            DebugLog.Debug("[FleetInfoFetcher] GetVehicleInfo()  para( groupID=" + groupID + ", companyID=" + companyID + ", timeZone=" + timeZone + ") Start Time="+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            List<VehicleInfo> vehicles = new List<VehicleInfo>();
            FleetInfo fleetInfo = new FleetInfo();
            fleetInfo.allVehicle = new List<VehicleInfo>();
            fleetInfo.drivingVehicle = new List<VehicleInfo>();
            fleetInfo.parkingVehicle = new List<VehicleInfo>();
            fleetInfo.misstargetVehicle = new List<VehicleInfo>();
            fleetInfo.breakVehicle = new List<VehicleInfo>();
            fleetInfo.alertVehicle = new List<VehicleInfo>();
            fleetInfo.historyVehicle = new List<VehicleInfo>();
            //chenyangwen api 20140324
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                List<Models.Vehicle> vehicleTemp = null;
                if (-1 == groupID)
                {
                    vehicleTemp = vehicleDB.GetTenantVehiclesByCompannyID(companyID);
                }
                else
                {
                    vehicleTemp = vehicleDB.GetGroupVehiclesByGroupId(groupID);
                }
                fleetInfo.allVehicle = FormatVehicleInfo(vehicleTemp, groupID);
                DebugLog.Debug("[FleetInfoFetcher] GetVehicleInfo() End(return fleetInfo)[fleetInfo.allVehicle.Count=" + fleetInfo.allVehicle.Count + "]");
                return fleetInfo;
            }
            else
            {
                try
                {
                    //DataCache cache = CacheUtil.GetDataCacheFactory().GetDefaultCache();
                    //TenantDBInterface tenantDB = new TenantDBInterface();
                    //Models.Tenant tenant = tenantDB.GetTenantByCompanyID(companyID);
                    //List<CustomerData> customers = (List<CustomerData>)cache.Get(companyID + "_Cache");
                    //if (customers.Count == 0 || customers == null)
                    //{
                    //    //...todo
                    //}
                    //List<Models.API.Vehicle> apiVehicleInfos = new List<Models.API.Vehicle>();
                    //foreach (CustomerData customerTemp in customers)
                    //{
                    //    if (customerTemp.Vehicles.Count == 0 || customerTemp.Vehicles == null)
                    //    {
                    //        //...todo
                    //        continue;
                    //    }
                    //    apiVehicleInfos.AddRange(customerTemp.Vehicles);
                    //}
                    fleetInfo = GetInfoFromCache(companyID, groupID, timeZone, true);
                    foreach (VehicleInfo vehicle in fleetInfo.allVehicle)
                    {
                        vehicle.alerts = null;
                    }
                    //if (null == cache.Get("FleetInfo"))
                    //{
                    //cache.Put("FleetInfo", fleetInfo);
                    //}
                    DebugLog.Debug("[FleetInfoFetcher] GetVehicleInfo() End(return fleetInfo)[fleetInfo.allVehicle.Count=" + fleetInfo.allVehicle.Count + "] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    return fleetInfo;
                }
                catch (DBException dbException)
                {
                    DebugLog.Exception(DebugLog.DebugType.DBException, dbException.Message);
                }
                catch (HalException halException)
                {
                    DebugLog.Exception(DebugLog.DebugType.HttpException, halException.Message);
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
                }
                //chenyangwen api 20140324
                DebugLog.Debug("[FleetInfoFetcher] GetVehicleInfo(long groupID, string companyID) End (return null FleetInfo becauseOf Exception)");
                return fleetInfo;
            }
        }

        //
        private FleetInfo DataProcess(List<Models.API.Vehicle> apiVehicleInfos, long groupID,int timeZone, bool groupFlag)
        {
            DebugLog.Debug("[FleetInfoFetcher] DataProcess() para( apiVehicleInfos.Count=" + apiVehicleInfos.Count + ",groupID=" + groupID + ",timeZone=" + timeZone + ") Start Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            FleetInfo fleetInfo = new FleetInfo();
            fleetInfo.allVehicle = new List<VehicleInfo>();
            fleetInfo.drivingVehicle = new List<VehicleInfo>();
            fleetInfo.parkingVehicle = new List<VehicleInfo>();
            fleetInfo.misstargetVehicle = new List<VehicleInfo>();
            fleetInfo.breakVehicle = new List<VehicleInfo>();
            fleetInfo.alertVehicle = new List<VehicleInfo>();
            fleetInfo.historyVehicle = new List<VehicleInfo>();
            VehicleDBInterface vehicleInterface = new VehicleDBInterface();
            try
            {
                List<string> guids = new List<string>();
                foreach (Models.API.Vehicle apiVehicle in apiVehicleInfos)
                {
                    guids.Add(apiVehicle.Id);
                }
                DebugLog.Debug("[FleetInfoFetcher] DataProcess() <GetVehiclesByGUIDs>start Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                List<Models.Vehicle> dbvehicles = vehicleInterface.GetVehiclesByGUIDs(guids);
                DebugLog.Debug("[FleetInfoFetcher] DataProcess() <GetVehiclesByGUIDs>end dbvehicles.count=" + dbvehicles.Count + " Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                foreach (Models.API.Vehicle apiVehicle in apiVehicleInfos)
                {
                    DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce>start Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    Models.Vehicle dbvehicle = dbvehicles.Find(v => v.id == apiVehicle.Id);
                    //DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce> vehicleInterface.GetVehicleByGUID(" + apiVehicle.Id + ") Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    if (null == dbvehicle)
                    {
                        continue;
                    }
                    VehicleInfo vehicleInfo = new VehicleInfo();
                    vehicleInfo.license = dbvehicle.licence;
                    vehicleInfo.primarykey = dbvehicle.pkid;
                    vehicleInfo.vehicleID = apiVehicle.Id;
                    vehicleInfo.logoID = dbvehicle.logoid;
                    vehicleInfo.vin = apiVehicle.Vin;
                    vehicleInfo.alertType = AlertType.NOALERT;
                    //车速
                    if (apiVehicle.status.EngineOn)
                    {
                        vehicleInfo.speed = apiVehicle.status.Speed;
                    }
                    else
                    {
                        vehicleInfo.speed = 0;
                    }
                    //油量
                    if (null == apiVehicle.status.FuelLevel)
                    {
                        vehicleInfo.fuel = 0;
                    }
                    else
                    {
                        vehicleInfo.fuel = apiVehicle.status.FuelLevel.Value;
                    }
                    //行驶里程
                    if (null == apiVehicle.status.Odometer)
                    {
                        vehicleInfo.odometer = "0.0";
                    }
                    else
                    {
                        vehicleInfo.odometer = string.Format("{0:N0}", apiVehicle.status.Odometer.Value);
                    }
                    vehicleInfo.driver = dbvehicle.drivername;
                    //获取车的分组信息
                    if (groupFlag)
                    {
                        //DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce> vehicleInterface.GetGroupByVehicleId(" + dbvehicle.pkid + ") Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        VehicleGroup groupInfo = vehicleInterface.GetGroupByVehicleId(dbvehicle.pkid);
                        //DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce> vehicleInterface.GetGroupByVehicleId(" + dbvehicle.pkid + ") Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        if (null == groupInfo)
                        {
                            vehicleInfo.groupID = -2;
                            vehicleInfo.groupName = "-";
                        }
                        else
                        {
                            vehicleInfo.groupID = groupInfo.pkid;
                            vehicleInfo.groupName = groupInfo.name.Trim();
                        }
                        //获取指定车组的车
                        if (-1 != groupID)
                        {
                            if (vehicleInfo.groupID != groupID)
                            {
                                continue;
                            }
                        }
                    }
                    //当前地点信息
                    vehicleInfo.location = new Location();
                    GeoPointDTO point = new GeoPointDTO();
                    if (null != apiVehicle.VehicleLocation)
                    {
                        //DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce> BaiduWGSPoint.GeoPointDTO.InternationalToBaidu() Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(apiVehicle.VehicleLocation.Location.longitude, apiVehicle.VehicleLocation.Location.latitude);
                        if (null != apiVehicle.VehicleLocation.Location)
                        {
                            DebugLog.Debug("[FleetInfoFetcher] DataProcess() apiVehicle = " + apiVehicle.Id + "longitude = " + apiVehicle.VehicleLocation.Location.longitude + " latitude = " + apiVehicle.VehicleLocation.Location.latitude + " Time = " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss:fff"));
                        }
                    }
                    else
                    {
                        point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(0, 0);
                    }
                    vehicleInfo.location.latitude = point.latitude;
                    vehicleInfo.location.longitude = point.longitude;
                    //车辆名称
                    if (null != dbvehicle.name)
                    {
                        vehicleInfo.name = dbvehicle.name.Trim();
                    }
                    //车辆引擎状态
                    if (apiVehicle.status.EngineOn)
                    {
                        vehicleInfo.engineStatus = EngineStatus.ENGINEON;
                    }
                    else
                    {
                        vehicleInfo.engineStatus = EngineStatus.ENGINEOFF;
                    }
                    //驾驶司机信息
                    vehicleInfo.driver = dbvehicle.drivername;

                    //电话号码
                    vehicleInfo.telephone = dbvehicle.telephone;

                    //电量
                    if (null == apiVehicle.status.BatteryVoltage)
                    {
                        vehicleInfo.battery = 0;
                    }
                    else
                    {
                        vehicleInfo.battery = apiVehicle.status.BatteryVoltage.Value;
                    }
                    //车辆信息
                    vehicleInfo.Info = dbvehicle.info;
                    //历史车辆
                    if (null == apiVehicle.ConnectedDeviceId)
                    {
                        vehicleInfo.lastUsedTime = apiVehicle.UpdatedOn.ToUniversalTime();
                        fleetInfo.historyVehicle.Add(vehicleInfo);
                    }
                    else
                    {
                        //车辆alerts
                        vehicleInfo.alerts = apiVehicle.Alerts;
                        //丢失状态
                        vehicleInfo.misState = MisState.OK;
                        //故障车辆
                        if (null == apiVehicle.status.EngineLightStatus)
                        {
                            vehicleInfo.healthStatus = HealthStatus.UNKNOWN;
                        }
                        else if ("ON".Equals(apiVehicle.status.EngineLightStatus))
                        {
                            vehicleInfo.healthStatus = HealthStatus.ENGINELIGHTON;
                            fleetInfo.breakVehicle.Add(vehicleInfo);
                        }
                        else if ("OFF".Equals(apiVehicle.status.EngineLightStatus))
                        {
                            vehicleInfo.healthStatus = HealthStatus.ENGINELIGHTOFF;
                        }

                        DateTime now = DateTime.Now.ToUniversalTime();
                        TimeSpan upDateOnTime = new TimeSpan();
                        if (null != apiVehicle.VehicleLocation)
                        {
                            upDateOnTime = now - apiVehicle.VehicleLocation.UpdatedOn.ToUniversalTime();
                        }
                        else
                        {
                            upDateOnTime = now - now;
                        }
                        //行驶车辆
                        if (apiVehicle.status.EngineOn)
                        {
                            vehicleInfo.engineTime =string.Format("{0:N0}", (DateTime.Now.ToUniversalTime() - apiVehicle.UpdatedOn.ToUniversalTime()).TotalMinutes.ToString());
                            fleetInfo.drivingVehicle.Add(vehicleInfo);
                        }
#if false //fengpan 20140505
                        else if (Double.Parse(System.Configuration.ConfigurationManager.AppSettings["UpdateOnTime"]) <= upDateOnTime.TotalMinutes)
                        {
                            //丢失车辆
                            vehicleInfo.misState = MisState.MISSED;
                            fleetInfo.misstargetVehicle.Add(vehicleInfo);
                            vehicleInfo.lastUsedTime = apiVehicle.UpdatedOn.ToUniversalTime();
                        } 
#endif
                        else if (!apiVehicle.status.EngineOn)
                        {
                            //停驶车辆
                            fleetInfo.parkingVehicle.Add(vehicleInfo);
                        }
                        else
                        {
                            //...
                        }

                        //报警车辆
                        if (null != apiVehicle.Alerts)
                        {
                            List<Models.API.Alert> alertsTemp = apiVehicle.Alerts.FindAll(t => AlertConfigurationConstant.Speed.Equals(t.AlertType) || AlertConfigurationConstant.Rpm.Equals(t.AlertType) || AlertConfigurationConstant.EngineRpm.Equals(t.AlertType) || AlertConfigurationConstant.Motion.Equals(t.AlertType));
                            alertsTemp.Sort((y, x) => (x.TriggeredDateTime.CompareTo(y.TriggeredDateTime)));
                            Models.API.Alert alertTemp = alertsTemp.Find(t => t.TriggeredDateTime.ToUniversalTime().AddHours(timeZone).Day == now.AddHours(timeZone).Day);
                            if (null != alertTemp)
                            {
                                switch (alertTemp.AlertType)
                                {
                                    case AlertConfigurationConstant.Speed: vehicleInfo.alertType = AlertType.SPEEDALERT; break;
                                    case AlertConfigurationConstant.Rpm: vehicleInfo.alertType = AlertType.HIGHPRMALERT; break;
                                    case AlertConfigurationConstant.EngineRpm: vehicleInfo.alertType = AlertType.HIGHPRMALERT; break;
                                    case AlertConfigurationConstant.Motion: vehicleInfo.alertType = AlertType.MOTIONALERT; break;
                                    default: break;
                                }
                                fleetInfo.alertVehicle.Add(vehicleInfo);
                            }
                        }
                        //所有车辆
                        fleetInfo.allVehicle.Add(vehicleInfo);
                        DebugLog.Debug("[FleetInfoFetcher] DataProcess() <formatOneVehilce>end Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    }
                }
                DebugLog.Debug("[FleetInfoFetcher] DataProcess(List<Models.API.Vehicle> apiVehicleInfos, long groupID) End Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                return fleetInfo;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException,e.Message);
                return fleetInfo;
            }
        }

        //获取车辆列表
        public FleetInfo GetVehicleListInfo(string companyID, int timeZone)
        {
            DebugLog.Debug("[FleetInfoFetcher] GetVehicleListInfo(string companyID) Start");
            DebugLog.Debug("para( companyID:" + companyID + ")");
            List<VehicleInfo> vehiclesList = new List<VehicleInfo>();
            FleetInfo fleet = new FleetInfo();
            fleet.allVehicle = new List<VehicleInfo>();
            fleet.drivingVehicle = new List<VehicleInfo>();
            fleet.parkingVehicle = new List<VehicleInfo>();
            fleet.misstargetVehicle = new List<VehicleInfo>();
            fleet.breakVehicle = new List<VehicleInfo>();
            fleet.alertVehicle = new List<VehicleInfo>();
            fleet.historyVehicle = new List<VehicleInfo>();
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                List<VehicleInfo> vehicleAll = GetVehicleInfo(-1, companyID,timeZone).allVehicle;
                List<VehicleInfo> vehiclesInGroup = new List<VehicleInfo>();
                List<VehicleInfo> vehiclesNotInGroup = new List<VehicleInfo>();

                VehicleDBInterface dbInterface = new VehicleDBInterface();
                List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(companyID);
                for (int i = 0; i < groups.Count; i++)
                {
                    List<VehicleInfo> vehiclesInGroupTemp = GetVehicleInfo(groups[i].pkid, companyID,timeZone).allVehicle;
                    foreach (VehicleInfo temp1 in vehiclesInGroupTemp)
                    {
                        temp1.groupName = groups[i].name;
                        vehiclesInGroup.Add(temp1);
                        vehiclesList.Add(temp1);
                    }
                }
                for (int i = 0; i < vehicleAll.Count; i++)
                {
                    int flag = 0;
                    for (int j = 0; j < vehiclesInGroup.Count; j++)
                    {
                        if (vehiclesInGroup.ElementAt(j).name == vehicleAll.ElementAt(i).name)
                        {
                            flag = 1;
                        }
                    }
                    if (0 == flag)
                    {
                        vehicleAll.ElementAt(i).groupName = "-";
                        vehicleAll.ElementAt(i).groupID = -2;//表示无分组
                        vehiclesList.Add(vehicleAll.ElementAt(i));
                    }
                }
                fleet.allVehicle = vehiclesList;
                fleet.alertVehicle = vehiclesList;
                SortFleetInfo(fleet);
                DebugLog.Debug("[FleetInfoFetcher] GetVehicleListInfo() End(return fleet)[fleet.allVehicle.Count=" + fleet.allVehicle.Count + "]");
                return fleet;
            }
            else
            {
                try
                {
                    fleet = GetInfoFromCache(companyID, -1, timeZone, true);
                    VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                    foreach (VehicleInfo vehicle in fleet.allVehicle)
                    {
                        vehicle.alerts = null;
                        if (null == vehicleInterface.GetGroupByVehicleId(vehicle.primarykey))
                        {
                            vehicle.groupID = -2;
                            vehicle.groupName = "-";
                        }
                        else
                        {
                            vehicle.groupID = vehicleInterface.GetGroupByVehicleId(vehicle.primarykey).pkid;
                            vehicle.groupName = vehicleInterface.GetGroupByVehicleId(vehicle.primarykey).name.Trim();
                        }
                    }
                    SortFleetInfo(fleet);
                    DebugLog.Debug("[FleetInfoFetcher] GetVehicleListInfo() End(return fleet)[fleet.allVehicle.Count=" + fleet.allVehicle.Count + "]");
                    return fleet;
                }
                catch (Exception e)
                {
                    DebugLog.Debug("[FleetInfoFetcher] GetVehicleListInfo() Exception:" + e.Message);
                    Console.WriteLine(e.Message);
                    DebugLog.Debug("[FleetInfoFetcher] GetVehicleListInfo() End(return fleet)[Exception:return null FleetInfo]");
                    return fleet;
                }
            }
        }

        public List<VehicleInfo> FormatVehicleInfo(List<Models.Vehicle> vehicleTemp, long groupID)
        {
            DebugLog.Debug("[FleetInfoFetcher] FormatVehicleInfo()  para( groupID:" + groupID + ")  Start");
            List<VehicleInfo> vehicles = new List<VehicleInfo>();
            for (int i = 0; i < vehicleTemp.Count; ++i)
            {
                VehicleInfo model = new VehicleInfo();
                model.primarykey = vehicleTemp.ElementAt(i).pkid;
                model.license = vehicleTemp.ElementAt(i).licence;
                model.name = vehicleTemp.ElementAt(i).name;
                model.location = new Location();
                model.groupID = groupID;
                model.Info = vehicleTemp.ElementAt(i).info;
                //#861 车辆属性增加司机liangjiajie0319
                model.driver = vehicleTemp.ElementAt(i).drivername;
                //chenyangwen 2014/3/1
                if (1 == model.primarykey || 11 == model.primarykey || 6 == model.primarykey)
                {
                    //model.logoUrl = @"../../../Content/Home/images/Vehicle.png";
                    model.engineTime = "40";
                    model.location.longitude = 116.403083;
                    model.location.latitude = 39.999038;
                    model.engineStatus = EngineStatus.ENGINEOFF;
                    model.healthStatus = HealthStatus.ENGINELIGHTON;
                    model.alertType = AlertType.NOALERT;
                    model.misState = MisState.OK;
                    model.odometer = "255999";
                }
                else if (2 == model.primarykey || 12 == model.primarykey || 7 == model.primarykey)
                {
                    //model.logoUrl = @"../../../Content/Home/images/Vehicle.png";
                    model.engineTime = "50";
                    model.location.longitude = 116.46954;
                    model.location.latitude = 39.957489;
                    model.engineStatus = EngineStatus.ENGINEOFF;
                    model.healthStatus = HealthStatus.ENGINELIGHTOFF;
                    model.alertType = AlertType.NOALERT;
                    model.misState = MisState.OK;
                    model.odometer = "255777";

                }
                else if (3 == model.primarykey || 13 == model.primarykey || 8 == model.primarykey)
                {
                    //model.logoUrl = @"../../../Content/Home/images/Vehicle.png";
                    model.engineTime = "55";
                    model.location.longitude = 116.309714;
                    model.location.latitude = 39.961913;
                    model.engineStatus = EngineStatus.ENGINEON;
                    model.healthStatus = HealthStatus.ENGINELIGHTOFF;
                    model.alertType = AlertType.MOTIONALERT;
                    model.misState = MisState.OK;
                    model.odometer = "357777";
                }
                else if (4 == model.primarykey || 14 == model.primarykey || 9 == model.primarykey)
                {
                    //model.logoUrl = @"../../../Content/Home/images/VehicleImage.png";
                    model.engineTime = "45";
                    model.location.longitude = 116.362606;
                    model.location.latitude = 39.910573;
                    model.engineStatus = EngineStatus.ENGINEON;
                    model.healthStatus = HealthStatus.ENGINELIGHTOFF;
                    model.alertType = AlertType.NOALERT;
                    model.misState = MisState.OK;
                    model.odometer = "102987";
                }
                else if (5 == model.primarykey || 15 == model.primarykey || 10 == model.primarykey)
                {
                    //model.logoUrl = @"../../../Content/Home/images/VehicleImage.png";
                    model.engineTime = "100";
                    model.location.longitude = 116.652364;
                    model.location.latitude = 39.913672;
                    model.engineStatus = EngineStatus.ENGINEON;
                    model.healthStatus = HealthStatus.ENGINELIGHTOFF;
                    model.alertType = AlertType.NOALERT;
                    model.misState = MisState.MISSED;
                    model.lastUsedTime = DateTime.Parse("2013-03-11 15:30");
                    model.odometer = "102987";
                }
                else
                {
                    //model.logoUrl = @"../../../Content/Home/images/VehicleImage.png";
                    model.engineTime = "100";
                    model.location.longitude = 116.652364 + i * 0.1;
                    model.location.latitude = 39.913672 + i * 0.1;
                    model.engineStatus = EngineStatus.ENGINEON;
                    model.healthStatus = HealthStatus.ENGINELIGHTOFF;
                    model.alertType = AlertType.NOALERT;
                    model.misState = MisState.OK;
                    model.odometer = "255999";
                }
                vehicles.Add(model);
            }
            DebugLog.Debug("[FleetInfoFetcher] FormatVehicleInfo() End[vehicles.Count=" + vehicles.Count + "]");
            return vehicles;
        }

        //从cache获取customers信息，返回FleetInfo对象
        public FleetInfo GetInfoFromCache(string companyID, long groupID, int timeZone, bool groupFlag) 
        {
            DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache()  paras[companyID = " + companyID + " , groupID = " + groupID + " , timeZone = " + timeZone + "] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            FleetInfo fleetInfo = new FleetInfo();
            fleetInfo.alertVehicle = new List<VehicleInfo>();
            fleetInfo.allVehicle = new List<VehicleInfo>();
            fleetInfo.breakVehicle = new List<VehicleInfo>();
            fleetInfo.drivingVehicle = new List<VehicleInfo>();
            fleetInfo.historyVehicle = new List<VehicleInfo>();
            fleetInfo.parkingVehicle = new List<VehicleInfo>();
            fleetInfo.misstargetVehicle = new List<VehicleInfo>();
            try
            {

                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() [new CacheService()] head startTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                CacheService service = new CacheService();
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() [new CacheService()] foot startTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                TenantDBInterface tenantDB = new TenantDBInterface();

                Models.Tenant tenant = tenantDB.GetTenantByCompanyID(companyID);
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() CacheGet " + companyID + "_Cache head startTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                List<CustomerData> customers = (List<CustomerData>)service.CacheGet(companyID + "_Cache");
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() CacheGet "+ companyID + "_Cache foot endTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                List<Models.API.Vehicle> apiVehicleInfos = new List<Models.API.Vehicle>();
                if (customers == null || customers.Count == 0 || null == customers.ElementAt(0).Vehicles )
                {
                    //cache为空，需要从dataSyncTask取数据
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() APIUtil.GetVehiclesInfo[cache is empty] " + companyID + " foot startTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    customers = APIUtil.GetVehiclesInfo(companyID);
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() APIUtil.GetVehiclesInfo[cache is empty] " + companyID + " foot endTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    foreach (CustomerData customerTemp in customers)
                    {
                        if (customerTemp.Vehicles == null || customerTemp.Vehicles.Count == 0)
                        {
                            continue;
                        }
                        apiVehicleInfos.AddRange(customerTemp.Vehicles);
                    }
                    fleetInfo = DataProcess(apiVehicleInfos, groupID, timeZone, groupFlag);
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() <cacheIsEmpty> END(return fleetInfo)[fleetInfo.allVehicle.Count=" + fleetInfo.allVehicle.Count + "] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    return fleetInfo;
                }
                foreach (CustomerData customerTemp in customers)
                {
                    if (customerTemp.Vehicles == null || customerTemp.Vehicles.Count == 0)
                    {
                        continue;
                    }
                    apiVehicleInfos.AddRange(customerTemp.Vehicles);
                }
                fleetInfo = DataProcess(apiVehicleInfos, groupID, timeZone, groupFlag);
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                List<Models.Vehicle> vehiclesInDB = vehicleDB.GetTenantVehiclesByCompannyID(companyID);
                if (fleetInfo.allVehicle.Count != vehiclesInDB.Count)
                {
                    //与数据库车辆不一致时再次调用。
                    apiVehicleInfos = new List<Models.API.Vehicle>();
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() APIUtil.GetVehiclesInfo[different from DB] " + companyID + " foot startTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    customers = APIUtil.GetVehiclesInfo(companyID);
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() APIUtil.GetVehiclesInfo[different from DB] " + companyID + " foot endTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    foreach (CustomerData customerTemp in customers)
                    {
                        if (customerTemp.Vehicles == null || customerTemp.Vehicles.Count == 0)
                        {
                            continue;
                        }
                        apiVehicleInfos.AddRange(customerTemp.Vehicles);
                    }
                    fleetInfo = DataProcess(apiVehicleInfos, groupID, timeZone, groupFlag);
                    DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache()  <diffrentFromDB> END(return fleetInfo)[fleetInfo.allVehicle.Count=" + fleetInfo.allVehicle.Count + "] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    return fleetInfo;
                }
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() <normal>  END(return fleetInfo)[fleetInfo.allVehicle.Count=" + fleetInfo.allVehicle.Count + "] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                return fleetInfo;
            }
            catch (Exception e)
            {
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache() Exception:" + e.Message);
                DebugLog.Debug("[FleetInfoFetcher] getInfoFromCache()  END(return fleetInfo)[return null FleetInfo becauseOf Exception] Time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                return fleetInfo;
            }
        }
        public FleetInfo SortFleetInfo(FleetInfo fleetInfo)
        {
            DebugLog.Debug("[FleetInfoFetcher] SortFleetInfo() Start para[fleetInfo.allVehicle.Count = " + fleetInfo.allVehicle.Count + "]");
            try
            { 
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                fleetInfo.allVehicle = DealtheNoGroupVehicles(fleetInfo.allVehicle);
                fleetInfo.alertVehicle = DealtheNoGroupVehicles(fleetInfo.alertVehicle);
                fleetInfo.breakVehicle = DealtheNoGroupVehicles(fleetInfo.breakVehicle);
                fleetInfo.drivingVehicle = DealtheNoGroupVehicles(fleetInfo.drivingVehicle);
                fleetInfo.historyVehicle = DealtheNoGroupVehicles(fleetInfo.historyVehicle);
                fleetInfo.misstargetVehicle = DealtheNoGroupVehicles(fleetInfo.misstargetVehicle);
                fleetInfo.parkingVehicle = DealtheNoGroupVehicles(fleetInfo.parkingVehicle);
                return fleetInfo;
            }
            catch(Exception e)
            {
                DebugLog.Debug("[FleetInfoFetcher] SortFleetInfo()" + e.Message);
                return fleetInfo;
            }
        }
        public List<VehicleInfo> DealtheNoGroupVehicles(List<VehicleInfo> vehicles)
        {
            DebugLog.Debug("[FleetInfoFetcher] SortFleetInfo() Start para[fleetInfo.allVehicle.Count = " + vehicles.Count + "]");
            try
            {
                vehicles.Sort((x, y) => x.name.Trim().CompareTo(y.name.Trim()));
                vehicles.Sort((x, y) => x.groupName.CompareTo(y.groupName));
                List<VehicleInfo> noGroupVehiclesAll = vehicles.FindAll(t => t.groupID == -2);
                noGroupVehiclesAll.Sort((x, y) => x.groupName.CompareTo(y.groupName));
                vehicles.RemoveAll(t => t.groupID == -2);
                vehicles.AddRange(noGroupVehiclesAll);
                return vehicles;
            }
            catch (Exception e)
            {
                DebugLog.Debug("[FleetInfoFetcher] DealtheNoGroupVehicles()" + e.Message);
                return vehicles;
            }
        }
        /// <summary>
        /// fengpan 20140507
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<CustomerData> GetCustomersInfoFromDataSyncTask(string companyID)
        {
            DebugLog.Debug("[FleetInfoFetcher] GetCustomersInfoFromDataSyncTask() paras:[companyID=" + companyID + "]:");
            try
            {
                TaskEngine<List<CustomerData>> task = new TaskEngine<List<CustomerData>>();
                task.AddPara("TaskName", "DataSyncTask");
                List<TaskParameter> paras = new List<TaskParameter>();
                TenantDBInterface tenantDB = new TenantDBInterface();
                List<Models.Customer> customers = tenantDB.GetCustomersByCompanyID(companyID);
                foreach (Models.Customer customer in customers)
                {
                    TaskParameter para = new TaskParameter() { Name = "Customer", Value = customer.guid };
                    paras.Add(para);
                }
                task.AddPara("Parameters", paras);
                List<CustomerData> customerDatas = task.execute().Result;
                CacheService service = new CacheService();
                service.CachePut(companyID + "_Cache", customerDatas);
                DebugLog.Debug("[FleetInfoFetcher] GetCustomersInfoFromDataSyncTask() return:[customersData.Count = " + customerDatas.Count + "]");
                return customerDatas;
            }
            catch(Exception e)
            {
                DebugLog.Debug("[FleetInfoFetcher] GetCustomersInfoFromDataSyncTask() Exception:" + e.Message);
                return new List<CustomerData>();
            }
        }
    }
}