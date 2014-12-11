using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.Constant;

namespace FleetManageToolWebRole.DB_interface
{
    public class GeofenceDBInterface
    {
        //chenyangwen 2014/02/12
        //获取租户的所有电子围栏
        public List<Geofence> GetGeofences (string companyID)
        {
            try
            {
                List<Geofence> result = new List<Geofence>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence> geofences = from geofenceDB in dbContext.Geofence
                                                  join tenantDB in dbContext.Tenant on geofenceDB.tenantid equals tenantDB.pkid
                                                  where tenantDB.companyid == companyID
                                                  select geofenceDB;
                foreach (Geofence geofenceTemp in geofences)
                {
                    List<Geofence_Vehicle> geofencevehicles = this.GetGeofenceVehicles(geofenceTemp.pkid);
                    geofenceTemp.Geofence_Vehicle = geofencevehicles;
                    result.Add(geofenceTemp);
                }
                return result;
            }catch(Exception e){
                throw new DBException("获取租户的所有电子围栏异常," + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //通过ID获取电子围栏
        public Geofence GetGeofenceByGeofenceID(long geofenceID)
        {
            try
            {
                Geofence result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence> geofences = from geofenceDB in dbContext.Geofence
                                                  join tenantDB in dbContext.Tenant on geofenceDB.tenantid equals tenantDB.pkid
                                                  where geofenceDB.pkid == geofenceID
                                                  select geofenceDB;
                foreach (Geofence geofenceTemp in geofences)
                {
                    result = geofenceTemp;
                    List<Geofence_Vehicle> geofencevehicles = this.GetGeofenceVehicles(geofenceTemp.pkid);
                    result.Geofence_Vehicle = geofencevehicles;
                }
                return result;
            }catch(Exception e){
                throw new DBException("通过ID获取电子围栏异常," + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //获取电子围栏的车辆
        public List<Geofence_Vehicle> GetGeofenceVehicles(long geofenceid)
        {
            try
            {
                List<Geofence_Vehicle> result = new List<Geofence_Vehicle>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence_Vehicle> geofencevehicles = from geofencevehicleDB in dbContext.Geofence_Vehicle
                                                                 where geofencevehicleDB.geofenceid == geofenceid
                                                                 select geofencevehicleDB;
                foreach (Geofence_Vehicle geofencevehicleTemp in geofencevehicles)
                {
                    result.Add(geofencevehicleTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取电子围栏的车辆异常:" + e.Message);
            }
        }

        //获取电子围栏的车辆id
        public List<long> GetGeofenceVehicleids(long geofenceid)
        {
            try
            {
                List<long> result = new List<long>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence_Vehicle> geofencevehicles = from geofencevehicleDB in dbContext.Geofence_Vehicle
                                                                 where geofencevehicleDB.geofenceid == geofenceid
                                                                 select geofencevehicleDB;
                foreach (Geofence_Vehicle geofencevehicleTemp in geofencevehicles)
                {
                    result.Add(geofencevehicleTemp.vehicleid);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取电子围栏的车辆异常:" + e.Message);
            }
        }

        //通过车辆ID获取电子围栏
        public List<Geofence_Vehicle> GetGeofenceVehiclesByVehicleID(long vehicleID)
        {
            try
            {
                List<Geofence_Vehicle> result = new List<Geofence_Vehicle>();
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence_Vehicle> geofencevehicles = from geofencevehicleDB in dbContext.Geofence_Vehicle
                                                                 join geofenceDB in dbContext.Geofence on geofencevehicleDB.geofenceid equals geofenceDB.pkid
                                                                 where geofencevehicleDB.vehicleid == vehicleID && geofenceDB.status == GeofenceConstant.Active
                                                                 select geofencevehicleDB;
                foreach (Geofence_Vehicle geofencevehicleTemp in geofencevehicles)
                {
                    result.Add(geofencevehicleTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过车辆ID获取电子围栏获取:" + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //删除电子围栏的车辆
        public void DeleteGeofenceVehicle(long geofenceID, long vehicleID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
            
                Geofence_Vehicle geofencevehicleTemp = dbContext.Geofence_Vehicle.First(Geofence_Vehicle => Geofence_Vehicle.geofenceid == geofenceID && Geofence_Vehicle.vehicleid == vehicleID);
                dbContext.Geofence_Vehicle.Remove(geofencevehicleTemp);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除电子围栏车辆异常:" + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //添加电子围栏
        public void DeleteGeofence(long geofenceID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
            
                var geofenceVehicles = dbContext.Geofence_Vehicle.Where(Geofence_Vehicle => Geofence_Vehicle.geofenceid == geofenceID);
                foreach (Geofence_Vehicle geoifenceTemp in geofenceVehicles)
                {
                    dbContext.Geofence_Vehicle.Remove(geoifenceTemp);
                }
                dbContext.SaveChanges();

                Geofence geofenceTemp = dbContext.Geofence.First(Geofence => Geofence.pkid == geofenceID);
                dbContext.Geofence.Remove(geofenceTemp);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除电子围栏异常:" + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //更新电子围栏
        public void UpdateGeofence(Geofence updateGeofence)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Geofence> geofences = from geofenceDB in dbContext.Geofence
                                                  join tenantDB in dbContext.Tenant on geofenceDB.tenantid equals tenantDB.pkid
                                              where geofenceDB.pkid == updateGeofence.pkid
                                                  select geofenceDB;
                //从数据库获取数据
                foreach (Geofence geofenceTemp in geofences)
                {
                    geofenceTemp.status = updateGeofence.status;
                    geofenceTemp.name = updateGeofence.name;
                    geofenceTemp.location = updateGeofence.location;
                    geofenceTemp.Baidulat = updateGeofence.Baidulat;
                    geofenceTemp.Baidulng = updateGeofence.Baidulng;
                    geofenceTemp.radiu = updateGeofence.radiu;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新电子围栏异常:" + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //添加电子围栏
        public long AddGeofence(string companyID, Geofence addGeofence)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                TenantDBInterface tenantInterface = new TenantDBInterface();
                addGeofence.tenantid = tenantInterface.GetTenantIDByCompanyID(companyID);
                Geofence result = dbContext.Geofence.Add(addGeofence);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("删除电子围栏异常:" + e.Message);
            }
        }

        //chenyangwen 2014/02/12
        //添加或更新电子围栏车辆
        public void AddOrUpdateGeofenceVehicle(long geofenceID, List<long> vehicelIDs)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                IEnumerable<Geofence_Vehicle> geofenceVehicles = from geofenceVehicleDB in dbContext.Geofence_Vehicle
                                                                 join geofenceDB in dbContext.Geofence on geofenceVehicleDB.geofenceid equals geofenceDB.pkid
                                                                 where geofenceDB.pkid == geofenceID
                                                                 select geofenceVehicleDB;
                foreach (Geofence_Vehicle geofenceVehicleTemp in geofenceVehicles)
                {
                    DeleteGeofenceVehicle(geofenceID, geofenceVehicleTemp.vehicleid);
                }

                //从数据库获取数据
                foreach (long vehicleID in vehicelIDs)
                {
                    Geofence_Vehicle geofencevehicle = new Geofence_Vehicle();
                    geofencevehicle.vehicleid = vehicleID;
                    geofencevehicle.geofenceid = geofenceID;
                    dbContext.Geofence_Vehicle.Add(geofencevehicle);
                }
                dbContext.SaveChanges();
            }catch(Exception e){
                throw new DBException("更新电子围栏车辆异常:" + e.Message);
            }
        }

        //单个添加电子围栏车辆
        public void AddGeofenceVehicle(Geofence_Vehicle geofenceVehicle)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Geofence_Vehicle.Add(geofenceVehicle);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加电子围栏车辆异常:" + e.Message);
            }
        }

        //一次性,添加多个电子围栏车辆
        public void AddGeofenceVehicles(List<Geofence_Vehicle> geofenceVehicles)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                foreach (Geofence_Vehicle geofenceTemp in geofenceVehicles)
                {
                    dbContext.Geofence_Vehicle.Add(geofenceTemp);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加多个电子围栏车辆异常:" + e.Message);
            }
        }

        //mabiao 2014/3/21
        //通过geofenceID和vehicleID获取GUID
        public string GetGUIDByGeofenceIDAndVehicleID(long geofenceID, long vehicleID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                Geofence_Vehicle geofencevehicle = dbContext.Geofence_Vehicle.First(Geofence_Vehicle => Geofence_Vehicle.geofenceid == geofenceID && Geofence_Vehicle.vehicleid == vehicleID);
                return geofencevehicle.ihpleDgeofenceid;
            }
            catch (Exception)
            {
                throw new DBException("查询电子围栏车辆异常");
            }
        }
        /// <summary>
        /// 通过围栏名称获取围栏位置信息 fengpan
        /// </summary>
        /// <param name="geoName"></param>
        /// <returns></returns>
        public string GetGeofenceLocationInfoByGeofenceName(string geoName)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                Geofence geofence = dbContext.Geofence.First(geo => geo.name == geoName);
                return geofence.location;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }
}
