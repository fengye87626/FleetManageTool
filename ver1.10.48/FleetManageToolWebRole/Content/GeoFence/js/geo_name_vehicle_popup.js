//  by zhangbo

// 围栏ID 名称 中心经度  中心纬度 半径 地址 保存类型选项（string:type 选项有"new","edit","addcar"）
function SaveGeoByPopup(GeoFenceID, name, lng, lat, radius, locationInfo, type) {

    //var vehiclesHave = [{ "name": "zhang" }, { "name": "du" }, { "name": "er" }, { "name": "埃姆斯" }, { "name": "布鲁塞尔" }, { "name": "迪斯特" }, { "name": "撒斯坦" }, { "name": "bsbss" }, { "name": "bibibi" }];
    //var vehiclesAdd = [{ "name": "开始1" }, { "name": "du" }, { "name": "er" }, { "name": "埃姆斯" }, { "name": "布鲁塞尔" }, { "name": "迪斯特" }, { "name": "撒斯坦" }, { "name": "bsbss" }, { "name": "结束" }];
    var CompanyID = GetCompanyID();
    if ("new" == type) {
        var title = LanguageScript.common_NameAndAssignGeofence;
        var name = "";
        $.ajax({
            type: "POST",
            url: "/" + CompanyID + "/GeoFence/GetTenantVehiclesByCompannyID",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: "geofenceID=" + GeoFenceID,
            success: function (result) {
                geo_name_vehicle_popup(type, title, name, locationInfo, result.hasvehicles, result.addvehicles, GeoFenceID, lng, lat, radius);
            }
        });
    } else if ("edit" == type) {
        var title = LanguageScript.common_NameAndAssignGeofence;
        $.ajax({
            type: "POST",
            url: "/" + CompanyID + "/GeoFence/GetTenantVehiclesByCompannyID",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: "geofenceID=" + GeoFenceID,
            success: function (result) {
                geo_name_vehicle_popup(type, title, name, locationInfo, result.hasvehicles, result.addvehicles, GeoFenceID, lng, lat, radius);
            }
        });
    } else if ("addcar" == type) {
        var title = LanguageScript.common_NameAndAssignGeofence;
        $.ajax({
            type: "POST",
            url: "/" + CompanyID + "/GeoFence/GetTenantVehiclesByCompannyID",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            data: "geofenceID=" + GeoFenceID,
            success: function (result) {
                ////关闭被操作的GeoFence的InfoBox，以及修改被操作的GeoFence的选择态
                //GeoFenceCollection.click_ManagerProcess();

                geo_name_vehicle_popup(type, title, name, locationInfo, result.hasvehicles, result.addvehicles, GeoFenceID, lng, lat, radius);
            }
        });
    }
}

//  为电子围栏命名或指派车辆 参照popup_012001.png (作为共通处理)
//  参数说明
//  ①string:type                选项有"new","edit","addcar"）
//  ②String:title               该popup的标题 
//  ③String:name                电子围栏的名称
//  ④String:locationInfo              电子围栏的中心地址
//  ⑥object:vehiclesHave        电子围栏已经有的车辆
//  ⑦object:vehiclesAdd         电子围栏待选新增车辆
function geo_name_vehicle_popup(type, title, name, locationInfo, vehiclesHave, vehiclesAdd, GeoFenceID, lng, lat, radius) {

    if (document.getElementById("geo_name_vehicle_popup") != null) {
        return;
    }

    var oldVehicleIDs = "";
    for (var i = 0; i < vehiclesHave.length; i++) {
        oldVehicleIDs += vehiclesHave[i].pkid + ",";
    }
    oldVehicleIDs = oldVehicleIDs.substr(0, oldVehicleIDs.length - 1);

    $("#body_position").before(function () {
        var name_readonly = "";
        var center_readonly = "";
        var vHave = "";
        var vAdd = "";
        var saveButton = "style='background-color:rgb(177,172,172)' ";
        var numHave = 0;
        name = $.trim(name);
		locationInfo =locationInfo.replace(/[^a-zA-Z0-9\_\s\,\.\u4e00-\u9fa5]/g," ");
        locationInfo = $.trim(locationInfo);
	

        if (type != "new") saveButton = "style='background-color:white'";
        if (vehiclesHave)
            for (var x = 0; x < vehiclesHave.length; x++) {

                var temp = "<tr style='border-bottom:0px; height:20px;width:280px;top:-10px;'>" +
                    "<td><input type='checkbox' " +
                    "value ='" + vehiclesHave[x].pkid +
                    "'checked='checked' id='chb" + x + "'";
                if ("edit" == type) {
                    temp += " ";
                }
                temp += "/></td>" +
                "<td><label id='v" + x + "' for='chb" + x + "' class ='GeoFence_Vehicle' title='" + $.trim(vehiclesHave[x].name) + "'>" + vehiclesHave[x].name + "</label></td>" +
            "</tr>";
                vHave += temp;
                numHave = x + 1;
            }
        if (vehiclesAdd)

            //type是Edit时，只可以修改GeoFence的name和Location信息，不可以修改包含的车辆
            for (var y = 0; y < vehiclesAdd.length; y++) {

                var temp = " <tr style='border-bottom:0px;height:20px;width:280px;top:-10px;'>" +
                    "<td><input type='checkbox' onclick='checkboxChange(this.id," + vehiclesAdd[y].pkid + ")'" +
                    "value ='" + vehiclesAdd[y].pkid +
                    "' id='chb" + (y + numHave) + "'";
                if ("edit" == type) {
                    temp += "";
                }
                temp += "/></td>" +
                    "<td><label id='v" + (y + numHave) + "' for='chb" + (y + numHave) + "' class ='GeoFence_Vehicle' title='" + $.trim(vehiclesAdd[y].name) + "'>" + vehiclesAdd[y].name + "</label></td>" +
                "</tr>";
                vAdd += temp;
            }

        var view = "<div id='geo_name_vehicle_background'></div>" +
            "<div    id='geo_name_vehicle_popup'>" +
            "<div class='geo_name_vehicle_title'>" + title + "</div>" +
            "<div    id='geo_name_vehicle_cls' />" +
            "<div class='geo_name_vehicle_con_name'>" + LanguageScript.page_geofence_GeofenceName + ":</div>";

        //type是addcar时，只可以修改包含的车辆，不可以修改GeoFence的name和Location信息
        if ("addcar" == type) {
            view += "<input  id='geo_name_vehicle_input_name' type='text' disabled='disabled'  value=" + "'" + name + "' maxlength='40'/>";
        } else {
            view += "<input  id='geo_name_vehicle_input_name' type='text' value=" + "'" + name + "' maxlength='40'/>";
        }
        //add begin for 1162 & 934 by li-xiaofei 2014/4/21
        view += "<p class='validateTips_Geo_Name_Vehicle'style='margin:100px 16px 8px 16px; color:gray;font-size:12px'>支持1-40个中文、半角字符的英文、数字、空格、下划线</p>";
        view += "<div id='ui-geo_name' class='show_name_must_fill'>*</div>";
        //add end for 1162 & 934 by li-xiaofei 2014/4/21
        view += "<div class='show_name_right'>" +
                "<img id='name_right' src='/Content/Tenant/images/Right.png')' style='display:none;'/>" +
                "<img id='name_error' src='/Content/Tenant/images/Error.png')' style='display:none;'/>" +
                "</div>" +
        "<div class='geo_name_vehicle_con_center'>" + LanguageScript.page_geofence_CenterLocation + ":</div>";
        if ("addcar" == type)
        {
            view += "<input  id='geo_name_vehicle_input_center' type='text' disabled='disabled' value=" + "'" + locationInfo + "' maxlength='100'/>";
        }else{
            view += "<input  id='geo_name_vehicle_input_center' type='text' value=" + "'" + locationInfo + "' maxlength='100'/>";
        }
        //add begin for 1162 & 934 by li-xiaofei 2014/4/21
        view += "<p class='validateTips_Geo_Name_Vehicle_Center'style='margin:65px 16px 8px 16px; color:gray;font-size:12px;width=390px'>支持1-100个中文、半角字符的英文、数字、下划线、英文标点的,与.</p>";
        view += "<div id='ui-geo_center_name' class='show_name_center_must_fill'>*</div>";
        //add end for 1162 & 934 by li-xiaofei 2014/4/21
        view += "<div class='show_location_right'>" +
               "<img id='location_right' src='/Content/Tenant/images/Right.png')' style='display:none;'/>" +
               "<img id='location_error' src='/Content/Tenant/images/Error.png')' style='display:none;'/>" +
               "</div>" +
       "<div class='geo_name_vehicle_con_vehicle'>" + LanguageScript.page_geofence_AssignVehicle + ":</div>" +
            "<div class='geo_name_vehicle_vehicle_sel' id ='geo_name_vehicle_vehicle_sel_id' >" +
            "<table style='position:absolute;'>" + vHave + vAdd +
            "</table>" +
            "</div>" +
            "<button class='geo_name_vehicle_con_save' "+"><div style='color:black;'>" + LanguageScript.common_save + "</div></button>" +
            "<div    id='geo_name_vehicle_con_cancel'>" + LanguageScript.common_cancel + "</div>" +
        "</div>";

        return view;
    });


    $("#geo_name_vehicle_cls").click(function () {
        $("#geo_name_vehicle_background").remove();
        $("#geo_name_vehicle_popup").remove();
    });

    $("#geo_name_vehicle_con_cancel").click(function () {
        $("#geo_name_vehicle_background").remove();
        $("#geo_name_vehicle_popup").remove();
    });

    $("#geo_name_vehicle_input_name").blur(function () {
        var name = $("#geo_name_vehicle_input_name");

       if ("" == $.trim(name.val())) {
           $(".validateTips_Geo_Name_Vehicle").text("电子围栏名称不能为空");
            $(".validateTips_Geo_Name_Vehicle").css("color", "red");
            $("#name_right").hide();
            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle").css("font-size", "12px");
            return;
        }
        else if (isGeoName($.trim(name.val())) != 1) {
            $(".validateTips_Geo_Name_Vehicle").text("电子围栏名称格式错误");
            $(".validateTips_Geo_Name_Vehicle").css("color", "red");
            $("#name_right").hide();
            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle").css("font-size", "12px");
            return;
        } else if (!checkGeofence_same_name(GeoFenceID, lat, lng, name.val(), radius, type, oldVehicleIDs)) {//重名检测liangjiajie0311
            return;
        }else {
            $("#name_right").show();
            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle").css("font-size", "12px");
        }

    });
    // add by li-xiaofei for1162&934 on 2014/4/21
    $("#geo_name_vehicle_input_name").focus(function () {
        $(".validateTips_Geo_Name_Vehicle").text("支持1-40个中文、半角字符的英文、数字、空格、下划线");
        $(".validateTips_Geo_Name_Vehicle").css("color", "gray");
        $("#name_right").hide();
        $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
        $(".validateTips_Geo_Name_Vehicle").css("font-size", "12px");
    });
    // add end by li-xiaofei for1162&934 on 2014/4/21


    $("#geo_name_vehicle_input_center").blur(function () {
        var center = $("#geo_name_vehicle_input_center");
        if ("" == $.trim(center.val())) {
            // $(".validateTips_Geo_Name_Vehicle_Center").text(LanguageScript.error_e01266);
            $(".validateTips_Geo_Name_Vehicle_Center").text("围栏中心位置名称不能为空");

            $(".validateTips_Geo_Name_Vehicle_Center").css("color", "red");
            $("#location_right").hide();
            $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
            return;
        }
        else if (isGeoCenter($.trim(center.val())) != 1) {
            //   $(".validateTips_Geo_Name_Vehicle_Center").text(LanguageScript.error_e01267
            $(".validateTips_Geo_Name_Vehicle_Center").text("围栏中心位置名称格式错误");
            $(".validateTips_Geo_Name_Vehicle_Center").css("color", "red");
            $("#location_right").hide();
            $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
            return;
        }
        else {
            $("#location_right").show();
            $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
        }

    });

    // add by li-xiaofei for1162&934 on 2014/4/21
    $("#geo_name_vehicle_input_center").focus(function () {
        $(".validateTips_Geo_Name_Vehicle_Center").text("支持1-100个中文、半角字符的英文、数字、下划线、英文标点的,与.");
        $(".validateTips_Geo_Name_Vehicle_Center").css("color", "gray");
        $("#location_right").hide();
        $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
        $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
        $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
    });
    // add end by li-xiaofei for1162&934 on 2014/4/21

    $(".geo_name_vehicle_con_save").click(function () {

        //add for 1162 & 934 by li-xiaofei 2014/4/21
        var name = $("#geo_name_vehicle_input_name");
        if ("" == $.trim($("#geo_name_vehicle_input_name").val())) {
            // $(".validateTips_Geo_Name_Vehicle").text(LanguageScript.error_e01266);
            $(".validateTips_Geo_Name_Vehicle").text("电子围栏名称不能为空");
            $(".validateTips_Geo_Name_Vehicle").css("color", "red");
            $("#name_right").hide();
            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
            return;
        }
        else if (isGeoName($.trim($("#geo_name_vehicle_input_name").val())) != 1) {
            //  $(".validateTips_Geo_Name_Vehicle").text(LanguageScript.error_e01267);
            $(".validateTips_Geo_Name_Vehicle").text("电子围栏名称格式错误");
            $(".validateTips_Geo_Name_Vehicle").css("color", "red");
            $("#name_right").hide();
            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
            return;
        }else if (!checkGeofence_same_name(GeoFenceID, lat, lng, name.val(), radius, type, oldVehicleIDs)) {//重名检测liangjiajie0311
            return;
        }
        if ("" == $.trim($("#geo_name_vehicle_input_center").val())) {
            // $(".validateTips_Geo_Name_Vehicle_Center").text(LanguageScript.error_e01266);
            $(".validateTips_Geo_Name_Vehicle_Center").text("围栏中心位置名称不能为空");

            $(".validateTips_Geo_Name_Vehicle_Center").css("color", "red");
            $("#location_right").hide();
            $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
            return;
        }
        else if (isGeoCenter($.trim($("#geo_name_vehicle_input_center").val())) != 1) {
            //   $(".validateTips_Geo_Name_Vehicle_Center").text(LanguageScript.error_e01267
            $(".validateTips_Geo_Name_Vehicle_Center").text("围栏中心位置名称格式错误");
            $(".validateTips_Geo_Name_Vehicle_Center").css("color", "red");
            $("#location_right").hide();
            $(".validateTips_Geo_Name_Vehicle_Center").css("margin", "65px 16px 8px 16px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("font-size", "12px");
            $(".validateTips_Geo_Name_Vehicle_Center").css("width", "390px");
            return;
        }
    
        geo_name_not_same_to_save(GeoFenceID, lat, lng, name, radius, type, oldVehicleIDs);
        //add for 1162 & 934 by li-xiaofei 2014/4/21
    });
}

       // 保存GEO之前要检查 是否与已知的GEO名称重合
    function checkGeofence_same_name(GeoFenceID, lat, lng, name, radius, type, oldVehicleIDs) {
                        var checkflag = true;
                        //取得租户公司ID
                        var CompanyID = GetCompanyID();

                        //新加电子围栏
                        if (GeoFenceID == (-1)) {
                            $.ajax({
                                type: "POST",
                                async: false,
                                url: "/" + CompanyID + "/GeoFence/GetGeofencesInfo",
                                data: { group_id: (-1) },
                                contentType: "application/x-www-form-urlencoded",
                                dataType: "json",
                                success: function (data) {
                                    if (data) {
                                        var flag = false;
                                        for (var i = 0; i < data.length; i++) {

                                            var name_have = $.trim(data[i].geofence.name);
                                            //判断名称是否重合
                                            if (name_have == name) {
                                                flag = true;
                                                break;
                                            }
                                        }
                                        if (flag == true) {
                                            // add by li-xiaofei for 1162 &934
                                            $(".validateTips_Geo_Name_Vehicle").text("您添加的围栏已重名!");
                                            $(".validateTips_Geo_Name_Vehicle").css("color", "red");
                                            $(".validateTips_Geo_Name_Vehicle").css("margin", "100px 16px 8px 16px");
                                            $(".validateTips_Geo_Name_Vehicle").css("font-size", "12px");
                                            $("#name_right").hide();
                                            checkflag = false;
                                            // add by li-xiaofei for 1162 &934
                                        } 
                                    }
                                }
                            });
                        } 
                       
                        return checkflag;
                    }


//这个函数的含义：检查是否有重名之后，再保存GEO
function geo_name_not_same_to_save(GeoFenceID, lat, lng, name, radius, type, oldVehicleIDs) {
    var name = $("#geo_name_vehicle_input_name").val();
    name = $.trim(name);
    if (type == 'new') {
        if (isGeoName(name) ) {
            $("#name_right").show();
            $("#name_error").hide();
        } else {
            $("#name_error").show();
            $("#name_right").hide();
        }
    }
    var center = $("#geo_name_vehicle_input_center").val();
    center = $.trim(center);
    if(type == 'new'){
    if (isGeoCenter(center)) {
        $("#location_right").show();
        $("#location_error").hide();
    } else {
        $("#location_error").show();
        $("#location_right").hide();
    }
   }

        if (isGeoName(name) && isGeoCenter(center)) {

            var vehicleIDs = "";

            var len = $("#geo_name_vehicle_vehicle_sel_id").find("input").length;
            for(var i =0; i<len; i++){
                var checkbox = $("#geo_name_vehicle_vehicle_sel_id").find("input")[i].checked;
                if (checkbox == true){
                    vehicleIDs += $("#geo_name_vehicle_vehicle_sel_id").find("input")[i].value + ',';
                }
            }

            vehicleIDs = vehicleIDs.substr(0, vehicleIDs.length - 1);

            //alert(vehicleIDs);

            //分情况保存数据  新添加围栏  更新围栏  添加车辆 "new","edit","addcar"
            if (type == 'new') {
                requestAddGeofence(lat, lng, center, name, radius, vehicleIDs, oldVehicleIDs);
            } else if (type == 'edit') {
                requestUpdateGeofence(GeoFenceID, lat, lng, center, name, radius, vehicleIDs, oldVehicleIDs);
            } else if (type == 'addcar') {
                requestAddGeofenceVehicle(GeoFenceID, lat, lng, center, name, radius, vehicleIDs, oldVehicleIDs);
            }
    }
}

//**********编辑分组向后台传送数据*******/
//追加电子围栏
function requestAddGeofence(Baidulat, Baidulng, location_value, name, radius, vehicleIDstr, oldVehicleIDs) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async:false,
        url: "/" + CompanyID + "/GeoFence/UpdateGeofence",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "geofenceID=" + (-1) + "&Baidulat=" + Baidulat + "&Baidulng=" + Baidulng + "&location=" + location_value + "&name=" + name + "&radius=" + radius + "&vehicleIDstr=" + vehicleIDstr + "&oldVehicleIDs=" + oldVehicleIDs,
        success: function (msg) {
            if (msg.toLocaleLowerCase() == "e400010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400010);
                return;
            } else if (msg.toLocaleLowerCase() == "e400011") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400011);
                return;
            } else if (msg.toLocaleLowerCase() == "e400012") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400012);
                return;
            } else if (msg.toLocaleLowerCase() == "e400013") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400013);
                return;
            } else if (msg.toLocaleLowerCase() == "e400014") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400014);
                return;
            } else if (msg.toLocaleLowerCase() == "e400016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400016);
                return;
            } else if (msg.toLocaleLowerCase() == "e404010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_404010);
                return;
            } else if (msg.toLocaleLowerCase() == "e409002") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_409002);
                return;
            } else if (msg.toLocaleLowerCase() == "e500015") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500015);
                return;
            } else if (msg.toLocaleLowerCase() == "e500016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500016);
                return;
            } else if (msg.toLocaleLowerCase() != "ok") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_othererror);
                return;
            }

            HrefFlag = 0;
			var nowUrl = location.href;
            var NextURL = nowUrl.substring(0, nowUrl.lastIndexOf("/"));
            NextURL += "/Landing";
            location.href = NextURL;
            //chenyangwen
            $("#geo_name_vehicle_background").remove();
            $("#geo_name_vehicle_popup").remove();
            trans();
        }
    });
}

//编辑围栏信息：中心地点、半径
function requestUpdateGeofence(geofenceID, Baidulat, Baidulng, location_value, name, radius, vehicleIDstr, oldVehicleIDs) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/UpdateGeofence",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "geofenceID=" + geofenceID + "&Baidulat=" + Baidulat + "&Baidulng=" + Baidulng + "&location=" + location_value + "&name=" + name + "&radius=" + radius + "&vehicleIDstr=" + vehicleIDstr + "&oldVehicleIDs=" + oldVehicleIDs,
        success: function (msg) {
            if (msg.toLocaleLowerCase() == "e400010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400010);
                return;
            } else if (msg.toLocaleLowerCase() == "e400011") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400011);
                return;
            } else if (msg.toLocaleLowerCase() == "e400012") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400012);
                return;
            } else if (msg.toLocaleLowerCase() == "e400013") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400013);
                return;
            } else if (msg.toLocaleLowerCase() == "e400014") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400014);
                return;
            } else if (msg.toLocaleLowerCase() == "e400016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400016);
                return;
            } else if (msg.toLocaleLowerCase() == "e404010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_404010);
                return;
            } else if (msg.toLocaleLowerCase() == "e409002") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_409002);
                return;
            } else if (msg.toLocaleLowerCase() == "e500015") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500015);
                return;
            } else if (msg.toLocaleLowerCase() == "e500016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500016);
                return;
            } else if (msg.toLocaleLowerCase() != "ok") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_othererror);
                return;
            }
            HrefFlag = 0;
            var nowUrl = location.href;
            var NextURL = nowUrl.substring(0, nowUrl.lastIndexOf("/"));
            NextURL += "/Landing";
            location.href = NextURL;
            //chenyangwen
            $("#geo_name_vehicle_background").remove();
            $("#geo_name_vehicle_popup").remove();
            trans();
        }
    });
}

//请求给某个GeoFence追加车辆
function requestAddGeofenceVehicle(geofenceID, Baidulat, Baidulng, location_value, name, radius, vehicleIDstr, oldVehicleIDs) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/UpdateGeofence",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "geofenceID=" + geofenceID + "&Baidulat=" + Baidulat + "&Baidulng=" + Baidulng + "&location=" + location_value + "&name=" + name + "&radius=" + radius + "&vehicleIDstr=" + vehicleIDstr + "&oldVehicleIDs=" + oldVehicleIDs,
        success: function (msg) {
            if (msg.toLocaleLowerCase() == "e400010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400010);
                return;
            } else if (msg.toLocaleLowerCase() == "e400011") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400011);
                return;
            } else if (msg.toLocaleLowerCase() == "e400012") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400012);
                return;
            } else if (msg.toLocaleLowerCase() == "e400013") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400013);
                return;
            } else if (msg.toLocaleLowerCase() == "e400014") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400014);
                return;
            } else if (msg.toLocaleLowerCase() == "e400016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_400016);
                return;
            } else if (msg.toLocaleLowerCase() == "e404010") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_404010);
                return;
            } else if (msg.toLocaleLowerCase() == "e409002") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_409002);
                return;
            } else if (msg.toLocaleLowerCase() == "e500015") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500015);
                return;
            } else if (msg.toLocaleLowerCase() == "e500016") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_500016);
                return;
            } else if (msg.toLocaleLowerCase() != "ok") {
                user_dialog_error6(LanguageScript.page_update_Geo_error_othererror);
                return;
            }

            //追加车辆成功后，变更GeoFence的InfoBox内容
            updateGeoFenceVehicleList(geofenceID);

            ////把地图上其他被选中的GeoFence样式，变更为未选中的样式
            ////把地图上打开的InfoBox关闭
            //GeoFenceCollection.click_ManagerProcess();

            //把save geofence的Popup消去
            $("#geo_name_vehicle_background").remove();
            $("#geo_name_vehicle_popup").remove();
        }
    });
}

function checkboxChange(chbid, pkid) {
    var CompanyID = GetCompanyID();
    $.ajax({
        type: "POST",
        async: false,
        url: "/" + CompanyID + "/GeoFence/getGeofenceNumByVehicleID",
        contentType: "application/x-www-form-urlencoded",
        dataType: "text",
        data: "vehicleID=" + pkid,
        success: function (msg) {
            GeoNum = parseInt(msg);
            if (GeoNum >= 6) {
                showPopup6();
                $("#"+chbid)[0].checked = false;
            }
        }
    });
}

//如果一个车辆的Geo数量超过6，那么将弹出dialog
function showPopup6() {
    user_dialog_error6(LanguageScript.error_e01241);
}
//wenti
//dialog 车辆的围栏数超限
function user_dialog_error6(text) {
    $(".user_error")[0].innerHTML = '<p style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;">' + text + '</p>';
    $(function () {
        $(".user_error").dialog({
            resizable: false,
            height: 140,
            width: 280,
            position: ['center', 250],
            modal: true,
	        zIndex:1110,

            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                    //var allurl = window.location.href;
                    //if (allurl.indexOf("/GeoFence/Landing") < 0) {
                    //    var tiao = localhostUrl() + "/GeoFence/Landing";

                    //    HrefFlag = 0;
                    //    location.href = tiao;
                    //}
                }
            }
        });
    });
}

var isGeoName =  function (str) {
    var reg = /^[a-zA-Z0-9\_\s\u4e00-\u9fa5]{1,40}$/;
    return reg.test(str);
}

var isGeoCenter = function (str) {
    //var reg = /^[a-zA-Z0-9\_\,\.\s\u4e00-\u9fa5]{1,100}$/;
      var reg = /^[a-zA-Z0-9\_\s\,\.\u4e00-\u9fa5]{1,100}$/
    return reg.test(str);
}
