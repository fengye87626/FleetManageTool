using FleetManageTool.WebAPI.JSON;
using Newtonsoft.Json;
using System;

namespace FleetManageTool.WebAPI
{
	public interface IHalResource
	{
		HalLinkCollection Links { get; set; }
		bool IsNew { get; set; }
	}

    [Serializable]
	public abstract class HalResource : IHalResource
	{
		private HalLinkCollection _links = new HalLinkCollection();

		[JsonIgnore]
		public HalLinkCollection Links
		{
			get { return _links; }
			set { _links = value; }
		}

		private bool _isNew = true;

		[JsonIgnore]
		public bool IsNew
		{
			get { return _isNew; }
			set { _isNew = value; }
		}
	}
}