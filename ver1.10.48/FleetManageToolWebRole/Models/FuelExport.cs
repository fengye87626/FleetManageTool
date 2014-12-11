using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    public class FuelExport
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
        private String _miles;

        public String miles
        {
            get { return _miles; }
            set { _miles = value; }
        }
        private String _gallon;

        public String gallon
        {
            get { return _gallon; }
            set { _gallon = value; }
        }
        private String _estimated;

        public String estimated
        {
            get { return _estimated; }
            set { _estimated = value; }
        }
        private String _cost;

        public String cost
        {
            get { return _cost; }
            set { _cost = value; }
        }
    }
}