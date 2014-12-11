using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.Models.Common;

namespace FleetManageTool.Models.page
{
    [Serializable]
    public class OBU
    {
        //OBU类型 ESN或IMEI
        public OBUtype obuType { get; set; }

        //OBU的ESN号或者是IMEI码
        public string ESNIMEICode { get; set; }

        //OBU的注册秘钥
        public string RegistrationKey { get; set; }

        //obu状态
        public OBUStatus obuStatus { get; set; }
    }
}