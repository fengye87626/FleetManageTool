using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Filters;
using FleetManageToolWebRole.BusinessLayer;
using FleetManageTool.Models.page;
//caoyandong-Operating
using FleetManageTool.Models.Common;
using FleetManageToolWebRole.Util;
using FleetManageToolWebRole.Models.Common;
using System.Threading;

namespace FleetManageToolWebRole.Controllers
{
    [SessionFilter]
    [ReuqestFilter]
    public class GeoFenceController : Controller
    {
        //chenyangwen 2014/02/12
        //获取电子围栏的车辆
        //mabiao 20140308 封装
        [LogFilter]
        public JsonResult GetTenantVehiclesByCompannyID(long geofenceID)
        {
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            GeofenceInfo results = geofenceInterface.GetTenantVehiclesByCompannyID(Session["companyID"].ToString(), geofenceID);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [LogFilter]
        //获取geofence画面需要的的数据
        public ActionResult Landing()
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            ViewBag.GeofenceName = "东北一家人 乐呵火锅";
            ViewBag.GeofenceLocation = "北京市 青年大街 热闹路 0707007号";
            ViewBag.GeofenceVehicles = "路虎 陆地战舰, 劳斯莱斯 豪华版, 劳斯莱斯 典藏版, Blue Honda Element, Van 4";
            ViewBag.GeofenceStatus = "激活";
            ViewBag.lat = Session["latForSearch"];
            ViewBag.lng = Session["lngForSearch"];
            ViewBag.strSelect = Session["strSelectForSearch"];
            ViewBag.zoom = Session["zoomForSearch"];
            ViewBag.showType = Session["showTypeForSearch"];
            CleanSearchPara();
            ViewBag.companyID = "ABCSoft";
            DateTime now = DateTime.Now;

            ViewBag.Update_Time = now.ToString("yyyy年MM月dd日 HH:mm");//显示当前时间
            //DB操作
            //TO DO
            //... ...
            return View();
        }

        [LogFilter]
        //获取SetLocation画面需要的的数据
        [RoleFilter]
        public ActionResult SetLocation(long SelectGeoFenceID)
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            Session["SelectGeoFenceID"] = SelectGeoFenceID;

            DateTime now = DateTime.Now;
            ViewBag.Update_Time = now.ToString("yyyy年MM月dd日 HH:mm");//显示当前时间
            //DB操作
            //TO DO
            //... ...
            return View();
        }

        //获取EditShapes画面需要的的数据
        [RoleFilter]
		[LogFilter]
        public ActionResult EditShape(/*long EditGeoFenceID, double centerlng, double centerlat, int zoom, double radiu, string EditGeoFenceName, string EditGeoFenceLocation*/)
        {
            //mabiao js多语言
            Language language = new Language();
            string language_string = language.LanguageScript();
            ViewBag.Language = language_string;
            //Response.Write("<script>var language_string = " + language_string + " LanguageScript = eval(language_string)[0].Shared</script>");

            //chenyangwen 2014/3/8
            CacheConfig.CacheSetting(Response);
            //chenyangwen 2014/3/8
            /*Session["EditGeoFenceID"] = EditGeoFenceID;
            Session["EditGeoFenceCenterlng"] = centerlng;
            Session["EditGeoFenceCenterlat"] = centerlat;
            Session["EditGeoFenceZoom"] = zoom;
            Session["EditGeoFenceName"] = EditGeoFenceName;
            Session["EditGeoFenceRadiu"] = radiu;
            Session["EditGeoFenceLocation"] = EditGeoFenceLocation;*/

            DateTime now = DateTime.Now;
            ViewBag.Update_Time = now.ToString("yyyy年MM月dd日 HH:mm");//显示当前时间
            //DB操作
            //TO DO
            //... ...
            return View();
        }


        /*Yueqingqing*/
        //获取GeoFencesInfo
        //chenyangwen 2014/02/12
        //mabiao 20140308 封装
        [LogFilter]
        public JsonResult GetGeofencesInfo(long group_id)
        {
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            List<GeofenceInfo> results = geofenceInterface.GetGeofencesInfo(Session["companyID"].ToString(), group_id);

            //将电子围栏按名称排序。
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
            results.Sort((x, y) => x.geofence.name.Trim().CompareTo(y.geofence.name.Trim()));
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        //请求删除GeoFenceID中的VehicleNo
        //chenyangwen 2014/02/12
        //mabiao 20140308 封装
        [RoleFilter]
        [LogFilter]
        public bool DeleteGeoVehicle(long geofenceID, long vehicleID)
        {
                //caoyandong-Operating
                OperatorLog.log(OperateType.DEL, "DeleteGeoVehicle", Session["companyID"].ToString());
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            bool results = geofenceInterface.DeleteGeoVehicle(Session["companyID"].ToString(), geofenceID, vehicleID);
            return results;
        }

        //请求(停用)Deactive GeoFenceID
        //chenyangwen 2014/02/12
        //mabiao 20140308 封装
        [RoleFilter]
        [LogFilter]
        public bool InactiveOrActiveGeo(long geofenceID,string status)
        {
                //caoyandong-Operating
                OperatorLog.log(OperateType.DEACTE, "DeactiveGeo", Session["companyID"].ToString());
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            bool results = geofenceInterface.InactiveOrActiveGeo(Session[StringConst.companyID].ToString(), geofenceID, status);
            return results;
        }

        //请求(删除)Activate GeoFenceID
        //chenyangwen 2014/02/12
        //mabiao 20140308 封装
        [RoleFilter]
        [LogFilter]
        public bool DeleteGeo(long geofenceID)
        {
            //caoyandong-Operating
            OperatorLog.log(OperateType.DEL, "DeleteGeo", Session["companyID"].ToString());

            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            bool results = geofenceInterface.DeleteGeo(Session["companyID"].ToString(), geofenceID);
            return results;
        }

        //请求根据车辆的id取得已经绑定的Geo的数量
        //mabiao 20140308 封装
        [RoleFilter]
        [LogFilter]
        public long GetGeofenceNumByVehicleID(long vehicleID)
        {
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            long results = geofenceInterface.GetGeofenceNumByVehicleID(Session["companyID"].ToString(), vehicleID);
            return results;
        }

        [RoleFilter]
        [LogFilter]
        //通过GeoFenceID取得GeoFence信息
        public JsonResult GetGeoFenceInfoByGeoFenceId(long geofenceID)
        {
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            GeofenceInfo results = geofenceInterface.GetGeoFenceInfoByGeoFenceId(Session[StringConst.companyID].ToString(), geofenceID);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        
        //Geofence Landing页面 迁移到EditShape画面
        [RoleFilter]
        public void SetEditGeoFenceInfo(string EditGeoFenceID, string centerlng, string centerlat, string zoom, string EditGeoFenceName, string radius, string EditGeoFenceLocation)
        {
            Session["EditGeoFenceID"] = EditGeoFenceID;
            Session["EditGeoFenceCenterlng"] = centerlng;
            Session["EditGeoFenceCenterlat"] = centerlat;
            Session["EditGeoFenceZoom"] = zoom;
            Session["EditGeoFenceName"] = EditGeoFenceName;
            Session["EditGeoFenceRadiu"] = radius;
            Session["EditGeoFenceLocation"] = EditGeoFenceLocation;
        }

        //取得EditShap画面所需要的数据：设定的中心点的经纬度、被编辑的GeoFenceID
        [RoleFilter]
        [LogFilter]
        public string GetEditGeoFenceInfo()
        {
            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();

            int EditGeoFenceZoom = int.Parse(Session["EditGeoFenceZoom"].ToString());
            Geofence geofence = new Geofence();
            geofence.pkid = long.Parse(Session["EditGeoFenceID"].ToString());
            geofence.Baidulng = double.Parse(Session["EditGeoFenceCenterlng"].ToString());
            geofence.Baidulat = double.Parse(Session["EditGeoFenceCenterlat"].ToString());
            geofence.name = Session["EditGeoFenceName"].ToString();
            geofence.radiu = double.Parse(Session["EditGeoFenceRadiu"].ToString());
            geofence.location = Session["EditGeoFenceLocation"].ToString();

            string results = geofenceInterface.GetEditGeoFenceInfo(geofence, EditGeoFenceZoom);
            return results; 
        }


        //chenyangwen 2014/02/12
        //编辑电子围栏
        //mabiao 20140308 封装
        [RoleFilter]
        [LogFilter]
        public string  UpdateGeofence(long geofenceID, float Baidulat, float Baidulng, string location, string name, float radius, string vehicleIDstr, string oldVehicleIDs)
        {
            string result = "OK";
            //caoyandong-Operating
            OperatorLog.log(OperateType.EDIT, "UpdateGeofence", Session["companyID"].ToString());

            GeofenceApiInterface geofenceInterface = new GeofenceApiInterface();
            Geofence newGeofence = new Geofence();
            newGeofence.Baidulat = Baidulat;
            newGeofence.Baidulng = Baidulng;
            newGeofence.location = location;
            newGeofence.name = name;
            newGeofence.radiu = radius;
            newGeofence.status = StringConst.Active;
            newGeofence.pkid = geofenceID;
            result = geofenceInterface.UpdateGeofence(Session[StringConst.companyID].ToString(), newGeofence, vehicleIDstr, oldVehicleIDs);
            return result;
        }

        [LogFilter]
        public void SearchPara(String zoom, String showType, String lat, String lng, String strSelect)
        {
            Session["zoomForSearch"] = zoom;
            Session["showTypeForSearch"] = showType;
            Session["latForSearch"] = lat;
            Session["lngForSearch"] = lng;
            Session["strSelectForSearch"] = strSelect;
            return;
        }
        [LogFilter]
        public void CleanSearchPara()
        {
            Session["zoomForSearch"] = null;
            Session["showTypeForSearch"] = null;
            Session["latForSearch"] = null;
            Session["lngForSearch"] = null;
            Session["strSelectForSearch"] = null;
            return;
        }
    }
}
