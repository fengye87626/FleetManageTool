using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using FleetManageTool.WebAPI.Exceptions;
using FleetManageTool.WebAPI.JSON;
using Newtonsoft.Json;

namespace FleetManageTool.WebAPI.Persister
{
	public class BasicHalPersisterStrategy : IHalPersisterStrategy
	{
		public IHalPersistResult<T> Persist<T>(T resource, HalLink link = null) where T : IHalResource
		{
			link = link ?? resource.Links.FirstOrDefault(l => l.Rel == "self");
			if (link == null) {
				throw new HalPersisterException("No link found for persisting: " + resource);
			}
			var href = new Uri(link.Href, UriKind.Relative);
			if (link.IsTemplated) {
				href = HalClient.ResolveTemplate(link, new Dictionary<string, object>());
			}
			var json = JsonConvert.SerializeObject(resource);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var result = 
				resource.IsNew ? HttpClient.PostAsync(href, content).Result : HttpClient.PutAsync(href, content).Result;
			var ret = new HalPersistResult<T> {Success = result.IsSuccessStatusCode, HttpStatusCode = (int) result.StatusCode};
			if (result.IsSuccessStatusCode) {
				var settings = new JsonSerializerSettings {Converters = new List<JsonConverter>() {new HalResourceConverter(resource.GetType())}};
				ret.Resource = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result, settings);
			}
			return ret;
		}

		public bool CanPersist(Type type)
		{
			return type.IsSubclassOf(typeof (HalResource));
		}

		public HalClient HalClient { get; set; }

		public HttpClient HttpClient { get; set; }
        public IHalResult Delete(IHalResource resource)
		{
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");
			var link = resource.Links.FirstOrDefault(l => l.Rel == "self");
			if (link == null)
				throw new HalPersisterException("No link found for deleting: " + resource);
			var result = HttpClient.DeleteAsync(link.Href).Result;
            return new HalResult { Success = result.IsSuccessStatusCode };
		}
	}
}
