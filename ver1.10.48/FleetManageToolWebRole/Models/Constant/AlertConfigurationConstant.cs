using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.Constant
{
    public class AlertConfigurationConstant
    {
        //alertType
        public const string Speed = "Speed";

        public const string Motion = "Motion";

        public const string Rpm = "EngineRPM";

        public const string EngineRpm = "Engine RPM";

        public const string Speed_cn = "时速";

        public const string Motion_cn = "震动";

        public const string Rpm_cn = "发动机转速";

        public const string Geo1 = "Geo1";

        public const string Geo2 = "Geo2";

        public const string Geo3 = "Geo3";

        public const string Geo4 = "Geo4";

        public const string Geo5 = "Geo5";

        public const string Geo6 = "Geo6";

        public const string GEO_1 = "GEO_1";

        public const string GEO_2 = "GEO_2";

        public const string GEO_3 = "GEO_3";

        public const string GEO_4 = "GEO_4";

        public const string GEO_5 = "GEO_5";

        public const string GEO_6 = "GEO_6";

        //ParameterType
        public const string SPEED_THRESHOLD = "SPEED_THRESHOLD";

        public const string MOTION_THRESHOLD = "MOTION_THRESHOLD";

        public const string RPM_THRESHOLD = "RPM_THRESHOLD";

        public const string RPM_DURATION_THRESHOLD = "RPM_DURATION";

        //catagroy
        public const string HighSpeed = "High Speed";

        public const string HighMotion = "Motion Alerts";

        public const string HighRpm = "High RPM";

        public const string HighBatter = "Batter Level";

        public const string GeoFence = "GeoFence";
    }
}