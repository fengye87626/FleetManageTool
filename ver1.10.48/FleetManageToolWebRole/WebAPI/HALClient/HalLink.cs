using Newtonsoft.Json;
using System;

namespace FleetManageTool.WebAPI
{
    [Serializable]
	public class HalLink
	{
        [JsonProperty("Rel")]
		public string Rel { get; set; }

		[JsonProperty("href")]
		public string Href { get; set; }

		[JsonProperty("IsTemplated")]
		public bool IsTemplated { get; set; }

        [JsonProperty("Title")]
        public bool Title { get; set; }
	}
}