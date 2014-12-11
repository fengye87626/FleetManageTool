using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class DiagnosticList : HalResource
    {
        [HalEmbedded("self-diagnostic")]
        public List<DiagnosticCode> self_diagnostic { get; set; }
    }
}