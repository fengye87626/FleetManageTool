using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;
using FleetManageToolWebRole.Models.API;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class AlertConfigurations : HalResource
    {
        [JsonProperty("TotalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("alert-configuration")]
        public List<AlertConfiguration> alertconfigurations { get; set; }
    }
}