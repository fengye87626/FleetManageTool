using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole.Filters
{
    public class ReuqestFilter : ActionFilterAttribute
    {
        private bool IsHttpsPage(ActionExecutingContext filterContext)
        {
            try
            {
                if ("Login".Equals(filterContext.ActionDescriptor.ActionName) && "FleetManageToolWebRole.Controllers.AccountController".Equals(filterContext.Controller.ToString()))
                {
                    return true;
                }
                if ("Logout".Equals(filterContext.ActionDescriptor.ActionName) && "FleetManageToolWebRole.Controllers.AccountController".Equals(filterContext.Controller.ToString()))
                {
                    return true;
                }
                if ("Register".Equals(filterContext.ActionDescriptor.ActionName) && "FleetManageToolWebRole.Controllers.AccountController".Equals(filterContext.Controller.ToString()))
                {
                    return true;
                }
                if ("FleetManageToolWebRole.Controllers.TenantController".Equals(filterContext.Controller.ToString()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                DebugLog.Exception("ReuqestFilter IsHttpsPage exception = " + exception.Message);
                DebugLog.Exception("ReuqestFilter IsHttpsPage exception = " + exception.StackTrace);
                throw exception;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                HttpRequestBase req = filterContext.HttpContext.Request;
                HttpResponseBase res = filterContext.HttpContext.Response;

                if (!req.IsSecureConnection && IsHttpsPage(filterContext))//https
                {
                    DebugLog.Debug("ReuqestFilter OnActionExecuting https");
                    var builder = new UriBuilder(req.Url)
                    {
                        Scheme = Uri.UriSchemeHttps,
                        Port = 443
                    };
                    res.Redirect(builder.Uri.ToString());
                }
                else if (req.IsSecureConnection && !IsHttpsPage(filterContext))
                {
                    DebugLog.Debug("ReuqestFilter OnActionExecuting http");
                    var builder = new UriBuilder(req.Url)
                    {
                        Scheme = Uri.UriSchemeHttp,
                        Port = 80
                    };
                    res.Redirect(builder.Uri.ToString());
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception exception)
            {
                DebugLog.Exception("ReuqestFilter OnActionExecuting exception = " + exception.Message);
                DebugLog.Exception("ReuqestFilter OnActionExecuting exception = " + exception.StackTrace);
            }
        }
    }
}