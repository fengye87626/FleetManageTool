using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    [Serializable]
    public class VehicleModels
    {
        public const int DRIVING = 1;
        public const int PARKING = 2;
        public const int MISSTARGET = 3;

        public const int SPEEDALERT = 4;
        public const int PRMALERT = 5;
        public const int MOTIONALERT = 6;
        public const int ENGINEALERT = 7;
        public const int OK = 8;

        public const int ENGINEON = 1;
        public const int ENGINEOFF = 0;

        public long pkid { get; set; }

        public string vehicleID { get; set; }

        public string name { get; set; }

        public string logoUrl { get; set; }

        public string location { get; set; }

        public string engineTime { get; set; }

        public int engine { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public int state { get; set; }

        public string ESNIMEICode { get; set; }

        public string RegistrationKey { get; set; }

        public string Info { get; set; }

        public string license { get; set; }

        public long odometer { get; set; }

        public double fuel { get; set; }

        public string driver { get; set; }

        public double battery { get; set; }

        //fengpan
        public long groupID { get; set; }

        public int missState { get; set; }

        public int drivingstate { get; set; }

        public int alertstate { get; set; }
    }
}