using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class CustomerData
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("TenantId")]
        public string TenantId { get; set; }

        [JsonProperty("AccountId")]
        public string AccountId { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Devices")]
        public List<Device> Devices { get; set; }

        [JsonProperty("Vehicles")]
        public List<Vehicle> Vehicles { get; set; }
    }
}