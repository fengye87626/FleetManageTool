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
    public class AlertConfiguration : HalResource
    {
        public enum AlertConfigurationState { ENABLED, DISABLED};

        public enum AlertConfigurationCategory { BATTERY_THRESHOLD, ENTER_GEOFENCE, EXIT_GEOFENCE, GEOFENCE, MOTION_THRESHOLD, RPM_THRESHOLD, RPM_DURATION, SPEED_THRESHOLD };

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Notifications")]
        public List<AlertConfigurationNotification> Notifications { get; set; }

        [JsonProperty("Parameters")]
        public List<AlertConfigurationParameter> Parameters { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("VehicleId")]
        public string VehicleId { get; set; }
    }
}
