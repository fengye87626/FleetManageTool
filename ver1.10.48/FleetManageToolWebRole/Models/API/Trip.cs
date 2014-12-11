using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Trip : HalResource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("StartDateTime")]
        public Nullable<DateTime> StartDateTime { get; set; }

        [JsonProperty("EndDateTime")]
        public Nullable<DateTime> EndDateTime { get; set; }

        [JsonProperty("StartLocation")]
        public string StartLocation { get; set; }

        [JsonProperty("EndLocation")]
        public string EndLocation { get; set; }

        [JsonProperty("Distance")]
        public double Distance { get; set; }

        [JsonProperty("IdleTime")]
        public double IdleTime { get; set; }

        [JsonProperty("TripRoute")]
        public List<TripRouteDetail> TripRoute { get; set; }

        [JsonProperty("VehicleId")]
        public string VehicleId { get; set; }
    }
}
