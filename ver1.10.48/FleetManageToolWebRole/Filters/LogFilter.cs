using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String paras = "Parameters:";
            var paraNames = filterContext.ActionParameters.Keys;
            foreach (string paraName in paraNames)
            {
                paras += paraName + " = " + filterContext.ActionParameters[paraName] + ";";
            }
            DebugLog.Debug("Controller : " + filterContext.Controller + "; Action : " + filterContext.ActionDescriptor.ActionName + ";" + paras);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result;
            var exception = filterContext.Exception;
            DebugLog.Debug("Controller : " + filterContext.Controller + "; Action : " + filterContext.ActionDescriptor.ActionName + " ; Result : " + result + " ; Exception : " + exception);
            base.OnActionExecuted(filterContext);
        }
    }
}