using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class DiagnosticCode : HalResource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("DiagnosticType")]
        public string DiagnosticType { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Created")]
        public DateTime Created { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("IsCleared")]
        public Boolean IsCleared { get; set; }
    }
}
