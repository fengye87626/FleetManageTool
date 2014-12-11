using FleetManageTool.WebAPI;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.API;
using FleetManageToolWebRole.DB_interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FleetManageTool.WebAPI.Exceptions;
using Microsoft.ApplicationServer.Caching;

namespace FleetManageToolWebRole.Util
{
    public class APIUtil
    {
        private static object lockObj = new object();
        //存储某一租户数据
        public static void StoreData(String companyID, List<CustomerData> customerDatas)
        {
            if (null == customerDatas )
            {
                return;
            }
            DebugLog.Debug("APIUtil StoreData Start");
            try
            {
                foreach (CustomerData customerData in customerDatas)
                {
                    if (null == customerData.Vehicles || 0 == 
                        customerData.Vehicles.Count)
                    {
                        return;
                    }
                    //同步车辆,需要同步alertConfgirution
                    UpdateVehicleObu(companyID, customerData.Vehicles, customerData.Devices);
                    //chenyangwen 20140409 add trip and alert
                    VehicleDBInterface vehicleDB = new VehicleDBInterface();
                    foreach (Models.API.Vehicle vehicle in customerData.Vehicles)
                    {
                        Models.Vehicle vehicledb = vehicleDB.GetVehicleByGUID(vehicle.Id);
                        Models.Trip recentTrip = vehicleDB.GetLastTrip(vehicledb.pkid);
                        //chenyangwen 20140505 1139
                        ProcessTrip(vehicle.Trips, vehicledb, recentTrip, vehicle.status);

                        Models.Alert recentAlert = vehicleDB.GetLastAlert(vehicledb.pkid);
                        Models.Alert engineAlert = EngineAlertStatu(vehicle);
                        ProcessAlert(vehicle.Alerts, vehicledb, recentAlert, engineAlert);
                    }
                }
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "APIUtil StoreData Exception = " + e.Message);
                throw e;
            }
            DebugLog.Debug("APIUtil StoreData End");
        }
		
		/// <summary>
        /// fengpan 20140507 同步某一租户数据
        /// </summary>
        /// <param name="companyID"></param>
        public static void UpdateTenantData(String companyID)
        {
            TenantDBInterface tenantDB = new TenantDBInterface();
            List<Models.Customer> customers = tenantDB.GetCustomersByCompanyID(companyID);
            if (null == customers)
            {
                return;
            }
            DebugLog.Debug("APIUtil UpdateTenantData Start");
            try
            {
                foreach (FleetManageToolWebRole.Models.Customer customer in customers)
                {
                    List<Models.API.Vehicle> vehicles = GetCustomerVehicles(customer.guid);
                    List<Models.API.Device> devices =  GetCustomerDevices(customer.guid);
                    if (null == vehicles || 0 == vehicles.Count)
                    {
                        continue;
                    }
                    UpdateVehicleObu(companyID, vehicles, devices);
                }
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "APIUtil UpdateTenantData Exception = " + e.Message);
                throw e;
            }
            DebugLog.Debug("APIUtil UpdateTenantData End");
        }

        public static Models.Alert EngineAlertStatu(Models.API.Vehicle vehicle)
        {
            if (null == vehicle.status.MILAlertOn)
            {
                return null;
            }
            CacheService service = new CacheService();
            DebugLog.Debug("APIUtil EngineAlertStatu Start vehicle.status.MILAlertOn = " + vehicle.status.MILAlertOn + "UpdateOn = " + vehicle.UpdatedOn);
            if (null == service.CacheGet(vehicle.Id + "_engine"))
            {
                service.CachePut(vehicle.Id + "_engine", vehicle.status.MILAlertOn);
                if (vehicle.status.MILAlertOn.Value)
                {
                    service.CachePut(vehicle.Id + "_engineTime", DateTime.Now.ToUniversalTime());
                }
                return null;
            }
            Boolean isEngineLight = (Boolean)service.CacheGet(vehicle.Id + "_engine");
            DebugLog.Debug("APIUtil EngineAlertStatu Running isEngineLight = " + isEngineLight);
            if (isEngineLight)
            {
                if (null != vehicle.status.MILAlertOn && !vehicle.status.MILAlertOn.Value)
                {
                    DebugLog.Debug("APIUtil EngineAlertStatu Running ADDAlert ");
                    Models.Alert alert = new Models.Alert();
                    alert.AlertType = "ENGINE";
                    alert.guid = "engineGUID";
                    alert.EngineEndTime = DateTime.Now.ToUniversalTime();
                    alert.TriggeredDateTime = (DateTime)service.CacheGet(vehicle.Id + "_engineTime");
                    service.CachePut(vehicle.Id + "_engine", vehicle.status.MILAlertOn);
                    return alert;
                }
            }
            else
            {
                if (null != vehicle.status.MILAlertOn && vehicle.status.MILAlertOn.Value)
                {
                    service.CachePut(vehicle.Id + "_engine", vehicle.status.MILAlertOn);
                    service.CachePut(vehicle.Id + "_engineTime", DateTime.Now.ToUniversalTime());
                }
            }
            DebugLog.Debug("APIUtil EngineAlertStatu End");
            return null;
        }

        //处理trip数据
        //chenyangwen 20140409 add trip and alert
        public static void ProcessTrip(List<Models.API.Trip> trips, Models.Vehicle vehicledb, Models.Trip recentTrip, Status vehicleStatus)
        {
            if (null == trips || null == vehicledb)
            {
                return;
            }
            DebugLog.Debug("APIUtil ProcessTrip start");
            try
            {
                DebugLog.Debug("APIUtil ProcessTrip Start trips.Count = " + trips.Count + ";vehicledb.guid = " + vehicledb.id);
                List<Models.API.Trip> tripsDay = new List<Models.API.Trip>();
                if (null == recentTrip)
                {
                    tripsDay = GetAllTrips(trips, vehicledb.id);
                }
                else
                {
                    tripsDay = findNowTrips(trips, recentTrip.guid, vehicledb.id);
                }
                DebugLog.Debug("APIUtil ProcessTrip Start tripsDay.Count = " + tripsDay.Count + ";vehicledb.pkid = " + vehicledb.pkid);
                //chenyangwen 20140505 1139
                //tripsDay.Sort((t1, t2) => t1.StartDateTime.Value.CompareTo(t2.StartDateTime.Value));
                if (null != tripsDay && tripsDay.Count >= 1 && 
                    null != vehicleStatus && null != vehicleStatus.EngineOn && true == vehicleStatus.EngineOn)
                {
                    DebugLog.Debug("APIUtil ProcessTrip Start vehicleStatus.EngineOn = true");
                    tripsDay.RemoveAt(0);
                }

                if (null != tripsDay && 0 < tripsDay.Count)
                {
                    tripsDay = tripsDay.Distinct(new TripComparer()).ToList();
                }

                if (null != tripsDay && 0 < tripsDay.Count)
                {
                    tripsDay = RemoveDuplicateTrip(tripsDay, vehicledb.pkid);
                }
                
                if (null != tripsDay && 0 < tripsDay.Count)
                {
                    StoreTripsToDB(tripsDay, vehicledb.pkid);//tripsDay.Distinct(new TripComparer()).ToList()去掉重复数据
                }
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "APIUtil ProcessTrip" + e.Message);
                throw e;
            }
            DebugLog.Debug("APIUtil ProcessTrip End");
        }

        private static List<Models.API.Alert> RemoveDuplicateAlert(List<Models.API.Alert> alerts, long vehiclePKID)
        {
            DebugLog.Debug("APIUtil RemoveDuplicateAlert Start");
            List<Models.API.Alert> result = new List<Models.API.Alert>();
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            foreach (Models.API.Alert alertTemp in alerts)
            {
                if (!(vehicleDB.isExistenceAlert(alertTemp.Id, vehiclePKID)))
                {
                    result.Add(alertTemp);
                }
                else
                {
                    DebugLog.Debug("APIUtil RemoveDuplicateAlert is Existence Alert GUID = " + alertTemp.Id);
                }
            }
            DebugLog.Debug("APIUtil RemoveDuplicateAlert End");
            return result;
        }

        //处理alert数据
        //chenyangwen 20140409 add trip and alert
        public static void ProcessAlert(List<Models.API.Alert> alerts, Models.Vehicle vehicledb, Models.Alert recentAlert, Models.Alert engineAlert)
        {
            if (null == alerts || null == vehicledb)
            {
                return;
            }
            DebugLog.Debug("APIUtil ProcessAlert Start");
            try
            {
                DebugLog.Debug("APIUtil ProcessTrip Start alerts.Count = " + alerts.Count + ";vehicledb.guid = " + vehicledb.id);
                List<Models.API.Alert> alertsDay = new List<Models.API.Alert>();
                if (null == recentAlert)
                {
                    alertsDay = GetAllAlerts(vehicledb.id);
                }
                else
                {
                    alertsDay = findNowAlerts(alerts, recentAlert.guid, vehicledb.id);
                }

                if (null != alertsDay && 0 < alertsDay.Count)
                {
                    alertsDay = alertsDay.Distinct(new AlertAPIComparer()).ToList();
                }

                if (null != alertsDay && 0 < alertsDay.Count)
                {
                    alertsDay = RemoveDuplicateAlert(alertsDay, vehicledb.pkid);
                }

                if (null != alertsDay && 0 < alertsDay.Count)
                {
                    StoreAlertsToDB(alertsDay, vehicledb.pkid, engineAlert);
                }
                
                DebugLog.Debug("APIUtil ProcessAlert End");
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "APIUtil ProcessTrip" + e.Message);
                throw e;
            }
        }

        private static List<Models.API.Trip> RemoveDuplicateTrip(List<Models.API.Trip> trips, long vehiclePKID)
        {
            DebugLog.Debug("APIUtil RemoveDuplicateTrip Start");
            List<Models.API.Trip> result = new List<Models.API.Trip>();
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            foreach (Models.API.Trip tripTemp in trips)
            {
                if (!(vehicleDB.isExistenceTrip(tripTemp.Id, vehiclePKID)))
                {
                    result.Add(tripTemp);
                }
                else
                {
                    DebugLog.Debug("APIUtil RemoveDuplicateAlert is Existence Trip GUID = " + tripTemp.Id);
                }
            }
            DebugLog.Debug("APIUtil RemoveDuplicateTrip End");
            return result;
        }

        //查找现在要存储的trips
        //chenyangwen 20140409 add trip and alert
        public static List<Models.API.Trip> findNowTrips(List<Models.API.Trip> trips, string tripID, string vehicleGuid)
        {
            List<Models.API.Trip> result = new List<Models.API.Trip>();
            int index = trips.FindIndex(t => t.Id == tripID);
            DebugLog.Debug("APIUtil findNowTrips Start trips.Count = " + trips.Count + ";tripID = " + tripID + ";vehicleGuid = " + vehicleGuid + "; index = " + index);
            if (-1 != index)
            {
                DebugLog.Debug("APIUtil findNowTrips index = " + index);
                result.AddRange(trips.GetRange(0, index));
            }
            else
            {
                List<Models.API.Trip> resultTemp = new List<Models.API.Trip>();
                int page = 1;
                while (true)
                {
                    page++;//翻页
                    trips = GetPageTrips(vehicleGuid, page);
                    if (null == trips || 0 == trips.Count)
                    {
                        break;
                    }

                    index = trips.FindIndex(t => t.Id == tripID);
                    if (-1 == index)
                    {
                        resultTemp.AddRange(trips);
                    }
                    else
                    {
                        resultTemp.AddRange(trips.GetRange(0, index));
                        break;
                    }

                    DebugLog.Debug("APIUtil findNowTrips While index = " + index);
                }

                if (0 < resultTemp.Count)
                {
                    GetTripDetailList(resultTemp);
                    result.AddRange(resultTemp);
                }
            }

            DebugLog.Debug("APIUtil findNowTrips end result.Count = " + result.Count);
            return result;
        }

        //获取指定页数的trips
        //chenyangwen 20140409 add trip and alert
        public static List<Models.API.Trip> GetPageTrips(string vehicleGuid, int page)
        {
            DebugLog.Debug("APIUtil GetPageTrips Start vehicleGuid = " + vehicleGuid);
            IHalClient client = HalClient.GetInstance();
            HalLink link = new HalLink { Href = URI.VEHICLETRIPPAGE, IsTemplated = true };
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["ID"] = vehicleGuid;
            parameters["page"] = page + "";//有待分析，传入int数据没办法转换到url中
            Trips trips = client.Get<Trips>(link, parameters).Result;//获取某一页的trips
            List<Models.API.Trip> result = new List<Models.API.Trip>();
            if (null != trips && null != trips.trips)
            {
                result.AddRange(trips.trips);
            }
            DebugLog.Debug("APIUtil GetPageTrips End vehicleGuid = " + vehicleGuid);
            return result;
        }

        //存取Trip数据到DB
        //chenyangwen 20140409 add trip and alert
        public static void StoreTripsToDB(List<Models.API.Trip> trips, long vehicleID)
        {
            DebugLog.Debug("APIUtil StoreTripsToDB Start vehicleID = " + vehicleID);
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            List<Models.Trip> tripdbs = new List<Models.Trip>();
            foreach (Models.API.Trip trip in trips)
            {
                Models.Trip tripdb = new Models.Trip();
                tripdb.vehicleId = vehicleID;
                tripdb.guid = trip.Id;
                if (null != trip.StartDateTime && !trip.StartDateTime.Value.Year.Equals(1))
                {
                    //转换成UTC时间存储到数据库
                    tripdb.startTime = trip.StartDateTime.Value.ToUniversalTime();
                }
                if (null != trip.EndDateTime && !trip.EndDateTime.Value.Year.Equals(1))
                {
                    //转换成UTC时间存储到数据库
                    tripdb.endtime = trip.EndDateTime.Value.ToUniversalTime();
                }
                if (null != trip.TripRoute && trip.TripRoute.Count >= 1)
                {
                    TripRouteDetail tempFirstTrip = trip.TripRoute.FirstOrDefault(t => t.location != null && t.location.longitude != 0 && t.location.latitude != 0);
                    if (null != tempFirstTrip)
                    {
                        tripdb.startlocationLat = tempFirstTrip.location.latitude;
                        tripdb.startlocationLng = tempFirstTrip.location.longitude;
                    }
                    if (null == trip.TripRoute.First().location || 0 == trip.TripRoute.First().location.latitude || 0 == trip.TripRoute.First().location.longitude)
                    {
                        tripdb.isFirstFlag = 0;
                    }
                    else
                    {
                        tripdb.isFirstFlag = 1;
                    }
                    TripRouteDetail tempLastTrip = trip.TripRoute.LastOrDefault(t => t.location != null && t.location.longitude != 0 && t.location.latitude != 0);
                    if (null != tempLastTrip)
                    {
                        tripdb.endlocationLat = tempLastTrip.location.latitude;
                        tripdb.endlocationLng = tempLastTrip.location.longitude;
                    }
                    if (null == trip.TripRoute.Last().location || 0 == trip.TripRoute.Last().location.latitude || 0 == trip.TripRoute.Last().location.longitude)
                    {
                        tripdb.isLastFlag = 0;
                    }
                    else
                    {
                        tripdb.isLastFlag = 1;
                    }
                }
                tripdb.distance = trip.Distance;
                tripdb.IdleTime = trip.IdleTime;
                tripdbs.Add(tripdb);
            }
            vehicleDB.AddTrip(vehicleID, tripdbs);
            DebugLog.Debug("APIUtil StoreTripsToDB End vehicleID = " + vehicleID);
        }

        //查找现在要存储的trips
        //chenyangwen 20140409 add trip and alert
        public static List<Models.API.Alert> findNowAlerts(List<Models.API.Alert> alerts, string alertID, string vehicleGuid)
        {
            DebugLog.Debug("APIUtil findNowAlerts Start alerts.Count = " + alerts.Count + ";alertID = " + alertID + ";vehicleGuid = " + vehicleGuid);
            List<Models.API.Alert> result = new List<Models.API.Alert>();
            alerts.Sort((a1, a2) => a2.TriggeredDateTime.CompareTo(a1.TriggeredDateTime));
            int index = alerts.FindIndex(t => null != t.Id ? t.Id == alertID : false);
            DebugLog.Debug("APIUtil findNowAlerts Running index = " + index);
            if (-1 == index)
            {
                int page = 1;
                while (true)
                {
                    page++;//翻页
                    alerts = GetPageAlerts(vehicleGuid, page);
                    if (null == alerts || 0 == alerts.Count)
                    {
                        break;
                    }

                    index = alerts.FindIndex(t => null != t.Id ? t.Id == alertID : false);
                    DebugLog.Debug("APIUtil findNowAlerts While index = " + index);
                    if (-1 == index)
                    {
                        result.AddRange(alerts);
                    }
                    else
                    {
                        result.AddRange(alerts.GetRange(0, index));
                        break;
                    }
                }
            }
            else
            {
                result.AddRange(alerts.GetRange(0, index));
            }

            return result;
        }

        //获取指定页数的trips
        //chenyangwen 20140409 add trip and alert
        public static List<Models.API.Alert> GetPageAlerts(string vehicleGuid, int page)
        {
            DebugLog.Debug("APIUtil GetPageAlerts Start vehicleGuid = " + vehicleGuid);
            IHalClient client = HalClient.GetInstance();
            HalLink link = new HalLink { Href = URI.VEHICLEALERTPAGE, IsTemplated = true };
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["ID"] = vehicleGuid;
            parameters["page"] = page + "";//有待分析，传入int数据没办法转换到url中
            Alerts alerts = client.Get<Alerts>(link, parameters).Result;//获取某一页的trips
            List<Models.API.Alert> result = new List<Models.API.Alert>();
            if (null != alerts && null != alerts.alerts)
            {
                result.AddRange(alerts.alerts);
            }
            DebugLog.Debug("APIUtil GetPageAlerts End vehicleGuid = " + vehicleGuid);
            return result;
        }

        //存取Alert数据到DB
        //chenyangwen 20140409 add trip and alert
        public static void StoreAlertsToDB(List<Models.API.Alert> alerts, long vehicleID, Models.Alert engineAlert)
        {
            if (null == alerts)
            {
                return;
            }
            DebugLog.Debug("APIUtil StoreAlertsToDB Start vehicleID = " + vehicleID);
            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            List<Models.Alert> alertdbs = new List<Models.Alert>();
            foreach (Models.API.Alert alert in alerts)
            {
                Models.Alert alertdb = new Models.Alert();
                alertdb.AlertType = alert.AlertType;
                if (null != alert.Details)
                {
                    alertdb.Value = alert.Details.ToString();
                }
                alertdb.vehicleId = vehicleID;
                if (null != alert.Id)
                {
                    alertdb.guid = alert.Id;
                    DebugLog.Debug("APIUtil StoreAlertsToDB alertdb.guid = " + alertdb.guid);
                }
                else if (null != alert.Links.Find(l => l.Rel == "self"))
                {
                    HalLink link = alert.Links.Find(l => l.Rel == "self");
                    alertdb.guid = link.Href.Substring(link.Href.LastIndexOf("/") + 1);
                }
                if (null != alert.TriggeredDateTime && !alert.TriggeredDateTime.Year.Equals(1))
                {
                    alertdb.TriggeredDateTime = alert.TriggeredDateTime.ToUniversalTime();
                }
                else
                {
                    alertdb.TriggeredDateTime = new DateTime(0);
                }
                alertdbs.Add(alertdb);
            }
            if (null != engineAlert)
            {
                alertdbs.Add(engineAlert);
            }
            vehicleDB.AddAlert(vehicleID, alertdbs);
            DebugLog.Debug("APIUtil StoreAlertsToDB End vehicleID = " + vehicleID);
        }

        //验证用户输入的OBU是否是正确的
        public static Device ValidateObu(string carrierid, string regkey)
        {
            try
            {
                DebugLog.Debug("APIUtil ValidateObu Start carrierid = " + carrierid + " ;regkey = " + regkey);
                IHalClient client = HalClient.GetInstance();

                //通过ID查找Device
                HalLink idLink = new HalLink { Href = URI.FINDDEVICEIDBYCARRIERID, IsTemplated = true };
                Dictionary<string, object> idPara = new Dictionary<string, object>();
                idPara["id"] = carrierid;

                //通过Regkey查找Device
                HalLink regkeyLink = new HalLink { Href = "/api/v1/devices/find/regkey/{id}", IsTemplated = true };
                Dictionary<string, object> regkeyPara = new Dictionary<string, object>();
                regkeyPara["id"] = regkey;

                Task<Device> deviceTask1 = client.Get<Device>(idLink, idPara);
                Task<Device> deviceTask2 = client.Get<Device>(regkeyLink, regkeyPara);
                Task<Device>[] deviceTasks = { deviceTask1, deviceTask2 };

                Device[] devices = Task.WhenAll(deviceTasks).Result;
                DebugLog.Debug("APIUtil ValidateObu End carrierid = " + carrierid + " ;regkey = " + regkey + " ;devices.Length = " + devices.Length);
                if (devices.Length > 1)
                {
                    if (null != devices[0].Id && devices[0].Id.Equals(devices[1].Id))
                    {
                        return devices[0];
                    }
                }
                return null;
            }
            catch (DBException dbException)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, dbException.Message);
                throw dbException;
            }
            catch (HalException halException)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, halException.Message);
                throw halException;
            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
                throw exception;
            }
        }

        //获取设备
        public static List<Device> GetDevices(HalLink link, Dictionary<string, object> parameters)
        {
            IHalClient client = HalClient.GetInstance();

            Task<Devices> task = client.Get<Devices>(link, parameters);
            Devices firstResults = task.Result;
            List<Device> result = new List<Device>();

            if (null == firstResults.devices)
            {
                return null;
            }

            result.AddRange(firstResults.devices);

            HalLink nextResultLink = firstResults.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);

            while (null != nextResultLink)
            {
                HalLink nextLink = new HalLink { Href = nextResultLink.Href, IsTemplated = false };
                Task<Devices> nextResult = client.Get<Devices>(nextLink);
                Devices nextPageResult = nextResult.Result;
                if (null != nextPageResult.devices)
                {
                    result.AddRange(nextPageResult.devices);
                }
                nextResultLink = nextPageResult.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);
            }
            return result;
        }

        //获取Trips
        public static List<Models.API.Trip> GetAllTrips(List<Models.API.Trip> apiTrips, string vehicleGuid)
        {
            DebugLog.Debug("APIUtil GetAllTrips Start");
            DebugLog.Debug("APIUtil GetAllTrips apiTrips.Count = " + apiTrips.Count + ", vehicleGuid = " + vehicleGuid);
            List<Models.API.Trip> result = new List<Models.API.Trip>();
            if (apiTrips.Count >=0 && apiTrips.Count < 50)
            {
                result.AddRange(apiTrips);
            }
            else if (apiTrips.Count >= 50)
            {
                result.AddRange(apiTrips.GetRange(0,50));
            }
            else
            {
                return null;
            }
            DebugLog.Debug("APIUtil GetAllTrips result.Count = " + result.Count + ", vehicleGuid = " + vehicleGuid);

            List<Models.API.Trip> resultTemp = new List<Models.API.Trip>();
            int page = 2;
            List<Models.API.Trip> trips = GetPageTrips(vehicleGuid, page);
            while (null != trips && 0 != trips.Count)
            {
                resultTemp.AddRange(trips);
                page++;//翻页
                trips = GetPageTrips(vehicleGuid, page);
            }

            if (0 < resultTemp.Count)
            {
                DebugLog.Debug("APIUtil GetAllTrips resultTemp.Count = " + resultTemp.Count + ", vehicleGuid = " + vehicleGuid);
                GetTripDetailList(resultTemp);
                result.AddRange(resultTemp);
            }

            DebugLog.Debug("APIUtil GetAllTrips resultAll.Count = " + result.Count + ", vehicleGuid = " + vehicleGuid);
            DebugLog.Debug("APIUtil GetAllTrips end result.Count = " + result.Count);
            return result;
        }

        //获取Alerts
        public static List<Models.API.Alert> GetAllAlerts(string vehicleGuid)
        {
            DebugLog.Debug("APIUtil GetAllAlerts Start");
            HalLink link = new HalLink { Href = URI.VEHICLEALERTS, IsTemplated = true };
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["ID"] = vehicleGuid;
            IHalClient client = HalClient.GetInstance();

            Task<Models.API.Alerts> task = client.Get<Models.API.Alerts>(link, parameters);
            Models.API.Alerts firstResults = task.Result;
            List<Models.API.Alert> result = new List<Models.API.Alert>();

            if (null == firstResults.alerts)
            {
                return null;
            }

            result.AddRange(firstResults.alerts);

            HalLink nextResultLink = firstResults.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);

            while (null != nextResultLink)
            {
                HalLink nextLink = new HalLink { Href = nextResultLink.Href, IsTemplated = false };
                Task<Models.API.Alerts> nextResult = client.Get<Models.API.Alerts>(nextLink);
                Models.API.Alerts nextPageResult = nextResult.Result;
                if (null != nextPageResult.alerts)
                {
                    result.AddRange(nextPageResult.alerts);
                }
                nextResultLink = nextPageResult.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);
            }
            DebugLog.Debug("APIUtil GetAllAlerts End");
            return result;
        }

        //获取Geofences
        public static List<Models.API.Geofence> GetGeofences(HalLink link, Dictionary<string, object> parameters)
        {
            IHalClient client = HalClient.GetInstance();

            Task<Models.API.Geofences> task = client.Get<Models.API.Geofences>(link, parameters);
            Models.API.Geofences firstResults = task.Result;
            List<Models.API.Geofence> result = new List<Models.API.Geofence>();

            if (null == firstResults.geofences)
            {
                return null;
            }

            result.AddRange(firstResults.geofences);

            HalLink nextResultLink = firstResults.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);

            while (null != nextResultLink)
            {
                HalLink nextLink = new HalLink { Href = nextResultLink.Href, IsTemplated = false };
                Task<Models.API.Geofences> nextResult = client.Get<Models.API.Geofences>(nextLink);
                Models.API.Geofences nextPageResult = nextResult.Result;
                if (null != nextPageResult.geofences)
                {
                    result.AddRange(nextPageResult.geofences);
                }
                nextResultLink = nextPageResult.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);
            }
            return result;
        }

        //同步车辆Api数据到数据库
        public static long addVehicleToDB(String companyID, Models.API.Vehicle vehicle)
        {
            try
            {
                DebugLog.Debug("APIUtil addVehicleToDB Start");
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                TenantDBInterface tenantDB = new TenantDBInterface();
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                Models.Vehicle vehicledb = new Models.Vehicle();
                vehicledb = new Models.Vehicle();
                vehicledb.id = vehicle.Id;
                vehicledb.tenantid = tenantDB.GetTenantIDByCompanyID(companyID);
                vehicledb.info = vehicle.Make + vehicle.Model + vehicle.year;
                vehicledb.vin = vehicle.Vin;
                vehicledb.isMMYEditable = vehicle.IsMMYEditable ? 1 : 0;
                vehicledb.isVinEditable = vehicle.IsVinEditable ? 1 : 0;
                if (null != vehicle.FriendlyName)
                {
                    vehicledb.name = vehicle.FriendlyName;
                }
                if (!(null == vehicle.Make || "UNKNOWN".Equals(vehicle.Make)
                    || null == vehicle.year || "9999".Equals(vehicle.year)
                    || null == vehicle.Model || "UNKNOWNCAR".Equals(vehicle.Model)))
                {
                    long mmyid = vehicleDB.GetMMYIdByMMY(vehicle.Model.Trim(), vehicle.Make.Trim(), vehicle.year.Trim());
                    if (-1 != mmyid)
                    {
                        vehicledb.mmyid = mmyid;
                    }
                }
                //添加车辆到数据库
                long vehicleID = vehicleDB.AddVehicle(companyID, vehicledb);
                List<Models.AlertConfiguration> alertConfigurationDBs = alertConfigDB.GetTenantAlertConfiguration(companyID);
                AddVehicleAlertConfiguration(vehicleID, vehicle.AlertConfigurations);
                AddAlertConfigurationToAPI(alertConfigurationDBs, vehicle.AlertConfigurations);
                DebugLog.Debug("APIUtil addVehicleToDB End");
                return vehicleID;
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil addVehicleToDB Excetion="+e.Message);
                throw e;
            }
        }

        public static void AddAlertConfigurationToAPI(List<Models.AlertConfiguration> alertConfigurationDBs, List<Models.API.AlertConfiguration> apiAlertConfigurations)
        {
            try
            {
                DebugLog.Debug("APIUtil AddAlertConfigurationToAPI Start");
                List<Task<IHalResult>> results = new List<Task<IHalResult>>();
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                foreach (Models.API.AlertConfiguration apiAlertConfig in apiAlertConfigurations)
                {
                    DebugLog.Debug("Worker AddAlertConfigurationToAPI apiAlertConfig.category = " + apiAlertConfig.Category);
                    if ("Motion Alerts".Equals(apiAlertConfig.Category) || "High RPM".Equals(apiAlertConfig.Category) || "High Speed".Equals(apiAlertConfig.Category) || "HighRPM".Equals(apiAlertConfig.Category) || "HighSpeed".Equals(apiAlertConfig.Category) || "MotionAlerts".Equals(apiAlertConfig.Category))
                    {
                        Models.AlertConfiguration dbAlertConfig = alertConfigurationDBs.Find(a => a.category == apiAlertConfig.Category);
                        DebugLog.Debug("Worker AddAlertConfigurationToAPI dbAlertConfig.category = " + dbAlertConfig.category);
                        results.Add(createTask(apiAlertConfig, dbAlertConfig));
                    }
                }
                IHalResult[] iHalResult = Task.WhenAll(results.ToArray()).Result;
                for (int i = 0; i < iHalResult.Length; i++)
                {
                    DebugLog.Debug("Worker AddAlertConfigurationToAPI i = " + i + " results isSuccess = " + iHalResult[i].Success);
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("Worker AddAlertConfigurationToAPI Exception = " + e.Message);
            }
        }

        //20140418
        public static void UpdateVehicleAlertConfiguration(List<Models.AlertConfiguration> alertConfigurationDBs, List<Models.API.AlertConfiguration> apiAlertConfigurations)
        {
            try
            {
                DebugLog.Debug("Worker UpdateVehicleAlertConfiguration start ");
                if (alertConfigurationDBs == null || apiAlertConfigurations == null || 0 == alertConfigurationDBs.Count)
                {
                    return;
                }
                List<Task<IHalResult>> results = new List<Task<IHalResult>>();
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();

                foreach (Models.API.AlertConfiguration apiAlertConfig in apiAlertConfigurations)
                {
                    DebugLog.Debug("Worker UpdateVehicleAlertConfiguration apiAlertConfig.category = " + apiAlertConfig.Category);
                    if ("GeoFence".Equals(apiAlertConfig.Category))
                    {
                        Models.AlertConfiguration dbAlertConfig = alertConfigurationDBs.Find(a => a.category == apiAlertConfig.Category);
                        List<AlertConfigurationParameter> paras = new List<AlertConfigurationParameter>();
                        AlertConfigurationParameter para1 = new AlertConfigurationParameter() { ParameterType = "ENTER_GEOFENCE", Value = "true", ParameterColumnName = "GEO_DIRECTION" };
                        AlertConfigurationParameter para2 = new AlertConfigurationParameter() { ParameterType = "EXIT_GEOFENCE", Value = "true", ParameterColumnName = "GEO_DIRECTION" };
                        paras.Add(para1);
                        paras.Add(para2);
                        DebugLog.Debug("Worker UpdateVehicleAlertConfiguration dbAlertConfig.category = " + dbAlertConfig.category);
                        results.Add(createTask(apiAlertConfig, dbAlertConfig, paras));
                    }
                }
                IHalResult[] iHalResult = Task.WhenAll(results.ToArray()).Result;
                for (int i = 0; i < iHalResult.Length; i++)
                {
                    DebugLog.Debug("Worker UpdateVehicleAlertConfiguration i = " + i + " results isSuccess = " + iHalResult[i].Success);
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("Worker UpdateVehicleAlertConfiguration Exception : " + e.Message);
                return;
            }
        }

        public static Task<IHalResult> createTask(Models.API.AlertConfiguration apiAlertConfig, Models.AlertConfiguration dbAlertConfig, List<AlertConfigurationParameter> parametersList = null)
        {
            DebugLog.Debug("Worker createTask Start");
            IHalClient client = HalClient.GetInstance();
            HalLink idLink = new HalLink { Href = URI.ALERTCONFIGURATIONS, IsTemplated = true };
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["ID"] = apiAlertConfig.Id;
            paras["State"] = dbAlertConfig.state;
            List<AlertConfigurationNotification> notifications = new List<AlertConfigurationNotification>();
            foreach (Notification notification in dbAlertConfig.Notification)
            {
                Models.API.AlertConfigurationNotification apiNo = new AlertConfigurationNotification() { NotificationType = notification.type, Value = notification.value };
                notifications.Add(apiNo);
            }
            List<AlertConfigurationParameter> parameters = new List<AlertConfigurationParameter>();
            if (null == parametersList)
            {
                foreach (Parameter parameter in dbAlertConfig.Parameter)
                {
                    Models.API.AlertConfigurationParameter apiPara = new AlertConfigurationParameter() { ParameterType = parameter.ParameterType, Value = parameter.value, ParameterColumnName = parameter.ParameterColumnName };
                    parameters.Add(apiPara);
                }
            }
            else
            {
                parameters.AddRange(parametersList);
            }
            paras["Notifications"] = notifications;
            paras["Parameters"] = parameters;
            return client.Put(idLink, paras);
        }

        //更新Obu
        public static void UpdateObu(long vehicleID, Models.API.Vehicle vehicle)
        {
            try
            {
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                DebugLog.Debug("APIUtil UpdateObu Start vehicleID = " + vehicleID + ";vehicle.guid =" + vehicle.Id);
                HalLink deviceLink = vehicle.Links.FirstOrDefault(v => v.Rel == "device");
                //假如存在设备
                if (null != deviceLink || null != vehicle.ConnectedDeviceId)
                {
                    string deviceguid = "";
                    if (null != vehicle.ConnectedDeviceId)
                    {
                        deviceguid = vehicle.ConnectedDeviceId;
                    }
                    else if (null != deviceLink)
                    {
                        deviceguid = deviceLink.Href.Substring(deviceLink.Href.LastIndexOf('/') + 1);
                    }
                    DebugLog.Debug("APIUtil UpdateObu Running deviceguid = " + deviceguid);
                    Obu obu = vehicleDB.GetOBUByGUID(deviceguid);
                    DealObu(obu, vehicleID);
                }
                DebugLog.Debug("APIUtil UpdateObu end vehicleID = " + vehicleID + ";vehicle.guid =" + vehicle.Id);
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil UpdateObu Exception="+e.Message);
            }
        }

        public static void AddVehicleAlertConfiguration(long vehicleID, List<Models.API.AlertConfiguration> alertConfigurations)
        {
            try
            {
                DebugLog.Debug("APIUtil AddVehicleAlertConfiguration start vehicleID = " + vehicleID);
                List<Vehicle_AlertConfiguration> alertConfigs = new List<Vehicle_AlertConfiguration>();
                foreach (Models.API.AlertConfiguration alertConfiguration in alertConfigurations)
                {
                    Vehicle_AlertConfiguration alertConfig = new Vehicle_AlertConfiguration();
                    alertConfig.alertConfigurationGuID = alertConfiguration.Id;
                    alertConfig.vehicleID = vehicleID;
                    alertConfig.category = alertConfiguration.Category;
                    alertConfigs.Add(alertConfig);
                }
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                vehicleDB.AddVehicleAlertConfiguration(alertConfigs);
                DebugLog.Debug("APIUtil AddVehicleAlertConfiguration end vehicleID = " + vehicleID);
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil AddVehicleAlertConfiguration Exception = " + e.Message);
            }
        }

        //通不过不更新车辆和Obu
        public static void UpdateVehicleObu(String companyID, List<Models.API.Vehicle> vehilces, List<Models.API.Device> devices)
        {
            try
            {
                DebugLog.Debug("APIUtil UpdateVehicleObu Start companyID = " + companyID + ";vehilces.Count =" + vehilces.Count);
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                TenantDBInterface tenantDB = new TenantDBInterface();
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                foreach (Models.API.Vehicle vehicle in vehilces)
                {
                    //获取车的alertconfiguration fengpan20140507
                    vehicle.AlertConfigurations = GetVehicleAlertConfigurations(vehicle.Id);

                    //车辆是否已经保存在数据库中
                    Models.Vehicle vehicledb = vehicleDB.GetVehicleByGUID(vehicle.Id);

                    //假如没有，插入数据库
                    if (null == vehicledb)
                    {
                        DebugLog.Debug("APIUtil UpdateVehicleObu Running vehicledb = null");
                        long vehicleID = addVehicleToDB(companyID, vehicle);//同步车辆到数据库
                        UpdateObu(vehicleID, vehicle);//更新Obu
                        ////向数据库添加
                        //AddVehicleAlertConfiguration(vehicleID, vehicle.AlertConfigurations);
                        //List<Models.AlertConfiguration> alertConfigurationDBs = alertConfigDB.GetTenantAlertConfiguration(companyID);
                        ////调用api重新设定
                        //UpdateVehicleAlertConfiguration(alertConfigurationDBs, vehicle.AlertConfigurations);
                        DebugLog.Debug("APIUtil UpdateVehicleObu Running vehicle.guid = " + vehicle.Id);
                    }
                    else
                    {
                        HalLink deviceLink = vehicle.Links.FirstOrDefault(v => v.Rel == "device");
                        //假如不存在设备,则该车辆为历史车辆，删除与OBU之间的关系,历史车辆
                        if (null == deviceLink)
                        {
                            DebugLog.Debug("APIUtil UpdateVehicleObu Running deviceLink = null");
                            Obu obu = vehicleDB.GetOBUByVehicleId(vehicledb.pkid);
                            if (null != obu)
                            {
                                vehicleDB.DeleteVehicleObuByVehicleID(vehicledb.pkid);
                            }
                        }
                        else if (null != deviceLink)
                        {
                            vehicle.ConnectedDeviceId = deviceLink.Href.Split('/').ElementAt(4);
                            //需要重新设定
                            List<Models.API.AlertConfiguration> updateAlertconfigurations = new List<Models.API.AlertConfiguration>();
                            //需要删除
                            List<Vehicle_AlertConfiguration> deleteAlertconfigurations = new List<Vehicle_AlertConfiguration>();
                            //数据库中的geofece配置
                            List<Vehicle_AlertConfiguration> geofenceAlertConfigurationsDB = vehicleDB.GetVehicleGeoAlertConfiguration(vehicledb.pkid);
                            //API中geofence配置
                            List<Models.API.AlertConfiguration> geofenceAlertConfigurations = new List<Models.API.AlertConfiguration>();
                            geofenceAlertConfigurations = vehicle.AlertConfigurations.FindAll(t => t.Category.Equals("GeoFence"));
                            //geofenceAlertConfigurations = vehicle.AlertConfigurations;
                            foreach (Models.API.AlertConfiguration geoConfigTempAdd in geofenceAlertConfigurations)
                            {
                                if (null == geofenceAlertConfigurationsDB.Find(t => t.alertConfigurationGuID == geoConfigTempAdd.Id))
                                {
                                    updateAlertconfigurations.Add(geoConfigTempAdd);
                                }
                            }
                            foreach (Vehicle_AlertConfiguration geoConfigTempDel in geofenceAlertConfigurationsDB)
                            {
                                if (null == geofenceAlertConfigurations.Find(t => t.Id == geoConfigTempDel.alertConfigurationGuID))
                                {
                                    deleteAlertconfigurations.Add(geoConfigTempDel);
                                }
                            }
                            //从数据库删除
                            vehicleDB.DeleteVehicleAlertConfiguration(vehicledb.pkid, deleteAlertconfigurations);
                            //向数据库添加
                            AddVehicleAlertConfiguration(vehicledb.pkid, updateAlertconfigurations);
                            List<Models.AlertConfiguration> alertConfigurationDBs = alertConfigDB.GetTenantAlertConfiguration(companyID);
                            //调用api重新设定
                            UpdateVehicleAlertConfiguration(alertConfigurationDBs, updateAlertconfigurations);
                            DebugLog.Debug("APIUtil UpdateVehicleObu Running vehicle.guid = " + vehicledb.id);

                            Obu obu = vehicleDB.GetOBUByVehicleId(vehicledb.pkid);
                            if (null == obu)
                            {
                                Obu device = vehicleDB.GetOBUByGUID(vehicle.ConnectedDeviceId);
                                if (null == device)
                                {
                                    Device newDevice = devices.Find(d => d.Id == vehicle.ConnectedDeviceId);
                                    if (null != newDevice)
                                    {
                                        Models.Obu newobu = new Models.Obu() { guid = newDevice.Id, id = newDevice.LabelId, idtype = newDevice.LabelIdType, regkey = newDevice.RegistrationNumber, status = "Active" };
                                        newobu.tenantid = tenantDB.GetTenantIDByCompanyID(companyID);
                                        newobu.pkid = vehicleDB.AddOBU(newobu);
                                        device = newobu;
                                    }
                                }
                                DealObu(device, vehicledb.pkid);
                            }
                            else if (obu.guid != vehicle.ConnectedDeviceId)
                            {
                                DebugLog.Debug("UpdateVehicleObu Run DeleteVehicleObuByVehicleID vehicledb.pkid = " + vehicledb.pkid);
                                vehicleDB.DeleteVehicleObuByVehicleID(vehicledb.pkid);
                                Vehicle_Obu vehicleobu = new Vehicle_Obu() { obuid = obu.pkid, vehicleid = vehicledb.pkid };
                                vehicleDB.AddVehicleObu(vehicleobu);
                            }
                        }
                    }
                }
                DebugLog.Debug("APIUtil UpdateVehicleObu End companyID = " + companyID + ";vehilces.Count =" + vehilces.Count);
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil UpdateVehicleObu Exception: " + e.Message);
                return;
            }
        }

        public static void DealObu(Obu obu, long vehicleID)
        {
            try
            {
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                DebugLog.Debug("APIUtil DealObu Start vehicleID = " + vehicleID);
                if (null != obu)
                {
                    DebugLog.Debug("APIUtil UpdateObu Running obu.pkid = " + obu.pkid);
                    Vehicle_Obu vehicleobu = vehicleDB.GetVehicleObuByObuID(obu.pkid);
                    if (null == vehicleobu)
                    {
                        //新关联到车辆的OBU
                        DebugLog.Debug("APIUtil UpdateObu Running vehicleobu = null");
                        vehicleobu = new Vehicle_Obu() { obuid = obu.pkid, vehicleid = vehicleID };
                        vehicleDB.AddVehicleObu(vehicleobu);
                    }
                    else
                    {
                        //旧的Obu重新关联到新的车辆，之前关联的车辆成为历史车辆
                        DebugLog.Debug("APIUtil UpdateObu Running vehicleobu != null");
                        vehicleDB.DeleteVehicleObu(vehicleobu);
                        vehicleobu = new Vehicle_Obu() { obuid = obu.pkid, vehicleid = vehicleID };
                        vehicleDB.AddVehicleObu(vehicleobu);
                    }
                }
                DebugLog.Debug("APIUtil DealObu Start vehicleID = " + vehicleID);
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil DealObu Exception : " + e.Message);
                return;
            }
        }

        public static List<Device> GetDevicesOfCustomer(String customerid)
        {
            DebugLog.Debug("APIUtil GetDevicesOfCustomer start customerid = " + customerid);
            try
            {
                List<Device> result = new List<Device>();
                IHalClient client = HalClient.GetInstance();
                HalLink link = new HalLink { Href = URI.CUSTOMERDEVICES, IsTemplated = true };
                Dictionary<String, object> para = new Dictionary<string, object>();
                para.Add("ID", customerid);
                Devices devices = client.Get<Devices>(link, para).Result;
                if (null != devices && null != devices.devices)
                {
                    result = devices.devices;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("APIUtil GetDevicesOfCustomer Exception : " + e.Message);
                return new List<Device>();
            }
        }
		
        /// <summary>
        /// fengpan 20140507 获取一个customer下的车辆。
        /// </summary>
        /// <param name="customerGuid"></param>
        /// <returns></returns>
        public static List<Models.API.Vehicle> GetCustomerVehicles(string customerGuid)
        {
            DebugLog.Debug("[APIUtil] GetCustomerVehicles paras:[customerGuid = " + customerGuid + "] start");
            try
            {
                //IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.CUSTOMERVEHICLES, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = customerGuid;
                //var result = client.Get<Vehicles>(idLink, paras).Result;
                //DebugLog.Debug("[APIUtil] GetCustomerVehicles return:[result.vehicles.Count = " + result.vehicles.Count + "] end");
                return GetVehicles(idLink, paras);
            }
            catch (Exception e)
            {
                DebugLog.Debug("[APIUtil] GetCustomerVehicles Exception:"+e.Message);
                return new List<Models.API.Vehicle>();
            }
        }

        //获取所有车辆
        private static List<Models.API.Vehicle> GetVehicles(HalLink link, Dictionary<string, object> parameters)
        {
            try
            {
                IHalClient client = HalClient.GetInstance();

                Task<Models.API.Vehicles> task = client.Get<Models.API.Vehicles>(link, parameters);
                Vehicles firstResults = task.Result;
                List<Models.API.Vehicle> result = new List<Models.API.Vehicle>();

                if (null == firstResults.vehicles)
                {
                    return null;
                }

                result.AddRange(firstResults.vehicles);

                HalLink nextResultLink = firstResults.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);

                while (null != nextResultLink)
                {
                    HalLink nextLink = new HalLink { Href = nextResultLink.Href, IsTemplated = false };
                    Task<Models.API.Vehicles> nextResult = client.Get<Models.API.Vehicles>(nextLink);
                    Vehicles nextPageResult = nextResult.Result;
                    if (null != nextPageResult.vehicles)
                    {
                        result.AddRange(nextPageResult.vehicles);
                    }
                    nextResultLink = nextPageResult.Links.FirstOrDefault(l => l.Rel == URI.LINK_NEXT);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// fengpan 20140507 获取一个customer的所有devices
        /// </summary>
        /// <param name="customerGuid"></param>
        /// <returns></returns>
        public static List<Models.API.Device> GetCustomerDevices(string customerGuid)
        {
            DebugLog.Debug("[APIUtil] GetCustomerDevices() paras:[customerGuid = " + customerGuid + "] start");
            try
            {
                //IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.CUSTOMERDEVICES, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = customerGuid;
                //var result = client.Get<Devices>(idLink, paras).Result;
                return GetDevices(idLink, paras);
            }
            catch (Exception e)
            {
                DebugLog.Debug("[APIUtil] GetCustomerDevices Exception:" + e.Message);
                return new List<Device>();
            }
        }

        /// <summary>
        /// fengpan 20140507 获取车辆的配置信息。
        /// </summary>
        /// <param name="vehicleGuid"></param>
        /// <returns></returns>
        public static List<Models.API.AlertConfiguration> GetVehicleAlertConfigurations(string vehicleGuid)
        {
            DebugLog.Debug("[APIUtil] GetVehicleAlertConfigurations() paras:[vehicleGuid = " + vehicleGuid + "] start");
            try
            {
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEALERTCONFIGURATIONS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGuid;
                var result = client.Get<AlertConfigurations>(idLink, paras).Result;
                if (null == result.alertconfigurations)
                {
                    return new List<Models.API.AlertConfiguration>();
                }
                DebugLog.Debug("[APIUtil] GetVehicleAlertConfigurations() paras:[result.alertconfigurations = " + result.alertconfigurations.Count + "] start");
                return result.alertconfigurations;
            }
            catch (Exception e)
            {
                DebugLog.Debug("[APIUtil] GetVehicleAlertConfigurations() Exception:" + e.Message);
                return new List<Models.API.AlertConfiguration>();
            }
        }

        /**********************/
        //mabiao 20140507 
        public static List<CustomerData> GetVehiclesInfo(string companyId)
        {
            try
            {
                DebugLog.Debug("APIUtil GetVehiclesInfo Start companyId = " + companyId + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                List<FleetManageToolWebRole.Models.Vehicle> allvehicles = vehicleInterface.GetVehiclesByCompanyID(companyId);
                if (null == allvehicles || 0 == allvehicles.Count)
                {
                    DebugLog.Debug("APIUtil GetVehiclesInfo End, null == allvehicles");
                    return new List<CustomerData>();
                }

                List<CustomerData> customerDatas = new List<CustomerData>();

                List<Models.API.Vehicle> vehiclesFromApi = GetALLVehicles(allvehicles);
                if (null != vehiclesFromApi)
                {
                    GetVehiclesAlerts(vehiclesFromApi);
                    GetVehiclesTrips(vehiclesFromApi);
                    GetVehiclesEngine(vehiclesFromApi);

                    DoWithVehiclesLocation(vehiclesFromApi, companyId);

                    saveDataToCatch(vehiclesFromApi, companyId, customerDatas);
                }
                else
                {
                    DebugLog.Debug("APIUtil GetVehiclesInfo End, null == vehiclesFromApi");
                }

                DebugLog.Debug("APIUtil GetVehiclesInfo END ,EndTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                return customerDatas;
            }
            catch (DBException dbException)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, "GetVehiclesInfo " + dbException.Message);
                DebugLog.Exception(DebugLog.DebugType.DBException, "GetVehiclesInfo " + dbException.StackTrace);
                throw dbException;
            }
            catch (HalException halException)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, "GetVehiclesInfo " + halException.Message);
                DebugLog.Exception(DebugLog.DebugType.HttpException, "GetVehiclesInfo " + halException.StackTrace);
                throw halException;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesInfo " + e.Message);
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesInfo " +  e.StackTrace);
                throw e;
            }
        }

        private static void saveDataToCatch(List<Models.API.Vehicle> vehiclesFromApi, string companyID, List<CustomerData> customerDatas)
        {
            DebugLog.Debug("APIUtil saveDataToCatch Start");
            if (null != customerDatas)
            {
                CustomerData data = new CustomerData();
                data.Vehicles = vehiclesFromApi;
                customerDatas.Add(data);
                CacheService service = new CacheService();
                service.CachePut(companyID + "_Cache", customerDatas);
            }
            DebugLog.Debug("APIUtil saveDataToCatch End");
        }

        private static void DoWithVehiclesLocation(List<Models.API.Vehicle> vehiclesFromApi, string companyID)
        {
            try
            {
                DebugLog.Debug("APIUtil DoWithVehiclesLocation Start");
                if (null == vehiclesFromApi)
                {
                    DebugLog.Debug("APIUtil DoWithVehiclesLocation End. vehiclesFromApi is null.");
                    return;
                }

                if (0 >= vehiclesFromApi.Count)
                {
                    DebugLog.Debug("APIUtil DoWithVehiclesLocation End. vehiclesFromApi.Count = " + vehiclesFromApi.Count);
                    return;
                }

                CacheService service = new CacheService();
                object myObject = service.CacheGet(companyID + "_Cache");
                if (null == myObject)
                {
                    DebugLog.Debug("APIUtil DoWithVehiclesLocation End. service.CacheGet is null, myObject.");
                    return;
                }

                List<CustomerData> customers = (List<CustomerData>)myObject;
                if (null == customers)
                {
                    DebugLog.Debug("APIUtil DoWithVehiclesLocation End. customers is null.");
                    return;
                }

                if (0 >= customers.Count)
                {
                    DebugLog.Debug("APIUtil DoWithVehiclesLocation End. customers.Count = " + customers.Count);
                    return;
                }

                foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
                {
                    if (null == vehicleTemp)
                    {
                        DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicleTemp is null");
                        continue;
                    }

                    if (null == vehicleTemp.VehicleLocation)
                    {
                        DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicleTemp.VehicleLocation is null, vehicleTemp.Id = " + vehicleTemp.Id);
                        continue;
                    }

                    if (null == vehicleTemp.VehicleLocation.Location)
                    {
                        DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicleTemp.VehicleLocation.Location is null");
                        continue;
                    }

                    DebugLog.Debug("APIUtil DoWithVehiclesLocation do with Vehicles Locationve begin. vehicle.Id = " + vehicleTemp.Id + ", longitude = " + vehicleTemp.VehicleLocation.Location.longitude + ", latitude = " + vehicleTemp.VehicleLocation.Location.latitude);

                    if ((-0.1 < vehicleTemp.VehicleLocation.Location.longitude && 1 > vehicleTemp.VehicleLocation.Location.longitude) &&
                        (-0.1 < vehicleTemp.VehicleLocation.Location.latitude && 1 > vehicleTemp.VehicleLocation.Location.latitude))
                    {
                        DebugLog.Debug("APIUtil DoWithVehiclesLocation longitude = " + vehicleTemp.VehicleLocation.Location.longitude + ", latitude = " + vehicleTemp.VehicleLocation.Location.latitude);

                        foreach (Models.API.CustomerData customer in customers)
                        {
                            if (null == customer)
                            {
                                DebugLog.Debug("APIUtil DoWithVehiclesLocation customer is null");
                                continue;
                            }

                            if (null == customer.Vehicles)
                            {
                                DebugLog.Debug("APIUtil DoWithVehiclesLocation customer.Vehicles is null");
                                continue;
                            }

                            if (null == customer.Vehicles)
                            {
                                DebugLog.Debug("APIUtil DoWithVehiclesLocation customer.Vehicles is null");
                                continue;
                            }

                            if (0 >= customer.Vehicles.Count)
                            {
                                DebugLog.Debug("APIUtil DoWithVehiclesLocation customer.Vehicles.Count = " + customer.Vehicles.Count);
                                continue;
                            }

                            foreach (Models.API.Vehicle vehicle in (customer.Vehicles))
                            {
                                if (null == vehicle)
                                {
                                    DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicle is null");
                                    continue;
                                }

                                if (null == vehicle.VehicleLocation)
                                {
                                    DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicle.VehicleLocation is null, vehicle.Id = " + vehicle.Id);
                                    continue;
                                }

                                if (null == vehicle.VehicleLocation.Location)
                                {
                                    DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicle.VehicleLocation.Location is null");
                                    continue;
                                }

                                if (vehicle.Id == vehicleTemp.Id)
                                {
                                    vehicleTemp.VehicleLocation.Location.longitude = vehicle.VehicleLocation.Location.longitude;
                                    vehicleTemp.VehicleLocation.Location.latitude = vehicle.VehicleLocation.Location.latitude;
                                    DebugLog.Debug("APIUtil DoWithVehiclesLocation vehicle.Id = " + vehicle.Id);
                                }
                            }
                        }
                        DebugLog.Debug("APIUtil DoWithVehiclesLocation do with Vehicles Locationve end. vehicle.Id = " + vehicleTemp.Id);
                    }
                }
                DebugLog.Debug("APIUtil DoWithVehiclesLocation End");
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "DoWithVehiclesLocation " + e.Message);
            }
        }

        private static List<Models.API.Vehicle> GetALLVehicles(List<FleetManageToolWebRole.Models.Vehicle> allvehicles)
        {
            try
            {
                DebugLog.Debug("APIUtil GetALLVehicles Start" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                List<Models.API.Vehicle> result = new List<Models.API.Vehicle>();
                IHalClient client = HalClient.GetInstance();
                List<Task<Models.API.Vehicle>> vehiclesTasks = new List<Task<Models.API.Vehicle>>();
                foreach (FleetManageToolWebRole.Models.Vehicle vehicleTemp in allvehicles)
                {
                    HalLink vehicleLink = new HalLink { Href = URI.VEHICLES, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("ID", vehicleTemp.id);
                    Task<Models.API.Vehicle> vehicleTask = client.GetOrDefault<Models.API.Vehicle>(vehicleLink, parameters);
                    vehiclesTasks.Add(vehicleTask);
                }
                if (null == Task.WhenAll(vehiclesTasks).Result)
                {
                    return result;
                }
                foreach (Models.API.Vehicle vehicleTemp in Task.WhenAll(vehiclesTasks).Result)
                {
                    if (null != vehicleTemp && 0 != vehicleTemp.Links.Count)
                    {
                        HalLink deviceLink = vehicleTemp.Links.FirstOrDefault(l => l.Rel == URI.LINK_DEVICE);
                        if (null != deviceLink)
                        {
                            string deviceGUID = deviceLink.Href.Split('/').ElementAt(4);
                            vehicleTemp.ConnectedDeviceId = deviceGUID;
                        }
                        result.Add(vehicleTemp);
                    }
                }
                DebugLog.Debug("APIUtil GetALLVehicles End" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetCustomerVehicles" + e.Message);
                return null;
            }
        }

        private static void GetVehiclesAlerts(List<Models.API.Vehicle> vehiclesFromApi)
        {
           try
           {
               DebugLog.Debug("APIUtil GetVehiclesAlerts Start" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
               IHalClient client = HalClient.GetInstance();
               List<Task<Alerts>> alertTasks = new List<Task<Alerts>>();
               foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
               {
                   //TODO历史车辆跳过
                   if (null == vehicleTemp.ConnectedDeviceId)
                   {
                       continue;
                   }
                   HalLink alertLink = new HalLink { Href = URI.VEHICLEALERTTIME, IsTemplated = true };
                   Dictionary<string, object> parameters = new Dictionary<string, object>();
                   parameters.Add("ID", vehicleTemp.Id);
                   parameters.Add("startTime", DateTime.Now.AddHours(-36).ToString("o")); //获取一天半
                   Task<Alerts> alertTask = client.GetOrDefault<Alerts>(alertLink, parameters);
                   alertTasks.Add(alertTask);
               }
               VehicleDBInterface vehicleDB = new VehicleDBInterface();
               TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
               DebugLog.Debug("APIUtil GetVehiclesAlerts Result Begin" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
               foreach (Alerts alertsTemp in Task.WhenAll(alertTasks).Result)
               {
                   try
                   {
                       if (null != alertsTemp && 0 != alertsTemp.Links.Count)
                       {
                           HalLink nextResultLink = alertsTemp.Links.FirstOrDefault(l => l.Rel == URI.LINK_SELE);
                           string vehicleGUID = nextResultLink.Href.Split('/').ElementAt(4);//TODO
                           foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
                           {
                               if (null != vehicleTemp && vehicleGUID.Equals(vehicleTemp.Id))
                               {
                                   vehicleTemp.Alerts = alertsTemp.alerts;
                                   HalLink nextHref  = alertsTemp.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                                   while (null != nextHref)
                                   {
                                       HalLink link = new HalLink() { Href = nextHref.Href, IsTemplated = false };
                                       Alerts nextAlerts = client.Get<Alerts>(link).Result;
                                       if (null == nextAlerts || null == nextAlerts.alerts)
                                       {
                                           break;
                                       }
                                       vehicleTemp.Alerts.AddRange(nextAlerts.alerts);
                                       nextHref = nextAlerts.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                                   }
                                   continue;
                                   /*FleetManageToolWebRole.Models.Alert lastAlert = vehicleDB.GetLastAlertByGUID(vehicleTemp.Id);
                                   int page = 1;
                                   int maxPage = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FlipPageNum"]);
                                   while (null == lastAlert || null == vehicleTemp.Alerts.Find(t => t.TriggeredDateTime.ToUniversalTime().CompareTo(lastAlert.TriggeredDateTime.Value.Add(backTimeZone).ToUniversalTime()) <= 0))
                                   {
                                       page++;
                                       if (page > maxPage)
                                       {
                                           break;
                                       }
                                       List<Models.API.Alert> pageResult = APIUtil.GetPageAlerts(vehicleTemp.Id, page);
                                       if (0 != pageResult.Count)
                                       {
                                           vehicleTemp.Alerts.AddRange(pageResult);
                                       }
                                       else
                                       {
                                           break;
                                       }
                                   }
                                   continue;*/
                               }
                           }
                       }
                   }
                   catch (Exception e)
                   {
                       DebugLog.Debug("APIUtil GetVehiclesAlerts " + e.Message);
                   }
               }
               DebugLog.Debug("APIUtil GetVehiclesAlerts End" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
           }
           catch (Exception e)
           {
               DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesAlerts " + e.Message);
           }
        }

        private static void GetVehiclesTrips(List<Models.API.Vehicle> vehiclesFromApi)
        {
            try
            {
                DebugLog.Debug("APIUtil GetVehiclesTrips Start" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                IHalClient client = HalClient.GetInstance();
                List<Task<Trips>> tripTasks = new List<Task<Trips>>();
                List<Task<Models.API.Trip>> tripDetailTasks = new List<Task<Models.API.Trip>>();
                foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
                {
                    //TODO历史车辆跳过
                    if (null == vehicleTemp.ConnectedDeviceId)
                    {
                        continue;
                    }
                    HalLink tripLink = new HalLink { Href = URI.VEHICLETRIPS, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("ID", vehicleTemp.Id);
                    Task<Trips> tripTask = client.GetOrDefault<Trips>(tripLink, parameters);
                    tripTasks.Add(tripTask);
                }
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                DebugLog.Debug("APIUtil GetVehiclesTrips Result Begin" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                foreach (Trips tripTemp in Task.WhenAll(tripTasks).Result)
                {
                    if (null != tripTemp && 0 != tripTemp.Links.Count)
                    {
                        HalLink nextResultLink = tripTemp.Links.FirstOrDefault(l => l.Rel == URI.LINK_SELE);
                        string vehicleGUID = nextResultLink.Href.Split('/').ElementAt(4);
                        foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
                        {
                            if (null != vehicleTemp && vehicleGUID.Equals(vehicleTemp.Id))
                            {
                                vehicleTemp.Trips = tripTemp.trips;
                                //ToDo 查找最后一条Trip
                                FleetManageToolWebRole.Models.Trip lastTrip = vehicleDB.GetLastTripByGUID(vehicleTemp.Id);
                                int page = 1;
                                int maxPage = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FlipPageNum"]);
                                while (null == lastTrip||
                                       null == vehicleTemp.Trips.Find(t => (t.StartDateTime != null || t.EndDateTime != null) &&
                                                                          (t.StartDateTime == null ? t.EndDateTime.Value.ToUniversalTime() : t.StartDateTime.Value.ToUniversalTime()).CompareTo(
                                                                                                                    lastTrip.startTime == null ? lastTrip.endtime.Value.Add(backTimeZone).ToUniversalTime() :
                                                                                                                    lastTrip.startTime.Value.Add(backTimeZone).ToUniversalTime()) <= 0)
                                       )
                                {
                                    page++;
                                    if (page > maxPage)
                                    {
                                        break;
                                    }
                                    List<Models.API.Trip> pageResult = APIUtil.GetPageTrips(vehicleTemp.Id, page);
                                    if (0 != pageResult.Count)
                                    {
                                        vehicleTemp.Trips.AddRange(pageResult);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                continue;
                            }
                        }
                    }
                }
                DebugLog.Debug("APIUtil GetVehiclesTrips End" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesTrips " + e.Message);
            }
        }

        private static void GetVehiclesEngine(List<Models.API.Vehicle> vehiclesFromApi)
        {
            try
            {
                DebugLog.Debug("APIUtil GetVehiclesEngine Start" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
                List<FleetManageToolWebRole.Models.Alert> engnieAlerts = new List<FleetManageToolWebRole.Models.Alert>();
                VehicleDBInterface vehicleDB = new VehicleDBInterface();
                foreach (Models.API.Vehicle vehicleTemp in vehiclesFromApi)
                {
                    FleetManageToolWebRole.Models.Alert engnieAlert = APIUtil.EngineAlertStatu(vehicleTemp);
                    if (null != engnieAlert)
                    {
                        engnieAlert.vehicleId = vehicleDB.GetVehicleByGUID(vehicleTemp.Id).pkid;
                        engnieAlerts.Add(engnieAlert);
                    }
                }
                vehicleDB.AddAlert(0, engnieAlerts);
                DebugLog.Debug("APIUtil GetVehiclesEngine End" + ",StartTime.toString=" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"));
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesEngine " + e.Message);
            }
        }
        /****************************/
        //caoyandong #
        public static Models.API.Vehicle GetOneVehicleInfo(string VehicleId)
        {
            try
            {
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLES, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = VehicleId;
                Models.API.Vehicle result = new Models.API.Vehicle();
                result = client.Get<Models.API.Vehicle>(idLink, paras).Result;
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehiclesEngine " + e.Message);
                throw new Exception(e.Message);
            }
        }

        public static void GetTripDetailList(List<Models.API.Trip> trips)
        {
            DebugLog.Debug("APIUtil GetTripDetailList Start Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            DebugLog.Debug("APIUtil GetTripDetailList trips.count:" + trips.Count);
            int count = trips.Count;
            if (0 >= count)
            {
                DebugLog.Debug("APIUtil GetTripDetailList END Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                return;
            }

            int pageNumber = 20;
            int range = count / pageNumber;
            int i = 0;
            for (; i < range; ++i)
            {
                GetTripDetailListByRange(trips.GetRange(i * pageNumber, pageNumber));
            }

            if (0 < (count % pageNumber))
            {
                GetTripDetailListByRange(trips.GetRange(i * pageNumber, count % pageNumber));
            }

            DebugLog.Debug("APIUtil GetTripDetailList END Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
        }

        public static void GetTripDetailListByRange(List<Models.API.Trip> trips)
        {
            DebugLog.Debug("APIUtil GetTripDetailListByRange Start Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            try
            {
                IHalClient client = HalClient.GetInstance();
                List<Task<Models.API.Trip>> tripTasks = new List<Task<Models.API.Trip>>();
                foreach (Models.API.Trip tripTemp in trips)
                {
                    HalLink tripLink = new HalLink { Href = Models.API.URI.TRIPS, IsTemplated = true };
                    Dictionary<string, object> idPara = new Dictionary<string, object>();
                    idPara[FleetManageToolWebRole.Models.Constant.CommonConstant.ID] = tripTemp.Id;
                    Task<Models.API.Trip> tripTask = client.Get<Models.API.Trip>(tripLink, idPara);
                    tripTasks.Add(tripTask);
                }

                foreach (Models.API.Trip tripTemp in Task.WhenAll(tripTasks).Result)
                {
                    trips.Find(t => t.Id == tripTemp.Id).TripRoute = tripTemp.TripRoute;
                    if (tripTemp.TripRoute.Count <= 0)
                    {
                        DebugLog.Debug("APIUtil GetTripDetailListByRange Count of tripTemp TripRoute is 0.Trip GUID is :" + tripTemp.Id);
                    }
                }
                DebugLog.Debug("APIUtil GetTripDetailListByRange END Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                throw e;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                throw e;
            }
        }


        public static void testTripComparer()
        {
            List<Models.API.Trip> tripsDay = new List<Models.API.Trip>();
            Models.API.Trip trip0 = new Models.API.Trip();
            trip0.Id = "0105af56-4ce1-482a-b34c-6b17e8002b90";
            tripsDay.Add(trip0);

            Models.API.Trip trip1 = new Models.API.Trip();
            trip1.Id = "0105af56-4ce1-482a-b34c-6b17e8002b90";
            tripsDay.Add(trip1);

            Models.API.Trip trip2 = new Models.API.Trip();
            trip2.Id = "0105af56-4ce1-482a-b34c-6b17e8002b90";
            tripsDay.Add(trip2);

            List<Models.API.Trip> tripsDayDistinct = tripsDay.Distinct(new TripComparer()).ToList();
            int ii = tripsDayDistinct.Count();

            List<Models.API.Alert> AlertsDay = new List<Models.API.Alert>();
            Models.API.Alert Alert0 = new Models.API.Alert();
            Alert0.Id = "P5522R2520034561122699991";
            AlertsDay.Add(Alert0);

            Models.API.Alert Alert1 = new Models.API.Alert();
            Alert1.Id = "P5522R2520034561122699991";
            AlertsDay.Add(Alert1);

            Models.API.Alert Alert2 = new Models.API.Alert();
            Alert2.Id = "P5522R2520034561122699991";
            AlertsDay.Add(Alert2);

            List<Models.API.Alert> AlertsDayDistinct = AlertsDay.Distinct(new AlertAPIComparer()).ToList();



            ii = AlertsDayDistinct.Count();
        }

        public static void testRemoveDuplicateTripAndAlert()
        {
            List<Models.API.Trip> result1 = new List<Models.API.Trip>();
            Models.API.Trip trip1 = new Models.API.Trip();
            Models.API.Trip trip2 = new Models.API.Trip();
            trip1.Id = "0105af56-4ce1-482a-b34c-6b17e8002b90";
            trip2.Id = "76bc1e8f-e9cb-40f2-bdfd-95f4dc8cc";
            result1.Add(trip1);
            result1.Add(trip2);
            long vehiclePKID = 10484;
            result1 = RemoveDuplicateTrip(result1, vehiclePKID);

            List<Models.API.Alert> result2 = new List<Models.API.Alert>();
            Models.API.Alert Alert1 = new Models.API.Alert();
            Models.API.Alert Alert2 = new Models.API.Alert();
            Alert1.Id = "P5522R2520034561122699991";
            Alert2.Id = "P5522R25200345611226999910";
            result2.Add(Alert1);
            result2.Add(Alert2);
            vehiclePKID = 10470;
            result2 = RemoveDuplicateAlert(result2, vehiclePKID);

            return;
        }

    }

    class TripComparer : IEqualityComparer<Models.API.Trip>
    {
        public bool Equals(Models.API.Trip x, Models.API.Trip y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            return (x.Id).Equals(y.Id);
        }

        public int GetHashCode(Models.API.Trip product)
        {
            if (Object.ReferenceEquals(product, null))
            {
                return 0;
            }
            int tripId = product.Id == null ? 0 : product.Id.GetHashCode();

            return tripId;
        }
    }

    class AlertAPIComparer : IEqualityComparer<Models.API.Alert>
    {
        public bool Equals(Models.API.Alert x, Models.API.Alert y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            return (x.Id).Equals(y.Id);
        }

        public int GetHashCode(Models.API.Alert product)
        {
            if (Object.ReferenceEquals(product, null))
            {
                return 0;
            }
            int tripId = product.Id == null ? 0 : product.Id.GetHashCode();

            return tripId;
        }
    }

}