using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Models.API;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class VehicleInfo
    {
        //对应数据库的主键
        public long primarykey { get; set; }

        //关联OBU
        public OBU obu { get; set; }

        //车辆ID
        public string vehicleID { get; set; }

        //车辆名称
        public string name { get; set; }

        //车辆logoID
        public Nullable<long> logoID { get; set; }

        //车辆的位置
        public string locationName { get; set; }

        //车辆位置
        public Location location { get; set; }
        
        //车辆的引擎启动时间
        public string engineTime { get; set; }

        //车型
        public string Info { get; set; }

        //车辆牌照
        public string license { get; set; }

        //车辆当天行驶里程
        public string odometer { get; set; }

        //油量
        public double fuel { get; set; }

        //驾驶员
        public string driver { get; set; }

        //电话
        public string telephone { get; set; }

        //电量
        public double battery { get; set; }

        //警告
        public AlertType alertType { get; set; }

        //车辆的引擎状况 ENGINEON ENGINEOFF
        public EngineStatus engineStatus { get; set; }

        //健康状况
        public HealthStatus healthStatus { get; set; }

        //groupID
        public long groupID{ get; set; }

        //groupName
        public string groupName { get; set; }

        //misState
        public MisState misState { get; set; }

        //最后定位时间 for history vehicle   fengpan
        public DateTime lastUsedTime { get; set; }

        //车辆VIn
        public string vin { get; set; }

        public int? speed { get; set; }

        public List<Trip> trips { get; set; }

        public List<Alert> alerts { get; set; }
    }
}