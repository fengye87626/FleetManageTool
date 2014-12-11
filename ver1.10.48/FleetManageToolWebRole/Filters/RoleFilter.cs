using System.Web.Mvc;
using FleetManageToolWebRole.Util;
using Microsoft.ApplicationServer.Caching;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.DB_interface;

namespace FleetManageToolWebRole.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string companyid = filterContext.HttpContext.Session["companyID"].ToString();
            string roleid = filterContext.HttpContext.Session["roleID"].ToString();
            DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting companyid = " + companyid + ";roleid = " + roleid);
            FleetUser user = (FleetUser)filterContext.HttpContext.Session["nowUser"];
            if (roleid == null || roleid.Equals("") || companyid == null || companyid.Equals("") )
            {
                DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting companyid = null;roleid = null");
                string urlLogin = "/" + companyid + "/Account/Login";
                filterContext.Result = new RedirectResult(urlLogin);
            }
            else if (!roleid.Equals("1"))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting AjaxRequest 599");
                    filterContext.Result = new HttpStatusCodeResult(599);
                }
                else
                {
                    DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting roleid = " + roleid);
                    string urlLogin = "/" + companyid + "/Account/Login";
                    filterContext.Result = new RedirectResult(urlLogin);
                }
            }
            else if (roleid.Equals("1"))
            {
                //chenyangwen 20140422 907
                //FleetUser user = (FleetUser)filterContext.HttpContext.Session["nowUser"];
                if (null != user)
                {
                    UserDBInterface usedB = new UserDBInterface();
                    FleetUser_Role tempRole = usedB.GetUserRoleByUserID(user.pkid);
                    if (null == tempRole)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting AjaxRequest = 799");
                            filterContext.Result = new HttpStatusCodeResult(799);
                        }
                    }
                    else if (tempRole.roleid != 1)
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            DebugLog.Debug("FleetManageToolWebRole.Filters RoleFilter OnActionExecuting AjaxRequest = 599");
                            filterContext.Result = new HttpStatusCodeResult(599);
                        }
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