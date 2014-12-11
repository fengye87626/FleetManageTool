using System.Web.Mvc;
using FleetManageToolWebRole.Util;
using System.Text;
using System.IO;
using System.Web;
using System;
using System.Collections.Generic;

namespace FleetManageToolWebRole.Filters
{
    public class SessionTimeoutTimerFilter : ActionFilterAttribute
    {
        public static String IntervalToken = String.Empty;

        public static TimeSpan RequestTime = new TimeSpan(DateTime.Now.Ticks);

        public static int TimeOut = 20 * 60;//20分钟

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            String intervalTokenTemp = req["intervalToken"];
            if (null != intervalTokenTemp)
            {
                if (intervalTokenTemp.Equals(IntervalToken))
                {
                    TimeSpan tempTime = new TimeSpan(DateTime.Now.ToUniversalTime().Ticks);
                    if (TimeOut <= tempTime.TotalSeconds - RequestTime.TotalSeconds)
                    {
                        filterContext.HttpContext.Session["nowUser"] = null;
                        filterContext.HttpContext.Session["companyID"] = null;
                        filterContext.Result = new HttpStatusCodeResult(899);
                    }
                }
                else
                {
                    IntervalToken = intervalTokenTemp;
                    RequestTime = new TimeSpan(DateTime.Now.ToUniversalTime().Ticks);
                }
            }
        }
    }
}