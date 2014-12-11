using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page.MapSearch
{
    [Serializable]
    public class MapAreaSuggest
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<SuggestInfo> result { get; set; }
    }
}