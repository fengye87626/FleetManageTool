using System.Web.Mvc;
using FleetManageToolWebRole.Util;
using System.Text;
using System.IO;
using System.Web;

namespace FleetManageToolWebRole.Filters
{
    public class AdminSessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Compare to the session
            if (filterContext.HttpContext.Session["adminstrator"] == null ||
                filterContext.HttpContext.Session["adminstrator"].ToString().Equals(string.Empty))
            {
                //chenyangwen 20140528 #1653
                if ((filterContext.HttpContext.Request.IsAjaxRequest()) || (filterContext.HttpContext.Request.Headers["X-Requested-With"] != null && "XMLHttpRequest".Equals(filterContext.HttpContext.Request.Headers["X-Requested-With"])))
                {
                    filterContext.Result = new HttpStatusCodeResult(499);
                    DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting AjaxRequest = 499");
                    filterContext.HttpContext.Response.Write("time out");
                }else if(filterContext.HttpContext.Request.Files.Count >= 1){
                    filterContext.Result = new HttpStatusCodeResult(599);
                    DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting AjaxRequest = 499");
                    filterContext.HttpContext.Response.Write("time out");
                }
                else
                {
                    string urlLogin = "/hck-fleetadmin";
                    DebugLog.Debug("FleetManageToolWebRole.Filters SessionFilter OnActionExecuting urlLogin = " + urlLogin);
                    filterContext.Result = new RedirectResult(urlLogin);
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}