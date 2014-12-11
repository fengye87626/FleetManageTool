using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Filters;
using FleetManageToolWebRole.BusinessLayer;
using System.Globalization;
//caoyandong-Operating
using FleetManageTool.Models.Common;
using FleetManageTool.Util;
using FleetManageToolWebRole.Util;
using FleetManageToolWebRole.Models.API;
using FleetManageTool.WebAPI;
using Newtonsoft.Json;
using Resource.String;

namespace FleetManageToolWebRole.Controllers
{
    //用于公司的controller
    
    public class TenantController : Controller
    {
        //生成验证码的Action
        [LogFilter]
        [ReuqestFilter]
        public ActionResult GetValidateCode()
        {
            ValidateCode validate = new ValidateCode();
            string code = validate.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            var bytes = validate.CreateValidateImage(code);
            return File(bytes, @"image/jpeg");
        }

        //重置租户管理员密码
        [LogFilter]
        [AdminSessionFilter]
        public string ResetTenant(string password, string tenantID, long userID)
        {
            if (null == Session["adminstrator"])
            {
                return "false";
            }
            MulityTenantFetcher fetcher = new MulityTenantFetcher();
            if (fetcher.UpdateTenantAdmin(password, tenantID, userID))
            {
                return "true";
            }else{
                return "false";
            }
        }

        //后台管理页面Action
        [LogFilter]
        [ReuqestFilter]
        [AdminSessionFilter]
        public ActionResult ManagerSystem()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");
            if (null == Session["adminstrator"])
            {
                return RedirectToAction("ManagerLogin");
            }
            Session["version"] = System.Configuration.ConfigurationManager.AppSettings["TestVersion"];
            return View();
        }

        //域名后台管理页面Action
        [LogFilter]
        [AdminSessionFilter]
        public ActionResult ManagerSystemDomain()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");
            if (null == Session["adminstrator"])
            {
                return RedirectToAction("ManagerSystemDomain");
            }
            Session["version"] = System.Configuration.ConfigurationManager.AppSettings["TestVersion"];
            return View();
        }



        /******************************add for domain config by li-xiaofei on 2014/5/22 begin************/
        //获取所有Domain
        [LogFilter]
        [AdminSessionFilter]
        public JsonResult GetDomainConfigs()
        {
            if (null == Session["adminstrator"])
            {
                RedirectToAction("ManagerLogin");
            }

            DomainDBInterface domainDBInterface = new DomainDBInterface();


            List<FleetManageToolWebRole.Models.DomainSetting> domainSettings = domainDBInterface.GetDomainSettings();
            return Json(domainSettings, JsonRequestBehavior.AllowGet);
        }

        /*
         *获取显示用图片
         *  
         */
        [AdminSessionFilter]
        public ActionResult DrawImageDomain()
        {
            string url = Request.Url.Host;
            return DrawImageDomainWithUrl(url);
        }


        /*
         *通过域名获取显示图片
         *  
         */
        [AdminSessionFilter]
        public ActionResult DrawImageDomainWithUrl(string url)
        {

            try
            {
                DomainDBInterface domainDBInterface = new DomainDBInterface();
                DomainSetting domainSetting = domainDBInterface.GetResource(url);

                if (domainSetting != null)
                {

                    {
                        if (domainSetting.loginlogo != null)
                        {
                            return File(domainSetting.loginlogo, @"image/jpeg");
                        }

                    }

                }


                FileStream file = null;
               
                file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Images/Logo.png", FileMode.Open, FileAccess.Read);
               
                BinaryReader binaryReader = new BinaryReader(file);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                var bytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                return File(bytes, @"image/jpeg");

            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
            }
            return null;
        }


        /*
         *删除域名配置
         * 
         */
        [AdminSessionFilter]
        public string DelDomainConfig()
        {
            DomainDBInterface domainDBInterface = new DomainDBInterface();
            DomainSetting domainSetting = new DomainSetting();
            string pkid = Request["pkid"];
            domainDBInterface.DeleteDomainSetting(System.Int64.Parse(pkid));
            return "OK";
        }


        /*
         *添加域名配置
         * 
         */
        [AdminSessionFilter]
        public string AddDomainConfig()
        {
            DomainDBInterface domainDBInterface = new DomainDBInterface();
            DomainSetting domainSetting = new DomainSetting();

            string pkid = Request["pkid"];

            string loginsubmitflag = Request["loginsubmitflag"];
            string DomainUrl = Request["Domain_dialog_Name_div"];
            if (loginsubmitflag == "1")
            {
                if (0 != Request.Files[0].ContentLength && null != Request.Files[0])
                {

                    HttpPostedFileBase file = Request.Files[0];
                    Stream fileStream = file.InputStream;
                    byte[] imagebytes = new byte[fileStream.Length];
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    imagebytes = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));
                    domainSetting.loginlogo = imagebytes;
                }
            }
    
            domainSetting.domainurl = DomainUrl;

            if (null != pkid && "" != pkid.Trim())
            {
                domainDBInterface.editDomainSetting(System.Int64.Parse(pkid), domainSetting, loginsubmitflag);

            }
            else
            {

                domainDBInterface.AddDomainSetting(domainSetting);
            }

            return "OK";
        }

         /*
         *域名重复校验
         * 
         */
        [AdminSessionFilter]
        public string CheckDomainUrl()
        {

            DomainDBInterface domainDBInterface = new DomainDBInterface();
            DomainSetting domainSetting = new DomainSetting();

            string pkid = Request["pkid"];
            string DomainUrl = Request["Domain_dialog_Name_div"];

            return domainDBInterface.CheckDomainUrl(pkid, DomainUrl);

        }

        /******************************add for domain config by li-xiaofei on 2014/5/22 end************/

        //后台登陆页面
        [LogFilter]
        [ReuqestFilter]
        public ActionResult ManagerLogin()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/03/04
            if (null == Request["ManagerName"] || (null != Session["token"] && Session["token"].ToString().Equals(Request["token"])))
            {
                //chenyanwgen 2014/3/4
                Session["adminstrator"] = null;
                return View();
            }
            else
            {
                //chenyangwen 2014/03/04
                Session["token"] = Request["token"];
                UserDBInterface dbInterface = new UserDBInterface();

                string ManagerName =StringUtil.UnicodeToString(Request["ManagerName"]);
                string Password = Request["ManagerPassword"];
                string ManagerPassword = MD5Model.getMD5String(Password);
                Administrator admin = dbInterface.GetAdminstration(ManagerName.ToLower(), ManagerPassword);
                if (null == admin)
                {
                    ViewBag.Error = "提供的用户名或密码不正确";
                    return View();
                }
                else
                {
                    //caoyandong-Operating
                    //OperatorLog.log(OperateType.LOGIN, "ManagerLogin", "-1");
                    //chenyangwen 2014/03/04
                    Session["token"] = null;
                    Session["adminstrator"] = admin;
                    return RedirectToAction("ManagerSystem");
                }
            }
        }

        //判断公司ID是否存在
        [LogFilter]
        public string IsCompanyIDExist(string companyID)
        {
            MulityTenantFetcher fetcher = new MulityTenantFetcher();
            if (fetcher.IsCompanyId(companyID.ToLower()))
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        //判断公司名是否存在
        [LogFilter]
        public string IsCompanyNameExist(string companyName)
        {
            TenantDBInterface dbInterface = new TenantDBInterface();
            FleetManageToolWebRole.Models.Tenant tenantTemp = new FleetManageToolWebRole.Models.Tenant();
            tenantTemp.companyname = companyName;
            FleetManageToolWebRole.Models.Tenant tenant = dbInterface.IsCompanyRegisted(tenantTemp);
            if (null != tenant)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        //判断验证码是否正确
        [LogFilter]
        public string IsValidateRight(string inputCode)
        {
            if (null != Session["ValidateCode"] && Session["ValidateCode"].ToString().Equals(inputCode))
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        //设置租户状态
        [LogFilter]
        [AdminSessionFilter]
        public string SetTenantStatus(string tenantID, string status)
        {
            if (null == Session["adminstrator"])
            {
                return "false";
            }
            MulityTenantFetcher fetcher = new MulityTenantFetcher();
            if (fetcher.SetTenantStatus(tenantID, status))
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        //获取所有租户
        [LogFilter]
        [AdminSessionFilter]
        public JsonResult GetTenants()
        {
            if (null == Session["adminstrator"])
            {
                RedirectToAction("ManagerLogin");
            }
            MulityTenantFetcher fetcher = new MulityTenantFetcher();
            List<FleetManageToolWebRole.Models.Tenant> tenants = fetcher.GetAllTenants();
            return Json(tenants, JsonRequestBehavior.AllowGet);
        }

        //登出Action
        [LogFilter]
        public ActionResult AdminLogout()
        {
            Session["adminstrator"] = null;
            return RedirectToAction("ManagerLogin");
        }
        [LogFilter]
        public string Check_Accunt_password(string username,string password)
        {

            string newPassword = MD5Model.getMD5String(password);
            Administrator tempUser = new Administrator();
            UserDBInterface userDbInterface = new UserDBInterface();
            tempUser = userDbInterface.GetAdminstration(username, newPassword);
            if (null != tempUser)
            {
                return "OK";
            }
            else
            {
                return "NG";
            }
        }
        [LogFilter]
        [AdminSessionFilter]
        public string EditAdmin_info(string newpassword)
        {
            UserDBInterface dbinterface = new UserDBInterface();
            string md5Password = MD5Model.getMD5String(newpassword);
            dbinterface.UpdateAdminPassword("admin", md5Password);
            return "OK";
        }

        //Add by LiYing for upload CSV Start
        [AdminSessionFilter]
        public String UploadCSVFile()
        {  
            //已注册的数据
            List<string> existedDataString = new List<string>();
            //符合条件的数据
            List<Obu_Check> rightData = new List<Obu_Check>();
            Dictionary<string, string> key_value = new Dictionary<string, string>();

            Stream stream = Request.Files[0].InputStream;

            //将文件写入到Storage 
            //chenyangwen 20140619 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            UploadFileToStorage.EnsureContainerExists();
            UploadFileToStorage.SaveFile(Request.Files[0].FileName.Substring(0, Request.Files[0].FileName.LastIndexOf('.')) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", Request.Files[0].ContentType, bytes);
            


            TenantDBInterface dbInterface = new TenantDBInterface();
            int countTemp = -1;
            if (stream.Length > 0)
            {
                List<HW_Model> hwList = new List<HW_Model>();
                hwList = dbInterface.GetAllHW_Model();
                Dictionary<string, string[]> hwMap = new Dictionary<string, string[]>();
                //将hwList转化为Map
                if (null != hwList && hwList.Count > 0)
                {
                    for (int i = 0; i < hwList.Count; i++)
                    {
                        string[] modelArray = new string[3];
                        HW_Model model = new HW_Model();
                        model = hwList.ElementAt(i);
                        modelArray[0] = model.ByteIDCheck;
                        modelArray[1] = model.LabelIDCheck;
                        modelArray[2] = model.IccIDCheck;
                        hwMap.Add(model.Name, modelArray);
                    }
                }

                StreamReader reader = new StreamReader(stream, Encoding.Default);
                List<string> list = new List<string>();
                String value;

                while ((value = reader.ReadLine()) != null)
                {
                    if (value.Length > 0)
                    {
                        list.Add(value);
                    }
                }
               
                if (list.Count > 0)
                {
                    countTemp = list.Count;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string temp = list.ElementAt(i);
                        Obu_Check obu = new Obu_Check();
                        if (temp.Contains(',')) 
                        {

                            if (temp.LastIndexOf(",") == temp.Length -1)
                            {
                                temp = temp.Remove(temp.Length - 1);
                            }
                            string[] dataArray = temp.Split(',');
                            if(dataArray.Length == 10)
                            {
                                obu.byteid = dataArray[0].Trim();
                                obu.labelid = dataArray[1].Trim();
                                obu.mdakey = dataArray[2].Trim();
                                obu.mdspc = dataArray[3].Trim();
                                obu.mdots = dataArray[4].Trim();
                                obu.mdmsn = dataArray[5].Trim();
                                obu.mdtadm = dataArray[6].Trim();
                                obu.regkey = dataArray[7].Trim();
                                obu.authtoken = dataArray[8].Trim();
                                obu.hwmodel = dataArray[9].Trim();
                                obu.iccid = "";

                            }
                            else if(dataArray.Length == 11)
                            {
                                obu.byteid = dataArray[0].Trim();
                                obu.labelid = dataArray[1].Trim();
                                obu.mdakey = dataArray[2].Trim();
                                obu.mdspc = dataArray[3].Trim();
                                obu.mdots = dataArray[4].Trim();
                                obu.mdmsn = dataArray[5].Trim();
                                obu.mdtadm = dataArray[6].Trim();
                                obu.regkey = dataArray[7].Trim();
                                obu.authtoken = dataArray[8].Trim();
                                obu.hwmodel = dataArray[9].Trim();
                                obu.iccid = dataArray[10].Trim();
                            }
                            else if (dataArray.Length < 10)
                            {
                                temp += "===" + i + "===" + "LESS";
                                key_value.Add(i + "", temp);
                                countTemp = countTemp - 1;
                            }
                            else if (dataArray.Length > 11)
                            {
                                temp += "===" + i + "===" + "MORE";
                                key_value.Add(i + "", temp);
                                countTemp = countTemp - 1;
                            }
                        }

                        if (null != obu.byteid && obu.byteid.Length > 0)
                        {
                            if (CheckCSVData(hwMap, obu))
                            {
                                rightData.Add(obu);
                                temp += "===" + i + "===" + "EX";
                                existedDataString.Add(temp);
                                
                            }
                            else
                            {
                                temp += "===" + i + "===" + "ER";
                                key_value.Add(i + "", temp);
                                countTemp = countTemp - 1;
                            }
                        }  
                    }
                }

                reader.Close();
                
                //将rightData和数据库匹配，若有已注册的数据，返回已注册的数据
                //调用db方法，查询匹配数据库
                int rightDataCount = rightData.Count;
                if (rightDataCount > 0)
                {
                    List<Obu_Check> allList = new List<Obu_Check>();
                    allList = dbInterface.SearchAllOBU();
                    if (allList.Count > 0)
                    {
                        for (int i = 0; i < rightDataCount; i++)
                        {
                           // if (allList.Contains(rightData.ElementAt(i)))
                            Obu_Check check = new Obu_Check();
                            check = rightData.ElementAt(i);
                            Obu_Check checkTemp = new Obu_Check();
                            checkTemp = allList.Find(t => t.byteid.Equals(check.byteid) && t.labelid.Equals(check.labelid) && t.regkey.Equals(check.regkey));
                            if (null != checkTemp)
                            {
                                // existedData.Add(rightData.ElementAt(i));
                                string[] strTemp = Regex.Split(existedDataString.ElementAt(i), "===", RegexOptions.IgnoreCase);
                                key_value.Add(strTemp[1] + "", existedDataString.ElementAt(i));
                                countTemp = countTemp - 1;
                            }
                        }
                    }

                }

                if (countTemp == list.Count)
                {
                    //所有数据都合格 ，将所有数据插入数据库    rightData
                    rightData = rightData.Distinct(new List_OubCheck()).ToList();
                    dbInterface.SaveCSVData(rightData);
                    return JsonConvert.SerializeObject(rightData.Count);
                   
                }
            }

            Dictionary<string, string> resultMap = key_value.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);
           
            String result = JsonConvert.SerializeObject(resultMap);
            return result;
            
        }

        //检查每条数据是否符合
        public bool CheckCSVData(Dictionary<string, string[]> map, Obu_Check obu) 
        {
            try
            {
                if (CheckStringEmpty(obu.hwmodel))
                {
                    string[] value = new string[3];
                    value = map[obu.hwmodel];
                    if (null != value && value.Length > 0)
                    {

                        if (!CheckStringEmpty(obu.byteid))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.labelid))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.mdakey))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.mdspc))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.mdots))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.mdmsn))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.mdtadm))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.regkey))
                        {
                            return false;
                        }
                        if (!CheckStringEmpty(obu.authtoken))
                        {
                            return false;
                        }
                        if (value[2].Equals("Y"))
                        {
                            if (!CheckICCID(obu.iccid))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!obu.iccid.Equals(""))
                            {
                                return false;
                            }
                        }

                    }

                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                DebugLog.Exception(e.Message);
                DebugLog.Exception(e.StackTrace);
                return false;
            }
        }

        //判断字符串是否为空
        private bool CheckStringEmpty(string str)
        {
            bool result = false;
            if (null != str && str.Length > 0 && str.Length < 50)
            {
                result = true;
            }
            return result;
        }

        //22 digits
        private bool CheckICCID(string iccid)
        {
            bool result = false;
            if (null != iccid && iccid.Trim().Length == 22)
            {
                result = isDigit(iccid);
            }
            return result;
        }

        //判断字符串是否都是数字
        private bool isDigit(String strNum)
        {
            bool result = Regex.IsMatch(strNum, "^[0-9]+$");

            return result;
        }


        //验证是否OBU是否可以注册
        public string CheckOBU(string esn, string regKey)
        {
            try
            {
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                int count = vehicleInterface.CheckOBUAndRegKeyMatch(esn, regKey);
                if (count > 0)
                {
                    Device device = APIUtil.ValidateObu(esn, regKey);
                    if (null != device)
                    {
                        TenantDBInterface tenantdbInterface = new TenantDBInterface();
                        HalLink customerLink = device.Links.Find(t => t.Rel == "customer");
                        if (null != customerLink)
                        {
                            string customerid = customerLink.Href.Substring(customerLink.Href.LastIndexOf("/") + 1);
                            //if (!tenantdbInterface.IsCustomerRegistered(customerid))
                            if (!tenantdbInterface.isOBUExist(esn, regKey))
                            {
                            //    vehicleInterface.updateOBUStatus(esn, regKey);
                                return "OK";
                            }
                            else
                            {
                                return "Regist";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantController CheckOBU exception = " + e.Message);
                return "Error";
            }

            return "Error";
        }
        //Add by LiYing End

        public string DoRegister()
        {
            try
            {
                DebugLog.Debug("VehiclesController TenantController DoRegister Start");
				MulityTenantFetcher tenantFetcher = new MulityTenantFetcher();
                //创建的租户
                TenantStatus tenantStatus = TenantStatus.Active;
                FleetManageToolWebRole.Models.Tenant registerTenant = new FleetManageToolWebRole.Models.Tenant();
                registerTenant.companyname = Request["companyName"];
                registerTenant.companyid = Request["companyID"];
                registerTenant.email = Request["hideRemindEmail"];
                registerTenant.telephone = Request["companyTel"];
                registerTenant.status = tenantStatus.ToString();

                //创建的用户
                FleetUser NewUser = new FleetUser();
                NewUser.email = Request["companyEmail"];
                NewUser.password = MD5Model.getMD5String(Request["companyPwd"]);
                NewUser.telephone = registerTenant.telephone;
                NewUser.username = registerTenant.companyid;
                FleetUser_Role user_role = new FleetUser_Role();
                user_role.roleid = 1;
                NewUser.FleetUser_Role.Add(user_role);
                if ("OK" == CheckOBU(Request["OBUESNCode"], Request["RegisterKey"]))
                {
                    //创建的OBU
                    Obu obu = new Obu() { id = Request["OBUESNCode"], regkey = Request["RegisterKey"] };

                    tenantFetcher.Regist(registerTenant, NewUser, obu);

                    string url = @"/" + registerTenant.companyid;
                    //caoyandong-Operating
                    OperatorLog.log(OperateType.REGISTER, "Register", registerTenant.companyid);
		    		DebugLog.Debug("VehiclesController TenantController DoRegister Success");
                    return "OK";
                }
                else if ("Regist" == CheckOBU(Request["OBUESNCode"], Request["RegisterKey"]))
                {
                    return "Regist";
                }
                else
                {
                    return "ObuError";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
   				DebugLog.Debug("VehiclesController TenantController DoRegister exception = " + e.Message);
                ViewBag.Alert = "注册失败，请确认填写的信息！";
                return "InfoError";
            }
        }

        //Add by LiYing Start of OBU
        public int getAllOBU()
        {
            List<Obu_Check> obuList = new List<Obu_Check>();
            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                obuList = dbInterface.SearchAllOBU();
                
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantController getAllOBU exception = " + e.Message);
            }

            return obuList.Count;
        }

        [AdminSessionFilter]
        public JsonResult getPageOBUData(int page)
        {
            List<Obu_Check> obuList = new List<Obu_Check>();
            List<Obu> useOBUList = new List<Obu>();
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                obuList = dbInterface.getPageOBUData(page, 100);
                int count = getAllOBU();

                map.Add("count", count);
                map.Add("data", obuList);
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantController getPageOBUData exception = " + e.Message);
            }
            
            return Json(map, JsonRequestBehavior.AllowGet);
        }


        public string deleteOBU(long id)
        {
            TenantDBInterface dbInterface = new TenantDBInterface();
            
            string result = "false";
            try
            {
                result = dbInterface.deleteOBU(id);
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantController deleteOBU exception = " + e.Message);
            }
            
            return result;
        }
        //后台管理页面Action
   /*     [LogFilter]
        public ActionResult ManagerSystemOBU()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");
            if (null == Session["adminstrator"])
            {
                return RedirectToAction("ManagerLogin");
            }
            Session["version"] = System.Configuration.ConfigurationManager.AppSettings["TestVersion"];
            return View();
        }
	*/
        [LogFilter]
        [AdminSessionFilter]
        public JsonResult getOBUBYKey(string key, int pageIndex)
        {
            List<Obu_Check> obuList = new List<Obu_Check>();
            int count = 0;
            TenantDBInterface dbInterface = new TenantDBInterface();
            Dictionary<string, Object> map = new Dictionary<string, object>();
            try 
            {
                obuList = dbInterface.searchOBUByKey(key, pageIndex);
                count = dbInterface.searchOBUByKeyCount(key);
                map.Add("count", count);
                map.Add("data", obuList);
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantController getOBUBYKey exception = " + e.Message);
            }

            return Json(map, JsonRequestBehavior.AllowGet); 
        }

        class List_OubCheck : IEqualityComparer<Obu_Check>
        {
            public bool Equals(Obu_Check x, Obu_Check y)
            {
                if (x.byteid.Equals(y.byteid) && x.labelid.Equals(y.labelid) && x.regkey.Equals(y.regkey))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(Obu_Check obj)
            {
                return 0;
            }
        }
        //Add by LiYing End of OBU



        //Add by caoyandong for MMY uploading
        //20140603
        [AdminSessionFilter]
        public String UploadMMYCSVFile()
        {
            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();

                List<MMY> mmy_cn = dbInterface.getMMYbylanguage("ZH-CN");
                List<MMY> mmy_en = dbInterface.getMMYbylanguage("ZH-EN");

                List<long> mmyindex = new List<long>();
                List<long> csvindex = new List<long>();
                //List<MMY> result = mmy_cn.FindAll(x => mmy_en.Find(y => y.mmyMake == x.mmyMake) != null);
                List<string> errorinfo = new List<string>();

                Dictionary<string, string> key_value = new Dictionary<string, string>();

                Stream stream = Request.Files[0].InputStream;
                if (stream.Length > 0)
                {
                    StreamReader reader = new StreamReader(stream, Encoding.Default);
                    List<string> list = new List<string>();
                    String value;

                    while ((value = reader.ReadLine()) != null)
                    {
                        if (value.Length > 0)
                        {
                            list.Add(value);
                        }
                    }

                    if (list.Count > 0)
                    {
                        int i = 0;
                        for (i = 0; i < list.Count; i++)
                        {
                            string temp = list.ElementAt(i);
                            if (temp.Contains(','))
                            {

                                if (temp.LastIndexOf(",") == temp.Length - 1)
                                {
                                    temp = temp.Remove(temp.Length - 1);
                                }
                                string[] dataArray = temp.Split(',');
                                if (dataArray.Length == 6)//2
                                {
                                    int j = 0;
                                    for (j = 0; j < 6; ++j)
                                    {
                                        if (dataArray[j].Trim().Length != 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (errorinfo.Count < 10)
                                            {
                                                errorinfo.Add(ihpleD_String_cn.rowNo + (i + 1) + ihpleD_String_cn.mmytip_emptycolumn);
                                                break;
                                            }
                                            else
                                            {
                                                return JsonConvert.SerializeObject(errorinfo);
                                            }

                                        }
                                    }
                                    if (j == 6)
                                    {
                                        int t = 0;
                                        for (t = 0; t < 2; ++t)
                                        {
                                            if (dataArray[2].Trim() == dataArray[2 + t * 3])
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                if (errorinfo.Count < 10)
                                                {
                                                    errorinfo.Add(ihpleD_String_cn.rowNo + (i + 1) + ihpleD_String_cn.mmytip_year);
                                                    break;
                                                }
                                                else
                                                {
                                                    return JsonConvert.SerializeObject(errorinfo);
                                                }
                                            }
                                        }
                                        if (t == 2)
                                        {
                                            //检查是否与数据库中文数据重复
                                            MMY mmytempe = mmy_en.Find(mmye => dataArray[0] == mmye.mmyMake
                                                 && dataArray[1] == mmye.mmyModel
                                                 && dataArray[2] == mmye.mmyYear);
                                            MMY mmytempc = mmy_cn.Find(mmyc => dataArray[3] == mmyc.mmyMake
                                                 && dataArray[4] == mmyc.mmyModel
                                                 && dataArray[5] == mmyc.mmyYear);
                                            if (null != mmytempc && mmytempe != null)
                                            {
                                                mmyindex.Add(mmytempe.mmyIndex);
                                            }
                                            if (null == mmytempc || mmytempe == null)
                                            {
                                                csvindex.Add((long)i);
                                            }
                                        }
                                        else
                                        {
                                            if (errorinfo.Count < 10)
                                            {
                                                errorinfo.Add(ihpleD_String_cn.rowNo + (i + 1) + ihpleD_String_cn.mmytip_year);
                                            }
                                            else
                                            {
                                                return JsonConvert.SerializeObject(errorinfo);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (errorinfo.Count < 10)
                                        {
                                            errorinfo.Add(ihpleD_String_cn.rowNo + (i + 1) + ihpleD_String_cn.mmytip_emptycolumn);
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(errorinfo);
                                        }
                                    }
                                }
                                else
                                {
                                    if (errorinfo.Count < 10)
                                    {
                                        errorinfo.Add(ihpleD_String_cn.rowNo + (i + 1) + ihpleD_String_cn.mmytip_sixcolumns);
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(errorinfo);
                                    }
                                }
                            }
                            continue;
                        }
                    }
                    reader.Close();
                    if (errorinfo.Count == 0)
                    {
                        //DB code here

                        //insert
                        List<string> listtoinsert = new List<string>();
                        for (int ff = 0; ff < csvindex.Count; ++ff)
                        {
                            listtoinsert.Add(list.ElementAt((int)csvindex[ff]));
                        }
                        if (mmy_cn.Count == 0)
                        {
                            dbInterface.SaveMMYData(listtoinsert, 0);
                        }
                        else
                        {
                            mmy_cn.Sort((x, y) => y.mmyIndex.CompareTo(x.mmyIndex));
                            dbInterface.SaveMMYData(listtoinsert, mmy_cn[0].mmyIndex);
                        }

                        //delete
                        for (int iIndex = 0; iIndex < mmyindex.Count; ++iIndex)
                        {
                            mmy_cn.Remove(mmy_cn.Find(x => (x.mmyIndex == mmyindex[iIndex])));
                        }
                        dbInterface.DeleteMMYData(mmy_cn);

                        return JsonConvert.SerializeObject(listtoinsert.Count);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(errorinfo);
                    }
                }
                else
                {
                    return "empty";
                }
            }
            catch (Exception e)
            {
                DebugLog.Debug("异常堆栈信息："+e.StackTrace);
                DebugLog.Debug("异常信息："+ e.Message);
                return "-1";
            }
            
        }
    }
}
