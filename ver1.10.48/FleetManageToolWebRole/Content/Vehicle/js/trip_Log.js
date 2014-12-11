$(document).ready(function () {

    //fengpan 初始化select月份
    initSelectMonth();

    //地图变量
    var RightBMapObj = null;

    //刷新时间；
    var realTimeTripLogInterval = 1;

    /*fengpan 初始化翻页*/
    VehicleAlertInit(1);
    GeofenceAlertInit(1);

    var x = 1000;
    trip_log_list_height = 1000;
    trip_log_map_height = 1000;
    trip_log_height = 1000;
    fuel_log_height = 1000;
    alert_height = 1000;
    geofence_height = 1000;
    right_detail_height = 1000;
    TripLog_Element = null;
    Old_date_time = null;
    //mabiao 20140312 
    TripLogList_Element = null;
    ihpleDTripHeight = 0;
    transformLocation = new Array();
    transformLocationList = new Array();
    ihpleD_TripDetailLocation = new Array();

    var date = new Date();
    ihpleDTripDate = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
    ihpleDTripListDate = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
    ihpleDTripNow = date.getFullYear().toString() +
                        (date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) +
                        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
                        (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
                        (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes());
    if (x != 0) {
        document.getElementById("u_left").style.height = x + "px";
        document.getElementById("u_right").style.height = x + "px";
    }
    document.getElementById("vehicle_detail_info_bg_img").style.height = x + "px";


    /*********** fengpan EditVehicle 20140226**************/
    $("#vehicle_detail_edit_bg").click(function () {
        var vehicleID = $("#vehicleID").val();
        var CompanyID = GetCompanyID();
	//fengpan #508 20140304
        $.ajax({       
            type: "POST",
            url: "/" + CompanyID + "/Vehicles/Edit_vehicle",
            data: { VehicleID: vehicleID },
            contentType: "application/x-www-form-urlencoded",
            datatype: "text",
            success: function (msg) {
                if ("OK" == msg)
                {
                    var url = "/" + CompanyID + "/Setting/Tenant";
                    location.href = url;
                    trans();
                }
            }
        });
	//fengpan #508 20140304
    });
    var roleId = GetRoleID();
    if (1 != roleId)
    {
        SetBtnToneDown("vehicle_detail_edit_bg");
    }
    /*********** fengpan EditVehicle20140226**************/
    /********triplog list***********/
    $("#Vehicles_trip_log_list").hide();

    $("#Vehicles_trip_log_list_btn").click(function () {
        $("#Vehicles_trip_log_list_btn").removeClass();
        $("#Vehicles_trip_log_list_btn").addClass("cls_Vehicles_trip_log_map_btn");
        $("#Vehicles_trip_log_map_btn").removeClass();
        $("#Vehicles_trip_log_map_btn").addClass("cls_Vehicles_trip_log_list_btn");
        $("#Vehicles_trip_log").hide();
        $("#Vehicles_trip_log_list").show();
        trip_log_height = trip_log_list_height;
        TripLogHeight();


    });

    $("#Vehicles_trip_log_map_btn").click(function () {
        $("#Vehicles_trip_log_map_btn").removeClass();
        $("#Vehicles_trip_log_map_btn").addClass("cls_Vehicles_trip_log_map_btn");
        $("#Vehicles_trip_log_list_btn").removeClass();
        $("#Vehicles_trip_log_list_btn").addClass("cls_Vehicles_trip_log_list_btn");
        $("#Vehicles_trip_log_list").hide();
        $("#Vehicles_trip_log").show();
        trip_log_height = trip_log_map_height;
        TripLogHeight();
    });

    /******* Vehicles triplog fuellog alert geofence tab********/
    $("#Vehicles_detail_fuel_log").hide();
    $("#Vehicles_detail_alert").hide();
    $("#Vehicles_detail_geofence").hide();


    $("#Vehicles_detail_tab_trip_log").click(function () {
        $("#Vehicles_detail_tab_fuel_log").removeClass();
        $("#Vehicles_detail_tab_fuel_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_alert").removeClass();
        $("#Vehicles_detail_tab_alert").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_geofence").removeClass();
        $("#Vehicles_detail_tab_geofence").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_trip_log").removeClass();
        $("#Vehicles_detail_tab_trip_log").addClass("cls_Vehicles_detail_tab_choose");
        $("#Vehicles_detail_fuel_log").hide();
        $("#Vehicles_detail_alert").hide();
        $("#Vehicles_detail_geofence").hide();
        $("#Vehicles_detail_trip_log").show();
        TripLogHeight();
    });

    $("#Vehicles_detail_tab_fuel_log").click(function () {

        $("#Vehicles_detail_tab_alert").removeClass();
        $("#Vehicles_detail_tab_alert").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_geofence").removeClass();
        $("#Vehicles_detail_tab_geofence").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_trip_log").removeClass();
        $("#Vehicles_detail_tab_trip_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_fuel_log").removeClass();
        $("#Vehicles_detail_tab_fuel_log").addClass("cls_Vehicles_detail_tab_choose");
        $("#Vehicles_detail_alert").hide();
        $("#Vehicles_detail_geofence").hide();
        $("#Vehicles_detail_trip_log").hide();
        $("#Vehicles_detail_fuel_log").show();

    /*mabiao for chart */
    $.jqplot.config.enablePlugins = true;
    s1 = [['24-Mar-2014', 0], ['25-Mar-2014', 0], ['26-Mar-2014', 0], ['27-Mar-2014', 0]];

    s2 = [3, 5, 7, 4, 8];
    s3 = [9, 11, 15, 8, 15];
    s4 = [8, 7, 12, 18, 4];
    s5 = [13, 17, 21, 19, 11];
    l1 = [];
    l2 = [];
    l3 = [];

    for (var i = 0; i < 100; i++) {
        l1.push(Math.random() * 7);
        l2.push(Math.random() * 13);
        l3.push(Math.random() * 2);
    }


    plot1 = $.jqplot('chart1', [s1], {
        axes: {
            xaxis: {
                renderer: $.jqplot.DateAxisRenderer,
                tickOptions: {
                    min: null,
                    max: null,
                    formatString: '%m-%d',

                },
                numberTicks: 4
            },
            yaxis: {
                tickOptions: {
                    show: true,
                    min: 0,
                    max: null,
                    formatString: '%d',

                    }
                }
            },
            highlighter: {
                sizeAdjust: 10,
                tooltipLocation: 'n',
                useAxesFormatters: false,
                formatString: 'Hello %s dayglow %d',
                tooltipContentEditor: editit
            },
            cursor: {
                show: true,
                zoom: true
            },
        });
        /*mabiao for chart */
        document.getElementById("vehicle_detail_info_bg_img").style.height = fuel_log_height + "px";
        document.getElementById("u_left").style.height = fuel_log_height + "px";
        document.getElementById("u_right").style.height = fuel_log_height + "px";
        
    });

    $("#Vehicles_detail_tab_geofence").click(function () {
        $(".paper-input-value").val("");
        $("#Vehicles_detail_tab_fuel_log").removeClass();
        $("#Vehicles_detail_tab_fuel_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_alert").removeClass();
        $("#Vehicles_detail_tab_alert").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_trip_log").removeClass();
        $("#Vehicles_detail_tab_trip_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_geofence").removeClass();
        $("#Vehicles_detail_tab_geofence").addClass("cls_Vehicles_detail_tab_choose");
        $("#Vehicles_detail_fuel_log").hide();
        $("#Vehicles_detail_alert").hide();
        $("#Vehicles_detail_trip_log").hide();
        $("#Vehicles_detail_geofence").show();
        //Vehicles_detail_geoAlert_dis();//fengpan 20140306 重构
        document.getElementById("vehicle_detail_info_bg_img").style.height = geofence_height + "px";
        document.getElementById("u_left").style.height = geofence_height + "px";
        document.getElementById("u_right").style.height = geofence_height + "px";
    });
    $("#geofence_choose_month").change(function () {
        $("#geofence_choose_month_1").empty();
        var year = $("#geofence_choose_month").val();
        var today = new Date();
        if (year == today.getFullYear().toString()) {
            for (var i = 1; i <= today.getMonth() + 1 ; i++) {
                if (i < 10) {
                    $("#geofence_choose_month_1").append("<option>0" + i + "</option>");
                } else {
                    $("#geofence_choose_month_1").append("<option>" + i + "</option>");
                }
            }
        } else if (year >= 2013) {
            for (var i = 1; i <= 12; i++) {
                if (i < 10) {
                    $("#geofence_choose_month_1").append("<option>0" + i + "</option>");
                } else {
                    $("#geofence_choose_month_1").append("<option>" + i + "</option>");
                }
            }
        }
        //$("#geofenceTable tr:not(:first)").remove();
        $("#geofence_choose_month_1").selectpicker('refresh');
        GeofenceAlertInit(1);
    });
    $("#geofence_choose_month_1").change(function () {
        //$("#geofenceTable tr:not(:first)").remove();
        GeofenceAlertInit(1);
    });

    $("#Vehicles_detail_tab_alert").click(function () {
        $(".paper-input-value").val("");
        $("#Vehicles_detail_tab_fuel_log").removeClass();
        $("#Vehicles_detail_tab_fuel_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_trip_log").removeClass();
        $("#Vehicles_detail_tab_trip_log").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_geofence").removeClass();
        $("#Vehicles_detail_tab_geofence").addClass("cls_Vehicles_detail_tab_unchoose");
        $("#Vehicles_detail_tab_alert").removeClass();
        $("#Vehicles_detail_tab_alert").addClass("cls_Vehicles_detail_tab_choose");
        $("#Vehicles_detail_fuel_log").hide();
        $("#Vehicles_detail_trip_log").hide();
        $("#Vehicles_detail_geofence").hide();
        $("#Vehicles_detail_alert").show();
        /*fengpan 20140305*/
        //Vehicles_detail_alert_dis();
        /*fengpan 20140305*/
        document.getElementById("vehicle_detail_info_bg_img").style.height = alert_height + "px";
        document.getElementById("u_left").style.height = alert_height + "px";
        document.getElementById("u_right").style.height = alert_height + "px";
    });
    $("#alert_choose_month").change(function () {
        //$("#alertTable tr:not(:first)").remove();
        $("#alert_choose_month_1").empty();
        var year = $("#alert_choose_month").val();
        var today = new Date();
        if (year == today.getFullYear().toString()) {
            for (var i = 1; i <= today.getMonth() + 1; i++) {
                if (i < 10) {
                    $("#alert_choose_month_1").append("<option>0" + i + "</option>");
                } else {
                    $("#alert_choose_month_1").append("<option>" + i + "</option>");
                }
            }
        } else if (year >= 2013) {
            for (var i = 1; i <= 12; i++) {
                if (i < 10) {
                    $("#alert_choose_month_1").append("<option>0" + i + "</option>");
                } else {
                    $("#alert_choose_month_1").append("<option>" + i + "</option>");
                }
            }
        }
        $("#alert_choose_month_1").selectpicker('refresh');
        VehicleAlertInit(1);
    });
    $("#alert_choose_month_1").change(function () {
        VehicleAlertInit(1);
    });

    var todaydate = new Date();
    var endday = new Date();
    var tempmonth = "";
    var tempday = "";
    if ((todaydate.getMonth() + 1) < 10) {
        tempmonth = "/0" + (todaydate.getMonth() + 1);
    } else {
        tempmonth = "/" + (todaydate.getMonth() + 1);
    }
    if (endday.getDate() < 10) {
        tempday = "/0" + endday.getDate();
    } else {
        tempday = "/" + endday.getDate();
    }
    document.getElementById('date_timepicker_start').value = todaydate.getFullYear() + tempmonth + "/0" + 1;
    document.getElementById('date_timepicker_end').value = endday.getFullYear() + tempmonth + tempday;

    var startTime = $("#date_timepicker_start").val();
    var endTime = $("#date_timepicker_end").val();

    if (startTime != null && startTime != "") {
        startTime = startTime.replace(/\//g, '');
        startTime += "000000";
        Old_date_time = startTime;
    } else {
        Old_date_time = null;
    }
    //chenyangwen 20140716 #2124
    //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
    getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), 1);
    /*****mabiao trip log list***/
    getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));

    $(function () {
        $("#date_timepicker_start").datetimepicker({
            lang: 'ch',
            format: 'Y/m/d',
            minDate: '2013/01/01',
            yearStart: 2013,
            onShow: function (ct) {
                this.setOptions({
                    maxDate: $("#date_timepicker_end").val() ? $('#date_timepicker_end').val() : false
                })
            },
            timepicker: false
        });
        $("#date_timepicker_end").datetimepicker({
            lang: 'ch',
            format: 'Y/m/d',
            minDate: '2013/01/01',
			maxDate:0,
            yearStart: 2013,
            onShow: function (ct) {
                this.setOptions({
                    minDate: $("#date_timepicker_start").val() ? $('#date_timepicker_start').val() : false
                })
            },
            timepicker: false
        });
    });

    $("#date_time_ok").click(function () {
        var startTime = $("#date_timepicker_start").val();
        var endTime = $("#date_timepicker_end").val();

        if (StartDateCheck(startTime, endTime) == 0) {
            datepickerforTip(LanguageScript.page_report_datepickererror1);
            $("#datepickerTip").dialog("open");
        } else if (StartDateCheck(startTime, endTime) == 1) {
            datepickerforTip(LanguageScript.page_report_datepickererror2);
            $("#datepickerTip").dialog("open");
        } else if ((CheckDatepicker(startTime) != true) ||
            (CheckDatepicker(endTime) != true)) {
            datepickerforTip(LanguageScript.datepickerTip);
            $("#datepickerTip").dialog("open");
        } else {
            if (startTime != null && startTime != "") {
                startTime = startTime.replace(/\//g, '');
                startTime += "000000";
                Old_date_time = startTime;
            } else {
                Old_date_time = null;
            }
            if (endTime != null && endTime != "") {

                var date = new Date(endTime);
                date.setDate(date.getDate() + 1);
                endTime = date.format("yyyyMMdd");
                endTime += "000000";
                TripLog_Element = endTime;
                TripLogList_Element = endTime;
                ihpleDTripDate = endTime;
            } else {
                var date = new Date();
                date.setDate(date.getDate() + 1);
                endTime = date.format("yyyyMMdd");
                endTime += "000000";
                TripLog_Element = endTime;
                TripLogList_Element = endTime;
                ihpleDTripDate = endTime;
            }

        Loading_Bind("Vehicles_Trip_log_viewmore");
        Loading_Bind("Vehicles_Trip_log_list_viewmore");

        $(".cls_Vehicles_trip_log_detail").remove();
        $(".cls_Vehicles_trip_log_day").remove();
        $(".cls_Vehicles_trip_log_list_text").remove();
        $(".cls_Vehicles_trip_log_detail_moreinfo").remove();
    

        //chenyangwen 20140716 #2124
        //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
        getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), 1);
        getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
		}
    });

    /*****zhangbo 右上角Location map***/
    showRightMap();

    getFuelLogData();

    showLocationAdress($("#VehicleLocationLongtitude").val(), $("#VehicleLocationLatitude").val());

    ChangeLeft("common_vehicle_cover");
    ChangeLocationTime();

    //定时刷新 20140513 冯盼
    var realTimeHomeInterval = null;
    if (realTimeHomeInterval) {
        window.clearInterval(realTimeHomeInterval);
    }

    var intervalToken = Math.round(Math.random() * 1000000);
    realTimeHomeInterval = window.setInterval(function () {
        //chenyangwen 20140611 #1357
        GetCurrentVehicleInfo(intervalToken);
        ChangeLocationTime();
    }, 1 * 60 * 1000);
    //chenyanwgen 20140611 #1357

    $.ajaxSetup({
        statusCode: {
            899: function (data) {
                window.clearInterval(realTimeHomeInterval);
            }
        }
    });
});

//获取当前车辆的信息
//chenyangwen 20140611 #1357
function GetCurrentVehicleInfo(intervalToken) {
    var vehicleID = $("#vehicleID").val();
    var vehicleThreshold = $("#VehicleSpeedThreshold").val();
    $.ajax({
        type: "POST",
        //url: "/" + GetCompanyID() + "/Vehicles/GetCurrentVehicleInfo",
        url: "/" + GetCompanyID() + "/AjaxResponse/GetCurrentVehicleInfo",
        data: { vehicleId: vehicleID, intervalToken: intervalToken },
        contentType: "application/x-www-form-urlencoded",
        datatype: "json",
        success: function (result) {
            if (null != result && "" != result) {
                //地图
                updateRightMap(result);
                //地址信息
                if (null != result.location) {
                    showLocationAdress(result.location.longitude, result.location.latitude);
                }
                //行驶里程
                $("#vehicleOdometer").text(result.odometer + "公里");
                $("#vehicleOdometer").css("color", "#424242");
                //速度
                if (null != result.speed) {
                    if (null == vehicleThreshold) {
                        $("#vehicleSpeed").css("color", "#339900");
                    }
                    else if (result.speed < vehicleThreshold) {
                        $("#vehicleSpeed").css("color", "#339900");
                    }
                    else if (result.speed > vehicleThreshold) {
                        $("#vehicleSpeed").css("color", "#cc0000");
                    }
                    $("#vehicleSpeed").text(result.speed);
                }
                else {
                    $("#vehicleSpeed").text(LanguageScript.export_undefine);
                    $("#vehicleSpeed").css("color", "#424242");
                }
                //发动机
                if (null == result.engineStatus) {
                    $("#vehicleEngin").text(LanguageScript.export_undefine);
                    $("#vehicleEngin").css("color", "#424242");
                } else if (result.engineStatus == 0)//引擎on
                {
                    $("#vehicleEngin").text("ON");
                    $("#vehicleEngin").css("color", "#339900");
                }
                else {
                    $("#vehicleEngin").text("OFF");
                    $("#vehicleEngin").css("color", "#424242");
                    $("#vehcleSpeed").text(0);
                }
                //电压
                if (null == result.battery) {
                    $("#vehicleBattery").text(LanguageScript.export_undefine);
                } else {
                    $("#vehicleBattery").text(result.battery + " " + LanguageScript.unit_fu);
                }
                //油量

                var $_vehicleFuel = $("#vehicleFuel");

                if (result.fuel == null) {
                    $_vehicleFuel.text(LanguageScript.export_undefine);
                    $_vehicleFuel.css("color", "#424242");
                } else if (result.fuel >= 25 && result.fuel < 100) {
                    $_vehicleFuel.css("color", "#339900");
                    $_vehicleFuel.text(result.fuel + "%");
                } else if (result.fuel >= 10) {
                    $_vehicleFuel.css("color", "#CC9900");
                    $_vehicleFuel.text(result.fuel + "%");
                } else {
                    $_vehicleFuel.css("color", "#CC0000");
                    $_vehicleFuel.text(result.fuel + "%");
                }
                
                //车辆健康
                if (1 == result.healthStatus) {
                    $("#vehicleHealthStatus").text("健康");
                    $("#vehicleHealthStatus").css("color", "#339900");
                } else if (0 == result.healthStatus) {
                    $("#vehicleHealthStatus").text("有故障");
                    $("#vehicleHealthStatus").css("color", "#cc0000");
                } else if (2 == result.healthStatus) {
                    $("#vehicleHealthStatus").text(LanguageScript.export_undefine);
                    $("#vehicleHealthStatus").css("color", "#424242");
                }
            } else {
                return;
            }
        },
        error: function () {
            //..
        }
    });
}

function showLocationAdress(longtitude,latitude) {
    var CompanyID = GetCompanyID();
    geocoderLocationForVehicleList(longtitude, latitude, null, null, "vehicle_detail_info_location_rtf_up", "vehicle_detail_info_location_rtf_down");
}

/**********************************************************************************************************************************/
/*fengpan 20140305*/
//var alertPageNum = 0;
//var alertHoldjson = '';
var alertOnePageCount = 10;

function addJsonstr(alertHoldjson) {
    //if ("" != msg) {
    //    alertHoldjson = msg;
    //}
    $("#alertTable tr:not(:first)").remove();
    var i = 0;
    var length = alertOnePageCount;
    var alert = "";
    var Vehicles_detail_alert_detial_info = "";
    var Vehicles_detail_alert_detial_unit = "";

    var Vehicles_alert_list = '';
    for (; i < length; i++) {
        if (i >= alertHoldjson.length) {
            break;
        }
        if ("0" == alertHoldjson[i].alertType)
        {
            alert = LanguageScript.common_alertTypes_SPEED;
            Vehicles_detail_alert_detial_info = LanguageScript.export_speedalertdetail;
            Vehicles_detail_alert_detial_unit = LanguageScript.common_KmPerHour;
        }
        if ("1" == alertHoldjson[i].alertType)
        {
            alert = LanguageScript.common_alertTypes_MOTION;
            Vehicles_detail_alert_detial_info = LanguageScript.page_vehicle_MotionIntensity + ":";
            Vehicles_detail_alert_detial_unit = "G";
        }
        if ("2" == alertHoldjson[i].alertType)
        {
            alert = LanguageScript.common_alertTypes_EngineRPM;
            Vehicles_detail_alert_detial_info = LanguageScript.export_RPMalertdetail + ":";
            Vehicles_detail_alert_detial_unit = "RPM";
        }
        if (null == alertHoldjson[i].detail.alertInfo)
        {
            alertHoldjson[i].detail.alertInfo = LanguageScript.export_undefine;
        }
        //rpm报警
        if ("2" == alertHoldjson[i].alertType) {
            if (null == alertHoldjson[i].detail.duration) {
                alertHoldjson[i].detail.duration = LanguageScript.export_undefine;
            }
            Vehicles_alert_list += Vehicles_rpm_alert_list_add(transTime(alertHoldjson[i].alertTime).format("yyyy-MM-dd HH:mm"), alert, alertHoldjson[i].detail.alertInfo, Vehicles_detail_alert_detial_info, Vehicles_detail_alert_detial_unit, alertHoldjson[i].detail.duration);
        } else {
            Vehicles_alert_list += Vehicles_alert_list_add(transTime(alertHoldjson[i].alertTime).format("yyyy-MM-dd HH:mm"), alert, alertHoldjson[i].detail.alertInfo, Vehicles_detail_alert_detial_info, Vehicles_detail_alert_detial_unit);
        }
    }
    $("#alertTable").append(Vehicles_alert_list);
}
function Vehicles_alert_list_add(Vehicles_detail_alert_time, Vehicles_detail_alert_type, Vehicles_detail_alert_detial, Vehicles_detail_alert_detial_info, Vehicles_detail_alert_detial_unit) {
    return '<tr>'+
                '<td>' + Vehicles_detail_alert_time + '</td>' +
                '<td>' + Vehicles_detail_alert_type + '</td>' +
                '<td>' + Vehicles_detail_alert_detial_info + Vehicles_detail_alert_detial + Vehicles_detail_alert_detial_unit + '</td>' +
            '</tr>';
}

function Vehicles_rpm_alert_list_add(Vehicles_detail_alert_time, Vehicles_detail_alert_type, Vehicles_detail_alert_detial, Vehicles_detail_alert_detial_info, Vehicles_detail_alert_detial_unit, Vehicles_detail_alert_detial_duration) {
    return '<tr>' +
                '<td>' + Vehicles_detail_alert_time + '</td>' +
                '<td>' + Vehicles_detail_alert_type + '</td>' +
                '<td>' + Vehicles_detail_alert_detial_info + Vehicles_detail_alert_detial + Vehicles_detail_alert_detial_unit + ',' + LanguageScript.page_vehicles_duration + ':' + Vehicles_detail_alert_detial_duration + LanguageScript.common_Second + '</td>' +
            '</tr>';
}

//var geoAlertPageNum = 0;
//var geoAlertHoldjson = '';

function geoAddJsonstr(geoAlertHoldjson) {
    //if ("" != msg) {
    //    geoAlertHoldjson = msg;
    //}
    $("#geofenceTable tr:not(:first)").remove();
    var i = 0;
    var length = alertOnePageCount;
    var alert = "";
    var Vehicles_goeAlert_list = '';
    for (; i < length; i++) {
        if (i >= geoAlertHoldjson.length) {
            break;
        }
        if (null == geoAlertHoldjson[i].geofenceName) {
            ageoAlertHoldjson[i].geofenceName = LanguageScript.export_undefine;
        }
        if (null == geoAlertHoldjson[i].alertInfo) {
            geoAlertHoldjson[i].alertInfo = LanguageScript.export_undefine;
        }
        Vehicles_goeAlert_list += Vehicles_geoAlert_list_add(transTime(geoAlertHoldjson[i].alertTime).format("yyyy-MM-dd HH:mm"), geoAlertHoldjson[i].geofenceName, geoAlertHoldjson[i].locationName, geoAlertHoldjson[i].alertInfo);
    }
    $("#geofenceTable").append(Vehicles_goeAlert_list);
}
function Vehicles_geoAlert_list_add(Vehicles_detail_geoAlert_time, Vehicles_detail_geo_name, Vehicles_detail_geoAlert_location, Vehicles_detail_geoAlert_info) {
    if ("" == Vehicles_detail_geoAlert_location || null == Vehicles_detail_geoAlert_location)
    {
        Vehicles_detail_geoAlert_location = LanguageScript.export_undefine;
    }
    return '<tr>' +
                '<td>' + Vehicles_detail_geoAlert_time + '</td>' +
		//fengpan 20140324 #834
                '<td><div style="white-space:nowrap;text-overflow:ellipsis;overflow: hidden;width:200px;" title=' + Vehicles_detail_geo_name + '>' + Vehicles_detail_geo_name + '</div></td>' +
                '<td>' + Vehicles_detail_geoAlert_info + '</td>' +
            '</tr>';
}
/* fengpan 20140305*/
/*fengpan  20140309*/
//默认加载  
function VehicleAlertInit(pagenumber) {
    //向服务器发送请求，查询满足条件的记录  
    //$.getJSON('',{},function(data){
    //data 为返回json 对象 并包括(pagecount)的key-value值;
    //$("#alertTable tr:not(:first)").remove();
    $("#vehicle_alert_pageBar").attr("alert-click", "false");
    var CompanyID = GetCompanyID();
    var vehicleID = $("#vehicleID").val();
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetVehicleAlertsOfOnePage",
        url: "/" + CompanyID + "/AjaxResponse/GetVehicleAlertsOfOnePage",
        data: { vehicleId: vehicleID, pageNum: pagenumber, month: $("#alert_choose_month").val() + "年" + $("#alert_choose_month_1").val() + "月" },
        contentType: "application/x-www-form-urlencoded",
        datatype: "json",
        success: function (msg) {
            if (msg.alerts != null && msg.alerts.length != 0) {
                $("#alertTable tr:not(:first)").remove();
                alertPageNum = 0;
                addJsonstr(msg.alerts);
                $("#vehicle_alert_pageBar").attr("alert-click", "true");
            }
            else {
                $("#alertTable tr:not(:first)").remove();
            }
            var pageNum = parseInt(msg.alertsCount);
            pageNum = (pageNum % alertOnePageCount == 0 ? pageNum / alertOnePageCount : pageNum / alertOnePageCount + 1);
            if (pageNum == 0) {
                pageNum = 1;
            }
            $("#vehicle_alert_pageBar").pager({ pagenumber: pagenumber, pagecount: pageNum, buttonClickCallback: VehicleAlertPageClick });
        },
        error: function () {
            $("#alertTable tr:not(:first)").remove();
        }
    });
    //$.ajax({
    //    type: "POST",
    //    url: "/" + CompanyID + "/Vehicles/GetVehicleAlertsPageCount",
    //    data: { vehicleId: vehicleID, month: $("#alert_choose_month").val() + "年" + $("#alert_choose_month_1").val() + "月" },
    //    contentType: "application/x-www-form-urlencoded",
    //    datatype: "int",
    //    success: function (msg) {
    //        if (msg != null) {
    //            var pageNum = parseInt(msg);
    //            pageNum = (pageNum % 10 == 0 ? pageNum / 10 : pageNum / 10 + 1);
    //            if (pageNum == 0) {
    //                pageNum = 1;
    //            }
    //            $("#vehicle_alert_pageBar").pager({ pagenumber: pagenumber, pagecount: pageNum, buttonClickCallback: VehicleAlertPageClick });
    //        }
    //    }
    //});
    //var data = { 'pagecount': 15 };
    //$("#vehicle_alert_pageBar").pager({ pagenumber: pagenumber, pagecount: data.pagecount, buttonClickCallback: VehicleAlertPageClick });
    //});  
}
function GeofenceAlertInit(pagenumber) {
    //向服务器发送请求，查询满足条件的记录  
    //$.getJSON('',{},function(data){
    //data 为返回json 对象 并包括(pagecount)的key-value值;
    //$("#geofenceTable tr:not(:first)").remove();
    var vehicleID = $("#vehicleID").val();
    var CompanyID = GetCompanyID();
    $("#geofence_alert_pageBar").attr("geo-click", "false");
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetGeofenceAlertsOfOnePage",
        url: "/" + CompanyID + "/AjaxResponse/GetGeofenceAlertsOfOnePage",
        data: { vehicleId: vehicleID, pageNum: pagenumber, month: $("#geofence_choose_month").val() + "年" + $("#geofence_choose_month_1").val() + "月" },
        contentType: "application/x-www-form-urlencoded",
        datatype: "json",
        success: function (msg) {
            if (msg.alerts != null && msg.alerts.length != 0) {
                geoAddJsonstr(msg.alerts);
                $("#geofence_alert_pageBar").attr("geo-click", "true");
            }
            else {
                $("#geofenceTable tr:not(:first)").remove();
            }
            var pageCount = parseInt(msg.alertsCount);
            pageCount = (pageCount % alertOnePageCount == 0 ? pageCount / alertOnePageCount : pageCount / alertOnePageCount + 1);
            if (pageCount == 0) {
                pageCount = 1;
            }
            $("#geofence_alert_pageBar").pager({ pagenumber: pagenumber, pagecount: pageCount, buttonClickCallback: GeofenceAlertPageClick });
        }
    });
    //$.ajax({
    //    type: "POST",
    //    url: "/" + CompanyID + "/Vehicles/GetGeofenceAlertsPageCount",
    //    data: { vehicleId: vehicleID, month: $("#geofence_choose_month").val() + "年" + $("#geofence_choose_month_1").val() + "月" },
    //    contentType: "application/x-www-form-urlencoded",
    //    datatype: "int",
    //    success: function (msg) {
    //        if (msg != null) {
    //            var pageCount = parseInt(msg);
    //            pageCount = (pageCount % 10 == 0 ? pageCount / 10 : pageCount / 10 + 1);
    //            if (pageCount == 0)
    //            {
    //                pageCount = 1;
    //            }
    //            $("#geofence_alert_pageBar").pager({ pagenumber: pagenumber, pagecount: pageCount, buttonClickCallback: GeofenceAlertPageClick });
    //        }
    //    }
    //});
}

//回调函数  
VehicleAlertPageClick = function (pageclickednumber) {
    //alert(pageclickednumber);
    if ($("#vehicle_alert_pageBar").attr("alert-click") == "true") {
        VehicleAlertInit(pageclickednumber);
        $("#result").html("Clicked Page " + pageclickednumber);
    } else {
        return;
    }
}
GeofenceAlertPageClick = function (pageclickednumber) {
    //alert(pageclickednumber);
    if ($("#geofence_alert_pageBar").attr("geo-click") == "true") {
        GeofenceAlertInit(pageclickednumber);
        $("#result").html("Clicked Page " + pageclickednumber);
    }
}
/**********************************************************************************************************************************/


var add_map = function (id,obj) {
    var div_id = "cls_Vehicles_trip_log_detail_map" + id;
    var map_close_id = "Vehicles_trip_log_detail_map_close" + id;
    var map_info_distance = obj.distance + LanguageScript.unit_km;
    var map_info_time = (obj.DriveTime.Days == 0 ? "" : obj.DriveTime.Days + "天") +
                        (obj.DriveTime.Hours == 0 ? "" : obj.DriveTime.Hours + LanguageScript.common_Hour) +
                        (obj.DriveTime.Minutes == 0 ? "" : obj.DriveTime.Minutes + LanguageScript.common_Minute) +
                        obj.DriveTime.Seconds + LanguageScript.common_Second;
    var starttime = transTime(obj.StartTime).format("yyyyMMddhhmmss");
    var endtime = transTime(obj.EndTime).format("yyyyMMddhhmmss");
    var map_info_starttime = starttime.substring(8, 10) + ":" + starttime.substring(10, 12);
    var map_info_startlocation = obj.startLocation;
    var map_info_endtime = endtime.substring(8, 10) + ":" + endtime.substring(10, 12);
    var map_info_endlocation = obj.endLocation;
    //var map_info_slowspeed = obj.idleTime.substring(0, 2) + LanguageScript.common_Hour + obj.idleTime.substring(2, 4) + LanguageScript.common_Minute + obj.idleTime.substring(4, 6) + LanguageScript.common_Second;
    var map_info_slowspeed = formatSeconds(obj.idleTime);
    if (document.getElementById(div_id) != null) {
        return;
    }
    if ($(".cls_Vehicles_trip_log_detail_moreinfo").length > 0) {
        $(".cls_Vehicles_trip_log_detail_moreinfo").remove();
    } 

    $("#" + id).parent().parent().after(function () {
        return "<div class='cls_Vehicles_trip_log_detail_moreinfo'>" +
            "<div  id='Vehicles_trip_log_detail_map_close" + id + "' class='cls_Vehicles_trip_log_detail_map_close'></div>" +
             "<div  id='Vehicles_trip_log_detail_map_info_open" + id + "' class='cls_Vehicles_trip_log_detail_map_info_open'>"+
                    "<div class='cls_Vehicles_trip_log_detail_map_info_open_text'> " + LanguageScript.page_vehicles_TripDetail + " </div>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_open_img'></div>" +
                    "</div>" +
             "<div  id='Vehicles_trip_log_detail_map_info_close" + id + "' class='cls_Vehicles_trip_log_detail_map_info_close'>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_close_text'> " + LanguageScript.page_vehicles_TripDetail + " </div>" +
                     "<div class='cls_Vehicles_trip_log_detail_map_info_close_img'></div>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail'>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_distance'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.common_Distance + "</div>" + ":&nbsp;&nbsp;" + map_info_distance + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_time'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_TotalTime + "</div>" + ":&nbsp;&nbsp;" + map_info_time + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_starttime'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_StartTime + "</div>" + ":&nbsp;&nbsp;" + map_info_starttime + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_startlocation'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_StartLocation + "</div>" + ":&nbsp;&nbsp;" + map_info_startlocation + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_endtime'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EndTime + "</div>" + ":&nbsp;&nbsp;" + map_info_endtime + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_endlocation'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EndLocation + "</div>" + ":&nbsp;&nbsp;" + map_info_endlocation + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_slowspeed'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EngineIdle + "</div>" + ":&nbsp;&nbsp;" + map_info_slowspeed + "</div>" +
                    "</div>"+
                    "</div>" +
                    "<div id='cls_Vehicles_trip_log_detail_map" + id + "' class='cls_Vehicles_trip_log_detail_map'></div>" +
                    "<div  class='cls_Vehicles_trip_log_detail_map_cover'></div>" +
                     "<div class='cls_Vehicles_trip_log_detail_line_map'></div>" +
                    "</div>";
    });

   
    var arrayTripRout = new Array();
    for (var i = 0; i < obj.linePoint.length; i++) {
        var point = { lng: 0, lat: 0 };
        point.lng = obj.linePoint[i].longitude;
        point.lat = obj.linePoint[i].latitude;
        arrayTripRout.push(point);
    }
    //Debug

    $("#Vehicles_trip_log_detail_map_info_close" + id).show();

    $("#Vehicles_trip_log_detail_map_info_close" + id).click(function () {
        $("#Vehicles_trip_log_detail_map_info_close" + id).hide();
        $("#Vehicles_trip_log_detail_map_info_open" + id).show();
    });
    $("#Vehicles_trip_log_detail_map_info_open" + id).click(function () {
        $("#Vehicles_trip_log_detail_map_info_open" + id).hide();
        $("#Vehicles_trip_log_detail_map_info_close" + id).show();
    });
    
    var ihpleD_map = new ihpleD_Map(div_id, arrayTripRout[0].lng, arrayTripRout[0].lat, 15, 0, 1, 1);
    var BMapObj = ihpleD_map.get_mapObj();
    var mapObj = BMapObj.get_mapObj();
    var TripRoutMap = new ihpleD_ShowTripRout(mapObj, arrayTripRout);//zhangbo

    TripLogHeight();
    $("#" + map_close_id).click(function (e) {
        var id = $(e.currentTarget).attr('id');
        $("#" + id).parent().remove();
        TripLogHeight();
    });
}

//mabiao 20140301 
function TripLogHeight() {
    if ($("#Vehicles_trip_log")[0].style.display == "none") {
        if ($("#Vehicles_trip_log_list")[0].clientHeight > 780) {
            var height = 300 + $("#Vehicles_trip_log_list")[0].clientHeight;
            document.getElementById("vehicle_detail_info_bg_img").style.height = (height) + "px";
            SetLeftBarHeight(height);
        } else {
            var height = 1080;
            document.getElementById("vehicle_detail_info_bg_img").style.height = (height) + "px";
            SetLeftBarHeight(height);
        }
    } else if ($("#Vehicles_trip_log_list")[0].style.display == "none") {
        if ($("#Vehicles_trip_log")[0].clientHeight > 780) {
            var height = 280 + $("#Vehicles_trip_log")[0].clientHeight;
            document.getElementById("vehicle_detail_info_bg_img").style.height = (height) + "px";
            SetLeftBarHeight(height);
        } else {
            var height = 1060;
            document.getElementById("vehicle_detail_info_bg_img").style.height = (height) + "px";
            SetLeftBarHeight(height);
        }
    }
}


var add_day_API = function (str) {
    return "<div class='cls_Vehicles_trip_log_day'>" +
        "<div class='cls_Vehicles_trip_log_day_date'>"+
            str +
    "</div>" +
    "<div class='cls_Vehicles_trip_log_day_line'></div>" +
"</div>";
}


var add_trip_log_list = function (obj) {
    var view = "";
    //fengpan 20140324 #832
    var startdate = obj.StartTime.substring(0, 4) + "-" + obj.StartTime.substring(4, 6) + "-" + obj.StartTime.substring(6, 8) + " ";
    var enddate = obj.EndTime.substring(0, 4) + "-" + obj.EndTime.substring(4, 6) + "-" + obj.EndTime.substring(6, 8) + " ";
    var starttime = obj.StartTime.substring(8, 10) + ":" + obj.StartTime.substring(10, 12);
    var endtime = obj.EndTime.substring(8, 10) + ":" + obj.EndTime.substring(10, 12);
    var Geonum = obj.geofenceInfo.length;
    var id = obj.StartTime;
    var minus = (new Date(obj.EndTime.substr(0, 4), obj.EndTime.substr(4, 2), obj.EndTime.substr(6, 2)) -
                new Date(obj.StartTime.substr(0, 4), obj.StartTime.substr(4, 2), obj.StartTime.substr(6, 2))) / (24 * 3600 * 1000);
    var alertsCount = 0;
    if (0 == obj.healthStatus) {
        alertsCount++;
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (1 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (0 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (2 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    if (0 < obj.geofenceInfo.length) {
        alertsCount++;
    }
    var listHeight = alertsCount > 3 ? (alertsCount * 17) : 50;
    view += "<div class='cls_Vehicles_trip_log_list_text' style='height:" + listHeight + "px'>" +
                "<div class='cls_Vehicles_trip_log_list_text_time' style='line-height:" + listHeight + "px;top:0px'>" +
                    startdate +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_time' style='top:" + (listHeight-38)/2 + "px'>" +
                    starttime + "<br/>" +
                    endtime +
                    (minus == 0 ? "" : "<div class='cls_minusDay'>+" + minus + "天" + "</div>") +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_location' style='line-height:normal;top:" + (listHeight - 38) / 2 + "px;'>" +
                    "<div class='cls_cutOff' title='" + obj.startLocation + "'>" +
                        obj.startLocation +
                    "</div>" +
                    "<div class='cls_cutOff' title='" + obj.endLocation + "'>" +
                        obj.endLocation +
                    "</div>" +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_distance'  style='line-height:" + listHeight + "px'>" +
                    obj.distance + LanguageScript.unit_km +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_alertinfo'>" +
                    trip_log_list_alertinfo(obj,id) +
                "</div>" +
            "</div>" +
            "<div id='list_" + id + "' class='cls_Vehicles_trip_log_list_text' style='display:none; height:" + (Geonum + 1) / 2 * 17 + "px'>" +
                trip_log_list_alertinfo_detail(obj.geofenceInfo) +
            "</div>";
    return view;
    
}
var trip_log_list_alertinfo = function (obj ,id) {
    var view = "";
    var i = 0;
    if (0 == obj.healthStatus) {
        view += "<div class='cls_trip_log_list_alertinfo_alert'>" + LanguageScript.common_alertTypes_EngineLightOn + "</div>";
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (1 == obj.alerts[i]) {
            view += "<div class='cls_trip_log_list_alertinfo_alert'>" + LanguageScript.common_alertTypes_MOTION + "</div>";
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (0 == obj.alerts[i]) {
            view += "<div class='cls_trip_log_list_alertinfo_alert'>" + LanguageScript.common_alertTypes_SPEED + "</div>";
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (2 == obj.alerts[i]) {
            view += "<div class='cls_trip_log_list_alertinfo_alert'>" + LanguageScript.common_alertTypes_EngineRPM + "</div>";
            break;
        }
    }
    if (0 < obj.geofenceInfo.length) {
        view += "<div class='cls_trip_log_list_alertinfo_geo'>" + LanguageScript.page_vehicledetail_GeofenceAlert + "</div>" +
           "<div class='cls_trip_log_list_alertinfo_alert_img' onclick=ClickAlertInfoShow('list_" + id + "',event)></div>";
    }
    return view;
}
function ClickAlertInfoShow(id, e) {
    if (navigator.userAgent.indexOf("MSIE") >= 0) {
        if ("cls_trip_log_list_alertinfo_alert_img" == e.srcElement.className) {
            $("#" + id).show();
            e.srcElement.className = "cls_trip_log_list_alertinfo_alert_img_hide";
        } else if ("cls_trip_log_list_alertinfo_alert_img_hide" == e.srcElement.className) {
            $("#" + id).hide();
            e.srcElement.className = "cls_trip_log_list_alertinfo_alert_img";
        }
    } else {
        if ("cls_trip_log_list_alertinfo_alert_img" == e.currentTarget.className) {
            $("#" + id).show();
            e.currentTarget.className = "cls_trip_log_list_alertinfo_alert_img_hide";
        } else if ("cls_trip_log_list_alertinfo_alert_img_hide" == e.currentTarget.className) {
            $("#" + id).hide();
            e.currentTarget.className = "cls_trip_log_list_alertinfo_alert_img";
        }
    }
    TripLogHeight();
}
var trip_log_list_alertinfo_detail = function (obj) {
    var view = "";
    var i = 0;
    for (i = 0; i < obj.length; i++) {
        view +="<div class='cls_Vehicles_trip_log_list_text_alertinfo_detail'>" +
                    obj[i]+
                "</div>" ;
    }
    return view;
}
/********mabiao****/
/***triplog***/
function getXmlHttpRequest() {

    if (window.XMLHttpRequest) {// code for all new browsers
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {// code for IE5 and IE6
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    
    return xmlhttp;
}
/************************UI TRIP****************/
var add_day = function (obj) {
    var date = obj.startLocation;
    return "<div class='cls_Vehicles_trip_log_day'>" +
        "<div class='cls_Vehicles_trip_log_day_date'>"+
            date+
    "</div>" +
    "<div class='cls_Vehicles_trip_log_day_line'></div>" +
"</div>";
}
//chenyangwen 20140716 #2124
//function getTripLogData(vehicle) {
function getTripLogData(vehicle, page) {
    var CompanyID = GetCompanyID();
    if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
        //chenyangwen 20140716 #2124
        //getTripLogData_API(vehicle);
        getTripLogData_API(vehicle, page);
        return;
    }
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/Vehicles/GetTripLog', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_common_ViewMore);
                $("#Vehicles_Trip_log_viewmore").click(function () {
                    Loading_Bind("Vehicles_Trip_log_viewmore");
                    //chenyangwen 20140716 #2124
                    //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                    getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), page + 1);
                });
                if (txt == "[]" || txt == "") {
                    return;
                }
                var obj = eval(txt);
                var view = "";
                var lasttrip = obj.length - 1;
                TripLog_Element = obj[lasttrip];
                for (var i = 0; i < obj.length; i++) {
                    if ("10101080000" == transTime(obj[i].StartTime).format("yyyyMMddhhmmss")) {
                        obj[i].StartTime = "";
                    } else {
                        obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss");
                    }
                    if ("10101080000" == transTime(obj[i].EndTime).format("yyyyMMddhhmmss")) {
                        obj[i].EndTime = "";
                    } else {
                        obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss");
                    }
                    if (obj[i].type == "Day") {
                        view += add_day(obj[i]);
                    } else if (obj[i].type == "Normal") {
                        view += Add_Trip_Log(obj[i]);
                    } else if (obj[i].type == "Trailer") {
                        view += Add_Trip_Log_Trailer(obj[i]);
                    } else if (obj[i].type == "Final") {
                        view += Add_Trip_Log_Final(obj[i]);
                        Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
						$("#Vehicles_Trip_log_viewmore").css("cursor", "default");
                    } else if (obj[i].type == "Driving") {
                        view += Add_Trip_Log_Driving(obj[i]);
                    }

                }
                $("#Vehicles_Trip_log_viewmore").before(view);
                TripLogHeight();
                $(".cls_Vehicles_trip_log_detail_One_moreinfo").unbind();
                $(".cls_Vehicles_trip_log_detail_Two_moreinfo").unbind();
                $(".cls_Vehicles_trip_log_detail_One_moreinfo").click(function (e) {
                    var id = $(e.currentTarget).attr('id');
                    getTripLog_MoreData(id);
                });
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //... ...
                Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_common_ViewMore);
                $("#Vehicles_Trip_log_viewmore").click(function () {
                    Loading_Bind("Vehicles_Trip_log_viewmore");
                    //chenyangwen 20140716 #2124
                    //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                    getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), page + 1);
                });
            }
        } else {
            //... ...
        }
    }
    if (TripLog_Element != null) {
        var message = 'vehicle=' + vehicle + '&' + 'type=' + TripLog_Element.type + '&' + 'starttime=' + TripLog_Element.StartTime + '&' + 'endtime=' + TripLog_Element.EndTime;
    } else {
        var message = 'vehicle=' + vehicle + '&' + 'type=' + '&' + 'starttime='+ '&' + 'endtime=';
    }
    xmlhttp.send(message);
}
//正常状态下（没发生拖车情况时 一个节点与下方线段的表示） mabiao 20140304
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
    var DetailHeight = 100 + AddHeight;
    var LineHeight = 70 + AddHeight;
    var midHeight = 34;
    var midText = '';
    var midKMText = '';
    if ((endtime != "" && endtime != undefined) && (starttime != "" && starttime != undefined)) {
        midHeight = 64;
    }
    if ((DetailHeight / 2) > midHeight) {
        midKMText = 'position:absolute;top:' + (DetailHeight / 2 - 16) + 'px;';
        midText = ' style=position:absolute;top:' + (DetailHeight / 2) + 'px;';
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + obj.endLocation + ">" + obj.endLocation + "</div>" +
                    Add_Trip_Log_Info(obj) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle'></div>" +
               "<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time(starttime, endtime) +
               "<div style='font-size:10pt;" + midKMText + "' class='cls_Vehicles_trip_log_detail_info' title='" + LanguageScript.common_MilesTravell +"'>" + obj.distance + LanguageScript.unit_km + "</div>" +
                "<div id=" + date_time + " class='cls_Vehicles_trip_log_detail_One_moreinfo' " + midText + ">" +
                     LanguageScript.page_vehicledetail_ViewRoute +
                "</div>"+
         "</div>" +
   "</div>";
    return view;
}

//拖车情况下（一个节点与下方线段的表示） mabiao 20140304
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
    var AddHeight = Add_Trip_Log_Height(obj);
    var DetailHeight = 100 + AddHeight;
    var LineHeight = 70 + AddHeight;
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + obj.endLocation + ">" + obj.endLocation + "</div>" +
                    //Add_Trip_Log_Info(obj.Content) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle_red'></div>" +
               "<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px;background-color:#FF0000'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time(starttime, endtime) +
               "<div style='font-size:10pt;color:#FF0000;position:absolute;top:50px;' class='cls_Vehicles_trip_log_detail_info' >" + LanguageScript.common_Trailer + "</div>" +
         "</div>" +
   "</div>";

    return view;
}
//最后的节点 只描画节点 并且 显示信息为 离开时间 地点
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
    var DetailHeight = 30;
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + obj.endLocation + ">" + obj.endLocation + "</div>" +
                    //Add_Trip_Log_Info(obj.Content) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle_final'></div>" +
               //"<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px;background-color:#CC0000'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time(starttime, endtime) +
               //"<div style='font-size:10pt;top:10px;color:#CC0000;' class='cls_Vehicles_trip_log_detail_info'  title='发生拖车'>发生拖车</div>" +
         "</div>" +
   "</div>";

    return view;
}
/**********************************************/
//chenyangwen 20140716 #2124
//function getTripLogData_API(vehicle) {
function getTripLogData_API(vehicle, page) {
    var CompanyID = GetCompanyID();
    var timeZone = new Date().getTimezoneOffset() / 60 * -1;
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetTrips",
        url: "/" + CompanyID + "/AjaxResponse/GetTripsInVehicle",
        data: { vehicleID: vehicle, nearTime: TripLog_Element, oldTime: Old_date_time, timeZone: timeZone },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            ihpleDTripHeight = 0;
            Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_common_ViewMore);
            $("#Vehicles_Trip_log_viewmore").click(function () {
                Loading_Bind("Vehicles_Trip_log_viewmore");
                //chenyangwen 20140716 #2124
                //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), page + 1);
            });
            if (null == msg || 0 == msg.length) {
                Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
                $("#Vehicles_Trip_log_viewmore").css("cursor", "default");
                return;
            }
            transformLocation = new Array();
            var obj = msg;
            var view = "";
            for (var i = 0; i < obj.length; i++) {
                if (ihpleDTripHeight > 710) {
                    if ("10101080000" != obj[i - 3].StartTime) {
                        TripLog_Element = obj[i - 3].StartTime;
                    } else if ("10101080000" != obj[i - 3].EndTime) {
                        TripLog_Element = obj[i - 3].EndTime;
                    }
                    break;
                } else {
                    TripLog_Element = null;
                }
                obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss");
                obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss");
                obj[i].distance = (obj[i].distance == null ? 0 : obj[i].distance).toFixed(1);
                if (0 == i ) {
                    //TODO 虚线
                    if (null != TripLog_Element) {
                        continue;
                    }
                    if (obj[i].EndTime == "10101080000" && page == 1) {   //正在行驶的车 到达时间为当前时间加一年
                        view += Add_Trip_Log_Driving();
                        view += DayJudge(obj[i]);
                    } else {
                        view += DayJudge(obj[i]);
                        view += Add_Trip_Log_API(obj[i], "none", obj[i].EndTime);
                    }
                    //obj.StartTime 不是今天
                    //view += day("昨天"/"date");

                } else {
                    if (obj[i].isLastFlag != null && obj[i].isLastFlag != 0 && obj[i - 1].startlocationGPSLng != null && obj[i - 1].startlocationGPSLat != null && obj[i - 1].startlocationGPSLng != 0 && obj[i - 1].startlocationGPSLat != 0 &&
                        obj[i-1].isFirstFlag != null && obj[i-1].isFirstFlag != 0 &&[i].endlocationGPSLng != null && obj[i].endlocationGPSLat != null && obj[i].endlocationGPSLng != 0 && obj[i].endlocationGPSLat != 0 &&
                        GetDistance(obj[i - 1].startlocationGPSLat, obj[i - 1].startlocationGPSLng, obj[i].endlocationGPSLat, obj[i].endlocationGPSLng) > TrailerDistance) {
                        view += Add_Trip_Log_Trailer_API(obj[i - 1], obj[i - 1].StartTime, "none");
                        view += DayJudge(obj[i]);
                        view += Add_Trip_Log_API(obj[i], "none", obj[i].EndTime);
                        view += DayJudge(obj[i]);
                    } else {
                        view += Add_Trip_Log_API(obj[i], obj[i - 1].StartTime, obj[i].EndTime);
                        view += DayJudge(obj[i]);
                    }
                    
                }
                // 第一次trip 判断
                if (i == (msg.length - 1)) {
                    view += Add_Trip_Log_Final_API(obj[i], obj[i].StartTime, "none");
                    Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
					$("#Vehicles_Trip_log_viewmore").css("cursor", "default");
                }
                
            }

            $("#Vehicles_Trip_log_viewmore").before(view);
            TripLogHeight();
            $(".cls_Vehicles_trip_log_detail_One_moreinfo").unbind();
            $(".cls_Vehicles_trip_log_detail_Two_moreinfo").unbind();
            $(".cls_Vehicles_trip_log_detail_One_moreinfo").click(function (e) {
                var id = $(e.currentTarget).attr('id');
                getTripLog_MoreData_API(id);
            });
            for (var k = 0; k < transformLocation.length; ++k) {
                tripcoderLocation(transformLocation[k].lng, transformLocation[k].lat, transformLocation[k].documentID, transformLocation[k].id, transformLocation[k].flag, transformLocation[k].locationFlag,
                                function (adr, id, flag, ElementID) {
                                    var Location = { id:id,location: adr, flag: flag };
                                    ihpleD_TripDetailLocation.push(Location);

                                    var pointLoca = baiduForTripTwo(adr);

                                    $("#" + ElementID + "_Area").empty();
                                    $("#" + ElementID + "_Address").empty();
                                    $("#" + ElementID + "_Area").append(function () {
                                        return "" + pointLoca.area + "";
                                    });
                                    $("#" + ElementID + "_Area").attr("title", adr.replace(/,/g , ""));

                                    $("#" + ElementID + "_Address").append(function () {
                                        return "" + pointLoca.address + "";
                                    });
                                    $("#" + ElementID + "_Address").attr("title", adr.replace(/,/g , ""));
                                    var url = "/Dashboard/WriteLocationToDB"
                                    LocationWriteToDB(url, id, flag, adr);
                                });
            }
        },
        error: function () {
            Loading_UnBind("Vehicles_Trip_log_viewmore", LanguageScript.page_common_ViewMore);
            $("#Vehicles_Trip_log_viewmore").click(function () {
                Loading_Bind("Vehicles_Trip_log_viewmore");
                //chenyanwgen 20140716 #2124
                //getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                getTripLogData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1), page + 1);
            });
        }
    });
}
function getTripLog_MoreData_API(id) {
    var CompanyID = GetCompanyID();
    var timeZone = new Date().getTimezoneOffset() / 60 * - 1;
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/GetTripDetail",
        data: { tripGUID: id ,timeZone:timeZone},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            add_map_API(id, msg);
        }
    });
}
var add_map_API = function (id, obj) {
    if (0 == obj.linePoint.length) {
        tripErrorDialog();
        return;
    }
    var div_id = "cls_Vehicles_trip_log_detail_map" + id;
    var map_close_id = "Vehicles_trip_log_detail_map_close" + id;
    var map_info_distance = obj.distance.toFixed(1) + LanguageScript.unit_km;
    var map_info_time = (obj.DriveTime.Days == 0 ? "" : obj.DriveTime.Days + "天") +
                        (obj.DriveTime.Hours == 0 ? "" : obj.DriveTime.Hours + LanguageScript.common_Hour) +
                        (obj.DriveTime.Minutes == 0 ? "" : obj.DriveTime.Minutes + LanguageScript.common_Minute) + 
                        obj.DriveTime.Seconds + LanguageScript.common_Second;
    var starttime = transTime(obj.StartTime).format("yyyyMMddhhmmss");
    var endtime = transTime(obj.EndTime).format("yyyyMMddhhmmss");
    if ("10101080000" == starttime) {
        var map_info_starttime = LanguageScript.export_undefine;
    } else {
        var map_info_starttime = starttime.substr(8, 2) + ":" + starttime.substr(10, 2) + ":" + starttime.substr(12, 2);
    }
    if (null == obj.startLocation) {
        for (var i = 0; i < ihpleD_TripDetailLocation.length; i++) {
            if (id == ihpleD_TripDetailLocation[i].id && 0 == ihpleD_TripDetailLocation[i].flag) {
                var map_info_startlocation = ihpleD_TripDetailLocation[i].location;
                break;
            }
        }
    }else {
        var map_info_startlocation = obj.startLocation;
    }
    if ("10101080000" == endtime) {
        var map_info_endtime = LanguageScript.export_undefine;
    } else {
        var map_info_endtime = endtime.substr(8, 2) + ":" + endtime.substr(10, 2) +":" + endtime.substr(12, 2);
    }
    if (null == obj.endLocation) {
        for (var i = 0; i < ihpleD_TripDetailLocation.length; i++) {
            if (id == ihpleD_TripDetailLocation[i].id && 1 == ihpleD_TripDetailLocation[i].flag) {
                var map_info_endlocation = ihpleD_TripDetailLocation[i].location;
                break;
            }
        }
    } else {
        var map_info_endlocation = obj.endLocation;
    }
    //var map_info_slowspeed = obj.idleTime.substring(0, 2) + LanguageScript.common_Hour + obj.idleTime.substring(2, 4) + LanguageScript.common_Minute + obj.idleTime.substring(4, 6) + LanguageScript.common_Second;
    var map_info_slowspeed = formatSeconds(obj.idleTime);
    if (document.getElementById(div_id) != null) {
        return;
    }
    if ($(".cls_Vehicles_trip_log_detail_moreinfo").length > 0) {
        $(".cls_Vehicles_trip_log_detail_moreinfo").remove();
    }

    $("#" + id).parent().parent().after(function () {
        return "<div class='cls_Vehicles_trip_log_detail_moreinfo'>" +
            "<div  id='Vehicles_trip_log_detail_map_close" + id + "' class='cls_Vehicles_trip_log_detail_map_close'></div>" +
             "<div  id='Vehicles_trip_log_detail_map_info_open" + id + "' class='cls_Vehicles_trip_log_detail_map_info_open'>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_open_text'> " + LanguageScript.page_vehicles_TripDetail + " </div>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_open_img'></div>" +
                    "</div>" +
             "<div  id='Vehicles_trip_log_detail_map_info_close" + id + "' class='cls_Vehicles_trip_log_detail_map_info_close'>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_close_text'> " + LanguageScript.page_vehicles_TripDetail + " </div>" +
                     "<div class='cls_Vehicles_trip_log_detail_map_info_close_img'></div>" +
                    "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail'>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_distance'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.common_Distance + "</div>" + ":&nbsp;&nbsp;" + map_info_distance + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_time'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_TotalTime + "</div>" + ":&nbsp;&nbsp;" + map_info_time + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_starttime'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_StartTime + "</div>" + ":&nbsp;&nbsp;" + map_info_starttime + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_startlocation'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_StartLocation + "</div>" + "<div class='cls_cutOff' title='" + $.trim(map_info_startlocation).replace(/,/g, "") + "'>:&nbsp;&nbsp;" + baiduLocationForTrip($.trim(map_info_startlocation)) + "</div></div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_endtime'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EndTime + "</div>" + ":&nbsp;&nbsp;" + map_info_endtime + "</div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_endlocation'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EndLocation + "</div>" + "<div class='cls_cutOff' title='" + $.trim(map_info_endlocation).replace(/,/g, "") + "'>:&nbsp;&nbsp;" + baiduLocationForTrip($.trim(map_info_endlocation)) + "</div></div>" +
                        "<div class='cls_Vehicles_trip_log_detail_map_info_close_detail_slowspeed'>" +
                            "<div style='position:relative;float:left;width:31.82%;'>" + LanguageScript.page_vehicledetail_EngineIdle + "</div>" + ":&nbsp;&nbsp;" + map_info_slowspeed + "</div>" +
                    "</div>" +
                    "</div>" +
                    "<div id='cls_Vehicles_trip_log_detail_map" + id + "' class='cls_Vehicles_trip_log_detail_map'></div>" +
                    "<div  class='cls_Vehicles_trip_log_detail_map_cover'></div>" +
                     "<div class='cls_Vehicles_trip_log_detail_line_map'></div>" +
                    "</div>";
    });


    var arrayTripRout = new Array();
    for (var i = 0; i < obj.linePoint.length; i++) {
        var point = { lng: 0, lat: 0 };
        point.lng = obj.linePoint[i].longitude;
        point.lat = obj.linePoint[i].latitude;
        arrayTripRout.push(point);
    }
    //Debug

    $("#Vehicles_trip_log_detail_map_info_close" + id).show();

    $("#Vehicles_trip_log_detail_map_info_close" + id).click(function () {
        $("#Vehicles_trip_log_detail_map_info_close" + id).hide();
        $("#Vehicles_trip_log_detail_map_info_open" + id).show();
    });
    $("#Vehicles_trip_log_detail_map_info_open" + id).click(function () {
        $("#Vehicles_trip_log_detail_map_info_open" + id).hide();
        $("#Vehicles_trip_log_detail_map_info_close" + id).show();
    });
    if(0 != arrayTripRout.length){
        var ihpleD_map = new ihpleD_Map(div_id, arrayTripRout[0].lng, arrayTripRout[0].lat, 15, 0, 1, 1);
        var BMapObj = ihpleD_map.get_mapObj();
        var mapObj = BMapObj.get_mapObj();
        var TripRoutMap = new ihpleD_ShowTripRout(mapObj, arrayTripRout);//zhangbo
    }else {
        var ihpleD_map = new ihpleD_Map(div_id, 116.404, 39.915, 15, 0, 1, 1);
        var BMapObj = ihpleD_map.get_mapObj();
        var mapObj = BMapObj.get_mapObj();
        var TripRoutMap = new ihpleD_ShowTripRout(mapObj, arrayTripRout);//zhangbo
    }
    TripLogHeight();
    $("#" + map_close_id).click(function (e) {
        var id = $(e.currentTarget).attr('id');
        $("#" + id).parent().remove();
        TripLogHeight();
    });
}
var DayJudge = function (obj) {
    //if (ihpleDTripHeight > 710) { return "";}//mabiao for bug 20140429
    if ("10101080000" != obj.StartTime) {
        var dayMinus = minusDay(ihpleDTripDate, obj.StartTime);//跟上个Trip的是否是同一天
        var nowdayMinus = minusDay(ihpleDTripNow, obj.StartTime);//今天
        if (1 == dayMinus && 1 == nowdayMinus) {
            ihpleDTripHeight += 55;
            if (ihpleDTripHeight > 710) {
                return '';
            }
            ihpleDTripDate = obj.StartTime;
            return add_day_API(LanguageScript.common_yesterday);
        } else if (1 <= dayMinus) {
            ihpleDTripHeight += 55;
            if (ihpleDTripHeight > 710) {
                return '';
            }
            ihpleDTripDate = obj.StartTime;
            return add_day_API(obj.StartTime.substr(4, 2) + LanguageScript.common_month + obj.StartTime.substr(6, 2) + LanguageScript.common_day);
        } else {
            return "";
        }
    } else if ("10101080000" != obj.EndTime) {
        var dayMinus = minusDay(ihpleDTripDate, obj.EndTime);
        var nowdayMinus = minusDay(ihpleDTripNow, obj.EndTime);
        if (1 == dayMinus && 1 == nowdayMinus) {
            ihpleDTripHeight += 55;
            if (ihpleDTripHeight > 710) {
                return '';
            }
            ihpleDTripDate = obj.EndTime;
            return add_day_API(LanguageScript.common_yesterday);
        } else if (1 <= dayMinus) {
            ihpleDTripHeight += 55;
            if (ihpleDTripHeight > 710) {
                return '';
            }
            ihpleDTripDate = obj.EndTime;
            return add_day_API(obj.EndTime.substr(4, 2) + LanguageScript.common_month + obj.EndTime.substr(6, 2) + LanguageScript.common_day);
        } else {
            return "";
        }
    }
}
var minusDay = function (day1, day2) {
    return ((new Date(day1.substr(0, 4) + "/" + day1.substr(4, 2) + "/" + day1.substr(6, 2)) -
            new Date(day2.substr(0, 4) + "/" + day2.substr(4, 2) + "/" + day2.substr(6, 2))) /
            (24 * 60 * 60 * 1000));
}
var Add_Trip_Log_Height = function (obj) {
    var length = 0;
    if (0 == obj.healthStatus) {
        length++;
    }
    var height = 0;
    if (obj.alerts != undefined && obj.alerts.length > 0) {
        for (var i = 0; i < obj.alerts.length; i++) {
            if (1 == obj.alerts[i]) {
                length++;
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (0 == obj.alerts[i]) {
                length++;
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (2 == obj.alerts[i]) {
                length++;
                break;
            }
        }
    }
    if (obj.geofenceInfo != undefined) {
        length += obj.geofenceInfo.length;
    }
    if (length >= 5) {
        height = (length - 4) * 18;
    }
    return height;
}

var Add_Trip_Log_Height_API = function (obj) {
    var length = 0;
    if (0 == obj.healthStatus) {
        length++;
    }
    var height = 0;
    if (obj.alerts != undefined && obj.alerts.length > 0) {
        for (var i = 0; i < obj.alerts.length; i++) {
            if (1 == obj.alerts[i]) {
                length++;
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (0 == obj.alerts[i]) {
                length++;
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (2 == obj.alerts[i]) {
                length++;
                break;
            }
        }
    }
    if (obj.geofenceInfo != undefined) {
        length += obj.geofenceInfo.length;
    }
    if (length >= 4) {
        height = (length - 3) * 18;
    }
    return height;
}

//正常状态下（没发生拖车情况时 一个节点与下方线段的表示） mabiao 20140304
var Add_Trip_Log_API = function (obj, start, end) {
    if ($("#" + obj.id + "_location_Area").length > 0) {
        return '';
    }
    if (start == "" || "10101080000" == start ) {
        var starttime = LanguageScript.export_undefine;
    } else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || "10101080000" == end) {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.endLocation) {
        var location_Area = "";
        var location_Address = "";
	var endLocation = "";
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null ,locationFlag:0};
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.documentID = obj.id + "_location";
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocation.push(point);
    } else {
    	var endLocation = obj.endLocation;
        var point = baiduForTripTwo(obj.endLocation);
        var location_Area = point.area;
        var location_Address = point.address;
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
    var date_time = obj.StartTime;
    var AddHeight = Add_Trip_Log_Height_API(obj);
    var DetailHeight = 100 + AddHeight;
    var LineHeight = 70 + AddHeight;
    ihpleDTripHeight += DetailHeight;
    if (ihpleDTripHeight > 710) {
        return "";
    }
    var midHeight = 34;
    var midText = '';
    var midKMText = '';
    if ((endtime != "" && endtime != undefined) && (starttime != "" && starttime != undefined)) {
        midHeight = 64;
    }
    if ((DetailHeight / 2) > midHeight) {
        midKMText = ' position:absolute;top:' + (DetailHeight / 2 - 16) + 'px;';
        midText = ' style=position:absolute;top:' + (DetailHeight / 2) + 'px;';
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div id= '" + obj.id + "_location_Area" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(endLocation).replace(/,/g, "") + ">" + location_Area + "</div>" +
            "<div id= '" + obj.id + "_location_Address" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(endLocation).replace(/,/g, "") + ">" + location_Address + "</div>" +
                    Add_Trip_Log_Info(obj) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle'></div>" +
               "<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time_API(endtime, starttime) +
               "<div style='font-size:10pt;" + midKMText + "' class='cls_Vehicles_trip_log_detail_info' title='" + LanguageScript.common_MilesTravell + "'>" + obj.distance + LanguageScript.unit_km + "</div>" +
               "<div id=" + obj.id + " class='cls_Vehicles_trip_log_detail_One_moreinfo' " + midText + ">" +
                    LanguageScript.page_vehicledetail_ViewRoute +
               "</div>" +
         "</div>" +
   "</div>";

    return view;
}

//拖车情况下（一个节点与下方线段的表示） mabiao 20140304
var Add_Trip_Log_Trailer_API = function (obj, start, end) {
    if ($("#" + obj.id + "_locationTrailer_Area").length > 0) {
        return '';
    }
    if (start == "" || "10101080000" == start) {
        var starttime = LanguageScript.export_undefine;
    } else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || "10101080000" == end) {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.startLocation) {
        var location_Area = "";
        var location_Address = "";
	var startLocation = "";
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.startlocationLng;
        point.lat = obj.startlocationLat;
        point.id = obj.id;
        point.documentID = obj.id + "_locationTrailer";
        point.flag = 0;
        point.locationFlag = obj.isFirstFlag;
        transformLocation.push(point);
    } else {
    	var startLocation = obj.startLocation;
        var point = baiduForTripTwo(obj.startLocation);
        var location_Area = point.area;
        var location_Address = point.address;
    }
    if (null == obj.endLocation) {
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocation.push(point);
    }

    var date_time = obj.StartTime;
    var AddHeight = Add_Trip_Log_Height_API(obj);
    var DetailHeight = 100 + AddHeight;
    var LineHeight = 70 + AddHeight;
    ihpleDTripHeight += DetailHeight;
    if (ihpleDTripHeight > 710) {
        return "";
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div id = '" + obj.id + "_locationTrailer_Area" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(startLocation).replace(/,/g, "") + ">" + location_Area + "</div>" +
            "<div id = '" + obj.id + "_locationTrailer_Address" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(startLocation).replace(/,/g, "") + ">" + location_Address + "</div>" +
                    //Add_Trip_Log_Info(obj.Content) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle_red'></div>" +
               "<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px;background-color:#FF0000'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time_API(endtime, starttime)+
               "<div style='font-size:10pt;color:#FF0000;position:absolute;top:50px;' class='cls_Vehicles_trip_log_detail_info' >" + LanguageScript.common_Trailer + "</div>" +
         "</div>" +
   "</div>";

    return view;
}
//最后的节点 只描画节点 并且 显示信息为 离开时间 地点
var Add_Trip_Log_Final_API = function (obj, start, end) {
    if (start == "" || "10101080000" == start) {
        var starttime = LanguageScript.export_undefine;
    } else if (start != "none") {
        var starttime = start.substring(8, 10) + ":" + start.substring(10, 12);
    } else {
        var starttime = "";
    }
    if (end == "" || "10101080000" == end) {
        var endtime = LanguageScript.export_undefine;
    } else if (end != "none") {
        var endtime = end.substring(8, 10) + ":" + end.substring(10, 12);
    } else {
        var endtime = "";
    }
    if (null == obj.startLocation) {
        var location_Area = "";
        var location_Address = "";
	var startLocation = "";
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.startlocationLng;
        point.lat = obj.startlocationLat;
        point.id = obj.id;
        point.documentID = obj.id + "_locationFinal";
        point.flag = 0;
        point.locationFlag = obj.isFirstFlag;
        transformLocation.push(point);
    } else {
    	var startLocation = obj.startLocation;	
        var point = baiduForTripTwo(obj.startLocation);
        var location_Area = point.area;
        var location_Address = point.address;
    }
    if (null == obj.endLocation) {
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocation.push(point);
    }
    var date_time = obj.StartTime;
    var DetailHeight = 30;
    ihpleDTripHeight += DetailHeight;
    if (ihpleDTripHeight > 710) {
        return "";
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:" + DetailHeight + "px'>" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
            "<div id= '" + obj.id + "_locationFinal_Area" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(startLocation).replace(/,/g,"") + ">" + location_Area + "</div>" +
            "<div id= '" + obj.id + "_locationFinal_Address" + "' style='font-size:10pt;' class='cls_Vehicles_trip_log_detail_One_info' title=" + $.trim(startLocation).replace(/,/g, "") + ">" + location_Address + "</div>" +
                    //Add_Trip_Log_Info(obj.Content) +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle_final'></div>" +
               //"<div class='cls_Vehicles_trip_log_detail_line' style='height:" + LineHeight + "px;background-color:#CC0000'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
                        Add_Trip_Log_Time_API(endtime, starttime) +
               //"<div style='font-size:10pt;top:10px;color:#CC0000;' class='cls_Vehicles_trip_log_detail_info'  title='发生拖车'>发生拖车</div>" +
         "</div>" +
   "</div>";

    return view;
}

//mabiao 20140310 TripLog 需求变更
//引擎On时，算出 发动车时间，用虚线表示
var Add_Trip_Log_Driving = function (obj) {
    ihpleDTripHeight += 70;
    if (ihpleDTripHeight > 710) {
        return "";
    }
    var view = "<div class='cls_Vehicles_trip_log_detail' style='height:70px;' >" +
            "<div class='cls_Vehicles_trip_log_detail_left_icon'>" +
         "</div>" +
         "<div class='cls_Vehicles_trip_log_detail_middle'>" +
               "<div class='cls_Vehicles_trip_log_detail_circle' style='height:0px'></div>" +
               "<div  class='cls_Vehicles_trip_log_detail_line_arrow'></div>" +
          "</div>" +
          "<div class='cls_Vehicles_trip_log_detail_One_right'>" +
         "</div>" +
   "</div>";

    return view;
}

//mabiao 20140304 左侧具体详情Alert
var Add_Trip_Log_Info = function (obj) {
    var view = "";
    if (0 == obj.healthStatus) {
        view += '<div class="cls_Vehicles_trip_log_alert_Two_status" >' + LanguageScript.common_alertTypes_EngineLightOn + '</div>';
    }
    if (obj.alerts != undefined && obj.alerts.length >0) {
        for (var i = 0; i < obj.alerts.length; i++) {
            if (1 == obj.alerts[i]) {
                view += '<div class="cls_Vehicles_trip_log_alert_Two_status" >' + LanguageScript.common_alertTypes_MOTION + '</div>';
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (0 == obj.alerts[i]) {
                view += '<div class="cls_Vehicles_trip_log_alert_Two_status" >' + LanguageScript.common_alertTypes_SPEED + '</div>';
                break;
            }
        }
        for (var i = 0; i < obj.alerts.length; i++) {
            if (2 == obj.alerts[i]) {
                view += '<div class="cls_Vehicles_trip_log_alert_Two_status" >' + LanguageScript.common_alertTypes_EngineRPM + '</div>';
                break;
            }
        }
    }
    if (obj.geofenceInfo != undefined && obj.geofenceInfo.length >0 ) {
        for (var i = 0; i < obj.geofenceInfo.length; i++) {
            view += '<div class="cls_Vehicles_trip_log_detail_Two_info" title="' + obj.geofenceInfo[i] + '">' + obj.geofenceInfo[i] + '</div>';
        }
    }
    return view;
}
var Add_Trip_Log_Time = function (StartTime, EndTime) {
    var view = "<div class='cls_Vehicles_trip_log_detail_One_time' style='top:-10px'>";
    if ((EndTime == "" || EndTime == undefined) || (StartTime == "" || StartTime == undefined)) {
        view = "<div class='cls_Vehicles_trip_log_detail_One_time' style='top:3px'>";
    }
    if (EndTime != "" && EndTime != undefined) {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_End" title="' + LanguageScript.common_LeaveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + EndTime + '</div>';
    }
    if ((EndTime != "" && EndTime != undefined) && (StartTime != "" && StartTime != undefined)) {
        view += "<div class='cls_Vehicles_trip_log_detail_line_arrow10'></div>";
    }
    if (StartTime != "" && StartTime != undefined) {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_Start" title="' + LanguageScript.common_ArriveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + StartTime + '</div>';
    }
    view += '<div style="clear:both;"></div></div>';
    
    return view;
}

/*****************/
var Add_Trip_Log_Time_API = function (EndTime, StartTime) {
    var view = "<div class='cls_Vehicles_trip_log_detail_One_time' style='top:-10px'>";
    if ((EndTime == "" || EndTime == undefined) || (StartTime == "" || StartTime == undefined)) {
        view = "<div class='cls_Vehicles_trip_log_detail_One_time' style='top:5px'>";
    }
    if (StartTime != "" && StartTime != undefined) {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_End" title="' + LanguageScript.common_LeaveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + StartTime + '</div>';
    }
    if ((EndTime != "" && EndTime != undefined) && (StartTime != "" && StartTime != undefined)) {
        view += "<div class='cls_Vehicles_trip_log_detail_line_arrow10'></div>";
    }
    if (EndTime != "" && EndTime != undefined) {
        view += '<div class = "cls_Vehicles_trip_log_detail_Left_Start" title="' + LanguageScript.common_ArriveAt + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + EndTime + '</div>';
    }
    view += '<div style="clear:both;"></div></div>';

    return view;
}
var add_trip_log_list_API = function (obj) {
    var view = "";
    //fengpan 20140324 #832
    if (obj.StartTime != "" && obj.StartTime != "10101080000") {
        var startdate = obj.StartTime.substring(0, 4) + "-" + obj.StartTime.substring(4, 6) + "-" + obj.StartTime.substring(6, 8) + " ";
        var starttime = obj.StartTime.substring(8, 10) + ":" + obj.StartTime.substring(10, 12);
    } else if (obj.StartTime == "") {
        var startdate = "    ";
        var starttime = "    ";
    } else if (obj.StartTime == "10101080000") {
        var startdate = LanguageScript.export_undefine;
        var starttime = LanguageScript.export_undefine;
    }
    if (obj.EndTime != "" && obj.EndTime != "10101080000") {
        var enddate = obj.EndTime.substring(0, 4) + "-" + obj.EndTime.substring(4, 6) + "-" + obj.EndTime.substring(6, 8) + " ";
        var endtime = obj.EndTime.substring(8, 10) + ":" + obj.EndTime.substring(10, 12);
    } else if (obj.EndTime == "") {
        var enddate = "    ";
        var endtime = "    ";
    } else if (obj.EndTime == "10101080000") {
        var enddate = LanguageScript.export_undefine;
        var endtime = LanguageScript.export_undefine;
    }
    if (null == obj.startLocation) {
        var startlocation = '&nbsp';
        var startlocationTip = "";
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null ,locationFlag:0};
        point.lng = obj.startlocationLng;
        point.lat = obj.startlocationLat;
        point.id = obj.id;
        point.documentID = obj.id + "_StartList";
        point.flag = 0;
        point.locationFlag = obj.isFirstFlag;
        transformLocationList.push(point);
    } else {
        var startlocation = baiduLocationForTrip(obj.startLocation);
        var startlocationTip = $.trim(obj.startLocation).replace(/,/g, "");
    }

    if (null == obj.endLocation) {
        var endlocation = '&nbsp';
	var endlocationTip = "";
        var point = { lng: 0, lat: 0, documentID: null, func: null, id: null, flag: null, locationFlag: 0 };
        point.lng = obj.endlocationLng;
        point.lat = obj.endlocationLat;
        point.id = obj.id;
        point.documentID = obj.id + "_EndList";
        point.flag = 1;
        point.locationFlag = obj.isLastFlag;
        transformLocationList.push(point);
    } else {
        var endlocation = baiduLocationForTrip(obj.endLocation);
        var endlocationTip = $.trim(obj.endLocation).replace(/,/g, "");
    }
    var Geonum = obj.geofenceInfo.length;
    var id = obj.StartTime;

    var minus = 0;
    if (obj.EndTime != "" && obj.EndTime != "10101080000" && obj.StartTime != "" && obj.StartTime != "10101080000") {
        minus =( new Date(obj.EndTime.substr(0, 4), obj.EndTime.substr(4, 2), obj.EndTime.substr(6, 2)) -
                new Date(obj.StartTime.substr(0, 4), obj.StartTime.substr(4, 2), obj.StartTime.substr(6, 2)))/(24*3600*1000);
    }
    var alertsCount = 0;
    if (0 == obj.healthStatus) {
        alertsCount++;
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (1 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (0 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    for (i = 0; i < obj.alerts.length; i++) {
        if (2 == obj.alerts[i]) {
            alertsCount++;
            break;
        }
    }
    if (0 < obj.geofenceInfo.length) {
        alertsCount++;
    }
    var listHeight = alertsCount > 3 ? (alertsCount * 17) : 50;

    view += "<div class='cls_Vehicles_trip_log_list_text' style='height:" + listHeight + "px';>" +
                "<div class='cls_Vehicles_trip_log_list_text_time' style='line-height:" + listHeight + "px;top:0px'>" +
                    startdate +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_time'>" +
                    starttime+"<br/>"+
                    endtime +
                    (minus == 0?"":"<div class='cls_minusDay'>+" + minus + "天" + "</div>" )+
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_location'  style='line-height:normal;top:6px;'>" +
                    "<div id='" + obj.id + "_StartList" + "' class='cls_cutOff' title='" + startlocationTip + "'>" +
                        startlocation +
                    "</div>" +
                    "<div id='" + obj.id + "_EndList" + "' class='cls_cutOff' title='" + endlocationTip + "'>" +
                    endlocation +
                    "</div>"+
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_distance' >" +
                    obj.distance + LanguageScript.unit_km +
                "</div>" +
                "<div class='cls_Vehicles_trip_log_list_text_alertinfo'>" +
                    trip_log_list_alertinfo(obj, id) +
                "</div>" +
            "</div>" +
            "<div id='list_" + id + "' class='cls_Vehicles_trip_log_list_text' style='display:none; height:" + (Geonum + 1) / 2 * 17 + "px'>" +
                trip_log_list_alertinfo_detail(obj.geofenceInfo) +
            "</div>";
    return view;

}
function getTripLog_ListData_API(vehicle) {
    var CompanyID = GetCompanyID();
    var timeZone = new Date().getTimezoneOffset() / 60  * - 1;
    $.ajax({
        type: "POST",
        //url: "/" + CompanyID + "/Vehicles/GetTrips",
        url: "/" + CompanyID + "/AjaxResponse/GetTripsInVehicle",
        data: { vehicleID: vehicle, nearTime: TripLogList_Element, oldTime: Old_date_time, timeZone: timeZone },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_common_ViewMore);
            $("#Vehicles_Trip_log_list_viewmore").click(function () {
                Loading_Bind("Vehicles_Trip_log_list_viewmore");
                getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
            });
            if (null == msg || 0 == msg.length) {
                Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
                $("#Vehicles_Trip_log_list_viewmore").css("cursor", "default");
                return;
            }
            transformLocationList = new Array();
            var obj = msg;
            var view = "";
            for (var i = 0; i < obj.length; i++) {
                if (i > 14) { break;}
                obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss");
                obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss");
                obj[i].distance = (obj[i].distance == null ? 0 : obj[i].distance).toFixed(1);
                if ("10101080000" != obj[i].StartTime) {
                    TripLogList_Element = obj[i].StartTime;
                } else if ("10101080000" != obj[i].EndTime) {
                    TripLogList_Element = obj[i].EndTime;
                }
                if (0 == i && obj[i].EndTime == "10101080000") {
                    obj[i].EndTime = "";
                    obj[i].endLocation = LanguageScript.common_driving;
                }
                view += add_trip_log_list_API(obj[i]);
                if (i == (obj.length - 1)) {
                    Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
					$("#Vehicles_Trip_log_list_viewmore").css("cursor", "default");
                }
            }

            $("#Vehicles_Trip_log_list_viewmore").before(view);
            TripLogHeight();
            for (var k = 0; k < transformLocationList.length; ++k) {
                tripcoderLocation(transformLocationList[k].lng, transformLocationList[k].lat, transformLocationList[k].documentID, transformLocationList[k].id, transformLocationList[k].flag, transformLocationList[k].locationFlag,
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
        },
        error: function () {
            //数据请求失败
            Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_common_ViewMore);
            $("#Vehicles_Trip_log_list_viewmore").click(function () {
                Loading_Bind("Vehicles_Trip_log_viewmore");
                getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
            });
        }
    });
}
function getTripLog_ListData(vehicle) {
    var CompanyID = GetCompanyID();
    if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
        getTripLog_ListData_API(vehicle);
        return;
    }
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/Vehicles/GetTripLogList', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_common_ViewMore);
                $("#Vehicles_Trip_log_list_viewmore").click(function () {
                    Loading_Bind("Vehicles_Trip_log_list_viewmore");
                    getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                });
                if (txt == "[]" || txt == "") {
                    return;
                }
                var obj = eval("(" + txt + ")");
                var view = "";
                var lasttrip = obj.length - 1;
                TripLogList_Element = obj[lasttrip];
                for (var i = 0; i < obj.length; i++) {
                    obj[i].StartTime = transTime(obj[i].StartTime).format("yyyyMMddhhmmss");
                    obj[i].EndTime = transTime(obj[i].EndTime).format("yyyyMMddhhmmss");
                    view += add_trip_log_list(obj[i]);
                }
                if (15 > obj.length) {
                    Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_vehicleDetail_ShowAll);
					$("#Vehicles_Trip_log_list_viewmore").css("cursor", "default");
                }
                $("#Vehicles_Trip_log_list_viewmore").before(view);
                TripLogHeight();
                //chenyangwen 20140528 #1653
            } else if (499 == xmlhttp.status) {
                window.location.href = "/";
            }
            else {
                //数据请求失败
                Loading_UnBind("Vehicles_Trip_log_list_viewmore", LanguageScript.page_common_ViewMore);
                $("#Vehicles_Trip_log_list_viewmore").click(function () {
                    Loading_Bind("Vehicles_Trip_log_viewmore");
                    getTripLog_ListData(location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1));
                });
            }
        } else {
            //... ...
        }
    }
    if (TripLogList_Element != null) {
        var message = 'vehicle=' + vehicle + '&' + 'type=' + TripLogList_Element.type + '&' + 'starttime=' + TripLogList_Element.StartTime + '&' + 'endtime=' + TripLogList_Element.EndTime;
    } else {
        var message = 'vehicle=' + vehicle + '&' + 'type=' + '&' + 'starttime=' + '&' + 'endtime=';
    }
    xmlhttp.send(message);
}

/*****************/
/********trip log more******/
function getTripLog_MoreData(id) {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    //使用post传送
    xmlhttp.open('post', '/' + CompanyID + '/Vehicles/GetTripLogMore', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                var obj = eval("(" + txt + ")");
                add_map(id, obj);
            } else {
                //... ...
            }
            //chenyangwen 20140528 #1653
        } else if (499 == xmlhttp.status) {
            window.location.href = "/";
        }
        else {
            //... ...
        }
    }
    var message = 'vehicle=1' + '&' + 'endtime=' + id;
    xmlhttp.send(message);
}

function getFuelLogData() {
    var CompanyID = GetCompanyID();
    var xmlhttp = getXmlHttpRequest();
    $("#alert_choose_oil").change(function () {
        $("#alert_choose_oil_1").empty();
        var year = $("#alert_choose_oil").val();
        var today = new Date();
        if (year == today.getFullYear().toString()) {
            for (var i = 1; i <= today.getMonth() + 1 ; i++) {
                if (i < 10) {
                    $("#alert_choose_oil_1").append("<option>0" + i + "</option>");
                } else {
                    $("#alert_choose_oil_1").append("<option>" + i + "</option>");
                }
            }
        } else if (year >= 2013) {
            for (var i = 1; i <= 12; i++) {
                if (i < 10) {
                    $("#alert_choose_oil_1").append("<option>0" + i + "</option>");
                } else {
                    $("#alert_choose_oil_1").append("<option>" + i + "</option>");
                }
            }
        }
        $("#alert_choose_oil_1").selectpicker('refresh');
    })
    //使用post传送
    //xmlhttp.open('post', '/' + CompanyID + '/Vehicles/GetFuelLog', true);
    xmlhttp.open('post', '/' + CompanyID + '/AjaxResponse/GetFuelLog', true);
    //chenyangwen 20140528 #1653
    xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    //post方式需要设置消息头
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = function () {
        if (4 == xmlhttp.readyState) {
            if (200 == xmlhttp.status) {
                var txt = xmlhttp.responseText;
                if (txt == "") {
                    return;
                }
                var obj = eval("(" + txt + ")");
                var view = "";
                for (var i = 0; i < obj.length; i++) {
                    view +=FuelLogView(obj[i].date,obj[i].since,obj[i].L,obj[i].cost,obj[i].Mile_Cost);
                }
                $("#Vehicles_fuel_log_list_viewmore").before(view);
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
    xmlhttp.send(null);
}

var FuelLogView = function( date,since,L,cost,Mile_Cost){
    return '<div class="cls_Vehicles_fuel_log_list_text">'+
                '<div class="cls_Vehicles_fuel_log_list_text_time">'+ date +'</div>'+
                '<div class="cls_Vehicles_fuel_log_list_text_status">' + since + LanguageScript.unit_km + ' </div>' +
                '<div class="cls_Vehicles_fuel_log_list_text_galllon">' + L + LanguageScript.common_Litre + '</div>' +
                '<div class="cls_Vehicles_fuel_log_list_text_EA">￥'+ cost+'</div>'+
                '<div class="cls_Vehicles_fuel_log_list_text_CM">￥' + Mile_Cost + '</div>' +
          '</div>';
}

//Loading Img
function Loading_Bind(id) {
    $("#" + id).empty();
    $("#" + id).unbind();
    $("#" + id).css("cursor", "default");
    $("#" + id).append(function () {
        return '<div id="loading_img"><img src="../../../Content/Common/images/loading_style.gif"  style="position:relative;width:35px;top:0px;"/></div>' +
                '<div id="loading_text"> ' + LanguageScript.page_vehicledetail_Loading + '</div>';
    });
}

//Loading Img
function Loading_UnBind(id, Loading_Text) {
    $("#" + id).unbind();
    $("#" + id).css("cursor", "pointer");
    $("#" + id).empty();
    $("#" + id)[0].innerHTML = Loading_Text;
}


// Chart function
function editit(str, si, pi, plot) {
    return "<b><i>NHT: " + plot.targetId + ', Series: ' + si + ', Point: ' + pi + ', ' + str + "</b></i>";
}

/*****zhangbo 右上角Location map***/
function showRightMap() {

    var VehicleInfo = { lng: 116.404, lat: 39.915, iconKind: 1 };
    var iconKind = 1;
    if (($("#VehicleMisState").val() == 1) || ($("#VehicleMisState").val() == "MISSED")) {
        iconKind = 5;
    } else {
        if ($("#VehicleEngine").val() == "ON") {
            if (($("#VehicleHealth").val() == "Alert") || ($("#VehicleAlert").val() == "Alert")) {
                iconKind = 3;
            } else {
                iconKind = 1;
            }
        } else if ($("#VehicleEngine").val() == "OFF") {
            if (($("#VehicleHealth").val() == "Alert") || ($("#VehicleAlert").val() == "Alert")) {
                iconKind = 4;
            } else {
                iconKind = 2;
            }
        }
    }

    if ($("#VehicleLocationLongtitude").val() != null && $("#VehicleLocationLatitude").val() != 0 &&
            $("#VehicleLocationLongtitude").val() != null && $("#VehicleLocationLatitude").val() != 0) {
        VehicleInfo.lat = $("#VehicleLocationLatitude").val();
        VehicleInfo.lng = $("#VehicleLocationLongtitude").val();
    }

    VehicleInfo.iconKind = iconKind;

    //如果有坐标（0,0）的点，那么默认在上海（121.483875,31.225202 ）
    if (0 < VehicleInfo.lng && 1 > VehicleInfo.lng && 0 < VehicleInfo.lat && 1 > VehicleInfo.lat) {
        VehicleInfo.lng = 121.483875;
        VehicleInfo.lat = 31.225202;
    }

    var ihpleD_map = new ihpleD_Map("vehicle_detail_BMap", VehicleInfo.lng, VehicleInfo.lat, 14, 0, 0, 0);
    RightBMapObj = ihpleD_map.get_mapObj();
    var mapObj = RightBMapObj.get_mapObj();
    var vehiclesMap = new ihpleD_ShowOneVehicle(mapObj, VehicleInfo);
}
var baiduForTripTwo = function (adr) {
    var adrArray = $.trim(adr).split(",");
    var area = "";
    var address = "";
    for (var m = 0; m < adrArray.length; m++) {
        if (m <= 1) {
            area += adrArray[m];
        } else {
            address += adrArray[m];
        }
    }
    var point = { area: area, address: address };
    return point;
}

//trip route 为空 
//mabiao 20140511 
function tripErrorDialog() {
    var text = LanguageScript.page_vehicleDetail_tripRouteError;
    $(".tripError")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".tripError").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}


function updateRightMap(currentVehicle) {
    var VehicleInfo = { lng: 116.404, lat: 39.915, iconKind: 1 };
    var iconKind = 1;
    if (1 == currentVehicle.misState) {
        iconKind = 5
    } else {
        if (0 == currentVehicle.engineStatus) {
            if (3 == currentVehicle.alertType && 0 != currentVehicle.healthStatus) {
                iconKind = 1;
            }
            else {
                iconKind = 3;
            }
        } else {
            if (3 == currentVehicle.alertType && 0 != currentVehicle.healthStatus) {
                iconKind = 2;
            }
            else {
                iconKind = 4;
            }
        }
    }
    if (currentVehicle.location.longitude != null && currentVehicle.location.longitude != 0 &&
            currentVehicle.location.latitude != null && currentVehicle.location.latitude != 0) {
        VehicleInfo.lat = currentVehicle.location.latitude;
        VehicleInfo.lng = currentVehicle.location.longitude;
    }

    VehicleInfo.iconKind = iconKind;

    //如果有坐标（0,0）的点，那么默认在上海（121.483875,31.225202 ）
    if (0 < VehicleInfo.lng && 1 > VehicleInfo.lng && 0 < VehicleInfo.lat && 1 > VehicleInfo.lat) {
        VehicleInfo.lng = 121.483875;
        VehicleInfo.lat = 31.225202;
    }

    var mapObj = RightBMapObj.get_mapObj();

    //清空上次mark 
    mapObj.clearOverlays();
    RightBMapObj.setNewMapView(VehicleInfo.lng, VehicleInfo.lat, 14);

    //再描绘
    var vehiclesMap = new ihpleD_ShowOneVehicle(mapObj, VehicleInfo);
}

function initSelectMonth() {
    var year = $("#geofence_choose_month").val();
    var today = new Date();
    if (year == today.getFullYear().toString()) {
        for (var i = 1; i < today.getMonth() + 1 ; i++) {
            if (i < 10) {
                $("#geofence_choose_month_1").append("<option>0" + i + "</option>");
                $("#alert_choose_month_1").append("<option>0" + i + "</option>");
                $("#alert_choose_oil_1").append("<option>0" + i + "</option>");
            } else {
                $("#geofence_choose_month_1").append("<option>" + i + "</option>");
                $("#alert_choose_month_1").append("<option>" + i + "</option>");
                $("#alert_choose_oil_1").append("<option>" + i + "</option>");
            }
        }
        if (today.getMonth() + 1 < 10) {
            $("#geofence_choose_month_1").append('<option selected="selected">0' + (today.getMonth() + 1) + '</option>');
            $("#alert_choose_month_1").append('<option selected="selected">0' + (today.getMonth() + 1) + '</option>');
            $("#alert_choose_oil_1").append('<option selected="selected">0' + (today.getMonth() + 1) + '</option>');
        } else {
            $("#geofence_choose_month_1").append('<option selected="selected">' + (today.getMonth() + 1) + '</option>');
            $("#alert_choose_month_1").append('<option selected="selected">' + (today.getMonth() + 1) + '</option>');
            $("#alert_choose_oil_1").append('<option selected="selected">' + (today.getMonth() + 1) + '</option>');
        }
    } else if (year >= 2013) {
        for (var i = 1; i <= 12; i++) {
            if (i < 10) {
                $("#geofence_choose_month_1").append("<option>0" + i + "</option>");
                $("#alert_choose_month_1").append("<option>0" + i + "</option>");
                $("#alert_choose_oil_1").append("<option>0" + i + "</option>");
            } else {
                $("#geofence_choose_month_1").append("<option>" + i + "</option>");
                $("#alert_choose_month_1").append("<option>" + i + "</option>");
                $("#alert_choose_oil_1").append("<option>" + i + "</option>");
            }
        }
    }

    $("#geofence_choose_month_1").selectpicker();
    $("#alert_choose_month_1").selectpicker();
    $("#alert_choose_oil_1").selectpicker();
    $("#alert_choose_month").selectpicker();
    $("#geofence_choose_month").selectpicker();
    $("#alert_choose_oil").selectpicker();
    
}