using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class UtilizationReport
    {
        public long id { get; set; }

        //车辆名称
        public string name { get; set; }

        //日期
        public string date { get; set; }

        //使用率
        public string utilization { get; set; }

        //车辆牌照
        public string licence { get; set; }

        //行驶时间
        public string drivingTime { get; set; }

        //所属分组
        public string group { get; set; }
        //当日发动机运行时间
        public string engineontime { get; set; }
        //当日怠速时间
        public string tripidle { get; set; }

    }
}