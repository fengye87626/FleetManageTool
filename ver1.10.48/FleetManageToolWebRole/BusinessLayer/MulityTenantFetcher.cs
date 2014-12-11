using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.DB_interface;
using FleetManageTool.WebAPI;
using FleetManageTool.Models;
using FleetManageToolWebRole.Models;
using FleetManageTool.Util;
using FleetManageToolWebRole.Models.API;
using FleetManageToolWebRole.Util;
using FleetManageTool.WebAPI.Exceptions;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class MulityTenantFetcher
    {
        //注册一个租户
        public void Regist(FleetManageToolWebRole.Models.Tenant tenant, FleetManageToolWebRole.Models.FleetUser user, Obu obu)
        {
            long deleteTenantID = -1;
            TenantDBInterface tenantdbInterface = new TenantDBInterface();
            //API
            try
            {
                DebugLog.Debug("MulityTenantFetcher Regist Customer Start");
                Device device = APIUtil.ValidateObu(obu.id, obu.regkey);
                if (null != device)
                {
                    DebugLog.Debug("MulityTenantFetcher Regist Customer null != device");

                    UserDBInterface userdbInterface = new UserDBInterface();
                    VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                    HalLink customerLink = device.Links.Find(t => t.Rel == "customer");
                    if (null != customerLink)
                    {
                        string customerid = customerLink.Href.Substring(customerLink.Href.LastIndexOf("/") + 1);
                        DebugLog.Debug("MulityTenantFetcher Regist Customer customerid = " + customerid);
                        //if (!tenantdbInterface.IsCustomerRegistered(customerid))
                        if (!tenantdbInterface.isOBUExist(obu.id, obu.regkey))
                        {
                            //DB相关的处理
                            long tenantID = tenantdbInterface.RegistTenant(tenant);
                            deleteTenantID = tenantID;
                            DebugLog.Debug("MulityTenantFetcher Regist Customer tenantID = " + tenantID);
                            List<Device> devices = APIUtil.GetDevicesOfCustomer(customerid);
                            DebugLog.Debug("MulityTenantFetcher Regist Customer devices.Count = " + devices.Count);
                            long obuid = -1;
                            foreach (Device deviceTemp in devices)
                            {
                                //chenyangwen 20140520 #1569
                                if (0 < vehicleInterface.CheckOBUAndRegKeyMatch(deviceTemp.LabelId, deviceTemp.RegistrationNumber))
                                {
                                    //DB相关的处理
                                    Obu newobu = new Obu();
                                    newobu.regkey = deviceTemp.RegistrationNumber;
                                    newobu.id = deviceTemp.LabelId;
                                    newobu.guid = deviceTemp.Id;
                                    newobu.tenantid = tenantID;
                                    newobu.idtype = deviceTemp.LabelIdType;
                                    newobu.status = "Active";
                                    obuid = vehicleInterface.AddOBU(newobu);

                                    //Add by LiYing start
                                    vehicleInterface.updateOBUStatus(newobu.id, newobu.regkey);
                                    //Add by LiYing update
                                }
                            }

                            Models.Customer customer = new Models.Customer() { guid = customerid, obuid = obuid, tenantid = tenantID };
                            tenantdbInterface.AddACustomer(customer);

                            user.tenantid = tenantID;
                            userdbInterface.AddUser(tenant.companyid, user);

                            AddDefaultAlertConfiguration(tenantID, tenant.email);
                        }
                        else
                        {
                            DebugLog.Debug("MulityTenantFetcher Regist Customer Has Registered");
                            throw new Exception("Obu is Register");
                        }
                    }
                }
            }
            catch (DBException dbException)
            {
                DebugLog.Debug("MulityTenantFetcher Regist Exception = " + dbException.Message);

                if (deleteTenantID != null && deleteTenantID != -1)
                {
                    tenantdbInterface.DeleteTenant(deleteTenantID);
                }
                throw dbException;
            }
            catch (HalException halException)
            {
                DebugLog.Debug("MulityTenantFetcher Regist Exception = " + halException.Message);
                throw halException;
            }
            catch (Exception exception)
            {
                DebugLog.Debug("MulityTenantFetcher Regist Exception = " + exception.Message);
                throw exception;
            }
        }

        public void AddDefaultAlertConfiguration(long tenantid, string email){
            AlertConfigurationDBInterface alertDB = new AlertConfigurationDBInterface();
            string[] emails = email.Split(';');

            List<Notification> notifications = new List<Notification>();
            foreach (string emailTemp in emails)
            {
                Notification notification = new Notification() { type = "EMAIL", value = emailTemp };
                notifications.Add(notification);
            }
            List<Parameter> speedparameters = new List<Parameter>();
            Parameter speedparameter = new Parameter() { ParameterColumnName = "LimitValue", ParameterType = "SPEED_THRESHOLD", value = 10 + "" };
            speedparameters.Add(speedparameter);
            Models.AlertConfiguration speedAlertConfiguration = new Models.AlertConfiguration() { category = "High Speed", state = "DISABLE", tenantid = tenantid, Notification = notifications, Parameter = speedparameters };
            alertDB.AddTenantAlertConfiguration(speedAlertConfiguration);

            List<Notification> notifications1 = new List<Notification>();
            foreach (string emailTemp in emails)
            {
                Notification notification = new Notification() { type = "EMAIL", value = emailTemp };
                notifications1.Add(notification);
            }
            List<Parameter> rpmparameters = new List<Parameter>();
            Parameter rpmparameter1 = new Parameter() { ParameterColumnName = "DurationThreshold", ParameterType = "RPM_DURATION", value = 0 + "" };
            Parameter rpmparameter2 = new Parameter() { ParameterColumnName = "LimitValue", ParameterType = "RPM_THRESHOLD", value = 2100 + "" };
            rpmparameters.Add(rpmparameter1);
            rpmparameters.Add(rpmparameter2);
            Models.AlertConfiguration rpmAlertConfiguration = new Models.AlertConfiguration() { category = "High RPM", state = "DISABLE", tenantid = tenantid, Notification = notifications1, Parameter = rpmparameters };
            alertDB.AddTenantAlertConfiguration(rpmAlertConfiguration);

            List<Notification> notifications2 = new List<Notification>();
            foreach (string emailTemp in emails)
            {
                Notification notification = new Notification() { type = "EMAIL", value = emailTemp };
                notifications2.Add(notification);
            }
            List<Parameter> motionparameters = new List<Parameter>();
            Parameter motionparameter = new Parameter() { ParameterColumnName = "LimitValue", ParameterType = "MOTION_THRESHOLD", value = 0 + "" };
            motionparameters.Add(motionparameter);
            Models.AlertConfiguration motionAlertConfiguration = new Models.AlertConfiguration() { category = "Motion Alerts", state = "DISABLE", tenantid = tenantid, Notification = notifications2, Parameter = motionparameters };
            alertDB.AddTenantAlertConfiguration(motionAlertConfiguration);

            List<Notification> notifications3 = new List<Notification>();
            foreach (string emailTemp in emails)
            {
                Notification notification = new Notification() { type = "EMAIL", value = emailTemp };
                notifications3.Add(notification);
            }
            Models.AlertConfiguration geofenceAlertConfiguration = new Models.AlertConfiguration() { category = "GeoFence", state = "ENABLED", tenantid = tenantid, Notification = notifications3 };
            alertDB.AddTenantAlertConfiguration(geofenceAlertConfiguration);
        }

        //从数据库获取所有租户
        public List<Models.Tenant> GetAllTenants()
        {
            TenantDBInterface dbInterface = new TenantDBInterface();
            UserDBInterface userInterface = new UserDBInterface();
            List<Models.Tenant> tenants = dbInterface.GetTenants();
            foreach (Models.Tenant tenantTemp in tenants)
            {
                List<FleetUser> users = userInterface.GetAllUser(tenantTemp.companyid);
                List<FleetUser> adminUsers = users.FindAll(FleetUser => FleetUser.FleetUser_Role.ElementAt(0).roleid == 1);
                tenantTemp.FleetUser = adminUsers;
            }
            return tenants;
        }

        //设置租户状态
        public bool SetTenantStatus(string tenantID, string status)
        {
            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                CacheService service = new CacheService();
                dbInterface.UpdateTenantStatus(tenantID, status);
                service.CachePut("TenantID_" + tenantID + "_Status", status);
            }
            catch (Exception e)
            {
                DebugLog.Debug("重置租户失败," + e.Message);
                return false;
            }
            return true;
        }

        //更新租户信息
        public bool UpdateTenantAdmin(string password, string tenantID, long userID)
        {
            try
            {
                string newPassword = MD5Model.getMD5String(password);
                UserDBInterface dbInterface = new UserDBInterface();
                FleetUser updateUser = dbInterface.GetUserByID(tenantID, userID);
                updateUser.password = newPassword;
                dbInterface.UpdateUser(updateUser);
            }
            catch (Exception e)
            {
                DebugLog.Debug("MulityTenantFetcher UpdateTenantAdmin exeption = " + e.Message);
                return false;
            }
            return true;
        }

        public bool IsCompanyId(string companyID)
        {
            TenantDBInterface dbInterface = new TenantDBInterface();
            UserDBInterface userInterface = new UserDBInterface();
            Models.Tenant tenantTemp = new Models.Tenant();
            tenantTemp.companyid = companyID;
            Models.Tenant tenant = dbInterface.IsCompanyRegisted(tenantTemp);
            FleetUser user = userInterface.GetUserByUserName(companyID);
            if (null != tenant || null != user)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}