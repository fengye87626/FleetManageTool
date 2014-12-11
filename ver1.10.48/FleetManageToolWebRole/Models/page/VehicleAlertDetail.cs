using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page
{
    public class VehicleAlertDetail
    {
        //报警信息
        public string alertInfo { set; get; }

        //rpm持续时间
        public string duration { set; get; }
    }
}