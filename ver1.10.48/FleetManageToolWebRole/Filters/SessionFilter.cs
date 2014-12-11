using System.Web.Mvc;
using FleetManageToolWebRole.Util;
using System.Text;
using System.IO;
using System.Web;

namespace FleetManageToolWebRole.Filters
{
    public class SessionFilter :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string companyid = string.Empty;
            string filepath = filterContext.HttpContext.Request.FilePath;
            string [] paths = filepath.Split('/');
            companyid = paths[1];            

            //Compare to the session
            if (companyid.Equals(""))
            {
                string urlAdminLogin = "/Tenant/ManagerLogin";
                DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting urlAdminLogin = " + urlAdminLogin);
                filterContext.Result = new RedirectResult(urlAdminLogin);
            }
            else if (filterContext.HttpContext.Session["companyID"] == null ||
                filterContext.HttpContext.Session["companyID"].ToString().Equals(string.Empty) ||
                !filterContext.HttpContext.Session["companyID"].ToString().Equals(companyid))
            {
                //chenyangwen 20140611 #1357
                if (null != filterContext.HttpContext.Request["intervalToken"] || filterContext.HttpContext.Request.FilePath.Contains("DrawImage"))
                {
                    filterContext.Result = new HttpStatusCodeResult(899);
                }
                else
                {
                    //chenyangwen 20140528 #1653
                    if ((filterContext.HttpContext.Request.IsAjaxRequest()) || (filterContext.HttpContext.Request.Headers["X-Requested-With"] != null && "XMLHttpRequest".Equals(filterContext.HttpContext.Request.Headers["X-Requested-With"])))
                    {
                        filterContext.Result = new HttpStatusCodeResult(499);
                        DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting AjaxRequest = 499");
                        filterContext.HttpContext.Response.Write("time out");
                    }
                    else
                    {
                        string urlLogin = "/" + companyid + "/Account/Login";
                        DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting urlLogin = " + urlLogin);
                        filterContext.Result = new RedirectResult(urlLogin);
                    }
                }
            }
            DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting CacheService");
            CacheService service = new CacheService();
            object status = service.CacheGet("TenantID_" + companyid + "_Status");
            if (null != status)
            {
                DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting status = " + status);
                //service.CacheRemove("TenantID_" + companyid + "_Status");//ChenXueliang delete on 2014-06-26
                if ("InActive".Equals(status.ToString()))
                {
                    CacheConfig.CacheSetting(filterContext.HttpContext.Response);
                    filterContext.HttpContext.Session["nowUser"] = null;
                    filterContext.HttpContext.Session["companyID"] = null;
                    filterContext.HttpContext.Session["roleID"] = null;
                    filterContext.HttpContext.Session["version"] = null;
                    filterContext.HttpContext.Session["TimeZone"] = null;
                    filterContext.HttpContext.Session["NowDate"] = null;
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting AjaxRequest = 699");
                        filterContext.Result = new HttpStatusCodeResult(699);
                    }
                    else
                    {
                        string urlLogin = "/" + companyid + "/Account/Login";
                        DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting 您的账户已被停用");
                        HttpCookie cookie = new HttpCookie("InActiveInfo", HttpUtility.UrlEncode("您的账户已被停用"));
                        filterContext.HttpContext.Response.AppendCookie(cookie);
                        filterContext.Result = new RedirectResult(urlLogin);
                    }
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}