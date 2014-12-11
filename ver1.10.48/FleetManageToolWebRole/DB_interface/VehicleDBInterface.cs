using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole.DB_interface
{
    public class VehicleDBInterface
    {
        //测试OK
        public List<Vehicle> GetTenantVehiclesByCompannyID(string companyID)
        {
            List<Vehicle> result = new List<Vehicle>();
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                var vehicles = from vehicle in db.Vehicle
                               join tenant in db.Tenant on vehicle.tenantid equals tenant.pkid
                               where tenant.companyid == companyID
                               select vehicle;
                foreach (Vehicle vehicle in vehicles)
                {
                    var vehicleobus = from vehicleobu in db.Vehicle_Obu
                                      where vehicleobu.vehicleid == vehicle.pkid
                                      select vehicleobu;
                    if (null != vehicleobus && 0 != vehicleobus.Count())
                    {
                        if (vehicle.vin == null)
                        {
                            vehicle.vin = "";
                        }
                        VehicleGroup group =GetGroupByVehicleId(vehicle.pkid);
                        if (null == group)
                        {
                            vehicle.VehicleGroup_Vehicle = null; 
                        }else{
                            vehicle.VehicleGroup_Vehicle.Add(new VehicleGroup_Vehicle() { vehicleid = vehicle.pkid, VehicleGroup = group });
                        }
                        result.Add(vehicle);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取租户的所有车辆异常," + e.Message);
            }
        }

        //测试OK
        public Vehicle GetVehicleByID(long vehicleID)
        {
            try
            {
                Vehicle result = null;
                var db = new FleetManageToolDBContext();
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                                where vehicleDB.pkid == vehicleID 
                                                select vehicleDB;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    result = vehicleTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过pkid取得车辆信息异常:" + e.Message);
            }
        }

        //测试OK
        public List<Vehicle> GetVehicleByIDs(List<long> vehicleIDs)
        {
            try
            {
                List<Vehicle> result = new List<Vehicle>();
                var db = new FleetManageToolDBContext();
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                                where vehicleIDs.Contains(vehicleDB.pkid)
                                                select vehicleDB;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    result.Add(vehicleTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过pkids取得车辆信息异常:" + e.Message);
            }
        }

        //测试OK
        //通过公司ID获取公司现有车组
        public List<VehicleGroup> GetGroupsByCompannyID(string companyID)
        {
            List<VehicleGroup> groups = new List<VehicleGroup>();
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<VehicleGroup> groupsTemp = from groupsDB in db.VehicleGroup
                                                       join tenantDB in db.Tenant on groupsDB.tenantid equals tenantDB.pkid
                                                       where tenantDB.companyid == companyID
                                                       select groupsDB;
                foreach (VehicleGroup group in groupsTemp)
                {
                    groups.Add(group);
                }
                return groups;
            }
            catch (Exception e)
            {
                throw new DBException("获取租户的所有车组异常," + e.Message);
            }
        }

        //mabiao 20140507
        public List<Vehicle> GetVehiclesByCompanyID(string companyID)
        {
            List<Vehicle> result = new List<Vehicle>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            try
            {
                IEnumerable<Vehicle> vehicles = from vehicle in dbContext.Vehicle
                                                join tenant in dbContext.Tenant on companyID equals tenant.companyid
                                                where vehicle.tenantid == tenant.pkid
                                                select vehicle;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    result.Add(vehicleTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取租户的所有车异常," + e.Message);
            }
        }

		public VehicleGroup GetGroupByVehicleId(long vehicleID)
        {
            VehicleGroup result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<VehicleGroup> vehicleGroups = from groupDB in db.VehicleGroup
                                                          join vehicleGroupDB in db.VehicleGroup_Vehicle on groupDB.pkid equals vehicleGroupDB.groupid
                                                          where vehicleGroupDB.vehicleid == vehicleID
                                                          select groupDB;
                foreach (VehicleGroup vehicleGroup in vehicleGroups)
                {
                    result = vehicleGroup;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取车辆的车组异常," + e.Message);
            }
        }

        //测试OK
        public Vehicle GetVehicleByGUID(string guid)
        {
            Vehicle result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                                where vehicleDB.id == guid
                                                select vehicleDB;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    result = vehicleTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过ID获取车辆异常," + e.Message);
            }
        }
        //测试OK
        public List<Vehicle> GetVehiclesByGUIDs(List<string> guids)
        {
            List<Vehicle> result = new List<Vehicle>();
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                //IEnumerable<Vehicle> vehicles = db.Vehicle.Where(v => null != guids.Find(t => t == v.id));
                foreach (string guid in guids)
                {
                    IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                                    where vehicleDB.id == guid
                                                    select vehicleDB;
                    foreach (Vehicle vehicleTemp in vehicles)
                    {
                        result.Add(vehicleTemp);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过IDs获取车辆异常," + e.Message);
            }
        }

        //通过GUID获取Obu
        public Obu GetOBUByGUID(string guid)
        {
            Obu result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Obu> obus = from obuDB in db.Obu
                                        where obuDB.guid == guid
                                        select obuDB;
                foreach (Obu obu in obus)
                {
                    result = obu;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过GUID获取OBU异常," + e.Message);
            }
        }

        public Obu GetOBUByVehicleId(long vehicleID)
        {
            Obu result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Obu> obus = from obuDB in db.Obu
                                        join vehicleObuDB in db.Vehicle_Obu on obuDB.pkid equals vehicleObuDB.obuid
                                        where vehicleObuDB.vehicleid == vehicleID
                                        select obuDB;
                foreach (Obu obu in obus)
                {
                    result = obu;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过GUID获取OBU异常," + e.Message);
            }
        }

        public Vehicle GetVehicleByVehicleName(string companyID, string vehiclename)
        {
            Vehicle result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                                join tenantDB in db.Tenant on vehicleDB.tenantid equals tenantDB.pkid
                                                join vehicle_obuDB in db.Vehicle_Obu on vehicleDB.pkid equals vehicle_obuDB.vehicleid
                                                where vehicleDB.name == vehiclename && tenantDB.companyid == companyID
                                                select vehicleDB;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    result = vehicleTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过车名获取车名异常," + e.Message);
            }
        }

        public void AddVehicleObu(Vehicle_Obu vehicleObu)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Vehicle_Obu.Add(vehicleObu);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加Vehicle_OBU异常," + e.Message);
            }
        }

        public long GetLogoIDByVehicleID(long vehicleID)
        {
            long result = 0;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Vehicle> vehicles = from vehicleDB in db.Vehicle
                                            where vehicleDB.pkid == vehicleID
                                                select vehicleDB;
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    if (null != vehicleTemp.logoid)
                    {
                        result = (long)vehicleTemp.logoid;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取车辆图标异常," + e.Message);
            }
        }

        //测试OK
        //通过车组获取该该车组的车辆
        public List<Vehicle> GetGroupVehiclesByGroupId(long groupID)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetGroupVehiclesByGroupId start");
                List<Vehicle> vehicles = new List<Vehicle>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Vehicle> vehiclesTemp = from vehicleDB in db.Vehicle
                                                    join vehiclegroupDB in db.VehicleGroup_Vehicle on vehicleDB.pkid equals vehiclegroupDB.vehicleid
                                                    where vehiclegroupDB.groupid == groupID
                                                    select vehicleDB;
                foreach (Vehicle vehicle in vehiclesTemp)
                {
                    //fengpan 排除历史车辆 20140520
                    var vehicleobus = from vehicleobu in db.Vehicle_Obu
                                      where vehicleobu.vehicleid == vehicle.pkid
                                      select vehicleobu;
                    if (null != vehicleobus && 0 != vehicleobus.Count())
                    {
                        vehicles.Add(vehicle);
                    }
                }
                DebugLog.Debug("VehicleDBInterface GetGroupVehiclesByGroupId end");
                return vehicles;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new DBException("通过车组获取组内车辆异常," + e.Message);
            }
        }

        public Vehicle_Obu GetVehicleObuByObuID(long obuID)
        {
            Vehicle_Obu result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            try
            {
                IEnumerable<Vehicle_Obu> vehicleobus = from vehicleDB in db.Vehicle_Obu
                                                       where vehicleDB.obuid == obuID
                                                    select vehicleDB;
                foreach (Vehicle_Obu vehicleobu in vehicleobus)
                {
                    result = vehicleobu;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过obuid获取车辆和OBU关系异常," + e.Message);
            }
        }

        public void DeleteVehicleObu(Vehicle_Obu vehicleobu)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                IEnumerable<Vehicle_Obu> vehicleobus = dbContext.Vehicle_Obu.Where(Vehicle_Obu => Vehicle_Obu.obuid == Vehicle_Obu.obuid);
                foreach (Vehicle_Obu vehicleobuTemp in vehicleobus)
                {
                    dbContext.Vehicle_Obu.Remove(vehicleobuTemp);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除车组异常," + e.Message);
            }
        }

        public void DeleteVehicleObuByVehicleID(long vehicleID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                IEnumerable<Vehicle_Obu> vehicleobus = dbContext.Vehicle_Obu.Where(Vehicle_Obu => Vehicle_Obu.vehicleid == vehicleID);
                foreach (Vehicle_Obu vehicleobuTemp in vehicleobus)
                {
                    dbContext.Vehicle_Obu.Remove(vehicleobuTemp);
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除车辆OBU异常," + e.Message);
            }
        }

        public void DeleteGeofenceVehicleByVehicleID(long vehicleID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                IEnumerable<Geofence_Vehicle> vehicleGeofences = dbContext.Geofence_Vehicle.Where(geo => geo.vehicleid == vehicleID);
                foreach (Geofence_Vehicle vehicleGeofence in vehicleGeofences)
                {
                    dbContext.Geofence_Vehicle.Remove(vehicleGeofence);
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除电子围栏异常," + e.Message);
            }
        }

        public void UpdateGroup(string companyID, VehicleGroup updateGroup)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<VehicleGroup> groups = from groupDB in dbContext.VehicleGroup
                                                   join tenantDB in dbContext.Tenant on groupDB.tenantid equals tenantDB.pkid
                                               where groupDB.pkid == updateGroup.pkid
                                                   select groupDB;
                //从数据库获取数据
                foreach (VehicleGroup groupTemp in groups)
                {
                    groupTemp.name = updateGroup.name;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新车组异常," + e.Message);
            }
        }

        public void UpdateGroupAddVehicle(VehicleGroup updateGroup, List<Vehicle> vehicles)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<VehicleGroup> groups = from groupDB in dbContext.VehicleGroup
                                               where groupDB.pkid == updateGroup.pkid
                                                   select groupDB;
                //从数据库获取数据
                //#587判断分组名是否存在 liangjiajie20140401
                if ( 0 != groups.Count())
                {

                    foreach (VehicleGroup groupTemp in groups)
                    {
                        foreach (Vehicle vehicleTemp in vehicles)
                        {
                            VehicleGroup_Vehicle vehiclegroup = new VehicleGroup_Vehicle();
                            vehiclegroup.groupid = groupTemp.pkid;
                            vehiclegroup.vehicleid = vehicleTemp.pkid;
                            dbContext.VehicleGroup_Vehicle.Add(vehiclegroup);
                        }
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new DBException("该分组不存在");
                }
            }
            catch (Exception e)
            {
                throw new DBException("添加车组车辆异常," + e.Message);
            }

        }

        public void UpdateGroupRemoveVehicle(VehicleGroup updateGroup, List<Vehicle> vehicles)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<VehicleGroup_Vehicle> vehiclegroups = dbContext.VehicleGroup_Vehicle.Where(vehiclegroup_vehicle => vehiclegroup_vehicle.groupid == updateGroup.pkid);
                foreach (VehicleGroup_Vehicle vehiclegroupTemp in vehiclegroups)
                {
                    var vehicle = vehicles.Find(Vehicle => Vehicle.pkid == vehiclegroupTemp.vehicleid);
                    if (null != vehicle)
                    {
                        dbContext.VehicleGroup_Vehicle.Remove(vehiclegroupTemp);
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除车组车辆异常," + e.Message);
            }
        }

        public void DeleteGroup(VehicleGroup deleteGroup)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<VehicleGroup_Vehicle> vehiclegroups = dbContext.VehicleGroup_Vehicle.Where(vehiclegroup_vehicle => vehiclegroup_vehicle.groupid == deleteGroup.pkid);
                foreach (VehicleGroup_Vehicle vehiclegroupTemp in vehiclegroups)
                {
                    dbContext.VehicleGroup_Vehicle.Remove(vehiclegroupTemp);
                }
                dbContext.SaveChanges();

                VehicleGroup groups = dbContext.VehicleGroup.First(vehiclegroup => vehiclegroup.pkid == deleteGroup.pkid);
                dbContext.VehicleGroup.Remove(groups);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除车组异常," + e.Message);
            }
        }

        //添加OBU
        public long AddOBU(Obu OBU)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                Obu result = dbContext.Obu.Add(OBU);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加OBU异常," + e.Message);
            }
        }

        //添加group
        public long AddGroup(string companyID, VehicleGroup group)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                VehicleGroup result = dbContext.VehicleGroup.Add(group);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加车组异常," + e.Message);
            }
        }

        public void UpdateVehicle(Vehicle updateVehicle, Logo updateLogo)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Vehicle> vehicles = from vehicleDB in dbContext.Vehicle
                                             where vehicleDB.pkid == updateVehicle.pkid
                                                select vehicleDB;
                LogoDBInterface logoDB = new LogoDBInterface();
                //从数据库获取数据
                foreach (Vehicle vehicleTemp in vehicles)
                {
                    vehicleTemp.name = updateVehicle.name;
                    //vehicleTemp.logoid = updateVehicle.logoid;
                    //vehicleTemp.vin = updateVehicle.vin;
                    if (null != vehicleTemp.logoid && null != updateLogo)
                    {
                        logoDB.UpdateLogo((long)vehicleTemp.logoid, updateLogo);
                    }
                    else if (null != updateLogo)
                    {
                        long logoID = logoDB.InsertLogoAndGetId(updateLogo);
                        vehicleTemp.logoid = logoID;
                    }
                    vehicleTemp.vin = updateVehicle.vin;
                    if (1 == updateVehicle.isMMYEditable)
                    {
                        vehicleTemp.info = updateVehicle.info;
                        vehicleTemp.mmyid = updateVehicle.mmyid;
                    }
					//#861 增加保存驾驶司机信息liangjiajie0319
                    vehicleTemp.drivername = updateVehicle.drivername;
                    vehicleTemp.licence = updateVehicle.licence;
                    vehicleTemp.telephone = updateVehicle.telephone;
                    vehicleTemp.lable = updateVehicle.lable;
                    if (updateVehicle.vin.Trim().Length == VINConstant.VINLength)
                    {
                        vehicleTemp.isVinEditable = VINConstant.VINNotEditable;
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新车辆异常," + e.Message);
            }
        }

        public void RemoveVehicle(Vehicle removeVehicle)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;
            try
            {
                Vehicle vehicles = dbContext.Vehicle.First(vehicle => vehicle.pkid == removeVehicle.pkid);
                dbContext.Vehicle.Remove(vehicles);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除车辆异常," + e.Message);
            }
        }

        public VehicleGroup GetVehicleGroupByGroupName(string groupName,string companyid)
        {
            VehicleGroup result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<VehicleGroup> groups = from groupDB in db.VehicleGroup
                                                   join tenantDB in db.Tenant on groupDB.tenantid equals tenantDB.pkid
                                                   where groupDB.name == groupName && tenantDB.companyid == companyid
                                                   select groupDB;
                foreach (VehicleGroup groupTemp in groups)
                {
                    result = groupTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过组名获取车组异常," + e.Message);
            }
        }

        //添加OBU
        public long AddVehicle(string companyID, Vehicle vehicle)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                Vehicle result = dbContext.Vehicle.Add(vehicle);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加车辆异常," + e.Message);
            }
        }

        public void DeleteVehicleAlertConfiguration(long vehicleID)
        {
            var dbContext = new FleetManageToolDBContext();
            try
            {
                IEnumerable<Vehicle_AlertConfiguration> vehicleAlertConfigs = from vehicleAlertConfigDB in dbContext.Vehicle_AlertConfiguration
                                                                              where vehicleAlertConfigDB.vehicleID == vehicleID
                                                                              select vehicleAlertConfigDB;
                foreach (Vehicle_AlertConfiguration vehicleAlertConfig in vehicleAlertConfigs)
                {
                    dbContext.Vehicle_AlertConfiguration.Remove(vehicleAlertConfig);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加Alert Configuration异常," + e.Message);
            }
        }
        /// <summary>
        /// fengpan 20140507 获取车的geofencealert。
        /// </summary>
        /// <param name="vehicleID"></param>
        public List<Vehicle_AlertConfiguration> GetVehicleGeoAlertConfiguration(long vehicleID)
        {
            List<Vehicle_AlertConfiguration> ret = new List<Vehicle_AlertConfiguration>();
            var dbContext = new FleetManageToolDBContext();
            try
            {
                IEnumerable<Vehicle_AlertConfiguration> vehicleAlertConfigs = from vehicleAlertConfigDB in dbContext.Vehicle_AlertConfiguration
                                                                              where vehicleAlertConfigDB.vehicleID == vehicleID && vehicleAlertConfigDB.category.Equals("GeoFence")
                                                                              select vehicleAlertConfigDB;
                foreach (Vehicle_AlertConfiguration vehicleAlertConfig in vehicleAlertConfigs)
                {
                    ret.Add(vehicleAlertConfig);
                }
                return ret;
            }
            catch (Exception e)
            {
                throw new DBException("获取geoAlert Configuration异常," + e.Message);
            }
        }


        public void DeleteVehicleAlertConfiguration(long vehicleID, List<Vehicle_AlertConfiguration> alertconfigurations)
        {
            var dbContext = new FleetManageToolDBContext();
            try
            {
                IEnumerable<Vehicle_AlertConfiguration> vehicleAlertConfigs = from vehicleAlertConfigDB in dbContext.Vehicle_AlertConfiguration
                                                                              where vehicleAlertConfigDB.vehicleID == vehicleID
                                                                              select vehicleAlertConfigDB;
                foreach (Vehicle_AlertConfiguration vehicleAlertConfig in vehicleAlertConfigs)
                {
                    if (null != alertconfigurations.Find(t => t.alertConfigurationGuID == vehicleAlertConfig.alertConfigurationGuID))
                    {
                        dbContext.Vehicle_AlertConfiguration.Remove(vehicleAlertConfig); 
                    }
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除Alert Configuration异常," + e.Message);
            }
        }
        public void AddVehicleAlertConfiguration(List<Vehicle_AlertConfiguration> vehicleAlertConfigurations)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                foreach (Vehicle_AlertConfiguration vehicleAlertConfiguration in vehicleAlertConfigurations )
                {
                    dbContext.Vehicle_AlertConfiguration.Add(vehicleAlertConfiguration);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加Alert Configuration异常," + e.Message);
            }
        }

        public long AddTrip(long vehicleID, Trip trip)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                Trip result = dbContext.Trip.Add(trip);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加Trip异常," + e.Message);
            }
        }

        public void AddTrip(long vehicleID, List<Trip> trips)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                for (int i=trips.Count-1; i>=0; --i)//倒序插入
                {
                //foreach (Trip trip in trips)
                //{
                    dbContext.Trip.Add(trips[i]);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加Trips异常," + e.Message);
            }
        }

        //chenyanwgen 20140403
        public Trip GetLastTrip(long vehicleID)
        {
            try
            {
                Trip result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Trip> trips = (from tripDB in dbContext.Trip
                                           where tripDB.vehicleId == vehicleID
                                           orderby tripDB.startTime == null ? tripDB.endtime : tripDB.startTime descending
                                           select tripDB).Take(1);
                foreach (Trip tripTemp in trips)
                {
                    result = tripTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取最近时间的Trip失败," + e.Message);
            }
        }

        public long AddAlert(long vehicleID, Alert alert)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                Alert result = dbContext.Alert.Add(alert);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加Alert异常," + e.Message);
            }
        }

        public void AddAlert(long vehicleID, List<Alert> alerts)
        {
            //给数据库插入数据
            var dbContext = new FleetManageToolDBContext();
            try
            {
                for (int i = alerts.Count-1; i >= 0; --i)//倒序插入
                {
                //foreach (Alert alert in alerts)
                //{
                    dbContext.Alert.Add(alerts[i]);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加Alerts异常," + e.Message);
            }
        }

        public Alert GetLastAlert(long vehicleID)
        {
            try
            {
                Alert result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Alert> alerts = (from alertDB in dbContext.Alert
                                             where alertDB.vehicleId == vehicleID && alertDB.AlertType != "engineGUID"
                                             orderby alertDB.TriggeredDateTime descending
                                             select alertDB).Take(1);
                foreach (Alert alertTemp in alerts)
                {
                    result = alertTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取最近时间的Trip失败," + e.Message);
            }
        }
		
		//根据VehicleID取Alert
        public List<Alert> GetAlertByVehicleID(long VehicleID)
        {
            try
            {
                List<Alert> result = new List<Alert>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Alert> alerts = from alertDB in db.Alert
                                            where alertDB.vehicleId == VehicleID
                                            select alertDB;
                foreach (Alert alertTemp in alerts)
                {

                    result.Add(alertTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取Alert异常," + e.Message);
            }
        }
		

        //
        //根据VehicleI和日期取Alert
        public List<Alert> GetAlertByVehicleIDandTimeforReport(long VehicleID,DateTime startdate,DateTime enddate)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDandTimeforReport start");
                List<Alert> result = new List<Alert>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Alert> alerts = from alertDB in db.Alert
                                            where alertDB.vehicleId == VehicleID
                                            && alertDB.TriggeredDateTime != null && alertDB.TriggeredDateTime >=startdate && alertDB.TriggeredDateTime <= enddate
                                            && alertDB.AlertType != "Geo1"
                                            && alertDB.AlertType != "Geo2"
                                            && alertDB.AlertType != "Geo3"
                                            && alertDB.AlertType != "Geo4"
                                            && alertDB.AlertType != "Geo5"
                                            && alertDB.AlertType != "Geo6"
                                            && alertDB.AlertType != "Battery Level"
                                            orderby alertDB.TriggeredDateTime descending
                                            select alertDB;
                foreach (Alert alertTemp in alerts)
                {

                    result.Add(alertTemp);
                }
                DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDandTimeforReport end");
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new DBException("获取Alert异常," + e.Message);
            }
        }
        //根据VehicleI和日期取Alert
        //public List<Alert> GetAlertByVehicleIDandTimeforReportOneDay(long VehicleID, DateTime date)
        //{
        //    try
        //    {
        //        DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDandTimeforReportOneDay start");
        //        List<Alert> result = new List<Alert>();
        //        var db = new FleetManageToolDBContext();
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        IEnumerable<Alert> alerts = from alertDB in db.Alert
        //                                    where alertDB.vehicleId == VehicleID
        //                                    && alertDB.TriggeredDateTime != null && alertDB.TriggeredDateTime.Value.Year == date.Year && alertDB.TriggeredDateTime.Value.Month == date.Month && alertDB.TriggeredDateTime.Value.Day == date.Day
        //                                    && alertDB.AlertType != "Geo1"
        //                                    && alertDB.AlertType != "Geo2"
        //                                    && alertDB.AlertType != "Geo3"
        //                                    && alertDB.AlertType != "Geo4"
        //                                    && alertDB.AlertType != "Geo5"
        //                                    && alertDB.AlertType != "Geo6"
        //                                    && alertDB.AlertType != "Battery Level"
        //                                    orderby alertDB.TriggeredDateTime descending
        //                                    select alertDB;
        //        foreach (Alert alertTemp in alerts)
        //        {

        //            result.Add(alertTemp);
        //        }
        //        DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDandTimeforReportOneDay end");
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        DebugLog.Debug(e.StackTrace);
        //        throw new DBException("获取Alert异常," + e.Message);
        //    }
        //}





		//通过车组获取该该车组的车辆
        public List<Vehicle> GetGroupVehicles()
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetGroupVehicles start");
                List<Vehicle> vehicles = new List<Vehicle>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Vehicle> vehiclesTemp = from vehicleDB in db.Vehicle
                                                    join vehiclegroupDB in db.VehicleGroup_Vehicle on vehicleDB.pkid equals vehiclegroupDB.vehicleid
                                                    select vehicleDB;
                foreach (Vehicle vehicle in vehiclesTemp)
                {
                    vehicles.Add(vehicle);
                }
                DebugLog.Debug("VehicleDBInterface GetGroupVehicles end");
                return vehicles;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new DBException("获取车组车辆异常," + e.Message);
            }
        }
        //根据车辆ID取trip
        public List<Trip> GetTripByVehicleID(long vehicleID)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetTripByVehicleID start");
                List<Trip> result = new List<Trip>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Trip> trips = from tripDB in db.Trip
                                          where tripDB.vehicleId == vehicleID
                                          select tripDB;
                foreach (Trip trip in trips)
                {
                    result.Add(trip);
                }
                DebugLog.Debug("VehicleDBInterface GetTripByVehicleID end");
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new DBException("获取Trip异常," + e.Message);
            }
        }

        //根据OBU取车辆
        //public List<Vehicle> GetTenantVehiclesObuByCompannyID(string companyID)
        //{
        //    try
        //    {
        //        List<Vehicle> result = new List<Vehicle>();
        //        var db = new FleetManageToolDBContext();
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        var vehicles = from vehicleDB in db.Vehicle
        //                       join tenantDB in db.Tenant on vehicleDB.tenantid equals tenantDB.pkid
        //                       where tenantDB.companyid == companyID
        //                       select vehicleDB;
        //        foreach (Vehicle vehicle in vehicles)
        //        {
        //            var vehicleObu = from vehicleObuDB in db.Vehicle_Obu
        //                           where vehicleObuDB.vehicleid == vehicle.pkid
        //                           select vehicleObuDB;
        //            if (null != vehicleObu && 0 != vehicleObu.Count())
        //            {
        //                result.Add(vehicle);
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new DBException("通过Obu获取Vehilce异常," + e.Message);
        //    }
        //}

        //mabiao 获取Geofence Alert
        public List<Alert> GetAlertByVehicleIDAndTime(long vehicleID, DateTime startTime, DateTime endTime)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDAndTime start");
                List<Alert> result = new List<Alert>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Alert> alerts = from alertDB in dbContext.Alert
                                            where alertDB.vehicleId == vehicleID
                                            && alertDB.TriggeredDateTime <= endTime
                                            && alertDB.TriggeredDateTime >= startTime
                                            orderby alertDB.TriggeredDateTime descending
                                            select alertDB;
                foreach (Alert alertTemp in alerts)
                {
                    result.Add(alertTemp);
                }
                DebugLog.Debug("VehicleDBInterface GetAlertByVehicleIDAndTime end");
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.StackTrace);
            }
        }

        //mabiao 获取Geofence Alert
        public List<Alert> GetEngineAlertByVehicleIDAndTime(long vehicleID, DateTime startTime, DateTime endTime)
        {
            List<Alert> result = new List<Alert>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            IEnumerable<Alert> alerts = from alertDB in dbContext.Alert
                                        where alertDB.vehicleId == vehicleID && alertDB.AlertType.Trim() == TripConstant.Engine
                                        && !(alertDB.TriggeredDateTime < startTime && alertDB.EngineEndTime < startTime)
                                        && !(alertDB.TriggeredDateTime > endTime && alertDB.EngineEndTime > endTime)
                                        orderby alertDB.TriggeredDateTime descending
                                        select alertDB;
            foreach (Alert alertTemp in alerts)
            {
                result.Add(alertTemp);
            }
            return result;
        }
		
        //通过pkid获取一条trip
        public Trip GetTripByGuid(string guid)
        {
            Trip result = new Trip ();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            IEnumerable<Trip> trip = from tripDB in dbContext.Trip
                                     where tripDB.guid == guid
                                     select tripDB;
            foreach (Trip tripTemp in trip)
            {
                result = tripTemp;
                break;
            }
            return result;
        }

        //time: 格式是什么？UTC还是随意？
        public List<Trip> GetTripsByVehicleIDAndTime(long vehicleID, DateTime tripEndTime, int count)
        {
            List<Trip> result = new List<Trip>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            IEnumerable<Trip> trips = (from tripDB in dbContext.Trip
                                       where tripDB.vehicleId == vehicleID
                                       && (tripDB.endtime == null ? tripDB.startTime < tripEndTime : tripDB.endtime < tripEndTime)
                                       orderby tripDB.endtime == null ? tripDB.startTime : tripDB.endtime descending
                                       select tripDB).Take(count);
            foreach (Trip tripTemp in trips)
            {
                result.Add(tripTemp);
            }
            return result;
        }

        //public List<Trip> GetTripsByVehicleIDAndTimeForReport(long vehicleID, DateTime date)
        //{
        //    try
        //    {
        //        DebugLog.Debug("VehicleDBInterface GetTripsByVehicleIDAndTimeForReport start");
        //        List<Trip> result = new List<Trip>();
        //        var dbContext = new FleetManageToolDBContext();
        //        dbContext.Configuration.ProxyCreationEnabled = false;
        //        dbContext.Configuration.LazyLoadingEnabled = false;

        //        IEnumerable<Trip> trips = from tripDB in dbContext.Trip
        //                                  where tripDB.vehicleId == vehicleID
        //                                  && tripDB.startTime != null && tripDB.endtime != null && tripDB.startTime.Value.Year == date.Year && tripDB.startTime.Value.Month == date.Month && tripDB.startTime.Value.Day == date.Day
        //                                  orderby tripDB.startTime descending
        //                                  select tripDB;
        //        foreach (Trip tripTemp in trips)
        //        {
        //            result.Add(tripTemp);
        //        }
        //        DebugLog.Debug("VehicleDBInterface GetTripsByVehicleIDAndTimeForReport end");
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        DebugLog.Debug(e.StackTrace);
        //        throw new Exception(e.StackTrace);
        //    }
        //}

        //根据开始时间和结束时间取trip
        public List<Trip> GetTripsByVehicleIDAnd2TimeForReport(long vehicleID,DateTime tripStartTime, DateTime tripEndTime)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetTripsByVehicleIDAnd2TimeForReport start");
                List<Trip> result = new List<Trip>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Trip> trips = from tripDB in dbContext.Trip
                                          where tripDB.vehicleId == vehicleID
                                          && tripDB.startTime != null && tripDB.startTime >= tripStartTime
                                          && tripDB.endtime != null && tripDB.endtime <= tripEndTime
					                      && tripDB.startTime < tripDB.endtime 		
                                          orderby tripDB.startTime descending
                                          select tripDB;
                result = trips.ToList();
                //foreach (Trip tripTemp in trips)
                //{
                //    result.Add(tripTemp);
                //}
                DebugLog.Debug("VehicleDBInterface GetTripsByVehicleIDAnd2TimeForReport end");
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.StackTrace);
            }
        }

        //mabiao 20140404
        //WriteLocationToDB
        //flag: true 离开地点 false到达地点
        public void WriteLocationToDB(string tripGUID, Boolean flag, string address)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface WriteLocationToDB start");
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;


                IEnumerable<Trip> trips = from tripDB in dbContext.Trip
                                          where tripDB.guid == tripGUID
                                          select tripDB;
                foreach (Trip tripTemp in trips)
                {
                    if (flag.Equals(true))
                    {
                        tripTemp.startlocation = address;
                    }
                    else if (flag.Equals(false))
                    {
                        tripTemp.endlocation = address;
                    }
                }
                dbContext.SaveChanges();
                DebugLog.Debug("VehicleDBInterface WriteLocationToDB end");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new Exception(e.Message);
            }
        }

        //gaoqingbo & caoyandong
		//20140419
        public void WriteLocationToDBReport(string[] strStart, string[] endStart)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface WriteLocationToDBReport start");
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                for (var i = 0; i < strStart.Length; ++i)
                {
                    string[] startStr = (strStart[i]).Split('|');
                    String start_id = startStr[0];
                    String start_locatiion = startStr[1];
                    IEnumerable<Trip> trips = from tripDB in dbContext.Trip
                                              where tripDB.guid == start_id
                                              select tripDB;
                    foreach (Trip tripTemp in trips)
                    {
                        tripTemp.startlocation = start_locatiion;
                    }
                }

                for (var i = 0; i < endStart.Length; ++i)
                {
                    string[] endStr = (endStart[i]).Split('|');
                    String end_id = endStr[0];
                    String end_locatiion = endStr[1];
                    IEnumerable<Trip> trips = from tripDB in dbContext.Trip
                                              where tripDB.guid == end_id
                                              select tripDB;
                    foreach (Trip tripTemp in trips)
                    {
                        tripTemp.endlocation = end_locatiion;
                    }
                }
                DebugLog.Debug("VehicleDBInterface WriteLocationToDBReport end");
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
		
        public int getAlertCount(long vehicleID, DateTime startDate, DateTime endDate)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                int alertSize = (from alertDB in dbContext.Alert
                                 where alertDB.vehicleId == vehicleID
                                 && alertDB.TriggeredDateTime <= endDate && alertDB.TriggeredDateTime >= startDate
                                 && (alertDB.AlertType == AlertConfigurationConstant.Speed ||
                                        alertDB.AlertType == AlertConfigurationConstant.Rpm ||
                                        alertDB.AlertType == AlertConfigurationConstant.Motion ||
                                        alertDB.AlertType == AlertConfigurationConstant.Speed_cn ||
                                        alertDB.AlertType == AlertConfigurationConstant.Motion_cn ||
                                        alertDB.AlertType == AlertConfigurationConstant.Rpm_cn ||
                                        alertDB.AlertType == AlertConfigurationConstant.EngineRpm)
                                 select alertDB).Count();
                return alertSize;
            }
            catch (Exception exception)
            {
                throw new DBException("获取Alert条数失败," + exception.Message);
            }
        }

        public List<Alert> getAlertPage(long vehicleID, DateTime startDate, DateTime endDate, int pageIndex, int pageSize,int cacheCount = 0)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                int skipCount = (pageIndex - 1) >= 0 ? (pageIndex - 1) * pageSize : 0;
                List<Alert> result = new List<Alert>();
                IEnumerable<Alert> alerts = (from alertDB in dbContext.Alert
                                             where alertDB.vehicleId == vehicleID
                                             && alertDB.TriggeredDateTime <= endDate && alertDB.TriggeredDateTime >= startDate
                                             && (alertDB.AlertType == AlertConfigurationConstant.Speed ||
                                                    alertDB.AlertType == AlertConfigurationConstant.Rpm ||
                                                    alertDB.AlertType == AlertConfigurationConstant.Motion ||
                                                    alertDB.AlertType == AlertConfigurationConstant.Speed_cn ||
                                                    alertDB.AlertType == AlertConfigurationConstant.Motion_cn ||
                                                    alertDB.AlertType == AlertConfigurationConstant.Rpm_cn ||
                                                    alertDB.AlertType == AlertConfigurationConstant.EngineRpm)
                                             orderby alertDB.TriggeredDateTime descending
                                             select alertDB).Skip(skipCount-cacheCount).Take(pageSize);
                foreach (Alert alert in alerts)
                {
                    result.Add(alert);
                }
                return result;
            }
            catch (Exception exception)
            {
                throw new DBException("获取Alert数据失败," + exception.Message);
            }
        }
        //chenyangwen 20140505 #1353
        public List<Obu> GetObuByTenantIDNoCar(String companyID)
        {
            List<Obu> result = new List<Obu>();
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                var obus = from obuDB in dbContext.Obu
                           join tenantDB in dbContext.Tenant on obuDB.tenantid equals tenantDB.pkid
                           where tenantDB.companyid == companyID
                           select obuDB;
                foreach(Obu obu in obus){
                    var obuvehicle = from obuvehicleDB in dbContext.Vehicle_Obu
                                     where obuvehicleDB.obuid == obu.pkid
                                     select obuvehicleDB;
                    if (null == obuvehicle || 0 == obuvehicle.Count())
                    {
                        result.Add(obu);
                    }
                }
                return result;
            }
            catch (Exception exception)
            {
                throw new DBException("VehicleDBInterface GetObuByTenantIDNoCar Have a Exception " + exception.Message);
            }
            
        }

        public int getGeoAlertCount(long vehicleID, DateTime startDate, DateTime endDate)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                int alertsSize = (from alertDB in dbContext.Alert
                                  where alertDB.vehicleId == vehicleID
                                  && alertDB.TriggeredDateTime <= endDate && alertDB.TriggeredDateTime >= startDate
                                  && (alertDB.AlertType == AlertConfigurationConstant.Geo1 ||
                                  alertDB.AlertType == AlertConfigurationConstant.Geo2 || 
                                  alertDB.AlertType == AlertConfigurationConstant.Geo3 || 
                                  alertDB.AlertType == AlertConfigurationConstant.Geo4 ||
                                  alertDB.AlertType == AlertConfigurationConstant.Geo5 ||
                                  alertDB.AlertType == AlertConfigurationConstant.Geo6 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_1 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_2 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_3 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_4 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_5 ||
                                  alertDB.AlertType == AlertConfigurationConstant.GEO_6)
                                  select alertDB).Count();
                return alertsSize;
            }
            catch (Exception exception)
            {
                throw new DBException("获取GeoAlert条数失败," + exception.Message);
            }
        }

        public List<Alert> getGeoAlertPage(long vehicleID, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, int cacheCount = 0)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                int skipCount = (pageIndex - 1) >= 0 ? (pageIndex - 1) * pageSize : 0;
                List<Alert> result = new List<Alert>();
                IEnumerable<Alert> alerts = (from alertDB in dbContext.Alert
                                             where alertDB.vehicleId == vehicleID
                                             && alertDB.TriggeredDateTime <= endDate && alertDB.TriggeredDateTime >= startDate
                                             && (alertDB.AlertType == AlertConfigurationConstant.Geo1 ||
                                             alertDB.AlertType == AlertConfigurationConstant.Geo2 ||
                                             alertDB.AlertType == AlertConfigurationConstant.Geo3 ||
                                             alertDB.AlertType == AlertConfigurationConstant.Geo4 ||
                                             alertDB.AlertType == AlertConfigurationConstant.Geo5 ||
                                             alertDB.AlertType == AlertConfigurationConstant.Geo6 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_1 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_2 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_3 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_4 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_5 ||
                                             alertDB.AlertType == AlertConfigurationConstant.GEO_6)
                                             orderby alertDB.TriggeredDateTime descending
                                             select alertDB).Skip(skipCount - cacheCount).Take(pageSize);
                foreach (Alert alert in alerts)
                {
                    result.Add(alert);
                }
                return result;
            }
            catch (Exception exception)
            {
                throw new DBException("获取GeoAlert数据失败," + exception.Message);
            }
        }

        public long GetMMYIdByMMY(string model, string make, string year)
        {
            long result = -1;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<MMY> mmys = from mmyDB in db.MMY
                                        where (mmyDB.mmyMake.Equals(make) && mmyDB.mmyModel.Equals(model) && mmyDB.mmyYear.Equals(year) && mmyDB.language.Equals("ZH-CN"))
                                        || (mmyDB.mmyMake.Equals(make) && mmyDB.mmyModel.Equals(model) && mmyDB.mmyYear.Equals(year) && mmyDB.language.Equals("ZH-EN"))
                                        select mmyDB;
                foreach (MMY mmy in mmys)
                {
                    result = mmy.mmyIndex;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过MMY获取MMYID异常," + e.Message);
            }
        }

        //通过mmyId取得mmy对象
        public MMY GetMMYById(long mmyid)
        {
            MMY result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<MMY> mmys = from mmyDB in db.MMY
                                       where mmyDB.mmyIndex == mmyid && mmyDB.language.Equals("ZH-CN")
                                       select mmyDB;
                foreach (MMY mmy in mmys)
                {
                    result = mmy;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过MMYID获取MMY异常," + e.Message);
            }
        }

        // 取得所有MMY中makeList（中文）
        public List<String> GetMakes_Chinese()
        {
            List<String> result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<String> cmakes = from mmyDB in db.MMY
                                             where mmyDB.language.Equals("ZH-CN")
                                             orderby mmyDB.mmyMake
                                        select mmyDB.mmyMake.Trim();

                cmakes = cmakes.Distinct();
                if (cmakes.Count() != 0)
                {
                    result = new List<string>();
                }

                foreach (String cmake in cmakes)
                {
                    result.Add(cmake);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取所有make异常," + e.Message);
            }
        }

        // 取得所有对应make的modelList（中文）
        public List<String> GetModelsByMake(String make)
        {
            List<String> result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<String> cmodels = from mmyDB in db.MMY
                                              where mmyDB.mmyMake.Trim() == make.Trim() && mmyDB.language.Equals("ZH-CN")
                                              orderby mmyDB.mmyModel
                                              select mmyDB.mmyModel.Trim();
                cmodels = cmodels.Distinct();

                if (cmodels.Count() != 0)
                {
                    result = new List<string>();
                }
                
                foreach (String cmodel in cmodels)
                {
                    result.Add(cmodel);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过make获取所有model异常," + e.Message);
            }
        }

        //通过model取得mmy对象（显示对应model下的所有year）（中文）
        public List<MMY> GetYearsByModel(String make,String model)
        {
            List<MMY> result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<MMY> mmys = from mmyDB in db.MMY
                                        where mmyDB.mmyModel.Trim() == model.Trim() && mmyDB.mmyMake.Trim() == make.Trim() && mmyDB.language.Equals("ZH-CN")
                                        orderby mmyDB.mmyYear
                                        select mmyDB;
                if (mmys.Count() != 0)
                {
                    result = new List<MMY>();
                }
                foreach (MMY mmy in mmys)
                {
                    result.Add(mmy);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过mack,model获取对应所有MMY异常," + e.Message);
            }
        }



        //根据companyid获取历史车辆
        public List<Vehicle> GetHistoryVehiclesByCompannyID(string companyID)
        {
            try
            {
                DebugLog.Debug("VehicleDBInterface GetHistoryVehiclesByCompannyID start");
                List<Vehicle> resultall = new List<Vehicle>();
                List<Vehicle> resultobu = new List<Vehicle>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var vehicles = from vehicleDB in db.Vehicle
                               join tenantDB in db.Tenant on vehicleDB.tenantid equals tenantDB.pkid
                               where tenantDB.companyid == companyID
                               select vehicleDB;
                foreach (Vehicle vehicle in vehicles)
                {
                    resultall.Add(vehicle);
                }
                resultobu = GetTenantVehiclesByCompannyID(companyID);
                foreach (Vehicle vehicle in resultobu)
                {
                    int index = resultall.FindIndex(v => v.id == vehicle.id);
                    resultall.RemoveAt(index);
                }
                DebugLog.Debug("VehicleDBInterface GetHistoryVehiclesByCompannyID end");
                return resultall;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.StackTrace);
                throw new DBException("根据companyid获取历史车辆," + e.Message);
            }
        }

        public Alert GetLastAlertByGUID(string vehicleGUID)
        {
            try
            {
                Alert result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Alert> alerts = (from alertDB in dbContext.Alert
                                             join vehicleDB in dbContext.Vehicle on vehicleGUID equals vehicleDB.id
                                             where alertDB.vehicleId == vehicleDB.pkid && alertDB.AlertType != "engineGUID"
                                             orderby alertDB.TriggeredDateTime descending
                                             select alertDB).Take(1);
                foreach (Alert alertTemp in alerts)
                {
                    result = alertTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取最近时间的Trip失败," + e.Message);
            }
        }

        public Trip GetLastTripByGUID(string vehicleGUID)
        {
            try
            {
                Trip result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Trip> trips = (from tripDB in dbContext.Trip
                                           join vehicleDB in dbContext.Vehicle on vehicleGUID equals vehicleDB.id
                                           where tripDB.vehicleId == vehicleDB.pkid
                                           orderby tripDB.startTime == null ? tripDB.endtime : tripDB.startTime descending
                                           select tripDB).Take(1);
                foreach (Trip tripTemp in trips)
                {
                    result = tripTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取最近时间的Trip失败," + e.Message);
            }
        }
		
		//Add by LiYing Start
		//本地OBU列表中检查待注册OBU是否存在,以及和Reg-KEY是否匹配
        public int CheckOBUAndRegKeyMatch(string esn, string regkey) 
        {
            int reuslt = 0;
            try
            {
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                int count = (from Obu_CheckDB in db.Obu_Check
                            where Obu_CheckDB.labelid == esn 
                            && Obu_CheckDB.regkey == regkey
                            select Obu_CheckDB).Count();

                reuslt = count; 
            }
            catch (Exception e)
            {
                throw new DBException("检查OBU是否存在以及是否和Reg-Key匹配" + e.Message);             
            }


            return reuslt;
        }

        //更新OBU status
        public void updateOBUStatus(string esn, string regkey)
        {
            try
            {
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Obu_Check> obu_checks = from Obu_CheckDB in db.Obu_Check
                                                   where Obu_CheckDB.labelid == esn
                                                   && Obu_CheckDB.regkey == regkey
                                                   select Obu_CheckDB;
                if (null != obu_checks && obu_checks.Count() > 0)
                {
                    foreach (Obu_Check obuTemp in obu_checks)
                    {
                        obuTemp.status = 1;
                        
                        break;
                    }
                    db.SaveChanges();
                }
                
                
            }
            catch (Exception e)
            {
                throw new DBException("更新OBU status" + e.Message);
            }
        }
		//Add by LiYing End

        //zhangbo add for 查询一个company下的所有历史车辆的id
        //根据companyid获取历史车辆
        public List<long> GetHistoryVehicleidsByCompannyID(string companyID)
        {
            try
            {
                List<Vehicle> resultall = new List<Vehicle>();
                List<Vehicle> resultobu = new List<Vehicle>();
                var db = new FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var vehicles = from vehicleDB in db.Vehicle
                               join tenantDB in db.Tenant on vehicleDB.tenantid equals tenantDB.pkid
                               where tenantDB.companyid == companyID
                               select vehicleDB;
                foreach (Vehicle vehicle in vehicles)
                {
                    resultall.Add(vehicle);
                }
                resultobu = GetTenantVehiclesByCompannyID(companyID);
                foreach (Vehicle vehicle in resultobu)
                {
                    int index = resultall.FindIndex(v => v.id == vehicle.id);
                    resultall.RemoveAt(index);
                }

                List<long>result = new List<long>();
                foreach (Vehicle vehicle in resultall)
                {
                    result.Add(vehicle.pkid);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("根据companyid获取历史车辆," + e.Message);
            }
        }

        public bool isExistenceTrip(string guid, long vehiclePKID)
        {
            try
            {
                bool result = false;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                int count = (from tripDB in dbContext.Trip
                             where tripDB.guid == guid 
                             && tripDB.vehicleId == vehiclePKID
                             select tripDB).Count();
                if (count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("根据guid判断是否已经存在trip," + e.Message);
            }
        }

        public bool isExistenceAlert(string guid, long vehiclePKID)
        {
            try
            {
                bool result = false;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                int count = (from alertDB in dbContext.Alert
                             where alertDB.guid == guid
                             && alertDB.vehicleId == vehiclePKID
                             select alertDB).Count();
                if (count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("根据guid判断是否已经存在alert," + e.Message);
            }
        }
    }
}