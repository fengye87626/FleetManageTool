//[DELETE-Start] ABCSoft-yueqq 2014-03-06:GeoFence需求变更删除SetLocation画面

//$(document).ready(function () {

//    /***Map显示，GeoFence标注***/
//    var BMapObj = null; //Baidu_Map对象
//    var Radius = 0;     //GeoFence的半径大小
//    var GeoFenceID = -1;    //GeoFenceId
//    var GeoName = "";
//    var GeoLocation = "";
//    //取得CompanyID
//    var CompanyID = GetCompanyID();

//    //取得GeoFenceInfo
//    $.ajax({
//        type: "POST",
//        url: "/" + CompanyID + "/GeoFence/getSelectGeoFenceInfo",
//        contentType: "application/x-www-form-urlencoded",
//        dataType: "json",
//        success: function (msg) {

//            var CenterPoint = { lng: msg.geofence.Baidulng, lat: msg.geofence.Baidulat };
//            GeoFenceID = msg.geofence.pkid;
//            Radius = msg.geofence.radiu;
//            GeoName = msg.geofence.name;
//            GeoLocation = msg.geofence.location;
//            if (GeoName) {
//                GeoName = $.trim(GeoName);
//            } else {
//                GeoName = "";
//            }

//            var ihpleD_map = new ihpleD_Map("geofenceSetLocation_BMap", CenterPoint.lng, CenterPoint.lat, 11, 1, 1, 1);
//            BMapObj = ihpleD_map.get_mapObj();
//            BMapObj.setNavControlOffSet(10, 62);
//        }
//    });

//    //跳转到EditShape画面，进行GeoFence大小的编辑
//    $("#geofenceSetLocation_SetLocation").click(function () {
//        trans();
//        var mapInfoJson = BMapObj.getMapInfo();
//        var obj = eval("(" + mapInfoJson + ")");
//        var Lng = obj.center.lng;
//        var Lat = obj.center.lat;
//        var Zoom = obj.zoomLevel;
//        var nowUrl = location.href;// "http://localhost:1180/GeoFence/SetLocation?SelectGeoFenceID=%200xFFFF"
//        var NextURL = nowUrl.substring(0, nowUrl.lastIndexOf("/"));

//        if (GeoFenceID == (-1)) {
//            NextURL += "/EditShape?EditGeoFenceID=" + GeoFenceID + "&centerlng=" + Lng + "&centerlat=" + Lat + "&zoom=" + Zoom + "&radiu=" + Radius + "&EditGeoFenceName=\"" + GeoName + "\"";
//            location.href = NextURL;
//        } else {
//            NextURL += "/EditShape?EditGeoFenceID=" + GeoFenceID + "&centerlng=" + Lng + "&centerlat=" + Lat + "&zoom=" + Zoom + "&radiu=" + Radius + "&EditGeoFenceName=\"" + GeoName + "\"" + "&EditGeoFenceLocation=\"" + $.trim(GeoLocation) + "\"";;
//            location.href = NextURL;
//        }
//    });

//    //返回到前一画面Landing.cshtml
//    $("#geofenceSetLocation_Cancel").click(function () {
//        window.history.back();
//    });
//});
//[DELETE-End] ABCSoft-yueqq 2014-03-06:GeoFence需求变更删除SetLocation画面
