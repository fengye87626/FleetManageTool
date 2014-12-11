using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    public class TripReport
    {
        public long id { get; set; }

        //车辆名称
        public string name { get; set; }

        //日期
        public string date { get; set; }

        //public string geofencestatus { get; set; }

        //行驶里程
        public string distance { get; set; }

        public string licence { get; set; }

        //行驶时间
        public string drivingTime { get; set; }

        //怠速时间
        public string idleTime { get; set; }

        //怠速时间比
        public string idleDivide { get; set; }
 
        //当天行程段数
        public int tripCnt { get; set; }

        //离开时间
        public string leavetime { get; set; }

        //到达时间
        public string arrivaltime { get; set; }

        //省
        public string leavelocation_province { get; set; }
        //市
        public string leavelocation_city { get; set; }
        //区
        public string leavelocation_district { get; set; }
        //街道及街道号
        public string leavelocation_street { get; set; }

        //省
        public string arrivallocation_province { get; set; }
        //市
        public string arrivallocation_city { get; set; }
        //区
        public string arrivallocation_district { get; set; }
        //街道及街道号
        public string arrivallocation_street { get; set; }

        //距离
        public string tripdistance { get; set; }
        //所属分组
        public string group { get; set; }
        //行程时间
        public string triptime { get; set; }
        //行程怠速时间
        public string tripidletime { get; set; }
        //形成怠速时间比
        public string tripidlerate { get; set; }
        //平均速度
        public string tripavespeed { get; set; }


        //使用率
        public string utilization { get; set; }
        //guid
        public string guid { get; set; }
        //离开地点纬度
        public double startlocctionlat { get; set; }
        //离开地点经度
        public double startlocctionlng { get; set; }
        //到达地点纬度
        public double endlocctionlat { get; set; }
        //到达地点经度
        public double endlocctionlng { get; set; }
        //起点位置是否为精确位置
        public int isFirstFlag { get; set; }
        //终点位置是否为精确位置
        public int isLastFlag { get; set; }
    }
}