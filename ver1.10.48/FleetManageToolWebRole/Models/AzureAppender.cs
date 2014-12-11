using log4net.Appender;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.IO;

namespace FleetManageToolWebRole.Models
{
    public class AzureAppender : RollingFileAppender
    {
        public override string File
        {
            set
            {
                base.File = RoleEnvironment.GetLocalResource("Log4Net").RootPath + @"\"
                    + new FileInfo(value).Name + "_"
                   + Process.GetCurrentProcess().ProcessName;
            }
        }
    }
}