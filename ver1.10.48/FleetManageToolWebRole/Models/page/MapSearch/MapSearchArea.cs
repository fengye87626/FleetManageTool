using FleetManageTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models.page.MapSearch
{
    [Serializable]
    public class MapSearchArea
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<MapSearchContent> results { get; set; }
    }
}