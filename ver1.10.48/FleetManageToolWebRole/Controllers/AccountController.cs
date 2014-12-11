using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using System.Security.Cryptography;
using FleetManageToolWebRole.Filters;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.DB_interface;
//caoyandong-operating
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Util;
using FleetManageTool.Util;
using FleetManageToolWebRole.BusinessLayer;
using System.Threading;

namespace FleetManageToolWebRole.Controllers
{
    [ReuqestFilter]
    public class AccountController : Controller
    {
        [LogFilter]
        public ActionResult Logout()
        {
            try
            {
                //caoyandong-Operating
                if (null != Session["companyID"])
                {
                    //chenyangwen worker
                    CacheUtil.MinusFleetCounter(Session["companyID"].ToString());
                    OperatorLog.log(OperateType.LOGOUT, "UserLogout", Session["companyID"].ToString());
                }
                //chenyangwen 2014/03/04
                Session["nowUser"] = null;
                Session["companyID"] = null;
                Session["roleID"] = null;
                Session["version"] = null;

                Session["TimeZone"] = null;
                Session["NowDate"] = null;
                return View("Login");
            }
            catch (Exception exception)
            {
                DebugLog.Exception(exception.Message);
                return View("Login");
            }
        }

        [LogFilter]
        public ActionResult Login()
        {
            //此处从DB获取所有的车辆的vin，在进入首页的时候显示所有车辆的信息
            //chenyangwen 2014/03/04
            if (null == Request["UserName"] || (null != Session["token"] && Session["token"].ToString().Equals(Request["token"])))
            {
                return View();
            }
            else
            {
                //chenyangwen 2014/03/04
                Session["token"] = Request["token"];
                //chenyangwen 2014/03/04
                string UserName = StringUtil.UnicodeToString(Request["UserName"]);
                string Password = Request["Password"];

                //chenyangwen 2014/03/06
                UserDBInterface userdbInterface = new UserDBInterface();
                TenantDBInterface tenantInterface = new TenantDBInterface();
                string newPassword = MD5Model.getMD5String(Password);
                FleetUser loginUser = userdbInterface.LoginIsUser(UserName, newPassword);
                if (null != loginUser)
                {
                    Tenant tenant = tenantInterface.GetTenantByTenantID((long)loginUser.tenantid);
                    if ("Active".Equals(tenant.status))
                    {
                        //chenyangwen worker
                        Object oldCompanID = Session["companyID"];

                        Session["token"] = null;
                        Session["nowUser"] = loginUser;
                        Session["companyID"] = tenant.companyid;
                        Session["roleID"] = userdbInterface.GetUserRoleByUserID(loginUser.pkid).roleid;
                        Session["version"] = System.Configuration.ConfigurationManager.AppSettings["TestVersion"];
                        Session["TrailerDistance"] = System.Configuration.ConfigurationManager.AppSettings["TrailerDistance"];
                        //caoyandong-Operating
                        OperatorLog.log(OperateType.LOGIN, "UserLogin", Session["companyID"].ToString());

                        //chenyangwen worker
                        if (null != oldCompanID && !oldCompanID.ToString().Equals(tenant.companyid))
                        {
                            CacheUtil.MinusFleetCounter(oldCompanID.ToString());
                            CacheUtil.AddFleetCounter(tenant.companyid);
                        }
                        else if (null == oldCompanID)
                        {
                            CacheUtil.AddFleetCounter(tenant.companyid);
                        }

                        return Redirect(tenant.companyid + "/DashBoard/Home");
                        //return RedirectToAction("Home", "DashBoard", new { lat = latD, lng = lngG });
                    }
                    else if ("InActive".Equals(tenant.status))
                    {
                        ViewBag.Error = "您的账户已被停用";
                        return View();
                    }
                    return View();
                }
                else
                {
                    ViewBag.Error = "提供的用户名或密码不正确";
                    return View();
                }
            }
        }

        //注册租户
        [LogFilter]
        public ActionResult Register()
        {
            //mabiao js多语言
	    	DebugLog.Debug("VehiclesController AccountController Register Start");
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");
            Session["version"] = System.Configuration.ConfigurationManager.AppSettings["TestVersion"];
	    	DebugLog.Debug("VehiclesController AccountController Register End");
            return View();
        }
    }
}
