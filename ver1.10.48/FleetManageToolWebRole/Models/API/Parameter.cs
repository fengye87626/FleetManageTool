using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class AlertConfigurationParameter
    {
        [JsonProperty("ParameterType")]
        public string ParameterType { get; set; }

        [JsonProperty("ParameterColumnName")]
        public string ParameterColumnName { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
