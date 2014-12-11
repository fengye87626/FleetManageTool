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
    public class TripRouteDetail
    {
        [JsonProperty("Time")]
        public DateTime Time { get; set; }

        [JsonProperty("Location")]
        public Location location { get; set; }
    }
}
