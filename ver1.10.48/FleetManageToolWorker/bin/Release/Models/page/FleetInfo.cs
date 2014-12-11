using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class FleetInfo
    {
        //所有车辆
        public List<VehicleInfo> allVehicle { get; set; }

        //行驶车辆
        public List<VehicleInfo> drivingVehicle { get; set; }

        //停驶车辆
        public List<VehicleInfo> parkingVehicle { get; set; }

        //无联系车辆
        public List<VehicleInfo> misstargetVehicle { get; set; }

        //故障车辆
        public List<VehicleInfo> breakVehicle { get; set; }

        //报警车辆
        public List<VehicleInfo> alertVehicle { get; set; }

        //历史车辆
        public List<VehicleInfo> historyVehicle { get; set; }
    }
}