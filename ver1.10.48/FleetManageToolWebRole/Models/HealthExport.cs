using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    public class HealthExport
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
        private String _warningtype;

        public String warningtype
        {
            get { return _warningtype; }
            set { _warningtype = value; }
        }
        private String _warninginfo;

        public String warninginfo
        {
            get { return _warninginfo; }
            set { _warninginfo = value; }
        }
    }
}