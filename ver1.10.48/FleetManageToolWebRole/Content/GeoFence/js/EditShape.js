/**Yueqingqing**/
//Baidu_Map对象
var BMapObj = null;
//被编辑的GeoFence对象
var Edit_Geofence = null; 
//保存GeoFence被编辑前的中心点信息
var S_CenterPoint = { lng: 0, lat: 0 };

//GeoFence编辑画面，支持地点检索功能（使用autocomplete控件）
var mapPointList = [{ title: "", address: "", lat: 0, lng: 0 }];

$.ajaxSetup({
    statusCode: {
        499: function (data) {
            window.location.reload();
        }
        , 599: function (data) {
            alert("用户权限已被修改，请重新登录");
            window.location.href = "/";
        }, 699: function (data) {
            alert(LanguageScript.page_common_tenant_inactive);
            window.location.href = "/";
        }
    }
});
$(document).ready(function () {

    HrefFlag = 1;

    /***Map显示，GeoFence标注***/
    var mapObj = null;  //map对象

    var GeoFenceID =(-1); //新规GeoFence时，GeoFence默认ID为-1
    var Radius = 0;
    var GeoFenceName = "";
    var GeoFenceLocation = "";
    var CompanyID = GetCompanyID();
    var Zoom = 11;

    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/GeoFence/getEditGeoFenceInfo",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (obj) {

            //取得将要被编辑的GeoFence信息：
            S_CenterPoint.lng = obj.GeoFencelng;
            S_CenterPoint.lat = obj.GeoFencelat;
            GeoFenceID = obj.GeoFenceID;
            Radius = obj.GeoFenceradius;
            Zoom = obj.zoom;
            GeoFenceName = obj.GeoFenceName;
            GeoFenceLocation = obj.location;

            //以编辑的GeoFence信息的中心点为中心，显示地图
            var ihpleD_map = new ihpleD_Map("geofenceEditShapes_BMap", S_CenterPoint.lng, S_CenterPoint.lat, Zoom, 1, 1, 1);
            BMapObj = ihpleD_map.get_mapObj();
            mapObj = BMapObj.get_mapObj();

            //Navigation控件偏移量设定
            BMapObj.setNavControlOffSet(10, 62);

            //创建一个可编辑的GeoFence对象
            //(mapObj, longitude, latitude, radius, geoId, viewMode, name, infoBox_content)
            Edit_Geofence = new GeoFence(mapObj, S_CenterPoint.lng, S_CenterPoint.lat, Radius,
                            GeoFenceID, Settings.viewModeEditGeoFence, GeoFenceName, "");

            //通过经纬度进行反地址解析
            var LocationInfo = GeoShowAddress(S_CenterPoint.lng, S_CenterPoint.lat, "geofenceEditShapes_Location");
        }
    });

    //GeoFence编辑画面，支持地点检索功能（使用autocomplete控件）
    mapPointList = [{ title: "", address: "", lat: 0, lng: 0 }];
    $("#searched_input_drug").autocomplete({
        source: function (request, response) {

            //Location信息检索结束后的回调函数
            function receiveSearchResult(results) {
                // 判断检索状态是否正确
                if (Search.getStatus() == BMAP_STATUS_SUCCESS)
                {
                    mapPointList.length = 0;
                    for (var i = 0; i < results.getCurrentNumPois() ; i++)
                    {
                        if (results.getPoi(i).point.lat != null || results.getPoi(i).point.lng != null ||
                            (results.getPoi(i).point.lat != 0 && results.getPoi(i).point.lng != 0))
                        {
                            var mapPoint = { title: "", address: "", lat: 0, lng: 0 };
                            mapPoint.title = results.getPoi(i).title;
                            mapPoint.address = results.getPoi(i).address;
                            mapPoint.lng = results.getPoi(i).point.lng;
                            mapPoint.lat = results.getPoi(i).point.lat;
                            mapPointList.push(mapPoint);
                        }
                    }
                    response($.map(mapPointList, function (item) {
                        $("#location_search_warning").hide();
                        return {
                            label: item.title + "," + item.address + "",
                            value: item.title + "," + item.address + ""
                        }
                    }));
                }
                else
                {
                    response($.map("", function (item) {
                        $("#location_search_warning").hide();
                        return {
                            label: "",
                            value: ""
                        }
                    }));
                    mapPointList.length = 0;
                }
            }

            var opts = { onSearchComplete: receiveSearchResult };
            var Search = new BMap.LocalSearch("全国", opts);
            Search.search(request.term);

        },
        //select方法
        select: function (e, ui) {
            var selectVal = ui.item.value;

            //设定检索的结果
            setSearchLocation($.trim(selectVal));
        }
    });

    //文本框的回车事件:
    $("#searched_input_drug").keypress(function (e) {
        var curKey = e.which;
        if (curKey == 13)
        {
            $("#location_search_warning").hide();
            setTimeout(function () {
                $("#location_search_warning").hide();
            }, 4000);

            var keyValue = $("#searched_input_drug").val();
            setSearchLocation($.trim(keyValue),true);
        }
        else {
            $("#location_search_warning").hide();
        }
    });

    $("#searched_input_drug").mousedown(function (e) {
        $("#location_search_warning").hide();
    });
    $("#searched_input_drug").blur(function (e) {
        $("#location_search_warning").hide();
    });

    //搜索图片的点击事件
    $("#location_seach_input_icon").click(function () {
        var inputcontent = $("#searched_input_drug").val();
        setSearchLocation($.trim(inputcontent),true);
    })

    //跳转到Save画面，进行GeoFence信息的保存和详细设定
    $("#geofenceEditShapes_SaveShape").click(function () {

        //判断GeoFence是否重合（中心点距离50米以内，半径相差1米以内：认为GeoFence重合）
        checkGeofence(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation);
    });

    //返回到前一画面Landing.cshtml
    $("#geofenceEditShapes_Back").click(function () {
        window.history.back();
    });

});
/**Yueqingqing**/

// 张博 start
function checkGeofence(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation) {
    //取得租户公司ID
    var CompanyID = GetCompanyID();

    //新加电子围栏
    if (GeoFenceID == (-1)) {
        $.ajax({
            type: "POST",
            url: "/" + CompanyID + "/GeoFence/GetGeofencesInfo",
            data: { group_id: (-1) },
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            success: function (data) {
                if (data) {
                    var flag = false;
                    for (var i = 0; i < data.length; i++) {

                        //取得当前GeoFence的中心点和半径
                        var CenterPoint = Edit_Geofence.getCenter();
                        var radius = Edit_Geofence.getRadius();
                        //判断是否重合（中心点距离50米以内，半径相差1米以内：认为是重合）
                        if (Math.abs(data[i].geofence.Baidulat - CenterPoint.lat) < 0.001 && Math.abs(data[i].geofence.Baidulng - CenterPoint.lng) < 0.001 && Math.abs(data[i].geofence.radiu - radius) < 1) {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true) {
                        geofence_dialog_error(LanguageScript.error_e01239);
                    } else {
                        toSaveGEO(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation);
                    }
                } else {
                    toSaveGEO(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation);
                }
            }
        });
    } else {
        //编辑电子围栏
        toSaveGEO(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation);
    }
}
// 张博 end

function toSaveGEO(Edit_Geofence, GeoFenceID, GeoFenceName, GeoFenceLocation) {

    //取得保存时，GeoFence的最新信息（中心点、半径）
    var CenterPoint = Edit_Geofence.getCenter();
    var radius = Edit_Geofence.getRadius();

    var location = "";

    //新规GeoFence时，Location信息为百度地图解析的中心点地址
    if ("" == GeoFenceLocation || (GeoFenceLocation==undefined)) {
        location = $("#geofenceEditShapes_Location").text();
    } else {

        //编辑存在的GeoFence时：
        //当中心点位置变更时，Location信息使用百度地图解析的新的中心点的地址
        //判断出中心点没有变更时，Location信息使用编辑前的数据库中保存的Location信息
        if ((Math.abs(S_CenterPoint.lat - CenterPoint.lat) < 0.001) && (Math.abs(S_CenterPoint.lng - CenterPoint.lng) < 0.001)) {
            location = GeoFenceLocation;
        }
        else {
            location = $("#geofenceEditShapes_Location").text();
        }
    }
    //新围栏
    if (GeoFenceID == -1) {

        //围栏ID 名称 中心经度  中心纬度 半径 地址 保存类型选项（string:type 选项有"new","edit","addcar"）
        SaveGeoByPopup(GeoFenceID, "", CenterPoint.lng, CenterPoint.lat, radius, location, "new");
    } else {
        //GeoFence代码重构：
        //围栏ID 名称 中心经度  中心纬度 半径 地址 保存类型选项（string:type 选项有"new","edit","addcar"）
        SaveGeoByPopup(GeoFenceID, GeoFenceName, CenterPoint.lng, CenterPoint.lat, radius, location, "edit");
    }
}


//wenti
//dialog 围栏重合
function geofence_dialog_error(text) {
    $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".user_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            position: ['center', 250],
            zIndex: 1110,
            modal: true,

            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                    ihpleDSaveGeofenceFlag = true;
                }
            }
        });
    });
}

//点击搜索按钮后或者选择下拉列表内容后执行的业务逻辑
function setSearchLocation(select,clickFlag) {
    select = select.replace(/[^a-zA-Z0-9\_\-\s\,\.\u4e00-\u9fa5]/g, '');
    //判断检索字符的有效性
    if (null == select || $.trim(select) == "")
    {
        //搜索字符无效，提示用户
        $("#location_search_warning").attr("value", LanguageScript.error_e01226);
        setTimeout(function () {
            $("#location_search_warning").show();
        }, 100);
        return;
    }

    var selectAll = select;
    var selectSplit = selectAll.split(",");
    var selectVal = selectSplit[0];

    var flag = false;//记录是否已经有检索结果
    //查找检索结果列表
    for (var i = 0; i < mapPointList.length; i++)
    {
        var point = mapPointList[i];
        var nameaddr = point.title + "," + point.address + "";
        if (selectAll == nameaddr)
        {
            flag = true;
            //设定地图中心和被编辑的GeoFence的中心
            setGeofenceCenterBySearch(point.lng, point.lat);
            break;
        }
    }

    //检索列表存在匹配内容时，直接返回
    if (flag) {
        return;
    } else {
        //向百度再次请求检索,并把检索地点作为GeoFence中心点
        req_LocalSearch(select,clickFlag);
    }
}

//请求本地检索,并把检索地点作为GeoFence中心点
function req_LocalSearch(search_val,clickFlag) {

    var searchAll = search_val;
    var searchSplit = searchAll.split(",");
    var searchVal = "";
    if (clickFlag) {
        searchVal = search_val;
    } else {
        searchVal = searchSplit[0];
    }

    //Location信息检索结束后的回调函数
    function getresult(results) {

        // 判断检索状态是否正确:错误时，提示用户：未检索到匹配内容，请提供更详尽的检索条件
        if (search.getStatus() == BMAP_STATUS_SUCCESS) {

            var flag = false;
            //Search name and address match.
            for (var i = 0; i < results.getCurrentNumPois() ; i++) {
                var nameaddress = results.getPoi(i).title;
                if (searchAll == nameaddress) {
                    if (results.getPoi(i).point.lat != null || results.getPoi(i).point.lng != null ||
                        (results.getPoi(i).point.lat != 0 && results.getPoi(i).point.lng != 0)) {
                        var longitude = results.getPoi(i).point.lng;
                        var latitude = results.getPoi(i).point.lat;

                        flag = true;

                        //设定地图中心和被编辑的GeoFence的中心
                        setGeofenceCenterBySearch(longitude, latitude);
                        break;
                    }
                }
            }
            if (!flag) {
                //Search name match.
                for (var i = 0; i < results.getCurrentNumPois() ; i++) {
                    if (results.getPoi(i).point.lat != null || results.getPoi(i).point.lng != null ||
                            (results.getPoi(i).point.lat != 0 && results.getPoi(i).point.lng != 0)) {
                        var longitude = results.getPoi(i).point.lng;
                        var latitude = results.getPoi(i).point.lat;

                        flag = true;

                        //设定地图中心和被编辑的GeoFence的中心
                        setGeofenceCenterBySearch(longitude, latitude);
                        break;
                    }
                }
                if (!flag) {
                    $("#location_search_warning").attr("value", LanguageScript.error_e01226);
                    setTimeout(function () {
                        $("#location_search_warning").show();
                    }, 100);
                }
            }
        } else {
            
            $("#location_search_warning").attr("value", LanguageScript.error_e01240);
            setTimeout(function () {
                $("#location_search_warning").show();
            }, 100);
        }
    }

    var opts = { onSearchComplete: getresult };
    var search = new BMap.LocalSearch("全国", opts);
    search.search(searchVal);
    $("#searched_input_drug").autocomplete("close")
}

//设定编辑中的GeoFence的中心点
function setGeofenceCenterBySearch(lng, lat) {

    if ((0 == lng) && (0 == lat)) {
        return;
    }

    //取得MapObject
    var mapObj = BMapObj.get_mapObj();
    mapObj.setCenter(new BMap.Point(lng, lat));	//设置地图中心点

    //移动GeoFence的中心地点
    Edit_Geofence.moveCirle_EditMode(lng, lat);
}