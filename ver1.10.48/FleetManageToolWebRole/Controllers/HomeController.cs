using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Filters;
using Newtonsoft.Json;
using FleetManageTool.Models.page;
using FleetManageToolWebRole.BusinessLayer;
using System.Text.RegularExpressions;

namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class WebProxy_Interface : IWebProxy
    {

        // The credentials to be used with the web proxy.
        private ICredentials iCredentials;

        // Uri of the associated proxy server.
        private Uri webProxyUri;

        public WebProxy_Interface(Uri proxyUri)
        {

            webProxyUri = proxyUri;

        }

        // Get and Set the Credentials property.
        public ICredentials Credentials
        {
            get
            {
                return iCredentials;
            }
            set
            {
                if (iCredentials != value)
                    iCredentials = value;
            }
        }

        // Return the web proxy for the specified destination(destUri).
        public Uri GetProxy(Uri destUri)
        {

            // Always use the same proxy.
            return webProxyUri;

        }

        // Return whether the web proxy should be bypassed for the specified destination(hostUri).
        public bool IsBypassed(Uri hostUri)
        {

            // Never bypass the proxy.
            return false;

        }
    };

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int TestNUnit(int a, int b)
        {
            return a + b;
        }

        public JsonResult GetGeoFenceDrugList() 
        {

            List<String> output = new List<string>();
            string input = Request.QueryString["key"];
            if (null == input || input.Equals(""))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
            List<Geofence> geofencesList = new List<Geofence>();
            String companyid = Session["companyID"].ToString();
            geofencesList = geofenceInterface.GetGeofences(companyid);

            foreach (Geofence i in geofencesList)
            {
                if (i.name.Trim().ToLower().Contains(input.ToLower()))
                {
                    output.Add(i.name.Trim());
                }
            }
           return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehiclesDrugList()
        {
            try
            {
                List<String> output = new List<string>();
                string input = Request.QueryString["key"];
                if (null == input || input.Equals(""))
                {
                    return Json(output, JsonRequestBehavior.AllowGet);
                }

            VehicleDBInterface db = new VehicleDBInterface();
            List<Vehicle> vehicleList = new List<Vehicle>();
            String companyid = Session["companyID"].ToString();
            vehicleList = db.GetTenantVehiclesByCompannyID(companyid);
            List<Vehicle> vehicleResutltList = new List<Vehicle>();

            foreach (Vehicle i in vehicleList)
            {
                if (null != i.licence && i.licence.Length > 0)
                {
                    if (i.name.Trim().ToLower().Contains(input.ToLower()) || i.licence.Trim().ToLower().Contains(input.ToLower()))
                    {
                        vehicleResutltList.Add(i);
                    }
                }
                else 
                {
                    if (i.name.Trim().ToLower().Contains(input.ToLower()))
                    {
                        vehicleResutltList.Add(i);
                    }
                }
                
            }
            //vehicleResutltList 排序,名字，车牌号，Vin
                vehicleList.Sort((x, y) => x.vin != null ? x.vin.CompareTo(y.vin != null ? y.vin : "") : "".CompareTo(y.vin != null ? y.vin : ""));
                vehicleList.Sort((x, y) => x.licence != null ? x.licence.CompareTo(y.licence != null ? y.licence : "") : "".CompareTo(y.licence != null ? y.licence : ""));
                vehicleList.Sort((x, y) => x.name != null ? x.name.CompareTo(y.name != null ? y.name : "") : "".CompareTo(y.name != null ? y.name : ""));
            return Json(vehicleResutltList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public JsonResult SearchVehicles()
        {
            try
            {
                string select = Request.QueryString["selectcontent"];
                FleetInfoFetcher fetcher = new FleetInfoFetcher();
                List<VehicleInfo> vehicleList = new List<VehicleInfo>();
                String companyid = Session["companyID"].ToString();
                vehicleList = fetcher.GetVehicleInfo(-1, companyid, 0).allVehicle;
                //vehicleResutltList 排序,名字，车牌号，Vin
                vehicleList.Sort((x, y) => x.vin != null ? x.vin.CompareTo(y.vin != null ? y.vin : "") : "".CompareTo(y.vin != null ? y.vin : ""));
                vehicleList.Sort((x, y) => x.license != null ? x.license.CompareTo(y.license != null ? y.license : "") : "".CompareTo(y.license != null ? y.license : ""));
                vehicleList.Sort((x, y) => x.name != null ? x.name.CompareTo(y.name != null ? y.name : "") : "".CompareTo(y.name != null ? y.name : ""));

                List<PointArea> gpsList = new List<PointArea>();
                PointArea gpsPoint = new PointArea();
                long id = 0;
                foreach (VehicleInfo i in vehicleList)
                {
                    if (null != i.license && i.license.Length > 0)
                    {
                        if (i.name.Trim().Equals(select) || i.license.Trim().Equals(select))
                        {
                            gpsPoint.lat = (double)i.location.latitude;
                            gpsPoint.lng = (double)i.location.longitude;
                            gpsList.Add(gpsPoint);
                            id = i.primarykey;
                            break;
                        }
                    }
                    else
                    {
                        if (i.name.Trim().Equals(select))
                        {
                            gpsPoint.lat = (double)i.location.latitude;
                            gpsPoint.lng = (double)i.location.longitude;
                            gpsList.Add(gpsPoint);
                            id = i.primarykey;
                            break;
                        }
                    }

                }
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("id", id + "");
                map.Add("gpsList", gpsList);
                return Json(map, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        //Add by LiYing start
        public JsonResult SearchVehiclesByID()
        {
            string select = Request.QueryString["selectcontent"];
            FleetInfoFetcher fetcher = new FleetInfoFetcher();
            List<VehicleInfo> vehicleList = new List<VehicleInfo>();
            String companyid = Session["companyID"].ToString();
            vehicleList = fetcher.GetVehicleInfo(-1, companyid, 0).allVehicle;

            List<PointArea> gpsList = new List<PointArea>();
            PointArea gpsPoint = new PointArea();

            if (Regex.IsMatch(select, @"^[+-]?\d*$"))
            {
                foreach (VehicleInfo i in vehicleList)
                {
                    if (i.primarykey == long.Parse(select))
                    {
                        gpsPoint.lat = (double)i.location.latitude;
                        gpsPoint.lng = (double)i.location.longitude;
                        gpsList.Add(gpsPoint);
                        break;
                    }
                }
            }
            else
            {
                foreach (VehicleInfo i in vehicleList)
                {
                    if (null != i.license && i.license.Length > 0)
                    {
                        if (i.name.Trim().Equals(select) || i.license.Trim().Equals(select))
                        {
                            gpsPoint.lat = (double)i.location.latitude;
                            gpsPoint.lng = (double)i.location.longitude;
                            gpsList.Add(gpsPoint);
                            break;
                        }
                    }
                    else
                    {
                        if (i.name.Trim().Equals(select))
                        {
                            gpsPoint.lat = (double)i.location.latitude;
                            gpsPoint.lng = (double)i.location.longitude;
                            gpsList.Add(gpsPoint);
                            break;
                        }
                    }
                    
                }
            }
            
            return Json(gpsList, JsonRequestBehavior.AllowGet);
        }
        //Add by LiYing end
        public JsonResult SearchGeoFence()
        {
            string select = Request.QueryString["selectcontent"];
            GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
            List<Geofence> geofencesList = new List<Geofence>();
            String companyid = Session["companyID"].ToString();
            geofencesList = geofenceInterface.GetGeofences(companyid);

            List<PointAreaForGeo> gpsList = new List<PointAreaForGeo>();
            PointAreaForGeo gpsPoint = new PointAreaForGeo();
            try
            {
                foreach (Geofence i in geofencesList)
                {
                    if (i.name.Trim().Equals(select))
                    {
                        //Get point GPS
                        //test data
                        gpsPoint.lat = (double)i.Baidulat;
                        gpsPoint.lng = (double)i.Baidulng;
                        gpsPoint.radius = (double)i.radiu;
                        gpsList.Add(gpsPoint);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            return Json(gpsList, JsonRequestBehavior.AllowGet);
        }

        private T searchAreaByMap<T>(String url)
        {
            try
            {
                if (url == null || url.Equals(""))
                {
                    throw new Exception("URL losed.");
                }

                HttpClientHandler aHandler = new HttpClientHandler();

                aHandler.UseCookies = true;
                aHandler.AllowAutoRedirect = true;
                IWebProxy proxy = new WebProxy_Interface(new Uri("http://proxy.ABCSoft.com:8080/"));
                proxy.Credentials = new NetworkCredential("wang-yong", "*****MyHope13");
                aHandler.Proxy = proxy;

                HttpClient client = new HttpClient(aHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception("Http request failure!");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<String> convertSearchStr(MapAreaSuggest searchObj)
        {
            List<String> convertList = new List<string>();
            List<SuggestInfo> contentList = new List<SuggestInfo>();

            contentList = searchObj.result;
            if (contentList.Count != 0)
            {
                foreach (SuggestInfo i in contentList)
                {
                    if (null == i.name)
                    {
                        continue;
                    }
                    else
                    {
                        String addressname = i.name;
                        convertList.Add(addressname);
                    }
                }
            }

            return convertList;
        }

        private MapSearchContent convertPoint(MapSearchArea areaObj)
        {
            List<MapSearchContent> contentList = new List<MapSearchContent>();
            MapSearchContent areaPoint = new MapSearchContent();

            contentList = areaObj.results;
            if (contentList.Count != 0)
            {
                foreach (MapSearchContent i in contentList)
                {
                    if (null != i.location)
                    {
                        areaPoint = i;
                        break;
                    }
                }
            }
            return areaPoint;
        }
    }
}
