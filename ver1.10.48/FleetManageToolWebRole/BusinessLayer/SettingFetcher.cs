using FleetManageTool.Models.page;
using FleetManageTool.WebAPI;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.API;
using FleetManageToolWebRole.Models.Common;
using FleetManageToolWebRole.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class SettingFetcher
    {
        public bool RegisterOBU(OBU obu) {

            DebugLog.Debug("SettingFetcher RegisterOBU Start");
            DebugLog.Debug("para( OBU.obuType:" + obu.obuType.ToString() + ";OBU.ESNIMEICode:" + obu.ESNIMEICode + ";OBU.RegistrationKey:" + obu.RegistrationKey + ";OBU.obuStatus:" + obu.obuStatus + ")");
            //...
            DebugLog.Debug("SettingFetcher RegisterOBU End");
            return true;
        }
        public bool SetAlertEmail(string alertEmails) {
            DebugLog.Debug("SettingFetcher SetAlertEmail Start");
            DebugLog.Debug("para( alertEmails:" + alertEmails + ")");

            string []emails = alertEmails.Split(';');
            //for (int i = 0; i < emails.Length; i++)
            //{ 
            //    //...
            //}
            DebugLog.Debug("SettingFetcher SetAlertEmail End");
                return true;
        }
        //获取alert阀值
        public AlertConfigurationInfo GetAlertThresholds()
        { //fengpan 20140318 #773
            DebugLog.Debug("SettingFetcher GetAlertThresholds Start");
            DebugLog.Debug("para()");
            AlertConfigurationInfo alertConfigs = new AlertConfigurationInfo();
            alertConfigs.speed = "200";
            alertConfigs.rpm = "3000";
            alertConfigs.rpmDuration = "5";
            alertConfigs.motion = "2.45612";
			DebugLog.Debug("SettingFetcher GetAlertThresholds End");
            return alertConfigs;
            
        }

        //API更新车辆
        public string UpdateOdometerFromApi(long VehicleID,double odometer, string companyID)
        {
            DebugLog.Debug("SettingFetcher UpdateOdometerFromApi Start");
            DebugLog.Debug("para( VehicleID:" + VehicleID + ";odometer:" + odometer + ";companyID:" + companyID+")");

            //到时候还要删除的！
            if (("ABCSoft".Equals(companyID)||("ihpleD".Equals(companyID))))
            {
                return "OK";
            }

            try
            {

                string returnValue = "";

                // 数据检索
                VehicleDBInterface dbVehicleInterface = new VehicleDBInterface();
                FleetManageToolWebRole.Models.Vehicle newVehicle = dbVehicleInterface.GetVehicleByID(VehicleID);
                Obu obu = dbVehicleInterface.GetOBUByVehicleId(newVehicle.pkid);

                if (newVehicle != null && obu != null)
                {
                    // 创建连接
                    IHalClient client = HalClient.GetInstance();

                    HalLink vehicleLink = new HalLink { Href = Models.API.URI.VEHICLESODOMETER, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    // 参数设定
                    parameters.Add(StringConst.ID, newVehicle.id == null ? String.Empty : newVehicle.id);
                    parameters.Add(StringConst.Odometer, odometer);

                    // API调用
                    Task<IHalResult> vehicleTask = client.Put(vehicleLink, parameters);
                    if (vehicleTask.Result.Success == true)
                    {
                        returnValue = "OK";
                        UpdateOdometerToCache(companyID, newVehicle.id, odometer);
                    }
                    else
                    {
                        returnValue = vehicleTask.Result.StatusCode;
                    }
                }
                DebugLog.Debug("returnValue:" + returnValue);
                DebugLog.Debug("SettingFetcher UpdateOdometerFromApi End");
                // 返回：true，false
                return returnValue;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// fengpan 20140715 将设定的里程更新到cache中
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="vehicleGuid"></param>
        /// <param name="odometer"></param>
        public void UpdateOdometerToCache(string companyID,string vehicleGuid,double odometer)
        {
            DebugLog.Debug("SettingFetcher UpdateOdometerFromCache() start");
            try 
            {
                CacheService service = new CacheService();
                List<CustomerData> customers = (List<CustomerData>)service.CacheGet(companyID + "_Cache");
                if (null != customers && 0 != customers.Count && null != customers.ElementAt(0).Vehicles)
                {
                    if (null != customers.ElementAt(0).Vehicles.Find(v => v.Id == vehicleGuid))
                    {
                        customers.ElementAt(0).Vehicles.Find(v => v.Id == vehicleGuid).status.Odometer = odometer;
                        service.CachePut(companyID + "_Cache", customers);
                    }
                }
                DebugLog.Debug("SettingFetcher UpdateOdometerFromCache() end");
            }
            catch(Exception e)
            {
                DebugLog.Debug("SettingFetcher UpdateOdometerFromCache() Exception = " + e.Message + "\n" + e.StackTrace);
            }
        }

        //API更新车辆
        //caoyandong
        public string UpdateMMYFromApi(long VehicleID,string companyID,long mmyid,string name,string vin)
        {
            DebugLog.Debug("SettingFetcher UpdateMMYFromApi Start");
            DebugLog.Debug("para( VehicleID:" + VehicleID + ";companyID:" + companyID + ")");

            //到时候还要删除的！
            if (("ABCSoft".Equals(companyID) || ("ihpleD".Equals(companyID))))
            {
                return "OK";
            }

            try
            {

                VehicleDBInterface dbVehicleInterface = new VehicleDBInterface();
                FleetManageToolWebRole.Models.Vehicle newVehicle = dbVehicleInterface.GetVehicleByID(VehicleID);

                string strtemp = "";

                Models.API.Vehicle TargetVehicle = new Models.API.Vehicle();
                TargetVehicle = APIUtil.GetOneVehicleInfo(newVehicle.id);
                if (TargetVehicle != null)
                {
                    if (TargetVehicle.Description != null)
                    {
                        strtemp = TargetVehicle.Description;
                    }
                }

                string returnValue = "";

                // 数据检索
                Obu obu = dbVehicleInterface.GetOBUByVehicleId(newVehicle.pkid);
                MMY mmy = dbVehicleInterface.GetMMYById(mmyid);
                if (newVehicle != null && obu != null)
                {
                    // 创建连接
                    IHalClient client = HalClient.GetInstance();

                    HalLink vehicleLink = new HalLink { Href = Models.API.URI.VEHICLES, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    
                    // 参数设定
                    parameters.Add(StringConst.ID, newVehicle.id == null ? String.Empty : newVehicle.id.Trim());
                    parameters.Add(StringConst.Year, mmy.mmyYear);
                    parameters.Add(StringConst.Make, mmy.mmyMake.Trim());
                    parameters.Add(StringConst.Vin, vin.Trim());
                    parameters.Add(StringConst.Model, mmy.mmyModel.Trim());
                    parameters.Add(StringConst.Nickname, name == null ? String.Empty : name.Trim());
                    parameters.Add(StringConst.Description, strtemp == null ? String.Empty : strtemp.Trim());

                    // API调用
                    Task<IHalResult> vehicleTask = client.Put(vehicleLink, parameters);

                    if (vehicleTask.Result.Success == true)
                    {
                        returnValue = "OK";
                    }
                    else
                    {
                        string[] ecode = vehicleTask.Result.ReasonPhrase.Split(':');
                        returnValue = ecode[0];
                    }
                }
                DebugLog.Debug("returnValue:" + returnValue);
                DebugLog.Debug("SettingFetcher UpdateMMYFromApi End");
                // 返回：true，false
                return returnValue;
            }
            catch (FleetManageTool.WebAPI.Exceptions.HalException e)
            {
                DebugLog.Debug(e.StackTrace);
                return "error";
            }
        }
        //更新报警邮箱时配置alertconfiguration
        public bool setAlertConfiguration(string companyID, string companyEmail)
        {
            DebugLog.Debug("[SettingFetcher] setAlertConfiguration() paras[companyID= " + companyID + ",companyEmail=" + companyEmail + "]");
            try
            {
                AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                //从数据库删除原有notification
                alertConfigDB.DeleteNotifications(companyID);
                //添加新的notification到数据库
                //companyEmail = companyEmail.Substring(0, companyEmail.Length - 1);
                companyEmail = companyEmail.Trim(';');
                string[] emails = companyEmail.Split(';');
                long[] alertconfigurationIDs = alertConfigDB.GetAlertConfigurationIDs(companyID);
                List<Models.Notification> notifications = new List<Notification>();
                for (int j = 0; j < alertconfigurationIDs.Length; j++)
                {
                    for (int i = 0; i < emails.Length; i++)
                    {
                        if ("" == emails[i] || null == emails[i])
                        {
                            continue;
                        }
                        else
                        {
                            Models.Notification notification = new Notification();
                            notification.alertconfigurationID = alertconfigurationIDs[j];
                            notification.type = "EMAIL";
                            notification.value = emails[i];
                            notifications.Add(notification);
                            //alertConfigDB.AddNotifications(companyID, notification);
                        }
                    }
                }
                alertConfigDB.AddNotifications(companyID, notifications);

                ////设置alertconfiguration
                //List<Models.AlertConfiguration> alertConfigurationDBs = alertConfigDB.GetTenantAlertConfiguration(companyID);
                //AlertFetcher alertFetcher = new AlertFetcher();
                //int speedThreshold = 0;
                //int rpmThreshold = 0;
                //int rpmDuringThreshold = 0;
                //float motionThreshold = 0;
                //foreach (Models.AlertConfiguration alertConfigurationTemp in alertConfigurationDBs)
                //{
                //    if (null != alertConfigurationTemp.Parameter.ToList().Find(t => "SPEED_THRESHOLD".Equals(t.ParameterType)))
                //    {
                //        speedThreshold = Int32.Parse(alertConfigurationTemp.Parameter.ToList().Find(t => "SPEED_THRESHOLD".Equals(t.ParameterType)).value);
                //    }
                //    if (null != alertConfigurationTemp.Parameter.ToList().Find(t => "RPM_THRESHOLD".Equals(t.ParameterType)))
                //    {
                //        rpmThreshold = Int32.Parse(alertConfigurationTemp.Parameter.ToList().Find(t => "RPM_THRESHOLD".Equals(t.ParameterType)).value);
                //    }
                //    if (null != alertConfigurationTemp.Parameter.ToList().Find(t => "RPM_DURATION".Equals(t.ParameterType)))
                //    {
                //        rpmDuringThreshold = Int32.Parse(alertConfigurationTemp.Parameter.ToList().Find(t => "RPM_DURATION".Equals(t.ParameterType)).value);
                //    }
                //    if (null != alertConfigurationTemp.Parameter.ToList().Find(t => "MOTION_THRESHOLD".Equals(t.ParameterType)))
                //    {
                //        motionThreshold = float.Parse(alertConfigurationTemp.Parameter.ToList().Find(t => "MOTION_THRESHOLD".Equals(t.ParameterType)).value);
                //    }
                //}
                //alertFetcher.SetAlertConfigInfo(companyID, alertConfigurationDBs, speedThreshold, rpmThreshold, rpmDuringThreshold, motionThreshold + "", true);
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DebugLog.Debug("[SettingFetcher] setAlertConfiguration() Exception:[e.Message=" + e.Message + "]");
                return false;
            }
        }

    }
}