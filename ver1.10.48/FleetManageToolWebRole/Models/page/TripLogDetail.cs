using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.Models.Common;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class TripLogDetail
    {
        //开始时间
        public DateTime StartTime { get; set; }

        //结束时间
        public DateTime EndTime { get; set; }

        //行驶时间
        public TimeSpan DriveTime { get; set; }

        //怠速时间
        public double idleTime { get; set; }

        //距离
        public double distance { get; set; }

        //开始地点
        public string startLocation { get; set; }

        //结束地点
        public string endLocation { get; set; }

        //路线点
        public List<LocationPoint> linePoint { get; set; }
    }
}
