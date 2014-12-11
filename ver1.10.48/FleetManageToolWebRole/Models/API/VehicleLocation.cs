using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class VehicleLocation
    {
        [JsonProperty("Location")]
        public Location Location { get; set; }

        [JsonProperty("Heading")]
        public double Heading { get; set; }

        [JsonProperty("UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
    }
}