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
    class DiagnosticCodes : HalResource
    {
        [JsonProperty("TotalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [HalEmbedded("diagnostic-code")]
        public List<DiagnosticCode> diagnosticCode { get; set; }
    }
}
