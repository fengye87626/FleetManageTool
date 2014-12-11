$(document).ready(function () {
	WaitingTAB = "";
    SetLeftBarHeight(800);

    //logosubmitflag = 0;
    odometerFlag = 0;
    currentid = 0;
    //PreviousVIN = "";
    //caoyandong
    pagenum_user = 1;

    Setting_accunt_display();
    Setting_tenant_display();
    $("#ui-right_name").hide();
    $("#ui-right_email").hide();
    $("#ui-right_tel").hide();
    $("#ui-right_ESN").hide();
    $("#ui-right_KEY").hide();
    $("#ui-right_groupadd").hide();
    $("#ui-right_groupedit").hide();
    var tabNum = $("#tabNum").val();//fengpan #508 20140304
    var vehicleId = $("#VehicleID").val();
    if ("" == tabNum) {//fengpan #508 20140304
        tabNum = "0";
    }
    switch (tabNum)
    {
        case "0":
            Height_User(8, 50);
            $("#Setting_container_tenant").show();
            $("#Setting_container_user").hide();
            $("#Setting_container_obu").hide();
            $("#Setting_container_group").hide();
            break;
        case "1":
            Height_User(8, 50);
            $("#Setting_container_user_list").empty();
            $("#pageBar").attr("successflag", "false");
            getUserData(pagenum_user);
            bind_event();
            $("#user_edit_sure").remove();
            $("#user_edit_back").remove();
            var roleID = GetRoleID();
            if (1 != roleID) {
                unbind_event();
            }
            $("#Setting_container_tenant").hide();
            $("#Setting_container_user").show();
            $("#Setting_container_obu").hide();
            $("#Setting_container_group").hide();
            break;
        case "2":
            Height_User(8, 50);
            unbind_OBU_editANDadd();
            $("#Setting_container_OBU_btn_return").unbind();
            $("#Setting_container_OBU_btn_confirm").unbind();
            $("#Setting_container_OBU_btn_confirm").hide();
            $("#Setting_container_OBU_btn_return").hide();
            var roleID = GetRoleID();
            if (1 == roleID) {
                bind_OBU_editANDadd();
            } else {
                //...
            }
            $("#Setting_tenant").removeClass("setting_img_bg_normal");
            $("#Setting_user").removeClass("setting_img_bg_disable");
            $("#Setting_OBU").removeClass("setting_img_bg_disable");
            $("#Setting_Group").removeClass("setting_img_bg_disable");

            $("#Setting_tenant").addClass("setting_img_bg_disable");
            $("#Setting_user").addClass("setting_img_bg_disable");
            $("#Setting_OBU").addClass("setting_img_bg_normal");
            $("#Setting_Group").addClass("setting_img_bg_disable");
            getObuData(vehicleId);
            
            
            $("#Setting_container_tenant").hide();
            $("#Setting_container_user").hide();
            $("#Setting_container_obu").show();
            $("#Setting_container_group").hide();
            //var NowUrl = window.location.href;
            //if (NowUrl.indexOf("tabNum=2") + 8 < NowUrl.length) {
            //    HrefFlag++;
            //}
            clearSession();//fengpan #508 20140304
            break;
        case "3":
            Height_User(8, 50);
            //获取group数据
            GetGroup();
            //绑定事件 TONEUP
            Group_ToneUp();
            $("#group_edit_sure").remove();
            $("#group_edit_back").remove();
            $("#li_add").remove();
            var roleID = GetRoleID();
            if (1 != roleID) {
                Group_ToneDown();
            }
            $("#Setting_container_tenant").hide();
            $("#Setting_container_user").hide();
            $("#Setting_container_obu").hide();
            $("#Setting_container_group").show();
            break;
        default:
            break;
    }
    //$("#Setting_container_tenant").show();
    //$("#Setting_container_user").hide();
    //$("#Setting_container_obu").hide();
    //$("#Setting_container_group").hide();

    //$("#Edit_accunt_button").click(function () {
    //    $("#accunt_info").hide();
    //    $("#accunt_edit").show();
    //    accunt_edit_display();
    //});

    //$("#Setting_container_tenant_btn_edit").click(function () {
    //    $("#Setting_tenant_view").hide();
    //    $("#Setting_tenant_Edit").show();
    //    $("#tenant_edit_inputs").html($("#tenant_edit_inputs").html());
    //    tenant_edit_dis();
    //});

    //$("#Setting_container_tenant_btn_confirm").click(function () {
    //    $("#Setting_tenant_view").show();
    //    $("#Setting_tenant_Edit").hide();
    //    remove_right_error();
    //    tenant_edit_confirm();
    //    //Setting_tenant_display();
    //});
    //$("#Setting_container_tenant_btn_return").click(function () {
    //    $("#Setting_tenant_view").show();
    //    $("#Setting_tenant_Edit").hide();
    //    remove_right_error();
    //    Setting_tenant_display();
    //});
    $("#Setting_tenant").click(function () {
        Set_tenant();
    });
    $("#Setting_user").click(function () {
        Set_user();
    });
    $("#Setting_OBU").click(function () {
        $("#all_pageBar_Setting").find("input").val("");
        Set_OBU();
    });
    $("#Setting_Group").click(function () {
        Set_group();
    });
    $("#Setting_Alert").click(function () {//fengpan 20140308 #638
        Set_alert();
    });
    /********** Accunt and common fengpan **********/

    
    /*************Tenant fengpan****************/
    
    ///*正则检测公司公司的电话*/
    //$("#Setting_tenant_edit_tel").blur(function () {
    //    var TelValue = document.getElementById("Setting_tenant_edit_tel").value;
    //    checkTel(TelValue);
    //    if(true){
    //        $("#accunt_edit_confirmPassword_right").remove();
    //        $("#accunt_edit_confirmPassword_error").remove();
    //        $("#Setting_tenant_edit_tel").append('<div id="accunt_edit_confirmPassword_right" class="right"></div>');
    //    }
    //});
    ///*正则检测公司Email*/
    //$("#Setting_tenant_edit_mail").blur(function () {
    //    var emailValue = document.getElementById("Setting_tenant_edit_mail").value;
    //    checkEmail(emailValue);
    //});
    
    
   
    

    var checkbox_num = function () {
        var input_num = $("#Setting_container_obu_view").find("input").length;
        var checkbox_true_num = 0;
        for (var i = 0; i < input_num; i++) {
            var checkbox = $("#Setting_container_obu_view").find("input")[i].checked;
            if (checkbox == true) {
                checkbox_true_num++;
            }
        }
        return checkbox_true_num;
    }
    //OBU BTN ADD
    //$("#Setting_container_OBU_btn_add").click(function () {
        //OBU_add();
        //OBU_registion_OBU();
    //})

    $("#Setting_container_OBU_btn_edit").unbind();
    $("#Setting_container_OBU_btn_edit").click(function () {
        OBU_edit();
    })
    
    /********** OBU fengpan **************/
    ChangeLeft("common_setting_cover");
    ChangeLocationTime();


    //fengpan 20140616
    mmy_dialog_changeBlurEnent();

});

var editVehicleFlag = 0;

$.ajaxSetup({
    statusCode: {
        499: function (data) {
            window.location.reload();
        },
        599: function (data) {
            alert(LanguageScript.page_common_Role_Change);
            window.location.href = "/";
        },
        699: function (data) {
            alert(LanguageScript.page_common_tenant_inactive);
            window.location.href = "/";
        },
        799: function (data) {
            alert(LanguageScript.page_common_tenant_deleted);
            window.location.href = "/";
        }
    }
});

/*************fengpan***************/
function getByteLen(val) {
    var len = 0;
    for (var i = 0; i < val.length; ++i) {
        if (val[i].match(/[^\x00-\xff]/ig) != null)
            len += 2;
        else
            len += 1;
    }
    return len;
}
function remove_right_error() {
    $("#tenant_edit_phone_right").remove();
    $("#tenant_edit_phone_error").remove();
    $("#tenant_edit_mail_right").remove();
    $("#tenant_edit_mail_error").remove();
    $("#tenant_edit_name_right").remove();
    $("#tenant_edit_name_error").remove();
    $("#tenant_edit_summary_right").remove();
    $("#tenant_edit_summary_error").remove();
}
function tenant_edit_dis(tenant__summary, tenant__email) {//fengpan #533 20140304
    //$("#tenant_name_response").hide();
    //$("#tenant_summary_response").css("display", "none");
    //Redmine#440liangjiajie0306
    var tenant_name = $.trim($("#tenant_name").html());
    var tenant_id = $.trim($("#tenant_id").html());
    var tenant_email = tenant__email;
    var tenant_tel = $.trim($("#tenant_tel").html());
    var tenant_logo_url = ""
    //var tenant_summary = $.trim($("#tenant_summary").html());
    var tenant_summary = tenant__summary;//fengpan #533 20140304

    $("#tenant_edit_name").val(tenant_name);
    $("#tenant_edit_name").keyup(function () {
        $("#tenant_edit_name").attr("title", $("#tenant_edit_name").val());
    }).keyup();
    $("#tenant_companyID").html(tenant_id);
    //$("#Setting_tenant_edit_mail").val(tenant_email);
    $("#Setting_tenant_edit_tel").val(tenant_tel);
    $("#tenant_edit_summary").val(tenant_summary);

    //初始化所有默认标志位true
    //Redmine#440liangjiajie0306
    var check_tenant_name = true;
    var check_tenant_mail = true;
    var check_tenant_tel = true ; 
    var check_tenant_summary = true;

    //Redmine#440liangjiajie0306检测输入框的值
    tenant_check_name(document.getElementById("tenant_edit_name").value);
    tenant_check_tel(document.getElementById("Setting_tenant_edit_tel").value);
    //tenant_check_mail(document.getElementById("Setting_tenant_edit_mail").value);
    tenant_check_summary(document.getElementById("tenant_edit_summary").value);

    function tenant_check_name(NameValue) {
        //排除公司名称未做修改的情况
        //liangjiajie20140403
        if (tenant_name == NameValue) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_edit_name_red").append('<div id="tenant_edit_name_right" class="tenant_right"></div>');
            $("#tenant_name_response").text("");
            check_tenant_name = true;
            return;
        }
        var result = isName(NameValue);
        if (1 == result) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_edit_name_red").append('<div id="tenant_edit_name_right" class="tenant_right"></div>');
            $("#tenant_name_response").text("");    //Redmine#440liangjiajie0306使用空白字符达到隐藏效果
            check_tenant_name = true;
        } else if (2 == result) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_name_response").text(LanguageScript.common_EnterCompanyName);
            $("#tenant_name_response").css("color", "red");
            check_tenant_name = false;
        } else if (3 == result) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_name_response").text(LanguageScript.error_e01250);
            $("#tenant_name_response").css("color", "red");
            check_tenant_name = false;
        } else if (4 == result) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_name_response").text(LanguageScript.error_e01249);
            $("#tenant_name_response").css("color", "red");
            check_tenant_name = false;
        } else if (5 == result) {
            $("#tenant_edit_name_right").remove();
            $("#tenant_name_response").text(LanguageScript.error_e01251);
            $("#tenant_name_response").css("color", "red");
            check_tenant_name = false;
        }
    }
    function tenant_check_tel(TelValue) {
        var result = isPhone($.trim(TelValue));
        if (1 == result) {
            //$("#tenant_edit_phone_right").remove();
            //$("#tenant_edit_tel_td").append('<div id="tenant_edit_phone_right" class="tenant_right"></div>');
            $("#tenant_phone_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
            check_tenant_tel = true;
        } else if (-1 == result) {
            //$("#tenant_edit_phone_right").remove();
            $("#tenant_phone_response").text(LanguageScript.common_CorrectFormatTel);
            $("#tenant_phone_response").css("color", "red");
            check_tenant_tel = false;
        } else if (0 == result) {
            $("#tenant_phone_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
            //$("#tenant_edit_phone_right").remove();
            check_tenant_tel = true;

        }
    }
    
    function tenant_check_summary(summaryValue) {
        if (isSummary(summaryValue)) {
            $("#tenant_summary_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
            check_tenant_summary = true;
        }
        else {
            $("#tenant_summary_response").text("输入的内容大于1000个字符");
            $("#tenant_summary_response").css("color", "red");
            check_tenant_summary = false;
        }
    }

    /*逻辑：错误字符->空->过长->正确*/
    //Redmine#440liangjiajie0308调整判断逻辑
    function isName(NameValue) {
        NameValue = $.trim(NameValue);
        var reg = /^[a-zA-Z0-9\_\s\,\.\u4e00-\u9fa5]{1,40}$/;
        var len = getByteLen(NameValue);
        var result = 0;
        if (0 == len) {//输入为空
            result = 2;
            return result;
        } else if (!reg.test(NameValue)) {//输入错误字符
            result = 4;
            return result;
        } else  if (len > 80) {//输入过长
            result = 3;
            return result;
        } else {
            //增加公司名称同名检测逻辑，调用和注册页面一样的ajax
            //liangjiajie20140403
            $.ajax({
                type: "POST",
                async: false,
                url: "/hck-fleetadmin/IsCompanyNameExist",
                data: { companyName: NameValue },
                contentType: "application/x-www-form-urlencoded",
                dataType: "text",
                success: function (msg) {
                    result = msg;
               }
            });
            if ("false" == result) {
                result = 1;
                return result;
            } else {
                result = 5;
                return result;
            }
        }
    }

    function isSummary(summaryValue) {
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
    

    /*正则检测公司的名称*/
    $("#tenant_edit_name").blur(function () {
        var NameValue = document.getElementById("tenant_edit_name").value;
        $("#tenant_edit_name").attr("title", NameValue);
        //alert(NameValue.length);
        //isPhone(TelValue);
        tenant_check_name(NameValue);
        //if (4 < NameValue.length && 28 > NameValue.length) {
        //    $("#tenant_edit_name_right").remove();
        //    $("#tenant_edit_name_error").remove();
        //    $("#tenant_edit_name_td").append('<div id="tenant_edit_name_right" class="tenant_right"></div>');
        //}
        //else {
        //    $("#tenant_edit_name_right").remove();
        //    $("#tenant_edit_name_error").remove();
        //    $("#tenant_edit_name_td").append('<div id="tenant_edit_name_error" class="tenant_error"></div>');
        //}
    });

    /*显示与隐藏公司名字输入提示*/
    $("#tenant_edit_name").mousedown(function () {
        //$("#tenant_name_response").show();    //Redmine#440liangjiajie0306常显示 注释
        //$("#Setting_tenant_edit_tel").text("");
        $("#tenant_name_response").text(LanguageScript.common_CompanyNameRule);
        $("#tenant_name_response").css("color", "#aaa");
    });
    $("#tenant_edit_name").focus(function () {
        //$("#tenant_name_response").show();    //Redmine#440liangjiajie0306常显示 注释
        //$("#Setting_tenant_edit_tel").text("");
        $("#tenant_name_response").text(LanguageScript.common_CompanyNameRule);
        $("#tenant_name_response").css("color", "#aaa");
    });

    /*正则检测公司的电话*/
    $("#Setting_tenant_edit_tel").blur(function () {
        var TelValue = document.getElementById("Setting_tenant_edit_tel").value;
        //isPhone(TelValue);
        tenant_check_tel(TelValue);
        //if (isPhone(TelValue)) {
        //    $("#tenant_edit_phone_right").remove();
        //    $("#tenant_edit_phone_error").remove();
        //    $("#tenant_edit_tel_td").append('<div id="tenant_edit_phone_right" class="tenant_right"></div>');
        //}
        //else {
        //    $("#tenant_edit_phone_right").remove();
        //    $("#tenant_edit_phone_error").remove();
        //    $("#tenant_edit_tel_td").append('<div id="tenant_edit_phone_error" class="tenant_error"></div>');
        //}
    });

    /*显示与隐藏电话输入提示*/
    $("#Setting_tenant_edit_tel").mousedown(function () {
        //$("#tenant_phone_response").show();    //Redmine#440liangjiajie0306常显示 注释
        //$("#tenant_name_response").text("");
        $("#tenant_phone_response").text(LanguageScript.common_NoPhoneInput);
        $("#tenant_phone_response").css("color", "#aaa");
    });
    $("#Setting_tenant_edit_tel").focus(function () {
        //$("#tenant_phone_response").show();    //Redmine#440liangjiajie0306常显示 注释
        //$("#tenant_name_response").text("");
        $("#tenant_phone_response").text(LanguageScript.common_NoPhoneInput);
        $("#tenant_phone_response").css("color", "#aaa");
    });
    /*正则检测公司简介*/
    $("#tenant_edit_summary").blur(function () {
        var summaryValue = document.getElementById("tenant_edit_summary").value;
        //isEmail(emailValue);
        tenant_check_summary(summaryValue);
        //if (4 < summaryValue.length && 100 > summaryValue.length) {
        //    $("#tenant_edit_summary_right").remove();
        //    $("#tenant_edit_summary_error").remove();
        //    $("#tenant_edit_summary_td").append('<div id="tenant_edit_summary_right" class="tenant_right" style="left:210px;top:45px"></div>');
        //}
        //else {
        //    $("#tenant_edit_summary_right").remove();
        //    $("#tenant_edit_summary_error").remove();
        //    $("#tenant_edit_summary_td").append('<div id="tenant_edit_summary_error" class="tenant_error" style="left:210px;top:45px"></div>');
        //}
    });
    /*显示与隐藏公司简介输入提示*/
    $("#tenant_edit_summary").mousedown(function () {
        //$("#tenant_summary_response").show();    //Redmine#440liangjiajie0306常显示 注释
        $("#tenant_summary_response").text(LanguageScript.error_e01258);
        $("#tenant_summary_response").css("color", "#aaa");
    });

    $("#tenant_edit_summary").focus(function () {
        //$("#tenant_summary_response").show();    //Redmine#440liangjiajie0306常显示 注释
        $("#tenant_summary_response").text(LanguageScript.error_01258);
        $("#tenant_summary_response").css("color", "#aaa");
    });

    $("#Setting_container_tenant_btn_confirm").click(function () {
        /*liangjiajie 防止编辑->确认（错误数据没法修改）*/
	    //Redmine#440liangjiajie0306点击确认检查一次
        tenant_check_name(document.getElementById("tenant_edit_name").value);
        tenant_check_tel(document.getElementById("Setting_tenant_edit_tel").value);
        //tenant_check_mail(document.getElementById("Setting_tenant_edit_mail").value);
        tenant_check_summary(document.getElementById("tenant_edit_summary").value);

        //$("#Setting_tenant_view").show();
        //$("#Setting_tenant_Edit").hide();
        if (check_tenant_name == true &&
            check_tenant_tel == true &&
            //check_tenant_mail == true &&
            check_tenant_summary == true) {
            remove_right_error();
            tenant_edit_confirm();
            //document.forms['editTenant'].submit();
	    $("#Setting_container_tenant_btn_return").unbind();
            $("#Setting_container_tenant_btn_confirm").unbind();
            HrefFlag--;
        }
        
        //remove_right_error();
        //tenant_edit_confirm();
        //Setting_tenant_display();
    });
    $("#Setting_container_tenant_btn_return").click(function () {
        HrefFlag--;
        $("#Setting_tenant_view").show();
        $("#Setting_tenant_Edit").hide();
        //remove_right_error();
        Setting_tenant_display();
        $("#Setting_container_tenant_btn_return").unbind();
        $("#Setting_container_tenant_btn_confirm").unbind();
    });

    function tenant_edit_confirm() {
        var tenant_name_input = $.trim($("#tenant_edit_name").val());
        var tenant_email_input = $.trim($("#hideRemindEmail").val());// gaoqingbo 20140313
        var tenant_tel_input = $.trim($("#Setting_tenant_edit_tel").val());
        var tenant_summary_input = unicode($.trim($("#tenant_edit_summary").val()));

        var CompanyID = GetCompanyID();

        if (check_tenant_name == true &&
        check_tenant_tel == true &&
        //check_tenant_mail == true &&
        check_tenant_summary == true) {
            $.ajax({
                type: "POST",
                url: "/" + CompanyID + "/Setting/EditTenant_info",
                data: { companyName: tenant_name_input, companyEmail: tenant_email_input, companyTel: tenant_tel_input, companyIntro: tenant_summary_input },
                contentType: "application/x-www-form-urlencoded",
                dataType: "text",
                success: function (msg) {
                    if ("OK" == msg) {
                        //...(fengpanDBtodo)+
                        $("#Setting_tenant_view").show();
                        $("#Setting_tenant_Edit").hide();
                        Setting_tenant_display();
                        $("#Setting_container_tenant_btn_confirm").unbind();
                        $("#Setting_container_tenant_btn_return").unbind();
		        /*Redmine#440liangjiajie0306*/
                        /*确认和返回的操作都是一样的，此处5句不能分开*/
                    }
                }
            });
        }
        
    }
}
/*************Tenant fengpan****************/
function unicode(s) {
    var len = s.length;
    var rs = "";
    for (var i = 0; i < len; i++) {
        var k = s.substring(i, i + 1);
        rs += (i == 0 ? "" : "%") + s.charCodeAt(i);
    }
    return rs;
}
/********** OBU fengpan **************/
function bind_accunt_button() { //Gao.qb 20140306 #533
    SetBtnToneUp("Edit_accunt_button");
    $("#Edit_accunt_button").click(function () {
        HrefFlag++;
        $("#accunt_info").hide();
        $("#accunt_edit").show();
        accunt_edit_display();
        $("#Edit_accunt_button").unbind();
        $("#accunt_edit_mail").attr("title", LanguageScript.common_NoMailInput);
        $("#accunt_edit_tel").attr("title", LanguageScript.common_NoPhoneInput);
        $("#accunt_edit_old_Password").attr("title", LanguageScript.page_setting_EnterOldPassWord);
        $("#accunt_edit_new_Password").attr("title", LanguageScript.common_PasswordPrompt);
        $("#accunt_edit_confirm_Password").attr("title", LanguageScript.common_PasswordConfirmInput);
    });
}

//var vehicleName = "车辆NO.1";
//var vin = "1G1BL52P7TR115520";
//var vehicleInformation = "2006 本田Element";
//var vehicleLicence = "MN DCE·353";
//var ESN = "6C4C729D";
function Setting_accunt_display() { //Gao.qb 20140306 #533
    var CompanyID = GetCompanyID();
    var roleId = GetRoleID();
    SetBtnToneDown("Edit_accunt_button");
    //if (1 != roleId) {
    //    tenant_tuneDown();
    //}
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetAccunt_info",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#accunt_name").html(msg.username);
            $("#accunt_tel").html(msg.telephone);
            $("#accunt_mail").html(msg.email);
            $("#accunt_mail").attr("title", msg.email);
            bind_accunt_button();
        }
    });
}
function bind_tenant_editBtn(tenant__summary, tenant__email) { //fengpan 20140306 #533
    SetBtnToneUp("Setting_container_tenant_btn_edit");
    $("#Setting_container_tenant_btn_edit").click(function () {
        HrefFlag++;
        $("#Setting_tenant_view").hide();
        $("#Setting_tenant_Edit").show();
        $("#tenant_edit_inputs").html($("#tenant_edit_inputs").html());
        tenant_edit_dis(tenant__summary, tenant__email);//fengpan #533 20140304
        $("#Setting_container_tenant_btn_edit").unbind();
    });
}
function Setting_tenant_display() {
    var tenant__summary = "";//fengpan #533 20140304
    //var tenant__mail = "";
    tenant_tuneDown();//fengpan 20140306 #533
    var CompanyID = GetCompanyID();
    var roleId = GetRoleID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetTenant_info",
        data: {},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#tenant_name")[0].innerHTML = $.trim(msg.companyname);
            $("#tenant_id")[0].innerHTML = $.trim(msg.companyid);
            //$("#tenant_mail")[0].innerHTML = $.trim(msg.email);//ljj
            //tenant__mail = $.trim(msg.email);
            var tempwords = "";
            //var tempemail = tenant__mail.split(';');
            //if (tempemail.length == 1) {
            //    tempwords += tempemail[0] + '\r\n';
            //}
            //else {
            //    for (var i = 0; i < tempemail.length - 1; ++i) {
            //        tempwords += tempemail[i] + '\r\n';
            //    }
            //}
            $("#tenant_tel")[0].innerHTML = $.trim(msg.telephone);
            tenant__summary = $.trim(msg.introduction);//fengpan #533 20140304
            
            if (document.all) {
                document.getElementById("tenant_summary").innerText = tenant__summary;//IE//fengpan #533 20140304
                //document.getElementById("tenant_mail").innerText = tempwords;//IE//fengpan #533 20140304
            }
            else {
                //document.getElementById("tenant_summary").innerText = tenant__summary;
                $("#tenant_summary").text(tenant__summary);//chrome firfox//fengpan #533 20140304
                //$("#tenant_mail").text(tempwords);//chrome firfox//fengpan #533 20140304
            }
            if (1 == roleId) {
                var editwords = "";
                //if (tempemail.length == 1) {
                //    editwords += tempemail[0] + ';\r\n';
                //} else {
                //    for (var i = 0; i < tempemail.length - 1; ++i) {
                //        editwords += tempemail[i] + ';\r\n';
                //    }
                //}
                bind_tenant_editBtn(tenant__summary, editwords);//fengpan 20140306 #533
            }
            //document.getElementById("tenant_mail").title = $.trim(msg.email);//liangjiajie 20140308 #邮件能显示title
        }
    });

}
function tenant_tuneDown() {
    //SetBtnToneDown("Edit_accunt_button");
    SetBtnToneDown("Setting_container_tenant_btn_edit");
}
function runicode(s) {
    var k = s.split("%");
    var rs = "";
    for (i = 0; i < k.length; i++) {
        rs += String.fromCharCode(k[i]);
    }
    return rs;
}

function accunt_edit_display() {
    var accunt_edit_code = '';
    var accunt_emailValue = '';
    var old_Password = '';
    var new_Password = '';
    var confirm_Password = '';
    var accunt_TelValue = '';
    var check_Email = false;
    var check_Tel = true;
    var check_oldpassword = false;
    var check_newpassword = false;
    var chek_confirmpassword = false;
    /*modified by caoyandong************************/
    accunt_edit_code += '<div id="accunt_owner" class="accunt_owner_edit">' + LanguageScript.page_setting_UserAccount + '</div>' +
                '<div id="accunt_name_edit" class="accunt_name_edit_tip ">&nbsp;&nbsp;&nbsp;' + LanguageScript.common_username + "&nbsp;:&nbsp;" + '</div>' +
                '<div id="accunt_name_edit" class="accunt_name_edit">' + $("#accunt_name").html() + '</div>' +
                '<div id="accunt_name_edit" class="accunt_mail_edit_tip">' + LanguageScript.common_Mail + "&nbsp;:&nbsp;" + '</div>' +
                '<div id="accunt_edit_mail_div" class="accunt_edit_mail"><input id="accunt_edit_mail" maxlength="50" style ="font-family:\'Microsoft YaHei\';height:19px;margin-left:-2px;margin-top:-3px;font-size:11pt;  width:86%" type="text" placeholder="' + LanguageScript.common_Mail + '" value = "' + $("#accunt_mail").text() + '" title="' + $("#accunt_mail").text() + '"><div id="accunt_edit_email_empty" class="error_empty" style = "font-color:red">*</div><div id="accunt_edit_email_right" class="right"style="display:none;"></div><div id="accunt_edit_email_error" class="error" style="display:none; left:98.3%"></div></div>' +
                '<div id="accunt_name_edit" class="accunt_tel_edit_tip">' + LanguageScript.common_Phone + "&nbsp;:&nbsp;" + '</div>' +
                '<div id="accunt_edit_tel_div" style="width:27.7%" class="accunt_edit_tel"><input id="accunt_edit_tel" maxlength="15" style ="font-family:\'Microsoft YaHei\';height:19px;margin-left:-2px;margin-top:-2px;font-size:10pt;width:85.6%;"type="text"  placeholder="' + LanguageScript.common_Phone + '" value="' + $("#accunt_tel").text() + '"><div id="accunt_edit_tel_right" class="right"style="display:none;"></div><div id="accunt_edit_tel_error" class="error"style="display:none;"></div></div>' +//输入最大长度15
                '<div id="accunt_name_edit" class="accunt_old_edit_tip">&nbsp;&nbsp;&nbsp;' + LanguageScript.page_setting_OldPassWord + '&nbsp;:&nbsp;</div>' +
                '<div id="accunt_edit_oldPassword_div" style="width:23.7%" class="accunt_edit_old_Password"><input id="accunt_edit_old_Password"  maxlength="20" style="width:100%;height:19px;" type="password" placeholder="' + LanguageScript.page_setting_OldPassWord + '"><div id="accunt_edit_oldPassword_empty" class="error_empty_psw" ></div><div id="accunt_edit_oldPassword_right" class="right_psw"style="display:none;"></div><div id="accunt_edit_oldPassword_error" class="error_psw"style="display:none;"></div></div>' +//输入最大长度20
                '<div id="accunt_name_edit" class="accunt_new_edit_tip">&nbsp;&nbsp;&nbsp;' + LanguageScript.page_setting_NewPassWord + '&nbsp;:&nbsp;</div>' +
                '<div id="accunt_edit_newPassword_div" style="width:23.7%" class="accunt_edit_new_Password"><input id="accunt_edit_new_Password"  maxlength="20" style="width:100%;margin-left:2px;height:19px;" type="password" placeholder="' + LanguageScript.page_setting_NewPassWord + '"><div id="accunt_edit_newPassword_empty" class="error_empty_psw"></div><div id="accunt_edit_newPassword_right" class="right_psw"style="display:none;"></div><div id="accunt_edit_newPassword_error" class="error_psw"style="display:none;"></div></div>' +//输入最大长度20
                '<div id="accunt_name_edit" class="accunt_confirm_edit_tip">' + LanguageScript.common_PasswordConfirm + "&nbsp;:&nbsp;" + '</div>' +
                '<div id="accunt_edit_confirmPassword_div" style="width:23.7%" class="accunt_edit_confirm_Password"><input id="accunt_edit_confirm_Password"  maxlength="20" style="width:100%;margin-left:2px;height:19px;" type="password" placeholder="' + LanguageScript.common_PasswordConfirm + '"><div id="accunt_edit_confirmPassword_empty" class="error_empty_psw"></div><div id="accunt_edit_confirmPassword_right" class="right_psw"style="display:none;"></div><div id="accunt_edit_confirmPassword_error" class="error_psw"style="display:none;"></div></div>';//输入最大长度20
    /*调整tab键选择顺序liangjiajie0308/
    /*modified by caoyandong************************/
    $("#accunt_edit_inputs").html(accunt_edit_code);
    checkEmail($.trim($("#accunt_edit_mail").val()));
    accunt_emailValue = $.trim($("#accunt_edit_mail").val());
    checkTel($.trim($("#accunt_edit_tel").val()));
    accunt_TelValue = $.trim($("#accunt_edit_tel").val());
    //SetBtnToneDown("accunt_edit_confirm");//0301
    /*正则检测Email*/
    $("#accunt_edit_mail").blur(function () {
        accunt_emailValue = document.getElementById("accunt_edit_mail").value;
        checkEmail(accunt_emailValue);
       // if (check_Email == true &&  //0301
       //check_Tel == true &&
       //check_oldpassword == true &&
       //check_newpassword == true &&
       //chek_confirmpassword == true
       // ) {
       //     SetBtnToneUp("accunt_edit_confirm");
       // }
       // else {
       //     SetBtnToneDown("accunt_edit_confirm");
        // }
        $("#email_input_info_left").text("");
    });
    //$("#accunt_edit_mail").mousedown(function () {
    //    $("#accunt_edit_mail_error").remove();
    //    $("#email_input_info_left").text(LanguageScript.common_NoMailInput);
    //    //$("#accunt_edit_mail_empty").remove();
    //    //$("#accunt_edit_mail_right").remove();
    //});
    //$("#accunt_edit_mail").focus(function () {
    //    $("#accunt_edit_mail_error").remove();
    //    $("#email_input_info_left").text(LanguageScript.common_NoMailInput);
    //    //$("#accunt_edit_mail_empty").remove();
    //    //$("#accunt_edit_mail_right").remove();
    //});

    /*用户账户输入信息检测*/
    function checkEmail(emailValue) {
        mail_result = isEmail(emailValue);
        if (0 == mail_result) {//liangjiajie28noinput
            //$("#accunt_edit_mail_div").append('<div id="accunt_edit_mail_empty" class="error_empty" style = "font-color:red">*</div>');
            $("#accunt_edit_email_error").show();
            $("#accunt_edit_email_right").hide();
            check_Email = false;
            return false;
        } else if (-1 == mail_result) {//liangjiajie28wronginput
            $("#accunt_edit_email_error").show();
            $("#accunt_edit_email_right").hide();
            check_Email = false;
            return false;
        }else if( 1 == mail_result ) {
            $("#accunt_edit_email_error").hide();
            $("#accunt_edit_email_right").show();
            check_Email = true;
        }
        return true;
    }

    /*正则检测电话号码*/
    $("#accunt_edit_tel").blur(function () {
        accunt_TelValue = document.getElementById("accunt_edit_tel").value;
        checkTel(accunt_TelValue);
       // if (check_Email == true &&  //0301
       //check_Tel == true &&
       //check_oldpassword == true &&
       //check_newpassword == true &&
       //chek_confirmpassword == true
       // ) {
       //     SetBtnToneUp("accunt_edit_confirm");
       // } else {
       //     SetBtnToneDown("accunt_edit_confirm");
        // }
        //$("#tel_input_info_left").text("");
    });
    //$("#accunt_edit_tel").mousedown(function () {
    //    $("#accunt_edit_tel_error").remove();
    //    //$("#accunt_edit_tel_right").remove();
    //    $("#tel_input_info_left").text(LanguageScript.common_NoPhoneInput);
    //});
    //$("#accunt_edit_tel").focus(function () {
    //    $("#accunt_edit_tel_error").remove();
    //    //$("#accunt_edit_tel_right").remove();
    //    $("#tel_input_info_left").text(LanguageScript.common_NoPhoneInput);
    //});
    function checkTel(TelValue) {
        tel_result = isPhone(TelValue)
        if ( -1 == tel_result ) {//电话号码错误
            //$("#accunt_edit_tel_right").remove();
            //$("#accunt_edit_tel_error").remove();
            $("#accunt_edit_tel_error").show();
            $("#accunt_edit_tel_right").hide();
            check_Tel = false;
            //alert("您输入的号码有误,请重新核对后再输入!");
            //document.getElementById("accunt_edit_tel").focus();
            return false;
        }
        else if( 1 == tel_result ){//电话号码正确
            //$("#accunt_edit_tel_right").remove();
            //$("#accunt_edit_tel_error").remove();
            $("#accunt_edit_tel_error").hide();
            $("#accunt_edit_tel_right").show();
            check_Tel = true;
        } else if (0 == tel_result) {//电话号码为空
            check_Tel = true;
        }
        return check_Tel;
    }
    /*验证旧密码*/
    function check_old_Password(){//封装原密码函数liangjiajie0312
    //$("#accunt_edit_old_Password").blur(function () {
        //$("#old_input_info_right").text("");
        //...get user pkid; 
        //to do
        var accunt_name = $("#accunt_name").html();
        old_Password = document.getElementById("accunt_edit_old_Password").value;
        if ("" == old_Password) {//liangjiajie28
            if ($("#accunt_edit_new_Password").val() == "" && $("#accunt_edit_confirm_Password").val() == "") {
                check_oldpassword = true;
                return true;
            } else {
                check_oldpassword = false;
                return false;
            }
            //$("#accunt_edit_oldPassword_div").append('<div id="accunt_edit_oldPassword_error" class="error"></div>');
            
        }

        old_Password = accunt_name.toLocaleLowerCase() + '&' + old_Password;
        var MD5Password = hex_md5($.trim(old_Password));
        $.ajax({
            type: "POST",
            url: "/" + CompanyID + "/Setting/Check_Accunt_password",
            data: { password: MD5Password },
            contentType: "application/x-www-form-urlencoded",
            dataType: "text",
            success: function (msg) {
                if ("OK" == msg) {
                    //...(fengpanDBtodo)+
                    $("#accunt_edit_oldPassword_error").hide();
                    $("#accunt_edit_oldPassword_right").show();
                    check_oldpassword = true;
                }
                else if ("NG" == msg) {
                    $("#accunt_edit_oldPassword_error").show();
                    $("#accunt_edit_oldPassword_right").hide();
                    check_oldpassword = false;
                }
            }
        });
    };

        //$("#accunt_edit_old_Password").mousedown(function () {
        //    $("#accunt_edit_oldPassword_error").remove();
        //    $("#old_input_info_right").text(LanguageScript.page_setting_EnterOldPassWord);
        //});
        //$("#accunt_edit_old_Password").focus(function () {
        //    $("#accunt_edit_oldPassword_error").remove();
        //    $("#old_input_info_right").text(LanguageScript.page_setting_EnterOldPassWord);
        //});
        $("#accunt_edit_old_Password").blur(function () {
            check_old_Password();
            check_new_Password();
            check__confirm_Password();
        });


    /*一次输入新密码*/
        function check_new_Password(){//封装新密码函数liangjiajie0312
        //$("#accunt_edit_new_Password").blur(function () {
            //$("#new_input_info_right").text("");
            //密码检测规则变更20140329 liangjiajie
            var reg = /^[a-zA-Z0-9\_\%\^\(\)\-\+\=\~\@\$\!\*\&\#\,\.]{6,20}$/;
            var numReg = /[0-9]/;
            //var reg = /^\w{6,20}$/;
            new_Password = $("#accunt_edit_new_Password").val();
           
            //confirm_Password = $("#accunt_edit_confirm_Password").val();

            //$("#accunt_edit_newPassword_empty").remove();//liangjiajie28

            if ("" == new_Password) {//liangjiajie28
                //$("#accunt_edit_newPassword_div").append('<div id="accunt_edit_newPassword_empty" class="error_empty" style = "font-color:red">*</div>');
                check_newpassword = false;
                return false;
            }
            if (reg.test(new_Password) && numReg.test(new_Password)) {
                //$("#accunt_edit_newPassword_right").remove();
                //$("#accunt_edit_newPassword_error").remove();
                //$("#accunt_edit_newPassword_empty").remove();//liangjiajie28
                $("#accunt_edit_newPassword_error").hide();
                $("#accunt_edit_newPassword_right").show();
                check_newpassword = true;
                return true;
            }
            else {
                //$("#accunt_edit_newPassword_right").remove();
                //$("#accunt_edit_newPassword_error").remove();
                //$("#accunt_edit_newPassword_empty").remove();//liangjiajie28
                $("#accunt_edit_newPassword_error").show();
                $("#accunt_edit_newPassword_right").hide();
                check_newpassword = false;
                return false;
            }
            // if (check_Email == true &&
            //check_Tel == true &&
            //check_oldpassword == true &&
            //check_newpassword == true &&
            //chek_confirmpassword == true
            // ) { } else if ("" != confirm_Password) {
            //     if (new_Password != confirm_Password)
            //     SetBtnToneDown("accunt_edit_confirm");
            // }
            //if ("" != confirm_Password) {
            //    if (new_Password != confirm_Password) {
            //        $("#accunt_edit_newPassword_right").remove();
            //        $("#accunt_edit_newPassword_error").remove();
            //        $("#accunt_edit_newPassword_div").append('<div id="accunt_edit_newPassword_error" class="error"></div>');
            //        //SetBtnToneDown("accunt_edit_confirm");
            //    } else {
            //        //SetBtnToneUp("accunt_edit_confirm");
            //    }
            //}
        };


        //$("#accunt_edit_new_Password").mousedown(function () {
        //    $("#accunt_edit_newPassword_right").remove();
        //    $("#accunt_edit_newPassword_error").remove();
        //    $("#new_input_info_right").text(LanguageScript.common_PasswordPrompt);
        //});
        //$("#accunt_edit_new_Password").focus(function () {
        //    $("#accunt_edit_newPassword_right").remove();
        //    $("#accunt_edit_newPassword_error").remove();
        //    $("#new_input_info_right").text(LanguageScript.common_PasswordPrompt);
        //});
        $("#accunt_edit_new_Password").blur(function () {
            check_old_Password();
            check_new_Password();
            check__confirm_Password();
        });

    /*验证新密码二次输入*/
        function check__confirm_Password(){//liangjiajie封装验证密码函数
        //$("#accunt_edit_confirm_Password").blur(function () {
            //$("#confirm_input_info_right").text("");
            new_Password = $("#accunt_edit_new_Password").val();
            confirm_Password = $("#accunt_edit_confirm_Password").val();

            //$("#accunt_edit_confirmPassword_empty").remove();//liangjiajie28

            //if ("" == confirm_Password) {//liangjiajie28
            //    $("#accunt_edit_confirmPassword_div").append('<div id="accunt_edit_confirmPassword_empty" class="error_empty" style = "font-color:red">*</div>');
            //    chek_confirmpassword = false;
            //    return false;
            //}
            //加密todo

            if ( confirm_Password == "") {
                chek_confirmpassword = false;
                return false;
            }

            if (new_Password == confirm_Password && confirm_Password != "") {
                //...DB todo
                //$("#accunt_edit_confirmPassword_right").remove();
                //$("#accunt_edit_confirmPassword_error").remove();
                //$("#accunt_edit_confirmPassword_empty").remove();//liangjiajie28
                $("#accunt_edit_confirmPassword_error").hide();
                $("#accunt_edit_confirmPassword_right").show();
                chek_confirmpassword = true;
                return true;
            }
            else {
                //$("#accunt_edit_confirmPassword_right").remove();
                //$("#accunt_edit_confirmPassword_error").remove();
                //$("#accunt_edit_confirmPassword_empty").remove();//liangjiajie28
                $("#accunt_edit_confirmPassword_error").show();
                $("#accunt_edit_confirmPassword_right").hide();
                chek_confirmpassword = false;
                return false;
                //alert("输入密码不一致！");
                //...
            }
            // if (check_Email == true &&//0301
            //check_Tel == true &&
            //check_oldpassword == true &&
            //check_newpassword == true &&
            //chek_confirmpassword == true
            // ) {
            //     SetBtnToneUp("accunt_edit_confirm");
            // } else {
            //     SetBtnToneDown("accunt_edit_confirm");
            // }
        };


        //$("#accunt_edit_confirm_Password").mousedown(function () {
        //    $("#accunt_edit_confirmPassword_right").remove();
        //    $("#accunt_edit_confirmPassword_error").remove();
        //    $("#confirm_input_info_right").text(LanguageScript.common_PasswordConfirmInput);
        //});
        //$("#accunt_edit_confirm_Password").focus(function () {
        //    $("#accunt_edit_confirmPassword_right").remove();
        //    $("#accunt_edit_confirmPassword_error").remove();
        //    $("#confirm_input_info_right").text(LanguageScript.common_PasswordConfirmInput);
        //});
        $("#accunt_edit_confirm_Password").blur(function () {
            check_old_Password();
            check_new_Password();
            check__confirm_Password();
        });


        var CompanyID = GetCompanyID();

        $("#accunt_edit_confirm").click(function () {
            var accunt_name = $("#accunt_name").html();
            new_Password = accunt_name.toLocaleLowerCase() + '&' + new_Password;
            var MD5Password = hex_md5($.trim(new_Password));
            var psw_old = $.trim(document.getElementById("accunt_edit_old_Password").value);
            var psw_new = $.trim($("#accunt_edit_new_Password").val());
            var psw_confirm = $.trim($("#accunt_edit_confirm_Password").val());
            if (check_Email == true 
		        && check_Tel == true 
		        && psw_old  == ""
                && psw_new == ""
                && psw_confirm == ""
                ) {
                $.ajax({
                    type: "POST",
                    url: "/" + CompanyID + "/Setting/EditAccunt_infoExpPsw",
                    data: { email: accunt_emailValue, tel: accunt_TelValue },
                    contentType: "application/x-www-form-urlencoded",
                    dataType: "text",
                    success: function (msg) {
                        if ("OK" == msg) {
                            //...
                            $("#accunt_edit_confirm").unbind();
                            $("#accunt_edit_return").unbind();
                            $("#accunt_info").show();
                            $("#accunt_edit").hide();
                            Setting_accunt_display();
                            HrefFlag--;
                        }
                    }
                });
            } else {
                if (check_Email == true &&
                   check_Tel == true &&
                   check_oldpassword == true &&
                   check_newpassword == true &&
                   chek_confirmpassword == true
                    ) {
                    //$("#accunt_mail").html(accunt_emailValue);
                    //$("#accunt_tel").html(accunt_TelValue);
                    //var CompanyID = GetCompanyID();
                    $.ajax({
                        type: "POST",
                        url: "/" + CompanyID + "/Setting/EditAccunt_info",
                        data: { password: MD5Password, email: accunt_emailValue, tel: accunt_TelValue },
                        contentType: "application/x-www-form-urlencoded",
                        dataType: "text",
                        success: function (msg) {
                            if ("OK" == msg) {
                                //...
                                $("#accunt_edit_confirm").unbind();
                                $("#accunt_edit_return").unbind();
                                $("#accunt_info").show();
                                $("#accunt_edit").hide();
                                Setting_accunt_display();
                                HrefFlag--;
                            }
                        }
                    });
                    //$("#accunt_info").show();
                    //$("#accunt_edit").hide();
                    //Setting_accunt_display();
                }
                else {
                    if (psw_old == "" && psw_new == "" && psw_confirm == "") {
                        $("#accunt_edit_oldPassword_error").hide();
                        $("#accunt_edit_oldPassword_right").hide();
                        $("#accunt_edit_newPassword_error").hide();
                        $("#accunt_edit_newPassword_right").hide();
                        $("#accunt_edit_confirmPassword_error").hide();
                        $("#accunt_edit_confirmPassword_right").hide();
                    } else {
                        if (false == check_oldpassword) {
                            //$("#accunt_edit_errorInfo_div").show();
                            $("#accunt_edit_oldPassword_error").show();
                            $("#accunt_edit_oldPassword_right").hide();
                        } else {
                            $("#accunt_edit_oldPassword_error").hide();
                            $("#accunt_edit_oldPassword_right").show();
                        }
                        if (false == check_newpassword) {
                            $("#accunt_edit_newPassword_error").show();
                            $("#accunt_edit_newPassword_right").hide();
                        } else {
                            $("#accunt_edit_newPassword_error").hide();
                            $("#accunt_edit_newPassword_right").show();
                        }
                        if (false == chek_confirmpassword) {
                            $("#accunt_edit_confirmPassword_error").show();
                            $("#accunt_edit_confirmPassword_right").hide();
                        } else {
                            $("#accunt_edit_confirmPassword_error").hide();
                            $("#accunt_edit_confirmPassword_right").show();
                        }
                    }
                    if (false == check_Email) {
                        $("#accunt_edit_email_error").show();
                        $("#accunt_edit_email_right").hide();
                    } else {
                        $("#accunt_edit_email_error").hide();
                        $("#accunt_edit_email_right").show();
                    }
                }
            }

        });
        $("#accunt_edit_return").click(function () {
            $("#accunt_info").show();
            $("#accunt_edit").hide();
            $("#accunt_edit_confirm").unbind();
            $("#accunt_edit_return").unbind();
            Setting_accunt_display();
            HrefFlag--;
        });
}
//4TAB  切换
function Set_tenant() {

    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        WaitingTAB = Set_tenant;
        Set_href();
        return;
    }
    vehicleId = "-1";
    Height_User(8, 50);
    $("#Setting_tenant").removeClass("setting_img_bg_normal");
    $("#Setting_tenant").removeClass("setting_img_bg_disable");
    $("#Setting_user").removeClass("setting_img_bg_disable");
    $("#Setting_OBU").removeClass("setting_img_bg_disable");
    $("#Setting_Group").removeClass("setting_img_bg_disable");
    $("#Setting_Alert").removeClass("setting_img_bg_disable");

    $("#Setting_Alert").addClass("setting_img_bg_disable");
    $("#Setting_tenant").addClass("setting_img_bg_normal");
    $("#Setting_user").addClass("setting_img_bg_disable");
    $("#Setting_OBU").addClass("setting_img_bg_disable");
    $("#Setting_Group").addClass("setting_img_bg_disable");

    $("#Setting_container_tenant").show();
    $("#Setting_container_user").hide();
    $("#Setting_container_obu").hide();
    $("#Setting_container_group").hide();
    $("#Setting_container_alert").hide();

    $("#Setting_tenant_view").show();
    $("#Setting_tenant_Edit").hide();
    Setting_tenant_display();

}
function Set_user() {
    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        WaitingTAB = Set_user;
        Set_href();
        return;
    }
    
    vehicleId = "-1";
    Height_User(8, 50);
    $("#Setting_tenant").removeClass("setting_img_bg_normal");
    $("#Setting_user").removeClass("setting_img_bg_disable");
    $("#Setting_OBU").removeClass("setting_img_bg_disable");
    $("#Setting_Group").removeClass("setting_img_bg_disable");
    $("#Setting_Alert").removeClass("setting_img_bg_disable");

    $("#Setting_Alert").addClass("setting_img_bg_disable");
    $("#Setting_tenant").addClass("setting_img_bg_disable");
    $("#Setting_user").addClass("setting_img_bg_normal");
    $("#Setting_OBU").addClass("setting_img_bg_disable");
    $("#Setting_Group").addClass("setting_img_bg_disable");
    $("#Setting_container_user_list").empty();
    $("#user_edit_sure").remove();
    $("#user_edit_back").remove();
    
    $("#Setting_container_tenant").hide();
    $("#Setting_container_user").show();
    $("#Setting_container_obu").hide();
    $("#Setting_container_group").hide();
    $("#Setting_container_alert").hide();
    unbind_event();
    $("#pageBar").attr("successflag", "false");
    getUserData(pagenum_user);
}
function Set_OBU() {
    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        WaitingTAB = Set_OBU;
        Set_href();
        return;
    }
    Height_User(8,50);
    unbind_OBU_editANDadd();
    $("#Setting_OBU_table").html('');
    $("#Setting_container_OBU_btn_return").unbind();
    $("#Setting_container_OBU_btn_confirm").unbind();
    $("#Setting_container_OBU_btn_confirm").hide();
    $("#Setting_container_OBU_btn_return").hide();
    $("#Setting_tenant").removeClass("setting_img_bg_normal");
    $("#Setting_user").removeClass("setting_img_bg_disable");
    $("#Setting_OBU").removeClass("setting_img_bg_disable");
    $("#Setting_Group").removeClass("setting_img_bg_disable");
    $("#Setting_Alert").removeClass("setting_img_bg_disable");

    $("#Setting_Alert").addClass("setting_img_bg_disable");
    $("#Setting_tenant").addClass("setting_img_bg_disable");
    $("#Setting_user").addClass("setting_img_bg_disable");
    $("#Setting_OBU").addClass("setting_img_bg_normal");
    $("#Setting_Group").addClass("setting_img_bg_disable");

    getObuData("-1");
    $("#Setting_container_tenant").hide();
    $("#Setting_container_user").hide();
    $("#Setting_container_obu").show();
    $("#Setting_container_group").hide();
    $("#Setting_container_alert").hide();
}
function Set_group() {
    group_notadd_id = null;//#750 liangjiajie20140401
    group_add_id = null;//#750 liangjiajie20140401
    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        WaitingTAB = Set_group;
        Set_href();
        return;
    }
    vehicleId = "-1";
    Height_User(8, 50);
    $("#Setting_tenant").removeClass("setting_img_bg_normal");
    $("#Setting_user").removeClass("setting_img_bg_disable");
    $("#Setting_OBU").removeClass("setting_img_bg_disable");
    $("#Setting_Group").removeClass("setting_img_bg_disable");
    $("#Setting_Alert").removeClass("setting_img_bg_disable");

    $("#Setting_Alert").addClass("setting_img_bg_disable");
    $("#Setting_tenant").addClass("setting_img_bg_disable");
    $("#Setting_user").addClass("setting_img_bg_disable");
    $("#Setting_OBU").addClass("setting_img_bg_disable");
    $("#Setting_Group").addClass("setting_img_bg_normal");


    $("#group_add_group").show();
    $("#group_edit_group").show();
    $("#group_del_group").show();

    $("#group_edit_sure").remove();
    $("#group_edit_back").remove();

    $("#li_add").remove();
    $("#group_group").empty();
    $("#group_added").empty();//#756 清空已有车辆列表liangjiajie0319
    $("#group_notadded").empty();//#756 清空未添加车辆列表liangjiajie0319

    $("#Setting_container_tenant").hide();
    $("#Setting_container_user").hide();
    $("#Setting_container_obu").hide();
    $("#Setting_container_group").show();
    $("#Setting_container_alert").hide();
    Group_ToneDown();
    //获取group数据
    GetGroup();
    
}
//设置alert阀值 fengpan20140308 #638
function Set_alert() {
    //if (HrefFlag == 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
    if (HrefFlag >= 2 || (HrefFlag == 1 && $("#accunt_edit")[0].style.display == "none")) {
        WaitingTAB = Set_alert;
        Set_href();
        return;
    }
    Height_User(8);
    $("#Setting_tenant").removeClass("setting_img_bg_normal");
    $("#Setting_tenant").removeClass("setting_img_bg_disable");
    $("#Setting_user").removeClass("setting_img_bg_disable");
    $("#Setting_OBU").removeClass("setting_img_bg_disable");
    $("#Setting_Group").removeClass("setting_img_bg_disable");
    $("#Setting_Alert").removeClass("setting_img_bg_disable");

    $("#Setting_Alert").addClass("setting_img_bg_normal");
    $("#Setting_tenant").addClass("setting_img_bg_disable");
    $("#Setting_user").addClass("setting_img_bg_disable");
    $("#Setting_OBU").addClass("setting_img_bg_disable");
    $("#Setting_Group").addClass("setting_img_bg_disable");

    $("#Setting_container_tenant").hide();
    $("#Setting_container_user").hide();
    $("#Setting_container_obu").hide();
    $("#Setting_container_group").hide();
    $("#Setting_container_alert").show();

    alert_disp();
}
/*fengpan alert阀值 start*/
function alert_disp() {
    unbind_alert_threshold();

    $("#speed_error_info").hide();//fengpan 20140318 #773
    $("#prm_error_info").hide();
    $("#motion_error_info").hide();
    $("#prm_error_info_time").hide();
    //fengpan 20140703
    $("#tenant_edit_mail_red").hide();
    $("#tenant_mail_response").hide();
    $("#tenant_mail_response").text("");
    

    $("#Setting_container_alert_btn_edit").show();
    $("#Setting_container_alert_btn_confirm").hide();
    $("#Setting_container_alert_btn_cancle").hide();

    $("#speed_threshold").show();
    $("#speed_threshold_edit").hide();
    $("#prm_threshold").show();
    $("#prm_threshold_edit").hide();
    $("#prm_threshold_time").show();
    $("#prm_threshold_edit_time").hide();
    $("#motion_threshold").show();
    $("#motion_threshold_edit").hide();
    //fengpan 20140703
    $("#tenant_mail_edit").show();
    $("#Setting_tenant_edit_mail").hide();

    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetAlertThresholdsInfo",//fengpan 20140318 #773
        data: {},
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            //var obj = eval(msg);
            //$("#speed_threshold_value").text(obj[0].speed);
            //$("#prm_threshold_value").text(obj[0].rpm);
            //$("#prm_threshold_value_time").text(obj[0].rpmTime);
            //$("#motion_threshold_value").text(obj[0].motion);
            if (null == msg.speed)
            {
                msg.speed = 0;
            }
            if (null == msg.rpm) {
                msg.rpm = 0;
            }
            if (null == msg.rpmDuration) {
                msg.rpmDuration = 0;
            }
            if (null == msg.motion) {
                msg.motion = 0;
            }
            $("#speed_threshold_value").text(msg.speed);
            $("#prm_threshold_value").text(msg.rpm);
            $("#prm_threshold_value_time").text(msg.rpmDuration);
            $("#motion_threshold_value").text(msg.motion);

            var tenant_mail = "";
            tenant_mail = $.trim(msg.alertEmails);
            var tempwords = "";
            var tempemail = tenant_mail.replace(/\n/g, "").split(';');
            for (var i = 0; i < tempemail.length; ++i) {
                if (tempemail[i] != "") {
                    if (i != tempemail.length - 1) {
                        tempwords += tempemail[i] + '\r\n';
                    }
                    else {
                        tempwords += tempemail[i];
                    }
                }
            }
            var editwords = "";
            for (var i = 0; i < tempemail.length; ++i) {
                if (tempemail[i] != "") {
                    if (i != tempemail.length - 1) {
                        editwords += tempemail[i] + ';\r\n';
                    }
                    else {
                        editwords += tempemail[i] + ';';
                    }
                }
            }
            //if (document.all) {
            //    document.getElementById("tenant_mail").innerText = tempwords;//IE//fengpan #533 20140304
            //    document.getElementById("Setting_tenant_edit_mail").innerText = editwords;//IE//fengpan #533 20140304
            //}
            //else {
            //    $("#tenant_mail").text(tempwords);//chrome firfox//fengpan #533 20140304
            //    $("#Setting_tenant_edit_mail").text(editwords);//chrome firfox//fengpan #533 20140304
            //}
            $("#tenant_mail").val(tempwords);
            $("#alertEmailsBackInfo").val(editwords);

            var roleID = GetRoleID();
            if (1 == roleID) {
                SetBtnToneUp("Setting_container_alert_btn_edit");
                bind_alert_threshold();
            } else {
                //...
            }
        }
    });
}
function bind_alert_threshold() {
    $("#Setting_container_alert_btn_edit").click(function () {
        $("#Setting_container_alert_btn_confirm").unbind();//fengpan 20140324 #863
        $("#Setting_container_alert_btn_cancle").unbind();//fengpan 20140324 #863
        confirm_cancle_click();
        $("#Setting_container_alert_btn_edit").unbind();

        $("#speed_threshold").hide();
        $("#speed_threshold_edit").show();
        $("#speed_threshold_input").val($("#speed_threshold_value").text());

        $("#prm_threshold").hide();
        $("#prm_threshold_edit").show();
        $("#prm_threshold_input").val($("#prm_threshold_value").text());

        $("#prm_threshold_time").hide();
        $("#prm_threshold_edit_time").show();
        $("#prm_threshold_input_time").val($("#prm_threshold_value_time").text());

        $("#motion_threshold").hide();
        $("#motion_threshold_edit").show();
        $("#motion_threshold_input").val($("#motion_threshold_value").text());

        //fengpan 20140703
        $("#tenant_mail_edit").hide();
        var editwords = $("#alertEmailsBackInfo").val();
        //if (document.all) {
        //    document.getElementById("Setting_tenant_edit_mail").innerText = editwords;//IE//fengpan #533 20140304
        //}
        //else {
        //    $("#Setting_tenant_edit_mail").text(editwords);//chrome firfox//fengpan #533 20140304
        //    $("#Setting_tenant_edit_mail").val(editwords);//chrome firfox//fengpan #533 20140304
        //}
        $("#Setting_tenant_edit_mail").val(editwords);
        $("#Setting_tenant_edit_mail").show();
        $("#tenant_edit_mail_red").show();
        $("#tenant_mail_response").show();

        $("#Setting_container_alert_btn_edit").hide();
        $("#Setting_container_alert_btn_confirm").show();
        $("#Setting_container_alert_btn_cancle").show();
        HrefFlag++;
    });
    $("#speed_threshold_input").mousedown(function () {
        $("#speed_error_info").hide();
        $("#speed_info").show();
    });
    $("#speed_threshold_input").focus(function () {
        $("#speed_error_info").hide();
        $("#speed_info").show();
    });
    $("#speed_threshold_input").blur(function () {
        $("#speed_info").hide();
        if (checkSpeed()) {
            $("#speed_error_info").hide();
        } else {
            $("#speed_error_info").show();
            return;
        }
    });

    $("#prm_threshold_input").mousedown(function () {
        $("#prm_error_info").hide();
        $("#prm_info").show();
    });
    $("#prm_threshold_input").focus(function () {
        $("#prm_error_info").hide();
        $("#prm_info").show();
    });
    $("#prm_threshold_input").blur(function () {
        $("#prm_info").hide();
        if (checkRpm()) {
            $("#prm_error_info").hide();
        } else {
            $("#prm_error_info").show();
            return;
        }
    });

    $("#prm_threshold_input_time").mousedown(function () {
        $("#prm_error_info_time").hide();
        $("#prm_info_time").show();
    });
    $("#prm_threshold_input_time").focus(function () {
        $("#prm_error_info_time").hide();
        $("#prm_info_time").show();
    });
    $("#prm_threshold_input_time").blur(function () {
        $("#prm_info_time").hide();
        if (checkRpmTime()) {
            $("#prm_error_info_time").hide();
        } else {
            $("#prm_error_info_time").show();
            return;
        }
    });

    $("#motion_threshold_input").mousedown(function () {
        $("#motion_error_info").hide();
        $("#motion_info").show();
    });
    $("#motion_threshold_input").focus(function () {
        $("#motion_error_info").hide();
        $("#motion_info").show();
    });
    $("#motion_threshold_input").blur(function () {
        $("#motion_info").hide();
        if (checkMotion()) {
            $("#motion_error_info").hide();
        } else {
            $("#motion_error_info").show();
            return;
        }
    });

    //fengpan 20140703
    $("#Setting_tenant_edit_mail").blur(function () {
        var emailValue = document.getElementById("Setting_tenant_edit_mail").value;
        tenant_check_mail(emailValue);
    });

    /*显示与隐藏电子邮件输入提示*/
    $("#Setting_tenant_edit_mail").mousedown(function () {
        //$("#tenant_mail_response").show();    //Redmine#440liangjiajie0306常显示 注释
        $("#tenant_mail_response").text(LanguageScript.common_NoticeMailPrompt);
        $("#tenant_mail_response").css("color", "#aaa");
    });
    $("#Setting_tenant_edit_mail").focus(function () {
        //$("#tenant_mail_response").show();    //Redmine#440liangjiajie0306常显示 注释
        $("#tenant_mail_response").text(LanguageScript.common_NoticeMailPrompt);
        $("#tenant_mail_response").css("color", "#aaa");
    });
}
function unbind_alert_threshold() {
    SetBtnToneDown("Setting_container_alert_btn_edit");
}
function confirm_cancle_click() {
    var CompanyID = GetCompanyID();
    $("#Setting_container_alert_btn_confirm").click(function () {
        if (checkSpeed() && checkRpm() && checkRpmTime() && checkMotion() && tenant_check_mail()) {
            //...
            $("#speed_threshold_value").text($.trim($("#speed_threshold_input").val()));
            $("#prm_threshold_value").text($.trim($("#prm_threshold_input").val()));
            $("#prm_threshold_value_time").text($.trim($("#prm_threshold_input_time").val()));
            $("#motion_threshold_value").text($.trim($("#motion_threshold_input").val()));
            //var emailsChangeFlag = 0;
            var tenant_email_input = $.trim($("#hideRemindEmail").val());
            var tempwords = "";
            var tempemail = tenant_email_input.replace(/\n/g, "").split(';');
            for (var i = 0; i < tempemail.length; ++i) {
                if (tempemail[i] != "") {
                    if (i != tempemail.length - 1) {
                        tempwords += tempemail[i] + '\r\n';
                    }
                    else {
                        tempwords += tempemail[i];
                    }
                }
            }
            var editwords = "";
            for (var i = 0; i < tempemail.length; ++i) {
                if (tempemail[i] != "") {
                    if (i != tempemail.length - 1) {
                        editwords += tempemail[i] + ';\r\n';
                    }
                    else {
                        editwords += tempemail[i] + ';';
                    }
                }
            }
            //if (document.all) {
            //    document.getElementById("tenant_mail").innerText = tempwords;//IE//fengpan #533 20140304
            //    document.getElementById("Setting_tenant_edit_mail").innerText = editwords;//IE//fengpan #533 20140304
            //}
            //else {
            //    $("#tenant_mail").text(tempwords);//chrome firfox//fengpan #533 20140304
            //    $("#Setting_tenant_edit_mail").text(editwords);//chrome firfox//fengpan #533 20140304
            //}
            $("#tenant_mail").val(tempwords);
            $("#Setting_tenant_edit_mail").val(editwords);

            $("#alertEmailsBackInfo").val(editwords);

            $.ajax({
                type: "POST",
                url: "/" + CompanyID + "/Setting/EditAlertThresholds",
                data: { alertEmails: tenant_email_input, speedThreshold: parseInt($.trim($("#speed_threshold_input").val())), rpmThreshold: parseInt($.trim($("#prm_threshold_input").val())), rpmTimeThreshold: parseInt($.trim($("#prm_threshold_input_time").val())), motionThreshold: $.trim($("#motion_threshold_input").val())},
                contentType: "application/x-www-form-urlencoded",
                dataType: "text",
                success: function (msg) {
                    $("#loadingImg").html('');
                    $("#loadingImg").css("z-index", "-1");
                    if ("OK" == msg) {
                        //...todo
                        //$("#speed_threshold_value").text($.trim($("#speed_threshold_input").val()));
                        //$("#prm_threshold_value").text($.trim($("#prm_threshold_input").val()));
                        //$("#prm_threshold_value_time").text($.trim($("#prm_threshold_input_time").val()));
                        //$("#motion_threshold_value").text($.trim($("#motion_threshold_input").val()));
                    }
                    else if("NG" == msg) {
                        //alert("error");
                        user_dialog_error(LanguageScript.page_setting_setThresholdFiled);
                        alert_disp();
                    }
                    HrefFlag--;
                },
                beforeSend:function(){
                    $("#loadingImg").html('<img style="position:relative;top:110px;width:65px;" src="../../../Content/Common/images/loading_style.gif"/>');
                    $("#loadingImg").css("z-index", "1");
                },
                error: function () {
                    $("#loadingImg").html('');
                    $("#loadingImg").css("z-index", "-1");
                    user_dialog_error(LanguageScript.page_setting_setThresholdFiled);
                    HrefFlag--;
                }
            });
        } else {
            return;
        }
        //HrefFlag--;
        $("#speed_threshold").show();
        $("#speed_threshold_edit").hide();

        $("#prm_threshold").show();
        $("#prm_threshold_edit").hide();

        $("#prm_threshold_time").show();
        $("#prm_threshold_edit_time").hide();

        $("#motion_threshold").show();
        $("#motion_threshold_edit").hide();

        //fengpan
        $("#tenant_mail_edit").show();
        $("#Setting_tenant_edit_mail").hide();
        $("#tenant_edit_mail_red").hide();
        $("#tenant_mail_response").hide();
        $("#tenant_mail_response").text("");

        $("#Setting_container_alert_btn_edit").show();
        $("#Setting_container_alert_btn_confirm").hide();
        $("#Setting_container_alert_btn_cancle").hide();
        bind_alert_threshold();
    });
    $("#Setting_container_alert_btn_cancle").click(function () {
        //HrefFlag--; fengpan 20140321
        $("#speed_error_info").hide();
        $("#prm_error_info").hide();
        $("#motion_error_info").hide();
        $("#prm_error_info_time").hide();

        $("#speed_threshold").show();
        $("#speed_threshold_edit").hide();

        $("#prm_threshold").show();
        $("#prm_threshold_edit").hide();

        $("#prm_threshold_time").show();
        $("#prm_threshold_edit_time").hide();

        $("#motion_threshold").show();
        $("#motion_threshold_edit").hide();

        //fengpan
        $("#tenant_mail_edit").show();
        $("#Setting_tenant_edit_mail").val($("#alertEmailsBackInfo").val());
        $("#Setting_tenant_edit_mail").hide();
        $("#tenant_edit_mail_red").hide();
        $("#tenant_mail_response").hide();
        $("#tenant_mail_response").text("");

        $("#Setting_container_alert_btn_edit").show();
        $("#Setting_container_alert_btn_confirm").hide();
        $("#Setting_container_alert_btn_cancle").hide();
        bind_alert_threshold();
        HrefFlag--;
    });
}
function checkSpeed() {
    var reg = /^[0-9]{1,3}$/;
    var speedValue = $.trim($("#speed_threshold_input").val());
    if (reg.test(speedValue)) {
        if (5 <= parseInt(speedValue) && 255 >= parseInt(speedValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
function checkRpm() {
    var reg = /^[0-9]{1,5}$/;
    var rpmValue = $.trim($("#prm_threshold_input").val());
    if (reg.test(rpmValue)) {
        if (0 <= parseInt(rpmValue) && 65534 >= parseInt(rpmValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
function checkRpmTime() {
    var reg = /^[0-9]{1,3}$/;
    var rpmTimeValue = $.trim($("#prm_threshold_input_time").val());
    if (reg.test(rpmTimeValue)) {
        if (0 <= parseInt(rpmTimeValue) && 100 >= parseInt(rpmTimeValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
function checkMotion() {
    //var reg = /^([0-9][\.][0-9]{1,5})|[0-9]{1}$/;//fengpan 20140318 #773
    var reg = /^\d+(|(\.[0-9]{1,5}))$/;//fengpan 20140324 #773
    var motionValue = $.trim($("#motion_threshold_input").val());
    if (reg.test(motionValue)) {
        if (0.0 <= parseFloat(motionValue) && 3.98565 >= parseFloat(motionValue)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
function tenant_check_mail(emailValue) {
    var email = $.trim(emailValue);

    //fengpan 20140703
    //if (document.all) {
    //    email = $.trim(document.getElementById("Setting_tenant_edit_mail").innerText);//IE//fengpan #533 20140304
    //}
    //else {
    //    email = $.trim($("#Setting_tenant_edit_mail").text());//chrome firfox//fengpan #533 20140304
    //}
    email = $.trim($("#Setting_tenant_edit_mail").val());
    //fengpan20140228
    
    var reg = /^([a-zA-Z0-9_\-\.]){1,}@([a-z0-9A-Z]?[a-z0-9A-Z]+)+(((\.\w+))*)+[\.][a-z]{2,4}$/;  //liangjiajie
    var arremail = email.split(';');
    if (0 == arremail.length || 1 == arremail.length) {
        if (1 == arremail.length && arremail[0] != "") {
            email = $.trim(arremail[0]);
            email = email.replace(/\n/g, "");
        }
        if ("" == email) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.page_tenant_EnterNoticeMail);
            $("#tenant_mail_response").css("color", "red");
            check_tenant_mail = false;
            return false;
        } else if (!reg.test(email)) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.common_CorrectFormatMail);
            $("#tenant_mail_response").css("color", "red");
            //$("#tenant_mail_response").show();    //Redmine#440liangjiajie0306常显示 注释
            check_tenant_mail = false;
            return false;
        } else if (email.length > 50) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.error_e01246);
            $("#tenant_mail_response").css("color", "red");
            check_tenant_mail = false;
            return false;
        } else if (reg.test(email)) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_edit_mail_red").append('<div id="tenant_edit_mail_right" class="tenant_right"></div>');
            $("#tenant_mail_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
            $("#hideRemindEmail").val(email);
            check_tenant_mail = true;
            return true;
        }
    } else {

        var flag = 1;
        var index = -1;
        var emailstr = "";
        for (var i = 0; i < arremail.length; ++i) {
            arremail[i] = $.trim(arremail[i]);
            var emailTemp = arremail[i];
            emailTemp = emailTemp.replace(/\n/g, "");
            if ("" == emailTemp) {
                if (0 != flag) {
                    flag = 1
                }
                continue;
            } else if (emailTemp.length > 50) {
                flag = 3;
                index = i;
                break;
            } else if (reg.test(emailTemp)) {
                emailstr += emailTemp + ";";
                flag = 0;
            } else {
                flag = 2;
                index = i;
                break;
            }
        }
        if (0 == flag) {
            var repeat = getRepeat(arremail);
            if (-1 == repeat) {
                $("#tenant_edit_mail_right").remove();
                $("#tenant_edit_mail_red").append('<div id="tenant_edit_mail_right" class="tenant_right"></div>');
                $("#tenant_mail_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
                $("#hideRemindEmail").val(emailstr);
                check_tenant_mail = true;    //Redmine#440liangjiajie0306正确改变标志位
                return true;
            } else {
                var temp = arremail[repeat];
                if (temp != "") {
                    $("#tenant_edit_mail_right").remove();
                    $("#tenant_mail_response").text(LanguageScript.common_Mail + ":" + temp + LanguageScript.error_e01247);
                    $("#tenant_mail_response").css("color", "red");
                    //$("#tenant_mail_response").show();    //Redmine#440liangjiajie0306空白代替隐藏
                    check_tenant_mail = false;
                    return false;
                } else {
                    $("#tenant_edit_mail_right").remove();
                    $("#tenant_edit_mail_red").append('<div id="tenant_edit_mail_right" class="tenant_right"></div>');
                    $("#tenant_mail_response").text("");    //Redmine#440liangjiajie0306空白代替隐藏
                    $("#hideRemindEmail").val(emailstr);
                    check_tenant_mail = true;    //Redmine#440liangjiajie0306正确改变标志位
                    return true;
                }
            }
        } else if (2 == flag) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.common_Mail + ":" + arremail[i] + LanguageScript.error_e01248);
            $("#tenant_mail_response").css("color", "red");
            //$("#tenant_mail_response").show();    //Redmine#440liangjiajie0306空白代替隐藏
            check_tenant_mail = false;
            return false;
        } else if (3 == flag) {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.error_e01246);
            $("#tenant_mail_response").css("color", "red");
            check_tenant_mail = false;    //Redmine#440liangjiajie0306正确改变标志位
            return false;
        } else {
            $("#tenant_edit_mail_right").remove();
            $("#tenant_mail_response").text(LanguageScript.common_CorrectFormatMail);
            $("#tenant_mail_response").css("color", "red");
            check_tenant_mail = false;
            return false;
        }
    }
    /**/

    //var emails = emailValue.split(';');
    //var flag = 0;
    //for (i = 0 ; i < emails.length; i++)
    //{
    //    result = isEmail(emails[i]);
    //    if (1 == result) {
    //        $("#tenant_edit_mail_right").remove();
    //        $("#tenant_edit_mail_td").append('<div id="tenant_edit_mail_right" class="tenant_right"></div>');
    //        $("#tenant_mail_response").hide();
    //        check_tenant_mail = true;
    //    }
    //    else if (-1 == result) {
    //        $("#tenant_edit_mail_right").remove();
    //        $("#tenant_mail_response").text("邮箱:" + emails[i] + "格式错误，请输入格式正确的邮箱");
    //        $("#tenant_mail_response").css("color", "red");
    //        $("#tenant_mail_response").show();
    //        check_tenant_mail = false;
    //        break;
    //    } else if (0 == result) {
    //        $("#tenant_edit_mail_right").remove();
    //        $("#tenant_mail_response").text("请输入邮箱账号");
    //        $("#tenant_mail_response").css("color", "red");
    //        check_tenant_mail = false;
    //        break;
    //    }
    //}
}
/*fengpan alert阀值end*/
//wenti
// 切换 dialog
//dialog 迁移提示
function Set_href() {
    //var title = " 分 组";
    var text = LanguageScript.common_DiaConEdit;
    //$("#body_position").before(function () {
    //    return "<div id= 'dialog_background'></div>" +
    //           "<div id= 'dialog' >" +
    //              "<div class='dialog_title'>" + title + "</div>" +
    //              "<div class='dialog_text'>" + text + "</div>" +
    //              "<div id='user_sure' class='cls_dialog_sure'>确定</div>" +
    //              "<div id='user_back' class='cls_dialog_sure'>取消</div>" +
    //    "</div>";
    //});
    //20140308caoyandong-jquery
    $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".user_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                "确定": function () {
                    $("#dialog").remove();
                    //HrefFlag--;
                    if ($("#accunt_edit")[0].style.display == "none")
                        HrefFlag = 0;
                    else {
                        HrefFlag = 1;
                    }
                    WaitingTAB();
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });

    $("#user_back").unbind();
    $("#user_sure").unbind();
    $("#user_back").click(function () {
        $("#dialog_background").remove();
        $("#dialog").remove();
    });
    $("#user_sure").click(function () {
        $("#dialog_background").remove();
        $("#dialog").remove();
        HrefFlag--;
        WaitingTAB();
    });
}


function Set_PageCheck(pageclickednumber) {
    //var title = " 分 组";
    var text = LanguageScript.common_DiaConEdit;
    //$("#body_position").before(function () {
    //    return "<div id= 'dialog_background'></div>" +
    //           "<div id= 'dialog' >" +
    //              "<div class='dialog_title'>" + title + "</div>" +
    //              "<div class='dialog_text'>" + text + "</div>" +
    //              "<div id='user_sure' class='cls_dialog_sure'>确定</div>" +
    //              "<div id='user_back' class='cls_dialog_sure'>取消</div>" +
    //    "</div>";
    //});
    //20140308caoyandong-jquery
    $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".user_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center', 250],
            buttons: {
                "确定": function () {
                    $("#dialog").remove();
                  
                    InitVehicleAllSetting(pageclickednumber)
                    currentPageAll = pageclickednumber;
                    $("#result").html("Clicked Page " + pageclickednumber);
					unbind_OBU_editBTN();
                    $("#Setting_container_OBU_btn_return").unbind();
                    $("#Setting_container_OBU_btn_confirm").unbind();
                    $("#Setting_container_OBU_btn_confirm").hide();
                    $("#Setting_container_OBU_btn_return").hide();
                    //fengpan 20140630
                    if ($("#accunt_edit")[0].style.display == "none")
                        HrefFlag = 0;
                    else {
                        HrefFlag = 1;
                    }
                    //HrefFlag--;
       
                    vehicleId = "-1";
                    unbind_OBU_editANDadd();
                    bind_OBU_editANDadd();
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}
/*add*/
//2014038caoyandong-jquery
function OBU_add() {
    //unbind_OBU_editANDadd();
    $(function () {
        var OBU_ESN = $("#OBU_dialog_ESN_div"),
          OBU_KEY = $("#OBU_dialog_KEY_div"),
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
            }
            else if (!reg.test($.trim(OBU_ESN.val()))) {
                $(".validateTips_ESN").text(LanguageScript.page_register_ESN_Validate);
                $(".validateTips_ESN").css("color", "red");
                $("#ui-right_ESN").hide();
                $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
            } else {
                $("#ui-right_ESN").show();
                $(".validateTips_ESN").css("margin", "-26px 0px 8px 0px");
            }
        });
        
        $("#dialog_form_OBU").dialog({
                height: 230, /* liying for add obu failed*/
                resizable: false, 
                autoOpen: false,
                width: 320,
                modal: true,
                position: ['center', 250],
                draggabled: true,
                buttons: {
                    "保存": function () {
                         if ($.trim(OBU_KEY.val()) == "") {
                             $(".validateTips_KEY").text(LanguageScript.error_e01253);
                             $(".validateTips_KEY").css("color", "red");
                             $("#ui-right_KEY").hide();
                             $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                         }
                         if ($.trim(OBU_ESN.val()) == "") {
                             $(".validateTips_ESN").text(LanguageScript.error_e01277);
                             $(".validateTips_ESN").css("color", "red");
                             $("#ui-right_ESN").hide();
                             $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                         }
                        //todo
                             if (-1 == reg.test($.trim(OBU_ESN.val()))) {
                                 $(".validateTips_ESN").text(LanguageScript.page_register_ESN_Validate);
                                 $(".validateTips_ESN").css("color", "red");
                                 $("#ui-right_ESN").hide();
                                 $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                             }
                             if (-1 == reg.test($.trim(OBU_KEY.val()))) {
                                 $(".validateTips_KEY").text(LanguageScript.page_register_Register_Validate);
                                 $(".validateTips_KEY").css("color", "red");
                                 $("#ui-right_KEY").hide();
                                 $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
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
                                             //modify by li-xiaofei for page
                                             --HrefFlag;
                                             $("#Setting_container_OBU_btn_add").attr("clicked", false);
                                             $("#dialog_form_OBU").dialog("close");

                                             //Add by LiYing 2014-06-12 start
                                             $("#Setting_container_OBU_btn_add").css("background-color", "rgb(255, 255, 255)");
                                             //Add by LiYing 2014-06-12 end
                                             getObuData("-1");
											 
                                             //chenyangwen 20140505 #1353
                                             //var newTable = '<table style="left:20px; border-collapse:collapse;width:832px;background-color:#f1f1f1;border:1px solid #ddd;" id="Setting_OBU_table"><tbody>' + vehicleTrInfo_NoCar($.trim(OBU_ESN.val()), $.trim(OBU_KEY.val())) + '</tbody></table>';
                                             //$("#Setting_container_obu_view").append(newTable);
                                             //chenyangwen 20140505 #1353
                                             //$("#dialog_form_OBU").dialog("close");
                                             //$("#OBU_dialog_ESN_div").val("");
                                             //$("#OBU_dialog_KEY_div").val("");
                                             //$("#ui-right_KEY").hide();
                                             //$(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                                             //$("#ui-right_ESN").hide();
                                             //$(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                                             //Height_User($("#Setting_container_obu_view").find("tr").length - 1, 80);
                                             //modify by li-xiaofei for page
                                             //Add by LiYing start
                                             $("#OBU_dialog_ESN_div").val("");
                                             $("#OBU_dialog_KEY_div").val("");
                                             $(".validateTips_ESN").text(LanguageScript.error_e01278);
                                             $(".validateTips_ESN").css("color", "gray");
                                             $(".validateTips_KEY").text(LanguageScript.page_register_Registerkey_Intro);
                                             $(".validateTips_KEY").css("color", "gray");
                                             $("#ui-right_KEY").hide();
                                             $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                                             $("#ui-right_ESN").hide();
                                             $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                                             //Add by LiYing end
                                         }
                                         else if("NG" == msg){
                                             //TODO 先不考虑错误的问题
                                             $("#add_vehicle_failed").text(LanguageScript.error_addOBU);
                                             $("#add_vehicle_failed").show();
                                         }
                                         else if ("error" == msg) {
                                             //TODO 先不考虑错误的问题
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
                        $("#Setting_container_OBU_btn_add").attr("clicked", false);
                        --HrefFlag;
                        $(this).dialog("close");
                        //Add by LiYing 2014-06-12 start
                        $("#Setting_container_OBU_btn_add").css("background-color", "rgb(255, 255, 255)");
                        //Add by LiYing 2014-06-12 end
                        $("#OBU_dialog_ESN_div").val("");
                        $("#OBU_dialog_KEY_div").val("");
                        $(".validateTips_ESN").text(LanguageScript.error_e01278);
                        $(".validateTips_ESN").css("color", "gray");
                        $(".validateTips_KEY").text(LanguageScript.page_register_Registerkey_Intro);
                        $(".validateTips_KEY").css("color", "gray");
                        $("#ui-right_KEY").hide();
                        $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_ESN").hide();
                        $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                        //bind_OBU_editANDadd();
                        Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
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
                $("#ui-right_KEY").hide();
                $(".validateTips_KEY").css("margin", "-13px 0px 8px 0px");
                $(".validateTips_KEY").text(LanguageScript.page_register_Registerkey_Intro);
                $(".validateTips_KEY").css("color", "gray");
            });
            $("#Setting_container_OBU_btn_add")
              .buttonset()
              .click(function () {
                  if ($("#Setting_container_OBU_btn_add").attr("clicked") != "true") {
                      ++HrefFlag;
                      /* liying for add obu failed*/
                      $("#add_vehicle_failed").hide();
                      //Add by LiYing 2014-06-12 start
                      $("#Setting_container_OBU_btn_add").css("background-color", "rgb(177, 172, 172)");
                      //Add by LiYing 2014-06-12 end
                      $("#dialog_form_OBU").dialog("open");
                  }
                  $("#Setting_container_OBU_btn_add").attr("clicked", true);
              });
    });
    
    //OBU_dialog_add_sure();
}

// Add gaoqingbo Start

function dialog_OdometerCheck() {

    var reg = /^[1-9]\d*$/;
    var _obu_Odometer = $("#OBU_dialog_Odometer_div");
    if ($.trim(_obu_Odometer.val()) == "") {
        $("#ui-right_Odometer").hide();
        $(".validateTips_Odometer").text("车辆里程不能为空");
        $(".validateTips_Odometer").css("color", "red");
        return false;
    } else if (!reg.test($.trim(_obu_Odometer.val()))) {
        $("#ui-right_Odometer").hide();
        $(".validateTips_Odometer").text(LanguageScript.error_e01283);
        $(".validateTips_Odometer").css("color", "red");
        return false;
    } else {
        $("#ui-right_Odometer").show();
        $(".validateTips_Odometer").css("margin", "-26px 0px 8px 0px");
        return true;
    }
}

function addOdometer(selectVehicleID) {

    $(function () {

        // odometer check
        $("#OBU_dialog_Odometer_div").blur(function () {
            dialog_OdometerCheck();
        });

        $("#dialog_form_OBU_updateOdometer").dialog({
                height: 140,
                resizable: false, 
                autoOpen: false,
                width: '31.25%',
                modal: true,
                position: ['center', 250],
                draggabled: true,
                buttons: {
                    "保存": function () {

                        if (dialog_OdometerCheck()) {

                            $.ajax({
                                type: "POST",
                                url: "/" + GetCompanyID() + "/Setting/UpdateOdometer",
                                data: { VehicleID: $("#selectVehicleID").val(), Odometer: $("#OBU_dialog_Odometer_div").val() },
                                contentType: "application/x-www-form-urlencoded",
                                dataType: "text",
                                success: function (msg) {

                                    if ("OK" == msg) {

                                        $("#dialog_form_OBU_updateOdometer").dialog("close");
                                        $("#OBU_dialog_Odometer_div").val("");
                                        $("#ui-right_Odometer").hide();
                                        $(".validateTips_Odometer").css("margin", "-13px 0px 8px 0px");
                                    }
                                    else
                                    {
                                        $("#dialog_form_OBU_updateOdometer").dialog("close");
                                        $("#OBU_dialog_Odometer_div").val("");
                                        $("#ui-right_Odometer").hide();
                                        $(".validateTips_Odometer").css("margin", "-13px 0px 8px 0px");
                                        if (msg == "400") {
                                            user_dialog_error(LanguageScript.page_update_Vehicle_error_400);
                                        }
                                        else if (msg == "500") {
                                            user_dialog_error(LanguageScript.page_update_Vehicle_error_500);
                                        } else {
                                            user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
                                        }
                                    }
                                },
                                error: function () {
                                    //...
                                }
                            });

                            
                        }
                    },
                    取消: function () {
                        $(this).dialog("close");
                        $("#OBU_dialog_Odometer_div").val("");
                        $("#ui-right_Odometer").hide();
                        $(".validateTips_Odometer").text(LanguageScript.common_NoticeOdometer);
                        $(".validateTips_Odometer").css("color", "gray");
                        $(".validateTips_Odometer").css("margin", "-13px 0px 8px 0px");
                        //bind_OBU_editANDadd();
                        Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
                    }
                }
            });
        $("#OBU_dialog_Odometer_div").focus(function () {
            $(".validateTips_Odometer").text(LanguageScript.common_NoticeOdometer);
            $(".validateTips_Odometer").css("color", "gray");
        });

        // 第一次点击设置里程按钮
        $("#ui-right_Odometer").hide();
        $("#dialog_form_OBU_updateOdometer").dialog("open");

        // 第一次以后注册设置里程按钮的点击事件
        $(".cls_Setting_container_OBU_Odometerlittle_btn")
            .buttonset()
            .click(function () {
                $("#ui-right_Odometer").hide();
                $("#selectVehicleID").val(this.id.substr(4));
                $("#dialog_form_OBU_updateOdometer").dialog("open");


        });
     });
        //Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length);
    //OBU_dialog_add_sure();
}

// Add gaoqingbo End

function OBU_dialog_add_sure() {
    $("#dialog").remove();
    $("#dialog_background").remove();
    $("#body_position").before(function () {
        return "<div id='dialog_background'></div>" +
               "<div id='dialog' >" +
                  "<div class='dialog_title'>" + LanguageScript.common_Add + "</div>" +
                  "<div id='OBU_dialog_ESN_div' class='OBU_dialog_ESN'>" + LanguageScript.common_ESN + ":<input id='OBU_ESN_input' type='text'/></div>" +
                  "<div id='OBU_dialog_KEY_div' class='OBU_dialog_KEY'>" + LanguageScript.page_settings_accountSettings_vehicleDiagnosticManagement_registrationKeyLabel + ":<input id='OBU_KEY_input' type='text'/></div>" +
                  "<div id='popup_error_info' class='popup_error_info'>" + LanguageScript.error_e01254 + "</div>" +
                  "<div id='popup_error_info_error' class='popup_error_info'>" + LanguageScript.error_e01255 + "</div>" +
                  "<div id='user_sure' class='cls_dialog_sure'>" + LanguageScript.common_save + "</div>" +
                  "<div id='user_back' class='cls_dialog_sure'>" + LanguageScript.common_cancel + "</div>" +
        "</div>";
    });
}
function OBU_registion_OBU() {
    var registion_OK;
    var OBU_ESN_id = "";
    var OBU_KEY_id = "";
    var check_ESN = false;
    var check_KEY = false;
    $("#OBU_ESN_input").blur(function () {
        OBU_ESN_id = $("#OBU_ESN_input").val();
        OBU_KEY_id = $("#OBU_KEY_input").val();
        if ("" == OBU_ESN_id) {
            $("#OBU_dialog_ESN_error").remove();
            $("#OBU_dialog_ESN_right").remove();
            $("#OBU_dialog_ESN_div").append('<div id="OBU_dialog_ESN_error" class="error" style="left:230px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + LanguageScript.error_e01256 + '</div>');
            check_ESN = false;
        }
        else {
            $("#OBU_dialog_ESN_error").remove();
            $("#OBU_dialog_ESN_right").remove();
            $("#OBU_dialog_ESN_div").append('<div id="OBU_dialog_ESN_right" class="right" style="left:230px"></div>');
            check_ESN = true;
        }
    })
    $("#OBU_KEY_input").blur(function () {
        OBU_ESN_id = $("#OBU_ESN_input").val();
        OBU_KEY_id = $("#OBU_KEY_input").val();
        registion_OK = registion_OBU(OBU_ESN_id, OBU_KEY_id);
        // 遍历是否注册过，todo；
        if ("" == OBU_KEY_id) {
            $("#OBU_dialog_KEY_error").remove();
            $("#OBU_dialog_KEY_right").remove();
            $("#OBU_dialog_KEY_div").append('<div id="OBU_dialog_KEY_error" class="error" style="top:3px;line-height:15px;left:230px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + LanguageScript.error_e01256 + '</div>');
            check_KEY = false;
        }
        else if (registion_OK) {
            $("#OBU_dialog_KEY_error").remove();
            $("#OBU_dialog_KEY_right").remove();
            $("#OBU_dialog_KEY_div").append('<div id="OBU_dialog_KEY_right" class="right" style="left:230px"></div>');
            check_KEY = true;
        }
        else if (!registion_OK) {
            $("#OBU_dialog_KEY_error").remove();
            $("#OBU_dialog_KEY_right").remove();
            $("#OBU_dialog_KEY_div").append('<div id="OBU_dialog_KEY_error" class="error" style="top:3px;line-height:15px;left:230px">&nbsp;&nbsp;&nbsp;&nbsp;' + LanguageScript.error_e01260 + '</div>');
            check_KEY = false;
        }
        else {
            //...
        }
    })
    $("#user_back").click(function () {
        $("#dialog_background").remove();
        $("#dialog").remove();
    });
    $("#user_sure").click(function () {
        
        
        if (check_ESN == true && check_KEY == true) {
            //...
            $("#dialog_background").remove();
            $("#dialog").remove();
            var VehicleId = '';
            var VehicleName = '';
            var VehicleVin = '';
            var VehicleInfo = '';
            var vehiclelicence = '';
            //var OBU_ADD_code = vehicleTrInfo("-", "-", "-", "-", "-", OBU_ESN_id, OBU_KEY_id);
            //$("#Setting_OBU_table").append(OBU_ADD_code);
            //alert($("#Setting_OBU_table").height());

            var CompanyID = GetCompanyID();
            $.ajax({
                type: "POST",
                url: "/" + CompanyID + "/Setting/RegistionOBU",
                data: { OBU_ESN_ID: OBU_ESN_id, OBU_KEY_ID: OBU_KEY_id },
                contentType: "application/x-www-form-urlencoded",
                dataType: "text",
                success: function (msg) {
                    //...
                    if ("OK" == msg) {
                        //...
                        //var OBU_ADD_code = vehicleTrInfo("-", "-", "-", "-", "-", OBU_ESN_id, OBU_KEY_id);
                        //$("#Setting_OBU_table").append(OBU_ADD_code);
                        getObuData("-1");
                    }
                    else {
                        //...
                    }
                },
                error: function () {
                    //...
                }
            });
            //getObuData();

           // if ($("#Setting_OBU_table").height() > 443) {
           //     document.getElementById("u_left").style.height = $("#Setting_OBU_table").height() + 337 + "px";
           //     document.getElementById("u_right").style.height = $("#Setting_OBU_table").height() + 337 + "px";
         // }
        }
        else if ("" == OBU_ESN_id || "" == OBU_ESN_id) {
            $("#popup_error_info").hide();
            $("#popup_error_info_error").hide();
            $("#popup_error_info").show();
        }
        else if (!registion_OK) {
            //...
            $("#popup_error_info").hide();
            $("#popup_error_info_error").hide();
            $("#popup_error_info_error").show();
        }
    });
}
function registion_OBU(OBU_ESN_id, OBU_KEY_id) {
    //todo...
    if ("6C4C729D" == OBU_ESN_id && "59e5-ad61" == OBU_KEY_id) {
        return false;
    }
    return true;
}



// Add by gaoqingbo

// TODO 支持多语言的时候需要修改【1. 后台传回来的值是EN和CN的<CN,EN>，需要显示什么在以下方法中判断即可】
// 在MMYid存在的情况下初始化MMYDialog
function initEditMMYDialog_existMmyId(mmyid) {
    $.ajax({
        type: "POST",
        url: "/" + GetCompanyID() + "/Setting/GetMMYDialogData",
        data: { mmyid: mmyid },
        dataType: "json",
        success: function (returnData) {
            if (returnData == null || returnData == "") {
                initEditMMYDialog_notEistMmyId();
            }
            else {
                var mmy = returnData.mmy;
                var make_list = returnData.make_list;
                var model_list = returnData.model_list;
                var year_list = returnData.year_list;
                var makeOptinStr = '<option title=" " value="">&nbsp;</option>';
                var modelOptinStr = '<option title=" " value="">&nbsp;</option>';
                var yearOptinStr = '<option title=" " value="">&nbsp;</option>';

                // make select初始化
                $.each(make_list, function (index, make) {
                    makeOptinStr += "<option value='" + make + "'>" + make + "</option>";
                });
                $("#make-list").html(""); //容错处理
                $("#make-list").html(makeOptinStr);
                // model select初始化
                $.each(model_list, function (index, model) {
                    modelOptinStr += "<option value='" + model + "'>" + model + "</option>";
                });
                $("#model-list").html(""); //容错处理
                $("#model-list").html(modelOptinStr);
                // make select初始化
                $.each(year_list, function (index, year) {
                    yearOptinStr += "<option data_selected_MmyId='" + year.mmyIndex + "' value='" + $.trim(year.mmyYear) + "'>" + $.trim(year.mmyYear) + "</option>";
                });
                $("#year-list").html(""); //容错处理
                $("#year-list").html(yearOptinStr);

                //初始值设定
                $("#make-list option[value='" + $.trim(mmy.mmyMake) + "']").attr("selected", true);
                $("#model-list option[value='" + $.trim(mmy.mmyModel) + "']").attr("selected", true);
                $("#year-list option[value='" + $.trim(mmy.mmyYear) + "']").attr("selected", true);

                // 添加对号
                $("#ui-right_make").show();
                $(".validateTips_make").css("margin", "-26px 0px 8px 0px");
                $("#ui-right_model").show();
                $(".validateTips_model").css("margin", "-26px 0px 8px 0px");
                $("#ui-right_year").show();
                $(".validateTips_year").css("margin", "-26px 0px 8px 0px");
                //fengpan 20140618
                $("#make-list").selectpicker('refresh');
                $("#model-list").selectpicker('refresh');
                $("#year-list").selectpicker('refresh');
                // 应该添加loding页面，如添加放在ajax前面
                $("#dialog_form_mmy").dialog("open");
            }
            
        },
        error: function () {
            //alert("@Resource.String.ihpleD_String_cn.error_e01229");
        }
    });
}

// TODO 支持多语言的时候需要修改【1. 后台传回来的值是EN和CN的<CN,EN>，需要显示什么在以下方法中判断即可】
// 在MMYid不存在的情况下初始化MMYDialog（只初始化make对应的select就可以了）
function initEditMMYDialog_notEistMmyId() {
    $.ajax({
        type: "POST",
        url: "/" + GetCompanyID() + "/Setting/GetMakeList",
        data: {},
        dataType: "json",
        success: function (returnData) {

            var make_list = returnData.make_list;
            var makeOptinStr = "<option title=' ' value=''>&nbsp;</option>";

            if (null != make_list && make_list != "") {
                // make select初始化
                $.each(make_list, function (index, make) {
                    makeOptinStr += "<option value='" + make + "'>" + make + "</option>";
                });
            }
            $("#make-list").html(""); //容错处理
            $("#make-list").html(makeOptinStr);
            //fengpan 20140618
            $("#make-list").selectpicker('refresh');
            $("#model-list").selectpicker('refresh');
            $("#year-list").selectpicker('refresh');

            // 应该添加loding页面，如添加放在ajax前面
            $("#dialog_form_mmy").dialog("open");
        },
        failure: function () {
            alert("@Resource.String.ihpleD_String_cn.error_e01229");
        }
    });
}
//冯盼 20140616 只绑定一次change事件
function mmy_dialog_changeBlurEnent()
{
    // 光标移走，添加check
    $("#make-list").blur(function () {
        if ($("#make-list").val() == "") {
            $(".validateTips_make").text(LanguageScript.error_e01290);
            $(".validateTips_make").css("color", "red");
            $(".validateTips_make").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_make").hide();
            return;
        } else {
            $("#ui-right_make").show();
            $(".validateTips_make").css("margin", "-26px 0px 8px 0px");
            $(".validateTips_make").css("color", "gray");
            $(".validateTips_make").text(LanguageScript.common_confirmMake);
        }
    });
    $("#model-list").blur(function () {
        if ($("#model-list").val() == "") {
            $(".validateTips_model").text(LanguageScript.error_e01291);
            $(".validateTips_model").css("color", "red");
            $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_model").hide();
            return;
        } else {
            $("#ui-right_model").show();
            $(".validateTips_model").css("color", "gray");
            $(".validateTips_model").text(LanguageScript.common_confirmModel);
            $(".validateTips_model").css("margin", "-26px 0px 8px 0px");
        }
    });
    $("#year-list").blur(function () {
        if ($("#year-list").val() == "") {
            $(".validateTips_year").text(LanguageScript.error_e01292);
            $(".validateTips_year").css("color", "red");
            $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_year").hide();
            return;
        } else {
            $("#ui-right_year").show();
            $(".validateTips_year").text(LanguageScript.common_confirmYear);
            $(".validateTips_year").css("margin", "-26px 0px 8px 0px");
            $(".validateTips_year").css("color", "gray");
        }
    });

    // 注册前两个select的change事件
    $("#make-list").change(function () {
        if ($("#make-list").val() == "") {
            $(".validateTips_make").text(LanguageScript.error_e01290);
            $(".validateTips_make").css("color", "red");
            $(".validateTips_make").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_make").hide();

            $(".validateTips_model").text(LanguageScript.common_confirmModel);
            $(".validateTips_model").css("color", "gray");
            $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_model").hide();

            $(".validateTips_year").text(LanguageScript.common_confirmYear);
            $(".validateTips_year").css("color", "gray");
            $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_year").hide();

            $("#model-list").html("<option title=' '  value=''>&nbsp;</option>");
            $("#year-list").html("<option title=' ' value=''>&nbsp;</option>");
            //fengpan 20140618
            $("#model-list").selectpicker('refresh');
            $("#year-list").selectpicker('refresh');
        } else {
            $("#ui-right_make").show();
            $(".validateTips_make").css("margin", "-26px 0px 8px 0px");
            $(".validateTips_make").css("color", "gray");
            $(".validateTips_make").text(LanguageScript.common_confirmMake);

            //fengpan 20140616
            $("#model-list").html("<option title=' ' value=''>&nbsp;</option>");
            $("#year-list").html("<option title=' ' value=''>&nbsp;</option>");
            //fengpan 20140618
            $("#model-list").selectpicker('refresh');
            $("#year-list").selectpicker('refresh');

            $.ajax({
                type: "POST",
                url: "/" + GetCompanyID() + "/Setting/GetModelList",
                data: { make: this.value },
                dataType: "json",
                //fengpan 20140616
                //beforeSend: function () {
                //    $("#model-list").html("<option value=''></option>");
                //    $("#year-list").html("<option value=''></option>");
                //},
                success: function (returnData) {

                    var model_list = returnData.model_list;
                    var modelOptinStr = "<option title=' ' value=''>&nbsp;</option>";

                    // model select初始化
                    $.each(model_list, function (index, model) {
                        modelOptinStr += "<option value='" + model + "'>" + model + "</option>";
                    });
                    $("#model-list").html(""); //容错处理
                    $("#model-list").html(modelOptinStr);
                    $("#year-list").html("<option title=' ' value=''>&nbsp;</option>");
                    //fengpan 20140618
                    $("#model-list").selectpicker('refresh');
                    $("#year-list").selectpicker('refresh');

                    // model和year的样式处理
                    $("#ui-right_model").hide();
                    $(".validateTips_model").text(LanguageScript.common_confirmModel);
                    $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_model").css("color", "gray");
                    $("#ui-right_year").hide();
                    $(".validateTips_year").text(LanguageScript.common_confirmYear);
                    $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_year").css("color", "gray");
                }
            })
        }
    });
    $("#model-list").change(function () {
        if ($("#model-list").val() == "") {
            $(".validateTips_model").text(LanguageScript.error_e01291);
            $(".validateTips_model").css("color", "red");
            $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_model").hide();

            $(".validateTips_year").text(LanguageScript.common_confirmYear);
            $(".validateTips_year").css("color", "gray");
            $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_year").hide();

            $("#year-list").html("<option title=' ' value=''>&nbsp;</option>");
            //fengpan 20140618
            $("#year-list").selectpicker('refresh');
            return;
        } else {
            $("#ui-right_model").show();
            $(".validateTips_model").css("color", "gray");
            $(".validateTips_model").text(LanguageScript.common_confirmModel);
            $(".validateTips_model").css("margin", "-26px 0px 8px 0px");

            //fengpan 20140616
            $("#year-list").html("<option title=' ' value=''></option>");
            $.ajax({
                type: "POST",
                url: "/" + GetCompanyID() + "/Setting/GetYearList",
                data: {
                    make: $("#make-list").val(),
                    model: this.value
                },
                dataType: "json",
                //beforeSend: function () {
                //$("#year-list").html("<option value=''></option>");
                //},
                success: function (returnData) {

                    var year_list = returnData.year_list;
                    var yearOptinStr = "<option title=' ' value=''>&nbsp;</option>";

                    // year select初始化
                    $.each(year_list, function (index, year) {
                        yearOptinStr += "<option data_selected_MmyId='" + year.mmyIndex + "' value='" + $.trim(year.mmyYear) + "'>" + $.trim(year.mmyYear) + "</option>";
                    });
                    $("#year-list").html(""); //容错处理
                    $("#year-list").html(yearOptinStr);
                    //fengpan 20140618
                    $("#year-list").selectpicker('refresh');

                    // year的样式处理
                    $("#ui-right_year").hide();
                    $(".validateTips_year").text(LanguageScript.common_confirmYear);
                    $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_year").css("color", "gray");
                }
            })
        }
    });
    $("#year-list").change(function () {
        if ($("#year-list").val() == "") {
            $(".validateTips_year").text(LanguageScript.error_e01292);
            $(".validateTips_year").css("color", "red");
            $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
            $("#ui-right_year").hide();
            return;
        } else {
            $("#ui-right_year").show();
            $(".validateTips_year").text(LanguageScript.common_confirmYear);
            $(".validateTips_year").css("margin", "-26px 0px 8px 0px");
            $(".validateTips_year").css("color", "gray");
        }
    })
    //fengpan 20140618
    $("#make-list").selectpicker();
    $("#model-list").selectpicker();
    $("#year-list").selectpicker();
    $("#role").selectpicker();
}

// 车辆编辑中编辑mmy按钮点击，初始化页面中元素，并注册dialog中各个控件的事件
function initEditMMYDialog(id,callback) {
    $(function () {

        // Dialog初始化
        $("#make-list").html("<option title=' ' value=''>&nbsp;</option>");
        $("#model-list").html("<option title=' ' value=''>&nbsp;</option>");
        $("#year-list").html("<option title=' ' value=''>&nbsp;</option>");
        
        // Dialog对号隐藏
        $("#ui-right_make").hide();
        $("#ui-right_model").hide();
        $("#ui-right_year").hide();
        $(".validateTips_make").text(LanguageScript.common_confirmMake);
        $(".validateTips_model").text(LanguageScript.common_confirmModel);
        $(".validateTips_year").text(LanguageScript.common_confirmYear);
        $(".validateTips_make").css("margin", "-13px 0px 8px 0px");
        $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
        $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
        $(".validateTips_make").css("color", "gray");
        $(".validateTips_model").css("color", "gray");
        $(".validateTips_year").css("color", "gray");

        var mmyid = $("#" + id).attr("data-mmyid");

        // 初始化三个select的值，两种情况[1.MMY为空；2MMY不为空]
        // 如果mmyid为空，只初始化make对应的select
        // 如果mmyid不为空，初始化三个select，并把对应的make，model，year设定为选中态
        if ($.isNumeric(mmyid)) {

            initEditMMYDialog_existMmyId(mmyid);
        } else {
            initEditMMYDialog_notEistMmyId();
        }
        
        // 注册dialog中两个按钮的事件
        $("#dialog_form_mmy").dialog({
            height: 280,
            resizable: false,
            autoOpen: false,
            width: 320,
            modal: true,
            position: ['center', 250],
            draggabled: true,
            buttons: {
                "保存": function () {

                    // 保存前check
                    if ($("#make-list").val() == "") {
                        $(".validateTips_make").text(LanguageScript.error_e01290);
                        $(".validateTips_make").css("color", "red");
                        $(".validateTips_make").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_make").hide();
                        return;
                    }

                    if ($("#model-list").val() == "") {
                        $(".validateTips_model").text(LanguageScript.error_e01291);
                        $(".validateTips_model").css("color", "red");
                        $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_model").hide();
                        return;
                    }

                    if ($("#year-list").val() == "") {
                        $(".validateTips_year").text(LanguageScript.error_e01292);
                        $(".validateTips_year").css("color", "red");
                        $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_year").hide();
                        return;
                    }

                    // check三个select不能为空
                    // 修改两个隐藏的input[1. info;2. mmyid]
                    // 修改tr中隐藏的attr[data-mmyid],以便下次操作使用
                    var selectedMake = $("#make-list").val();
                    var selectedModel = $("#model-list").val();
                    var selectedYear = $("#year-list").val();
                    var selectmmyid = $("#year-list").find("option:selected").attr("data_selected_mmyid");

                    //$("#OBU_input_2").val(selectedMake + " " + selectedModel + " " + selectedYear);
                    //$("#OBU_input_mmyid").val(selectmmyid);
                    //$("#OBU_input_2").parents().filter("tr").attr("data-mmyid", selectmmyid);
                    $("#OBU_input_mmyid_new").val(selectmmyid);
                    $("#OBU_input_new").val(selectedMake + " " + selectedModel + " " + selectedYear)
                    $(this).dialog("close");
                    callback(id);

                    $("#" + id + " .setting_button_save").css("color", "blue");
                    $("#" + id + " .setting_button_save").attr("clickFlag", "true");
                    $("#" + id + " .setting_button_cancle").css("color", "blue");
                    $("#" + id + " .setting_button_cancle").attr("clickFlag", "true");
                },
                取消: function () {

                    // 恢复原有的状态
                    // 恢复两个隐藏的input[1. info;2. mmyid]
                    // 恢复tr中隐藏的attr[data-mmyid],以便下次操作使用

                    $("#make-list").html("<option title=' ' value=''></option>");
                    $("#model-list").html("<option title=' ' value=''></option>");
                    $("#year-list").html("<option title=' ' value=''></option>");
                    // Dialog对号隐藏
                    $("#ui-right_make").hide();
                    $("#ui-right_model").hide();
                    $("#ui-right_year").hide();
                    $(".validateTips_make").text(LanguageScript.common_confirmMake);
                    $(".validateTips_model").text(LanguageScript.common_confirmModel);
                    $(".validateTips_year").text(LanguageScript.common_confirmYear);
                    $(".validateTips_make").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_model").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_year").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_make").css("color", "gray");
                    $(".validateTips_model").css("color", "gray");
                    $(".validateTips_year").css("color", "gray");
                    $(this).dialog("close");

                    $("#" + id + " .setting_button_save").css("color", "blue");
                    $("#" + id + " .setting_button_save").attr("clickFlag", "true");
                    $("#" + id + " .setting_button_cancle").css("color", "blue");
                    $("#" + id + " .setting_button_cancle").attr("clickFlag", "true");
                }
            }
        });
    });
}

//#733 判断重名分组liangjiajie0319
function IsVehicleNameExist(vehiclename) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/Setting/IsVehicleNameExist",
        data: { vehiclename: vehiclename },//
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            result = msg;
        }
    });
    if ("false" == result) {
        return true;
    } else {
        return false;
    }
}
//#733 判断重名分组liangjiajie0319

function unbind_OBU_editBTN() {
    $(".cls_Setting_container_OBU_little_btn").unbind();
    $(".cls_Setting_container_OBU_little_btn").css("color", "gray");
    $(".cls_Setting_container_OBU_little_btn").css("cursor", "default");

    // add 里程数
    $(".cls_Setting_container_OBU_Odometerlittle_btn").unbind();
    $(".cls_Setting_container_OBU_Odometerlittle_btn").css("color", "gray");
    $(".cls_Setting_container_OBU_Odometerlittle_btn").css("cursor", "default");
}

function unbind_OBU_editANDadd() {
    $("#Setting_container_OBU_btn_edit").unbind();
    $("#Setting_container_OBU_btn_edit").css("cursor", "default");
    $("#Setting_container_OBU_btn_edit").css("background-color", "rgb(177, 172, 172)");

    $("#Setting_container_OBU_btn_add").unbind();
    $("#Setting_container_OBU_btn_add").css("cursor", "default");
    $("#Setting_container_OBU_btn_add").css("background-color", "rgb(177, 172, 172)");
}

function bind_OBU_editANDadd() {
    $(".cls_Setting_container_OBU_little_btn").css("cursor", "pointer");
    $(".cls_Setting_container_OBU_little_btn").css("color", "blue");

    // 添加里程数按钮
    $(".cls_Setting_container_OBU_Odometerlittle_btn").css("cursor", "pointer");
    $(".cls_Setting_container_OBU_Odometerlittle_btn").css("color", "blue");

    // 车辆编辑按钮按下事件
    $(".cls_Setting_container_OBU_little_btn").click(function (e) {
        //HrefFlag++;
        //标志错误增加liangjiajie0308
        var id = e.currentTarget.id;
        currentid = id.substr(4);
        logosubmitflag = 0;
        OBU_dialog_edit_sure(id.substr(4));
    });

    $(".cls_Setting_container_OBU_Odometerlittle_btn").click(function (e) {

        $("#selectVehicleID").val(this.id.substr(4));
        addOdometer();
    });

    $("#Setting_container_OBU_btn_edit").css("cursor", "pointer");
    $("#Setting_container_OBU_btn_edit").css("background-color", "#fff");

    $("#Setting_container_OBU_btn_add").css("cursor", "pointer");
    $("#Setting_container_OBU_btn_add").css("background-color", "#fff");

    $("#Setting_container_OBU_btn_edit").click(function () {
        OBU_edit();
    });
    //20140308caoyandong-jquery
    //$("#Setting_container_OBU_btn_add").click(function () {
        OBU_add();
    //    OBU_registion_OBU();
    //});
}

//检测图片；
function checkLogo() {
    var path = $("#vehicleLogo").val();
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
            $("#vehicleLogo").val("");
            return false;
        }
    }
}
//清空session//fengpan #508 20140304
function clearSession() {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/ClearSession",
        data: {},
        contentType: "application/x-www-form-urlencoded",
        datatype: "text",
        success: function (msg) {
            if ("OK" == msg) {
                //....
            }
        }
    });
}
/***************fengpan************************/



/****************************************************************************/
/****************************************************************************/
/*****************************Setting User***********************************/
/******************************Ma Biao Start*********************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/

//重置密码
function getRandomString(len) {
    var chars = 'ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456780';
    var charsLen = chars.length;
    var randomPassword = '';
    for (var i = 0; i < len; ++i) {
        randomPassword += chars.charAt(Math.floor(Math.random() * charsLen));
    }
    return randomPassword;
}
//正则表达式 mail检测
function isEmail(str) {
    str = $.trim(str);
    //var reg = /^([a-zA-Z0-9_\-\.])+\@@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    //var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*$/;
    //var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
    var reg = /^([a-zA-Z0-9_\-\.]){1,}@([a-z0-9A-Z]?[a-z0-9A-Z]+)+(((\.\w+))*)+[\.][a-z]{2,4}$/;  //liangjiajie
    //var reg = /^(\w)+(\.\w+)*@(\w)+((\.\w+)){1,2}$/;
    var noEmailInput = 0;
    var wrongEmail = -1;
    var rightEmail = 1;
    if ("" == str) {
        return noEmailInput;
    } else if (reg.test(str)) {
        return rightEmail;
    } else {
        return wrongEmail;
    }
}
//正则表达式 Phone检测
function isPhone(str) {


    var reg = /(^(\d{3,4}-)?\d{7,8})$|(^(\d{11}))$/;
    var noPhoneInput = 0;
    var rightPhone = 1;
    var wrongPhone = -1;
    str = $.trim(str);
    if ("" == str) {
        return noPhoneInput;
    } else if (reg.test(str)) {
        return rightPhone;
    } else {
        return wrongPhone;
    }
}//liangjiajie
//正则表达式 用户名检测
function isUser(str) {
    var reg = /^[a-zA-Z0-9\_\-]{1,20}$/;
    return reg.test(str);
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
function getUserData(pagenum_user) {
    var CompanyID = GetCompanyID();
    $.ajax({
        data: { pagenum_user: pagenum_user },
        datatype: JSON,
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetUser",
        contentType: "application/x-www-form-urlencoded",
        success: function (dic) {
            if ($("#Setting_container_user")[0].style.display == "none") {
                return;
            }
            if (dic.userlist.length == 0) {
                var roleID = GetRoleID();
                if (1 != roleID) {
                    unbind_event();
                } else {
                    bind_event();
                }
                //return;
            }
            var view = "";
            view += UserView(dic.userlist);
            $("#Setting_container_user_list").empty();
            $("#Setting_container_user_list").append(view);

            if (dic.pagecount == 0) {
                dic.pagecount = 1;
            }

            $("#pageBar").pager({ pagenumber: pagenum_user, pagecount: dic.pagecount, buttonClickCallback: PageClick });
            $("#pageBar").attr("successflag", "true");
            //Height_User(obj.length);
            var roleID = GetRoleID();
            if (1 != roleID) {
                unbind_event();
            } else {
                bind_event();
            }
        }
    });
    //var xmlhttp = getXmlHttpRequest();
    ////使用post传送
    //xmlhttp.open('post', '/' + CompanyID + '/Setting/GetUser', true);
    ////chenyangwen 20140528 #1653
    //xmlhttp.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    ////post方式需要设置消息头
    //xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //xmlhttp.onreadystatechange = function () {
    //    if (4 == xmlhttp.readyState) {
    //        if (200 == xmlhttp.status) {
    //            var txt = xmlhttp.responseText;
    //            if (txt == "[]") {
    //                var roleID = GetRoleID();
    //                if (1 != roleID) {
    //                    unbind_event();
    //                } else {
    //                    bind_event();
    //                }
    //                return;
    //            }
	//	if($("#Setting_container_user")[0].style.display == "none"){
	//		return;
	//	}
    //            var obj = eval("(" + txt + ")");
    //            var view = "";
    //            view += UserView(obj);
    //            $("#Setting_container_user_list").empty();
    //            $("#Setting_container_user_list").append(view);
    //            Height_User(obj.length);
    //            var roleID = GetRoleID();
    //            if (1 != roleID) {
    //                unbind_event();
    //            } else {
    //                bind_event();
    //            }
    //            //chenyangwen 20140528 #1653
    //        } else if (499 == xmlhttp.status) {
    //            window.location.href = "/";
    //        }
    //        else {
    //            var roleID = GetRoleID();
    //            if (1 != roleID) {
    //                unbind_event();
    //            } else {
    //                bind_event();
    //            }
    //            //... ...
    //        }
    //    } else {
    //        //... ...
    //    }
    //}
    //xmlhttp.send("pagenum_user=" + pagenum_user);
    ////xmlhttp.send(null);
}

PageClick = function (pageclickednumber) {
    if (HrefFlag > 0) {
        $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + LanguageScript.common_DiaConEdit + '</p>';
        $(function () {
            $(".user_error").dialog({
                resizable: false,
                height: 140,
                width: 280,
                modal: true,
                position: ['center', 250],
                buttons: {
                    "确定": function () {
                        $("#dialog").remove();
                        $("#user_edit_sure").remove();
                        $("#user_edit_back").remove();
                        HrefFlag--;
                        if ($("#pageBar").attr("successflag") == "true") {
                            getUserData(pageclickednumber);
                            $("#result").html("Clicked Page " + pageclickednumber);
                        }
                        $(this).dialog("close");
                    },
                    取消: function () {
                        $(this).dialog("close");
                    }
                }
            });
        });
    } else {
        if ($("#pageBar").attr("successflag") == "true") {
            getUserData(pageclickednumber);
            $("#result").html("Clicked Page " + pageclickednumber);
        }
    }
}

//用户TAB 高度变化
function Height_User(listnum, height) {
    return;
    if (listnum >= 8) {
        var length = (listnum - 8) * height;
        //chenyangwen 20140505 #1353
        if (0 == length && height > 60) {
            length = height;
        }
        document.getElementById("u_left").style.height = (800 + length) + "px";
        document.getElementById("u_right").style.height = (800 + length) + "px";
    }
}
var UserView = function (obj) {
    var view = "";
        for (var i = 0; i < obj.length; i++) {
            view += '<div id="UserID_' + obj[i].pkid + '"class="Setting_container_user_list">' +
                       //'<div  class="Setting_container_user_list_form">' +
                       //    '<form >' +
                       //        '<input  type="checkbox" name="UserID_' + obj[i].pkid + '">' +
                       //    '</form>' +
                       //'</div>' +
                       '<div class="Setting_container_user_list_table" style="width:75%">' +
                           '<table style="table-layout:fixed; width:100%">' +
                               '<tr style="width:100%">' +
                                '<td style="width:25%; padding-left:30px"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(obj[i].username) + '">' + $.trim(obj[i].username) + '</div></td>';
            if (2 == obj[i].FleetUser_Role[0].roleid) {
                view += '<td style="width:17%;">' + LanguageScript.common_user + '</td>';
            } else if (1 == obj[i].FleetUser_Role[0].roleid) {
                view += '<td style="width:17%;">' + LanguageScript.page_admin_userDetails_role_tenantAdmin + '</td>';
            } else {
                view += '<td style="width:17%;">' + LanguageScript.common_user + '</td>';
            }
            view += '<td style="width:33.4%;"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(obj[i].email) + '">' + obj[i].email + '</div></td>' +
                              '<td style="width:25%;">' + obj[i].telephone + '</td>' +
                             '</tr>' +
                         '</table>' +
                     '</div>' +
                     '<div class="cls_Setting_container_user_btn_edit">' + LanguageScript.common_edit + '</div>' +
                     '<div class="cls_Setting_container_user_btn_del">' + LanguageScript.common_delete + '</div>' +
                     '<div class="cls_Setting_container_user_btn_reset">' + LanguageScript.common_resetPassword + '</div>' +
                  '</div>';
        }
   
    return view;
}

/*mabiao 20140305 重构后 已经废弃 之后会删掉*/
////获取checkbox 选中数量
////1为正常值 ，其他均为异常 做异常处理
//var checkbox_num = function () {
//    var input_num = $("#Setting_container_user_list").find("input").length;
//    var checkbox_true_num = 0;
//    for (var i = 0; i < input_num; i++) {
//        var checkbox = $("#Setting_container_user_list").find("input")[i].checked;
//        if (checkbox == true) {
//            checkbox_true_num++;
//        }
//    }
//    return checkbox_true_num;
//}


function updateTips(t) {
    $(".validateTips")
      .text(t)
      .addClass("ui-state-highlight");
    setTimeout(function () {
        $(".validateTips").removeClass("ui-state-highlight", 1500);
    }, 500);
}
//判断重名用户liangjiajie0310
function IsUserExist(userName) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/Setting/IsUserNameExist",
        data: { username: userName },//
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            result = msg;
            //if ("false" == msg) {
            //    return true;
            //} else {
            //    return false;
            //}
        }//逗号差分遗漏，造成ie8页面错误liangjiajie0321
    });

    if ("false" == result) {
        return true;
    } else {
        return false;
    }

}


/*mabiao 20140305 #585*/
//马骉重构 添加用户
//用户tab中添加处理
//20140306caoyandong-jquery
function User_Add() {
    unbind_event();
    $(function () {
            var name = $("#name"),
                role = $("#role"),
                email = $("#email"),
                phone = $("#phone");
            $("#name").blur(function () {
                if ("" == $.trim(name.val())) {
                    $(".validateTips_Username").text(LanguageScript.error_e01266);
                    $(".validateTips_Username").css("color", "red");
                    $("#ui-right_name").hide();
                    $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                    return;
                }
                else if (isUser($.trim(name.val())) != 1) {
                    $(".validateTips_Username").text(LanguageScript.error_e01267);
                    $(".validateTips_Username").css("color", "red");
                    $("#ui-right_name").hide();
                    $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                    return;
                }
                else if (false == IsUserExist($.trim(name.val()))) {//重名检测liangjiajie0311
                    $(".validateTips_Username").text(LanguageScript.error_e00217);
                    $(".validateTips_Username").css("color", "red");
                    $("#ui-right_name").hide();
                    $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                    return;
                } else {
                    $("#ui-right_name").show();
                    $(".validateTips_Username").css("margin", "-26px 0px 8px 0px");
                }
            });
            $("#email").blur(function () {
                if (-1 == isEmail($.trim(email.val()))) {
                    $(".validateTips_email").text(LanguageScript.error_e01268);
                    $(".validateTips_email").css("color", "red");
                    $("#ui-right_email").hide();
                    $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                    return;
                }
                if ("" == $.trim(email.val())) {
                    $(".validateTips_email").text(LanguageScript.error_e01269);
                    $(".validateTips_email").css("color", "red");
                    $("#ui-right_email").hide();
                    $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                    return;
                } else {
                    $("#ui-right_email").show();
                    $(".validateTips_email").css("margin", "-26px 0px 8px 0px");
                }
            });
            $("#phone").blur(function () {
                if (1 == isPhone($.trim(phone.val()))) {
                    $("#ui-right_tel").show();
                    $(".validateTips_tel").css("margin", "-12px 0px 8px 0px");
                }else if(-1 == isPhone($.trim(phone.val()))) {
                    $(".validateTips_tel").text(LanguageScript.error_e01270);
                    $(".validateTips_tel").css("color", "red");
                    $("#ui-right_tel").hide();
                    $(".validateTips_tel").css("margin", "4px 0px 8px 0px");
                    return;
                } 
            });
            $("#dialog_form_user").dialog({
                    resizable: false,
                    draggable: true,
                    autoOpen: false,
                    height: 370,
                    width: '31.25%',
                    modal: true,
                    position: ['center',150],
                    buttons: {
                        "保存": function () {
                            if ("" == $.trim(name.val())) {
                                $(".validateTips_Username").text(LanguageScript.error_e01266);
                                $(".validateTips_Username").css("color", "red");
                                $("#ui-right_name").hide();
                                $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                            }
                            else if (isUser($.trim(name.val())) != 1) {
                                $(".validateTips_Username").text(LanguageScript.error_e01267);
                                $(".validateTips_Username").css("color", "red");
                                $("#ui-right_name").hide();
                                $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                            }
                            var isEsixt = IsUserExist($.trim(name.val()));
                            if (false == isEsixt) {//重名检测liangjiajie0311
                                $(".validateTips_Username").text(LanguageScript.error_e00217);
                                $(".validateTips_Username").css("color", "red");
                                $("#ui-right_name").hide();
                                $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                            }
                            if (-1 == isEmail($.trim(email.val()))) {
                                $(".validateTips_email").text(LanguageScript.error_e01268);
                                $(".validateTips_email").css("color", "red");
                                $("#ui-right_email").hide();
                                $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                            }
                            if ("" == $.trim(email.val())) {
                                $(".validateTips_email").text(LanguageScript.error_e01269);
                                $(".validateTips_email").css("color", "red");
                                $("#ui-right_email").hide();
                                $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                            }
                            if (1 == isPhone($.trim(phone.val()))) {
                                $("#ui-right_tel").show();
                                $(".validateTips_tel").css("margin", "-12px 0px 8px 0px");
                            } else if (-1 == isPhone($.trim(phone.val()))) {
                                $(".validateTips_tel").text(LanguageScript.error_e01270);
                                $(".validateTips_tel").css("color", "red");
                                $("#ui-right_tel").hide();
                                $(".validateTips_tel").css("margin", "4px 0px 8px 0px");
                                return;
                            }
                            var add_user_info = new Array();
                            add_user_info[0] = name.val();
                            add_user_info[1] = role.val();

                            add_user_info[2] = email.val();
                            add_user_info[3] = phone.val();
                            if (isUser($.trim(name.val()))==1 &&
                                isEmail($.trim(email.val()))==1 &&
                                $.trim(name.val()) != "" &&
                                isEsixt != false && 
                                "" != $.trim(email.val()) &&
                                -1 != isPhone($.trim(phone.val()))
                                ) {
                                User_Add_Ajax(add_user_info);
                                $("#name").val("");
                                $("#role").val("");
                                $("#email").val("");
                                $("#phone").val("");
                                $(".validateTips_Username").text(LanguageScript.page_setting_UserNameRule);
                                $(".validateTips_Username").css("color", "gray");
                                $(".validateTips_tel").text(LanguageScript.common_NoPhoneInput);
                                $(".validateTips_tel").css("color", "gray");
                                $(".validateTips_email").text(LanguageScript.page_setting_EnterUserMail);
                                $(".validateTips_email").css("color", "gray");
                                $("#ui-right_email").hide();
                                $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                                $("#ui-right_name").hide();
                                $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                                $("#ui-right_tel").hide();
                                $(".validateTips_tel").css("margin", "4px 0px 8px 0px");
                                $("#UserID_Add").remove();
                                --HrefFlag;
                                $(this).dialog("close");
                                //Add by LiYing 2014-06-12 start
                                $("#Setting_container_user_btn_add").css("background-color", "rgb(255, 255, 255)");
                                //Add by LiYing 2014-06-12 end
                            }
                        },
                        取消: function () {
                            $("#name").val("");
                            $("#role").val("");
                            $("#email").val("");
                            $("#phone").val("");
                            $(".validateTips_Username").text(LanguageScript.page_setting_UserNameRule);
                            $(".validateTips_Username").css("color", "gray");
                            $(".validateTips_tel").text(LanguageScript.common_NoPhoneInput);
                            $(".validateTips_tel").css("color", "gray");
                            $(".validateTips_email").text(LanguageScript.page_setting_EnterUserMail);
                            $(".validateTips_email").css("color", "gray");
                            $("#ui-right_email").hide();
                            $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                            $("#ui-right_name").hide();
                            $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                            $("#ui-right_tel").hide();
                            $(".validateTips_tel").css("margin", "4px 0px 8px 0px");
                            --HrefFlag;
                            $(this).dialog("close");
                            //Add by LiYing 2014-06-12 start
                            $("#Setting_container_user_btn_add").css("background-color", "rgb(255, 255, 255)");
                            //Add by LiYing 2014-06-12 end
                            $("#UserID_Add").remove();
                            bind_event();
                            Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
                        }
                    }
                    
                        //close: function () {
                        //    allFields.val("").removeClass("ui-state-error");
                        //}
                });
                $("#name").focus(function () {
                    $(".validateTips_Username").text(LanguageScript.page_setting_UserNameRule);
                    $(".validateTips_Username").css("color", "gray");
                    $("#ui-right_name").hide();
                    $(".validateTips_Username").css("margin", "-13px 0px 8px 0px");
                });
                $("#phone").focus(function () {
                    $(".validateTips_tel").text(LanguageScript.common_NoPhoneInput);
                    $(".validateTips_tel").css("color", "gray");
                    $("#ui-right_tel").hide();
                    $(".validateTips_tel").css("margin", "4px 0px 8px 0px");
                });
                $("#email").focus(function () {
                    $(".validateTips_email").text(LanguageScript.page_setting_EnterUserMail);
                    $(".validateTips_email").css("color", "gray");
                    $("#ui-right_email").hide();
                    $(".validateTips_email").css("margin", "-13px 0px 8px 0px");
                });
                $("#Setting_container_user_btn_add")
                  .buttonset()
                  .click(function () {
                      ++HrefFlag;
                      $("#dialog_form_user").dialog("open");
                      //fengpan 20140618
                      $("#role").selectpicker('refresh');
                      //Add by LiYing 2014-06-12 start
                      $("#Setting_container_user_btn_add").css("background-color", "rgb(177, 172, 172)");
                      //Add by LiYing 2014-06-12 end
                      
                  });
            });
        

        //$("#Setting_container_user_list").append(function () {
        //    return '<div id="UserID_Add' + '"class="Setting_container_user_list">' +
        //           '<div class="Setting_container_user_list_table">' +
        //               '<table >' +
        //                   '<tr >' +
        //                    '<input type="text" style="width:120px;margin:0px 25px 0 3px; font-size:center;border-radius:3px;placeholder="姓名" maxlength = "15">' +
        //                    '<select style="width:70px;margin:0px 40px 0px 0px; font-size:center;border-radius:3px;">' +
        //                        '<option selected = "selected" value="2">用户</option>' +
        //                        '<option value="1">管理员</option>' +
        //                    '</select>' +
        //                    '<input type="text" style="width:180px;margin:0px 20px 0px 0px; font-size:center;border-radius:3px;placeholder="电子邮件"  maxlength="38">' +
        //                    '<input type="text" style="width:120px;margin:0px 27px 0px 0px; font-size:center;border-radius:3px;placeholder="电话号码" maxlength="15">' +
        //                   '</tr>' +
        //               '</table>' +
        //           '</div>' +
        //       '</div>';
        //});
        //$(".Setting_container_user_btn").append(function () {
        //    return "<div id='user_edit_sure' class='cls_Setting_container_user_btn'>确定</div>" +
        //           "<div id='user_edit_back' class='cls_Setting_container_user_btn'>取消</div>";
        //});
        Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
        //user_dialog_add_sure();
}
//20140306caoyandong-jquery

/*mabiao 20140305 #585*/
// mabiao 重写 通过ID 获取 USER Name
//用户tab中选择数量处理 liangjiajie
function User_Select(id) {
    //var input_num = $("#Setting_container_user_list").find("input").length;
    //var id = null;
    //var accunt_name = "";
    //for (var i = 0; i < input_num; i++) {
    //    var checkbox = $("#Setting_container_user_list").find("input")[i].checked;
    //    if (checkbox == true) {
    //        id = $("#Setting_container_user_list").find("input")[i].name;
    //        accunt_name  += $("#" + id).find("td")[0].innerHTML + " ";
    //    }
    //}
    var accunt_name = "";
    accunt_name = $("#" + id).find("td")[0].innerHTML;
    return accunt_name;
   
}

//用户tab中编辑处理
function User_Edit() {
    /*mabiao 20140305 #585*/
    /*$("#Setting_container_user_btn_edit").click(function () {
        var true_num = checkbox_num();
        if (0 == true_num) {
            var title = "编 辑";
            var text = "请您选择要编辑的用户。";
            user_dialog_error(text);
        } else if (1 < true_num) {
            var title = "编 辑";
            var text = "您选择的用户过多，请重新选择。";
            user_dialog_error(text);
        } else if (true_num == 1) {
            user_dialog_edit_sure();
        }
    })*/
    $(".cls_Setting_container_user_btn_edit").click(function (e) {
        var id = e.currentTarget.parentElement.id;
        user_dialog_edit_sure(id);
    });
    /*mabiao 20140305 #585*/
}

//用户tab中删除处理
//20140308caoyandong-jquery
function User_Del() {
    $(".cls_Setting_container_user_btn_del").click(function (e) {
        /*mabiao 20140305 #585*/
        /*var true_num = checkbox_num();
        if (0 == true_num) {
            var title = "删 除";
            var text = "请您选择要删除的用户。";
            user_dialog_error(text);
        }
        //} else if (1 < true_num) {
        //    var title = "删 除";
        //    var text = "您选择的用户过多，请重新选择。";
        //    user_dialog_error(text);
        //} else if (true_num == 1) {
        //    var title = "删 除";
        //    var text = "您确认要删除所选择的用户信息么？";
        //    user_dialog_del_sure(title, text);
        //}
        else {*/
            var id = e.currentTarget.parentElement.id;
            //var title = "删 除";
        //var text = "您确认要删除以下用户信息吗？" + "<div>" + User_Select(id) + "</div>";

            //fengpan 20140324 #730
            var accunt_name = $("#" + id).find("td div")[0].innerHTML;
            $(".setting_user_del")[0].innerHTML =
                '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;">用户:' + accunt_name + '</p>'+
                '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;">确定删除该用户吗？</p>';

            $(function () {
                $(".setting_user_del").dialog({
                    resizable: false,
                    height: 140,
                    position: ['center',250],
                    modal: true,
                    buttons: {
                        "确定": function () {
                            $(this).dialog("close");
                            User_Del_Ajax(id);
                            $("#pageBar").attr("successflag", "false");
                            getUserData(pagenum_user);
                        },
                        取消: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });

            
        /*mabiao 20140305 #585*/
        /*}*/
        /*mabiao 20140305 #585*/
    });

    
}

/*mabiao 20140305 #585*/
//mabiao 重构函数 不需判断
//用户tab中重置密码处理
function User_Reset() {
    //$("#Setting_container_user_btn_reset").click(function () {
    //    var true_num = checkbox_num();
    //    if (0 == true_num) {
    //        var title = "重置密码";
    //        var text = "请您选择要重置密码的用户。";
    //        user_dialog_error(text);
    //    } else if (1 < true_num) {
    //        var title = "重置密码";
    //        var text = "您选择的用户过多，请重新选择。";
    //        user_dialog_error(text);
    //    } else if (true_num == 1) {
    //        var title = "重置密码";
    //        var text = "<div style='padding:3px;'>您确定要重置以下用户密码吗？</div>" + "<div style='padding:3px;'>用户名：" + User_Select() + "</div>";
    //        user_dialog_reset_sure(title, text);
    //    }
    //});
    $(".cls_Setting_container_user_btn_reset").click(function (e) {
        var id = e.currentTarget.parentElement.id;
        var accunt_name = $("#" + id).find("td div")[0].innerHTML;
        $(".setting_user_reset")[0].innerHTML =
                '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;">用户:' + accunt_name + '</p>' +
                '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;">您确定重置其密码吗？</p>';

        $(function () {
            $(".setting_user_reset").dialog({
                resizable: false,
                height: 140,
                width:'27.3%',
                modal: true,
                position: ['center',250],
                buttons: {
                    "确定": function () {
                        $(this).dialog("close");
                        User_Reset_Ajax(id);
                    },
                    取消: function () {
                        $(this).dialog("close");
                    }
                }
            });
        });

        //var title = "重置密码";
        //var text = "<div style='padding:3px;'>您确定要重置以下用户密码吗？</div>" + "<div style='padding:3px;'>用户名：" + User_Select(id) + "</div>";
        //user_dialog_reset_sure(title, text, id);
        user_dialog_reset_sure(id);
    });
}

//用户tab中对button 进行tonedown处理
function unbind_event() {
    /*mabiao 20140305 #585*/
    $("#Setting_container_user_btn_add").unbind();
    $("#Setting_container_user_btn_add").css("cursor", "default");
    $("#Setting_container_user_btn_add").css("background-color", "rgb(177, 172, 172)");
    /*$("#Setting_container_user_btn_edit").unbind();*/
    $(".cls_Setting_container_user_btn_edit").unbind();
    $(".cls_Setting_container_user_btn_edit").css("cursor", "default");
    $(".cls_Setting_container_user_btn_edit").css("color", "gray");
    /*$("#Setting_container_user_btn_del").unbind();*/
    $(".cls_Setting_container_user_btn_del").unbind();
    $(".cls_Setting_container_user_btn_del").css("cursor", "default");
    $(".cls_Setting_container_user_btn_del").css("color", "gray");
    /*$("#Setting_container_user_btn_reset").unbind();*/
    $(".cls_Setting_container_user_btn_reset").unbind();
    $(".cls_Setting_container_user_btn_reset").css("cursor", "default");
    $(".cls_Setting_container_user_btn_reset").css("color", "gray");
    /*mabiao 20140305 #585*/
}

//用户tab中对button 进行toneup处理
function bind_event() {
    /*mabiao 20140305 #585*/
    $("#Setting_container_user_btn_add").unbind();
    /*$("#Setting_container_user_btn_edit").unbind();
    $("#Setting_container_user_btn_del").unbind();
    $("#Setting_container_user_btn_reset").unbind();*/
    $(".cls_Setting_container_user_btn_edit").unbind();
    $(".cls_Setting_container_user_btn_del").unbind();
    $(".cls_Setting_container_user_btn_reset").unbind();

    User_Add();
    $("#Setting_container_user_btn_add").css("cursor", "pointer");
    $("#Setting_container_user_btn_add").css("background-color", "#FFF");

    User_Edit();
    $(".cls_Setting_container_user_btn_edit").css("cursor", "pointer");
    $(".cls_Setting_container_user_btn_edit").css("color", "blue");

    User_Del();
    $(".cls_Setting_container_user_btn_del").css("cursor", "pointer");
    $(".cls_Setting_container_user_btn_del").css("color", "blue");

    User_Reset();
    $(".cls_Setting_container_user_btn_reset").css("cursor", "pointer");
    $(".cls_Setting_container_user_btn_reset").css("color", "blue");
    /*mabiao 20140305 #585*/
}

/*mabiao 20140305 #585*/
//mabiao 重构 通过参数id 获取Group ID 传给后台
//异步处理，DB删除用户
function User_Del_Ajax(id) {
    //var array = ''; //fengpan
    //var input_num = $("#Setting_container_user_list").find("input").length;
    //var id = null;
    //for (var i = 0; i < input_num; i++) {
    //    var checkbox = $("#Setting_container_user_list").find("input")[i].checked;
    //    if (checkbox == true) {
    //        id = $("#Setting_container_user_list").find("input")[i].name;
    //        array += id.substr(7) + ',';//fengpan
    //        //continue;//fengpan
    //    }
    //}
    //array = array.substring(0, array.length - 1);
    //var ids = array.split(',');
    //var array = id.split("_"); //fengpan

    //mabiao 20140305 #585*/
    var GroupID = id.substr(7);
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/DelUser",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "pkid=" + GroupID,
        success: function (msg) {
            if (msg == "OK") {
                /*mabiao 20140305 #585*/
                //for (var i = 0; i < ids.length; i++) {
                //    $("#UserID_" + ids[i]).remove();
                //}
                //$("#" + id).remove();
                ///*mabiao 20140305 #585*/
                //Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
                $("#pageBar").attr("successflag", "false");
                getUserData(pagenum_user);
            } else if (msg == "Error") {
                var text = LanguageScript.page_Setting_deleteUser_warning;
                user_dialog_error(text);
            }
        }
    });
}
//wenti
//20140308caoyandong-jquery
//dialog 异常处理 弹出信息 (作为共通error处理)
function user_dialog_error(text) {
    //$("#body_position").before(function () {
    //    return "<div id= 'dialog_background' ></div>" +
    //           "<div id= 'dialog' >" +
    //              "<div class='dialog_title'>"+title+"</div>" +
    //              "<div class='dialog_text'>"+text+"</div>" +
    //              "<div id='user_error' class='cls_dialog_sure'>确定</div>"
    //    "</div>";
    //})
    //$("#user_error").click(function () {
    //    $("#dialog_background").remove();
    //    $("#dialog").remove();
    //})
    $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".user_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            modal: true,
            position: ['center',250],
            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}
//dialog 异常处理 弹出信息 (作为共通error处理)
function OBU_dialog_error(text) {
    //$("#body_position").before(function () {
    //    $("#dialog").remove();
    //    return "<div id='dialog_background'></div>" +
    //           "<div id='dialog' >" +
    //              "<div class='dialog_title'>" + title + "</div>" +
    //              "<div class='dialog_text'>" + text + "</div>" +
    //              "<div id='user_error' class='cls_dialog_sure'>确定</div>"
    //    "</div>";
    //})
    //$("#user_error").click(function () {
    //    $("#dialog_background").remove();
    //    $("#dialog").remove();
    //})
    $(".OBU_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".OBU_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            position: ['center', 250],
            modal: true,
            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}
//20140308caoyandong-jqurey
//dialog正常处理 用户再次确认删除用户
function user_dialog_del_sure(id) {
    
        User_Del_Ajax(id);
}

//dialog正常处理，用户再次确认后 重置用户
function user_dialog_reset_sure(id) {
    //var id = id;
    //$("#body_position").before(function () {
    //    return "<div id= 'dialog_background' ></div>" +
    //           "<div id= 'dialog' >" +
    //              "<div class='dialog_title'>" + title + "</div>" +
    //              "<div class='dialog_text_1'>" + text + "</div>" +
    //              "<div id='user_sure' class='cls_dialog_sure'>确定</div>" +
    //              "<div id='user_back' class='cls_dialog_sure'>返回</div>" +
    //    "</div>";
    //})
    //$("#user_back").click(function () {
    //    $("#dialog_background").remove();
    //    $("#dialog").remove();
    //})
    //$("#user_sure").click(function () {
        //User_Reset_Ajax(id);
        //$("#dialog_background").remove();
        //$("#dialog").remove();
    //})
}

/*mabiao 20140305 #585*/
//马骉 重构函数 
//重置用户 Ajax函数
function User_Reset_Ajax(id) {
    /*此处遍历用户名，应该传参数会更快*/
    //var input_num = $("#Setting_container_user_list").find("input").length;
    //var id = null;
    //for (var i = 0; i < input_num; i++) {
    //    var checkbox = $("#Setting_container_user_list").find("input")[i].checked;
    //    if (checkbox == true) {
    //        id = $("#Setting_container_user_list").find("input")[i].name;
    //        var accunt_name = $("#" + id).find("td")[0].innerHTML;
    //        break;
    //    }
    //}
    var accunt_name = $("#" + id).find("td div")[0].innerHTML;
    /*mabiao for bug 20140219*/
    var password = getRandomString(8);
    /*mabiao for bug 20140219*/

    password_1 = accunt_name.toLocaleLowerCase() + '&' + password;
    var MD5Password = hex_md5($.trim(password_1));

    var array = id.split("_");

    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/ResetUser",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "pkid=" + array[1] + "&password=" + MD5Password,
        success: function (msg) {
            if ("OK" == msg) {
			 var buttons_with_copy = {};
                if ($.browser.msie) {
                    buttons_with_copy = {
                        "复制密码": function () {
                            if (window.clipboardData) {
                                window.clipboardData.setData("Text", password)
                                $(function () {
                                    $(".copy_password").dialog({
                                        resizable: false,
                                        height: 160,
                                        modal: true,
                                        buttons: {
                                            "确定": function () {
                                                $(this).dialog("close");
                                            }
                                        }
                                    });
                                });
                            }
                            else {
                                $(function () {
                                    $(".copy_failed")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + '请选中密码，使用 Ctrl+C 复制！<br>' + password + '</p>';
                                    $(".copy_failed").dialog({
                                        resizable: false,
                                        height: 160,
                                        modal: true,
                                        buttons: {
                                            "确定": function () {
                                                $(this).dialog("close");
                                            }
                                        }
                                    });
                                });
                            }
                            $(this).dialog("close");

                        },
                        确定: function () {
                            $(this).dialog("close");
                        }
                    };
                } else {
                    buttons_with_copy = {
                        确定: function () {
                            $(this).dialog("close");
                        }
                    };
                }
                //var title = "重置密码";
                //var text = " 用户密码已重置为：<div>" + password + "</div>"/*#167*/
                $(".setting_user_reset2")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;margin-top:1em;">' + LanguageScript.page_setting_UserResetedPassword + '&nbsp;:&nbsp;<br> ' + password + '</p>';
                $(function () {
                    $(".setting_user_reset2").dialog({
                        resizable: false,
                        height: 140,
                        width:280,
                        modal: true,
                        position: ['center', 250],
                        buttons: buttons_with_copy
                    });
                });
                //user_dialog_result(password);
            }
        }
    });
}
//dialog显示重置密码结果
//function user_dialog_result(title, text, password) {
//    $("#body_position").before(function () {
//        return "<div id= 'dialog_background'></div>" +
//               "<div id= 'dialog' >" +
//                  "<div class='dialog_title'>" + title + "</div>" +
//                  "<div class='dialog_text'>" + text + "</div>" +
//                  "<div id='user_copy' class='cls_dialog_sure'>复制密码</div>" +
//                  "<div id='user_error' class='cls_dialog_sure'>确定</div>"
//        "</div>";
//    })
//    $("#user_error").click(function () {
//        $("#dialog_background").remove();
//        $("#dialog").remove();
//    })


//    $("#user_copy").click(function () {
//        if (window.clipboardData) {
//            window.clipboardData.setData("Text", password)
//            alert('copy成功！');
//        }
//        else {
//            alert("请选中密码，使用 Ctrl+C 复制！");
//        }

//    })
//}

//dialog正常处理 用户再次确认编辑用户
function user_dialog_edit_sure(id) {
    HrefFlag++;
    unbind_event();
    $(".Setting_container_user_btn").append(function () {
        return "<div id='user_edit_sure' class='cls_Setting_container_user_btn'>" + LanguageScript.common_save + "</div>" +
               "<div id='user_edit_back' class='cls_Setting_container_user_btn'>" + LanguageScript.common_cancel + "</div>";
    })
    /*mabiao 20140305 #585*/
    ////判断被选中的用户
    //var input_num = $("#Setting_container_user_list").find("input").length;
    //var id = null;
    //for (var i = 0; i < input_num; i++) {
    //    var checkbox = $("#Setting_container_user_list").find("input")[i].checked;
    //    if (checkbox == true) {
    //        id = $("#Setting_container_user_list").find("input")[i].name;
    //        break;
    //    }
    //}
    /*mabiao 20140305 #585*/

    //保存信息
    var user_info = new Array();
    //替换div成input 进行输入
    var td_num = $("#" + id).find("td").length;
    for (var i = 0; i < td_num; i++) {
        
        var text = $("#" + id).find("td")[i].innerHTML;
        user_info[i] = text;
        if (0 == i) {//Add by gaoqingbo 添加用户名tooltip
            user_info[i] = $("#" + id).find("div")[1].innerHTML;
        }
        if (2 == i) {
            user_info[i] = $("#" + id).find("div")[2].innerHTML;//这里0为最外层div  1为 maildiv
        }
        if (3 == i && text == "") {
            user_info[i] = "";
        }
        //$($("#" + id).find("td")[count]).replaceWith(function () {
            switch (i) {
                //case 0: return '<input type="text"maxlength="20" style="width:140px;margin:0 20px 0 0px; font-size:center;border-radius:3px;"value=' + user_info[i] + '>';
                //case 0: return '<div style="width:150px;float:left;height:48px;margin:0px">' + user_info[i] + '</div>';
                //    break;
                case 1:
                    if (user_info[i] == LanguageScript.common_user) {
                        $("#" + id).find("td")[i].innerHTML =  '<select style="width:70px; font-size:center;border-radius:3px;">' +
                                           '<option selected = "selected" value="2">' + LanguageScript.common_user + '</option>' +
                                           '<option value="1">' + LanguageScript.page_admin_userDetails_role_tenantAdmin + '</option>' +
                                   '</select>';
                    } else if (user_info[i] == LanguageScript.page_admin_userDetails_role_tenantAdmin) {
                        $("#" + id).find("td")[i].innerHTML =  '<select style="width:70%; font-size:center;border-radius:3px;">' +
                                        '<option value="2">' + LanguageScript.common_user + '</option>' +
                                        '<option selected = "selected" value="1">' + LanguageScript.page_admin_userDetails_role_tenantAdmin + '</option>' +
                                   '</select>';
                        }
                    break;
                case 2: $("#" + id).find("td")[i].innerHTML = '<input type="text" style="width:90%; font-size:center;border-radius:3px;"value="' + user_info[i] + '" maxlength="50" title="' + LanguageScript.common_NoMailInput + '">';
                    break;
                case 3: $("#" + id).find("td")[i].innerHTML = '<input type="text" style="width:80%; font-size:center;border-radius:3px;" maxlength="15" value="' + user_info[i] + '" title="' + LanguageScript.common_NoPhoneInput + '">';
                    break;
            }
            
        //});
        
        }

    $("#user_edit_back").click(function () {
        $("#user_edit_sure").remove();
        $("#user_edit_back").remove();
        User_Edit_Back(id, user_info);
        HrefFlag--;
        bind_event();
    })

    $("#user_edit_sure").click(function () {
        User_Edit_Ajax(id, user_info);
    });
}

//编辑用户 Ajax函数
function User_Edit_Ajax(id, user_info) {
    var temp = '';
    temp = user_info[0];
    //mabiao 20140311 
    var length = 4;
    var value = $("#" + id).find("input")[0].value;

    if ( -1 == isEmail(value)) {
        var title = "用 户";
        var text = LanguageScript.common_CorrectFormatMail;
                user_dialog_error(text);
        return;
    } else if (0 == isEmail(value)) {
        var title = "用 户";
        var text = LanguageScript.common_NoMailInput;
                user_dialog_error(text);
        return;
    } else if (1 == isEmail(value)) {
        //...
    }

    var value = $("#" + id).find("input")[1].value;
    if (-1 == isPhone(value)) {
        var title = "用 户";
        var text = LanguageScript.common_CorrectFormatTel;
                user_dialog_error(text);
        return;
    } else if (0 == isPhone(value)) {
        //...
    }
    //mabiao 20140311
    var role = "";
    for (var i = 0; i < length; i++) {
        switch (i) {
            case 0: value = user_info[i];
                break;
            case 1:value = $("#" + id).find("select")[0].value;
                break;
            case 2: value = $("#" + id).find("input")[0].value;
                break;
            case 3: value = $("#" + id).find("input")[1].value;
                break;
                
        }
        if ("" != value && undefined != value) {
            user_info[i] = value;
        }
        if (i == 1) {
            if (user_info[i] == "1") {
                role = LanguageScript.page_admin_userDetails_role_tenantAdmin;
            } else if (user_info[i] == "2") {
                role = LanguageScript.common_user;
            }
        }
        if (i == 3) {
            user_info[i] = value;
        }
    }
    var array = id.split("_");
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/EditUser",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "pkid=" + $.trim(array[1]) + "&username=" + $.trim(user_info[0]) + "&role=" + $.trim(user_info[1]) + "&email=" + $.trim(user_info[2]) + "&telephone=" + $.trim(user_info[3]),
        success: function (msg) {
            if ("OK" == msg) {
                /*mabiao 20140305 #585*/
                //$("#" + id).find(".Setting_container_user_list_table")[0].outerHTML = "";
                $("#" + id).empty();
                /*mabiao 20140305 #585*/
                $("#" + id).append(function () {
                    return '<div class="Setting_container_user_list_table" style="width:75%">' +
                              '<table  style="table-layout:fixed; width:100%">' +
                                 '<tr style="width:100%">' +
                                        '<td style="width:25%; padding-left:30px"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[0]) + '">' + $.trim(user_info[0]) + '</div></td>' +
                                        '<td style="width:17%;">' + $.trim(role) + '</td>' +
                                        '<td style="width:33.4%;"><div style="width:200px; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[2]) + '">' + $.trim(user_info[2]) + '</div></td>' +
                                        '<td style="width:25%;">' + $.trim(user_info[3]) + '</td>' +
                                  '</tr>' +
                             '</table>' +
                           '</div>' +
                            /*mabiao 20140305 #585*/
                           '<div class="cls_Setting_container_user_btn_edit">' + LanguageScript.common_edit + '</div>' +
                           '<div class="cls_Setting_container_user_btn_del">' + LanguageScript.common_delete + '</div>' +
                           '<div class="cls_Setting_container_user_btn_reset">' + LanguageScript.common_resetPassword + '</div>';
                           /*mabiao 20140305 #585*/
                });
                $("#user_edit_sure").remove();
                $("#user_edit_back").remove();
                bind_event();
                HrefFlag--;
            } else if ("Error" == msg)
            {
                //role = "管理员";
                user_info[1] = "1";
                user_dialog_error(LanguageScript.page_Setting_updateUser_warning);
            }
            
        },
        error: function () {
            HrefFlag--;
        }
    });
}

//返回时 input 替换回 文本框
function User_Edit_Back(id, user_info) {
    /*mabiao 20140305 #585*/
    /*$("#" + id).find(".Setting_container_user_list_table")[0].outerHTML = "";
    $("#" + id).append(function () {
        return '<div class="Setting_container_user_list_table">' +
                  '<table >' +
                     '<tr >' +
                            '<td style="width:163px;">' + user_info[0] + '</td>' +
                            '<td style="width:131px;">' + user_info[1] + '</td>' +
                            '<td style="width:279px;">' + user_info[2] + '</td>' +
                            '<td style="width:167px;">' + user_info[3] + '</td>' +
                      '</tr>' +
                 '</table>' +
               '</div>';
    });*/
    $("#" + id).empty();
    if ("1" == user_info[1]) {
        user_info[1] = LanguageScript.page_admin_userDetails_role_tenantAdmin;
    } else if ("2" == user_info[1]) {
        user_info[1] = LanguageScript.common_user;
    }
    $("#" + id).append(function () {
        return '<div class="Setting_container_user_list_table" style="width:75%">' +
                  '<table  style="table-layout:fixed; width:100%">' +
                     '<tr style="width:100%">' +
                            '<td style="width:25%; padding-left:30px"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[0]) + '">' + $.trim(user_info[0]) + '</div></td>' +
                            '<td style="width:17%;">' + user_info[1] + '</td>' +
                            '<td style="width:33.4"><div style="width:200px; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[2]) + '">' + user_info[2] + '</div></td>' +
                            '<td style="width:25%;">' + user_info[3] + '</td>' +
                      '</tr>' +
                  '</table>' +
               '</div>' +
               '<div class="cls_Setting_container_user_btn_edit">' + LanguageScript.common_edit + '</div>' +
               '<div class="cls_Setting_container_user_btn_del">' + LanguageScript.common_delete + '</div>' +
               '<div class="cls_Setting_container_user_btn_reset">' + LanguageScript.common_resetPassword + '</div>';
    });
    /*mabiao 20140305 #585*/
}

//返回  确认 执行的事件
function user_dialog_add_sure() {

    //$("#user_edit_sure").click(function () {
        var add_user_info = new Array();
        var count = 0;
        var length = $("#UserID_Add").find("input,select").length;
        for (var i = 0; i < length; i++) {
            var type = $("#UserID_Add").find("input,select")[i].type;
            if (type != "checkbox") {
                add_user_info[count] = $("#UserID_Add").find("input,select")[i].value;
                count++;
            }
        }
        var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,40}$/;
        for (var i = 0; i < count; i++) {
            if (0 == i) {
                //fengpan
                var nameLen = getByteLen(add_user_info[i]);

                if (0 == nameLen) {            //判断用户名输入为空时处理
                    var title = LanguageScript.common_user;
                    var text = "请输入用户名";     //Redmine#522liangjiajie0306
                    user_dialog_error(text);
                    return;
                }
                //fengpan

                //modified by caoyandong
                //if (add_user_info[i].length == 0) {
                //    var title = "用 户";
                //    var text = "用户名不能为空，请重新输入！";
                //    user_dialog_error(title, text);
                //    return;
                //}
                //modified by caoyandong
                if (!isUser(add_user_info[i])) {
                    var title = LanguageScript.common_user;
                    var text = LanguageScript.error_e01227;
                    user_dialog_error(text);
                    return;
                }
                var num = $(".Setting_container_user_list").length;
                for (var j = 0; j < num; j++) {
                    var judge_id = $(".Setting_container_user_list")[j].id;
                    if ($("#" + judge_id).find("td").length == 0) {
                        continue;
                    }
                    var name = $("#" + judge_id).find("td")[0].innerHTML;
                    if (name == add_user_info[i]) {
                        var title = LanguageScript.common_user;
                        var text = "用户名已存在，请重新输入！";
                        user_dialog_error(text);
                        return;
                    }
                }
            }
            if (2 == i) {
                //modified by caoyandong

                if (-1 == isEmail(add_user_info[i])) {
                    var title = "用 户";
                    var text = LanguageScript.common_CorrectFormatMail;
                    user_dialog_error(text);
                    return;
                } else if (0 == isEmail(add_user_info[i])) {
                    var title = "用 户";
                    var text = LanguageScript.common_NoMailInput;
                    user_dialog_error(text);
                    return;
                } else if (1 == isEmail(add_user_info[i])) {
                    //...
                }
            }
            if (3 == i) {
                //modified by caoyandong
                //modified by caoyandong
                if (-1 == isPhone(add_user_info[i])) {
                    var title = "用 户";
                    var text = LanguageScript.error_e00225;
                    user_dialog_error(text);
                    return;
                } else if (0 == isPhone(add_user_info[i] )) {
                    //...
                }
            }
        }
        $("#user_edit_sure").remove();
        $("#user_edit_back").remove();
        User_Add_Ajax(add_user_info);
        $("#UserID_Add").remove();
    //});

    $("#user_edit_back").click(function () {
        $("#user_edit_sure").remove();
        $("#user_edit_back").remove();
        $("#UserID_Add").remove();
        bind_event();
        HrefFlag--;
        Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
    });

}
//wenti
//向DB传入数据
function User_Add_Ajax(user_info) {
    /*mabiao for bug 20140219*/
    var password = getRandomString(8);
    /*mabiao for bug 20140219*/

    var accunt_name = user_info[0];
    password_1 = accunt_name.toLocaleLowerCase() + '&' + password;
    var MD5Password = hex_md5($.trim(password_1));

    var CompanyID = GetCompanyID();

    var role = "";
    if (user_info[1] == "1") {
        role = LanguageScript.page_admin_userDetails_role_tenantAdmin;
    } else if (user_info[1] == "2") {
        role = LanguageScript.common_user;
    }
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/AddUser",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "username=" + user_info[0] + "&role=" + user_info[1] + "&email=" + user_info[2] + "&telephone=" + user_info[3] + "&password=" + MD5Password,
        success: function (msg) {
            $("#pageBar").attr("successflag", "false");
            getUserData(pagenum_user);
            //$("#Setting_container_user_list").append(function () {
            //   return '<div id="UserID_' + msg + '"class="Setting_container_user_list" style="width:100%">' +
            //       '<div class="Setting_container_user_list_table" style="width:75%">' +
            //           '<table style="table-layout:fixed;width:100%">' +
            //               '<tr style="width:100%">' +
            //                '<td style="width:25%; padding-left:30px"><div style="width:100%; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[0]) + '">' + $.trim(user_info[0]) + '</div></td>' +
            //                //20140308caoyandong-jquery
            //                '<td style="width:17%;">' + role + '</td>' +
            //                '<td style="width:33.4%;"><div style="width:200px; overflow:hidden;text-overflow:ellipsis;white-space:nowrap;" title="' + $.trim(user_info[2]) + '">' + user_info[2] + '</div></td>' +
            //                '<td style="width:25%;">' + user_info[3] + '</td>' +
            //               '</tr>' +
            //           '</table>' +
            //       '</div>' +
            //       '<div class="cls_Setting_container_user_btn_edit">' + LanguageScript.common_edit + '</div>' +
            //       '<div class="cls_Setting_container_user_btn_del">' + LanguageScript.common_delete + '</div>' +
            //       '<div class="cls_Setting_container_user_btn_reset">' + LanguageScript.common_resetPassword + '</div>';
            //   '</div>';
            //});
            var text = LanguageScript.page_setting_AddNewUserPassword + "：" + "<br>" + password;/*liangjiajie*/
            user_dialog_error( text);
            bind_event();
        },
        error: function () {
            HrefFlag--;
            bind_event();
        }
    });
}
/****************************************************************************/
/****************************************************************************/
/*****************************Setting User***********************************/
/******************************Ma Biao End***********************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/

/****************************************************************************/
/****************************************************************************/
/*****************************Setting Group**********************************/
/******************************Ma Biao Start********************* ***********/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/

//向DB传入数据
function GetGroup() {
    //#756 当分组数据加载前先隐藏编辑删除按钮liangjiajie0318
    $("#group_edit_group").hide();
    $("#group_del_group").hide();
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetGroup",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {

            if (0 == msg.length) {
                var roleID = GetRoleID();
                if (1 != roleID) {
                    Group_ToneDown();
                } else {
                    Group_ToneUp();
                }
                return;
            }
            //#756 当分组数不为零时显示编辑删除按钮liangjiajie0318
            $("#group_edit_group").show();
            $("#group_del_group").show();
            //liangjiajie0318
            /*var obj = eval("(" + msg + ")");*/
            group_left(msg);
            
            GetGroupData(msg[0].pkid);
        },
        error: function () {
            var roleID = GetRoleID();
            if (1 != roleID) {
                Group_ToneDown();
            } else {
                Group_ToneUp();
            }
        }
    });
}


//group 左侧list 单击改变
function group_left(obj) {
    $("#group_group").empty();

    $("#group_group").append(function () {
        var view = "";
        for (var i = 0; i < obj.length; i++) {
            var name = $.trim(obj[i].name);
            if (0 == i) {
                view += '<li id="li_' + obj[i].pkid + '"class="group_li group_choose" title="' + $.trim(name) + '" >' + name + '</li>';
            } else {
                view += '<li id="li_' + obj[i].pkid + '"class="group_li" title="' + $.trim(name) + '">' + name + '</li>';
            }
        }
        return view;
    });
    BindGroup_li("normal");
}

//向DB传入数据
function GetGroupData(groupID) {
    //var CompanyID = GetCompanyID();
    //$.ajax({
    //    type: "POST",
    //    url: "/" + CompanyID + "/Setting/GetGroupData",
    //    contentType: "application/x-www-form-urlencoded",
    //    dataType: "json",
    //    data: "groupID=" + groupID,
    //    success: function (msg) {
    //        /*var obj = eval("(" + msg + ")");*/
    //        GetNotGroupData(groupID,msg);
    //        //group_middle(groupID, msg);
    //        /*group_right(group,obj[1].NotAdded);*/
    //    },
    //    error: function () {
    //        var roleID = GetRoleID();
    //        if (1 != roleID) {
    //            Group_ToneDown();
    //        } else {
    //            Group_ToneUp();
    //        }
    //    }
    //});

    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID + "/Setting/GetGroupData",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: "groupID=" + groupID,
        success: function (msg) {
            var groupData = msg.group_vehicles;
            var notGroupData = msg.not_group_vehicles;
            /*var obj = eval("(" + msg + ")");*/
            /*group_middle(pkid, msg);*/
            group_right(groupID, notGroupData);
            group_middle(groupID, groupData);
            Group_ToneUp();
            var roleID = GetRoleID();
            if (1 != roleID) {
                Group_ToneDown();
            }
        },
        error: function () {
            Group_ToneUp();
            var roleID = GetRoleID();
            if (1 != roleID) {
                Group_ToneDown();
            }
        }
    });
}

//向中间 已添加车辆中插入html
function group_middle(pkid, obj) {
    $("#group_added").empty();
    $("#group_added").append(function () {
        var view = "";
        for (var i = 0; i < obj.length; i++) {
            view += '<li id="' + pkid + '_' + obj[i].pkid + '"class="added_li"  title="' + $.trim(obj[i].name) + '">' + obj[i].name + '</li>';
        }
        return view;
    });
    AddedVehicles();//mabiao 20140310 已添加车辆 点击事件
    //$(".added_li").click(function (e) {
    //    var clickId = $(e.currentTarget).attr('id');
    //    var bCTRL = e.ctrlKey;
    //    var bSHIFT = e.shiftKey
    //    if (bSHIFT) {
    //        var nextlength = $("#" + group_add_id).nextAll("#" + clickId).length;
    //        if (0 != nextlength) {
    //            $("#" + group_add_id).nextUntil("#" + clickId).addClass("added_choose");
    //        }

    //        var prevlength = $("#" + group_add_id).prevAll("#" + clickId).length;
    //        if (0 != prevlength) {
    //            $("#" + group_add_id).prevUntil("#" + clickId).addClass("added_choose");
    //        }

    //        $("#" + clickId).addClass("added_choose");
    //    } else if (bCTRL) {
    //        var judge = $("#" + clickId).hasClass("added_choose");
    //        if (false == judge) {
    //            $("#" + clickId).addClass("added_choose");
    //        } else {
    //            $("#" + clickId).removeClass("added_choose");
    //        }
    //    } else {
    //        var number = $("#group_added").find(".added_li").length;
    //        for (var i = 0; i < number; i++) {
    //            var added_id = $("#group_added").find(".added_li")[i].id;
    //            $("#" + added_id).removeClass("added_choose");
    //        }
    //        $("#" + clickId).addClass("added_choose");
    //        group_add_id = clickId;
    //    }
    //});

}

function group_right(pkid, obj) {
    $("#group_notadded").empty();
    $("#group_notadded").append(function () {
        var view = "";
        for (var i = 0; i < obj.length; i++) {
            view += '<li id="' + pkid + '_' + obj[i].pkid + '"class="notadded_li" title="' + $.trim(obj[i].name) + '">' + obj[i].name + '</li>';
            //liangjiajie
        }
        return view;
    });
    NoAddedVehicles();//mabiao 20140310 未添加车辆 点击事件
    //$(".notadded_li").click(function (e) {
    //    var clickId = $(e.currentTarget).attr('id');
    //    var bCTRL = e.ctrlKey;
    //    var bSHIFT = e.shiftKey
    //    if (bSHIFT) {
    //        var nextlength = $("#" + group_notadd_id).nextAll("#" + clickId).length;
    //        if (0 != nextlength) {
    //            $("#" + group_notadd_id).nextUntil("#" + clickId).addClass("notadded_choose");
    //        }

    //        var prevlength = $("#" + group_notadd_id).prevAll("#" + clickId).length;
    //        if (0 != prevlength) {
    //            $("#" + group_notadd_id).prevUntil("#" + clickId).addClass("notadded_choose");
    //        }
            
    //        $("#" + clickId).addClass("notadded_choose");
    //    } else if (bCTRL) {
    //        var judge = $("#" + clickId).hasClass("notadded_choose");
    //        if (false == judge) {
    //            $("#" + clickId).addClass("notadded_choose");
    //        } else {
    //            $("#" + clickId).removeClass("notadded_choose");
    //        }
    //    } else {
    //        var number = $("#group_notadded").find(".notadded_li").length;
    //        for (var i = 0; i < number; i++) {
    //            var notadded_id = $("#group_notadded").find(".notadded_li")[i].id;
    //            $("#" + notadded_id).removeClass("notadded_choose");
    //        }
    //        $("#" + clickId).addClass("notadded_choose");
    //        group_notadd_id = clickId;
    //    }
    //});
}
//////////////////////////////////////
//Group Tab中 BTN ToneDown 处理
function Group_ToneDown() {
    $("#group_add_vehicle").unbind();
    $("#group_add_vehicle").css("cursor", "default");
    $("#group_add_vehicle").css("background-color", "rgb(177, 172, 172)");
    $("#group_del_vehicle").unbind();
    $("#group_del_vehicle").css("cursor", "default");
    $("#group_del_vehicle").css("background-color", "rgb(177, 172, 172)");
    $("#group_add_group").unbind();
    $("#group_add_group").css("cursor", "default");
    $("#group_add_group").css("background-color", "rgb(177, 172, 172)");
    $("#group_del_group").unbind();
    $("#group_del_group").css("cursor", "default");
    $("#group_del_group").css("background-color", "rgb(177, 172, 172)");
    $("#group_edit_group").unbind();
    $("#group_edit_group").css("cursor", "default");
    $("#group_edit_group").css("background-color", "rgb(177, 172, 172)");
}

//Group Tab中 BTN ToneUp处理
function Group_ToneUp() {
    $("#group_add_vehicle").unbind();
    $("#group_del_vehicle").unbind();
    $("#group_add_group").unbind();
    $("#group_edit_group").unbind();
    //mabiao for 20140304 #593
    $("#group_del_group").unbind();
    //mabiao for 20140304 #593
    group_add_vehicle();
    //$("#group_add_vehicle").css("cursor", "pointer");
    //$("#group_add_vehicle").css("background-color", "#FFF");
    //#756 分组添加和移除车辆不能统一toneup，需要考虑是否有车才toneup

    group_del_vehicle();
    //$("#group_del_vehicle").css("cursor", "pointer");
    //$("#group_del_vehicle").css("background-color", "#FFF");
    //#756 分组添加和移除车辆不能统一toneup，需要考虑是否有车才toneup

    group_add_group();
    $("#group_add_group").css("cursor", "pointer");
    $("#group_add_group").css("background-color", "#FFF");

    group_del_group();
    $("#group_del_group").css("cursor", "pointer");
    $("#group_del_group").css("background-color", "#FFF");

    group_edit_group();
    $("#group_edit_group").css("cursor", "pointer");
    $("#group_edit_group").css("background-color", "#FFF");
}

//Group 添加车辆BTN 绑定事件
function group_add_vehicle() {
//#756 Group 添加车辆BTN tonedown toneup 处理liangjiajie0319
    SetBtnToneDown("group_add_vehicle");
    var roleID = GetRoleID();
    var notadded_num = $("#group_notadded").find(".notadded_li").length;
    if (0 != notadded_num && 1 == roleID ) {
        SetBtnToneUp("group_add_vehicle");
    }
    $("#group_add_vehicle").click(function () {
        SetBtnToneDown("group_add_vehicle");
        var notadded_num = $("#group_notadded").find(".notadded_li").length;
        var pkid_array = new Array();
        var pkid_array_num = 0;
        var group_id = "";
        for (var i = 0; i < notadded_num; i++) {
            var right_id = $("#group_notadded").find(".notadded_li")[i].id;
            var judge = $("#" + right_id).hasClass("notadded_choose");
            if (true == judge) {
                var array = right_id.split("_");
                if (0 == pkid_array_num) {
                    group_id = array[0];
                }
                pkid_array[pkid_array_num] = array[1];
                pkid_array_num++;
            }
        }
        if (0 == pkid_array_num) {
            group_add_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
            //SetBtnToneUp("group_add_vehicle");
            return;
        }
        SendGroupAddVehicle(group_id,pkid_array, pkid_array_num);
    });
}

//**********添加车辆向DB传送数据*******/
//向DB传入数据
function SendGroupAddVehicle(group_id, array, num) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID+"/Setting/GroupAddVehicle",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "Vehicle=" + array +"&GroupID=" + group_id,
        success: function (msg) {
            if ("NG" == msg) {
                var text = LanguageScript.error_e01285;
                OBU_dialog_error(text);//#587提示用户车辆数据变更 复用错误提示框 liangjiajie0325
                group_add_vehicle();
                group_del_vehicle();
                return;
            }
            if ("OK" == msg) {
                for (var i = 0; i < num; i++) {
                    var vehicle_name = $("#" +group_id+ "_" + array[i])[0].innerHTML;
                    $("#" + group_id + "_" + array[i]).unbind();
                    $("#" + group_id + "_" + array[i]).remove();

                    $("#group_added").append(function () {
                        var view = "";
                        var added_vehicle_num = $("#group_added").find(".added_li").length;
                        view += '<li id="' + group_id + '_' + array[i] + '"class="added_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';//liangjiajie0312
                        return view;
                    });
                    //下列注释函数段为对操作后已有车辆列表进行排序#750 liangjiajie20140401
                    //var middle_length = $("#group_added").find("li.added_li").length;
                    //var j = 0
                    //for (j = 0; j < middle_length; j++) {
                    //    var text = $("#group_added").find("li.added_li")[j].innerHTML;
                    //    var str_judge = vehicle_name.localeCompare(text);
                    //    if (str_judge < 0) {
                    //        var vehicleID = $("#group_added").find("li.added_li")[j].id;
                    //        $("#" + vehicleID).before(function () {
                    //            var view = "";
                    //            var added_vehicle_num = $("#group_added").find(".added_li").length;
                    //            view += '<li id="' + group_id + '_' + array[i] + '"class="added_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';//liangjiajie0312
                    //            return view;
                    //        });
                    //        break;
                    //    }
                    //}
                    //if (middle_length == j) {
                    //    $("#group_added").append(function () {
                    //        var view = "";
                    //        var added_vehicle_num = $("#group_added").find(".added_li").length;
                    //        view += '<li id="' + group_id + '_' + array[i] + '"class="added_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';
                    //        //liangjiajie
                    //        return view;
                    //    });
                    //}
                }


                $(".added_li").unbind();
                group_add_id = null;//#750 liangjiajie20140401
                AddedVehicles();//mabiao 20140310 已添加车辆 点击事件
                group_add_vehicle();
                group_del_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
                //SetBtnToneUp("group_add_vehicle");
            }
        },
        error: function () {
            group_add_vehicle();
            group_del_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
            //SetBtnToneUp("group_add_vehicle");
        }
    });
}

//Group 删除车辆BTN 绑定事件
function group_del_vehicle() {
//#756 重新设定车辆添加移除按钮状态liangjiajie0319
    SetBtnToneDown("group_del_vehicle");
    var roleID = GetRoleID();
    var added_num = $("#group_added").find(".added_li").length;
    if (0 != added_num && 1 == roleID) {
        SetBtnToneUp("group_del_vehicle");
    }
    $("#group_del_vehicle").click(function () {
        SetBtnToneDown("group_del_vehicle");
        var added_num = $("#group_added").find(".added_li").length;
       var pkid_array = new Array();
       var pkid_array_num = 0;
       for (var i = 0; i < added_num; i++) {
           var left_id = $("#group_added").find(".added_li")[i].id;
           var judge = $("#" + left_id).hasClass("added_choose");
           if (true == judge) {
               var array = left_id.split("_");
               if (0 == pkid_array_num) {
                   group_id = array[0];
               }
               pkid_array[pkid_array_num] = array[1];
               pkid_array_num++;
           }
       }
       if (0 == pkid_array_num) {
           group_del_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
           //SetBtnToneUp("group_del_vehicle");
           return;
       }
       SendGroupDelVehicle(group_id,pkid_array, pkid_array_num);
    });
}

//**********添加车辆向DB传送数据*******/
//向DB传入数据
function SendGroupDelVehicle(group_id, array, num) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/" + CompanyID+"/Setting/GroupDelVehicle",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "Vehicle=" + array + "&GroupID=" + group_id,
        success: function (msg) {
            if ("OK" == msg) {
			GetGroupData(group_id);
               // for (var i = 0; i < num; i++) {
                  //  var vehicle_name = $("#" +group_id+"_"+ array[i])[0].innerHTML;
                   // $("#" + group_id + "_" + array[i]).unbind();
                   // $("#" + group_id + "_" + array[i]).remove();

                  //  $("#group_notadded").append(function () {
                 //       var view = "";
                  //      var added_vehicle_num = $("#group_notadded").find(".notadded_li").length;
                  //      view += '<li id="' + group_id + '_' + array[i] + '"class="notadded_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';
                  //      return view;
                  //  });
                    //以下注释代码段是对操作后的可添加车辆列表进行排序操作#750 liangjiajie20140401
                    //var right_length = $("#group_notadded").find("li.notadded_li").length;
                    //var j = 0
                    //for (j = 0; j < right_length; j++) {
                    //    var text = $("#group_notadded").find("li.notadded_li")[j].innerHTML;
                    //    var str_judge = vehicle_name.localeCompare(text);
                    //    if (str_judge < 0) {
                    //        var vehicleID = $("#group_notadded").find("li.notadded_li")[j].id;
                    //        $("#" + vehicleID).before(function () {
                    //            var view = "";
                    //            var added_vehicle_num = $("#group_notadded").find(".notadded_li").length;
                    //            view += '<li id="' + group_id + '_' + array[i] + '"class="notadded_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';
                    //            return view;
                    //        });
                    //        break;
                    //    }
                    //}
                    //if (right_length == j) {
                    //    $("#group_notadded").append(function () {
                    //        var view = "";
                    //        var added_vehicle_num = $("#group_notadded").find(".notadded_li").length;
                    //        view += '<li id="' + group_id + '_' + array[i] + '"class="notadded_li" title="' + $.trim(vehicle_name) + '">' + vehicle_name + '</li>';
                    //        return view;
                    //    });
                    //}
               // }

                $(".notadded_li").unbind();
                group_notadd_id = null;//#750 liangjiajie20140401
                NoAddedVehicles();//mabiao 20140310 未添加车辆 点击事件
                //$(".notadded_li").click(function (e) {
                //    var clickId = $(e.currentTarget).attr('id');
                //    var bCTRL = e.ctrlKey;
                //    var bSHIFT = e.shiftKey
                //    if (bSHIFT) {
                //        var nextlength = $("#" + group_notadd_id).nextAll("#" + clickId).length;
                //        if (0 != nextlength) {
                //            $("#" + group_notadd_id).nextUntil("#" + clickId).addClass("notadded_choose");
                //        }

                //        var prevlength = $("#" + group_notadd_id).prevAll("#" + clickId).length;
                //        if (0 != prevlength) {
                //            $("#" + group_notadd_id).prevUntil("#" + clickId).addClass("notadded_choose");
                //        }

                //        $("#" + clickId).addClass("notadded_choose");
                //    } else if (bCTRL) {
                //        var judge = $("#" + clickId).hasClass("notadded_choose");
                //        if (false == judge) {
                //            $("#" + clickId).addClass("notadded_choose");
                //        } else {
                //            $("#" + clickId).removeClass("notadded_choose");
                //        }
                //    } else {
                //        var number = $("#group_notadded").find(".notadded_li").length;
                //        for (var i = 0; i < number; i++) {
                //            var notadded_id = $("#group_notadded").find(".notadded_li")[i].id;
                //            $("#" + notadded_id).removeClass("notadded_choose");
                //        }
                //        $("#" + clickId).addClass("notadded_choose");
                //        group_notadd_id = clickId;
                //    }
                //});
                group_add_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
                group_del_vehicle();
                //SetBtnToneUp("group_del_vehicle");
            }
        },
        error: function () {
            group_add_vehicle();//#756 重新设定车辆添加移除按钮状态liangjiajie0319
            group_del_vehicle();
            //SetBtnToneUp("group_del_vehicle");
        }
    });
}
//判断重名分组liangjiajie0310
function IsGrouprExist(groupName) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/Setting/IsGroupNameExist",
        data: { groupname: groupName },//
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            result = msg;
            //if ("false" == msg) {
            //    return true;
            //} else {
            //    return false;
            //}
        }
    });
    if ("false" == result) {
        return true;
    } else {
        return false;
    }
}

//检测分组名是否与前台存在分组名一致liangjiajie20140328
function CheckGroupName(add_group_name ) {
    var group_num = $("#group_group").find("li.group_li").length;
    for (var i = 0; i < group_num; i++) {
        var groupname = $("#group_group").find("li.group_li")[i].innerHTML;
        if (add_group_name == groupname) {
            return true;
        }
    }
    return false;
}
//wenti
//Group 添加分组BTN 绑定事件
function group_add_group() {

    $(function () {
        var name = $("#group_name");
        $("#group_name").blur(function () {
            var len = $.trim(name.val()).length;
            //分组名称输入正则 更新 liangjiajie 20140329 
            var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,20}$/;
            if (0 == len) {            //判断输入名称为空时处理
                $(".validateTips_group").text(LanguageScript.error_e01230);
                $(".validateTips_group").css("color", "red");
                $("#ui-right_groupadd").hide();
                $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
            } else if (len > 20) {            //判断分组名过长（大于20字节数）
                //$(".validateTips_group").text("分组名过长");
                //$(".validateTips_group").css("color", "red");
                $("#ui-right_groupadd").hide();
                $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
            } else if (!reg.test($.trim(name.val()))) {
                $(".validateTips_group").text(LanguageScript.error_e01274);
                $(".validateTips_group").css("color", "red");
                $("#ui-right_groupadd").hide();
                $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
            } else if (len <= 20 && len > 0) {
                //判断输入名称相同时处理
                //排除 "未分组" 字段保存liangjiajie0321
                //liangjiajie0328 检测输入分组名是否与前台所在分组重名
                if ("未分组" == $.trim(name.val()) || true == CheckGroupName($.trim(name.val()))) {
                    $(".validateTips_group").text(LanguageScript.page_setting_GroupNameExist);
                    $(".validateTips_group").css("color", "red");
                    $("#ui-right_groupadd").hide();
                    $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                } else {
                    $("#ui-right_groupadd").show();
                    $(".validateTips_group").css("margin", "-26px 0px 8px 0px");
                }
            }
            return;
        });
        $("#dialog_form_group").dialog({
            closeOnEscape: false,
            draggable: true, 
            resizable: false, 
            autoOpen: false,
            height: 145,
            width: '31.25%',
            modal: true,
	        position: ["center",250],
            draggabled: false,
            buttons: {
                "保存": function () {
                    var len = $.trim(name.val()).length;
                    //分组名称输入正则 更新 liangjiajie 20140329 
                    var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,20}$/;
                    if (0 == len) {            //判断输入名称为空时处理
                        $(".validateTips_group").text(LanguageScript.error_e01230);
                            $(".validateTips_group").css("color", "red");
                            $("#ui-right_groupadd").hide();
                            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                            return false;
                        } else if (len > 20) {            //判断分组名过长（大于20字节数）
                            //$(".validateTips_group").text("分组名过长");
                            //$(".validateTips_group").css("color", "red");
                            $("#ui-right_groupadd").hide();
                            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                            return false;
                        } else if (!reg.test($.trim(name.val()))) {
                            $(".validateTips_group").text(LanguageScript.error_e01274);
                            $(".validateTips_group").css("color", "red");
                            $("#ui-right_groupadd").hide();
                            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                            return false;
                        } else if (len <= 20 && len > 0) {
                            //判断输入名称相同时处理
                            //排除 "未分组" 字段保存liangjiajie0321
                            if ("未分组" == $.trim(name.val()) || true == CheckGroupName($.trim(name.val())) || false == IsGrouprExist($.trim(name.val()))) {
                                $(".validateTips_group").text(LanguageScript.page_setting_GroupNameExist);
                                $(".validateTips_group").css("color", "red");
                                $("#ui-right_groupadd").hide();
                                $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                                return false;
                            }
                        }
                        SendGroupAddGroup($.trim(name.val()));
                        //#756添加分组后数量至少为1，显示编辑删除按钮liangjiajie0318
                        $("#group_edit_group").show();
                        $("#group_del_group").show();
                        $("#ui-right_groupadd").hide();
                        $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                        //liangjiajie0318

                    $("#group_name").val("");
                    $(this).dialog("close");
                },

                取消: function () {
                    $("#group_name").val("");
                    $(this).dialog("close");
                    $(".validateTips_group").text(LanguageScript.page_setting_GroupNameRule);
                    $(".validateTips_group").css("color", "gray");
                    AddGroupCancel();
                    $("#ui-right_groupadd").hide();
                    $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                }
            }
        });
        $("#group_name").focus(function () {//liangjiajie20140403
            $(".validateTips_group").text(LanguageScript.page_setting_GroupNameRule);
            $(".validateTips_group").css("color", "gray");
            $("#ui-right_groupadd").hide();
            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
        });
        $("#group_add_group")
          .buttonset()
          .click(function () {
              Group_ToneDown();
              BindGroup_li("add");
              //以下代码段是滚动列表至最下端 #750 liangjiajie20140401
              //document.getElementById('group_group').scrollTop = document.getElementById('group_group').scrollHeight;
              $("#dialog_form_group").dialog("open");

          });
    });
}



//AddGroup cancel
function AddGroupCancel() {
    $("#li_add").remove();
    $("#group_edit_sure").remove();
    $("#group_edit_back").remove();
    Group_ToneUp();
    BindGroup_li("normal");
    document.getElementById('group_group').scrollTop = document.getElementById('group_group').scrollHeight;
    //HrefFlag--;
}
//AddGroup Sure
//20140308caoyandong-jquery
//function AddGroupSure(g_name) {
//    var add_group_name = g_name;
//    //liangjiajie27
//    var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,40}$/;
//    //var len = getByteLen(add_group_name);
//    var len = add_group_name.length;
//    if (0 == len) {            //判断输入名称为空时处理
//        var text = "输入的分组名称不能为空！";
//        updateTips("输入的分组名称不能为空！");
//        return false;
//    }else if (!reg.test(add_group_name)) {
//        var text = "分组名称只能是半角的中英文、数字、下划线";
//        user_dialog_error(text);
//        return false;
//    }else  if (len <= 20 && len > 0) {
//        //判断输入名称相同时处理
//        var group_num = $("#group_group").find("li.group_li").length;
//        for (var i = 0; i < group_num; i++) {
//            var groupname = $("#group_group").find("li.group_li")[i].innerHTML;
//            if (add_group_name == groupname) {
//                updateTips("输入的分组名称不能相同！");
//                return;
//            }
//        }
//    }  
    //liangjiajie27

    //SendGroupAddGroup(add_group_name);

    //$("#li_add").remove();
    //$("#group_edit_sure").remove();
    //$("#group_edit_back").remove();
    //Group_ToneUp();
    //BindGroup_li("normal");
    //HrefFlag--;
//}
//**********添加分组向DB传送数据*******/
//向DB传入数据
function SendGroupAddGroup(name) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/"+ CompanyID+"/Setting/GroupAddGroup",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "name=" + name,
        success: function (msg) {
                $("#group_group").append(function () {
                    var view = "";
                    view += '<li id="li_' + msg + '"class="group_li" title="' + $.trim(name) + '">' + name + '</li>';
                    return view;
                });
                var number = $("#group_group").find(".group_li").length;
                for (var i = 0; i < number; i++) {
                    var group_id = $("#group_group").find(".group_li")[i].id;
                    $("#" + group_id).removeClass("group_choose");
                }
                $("#li_" + msg).addClass("group_choose");
                GetGroupData(msg);
                //重新绑定左侧click事件
                BindGroup_li("normal");
                document.getElementById('group_group').scrollTop = document.getElementById('group_group').scrollHeight;
        }
    });
}
//20140308caoyandong-jquery
//Group 删除分组BTN 绑定事件
function group_del_group() {
    $("#group_del_group").click(function () {
        group_dialog_reset_sure();
    });
}
//dialog正常处理，用户再次确认后 删除分组
function group_dialog_reset_sure() {
    var group_num = $("#group_group").find(".group_li").length;
    if (0 == group_num) {
        return;
    }

    for (var i = 0; i < group_num; i++) {
        var groupID = $("#group_group").find(".group_li")[i].id
        var judge = $("#" + groupID).hasClass("group_choose");
        if (true == judge) {
            var delgroupname = $("#group_group").find(".group_li")[i].innerHTML;
            break;
        }
    }

    $(".setting_group_del")[0].innerHTML =
        '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;" title="' + delgroupname + '">' + LanguageScript.page_setting_Group + ':' + delgroupname + '</p>' +
        '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;margin-top:1em;">' + LanguageScript.page_setting_DiaDelGroup + '</p>';


    $(function () {
        $(".setting_group_del").dialog({
            resizable: false,
            height: 140,
            position: ["center",250],
            modal: true,
            buttons: {
                "确定": function () {
                    var group_num = $("#group_group").find(".group_li").length;
                    for (var i = 0; i < group_num; i++) {
                        var groupID = $("#group_group").find(".group_li")[i].id
                        var judge = $("#" + groupID).hasClass("group_choose");
                        if (true == judge) {
                            var array = groupID.split("_");
                            SendGroupDelGroup(array[1]);
                            break;
                        }
                    }
                    //SendGroupDelGroup(array[1]);
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });

    //$("#body_position").before(function () {
    //    return "<div id= 'dialog_background'></div>" +
    //           "<div id= 'dialog' >" +
    //              "<div class='dialog_title'>" + title + "</div>" +
    //              "<div class='dialog_text'>" + text + "</div>" +
    //              "<div id='user_sure' class='cls_dialog_sure'>确定</div>" +
    //              "<div id='user_back' class='cls_dialog_sure'>返回</div>" +
    //    "</div>";
    //})
    //$("#user_back").click(function () {
    //    $("#dialog_background").remove();
    //    $("#dialog").remove();
    //})
    //$("#user_sure").click(function () {
    //    var group_num = $("#group_group").find(".group_li").length;
    //    for (var i = 0; i < group_num; i++) {
    //        var groupID = $("#group_group").find(".group_li")[i].id
    //        var judge = $("#" + groupID).hasClass("group_choose");
    //        if (true == judge) {
    //            var array = groupID.split("_");
    //            SendGroupDelGroup(array[1]);
    //            break;
    //        }
    //    }
    //    $("#dialog_background").remove();
    //    $("#dialog").remove();
    //})
}

//**********删除分组向DB传送数据*******/
//向DB传入数据
function SendGroupDelGroup(pkid) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/"+CompanyID+"/Setting/GroupDelGroup",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "pkid=" + pkid,
        success: function (msg) {
            if ("OK" == msg) {
                //GetGroup();
                $("#li_" + pkid).remove();
                $("#group_added").empty();
                $("#group_notadded").empty();
                
                var group_num = $("#group_group").find(".group_li").length;
                if (0 == group_num) {
                    //#756 分组数为零时隐藏编辑删除按钮liangjiajie0318
                    $("#group_edit_group").hide();
                    $("#group_del_group").hide();
                    //#756 分组数为零需要加载车辆添加删除设置函数liangjiajie0318
                    group_add_vehicle();
                    group_del_vehicle();
                    return;
                }
                for (var i = 0; i < group_num; i++) {
                    $("#group_group").find(".group_li")[i].className = 'group_li';
                }
                var group_first_id = $("#group_group").find(".group_li")[0].id;
                $("#" + group_first_id).addClass("group_choose");
                var group = group_first_id.split("_");
                GetGroupData(group[1]);
                document.getElementById('group_group').scrollTop = 0;
            }
        }
    });
}
//wenti
//Group 编辑分组BTN 绑定事件
function group_edit_group() {
    if (0 == $("#group_group").children(".group_li").length) {
        return;
    }
    var groupPkid = $("#group_group").children(".group_choose")[0].id;
        $(function () {
            var newname = $("#group_name_edit");
            $("#group_name_edit").blur(function(){
                var save_value = groupPkid;
                var array_save = save_value.split("_");
                if ($.trim(newname.val()) != $.trim($("#group_group").children(".group_choose")[0].innerHTML)) {
                    var len = $.trim(newname.val()).length;
                    //分组名称输入正则 更新 liangjiajie 20140329 
                    var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,20}$/;
                    if (0 == len) {            //判断输入名称为空时处理
                        $(".validateTips_group").text(LanguageScript.error_e01230);
                        $(".validateTips_group").css("color", "red");
                        $("#ui-right_groupedit").hide();
                        $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                    } else if (len > 20) {            //判断分组名过长（大于20字节数）
                        //$(".validateTips_group").text("分组名过长");
                        //$(".validateTips_group").css("color", "red");
                        $("#ui-right_groupedit").hide();
                        $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                    } else if (!reg.test($.trim(newname.val()))) {
                        $(".validateTips_group").text(LanguageScript.error_e01274);
                        $(".validateTips_group").css("color", "red");
                        $("#ui-right_groupedit").hide();
                        $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                    } else if (len <= 20 && len > 0) {
                        //判断输入名称相同时处理
                        //排除"未分组"字段liangjiajie0321
                        //liangjiajie0328 检测输入分组名是否与前台分组重名和输入规则检测
                        if ("未分组" == $.trim(newname.val()) || true == CheckGroupName($.trim(newname.val()))) {
                            $(".validateTips_group").text(LanguageScript.page_setting_GroupNameExist);
                            $(".validateTips_group").css("color", "red");
                            $("#ui-right_groupedit").hide();
                            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                        } else {
                            $("#ui-right_groupedit").show();
                            $(".validateTips_group").css("margin", "-26px 0px 8px 0px");
                        }
                    }
                } else {
                    $("#ui-right_groupedit").show();
                    $(".validateTips_group").css("margin", "-26px 0px 8px 0px");
                }
                return;
            });
            $("#dialog_form_group_edit").dialog({
                draggable: true, 
                resizable: false, 
                autoOpen: false,
                closeOnEscape: false,
                height: 145,
                width: '31.25%',
                modal: true,
                position: ["center",250],
                draggabled: false,
                buttons: {
                    "保存": function () {
                        var save_value = groupPkid;
                        var array_save = save_value.split("_");
                        if ($.trim(newname.val()) != $.trim($("#group_group").children(".group_choose")[0].innerHTML)) {
                            var len = $.trim(newname.val()).length;
                            //分组名称输入正则 更新 liangjiajie 20140329 
                            var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,20}$/;
                            if (0 == len) {            //判断输入名称为空时处理
                                $(".validateTips_group").text(LanguageScript.error_e01230);
                                $(".validateTips_group").css("color", "red");
                                return false;
                            } else if (len > 20) {            //判断分组名过长（大于20字节数）
                                //$(".validateTips_group").text("分组名过长");
                                //$(".validateTips_group").css("color", "red");
                                return false;
                            } else if (!reg.test($.trim(newname.val()))) {
                                $(".validateTips_group").text(LanguageScript.error_e01274);
                                $(".validateTips_group").css("color", "red");
                                return false;
                            } else if (len <= 20 && len > 0) {
                                //判断输入名称相同时处理
                                //排除"未分组"字段liangjiajie0321
                                if ("未分组" == $.trim(newname.val()) || true == CheckGroupName($.trim(newname.val()))|| false == IsGrouprExist($.trim(newname.val()))) {
                                    $(".validateTips_group").text(LanguageScript.page_setting_GroupNameExist);
                                    $(".validateTips_group").css("color", "red");
                                    return false;
                                }
                            }
                            SendGroupEditGroup(array_save[1], $.trim(newname.val()));
                            $(this).dialog("close");
                            $("#ui-right_groupedit").hide();
                            $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                        }
                        else {
                            $(this).dialog("close");
                        }
                        Group_ToneUp();
                        BindGroup_li("normal");
                    },

                    取消: function () {
                        $("#group_name_edit").val("");
                        $(".validateTips_group").text(LanguageScript.page_setting_GroupNameRule);
                        $(".validateTips_group").css("color", "gray");
                        $(this).dialog("close");
                        Group_ToneUp();
                        BindGroup_li("normal");
                        $("#ui-right_groupedit").hide();
                        $(".validateTips_group").css("margin", "-13px 0px 8px 0px");
                    }
                }
            });
            $("#group_name_edit").focus(function () {
                $(".validateTips_group").text(LanguageScript.page_setting_GroupNameRule);
                $(".validateTips_group").css("color", "gray");
            });
            $("#group_edit_group")
              .buttonset()
              .click(function () {
                  Group_ToneDown();
                  BindGroup_li("eidt");
                  
                  $("#dialog_form_group_edit").dialog("open");
                  
                  $("#group_name_edit").val($("#group_group").children(".group_choose")[0].innerHTML);
                  document.getElementById('group_group').scrollTop = document.getElementById('group_group').scrollHeight;
                  $("#ui-right_groupedit").show();
                  $(".validateTips_group").css("margin", "-26px 0px 8px 0px");
              });
        });
}
function UserEditOnblur() {
    if (undefined == $("#li_add")[0]) {
        return;
    }
    HrefFlag--;
    var save_value = $("#li_add")[0].name;
    var array_save = save_value.split(",");
    var add_group_name = array_save[1];
    $("#li_add").replaceWith(function () {
        return '<li id="li_' + array_save[0] + '"class="group_li group_choose" title="' + $.trim(add_group_name) + '">' + add_group_name + '</li>';
    });
    $("#group_add_group").show();
    $("#group_edit_group").show();
    $("#group_del_group").show();
    $("#group_edit_sure").remove();
    $("#group_edit_back").remove();
    Group_ToneUp();
    BindGroup_li("normal");
}
function BindGroup_li(type) {
    $(".group_li").unbind();
    $(".group_li").click(function (e) {
        var clickId = $(e.currentTarget).attr('id');
        if (clickId == "li_add") {
            return;
        }
        if ("edit" == type) {
            UserEditOnblur();
            return;
        } else if ("add" == type) {
            if ($("#li_add")[0].value == "" ) {
                AddGroupCancel();
            } else {
                var title = " 分 组";
                var text = LanguageScript.common_DiaConEdit;
                user_dialog_add(title, text);
            }

        }
        var number = $("#group_group").find(".group_li").length;
        for (var i = 0; i < number; i++) {
            var group_id = $("#group_group").find(".group_li")[i].id;
            $("#" + group_id).removeClass("group_choose");
        }
        $("#" + clickId).addClass("group_choose");
        var group = clickId.split("_");
        GetGroupData(group[1]);
    });
}
//dialog正常处理 用户再次确认删除用户
function user_dialog_add(title, text) {
    $("#body_position").before(function () {
        return "<div id= 'dialog_background'></div>" +
               "<div id= 'dialog' >" +
                  "<div class='dialog_title'>" + title + "</div>" +
                  "<div class='dialog_text'>" + text + "</div>" +
                  "<div id='user_sure' class='cls_dialog_sure'>" + LanguageScript.common_save + "</div>" +
                  "<div id='user_back' class='cls_dialog_sure'>" + LanguageScript.common_cancel + "</div>" +
        "</div>";
    })
    $("#user_back").click(function () {
        $("#dialog_background").remove();
        $("#dialog").remove();
    })
    $("#user_sure").click(function () {
        $("#dialog_background").remove();
        $("#dialog").remove();
        AddGroupCancel();
        
    })
}


//**********编辑分组向DB传送数据*******/
//向DB传入数据
function SendGroupEditGroup(pkid, name) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        url: "/"+CompanyID+"/Setting/GroupEditGroup",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "pkid="+pkid+"&name=" + name,
        success: function (msg) {
            if ("OK" == msg) {
                $("#li_"+ pkid).replaceWith(function () {
                    return '<li id="li_' + pkid + '"class="group_li group_choose" title="' + $.trim(name) + '">' + name + '</li>';
                });
                BindGroup_li("normal");
            }
        }
    });
}
//已添加车辆时 点击触发事件 
//mabiao 20140310 
function AddedVehicles() {
    $(".added_li").click(function (e) {
        var clickId = $(e.currentTarget).attr('id');
        var bCTRL = e.ctrlKey;
        var bSHIFT = e.shiftKey;
	//#750 liangjiajie20140401
        var number = $("#group_added").find(".added_li").length;
        if (bSHIFT &&  null != group_add_id) {
            //先清空被选中 liangjiajie20140401
            for (var i = 0; i < number; i++) {
                var added_id = $("#group_added").find(".added_li")[i].id;
                $("#" + added_id).removeClass("added_choose");
            }
            $("#" + group_add_id).addClass("added_choose");
            var nextlength = $("#" + group_add_id).nextAll("#" + clickId).length;
            if (0 != nextlength) {
                $("#" + group_add_id).nextUntil("#" + clickId).addClass("added_choose");
            }

            var prevlength = $("#" + group_add_id).prevAll("#" + clickId).length;
            if (0 != prevlength) {
                $("#" + group_add_id).prevUntil("#" + clickId).addClass("added_choose");
            }
            $("#" + clickId).addClass("added_choose");
        } else if (bCTRL) {
            var judge = $("#" + clickId).hasClass("added_choose");
            if (false == judge) {
                $("#" + clickId).addClass("added_choose");
            } else {
                $("#" + clickId).removeClass("added_choose");
            }
        } else {
            for (var i = 0; i < number; i++) {
                var added_id = $("#group_added").find(".added_li")[i].id;
                $("#" + added_id).removeClass("added_choose");
            }
            $("#" + clickId).addClass("added_choose");
            group_add_id = clickId;
        }
    });
}


//已添加车辆时 点击触发事件 
//mabiao 20140310 
function NoAddedVehicles() {
    $(".notadded_li").click(function (e) {
        var clickId = $(e.currentTarget).attr('id');
        var bCTRL = e.ctrlKey;
        var bSHIFT = e.shiftKey;
	//#750 liangjiajie20140401
	    var number = $("#group_notadded").find(".notadded_li").length;
        if (bSHIFT && null != group_notadd_id) {
            //先清空被选中 liangjiajie20140401
            for (var i = 0; i < number; i++) {
                var notadded_id = $("#group_notadded").find(".notadded_li")[i].id;
                $("#" + notadded_id).removeClass("notadded_choose");
            }
            $("#" + group_notadd_id).addClass("notadded_choose");
            var nextlength = $("#" + group_notadd_id).nextAll("#" + clickId).length;
            if (0 != nextlength) {
                $("#" + group_notadd_id).nextUntil("#" + clickId).addClass("notadded_choose");
            }

            var prevlength = $("#" + group_notadd_id).prevAll("#" + clickId).length;
            if (0 != prevlength) {
                $("#" + group_notadd_id).prevUntil("#" + clickId).addClass("notadded_choose");
            }

            $("#" + clickId).addClass("notadded_choose");
        } else if (bCTRL) {
            var judge = $("#" + clickId).hasClass("notadded_choose");
            if (false == judge) {
                $("#" + clickId).addClass("notadded_choose");
            } else {
                $("#" + clickId).removeClass("notadded_choose");
            }
        } else {
            for (var i = 0; i < number; i++) {
                var notadded_id = $("#group_notadded").find(".notadded_li")[i].id;
                $("#" + notadded_id).removeClass("notadded_choose");
            }
            $("#" + clickId).addClass("notadded_choose");
            group_notadd_id = clickId;
        }
    });
}
/****************************************************************************/
/****************************************************************************/
/*****************************Setting Group**********************************/
/******************************Ma Biao End***********************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/

