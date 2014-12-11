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
    public class Tenants : HalResource
    {
        [JsonProperty("TotalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("tenant")]
        public List<Tenant> tenants { get; set; }
    }
}
