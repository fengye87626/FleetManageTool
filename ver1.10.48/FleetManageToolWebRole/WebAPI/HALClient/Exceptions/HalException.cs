using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FleetManageTool.WebAPI.Exceptions
{
	class HalException : System.Exception
	{
        //chenyangwen 20140612 #1830
        public string ReasonPhrase { get; set; }

		public HalException(string message) : base(message) {}
	}
}
