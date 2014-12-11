using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Vehicles : HalResource
    {
        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("vehicle")]
        public List<Vehicle> vehicles { get; set; }
    }
}