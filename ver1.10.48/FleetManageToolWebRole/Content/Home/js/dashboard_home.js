var BMapObj = null;
var arrLocationforMarker = new Array();
$(document).ready(function () {
	arrLocationforMarker = [];
    initSelect();
    $("#pageBar").hide();
    totalnum_short = 0;
    totalnum_tall = 0;
    pagenumber = 1;
    pagegroupid = -1;
    totalelement = 0;
    ArrAllshort = new Array();
    ArrAlllong = new Array();
    ArrLocationAll = new Array();
    ArrLocationOne = new Array();
    //保存时区信息到session
    var timeZone = new Date().getTimezoneOffset() / 60 * -1;
    //var dateTimeYear = Date.now();
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Dashboard/SaveTimeZone",
        data: { timeZone: timeZone/*, date:dateTime*/ },
        contentType: "application/x-www-form-urlencoded",
        dataType: "",
        success: function (msg) {

        }
    });


    arrayVehicles = null;
    transformLocation = new Array();
    var date = new Date();
    ihpleDTripHeight = 0;
    ihpleDTripDate = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
    ihpleDTripNow = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
    //画面每隔1分钟更新
    var realTimeHomeInterval = null;

    //地图
    getVehicleInfoForMap(-1);

    //车辆list
    //chenyangwen 20140611 #1357
    var intervalTokenTemp = Math.round(Math.random() * 1000000);
    getVehicleInfoForList(-1, intervalTokenTemp);
    /*fengpan 上部button显示隐藏*/
    $("#dashboard_button_hide_button_down").click(function () {
        //显示TripLog之前，保存当前地图的信息
        var mapInfoJson = BMapObj.getMapInfo();
        var obj = eval("(" + mapInfoJson + ")");
        var lng = obj.center.lng;
        var lat = obj.center.lat;
        var Zoom = obj.zoomLevel;
        if (pagenumber * $("#CountPerShortPage").val() > totalelement) {
            if (totalelement % $("#CountPerTallPage").val() == 0) {
                pagenumber = parseInt(totalelement / $("#CountPerTallPage").val());
            }
            else {
                pagenumber = parseInt(totalelement / $("#CountPerTallPage").val()) + 1;
            }
        } else {
            if ((pagenumber * $("#CountPerShortPage").val()) % $("#CountPerTallPage").val() == 0) {
                pagenumber = parseInt((pagenumber * $("#CountPerShortPage").val()) / $("#CountPerTallPage").val());
            }
            else {
                pagenumber = parseInt((pagenumber * $("#CountPerShortPage").val()) / $("#CountPerTallPage").val()) + 1;
            }
        }
        if (pagenumber == 0) {
            pagenumber = 1;
        }
        //ArrAllshort = new Array();
        //ArrAlllong = new Array();
        //getVehicleInfoForList(pagegroupid);
        AppendTrTall(pagenumber);
        $("#dashboard_button_hide_line_up").show();
        $("#dashboard_button_hide_button_up").show();
        $("#dashboard_button_hide_line_down").hide();
        $("#dashboard_button_hide_button_down").hide();
        $("#dashboard_status_button").hide();
        $("#dashboard_title_lable").css("top","-121px");
        $("#dashboard_BMap_mother").css("top", "-121px");
        $("#dashboard_BMap").css("height", "727px");

        //显示TripLog之后，更新地图的中心点
        setTimeout(function () {
            BMapObj.setNewMapView(lng, lat, Zoom);
        }, 100);
    });
    $("#dashboard_button_hide_button_up").click(function () {
        //显示TripLog之前，保存当前地图的信息
        var mapInfoJson = BMapObj.getMapInfo();
        var obj = eval("(" + mapInfoJson + ")");
        var lng = obj.center.lng;
        var lat = obj.center.lat;
        var Zoom = obj.zoomLevel;
        if (pagenumber * $("#CountPerTallPage").val() > totalelement) {
            if (totalelement % $("#CountPerShortPage").val() == 0) {
                pagenumber = parseInt(totalelement / $("#CountPerShortPage").val());
            }
            else {
                pagenumber = parseInt(totalelement / $("#CountPerShortPage").val()) + 1;
            }
        } else {
            if ((pagenumber * $("#CountPerTallPage").val()) % $("#CountPerShortPage").val() == 0) {
                pagenumber = parseInt((pagenumber * $("#CountPerTallPage").val()) / $("#CountPerShortPage").val());
            }
            else {
                pagenumber = parseInt((pagenumber * $("#CountPerTallPage").val()) / $("#CountPerShortPage").val()) + 1;
            }
        }
        if (pagenumber == 0) {
            pagenumber = 1;
        }
        //ArrAllshort = new Array();
        //ArrAlllong = new Array();
        //getVehicleInfoForList(pagegroupid);
        AppendTrShort(pagenumber);
        $("#dashboard_button_hide_line_up").hide();
        $("#dashboard_button_hide_button_up").hide();
        $("#dashboard_button_hide_line_down").show();
        $("#dashboard_button_hide_button_down").show();
        $("#dashboard_status_button").show();
        $("#dashboard_title_lable").css("top", "5px");
        $("#dashboard_BMap_mother").css("top", "5px");
        $("#dashboard_BMap").css("height", "602px");

        //显示TripLog之后，更新地图的中心点
        setTimeout(function () {
            BMapObj.setNewMapView(lng, lat, Zoom);
        }, 100);
    });
    
    /***右侧trip log隐藏 显示***/
    $("#Home_hide_trip_log").hide();
    
    $("#show_trip_log").click(function () {

        //隐藏TripLog之前，保存当前地图的信息
        var mapInfoJson = BMapObj.getMapInfo();
        var obj = eval("(" + mapInfoJson + ")");
        var lng = obj.center.lng;
        var lat = obj.center.lat;
        var Zoom = obj.zoomLevel;
        //ArrAllshort = new Array();
        //ArrAlllong = new Array();
        //getVehicleInfoForList(pagegroupid);
        //AppendTrShort(pagenumber);
        /*fengpan 20140408*/
        $("#dashboard_button_hide_line_up").css("width", "97%");
        $("#dashboard_button_hide_button_up").css("width", "97%");
        $("#dashboard_button_hide_line_down").css("width", "97%");
        $("#dashboard_button_hide_button_down").css("width", "97%");

        $("#Home_show_trip_log").hide();
        $("#dashboard_viewalltriplog").hide();
        $("#Home_hide_trip_log").show();
        $("#dashboard_BMap").css("width", "96.6%");

        //$("#dashboard_hide").show();
        //$("#dashboard_show").hide();
        $("#dashboard_status_bar").css("width", "98%");


        var display = $("#dashboard_vehicleslist").css("display");
        if ("none" != display)
        {
            $("#dashboard_BMap")[0].style.zIndex = -20000;
            $("#dashboard_vehicleslist_long").show();
            $("#dashboard_vehicleslist").hide();
            
        }
        $(".dashboard_viewgroup_text").css("left", "68%");
        $(".dashboard_choosed_bg_container").css("left", "93%");
        $(".dashboard_showmap_bg_container").css("left", "94.3%");
        $(".dashboard_unchoosed_bg_container").css("left", "88%");
        $(".dashboard_showlist_bg_container").css("left", "89.4%");
        $(".dashboard_selectgroup").css("left", "72%");

        $(".dashboard_choosed_bg_container").attr("flag", "long");
        $(".dashboard_unchoosed_bg_container").attr("flag", "long");

        $("#pageBarlong").css("margin-left", "20%");
        $("#pageBarshort").css("margin-left", "20%");
        //隐藏TripLog之后，更新地图的中心点
        setTimeout(function () {
            BMapObj.setNewMapView(lng, lat, Zoom);
        }, 100);
    });

    $("#hide_trip_log").click(function () {
    
	//显示TripLog之前，保存当前地图的信息
        var mapInfoJson = BMapObj.getMapInfo();
        var obj = eval("(" + mapInfoJson + ")");
        var lng = obj.center.lng;
        var lat = obj.center.lat;
        var Zoom = obj.zoomLevel
        //ArrAllshort = new Array();
        //ArrAlllong = new Array();
        //getVehicleInfoForList(pagegroupid);
        //AppendTrShort(pagenumber);
        /*fengpan 20140408*/
        $("#dashboard_button_hide_line_up").css("width", "77.5%");
        $("#dashboard_button_hide_button_up").css("width", "77.5%");
        $("#dashboard_button_hide_line_down").css("width", "77.5%");
        $("#dashboard_button_hide_button_down").css("width", "77.5%");

        $("#Home_hide_trip_log").hide();
        $("#dashboard_viewalltriplog").show();
        $("#Home_show_trip_log").show();
        $("#dashboard_BMap").css("width", "77.5%");

        //$("#dashboard_hide").hide();
        //$("#dashboard_show").show();
        $("#dashboard_status_bar").css("width", "79%");

        var display = $("#dashboard_vehicleslist_long").css("display");
        if ("none" != display) {
            $("#dashboard_BMap")[0].style.zIndex = -20000;
            $("#dashboard_vehicleslist").show();
            $("#dashboard_vehicleslist_long").hide();
            
        }
        $(".dashboard_viewgroup_text").css("left", "49%");
        $(".dashboard_choosed_bg_container").css("left", "74%");
        $(".dashboard_showmap_bg_container").css("left", "69.6%");
        $(".dashboard_unchoosed_bg_container").css("left", "69.4%");
        $(".dashboard_showlist_bg_container").css("left", "64.8%");
        $(".dashboard_selectgroup").css("left", "54%"); 

        $(".dashboard_choosed_bg_container").attr("flag", "short");
        $(".dashboard_unchoosed_bg_container").attr("flag", "short");

        $("#pageBarlong").css("margin-left", "15%");
        $("#pageBarshort").css("margin-left", "15%");
        //显示TripLog之后，更新地图的中心点
        setTimeout(function () {
            BMapObj.setNewMapView(lng, lat, Zoom);
        }, 100);
    });

    var x = 824;
    if (x != 0) {
        document.getElementById("u_left").style.height = x + "px";
        document.getElementById("u_right").style.height = x + "px";
    }
    
    /***右侧选择车辆下拉单**/
    /*stub*/
    var choose_group = "all";

    Click_List();

    $("#Home_choose_vehicle")[0].disabled = true;
    //fengpan 20140617
    $('#Home_choose_vehicle').prop('disabled', true);
    $('#Home_choose_vehicle').selectpicker('refresh');

    getGroupsData();
    /**chenyangwen**/
    
    getVehiclesData(-1);
    $("#select_group").change(function () {
        $(".paper-input-value").val("");
        var choose_group = document.getElementById("select_group").value;
        $("#Home_choose_vehicle")[0].disabled = true;
        $("#select_group")[0].disabled = true;
        //fengpan 20140617
        $('#Home_choose_vehicle').prop('disabled', true);
        $('#Home_choose_vehicle').selectpicker('refresh');
        $('#select_group').prop('disabled', true);
        $('#select_group').selectpicker('refresh');
        getVehiclesData(choose_group);
        //chenyangwen 20140611 #1357
        var intervalToken = Math.round(Math.random() * 1000000);
        getVehicleInfoForMapUpdate(choose_group, true, intervalToken, true);
        pagenumber = 1;
        pagegroupid = choose_group;
        ArrAllshort = new Array();
        ArrAlllong = new Array();
        //chenyangwen 20140611 #1357
        getVehicleInfoForList(choose_group, intervalToken);
    });
    //liangjiajie#283单一函数功能：选取不同车辆，获得该车的TripLog
    $("#Home_choose_vehicle").change(function () {
        var choose_vehicle = document.getElementById("Home_choose_vehicle").value;
        //chenyangwen 20140611 #1357
        var intervalToken = Math.round(Math.random() * 1000000);
        getTripLogData(choose_vehicle, intervalToken);
        ChooseVehicleUpdateMap(choose_vehicle);
    });
    //20140304caoyandong覆盖物删除
    //ChangeLeft("common_dashboard_cover");
    ChangeLocationTime();

    //	时时刷新画面
	if (realTimeHomeInterval) {
		window.clearInterval(realTimeHomeInterval);
	}


    //  张博 start
    //chenyangwen 20140611 #1357
	var intervalToken = Math.round(Math.random() * 1000000);
	realTimeHomeInterval = window.setInterval(function () {
	    ArrAllshort = new Array();
	    ArrAlllong = new Array();
	    var choose_vehicle = document.getElementById("Home_choose_vehicle").value;
        //chenyangwen 20140611 #1357
	    getTripLogData(choose_vehicle, intervalToken);
	    var choose_group = document.getElementById("select_group").value;
	    //chenyangwen 20140611 #1357
	    getVehicleInfoForMapUpdate(choose_group, false, intervalToken, true);
	    getVehicleInfoForList(choose_group, intervalToken);
	    getvehilceInfoForSnapShort(intervalToken);

	    ChangeLocationTime();

	}, 1 * 60 * 1000);
    //  张博end
    //chenyanwgen 20140611 #1357
	$.ajaxSetup({
	    statusCode: {
	        899: function (data) {
	            window.clearInterval(realTimeHomeInterval);
	        }
	    }
	});
});
/***** fengpan *********/
//chenyangwen 20140611 #1357
function getvehilceInfoForSnapShort(intervalToken) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Dashboard/GetVehicleInformation",
        url: "/" + CompanyID + "/AjaxResponse/GetVehicleInformation",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: { groupID: -1, intervalToken: intervalToken },
        beforeSend: function (XMLHttpRequest) {

            //$("#dashboard_span4_driving").hide();
            //$("#dashboard_span4_parking").hide();
            //$("#dashboard_span4_miss").hide();
            //$("#dashboard_span4_break").hide();
            //$("#dashboard_span4_alert").hide();


            //$("#dashboard_span4_1_driving").hide();
            //$("#dashboard_span4_1_parking").hide();
            //$("#dashboard_span4_1_miss").hide();
            //$("#dashboard_span4_1_break").hide();
            //$("#dashboard_span4_1_alert").hide();

            //$("#dashboard_span4_driving_load").show();
            //$("#dashboard_span4_parking_load").show();
            //$("#dashboard_span4_miss_load").show();
            //$("#dashboard_span4_break_load").show();
            //$("#dashboard_span4_alert_load").show();

            //$("#dashboard_span4_driving_load_long").show();
            //$("#dashboard_span4_parking_load_long").show();
            //$("#dashboard_span4_miss_load_long").show();
            //$("#dashboard_span4_break_load_long").show();
            //$("#dashboard_span4_alert_load_long").show();
        },
        success: function (result) {
            //$("#dashboard_span4_driving_load").hide();
            //$("#dashboard_span4_parking_load").hide();
            //$("#dashboard_span4_miss_load").hide();
            //$("#dashboard_span4_break_load").hide();
            //$("#dashboard_span4_alert_load").hide();

            //$("#dashboard_span4_driving_load_long").hide();
            //$("#dashboard_span4_parking_load_long").hide();
            //$("#dashboard_span4_miss_load_long").hide();
            //$("#dashboard_span4_break_load_long").hide();
            //$("#dashboard_span4_alert_load_long").hide();
            if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
                showBoard(result);
            } else {
                ABCSoft_board();
            }
        }
    });
}
/****** fengpan ********/
/**triplog 变更**/
/**chenyangwen**/
function Click_List() {
    $("#dashboard_unchoosed_bg,#dashboard_showlist_bg").click(
        function list_show() {
            $(".paper-input-value").val("");
            $("#map_model").hide();//fengpan 20140324 #867
            $("#list_model").show();

            //var left = $(".dashboard_choosed_bg_container").css("left");
            var flag = $(".dashboard_choosed_bg_container").attr("flag");
            if ("short" == flag) {
                $("#dashboard_vehicleslist").show();
                $("#dashboard_BMap")[0].style.zIndex=-20000;
                $("#dashboard_choosed_bg_img").removeClass("dashboard_choosed_bg_normal");
                $("#dashboard_choosed_bg_img").addClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").removeClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").addClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg").removeClass("cursor_style");
                $("#dashboard_showlist_bg").removeClass("cursor_style");
                $("#dashboard_choosed_bg").addClass("cursor_style");
                $("#dashboard_showmap_bg").addClass("cursor_style");
            } else if ("long" == flag) {
                $("#dashboard_vehicleslist_long").show();
                $("#dashboard_BMap")[0].style.zIndex = -20000;
                $("#dashboard_choosed_bg_img").removeClass("dashboard_choosed_bg_normal");
                $("#dashboard_choosed_bg_img").addClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").removeClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").addClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg").removeClass("cursor_style");
                $("#dashboard_showlist_bg").removeClass("cursor_style");
                $("#dashboard_choosed_bg").addClass("cursor_style");
                $("#dashboard_showmap_bg").addClass("cursor_style");
            }
            $("#dashboard_unchoosed_bg,#dashboard_showlist_bg").unbind();
            Click_Map();
        }
    );
    
}
function Click_Map() {
    $("#dashboard_choosed_bg,#dashboard_showmap_bg").click(
        function list_show() {
            $("#map_model").show();//fengpan 20140324 #867
            $("#list_model").hide();

            //var left = $(".dashboard_choosed_bg_container").css("left");
            var flag = $(".dashboard_choosed_bg_container").attr("flag");
            if ("short" == flag) {
                $("#dashboard_vehicleslist").hide();
                $("#dashboard_vehicleslist_long").hide();
                $("#dashboard_BMap")[0].style.zIndex = "";
                $("#dashboard_choosed_bg_img").removeClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_choosed_bg_img").addClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").removeClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").addClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_choosed_bg").removeClass("cursor_style");
                $("#dashboard_showmap_bg").removeClass("cursor_style");
                $("#dashboard_unchoosed_bg").addClass("cursor_style");
                $("#dashboard_showlist_bg").addClass("cursor_style");
            } else if ("long" == flag) {
                $("#dashboard_vehicleslist_long").hide();
                $("#dashboard_vehicleslist").hide();
                $("#dashboard_BMap")[0].style.zIndex = "";
                $("#dashboard_choosed_bg_img").removeClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_choosed_bg_img").addClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").removeClass("dashboard_choosed_bg_normal");
                $("#dashboard_unchoosed_bg_img").addClass("dashboard_unchoosed_bg_normal");
                $("#dashboard_choosed_bg").removeClass("cursor_style");
                $("#dashboard_showmap_bg").removeClass("cursor_style");
                $("#dashboard_unchoosed_bg").addClass("cursor_style");
                $("#dashboard_showlist_bg").addClass("cursor_style");
            }
            var groupid = $("#select_group").val();
            $("#dashboard_choosed_bg,#dashboard_showmap_bg").unbind();
            Click_List();
        }
    );
}

//mabiao 20140307 TripLog 封装
var Add_Trip_Log_Height = function (obj) {
    var length = 0;
    var height = 0;

    if (0 == obj.healthStatus) {
        length++;
    }
    
    if (obj.alerts != undefined && obj.alerts.length >0) {
        length ++;
    }
    if (length >= 1) {
        height = (length - 1) * 20;
    }
    return height;
}

var Add_Trip_Log_API = function (obj, start, end) {
    if (start == "" || start == "10101080000") {
        var starttime = LanguageScript.export_undefine;
    }else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || end == "10101080000") {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.endLocation) {
        var location = "";
        var locationTip = "";
        if ($("#" + obj.id + "_location").length == 0) {
            var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
            point.lng = obj.endlocationLng;
            point.lat = obj.endlocationLat;
            point.id = obj.id;
            point.documentID = obj.id + "_location";
            point.flag = 1;
            point.locationFlag = obj.isLastFlag;
            transformLocation.push(point);
        } else {
            location = $("#" + obj.id + "_location")[0].innerHTML;
            locationTip = $("#" + obj.id + "_location")[0].title;
        }
    } else {
        var location = baiduLocationForTrip(obj.endLocation);
        var locationTip = obj.endLocation.replace(/,/g,"");
    }
    if (null == obj.startLocation) {
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.startlocationLng;
        point.lat = obj.startlocationLat;
        point.id = obj.id;
        point.flag = 0;
        point.locationFlag = obj.isFirstFlag;
        transformLocation.push(point);
    }
    var AddHeight = Add_Trip_Log_Height(obj);
    var DetailHeight = 70+ AddHeight;
    var LineHeight = 40 + AddHeight;
    var overflowText = '';
    ihpleDTripHeight += DetailHeight;
    if (ihpleDTripHeight > 710) {
        var lastHeight = 710 - ihpleDTripHeight + DetailHeight;
        if (lastHeight < 24) { return ""; }
        if (lastHeight >= 25 ) { DetailHeight = 25; }
        if (lastHeight >= 51) { DetailHeight = 51; }
        if (lastHeight >= 70) { DetailHeight = 70; }
        if (lastHeight >= 85) { DetailHeight = 85; }
        overflowText = 'overflow:hidden;';
    }
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;' + overflowText + '">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:' + LineHeight + 'px;">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                Add_Trip_Log_Time_API(endtime, starttime) +
            '</div>' +
            '<div id= "' + obj.id + "_location" + '" class="cls_Home_trip_log_alert_info" title=' + locationTip + '>' +
                    location +
            '</div>' +
            Add_Trip_Log_Info(obj) +
        '</div>' +
    '</div>';
}

var Add_Trip_Log_Trailer_API = function (obj, start, end) {
    if (start == "" || start == "10101080000") {
        var starttime = LanguageScript.export_undefine;
    } else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || end == "10101080000") {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.startLocation) {
        var location = "";
        var locationTip = "";
        if ($("#" + obj.id + "_locationTrailer").length == 0) {
            var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
            point.lng = obj.startlocationLng;
            point.lat = obj.startlocationLat;
            point.id = obj.id;
            point.documentID = obj.id + "_locationTrailer";
            point.flag = 0;
            point.locationFlag = obj.isFirstFlag;
            transformLocation.push(point);
        } else {
            location = $("#" + obj.id + "_locationTrailer")[0].innerHTML;
            locationTip = $("#" + obj.id + "_locationTrailer")[0].title;
        }
    } else {
        var location = baiduLocationForTrip(obj.startLocation);
        var locationTip = obj.startLocation.replace(/,/g, "");
    }
    if (null == obj.endLocation) {
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null ,locationFlag:0};
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocation.push(point);
    }
    var DetailHeight = 70 ;
    var LineHeight = 40;
    ihpleDTripHeight += DetailHeight;
    var overflowText = '';
    if (ihpleDTripHeight > 710) {
        var lastHeight = 710 - ihpleDTripHeight + 70;
        if (lastHeight < 24) { return ""; }
        if (lastHeight >= 25 ) { DetailHeight = 25; }
        if (lastHeight > 50) { DetailHeight = 50; }
        overflowText = 'overflow:hidden;';
    }
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;' + overflowText + '">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle_red">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:' + LineHeight + 'px;background-color:#FF0000">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                 Add_Trip_Log_Time_API(endtime, starttime) +
            '</div>' +
            '<div id = "' + obj.id + "_locationTrailer" + '" class="cls_Home_trip_log_alert_info" title=' + locationTip + ' >' +
                    location +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_info" " style="color:#FF0000;">' +
                    LanguageScript.common_Trailer +
            '</div>' +
        '</div>' +
    '</div>';
}

var Add_Trip_Log_Final_API = function (obj, start, end) {
    if (start == "" || start == "10101080000") {
        var starttime = LanguageScript.export_undefine;
    } else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || end == "10101080000") {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.startLocation) {
        var location = "";
        var locationTip = "";
        if ($("#" + obj.id + "_locationFinal").length == 0) {
            var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
            point.lng = obj.startlocationLng;
            point.lat = obj.startlocationLat;
            point.id = obj.id;
            point.documentID = obj.id + "_locationFinal";
            point.flag = 0;
            point.locationFlag = obj.isLastFlag;
            transformLocation.push(point);
        } else {
            location = $("#" + obj.id + "_locationFinal")[0].innerHTML;
            locationTip = $("#" + obj.id + "_locationFinal")[0].title;
        }
    } else {
        var location = baiduLocationForTrip(obj.startLocation);
        var locationTip = obj.startLocation.replace(/,/g, "");
    }
    if (null == obj.endLocation) {
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null ,locationFlag :0};
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocation.push(point);
    }
    var DetailHeight = 50;
    var LineHeight = 40;
    ihpleDTripHeight += DetailHeight;
    var overflowText = '';
    if (ihpleDTripHeight > 710) {
        var lastHeight = 710 - ihpleDTripHeight + 50;
        if (lastHeight < 24) { return ""; }
        if (lastHeight >= 25) { DetailHeight = 25; }
        overflowText = 'overflow:hidden;';
    }
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;' + overflowText + '">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle_final">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:0px;">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                 Add_Trip_Log_Time_API(endtime, starttime) +
            '</div>' +
            '<div id= "' + obj.id + "_locationFinal" + '" class="cls_Home_trip_log_alert_info" title=' + location + '>' +
                    location +
            '</div>' +
        '</div>' +
    '</div>';
}
/************************UI Trip****/
var Add_Trip_Log = function (obj) {
    if (obj.StartTime != "") {
        var starttime = obj.StartTime.substring(8, 10) + ":" + obj.StartTime.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (obj.EndTime != "") {
        var endtime = obj.EndTime.substring(8, 10) + ":" + obj.EndTime.substring(10, 12);
    } else {
        var endtime = "";
    }
    var date_time = obj.StartTime;
    var AddHeight = Add_Trip_Log_Height(obj);
    var DetailHeight = 70 + AddHeight;
    var LineHeight = 40 + AddHeight;
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:' + LineHeight + 'px;">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                Add_Trip_Log_Time(starttime, endtime) +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_info" title=' + obj.endLocation + '>' +
                    obj.endLocation +
            '</div>' +
            Add_Trip_Log_Info(obj) +
        '</div>' +
    '</div>';
}

var Add_Trip_Log_Trailer = function (obj) {
    if (obj.StartTime != "") {
        var starttime = obj.StartTime.substring(8, 10) + ":" + obj.StartTime.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (obj.EndTime != "") {
        var endtime = obj.EndTime.substring(8, 10) + ":" + obj.EndTime.substring(10, 12);
    } else {
        var endtime = "";
    }
    var date_time = obj.StartTime;
    var DetailHeight = 70 ;
    var LineHeight = 40;
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle_red">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:' + LineHeight + 'px;background-color:#FF0000">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                Add_Trip_Log_Time(starttime, endtime) +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_info" title=' + obj.endLocation + ' >' +
                    obj.endLocation +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_info" " style="color:#FF0000;">' +
                    LanguageScript.common_Trailer +
            '</div>' +
            Add_Trip_Log_Info(obj) +
        '</div>' +
    '</div>';
}

var Add_Trip_Log_Final = function (obj) {
    if (obj.StartTime != "") {
        var starttime = obj.StartTime.substring(8, 10) + ":" + obj.StartTime.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (obj.EndTime != "") {
        var endtime = obj.EndTime.substring(8, 10) + ":" + obj.EndTime.substring(10, 12);
    } else {
        var endtime = "";
    }
    var date_time = obj.StartTime;
    var DetailHeight = 50;
    var LineHeight = 40;
    return '<div class="cls_Home_trip_log_alert" style="height:' + DetailHeight + 'px;">' +
        '<div class="cls_Home_trip_log_alert_left">' +
            '<div class="cls_Home_trip_log_alert_circle_final">' +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_line" style="height:0px;">' +
            '</div>' +
        '</div>' +
        '<div class="cls_Home_trip_log_alert_right">' +
            '<div class="cls_Home_trip_log_alert_time">' +
                Add_Trip_Log_Time(starttime, endtime) +
            '</div>' +
            '<div class="cls_Home_trip_log_alert_info" title=' + obj.endLocation + '>' +
                    obj.endLocation +
            '</div>' +
            Add_Trip_Log_Info(obj) +
        '</div>' +
    '</div>';
}
var Add_Trip_Log_Time = function (StartTime, EndTime) {
    var view = "";
    if (StartTime != "") {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_Start" title="' + LanguageScript.common_ArriveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + StartTime + '</div>';
    }
    if (EndTime != "") {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_End" title="' + LanguageScript.common_LeaveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + EndTime + '</div>';
    }
    return view;
}
//chenyangwen 20140611 #1357
function getTripLogData(Vehicle, intervalToken) {
    var CompanyID = GetCompanyID();
    if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
        getTripLogData_API(Vehicle, intervalToken);
        return;
    }
    var xmlhttp = getXmlHttpRequest();
    //mabiao 2014/3/24 #829
    //$("#Home_trip_log").empty();
    //使用post传送异步
    xmlhttp.open('post', '/' + CompanyID + '/Dashboard/GetTripLog', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
				
                var txt = xmlhttp.responseText;
				ihpleDTripHeight = 0;
                if (txt == null) {
                    $("#Home_trip_log").empty();
                    return;
                }
                var obj = eval("(" + txt + ")");
                var view = "";
                //mabiao 20140307 trip Log 封装
                for (var i = 0; i < obj.length; i++) {
                    if ("10101080000" == transTime(obj[i].StartTime).format("yyyyMMddhhmmss")) {
                        obj[i].StartTime = "";
                    } else {
                        obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss")
                    }
                    if ("10101080000" == transTime(obj[i].EndTime).format("yyyyMMddhhmmss")) {
                        obj[i].EndTime = "";
                    } else {
                        obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss")
                    }
                    if (obj[i].type == "Day") {
                        view += day(obj[i].startLocation);
                    } else if (obj[i].type == "Normal") {
                        view += Add_Trip_Log(obj[i]);
                    } else if (obj[i].type == "Trailer") {
                        view += Add_Trip_Log_Trailer(obj[i]);
                    } else if (obj[i].type == "Final") {
                        view += Add_Trip_Log_Final(obj[i]);
                    } else if (obj[i].type == "Driving") {
                        view += Add_Trip_Log_Driving();
                    }
                }
                //mabiao 2014/3/24 #829
                $("#Home_trip_log").empty();
                $("#Home_trip_log").append(view);
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //mabiao 2014/3/24 #829
                $("#Home_trip_log").empty();
                //... ...
            }
        } else {
            //... ...
        }
    }
    var message = 'Vehicle=' + Vehicle + "&intervalToken=" + intervalToken;
    xmlhttp.send(message);
}
/**********************/
var Add_Trip_Log_Driving = function () {
    ihpleDTripHeight += 40;
    if (ihpleDTripHeight > 710) {
        return "";
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:40px'>" +
               "<div class='cls_trip_log_detail_line_arrow'>" +
               "</div></div>";

    return view;
}
//mabiao 20140307 Trip Log 封装
var Add_Trip_Log_Info = function (obj) {
    var view = "";
    if (0 == obj.healthStatus) {
        view += '<div class="cls_Home_trip_log_alert_status" >' + LanguageScript.common_alertTypes_EngineLightOn + '</div>';
    }
    if (obj.alerts != undefined && obj.alerts.length > 0) {
        var string_ = "";
        for (var i = 0; i < obj.alerts.length; i++) {
            if (1 == obj.alerts[i]) {
                string_ += LanguageScript.common_alertTypes_MOTION + " ";
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (0 == obj.alerts[i]) {
                string_ += LanguageScript.common_alertTypes_SPEED + " ";
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (2 == obj.alerts[i]) {
                string_ += LanguageScript.common_alertTypes_EngineRPM + " ";
                break;
            }
        }
        view += '<div class="cls_Home_trip_log_alert_status" >' + string_ + '</div>';
    }
    return view;
}
var day = function (text) {
    ihpleDTripHeight += 55;
    var styleText = '';
    if (ihpleDTripHeight > 710) {
        var lastHeight = 710 -(ihpleDTripHeight-55);
        if (lastHeight >= 25) {
            styleText = 'style="height:' + lastHeight + ';overflow:hidden;"';
        } else {
            return "";
        }
    }
        return  '<div class="cls_Home_trip_log_day" ' + styleText + '>' +
            '<div class="cls_Home_trip_log_day_date">'+
                text +
            '</div>'+
            '<div class="cls_Home_trip_log_day_line">'+
            '</div>'+
        '</div>';
}

var Add_Trip_Log_Time_API = function (EndTime, StartTime) {
    var view = "";
    if (EndTime != "") {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_Start" title="' + LanguageScript.common_ArriveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + EndTime + '</div>';
    }
    if (StartTime != "") {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_End" title="' + LanguageScript.common_LeaveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + StartTime + '</div>';
    }
    return view;
}

function showBoard(result)
{
    if (null == result.drivingVehicle) {
        $("#dashboard_span4_driving").text(0);
        $("#dashboard_span4_1_driving").text(0);
    }
    else {
        $("#dashboard_span4_driving").text(result.drivingVehicle.length);
        $("#dashboard_span4_1_driving").text(result.drivingVehicle.length);
    }
    if (null == result.parkingVehicle) {
        $("#dashboard_span4_parking").text(0);
        $("#dashboard_span4_1_parking").text(0);
    }
    else {
        $("#dashboard_span4_parking").text(result.parkingVehicle.length);
        $("#dashboard_span4_1_parking").text(result.parkingVehicle.length);
    }
    if (null == result.misstargetVehicle) {
        $("#dashboard_span4_miss").text(0);
        $("#dashboard_span4_1_miss").text(0);
    } else {
        $("#dashboard_span4_miss").text(result.misstargetVehicle.length);
        $("#dashboard_span4_1_miss").text(result.misstargetVehicle.length);
    }
    if (null == result.breakVehicle) {
        $("#dashboard_span4_1_break").text(0);
        $("#dashboard_span4_1_break").text(0);
    } else {
        $("#dashboard_span4_break").text(result.breakVehicle.length);
        $("#dashboard_span4_1_break").text(result.breakVehicle.length);
    }
    if (null == result.alertVehicle) {
        $("#dashboard_span4_alert").text(0);
        $("#dashboard_span4_1_alert").text(0);
    } else {
        $("#dashboard_span4_alert").text(result.alertVehicle.length);
        $("#dashboard_span4_1_alert").text(result.alertVehicle.length);
    }
    //Add by LiYing start
    $("#dashboard_span4_driving").show();
    $("#dashboard_span4_parking").show();
    $("#dashboard_span4_miss").show();
    $("#dashboard_span4_break").show();
    $("#dashboard_span4_alert").show();

    //Add by LiYing End
    //$("#dashboard_span4_driving").text(result.drivingVehicle.length);
    //$("#dashboard_span4_parking").text(result.parkingVehicle.length);
    //$("#dashboard_span4_miss").text(result.misstargetVehicle.length);
    //$("#dashboard_span4_break").text(result.breakVehicle.length);
    //$("#dashboard_span4_alert").text(result.alertVehicle.length);

    //$("#dashboard_span4_1_driving").text(result.drivingVehicle.length);
    //$("#dashboard_span4_1_parking").text(result.parkingVehicle.length);
    //$("#dashboard_span4_1_miss").text(result.misstargetVehicle.length);
    //$("#dashboard_span4_1_break").text(result.breakVehicle.length);
    //$("#dashboard_span4_1_alert").text(result.alertVehicle.length);
}

function ABCSoft_board() {
    $("#dashboard_span4_driving").text(2);
    $("#dashboard_span4_parking").text(2);
    $("#dashboard_span4_miss").text(1);
    $("#dashboard_span4_break").text(1);
    $("#dashboard_span4_alert").text(1);

    $("#dashboard_span4_1_driving").text(2);
    $("#dashboard_span4_1_parking").text(2);
    $("#dashboard_span4_1_miss").text(1);
    $("#dashboard_span4_1_break").text(1);
    $("#dashboard_span4_1_alert").text(1);
}

function getVehicleInfoForMap(groupID) {
    arrayVehicles = new Array();
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Dashboard/GetVehicleInformation",
        url: "/" + CompanyID + "/AjaxResponse/GetVehicleInformation",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: "groupID=" + groupID,
        beforeSend: function (XMLHttpRequest) {
            //$("#dashboard_span4_driving_load").show();
            //$("#dashboard_span4_parking_load").show();
            //$("#dashboard_span4_miss_load").show();
            //$("#dashboard_span4_break_load").show();
            //$("#dashboard_span4_alert_load").show();
            $("#dashboard_span4_driving").text(0);
            $("#dashboard_span4_parking").text(0);
            $("#dashboard_span4_miss").text(0);
            $("#dashboard_span4_break").text(0);
            $("#dashboard_span4_alert").text(0);
        },
        success: function (result) {
            //$("#dashboard_span4_driving_load").hide();
            //$("#dashboard_span4_parking_load").hide();
            //$("#dashboard_span4_miss_load").hide();
            //$("#dashboard_span4_break_load").hide();
            //$("#dashboard_span4_alert_load").hide();
            if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
                if (-1 == groupID) {
                    showBoard(result);
                }
            } else {
                ABCSoft_board();
            }
            arrayVehicles = [];
            //循环取得VehicleList中的数据信息
            for (var i = 0; i < result.allVehicle.length; i++) {
                var InfoWinContent = "";
                var iconKind = 1;
                if (1 == result.allVehicle[i].misState) {
                    iconKind = 5
                } else { 
                    if (0 == result.allVehicle[i].engineStatus) {
                        if (3 == result.allVehicle[i].alertType && 0 != result.allVehicle[i].healthStatus) {
                            iconKind = 1;
                        }
                        else {
                            iconKind = 3;
                        }
                    } else {
                        if (3 == result.allVehicle[i].alertType &&  0 != result.allVehicle[i].healthStatus) {
                            iconKind = 2;
                        }
                        else {
                            iconKind = 4;
                        }
                    }
                }
                if (3 == result.allVehicle[i].alertType) {
                    vehiclealert = "OK";
                } else {
                    vehiclealert = LanguageScript.common_alert;
                }
                 if (0 == result.allVehicle[i].healthStatus) {
                    health_status = LanguageScript.common_EngineOn;
                } else if (1 == result.allVehicle[i].healthStatus) {
                    health_status = "OK";
                } else {
                    health_status = LanguageScript.export_undefine;
                }

                InfoWinContent = VehicleInfoWinContent(result.allVehicle[i].logoUrl, result.allVehicle[i].name, result.allVehicle[i].engineTime, health_status, vehiclealert, result.allVehicle[i].primarykey, result.allVehicle[i].driver, result.allVehicle[i].Info, result.allVehicle[i].speed);
                
                //把Vehicle的（VehicleID、车牌号licence经纬度、Marker使用的图标类型、Infowindow信息）进行存储
                 var VehicleInfo = { vehicelID: result.allVehicle[i].primarykey, licence: result.allVehicle[i].license, lng: result.allVehicle[i].location.longitude, lat: result.allVehicle[i].location.latitude, iconKind: iconKind, info: InfoWinContent, name: $.trim(result.allVehicle[i].name) };

                //把一辆Vehicle的信息push到数组arrayVehicles中
                if (!(-0.1 < result.allVehicle[i].location.longitude && 1 > result.allVehicle[i].location.longitude && -0.1 < result.allVehicle[i].location.latitude && 1 > result.allVehicle[i].location.latitude)) {
                    arrayVehicles.push(VehicleInfo);
                }
            }

            //调用此函数，将坐标转换为地址，存储在隐藏标签input中
            saveMapPopupAddress(arrayVehicles);

            //地图画面显示：默认以第一辆车的经纬度为地图中心进行描画
            //(11,1,1,1)
            //11:默认的地图显示Level;displayNavCtl控件显示
            //1:NavigationControl控件显示
            //1:地图支持拖拽
            //1:地图支持鼠标滚轮缩放
            if (0 != arrayVehicles.length) {
                var ihpleD_map = new ihpleD_Map("dashboard_BMap", result.allVehicle[0].longitude, result.allVehicle[0].latitude, 11, 1, 1, 1);
                BMapObj = ihpleD_map.get_mapObj();
                var mapObj = BMapObj.get_mapObj();
                //描画Vehicle的地理位置的Marker
                var vehiclesMap = new ihpleD_ShowVehicles(mapObj, arrayVehicles, true, false, true);
            } else {
                var ihpleD_map = new ihpleD_Map("dashboard_BMap", 116.404, 39.915, 11, 1, 1, 1);
                BMapObj = ihpleD_map.get_mapObj();
                var mapObj = BMapObj.get_mapObj();
            }
            
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

                    setTimeout(function () { addMarkerForCommonAddress(BMapObj, lng, lat); },2000);
                    var a = { lng: lng, lat: lat, zoom: zoom };
                    arrLocationforMarker.push(a);

                } else if (showType == 1) {

                    markerPopupAddressForPopup(lng, lat, ("map_popup_address" + $.trim($("#strSelectForSearch").val())));
                }

                $("#Latitude").attr("value", "");
                $("#Longitude").attr("value", "");
                $("#zoomForSearch").attr("value", "");
                $("#showTypeForSearch").attr("value", "");

                //清空session
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/" + CompanyID + "/Dashboard/CleanSearchPara",
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

/********mabiao****/
/***triplog select***/
function getXmlHttpRequest() {

    if (window.XMLHttpRequest) {// code for all new browsers
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {// code for IE5 and IE6
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    return xmlhttp;
}

function getGroupsData() {
    var CompanyID = GetCompanyID();
    //chenyangwen 从后台获取数组数据并添加到下拉列表中
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Dashboard/GetGroups",
        url: "/" + CompanyID + "/AjaxResponse/GetGroups",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#select_group").empty();
            $("#select_group").append("<option value='-1'>" + LanguageScript.page_Dasboard_FleetLocation_EntireFleet + "</option>");
            for (var i = 0; i < msg.length; ++i) {
                var options = "<option value='" + msg[i].pkid +"'>" + $.trim(msg[i].name) + "</option>";
                $("#select_group").append(options);
                //fengpan 20140617
                $('#select_group').selectpicker('refresh');
            }
        }
    });
}

//liangjiajie#283查看更多triplog函数
function getVehiclesAllTripLog(){
    $("#dashboard_viewalltriplog").click(function () {
        trans();
        var choose_vehicle = document.getElementById("Home_choose_vehicle").value;
        var CompanyID = GetCompanyID();
        var url = "/" + CompanyID + "/Vehicles/Detail?VehicleID=" + choose_vehicle;
        location.href = url;
    });
}

function getVehiclesData(groupID) {
    var CompanyID = GetCompanyID();
    //chenyangwen 根据车族从后台获取该车组中车辆数据并添加到下拉列表中
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Dashboard/getVehicles",
        url: "/" + CompanyID + "/AjaxResponse/getVehicles",
        data: { groupID: groupID },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#Home_choose_vehicle").empty();
            //liangjiajie
            //mabiao 2014/3/24 #829
            //$("#Home_trip_log").empty();
            $("#Home_choose_vehicle")[0].disabled = false;
            $("#select_group")[0].disabled = false;
            //fengpan 20140617
            $('#Home_choose_vehicle').prop('disabled', false);
            $('#Home_choose_vehicle').selectpicker('refresh');
            $('#select_group').prop('disabled', false);
            $('#select_group').selectpicker('refresh');
            //Add by gaoqingbo 
            //var strSelect = getUrlParam("strSelect");
            var strSelect = $("#strSelectForSearch").val();
            var chooseVehivleValue = "";
            var ajaxPara = this.data;//当前取得参数的方法取出参数的格式是ParameterKey=ParameterValue
            for (var i = 0; i < msg.length; ++i) {

                if (strSelect == $.trim(msg[i].pkid) && ajaxPara == "groupID=-1") {
                    chooseVehivleValue = $.trim(msg[i].pkid);
                }
                var options = "<option value='" + $.trim(msg[i].pkid) + "'>" + $.trim(msg[i].name) + "</option>";
                $("#Home_choose_vehicle").append(options);
            }
            
            // Add by gaoqingbo 
            var firstVehicleID;
            if (chooseVehivleValue) {
                $("#Home_choose_vehicle option[value='" + chooseVehivleValue + "']").attr("selected", "selected");
                firstVehicleID = chooseVehivleValue;
            } else {
                firstVehicleID = $("#Home_choose_vehicle").children().first().val();
            }
            //fengpan 20140617
            $("#Home_choose_vehicle").selectpicker('refresh');
            
            //liangjiajie#283
            if (firstVehicleID) {
                //chenyangwen 20140611 #1357
                var intervalToken = Math.round(Math.random() * 1000000);
                getTripLogData(firstVehicleID, intervalToken);
                getVehiclesAllTripLog();//toneup处理
                $("#dashboard_viewalltriplog").css("cursor", "pointer");
                $("#dashboard_viewalltriplog").css("background-color", "#FFF");
            } else {
                //mabiao 2014/3/24 #829
                $("#Home_trip_log").empty();
                $("#dashboard_viewalltriplog").unbind();//tonedown处理
                $("#dashboard_viewalltriplog").css("cursor", "default");
                $("#dashboard_viewalltriplog").css("background-color", "rgb(177, 172, 172)");
            }
                //liangjiajie#283
        },
        error: function () {
            $("#select_group")[0].disabled = false;
            $("#Home_choose_vehicle")[0].disabled = false;
            //fengpan 20140617
            $('#Home_choose_vehicle').prop('disabled', false);
            $('#Home_choose_vehicle').selectpicker('refresh');
            $('#select_group').prop('disabled', false);
            $('#select_group').selectpicker('refresh');
        }
    });
    
}
/*********trip log*****/
//chenyangwen 20140611 #1357
function getTripLogData_API(Vehicle, intervalToken) {
    var CompanyID = GetCompanyID();
    var timeZone = new Date().getTimezoneOffset() / 60 * - 1;
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Dashboard/GetTrips",
        url: "/" + CompanyID + "/AjaxResponse/GetTrips",
        data: { vehicleID: Vehicle, timeZone: timeZone, intervalToken:intervalToken },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            if (null == msg || msg.length == 0) {
                $("#Home_trip_log").empty();
                return;
            }
            transformLocation = new Array();
            ihpleDTripHeight = 0;
            var date = new Date();
            ihpleDTripDate = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
            var obj = msg;
            var view = "";
            //mabiao 20140307 trip Log 封装
            for (var i = 0; i < obj.length; i++) {
                if (ihpleDTripHeight > 710) {
                    break;
                }
                obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss");
                obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss");
                obj[i].distance = (obj[i].distance == null ? 0 : obj[i].distance).toFixed(1);
                if (0 == i) {
                    //TODO 虚线
                    
                    if (obj[i].EndTime == "10101080000") {   //正在行驶的车 到达时间为当前时间加一年 
                        view += Add_Trip_Log_Driving();
                        view += DayJudge(obj[i]);
                    } else {
                        view += DayJudge(obj[i]);
                        view += Add_Trip_Log_API(obj[i], "none", obj[i].EndTime);
                    }
                    //obj.StartTime 不是今天
                    //view += day("昨天"/"date");
                    
                } else {
                    if (obj[i].isLastFlag != null && obj[i].isLastFlag!= 0 && obj[i - 1].startlocationGPSLng != null && obj[i - 1].startlocationGPSLat != null && obj[i - 1].startlocationGPSLng != 0 && obj[i - 1].startlocationGPSLat != 0 &&
                        obj[i - 1].isFirstFlag != null && obj[i - 1].isFirstFlag != 0 && obj[i].endlocationGPSLng != null && obj[i].endlocationGPSLat != null && obj[i].endlocationGPSLng != 0 && obj[i].endlocationGPSLat != 0 &&
                        GetDistance(obj[i - 1].startlocationGPSLat, obj[i - 1].startlocationGPSLng, obj[i].endlocationGPSLat, obj[i].endlocationGPSLng) > TrailerDistance) {
                        view += Add_Trip_Log_Trailer_API(obj[i - 1], obj[i - 1].StartTime, "none");
                        view += DayJudge(obj[i]);
                        view += Add_Trip_Log_API(obj[i], "none", obj[i].EndTime);
                        view += DayJudge(obj[i]);
                    }else{
                        view += Add_Trip_Log_API(obj[i], obj[i - 1].StartTime, obj[i].EndTime);
                        view += DayJudge(obj[i]);
                    }
                    //obj[i].StartTime 与 obj[i-1].StartTime 比较
                    //view += day("昨天"/"date");
                    
                }// 第一次trip 判断
                if (i == (msg.length-1)) {
                    view += Add_Trip_Log_Final_API(obj[i], obj[i].StartTime, "none");
                }
            }
            $("#Home_trip_log").empty();
            $("#Home_trip_log").append(view);
            for (var k = 0; k < transformLocation.length; ++k) {
                tripcoderLocation(transformLocation[k].lng, transformLocation[k].lat, transformLocation[k].documentID, transformLocation[k].id, transformLocation[k].flag, transformLocation[k].locationFlag,
                                function (adr, id, flag, ElementID) {
                                    var pointLoca = baiduLocationForTrip(adr);

                                    $("#" + ElementID + "").empty();
                                    $("#" + ElementID + "").append(function () {
                                        return "" + pointLoca + "";
                                    });
                                    $("#" + ElementID + "").attr("title", adr.replace(/,/g, ""));
                                    var url = "/Dashboard/WriteLocationToDB"
                                    LocationWriteToDB(url, id, flag, adr);
                });
            }
        }
    });

}
/*******mabiao***/
var DayJudge = function (obj) {
    if ("10101080000" != obj.StartTime) {
        var dayMinus = minusDay(ihpleDTripDate, obj.StartTime);
        var dayNow = minusDay(ihpleDTripNow, obj.StartTime);
        if (1 == dayNow && 1 == dayMinus) {
            ihpleDTripDate = obj.StartTime;
            return day(LanguageScript.common_yesterday);
        } else if (1 <= dayMinus) {
            ihpleDTripDate = obj.StartTime;
            return day(obj.StartTime.substr(4, 2) + LanguageScript.common_month + obj.StartTime.substr(6, 2) + LanguageScript.common_day);
        } else {
            return "";
        }
    } else if ("10101080000" != obj.EndTime) {
        var dayMinus = minusDay(ihpleDTripDate, obj.EndTime);
        var dayNow = minusDay(ihpleDTripNow, obj.EndTime);
        if (1 == dayNow && 1 == dayMinus) {
            ihpleDTripDate = obj.EndTime;
            return day(LanguageScript.common_yesterday);
        } else if (1 <= dayMinus) {
            ihpleDTripDate = obj.EndTime;
            return day(obj.EndTime.substr(4, 2) + LanguageScript.common_month + obj.EndTime.substr(6, 2) + LanguageScript.common_day);
        } else {
            return "";
        }
    }
}
var minusDay = function (day1, day2) {
    return ((new Date(day1.substr(0, 4) + "/" + day1.substr(4, 2) + "/" + day1.substr(6, 2)) -
            new Date(day2.substr(0, 4) + "/" + day2.substr(4, 2) + "/" + day2.substr(6, 2)))/
            (24 * 60 * 60 *1000));
}
/*******Yueqingqing***/
//VehicleInfoWin
function VehicleInfoWinContent(ImageUrl, name, engine_on_time, health_status, alert, VehicleID, driver, motorcycle_type, speed) {
    if (null == engine_on_time) {
        engine_on_time = "";
    }
    else {
        engine_on_time = stringToHourMin(engine_on_time);
    }
    var CompanyID = GetCompanyID();
    var health_style = '';
    var alert_style = '';
    var speedStr = "";
    if (health_status == "OK") {
        health_style = "style = 'color:#339900'";
    } else if (health_status == LanguageScript.export_undefine) {
        health_style = "style = 'color:#424242'";
    }
    if (alert == "OK") {
        alert_style = "style = 'color:#339900'";
    }

    if (name) {
        nameStr = $.trim(name);
    } else {
        nameStr = LanguageScript.export_undefine;
    }

    if (driver) {
        driverStr = $.trim(driver);
    } else {
        driverStr = LanguageScript.export_undefine;
    }

    if (motorcycle_type) {
        motorcycle_typetr = $.trim(motorcycle_type);
    } else {
        motorcycle_typetr = LanguageScript.export_undefine;
    }

    if (null == speed || speed ==0)
    {
        speedStr = "";
    }
    else {
        speedStr = speed + LanguageScript.common_KmPerHour;
    }
    //20140304caoyandong 改动title：发动机运行时间和健康状况
    return "<div id='VehiclesInfo_style" + VehicleID + "' class ='VehiclesInfo_style'>" +
                "<div id = 'VehiclesInfo_left' class='VehiclesInfo_left_style'>" +
                    "<img title='" + driverStr + "' style='float:left; id='imgDemo2' src='/" + CompanyID + "/AjaxResponse/DrawImage?vehicleID=" + VehicleID + "&type=vehicleLogo' width='66' height='66'/>" +
                "</div>" +
                "<div id = 'VehiclesInfo_right' class='VehiclesInfo_right_style'>" +
                    "<div id = 'VehiclesInfo_Name' class='VehiclesInfo_Name_style' title='" + motorcycle_typetr + "'>" +
                        nameStr +
                    "</div>" +
                    "<div class='VehiclesInfo_Location_style'>" +
                        "<div id = 'VehiclesInfo_Location" + VehicleID + "' class='VehiclesInfo_Location_text_style' title = ''>" +
                        "</div>" +
                    "</div>" +
                    "<div  class='VehiclesInfo_Status_icon_style'></div>" +
                    "<div id = 'VehiclesInfo_EngineOnTime' class='VehiclesInfo_EngineOnTime_style' title='" + LanguageScript.page_dashboard_EngnieOnTime + "'>" +
                        engine_on_time +
                    "</div>" +
                    "<div id = 'VehiclesInfo_Speed' class='VehiclesInfo_Speed_style' title='" + LanguageScript.page_dashboard_speed + "'>" + speedStr +
                    "</div>" +
                    "<div id = 'VehiclesInfo_Status' class='VehiclesInfo_Status_style' " + health_style +
                    "title='" + LanguageScript.common_HealthStatus + "'>" +
                        health_status +
                    "</div>" +
                    "<div  class='VehiclesInfo_Alert_icon_style'></div>" +
                    "<div id = 'VehiclesInfo_Status' class='VehiclesInfo_Alert_style'" + alert_style +
                    "title='" + LanguageScript.common_VehicleAlerts + "'>" +
                        alert +
                    "</div>" +
                    "<a href= \"/" + CompanyID + "/Vehicles/Detail?VehicleID=" + VehicleID + "\" onclick='trans()'>" +
                    "<div id = 'VehiclesInfo_Status' class='VehiclesInfo_view_detail_style'>" + LanguageScript.page_Dasboard_FleetLocation_ViewDetails + "</div>" +
                    "</a>" +
                "</div>" +
                "<div id='VehiclesInfo_Arrow' class ='VehiclesInfo_Arrow_style'>"+
                    "<div id='VehiclesInfo_ArrowIcon' class ='VehiclesInfo_ArrowIcon_style'></div>" +
                "</div>" +
        "</div>" +
    "</div>"
}
/* chenyanwgen */
function trClick(vehicleID) {
    $("#Home_choose_vehicle").val(vehicleID);
    //chenyangwen 20140611 #1357
    var intervalToken = Math.round(Math.random() * 1000000);
    getTripLogData(vehicleID, intervalToken);
}
/* chenyanwgen */



/*liangjiajie0312调整主界面列表车辆排序，参考landing.js getGroupIDs() 排序函数*/
/*     此处引用VihiclesController.cs 中的action   注明影响范围*/
//chenyangwen 20140611 #1357
function getVehicleInfoForList(groupID, intervalToken) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetGroups",
        url: "/" + CompanyID + "/AjaxResponse/GetGroupsInVehicle",
        data: { intervalToken: intervalToken },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            showVehicleInfoList(msg, groupID);
        }
    });
}

//车辆列表显示
function showVehicleInfoList(groupids,groupID) {
    var vehicleListAllNum = 0;
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetVehicleInformation",
        url: "/" + CompanyID + "/AjaxResponse/GetVehicleInformationInVehicle",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {},
        success: function (result) {
            ArrAllshort = new Array();
            ArrAlllong = new Array();
            ArrLocationAll = new Array();
            var flag1 = 0;
            var flag2 = 0;
            /* chenyangwen 2014/02/10 */
            $("#vehicle_list_table").empty();
            $("#vehicle_list_long_table").empty();

            /*liangjiajie*/
            var head = '<tr style="height: 50px; font-family:Microsoft YaHei; font-weight: bolder;background-color: #f1f1f1 ">' +
                       '<td style="width:12%; padding-left:10px;  word-break:break-all;  ">' + LanguageScript.page_dashboard_LocatedGroup + '</td>' + //liangjiajie0311
                       '<td style="width:12%;"   >' + LanguageScript.common_pic + '</td>' +
                       '<td style="width:12%;  word-break:break-all;  ">' + LanguageScript.common_vehicleName + '</td>' +

                       //'<td style="width:12%;  word-break:break-all;  ">' + LanguageScript.page_dashboard_LocatedGroup + '</td>' + //liangjiajie0311

                       '<td style="width:16%;  word-break:break-all;  ">' + LanguageScript.common_LocationAlert + '</td>' +
                       '<td style="width:12%;  word-break:break-all; ">' + LanguageScript.page_vehicles_VehiclesContinuous + '<br>' + LanguageScript.page_vehicles_TravelTime + '</td>' +
                       '<td style="width:15%;  word-break:break-all; ">' + LanguageScript.common_total_mileage + '</td>' +
                       '<td style="width:12%;  word-break:break-all;  ">' + LanguageScript.common_VehicleHealth + '</td>' +
                       '<td style="width:9%;  padding-left:10px; ">' +
                          '<div class="Dashaboard_List_title" style="display:block;"></div></td></tr>';//liangjiajie
            $("#vehicle_list_table").append(head);

            /*liangjiajie*/


            var head_long = '<tr style="height: 50px;font-family: Microsoft YaHei; font-weight: bolder;background-color: #f1f1f1">' +
                       '<td  style=" width:11%; padding-left:10px;  word-break:break-all;">' + LanguageScript.page_dashboard_LocatedGroup + '</td>' + //liangjiajie0311
                       '<td  style=" width:11%;   " >' + LanguageScript.common_pic + '</td>' +
                       '<td  style=" width:15%;  word-break:break-all;">' + LanguageScript.common_vehicleName + '</td>' +

                       //'<td  style=" width:11%;  word-break:break-all;">' + LanguageScript.page_dashboard_LocatedGroup + '</td>' + //liangjiajie0311

                       '<td  style=" width:21%;  word-break:break-all;">' + LanguageScript.common_LocationAlert + '</td>' +
                       '<td  style=" width:15%;  word-break:break-all;">' + LanguageScript.page_Dashboard_EngineOnTime_hitInfo + '</td>' +
                       '<td  style=" width: 12%;  word-break:break-all;">' + LanguageScript.common_total_mileage + '</td>' +
                       '<td  style=" width: 9%;  word-break:break-all;">' + LanguageScript.common_VehicleHealth + '</td>' +
                       '<td  style=" width: 6%;  padding-left:10px;   ">' +
                          '<div class="Dashaboard_List_title" style="display:block;"></div></td></tr>';//liangjiajie
            $("#vehicle_list_long_table").append(head_long);
            /* chenyangwen 2014/02/10 */

            for (var j = 0; j < groupids.length; j++) {
                flag2 = 0;
                for (var i = 0; i < result.allVehicle.length; i++) {
                    var InfoWinContent = "";

                    if ( (result.allVehicle[i].groupID == groupids[j].pkid) && (groupID == result.allVehicle[i].groupID || groupID == -1) ) {
                        flag2 = 1;
                        //if (3 == result.allVehicle[i].alertType && 1 == result.allVehicle[i].healthStatus) {

                        //    //health_status = "OK";
                        //    vehiclealert = LanguageScript.page_vehicles_Health;
                        //    vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#339900">' + vehiclealert + '</div>'
                        //} else if (3 == result.allVehicle[i].alertType && 1 != result.allVehicle[i].healthStatus) {
                        //    //health_status = LanguageScript.common_EngineOn;
                        //    vehiclealert = LanguageScript.page_vehicles_Health;
                        //    vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#339900">' + vehiclealert + '</div>'

                        //} else if (3 != result.allVehicle[i].alertType && 1 == result.allVehicle[i].healthStatus) {
                        //    //health_status = "OK";
                        //    vehiclealert = LanguageScript.common_EngineOn;
                        //    vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#ff0000">' + vehiclealert + '</div>'
                        //} else {
                        //    //health_status = LanguageScript.common_EngineOn;
                        //    vehiclealert = LanguageScript.common_EngineOn;
                        //    vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#ff0000">' + vehiclealert + '</div>'
                        //}
                        if ( 1 == result.allVehicle[i].healthStatus) {
                            health_status = LanguageScript.page_vehicles_Health;
                            vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#339900">' + health_status + '</div>'
                        } else if (0 == result.allVehicle[i].healthStatus) {
                            health_status = LanguageScript.common_EngineOn;
                            vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#ff0000">' + health_status + '</div>'
                        }
                        else {
                            health_status = LanguageScript.export_undefine;
                            vehiclealertDiv = '<div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;color:#424242">' + health_status + '</div>'
                        }
                        //var CompanyID = GetCompanyID();
                        /* chenyangwen 2014/02/10 */
                        /*Redmind#106liangjiajie0306*/


                        //车辆未分组时显示未分组liangjiajie0321还没加到资源文件
                        if (-2 == result.allVehicle[i].groupID) {
                            result.allVehicle[i].groupName = "未分组";
                        }
                        //地址未转换完成前显示""
                        if (null == result.allVehicle[i].location) {
                            result.allVehicle[i].locationName = "";
                        }

                        if (null == result.allVehicle[i].engineTime) {
                            result.allVehicle[i].engineTime = "";
                        }
                        else {
                            result.allVehicle[i].engineTime = stringToHourMin(result.allVehicle[i].engineTime);
                        }
                        var tr = '';
                        if ( vehicleListAllNum % 2 == 1) {
                            tr = '<tr style="background-color:#f7f7f7;height: 50px; font-family:Microsoft YaHei;" onclick="trClick(' + result.allVehicle[i].primarykey + ')">';
                        }
                        else {
                            tr = '<tr style="background-color:#e6e6e6;height: 50px; font-family:Microsoft YaHei;" onclick="trClick(' + result.allVehicle[i].primarykey + ')">';
                        }

                        tr += '<td style="width:12%;padding-left:10px;" id="vehicles_groupName' + result.allVehicle[i].primarykey + '" class="vehicles_groupName"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].groupName) + '">' + result.allVehicle[i].groupName + '</div></td>' +
                                 '<td style="width:12%;"          ><img src="/' + CompanyID + '/AjaxResponse/DrawImage?vehicleID=' + result.allVehicle[i].primarykey + '&type=vehicleLogo" alt="' + LanguageScript.common_NoPic + '" style="height:56px;width:55px;"></td>' +
                                 '<td style="width:12%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].name) + '">' + result.allVehicle[i].name + '</div></td>' +

                                 //'<td style="width:12%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].groupName) + '">' + result.allVehicle[i].groupName + '</div></td>' +

                                 '<td style="width:16%;"><div class="locationName' + result.allVehicle[i].primarykey + '" style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;"></div></td>' +
                                 '<td style="width:12%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].engineTime) + '">' + result.allVehicle[i].engineTime + '</div></td>' +
                                 '<td style="width:15%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].odometer) + LanguageScript.unit_km + '">' + $.trim(result.allVehicle[i].odometer) + LanguageScript.unit_km + '</div></td>' +
                                 '<td style="width:12%;">' + vehiclealertDiv + '</td>' +
                                 '<td style="width:9%; padding-left:10px;">' +
                                    '<div class="Dashaboard_List" style="display:block;"><a href="/' + CompanyID + '/Vehicles/Detail?VehicleID=' + result.allVehicle[i].primarykey + '" style="color:blue;" onclick="trans()">' + LanguageScript.page_Dasboard_FleetLocation_ViewDetails + '</div></a></td></tr>';
                        //$("#vehicle_list_table").append(tr);

                        //geocoderLocationForVehicleList(result.allVehicle[i].location.longitude, result.allVehicle[i].location.latitude, "locationName" + result.allVehicle[i].primarykey);
                        /*liangjiajie*/
                        var tr_long = '';
                        if ( vehicleListAllNum % 2 == 1) {
                            tr_long = '<tr style="background-color:#f7f7f7;height: 50px;font-size: 10pt; font-family:Microsoft YaHei;" onclick="trClick(' + result.allVehicle[i].primarykey + ')">';
                        }
                        else {
                            tr_long = '<tr style="background-color:#e6e6e6;height: 50px; font-size: 10pt; font-family:Microsoft YaHei;" onclick="trClick(' + result.allVehicle[i].primarykey + ')">';
                        }

                        tr_long += '<td  style=" width:11%;padding-left:10px;" id="vehicles_groupName_long' + result.allVehicle[i].primarykey + '" class="vehicles_groupName_long"><div style="width:100%x; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].groupName) + '">' + result.allVehicle[i].groupName + '</div></td>' +
                                '<td  style=" width:11%;   "><img src="/' + CompanyID + '/AjaxResponse/DrawImage?vehicleID=' + result.allVehicle[i].primarykey + '&type=vehicleLogo" alt="' + LanguageScript.common_NoPic + '" style="height:56px;width:55px;"></td>' +
                                '<td  style=" width:15%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].name) + '">' + result.allVehicle[i].name + '</div></td>' +

                                //'<td  style=" width:11%;"><div style="width:100%x; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].groupName) + '">' + result.allVehicle[i].groupName + '</div></td>' +

                                '<td  style=" width:21%;"><div class="locationName' + result.allVehicle[i].primarykey + '" style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;"></div></td>' +
                                '<td  style=" width:15%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].engineTime) + '">' + result.allVehicle[i].engineTime + '</div></td>' +
                                '<td  style=" width: 12%;"><div style="width:100%; white-space:nowrap; overflow:hidden;text-overflow:ellipsis;" title="' + $.trim(result.allVehicle[i].odometer) + LanguageScript.unit_km + '">' + $.trim(result.allVehicle[i].odometer) + LanguageScript.unit_km + '</div></td>' +
                                '<td  style=" width: 9%;">' + vehiclealertDiv + '</td>' +
                                '<td  style=" width: 6%; padding-left:10px;">' +
                                    '<div class="Dashaboard_List" style="display:block;"><a href="/' + CompanyID + '/Vehicles/Detail?VehicleID=' + result.allVehicle[i].primarykey + '" style="color:blue;" onclick="trans()">' + LanguageScript.page_Dasboard_FleetLocation_ViewDetails + '</div></a></td></tr>';
                        //$("#vehicle_list_long_table").append(tr_long);
                        geocoderLocationForVehicleList(result.allVehicle[i].location.longitude, result.allVehicle[i].location.latitude, "locationName" + result.allVehicle[i].primarykey);
                        /*Redmind#106liangjiajie0306*/
                        /* chenyangwen 2014/02/10 */
                        ArrAllshort.push(tr);
                        ArrAlllong.push(tr_long);
                        ArrLocationOne.push(result.allVehicle[i].location.longitude);
                        ArrLocationOne.push(result.allVehicle[i].location.latitude);
                        ArrLocationOne.push(result.allVehicle[i].primarykey);
                        ArrLocationAll.push(ArrLocationOne);
                        ArrLocationOne = new Array();
                    }
                }
                if (flag2 == 1) {
                    flag1++;
                    vehicleListAllNum++;
                }
            }
            if (parseInt(ArrAllshort.length % $("#CountPerShortPage").val()) == 0) {
                totalnum_short = parseInt(ArrAllshort.length / $("#CountPerShortPage").val())
            }
            else {
                totalnum_short = parseInt(ArrAllshort.length / $("#CountPerShortPage").val()) + 1;
            }
            if (parseInt(ArrAlllong.length % $("#CountPerTallPage").val()) == 0) {
                totalnum_tall = parseInt(ArrAlllong.length / $("#CountPerTallPage").val());
            }
            else {
                totalnum_tall = parseInt(ArrAlllong.length / $("#CountPerTallPage").val()) + 1;
            }
            totalelement = ArrAllshort.length;
            if ($("#dashboard_button_hide_button_down")[0].style.display != "none") {
                AppendTrShort(pagenumber);
            } else {
                AppendTrTall(pagenumber);
            }
            
        }
    });
}
function AppendTrShort(pagenumber) {
    $("#vehicle_list_table tr:not(:first)").remove();
    $("#vehicle_list_long_table tr:not(:first)").remove();
    var ArrCurrentshort = new Array();
    var ArrCurrentlong = new Array();
    if (ArrAllshort.length - (pagenumber - 1) * $("#CountPerShortPage").val() >= $("#CountPerShortPage").val()) {
        for (var i = 0; i < $("#CountPerShortPage").val() ; ++i) {
            ArrCurrentshort.push(ArrAllshort[(pagenumber - 1) * $("#CountPerShortPage").val() + i]);
            ArrCurrentlong.push(ArrAlllong[(pagenumber - 1) * $("#CountPerShortPage").val() + i]);
        }
    }
    else {
        if (ArrAllshort.length < $("#CountPerShortPage").val())
            {
                for (var i = 0; i < ArrAllshort.length; ++i) {
                    ArrCurrentshort.push(ArrAllshort[i]);
                    ArrCurrentlong.push(ArrAlllong[i]);
                }
            }
            else{
            for (var i = 0; i < ArrAllshort.length - (pagenumber - 1) * $("#CountPerShortPage").val() ; ++i) {
                ArrCurrentshort.push(ArrAllshort[(pagenumber - 1) * $("#CountPerShortPage").val() + i]);
                ArrCurrentlong.push(ArrAlllong[(pagenumber - 1) * $("#CountPerShortPage").val() + i]);
                }
            }
        }
    for (var i = 0; i < ArrCurrentshort.length; ++i) {
        $("#vehicle_list_long_table").append(ArrCurrentlong[i]);
        $("#vehicle_list_table").append(ArrCurrentshort[i]);
        
    }
    for (var i = 0; i < ArrLocationAll.length; ++i) {
        geocoderLocationForVehicleList((ArrLocationAll[i])[0], (ArrLocationAll[i])[1], "locationName" + (ArrLocationAll[i])[2]);
    }
    //fengpan 20140414 #600
    if (null == $(".vehicles_groupName") || 0 == $(".vehicles_groupName").length) {
        //...
    }
    else {
        var firstRow = $(".vehicles_groupName");
        var groupsCnts = new Array();
        var groupCnt = 0;
        var j = 0;
        var cnt = 1;
        for (var i = 0; i < firstRow.length - 1; i++) {
            if (firstRow[i].children[0].innerHTML == firstRow[i + 1].children[0].innerHTML) {
                cnt++;
                continue;
            }
            else {
                groupCnt++;
                groupsCnts[j] = cnt;
                cnt = 1;
                j++;
            }
        }
        var removeNum = 1;
        groupsCnts[j] = cnt;
        for (var i = 0; i < groupsCnts.length; i++) {
            $("#" + $(".vehicles_groupName")[i].id).attr("rowspan", groupsCnts[i]);
            $("#" + $(".vehicles_groupName")[i].id).css("border-right", "1px solid #ddd");
            for (var j = 1; j < groupsCnts[i]; j++) {
                firstRow[removeNum + j - 1].parentNode.removeChild(firstRow[removeNum + j - 1]);
                //firstRow[removeNum+j-1].remove();
            }
            removeNum += groupsCnts[i];
        }
    }
    /**/
    if (null == $(".vehicles_groupName_long") || 0 == $(".vehicles_groupName_long").length) {
    }
    else {
        var firstRow_long = $(".vehicles_groupName_long");
        var groupsCnts_long = new Array();
        var groupCnt_long = 0;
        j = 0;
        cnt = 1;
        for (var i = 0; i < firstRow_long.length - 1; i++) {
            if (firstRow_long[i].children[0].innerHTML == firstRow_long[i + 1].children[0].innerHTML) {
                cnt++;
                continue;
            }
            else {
                groupCnt_long++;
                groupsCnts_long[j] = cnt;
                cnt = 1;
                j++;
            }
        }
        groupsCnts_long[j] = cnt;
        removeNum = 1;
        for (var i = 0; i < groupsCnts_long.length; i++) {
            $("#" + $(".vehicles_groupName_long")[i].id).attr("rowspan", groupsCnts_long[i]);
            $("#" + $(".vehicles_groupName_long")[i].id).css("border-right", "1px solid #ddd");
            for (var j = 1; j < groupsCnts_long[i]; j++) {
                firstRow_long[removeNum + j - 1].parentNode.removeChild(firstRow_long[removeNum + j - 1]);
                //firstRow_long[removeNum+j-1].remove();
            }
            removeNum += groupsCnts_long[i];
        }
        //fengpan 20140414 #600
    }
    PageCount = totalnum_short;
    if (PageCount == 0) {
        PageCount = 1;
    }
    var data = { 'pagecount': PageCount };
    $("#pageBarshort").pager({ pagenumber: pagenumber, pagecount: data.pagecount, buttonClickCallback: PageClick });
    $("#pageBarlong").pager({ pagenumber: pagenumber, pagecount: data.pagecount, buttonClickCallback: PageClick });
}
function AppendTrTall(pagenumber) {
    $("#vehicle_list_table tr:not(:first)").remove();
    $("#vehicle_list_long_table tr:not(:first)").remove();
    var ArrCurrentshort = new Array();
    var ArrCurrentlong = new Array();
    if (ArrAllshort.length - (pagenumber - 1) * $("#CountPerTallPage").val() >= $("#CountPerTallPage").val()) {
        for (var i = 0; i < $("#CountPerTallPage").val() ; ++i) {
            ArrCurrentshort.push(ArrAllshort[(pagenumber - 1) * $("#CountPerTallPage").val() + i]);
            ArrCurrentlong.push(ArrAlllong[(pagenumber - 1) * $("#CountPerTallPage").val() + i]);
        }
    }
    else {
        if (ArrAllshort.length < $("#CountPerTallPage").val())
            {
            for (var i = 0; i < ArrAllshort.length; ++i) {
                    ArrCurrentshort.push(ArrAllshort[i]);
                    ArrCurrentlong.push(ArrAlllong[i]);
                }
            }
            else{
            for (var i = 0; i < ArrAllshort.length - (pagenumber - 1) * $("#CountPerTallPage").val() ; ++i) {
                ArrCurrentshort.push(ArrAllshort[(pagenumber - 1) * $("#CountPerTallPage").val() + i]);
                ArrCurrentlong.push(ArrAlllong[(pagenumber - 1) * $("#CountPerTallPage").val() + i]);
                }
            }
        }
    for (var i = 0; i < ArrCurrentshort.length; ++i) {
        $("#vehicle_list_long_table").append(ArrCurrentlong[i]);
        $("#vehicle_list_table").append(ArrCurrentshort[i]);
    }
    for (var i = 0; i < ArrLocationAll.length; ++i) {
        geocoderLocationForVehicleList((ArrLocationAll[i])[0], (ArrLocationAll[i])[1], "locationName" + (ArrLocationAll[i])[2]);
    }
    //fengpan 20140414 #600
    if (null == $(".vehicles_groupName") || 0 == $(".vehicles_groupName").length) {
        //...
    }
    else {
        var firstRow = $(".vehicles_groupName");
        var groupsCnts = new Array();
        var groupCnt = 0;
        var j = 0;
        var cnt = 1;
        for (var i = 0; i < firstRow.length - 1; i++) {
            if (firstRow[i].children[0].innerHTML == firstRow[i + 1].children[0].innerHTML) {
                cnt++;
                continue;
            }
            else {
                groupCnt++;
                groupsCnts[j] = cnt;
                cnt = 1;
                j++;
            }
        }
        var removeNum = 1;
        groupsCnts[j] = cnt;
        for (var i = 0; i < groupsCnts.length; i++) {
            $("#" + $(".vehicles_groupName")[i].id).attr("rowspan", groupsCnts[i]);
            $("#" + $(".vehicles_groupName")[i].id).css("border-right", "1px solid #ddd");
            for (var j = 1; j < groupsCnts[i]; j++) {
                firstRow[removeNum + j - 1].parentNode.removeChild(firstRow[removeNum + j - 1]);
                //firstRow[removeNum+j-1].remove();
            }
            removeNum += groupsCnts[i];
        }
    }
    /**/
    if (null == $(".vehicles_groupName_long") || 0 == $(".vehicles_groupName_long").length) {
    }
    else {
        var firstRow_long = $(".vehicles_groupName_long");
        var groupsCnts_long = new Array();
        var groupCnt_long = 0;
        j = 0;
        cnt = 1;
        for (var i = 0; i < firstRow_long.length - 1; i++) {
            if (firstRow_long[i].children[0].innerHTML == firstRow_long[i + 1].children[0].innerHTML) {
                cnt++;
                continue;
            }
            else {
                groupCnt_long++;
                groupsCnts_long[j] = cnt;
                cnt = 1;
                j++;
            }
        }
        groupsCnts_long[j] = cnt;
        removeNum = 1;
        for (var i = 0; i < groupsCnts_long.length; i++) {
            $("#" + $(".vehicles_groupName_long")[i].id).attr("rowspan", groupsCnts_long[i]);
            $("#" + $(".vehicles_groupName_long")[i].id).css("border-right", "1px solid #ddd");
            for (var j = 1; j < groupsCnts_long[i]; j++) {
                firstRow_long[removeNum + j - 1].parentNode.removeChild(firstRow_long[removeNum + j - 1]);
                //firstRow_long[removeNum+j-1].remove();
            }
            removeNum += groupsCnts_long[i];
        }
        //fengpan 20140414 #600
    }
    PageCount = totalnum_tall;
    if (PageCount == 0) {
        PageCount = 1;
    }
    var data = { 'pagecount': PageCount };
    $("#pageBarshort").pager({ pagenumber: pagenumber, pagecount: data.pagecount, buttonClickCallback: PageClick });
    $("#pageBarlong").pager({ pagenumber: pagenumber, pagecount: data.pagecount, buttonClickCallback: PageClick });
}
PageClick = function (pageclickednumber) {
    //alert(pageclickednumber);
    if ($("#dashboard_button_hide_button_down")[0].style.display != "none") {
        AppendTrShort(pageclickednumber);
    } else {
        AppendTrTall(pageclickednumber);
    }
    $("#result").html("Clicked Page " + pageclickednumber);
}
//chenyangwen 20140611 #1357
function getVehicleInfoForMapUpdate(groupID, isSetZoom, intervalToken, isAjax) {

    var CompanyID = GetCompanyID();

    if (isAjax) {
        arrayVehicles = new Array();
        $.ajax({
            type: "POST",
            //url: "/" + CompanyID + "/Dashboard/GetVehicleInformation",
            url: "/" + CompanyID + "/AjaxResponse/GetVehicleInformation",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: "groupID=" + groupID + "&intervalToken=" + intervalToken,
            success: function (result) {
                //循环取得VehicleList中的数据信息
                arrayVehicles = [];
                for (var i = 0; i < result.allVehicle.length; i++) {
                    var InfoWinContent = "";
                    var iconKind = 1;
                    if (1 == result.allVehicle[i].misState) {
                        iconKind = 5;
                    } else {
                        if (0 == result.allVehicle[i].engineStatus) {
                            if (3 == result.allVehicle[i].alertType && 0 != result.allVehicle[i].healthStatus) {
                                iconKind = 1;
                            }
                            else {
                                iconKind = 3;
                            }
                        } else {
                            if (3 == result.allVehicle[i].alertType && 0 != result.allVehicle[i].healthStatus) {
                                iconKind = 2;
                            }
                            else {
                                iconKind = 4;
                            }
                        }
                    }

                    if (3 == result.allVehicle[i].alertType) {
                        vehiclealert = "OK";
                    } else {
                        vehiclealert = LanguageScript.common_alert;
                    }
                    if (2 == result.allVehicle[i].healthStatus) {
                        health_status = LanguageScript.export_undefine;
                    } else if (0 == result.allVehicle[i].healthStatus) {
                        health_status = LanguageScript.common_EngineOn;
                    } else if (1 == result.allVehicle[i].healthStatus) {
                        health_status = "OK";
                    }

                    InfoWinContent = VehicleInfoWinContent(result.allVehicle[i].logoUrl, result.allVehicle[i].name, result.allVehicle[i].engineTime, health_status, vehiclealert, result.allVehicle[i].primarykey, result.allVehicle[i].driver, result.allVehicle[i].Info, result.allVehicle[i].speed);
                    //把Vehicle的（VehicleID、车牌号licence经纬度、Marker使用的图标类型、Infowindow信息）进行存储
                    var VehicleInfo = { vehicelID: result.allVehicle[i].primarykey, licence: result.allVehicle[i].license, lng: result.allVehicle[i].location.longitude, lat: result.allVehicle[i].location.latitude, iconKind: iconKind, info: InfoWinContent, name: $.trim(result.allVehicle[i].name) };
                    //把一辆Vehicle的信息push到数组arrayVehicles中
                    if (!(-0.1 < result.allVehicle[i].location.longitude && 1 > result.allVehicle[i].location.longitude && -0.1 < result.allVehicle[i].location.latitude && 1 > result.allVehicle[i].location.latitude)) {
                        arrayVehicles.push(VehicleInfo);
                    }
                }

                //调用此函数，将坐标转换为地址，存储在隐藏标签input中
                saveMapPopupAddress(arrayVehicles);

                var mapObj = BMapObj.get_mapObj();
                //描画Vehicle的地理位置的Marker
                var vehiclesMap = new ihpleD_ShowVehicles(mapObj, arrayVehicles, true, true, isSetZoom);
                if (arrLocationforMarker.length != 0) {
                    //设置地图中心点
                    addMarkerForCommonAddress(BMapObj, arrLocationforMarker[0].lng, arrLocationforMarker[0].lat);
                }
            }
        });
    }
    else if (isAjax == false) {

        var mapObj = BMapObj.get_mapObj();
        //描画Vehicle的地理位置的Marker
        var vehiclesMap = new ihpleD_ShowVehicles(mapObj, arrayVehicles, true, true, isSetZoom);
    }
}

//mabiao 
//右侧车辆切换时 地图中心点联动
function ChooseVehicleUpdateMap(vehicle) {
    var mapInfoJson = BMapObj.getMapInfo();
    var obj = eval("(" + mapInfoJson + ")");
    var Zoom = obj.zoomLevel;
    var lng = null;
    var lat = null;
    for (var i = 0 ; i < arrayVehicles.length; i++) {
        if (vehicle == arrayVehicles[i].vehicelID) {
            lng = arrayVehicles[i].lng;
            lat = arrayVehicles[i].lat;
            break;
        }
    }
    //mabiao 20140321 #628
    if (null == lng || null == lat) {
        return;
    }
    BMapObj.setNewMapView(lng, lat, Zoom);
}

function stringToHourMin(sMinTime){
    if(sMinTime == null){return null};

    var sTime = "";
    var min_time = parseInt($.trim(sMinTime));
    var h = parseInt(min_time / 60);
    var m = parseInt(min_time % 60);

    if (h) {
        sTime +=(h + LanguageScript.common_Hour);
    }

    if (m) {
        sTime +=(m + LanguageScript.common_Min);
    }

    return sTime;
}

////调用此函数，将坐标转换为地址，存储在隐藏标签input中
function saveMapPopupAddress(arrayVehicles) {

    //每次调用之前清空
    $("#map_popup_address").empty();
    var view = "";

    if (arrayVehicles && arrayVehicles.length != 0) {
        for (var i = 0; i < arrayVehicles.length; i++) {
            view += '<input id="map_popup_address' + arrayVehicles[i].vehicelID + '" type="hidden" value="" />';
        }
        $("#map_popup_address").append(view);

        for (var i = 0; i < arrayVehicles.length; i++) {
            markerPopupAddress(arrayVehicles[i].lng, arrayVehicles[i].lat, ("map_popup_address" + arrayVehicles[i].vehicelID) );
        }
    }
}

//    函数功能：百度坐标转成地址,并把标签为ElementID的value设置为地址
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
function markerPopupAddress(lng, lat, ElementID) {

    var point = new BMap.Point(lng, lat);
    var geocoder = new BMap.Geocoder();

    geocoder.getLocation(point, function (ElementID) {

        return function (result) {

            var addComp = result.addressComponents;
            var locationInfo = "";
            var title = "";
            var array_address = [addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber];

            //解析后的地址信息字符串：没有省、没有逗号
            for (var i = 1; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                locationInfo += array_address[i];
            }

            //解析后的地址title信息字符串：有省、没有逗号
            for (var i = 0; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                title += array_address[i];
            }

            if (!$.trim(locationInfo)) {
                locationInfo = LanguageScript.export_undefine;
            }

            if (!$.trim(title)) {
                title = LanguageScript.export_undefine;
            }

            //设定ElementID内显示地址信息
            $("#" + ElementID + "")[0].value = locationInfo;

            //设定ElementID的title信息
            $("#" + ElementID + "").attr("title", title);
        }
    }(ElementID));
}

//    函数功能：百度坐标转成地址,并把标签为ElementID的value设置为地址，然后执行检索功能的弹出popup
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
function markerPopupAddressForPopup(lng, lat, ElementID) {

    var point = new BMap.Point(lng, lat);
    var geocoder = new BMap.Geocoder();

    geocoder.getLocation(point, function (ElementID) {

        return function (result) {

            var addComp = result.addressComponents;
            var locationInfo = "";
            var title = "";
            var array_address = [addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber];

            //解析后的地址信息字符串：没有省、没有逗号
            for (var i = 1; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                locationInfo += array_address[i];
            }

            //解析后的地址title信息字符串：有省、没有逗号
            for (var i = 0; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                title += array_address[i];
            }

            if (!$.trim(locationInfo)) {
                locationInfo = LanguageScript.export_undefine;
            }

            if (!$.trim(title)) {
                title = LanguageScript.export_undefine;
            }

            //设定ElementID内显示地址信息
            $("#" + ElementID + "")[0].value = locationInfo;

            //设定ElementID的title信息
            $("#" + ElementID + "").attr("title", title);

            //一下两行代码是触发地图上的mark显示popup
            $("#showPopUpForSearch")[0].value = $.trim($("#strSelectForSearch").val());
            $('#showPopUpForSearch').trigger("click");

            $("#Latitude").attr("value", "");
            $("#Longitude").attr("value", "");
            $("#zoomForSearch").attr("value", "");
            $("#showTypeForSearch").attr("value", "");

            //清空session
            $.ajax({
                type: "POST",
                async: false,
                url: "/" + CompanyID + "/Dashboard/CleanSearchPara",
                contentType: "application/x-www-form-urlencoded",
                data: "",
                dataType: "json",
                success: function (msg) {

                }
            });

        }
    }(ElementID));
}


/************冯盼 20140617 下拉框插件初始化**************/
function initSelect()
{
    $('#select_group').selectpicker();
    $('#Home_choose_vehicle').selectpicker();
}


