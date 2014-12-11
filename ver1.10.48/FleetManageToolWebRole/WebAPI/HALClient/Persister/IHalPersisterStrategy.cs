using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FleetManageTool.WebAPI.Persister
{
	public interface IHalPersisterStrategy
	{
		IHalPersistResult<T> Persist<T>(T resource, HalLink link = null) where T : IHalResource;
		bool CanPersist(Type type);
		HalClient HalClient { get; set; }
		HttpClient HttpClient { get; set; }
        IHalResult Delete(IHalResource resource);
	}
}
