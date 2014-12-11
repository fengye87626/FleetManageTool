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
    public class Geofence:HalResource
    {
        public enum GeofenceState { Active, InActive }

        public enum GeofenceType { CIRCLE, POLYGON }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Coordinates")]
        public List<Location> Coordinates { get; set; }

        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("GeoFenceType")]
        public string GeoFenceType { get; set; }

        [JsonProperty("Radius")]
        public double? Radius { get; set; }

        [JsonProperty("Index")]
        public int? Index { get; set; }

        [JsonProperty("Location")]
        public Location location { get; set; }

        [JsonProperty("VehicleId")]
        public string VehicleId { get; set; }

    }
}
