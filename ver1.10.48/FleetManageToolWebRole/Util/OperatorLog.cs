using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageTool.Models.Common;

namespace FleetManageToolWebRole.Util
{
    public class OperatorLog
    {
        public static void log(OperateType type, string info, string companyid)
        {
            /*caoyandong-operatelog*/
            System.DateTime time = DateTime.Now;
            Operate_Log log = new Operate_Log();
            log.explain = info;
            log.logtime = time;
            log.type = (long)type;
            OperateLogDBInterface inter = new OperateLogDBInterface();
            long id = inter.AddOperateLog(companyid, log);
        }
    }
}