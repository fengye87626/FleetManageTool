using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class GeofenceAlert
    {
        //报警时间
        public DateTime alertTime { get; set; }

        //电子围栏名称
        public string geofenceName { get; set; }

        //车辆报警地点
        public string locationName { get; set; }

        //电子围栏报警信息
        public string alertInfo { get; set; }
    }
}