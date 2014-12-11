var commit = [];//全局commit 变量。
var PreviousVIN = [0]; //
var logosubmitflag = [0];//图片变更flag
var odometerSubmitflag = [0];//里程变更flag
var arrAllSetting = new Array();//保存所有车辆tr信息
var currentPageAll = 1;//默认显示第一页
var onePageNum = 4;//一页显示多少个
//fengpan 20140625
function VehicleOneTrInfo(vehicleID, vehicleName, vin, vehicleInformation, vehicleDriver, vehicleLicence, telephone, ESN, registionKey, isVinEditable, IsMMYEditable, mmyid, odometer, vehiclelable, loadingFlag) {
    var CompanyID = GetCompanyID();
    vehicleName = vehicleName == null ? '' : vehicleName;
    vin = vin == null ? '' : vin;
    var tempInfo = "";
    if (vehicleInformation != null && vehicleInformation != "" && vehicleInformation.substring(1, 2) == " ") {
        tempInfo = vehicleInformation.substring(1, vehicleInformation.length);
        tempInfo = $.trim(tempInfo);
        vehicleInformation = tempInfo;
    } else {
        vehicleInformation = vehicleInformation == null ? '' : vehicleInformation;
    }
    vehicleDriver = vehicleDriver == null ? '' : vehicleDriver;
    vehicleLicence = vehicleLicence == null ? '' : vehicleLicence;
    vehicleName = vehicleName == null ? '' : vehicleName;
    telephone = telephone == null ? '' : telephone;
    var isVinEditable = isVinEditable == null ? '' : isVinEditable;
    var IsMMYEditable = IsMMYEditable == null ? '' : IsMMYEditable;
    var mmyid = mmyid == null ? '' : mmyid;
    odometer = odometer == null ? '' : odometer;
    vehiclelable = vehiclelable == null ? '' : vehiclelable;
    var edit_btn = '';
    if (null == vehicleID)
    {
        vehicleID = -1;
        edit_btn = '<td><div style="color:gray;cursor: default;font-size: 10pt;text-align: center;font-family: Microsoft YaHei;" trNum="' + loadingFlag + '">' + LanguageScript.common_edit + '</div></td>'
    } else {
        //注：value的值为fieldset的id
        if (1 != GetRoleID()) {
            edit_btn = '<td><div style="color:gray;cursor: default;font-size: 10pt;text-align: center;font-family: Microsoft YaHei;" trNum="' + loadingFlag + '">' + LanguageScript.common_edit + '</div></td>'
        }
        else {
            edit_btn = '<td><div id="vehicle_edit_' + vehicleID + '" value="vehicle_' + vehicleID + '" class="setting_button_edit" clickFlag="true" trNum="' + loadingFlag + '">' + LanguageScript.common_edit + '</div></td>'
        }
    }
    return '<tr><td>' +
            '<fieldset id="vehicle_' + vehicleID + '" data-IsVinEditable="' + isVinEditable + '" data-IsMMYEditable="' + IsMMYEditable + '" data-mmyid="' + mmyid + '">' +
                '<div id="vehicleInfo-left" style="float:left;position:relative;width:55%">' +
                    '<table style="width:100%">' +
                        '<tr>' +
                            '<td style="width: 57px;">' + LanguageScript.common_vehicleName + ':</td>' +
                            '<td class="setting-vehicle-normal"><div title="' + vehicleName + '">' + vehicleName + '</div> </td>' +
                            '<td class="setting-vehicle-input"><input maxlength="10" value="' + vehicleName + '" /></td>' +
                            '<td style="width: 57px;">' + LanguageScript.common_pic + ':</td>' +
                            '<td style="width: 40%;"><div id="preview_fake_' + vehicleID + '" class="preview_fake"><img id="OBU_logo_' + vehicleID + '" alt="' + LanguageScript.common_NoPic + '" style="height:33px;width:40px;" src="/' + CompanyID + '/Logo/DrawImage?vehicleID=' + vehicleID + '&type=vehicleLogo&temp=' + Date.parse(new Date()) + '" /></div></td>' +
                            //'<td class="setting-vehicle-button"><div class="setting-vehicle-button-img" style="height:19px;">' + LanguageScript.common_edit + '<input type="file" name="OBU_vehicleLogo" id="vehicleLogo_' + vehicleID + '"  style="position:relative;width:45px;height:19px;top:-19px; filter:alpha(opacity:0);opacity: 0;cursor:pointer;" onblur="checkVehicleLogo(' + vehicleID + ');" onchange="previewImage(this,' + vehicleID + ')" /></div></td>' +
                            '<td class="setting-vehicle-button"><div id="vehicleLogo_' + vehicleID + '" name="OBU_vehicleLogo" class="setting-vehicle-button-img" style="height:19px;">' + LanguageScript.common_edit + '</div></td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td style="">' + LanguageScript.common_VehicleLicence + ':</td>' +
                            '<td class="setting-vehicle-normal"><div title="' + vehicleLicence + '">' + vehicleLicence + '</div> </td>' +
                            '<td class="setting-vehicle-input"><input maxlength="9" value="' + vehicleLicence + '" /></td>' +
                            '<td>' + LanguageScript.page_setting_vehicleLicense + ':</td>' +
                            '<td><div title="' + vehicleInformation + '">' + vehicleInformation + '</div> </td>' +
                            '<td class="setting-vehicle-button" ><div id="vehicle_set_' + vehicleID + '" class="setting-vehicle-button-mmy"  value="vehicle_' + vehicleID + '">' + LanguageScript.common_set + '</div></td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td >' + LanguageScript.common_total_mileage + ':</td>' +
                            '<td class="setting-vehicle-normal"><div style="float:left;width:auto;">' + odometer + ' </div>' + LanguageScript.unit_km + '</td>' +
                            '<td class="setting-vehicle-input"><input style="float:left" value="' + odometer + '" />' + LanguageScript.unit_km + '</td>' +
                        '</tr>' +
                    '</table>' +
                    '<table style="width:100%"><tr>' +
                        '<td style="width:104px;">' + LanguageScript.common_vin + ':</td>' +
                        '<td class="setting-vehicle-normal-vin"><div>' + vin + ' </div></td>' +
                        '<td class="setting-vehicle-input-vin"><input style="width:40%;" value="' + vin + '" /></td>' +
                    '</tr></table>' +
                '</div>' +
                '<div id="vehicleInfo-middle" style="float:left;position:relative;width:40%;">' +
                    '<table style="width:100%">' +
                        '<tr>' +
                            '<td style="width:69px;">' + LanguageScript.page_setting_vehicleDriver + ':</td>' +
                            '<td class="setting-vehicle-normal"><div title="' + vehicleDriver + '">' + vehicleDriver + '</div></td>' +
                            '<td class="setting-vehicle-input"><input maxlength="40" value="' + vehicleDriver + '" /></td>' +
                            '<td style="width:57px;">' + LanguageScript.common_ESN + ':</td>' +
                            '<td style="width:36%"><div>' + ESN + '</div></td>' +
                            '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td>' + LanguageScript.page_setting_vehicleDriverTel + ':</td>' +
                            '<td class="setting-vehicle-normal"><div title="' + telephone + '">' + telephone + ' </div></td>' +
                            '<td class="setting-vehicle-input"><input value="' + telephone + '" /></td>' +
                            '<td>' + LanguageScript.page_settings_accountSettings_vehicleDiagnosticManagement_registrationKeyLabel + ':</td>' +
                            '<td><div>' + registionKey + '</div></td>' +
                        '</tr>' +
                    '</table>' +
                    '<table style="width:100%">' +
                        '<tr>' +
                            '<td style="width:33px;">' + LanguageScript.page_setting_vehicleLable + ':</td>' +
                            '<td class="setting-vehicle-normal"><textarea style="width:98%;height:50px;resize:none;" disabled="disabled">' + vehiclelable + '</textarea></td>' +
                            '<td class="setting-vehicle-input"><textarea style="width:98%;height:50px;resize:none;">' + vehiclelable + '</textarea></td>' +
                        '</tr>' +
                    '</table>' +
                '</div>' +
                '<div id="vehicleInfo-right" style="float:left;position:relative;width:5%;">' +
                    '<table>' +
                        '<tr>' +
                            edit_btn +
                        '</tr>' +
                        '<tr>' +
                            '<td style="padding:36px 0 15px 0"><div id="vehicle_save_' + vehicleID + '" value="vehicle_' + vehicleID + '" class="setting_button_save" style="display:none;" clickFlag="true" trNum="' + loadingFlag + '">' + LanguageScript.common_save + '</div></td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td><div id="vehicle_cancle_' + vehicleID + '" value="vehicle_' + vehicleID + '" class="setting_button_cancle" style="display:none;" clickFlag="true">' + LanguageScript.common_cancel + '</div></td>' +
                        '</tr>' +
                    '</table>' +
                '</div>' +
                //loadingFlag * 121     .loading_background的高度。
                '<div id="vehicle_loading_' + vehicleID + '" class="loading_background" style="top:' + (loadingFlag % onePageNum) * 121 + 'px"><img src="../../../Content/Common/images/loading_style.gif"  style="position:relative;width:50px;top:35px;"/> </div>' +
            '</fieldset>' +
        '</td></tr>';
}
function VehicleOneTrInfo_edit(id, vehiclename, vehiclevin, vehicleinfo, vehicledriver, vehiclelicence, telephone, vehicleEsn, vehicleKey, isVinEditable, IsMMYEditable, mmyid, odometer) {
    $("#" + id + " input")[0].value = $.trim(vehiclename);
    $("#" + id + " input")[1].value = $.trim(vehiclelicence);
    $("#" + id + " input")[2].value = $.trim(odometer);
    $("#" + id + " input")[3].value = $.trim(vehiclevin);
    $("#" + id + " input")[4].value = $.trim(vehicledriver);
    $("#" + id + " input")[5].value = $.trim(telephone);//tel
    $("#" + id + " textarea")[1].value = $("#" + id + " textarea")[0].value;//标签
}

//初始化点击事件
InitVehicleClickEvent = function () {
    $(".setting_button_edit").click(function (e) {
        var id = $("#" + e.currentTarget.id).attr('value');
        if ($("#" + id + " .setting_button_edit").attr("clickFlag") == "false") {
            return;
        }
        $(".setting_button_edit").attr("clickFlag", "false");
        $(".setting_button_edit").css("color", "gray");
        $(".setting_button_edit").css("cursor", "default");
        Vehicle_OBU_edit_display(id,$("#" + e.currentTarget.id).attr('trNum'));
    })
    $(".setting_button_save").click(function (e) {
        var id = $("#" + e.currentTarget.id).attr('value');
        if ($("#" + id + " .setting_button_save").attr("clickFlag") == "false")
        {
            return;
        }
        Edit_OBU_save(id, $("#" + e.currentTarget.id).attr('trNum'));
    })
    $(".setting_button_cancle").click(function (e) {
        var id = $("#" + e.currentTarget.id).attr('value');
        if ($("#" + id + " .setting_button_cancle").attr("clickFlag") == "false") {
            return;
        }
        Vehicle_OBU_OneTr_display(id,true)
        HrefFlag--;
        vehicleId = "-1";
        PreviousVIN[parseInt(id.substring(8))] = "";
        $(".setting_button_edit").attr("clickFlag", "true");
        $(".setting_button_edit").css("color", "blue");
        $(".setting_button_edit").css("cursor", "pointer");
    })
    $(".setting-vehicle-button-mmy").click(function (e) {
        var id = $("#" + e.currentTarget.id).attr('value');
        initEditMMYDialog(id, mmyCallBack);
        $("#" + id + " .setting_button_save").css("color", "gray");
        $("#" + id + " .setting_button_save").attr("clickFlag", "false");
        $("#" + id + " .setting_button_cancle").css("color", "gray");
        $("#" + id + " .setting_button_cancle").attr("clickFlag", "false");
    })
}
//保存成功后将input内的值显示
function Vehicle_OBU_OneTr_save(id) {
    $("#" + id).find("div")[1].innerHTML = $("#" + id + " input")[0].value;//vehiclename
    $("#" + id).find("div")[4].innerHTML = $("#" + id + " input")[1].value;//license
    $("#" + id).find("div")[7].innerHTML = $("#" + id + " input")[2].value;//odometer
    $("#" + id).find("div")[8].innerHTML = $("#" + id + " input")[3].value;//vin
    $("#" + id).find("div")[10].innerHTML = $("#" + id + " input")[4].value;//driver
    $("#" + id).find("div")[12].innerHTML = $("#" + id + " input")[5].value;//tel
    $("#" + id + " textarea")[0].value = $("#" + id + " textarea")[1].value;//标签
}
//编辑一辆车的信息
function Vehicle_OBU_OneTr_display(id,cancleflag) {
    $("#" + id).css("border-color", "#ddd");
    $("#" + id + " .setting-vehicle-normal").show();
    $("#" + id + " .setting-vehicle-input").hide();
    $("#" + id + " .setting-vehicle-normal-vin").show();
    $("#" + id + " .setting-vehicle-input-vin").hide();
    $("#" + id + " .setting-vehicle-button-img").hide();
    $("#" + id + " .setting-vehicle-button-mmy").hide();
    $("#" + id + " .setting_button_edit").show();
    $("#" + id + " .setting_button_save").hide();
    $("#" + id + " .setting_button_cancle").hide();
    $("#" + id + " textarea").attr("disabled", true);
    if (true == cancleflag) {
        $("#" + id).find("div")[5].innerHTML = $("#OBU_input_back").val();
        $("#" + id).find("div")[5].title = $("#OBU_input_back").val();
        $("#" + id).attr("data-mmyid", $("#OBU_input_mmyid_back").val());
    }
    $("#OBU_logo_" + id.substring(8)).attr('src', '/' + GetCompanyID() + '/Logo/DrawImage?vehicleID=' + id.substring(8) + '&type=vehicleLogo&temp=' + Date.parse(new Date()) + '');
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        document.getElementById('OBU_logo_' + id.substring(8)).style.display = 'block';
        //document.getElementById('preview_fake_' + id.substring(8)).filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = '';
        $('#preview_fake_' + id.substring(8)).removeClass();
    }
}

//MMY编辑之后回调函数
function mmyCallBack(id) {
    var tempInfo = "";
    if ($.trim($("#OBU_input_new").val()) != null && $.trim($("#OBU_input_new").val()) != "" && $.trim($("#OBU_input_new").val()).substring(1,2) == " ") {
        tempInfo = $.trim($("#OBU_input_new").val()).substring(1, $.trim($("#OBU_input_new").val()).length);
        tempInfo = $.trim(tempInfo);
    }
    $("#" + id).find("div")[5].innerHTML = tempInfo;//mmy
    $("#" + id).find("div")[5].title = tempInfo;
    $("#" + id).attr("data-mmyid", $("#OBU_input_mmyid_new").val());
}
// 车辆编辑按钮按下设置
function Vehicle_OBU_edit_display(id,trNum) {
    HrefFlag++;
    logosubmitflag[parseInt(id.substring(8))] = 0;
    odometerSubmitflag[parseInt(id.substring(8))] = 0;
    var vehiclename = $.trim($("#" + id).find("div")[1].innerHTML);
    var vehiclevin = $.trim($("#" + id).find("div")[8].innerHTML);
    PreviousVIN[parseInt(id.substring(8))] = vehiclevin;
    var vehicleinfo = $.trim($("#" + id).find("div")[5].innerHTML);
    var vehicledriver = $.trim($("#" + id).find("div")[10].innerHTML);
    var vehiclelicence = $.trim($("#" + id).find("div")[4].innerHTML);
    var vehicleEsn = $.trim($("#" + id).find("div")[11].innerHTML);
    var vehicleKey = $.trim($("#" + id).find("div")[13].innerHTML);
    var telephone = $.trim($("#" + id).find("div")[12].innerHTML);
    var odometer = $.trim($("#" + id).find("div")[7].innerHTML);
    var vehiclelabel = $.trim($("#" + id + " textarea")[0].value)
    var isVinEditable = $.trim($("#" + id).attr("data-isvineditable"));
    var IsMMYEditable = $.trim($("#" + id).attr("data-ismmyeditable"));
    var mmyid = $.trim($("#" + id).attr("data-mmyid"));

    $("#OBU_input_back").val(vehicleinfo);
    $("#OBU_input_mmyid_new").val("");
    $("#OBU_input_new").val("");
    $("#OBU_input_mmyid_back").val(mmyid);

    VehicleOneTrInfo_edit(id, vehiclename, vehiclevin, vehicleinfo, vehicledriver, vehiclelicence, telephone, vehicleEsn, vehicleKey, isVinEditable, IsMMYEditable, mmyid, odometer);

    $("#" + id).css("border-color", "white");
    $("#" + id + " .setting-vehicle-normal").hide();
    $("#" + id + " .setting-vehicle-input").show();
    $("#" + id + " .setting-vehicle-button-img").show();
    $("#" + id + " .setting_button_edit").hide();
    $("#" + id + " .setting_button_save").show();
    $("#" + id + " .setting_button_cancle").show();
    $("#" + id + " textarea").attr("disabled", false);

    var isVinEditable = $("#" + id).attr("data-isvineditable");
    var IsMMYEditable = $("#" + id).attr("data-ismmyeditable");
    if (isVinEditable == "1") {
        $("#" + id + " .setting-vehicle-normal-vin").hide();
        $("#" + id + " .setting-vehicle-input-vin").show();
    }
    if (IsMMYEditable == "1") {
        $("#" + id + " .setting-vehicle-button-mmy").show();
    }

    commit[parseInt(id.substring(8))] = new AjaxUpload("#vehicleLogo_" + id.substring(8), {
        action: "/" + GetCompanyID() + "/Setting/EditOBU_Vehicles",
        responseType: "text",
        autoSubmit: false,
        data: {

        },
        onChange: function (file, ext) {
            previewImage(file, id.substring(8));
        },
        onSubmit: function (file, ext) {
            //alert(file);
        },
        onComplete: function (file, response) {
            if (response.toLocaleLowerCase() == "e400001") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400001);
            } else if (response.toLocaleLowerCase() == "e400015") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400015);
            } else if (response.toLocaleLowerCase() == "e400037") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400037);
            } else if (response.toLocaleLowerCase() == "e400038") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400038);
            } else if (response.toLocaleLowerCase() == "e400039") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400039);
            } else if (response.toLocaleLowerCase() == "e400041") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400041);
            } else if (response.toLocaleLowerCase() == "e500014") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_500014);
                //odometer
            } else if (response.toLocaleLowerCase() == "400") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_400);
            } else if (response.toLocaleLowerCase() == "500") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_500);
            } else if (response.toLocaleLowerCase() == "error") {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
            } else if (response.toLocaleLowerCase() == "ok") {
                currentid = id.substring(8);
                var vehiclename_input = $("#" + id + " input")[0].value;
                var vehiclelicence_input = $("#" + id + " input")[1].value;
                var vehicleOdometer_input = $("#" + id + " input")[2].value;//odometer
                if (isVinEditable != "1") {
                    vehicleVin_input = PreviousVIN[parseInt(id.substring(8))];
                } else {
                    var vehicleVin_input = $("#" + id + " input")[3].value;
                }
                var vehicledriver_input = $("#" + id + " input")[4].value;
                var telephone_input = $("#" + id + " input")[5].value;//tel
                var isVinEditable = $("#" + id).attr("data-isvineditable");
                var isMMYEditable = $("#" + id).attr("data-ismmyeditable");
                var vehiclemmyid = $("#" + id).attr("data-mmyid");
                var vehiclelable = $("#" + id + " textarea")[1].value;//标签

                var vehicleEsn = $.trim($("#" + id).find("div")[11].innerHTML);
                var vehicleKey = $.trim($("#" + id).find("div")[13].innerHTML);
                var vehicleInfo = "";
                if ("" != $("#OBU_input_new").val()) {
                    vehicleInfo = $("#OBU_input_new").val();
                } else {
                    vehicleInfo = $("#OBU_input_back").val();
                }
                var loadingFlag = parseInt(trNum);
                arrAllSetting[parseInt(trNum)] = VehicleOneTrInfo(currentid, vehiclename_input, vehicleVin_input, vehicleInfo, vehicledriver_input, vehiclelicence_input, telephone_input, vehicleEsn, vehicleKey, isVinEditable, isMMYEditable, vehiclemmyid, vehicleOdometer_input, vehiclelable, loadingFlag);
                HrefFlag--;
                PreviousVIN[parseInt(id.substring(8))] = "";
                Vehicle_OBU_OneTr_save(id);
                Vehicle_OBU_OneTr_display(id);
                //Modify by LiYing for bug 2154 start
                $(".setting_button_edit").attr("clickFlag", "true");
                $(".setting_button_edit").css("color", "blue");
                $(".setting_button_edit").css("cursor", "pointer");
                //Modify by LiYing for bug 2154 End
               
            } else {
                user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
            }
            $("#vehicle_loading_" + id.substring(8)).css("z-index", "-1");
            
        }
    });
}
//保存
function Edit_OBU_save(id,trNum) {
    var vehiclename = $("#" + id).find("div")[1].innerHTML;
    currentid = id.substring(8);
    var vehiclename_input = $("#" + id + " input")[0].value;
    var vehiclelicence_input = $("#" + id + " input")[1].value;
    var vehicleOdometer_input = $("#" + id + " input")[2].value;//odometer
    var vehicleVin_input = $("#" + id + " input")[3].value;
    var vehicledriver_input = $("#" + id + " input")[4].value;
    var telephone_input = $("#" + id + " input")[5].value;//tel
    var isVinEditable = $("#" + id).attr("data-isvineditable");
    var isMMYEditable = $("#" + id).attr("data-ismmyeditable");
    var vehiclemmyid = $("#" + id).attr("data-mmyid");
    var vehiclelable = $("#" + id + " textarea")[1].value;//标签

    var vehicleEsn = $.trim($("#" + id).find("div")[11].innerHTML);
    var vehicleKey = $.trim($("#" + id).find("div")[13].innerHTML);
    var vehicleInfo = "";
    if ("" != $("#OBU_input_new").val()) {
        vehicleInfo = $("#OBU_input_new").val();
    } else {
        vehicleInfo = $("#OBU_input_back").val();
    }
    var loadingFlag = parseInt(trNum);

    //是否设置总里程
    if ($.trim($("#" + id + " input")[2].value) != $.trim($("#" + id).find("div")[7].innerHTML)) {
        odometerSubmitflag[parseInt(id.substring(8))] = 1;
    }
    else {
        odometerSubmitflag[parseInt(id.substring(8))] = 0;
    }

    //#861 增加驾驶司机正则liangjiajie0319
    var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,10}$/;     //车辆名称
    var reg_model = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,40}$/;     //车型正则
    var reg_driver = /^[a-zA-Z\s\u4e00-\u9fa5]{1,40}$/;     //驾驶司机正则
    var reg_licence = /^[A-Za-z0-9\u4E00-\u9FA5]{1,9}$/;     //车辆牌照正则
    var reg_vin = /^[a-zA-Z0-9]{17}$/;     //VIN正则   
    var reg_odometer = /^[1-9]\d*$/;//总里程正则
    var check_OBU_name_input = false;
    var check_OBU_info_input = false;
    var check_OBU_driver_input = false;//#861 驾驶司机判断结果标志liangjiajie0319
    var check_OBU_licence_input = false;
    var check_OBU_vin_input = false;
    var check_OBU_tel_input = false;//增加司机电话
    var check_OBU_odometer_input = false;//增加总里程
    var check_OBU_lable_input = false;//增加标签
    var title = LanguageScript.page_setting_VehiclesAndOBU;

    //tel check
    if (isPhone(telephone_input) == -1) {
        var text = LanguageScript.common_CorrectFormatTel;              //修改司机电话提示信息//Redmine#423liangjiajie0306
        OBU_dialog_error(text);
        check_OBU_tel_input = false;
    }
    else {
        check_OBU_tel_input = true;
    }

    //odometer check
    if (!reg_odometer.test(vehicleOdometer_input)) {
        //值为空，且用户为做修改。
        if ($("#" + id + " input")[2].value == $.trim($("#" + id).find("div")[7].innerHTML) && $("#" + id + " input")[2].value == "") {
            check_OBU_odometer_input = true;
        }
        else {
            var text = LanguageScript.error_e01283;              //修改总里程提示信息//Redmine#423liangjiajie0306
            OBU_dialog_error(text);
            check_OBU_odometer_input = false;
        }
    }
    else {
        check_OBU_odometer_input = true;
    }

    //lable check
    if (checkVehicleLable(vehiclelable)) {
        check_OBU_lable_input = true;
    }
    else {
        var text = LanguageScript.page_setting_vehicleLableError;
        OBU_dialog_error(text);
        check_OBU_lable_input = false;
    }

    //name check
    if ("" == vehiclename_input) {                           //判断车辆名称输入为空时处理
        var text = LanguageScript.page_setting_EnterVehicleName;              //修改车辆名称提示信息//Redmine#423liangjiajie0306
        OBU_dialog_error(text);
        check_OBU_name_input = false;
    } else if (!reg.test(vehiclename_input)) {
        var text = LanguageScript.error_e01262;
        OBU_dialog_error(text);
        check_OBU_name_input = false;
    } else if (vehiclename_input.length > 10) {                    //判断车辆名称过长（大于10）
        check_OBU_name_input = false;
    } else if (($.trim(vehiclename_input) != $.trim(vehiclename)) && (!IsVehicleNameExist(vehiclename_input))) { //#733 增加检测车辆名称同名检测功能liangjiajie0319
        var text = LanguageScript.page_setting_VehicleNameExist;
        OBU_dialog_error(text);
        check_OBU_name_input = false;
    } else {
        check_OBU_name_input = true;
    }

    // vin Check
    if (true == check_OBU_name_input && (isVinEditable == "1")) {

        if ("" == vehicleVin_input) {                           //判断车辆VIN输入为空时处理
            var text = LanguageScript.error_e01286;
            OBU_dialog_error(text);
            check_OBU_vin_input = false;
        } else if ("Unknown VIN" == vehicleVin_input && "Unknown VIN" == PreviousVIN[parseInt(id.substring(8))]) {
            check_OBU_vin_input = true;
        } else if (!reg_vin.test(vehicleVin_input)) {
            var text = LanguageScript.error_e01286;
            OBU_dialog_error(text);
            check_OBU_vin_input = false;
        } else if (vehicleVin_input.length > 17) {                    //判断车辆名称过长（大于17）
            check_OBU_vin_input = false;
        } else {
            check_OBU_vin_input = true;
        }
    } else if (isVinEditable != "1") {
        vehicleVin_input = PreviousVIN[parseInt(id.substring(8))];
        check_OBU_vin_input = true;
    }

    // 车型check [MMY是选择出来的，所有不需要check]
    if (true == check_OBU_vin_input && (isMMYEditable == "1")) {         //车辆名称输入正确再检查车型
        if (!vehiclemmyid) {                       //判断车型输入为空时处理
            var text = LanguageScript.page_setting_EnterModels;          //修改车型提示信息//Redmine#423liangjiajie0306
            OBU_dialog_error(text);
            check_OBU_info_input = false;
        } else {
            check_OBU_info_input = true;
        }
    } else if (isMMYEditable != "1") {

        check_OBU_info_input = true;
    }

    //#861 增加驾驶司机判断liangjiajie0319
    if (true == check_OBU_info_input) {           //车型正确后再检查驾驶司机
        if ("" == vehicledriver_input) {                        //判断驾驶司机输入为空时处理
            check_OBU_driver_input = true;
        } else if (!reg_driver.test(vehicledriver_input)) {
            var text = LanguageScript.error_e01284;//liangjiajie0321
            OBU_dialog_error(text);
            check_OBU_driver_input = false;
        } else if (vehicledriver_input.length > 40) {
            check_OBU_driver_input = false;
        } else {//Redmine#423liangjiajie0306
            check_OBU_driver_input = true;
        }
    }

    if (true == check_OBU_driver_input) {           //驾驶司机信息正确后再检查车辆牌照
        if ("" == vehiclelicence_input) {                        //判断车辆牌照输入为空时处理
            var text = LanguageScript.page_setting_EnterVehicleLicence;          //修改车辆牌照提示信息//Redmine#423liangjiajie0306
            OBU_dialog_error(text);
            check_OBU_licence_input = false;
        } else if (!reg_licence.test(vehiclelicence_input)) {//Redmine#423liangjiajie0306
            var text = LanguageScript.error_e01276;
            OBU_dialog_error(text);
            check_OBU_licence_input = false;
        } else if (vehiclelicence_input.length > 9) {                //判断车辆牌照过长（大于20字节数）
            check_OBU_licence_input = false;
        } else {
            check_OBU_licence_input = true;
        }
    }

    if (true == check_OBU_name_input &&
        true == check_OBU_info_input &&
        true == check_OBU_licence_input &&
        true == check_OBU_driver_input &&
        true == check_OBU_vin_input &&
        true == check_OBU_tel_input &&
        true == check_OBU_odometer_input &&
        true == check_OBU_lable_input) {

        $("#vehicle_loading_" + currentid).css("z-index", "1");

        //document.forms["editOBU"].submit();
        if (logosubmitflag[parseInt(id.substring(8))] == 1) {
            vehiclelable = unicode(vehiclelable);
            commit[parseInt(id.substring(8))].setData({
                "OBU_input_name": vehiclename_input,
                "OBU_input_mmyid": vehiclemmyid,
                "OBU_input_driver": vehicledriver_input,
                "OBU_input_licence": vehiclelicence_input,
                "OBU_input_vin": vehicleVin_input,
                "OBU_vehicleID": currentid,
                "OBU_input_tel": telephone_input,
                "OBU_input_odometer": vehicleOdometer_input,
                "OBU_input_vehiclelable": vehiclelable,
                "flag": logosubmitflag[parseInt(id.substring(8))],
                "odometerFlag": odometerSubmitflag[parseInt(id.substring(8))],
                "IsMMYEditable": isMMYEditable,
            });
            commit[parseInt(id.substring(8))].submit();
        }
        if (logosubmitflag[parseInt(id.substring(8))] == 0) {
            $.ajax({
                type: "POST",
                url: "/" + GetCompanyID() + "/Setting/EditOBU_Vehicles",
                data: {
                    OBU_input_name: vehiclename_input,
                    OBU_input_mmyid: vehiclemmyid,
                    OBU_input_driver: vehicledriver_input,
                    OBU_input_licence: vehiclelicence_input,
                    OBU_input_vin: vehicleVin_input,
                    OBU_input_tel: telephone_input,
                    OBU_vehicleID: currentid,
                    OBU_input_odometer: vehicleOdometer_input,
                    OBU_input_vehiclelable: unicode(vehiclelable),
                    flag: logosubmitflag[parseInt(id.substring(8))],
                    odometerFlag: odometerSubmitflag[parseInt(id.substring(8))],
                    IsMMYEditable: isMMYEditable,
                },
                dataType: "text",
                success: function (returnData) {

                    $("#OBU_dialog_Odometer_div").val("");
                    $("#ui-right_Odometer").hide();
                    $(".validateTips_Odometer").css("margin", "-13px 0px 8px 0px");

                    if (returnData.toLocaleLowerCase() == "e400001") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400001);
                    } else if (returnData.toLocaleLowerCase() == "e400015") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400015);
                    } else if (returnData.toLocaleLowerCase() == "e400037") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400037);
                    } else if (returnData.toLocaleLowerCase() == "e400038") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400038);
                    } else if (returnData.toLocaleLowerCase() == "e400039") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400039);
                    } else if (returnData.toLocaleLowerCase() == "e400041") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400041);
                    } else if (returnData.toLocaleLowerCase() == "e500014") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_500014);
                    //odometer
                    } else if (returnData.toLocaleLowerCase() == "400") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_400);
                    } else if (returnData.toLocaleLowerCase() == "500") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_500);
                    } else if (returnData.toLocaleLowerCase() == "error") {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
                    } else if (returnData.toLocaleLowerCase() == "ok") {
                        HrefFlag--;
                        PreviousVIN[parseInt(id.substring(8))] = "";
                        Height_User(8, 50);
                        Vehicle_OBU_OneTr_save(id);
                        Vehicle_OBU_OneTr_display(id);
                        arrAllSetting[parseInt(trNum)] = VehicleOneTrInfo(currentid, vehiclename_input, vehicleVin_input, vehicleInfo, vehicledriver_input, vehiclelicence_input, telephone_input, vehicleEsn, vehicleKey, isVinEditable, isMMYEditable, vehiclemmyid, vehicleOdometer_input, vehiclelable, loadingFlag);

                        //Modify by LiYing for bug 2154 start
                        $(".setting_button_edit").attr("clickFlag", "true");
                        $(".setting_button_edit").css("color", "blue");
                        $(".setting_button_edit").css("cursor", "pointer");
                        //Modify by LiYing for bug 2154 End
                    } else {
                        user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
                    }
                    $("#vehicle_loading_" + id.substring(8)).css("z-index", "-1");
                },
                error: function () {
                    //alert("11");
                    //Vehicle_OBU_OneTr_display(id);
                    $("#vehicle_loading_" + currentid).css("z-index", "-1");
                    user_dialog_error(LanguageScript.page_update_Vehicle_error_500014);
                    $(".setting_button_edit").attr("clickFlag", "true");
                    $(".setting_button_edit").css("color", "blue");
                    $(".setting_button_edit").css("cursor", "pointer");
                }
            });
        }
    }
    else {//liangjiajie0321
        return;
    }
}

function checkVehicleLogo(id) {
    var path = $("#vehicleLogo_"+id).val();
    if ("" == path) {//上传路径为空liangjiajie
        //fengpan
        return false;
    } else {
        var last = path.substring(path.lastIndexOf("."), path.length);
        last = last.toLocaleLowerCase();
        if (".jpg" == last || ".gif" == last || ".bmp" == last || ".png" == last || ".PNG" == last) {
            //$("#show_LogoInfo").show();
            //$("#logoright").show();
            //$("#logoerror").hide();
            return true;
        } else {//图片格式判断
            var title = LanguageScript.page_setting_VehiclesAndOBU;
            var text = LanguageScript.error_e01265;
            user_dialog_error(text);
            $("#vehicleLogo_" + id).val("");
            return false;
        }
    }
}



/****************************************/
/**************参数说明******************/
//file ：上传控件id， 参考本文件的vehicleLogo//
/*************#650liangjiajie0308***************/
function previewImage(file, id) {
    logosubmitflag[parseInt(id)] = 1;
    var MAXWIDTH = 55;
    var MAXHEIGHT = 56;
    //var div = document.getElementById('preview');
    if (file.files && file.files[0]) {
        //div.innerHTML = '<img id=OBU_logo>';
        var img = document.getElementById('OBU_logo_' + id);
        img.onload = function () {
            var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
            img.width = rect.width;
            img.height = rect.height;
            //fengpan 20140625
            //img.style.marginLeft = rect.left + 'px';
            //img.style.marginTop = rect.top + 'px';
        }
        var reader = new FileReader();
        reader.onload = function (evt) { img.src = evt.target.result; }
        reader.readAsDataURL(file.files[0]);
        img.style.display = 'block';
    }
    else if (navigator.userAgent.indexOf("MSIE") > 0) {
        //alert("else");
        file.select();
        file.blur();
        //fengpan 20140625
        //div.focus();
        //file.blur();
        var src = document.selection.createRange().text;//取得本地图片地址
        var div = document.getElementById('preview_fake_' + id);
        div.outerHTML = '<div id="preview_fake_' + id + '" class="preview_fake"><img id="OBU_logo_' + id + '" alt="' + LanguageScript.common_NoPic + '" style="height:33px;width:40px;" src="/' + GetCompanyID() + '/Logo/DrawImage?vehicleID=' + id + '&type=vehicleLogo&temp=' + Date.parse(new Date()) + '" /></div>';
        var img = document.getElementById('OBU_logo_' + id);
        img.style.display = 'none';
        var objPreviewFake = document.getElementById("preview_fake_" + id);
        objPreviewFake.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = src;
        objPreviewFake.style.width = '40px';
        objPreviewFake.style.height = '33px';
        objPreviewFake.style.marginTop = '0px';
        objPreviewFake.style.marginLeft = '0px';

        //img.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = src;
        //var sFilter = 'filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale,src="';
        //div.innerHTML = '<img id=OBU_logo>';
        //div.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = src;
        //var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
        //status = ('rect:' + rect.top + ',' + rect.left + ',' + rect.width + ',' + rect.height);
        //div.innerHTML = "<div id=divhead style='width:" + rect.width + "px;height:" + rect.height + "px;margin-top:" + rect.top + "px;margin-left:" + rect.left + "px;" + sFilter + src + "\"'></div>";
    } else {
        return;
    }
}
//缩放比例函数liangjiajie
function clacImgZoomParam(maxWidth, maxHeight, width, height) {
    var param = { top: 0, left: 0, width: width, height: height };
    if (width > maxWidth || height > maxHeight) {
        rateWidth = width / maxWidth;
        rateHeight = height / maxHeight;

        if (rateWidth > rateHeight) {
            param.width = maxWidth;
            param.height = Math.round(height / rateWidth);
        } else {
            param.width = Math.round(width / rateHeight);
            param.height = maxHeight;
        }
    }

    param.left = Math.round((maxWidth - param.width) / 2);
    param.top = Math.round((maxHeight - param.height) / 2);
    return param;
}
/***************************************/

InitVehicleAllSetting = function (pagenumber) {
    unbind_OBU_editBTN();
    $("#Setting_OBU_table tr").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrAllSetting.length; i++) {
        $("#Setting_OBU_table").append(arrAllSetting[i]);
    }
    var roleID = GetRoleID();
    if (1 == roleID) {
        bind_OBU_editANDadd();
    } else {
        unbind_OBU_editBTN();
    }
    //mergeGroup(1);
    // SetPageHeight("tab_vehicleAll");
    $("#all_pageBar_Setting").pager({ pagenumber: pagenumber, pagecount: arrAllSetting.length % onePageNum == 0 ? arrAllSetting.length / onePageNum : arrAllSetting.length / onePageNum + 1, buttonClickCallback: VehicleAllSettingPageClick });

    //fengpan 20140625 #1761
    InitVehicleClickEvent();

}
VehicleAllSettingPageClick = function (pageclickednumber) {
    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        Set_PageCheck(pageclickednumber);
        return;
    }
    InitVehicleAllSetting(pageclickednumber)
    currentPageAll = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}
/*add*/
function getObuData(vehicleId) {
    //var registionKey = "59e5-ad61";
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetOBU_Vehicles",
        data: {},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            var VehicleInfo = '';
            var j = -1;
            arrAllSetting = new Array();
            for (var i = 0; i < msg.length; ++i) {
                if (null != msg[i].vehicle && vehicleId != msg[i].vehicle.pkid) {
                    //TODO 以后这个地方传递的参数应该修改为传递对象
                    //fengpan 20140625 
                    //var tempData = vehicleTrInfo(msg[i].vehicle.pkid, msg[i].vehicle.name, msg[i].vehicle.vin, msg[i].vehicle.info, msg[i].vehicle.drivername, msg[i].vehicle.licence, msg[i].esn, /*msg[i].regkey*/msg[i].registrationKey, msg[i].vehicle.isVinEditable, msg[i].vehicle.isMMYEditable, msg[i].vehicle.mmyid);
                    var tempData = VehicleOneTrInfo(msg[i].vehicle.pkid, msg[i].vehicle.name, msg[i].vehicle.vin, msg[i].vehicle.info, msg[i].vehicle.drivername, msg[i].vehicle.licence, msg[i].vehicle.telephone, msg[i].esn, /*msg[i].regkey*/msg[i].registrationKey, msg[i].vehicle.isVinEditable, msg[i].vehicle.isMMYEditable, msg[i].vehicle.mmyid, msg[i].odometer, msg[i].vehicle.lable, i);
                    VehicleInfo += tempData;
                    arrAllSetting.push(tempData);

                } else if (null != msg[i].vehicle) {
                    j = i;
                    //var tempData = vehicleTrInfo_edit("tr_" + msg[i].vehicle.pkid, msg[i].vehicle.name, msg[i].vehicle.vin, msg[i].vehicle.info, msg[i].vehicle.drivername, msg[i].vehicle.licence, msg[i].esn, /*msg[i].regkey*/msg[i].registrationKey, msg[i].vehicle.isVinEditable, msg[i].vehicle.isMMYEditable, msg[i].vehicle.mmyid);
                    var tempData = VehicleOneTrInfo(msg[i].vehicle.pkid, msg[i].vehicle.name, msg[i].vehicle.vin, msg[i].vehicle.info, msg[i].vehicle.drivername, msg[i].vehicle.licence, msg[i].vehicle.telephone, msg[i].esn, /*msg[i].regkey*/msg[i].registrationKey, msg[i].vehicle.isVinEditable, msg[i].vehicle.isMMYEditable, msg[i].vehicle.mmyid, msg[i].odometer, msg[i].vehicle.lable, i);
                    VehicleInfo += tempData;
                    arrAllSetting.push(tempData);
                    editVehicleFlag = 1;//fengpan0301
                } else {
                    //chenyangwen 20140505 #1353
                    //var tempData = vehicleTrInfo_NoCar(msg[i].esn,msg[i].registrationKey);
                    var tempData = VehicleOneTrInfo(null, null, null, null, null, null, null, msg[i].esn, /*msg[i].regkey*/msg[i].registrationKey, null, null, null, null, null, i);
                    VehicleInfo += tempData;
                    arrAllSetting.push(tempData);
                }
            }
            $("#Setting_OBU_table tr").remove();
            if ("" != VehicleInfo) {
                if ("-1" != vehicleId) {
                    currentPageAll = parseInt(j / onePageNum + 1);
                    InitVehicleAllSetting( currentPageAll);
                    Vehicle_OBU_edit_display("vehicle_" + vehicleId, j);//id 未知
                    $(".setting_button_edit").attr("clickFlag", "false");
                    $(".setting_button_edit").css("color", "gray");
                    $(".setting_button_edit").css("cursor", "default");
                }
                else {
                    InitVehicleAllSetting(currentPageAll);
                }
            }
        },
        error: function () {
            var roleID = GetRoleID();
            if (1 == roleID) {
                bind_OBU_editANDadd();
            } else {
                unbind_OBU_editBTN();
            }
        }
    });
}

function checkVehicleLable(summaryValue) {
    //var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5\.\。\，\,\"\“\”\、\!\?\！\？\：\:]{0,1000}$/;
    //if (reg.test(summaryValue)) {
    //    return true;
    //} else {
    //    return false;
    //}
    var intro = summaryValue;
    var len = getByteLen(intro);
    if (len <= 2000) {//规则调整1000个中文也能保存liangjiajie0313
        intro = unicode(intro);

        return true;
    } else {

        return false;
    }
}