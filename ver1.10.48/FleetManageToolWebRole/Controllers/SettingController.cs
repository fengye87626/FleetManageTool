using FleetManageToolWebRole.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.DB_interface;
using System.IO;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageTool.Models.page;
using FleetManageTool.Models.Common;
//caoyandong-Operating
using FleetManageTool.Util;
using FleetManageToolWebRole.Util;
using System.Threading;
namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class SettingController : Controller
    {
        //
        // GET: /Setting/
        [LogFilter]
        public ActionResult Tenant(/*int tabnum=0,int Vehicle_id = -1*/)//fengpan #508 20140304
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            //Session["setting_vehicleID"] = null;
            //Session["setting_tabNum"] = null;
            //ViewBag.tabNum = tabnum;
            //ViewBag.Vehicle_id = Vehicle_id;
            return View();
        }
        //清空session//fengpan #508 20140304
        [LogFilter]
        public string ClearSession() {
            Session["setting_vehicleID"] = null;
            Session["setting_tabNum"] = null;
            return "OK";
        }
        /************** fengpan accunt *******************/
        [LogFilter]
        public JsonResult GetAccunt_info()
        {
            return Json(Session["nowUser"], JsonRequestBehavior.AllowGet);
        }

        [LogFilter]
        public string EditAccunt_info(string password, string email, string tel)
        {
            
            UserDBInterface dbinterface = new UserDBInterface();
            ((FleetUser)Session["nowUser"]).email = email;
            ((FleetUser)Session["nowUser"]).telephone = tel;

            ((FleetUser)Session["nowUser"]).FleetUser_Role.Add(dbinterface.GetUserRoleByUserID(((FleetUser)Session["nowUser"]).pkid));

            string md5Password = MD5Model.getMD5String(password);
            ((FleetUser)Session["nowUser"]).password = md5Password;
            dbinterface.UpdateUser(((FleetUser)Session["nowUser"]));
            Session["nowUser"] = dbinterface.GetUserByID(Session["companyID"].ToString(), ((FleetUser)Session["nowUser"]).pkid);
            //caoyandong-Operating
            OperatorLog.log(OperateType.EDIT, "EditAccunt", Session["companyID"].ToString());
            return "OK";
        }

        [LogFilter]
        public string EditAccunt_infoExpPsw(string email, string tel)
        {

            UserDBInterface dbinterface = new UserDBInterface();
            ((FleetUser)Session["nowUser"]).email = email;
            ((FleetUser)Session["nowUser"]).telephone = tel;

            ((FleetUser)Session["nowUser"]).FleetUser_Role.Add(dbinterface.GetUserRoleByUserID(((FleetUser)Session["nowUser"]).pkid));

            dbinterface.UpdateUserExpPsw(((FleetUser)Session["nowUser"]));
            Session["nowUser"] = dbinterface.GetUserByID(Session["companyID"].ToString(), ((FleetUser)Session["nowUser"]).pkid);
            //caoyandong-Operating
            OperatorLog.log(OperateType.EDIT, "EditAccunt", Session["companyID"].ToString());
            return "OK";
        }

        [LogFilter]
        public string Check_Accunt_password(string password) {

            string newPassword = MD5Model.getMD5String(password);
            FleetUser tempUser = new FleetUser();
            UserDBInterface userDbInterface = new UserDBInterface();
            tempUser = userDbInterface.GetUserByID(Session["companyID"].ToString(),((FleetUser)Session["nowUser"]).pkid);
            if (newPassword == tempUser.password)
            {
                return "OK";
            }
            else {
                return "NG";
            }
        }
        /************** fengpan accunt *******************/

        /************** fengpan Tenant *******************/
        [LogFilter]
        public JsonResult GetTenant_info() {
            TenantDBInterface dbinterface = new TenantDBInterface();
            Tenant TenantInfo = new Tenant();
            TenantInfo = dbinterface.GetTenantByCompanyID(Session["companyID"].ToString());
            //TenantInfo.introduction = StringToUnicode(TenantInfo.introduction);
            return Json(TenantInfo, JsonRequestBehavior.AllowGet);
        }

        [RoleFilter]
        [LogFilter]
        public string EditTenant_info(string companyName, string companyEmail, string companyTel, string companyIntro)
        {
            TenantDBInterface dbinterface = new TenantDBInterface();
            Tenant TenantInfo = new Tenant();
            TenantInfo.companyname = companyName;
            //TenantInfo.email = companyEmail;
            TenantInfo.introduction = StringUtil.UnicodeToString(companyIntro);
            TenantInfo.telephone = companyTel;
            TenantInfo.companyid = Session["companyID"].ToString();
            dbinterface.UpdateTenant(TenantInfo, 0);


            //caoyandong-Operating
            OperatorLog.log(OperateType.EDIT, "EditTenant", Session["companyID"].ToString());
            return "OK";
        }
        [LogFilter]
        private void SetAlertThread(object obj){
            SettingFetcher settingFetcher = new SettingFetcher();
            settingFetcher.setAlertConfiguration(Session["companyID"].ToString(), obj.ToString());
        }
        /************** fengpan Tenant *******************/

        /************** fengpan OBU *******************/
        [LogFilter]
        public JsonResult GetOBU_Vehicles(){
            DebugLog.Debug("SettingController GetOBU_Vehicles() startTime = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            VehicleDBInterface dbinterface = new VehicleDBInterface();
            List<Dictionary<string, object>> return_vehicles = new List<Dictionary<string, object>>();
            List<Vehicle> OBU_vehicles = dbinterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());
            //chenyangwen 20140505 #1353
            List<Obu> obuNoCar = dbinterface.GetObuByTenantIDNoCar(Session["companyID"].ToString());

            Dictionary<string, object> dic = null;
            Obu obu = null;
            String esn = "";
            String registrationKey = "";
            //fengpan 20140627 从cache获取总里程
            FleetInfoFetcher fleetFetcher = new FleetInfoFetcher();
            DebugLog.Debug("SettingController GetOBU_Vehicles() GetInfoFromCache startTime = " +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var fleetInfo = fleetFetcher.GetInfoFromCache(Session["companyID"].ToString(), -1, Int32.Parse(Session["TimeZone"].ToString()), false);
            DebugLog.Debug("SettingController GetOBU_Vehicles() GetInfoFromCache endTime = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            foreach (Vehicle vehicle in OBU_vehicles)
            {
                dic = new Dictionary<string, object>();
                obu = dbinterface.GetOBUByVehicleId(vehicle.pkid);
                VehicleInfo vehicleInfo = fleetInfo.allVehicle.Find(t => t.primarykey == vehicle.pkid);
                esn = "";
                registrationKey = "";
                if (null != obu)
                {
                    esn = obu.id;
                    registrationKey = obu.regkey;
                }
                dic.Add("vehicle", vehicle);
                dic.Add("esn", esn);
                dic.Add("registrationKey", registrationKey);
                if (null != vehicleInfo)
                {
                    dic.Add("odometer", vehicleInfo.odometer.Replace(",",""));
                }
                else 
                {
                    dic.Add("odometer", "");
                }
                return_vehicles.Add(dic);
            }
            //mabiao  20140506
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
            return_vehicles.Sort((x, y) => ((Vehicle)x["vehicle"]).name.Trim().CompareTo(((Vehicle)y["vehicle"]).name.Trim()));
            //chenyangwen 20140505 #1353
            foreach (Obu obuTemp in obuNoCar)
            {
                Dictionary<string, object> dicesn = new Dictionary<string, object>();
                dicesn.Add("vehicle", null);
                dicesn.Add("esn", obuTemp.id);
                dicesn.Add("registrationKey", obuTemp.regkey);
                return_vehicles.Add(dicesn);
            }
            DebugLog.Debug("SettingController GetOBU_Vehicles() endTime = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            return Json(return_vehicles, JsonRequestBehavior.AllowGet);
        }
        [RoleFilter]
        [LogFilter]
        public string RegistionOBU(string ESN, string RegistrationKey)
        {
            SettingFetcher obuIF = new SettingFetcher();
            OBU obu = new OBU();
            //obu.obuType = OBUtype.ESN;
            obu.ESNIMEICode = ESN;
            obu.RegistrationKey = RegistrationKey;
            if (obuIF.RegisterOBU(obu))
            {
                //caoyandong-Operating
                OperatorLog.log(OperateType.REGISTER, "RegistionOBU", Session["companyID"].ToString());
                return "OK";
            }
            else {
                return "NG";
            }
        }

        [RoleFilter]
        [LogFilter]
        public string UpdateOdometer(string VehicleID, string Odometer)
        {
            string returnVal = null;
            SettingFetcher settingFetcher = new SettingFetcher();
            try
            {
                string statuscode = settingFetcher.UpdateOdometerFromApi(Int64.Parse(VehicleID), double.Parse(Odometer), Session["companyID"].ToString());
                if (statuscode == "OK")
                {
                    returnVal = "OK";
                }
                else if (statuscode == "InternalServerError")
                {
                    returnVal = "500";
                }
                else if (statuscode == "BadRequest")
                {
                    returnVal = "400";
                }
                else
                {
                    returnVal = "error";
                }
                return returnVal;
            }
            catch (Exception e)
            {
                DebugLog.Debug("SettingFetcher UpdateOdometer Error " + e.Message);
                throw new Exception(e.Message);
            }
        }
        public string UpdateMMY(string VehicleID,long mmyid,string name,string vin)
        {

            SettingFetcher settingFetcher = new SettingFetcher();
            string returnVal = settingFetcher.UpdateMMYFromApi(Int64.Parse(VehicleID), Session["companyID"].ToString(), mmyid,name,vin);

            return returnVal;
        }
        [RoleFilter]
        [LogFilter]
        public string EditOBU_Vehicles(/*string vehicleID, string vehicleName, string vehicleInfo, string vehicleLicence*/)
        {
            DebugLog.Debug("SettingFetcher EditOBU_Vehicles Start");
            string statuscode = null;
            try
            {
                //chenyangwen 2014/3/8
                CacheConfig.CacheSetting(Response);
                Logo vehicleLOGO = null;
                if (Int32.Parse(Request["flag"]) == 1)
                {
                    if (0 != Request.Files[0].ContentLength && null != Request.Files[0])
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        Stream fileStream = file.InputStream;
                        byte[] imagebytes = new byte[fileStream.Length];
                        BinaryReader binaryReader = new BinaryReader(fileStream);
                        imagebytes = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                        LogoType logotyPe = LogoType.VehicleLogo;
                        vehicleLOGO = new Logo();
                        vehicleLOGO.data = imagebytes;
                        vehicleLOGO.type = logotyPe.ToString();
                    }
                }
                long vehicleID = int.Parse(Request["OBU_vehicleID"]);
                VehicleDBInterface dbInterface = new VehicleDBInterface();
                Vehicle updateVehicle = dbInterface.GetVehicleByID(vehicleID);
                updateVehicle.name = Request["OBU_input_name"];
                if (Request["OBU_input_vin"] != null)
                {
                    updateVehicle.vin = Request["OBU_input_vin"];
                }
                updateVehicle.drivername = Request["OBU_input_driver"];
                updateVehicle.licence = Request["OBU_input_licence"];
                updateVehicle.telephone = Request["OBU_input_tel"];
                updateVehicle.lable = StringUtil.UnicodeToString(Request["OBU_input_vehiclelable"].ToString());
                if ("".Equals(Request["IsMMYEditable"]))
                {
                    updateVehicle.isMMYEditable = 0;
                }
                else
                {
                    updateVehicle.isMMYEditable = Int32.Parse(Request["IsMMYEditable"]);
                }
                if (Request["OBU_input_mmyid"] != null && !"".Equals(Request["OBU_input_mmyid"]))
                {
                    MMY mmy = dbInterface.GetMMYById(Int32.Parse(Request["OBU_input_mmyid"]));

                    if (1 == updateVehicle.isMMYEditable)
                    {
                        if (null == mmy)
                        {
                            statuscode = "e400041";
                            return statuscode;
                        }
                        updateVehicle.info = mmy.mmyMake.Trim() + " " + mmy.mmyModel.Trim() + " " + mmy.mmyYear;
                        updateVehicle.mmyid = Int32.Parse(Request["obu_input_mmyid"]);
                        string temp = UpdateMMY(vehicleID.ToString(), (long)updateVehicle.mmyid, updateVehicle.name, updateVehicle.vin);
                        if (temp == "OK")
                        {
                            dbInterface.UpdateVehicle(updateVehicle, vehicleLOGO);
                            statuscode = temp;
                        }
                        else
                        {
                            statuscode = temp;
                        }
                    }
                    else
                    {
                        dbInterface.UpdateVehicle(updateVehicle, vehicleLOGO);
                        statuscode = "OK";
                    }
                }
                //更新里程
                string odometerStatusCode = null;
                if (Int32.Parse(Request["odometerFlag"]) == 1 && "" != Request["OBU_input_odometer"])
                {
                    odometerStatusCode = UpdateOdometer(Request["OBU_vehicleID"], Request["OBU_input_odometer"]);
                    if ("OK".Equals(statuscode))
                    {
                        statuscode = odometerStatusCode;
                    }
                }
                /******#861增加车辆驾驶司机保存项liangjiajie0319******/
                //caoyandong-Operating
                OperatorLog.log(OperateType.EDIT, "EditOBU", Session["companyID"].ToString());
                //return RedirectToAction("Tenant", "Setting");
                DebugLog.Debug("SettingFetcher EditOBU_Vehicles Success");
                return statuscode;
            }
            catch (Exception e)
            {
                DebugLog.Debug("SettingFetcher EditOBU_Vehicles Error = " + e.Message);
                return "NG";
            }
        }

        [RoleFilter]
        [LogFilter]
        //TODO 支持多语言的时候需要修改【1. 传给前台的值是EN和CN的<CN,EN>【List<String,String>】，需要显示什么在前台方法中判断即可】
        // mmyid不为空的情况下，取得设定MMY中Dialog中的值（中文）
        public JsonResult GetMMYDialogData(long mmyid)
        {
            try
            {
				//mabiao  20140506
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                // 实例化DBInterface
                VehicleDBInterface dbInterface = new VehicleDBInterface();
                // 取得MMY
                MMY mmy = dbInterface.GetMMYById(mmyid);
                if (mmy == null)
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // 取得make-list
                    List<String> makeList = dbInterface.GetMakes_Chinese();
                    //mabiao 20140506 sort
                    if (null != makeList)
                    {
                        makeList.Sort();
                    }
                    // 取得model-list
                    List<String> modelList = dbInterface.GetModelsByMake(mmy.mmyMake);
                    //mabiao 20140506 sort
                    if (null != modelList)
                    {
                        modelList.Sort();
                    }
                    // 取得year-list
                    List<MMY> yearList = dbInterface.GetYearsByModel(mmy.mmyMake, mmy.mmyModel);
                    //mabiao 20140506 sort
                    if (null != yearList)
                    {
                        yearList.Sort((x,y) => x.mmyYear.CompareTo(y.mmyYear));
                    }
                    if (mmy == null)
                    {
                        return Json(String.Empty, JsonRequestBehavior.AllowGet); ;
                    }
                    else
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("mmy", mmy);
                        dic.Add("make_list", makeList);
                        dic.Add("model_list", modelList);
                        dic.Add("year_list", yearList);
                        return Json(dic, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [RoleFilter]
        [LogFilter]
        //TODO 支持多语言的时候需要修改【1. 传给前台的值是EN和CN的<CN,EN>【List<String,String>】，需要显示什么在前台方法中判断即可】
        // mmyid为空的情况下，取得设定MMY中Dialog中的值（中文）
        public JsonResult GetMakeList()
        {
            try
            {
			    //mabiao  20140506
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                // 实例化DBInterface
                VehicleDBInterface dbInterface = new VehicleDBInterface();
                // 取得make-list
                List<String> makeList = dbInterface.GetMakes_Chinese();
                //mabiao 20140506 sort
                if (null != makeList)
                {
                    makeList.Sort();
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("make_list", makeList);
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [RoleFilter]
        [LogFilter]
        //TODO 支持多语言的时候需要修改【1. 传给前台的值是EN和CN的<CN,EN>【List<String,String>】，需要显示什么在前台方法中判断即可】
        // 根据make值，取得对应的所有model值（中文）
        public JsonResult GetModelList(String make)
        {
            try
            {
				//mabiao  20140506
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                // 实例化DBInterface
                VehicleDBInterface dbInterface = new VehicleDBInterface();
                // 取得model-list
                List<String> modelList = dbInterface.GetModelsByMake(make);
                //mabiao 20140506 sort
                if (null != modelList)
                {
                    modelList.Sort();
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("model_list", modelList);
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [RoleFilter]
        [LogFilter]
        //TODO 支持多语言的时候需要修改【1. 传给前台的值是EN和CN的<CN,EN>【List<String,String>】，需要显示什么在前台方法中判断即可】
        // 根据make,model值，取得对应的所有MMY的合计（中文）
        public JsonResult GetYearList(String make,String model)
        {
            try
            {
				//mabiao  20140506
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                // 实例化DBInterface
                VehicleDBInterface dbInterface = new VehicleDBInterface();
                // 取得model-list
                List<MMY> yearList = dbInterface.GetYearsByModel(make,model);
                //mabiao 20140506 sort
                if (null != yearList)
                {
                    yearList.Sort((x, y) => x.mmyYear.CompareTo(y.mmyYear));
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("year_list", yearList);
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /************** fengpan OBU *******************/
        /************** fengpan alert阀值 *******************/
        [LogFilter]
        public JsonResult GetAlertThresholdsInfo() 
        {
            Dictionary<string, string> returnData = new Dictionary<string, string>();
            try
            {
                TenantDBInterface dbinterface = new TenantDBInterface();
                Tenant TenantInfo = new Tenant();
                TenantInfo = dbinterface.GetTenantByCompanyID(Session["companyID"].ToString());
                returnData.Add("alertEmails", TenantInfo.email);

                if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
                {
                    SettingFetcher settingFetcher = new SettingFetcher();//fengpan 20140318 #773
                    returnData.Add("motion", settingFetcher.GetAlertThresholds().motion);
                    returnData.Add("rpm", settingFetcher.GetAlertThresholds().rpm);
                    returnData.Add("rpmDuration", settingFetcher.GetAlertThresholds().rpmDuration);
                    returnData.Add("speed", settingFetcher.GetAlertThresholds().speed);

                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AlertFetcher alertFetcher = new AlertFetcher();
                    alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString());
                    returnData.Add("motion", alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString()).motion);
                    returnData.Add("rpm", alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString()).rpm);
                    returnData.Add("rpmDuration", alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString()).rpmDuration);
                    returnData.Add("speed", alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString()).speed);
                    return Json(returnData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("SettingController GetAlertThresholdsInfo() Exception=" + e.Message + "\n" + e.StackTrace);
                return new JsonResult();
            }
        }
        [LogFilter]
        public string EditAlertThresholds(string alertEmails, int speedThreshold, int rpmThreshold, int rpmTimeThreshold, string motionThreshold = "0.0")
        {
            try
            {
                DebugLog.Debug("SettingController EditAlertThresholds start");
                Tenant TenantInfo = new Tenant();
                TenantInfo.email = alertEmails;
                TenantInfo.companyid = Session["companyID"].ToString();
                TenantDBInterface dbinterface = new TenantDBInterface();
                dbinterface.UpdateTenantEmail(TenantInfo);
                SetAlertThread(alertEmails);
                //Thread th = new Thread(SetAlertThread);
                //th.Start(alertEmails);

                if ("ABCSoft".Equals(Session["companyID"].ToString()) || "ihpleD".Equals(Session["companyID"].ToString()))
                {
                    return "OK";
                }
                else
                {
                    AlertConfigurationDBInterface alertConfigDB = new AlertConfigurationDBInterface();
                    List<Models.AlertConfiguration> alertConfigurationDBs = alertConfigDB.GetTenantAlertConfiguration(Session["companyID"].ToString());
                    AlertFetcher alertFetcher = new AlertFetcher();
                    AlertConfigurationInfo alertConfigsDB = alertFetcher.GetAlertConfigInfoFromDB(Session["companyID"].ToString());//从数据库获取配置信息

                    if (motionThreshold.Equals(alertConfigsDB.motion) && Int32.Parse(alertConfigsDB.rpm) == rpmThreshold && Int32.Parse(alertConfigsDB.rpmDuration) == rpmTimeThreshold && Int32.Parse(alertConfigsDB.speed) == speedThreshold)
                    {
                        DebugLog.Debug("SettingController EditAlertThresholds end return OK");
                        return "OK";
                    }

                    /*********************************************/
                    SetAlertConfigInfoThread parameterThread = new SetAlertConfigInfoThread(Session["companyID"].ToString(), alertConfigurationDBs, speedThreshold, rpmThreshold, rpmTimeThreshold, motionThreshold, true);
                    parameterThread.Start();
                    return "OK";
                    /*********************************************/
                    
                    //if (alertFetcher.SetAlertConfigInfo(Session["companyID"].ToString(), alertConfigurationDBs, speedThreshold, rpmThreshold, rpmTimeThreshold, motionThreshold, true))
                    //{
                    //    DebugLog.Debug("SettingController EditAlertThresholds end  return OK");
                    //    return "OK";
                    //}
                    //else
                    //{
                    //    DebugLog.Debug("SettingController EditAlertThresholds end  return NG");
                    //    return "NG";
                    //}
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("SettingController EditAlertThresholds Excettion=" + e.Message+"\n"+e.StackTrace);
                return "NG";
            }
        }
        /************** fengpan alert阀值 *******************/
        [LogFilter]
        public JsonResult GetUser()
        {
            UserDBInterface dbInterface = new UserDBInterface();
            List<FleetUser> users = dbInterface.GetAllUser(Session["companyID"].ToString());
            FleetUser cur_user = (FleetUser)Session["nowUser"];
            foreach (FleetUser userTemp in users)
            {
                if (userTemp.pkid == cur_user.pkid)
                {
                    users.Remove(userTemp);
                    break;
                }
            }
            foreach (FleetUser userTemp in users)
            {
                FleetUser_Role roles = dbInterface.GetUserRoleByUserID(userTemp.pkid);
                userTemp.FleetUser_Role.Add(roles);
            }
            if (null != users)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                users.Sort((x, y) => x.username.Trim().CompareTo(y.username.Trim()));
            }
            int usercount = 0;
            if(users.Count % 9 == 0)
            {
                usercount = users.Count / 9;
            }else{
                usercount = users.Count / 9 + 1;
            }
            List<FleetUser> usersforpage = new List<FleetUser>();
            int pagenum = Int32.Parse(Request["pagenum_user"]);
            if (pagenum * 9 >= users.Count)
            {
                for (int i = (pagenum - 1) * 9; i < users.Count; ++i)
                {
                    usersforpage.Add(users[i]);
                }
            }
            else
            {
                for (int i = (pagenum - 1) * 9; i < pagenum * 9; ++i)
                {
                    usersforpage.Add(users[i]);
                }
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("pagecount", usercount);
            dic.Add("userlist", usersforpage);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [RoleFilter]
        [LogFilter]
        // 添加用户
        public String AddUser(String username,String role,String email,String telephone,String password)
        {
            

            UserDBInterface dbInterface = new UserDBInterface();


            //todo
            string newPassword = MD5Model.getMD5String(password);

            FleetUser adduser = new FleetUser();
            adduser.username = username;
            adduser.telephone = telephone;
            adduser.email = email;

            FleetUser_Role user_role = new FleetUser_Role();
            //user_role = adduser.user_role.ElementAt(0);
            if (role == "1")
            {
                user_role.roleid = 1;
            }
            else if (role == "2")
            {
                user_role.roleid = 2;
            }
            adduser.FleetUser_Role.Add(user_role);
            adduser.password = newPassword;

            String AddUserID = dbInterface.AddUser(Session["companyID"].ToString(), adduser).ToString();


            String result = AddUserID ;
            //caoyandong-Operating
            OperatorLog.log(OperateType.ADD, "AddUser", Session["companyID"].ToString());
            return result;
        }
        [RoleFilter]
        [LogFilter]
        // 删除用户
        public String DelUser(String pkid)
        {
            string result = null;
                //caoyandong-Operating
                OperatorLog.log(OperateType.DEL, "DelUser", Session["companyID"].ToString());
            UserDBInterface dbInterface = new UserDBInterface();

            if (! "".Equals(pkid))
            {
                string[] ids = pkid.Split(',');
                foreach (string id in ids)
                {
                    FleetUser delUser = new FleetUser();
                    delUser.pkid = System.Int64.Parse(id);
                    //chenyangwen 20140409
                    if (!isLastAdmin(delUser.pkid))
                    {
                        dbInterface.DeleteUser(delUser);
                        result = "OK";
                    }
                    else
                    {
                        result = "Error";
                        break;
                    }
                }
            }
            return result;
        }

        //chenyangwen 20140409 判断是否是最后一个管理员
        public bool isLastAdmin(long userID)
        {
            UserDBInterface dbInterface = new UserDBInterface();
            FleetUser_Role role = dbInterface.GetUserRoleByUserID(userID);
            if (1 == role.roleid)
            {
                List<FleetUser> users = dbInterface.GetAllUser(Session["companyID"].ToString());
                int count = users.FindAll(u => u.FleetUser_Role.ElementAt(0).roleid == 1 ).Count;
                if (count > 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
            else
            {
                return false;
            }
        }

        [RoleFilter]
        [LogFilter]
        // 重置用户密码
        public String ResetUser(String pkid,String password)
        {
            string result = null;
                //caoyandong-Operating
                OperatorLog.log(OperateType.RETPSW, "ResetUser", Session["companyID"].ToString());
            UserDBInterface dbInterface = new UserDBInterface();
            List<FleetUser> users = dbInterface.GetAllUser(Session["companyID"].ToString());

            string newPassword = MD5Model.getMD5String(password);

            long delpkid = long.Parse(pkid);
            foreach (FleetUser userTemp in users)
            {
                userTemp.FleetUser_Role.Add(dbInterface.GetUserRoleByUserID(userTemp.pkid));
                if (delpkid == userTemp.pkid)
                {
                    userTemp.password = newPassword;
                    dbInterface.UpdateUser(userTemp);
                    result = "OK";
                }
            }
            return result;
        }
        [RoleFilter]
        [LogFilter]
        // 编辑用户
        public String EditUser(String pkid, String username, String role, String email, String telephone)
        {
            string result = null;
                //caoyandong-Operating
                OperatorLog.log(OperateType.EDIT, "EditUser", Session["companyID"].ToString());
            UserDBInterface dbInterface = new UserDBInterface();
            List<FleetUser> users = dbInterface.GetAllUser(Session["companyID"].ToString());

            

            long delpkid = long.Parse(pkid);
            foreach (FleetUser userTemp in users)
            {
                userTemp.FleetUser_Role.Add(dbInterface.GetUserRoleByUserID(userTemp.pkid));
                
                if (delpkid == userTemp.pkid)
                {
                    userTemp.username = username;
                    userTemp.email = email;
                    userTemp.telephone = telephone;

                    if (role == "1")
                    {
                        FleetUser_Role user_role = new FleetUser_Role();
                        user_role = userTemp.FleetUser_Role.ElementAt(0);
                        userTemp.FleetUser_Role.ElementAt(0).roleid = 1;
                    }
                    else if (role == "2")
                    {
                        //chenyangwen 20140409
                        if (1 == userTemp.FleetUser_Role.ElementAt(0).roleid && isLastAdmin(userTemp.pkid))
                        {
                            result = "Error";
                            return result;
                        }
                        FleetUser_Role user_role = new FleetUser_Role();
                        user_role = userTemp.FleetUser_Role.ElementAt(0);
                        userTemp.FleetUser_Role.ElementAt(0).roleid = 2;
                    }
                    dbInterface.UpdateUser(userTemp);
                    result = "OK";
                }
            }
            return result;
        }


        /**********Group****/

        //从数据库获取所有的车组显示在下拉列表中
        [LogFilter]
        public JsonResult GetGroup()
        {
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            if (null != groups)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                groups.Sort((x, y) => x.name.Trim().CompareTo(y.name.Trim()));
            }
            return Json(groups, JsonRequestBehavior.AllowGet);
        } 

        //从数据库获取group所有车辆
        //根据车族获取车型
        [LogFilter]
        public JsonResult GetGroupData(String groupID)
        {
            int group = int.Parse(groupID);
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<Vehicle> vehicles_Group = new List<Vehicle>();
            if (-1 == group)
            {
                vehicles_Group = dbInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());
            }
            else
            {
                vehicles_Group = dbInterface.GetGroupVehiclesByGroupId(group);
            }
            //已有车辆列表，刷新加载时排序增加 #750 liangjiajie20140401
            vehicles_Group.Sort((x, y) => x.name.CompareTo(y.name));

            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles = dbInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());

            //mabiao 20140228 获取所有分组 
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            //mabiao 20140228 获取所有分组 

            /*List<Vehicle> group_vehicles = new List<Vehicle>();*/
            List<Vehicle> allgroup_vehicles = new List<Vehicle>();
            /*group_vehicles = dbInterface.GetGroupVehiclesByGroupId(group);*/

            //mabiao 20140228 获取所有分组的所有车辆
            foreach (VehicleGroup grouptemp in groups)
            {
                List<Vehicle> group_vehicles = new List<Vehicle>();
                group_vehicles = dbInterface.GetGroupVehiclesByGroupId(grouptemp.pkid);
                foreach (Vehicle vehicletemp in group_vehicles)
                {
                    allgroup_vehicles.Add(vehicletemp);
                }
            }

            List<Vehicle> not_group_vehicles = new List<Vehicle>();

            int count = 0;
            foreach (Vehicle all in vehicles)
            {
                count = 0;
                //foreach (Vehicle group_vehicle in group_vehicles)
                foreach (Vehicle group_vehicle in allgroup_vehicles)
                {
                    if (group_vehicle.pkid == all.pkid)
                    {
                        break;
                    }
                    count++;
                }

                /*if (count == group_vehicles.Count)*/
                if (count == allgroup_vehicles.Count)
                {
                    not_group_vehicles.Add(all);
                }
            }
            //可添加车辆列表，刷新加载时排序增加 #750 liangjiajie20140401
            not_group_vehicles.Sort((x, y) => x.name.CompareTo(y.name));

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("group_vehicles", vehicles_Group);
            dic.Add("not_group_vehicles", not_group_vehicles);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [RoleFilter]
        [LogFilter]
        //添加车辆
        public String GroupAddVehicle(String Vehicle,String GroupID)
        {
            string result = null;
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            List<Vehicle> vehicles = new List<Vehicle>();
            List<Vehicle> group_vehicles = new List<Vehicle>();
            VehicleGroup updateGroup = new VehicleGroup();

            String[] array = Vehicle.Split(',');
            //#587 增加车辆是否已经被分组判断 liangjiajie20140325
            if (false == IsVehicleAvailable(array))
            {
                return "NG";
            }
            foreach (VehicleGroup groupTemp in groups)
            {
                if (GroupID == groupTemp.pkid.ToString())
                {
                    updateGroup = groupTemp;
                    break;
                }
            }

            group_vehicles = dbInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());

            foreach (String vehicleID in array)
            {
                foreach(Vehicle vehicleTemp in group_vehicles)
                {
                    if (vehicleID == vehicleTemp.pkid.ToString())
                    {
                        vehicles.Add(vehicleTemp);
                        break;
                    }
                }
            }

            try
            {
                dbInterface.UpdateGroupAddVehicle(updateGroup, vehicles);
                result = "OK";
            }
            catch (Exception)
            {
                result = "NG";
            }

            //caoyandong-Operating
            OperatorLog.log(OperateType.ADD, "GroupAddVehicle", Session["companyID"].ToString());
            
            return result;
        }
        [RoleFilter]
        [LogFilter]
        //删除车辆
        public String GroupDelVehicle(String Vehicle, String GroupID)
        {
            string result = null;
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            List<Vehicle> vehicles = new List<Vehicle>();
            List<Vehicle> group_vehicles = new List<Vehicle>();
            VehicleGroup updateGroup = new VehicleGroup();

            String[] array = Vehicle.Split(',');

            foreach (VehicleGroup groupTemp in groups)
            {
                if (GroupID == groupTemp.pkid.ToString())
                {
                    updateGroup = groupTemp;
                    break;
                }
            }

            group_vehicles = dbInterface.GetGroupVehiclesByGroupId(updateGroup.pkid);

            foreach (String vehicleID in array)
            {
                foreach (Vehicle vehicleTemp in group_vehicles)
                {
                    if (vehicleID == vehicleTemp.pkid.ToString())
                    {
                        vehicles.Add(vehicleTemp);
                        break;
                    }
                }
            }
            dbInterface.UpdateGroupRemoveVehicle(updateGroup, vehicles);
                //caoyandong-Operating
                OperatorLog.log(OperateType.DEL, "GroupDelVehicle", Session["companyID"].ToString());
            result = "OK";
            return result;
        }
        [RoleFilter]
        [LogFilter]
        //添加Group
        public String GroupAddGroup(String name)
        {
            string result = null;
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            TenantDBInterface tenantDB = new TenantDBInterface();

            VehicleGroup addgroup = new VehicleGroup();
            long tenantID = tenantDB.GetTenantIDByCompanyID(Session["companyID"].ToString());
            addgroup.tenantid = tenantID;
            addgroup.name = name;
            result = dbInterface.AddGroup(Session["companyID"].ToString(), addgroup).ToString();
                //caoyandong-Operating
                OperatorLog.log(OperateType.ADD, "GroupAddGroup", Session["companyID"].ToString());

            return result;
        }
        [RoleFilter]
        [LogFilter]
        //删除Group
        public String GroupDelGroup(String pkid)
        {
            string result = null;
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            VehicleGroup delgroup = new VehicleGroup();
            foreach (VehicleGroup groupTemp in groups)
            {
                if (pkid == groupTemp.pkid.ToString())
                {
                    delgroup = groupTemp;
                    break;
                }
            }
            dbInterface.DeleteGroup(delgroup);
            OperatorLog.log(OperateType.DEL, "GroupDelGroup", Session["companyID"].ToString());
            result = "OK";
            return result;
        }
        [RoleFilter]
        [LogFilter]
        //编辑Group
        public String GroupEditGroup(String pkid, String name)
        {
            string result = null;
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            VehicleGroup editgroup = new VehicleGroup();
            foreach (VehicleGroup groupTemp in groups)
            {
                if (pkid == groupTemp.pkid.ToString())
                {
                    editgroup = groupTemp;
                    break;
                }
            }
            editgroup.name = name;
            dbInterface.UpdateGroup(Session["companyID"].ToString(), editgroup);
            //caoyandong-Operating
            OperatorLog.log(OperateType.EDIT, "GroupEditGroup", Session["companyID"].ToString());
            result = "OK";
            return result;
        }

        //判断用户名是否存在
        [LogFilter]
        public string IsUserNameExist(string username)
        {
            UserDBInterface userDB = new UserDBInterface();
            FleetUser user = userDB.GetUserByUserName(username.ToLower());
            if (null == user)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        //判断分组是否存在
        [LogFilter]
        public string IsGroupNameExist(string groupname)
        {
            VehicleDBInterface groupDB = new VehicleDBInterface();
            VehicleGroup group = groupDB.GetVehicleGroupByGroupName(groupname, Session["companyID"].ToString());
            if (null == group)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        //#733判断车辆名称是否存在liangjiajie0319
        [LogFilter]
        public string IsVehicleNameExist(string vehiclename)
        {
            VehicleDBInterface VehicleDB = new VehicleDBInterface();
            Vehicle vehicle = VehicleDB.GetVehicleByVehicleName(Session["companyID"].ToString(),vehiclename);
            if (null == vehicle)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }


        //#587  判断车辆是否已经被添加到分组中
        //liangjiajie 20140325
        public bool IsVehicleAvailable(string[] array)
        {
            VehicleDBInterface dbInterface = new VehicleDBInterface();
            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles = dbInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString());

            //mabiao 20140228 获取所有分组 
            List<VehicleGroup> groups = dbInterface.GetGroupsByCompannyID(Session["companyID"].ToString());
            //mabiao 20140228 获取所有分组 

            List<Vehicle> allgroup_vehicles = new List<Vehicle>();

            //mabiao 20140228 获取所有分组的所有车辆
            foreach (VehicleGroup grouptemp in groups)
            {
                List<Vehicle> group_vehicles = new List<Vehicle>();
                group_vehicles = dbInterface.GetGroupVehiclesByGroupId(grouptemp.pkid);
                foreach (Vehicle vehicletemp in group_vehicles)
                {
                    allgroup_vehicles.Add(vehicletemp);
                }
            }

            foreach (string vehicleID in array)
            {
                foreach (Vehicle vehicleTemp in allgroup_vehicles)
                {
                    if (vehicleID.Equals(vehicleTemp.pkid.ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
