using FleetManageTool.WebAPI;
using FleetManageToolWebRole.Util;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FleetManageToolWebRole
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //RefreshTokenTimer();  //chenxueliang del  20140701
            //chenyangwen 20140619
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher(
                (configName, configSettingPublisher) =>
                {
                    var connectionString =
                        RoleEnvironment.GetConfigurationSettingValue(configName);
                    configSettingPublisher(connectionString);
                }
            ); 
        }

        protected void RefreshTokenTimer()
        {
            //定时刷新Token
            String intervalTime = System.Configuration.ConfigurationManager.AppSettings["RefreshTokenTime"];
            Timer timer = new Timer(Int32.Parse(intervalTime));
            timer.Elapsed += new ElapsedEventHandler(RefreshToken);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        void RefreshToken(object sender, System.Timers.ElapsedEventArgs e)
        {
            DebugLog.Debug(" MvcApplication RefreshToken Start");
            IHalClient client = HalClient.GetInstance();
            DebugLog.Debug(" MvcApplication RefreshToken End");
        }

        //chenyangwen worker
        protected void Session_Start()
        {
            DebugLog.Debug(" MvcApplication Session_End " + Session["companyID"]);
        }

        protected void Session_End()
        {
            Object companyID = Session["companyID"];
            if (null != companyID)
            {
                CacheUtil.MinusFleetCounter(companyID.ToString());
                DebugLog.Debug(" MvcApplication Session_End " + Session["companyID"]);
            }
        }

        //chenyangwen 20140612
        protected void Application_Error()
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            string errInfo = "Error Caught in Application_Error event/n" + "Error in:" + Request.Url.ToString() + "/nError Message:" + objErr.Message.ToString() + "/nStack Trace:" + objErr.StackTrace.ToString();
            DebugLog.Exception(errInfo);
        }
    }
}