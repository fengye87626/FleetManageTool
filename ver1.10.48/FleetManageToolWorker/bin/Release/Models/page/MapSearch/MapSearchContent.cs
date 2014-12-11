using FleetManageTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page.MapSearch
{
    [Serializable]
    public class MapSearchContent
    {
        public string name { get; set; }

        public LocationPoint location { get; set; }

        public string address { get; set; }

        public string street_id { get; set; }

        public string uid { get; set; }
    }
}