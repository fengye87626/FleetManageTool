using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    [Serializable]
    public class AlertModels
    {
        public const int NO_ALERT = 0;
        public const int ENGINEPRM_ALERT = 1;
        public const int SPEED_ALERT = 2;
        public const int MOTION_ALERT = 4;
        public const int ENGINELIGHT_ALERT = 8;
        public const int GEOFENCE_ALERT = 16;

        public int timeZone { get; set; }

        public DateTime time { get; set; }

        public int alertType { get; set; }

        public string alertDetails { get; set; }
    }
}