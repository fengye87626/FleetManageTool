using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManageTool.WebAPI;
using Newtonsoft.Json;

namespace FleetManageToolWebRole.Models.API
{
    [Serializable]
    public class Customer:HalResource
    {
        public enum CustomerLoginStatus { ACTIVE = 0, INACTIVE };

        public enum CustomerLanguage { zh, en };

        public enum CustomerMeasurementSystem { Metric };

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

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("LoginStatus")]
        public int LoginStatus { get; set; }

        [JsonProperty("DefaultLanguage")]
        public string DefaultLanguage { get; set; }

        [JsonProperty("DefaultMeasurementSystem")]
        public string DefaultMeasurementSystem { get; set; }

        [JsonProperty("UserAccessMask")]
        public string UserAccessMask { get; set; }
    }
}
