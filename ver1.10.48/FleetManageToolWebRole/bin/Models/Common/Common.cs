using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageTool.Models.Common
{
    //OBU状态 激活，未激活，丢失目标
    public enum OBUStatus { ACTIVE, INACTIVE, MISSTARGET }

    //OBU类型 ESN或IMEI
    public enum OBUtype { ESN, IMEI }

    //引擎启动，引擎关闭
    public enum EngineStatus { ENGINEON, ENGINEOFF }

    //超速， 震动，高转速
    public enum AlertType { SPEEDALERT, MOTIONALERT, HIGHPRMALERT, NOALERT, ENGINEALERT }

    //引擎灯亮，引擎灯灭
    public enum HealthStatus { ENGINELIGHTON, ENGINELIGHTOFF, UNKNOWN }

    public enum MisState { OK, MISSED}

    //经纬度坐标类型（百度 、 GPS）
    public enum CoordinateType { BAIDU, GPS }
    //caoyandong-operatelog
    //操作log类型
    public enum OperateType { LOGIN, LOGOUT, ADD, DEL, EDIT, RETPSW, DEACTE, ACT ,REGISTER,REPORT}
}