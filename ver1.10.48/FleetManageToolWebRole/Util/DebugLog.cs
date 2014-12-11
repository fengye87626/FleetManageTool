using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace FleetManageToolWebRole.Util
{
    public class DebugLog
    {
        public static string LoggerName = "FleetManagerToolLogger";

        //public static string FleetManagerToolExceptionLogger = "FleetManagerToolExceptionLogger";

        public enum DebugType { Debug, DBException, HttpException, OtherException, Exception }

        public static void Debug(string message)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog debugLog = LogManager.GetLogger(LoggerName);
            string info = Enum.GetName(DebugType.Debug.GetType(), DebugType.Debug)  + ":" + message;
            debugLog.Debug(info);
        }

        public static void Exception(DebugType type, string message)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog exceptionLog = LogManager.GetLogger(LoggerName);

            string info = Enum.GetName(type.GetType(), type) + ":" + message;
            exceptionLog.Error(info);
        }

        public static void Exception(string message)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(LoggerName);

            string info = Enum.GetName(DebugType.Debug.GetType(), DebugType.Exception) + ":" + message;
            log.Error(info);
        }
    }
}