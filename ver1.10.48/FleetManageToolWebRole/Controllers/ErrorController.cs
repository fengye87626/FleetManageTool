using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.Util;
using FleetManageToolWebRole.Filters;

namespace FleetManageToolWebRole.Controllers
{
    [ReuqestFilter]
    [LogFilter]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            CacheConfig.CacheSetting(Response);
            return View();
        }

    }
}
