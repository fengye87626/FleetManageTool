using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Models.page;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class VehicleAlert
    {
        //报警时间
        public DateTime alertTime { get; set; }

        //超速， 震动，高转速
        public AlertType alertType { get; set; }

        //报警信息
        public VehicleAlertDetail detail { get; set; }
    }
}