$(document).ready(function () {
    ihpleDDtcFlag = null;
    ihpleDDtcThread = null;
    $("#vehicle_detail_DTC_warning").hide();
    $("#vehicle_detail_DTC_popup").hide();
    $("#DiagnosticTitle_Wrong").hide();
    $("#DiagnosticTitle_DTC").hide();
    $("#vehicle_detail_DTC_popup_mid").hide();
    //TO DO在API中获取当前状态
    //TO DO在API中获取上次DTC
    $("#vehicle_detail_DTC").click(function () {
        $("#vehicle_detail_DTC_popup").show();
        GetDtcFromDB();
        ihpleDDtcThread = window.setInterval(function () {
            GetDtcFromDB();
        }, 1 * 1000 * 2);
    });
    $("#vehicle_detail_DTC_popup_close").click(function () {
        $("#vehicle_detail_DTC_popup").hide();
        $("#vehicle_detail_DTC_warning").hide();
        window.clearInterval(ihpleDDtcThread);
    });

});


//当前不是获取中 判断当前状态 显示状态条
//mabiao 20140409 
function DisplayInit() {
    var view = "";

    //在点击获取或清除按钮前，请确认车辆正处于待检状态。
    view += '<div id="DTCInit" class="DTCBtn">' + LanguageScript.common_GetDTC + '</div>';
    view += '<div id="DTCText" style="color:#CC0000">' + LanguageScript.page_vehicleDetail_dtcScan + '</div>';
    view += '<div id="DTCInitClear01" class="DTCBtnClear01" style="background-color:rgb(255, 255, 255); cursor:pointer;" >' + LanguageScript.page_vehicleDetail_dtcClear01 + '</div>';
    view += '<div id="DTCInitClear02" class="DTCBtnClear02" style="background-color:rgb(255, 255, 255); cursor:pointer;" >' + LanguageScript.page_vehicleDetail_dtcClear02 + '</div>';

    $("#vehicle_detail_DTC_popup_up").empty();
    $("#vehicle_detail_DTC_popup_up").append(view);
    $("#DTCInit").click(function () {
        clickGetDTCBtn();
    });

    //add by zhangbo for clear
    clickClearDeviceDiagnostic();
    clickClearServerDiagnostic();
}

//获取故障码 
//mabiao 20140409
function GetDTCCode() {
    var view = '';

    //故障码获取中请耐心等待...
    view += '<div id="DTCComfirm" class="DTCBtn " style="background-color:rgb(177, 172, 172); cursor:default;">' + LanguageScript.common_GetDTC + '</div>';
    view += '<div id="DTCText" style="top:15px; color:#339900 ">' + LanguageScript.page_vehicleDetail_dtcProgress + '</div>';
    view += '<div id="DTCInitClear01" class="DTCBtnClear01" style="background-color:rgb(177, 172, 172); cursor:default;" >' + LanguageScript.page_vehicleDetail_dtcClear01 + '</div>';
    view += '<div id="DTCInitClear02" class="DTCBtnClear02" style="background-color:rgb(177, 172, 172); cursor:default;" >' + LanguageScript.page_vehicleDetail_dtcClear02 + '</div>';

    //console.log("function GetDTCCode()");

    $("#vehicle_detail_DTC_popup_up").empty();
    $("#vehicle_detail_DTC_popup_up").append(view);
    
}

//故障码清除中，请耐心等待...
function CleaningCode(){
    var view = '';

    //故障码清除中，请耐心等待...
    view += '<div id="DTCComfirm" class="DTCBtn " style="background-color:rgb(177, 172, 172); cursor:default;">' + LanguageScript.common_GetDTC + '</div>';
    view += '<div id="DTCText" style="top:15px; color:#339900 ">' + LanguageScript.page_vehicleDetail_dtcClear03 + '</div>';
    view += '<div id="DTCInitClear01" class="DTCBtnClear01" style="background-color:rgb(177, 172, 172); cursor:default;" >' + LanguageScript.page_vehicleDetail_dtcClear01 + '</div>';
    view += '<div id="DTCInitClear02" class="DTCBtnClear02" style="background-color:rgb(177, 172, 172); cursor:default;" >' + LanguageScript.page_vehicleDetail_dtcClear02 + '</div>';

    //console.log("function CleaningCode()");

    $("#vehicle_detail_DTC_popup_up").empty();
    $("#vehicle_detail_DTC_popup_up").append(view);
}

//已经获取到状态码
function GetDTCCodeAlready() {
    var view = "";

    //在点击获取或清除按钮前，请确认车辆正处于待检状态。
    view += '<div id="DTCInit" class="DTCBtn">' + LanguageScript.common_GetDTC + '</div>';
    view += '<div id="DTCText" style="color:#CC0000">' + LanguageScript.page_vehicleDetail_dtcScan + '</div>';
    view += '<div id="DTCInitClear01" class="DTCBtnClear01" style="background-color:rgb(255, 255, 255); cursor:pointer;" >' + LanguageScript.page_vehicleDetail_dtcClear01 + '</div>';
    view += '<div id="DTCInitClear02" class="DTCBtnClear02" style="background-color:rgb(255, 255, 255); cursor:pointer;" >' + LanguageScript.page_vehicleDetail_dtcClear02 + '</div>';

    //console.log("function GetDTCByApi()");

    $("#vehicle_detail_DTC_popup_up").empty();
    $("#vehicle_detail_DTC_popup_up").append(view);

    $("#DTCInit").click(function () {
        clickGetDTCBtn();
    });

    //add by zhangbo for clear
    clickClearDeviceDiagnostic();
    clickClearServerDiagnostic();
}

//后台通过Api获取DTC码
function GetDTCByApi() {
    var vehicleID = location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1);
    var CompanyID = GetCompanyID();
    ihpleDDtcFlag = new Date();

    //console.log("function GetDTCByApi()");

    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/DTCThreadStart",
        data: { vehicleID: vehicleID },
        contentType: "application/x-www-form-urlencoded",
    });
}

//台前请求后台，判断当前状态
//TO DO
//mabiao 20140415
function GetDtcFromDB() {
    var vehicleID = location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1);
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/GetDtcFromDB",
        data: { vehicleID: vehicleID },
        contentType: "application/x-www-form-urlencoded",
        datatype: "json",
        success: function (msg) {
            var view = "";
            if (null == msg || 0 == msg.length) {
                if (null != ihpleDDtcFlag) {
                    return;
                }

                //如果为空，则不显示
                $("#vehicle_detail_DTC_popup_down_table tr:not(#DiagnosticTitle_DTC):not(#DiagnosticTitle_Wrong)").remove();
                $("#DiagnosticTitle_Wrong").hide();
                $("#DiagnosticTitle_DTC").show();
                DisplayInit();
            } else {
                CodeSet(msg);
            }
            $("#loading_img_DTC").hide();
            $("#vehicle_detail_DTC_popup_mid").show();
            ihpleDDtcFlag = null;
        },
        error: function () {

        }
    });
}

var CodeSet = function (msg) {
    if (null != ihpleDDtcFlag && (ihpleDDtcFlag - transTime(msg[0].lastReadDate)) >= 0) {
        return ;
    }
    $("#vehicle_detail_DTC_popup_down_table tr:not(#DiagnosticTitle_DTC):not(#DiagnosticTitle_Wrong)").remove();
    $("#DiagnosticTitle_Wrong").hide();
    $("#DiagnosticTitle_DTC").show();
    var view = "";
    var ProgressFlag = 0;
    for (var i = 0; i < msg.length; i++) {
        if ("InProgress" == $.trim(msg[i].status) || "Cleaning" == $.trim(msg[i].status)) {
            var minusMinute = (new Date() - transTime(msg[i].lastReadDate)) / (1000 * 60);
            var errorView = "";
            if (minusMinute >= 3) {
                errorView = '<tr style="height:20px; line-height:20px;background-color:#e4e4e4">' +
                            '<td style="width:90px;text-align:center;">' + LanguageScript.page_vehicle_detail_DTC_Error_Code_Error + '</td>' +
                            '<td style="width:180px;text-align:center;">' + LanguageScript.page_vehicleDetail_dtcTimeOut + '</td>' +
                            '</tr>';
                $("#vehicle_detail_DTC_popup_down_table").append(errorView);

                //zhangbo change
                GetDTCCodeAlready();
                $("#DiagnosticTitle_Wrong").show();
                $("#DiagnosticTitle_DTC").hide();
                return;
            } else {

                if ("InProgress" == $.trim(msg[i].status)) { GetDTCCode(); }
                else if ("Cleaning" == $.trim(msg[i].status)) { CleaningCode(); }
                ProgressFlag = 1;
            }
        } else {
            view += '<tr style="height:20px; line-height:20px;background-color:#e4e4e4">' +
                      '<td style="width:90px;text-align:center;">' + msg[i].code + '</td>' +
                      '<td style="width:180px;text-align:center;">' + getErrorMessageByErrorCode(msg[i]) + '</td>' +
                      '</tr>';
            if ("Fail" == $.trim(msg[i].status)) {
                $("#DiagnosticTitle_Wrong").show();
                $("#DiagnosticTitle_DTC").hide();
            }
        }
    }
    $("#vehicle_detail_DTC_popup_down_table").append(view);

    if (0 == ProgressFlag) {
        
        //zhangbo change
        GetDTCCodeAlready();
    }
}

function ajaxClearDeviceDiagnostic() {

    var vehicleID = location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1);
    var CompanyID = GetCompanyID();

   $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/ClearDeviceDiagnostic",
        data: { vehicleID: vehicleID },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            if (msg != "False") {
                ihpleDDtcFlag = null;
            }
        },
        error: function () {

        }
    });
}

function ajaxClearServerDiagnostic() {

    var vehicleID = location.href.substr(location.href.indexOf("=") + 1, location.href.length - 1);
    var CompanyID = GetCompanyID();

    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/ClearServerDiagnostic",
        data: { vehicleID: vehicleID },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            if (msg != "False") {
                ihpleDDtcFlag = null;
            }
        },
        error: function () {

        }
    });
}

function clickClearDeviceDiagnostic() {

    $("#DTCInitClear01").click(function () {
        clickClearDeviceDTCBtn();
    });
}

function clickClearServerDiagnostic() {

    $("#DTCInitClear02").click(function () {
        clickClearServerDTCBtn();
    });
}

function clickGetDTCBtn() {
    $("#vehicle_detail_DTC_warning_btn_continue").unbind();
    $("#vehicle_detail_DTC_warning_btn_cancel").unbind();
    $("#vehicle_detail_DTC_warning_content").text("");
    $("#vehicle_detail_DTC_warning_content").text(LanguageScript.page_vehicle_detail_DTC_warning_content_1);

    $("#vehicle_detail_DTC_warning").show();
    $("#vehicle_detail_DTC_warning_close").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_cancel").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_continue").click(function () {
        $("#vehicle_detail_DTC_warning").hide();

        $("#DTCInit").unbind();
        GetDTCCode();
        GetDTCByApi();
    });
}

function clickClearDeviceDTCBtn() {
    $("#vehicle_detail_DTC_warning_btn_continue").unbind();
    $("#vehicle_detail_DTC_warning_btn_cancel").unbind();
    $("#vehicle_detail_DTC_warning_content").text("");
    $("#vehicle_detail_DTC_warning_content").text(LanguageScript.page_vehicle_detail_DTC_warning_content_1);

    $("#vehicle_detail_DTC_warning").show();
    $("#vehicle_detail_DTC_warning_close").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_cancel").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_continue").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
        $("#DTCInitClear01").unbind();
        CleaningCode();
        ihpleDDtcFlag = new Date();
        ajaxClearDeviceDiagnostic();
    });
}

function clickClearServerDTCBtn() {
    $("#vehicle_detail_DTC_warning_btn_continue").unbind();
    $("#vehicle_detail_DTC_warning_btn_cancel").unbind();
    $("#vehicle_detail_DTC_warning_content").text("");
    $("#vehicle_detail_DTC_warning_content").text(LanguageScript.page_vehicle_detail_DTC_warning_content_2);

    $("#vehicle_detail_DTC_warning").show();
    $("#vehicle_detail_DTC_warning_close").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_cancel").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
    });
    $("#vehicle_detail_DTC_warning_btn_continue").click(function () {
        $("#vehicle_detail_DTC_warning").hide();
        $("#DTCInitClear02").unbind();
        CleaningCode();
        ihpleDDtcFlag = new Date();
        ajaxClearServerDiagnostic();
    });
}

//如果是错误码（注意不是故障码）,那么通过这个函数取得中文的错误信息。
function getErrorMessageByErrorCode(oneMessage) {

    var message = "";

    switch ($.trim(oneMessage.code)) {
        case "E-20200":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20200;
            break;
        case "E-20201":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20201;
            break;
        case "E-20202":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20202;
            break;
        case "E-20203":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20203;
            break;
        case "E-20204":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20204;
            break;
        case "E-20205":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20205;
            break;
        case "E-20206":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20206;
            break;
        case "E-20207":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20207;
            break;
        case "E-20214":
            message = LanguageScript.page_vehicle_detail_DTC_Error_Code_E20214;
            break;
        default:
            message = oneMessage.message;
    }

    return message;
}
