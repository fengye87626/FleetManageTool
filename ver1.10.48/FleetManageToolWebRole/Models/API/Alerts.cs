using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Alerts : HalResource
    {
        [JsonProperty("TotalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("alert")]
        public List<Alert> alerts { get; set; }
    }
}