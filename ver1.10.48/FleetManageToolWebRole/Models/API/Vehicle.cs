using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using FleetManageTool.WebAPI;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Vehicle : HalResource
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("Vin")]
        public string Vin { get; set; }

        [JsonProperty("Year")]
        public string year { get; set; }

        [JsonProperty("Make")]
        public string Make { get; set; }

        [JsonProperty("FriendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("Status")]
        public Status status { get; set; }//API 返回的是NULL，所以对具体类型还不是很清楚，暂定为object

        [JsonProperty("VehicleLocation")]
        public VehicleLocation VehicleLocation { get; set; }

        [JsonProperty("ManualIdentification")]
        public bool ManualIdentification { get; set; }

        [JsonProperty("IsVinEditable")]
        public bool IsVinEditable { get; set; }

        [JsonProperty("IsMMYEditable")]
        public bool IsMMYEditable { get; set; }

        [JsonProperty("ValidateInputAttribute")]
        public string ValidateInputAttribute { get; set; }

        [JsonProperty("ConnectedDeviceId")]
        public string ConnectedDeviceId { get; set; }

        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }

        [JsonProperty("Heading")]
        public double Heading { get; set; }

        [JsonProperty("LastReadLocation")]
        public DateTime LastReadLocation { get; set; }

        [JsonProperty("UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [JsonProperty("Speed")]
        public int? Speed { get; set; }

        [JsonProperty("Odometer")]
        public double? Odometer { get; set; }

        [JsonProperty("GeoFences")]
        public List<Geofence> geoFences { get; set; }

        [JsonProperty("Trips")]
        public List<Trip> Trips { get; set; }

        [JsonProperty("DiagnosticCodes")]
        public List<DiagnosticCode> DiagnosticCodes { get; set; }

        [JsonProperty("AlertConfigurations")]
        public List<AlertConfiguration> AlertConfigurations { get; set; }

        [JsonProperty("Alerts")]
        public List<Alert> Alerts { get; set; }

        [JsonProperty("EngineLightStatus")]
        public String EngineLightStatus { get; set; }

        [JsonProperty("Description")]
        public String Description { get; set; }
        
    }
}