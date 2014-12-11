using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class AlertConfigurationNotification
    {
        [JsonProperty("NotificationType")]
        public string NotificationType { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
