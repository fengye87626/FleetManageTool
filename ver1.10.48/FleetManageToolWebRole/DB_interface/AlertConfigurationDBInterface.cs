using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.DB_interface
{
    public class AlertConfigurationDBInterface
    {
        //chenyangwen
        public List<AlertConfiguration> GetTenantAlertConfiguration(string companyID)
        {
            List<AlertConfiguration> results = new List<AlertConfiguration>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {

                IEnumerable<AlertConfiguration> alerts = from alertDB in dbContext.AlertConfiguration
                                                         join tenantDB in dbContext.Tenant on alertDB.tenantid equals tenantDB.pkid
                                                         where tenantDB.companyid == companyID
                                                         select alertDB;
                
                foreach (AlertConfiguration alertTemp in alerts)
                {
                    IEnumerable<Notification> notifis = from notiDBs in dbContext.Notification
                                                        where notiDBs.alertconfigurationID == alertTemp.pkid
                                                        select notiDBs;

                    IEnumerable<Parameter> paras = from paraDB in dbContext.Parameter
                                                   where paraDB.alertconfigurationID == alertTemp.pkid
                                                   select paraDB;
                    alertTemp.Notification = notifis.ToArray();
                    alertTemp.Parameter = paras.ToArray();
                    results.Add(alertTemp);
                }
                return results;
            }
            catch (Exception e)
            {
                throw new DBException("获取阀值异常," + e.Message);
            }
        }

        public void AddTenantAlertConfiguration(AlertConfiguration alertConfiguration)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
             
                dbContext.AlertConfiguration.Add(alertConfiguration);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加AlertConfiguration异常");
            }
        }

        //chenyangwen
        //获取alert阀值
        public List<Parameter> GetAlertThresholdsByCompanyID( string companyID)
        {
            List<Parameter> alertParameters = new List<Parameter>();

            TenantDBInterface tenantDB = new TenantDBInterface();
            //long tenantID = tenantDB.GetTenantIDByCompanyID(companyID);

            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Parameter> parameters = from paraDB in dbContext.Parameter
                                                    join alertConfigDB in dbContext.AlertConfiguration on paraDB.alertconfigurationID equals alertConfigDB.pkid
                                                    join tenantDb in dbContext.Tenant on alertConfigDB.tenantid equals tenantDb.pkid
                                                    where companyID == tenantDb.companyid
                                                    select paraDB;
                foreach (Parameter paraTemp in parameters)
                {
                    alertParameters.Add(paraTemp);
                }
                return alertParameters;
            }
            catch (Exception)
            {
                throw new DBException("获取阀值异常");
            }
        }

        //GetParameterIDByTenantID
        public long GetParameterIDByTenantID(string companyID, string alertType)
        {
            TenantDBInterface tenantDB = new TenantDBInterface();
            //long tenantID = tenantDB.GetTenantIDByCompanyID(companyID);

            long result = 0;
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Parameter> parameters = from paraDB in dbContext.Parameter
                                                    join alertConfigDB in dbContext.AlertConfiguration on paraDB.alertconfigurationID equals alertConfigDB.pkid
                                                    join tenantDb in dbContext.Tenant on companyID equals tenantDb.companyid
                                                    where alertConfigDB.tenantid == tenantDb.pkid && paraDB.ParameterType == alertType
                                                    select paraDB;
                foreach (Parameter paraTemp in parameters)
                {
                    result = paraTemp.pkid;
                }
                return result;
            }
            catch (Exception)
            {
                throw new DBException("获取阀值ID异常");
            }
        }

        //编辑alert阀值
        public void UpdateAlertThreshold(string companyID, Parameter parameter)
        {
            TenantDBInterface tenantDB = new TenantDBInterface();
            //long tenantID = tenantDB.GetTenantIDByCompanyID(companyID);

            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Parameter> parameters = from paraDB in dbContext.Parameter
                                                    join alertConfigDB in dbContext.AlertConfiguration on paraDB.alertconfigurationID equals alertConfigDB.pkid
                                                    join tenantDb in dbContext.Tenant on companyID equals tenantDb.companyid
                                                    where paraDB.pkid == parameter.pkid && alertConfigDB.tenantid == tenantDb.pkid
                                                    select paraDB;
                foreach (Parameter paraTemp in parameters)
                {
                    paraTemp.value = parameter.value;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new DBException("更新阀值异常");
            }
        }
        //通过车辆pkid获取alertconfiguration
        public Vehicle_AlertConfiguration GetVehicleAlertConfigurationByVehicleID(long vehicleID,string categroy)
        {
            Vehicle_AlertConfiguration result = new Vehicle_AlertConfiguration();
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Vehicle_AlertConfiguration> alertconfiguration = from alertconfigDB in dbContext.Vehicle_AlertConfiguration
                                                                             join vehicleDB in dbContext.Vehicle on alertconfigDB.vehicleID equals vehicleDB.pkid
                                                                             where alertconfigDB.vehicleID == vehicleID && alertconfigDB.category == categroy
                                                                             select alertconfigDB;
                if (0 == alertconfiguration.Count())
                {
                    throw new Exception("未找到");
                }
                else
                { 
                    foreach(Vehicle_AlertConfiguration v in alertconfiguration)
                    {
                        result = v;
                    }
                    return result;
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                throw new Exception("数据库异常");
            }
        }
        /// <summary>
        /// 获取geofence的Configurations
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="categroy"></param>
        /// <returns></returns>
        public List<Vehicle_AlertConfiguration> GetGeofenceConfigurationsByVehicleID(long vehicleID, string categroy)
        {
            List<Vehicle_AlertConfiguration> result = new List<Vehicle_AlertConfiguration>();
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Vehicle_AlertConfiguration> alertconfiguration = from alertconfigDB in dbContext.Vehicle_AlertConfiguration
                                                                             join vehicleDB in dbContext.Vehicle on alertconfigDB.vehicleID equals vehicleDB.pkid
                                                                             where alertconfigDB.vehicleID == vehicleID && alertconfigDB.category == categroy
                                                                             select alertconfigDB;
                if (0 == alertconfiguration.Count())
                {
                    return null;
                }
                else
                {
                    foreach (Vehicle_AlertConfiguration v in alertconfiguration)
                    {
                        result.Add(v);
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("数据库异常");
            }
        }
        //通过车辆pkid获取alert信息
        public List<Alert> GetVehicleAlertsByVehicleID(long vehicleID)
        {
            List<Alert> result = new List<Alert>();
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Alert> alerts = from alertDB in dbContext.Alert
                                            where alertDB.vehicleId == vehicleID 
                                            select alertDB;
                if (0 == alerts.Count())
                {
                    throw new Exception("未找到");
                }
                else
                {
                    foreach (Alert v in alerts)
                    {
                        result.Add(v);
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("数据库异常");
            }
        }
        //删除原有的notifications
        public void DeleteNotifications(string companyID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                List<Notification> notifications = new List<Notification>();
                IEnumerable<AlertConfiguration> alerts = from alertDB in dbContext.AlertConfiguration
                                                         join tenantDB in dbContext.Tenant on alertDB.tenantid equals tenantDB.pkid
                                                         where tenantDB.companyid == companyID
                                                         select alertDB;

                foreach (AlertConfiguration alertTemp in alerts)
                {
                    IEnumerable<Notification> notifis = from notiDBs in dbContext.Notification
                                                        where notiDBs.alertconfigurationID == alertTemp.pkid
                                                        select notiDBs;
                    foreach(Notification temp in notifis)
                    {
                        notifications.Add(temp);
                    }
                }
                foreach (Notification notificatonTemp in notifications.FindAll(t => "EMAIL".Equals(t.type)))
                {
                    dbContext.Notification.Remove(notificatonTemp);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除Notifications异常," + e.Message);
            }
        }
        //添加新的notifications
        public bool AddNotifications(string companyID,List<Notification> newNotifications)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                IEnumerable<AlertConfiguration> alerts = from alertDB in dbContext.AlertConfiguration
                                                         join tenantDB in dbContext.Tenant on alertDB.tenantid equals tenantDB.pkid
                                                         where tenantDB.companyid == companyID
                                                         select alertDB;

                foreach (AlertConfiguration alertTemp in alerts)
                {
                    foreach (Notification newNotificationTemp in newNotifications)
                    {
                        dbContext.Notification.Add(newNotificationTemp);
                    }
                }
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DBException("添加Notifications异常," + e.Message);
            }
        }
        //获取alertconfigurationIDs
        public long[] GetAlertConfigurationIDs(string companyID)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                List<Notification> notifications = new List<Notification>();
                IEnumerable<AlertConfiguration> alerts = from alertDB in dbContext.AlertConfiguration
                                                         join tenantDB in dbContext.Tenant on alertDB.tenantid equals tenantDB.pkid
                                                         where tenantDB.companyid == companyID
                                                         select alertDB;

                long[] alertconfigurationIDs = new long[4];
                int i = 0;
                foreach (AlertConfiguration alertTemp in alerts)
                {
                    alertconfigurationIDs[i] = alertTemp.pkid;
                    i++;
                }
                return alertconfigurationIDs;
            }
            catch (Exception e)
            {
                throw new DBException("添加Notifications异常," + e.Message);
            }
        }
    }
}