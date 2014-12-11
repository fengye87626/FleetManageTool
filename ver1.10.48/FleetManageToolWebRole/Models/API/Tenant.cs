using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Tenant : HalResource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
