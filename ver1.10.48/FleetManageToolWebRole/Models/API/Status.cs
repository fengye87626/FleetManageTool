using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Status
    {
        [JsonProperty("Temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("BatteryVoltage")]
        public double? BatteryVoltage { get; set; }

        [JsonProperty("EngineOn")]
        public bool EngineOn { get; set; }

        [JsonProperty("FuelLevel")]
        public double? FuelLevel { get; set; }

        [JsonProperty("MilAlertStatus")]
        public bool? MILAlertOn { get; set; }

        [JsonProperty("Odometer")]
        public double? Odometer { get; set; }

        [JsonProperty("EngineLightStatus")]
        public string EngineLightStatus { get; set; }

        [JsonProperty("Speed")]
        public int? Speed { get; set; }
    }
}