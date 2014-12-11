//add by li-xiaofei 
function scroll() {
    var width = 0;
    if (window.innerWidth)
        width = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        width = document.body.clientWidth;

}

function Height_Manager(height) {
    if (height >= 500) {

        var heightvar = height + 100 + "px";
        var heightvar1 = height + 120 + "px";
        var heightvar2 = height + 140 + "px";
        $("#divideLine4").css("top", heightvar);
        $("#friendLink").css("top", heightvar1);
        $("#TenantTableDiv").css("height", height);
        $("#table_div").css("height", heightvar2);
        $("#TenantTableDiv").css("border-bottom", "0px solid #ddd");
    }
}

//chenyangwen 20140618 logout
$.ajaxSetup({
    statusCode: {
        499: function (data) {
            window.location.reload();
        }, 599: function (data) {
            alert(LanguageScript.page_common_Role_Change);
            window.location.href = "/hck-fleetadmin";
        }
    }
});

// 
//Domain 重名校验函数
//
function checkDomain_same_url() {
    var flag = false;
    $.ajax({
        type: "POST",
        async: false,
        url: "/hck-fleetadmin/Tenant/CheckDomainUrl",
        data: {
            Domain_dialog_Name_div: $("#Domain_dialog_Name_div").val(),
            pkid: $("#unikey").val()
        },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (data) {

            if (data == "NG") {

                $(".validateTips_ESN").text("您添加的域名已重名!");
                $(".validateTips_ESN").css("color", "red");
                $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");

                $("#ui-right_ESN").hide();
                flag = false;
            } else {

                $("#ui-right_ESN").show();
                $(".validateTips_ESN").text("请输入域名");
                $(".validateTips_ESN").css("margin", "-26px 0px 8px 0px");
                $(".validateTips_ESN").css("color", "gray");
                flag = true;

            }
        }
    });
    return flag;
}



//图片预览
function previewLoginImage(file) {

    $("#Domian_login_logo").attr("loginsubmitflag", 1);

    var MAXWIDTH = 55;
    var MAXHEIGHT = 56;
    var div = document.getElementById('previewLogin');
    if (file.files && file.files[0]) {
        var img = document.getElementById('Domian_login_logo');
        img.onload = function () {
            var rect = clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
            img.width = rect.width;
            img.height = rect.height;
            img.style.marginLeft = rect.left + 'px';
            img.style.marginTop = rect.top + 'px';
        }
        var reader = new FileReader();
        reader.onload = function (evt) { img.src = evt.target.result; }
        reader.readAsDataURL(file.files[0]);
    }
    else {
        file.select();
        div.focus();
        //file.blur();
        var img = document.getElementById('Domian_login_logo');
        var src = document.selection.createRange().text;//取得本地图片地址
        img.outerHTML = '<div id="preview_login_fake"></div>';
        var objPreviewFake = document.getElementById("preview_login_fake");
        objPreviewFake.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = src;
        objPreviewFake.style.width = '55px';
        objPreviewFake.style.height = '56px';
        objPreviewFake.style.marginTop = '0px';
        objPreviewFake.style.marginLeft = '0px';

    }
}

window.onload = function () {
    $("#Domian_login_logo").attr("loginsubmitflag", 0);
    getDomainConfigData();
    scroll();

    var commitLogin = new AjaxUpload("#domain_login_Logo", {
        action: "/hck-fleetadmin/Tenant/AddDomainConfig",
        responseType: "text",
        autoSubmit: false,
        data: {

        },
        onChange: function (file, ext) {
            previewLoginImage(file);
        },
        onSubmit: function (file, ext) {

        },
        onComplete: function (file, response) {
            $(this).dialog("close");
            getDomainConfigData();
        }
    });


    //注册焦点事件
    $("#Domain_dialog_Name_div").focus(function () {
        $(".validateTips_ESN").text("请输入域名");
        $(".validateTips_ESN").css("color", "gray");
        $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
        $("#ui-right_ESN").hide();
    });

    //注册失焦事件
    $("#Domain_dialog_Name_div").blur(function () {
        var domainUrl = $("#Domain_dialog_Name_div");
        if ("" == $.trim(domainUrl.val())) {
            // $(".validateTips_Geo_Name_Vehicle_Center").text(LanguageScript.error_e01266);
            $(".validateTips_ESN").text("域名不能为空");
            $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
            $(".validateTips_ESN").css("color", "red");
            $("#location_right").hide();
            return;
        } else if (!isDomainUrl(domainUrl.val())) {
            $(".validateTips_ESN").text("域名格式不合法.");
            $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
            $(".validateTips_ESN").css("color", "red");
            $("#location_right").hide();
        }
        else {
            checkDomain_same_url();
        }
    });

    //隐藏对号
    $("#ui-right_ESN").hide();
    //定义删除确认对话框
    $("#DeleteConfig_dialog").dialog({
        resizable: false,
        autoOpen: false,
        height: 140,
        width: 250,
        modal: true,
        position: ['center', 250],
        buttons: {
            确定: function () {

                $.ajax({
                    type: "POST",
                    url: "/hck-fleetadmin/Tenant/DelDomainConfig",
                    data: {
                        pkid: $("#deletkey").val()

                    },
                    dataType: "text",
                    success: function (returnData) {

                        //  if (returnData == "400") {
                        //      user_dialog_error(LanguageScript.page_update_Vehicle_error_400);
                        //  }
                        //  else if (returnData == "500") {
                        //     user_dialog_error(LanguageScript.page_update_Vehicle_error_500);
                        // } else if (returnData != "OK") {
                        //     user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
                        // }
                        getDomainConfigData();

                    },
                    error: function () {
                        //alert("11");
                    }
                });

                $(this).dialog("close");
            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    });
    //定义添加修改对话框
    $("#dialog_form_Domain").dialog({
        height: 250,
        resizable: false,
        autoOpen: false,
        width: 320,
        modal: true,
        position: ['center', 250],
        draggabled: true,
        buttons: {
            "保存": function () {
                //非空校验
                var domainUrl = $("#Domain_dialog_Name_div");
                if ("" == $.trim(domainUrl.val())) {
                    $(".validateTips_ESN").text("域名不能为空");
                    $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_ESN").css("color", "red");
                    $("#location_right").hide();
                    return;
                } else if (!isDomainUrl(domainUrl.val())) {
                    $(".validateTips_ESN").text("域名格式不合法.");
                    $(".validateTips_ESN").css("margin", "-13px 0px 8px 0px");
                    $(".validateTips_ESN").css("color", "red");
                    $("#location_right").hide();
                    return;
                }

                else {
                    //同名校验
                    var flag = checkDomain_same_url();

                    if (flag != true) {
                        return;
                    }

                }

                debugger;
                if ($("#Domian_login_logo").attr("loginsubmitflag") == 1) {

                    commitLogin.setData({
                        "Domain_dialog_Name_div": $("#Domain_dialog_Name_div").val(),
                        "loginsubmitflag": $("#Domian_login_logo").attr("loginsubmitflag"),
                        "pkid": $("#unikey").val()
                    });

                    commitLogin.submit();
                } else {
                    $.ajax({
                        type: "POST",
                        url: "/hck-fleetadmin/Tenant/AddDomainConfig",
                        data: {
                            Domain_dialog_Name_div: $("#Domain_dialog_Name_div").val(),
                            loginsubmitflag: $("#Domian_login_logo").attr("loginsubmitflag"),
                            pkid: $("#unikey").val()
                        },
                        dataType: "text",
                        success: function (returnData) {

                            //  if (returnData == "400") {
                            //      user_dialog_error(LanguageScript.page_update_Vehicle_error_400);
                            //  }
                            //  else if (returnData == "500") {
                            //     user_dialog_error(LanguageScript.page_update_Vehicle_error_500);
                            // } else if (returnData != "OK") {
                            //     user_dialog_error(LanguageScript.page_update_Vehicle_error_else);
                            // }

                            $(this).dialog("close");
                            getDomainConfigData();

                        },
                        error: function () {
                            //alert("11");
                        }
                    });
                }
                $("#Domian_login_logo").attr("loginsubmitflag", 0);
                $(this).dialog("close");
            },
            取消: function () {
                $(this).dialog("close");
                $("#Domain_dialog_Name_div").val("");
                $("#Domian_login_logo").attr("loginsubmitflag", 0);
                //bind_OBU_editANDadd();
                // Height_User($("#Setting_container_user_list").find(".Setting_container_user_list").length, 50);
            }
        }
    });

    //添加按钮点击事件
    $("#Domain_container_btn_add").click(function () {
        // dialog_form_Domain
        $("#Domain_dialog_Name_div").val("");
        $("#Domian_login_logo").attr("src", "DrawImageDomain");
        $("#Domian_login_logo").attr("loginsubmitflag", 0);
        $("#unikey").val("");
        $("#dialog_form_Domain").dialog("open");

    });


}


//删除域名 点击处理函数
function Domain_del(pkidvalue_del) {

    $("#deletkey").val(pkidvalue_del);
    $("#DeleteConfig_dialog").dialog("open");


}

//编辑域名 点击处理函数
function Domain_edit(domainurl, pkid) {
    $("#Domain_dialog_Name_div").val(domainurl);
    $("#Domian_login_logo").attr("src", "DrawImageDomainWithUrl?url=" + domainurl);;
    $("#unikey").val(pkid);
    $("#dialog_form_Domain").dialog("open");

};

window.onresize = function () {
    scroll();
}




//检测图片；
function checkLogo(inputComp) {

    var path = inputComp.value;
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
            inputComp.val("");
            return false;
        }
    }
}


//缩放比例函数
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




//获取domain config数据
function getDomainConfigData() {

    // 从后台获取domain config列表并显示在表格中
    $.ajax({
        type: "POST",
        url: "/hck-fleetadmin/GetDomainConfigs",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {



            $("#Domain_table_title tr:not(:first)").remove();

            for (var i = 0; i < msg.length; ++i) {
                var options = '<tr><td class="td1" style="font-size:10pt;"><a href="http://' + msg[i].domainurl + '" target="_blank">' + $.trim(msg[i].domainurl)
                + '</td><td class="td2" style="font-size:10pt;">' +
                 '<img alt="无图片" style="height:30px;width:55px;" src="DrawImageDomainWithUrl?url=' + $.trim(msg[i].domainurl) + '"></td>'
                 + '<td class="td1" style="font-size:10pt;"><a href="#" onclick="Domain_edit(\'' + msg[i].domainurl + '\',' + msg[i].pkid + ')">' + '编辑' + '</a> &nbsp;&nbsp;&nbsp;<a href="#" onclick="Domain_del(' + msg[i].pkid + ')">' + '删除' + '</a></td></tr>'
                $("#Domain_table_title").append(options);
            }
            $("#loading_gif").hide();
            //var height = $("#tenantTable").height();
            //  Height_Manager(height);
        }
    });
}


var isDomainUrl = function (str) {
    var reg = /^[a-zA-Z0-9\_\s\.]{1,100}$/
    return reg.test(str);
}

function goBackPrePage() {
    window.location.href = "ManagerSystem";
}
//add end 








