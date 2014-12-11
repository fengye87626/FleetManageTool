/**chenyangwen**/
var BMapObj = null;
$.ajaxSetup({
    statusCode: {
        499: function (data) {
            window.location.reload();
        }
        , 599: function (data) {
            alert(LanguageScript.page_common_Role_Change);
            window.location.href = "/";
        }, 699: function (data) {
            alert(LanguageScript.page_common_tenant_inactive);
            window.location.href = "/";
        },
        799: function (data) {
            alert(LanguageScript.page_common_tenant_deleted);
            window.location.href = "/";
        }
    }
});
$(document).ready(function () {

    getData();

    //GeoFence地图画面相关
    GeoFenceMap(-1);

    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();
    var companyID = GetCompanyID();
    if (1 != roleID) {
        SetBtnToneDown("geofence_addgeofence_bg");
    } else {
        $("#geofence_addgeofence_bg").click(function () {
            //画面迁移显示进度条
            trans();

            //"添加电子围栏"==》跳转到EditGeoFence画面
            trans_AddGeoFence();
        });
    }

    $("#GeoFenceInfo_Deactivate").click(
        function list_show() {
            $("#geofence_tablelist").show();
        }
    );

    $("#geofence_unchoosed_bg,#geofence_showlist_bg").click(
        function list_show() {
            $("#geofence_tablelist").show();
            $("#geofence_BMap")[0].style.zIndex = -20000;
            $("#geofence_choosed_bg_img").removeClass("geofence_choosed_bg_normal");
            $("#geofence_choosed_bg_img").addClass("geofence_unchoosed_bg_normal");
            $("#geofence_unchoosed_bg_img").removeClass("geofence_unchoosed_bg_normal");
            $("#geofence_unchoosed_bg_img").addClass("geofence_choosed_bg_normal");
            $("#geofence_unchoosed_bg").removeClass("cursor_style");
            $("#geofence_showlist_bg").removeClass("cursor_style");
            $("#geofence_choosed_bg").addClass("cursor_style");
            $("#geofence_showmap_bg").addClass("cursor_style");
            GeoHeightList();
        }
    );
    $("#geofence_choosed_bg,#geofence_showmap_bg").click(
        function list_show() {
            $("#geofence_tablelist").hide();
            $("#geofence_BMap")[0].style.zIndex = "";
            $("#geofence_choosed_bg_img").removeClass("geofence_unchoosed_bg_normal");
            $("#geofence_choosed_bg_img").addClass("geofence_choosed_bg_normal");
            $("#geofence_unchoosed_bg_img").removeClass("geofence_choosed_bg_normal");
            $("#geofence_unchoosed_bg_img").addClass("geofence_unchoosed_bg_normal");
            $("#geofence_choosed_bg").removeClass("cursor_style");
            $("#geofence_showmap_bg").removeClass("cursor_style");
            $("#geofence_unchoosed_bg").addClass("cursor_style");
            $("#geofence_showlist_bg").addClass("cursor_style");
            SetLeftBarHeight(825);
        }

    );
    //fengpan 20140617 初始化下拉框插件
    $('.select_group').selectpicker();
    $("#select_group").change(function () {
        //地图刷新,且进行最佳视图调整
        var groupID = $("#select_group").val();
        updateGeoFencesByGroup(groupID, true);
    });
});
//mabiao for height GeoFence List
function GeoHeightList() {
    if ($("#geofence_tablelist")[0].clientHeight > 754) {
        var height = $("#geofence_tablelist")[0].clientHeight + 71;
        SetLeftBarHeight(height);
    }
}

//GeoFence地图画面相关:描画地图、显示Group_ID对应的GeoFence
/***Map显示，GeoFence标注，Infowindow处理***/
function GeoFenceMap(Group_ID) {
    //GeoFenceinfo array
    var arrayGeos = new Array();

    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/GeoFence/GetGeofencesInfo",
        data: "group_id="+Group_ID,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {

            $("#geofence_BMap")[0].style.zIndex = -20000;
            //地图画面显示：默认以第一个GeoFence中心点的经纬度为地图中心进行描画
            //(11,1,1,1)
            //11:默认的地图显示Level;
            //1:NavigationControl控件显示
            //1:地图支持拖拽
            //1:地图支持鼠标滚轮缩放
            if ((msg)&&(msg.length != 0))
            {
                var ihpleD_map = new ihpleD_Map("geofence_BMap", msg[0].geofence.Baidulng, msg[0].geofence.Baidulat, 11, 1, 1, 1);
            }
            else{
                //默认北京（lng:116.404, lat:39.915）为地图中心
                var ihpleD_map = new ihpleD_Map("geofence_BMap", 116.404, 39.915, 11, 1, 1, 1);
            }
            
            //取得map对象
            BMapObj = ihpleD_map.get_mapObj();
            var mapObj = BMapObj.get_mapObj();

            
            //描画所有GeoFence
            drawAllGeoFence(msg, true, true);

            //检索功能跳转时，设定地图中心点和缩放级别
            if ($("#Latitude").val() != null && $("#Latitude").val() != 0 &&
                    $("#Longitude").val() != null && $("#Longitude").val() != 0) {
                var lat = $("#Latitude").val();
                var lng = $("#Longitude").val();
                var zoom = $("#zoomForSearch").val();
                var showType = $("#showTypeForSearch").val();

                if (zoom) {

                    //设置地图中心点
                    BMapObj.setNewMapView(lng, lat, parseInt(zoom));

                } else {
                    BMapObj.setNewMapView(lng, lat, 14);
                }

                if (showType == 3) {
                    addMarkerForCommonAddress(BMapObj, lng, lat);
                }

                $("#Latitude").attr("value", "");
                $("#Longitude").attr("value", "");
                $("#zoomForSearch").attr("value", "");
                $("#showTypeForSearch").attr("value", "");

                //清空session
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/" + CompanyID + "/GeoFence/CleanSearchPara",
                    contentType: "application/x-www-form-urlencoded",
                    data: "",
                    dataType: "json",
                    success: function (msg) {

                    }
                });
            }
        }
    });
}

//GeoFence重新描画：不进行地图再描画，只更新显示的GeoFence，视野调整与否依据(b_ChangeViewport)
function updateGeoFencesByGroup(Group_ID, b_ChangeViewport) {

    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/GetGeofencesInfo",
        contentType: "application/x-www-form-urlencoded",
        data: "group_id=" + Group_ID,
        dataType: "json",
        success: function (msg) {
            //描画被选中的组的GeoFences
            drawAllGeoFence(msg, b_ChangeViewport, false);
        }
    });
}

//重新描画msg中的GeoFences
function drawAllGeoFence(msg, b_ChangeViewport, isfirst) {
    //GeoFenceInfo array
    var arrayGeos = new Array();

    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();
    var mapObj = BMapObj.get_mapObj();

    //GeoFence Table处理  chenyangwen 20140226
    $("#geofence_table").empty();
    var head = '<tr style="height:50px;font-size:10pt;font-family:Microsoft YaHei;font-weight:bolder;" >' +
        '<td style="width:17%;text-align:left;">' + LanguageScript.page_geofence_GeofenceName + '</td>' +
        '<td style="width:17%;text-align:left;">' + LanguageScript.page_geofence_CenterLocation + '</td>' +//中心位置
        '<td style="width:41.26%;text-align:left;">' + LanguageScript.page_geofence_VehicleTrack + '</td>' + //zhangbo #761
        '<td style="width:23.06%;text-align:left;">' + LanguageScript.common_Operating + '</td></tr>';
    $("#geofence_table").append(head);

    //循环取得每一个GeoFence信息
    for (var i = 0; i < msg.length; i++) {
        //InfoBox的HTML
        var InfoWinContent = "";

        //GeoFence的状态（激活或者停用：0:Inactive， 1：active）
        var stateKind = -1;
        var statestr = "";
        var vehiclestr = "";

        //把GeofenceName转换为Unicode
        var geofencename = unicode($.trim(msg[i].geofence.name));

        if (msg[i].geofence.status == "Active" && 1 == roleID) {
            stateKind = 1;
            statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick=req_DeactiveGeofenceDialog("' + geofencename + '",' + msg[i].geofence.pkid + ')>' + LanguageScript.common_Deactivate + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick="trans_EditButton(' + msg[i].geofence.pkid + ')">' + LanguageScript.common_edit + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick= req_DeleteGeofenceDialog("' + geofencename + '",' + msg[i].geofence.pkid + ')>' + LanguageScript.common_delete + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;width: 55px;" onclick="trans_AddCarsButton(' + msg[i].geofence.pkid + ')">' + LanguageScript.page_geofence_addOrDeleteVehicle + '</div></div>';//添加车辆Redmine#640左移列表按钮对其liangjiajie0308
        } else if (1 == roleID) {
            stateKind = 0;
            statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div  id=geofence_List_inner_Active_' + msg[i].geofence.pkid + ' class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick="reqActivateGeo(' + msg[i].geofence.pkid + ')">' + LanguageScript.common_active + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div id=geofence_List_inner_Delete_' + msg[i].geofence.pkid + ' class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick=req_DeleteGeofenceDialog("' + geofencename + '",' + msg[i].geofence.pkid + ')>' + LanguageScript.common_delete + '</div></div>';
        } else if (msg[i].geofence.status == "Active" && 2 == roleID) {
            stateKind = 1;
            statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_Deactivate + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_edit + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_delete + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;width: 55px;">' + LanguageScript.page_geofence_addOrDeleteVehicle + '</div></div>';//添加车辆Redmine#640左移列表按钮对其liangjiajie0308
        } else if (2 == roleID) {
            stateKind = 0;
            statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_active + '</div></div>';
            statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default;"><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_delete + '</div></div>';
        }


        //生成对应于激活、停用状态的GeoFence的Infobox内容
        if (stateKind != 0)//0:Inactive， 1：active
        {
            //GeoFence InfoBox上的车辆List的HTML
            var vehicleList = vehicleButton(msg[i].hasvehicles, "map", msg[i].geofence.pkid, stateKind);

            InfoWinContent = GeoFenceInfoWinContent(msg[i].geofence.name, msg[i].geofence.location, vehicleList, msg[i].geofence.pkid, msg[i].geofence.radiu);
        }
        else {
            InfoWinContent = InactiveGeoFenceInfoWinContent(msg[i].geofence.name, msg[i].geofence.location, msg[i].geofence.pkid, msg[i].geofence.radiu);
        }

        //GeoFence对象的数据
        var GeoInfo = { geoId: msg[i].geofence.pkid, lng: msg[i].geofence.Baidulng, lat: msg[i].geofence.Baidulat, radius: msg[i].geofence.radiu, stateKind: stateKind, name: $.trim(msg[i].geofence.name), info_content: InfoWinContent };
        //把每个GeoFence push到GeoFence对象的数组
        arrayGeos.push(GeoInfo);

        //GeoFence List画面的车辆List的HTML 
        var vehicleNameList = vehicleButton(msg[i].hasvehicles, "list_" + msg[i].geofence.pkid, msg[i].geofence.pkid, stateKind);

        //GeoFence Table处理 chenyangwen 20140112
        var tr = '<tr style="height:50px;font-size:10pt;font-family:Microsoft YaHei;" id = "geofenceList_tr_' + msg[i].geofence.pkid + '">' +
            '<td><div style="width:140px;text-align:left; white-space: nowrap; text-overflow: ellipsis;overflow: hidden;" title = "' + $.trim(msg[i].geofence.name) + '">' + msg[i].geofence.name + '</div></td>' +
            '<td><div style="width:140px;text-align:left; white-space: nowrap; text-overflow: ellipsis;overflow: hidden;" title = "' + $.trim(msg[i].geofence.location) + '">' + msg[i].geofence.location + '</div></td>' +
            '<td style="width:340px;text-align:left; padding-bottom:10px;padding-left: 25px;">' + vehicleNameList + '</td>' +//Redmine#640左移列表按钮对其liangjiajie0306
            '<td style="width:190px;text-align:left;padding-bottom:10px;padding-left: 15px;">' + statestr + '</td></tr>';//Redmine#640左移列表按钮对其liangjiajie0308
        $("#geofence_table").append(tr);

    }

    //对GeoFence按半径从大到小排序（保证半径小的GeoFence被描画在最上面）
    sortGEO(arrayGeos);
    //GeoFence代码重构：
    //var geoMap = new ihpleD_ShowGeoFences(mapObj, arrayGeos, true, ifViewport);
    ihpleD_DisplayGeoFences(mapObj, arrayGeos, true, b_ChangeViewport, isfirst);

    //mabiao for GeoFence List更新页面高度
    GeoHeightList();
}

//GeoFence List局部刷新
function updateGeoFenceList(msg, geofenceId) {
    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();
	
    var trID = "geofenceList_tr_" + geofenceId;
    $("#" + trID).empty();
    var stateKind = -1;
    var statestr = "";
    var vehiclestr = "";

    //把GeofenceName转换为Unicode
    var geofencename = unicode($.trim(msg.geofence.name));

    if (msg.geofence.status == "Active" && 1 == roleID) {
        stateKind = 1;
        statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick=req_DeactiveGeofenceDialog("' + geofencename + '",' + msg.geofence.pkid + ')>' + LanguageScript.common_Deactivate + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick="trans_EditButton(' + msg.geofence.pkid + ')">' + LanguageScript.common_edit + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick=req_DeleteGeofenceDialog("' + geofencename + '",' + msg.geofence.pkid + ')>' + LanguageScript.common_delete + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;width: 55px;" onclick="trans_AddCarsButton(' + msg.geofence.pkid + ')">' + LanguageScript.page_geofence_addOrDeleteVehicle + '</div></div>';//添加车辆onclick="trans_AddCarsButton(' + GeofenceID + ')">'
    } else if (1 == roleID) {
        stateKind = 0;
        statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div  id=geofence_List_inner_Active_' + msg.geofence.pkid + ' class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick="reqActivateGeo(' + msg.geofence.pkid + ')">' + LanguageScript.common_active + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" ><div id=geofence_List_inner_Delete_' + msg.geofence.pkid + ' class ="GeoFenceInfo_vehicleStateNameTxt_style" style="color:blue;cursor:pointer;" onclick=req_DeleteGeofenceDialog("' + geofencename + '",' + msg.geofence.pkid + ')>' + LanguageScript.common_delete + '</div></div>';
    } else if (msg.geofence.status == "Active" && 2 == roleID) {
        stateKind = 1;
        statestr = '<div class ="GeoFenceInfo_vehiclStateeButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_Deactivate + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_edit + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_delete + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;width: 55px;">' + LanguageScript.page_geofence_addOrDeleteVehicle + '</div></div>';//#liangjiajie0312
    } else if (2 == roleID) {
        stateKind = 0;
        statestr = '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_active + '</div></div>';
        statestr += '<div class ="GeoFenceInfo_vehicleStateButton_style" style="cursor:default; "><div class ="GeoFenceInfo_vehicleStateNameTxt_style" style="cursor:default;color:gray;">' + LanguageScript.common_delete + '</div></div>';
    }
    var vehicleNameList = vehicleButton(msg.hasvehicles, "list_" + msg.geofence.pkid, msg.geofence.pkid, stateKind);
    var td = '<td><div style="width:140px;text-align:left; white-space: nowrap; text-overflow: ellipsis;overflow: hidden;" title = "' + $.trim(msg.geofence.name) + '">' + msg.geofence.name + '</div></td>' +
            '<td><div style="width:140px;text-align:left; white-space: nowrap; text-overflow: ellipsis;overflow: hidden;" title = "' + $.trim(msg.geofence.location) + '">' + msg.geofence.location + '</div></td>' +
            '<td style="width:340px;text-align:left;padding-bottom:10px;padding-left: 25px;">' + vehicleNameList + '</td>' +//Redmine#640左移列表按钮对其liangjiajie0306
            '<td style="width:190px;text-align:left;padding-bottom:10px;padding-left: 15px;">' + statestr + '</td>';
    $("#" + trID).html(td);
}

function getXmlHttpRequest() {
    var xmlhttp = null;
    if ((typeof XMLHttpRequest) != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    } else {
        xmlhttp = new ActiveXObject('Microsoft.XMLHttp');
    }
    return xmlhttp;
}

//激活的GeoFenceInfoWindow的HTML内容
function GeoFenceInfoWinContent(name, location, vehicleList, GeoID, radiu) {
    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();

    //把GeofenceName转换为Unicode
    var geofencename = unicode($.trim(name));

    if (1 != roleID) {

        return "<div id='GeoFenceInfoID_" + GeoID + "' class ='GeoFenceInfo_style' >" +
        "<div id='GeoFenceInfo_Top' class ='GeoFenceInfo_Top_style'>" +
            "<div id='GeoFenceInfo_Name' class ='GeoFenceInfo_Name_style' title='" + $.trim(name) + "'>" +
                $.trim(name) +
            "</div>" +
            "<div id='GeoFenceInfo_Location' class ='GeoFenceInfo_Location_style' title='" + $.trim(location) + "'>" +
                $.trim(location) +
            "</div>" +
            "<div id='GeoFenceInfo_Pop_Radiu' class = 'GeoFenceInfo_Pop_Radiu_style' title ='" + LanguageScript.page_geofence_Radius + "'>" +
            parseInt(radiu)+ LanguageScript.page_geofence_mi +
            "</div>"+
        "</div>" +
        "<div id = 'GeoFenceInfo_centre' class='GeoFenceInfo_centre_style'>" +
            "<div id='GeoFenceInfo_TopLine' class ='GeoFenceInfo_TopLine_style'></div>" +
            "<div id='GeoFenceInfo_Title' class ='GeoFenceInfo_Title_style'>" + LanguageScript.page_geofence_VehicleTrack + ":</div>" +
            "<div id='GeoFenceInfo_VehiclesList' class ='GeoFenceInfo_VehiclesList_style'>" +
                vehicleList +
            "</div>" +
        "</div>" +
        "<div id = 'GeoFenceInfo_bottom' class='GeoFenceInfo_bottom_style'>" +
            "<div id='GeoFenceInfo_BottomLine' class ='GeoFenceInfo_BottomLine_style'></div>" +
            "<div id='GeoFenceInfo_EditShape' class ='GeoFenceInfo_BottomButton_style' style='cursor:default;background-color:rgb(177, 172, 172)' >" + LanguageScript.common_edit + "</div>" +
            "<div id='GeoFenceInfo_Deactivate' class ='GeoFenceInfo_BottomButton_style' style='cursor:default;background-color:rgb(177, 172, 172)' >" + LanguageScript.common_Deactivate + "</div>" +
            "<div id='GeoFenceInfo_Delete' class ='GeoFenceInfo_BottomButton_style' style='cursor:default;background-color:rgb(177, 172, 172)' >" + LanguageScript.common_delete + "</div>" +
            "<div id='GeoFenceInfo_AddVehicle' class ='GeoFenceInfo_BottomButton_style' style='cursor:default;background-color:rgb(177, 172, 172);'>" + LanguageScript.page_geofence_addOrDeleteVehicle + "</div>" +
        "</div>" +
        "<div id='GeoFenceInfo_Arrow' class ='GeoFenceInfo_Arrow_style'>" +
            "<div id='GeoFenceInfo_ArrowIcon' class ='GeoFenceInfo_ArrowIcon_style'></div>" +
            "   </div>" +
            "</div>" +
            "</div>"

    } else {
        return "<div id='GeoFenceInfoID_" + GeoID + "' class ='GeoFenceInfo_style' >" +
       "<div id='GeoFenceInfo_Top' class ='GeoFenceInfo_Top_style'>" +
           "<div id='GeoFenceInfo_Name' class ='GeoFenceInfo_Name_style' title='" + $.trim(name) + "'>" +
               $.trim(name) +
           "</div>" +
           "<div id='GeoFenceInfo_Location' class ='GeoFenceInfo_Location_style' title='" + $.trim(location) + "'>" +
               $.trim(location) +
           "</div>" +
           "<div id='GeoFenceInfo_Pop_Radiu' class = 'GeoFenceInfo_Pop_Radiu_style' title ='" + LanguageScript.page_geofence_Radius + "'>" +
            parseInt(radiu) + LanguageScript.page_geofence_mi +
            "</div>" +
       "</div>" +
       "<div id = 'GeoFenceInfo_centre' class='GeoFenceInfo_centre_style'>" +
           "<div id='GeoFenceInfo_TopLine' class ='GeoFenceInfo_TopLine_style'></div>" +
           "<div id='GeoFenceInfo_Title' class ='GeoFenceInfo_Title_style'>" + LanguageScript.page_geofence_VehicleTrack + ":</div>" +
           "<div id='GeoFenceInfo_VehiclesList' class ='GeoFenceInfo_VehiclesList_style'>" +
               vehicleList +
           "</div>" +
       "</div>" +
       "<div id = 'GeoFenceInfo_bottom' class='GeoFenceInfo_bottom_style'>" +
           "<div id='GeoFenceInfo_BottomLine' class ='GeoFenceInfo_BottomLine_style'></div>" +
           "<div id='GeoFenceInfo_EditShape' class ='GeoFenceInfo_BottomButton_style' style='cursor:pointer' onclick='trans_EditButton(" + GeoID + ")' >" + LanguageScript.common_edit + "</div>" +
           "<div id='GeoFenceInfo_Deactivate' class ='GeoFenceInfo_BottomButton_style' style='cursor:pointer' onclick = req_DeactiveGeofenceDialog('" + geofencename + "'," + GeoID + ") >" + LanguageScript.common_Deactivate + "</div>" +
           "<div id='GeoFenceInfo_Delete' class ='GeoFenceInfo_BottomButton_style' style='cursor:pointer' onclick = req_DeleteGeofenceDialog('" + geofencename + "','" + GeoID + "') >" + LanguageScript.common_delete + "</div>" +
           "<div id='GeoFenceInfo_AddVehicle' class ='GeoFenceInfo_BottomButton_style' style='cursor:pointer' onclick='trans_AddCarsButton(" + GeoID + ")' >" + LanguageScript.page_geofence_addOrDeleteVehicle + "</div>" +
       "</div>" +
       "<div id='GeoFenceInfo_Arrow' class ='GeoFenceInfo_Arrow_style'>" +
           "<div id='GeoFenceInfo_ArrowIcon' class ='GeoFenceInfo_ArrowIcon_style'></div>" +
           "   </div>" +
           "</div>" +
        "</div>"
    }   
}

//GeoFenceInfoWindow中车辆追踪信息块
function vehicleButton(vehicleList, str, GeofenceID, stateKind) {
    //chenyangwen 20140226
    var firstDiv = "";
    var firstText = "";
    if ("map" == str) {
        firstDiv = "GeoFenceInfo_vehicleButton_";
        firstText = "GeoFenceInfo_vehicleNameTxt"
    } else {
        firstDiv = "GeoFenceInfo_vehicleButton_" + str + "_";
        firstText = "GeoFenceInfo_" + str + "_vehicleNameTxt"
    }
    var vehicleNameList = "&nbsp;";

    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();
    if (1 != roleID || 0 == stateKind) {
        for (var i = 0; i < vehicleList.length; i++) {
            var GeoVehicleID = firstDiv + vehicleList[i].pkid;
            vehicleNameList += '<div id=' + GeoVehicleID + ' class ="GeoFenceInfo_vehicleButton_style" style="cursor:default;background-color:rgb(177, 172, 172);">' +
            '<div id= ' + firstText + ' class ="GeoFenceInfo_vehicleNameTxt_style">' + $.trim(vehicleList[i].name) + '</div>' +
            '</div>';
        }

    } else {
        for (var i = 0; i < vehicleList.length; i++) {
            var GeoVehicleID = firstDiv + vehicleList[i].pkid;
            vehicleNameList += '<div id=' + GeoVehicleID + ' class ="GeoFenceInfo_vehicleButton_style" onmouseover="vehicleButton_mouseover(' + GeoVehicleID + ')">' +
            '<div id=' + firstText + ' class ="GeoFenceInfo_vehicleNameTxt_style">' + $.trim(vehicleList[i].name) + '</div>' +
            '</div>';
        }
    }
    return vehicleNameList;
    //chenyangwen 20140226
}


//停用的的GeoFenceInfoWindow的HTML内容
function InactiveGeoFenceInfoWinContent(name, location, GeoID, radiu) {
    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();

    //把GeofenceName转换为Unicode
    var geofencename = unicode($.trim(name));

    if (1 != roleID) {
        return "<div id='InactiveGeoFenceInfoID_" + GeoID + "' class ='InactiveGeoFenceInfo_style'>" +
        "<div id='InactiveGeoFenceInfo_Top' class ='InactiveGeoFenceInfo_Top_style'>" +
            "<div id='InactiveGeoFenceInfo_Name' class ='InactiveGeoFenceInfo_Name_style' title='" + $.trim(name) + "'>" +
                $.trim(name) + LanguageScript.page_geofence_InActive +
            "</div>" +
            "<div id='InactiveGeoFenceInfo_Location' class ='InactiveGeoFenceInfo_Location_style' title='" + $.trim(location) + "'>" +
                $.trim(location) +
            "</div>" +
            "<div id='InactiveGeoFenceInfo_Pop_Radiu' class = 'InactiveGeoFenceInfo_Pop_Radiu_style' title ='" + LanguageScript.page_geofence_Radius + "'>" +
            parseInt(radiu) + LanguageScript.page_geofence_mi +
            "</div>" +
        "</div>" +
        "<div id = 'InactiveGeoFenceInfo_bottom' class='InactiveGeoFenceInfo_bottom_style'>" +
            "<div id='InactiveGeoFenceInfo_BottomLine' class ='InactiveGeoFenceInfo_BottomLine_style'></div>" +
            "<div id='InactiveGeoFenceInfo_ActivateButton' class ='InactiveGeoFenceInfo_ActivateButton_style' style='cursor:default;background-color:rgb(177, 172, 172)' >" + LanguageScript.common_active + "</div>" +
            "<div id='InactiveGeoFenceInfo_DeleteButton' class ='InactiveGeoFenceInfo_DeleteButton_style' style='cursor:default;background-color:rgb(177, 172, 172)' >" + LanguageScript.common_delete + "</div>" +
        "</div>" +
        "<div id='InactiveGeoFenceInfo_Arrow' class ='InactiveGeoFenceInfo_Arrow_style'>" +
            "<div id='InactiveGeoFenceInfo_ArrowIcon' class ='InactiveGeoFenceInfo_ArrowIcon_style'></div>" +
        "   </div>" +
        "</div>" +
        "</div>"
    } else {
                return "<div id='InactiveGeoFenceInfoID_" + GeoID + "' class ='InactiveGeoFenceInfo_style'>" +
            "<div id='InactiveGeoFenceInfo_Top' class ='InactiveGeoFenceInfo_Top_style'>" +
                "<div id='InactiveGeoFenceInfo_Name' class ='InactiveGeoFenceInfo_Name_style' title='" + $.trim(name) + LanguageScript.page_geofence_InActive + "'>" +
                    $.trim(name) + LanguageScript.page_geofence_InActive +
                "</div>" +
                "<div id='InactiveGeoFenceInfo_Location' class ='InactiveGeoFenceInfo_Location_style' title='" + $.trim(location) + "'>" +
                    $.trim(location) +
                "</div>" +
            "<div id='InactiveGeoFenceInfo_Pop_Radiu' class = 'InactiveGeoFenceInfo_Pop_Radiu_style' title ='" + LanguageScript.page_geofence_Radius + "'>" +
            parseInt(radiu) + LanguageScript.page_geofence_mi +
            "</div>" +
            "</div>" +
            "<div id = 'InactiveGeoFenceInfo_bottom' class='InactiveGeoFenceInfo_bottom_style'>" +
                "<div id='InactiveGeoFenceInfo_BottomLine' class ='InactiveGeoFenceInfo_BottomLine_style'></div>" +
                "<div id='InactiveGeoFenceInfo_ActivateButton' class ='InactiveGeoFenceInfo_ActivateButton_style' style='cursor:pointer'onclick = 'reqActivateGeo(" + GeoID + ")' >" + LanguageScript.common_active + "</div>" +
                "<div id='InactiveGeoFenceInfo_DeleteButton' class ='InactiveGeoFenceInfo_DeleteButton_style' style='cursor:pointer' onclick = req_DeleteGeofenceDialog('" + geofencename + "','" + GeoID + "') >" + LanguageScript.common_delete + "</div>" +
            "</div>" +
            "<div id='InactiveGeoFenceInfo_Arrow' class ='InactiveGeoFenceInfo_Arrow_style'>" +
                "<div id='InactiveGeoFenceInfo_ArrowIcon' class ='InactiveGeoFenceInfo_ArrowIcon_style'></div>" +
            "   </div>" +
            "</div>" +
            "</div>"
        }
}

//鼠标移入GeoFenceInfoWindow的VehiclButton上时，背景置成灰色并追加DeleteButton
function vehicleButton_mouseover(GeoVehicleId) {

    var id = GeoVehicleId.id;
    if ($("#" + id).children().length > 1) {
        return;
    }
    $("#" + id).removeClass();

    $("#" + id).addClass("GeoFenceInfo_vehicleButton_style_mousecover");

    $("#" + id).append(function () {
        var DeleteVehicleButtonId = "GeoFenceInfo_vehicleButtonClose";
        return '<div id="GeoFenceInfo_vehicleButtonClose" class ="GeoFenceInfo_vehicleButtonDelete_style" style="cursor:pointer;" onclick="DeleteVehicle(GeoFenceInfo_vehicleButtonClose)"></div>';
    })

    //追加鼠标移除时的动作
    var DeleteID = "GeoFenceInfo_vehicleButtonClose";
    $("#" + id).mouseleave(function () {
        $("#" + id).removeClass();
        $("#" + DeleteID).remove();
        $("#" + id).addClass("GeoFenceInfo_vehicleButton_style");
    });
}

//从某个GeoFence中，删除被选中的车辆
function DeleteVehicle(Delete_vehicleID) {

    var DeleteID = Delete_vehicleID.id;

    //查找Close部品的父节点：找到要Delete的Vehicle
    var tmp_GeoVehicleID = $("#" + DeleteID).parent()[0].id;
    var vName = $.trim($("#" + DeleteID).parent().text());

    //查找Close的祖先节点：找到要Delete的Geo
    var tmp_GeoFenceID = $("#" + DeleteID).parentsUntil(".GeoFenceInfo_style").parent()[0].id;
    var GeoFenceID = 0;
    var GeoVehicleID = '';
    if (undefined == tmp_GeoFenceID) {
        tmp_GeoFenceID = $("#" + DeleteID).parent()[0].id;
        var tmp1 = tmp_GeoFenceID.split("_");
        var index1 = tmp1.length - 2;
        var index2 = tmp1.length - 1;
        GeoFenceID = tmp1[index1];
        GeoVehicleID += tmp1[index2];
    } else {
        //把GeoFenceID拆分出来
        var tmp1 = tmp_GeoFenceID.split("_");
        GeoFenceID = tmp1[1];
        for (var i = 2; i < tmp1.length; i++) {
            GeoFenceID += "_" + tmp1[i];
        }
        //把VehicleID拆分出来
        var tmp2 = tmp_GeoVehicleID.split("_");
        //var GeoVehicleID = tmp2[1];
        GeoVehicleID = '';
        for (var i = 2; i < tmp2.length; i++) {
            GeoVehicleID += tmp2[i];
        }
    }
    
    //先弹出确认提示框：确认删除车辆
    req_DeleteGeoVehicleDialog(GeoFenceID, GeoVehicleID, vName);
}

//向后台请求从GeoFenceID中删除车辆VehicleID
function reqDeleteGeoVehicle(GeoFenceID, VehicleID) {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/GeoFence/DeleteGeoVehicle', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                if (txt == "False") {
                    user_dialog_error6(LanguageScript.error_01300);
                    return;
                }

                //删除车辆成功后，变更GeoFence的InfoBox内容
                updateGeoFenceVehicleList(GeoFenceID);

                //后台“删除车辆的请求”处理完毕后，从页面上把车辆remove
                $("#GeoFenceInfo_vehicleButton_" + VehicleID).remove();
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //... ...
            }
        } else {
            //... ...
        }
    }
    
    var message = 'geofenceID=' + GeoFenceID + '&' + 'vehicleID=' + VehicleID;
    xmlhttp.send(message);
}


//停用GeoFence，确认提示框
function req_DeactiveGeofenceDialog(geofenceName, geofencID) {

    //把Unicode转换为字符串
    var name = runicode(geofenceName);

    $("#DeactiveGeofence_dialog")[0].innerHTML =
       '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + name + '>围栏名称:' + name + '</p>' +
       '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">您确认要停用电子围栏?</p>';
    $(function () {
        $("#DeactiveGeofence_dialog").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                "确定": function () {
                    reqDeactiveGeo(geofencID);
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}

//向后台请求把被选中GeoFence停用
function reqDeactiveGeo(GeoFenceID) {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/GeoFence/InactiveOrActiveGeo', false);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                if (txt == "False") {
                    user_dialog_error6(LanguageScript.error_01300);
                    return;
                } else {

                    //请求成功后，GeoFence显示内容更新：
                    updateGeoFenceInfoByGeofenceID(GeoFenceID);
                }
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //... ...
            }
        } else {
            //... ...
        }
    }
    var message = 'geofenceID=' + GeoFenceID + "&status=" + "InActive";
    xmlhttp.send(message);
}


//删除GeoFence，确认提示框
function req_DeleteGeofenceDialog(geofenceName, geofencID) {
    //把Unicode转换为字符串
    var name = runicode(geofenceName);

    $("#DeleteGeofence_dialog")[0].innerHTML =
        '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + name + '>围栏名称 : ' + name + '</p>' +
        '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">您确认要删除电子围栏?</p>';

    $(function () {
        $("#DeleteGeofence_dialog").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                确定: function () {
                    reqDeleteGeo(geofencID);
                    $(this).dialog("close");
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}

//向后台请求把GeoFence删除
function reqDeleteGeo(GeoFenceID) {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/GeoFence/DeleteGeo', false);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                if (txt == "False") {
                    user_dialog_error6(LanguageScript.error_01300);
                    return;
                } else {

                    //请求成功后，GeoFence显示内容更新：
                    GeoFenceCollection.removeGeoFence(GeoFenceID);
                    var trID = "geofenceList_tr_" + GeoFenceID;
                    $("#" + trID).remove();
                }
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status)
            {
                window.location.href = "/";
            }
            else {
                //... ...
            }
        } else {
            //... ...
        }
    }
    var message = 'geofenceID=' + GeoFenceID;
    xmlhttp.send(message);
}


//向后台请求把选中的GeoFenceID激活
function reqActivateGeo(GeoFenceID) {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/GeoFence/InactiveOrActiveGeo', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                if (txt == "False") {
                    user_dialog_error6(LanguageScript.error_01300);
                    return;
                } else {

                    //请求成功后，GeoFence显示内容更新：
                    updateGeoFenceInfoByGeofenceID(GeoFenceID);
                }
            //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //... ...
            }
        } else {
            //... ...
        }
    }
    var message = 'geofenceID=' + GeoFenceID + "&status=" + "Active";
    xmlhttp.send(message);
}

function getData() {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/"+ CompanyID+"/Dashboard/GetGroups",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#select_group").empty();
            $("#select_group").append("<option value='-1'>" + LanguageScript.page_Dasboard_FleetLocation_EntireFleet + "</option>");
            for (var i = 0; i < msg.length; ++i) {
                var options = "<option value='" + msg[i].pkid + "' title = '" + $.trim(msg[i].name) + "'>" + $.trim(msg[i].name) + "</option>";
                $("#select_group").append(options);
            }
            //fengpan 20140617 #1924
            $('#select_group').selectpicker('refresh');
        }
    });
}

/**chenyangwen**/

//点击GeoFence弹出的Infobox上的编辑
function trans_EditButton(GeoID) {
    trans();
    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/GeoFence/GetGeoFenceInfoByGeoFenceId",
        contentType: "application/x-www-form-urlencoded",
        data: "geofenceID=" + GeoID,
        dataType: "json",
        success: function (msg) {

            //取得当前地图的缩放级别
            var mapInfoJson = BMapObj.getMapInfo();
            var obj = eval("(" + mapInfoJson + ")");
            var Zoom = obj.zoomLevel;

            //取得当前被编辑的GeoFence的必要信息：中心点、半径、Name、Location
            if (msg.geofence.pkid == GeoID) {
                var Lng = msg.geofence.Baidulng;
                var Lat = msg.geofence.Baidulat;

                var StateKind = -1;
                if ("Active" == msg.geofence.status)
                {
                    StateKind = 1;
                }else{
                    StateKind = 0;
                    //停用的GeoFence不可以编辑
                    return;
                }
                var GeoFenceradius = msg.geofence.radiu;
                var EditGeoFenceName = msg.geofence.name;
                var EditGeoFenceLocation = msg.geofence.location;
                var CompanyID = GetCompanyID();
                $.ajax({
                    type: "POST",
                    url: "/" + CompanyID + "/GeoFence/SetEditGeoFenceInfo",
                    contentType: "application/x-www-form-urlencoded",
                    data: { EditGeoFenceID: GeoID, centerlng: Lng, centerlat: Lat, zoom: Zoom, EditGeoFenceName: $.trim(EditGeoFenceName), radius: GeoFenceradius, EditGeoFenceLocation: $.trim(EditGeoFenceLocation) },
					success: function (msg) {
							NextURL = localhostUrl() + "/GeoFence/EditShape";
							location.href = NextURL;
							}
                });
               
            }
        }
    });
}

//点击GeoFence画面的“添加电子围栏”按钮：新规电子围栏-->跳转到EditGeoFence画面
function trans_AddGeoFence( ) {

    //取得CompanyID
    var CompanyID = GetCompanyID();

    //新规电子围栏时，GeoFenceID = (-1)(默认)，中心点为当前地图中心点、半径默认为(-1,描绘时，自适应)
    var GeoID = (-1);
    //取得当前地图的缩放级别和中心
    var mapInfoJson = BMapObj.getMapInfo();
    var obj = eval("(" + mapInfoJson + ")");
    var Zoom = obj.zoomLevel;
    var Center = obj.center;
    var GeoFenceradius = -1;
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/GeoFence/SetEditGeoFenceInfo",
        contentType: "application/x-www-form-urlencoded",
        data: { EditGeoFenceID: GeoID, centerlng: Center.lng, centerlat: Center.lat, zoom: Zoom, EditGeoFenceName: null, radius: GeoFenceradius, EditGeoFenceLocation: null},
		success: function (msg) {	
		NextURL = localhostUrl() + "/GeoFence/EditShape";
		location.href = NextURL;
		},
    });
    
}

// 点击Vehicle Marker弹出的Infowindow上的“+添加车辆”
function trans_AddCarsButton(GeoID) {
    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/GeoFence/GetGeoFenceInfoByGeoFenceId",
        contentType: "application/x-www-form-urlencoded",
        data: "geofenceID=" + GeoID,
        dataType: "json",
        success: function (msg) {

            if (msg.geofence.pkid == GeoID) {
                var Lng = msg.geofence.Baidulng;
                var Lat = msg.geofence.Baidulat;
                var StateKind = -1;
                if ("Active" == msg.geofence.status)
                {
                    StateKind = 1;
                } else {
                    //停用的GeoFence不可以追加车辆
                    StateKind = 0;
                    return;
                }
                var GeoFenceradius = msg.geofence.radiu;
                var EditGeoFenceName = msg.geofence.name;
                var GeoFenceLocation = msg.geofence.location;

                //围栏ID 名称 中心经度  中心纬度 半径 地址 保存类型选项（string:type 选项有"new","edit","addcar"）
                SaveGeoByPopup(GeoID, EditGeoFenceName, Lng, Lat, GeoFenceradius, GeoFenceLocation, "addcar");

            }
        }
    });
}


//变更GeoFenced的Infobox内容
function updateGeoFenceVehicleList(GeoFenceId) {
    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/GetGeoFenceInfoByGeoFenceId",
        contentType: "application/x-www-form-urlencoded",
        data: "geofenceID=" + GeoFenceId,
        dataType: "json",
        success: function (msg) {
            if (msg.geofence.pkid == GeoFenceId) 
	    {
                //更新GeoFenced的Infobox内容
                updateGeoFenceInfoBoxContent(msg, GeoFenceId);
                //GeoFence List局部刷新
                updateGeoFenceList(msg, GeoFenceId);
            }
        }
    });
}

//变更GeoFenced的Infobox内容
function updateGeoFenceInfoBoxContent(msg, GeoFenceId) {

    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();

    //InfoBox的HTML
    var InfoWinContent = "";

    //GeoFence的状态（激活或者停用：0:Inactive， 1：active）
    var stateKind = -1;
    if (msg.geofence.status == "Active") {
        stateKind = 1;
    } else {
        stateKind = 0;
    }

    //生成对应于激活、停用状态的GeoFence的Infobox内容
    if (stateKind != 0)//0:Inactive， 1：active
    {
        //GeoFence InfoBox上的车辆List的HTML
        var vehicleList = vehicleButton(msg.hasvehicles, "map", msg.geofence.pkid, stateKind);

        InfoWinContent = GeoFenceInfoWinContent(msg.geofence.name, msg.geofence.location, vehicleList, msg.geofence.pkid, msg.geofence.radiu);
    }
    else {
        InfoWinContent = InactiveGeoFenceInfoWinContent(msg.geofence.name, msg.geofence.location, msg.geofence.pkid, msg.geofence.radiu);
    }

    //追加车辆成功后，变更GeoFence的InfoBox内容：通过GeoFenceID取得当前操作的GeoFence
    var currentGeoFence = GeoFenceCollection.getByGeoFenceId(GeoFenceId);
    //变更当前GeoFence的InfoBox内容
    currentGeoFence.changeInfoBoxContent(InfoWinContent);
}



//变更GeoFenced的相关信息
function updateGeoFenceInfoByGeofenceID(GeoFenceId) {
    //取得CompanyID
    var CompanyID = GetCompanyID();

    //取得GeoFenceInfo
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/GetGeoFenceInfoByGeoFenceId",
        contentType: "application/x-www-form-urlencoded",
        data: "geofenceID=" + GeoFenceId,
        dataType: "json",
        success: function (msg) {
            if (msg.geofence.pkid == GeoFenceId) {
                //变更GeoFenced的相关信息
                ResetGeoFenceInfo(msg, GeoFenceId);

                //GeoFence List局部刷新
                updateGeoFenceList(msg, GeoFenceId);

            }
        }
    });
}

//变更GeoFenced的相关信息
function ResetGeoFenceInfo(msg, GeoFenceId) {

    //账户权限管理：1为管理员，2为普通用户
    var roleID = GetRoleID();

    //追加车辆成功后，变更GeoFence的InfoBox内容：通过GeoFenceID取得当前操作的GeoFence
    var currentGeoFence = GeoFenceCollection.getByGeoFenceId(GeoFenceId);

    //InfoBox的HTML
    var InfoWinContent = "";

    //GeoFence的状态（激活或者停用：0:Inactive， 1：active）
    var stateKind = -1;
    if (msg.geofence.status == "Active") {
        stateKind = 1;
    } else {
        stateKind = 0;
    }

    //生成对应于激活、停用状态的GeoFence的Infobox内容
    if (stateKind != 0)//0:Inactive， 1：active
    {
        //GeoFence InfoBox上的车辆List的HTML
        var vehicleList = vehicleButton(msg.hasvehicles, "map", msg.geofence.pkid, stateKind);
        InfoWinContent = GeoFenceInfoWinContent(msg.geofence.name, msg.geofence.location, vehicleList, msg.geofence.pkid, msg.geofence.radiu);

        //更新当前GeoFence为激活状态
        currentGeoFence.ActivateGeoFence(InfoWinContent);
    }
    else {
        InfoWinContent = InactiveGeoFenceInfoWinContent(msg.geofence.name, msg.geofence.location, msg.geofence.pkid, msg.geofence.radiu);

        //更新当前GeoFence为停用状态
        currentGeoFence.DeactiveGeoFence(InfoWinContent);
    }
}


// 把围栏按半径从大到小排序
function sortGEO(data){
    return data.sort(sortfunction);
}

function sortfunction(x, y) {
    return y.radius - x.radius;
}

function req_DeleteGeoVehicleDialog(GeoFenceID, GeoVehicleID, vName) {

    //把Unicode转换为字符串
    //var name = runicode(vName);

    $("#DeleteGeoVehicleDialog")[0].innerHTML =
        '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space: nowrap;text-overflow: ellipsis;overflow: hidden;" title = "'+vName+'">' + "车辆名称:" + vName + '</p>' +
        '<p class = "geofence_dialog_style" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">'+'您确认要删除车辆?' + '</p>';
    $(function () {
        $("#DeleteGeoVehicleDialog").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                "确定": function () {
                    reqDeleteGeoVehicle(GeoFenceID, GeoVehicleID);
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}