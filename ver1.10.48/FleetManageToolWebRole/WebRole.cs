using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            try
            {
                ServicePointManager.DefaultConnectionLimit = 512;

                // For information on handling configuration changes
                // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
                var diagnosticsConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();
                diagnosticsConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
                diagnosticsConfig.Directories.DataSources.Add(
                        new DirectoryConfiguration
                        {
                            Path = RoleEnvironment.GetLocalResource("Log4Net").RootPath,
                            Container = "log4net",
                            DirectoryQuotaInMB = 1024
                        }
                );
                string crashLogPath = RoleEnvironment.GetLocalResource("CrashLogs").RootPath;
                CrashDumps.EnableCollectionToDirectory(crashLogPath, true);
                DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticsConfig);
            }
            catch (Exception e)
            {
                DebugLog.Exception(e.Message);
            }
            return base.OnStart();
        }
    }
}
