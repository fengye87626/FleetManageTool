using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FleetManageTool.WebAPI
{
    [Serializable]
	public class HalResult : IHalResult
	{
		public bool Success { get; set; }

        public string StatusCode { get; set; }
        //chenyangwen 20140612 #1830
        public string ReasonPhrase { get; set; }
	}
	public interface IHalResult
	{
		bool Success { get; set; }

        string StatusCode { get; set; }
        //chenyangwen 20140612 #1830
        string ReasonPhrase { get; set; }
	}
}
