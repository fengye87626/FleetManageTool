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
    public class Alert : HalResource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("IsDisplayed")]
        public bool IsDisplayed { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("AlertType")]
        public string AlertType { get; set; }

        [JsonProperty("TriggeredDateTime")]
        public DateTime TriggeredDateTime { get; set; }

        [JsonProperty("VehicleId")]
        public string VehicleId { get; set; }

        [JsonProperty("Details")]
        public AlertDetails Details { get; set; }
    }
}
