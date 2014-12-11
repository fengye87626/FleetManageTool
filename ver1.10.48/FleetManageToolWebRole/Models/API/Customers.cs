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
    public class Customers : HalResource
    {
        [JsonProperty("TotalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("customer")]
        public List<Customer> customers { get; set; }
    }
}
