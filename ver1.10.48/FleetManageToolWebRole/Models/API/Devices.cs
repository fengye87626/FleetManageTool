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
    class Devices : HalResource
    {
        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("device")]
        public List<Device> devices { get; set; }
    }
}
