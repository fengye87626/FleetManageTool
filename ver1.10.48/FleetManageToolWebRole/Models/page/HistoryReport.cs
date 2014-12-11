using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page
{
    public class HistoryReport
    {
        public long id { get; set; }

        //车辆名称
        public string name { get; set; }
        //车牌号
        public string licence { get; set; }
        //最后定位时间
        public string time { get; set; }
        //最后定位终点
        //public string endlocation { get; set; }
        //最后定位起点
        //public string startlocation { get; set; }
        //最后定位地点
        public string location { get; set; }
        //最后定位地点
        public double locationlng { get; set; }
        //最后定位地点
        public double locationlat { get; set; }
        //guid
        public string guid { get; set; }
        //起始地点纬度
        public double startlocctionlat { get; set; }
        //起始地点经度
        public double startlocctionlng { get; set; }
        //到达地点纬度
        public double endlocctionlat { get; set; }
        //到达地点经度
        public double endlocctionlng { get; set; }
        //终点经纬度是否精确
        public int isLastFlag { get; set; }
        //起点经纬度是否精确
        public int isFirstFlag { get; set; }

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

        //省
        public string location_province { get; set; }
        //市
        public string location_city { get; set; }
        //区
        public string location_district { get; set; }
        //街道及街道号
        public string location_street { get; set; }
        
    }
}