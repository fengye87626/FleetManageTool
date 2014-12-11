using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class GeofenceInfo
    {
        //chenyangwen 2014/02/12
        //电子围栏
        public Geofence geofence { get; set; }

        //chenyangwen 2014/02/12
        //该电子围栏已有车辆
        public List<Vehicle> hasvehicles { get; set; }

        //chenyangwen 2014/02/12
        //该电子围栏可添加车辆
        public List<Vehicle> addvehicles { get; set; }
    }
}
