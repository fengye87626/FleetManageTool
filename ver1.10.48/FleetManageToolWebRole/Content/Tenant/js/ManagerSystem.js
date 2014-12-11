function scroll() {
    var width = 0;
    if (window.innerWidth)
        width = window.innerWidth;
    else if ((document.body) && (document.body.clientWidth))
        width = document.body.clientWidth;
    //$("#contentBody").css("margin-left", 0);
    //$("#contentBody").css("left", width / 2 - 474 + "px");
}
//20140305caoyandong
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

window.onload = function () {
    $("#domainConfigBtn")
    .buttonset()
    .click(function () {
        window.location.href="ManagerSystemDomain";
    });
    scroll();
    //chenyangwen 20140618 logout
    $.ajaxSetup({
        statusCode: {
            499: function (data) {
                window.location.reload();
            }
            , 599: function (data) {
                $("#loading").hide();
                $("#loading_gif").hide();
                window.location.reload();
            }
        }
    });
}

window.onresize = function () {
    scroll();
}

function user_dialog_reset_sure(title, text, companyID, companyName, userID) {
    $("#contentBody").before(function () {
        return "<div id='dialog_background'></div>" +
               "<div id='dialog' >" +
                  "<div class='dialog_title'>" + title + "</div>" +
                  "<div class='dialog_text_1'>" + text + "</div>" +
                  "<div id='user_sure' class='cls_dialog_sure'>" + LanguageScript.common_save + "</div>" +
                  "<div id='user_back' class='cls_dialog_sure'>" + LanguageScript.common_cancel + "</div>" +
        "</div>";
    });
    var left_height = document.getElementById("contentBody").scrollHeight;
    $("#user_back").click(function () {
        $("#user_sure").unbind();
        $("#user_back").unbind();
        $("#dialog_background").remove();
        $("#dialog").remove();
    });
    $("#user_sure").click(function () {
        $("#user_sure").unbind();
        $("#user_back").unbind();
        $("#dialog_background").remove();
        $("#dialog").remove();
        User_Reset_Ajax(username, companyID, companyName, userID);
    });
}
//wenti
function User_Reset_Ajax(username,companyID, companyName, userID) {
    var password = getRandomNum(8);
    var title = LanguageScript.page_tenant_ResetComplete;
    var text = LanguageScript.page_setting_ResetedPassword + ":<div>" + password + "</div>";
    //liangjiajie
    var str = username.toLocaleLowerCase() + "&" + password;
    var MD5Password = hex_md5(str);

    $.ajax({
        type: "POST",
        async: false,
        url: "/hck-fleetadmin/ResetTenant",
        data: { password: MD5Password, tenantID: companyID, userID: userID },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        success: function (msg) {
            if ("true" == msg) {
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
                        width: 280,
                        modal: true,
                        position: ['center', 250],
                        buttons: buttons_with_copy
                    });
                });
            } else {
                location.href = "/hck-fleetadmin";
            }

        }
    });
}

function user_dialog_result(title, text, password) {
    $("#contentBody").before(function () {
        return "<div id=dialog_background></div>" +
               "<div id=dialog >" +
                  "<div class='dialog_title'>" + title + "</div>" +
                  "<div class='dialog_text'>" + text + "</div>" +
                  "<div id='user_copy' class='cls_dialog_sure'>" + LanguageScript.common_CopyPassword + "</div>" +
                  "<div id='user_error' class='cls_dialog_sure'>" + LanguageScript.common_cancel + "</div>"
        "</div>";
    });
    $("#user_error").click(function () {
        $("#user_error").unbind();
        $("#dialog_background").remove();
        $("#dialog").remove();
    });
    $("#user_copy").click(function () {
        if (window.clipboardData) {
            window.clipboardData.setData("Text", password)
            alert(LanguageScript.common_CopySuccess);
        }
        else {
            alert(LanguageScript.common_CopyFail);
        }

    });
}

$(document).ready(function () {
    //caoyandong
    $("#oldpassword").val("");
    $("#newpassword").val("");
    $("#confirmpassword").val("");

    //liying20140529
    //getPageData(1);
    var searchFlag = false;
    var currentPage = 1;
    var searchKey = "";
    var obuPageSize = 100;
    //OBUInit(1, searchFlag);
    //var height = window.document.body.scrollHeight;
	var height = window.screen.height;
    var winWidth = 0;
    var winHeight = 0;
    
    $(window).resize(function () {
        winHeight = (document.documentElement.clientHeight - 114) / 2;
        winWidth = (document.documentElement.clientWidth - 114) / 2;
        $("#loading_gif").css("left", winWidth);
        $("#loading_gif").css("top", winHeight);
    });

    function page_ul_class() {
        $("#paperBar").find("ul").addClass("obu_page_ul");
    }
    function setFirindLinkTop() {
        var height = $("#OBUTableDivId").height() + 46 + 30 + 35;
        var div_height;
        if (height > 680) {
            $("#friendLink").css('top', height + 'px');
            div_height = height + $("#friendLink").height() + 6;
        } else {
            $("#friendLink").css('top', '680px');
            div_height = 680 + $("#friendLink").height() + 6;
        }
        $("#table_div").height(div_height);
    }

    $("#loading").height(height);
    //add by fengpan 20140527 4 tab切换事件
    $('#tabButton div').click(function () {
        if ("true" == $(this).attr("click-label")) {
            $(this).removeClass().addClass("selected").siblings().removeClass().addClass("normal");
            $(this).attr("click-label", "false").siblings().attr("click-label", "true");
            $("#tabContent > div").hide().eq($('#tabButton div').index(this)).show();

            switch ($(this).attr("tab_id")) {
                case "1":
                    $(".paper-input-value").val("");
                    $("#TenantTableDivId").show();
                    $("#OBUTableDivId").hide();
                    $("#friendLink").css('top', '680px');
                    $("#table_div").height('706px');
                    break;
                case "2":
                    $(".paper-input-value").val("");
                    $("#TenantTableDivId").hide();
                    $("#OBUTableDivId").show();
                    setFirindLinkTop();
                    break;
                case "3":
                    $(".paper-input-value").val("");
                    $("#friendLink").css('top', '680px');
                    $("#table_div").height('706px');
                    break;
            }
        }
    });

    getTenantData();
    getPageData(currentPage);

    
    //added by caoyandong
    winHeight = (document.documentElement.clientHeight - 114) / 2;
    winWidth = (document.documentElement.clientWidth - 114) / 2;
    $("#loading_gif").css("left", winWidth);
    $("#loading_gif").css("top", winHeight);
    $("#loading").height(height);
    $("#loading").show();
    $("#loading_gif").show();
    $("#ui-right_oldpassword").hide();
    $("#ui-right_newpassword").hide();
    $("#ui-right_confirmpassword").hide();
    setFirindLinkTop();
    //fengpan 20140527 MMY上传
    new AjaxUpload(
        "#file-uploader-MMY"
        , {
            action: "/hck-fleetadmin/UploadMMYCSVFile"
            , multiple: false
            , responseType: "text"
            , onSubmit: function (fileName, extension) {
                if (!(extension && /^(csv)$/i.test(extension))) {
                    //alert(LanguageScript.error_upload_csvFile);
                    $("#uploadCSVFormatDialog").dialog("open");
                    return false;

                }
                $("#loading_tip").text(LanguageScript.page_loading_readdata_wait);
                //added by caoyandong
                winHeight = (document.documentElement.clientHeight - 114) / 2;
                winWidth = (document.documentElement.clientWidth - 114) / 2;
                $("#loading_gif").css("left", winWidth);
                $("#loading_gif").css("top", winHeight);
                $("#loading").height(height);
                $("#loading").show();
                $("#loading_gif").show();
            }
            , onComplete: function (fileName, response) {
                try{
                    //在此处理后台返回的数据，若为空，提示成功，若不为空，则提示失败和失败原因，将错误数据显示在页面上
                    if (parseInt(response) >= 0) {
                        $("#loading").hide();
                        $("#loading_gif").hide();
                        if (parseInt(response) == 0) {
                            errortipinfo(LanguageScript.page_manger_mmyupdateok);
                        } else {
                            errortipinfo(LanguageScript.obu_upload_success_first + parseInt(response) + LanguageScript.obu_upload_success_end);
                        }
                        return false;
                    } else if (parseInt(response) == -1) {
                        $("#loading_gif").hide();
                        $("#loading").hide();
                        errortipinfo(LanguageScript.mmy_uploaderror);
                    } else if (response != "empty") {
                        $("#loading_gif").hide();
			            $("#loading").hide();
                        arr = new Array();
                        arr = response.split(',');
                        var words = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">';
                        if (arr.length == 1) {
                            arr[0] = arr[0].substr(2, arr[0].length - 4);
                            words += arr[0] + "</br>";
                        } else {
                            for (var i = 0; i < arr.length; ++i) {
                                if (i == 0) {
                                    arr[i] = arr[i].substr(2, arr[i].length - 3);
                                } else if (i == arr.length - 1) {
                                    arr[i] = arr[i].substr(1, arr[i].length - 3);
                                } else {
                                    arr[i] = arr[i].substr(1, arr[i].length - 2);
                                }
                                words += arr[i] + "</br>";
                            }
                        }
                        words += "</p>";
                        errortipinfo(words);
                    } else {
                        $("#loading_gif").hide();
                        $("#loading").hide();
                        errortipinfo(LanguageScript.obu_upload_empty);
                    }
                }catch(Exception){
                    $("#loading").hide();
                    $("#loading_gif").hide();
                    window.location.reload();
                }
            }
        });


    function getPageData(pageIndex) {
        $("#paperBar").attr("data-click", "true");
        //added by caoyandong
        winHeight = (document.documentElement.clientHeight - 114) / 2;
        winWidth = (document.documentElement.clientWidth - 114) / 2;
        $("#loading_gif").css("left", winWidth);
        $("#loading_gif").css("top", winHeight);
        $("#loading").height(height);
        $("#loading").show();
        $("#loading_gif").show();
        $.ajax({
            type: "POST",
            url: "/hck-fleetadmin/getPageOBUData",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: "page=" + pageIndex,
            success: function (msg) {
                if (null != msg) {
                    OBUTableHtml(msg.data);
                    if ($("#OBUTableDivId").css("display") != "none") {
                        setFirindLinkTop();
                    }
                    var pageNum = parseInt(msg.count);
                    pageNum = (pageNum % obuPageSize == 0 ? pageNum / obuPageSize : pageNum / obuPageSize + 1);
                    if (pageNum == 0) {
                        pageNum = 1;
                    }
                    $("#paperBar").pager({ pagenumber: pageIndex, pagecount: pageNum, buttonClickCallback: PaperClick });
                    $("#paperBar").find("li").eq(2).css("background-color", "#f1f1f1");
                    $("#paperBar").find("li").eq(5).css("background-color", "#f1f1f1");
                    page_ul_class();
                    $("#paperBar").find("ul").css("display", "inline-block");
                    $("#paperBar").show();
                }
                $("#paperBar").attr("data-click", "false");
            }
        });
    }

    function searchPageData(key, pageIndex) {
        
        //added by caoyandong
        winHeight = (document.documentElement.clientHeight - 114) / 2;
        winWidth = (document.documentElement.clientWidth - 114) / 2;
        $("#loading_gif").css("left", winWidth);
        $("#loading_gif").css("top", winHeight);
        $("#loading").height(height);
        $("#loading").show();
        $("#loading_gif").show();
        $("#paperBar").attr("data-click", "true");
        $.ajax({
            type: "POST",
            url: "/hck-fleetadmin/getOBUBYKey",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: { key: key, pageIndex: pageIndex },
            success: function (msg) {
                if (null != msg) {
                    OBUTableHtml(msg.data);
                    setFirindLinkTop();
		            if (msg.data.length == 0) {
                        $("#informationDialog-text").text(LanguageScript.obu_not_found);
                        $("#informationDialog").dialog("open");
                    }

                    var pageNum = parseInt(msg.count);
                    pageNum = (pageNum % obuPageSize == 0 ? pageNum / obuPageSize : pageNum / obuPageSize + 1);
                    if (pageNum == 0) {
                        pageNum = 1;
                    }
                    $("#paperBar").pager({ pagenumber: pageIndex, pagecount: pageNum, buttonClickCallback: PaperClick });
                    $("#paperBar").find("li").eq(2).css("background-color", "#f1f1f1");
                    $("#paperBar").find("li").eq(5).css("background-color", "#f1f1f1");
                    page_ul_class();
                    $("#paperBar").find("ul").css("display", "inline-block");
                    $("#paperBar").show();
                }
                $("#paperBar").attr("data-click", "false");
            }
        });
    }

    //keypress 事件
    $("#searchOBU").keypress(function (event) {
        if (event.keyCode == 13) {
            if ($("#searchOBU").val().trim().length > 0) {
                searchFlag = true;
                searchKey = $("#searchOBU").val();
            } else {
                searchFlag = false;
                searchKey = "";
            }
            OBUInit(1, searchFlag);
        }
    });

    //点击搜索图标事件
    $("#tile_seach_icon_img").click(function () {
        if ($("#searchOBU").val().length > 0) {
            searchFlag = true;
            searchKey = $("#searchOBU").val();
        } else {
            searchFlag = false;
            searchKey = "";
        }
        OBUInit(1, searchFlag);
    });
    //点击删除OBU
    var deleteOBUID;
    $(document).on('click', '.deleteOBU', function () {
        deleteOBUID = $(this).attr("data-id");
        $("#deleteOBUConfirmDialog").dialog("open");
        //$("#loading").show();
        //$("#loading_tip").text(LanguageScript.page_loading_readdata_wait);
        ////added by caoyandong
        //winHeight = (document.documentElement.clientHeight - 114) / 2;
        //winWidth = (document.documentElement.clientWidth - 114) / 2;
        //$("#loading_gif").css("left", winWidth);
        //$("#loading_gif").css("top", winHeight);
        //$("#loading").height(height);

        //$("#loading_gif").show();
        //$("#paperBar").attr("data-click", "true");
        //var $that = $(this);
        //var id = $that.attr("data-id");

        //$.ajax({
        //    type: "POST",
        //    url: "/hck-fleetadmin/deleteOBU",
        //    contentType: "application/x-www-form-urlencoded",
        //    dataType: "text",
        //    data: "id=" + id,
        //    success: function (msg) {
        //        if ("OK" == msg) {
        //            $("#informationDialog-text").text(LanguageScript.obu_delete_success);
        //            $("#informationDialog").dialog("open");
        //            //删除时此页面只剩下一条数据，则删除之后PageIndex-1
        //            if ($("#tenantTable tr").length == 2) {
        //                currentPage = currentPage - 1;
        //            }
        //        } else {
        //            $("#informationDialog-text").text(LanguageScript.obu_delete_failed);
        //            $("#informationDialog").dialog("open");
        //        }
        //        OBUInit(currentPage, searchFlag);
        //        $("#paperBar").attr("data-click", "false");
        //    }
        //});
    });

    function deleteOBUOpion() {
        
        $("#loading_tip").text(LanguageScript.page_loading_readdata_wait);
        //added by caoyandong
        winHeight = (document.documentElement.clientHeight - 114) / 2;
        winWidth = (document.documentElement.clientWidth - 114) / 2;
        $("#loading_gif").css("left", winWidth);
        $("#loading_gif").css("top", winHeight);
        $("#loading").height(height);
        $("#loading").show();
        $("#loading_gif").show();
        $("#paperBar").attr("data-click", "true");
        var id = deleteOBUID;

        $.ajax({
            type: "POST",
            url: "/hck-fleetadmin/deleteOBU",
            contentType: "application/x-www-form-urlencoded",
            dataType: "text",
            data: "id=" + id,
            success: function (msg) {
                if ("OK" == msg) {
                    $("#informationDialog-text").text(LanguageScript.obu_delete_success);
                    $("#informationDialog").dialog("open");
                    //删除时此页面只剩下一条数据，则删除之后PageIndex-1
                    if ($("#obuTable tr").length == 2) {
                        currentPage = currentPage - 1;
                    }
                } else {
                    $("#informationDialog-text").text(LanguageScript.obu_delete_failed);
                    $("#informationDialog").dialog("open");
                }
                OBUInit(currentPage, searchFlag);
                $("#paperBar").attr("data-click", "false");
            }
        });
    }

            function setDataFormat(str) {
                if (str < 10)
                {
                    str = "0" + str;
                }
                return str;
            }
    function OBUTableHtml(data) {
        var HTML = "";
        $.each(data, function (i, item) {
            var time;
            var str = "";
            if (null != item.createdate) {
                time = parseLocalDate(item.createdate).format("yyyy-MM-dd hh:mm:ss");
                //   var d = eval('new ' + str.substr(1, str.length - 2));
                //var month = str.getMonth() + 1;
                        //time = str.getFullYear() + "-" + setDataFormat(month) + "-" + setDataFormat(str.getDate()) + " " + setDataFormat(str.getHours()) + ":" + setDataFormat(str.getMinutes()) + ":" + setDataFormat(str.getSeconds());
            } else {
                time = "";
            }

                    var status = item.status == 1 ? LanguageScript.obu_status_used : LanguageScript.obu_status_unused;
                    var delelteDiv = '<div class="deleteOBU" style="color:blue; cursor:pointer; display:none" data-id=' + item.pkid + '>' + LanguageScript.obu_delete_option + '</div>';
            var nonDiv = '<div data-id=' + item.pkid + ' class="nonDeleteObu">' + "&nbsp;" + '</div>';
            var option = item.status == 1 ? "" : delelteDiv;
            HTML += '<tr style="border-bottom:1px solid #ddd; background-color:#fff" class="tr_hover_delete">'
                + '<td class="td_first" style="font-size:10pt;width:157px"><div class="obu_text_ellipsis">' + item.byteid + '</div></td>'
                + '<td class="td_middle" style="font-size:10pt; width:157px"><div class="obu_text_ellipsis">' + item.labelid + '</div></td>'
                + '<td class="td_middle" style="font-size:10pt; width:157px"><div class="obu_text_ellipsis">' + item.regkey + '</div></td>'
                + '<td class="td_middle" style="font-size:10pt; width:172px"><div class="obu_text_ellipsis">' + time + '</div></td>'
                + '<td class="td_middle" style="font-size:10pt; width:82px"><div class="obu_text_ellipsis">' + status + '</div></td>'
                + '<td class="td_end" style="font-size:10pt; width:57px" td-data-id=' + item.pkid + '>' + nonDiv + option + '</td>'
            + '</tr>';
        });
        $("#obuTable tr").eq(0).nextAll().remove();
        $("#obuTable").append(HTML);
        $("#loading_gif").hide();
        $("#loading").hide();
    }

    PaperClick = function (pageclickednumber) {
        //alert(pageclickednumber);
        if ($("#paperBar").attr("data-click") == "false") {
            OBUInit(pageclickednumber, searchFlag);
            $("#result").html("Clicked Page " + pageclickednumber);
        } else {
            return;
        }
    }

    function OBUInit(paperNumber, flag) {
        currentPage = paperNumber;
        if (flag) {
            //  var key = $("#searchOBU").val();
            searchPageData(searchKey, paperNumber);
        } else {
            getPageData(paperNumber);
        }
    }

    new AjaxUpload(
        "#file-uploader"
        , {
            action: "/hck-fleetadmin/UploadCSVFile"
            , multiple: false
            , responseType: "text"
            , onSubmit: function (fileName, extension) {
                if (!(extension && /^(csv)$/i.test(extension))) {
                    //alert(LanguageScript.error_upload_csvFile);
                    $("#uploadCSVFormatDialog").dialog("open");
                    return false;

                }
                $("#loading_tip").text(LanguageScript.page_loading_wait);
                
                //added by caoyandong
                winHeight = (document.documentElement.clientHeight - 114) / 2;
                winWidth = (document.documentElement.clientWidth - 114) / 2;
                $("#loading_gif").css("left", winWidth);
                $("#loading_gif").css("top", winHeight);
                $("#loading").height(height);

                $("#loading_gif").show();
                $("#loading").show();
            }
            , onComplete: function (fileName, response) {
                try{
                    var jsonData = JSON.parse(response);
                    //在此处理后台返回的数据，若为空，提示成功，若不为空，则提示失败和失败原因，将错误数据显示在页面上
                    if (parseInt(jsonData) > "0") {
                        $("#loading_gif").hide();
                        $("#loading").hide();
                        // alert("成功导入" + jsonData + "条数据");
                        $("#informationDialog-text").text(LanguageScript.obu_upload_success_first + jsonData + LanguageScript.obu_upload_success_end);
                        $("#informationDialog").dialog("open");
                        OBUInit(1, false);
                        searchKey = "";
                        $("#searchOBU").val("");
                        return false;
                    } else {
                        if (!$.isEmptyObject(jsonData)) {
                            $("#errorCSVDataTable tr").eq(0).nextAll().remove();
                            $("#loading_gif").hide();
                            $("#loading").hide();
                            showReponseData(jsonData);

                        } else {
                            $("#loading_gif").hide();
                            $("#loading").hide();
                            //alert("文件为空");
                            $("#informationDialog-text").text(LanguageScript.obu_upload_empty);
                            $("#informationDialog").dialog("open");
                        }
                    }
                }catch(Exception){
                    $("#loading").hide();
                    $("#loading_gif").hide();
                    window.location.reload();
                }
    
            }
        });

    function showReponseData(data) {
        var HTML = "";
        for (var i in data) {

            var str = data[i].split("===");
            var lineNumber = parseInt(str[1]) + 1;
            var statusType = str[2];
            var statusData;
            if (str[2] == "ER") {
                statusData = LanguageScript.obu_update_response_error;
            } else if (str[2] == "EX") {
                statusData = LanguageScript.obu_update_response_regist;
            } else if (str[2] == "MORE") {
                statusData = LanguageScript.obu_update_response_more;
            } else if (str[2] == "LESS") {
                statusData = LanguageScript.obu_update_response_less;
            }
            var temp = str[0].split(',');
            temp[0] = $.isEmptyObject(temp[0]) ? "" : temp[0];
            temp[1] = $.isEmptyObject(temp[1]) ? "" : temp[1];
            temp[7] = $.isEmptyObject(temp[7]) ? "" : temp[7];
            temp[9] = $.isEmptyObject(temp[9]) ? "" : temp[9];
            HTML += '<tr style="border-bottom: solid 1px #ddd;"">'
                        + '<td class="csv_error_dialog_td_first" style="width:50px"><div style="width:50px" class="obu_text_ellipsis">' + lineNumber + '</div></td>'
                        + '<td class="csv_error_dialog_td_middle" style="width:200px"><div style="width:200px" class="obu_text_ellipsis">' + temp[0] + '</div></td>'
                        + '<td class="csv_error_dialog_td_middle" style="width:200px"><div style="width:200px" class="obu_text_ellipsis">' + temp[1] + '</div></td>'
                        + '<td class="csv_error_dialog_td_middle" style="width:120px"><div style="width:120px" class="obu_text_ellipsis">' + temp[7] + '</div></td>'
                        + '<td class="csv_error_dialog_td_middle" style="width:120px"><div style="width:120px" class="obu_text_ellipsis">' + temp[9] + '</div></td>'
                        + '<td class="csv_error_dialog_td_end" style="width:110px">' + statusData + '</td>'
                    + '</tr>';
        }
        if (HTML != "") {
            $("#errorCSVDataTable").append(HTML);
            $("#error-csv-data").dialog("open");
        }

    }

    $("#error-csv-data").dialog({
        resizable: false,
        height: 400,
        width: 840,
        position: 'center',
        modal: true,
        autoOpen: false,
        buttons: {
            "确定": function () {

                $("#error-csv-data").dialog("close");
            }
        }
    });

/*    $(".tr_hover_delete").live("hover", function () {
        $(this).find(".deleteOBU").show();
    }); */

    $(".tr_hover_delete").live({
        mouseenter: function () {
            $(this).find(".nonDeleteObu").hide();
            $(this).find(".deleteOBU").show();
            $(this).css("border", "2px solid #ddd");
        }, mouseleave: function () {
            $(this).find(".deleteOBU").hide();
            $(this).find(".nonDeleteObu").show();
            $(this).css("border", "0px");
            $(this).css("border-bottom", "1px solid #ddd");
        }
    });

    Date.prototype.format = function (format) {
        var o = {
            "M+": this.getMonth() + 1, //month
            "d+": this.getDate(), //day
            "D+": this.getDate(), //day
            "h+": this.getHours(), //hour
            "H+": this.getHours(), //hour
            "m+": this.getMinutes(), //minute
            "s+": this.getSeconds(), //second
            "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
            "S": this.getMilliseconds() //millisecond
        }
        if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        return format;
    }

    var parseLocalDate = function (date) {
        var tempDate = new Date(parseInt(date.replace("/Date(", "").replace(")/", ""), 10));
     //   var timezoneOffset = new Date().getTimezoneOffset() / 60 * -1;
    //    tempDate.setHours(tempDate.getHours() + timezoneOffset);
        return tempDate;
    }
    
    $("#informationDialog").dialog({
        height: 140,
        resizable: false,
        autoOpen: false,
        width: 280,
        modal: true,
        position: ['center', 250],
        draggabled: true,
        buttons: {
            "确定": function () {
                $("#informationDialog").dialog("close");
            }

        }
    });

    $("#deleteOBUConfirmDialog").dialog({
        height: 140,
        resizable: false,
        autoOpen: false,
        width: 280,
        modal: true,
        position: ['center', 250],
        draggabled: true,
        buttons: {
            "确定": function () {
                deleteOBUOpion();
                $("#deleteOBUConfirmDialog").dialog("close");
            },
            "取消": function(){
                $("#deleteOBUConfirmDialog").dialog("close");
            }

        }
    });


    $("#uploadCSVFormatDialog").dialog({
        height: 140,
        resizable: false,
        autoOpen: false,
        width: 280,
        modal: true,
        position: ['center', 250],
        draggabled: true,
        buttons: {
            "确定": function () {
                $("#uploadCSVFormatDialog").dialog("close");
            }
        }
    });
});
//Add by LiYing for upload csv file End
function resetTenantPwd(userid) {
    //var id = userid.id;
    var arr = userid.split('|');
    var companyid = "";
    var companyname = "";
    var userID = 1;
    if (arr.length > 4) {
        companyid = arr[4];
        companyname = arr[3];
        username = arr[2];
        userID = arr[1];
    }
    //var title = "重置密码";
    //alert(data.data.companyid + " " + data.data.companyname + " " + data.data.userName + " " + data.data.userID);
    //var text = '<div style="padding:2px;">您确定要重置以下公司的管理员的密码吗?</div><div style="padding:2px;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;width:430px;" title=' + companyname + '>公司名:' + companyname + '</div><div style="padding:2px;">管理员ID:' + companyid + '</div><br><br>';
    //20140313caoyandong
    $(".setting_user_reset")[0].innerHTML =
        '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + companyname + '>' + LanguageScript.common_CompanyName + ' : ' + companyname + '</p>' +
        '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + username + '>' + LanguageScript.page_admin_userDetails_role_tenantAdmin + ' : ' + username + '</p>' +
        '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;">' + LanguageScript.page_tenant_DiaTenReset + '</p>';

        //'<p style="font-family:Microsoft YaHei;font-size:13pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;">' + LanguageScript.common_CompanyName + ':' + companyname + '<br>' + LanguageScript.page_admin_userDetails_role_tenantAdmin + ':' + username + '<br>' + LanguageScript.page_tenant_DiaTenReset + '</p>';


    $(function () {
        $(".setting_user_reset").dialog({
            resizable: false,
            height: 170,
            width: 280,
            position: 'center',
            modal: true,
            buttons: {
                "确定": function () {
                    User_Reset_Ajax(username, companyid, companyname, userID);
                    $(this).dialog("close");
                },
                取消: function () {
                    $(this).dialog("close");
                }
            }
        });
    });


    //user_dialog_reset_sure(title, text, companyid, companyname, userID);
}

function getTenantData() {
    //chenyangwen 从后台获取公司列表并显示在表格中
    $.ajax({
        type: "POST",
        url: "/hck-fleetadmin/GetTenants",
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (msg) {
            $("#tenantTable").empty();
            var fisrt = '<tr style="background-color:#f1f1f1;"><td class="td1" style="font-size:10pt;font-weight:bolder;">' + LanguageScript.page_tenant_CompanyName + '</td><td class="td2" style="font-size:10pt;font-weight:bolder;">' + LanguageScript.page_tenant_CompanyAdminID + '</td><td class="td1" style="font-size:10pt;font-weight:bolder;">' + LanguageScript.common_Operating + '</td></tr>'
            $("#tenantTable").append(fisrt);
            for (var i = 0; i < msg.length; ++i) {
                var text = LanguageScript.common_Activate;
                if (msg[i].status == "Active") {
                    text = LanguageScript.common_Deactivate;
                }else if(msg[i].status == "InActive"){
                    text = LanguageScript.common_Activate;
                }
                var adminUsers = "";
                for (var j = 0; j < msg[i].FleetUser.length; ++j) {
                    var tempID = "adminuser|" + msg[i].FleetUser[j].pkid + "|" + msg[i].FleetUser[j].username + "|" + $.trim(msg[i].companyname) + "|" + $.trim(msg[i].companyid);
                    adminUsers += '<div id="' + tempID + '" class ="TenantManage_userButton_style" ><div class ="TenantManage_userNameTxt_style" title=' + msg[i].FleetUser[j].username + '>' + msg[i].FleetUser[j].username + '</div><div class="TenantManage_userButton_style_mousecover" onclick="resetTenantPwd(\'' + tempID + '\')">' + LanguageScript.common_resetPassword + '</div></div>';
                }

                var options = '<tr ><td class="td1" style="font-size:10pt;">' + $.trim(msg[i].companyname) + '</td><td class="td2" style="font-size:10pt;">' +
                    adminUsers + '</td><td class="td1" style="font-size:10pt;"><div class="active" id="active' + i + '"  >' + text + '</div></td></tr>'
                    //adminUsers + '</td><td class="td1" style="font-size:10pt;"><a href="#" class="active" id="active' + i + '"  >' + text + '</a></td></tr>'
                $("#tenantTable").append(options);
                var companyid = $.trim(msg[i].companyid);
                var companyname = $.trim(msg[i].companyname);
                //$("#reset" + i).bind("click", { companyid: companyid, companyname: companyname }, resetTenantPwd);
                $("#active" + i).bind("click", { companyid: companyid,companyname: companyname }, SetTenantStatus);
            }
            $("#loading_gif").hide();
            $("#loading").hide();
            var height = $("#tenantTable").height();
            Height_Manager(height);
        }
    });
}

//function userButtonMouseOver(userid) {
//    var id = userid.id;
//    if ($("#" + id).children().length > 1) {
//        return;
//    }
//    $("#" + id).removeClass();

//    $("#" + id).addClass("TenantManage_userButton_style_mousecover");

//    var DeleteVehicleButtonId = "TenantManage_userButtonClose";

//    $("#" + id).append(function () {
//        var div = '<div id=' + DeleteVehicleButtonId + ' class ="tenantManage_usereset_style" style="cursor:pointer;">重置密码</div>';
//        return div;
//    })
//    var arr = id.split('_');
//    if (arr.length > 4) {
//        $("#" + DeleteVehicleButtonId).bind("click", { companyid: arr[4], companyname: arr[3], userID: arr[1], userName:arr[2] }, resetTenantPwd);
//    }
//    //追加鼠标移除时的动作
//    $("#" + id).mouseleave(function () {
//        $("#" + id).removeClass();
//        $("#" + DeleteVehicleButtonId).remove();
//        $("#" + id).addClass("TenantManage_userButton_style");
//    });
//}

function getRandomString(len) {
    var chars = 'ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456780';
    var charsLen = chars.length;
    var randomPassword = '';
    for(var i = 0; i < len; ++i)
    {
        randomPassword += chars.charAt(Math.floor(Math.random() * charsLen));
    }
    return randomPassword;
}

function getRandomNum(len) {
    var chars = '123456780';
    var charsLen = chars.length;
    var randomPassword = '';
    for (var i = 0; i < len; ++i) {
        randomPassword += chars.charAt(Math.floor(Math.random() * charsLen));
    }
    return randomPassword;
}

function SetTenantStatus(data) {
    var text = $(this).text();
    var active_id = $(this)[0].id;
    //chenyangwen 设置公司的状态
    if (LanguageScript.common_Activate == text) {

        //qiyong

        $(".setting_user_reset")[0].innerHTML =
            '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + data.data.companyname + '>' + LanguageScript.common_CompanyName + ' : ' + data.data.companyname + '</p>' +
            '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;">'+LanguageScript.page_manager_companyactivate+'</p>';
        $(function () {
            $(".setting_user_reset").dialog({
                resizable: false,
                height: 170,
                width: 280,
                position: 'center',
                modal: true,
                buttons: {
                    "确定": function () {
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: "/hck-fleetadmin/SetTenantStatus",
                            data: { tenantID: data.data.companyid, status: "Active" },
                            contentType: "application/x-www-form-urlencoded",
                            dataType: "text",
                            success: function (msg) {
                                if ("true" == msg) {
                                    text = LanguageScript.common_Deactivate;
                                    $("#" + active_id)[0].innerHTML = text;
                                } else {
                                    location.href = "/hck-fleetadmin";
                                }
                            }
                        });
                        $(this).dialog("close");
                    },
                    取消: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
            });
        });
    } else if (LanguageScript.common_Deactivate == text) {

        //liangjiajie0328
        $(".setting_user_reset")[0].innerHTML =
            '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;" title=' + data.data.companyname + '>' + LanguageScript.common_CompanyName + ' : ' + data.data.companyname + '</p>' +
            '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;white-space:nowrap;text-overflow:ellipsis;overflow: hidden;">'+LanguageScript.page_manager_companydeactivate+'</p>';
        $(function () {
            $(".setting_user_reset").dialog({
                resizable: false,
                height: 170,
                width: 280,
                position: 'center',
                modal: true,
                buttons: {
                    "确定": function () {
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: "/hck-fleetadmin/SetTenantStatus",
                            data: { tenantID: data.data.companyid, status: "InActive" },
                            contentType: "application/x-www-form-urlencoded",
                            dataType: "text",
                            success: function (msg) {
                                if ("true" == msg) {
                                    text = LanguageScript.common_Activate;
                                    $("#" + active_id)[0].innerHTML = text;
                                } else {
                                    location.href = "/hck-fleetadmin";
                                }
                            }
                        });
                        $(this).dialog("close");
                    },
                    取消: function () {
                        $(this).dialog("close");
                        return;
                    }
                }
            });
        });
    }
}
function funcresetadminpsw() {
    var check_oldpassword = false;
    var check_newpassword = false;
    var check_confirmpassword = false;
    var newadminpassword = "admin&";
    $(function () {
        $("#oldpassword").blur(function () {
            check_old_Password();
        });
        $("#newpassword").blur(function () {
            check_new_Password();
            if ($.trim($("#confirmpassword").val()) != "") {
                check_confirm_Password();
            }
        });
        $("#confirmpassword").blur(function () {
            if ($.trim($("#newpassword").val()) != "") {
                check_confirm_Password();
            }
            check_confirm_Password();
        });
        $("#oldpassword").focus(function () {
            $(".validateTips_oldpassword").text(LanguageScript.page_setting_EnterOldPassWord);
            $(".validateTips_oldpassword").css("color", "gray");
            $("#ui-right_oldpassword").hide();
            $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
        });
        $("#newpassword").focus(function () {
            $("#ui-right_newpassword").hide();
            if ($.trim($("#newpassword").val()) != "") {
                if ($("#newpassword").length > 5 && $("#newpassword").length < 21) {
                    $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
                    $(".validateTips_newpassword").css("color", "gray");
                    $(".validateTips_newpassword").css("margin", "-39px 0px 8px 0px");
                } else {
                    $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
                    $(".validateTips_newpassword").css("color", "gray");
                    $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
                }
            } else {
                $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
                $(".validateTips_newpassword").css("color", "gray");
                $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
            }
        });
        $("#confirmpassword").focus(function () {
            $(".validateTips_confirmpassword").text(LanguageScript.common_PasswordConfirmInput);
            $(".validateTips_confirmpassword").css("color", "gray");
            $("#ui-right_confirmpassword").hide();
            $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
        });
        $("#dialog_form_admin").dialog({
            resizable: false,
            height: 325,
            width: 280,
            position: ['center',150],
            modal: true,
            buttons: {
                "确定": function () {
                    check_old_Password();
                    check_new_Password();
                    check_confirm_Password();
                    newadminpassword = "admin&";
                    newadminpassword += $.trim($("#newpassword").val());
                    if ($("#ui-right_newpassword")[0].style.display == "block" && check_new_Password() == true && check_confirm_Password() == true && check_old_Password() == true) {
                        Admin_Edit_Ajax(newadminpassword);
                        $("#oldpassword").val("");
                        $("#newpassword").val("");
                        $("#confirmpassword").val("");
                        $(".validateTips_oldpassword").text(LanguageScript.page_setting_EnterOldPassWord);
                        $(".validateTips_oldpassword").css("color", "gray");
                        $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
                        $(".validateTips_newpassword").css("color", "gray");
                        $(".validateTips_confirmpassword").text(LanguageScript.common_PasswordConfirmInput);
                        $(".validateTips_confirmpassword").css("color", "gray");
                        $("#ui-right_oldpassword").hide();
                        $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_newpassword").hide();
                        $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
                        $("#ui-right_confirmpassword").hide();
                        $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
                        $(this).dialog("close");
                    }
                },
                取消: function () {
                    $("#oldpassword").val("");
                    $("#newpassword").val("");
                    $("#confirmpassword").val("");
                    $(".validateTips_oldpassword").text(LanguageScript.page_setting_EnterOldPassWord);
                    $(".validateTips_oldpassword").css("color", "gray");
                    $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
                    $(".validateTips_newpassword").css("color", "gray");
                    $(".validateTips_confirmpassword").text(LanguageScript.common_PasswordConfirmInput);
                    $(".validateTips_confirmpassword").css("color", "gray");
                    $("#ui-right_oldpassword").hide();
                    $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
                    $("#ui-right_newpassword").hide();
                    $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
                    $("#ui-right_confirmpassword").hide();
                    $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
                    $(this).dialog("close");
                }
            }
        });
        $("#resetadminpswText")
                  .buttonset()
                  .click(function () {
                      $("#dialog_form_user").dialog("open");
                      $("#ui-right_oldpassword").hide();
                      $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
                      $("#ui-right_newpassword").hide();
                      $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
                      $("#ui-right_confirmpassword").hide();
                      $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
                  });
    });
}

/*验证旧密码*/
function check_old_Password() {
    var accunt_name = "admin";
    old_Password = document.getElementById("oldpassword").value;
    if ("" == old_Password) {
        check_oldpassword = false;
        $(".validateTips_oldpassword").text("原密码不能为空");
        $(".validateTips_oldpassword").css("color", "red");
        $("#ui-right_oldpassword").hide();
        $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
        return false;
    }
    old_Password = accunt_name + '&' + old_Password;
    var MD5Password = hex_md5($.trim(old_Password));
    var result = false;
    $.ajax({
        type: "POST",
        url: "/hck-fleetadmin/Tenant/Check_Accunt_password",
        data: { username: accunt_name, password: MD5Password },
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        async:false,
        success: function (msg) {
            if ("OK" == msg) {
                $(".validateTips_oldpassword").text(LanguageScript.page_setting_EnterOldPassWord);
                $(".validateTips_oldpassword").css("color", "gray");
                $("#ui-right_oldpassword").show();
                $(".validateTips_oldpassword").css("margin", "-26px 0px 8px 0px");
                check_oldpassword = true;
                //return true;
                result = true;
            }
            else if ("NG" == msg) {
                $(".validateTips_oldpassword").text("原密码不正确");
                $(".validateTips_oldpassword").css("color", "red");
                $("#ui-right_oldpassword").hide();
                $(".validateTips_oldpassword").css("margin", "-13px 0px 8px 0px");
                check_oldpassword = false;
                //return false;
                result = false;
            }
        }
    });
    return result;
};
function check_new_Password() {
    var reg = /^[a-zA-Z0-9\_\%\^\(\)\-\+\=\~\@\$\!\*\&\#\,\.]{6,20}$/;
    var numReg = /[0-9]/;
    new_Password = $("#newpassword").val();
    if ("" == new_Password) {
        check_newpassword = false;
        $(".validateTips_newpassword").text("新密码不能为空");
        $(".validateTips_newpassword").css("color", "red");
        $("#ui-right_newpassword").hide();
        $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
        return false;
    }
    if (reg.test(new_Password) && numReg.test(new_Password)) {
        $(".validateTips_newpassword").text(LanguageScript.common_PasswordPrompt);
        $(".validateTips_newpassword").css("color", "gray");
        $("#ui-right_newpassword").show();
        $(".validateTips_newpassword").css("margin", "-26px 0px 8px 0px");
        check_newpassword = true;
        return true;
    }
    else {
        $(".validateTips_newpassword").text("密码格式不正确");
        $(".validateTips_newpassword").css("color", "red");
        $("#ui-right_newpassword").hide();
        $(".validateTips_newpassword").css("margin", "-13px 0px 8px 0px");
        check_newpassword = false;
        return false;
    }
};

/*验证新密码二次输入*/
function check_confirm_Password() {
    new_Password = $("#newpassword").val();
    confirm_Password = $("#confirmpassword").val();

    if (confirm_Password == "") {
        check_confirmpassword = false;
        $(".validateTips_confirmpassword").text("新密码不能为空");
        $(".validateTips_confirmpassword").css("color", "red");
        $("#ui-right_confirmpassword").hide();
        $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
        return false;
    }

    if (new_Password == confirm_Password && confirm_Password != "") {
        check_confirmpassword = true;
        $(".validateTips_confirmpassword").text(LanguageScript.common_PasswordConfirmInput);
        $(".validateTips_confirmpassword").css("color", "gray");
        $("#ui-right_confirmpassword").show();
        $(".validateTips_confirmpassword").css("margin", "-26px 0px 8px 0px");
        return true;
    }
    else {
        check_confirmpassword = false;
        $(".validateTips_confirmpassword").text("两次输入的密码不一致");
        $(".validateTips_confirmpassword").css("color", "red");
        $("#ui-right_confirmpassword").hide();
        $(".validateTips_confirmpassword").css("margin", "-13px 0px 8px 0px");
        return false;
    }
};
function Admin_Edit_Ajax(password) {
    var MD5Password = hex_md5($.trim(password));
    $.ajax({
        type: "POST",
        url: "/hck-fleetadmin/Tenant/EditAdmin_info",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: { newpassword: MD5Password },
        success: function (msg) {
            if ("OK" == msg) {
                //alert("1");
                return true;
            } else if ("Error" == msg) {
                //alert("0");
                return false;
            }
        },
        error: function () {
            return false;
        }
    });
}

function errortipinfo(text) {
    $(".errorTip").html('<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>');
    $(function () {
        $(".errorTip").dialog({
            resizable: false,
            height: 'auto',
            width: 355,
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
