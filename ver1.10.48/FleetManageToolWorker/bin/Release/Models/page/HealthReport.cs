using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class HealthReport
    {
        public long id { get; set; }

        //车辆名称
        public string name { get; set; }

        //日期
        public string date { get; set; }
        //报警时间
        public string warnningtime { get; set; }

        //报警类型
        public string warningtype { get; set; }

        //报警信息
        public string warninginfo { get; set; }

        //车辆牌照
        public string licence { get; set; }

        //超速报警
        public string speed { get; set; }
        //超转速报警
        public string round { get; set; }
        //引擎报警
        public string engine { get; set; }
        //震动报警
        public string shake { get; set; }
        //所属分组
        public string group { get; set; }
    }
}