using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FleetManageToolWebRole
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "WebsiteDefault",
                url: "hck-fleetadmin/{action}",
                defaults: new { controller = "Tenant", action = "ManagerLogin", id = UrlParameter.Optional }                
            );

            //routes.MapRoute(
            //    name: "TenantTransfer",
            //    url: "Tenant/{action}",
            //    defaults: new { controller = "Tenant", action = "ManagerLogin" }
            // );




            //add by li-xiaofei  for muti_domain 
            //routes.MapRoute(
            //     name: "DomainDefault",
            //     url: "Domain/GetResourceUrl",
            //    //defaults: new { controller = "Geofence", action = "Landing", id = UrlParameter.Optional }
            //    defaults: new { controller = "Domain", action = "GetResourceUrl", id = UrlParameter.Optional }
                //defaults: new { controller = "Vehicles", action = "Index", id = UrlParameter.Optional }

      //defaults: new { controller = "Vehicles", action = "Detail", id = UrlParameter.Optional }
  //);
            routes.MapRoute(
                name: "ErrorDeal",
                url: "Error/Index",
                defaults: new { controller = "Error", action = "Index" }
             );

            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "Account", action = "Register" }
             );

            routes.MapRoute(
                name: "TenantDefault",
                url: "{tenant}/{controller}/{action}/{id}",
                //defaults: new { controller = "Geofence", action = "Landing", id = UrlParameter.Optional }
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                //defaults: new { controller = "Vehicles", action = "Index", id = UrlParameter.Optional }

                //defaults: new { controller = "Vehicles", action = "Detail", id = UrlParameter.Optional }
            );

            //chenyangwen  2014/3/6
            routes.MapRoute(
                name: "WebDefault",
                url: "",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            
        }
    }
}