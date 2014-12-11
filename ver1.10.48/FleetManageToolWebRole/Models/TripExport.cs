using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    public class TripExport
    {
        private long _id;

        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private String _name;

        public String name
        {
            get { return _name; }
            set { _name = value; }
        }
        private String _date;

        public String date
        {
            get { return _date; }
            set { _date = value; }
        }
        private String _geofencestatus;

        public String geofencestatus
        {
            get { return _geofencestatus; }
            set { _geofencestatus = value; }
        }
        private String _distance;

        public String distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
}