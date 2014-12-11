using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class FuelReport
    {
        //车辆ID
        public long id { get; set; }

        //车辆名称
        public string name { get; set; }

        //日期
        public string date { get; set; }

        ////上次加满油后行驶里程
        //public double estimated { get; set; }

        ////费用
        //public double cost { get; set; }

        //车牌号
        public string licence { get; set; }

        //行驶时间
        public string driveTime { get; set; }

        //当天行驶里程
        public string miles { get; set; }

        //百公里油耗
        public string gallonsPerMile { get; set; }

        //油耗统计
        public string gallonsAll { get; set; }
        //所属分组
        public string group { get; set; }
    }
}