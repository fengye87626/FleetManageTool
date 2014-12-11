using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class TaskModel : HalResource
    {
        [JsonProperty("Updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("Created")]
        public DateTime Created { get; set; }

        [JsonProperty("IsFaulted")]
        public bool IsFaulted { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }
}