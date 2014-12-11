using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page.MapSearch
{
    [Serializable]
    public class SuggestInfo
    {
        public string name { get; set; }

        public string city { get; set; }

        public string district { get; set; }

        public string business { get; set; }

        public string cityid { get; set; }
    }
}