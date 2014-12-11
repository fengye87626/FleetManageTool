using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.Constant
{
    public class TripConstant
    {
        public enum AlertType { Speed, Motion, Rpm };

        public enum GeofenceType { GEO_1, GEO_2, GEO_3, GEO_4, GEO_5, GEO_6 };

        public const string Minutes = "分钟";

        public const int TripsNumber = 20;

        public const string Engine = "ENGINE";

        public const string Speed = "时速";

        public const string Motion = "震动";

        public const string Rpm = "超转速";

        public const string RpmWithEmpty = "Engine RPM";

        public const string Exited = "Exited";

        public const string Enter = "Entered";

        public const string SpeedAlert = "时速报警:";
        public const string SpeedUnit = "公里/小时";
        public const string MotionLevel = "震动等级:";
        public const string EngineRPM = "转速超过:";
        public const string SpeedAlertInfo = "速度报警";
        public const string MotionAlertInfo = "震动报警";
        public const string RPMAlertInfo = "超转速报警";
        public const string EngineAlertInfo = "引擎报警";
        public const string EngineAlertDetail = "引擎故障灯亮";
        public const string ExportUtilization = "使用率(%)";
        public const string ExportUndefine = "未知";
        public const string ExportTriptime = "行程时间(分钟)";
        public const string ExportTripidletime  = "发动机怠速时间(分钟)";
        public const string ExportTripidlerate = "怠速时间比(%)";
        public const string ExportTripdistance = "距离(公里)";
        public const string ExportSum = "小计";
        public const string ExportNogroup = "未分组";
        public const string ExportLeavetime = "出发时间";
        public const string ExportIdletime = "当日发动机怠速时间(小时)";
        public const string ExportGallons = "油耗统计(升)";
        public const string ExportGallonpermiles = "平均油耗(升/百公里)";
        public const string ExportEngineontime = "当日发动机运行时间(小时)";
        public const string ExportDrivingtime = "行驶时间(小时)";
        public const string ExportDistance = "行驶里程(公里)";
        public const string ExportAvespeed = "平均速度(公里/小时)";
        public const string ExportArrivaltime = "到达时间";
        public const string ExportAlerttime = "报警时间";
        public const string ExportDetailInfo = "报警详情";
        public const string ExportVehicleLicence = "车辆牌照";
        public const string ExportVehicleName = "车辆名称";
        public const string ExportGroupName = "所属分组";
        public const string ExportDate = "日期";
        public const string ExportVehicleLastTime = "最后定位时间";
        public const string ExportHour = "小时";
        public const string ExportMinute = "分钟";
        public const string ExportSecond = "秒";
        public const string ExportDurationThreshold = "持续时间:";
    }
}