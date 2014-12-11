using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.Models.Common;


namespace FleetManageTool.Models.page
{
    [Serializable]
    public class TripLog
    {
        //开始时间
        public DateTime StartTime { get; set; }

        //结束时间
        public DateTime EndTime { get; set; }

        //距离
        public double distance { get; set; }

        //开始地点
        public string startLocation { get; set; }

        //结束地点
        public string endLocation { get; set; }

        //引擎灯状况
        public HealthStatus healthStatus { get; set; }

        //包含的Alert
        public List<AlertType> alerts { get; set; }

        //进出围栏信息
        public List<string> geofenceInfo { get; set; }

        //trip Api pkid
        public string id { get; set; }

        //start lng
        public double startlocationLng { get; set; }

        //start lat
        public double startlocationLat { get; set; }

        //end lng
        public double endlocationLng { get; set; }

        //end lat
        public double endlocationLat { get; set; }

        //start lng
        public double startlocationGPSLng { get; set; }

        //start lat
        public double startlocationGPSLat { get; set; }

        //end lng
        public double endlocationGPSLng { get; set; }

        //end lat
        public double endlocationGPSLat { get; set; }

        public int isFirstFlag { get; set; }

        public int isLastFlag { get; set; }
        //类型 包括(day,Normal,Trailer,Final,Driving)
        public string type { get; set; }
    }
}
