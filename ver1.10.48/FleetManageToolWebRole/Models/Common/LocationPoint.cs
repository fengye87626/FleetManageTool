using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.Common
{
    [Serializable]
    public class LocationPoint
    {
        //经度
        public double longitude { get; set; }

        //纬度
        public double latitude { get; set; }

        //经纬度坐标类型（百度 、 GPS）
        public CoordinateType coordinateType { get; set; }
    }
}