using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Location
    {
        [JsonProperty("Latitude")]
        public double latitude { get; set; }

        [JsonProperty("Longitude")]
        public double longitude { get; set; }
    }
}
