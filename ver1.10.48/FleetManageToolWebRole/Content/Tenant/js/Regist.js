function scroll() {
    var width = 0;
    if (window.innerWidth)
        width = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        width = document.body.clientWidth;
}
window.onload = function () {
    scroll();
}
window.onresize = function () {
    scroll();
}

$.ajaxSetup({
    statusCode: {
        499: function (data) {
            window.location.reload();
        }
        , 599: function (data) {
            alert(LanguageScript.page_common_Role_Change);
            window.location.href = "/";
        }
    }
});

function remindIDInput() {
    $("#show_urlInfo").hide();
    $("#companyIDIntro").show();
}

//Add by LiYing 2014-4-25 Start
function companyIdErrorNULL() {
    if ($("#companyIDError").length == 0) {
        $("#show_urlInfo").append('<div id="companyIDError"></div>');
    }
}

function companyIDRightNULL() {
    if ($("#companyIDRight").length == 0) {
        $("#show_urlInfo").append('<img id="companyIDRight" style="" src="/Content/Tenant/images/Right.png"></img>');
    }
}
//Add by LiYing 2014-4-25 End
function showURL() {
    var urlID = $.trim($("#companyID").val());
    //var urlID = $("#companyID").val().trim();
    var reg = /^[a-zA-Z0-9/_/-]{1,20}$/;
    var result = "";
    //chenyangwen 2014/3/6
    if ("hck-fleetadmin" == urlID || "Register" == urlID) {
        $("#company_Admin").text("");
        $("#company_Admin").hide();
        $("#company_Admin_tip").hide();
        $("#companyIDIntro").hide();
        $("#show_urlInfo").show();
        $("#companyIDRight").hide();
        companyIdErrorNULL();
        $("#companyIDError").show();
        $("#companyIDError").text(LanguageScript.error_e01242);
        $("#companyIDError").css("color", "red");
        return false;
    }
    if ("null" == urlID.toLocaleLowerCase()) {
        $("#company_Admin").text("");
        $("#company_Admin").hide();
        $("#company_Admin_tip").hide();
        $("#companyIDIntro").hide();
        $("#show_urlInfo").show();
        $("#companyIDRight").hide();
        companyIdErrorNULL();
        $("#companyIDError").show();
        $("#companyIDError").text(LanguageScript.error_e01243);
        $("#companyIDError").css("color", "red");
        return false;
    }
    if (reg.test(urlID)) {
        $.ajax({
            type: "POST",
            async: false,
            url: "/hck-fleetadmin/IsCompanyIDExist",
            data: { companyID: urlID },
            contentType: "application/x-www-form-urlencoded",
            dataType: "text",
            success: function (msg) {
                result = msg;
                if ("false" == msg) {
                    $("#companyIDIntro").hide();
                    $("#show_urlInfo").show();
                    $("#companyIDError").text("");
                    $("#companyIDError").hide();
                    companyIDRightNULL();
                    $("#companyIDRight").show();
                    $("#company_Admin").show();
                    $("#company_Admin").text(urlID);
                    $("#company_Admin_tip").show();
                    /*$("#show_urlInfo").text("您的网址：http://www.ihpleDFleetManagerTool.com/" + urlID);
                    $("#show_urlInfo").css("font-size", "8pt");
                    $("#show_urlInfo").css("top", "2px");
                    $("#show_urlInfo").css("color", "#888");*/
                } else {
                    $("#company_Admin").text("");
                    $("#company_Admin").hide();
                    $("#company_Admin_tip").hide();
                    $("#companyIDIntro").hide();
                    $("#show_urlInfo").show();
                    $("#companyIDRight").hide();
                    companyIdErrorNULL();
                    $("#companyIDError").show();
                    $("#companyIDError").text(LanguageScript.error_e01242);
                    $("#companyIDError").css("font-size", "10pt");
                    $("#companyIDError").css("color", "red");
                }
            },
            error: function () {
                $("#company_Admin").text("");
                $("#company_Admin").hide();
                $("#company_Admin_tip").hide();
                $("#show_urlInfo").text("");
                $("#companyIDRight").hide();
                companyIdErrorNULL();
                $("#companyIDError").show();
                return false;
            }
        });
        if ("false" == result) {
            return true;
        } else {
            return false;
        }
    } else if (urlID == "") {
        $("#company_Admin").text("");
        $("#company_Admin").hide();
        $("#company_Admin_tip").hide();
        $("#companyIDIntro").hide();
        $("#show_urlInfo").show();
        $("#companyIDRight").hide();
        companyIdErrorNULL();
        $("#companyIDError").show();
        $("#companyIDError").text(LanguageScript.page_tenant_EnterCompanyID);
        $("#companyIDError").css("color", "red");
        return false;
    } else {
        $("#company_Admin").text("");
        $("#company_Admin").hide();
        $("#company_Admin_tip").hide();
        $("#companyIDIntro").hide();
        $("#show_urlInfo").show();
        $("#companyIDRight").hide();
        companyIdErrorNULL();
        $("#companyIDError").show();
        $("#companyIDError").text(LanguageScript.error_e01245);
        $("#companyIDError").css("color", "red");
        return false;
    }
}

function isPassword() {

    //密码输入框规则更新 20140329 liangjiajie
    var reg = /^[a-zA-Z0-9\_\%\^\(\)\-\+\=\~\@\$\!\*\&\#\,\.]{6,20}$/;
    var numReg = /[0-9]/;
    //var password = $("#companyPwd").val().trim();
    var password = $("#companyPwd").val();
    if (reg.test(password) && numReg.test(password)) {
        $("#PasswordIntro").hide();
        $("#show_pwdInfo1").show();
        $("#pwdright").show();
        $("#show_pwdInfo1_Inner").hide();
        $("#show_pwdInfo1_Inner_Empty").hide();
        return true;
    } else if (0 == password.length) {
        //liangjiajie27
        $("#PasswordIntro").hide();
        $("#show_pwdInfo1").show();
        $("#show_pwdInfo1_Inner_Empty").show();
        $("#pwdright").hide();
        $("#show_pwdInfo1_Inner").hide();
        return false;
    }else {
        $("#PasswordIntro").hide();
        $("#show_pwdInfo1").show();
        $("#pwdright").hide();
        $("#show_pwdInfo1_Inner").show();
        $("#show_pwdInfo1_Inner_Empty").hide();
        return false;
    }
}

function RemindPasswordInput() {
    $("#show_pwdInfo1").hide();
    $("#PasswordIntro").show();
}

function remindAlertEmailInput() {
    $("#alertEmail").hide();
    $("#AlertEmailIntro").show();
}

// 判断数组中重复数据(因为共通代码的Ready有取得u_right的方法，该页面没有这个对象，如果使用共通会报错，所以不能使用共通中的该方法)
function getRepeat(arr) {

    var arrSort = arr.sort();
    var returnVal = -1;
    $.each(arrSort, function (index, val) {

        if (index == arrSort.length) {

            return false;
        }
        if (arrSort[index] == arrSort[index + 1]) {

            returnVal = $.inArray(arrSort[index], arr);
            // 退出each循环
            return false;
        }
    });

    return returnVal;
}

function isAlertEmail() {
    //var email = $("#companyEmail").val().trim();
    var email = $.trim($("#alertEmailInput").val());
    //chenyangwen
    var reg = /^([a-zA-Z0-9_\-\.]){1,}@([a-z0-9A-Z]?[a-z0-9A-Z]+)+(((\.\w+))*)+[\.][a-z]{2,4}$/;//liangjiajie
    var arremail = email.split(';');
    if (0 == arremail.length || 1 == arremail.length) {
        if (1 == arremail.length && arremail[0] != "") {
            email = $.trim(arremail[0]);
            email = email.replace(/\n/g,"");
        }
        if ("" == email) {
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            $("#alertEmailError").text(LanguageScript.page_tenant_EnterNoticeMail);
            return false;
        } else if (!reg.test(email)) {
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            $("#alertEmailError").text(LanguageScript.common_CorrectFormatMail);
            return false;
        } else if(email.length > 50){
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            $("#alertEmailError").text(LanguageScript.error_e01246);
            return false;
        }else if (reg.test(email)) {
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailError").hide();
            $("#alertEmailRight").show();
            $("#hideRemindEmail").val(email);
            return true;
        }  
    } else {
        var emailstr = "";
        var flag = 1;
        var index = -1;
        for (var i = 0; i < arremail.length; ++i) {
            arremail[i] = $.trim(arremail[i]);
            var emailTemp = arremail[i];
            emailTemp = emailTemp.replace(/\n/g,"");
            if ("" == emailTemp) {
                if (0 != flag) {
                    flag = 1
                }
                continue;
            }else if(emailTemp.length > 50)
            {
                flag = 3;
                index = i;
                break;
            } else if (reg.test(emailTemp)) {
                emailstr += emailTemp + ";";
                flag = 0;
            }else {
                flag = 2;
                index = i;
                break;
            }
        }

        if (0 == flag) {
            //chenyangwen 2014/03/04
            var repeat = getRepeat(arremail);
            if (-1 == repeat) {
                $("#AlertEmailIntro").hide();
                $("#alertEmail").show();

                $("#alertEmailError").hide();
                $("#alertEmailRight").show();
                $("#hideRemindEmail").val(emailstr);
                return true;
            } else {
                var temp = arremail[repeat];
                if (temp != "") {
                    $("#AlertEmailIntro").hide();
                    $("#alertEmail").show();

                    $("#alertEmailRight").hide();
                    $("#alertEmailError").show();
                    $("#alertEmailError").text(LanguageScript.common_Mail + ":" + temp + LanguageScript.error_e01247);
                    return false;
                } else {
                    $("#AlertEmailIntro").hide();
                    $("#alertEmail").show();

                    $("#alertEmailError").hide();
                    $("#alertEmailRight").show();
                    $("#hideRemindEmail").val(emailstr);
                    return true;
                }
            }
            //chenyangwen 2014/03/04
        } else if (2 == flag) {
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            var temp = arremail[index];
            $("#alertEmailError").text(LanguageScript.common_Mail + ":" + temp + LanguageScript.error_e01248);
            return false;
        }else if(3 == flag){
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            var temp = arremail[index];
            $("#alertEmailError").text(LanguageScript.error_e01246);
            return false;
        } else {
            $("#AlertEmailIntro").hide();
            $("#alertEmail").show();

            $("#alertEmailRight").hide();
            $("#alertEmailError").show();
            $("#alertEmailError").text(LanguageScript.common_NoMailInput);
            return false;
        }
    }
}

function remindEmailInput() {
    $("#companyEmailIntro").show();
    $("#show_emailInfo").hide();
}

function isEmail() {
    //var email = $("#companyEmail").val().trim();
    var email = $.trim($("#companyEmail").val());
    //chenyangwen
    var reg = /^([a-zA-Z0-9_\-\.]){1,}@([a-z0-9A-Z]?[a-z0-9A-Z]+)+(((\.\w+))*)+[\.][a-z]{2,4}$/;  //liangjiajie
    if ("" == email) {
        $("#companyEmailIntro").hide();
        $("#show_emailInfo").show();
        $("#emailright").hide();
        $("#emailerror").show();
        $("#emailerror").text(LanguageScript.common_NoMailInput);
        return false;
    } else if (reg.test(email)) {
        $("#companyEmailIntro").hide();
        $("#show_emailInfo").show();
        $("#emailerror").hide();
        $("#emailright").show();
        return true;
    } else {
        $("#companyEmailIntro").hide();
        $("#show_emailInfo").show();
        $("#emailright").hide();
        $("#emailerror").show();
        $("#emailerror").text(LanguageScript.common_CorrectFormatMail);
        return false;
    }
}

function remindTelInput() {
    $("#companyTelIntro").show();
    $("#show_telInfo").hide();
}

function isTelphone() {
    //var telphone = $("#companyTel").val().trim();
    var telphone = $.trim($("#companyTel").val());
    var reg = /(^(\d{3,4}-)?\d{7,8})$|(^(\d{11}))$/;//电话号码
    
    if ("" == telphone) {
        $("#companyTelIntro").hide();
        $("#show_telInfo").show();
        $("#telright").hide();
        $("#telerror").show();
        $("#telerror").text("");
        $("#companyTel").val(telphone);
        return true;
    } else if (reg.test(telphone)) {
        $("#companyTelIntro").hide();
        $("#show_telInfo").show();
        $("#telerror").hide();
        $("#telright").show();
        return true; 
    } else {
        $("#companyTelIntro").hide();
        $("#show_telInfo").show();
        $("#telright").hide();
        $("#telerror").show();
        $("#telerror").text(LanguageScript.common_CorrectFormatTel);
        return false;
    }
}

function isEqualPwd() {
    var oldPassword = $("#companyPwd").val();
    var newPassword = $("#confirmPassword").val();
    if (oldPassword == newPassword) {
        if ("" == oldPassword) {
            //liangjiajie27
            $("#ConfirmIntro").hide();
            $("#show_pwdInfo2").show();
            $("#pwdequalright").hide();
            $("#pwdequalerror").hide();
            $("#pwdequalerror_Empty").show();
            return false;
        }
        $("#ConfirmIntro").hide();
        $("#show_pwdInfo2").show();
        $("#pwdequalerror").hide();
        $("#pwdequalright").show();
        $("#pwdequalerror_Empty").hide();
        return true;
    } else if ("" == newPassword) {
        //liangjiajie27
        $("#ConfirmIntro").hide();
        $("#show_pwdInfo2").show();
        $("#pwdequalright").hide();
        $("#pwdequalerror").hide();
        $("#pwdequalerror_Empty").show();
	    return false;
    } else {
        $("#ConfirmIntro").hide();
        $("#show_pwdInfo2").show();
        $("#pwdequalright").hide();
        $("#pwdequalerror").show();
        $("#pwdequalerror_Empty").hide();
        return false;
    }
}

function InputIsEqualPwd() {
    var oldPassword = $("#companyPwd").val();
    var newPassword = $("#confirmPassword").val();
    if ("" == newPassword ) {
        return;
    } else if (oldPassword != newPassword) {
        //liangjiajie27
        $("#ConfirmIntro").hide();
        $("#show_pwdInfo2").show();
        $("#pwdequalright").hide();
        $("#pwdequalerror").show();
        $("#pwdequalerror_Empty").hide();
        return false;
    } else {
        $("#ConfirmIntro").hide();
        $("#show_pwdInfo2").show();
        $("#pwdequalerror").hide();
        $("#pwdequalright").show();
        $("#pwdequalerror_Empty").hide();
        return true;
    }
}

function RemindConfirmInput() {
    $("#ConfirmIntro").show();
    $("#show_pwdInfo2").hide();
}

function remindNameInput() {
    $("#show_NameInfo").hide();
    $("#companyNameIntro").show();
}

function isTooLong() {
    var companyName = $("#companyName").val();
    //companyName = companyName.trim();
    companyName = $.trim(companyName);
    var reg = /^[a-zA-Z0-9\_\s\,\.\u4e00-\u9fa5]{1,40}$/;
    var len = companyName.length;
    if (0 == len) {
        $("#companyNameIntro").hide();
        $("#show_NameInfo").show();
        $("#compantNameright").hide();
        $("#companyNameError").show();
        $("#companyNameError").text(LanguageScript.common_EnterCompanyName);
        return false;
    } else if (!reg.test(companyName)) {
        $("#companyNameIntro").hide();
        $("#show_NameInfo").show();
        $("#compantNameright").hide();
        $("#companyNameError").show();
        $("#companyNameError").text(LanguageScript.error_e01249);
        return false;
    }else if(len > 40){
        $("#companyNameIntro").hide();
        $("#show_NameInfo").show();
        $("#compantNameright").hide();
        $("#companyNameError").show();
        $("#companyNameError").text(LanguageScript.error_e01250);
    }  else if (reg.test(companyName) && len <= 40 && len > 0) {
            $.ajax({
                type: "POST",
                async: false,
                url: "/hck-fleetadmin/IsCompanyNameExist",
                data: { companyName: companyName },
                contentType: "application/x-www-form-urlencoded",
                dataType: "text",
                success: function (msg) {
                    result = msg;
                    if ("false" == msg) {
                        $("#companyNameIntro").hide();
                        $("#show_NameInfo").show();
                        $("#companyNameError").text("");
                        $("#companyNameError").hide();
                        $("#compantNameright").show();
                    } else {
                        $("#companyNameIntro").hide();
                        $("#show_NameInfo").show();
                        $("#compantNameright").hide();
                        $("#companyNameError").show();
                        $("#companyNameError").text(LanguageScript.error_e01251);
                        $("#companyNameError").css("color", "red");
                    }
                }
            });
            if ("false" == result) {
                return true;
            } else {
                return false;
            }
    }
}

function isValidateCode() {
    var inputCode = $("#validateCode").val();
    $.ajax({
        type: "POST",
        async: false,
        url: "/hck-fleetadmin/IsValidateRight",
        data: { inputCode: inputCode },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            result = msg;
            if ("true" == msg) {
                $("#ValidateIntro").hide();
                $("#show_codeInfo").show();
                $("#codeerror").hide();
                $("#coderight").show();
            } else {
                $("#ValidateIntro").hide();
                $("#show_codeInfo").show();
                $("#coderight").hide();
                $("#codeerror").show();
            }
        },
        error: function () {
            $("#ValidateIntro").hide();
            $("#show_urlInfo").text("");
            return false;
         }
    });
    if ("true" == result) {
        return true;
    } else {
        return false;
    }
}

function RemindCodeInput() {
    $("#show_codeInfo").hide();
    $("#ValidateIntro").show();

}

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

function RemindIntro() {
    $("#show_IntroErrorInfo").hide();
    $("#show_IntroInfo").show();
}

function isESNCode() {
    var esn = $.trim($("#esncode").val());
    var reg = /^[a-zA-Z0-9\-]{1,17}$/;
    if ("" == esn) {
        $("#EsnIntro").hide();
        $("#show_esnInfo").show();
        $("#esnright").hide();
        $("#esnerror").show();
        $("#esnerror").text(LanguageScript.page_register_ESN_NoEmpty);
        return false;
    } else if (!reg.test(esn)) {
        $("#EsnIntro").hide();
        $("#show_esnInfo").show();
        $("#esnright").hide();
        $("#esnerror").show();
        $("#esnerror").text(LanguageScript.page_register_ESN_Validate);
        return false;
    } else {
        $("#EsnIntro").hide();
        $("#show_esnInfo").show();
        $("#esnerror").hide();
 //       $("#esnright").show(); /* liying for regist obu failed*/
        return true;
    }
}

function remindEsn() {
    $("#show_esnInfo").hide();
    $("#EsnIntro").show();
}

function isRegiserKey() {
    var esn = $.trim($("#regkey").val());
    var reg = /^[a-zA-Z0-9\-]{1,17}$/;
    if ("" == esn) {
        $("#RegisterKeyIntro").hide();
        $("#show_RegisterKeyInfo").show();
        $("#registerkeyright").hide();

        $("#registerkeyerror").show();
        $("#registerkeyerror").text(LanguageScript.page_register_Register_NoEmpty);
        return false;
    } else if (!reg.test(esn)) {
        $("#RegisterKeyIntro").hide();
        $("#show_RegisterKeyInfo").show();
        $("#registerkeyright").hide();
        $("#registerkeyerror").show();
        $("#registerkeyerror").text(LanguageScript.page_register_Register_Validate);
        return false;
    } else {
        $("#RegisterKeyIntro").hide();
        $("#show_RegisterKeyInfo").show();
        $("#registerkeyerror").hide();
  //      $("#registerkeyright").show(); /* liying for regist obu failed*/
        return true;
    }
}

function remindRegisterKey() {
    $("#show_RegisterKeyInfo").hide();
    $("#RegisterKeyIntro").show();
}

function unicode(s) {
    var len = s.length;
    var rs = "";
    for (var i = 0; i < len; i++) {
        var k = s.substring(i, i + 1);
        rs += (i == 0 ? "" : "%") + s.charCodeAt(i);
    }
    return rs;
}

function runicode(s) {
    var k = s.split("%");
    var rs = "";
    for (i = 0; i < k.length; i++) {
        rs += String.fromCharCode(k[i]);
    }
    return rs;
}

$(document).ready(function () {
    $("#companyEmail").val("");
    $("#companyPwd").val("");
    $("#alertEmailInput").val("");
});
localhostUrl = function () {
    var NowUrl = "http://" + window.location.host;
    return NowUrl;
}

