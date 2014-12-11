using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class ConfigureManager
    {
        // 获取Azure或App(Web).config下的配置节点及连接字符串
        public static string GetConfigure(string key)
        {
            string value = CloudConfigurationManager.GetSetting(key);
            if (string.IsNullOrEmpty(value))
            {
                if (null != ConfigurationManager.AppSettings[key])
                    value = ConfigurationManager.AppSettings[key];
            }
            return value;
        }
    }
}