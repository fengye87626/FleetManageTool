using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Models
{
    public class PointArea
    {
        private double _lat;

        public double lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        private double _lng;

        public double lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
    }
    public class PointAreaForGeo
    {
        private double _lat;

        public double lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        private double _lng;

        public double lng
        {
            get { return _lng; }
            set { _lng = value; }
        }

        private double _radius;

        public double radius
        {
            get { return _radius; }
            set { _radius = value; }
        }
    }
    public class MapSearchContent
    {
        private String _name;

        public String name
        {
            get { return _name; }
            set { _name = value; }
        }
        private PointArea _location;

        public PointArea location
        {
            get { return _location; }
            set { _location = value; }
        }
        private String _address;

        public String address
        {
            get { return _address; }
            set { _address = value; }
        }
        private String _street_id;

        public String street_id
        {
            get { return _street_id; }
            set { _street_id = value; }
        }
        private String _uid;

        public String uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
    }

    public class MapSearchArea
    {
        private String _status;

        public String status
        {
            get { return _status; }
            set { _status = value; }
        }
        private String _message;

        public String message
        {
            get { return _message; }
            set { _message = value; }
        }
        private List<MapSearchContent> _results;

        public List<MapSearchContent> results
        {
            get { return _results; }
            set { _results = value; }
        }
    }

    public class SuggestInfo
    {
        private String _name;
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }
        private String _city;
        public String city
        {
            get { return _city; }
            set { _city = value; }
        }
        private String _district;
        public String district
        {
            get { return _district; }
            set { _district = value; }
        }
        private String _business;
        public String business
        {
            get { return _business; }
            set { _business = value; }
        }
        private String _cityid;
        public String cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
    }

    public class MapAreaSuggest
    {
        private String _status;
        public String status
        {
            get { return _status; }
            set { _status = value; }
        }
        private String _message;
        public String message
        {
            get { return _message; }
            set { _message = value; }
        }

        private List<SuggestInfo> _result;
        public List<SuggestInfo> result
        {
            get { return _result; }
            set { _result = value; }
        }
    }
}