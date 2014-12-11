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
    public class Device : HalResource
    {
        public enum DeviceTypeValue { Basic, Premium }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("DeviceType")]
        public DeviceType deviceType { get; set; }

        [JsonProperty("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("ByteId")]
        public string ByteId { get; set; }

        [JsonProperty("LabelId")]
        public string LabelId { get; set; }

        [JsonProperty("LabelIdType")]
        public string LabelIdType { get; set; }

        [JsonProperty("Properties")]
        public List<object> Properties { get; set; }

        [JsonProperty("Capabilities")]
        public List<object> Capabilities { get; set; }

        [JsonProperty("Esn")]
        public string Esn { get; set; }

        [JsonProperty("VehicleId")]
        public string VehicleId { get; set; }

        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }

        [JsonProperty("LastConnection")]
        public DateTime LastConnection { get; set; }
    }
}
