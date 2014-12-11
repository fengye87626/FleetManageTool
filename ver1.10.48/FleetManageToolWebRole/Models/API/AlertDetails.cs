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
    public class AlertDetails : HalResource
    {
        [JsonProperty("LimitValue")]
        public string LimitValue { get; set; }

        [JsonProperty("DurationThreshold")]
        public string DurationThreshold { get; set; }

        [JsonProperty("Slot")]
        public string Slot { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Event")]
        public string Event { get; set; }

        public override string ToString()
        {
            string result = "{";
            if (null != LimitValue)
            {
                result += "\"LimitValue\":\"" + LimitValue + "\",";
            }
            if (null != DurationThreshold)
            {
                result += "\"DurationThreshold\":\"" + DurationThreshold + "\",";
            }
            if (null != Slot)
            {
                result += "\"Slot\":\"" + Slot + "\",";
            }
            if (null != Name)
            {
                result += "\"Name\":\"" + Name + "\",";
            }
            if (null != Event)
            {
                result += "\"Event\":\"" + Event + "\"";
            }
            result += "}";
            return result;
        }
    }
}