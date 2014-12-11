$(document).ready(function () {
    $("#ui-right_ESN").hide();
    $("#ui-right_KEY").hide();
    var pid = $(".vehicle_img_bg_normal").attr("id");
    //alert(pid);
    //20140307caoyandong#520
    if ("vehicle_all" == pid) {
        $("#v2_all").show();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();
        getData(1);
    } else if ("vehicle_drive" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").show();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();
        getData(2);
    } else if ("vehicle_stop" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").show();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();
        getData(3);
    } else if ("vehicle_health" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").show();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();
        getData(4);
    } else if ("vehicle_alert" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").show();
        $("#v2_missed").hide();
        $("#v2_history").hide();
        getData(5);
    } else if ("vehicle_missed" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").show();
        $("#v2_history").hide();

        $(".v1_container_1").hide();
        $("#v1_mis_title").show();
        $("#v1").hide();
        getData(6);
    } else if ("vehicle_history" == pid) {
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").show();
    } else {
    }

    var className = new Array();
    className[0] = $("#vehicle_all");
    className[1] = $("#vehicle_drive");
    className[2] = $("#vehicle_stop");
    className[3] = $("#vehicle_health");
    className[4] = $("#vehicle_alert");
    className[5] = $("#vehicle_missed");
    className[6] = $("#vehicle_history");

    $("#vehicle_all").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].removeClass("vehicle_img_bg_disable");

        className[0].addClass("vehicle_img_bg_normal");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_disable");
        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "default");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "pointer");

        //$("#v2_all").hide();
        getData(1);
        $("#v2_all").show();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();

    });
    $("#vehicle_drive").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");

        className[1].addClass("vehicle_img_bg_normal");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_disable");
        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "default");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "pointer");

        getData(2);
        $("#v2_all").hide();
        $("#v2_drive").show();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();

    });
    $("#vehicle_stop").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");

        className[2].addClass("vehicle_img_bg_normal");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_disable");
        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "default");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "pointer");

        getData(3);
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").show();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();

    });

    $("#vehicle_health").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");

        className[3].addClass("vehicle_img_bg_normal");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_disable");
        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "default");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "pointer");

        getData(4);
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").show();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").hide();

    });

    $("#vehicle_alert").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");

        className[4].addClass("vehicle_img_bg_normal");
        className[5].removeClass("vehicle_img_bg_disable");
        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "default");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "pointer");

        getData(5);
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").show();
        $("#v2_missed").hide();
        $("#v2_history").hide();

    });

    $("#vehicle_missed").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_disable");

        className[5].addClass("vehicle_img_bg_normal");
        className[6].removeClass("vehicle_img_bg_normal");
        className[6].addClass("vehicle_img_bg_disable");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "default");
        $("#vehicle_history").css("cursor", "pointer");

        getData(6);
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").show();
        $("#v2_history").hide();
    });

    $("#vehicle_history").click(function () {
        $(".paper-input-value").val("");
        className[0].removeClass("vehicle_img_bg_normal");
        className[0].addClass("vehicle_img_bg_disable");
        className[1].removeClass("vehicle_img_bg_disable");
        className[1].addClass("vehicle_img_bg_disable");
        className[2].removeClass("vehicle_img_bg_disable");
        className[2].addClass("vehicle_img_bg_disable");
        className[3].removeClass("vehicle_img_bg_disable");
        className[3].addClass("vehicle_img_bg_disable");
        className[4].removeClass("vehicle_img_bg_disable");
        className[4].addClass("vehicle_img_bg_disable");
        className[5].removeClass("vehicle_img_bg_normal");

        className[5].addClass("vehicle_img_bg_disable");
        className[6].removeClass("vehicle_img_bg_disable");
        className[6].addClass("vehicle_img_bg_normal");

        $("#vehicle_all").css("cursor", "pointer");
        $("#vehicle_drive").css("cursor", "pointer");
        $("#vehicle_stop").css("cursor", "pointer");
        $("#vehicle_health").css("cursor", "pointer");
        $("#vehicle_alert").css("cursor", "pointer");
        $("#vehicle_missed").css("cursor", "pointer");
        $("#vehicle_history").css("cursor", "default");

        getData(7);
        $("#v2_all").hide();
        $("#v2_drive").hide();
        $("#v2_stop").hide();
        $("#v2_health").hide();
        $("#v2_alert").hide();
        $("#v2_missed").hide();
        $("#v2_history").show();
    });
    //wenti
    $("#vehicle_add_vehicle").click(function () {
        $(function () {
        ++HrefFlag;
        $("#dialog_form_vihicle").dialog("close");
        var OBU_ESN = $("#OBU_dialog_ESN_div"),
          OBU_KEY = $("#OBU_dialog_KEY_div");
        //输入框 规则检测 更新 liangjiajie20140329
        reg = /^[a-zA-Z0-9\-]{1,17}$/;
        $("#OBU_dialog_KEY_div").blur(function () {
            if ($.trim(OBU_KEY.val()) == "") {
                $(".validateTips_KEY").text(LanguageScript.error_e01253);
                $(".validateTips_KEY").css("color", "red");
                $("#ui-right_KEY").hide();
                $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
            }
            else if (!reg.test($.trim(OBU_KEY.val()))) {
                $(".validateTips_KEY").text(LanguageScript.page_register_Register_Validate);
                $(".validateTips_KEY").css("color", "red");
                $("#ui-right_KEY").hide();
                $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
            } else {
                $("#ui-right_KEY").show();
                $(".validateTips_KEY").css("margin", "-26px 0px 8px 0px");
            }
        });
        $("#OBU_dialog_ESN_div").blur(function () {
            if ($.trim(OBU_ESN.val()) == "") {
                $(".validateTips_ESN").text(LanguageScript.error_e01252);
                $(".validateTips_ESN").css("color", "red");
                $("#ui-right_ESN").hide();
                $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
            } else if (!reg.test($.trim(OBU_ESN.val()))) {
                $(".validateTips_ESN").text(LanguageScript.page_register_ESN_Validate);
                $(".validateTips_ESN").css("color", "red");
                $("#ui-right_ESN").hide();
                $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
            }
            else {
                $("#ui-right_ESN").show();
                $(".validateTips_ESN").css("margin", "-26px 0px 8px 0px");
            }
        });
        $("#dialog_form_vihicle").dialog({
            draggable: true, 
            resizable: false,
            autoOpen: false,
            height: 230, /* liying for add obu failed*/
            width: 320,
            position: ['center', 250],
            modal: true,
            draggabled: false,
            buttons: {
                "保存": function () {
                    if ($.trim(OBU_ESN.val()) == "") {
                        $(".validateTips_ESN").text(LanguageScript.error_e01252);
                        $(".validateTips_ESN").css("color", "red");
                        $("#ui-right_ESN").hide();
                        $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                    }
                    if ($.trim(OBU_KEY.val()) == "") {
                        $(".validateTips_KEY").text(LanguageScript.error_e01253);
                        $(".validateTips_KEY").css("color", "red");
                        $("#ui-right_KEY").hide();
                        $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                    }
                        if ( -1 == reg.test($.trim(OBU_KEY.val()))) {
                            $(".validateTips_KEY").text(LanguageScript.page_register_Register_Validate);
                            $(".validateTips_KEY").css("color", "red");
                            $("#ui-right_KEY").hide();
                            $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                        }
                        if ( -1 == reg.test($.trim(OBU_ESN.val()))) {
                            $(".validateTips_ESN").text(LanguageScript.page_register_ESN_Validate);
                            $(".validateTips_ESN").css("color", "red");
                            $("#ui-right_ESN").hide();
                            $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                        }
                        if ($.trim(OBU_KEY.val()) != "" && $.trim(OBU_ESN.val()) != "" && reg.test($.trim(OBU_KEY.val())) == 1 && reg.test($.trim(OBU_ESN.val())) == 1) {
                            $.ajax({
                                type: "POST",
                                url: "/" + GetCompanyID() + "/Vehicles/AddVehicle",
                                data: { regkey: $.trim(OBU_KEY.val()), esn: $.trim(OBU_ESN.val()) },
                                contentType: "application/x-www-form-urlencoded",
                                dataType: "text",
                                success: function (msg) {
                                    if ("OK" == msg) {
                                        --HrefFlag;
                                        $("#dialog_form_vihicle").dialog("close");
                                        $("#OBU_dialog_ESN_div").val("");
                                        $("#OBU_dialog_KEY_div").val("");
                                        $("#ui-right_KEY").hide();
                                        $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                                        $("#ui-right_ESN").hide();
                                        $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                                    }
                                    else if ("NG" == msg) {
                                        //TODO 该设备不合法
                                        $("#add_vehicle_failed").text(LanguageScript.error_addOBU);
                                        $("#add_vehicle_failed").show();
                                    } else if ("error" == msg) {
                                        //添加OBU失败
                                        $("#add_vehicle_failed").text(LanguageScript.error_addobu_exception);
                                        $("#add_vehicle_failed").show();
                                    }
                                    else {
                                        //alert("该OBU已被注册，请使用未被注册的OBU");
                                        $("#add_vehicle_failed").text(LanguageScript.error_addOBU_registed);
                                        $("#add_vehicle_failed").show();
                                    }
                                },
                                error: function () {
                                    //...
                                }
                            });
                    }

                },
                取消: function () {
                    $("#OBU_dialog_ESN_div").val("");
                    $("#OBU_dialog_KEY_div").val("");
                    $(".validateTips_ESN").text(LanguageScript.error_e01278);
                    $(".validateTips_ESN").css("color", "gray");
                    $(".validateTips_KEY").text(LanguageScript.page_register_Registerkey_Intro);
                    $(".validateTips_KEY").css("color", "gray");
                    --HrefFlag;
                    $(this).dialog("close");
                    $("#ui-right_KEY").hide();
                    $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                    $("#ui-right_ESN").hide();
                    $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                }
            }
        });
        $("#OBU_dialog_ESN_div").focus(function () {
            $(".validateTips_ESN").text(LanguageScript.error_e01278);
            $(".validateTips_ESN").css("color", "gray");
            $("#ui-right_ESN").hide();
            $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
        });
        $("#OBU_dialog_KEY_div").focus(function () {
            $(".validateTips_KEY").text(LanguageScript.page_register_Registerkey_Intro);
            $(".validateTips_KEY").css("color", "gray");
            $("#ui-right_KEY").hide();
            $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
        });
	/* liying for add obu failed*/
        $("#add_vehicle_failed").hide();
        $("#dialog_form_vihicle").dialog("open");
        });
    });

    ChangeLeft("common_vehicle_cover");
    ChangeLocationTime();
    var roleID = GetRoleID();
    if ("2" == roleID) {
        SetBtnToneDown("vehicle_add_vehicle");
    }
});

//function getData(tabNum) {
//    getGroupIDs(tabNum);
//}
function vehicleInfo(VehicleId, vehicleImgUrl, vehicleName, vehicleDate, vehicleGroupName, vehicleHealth, vehiclePosition, vehicleLicence ,listType) {
    var CompanyID = GetCompanyID();
    var textSpan = '';
    var _positionCode = '';
    var _backgroundcolor = '';

    if (null == vehicleLicence)
    {
        vehicleLicence = LanguageScript.export_undefine;
    }
    if (LanguageScript.page_vehicles_Health == vehicleHealth) {
        //#825 表格宽度调整剂内部div宽度调整liangjiajie0319
        textSpan = '<td style="color:#339900"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;">' + vehicleHealth + '</div></td>';
    } else {
        //#825 表格宽度调整剂内部div宽度调整liangjiajie0319
        textSpan = '<td style="color:#ff0000"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;">' + vehicleHealth + '</div></td>';
    }
    if (vehicleImgUrl % 2 == 1) {
        _backgroundcolor = '<tr style="background-color:#f7f7f7;font-weight:lighter;">';
    }
    else {
        _backgroundcolor = '<tr style="background-color:#e6e6e6;font-weight:lighter;">';
    }

    if (vehiclePosition == null) {
        vehiclePosition = "";
    }

    //车辆未分组时显示未分组liangjiajie0321
    if ("-" == vehicleGroupName) {
        vehicleGroupName = "未分组";
    }
    return _backgroundcolor +
                        '<td style="padding-left:3%;" class="vehicles_groupName'+listType+'" id="vehicles_groupName' + VehicleId + listType +'"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleGroupName) + '">' + $.trim(vehicleGroupName) + '</div></td>' +
                        '<td ><img style="height:56px;width:63px" alt="' + LanguageScript.common_NoPic + '" src="/' + CompanyID + '/Logo/DrawImage?vehicleID=' + VehicleId + '&type=vehicleLogo" /></td>' +
                        '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleName) + '</div><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleDate) + '</div></td>' +
                        '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleLicence) + '">' + $.trim(vehicleLicence) + '</div></td>' +
                        textSpan +
                        '<td ><div class="locationName' + VehicleId + '" style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehiclePosition) + '">' + $.trim(vehiclePosition) + '</div></td>' +
                        '<td >' +
                            '<a href = "/' + CompanyID + '/Vehicles/Detail?VehicleId=' + VehicleId + '" onclick="trans()">' +
                            '<div  class="view_detail_button">' + LanguageScript.page_Dasboard_FleetLocation_ViewDetails + '</div>' +

                            '</a>' +
                        '</td>' +
                    '</tr>';
        //liangjiajie26
}

/*fengpan 20140408*/
function miss_vehicleInfo(VehicleId, vehicleImgUrl, vehicleName, vehicleDate, vehicleGroupName, vehicleHealth, vehiclePosition,vehicleMissedTime, vehicleLicence, listType) {
    var CompanyID = GetCompanyID();
    var textSpan = '';
    var _positionCode = '';
    var _backgroundcolor = '';

    if (LanguageScript.page_vehicles_Health == vehicleHealth) {
        //#825 表格宽度调整剂内部div宽度调整liangjiajie0319
        textSpan = '<td style="color:#339900"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;">' + vehicleHealth + '</div></td>';
    } else {
        //#825 表格宽度调整剂内部div宽度调整liangjiajie0319
        textSpan = '<td style="color:#ff0000"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;">' + vehicleHealth + '</div></td>';
    }
    if (vehicleImgUrl % 2 == 1) {
        _backgroundcolor = '<tr style="background-color:#f7f7f7">';
    }
    else {
        _backgroundcolor = '<tr style="background-color:#e6e6e6">';
    }

    if(vehiclePosition == null)
    {
        vehiclePosition = "";
    }
    if (vehicleMissedTime == null) {
        vehicleMissedTime = LanguageScript.export_undefine;
    }
    //车辆未分组时显示未分组liangjiajie0321
    if ("-" == vehicleGroupName) {
        vehicleGroupName = "未分组";
    }
    //#825 表格宽度调整剂内部div宽度调整liangjiajie0319
    return _backgroundcolor +
                    '<td style="padding-left:3%;" class="vehicles_groupName' + listType + '" id="vehicles_groupName' + VehicleId + listType + '"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + vehicleGroupName + '">' + vehicleGroupName + '</div></td>' +
                    '<td ><img style="height:56px;width:63px" alt="' + LanguageScript.common_NoPic + '" src="/' + CompanyID + '/Logo/DrawImage?vehicleID=' + VehicleId + '&type=vehicleLogo" /></td>' +
                    '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleName) + '</div><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleDate) + '</div></td>' +
                    '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleLicence) + '">' + $.trim(vehicleLicence) + '</div></td>' +
                    textSpan +
                    '<td ><div class="locationName' + VehicleId + '" style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehiclePosition) + '">' + $.trim(vehiclePosition) + '</div></td>' +
                    '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" >' + transTime(vehicleMissedTime).format("yyyy-MM-dd HH:mm") + '</div></td>' +
                    '<td >' +
                        '<a href = "/' + CompanyID + '/Vehicles/Detail?VehicleId=' + VehicleId + '" onclick="trans()">' +
                        '<div  class="view_detail_button" style="width:100%;">' + LanguageScript.page_Dasboard_FleetLocation_ViewDetails + '</div>' +
                        '</a>' +
                    '</td>' +
                '</tr>';
    //liangjiajie26
}


function history_vehicle_info(VehicleId, vehicleName, vehicleDate, vehiclePosition, vehicleLicence, vehicleLastUsedTime) {

    if (vehiclePosition == null) {
        vehiclePosition = "";
    }
    if (vehicleLicence == null) {
        vehicleLicence = LanguageScript.export_undefine;
    }

    var CompanyID = GetCompanyID();
    return '<tr style="background-color:#e6e6e6">' +
                '<td style="padding-left:3%;"><img style="height:56px;width:63px" alt="' + LanguageScript.common_NoPic + '" src="/' + CompanyID + '/Logo/DrawImage?vehicleID=' + VehicleId + '&type=vehicleLogo" /></td>' +
                '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleName) + '</div><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(vehicleName) + '\n' + $.trim(vehicleDate) + '">' + $.trim(vehicleDate) + '</div></td>' +
                '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" >' + vehicleLicence + '</div></td>' +
                '<td ><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" >' + transTime(vehicleLastUsedTime).format("yyyy-MM-dd HH:mm") + '</div></td>' +
                '<td ><div class="locationName' + VehicleId + '" style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + vehiclePosition + '">' + vehiclePosition + '</div></td>' +
            //#825 历史车辆表格宽度调整liangjiajie0319
            //#826 历史车辆表格宽度调整fengpan 20140324
            '</tr>';
}

function add_vehicle_popup() {
    var check_ESN = false;
    var check_KEY = false;
    $("#body_position").before(function () {
        return "<div id='add_vehicle_popup_background'></div>" +
               "<div id='add_vehicle_popup'>" +
                  "<div class='add_vehicle_popup_title'>" + LanguageScript.common_Add + "</div>" +
                  "<div id='OBU_popup_ESN_div' class='OBU_popup_ESN'>" + LanguageScript.common_ESN + ":<input id='OBU_ESN_input' type='text'/></div>" +
                  "<div id='OBU_popup_KEY_div' class='OBU_popup_KEY'>" + LanguageScript.page_settings_accountSettings_vehicleDiagnosticManagement_registrationKeyLabel + ":<input id='OBU_KEY_input' type='text'/></div>" +
                  "<div id='popup_error_info' class='popup_error_info'>" + LanguageScript.error_e01254 + "</div>" +
                  "<div id='popup_error_info_error' class='popup_error_info'>" + LanguageScript.error_e01255 + "</div>" +
                  "<div id='popup_sure' class='cls_popup_btn'>" + LanguageScript.common_save + "</div>" +
                  "<div id='popup_back' class='cls_popup_btn'>" + LanguageScript.common_cancel + "</div>" +
        "</div>";
        
    });
    var left_height = document.getElementById("u_right").scrollHeight;
    document.getElementById("add_vehicle_popup_background").style.height = (left_height + 138) + "px";

    var registion_OK;
    var OBU_ESN_id = "";
    var OBU_KEY_id = "";
    $("#OBU_ESN_input").blur(function () {
        OBU_ESN_id = $("#OBU_ESN_input").val();
        OBU_KEY_id = $("#OBU_KEY_input").val();
        if ("" == OBU_ESN_id || " " == OBU_ESN_id) {
            $("#OBU_popup_ESN_error").remove();
            $("#OBU_popup_ESN_right").remove();
            $("#OBU_popup_ESN_div").append('<div id="OBU_popup_ESN_error" class="error" style="left:230px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + LanguageScript.error_e01256 + '</div>');
            check_ESN = false;
        }
        else {
            $("#OBU_popup_ESN_error").remove();
            $("#OBU_popup_ESN_right").remove();
            $("#OBU_popup_ESN_div").append('<div id="OBU_popup_ESN_right" class="right" style="left:230px"></div>');
            $("#popup_error_info").hide();
            check_ESN = true;
        }
    })
    $("#OBU_KEY_input").blur(function () {
        OBU_ESN_id = $("#OBU_ESN_input").val();
        OBU_KEY_id = $("#OBU_KEY_input").val();
        if ("" == OBU_KEY_id || " "== OBU_KEY_id) {
            $("#OBU_popup_KEY_error").remove();
            $("#OBU_popup_KEY_right").remove();
            $("#OBU_popup_KEY_div").append('<div id="OBU_popup_KEY_error" class="error" style="left:230px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + LanguageScript.error_e01256 + '</div>');
            check_KEY = false;
        }
        else {
            //...
            $("#OBU_popup_KEY_error").remove();
            $("#OBU_popup_KEY_right").remove();
            $("#OBU_popup_KEY_div").append('<div id="OBU_popup_KEY_right" class="right" style="left:230px"></div>');
            $("#popup_error_info").hide();
            check_KEY = true;
        }
    });
    $("#popup_sure").click(function () {
        if (true == check_ESN && true == check_KEY) {
            $("#popup_error_info").hide();
            var companyID = GetCompanyID();
            registion_OK = registion_OBU(OBU_ESN_id, OBU_KEY_id);
            if (!registion_OK) {
                //...
                $("#popup_error_info_error").show();
            }
            else {
                //...
                $("#add_vehicle_popup_background").remove();
                $("#add_vehicle_popup").remove();
                //url = "/"+companyID+"/Setting/Tenant?tabNum=2"
                //location.href = url;
            }
        }
        else {
            if ("" == OBU_KEY_id || " " == OBU_KEY_id) {
                $("#popup_error_info").show();
            }
        }
    });
    $("#popup_back").click(function () {
        $("#add_vehicle_popup_background").remove();
        $("#add_vehicle_popup").remove();
    });
}

function registion_OBU(OBU_ESN_id, OBU_KEY_id) {
    //todo...
    if ("6C4C729D" == OBU_ESN_id && "59e5-ad61" == OBU_KEY_id) {
        return false;
    }
    return true;
}
function getData(tabNum) {
    var CompanyID = GetCompanyID();
    //hideAllTab();
    $("#emptyListImg").hide();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/GetGroups",
        data: {},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        beforeSend: function () {
            //$("#emptyListImg").show();
            $("#loadingImg").html('<img style="position:relative;top:110px;width:75px;" src="../../../Content/Common/images/loading_style.gif"/>');
            $("#loadingImg").css("z-index", "1");
        },
        success: function (msg) {
            dispatchGroup(msg,tabNum);
        }
    });
}

var arrAll = new Array();
var arrDrive = new Array();
var arrStop = new Array();
var arrHealth = new Array();
var arrAlert = new Array();
var arrMissed = new Array();
var arrHistory = new Array();

var currentPageAll = 1;
var currentPageDrive = 1;
var currentPageStop = 1;
var currentPageHealth = 1;
var currentPageAlert = 1;
var currentPageMissed = 1;
var currentPageHistory = 1;

var fleetInfoObj = null;

function dispatchGroup(groupids,tabNum) {
    var leftHeight = 1000;
    var vehicleType = "all";
    var vehicleListAll = "";
    var vehicleListDrive = "";
    var vehicleListStop = "";
    var vehicleListHealth = "";
    var vehicleListAlert = "";
    var vehicleListMissed = "";
    var vehicleListHistory = "";

    var vehicleListAllNum = 0;
    var vehicleListDriveNum = 0;
    var vehicleListStopNum = 0;
    var vehicleListHealthNum = 0;
    var vehicleListAlertNum = 0;
    var vehicleListMissedNum = 0;
    var vehicleListHistoryNum = 0;
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Vehicles/GetVehicleInformation",
        data: {},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        beforeSend: function () {
            //$("#emptyListImg").show();
            $("#loadingImg").html('<img style="position:relative;top:110px;width:75px;" src="../../../Content/Common/images/loading_style.gif"/>');
            $("#loadingImg").css("z-index", "1");
        },
        success: function (obj) {

            $("#loadingImg").html('');
            $("#loadingImg").css("z-index", "-1");


            fleetInfoObj = obj;
            var flag1 = 0;
            var flag2 = 0;
            var alertState = "";
            switch (tabNum) {
                case 1:
                    //all
                    arrAll = new Array();
                    for (var j = 0; j < groupids.length; j++) {
                        flag2 = 0;
                        for (var i = 0; i < obj.allVehicle.length; i++) {
                            if (0 == obj.allVehicle[i].healthStatus) {
                                alertState = LanguageScript.common_EngineOn;
                            } else {
                                alertState = LanguageScript.page_vehicles_Health;
                            }
                            if (obj.allVehicle[i].groupID == groupids[j].pkid) {
                                flag2 = 1;
                                var vehicleTemp = vehicleInfo(obj.allVehicle[i].primarykey, vehicleListAllNum, obj.allVehicle[i].name, obj.allVehicle[i].Info, $.trim(obj.allVehicle[i].groupName), alertState, obj.allVehicle[i].locationName, $.trim(obj.allVehicle[i].license), tabNum);
                                vehicleListAll += vehicleTemp;
                                if ("" != vehicleTemp) {
                                    arrAll.push(vehicleTemp);
                                }
                            }
                        }
                        if (flag2 == 1) {
                            vehicleListAllNum++;
                        }
                    }
                    $("#tab_vehicleAll tr:not(:first)").remove();
                    if ("" != vehicleListAll) {
                        //$("#tab_vehicleAll").append(vehicleListAll);
                        //mergeGroup(1);
                        //SetPageHeight("tab_vehicleAll");
                        InitVehicleAll(currentPageAll);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 2:
                    //drive
                    arrDrive = new Array();
                    if (CompanyID == "ABCSoft" || CompanyID == "ihpleD") {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.allVehicle.length; i++) {
                                if (0 == obj.allVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.allVehicle[i].groupID == groupids[j].pkid) {
                                    if (0 == obj.allVehicle[i].misState) {
                                        if (0 == obj.allVehicle[i].engineStatus) {
                                            flag2 = 1;
                                            var vehicleListDriveTemp = vehicleInfo(obj.allVehicle[i].primarykey, vehicleListDriveNum, obj.allVehicle[i].name, obj.allVehicle[i].Info, $.trim(obj.allVehicle[i].groupName), alertState, obj.allVehicle[i].locationName, obj.allVehicle[i].license, tabNum);
                                            vehicleListDrive += vehicleListDriveTemp;
                                            if ("" != vehicleListDriveTemp) {
                                                arrDrive.push(vehicleListDriveTemp);
                                            }
                                        }
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListDriveNum++;
                            }
                        }
                    } else {
                        //drive api
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.drivingVehicle.length; i++) {
                                if (0 == obj.drivingVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.drivingVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    var vehicleListDriveTemp = vehicleInfo(obj.drivingVehicle[i].primarykey, vehicleListDriveNum, obj.drivingVehicle[i].name, obj.drivingVehicle[i].Info, $.trim(obj.drivingVehicle[i].groupName), alertState, obj.drivingVehicle[i].locationName, $.trim(obj.drivingVehicle[i].license), tabNum);
                                    vehicleListDrive += vehicleListDriveTemp;
                                    if ("" != vehicleListDriveTemp) {
                                        arrDrive.push(vehicleListDriveTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListDriveNum++;
                            }
                        }
                    }
                    $("#tab_vehicleDrive tr:not(:first)").remove();
                    if ("" != vehicleListDrive) {
                        //$("#tab_vehicleDrive").append(vehicleListDrive);
                        //mergeGroup(2);
                        //SetPageHeight("tab_vehicleDrive");
                        InitVehicleDrive(currentPageDrive);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 3:
                    //stop
                    arrStop = new Array();
                    if (CompanyID == "ABCSoft" || CompanyID == "ihpleD") {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.allVehicle.length; i++) {
                                if (0 == obj.allVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.allVehicle[i].groupID == groupids[j].pkid) {

                                    if (0 == obj.allVehicle[i].misState) {
                                        if (1 == obj.allVehicle[i].engineStatus) {
                                            flag2 = 1;
                                            var vehicleListStopTemp = vehicleInfo(obj.allVehicle[i].primarykey, vehicleListStopNum, obj.allVehicle[i].name, obj.allVehicle[i].Info, $.trim(obj.allVehicle[i].groupName), alertState, obj.allVehicle[i].locationName, obj.allVehicle[i].license, tabNum);
                                            vehicleListStop += vehicleListStopTemp;
                                            if("" != vehicleListStopTemp)
                                            {
                                                arrStop.push(vehicleListStopTemp);
                                            }
                                        }
                                    }
                                }
                            }
                            if (1 == flag2) {
                                vehicleListStopNum++;
                            }
                        }
                    }
                    else {
                        //stop api
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.parkingVehicle.length; i++) {
                                if (0 == obj.parkingVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.parkingVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    vehicleListStopTemp = vehicleInfo(obj.parkingVehicle[i].primarykey, vehicleListStopNum, obj.parkingVehicle[i].name, obj.parkingVehicle[i].Info, $.trim(obj.parkingVehicle[i].groupName), alertState, obj.parkingVehicle[i].locationName, $.trim(obj.parkingVehicle[i].license), tabNum);
                                    vehicleListStop += vehicleListStopTemp;
                                    if ("" != vehicleListStopTemp) {
                                        arrStop.push(vehicleListStopTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListStopNum++;
                            }
                        }
                    }
                    $("#tab_vehicleStop tr:not(:first)").remove();
                    if ("" != vehicleListStop) {
                        //$("#tab_vehicleStop").append(vehicleListStop);
                        //mergeGroup(3);
                        //SetPageHeight("tab_vehicleStop");
                        InitVehicleStop(currentPageStop);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 4:
                    //health
                    arrHealth = new Array();
                    if ("ABCSoft" == CompanyID || "ihpleD" == CompanyID) {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.allVehicle.length; i++) {
                                if (0 == obj.allVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.allVehicle[i].groupID == groupids[j].pkid) {

                                    if (0 == obj.allVehicle[i].healthStatus) {
                                        flag2 = 1;
                                        vehicleListHealthTemp = vehicleInfo(obj.allVehicle[i].primarykey, vehicleListHealthNum, obj.allVehicle[i].name, obj.allVehicle[i].Info, $.trim(obj.allVehicle[i].groupName), LanguageScript.common_EngineOn, obj.allVehicle[i].locationName, obj.allVehicle[i].license, tabNum);
                                        vehicleListHealth += vehicleListHealthTemp;
                                        if ("" != vehicleListHealthTemp) {
                                            arrHealth.push(vehicleListHealthTemp);
                                        }
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListHealthNum++;
                            }
                        }
                    }
                    else {
                        //health api
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.breakVehicle.length; i++) {
                                if (0 == obj.breakVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.breakVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    vehicleListHealthTemp = vehicleInfo(obj.breakVehicle[i].primarykey, vehicleListHealthNum, obj.breakVehicle[i].name, obj.breakVehicle[i].Info, $.trim(obj.breakVehicle[i].groupName), alertState, obj.breakVehicle[i].locationName, $.trim(obj.breakVehicle[i].license), tabNum);
                                    vehicleListHealth += vehicleListHealthTemp;
                                    if ("" != vehicleListHealthTemp) {
                                        arrHealth.push(vehicleListHealthTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListHealthNum++;
                            }
                        }
                    }
                    $("#tab_vehicleHealth tr:not(:first)").remove();
                    if ("" != vehicleListHealth) {
                        //$("#tab_vehicleHealth").append(vehicleListHealth);
                        //mergeGroup(4);
                        //SetPageHeight("tab_vehicleHealth");
                        InitVehicleHealth(currentPageHealth);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 5:
                    //alert
                    arrAlert = new Array();
                    if ("ABCSoft" == CompanyID || "ihpleD" == CompanyID) {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.alertVehicle.length; i++) {
                                if (0 == obj.alertVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.alertVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    var vehicleListAlertTemp = "";
                                    if (0 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_vehicles_SpeedAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum);

                                    } else if (2 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_report_RPMAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum)
                                    } else if (1 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_report_MotionAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum);
                                    }
                                    vehicleListAlert += vehicleListAlertTemp;
                                    if ("" != vehicleListAlertTemp) {
                                        arrAlert.push(vehicleListAlertTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListAlertNum++;
                            }
                        }
                    }
                    else {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.alertVehicle.length; i++) {
                                if (0 == obj.alertVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.alertVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    var vehicleListAlertTemp = "";
                                    if (0 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_vehicles_SpeedAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum);
                                    } else if (2 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_report_RPMAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum);
                                    } else if (1 == obj.alertVehicle[i].alertType) {
                                        vehicleListAlertTemp = vehicleInfo(obj.alertVehicle[i].primarykey, vehicleListAlertNum, obj.alertVehicle[i].name, obj.alertVehicle[i].Info, $.trim(obj.alertVehicle[i].groupName), LanguageScript.page_report_MotionAlert, obj.alertVehicle[i].locationName, obj.alertVehicle[i].license, tabNum);
                                    }
                                    vehicleListAlert += vehicleListAlertTemp;
                                    if ("" != vehicleListAlertTemp) {
                                        arrAlert.push(vehicleListAlertTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListAlertNum++;
                            }
                        }
                    }
                    $("#tab_vehicleAlert tr:not(:first)").remove();
                    if ("" != vehicleListAlert) {
                        //$("#tab_vehicleAlert").append(vehicleListAlert);
                        //mergeGroup(5);
                        //SetPageHeight("tab_vehicleAlert");
                        InitVehicleAlert(currentPageAlert);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 6:
                    //missed
                    arrMissed = new Array();
                    if ("ABCSoft" == CompanyID || "ihpleD" == CompanyID) {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.allVehicle.length; i++) {
                                if (0 == obj.allVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.allVehicle[i].groupID == groupids[j].pkid) {

                                    if (1 == obj.allVehicle[i].misState) {
                                        flag2 = 1;
                                        var vehicleListMissedTemp = miss_vehicleInfo(obj.allVehicle[i].primarykey, vehicleListMissedNum, obj.allVehicle[i].name, obj.allVehicle[i].Info, $.trim(obj.allVehicle[i].groupName), alertState, obj.allVehicle[i].locationName, obj.allVehicle[i].lastUsedTime, obj.allVehicle[i].license, tabNum);
                                        vehicleListMissed += vehicleListMissedTemp;
                                        if ("" != vehicleListMissedTemp) {
                                            arrMissed.push(vehicleListMissedTemp);
                                        }
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListMissedNum++;
                            }
                        };
                    }
                    else {
                        for (var j = 0; j < groupids.length; j++) {
                            flag2 = 0;
                            for (var i = 0; i < obj.misstargetVehicle.length; i++) {
                                if (0 == obj.misstargetVehicle[i].healthStatus) {
                                    alertState = LanguageScript.common_EngineOn;
                                } else {
                                    alertState = LanguageScript.page_vehicles_Health;
                                }
                                if (obj.misstargetVehicle[i].groupID == groupids[j].pkid) {
                                    flag2 = 1;
                                    var vehicleListMissedTemp = miss_vehicleInfo(obj.misstargetVehicle[i].primarykey, vehicleListMissedNum, obj.misstargetVehicle[i].name, obj.misstargetVehicle[i].Info, $.trim(obj.misstargetVehicle[i].groupName), alertState, obj.misstargetVehicle[i].locationName, obj.misstargetVehicle[i].lastUsedTime, obj.misstargetVehicle[i].license, tabNum);;
                                    vehicleListMissed += vehicleListMissedTemp;
                                    if ("" != vehicleListMissedTemp) {
                                        arrMissed.push(vehicleListMissedTemp);
                                    }
                                }
                            }
                            if (flag2 == 1) {
                                vehicleListMissedNum++;
                            }
                        };
                    }
                    $("#tab_vehicleMissed tr:not(:first)").remove();
                    if ("" != vehicleListMissed) {
                        //$("#tab_vehicleMissed").append(vehicleListMissed);
                        //mergeGroup(6);
                        //SetPageHeight("tab_vehicleMissed");
                        InitVehicleMissed(currentPageMissed);
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                case 7:
                    //history 20140311
                    arrHistory = new Array();
                    if ("ABCSoft" == CompanyID || "ihpleD" == CompanyID) {
                        for (var j = 0; j < 2; j++) {
                            if (j == 0) {
                                var vehicleListHistoryTemp = history_vehicle_info(1, "奔驰123", "东风雪铁龙", "北京市通州区", "辽A54671", "2014-03-11 13:20");
                                vehicleListHistory += vehicleListHistoryTemp;
                                arrHistory.push(vehicleListHistoryTemp);
                            }
                            if (j == 1) {
                                var vehicleListHistoryTemp = history_vehicle_info(2, "东风雪铁龙东风雪铁龙", "东风雪铁龙", "辽宁省沈阳市沈河区市府大路青年大街", "辽A86943", "2014-03-11 09:20");
                                vehicleListHistory += vehicleListHistoryTemp;
                                arrHistory.push(vehicleListHistoryTemp);
                            }
                        };
                    }
                    else {
                        for (var i = 0; i < obj.historyVehicle.length; i++) {
                            var vehicleListHistoryTemp = history_vehicle_info(obj.historyVehicle[i].primarykey, obj.historyVehicle[i].name, obj.historyVehicle[i].Info, "", obj.historyVehicle[i].license, obj.historyVehicle[i].lastUsedTime);
                            vehicleListHistory += vehicleListHistoryTemp;
                            if ("" != vehicleListHistoryTemp) {
                                arrHistory.push(vehicleListHistoryTemp);
                            }
                        }
                    }
                    $("#tab_vehicleHistory tr:not(:first)").remove();
                    if ("" != vehicleListHistory) {
                        //$("#tab_vehicleHistory").append(vehicleListHistory);
                        //SetPageHeight("tab_vehicleHistory");
                        InitVehicleHistory(currentPageHistory);
                        if (!("ABCSoft" == CompanyID || "ihpleD" == CompanyID)) {
                            for (var i = 0; i < obj.historyVehicle.length; i++) {
                                geocoderLocationForVehicleList(obj.historyVehicle[i].location.longitude, obj.historyVehicle[i].location.latitude, "locationName" + obj.historyVehicle[i].primarykey);
                            }
                        }
                        $("#emptyListImg").hide();
                    }
                    else {
                        $("#emptyListImg").show();
                    }
                    break;
                default: break;
            }
            for (var i = 0; i < obj.allVehicle.length; i++)
            {
                geocoderLocationForVehicleList(obj.allVehicle[i].location.longitude, obj.allVehicle[i].location.latitude, "locationName" + obj.allVehicle[i].primarykey);
            }
        }
    });
}

function SetPageHeight(tabId) {
    if ($("#" + tabId)[0].offsetHeight > 702) {
        SetLeftBarHeight($("#" + tabId)[0].offsetHeight + 98);
    } else {
        SetLeftBarHeight(800);
    }
}

function mergeGroup(tabNum) {
    if (null == $(".vehicles_groupName" + tabNum) || $(".vehicles_groupName" + tabNum).length == 0) {
        return;
    }
    var firstRow = $(".vehicles_groupName" + tabNum);
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
    groupsCnts[j] = cnt;
    var removeNum = 1;
    for (var i = 0; i < groupsCnts.length; i++) {
        $("#" + $(".vehicles_groupName" + tabNum)[i].id).attr("rowspan", groupsCnts[i]);
        $("#" + $(".vehicles_groupName" + tabNum)[i].id).css("border-right", "1px solid #ddd");
        for (var j = 1; j < groupsCnts[i]; j++) {
            firstRow[removeNum + j - 1].parentNode.removeChild(firstRow[removeNum + j - 1]);
            //firstRow[removeNum+j-1].remove();
        }
        removeNum += groupsCnts[i];
    }
}


function hideAllTab()
{
    $("#tab_vehicleAll tr:not(:first)").hide();
    $("#tab_vehicleDrive tr:not(:first)").hide();
    $("#tab_vehicleStop tr:not(:first)").hide();
    $("#tab_vehicleHealth tr:not(:first)").hide();
    $("#tab_vehicleAlert tr:not(:first)").hide();
    $("#tab_vehicleMissed tr:not(:first)").hide();
    $("#tab_vehicleHistory tr:not(:first)").hide();
}

function GeocoderLocation(obj)
{
    for (var i = 0; i < obj.allVehicle.length; i++) {
        geocoderLocationForVehicleList(obj.allVehicle[i].location.longitude, obj.allVehicle[i].location.latitude, "locationName" + obj.allVehicle[i].primarykey);
    }
}
function GeocoderHistoryVehicleLocation(obj) {
    for (var i = 0; i < obj.historyVehicle.length; i++) {
        geocoderLocationForVehicleList(obj.historyVehicle[i].location.longitude, obj.historyVehicle[i].location.latitude, "locationName" + obj.historyVehicle[i].primarykey);
    }
}

//一页显示多少个
var onePageNum = 9;

InitVehicleAll = function (pagenumber) {
    $("#tab_vehicleAll tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrAll.length; i++) {
        $("#tab_vehicleAll").append(arrAll[i]);
    }
    mergeGroup(1);
    SetPageHeight("tab_vehicleAll");
    $("#all_pageBar").pager({ pagenumber: pagenumber, pagecount: arrAll.length % onePageNum == 0 ? arrAll.length / onePageNum : arrAll.length / onePageNum + 1, buttonClickCallback: VehicleAllPageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleAllPageClick = function (pageclickednumber) {
    InitVehicleAll(pageclickednumber)
    currentPageAll = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleDrive = function (pagenumber) {
    $("#tab_vehicleDrive tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrDrive.length; i++) {
        $("#tab_vehicleDrive").append(arrDrive[i]);
    }
    mergeGroup(2);
    SetPageHeight("tab_vehicleDrive");
    $("#drive_pageBar").pager({ pagenumber: pagenumber, pagecount: arrDrive.length % onePageNum == 0 ? arrDrive.length / onePageNum : arrDrive.length / onePageNum + 1, buttonClickCallback: VehicleDrivePageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleDrivePageClick = function (pageclickednumber) {
    InitVehicleDrive(pageclickednumber)
    currentPageDrive = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleStop = function (pagenumber) {
    $("#tab_vehicleStop tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrStop.length; i++) {
        $("#tab_vehicleStop").append(arrStop[i]);
    }
    mergeGroup(3);
    SetPageHeight("tab_vehicleStop");
    $("#stop_pageBar").pager({ pagenumber: pagenumber, pagecount: arrStop.length % onePageNum == 0 ? arrStop.length / onePageNum : arrStop.length / onePageNum + 1, buttonClickCallback: VehicleStopPageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleStopPageClick = function (pageclickednumber) {
    InitVehicleStop(pageclickednumber)
    currentPageStop = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleHealth = function (pagenumber) {
    $("#tab_vehicleHealth tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrHealth.length; i++) {
        $("#tab_vehicleHealth").append(arrHealth[i]);
    }
    mergeGroup(4);
    SetPageHeight("tab_vehicleHealth");
    $("#health_pageBar").pager({ pagenumber: pagenumber, pagecount: arrHealth.length % onePageNum == 0 ? arrHealth.length / onePageNum : arrHealth.length / onePageNum + 1, buttonClickCallback: VehicleHealthPageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleHealthPageClick = function (pageclickednumber) {
    InitVehicleHealth(pageclickednumber)
    currentPageHealth = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleAlert = function (pagenumber) {
    $("#tab_vehicleAlert tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrAlert.length; i++) {
        $("#tab_vehicleAlert").append(arrAlert[i]);
    }
    mergeGroup(5);
    SetPageHeight("tab_vehicleAlert");
    $("#alert_pageBar").pager({ pagenumber: pagenumber, pagecount: arrAlert.length % onePageNum == 0 ? arrAlert.length / onePageNum : arrAlert.length / onePageNum + 1, buttonClickCallback: VehicleAlertPageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleAlertPageClick = function (pageclickednumber) {
    InitVehicleAlert(pageclickednumber)
    currentPageAlert = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleMissed = function (pagenumber) {
    $("#tab_vehicleMissed tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrMissed.length; i++) {
        $("#tab_vehicleMissed").append(arrMissed[i]);
    }
    mergeGroup(6);
    SetPageHeight("tab_vehicleMissed");
    $("#missed_pageBar").pager({ pagenumber: pagenumber, pagecount: arrMissed.length % onePageNum == 0 ? arrMissed.length / onePageNum : arrMissed.length / onePageNum + 1, buttonClickCallback: VehicleMissedPageClick });
    GeocoderLocation(fleetInfoObj);
}
VehicleMissedPageClick = function (pageclickednumber) {
    InitVehicleMissed(pageclickednumber)
    currentPageMissed = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}

InitVehicleHistory = function (pagenumber) {
    $("#tab_vehicleHistory tr:not(:first)").remove();
    for (var i = (pagenumber - 1) * onePageNum; i < pagenumber * onePageNum && i < arrHistory.length; i++) {
        $("#tab_vehicleHistory").append(arrHistory[i]);
    }
    //mergeGroup(7);
    SetPageHeight("tab_vehicleHistory");
    $("#history_pageBar").pager({ pagenumber: pagenumber, pagecount: arrHistory.length % onePageNum == 0 ? arrHistory.length / onePageNum : arrHistory.length / onePageNum + 1, buttonClickCallback: VehicleHistoryPageClick });
    GeocoderHistoryVehicleLocation(fleetInfoObj);
}
VehicleHistoryPageClick = function (pageclickednumber) {
    InitVehicleHistory(pageclickednumber);
    currentPageHistory = pageclickednumber;
    $("#result").html("Clicked Page " + pageclickednumber);
}