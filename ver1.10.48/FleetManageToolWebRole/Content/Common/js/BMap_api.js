/*****************************************************
 *        ihpleD Map API v0.1
 *        Copyright 2014-2015, ABCSoft
 *        Date: Jan 2014
 *        by:   ZhangBo && YueQQ(ak:4nrWxxMUEThqwE4sFlW1qTRP)
******************************************************/

//////////////////////////////////////////////////////
//式样格式说明
//////////////////////////////////////////////////////
/*****************************************************
 *InfoWindowOptions式样：
 *    Number:width                信息窗宽度,单位像素
 *    Number:height                信息窗高度,单位像素
 *    Number:maxWidth                信息窗最大化时的宽度,单位像素
 *    Size:offset                    信息窗位置偏移值
 *    String:title                信息窗标题文字,支持HTML内容。
 *    Boolean:enableAutoPan        是否开启信息窗口打开时地图自动移动（默认开启）
 *    Boolean:enableCloseOnClick    是否开启点击地图关闭信息窗口（默认开启）
 *    Boolean:enableMessage        是否在信息窗里显示短信发送按钮（默认开启）
 *    String:message                自定义部分的短信内容,可选项
******************************************************/
/*****************************************************
 *PolylineOptions式样：
 *    String:strokeColor            折线颜色
 *    Number:strokeWeight            折线的宽度,以像素为单位
 *    Number:strokeOpacity        折线透明度,取值范围0 - 1。
 *    String:strokeStyle            折线的样式,solid或dashed
 *    Boolean:enableMassClear        是否在调用map.clearOverlays清除此覆盖物,默认为true
 *     Boolean:enableEditing        是否启用线编辑,默认为false
 *    Boolean:enableClicking        是否响应点击事件,默认为true
******************************************************/


// ihpleD_Map 类接口
// 构造函数:ihpleD_Map
// 参数列表:
// 参数String:mapDiv            地图容器div的id值
// 参数double:lng            设置地图中心的经度,如果输入0,则地图中心为北京
// 参数double:lat            设置地图中心的纬度,如果输入0,则地图中心为北京
// 参数long:zoomLevel        地图的缩放级别,范围3~18,其中3为显示全国(2000km),11为显示城市(10km)  //15为显示500m缩放尺,18为显示50m缩放尺
// 参数long:displayNavCtl    (默认不显示)是否显示缩放平移控件,0为不显示,1为显示
// 参数long:isDragging        (默认禁用)是否启用地图拖拽,0为不可以,1可以
// 参数long:isScrollWheel    (默认禁用)是否启用滚轮放大缩小,0为不可以,1可以
function Baidu_Map(mapDiv, lng, lat, zoomLevel, displayNavCtl, isDragging, isScrollWheel)
{
    zoomLevel = (zoomLevel == undefined || typeof zoomLevel == undefined)? 11 : zoomLevel;
    displayNavCtl = (displayNavCtl == undefined || typeof displayNavCtl == undefined)? 0 : displayNavCtl;
    isDragging = (isDragging == undefined || typeof isDragging == undefined)? 0 : isDragging;
    isScrollWheel = (isScrollWheel == undefined || typeof isScrollWheel == undefined)? 0 : isScrollWheel;

    //baidu map object
    var _mapObj;
    //baidu Navigation Control
    var _navicontrol;

        //如果经纬度中有一个为0,则地图中心设为北京市
    if((0==lng)||(0==lat))
    {
        _mapObj = new BMap.Map(mapDiv, { enableMapClick: false, vectorMapLevel: 99 });            // 创建Map实例
        _mapObj.centerAndZoom("北京",zoomLevel);        // 初始化地图,设置中心点坐标和地图级别。
    }
    else
    {
        //描画出地图
        _mapObj = new BMap.Map(mapDiv, { enableMapClick: false, vectorMapLevel: 99 });            // 创建Map实例
        var point = new BMap.Point(lng, lat);        // 创建点坐标
        _mapObj.centerAndZoom(point,zoomLevel);        // 初始化地图,设置中心点坐标和地图级别。
    }
    
    //添加默认缩放平移控件
    if( 1 == displayNavCtl )
    {
        _navicontrol = new BMap.NavigationControl();
        _mapObj.addControl(_navicontrol);    // 添加默认缩放平移控件

        _scaleControl = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT });
        _mapObj.addControl(_scaleControl);// 在右下添加比例尺控件
    }

    //启用或禁用地图拖拽
    if( 1 == isDragging )
    {
        _mapObj.enableDragging(true);    //启用地图拖拽
    }
    else 
    {
        _mapObj.disableDragging();    //禁用地图拖拽
    }
    
    
    //禁用或启用滚轮放大缩小
    if( 1 == isScrollWheel )
    {
        _mapObj.enableScrollWheelZoom(true);   //启用滚轮放大缩小
    }
    else
    {
        _mapObj.disableScrollWheelZoom();   //禁用滚轮放大缩小
    }
    
    
    ///////////////////////////////////////////////////////////////////////
    //类方法的实现
    ///////////////////////////////////////////////////////////////////////
    // 类方法 get_mapObj()取得私有成员 _mapObj
    this.get_mapObj = function()
    {
        return _mapObj;
    };
    
    
    // 类方法 addOneListener(event, callBackFunc) 给地图绑定事件  
    // 参数String:event                可选参数:zoomchange 缩放比例改变；click 鼠标左键单击事件；dblclick 鼠标左键双击事件；rightclick 鼠标右键单击事件
    // 参数function:callBackFunc        回调函数名称
    this.addOneListener = function(event, callBackFunc)
    {
        (event == undefined || typeof event == undefined)? alert("参数event错误") : event;
        (callBackFunc == undefined || typeof callBackFunc == undefined)? alert("参数callBackFunc错误") : callBackFunc;
        
        _mapObj.addEventListener(event, callBackFunc);
        
        var eventListener = new Object();
        eventListener.event = event;
        eventListener.callBackFunc = callBackFunc;
        return eventListener;
    };
    
    // 类方法 removeOneListener(event, callBackFunc)    解除地图已经绑定的事件  
    // 参数列表:
    // 参数Object:eventListener    事件的句柄
    this.removeOneListener = function(eventListener)
    {
        (eventListener == undefined || typeof eventListener == undefined)? alert("参数event错误") : eventListener;
        _mapObj.removeEventListener(eventListener.event, eventListener.callBackFunc);
    };
    
    // 类方法 getMapInfo()收到当前地图视野内的地图的信息。
    // 返回 JSON.stringify 地图的信息
    this.getMapInfo = function()
    {
        var sw = new Object();
        var ne = new Object();
        var bound = new Array();
        var mapInfoObj = new Object();
        
        //获得可视区域
        sw = _mapObj.getBounds().getSouthWest();    //获得可视区域左下角
        ne = _mapObj.getBounds().getNorthEast();    //获得可视区域右上角
        bound.push(sw);
        bound.push(ne);

        mapInfoObj.bound = bound;                                //地图的边界
        mapInfoObj.center = _mapObj.getCenter();                //当前地图的中心坐标
        mapInfoObj.zoomLevel = _mapObj.getZoom();                //当前地图的缩放比例
        
        var mapInfoJson = JSON.stringify(mapInfoObj);
        return mapInfoJson;
    };
    
    // 类方法 setNewMapView(center, zoomLevel)设置地图中心点、缩放级别
    // 参数列表:
    // 参数double:lng                窗体的经度
    // 参数double:lat                窗体的纬度
    this.setNewMapView = function(lng, lat, zoomLevel)
    {
        lng = (lng == undefined || typeof lng == undefined)? alert("参数center错误") : lng;
        lat = (lat == undefined || typeof lat == undefined)? alert("参数center错误") : lat;

        if (zoomLevel != undefined) {
            _mapObj.setZoom(zoomLevel);                 //将视图切换到指定的缩放等级,中心点坐标不变
        }
        _mapObj.setCenter(new BMap.Point(lng, lat));    //设置地图中心点

    };
    
    //    类方法createInfoWindow()创建指定InfoWindow
    //  参数列表:
    //    参数double:lng                窗体的经度
    //    参数double:lat                窗体的纬度
    //    参数HTMLElement:info                窗体显示的信息:html语言,如"<div>地址 : 北京市望京阜通东大街方恒国际中心A座16层</div>"
    //    参数InfoWindowOptions:opts    设置InfoWindow的样式
    this.createInfoWindow = function(lng, lat, info, opt)
    {
        if(opt == undefined)
        {
            //InfoWindowOptions类型
            var opts = {
              width : 200,                 // 信息窗口宽度
              height: 300,                 // 信息窗口高度
              title : "InfoWindow"    // 信息窗口标题
            }
        }
        
        var infoWindow = new BMap.InfoWindow(info, opts);                  // 创建信息窗口对象
        infoWindow.enableCloseOnClick();                                // 开启点击地图时关闭信息窗口
        _mapObj.openInfoWindow(infoWindow, new BMap.Point(lng, lat));    // 开启信息窗口
    };

    //    类方法setNavControlOffSet(offset:Size)制定NavigationControl的偏移量
    //  参数列表:
    //    参数Size:offset    设置NavigationControl的偏移量
    this.setNavControlOffSet = function (offset_x, offset_y) {
        var offset = new BMap.Size(offset_x, offset_y);
        _navicontrol.setOffset(offset);
    };

    //清除地图上的图标
    this.clearOverlays=function(){
        _mapObj.clearOverlays();
    };
}

//    类createOneMaker()创建标注
//    参数BMap.Map:_mapObj        地图
//    参数double:lng            设置maker的经度
//    参数double:lat            设置maker的纬度
//    参数String:iconStr        maker图标的文件路径
//    参数double:width            设置icon的宽度
//    参数double:height            设置icon的高度
//    参数double:anchor_width            设置icon 偏移的宽度
//    参数double:anchor_height        设置icon 偏移的高度

function createOneMaker(_mapObj, lng, lat, iconStr, width, height, anchor_width, anchor_height)
{
    var _markerObj;
    this.infoWindow = null;
    var point = new BMap.Point(lng, lat);

    var _markerInfo = {
        center: point,
        width: width,
        height: height,
        offset: new BMap.Size(anchor_width, anchor_height) //Marker偏移量
    };
    
    width = (width == undefined || typeof width == undefined)? 50 : width;
    height = (height == undefined || typeof height == undefined)? 50 : height;
    
    //判断指定的标注样式
    if (iconStr == undefined)
    {    
        _markerObj = new BMap.Marker(point);                            // 创建百度默认的普通标注
    }
    else
    {
        var myIcon = new BMap.Icon(iconStr, new BMap.Size(width, height), { anchor: new BMap.Size(anchor_width, anchor_height) });    // 创建标注所使用的图标
        _markerObj = new BMap.Marker(point,{icon:myIcon});                // 创建指定的标注
    }
    _mapObj.addOverlay(_markerObj);                            // 将标注添加到地图中
    _markerObj.enableMassClear();                            // 允许覆盖物在map.clearOverlays方法中被清除。
    
    // 在地图上显示 maker
    this.show = function()
    {
        _markerObj.show();
    };
    
    // 地图上不显示 marker
    this.hide = function()
    {
        _markerObj.hide();
    };
    
    // 添加监听事件函数
    // 参数String:event            事件
    // 参数function:callBackFunc    回调函数名称
    // 返回Object:eventListener    事件的句柄
    this.addOneListener = function(event, callBackFunc)
    {
        _markerObj.addEventListener(event, callBackFunc);
        
        var eventListener = new Object();
        eventListener.event = event;
        eventListener.callBackFunc = callBackFunc;

        return eventListener;
    };
    
    // 移除已经注册的事件监听函数
    // 参数Object:eventListener    事件的句柄
    this.removeOneListener = function(eventListener)
    {
        _markerObj.removeEventListener(eventListener.event, eventListener.callBackFunc);
    };
    
    // 为marker创建并打开信息窗口
    // 参数HTMLElement:info        窗体显示的信息:html语言,如"<h4 style='margin:0 0 5px 0;padding:0.2em 0'>天安门</h4>" +"<img style='float:right;margin:4px' id='imgDemo' src='http://app.baidu.com/map/images/tiananmen.jpg' width='139' height='104' title='天安门'/>" + 
    // 参数String:imgID            InfoWindow中含有的Img图片ID(img的id信息)
    this.createInfoWindow= function(mapObj, info, opts)
    {
        this.infoWindow = new BMapLib.InfoBox(mapObj, info, opts);        // 创建信息窗口对象
        //infoWindow.enableCloseOnClick();                        // 开启点击地图时关闭信息窗口
        
        return this.infoWindow;
    };

    // 打开marker的信息窗口
    this.openInfoWindow = function (infoWin) {
        infoWin.open(_markerObj);            // 开启信息窗口
    };

    // 关闭marker的信息窗口
    this.closeInfoWindow = function (infoWin) {
        infoWin.close();
    };


    // 打开Marker旁的Lable标注
    this.setLabel = function (marker, label) {
        var tmp_marker = marker.get_markerObj();
        tmp_marker.setLabel(label);            // 打开Marker旁的Lable标注
    };

    // 取得marker的信息
    this.getMarkerInfo = function () {
        return _markerInfo;
    };

    // 取得私有成员 _mapObj
    this.get_markerObj = function () {
        return _markerObj;
    };

    this.setIcon = function (iconStr, width, height, anchor_width, anchor_height) {

        var icon = new BMap.Icon(iconStr, new BMap.Size(width, height), {anchor: new BMap.Size(anchor_width, anchor_height)});
        _markerObj.setIcon(iconStr, icon);
    };
}


//    类createRout()创建Rout
//    参数BMap.Map:_mapObj        地图
//    参数Array<Point>:rout_points    设置trip route点的数组信息
//    参数PolylineOptions:opt        设置trip route的式样
function createRout(_mapObj, rout_points, opt)
{
    var _routeObj = new BMap.Polyline(rout_points, opt);    // 绘制折线
    _mapObj.addOverlay(_routeObj);                // 将绘制的折线添加到地图中
    _routeObj.enableMassClear();                // 允许覆盖物在map.clearOverlays方法中被清除

    // 在地图上显示 TripRout
    this.show = function()
    {
        _routeObj.show();
    };
    
    // 地图上不显示 TripRout
    this.hide = function()
    {
        _routeObj.hide();
    };

}    

//    函数功能：百度坐标转成地址,并把标签为ElementID的文言设置为地址，tooltip也设成地址，并执行callbackFunc(string:address)函数
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
//    参数function:callbackFunc 函数类型：callbackFunc(string:address) 其中address为地址
function geocoderLocation(lng, lat, ElementID, a, b, c,callbackFunc) {

    var point = new BMap.Point(lng, lat);
    var geocoder = new BMap.Geocoder();
    var locationInfo = "";

    geocoder.getLocation(point, function (a, b) {

        return function (result) {

            var addComp = result.addressComponents;

            var array_address = [addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber];

            //解析后的地址信息字符串
            for (var i = 0; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                locationInfo += (array_address[i] + ",");
            }
            //把最后一个','删除
            locationInfo = locationInfo.substr(0, locationInfo.lastIndexOf(','));

            //locationInfo = addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber;

            $("#" + ElementID + "").empty();

            if (!$.trim(locationInfo)) {
                locationInfo = LanguageScript.export_undefine;
            }
            if (c == null || c == 0) {
                locationInfo = "*" + locationInfo;
            }
            //设定ElementID内显示地址信息
            $("#" + ElementID + "").append(function () {
                return "" + locationInfo + "";
            });

            //设定ElementID的title信息
            $("#" + ElementID + "").attr("title", locationInfo);

            if (callbackFunc) {
                callbackFunc(locationInfo, a, b);
            }
        }
    }(a,b));
}

//fengpan
//    函数功能：百度坐标转成地址,并把标签为ElementID的文言设置为地址，tooltip也设成地址，并执行callbackFunc(string:address)函数
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
//    参数function:callbackFunc 函数类型：callbackFunc(string:address) 其中address为地址
function geocoderLocationForVehicleList(lng, lat, ElementID, callbackFunc, a, b, c) {

    var point = new BMap.Point(lng, lat);
    var geocoder = new BMap.Geocoder();
    var locationInfo = "";
    var locationInfoTitle = "";
    var locationUpInfo = "";
    var locationDownInfo = "";
    geocoder.getLocation(point, function (a, b) {

        return function (result) {

            var addComp = result.addressComponents;

            var array_address = [addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber];

            //解析后的地址信息字符串
            for (var i = 0; i < array_address.length ; i++) {
                if (("" == array_address[i]) || (null == array_address[i])) {
                    break;
                }
                if (i < 2) {
                    locationUpInfo += array_address[i];
                }
                else {
                    locationDownInfo += array_address[i];
                }
                if (i != 0) {
                    locationInfo += (array_address[i]);
                }
                locationInfoTitle += (array_address[i]);
            }
            //locationInfo = addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber;

            $("." + ElementID + "").empty();

            $("#" + a + "").empty();
            $("#" + b + "").empty();

            if (!$.trim(locationInfo)) {
                locationInfo = LanguageScript.export_undefine;
            }
            if (!$.trim(locationUpInfo)) {
                locationDownInfo = LanguageScript.export_undefine;
            }
            if (!$.trim(locationDownInfo)) {
                locationInfo = LanguageScript.export_undefine;
            }

            //设定ElementID内显示地址信息
            $("." + ElementID + "").append(function () {
                return "" + locationInfo + "";
            });
            //设定ElementID的title信息
            $("." + ElementID + "").attr("title", locationInfoTitle);

            //设定ElementID内显示地址信息(省市)
            $("#" + a + "").append(function () {
                return "" + locationUpInfo + "";
            });
            //设定ElementID的title信息
            $("#" + a + "").attr("title", locationInfoTitle);

            //设定ElementID内显示地址信息(区县)
            $("#" + b + "").append(function () {
                return "" + locationDownInfo + "";
            });
            //设定ElementID的title信息
            $("#" + b + "").attr("title", locationInfoTitle);

            if (callbackFunc) {
                callbackFunc(locationInfo, a, b);
            }
        }
    }(a, b));
}


//    函数功能：百度坐标转成地址,并把标签为ElementID的文言设置为地址，tooltip也设成地址，并执行callbackFunc(string:address)函数
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
//    参数function:callbackFunc 函数类型：callbackFunc(string:address) 其中address为地址
function tripcoderLocation(lng, lat, ElementID, a, b, c, callbackFunc) {

    var point = new BMap.Point(lng, lat);
    var geocoder = new BMap.Geocoder();

    //function formatDateTime(date) {
    //    var myDate = new Date(date);
    //    var year = myDate.getFullYear();
    //    var month = ("0" + (myDate.getMonth() + 1)).slice(-2);
    //    var day = ("0" + myDate.getDate()).slice(-2);
    //    var h = ("0" + myDate.getHours()).slice(-2);
    //    var m = ("0" + myDate.getMinutes()).slice(-2);
    //    var s = ("0" + myDate.getSeconds()).slice(-2);
    //    var mi = ("00" + myDate.getMilliseconds()).slice(-3);
    //    return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s + "." + mi;
    //};
    //function dateDiff(interval, date1, date2) {
    //    var objInterval = { 'D': 1000 * 60 * 60 * 24, 'H': 1000 * 60 * 60, 'M': 1000 * 60, 'S': 1000, 'T': 1 };
    //    interval = interval.toUpperCase();
    //    var dt1 = new Date(Date.parse(date1.replace(/-/g, '/')));
    //    var dt2 = new Date(Date.parse(date2.replace(/-/g, '/')));
    //    try {
    //        //alert(dt2.getTime() - dt1.getTime());
    //        //alert(eval_r('objInterval.'+interval));
    //        //alert((dt2.getTime() - dt1.getTime()) / eval_r('objInterval.'+interval));
    //        return Math.round((dt2.getTime() - dt1.getTime()) / eval_r('objInterval.' + interval));
    //    }
    //    catch (e) {
    //        return e.message;
    //    }
    //}

    geocoder.getLocation(point, function (a, b, ElementID) {
       // var beginDate = new Date();
        return function (result) {
            //var endDate = new Date();
            //var diffDate = endDate.getTime() - beginDate.getTime();
            //console.log("beginDate=" + formatDateTime(beginDate) + "&&&&&&&&&&&&&&&endDate=" + formatDateTime(endDate) + "*********dateDiff=" + diffDate + "ms");
            var locationInfo = "";
            if (result != null && null != result.addressComponents) {
                var addComp = result.addressComponents;
                var array_address = [addComp.province, addComp.city, addComp.district, addComp.street, addComp.streetNumber];

                //解析后的地址信息字符串
                for (var i = 0; i < array_address.length ; i++) {
                    if (("" == array_address[i]) || (null == array_address[i])) {
                        break;
                    }
                    locationInfo += (array_address[i] + ",");
                }
                //把最后一个','删除
                locationInfo = locationInfo.substr(0, locationInfo.lastIndexOf(','));

		//locationInfo = addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber;
		if (!$.trim(locationInfo)) {
		locationInfo = LanguageScript.export_undefine;
		}
		if (c == null || c == 0) {
		locationInfo = "*" + locationInfo;
		}
            } 
            if (callbackFunc) {
                callbackFunc(locationInfo, a, b, ElementID);
            }
            
        }
    }(a, b, ElementID));
}

//    函数功能：百度坐标转成地址,并把标签为ElementID的文言设置为地址,电子围栏专用
//    参数double:lng            设置解析地点的经度
//    参数double:lat            设置解析地点的纬度
//    参数string:ElementID      标签的id
function GeoShowAddress(lng, lat, ElementID) {

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

            $("#" + ElementID + "").empty();

            //设定ElementID内显示地址信息
            $("#" + ElementID + "").append(function () {
                return "" + locationInfo + "";
            });

            //设定ElementID的title信息
            $("#" + ElementID + "").attr("title", title);
        }
    }(ElementID));
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ihpleD FleetManageTool使用
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ihpleD FleetManageTool GPS坐标变换后,描绘指定的地图
function ihpleD_Map(mapDiv, lng, lat, zoomLevel, displayNavCtl, isDragging, isScrollWheel)
{
    var BMapObj;
    BMapObj = new Baidu_Map(mapDiv, lng, lat, zoomLevel, displayNavCtl, isDragging, isScrollWheel);
    
    // 类方法 get_mapObj()取得私有成员 mapObj
    this.get_mapObj = function()
    {
        return BMapObj;
    };
}

//ihpleD FleetManageTool GPS坐标变换后,描绘指定的车辆位置Marker
function ihpleD_ShowVehicles(mapObj, VehiclesInfo, haslabel, isClear, isSetZoom)
{
    //GPS坐标
    var gpsPoint = new BMap.Point(0, 0);

    //定义函数内部变量
    var eventMakerListener1, eventMakerListener2, eventMakerListener3;
    var iconStr, width, height, iconStr_12;
    var arrayPoints = new Array();
    var label;

    var opts = {
        boxStyle:
            {background:"url('../../../Content/Home/images/VehicleInfo_bk.png')",width: "306px",height: "92px"},
            closeIconUrl: "../../../Content/Common/images/InfoClose.png')",
            closeIconMargin: "1px 1px 0px 0px",
            enableAutoPan: true,
            align: INFOBOX_AT_TOP,
            offset: new BMap.Size(0, 60)
        };

    //地图添加点击事件，点击地图时关闭信息窗口
    mapObj.addEventListener("click", function (e) {
        if (e.overlay) {
            //alert('你点击的是覆盖物：' + e.overlay.toString());
            //点击的是覆盖物时，直接返回
            return;
        } else {
            //alert('你点击的是地图');
            //点击的是地图时，把当前地图上的Infowindow消去
            var Panes = mapObj.getPanes();
            $(Panes.floatPane.children).remove();
        }
    });

    if (true == isClear) {
        mapObj.clearOverlays();
    }

    $("#u_right").undelegate("#showPopUpForSearch", "click");
    $("#show_trip_log_text_select").undelegate("#Home_choose_vehicle", "change");
    for(var i = 0; i< VehiclesInfo.length; i++)
    {
        gpsPoint.lng = VehiclesInfo[i].lng;
        gpsPoint.lat = VehiclesInfo[i].lat;

        var temp_point = new BMap.Point(gpsPoint.lng, gpsPoint.lat);
        arrayPoints.push(temp_point);
            
        switch (VehiclesInfo[i].iconKind)
        {
            case 1:    //Health
                iconStr = '../../../Content/Home/images/icon_green.png';
                iconStr_12 = '../../../Content/Home/images/icon_green_12.png';
                width = 24;
                height = 35;
                break;
            case 2:    //Health+P
                iconStr = '../../../Content/Home/images/icon_greenP.png';
                iconStr_12 = '../../../Content/Home/images/icon_greenP_12.png';
                width = 24;
                height = 35;
                break;
            case 3:    //Alert
                iconStr = '../../../Content/Home/images/icon_red.png';
                iconStr_12 = '../../../Content/Home/images/icon_red_12.png';
                width = 24;
                height = 35;
                break;
            case 4:    //Alert + P
                iconStr = '../../../Content/Home/images/icon_redP.png';
                iconStr_12 = '../../../Content/Home/images/icon_redP_12.png';
                width = 24;
                height = 35;
                break;
            case 5:    //MissedTargets
                iconStr = '../../../Content/Home/images/icon_miss.png';
                iconStr_12 = '../../../Content/Home/images/icon_miss_12.png';
                width = 24;
                height = 35;
                break;
            default:
                break;
        }
        //创建车辆位置和对应状态的Marker
        var marker = new createOneMaker(mapObj, gpsPoint.lng, gpsPoint.lat, iconStr, width, height, width / 2, height);
        //显示Marker
        marker.show();

        // 检索之后，地图上的汽车弹出info
        $("#u_right").delegate("#showPopUpForSearch", "click", function (mapObj, markerObj, vehicleWinInfo, opts, id, lng, lat, name) {
            return function () {

                if (id == $("#showPopUpForSearch")[0].value) {

                    //把当前地图上的Infowindow消去
                    var Panes = mapObj.getPanes();
                    $(Panes.floatPane.children).remove();

                    //创建InfoWindow
                    var temp_infoWindow = markerObj.createInfoWindow(mapObj, vehicleWinInfo, opts);
                    markerObj.openInfoWindow(temp_infoWindow);

                    // infowindow上的地址信息
                    var locationInfo = $("#map_popup_address" + id + "")[0].value;
                    var title = $("#map_popup_address" + id + "")[0].title;
                    $("#VehiclesInfo_Location" + id + "").text(locationInfo);
                    $("#VehiclesInfo_Location" + id + "").attr("title", title);

                    $("#showPopUpForSearch")[0].value = -1;
                }
            }
        }(mapObj, marker, VehiclesInfo[i].info, opts, VehiclesInfo[i].vehicelID, VehiclesInfo[i].lng, VehiclesInfo[i].lat, VehiclesInfo[i].name));

        // 点击dashboard 画面右面的show_trip_log_text_select之后，地图上的汽车弹出info
        $("#show_trip_log_text_select").delegate("#Home_choose_vehicle", "change", function (mapObj, markerObj, vehicleWinInfo, opts, id, lng, lat) {
            return function () {
                var choose_vehicle = document.getElementById("Home_choose_vehicle").value;
                if (id.toString() == choose_vehicle) {

                    //把当前地图上的Infowindow消去
                    var Panes = mapObj.getPanes();
                    $(Panes.floatPane.children).remove();

                    //创建InfoWindow
                    var temp_infoWindow = markerObj.createInfoWindow(mapObj, vehicleWinInfo, opts);
                    //打开InfoWindow
                    markerObj.openInfoWindow(temp_infoWindow);

                    // infowindow上的地址信息
                    var locationInfo = $("#map_popup_address" + id + "")[0].value;
                    var title = $("#map_popup_address" + id + "")[0].title;
                    $("#VehiclesInfo_Location" + id + "").text(locationInfo);
                    $("#VehiclesInfo_Location" + id + "").attr("title", title);
                }
            }
        }(mapObj, marker, VehiclesInfo[i].info, opts, VehiclesInfo[i].vehicelID, VehiclesInfo[i].lng, VehiclesInfo[i].lat));

        //为Marker添加点击事件
        eventMakerListener1 = marker.addOneListener("click", function (mapObj, markerObj, vehicleWinInfo, opts, vehicleID, lng, lat) {
            return function () {

                //关闭地图上已经存在的InfoWindow
                var Panes = mapObj.getPanes();
                $(Panes.floatPane.children).remove();

                //创建InfoWindow
                var temp_infoWindow = markerObj.createInfoWindow(mapObj, vehicleWinInfo, opts);
                markerObj.openInfoWindow(temp_infoWindow);

                // infowindow上的地址信息
                var locationInfo = $("#map_popup_address" + vehicleID + "")[0].value;
                var title = $("#map_popup_address" + vehicleID + "")[0].title;
                $("#VehiclesInfo_Location" + vehicleID + "").text(locationInfo);
                $("#VehiclesInfo_Location" + vehicleID + "").attr("title", title);

                //点击Marker时，右侧的TripLog需要显示为点击对应的车辆的信息
                //Dashboard_home.js文件中方法
                getTripLogData(vehicleID);
                var select_length = $("#Home_choose_vehicle").find("option").length;
                for (var i = 0; i < select_length; i++) {

                    if ($("#Home_choose_vehicle").find("option")[i].value == vehicleID) {
                        $("#Home_choose_vehicle").find("option")[i].selected = true;
                    } else {
                        $("#Home_choose_vehicle").find("option")[i].selected = false;
                    }
                }
                $("#Home_choose_vehicle").selectpicker('refresh');

            };
        }(mapObj, marker, VehiclesInfo[i].info, opts, VehiclesInfo[i].vehicelID, VehiclesInfo[i].lng, VehiclesInfo[i].lat));

        //为Marker添加mouseover事件：Icon放大
        eventMakerListener2 = marker.addOneListener("mouseover", (function (marker, iconStr_12) {
            return function () {

                var _height = 42;
                var _width = 30;
                marker.setIcon(iconStr_12, _width, _height, _width / 2, _height);
            };
        })(marker, iconStr_12));

            
        //为Marker添加mouseout事件：Icon缩回原大小
        eventMakerListener3 = marker.addOneListener("mouseout", (function (marker, iconStr) {
            return function () {

                var _height = 35;
                var _width = 24;
                marker.setIcon(iconStr, _width, _height, _width / 2, _height);
            };
        })(marker, iconStr));

        if ((haslabel) && (VehiclesInfo[i].licence)) {

            //生成Lable，用于显示车牌号码信息
            label = new BMap.Label(VehiclesInfo[i].licence, { offset: new BMap.Size(25, -10) });

            //定制Lable显示的式样
            var opt = {
                backgroundColor: "white",
                borderRadius: "5px",
                borderColor: "black",
                borderWidth: "1px",
                borderStyle: "solid",
                fontFamily: "Microsoft YaHei",
                fontSize: "9px",
                fontWeight: "normal",
                fontStyle: "normal",
                textSecoration: "none",
                textAlign: "center"
            };
            //设定Lable显示的式样
            label.setStyle(opt);

            //Vehicle Marker旁边显示车辆号码的Lable
            marker.setLabel(marker, label);
        }
    }

    if (isSetZoom == true) {
        //让标注显示在最佳视野内
        var style_ViewportOptions = { zoomFactor: -1 }; //地图级别的偏移量,您可以在方法得出的结果上减掉一个偏移值
        var Viewport = mapObj.getViewport(arrayPoints, style_ViewportOptions);
		
		mapObj.setZoom(Viewport.zoom);
        mapObj.setCenter(Viewport.center);
        
    }
}

//ihpleD FleetManageTool GPS坐标变换后,描绘指定的一辆车位置Marker
function ihpleD_ShowOneVehicle(mapObj, VehicleInfo)
{
    //GPS坐标
    var gpsPoint = new BMap.Point(VehicleInfo.lng, VehicleInfo.lat);
    var iconStr, width, height;

        switch (VehicleInfo.iconKind) {
            case 1:    //Health
                iconStr = '../../../Content/Home/images/icon_green.png';
                width = 24;
                height = 35;
                break;
            case 2:    //Health+P
                iconStr = '../../../Content/Home/images/icon_greenP.png';
                width = 24;
                height = 35;
                break;
            case 3:    //Alert
                iconStr = '../../../Content/Home/images/icon_red.png';
                width = 24;
                height = 35;
                break;
            case 4:    //Alert + P
                iconStr = '../../../Content/Home/images/icon_redP.png';
                width = 24;
                height = 35;
                break;
            case 5:    //MissedTargets
                iconStr = '../../../Content/Home/images/icon_miss.png';
                width = 24;
                height = 35;
                break;
            default:
                break;
        }
        //创建车辆位置和对应状态的Marker
        var marker = new createOneMaker(mapObj, gpsPoint.lng, gpsPoint.lat, iconStr, width, height, width / 2, height);
        //显示Marker
        marker.show();
}


//ihpleD FleetManageTool GPS坐标变换后,描绘指定的TripRout
function ihpleD_ShowTripRout(mapObj, TripRoutInfo)
{
    //Trip Rout Points
    var arrayPoints = new Array();
    for (var i = 0; i < TripRoutInfo.length; i++) {
        var point = new BMap.Point(TripRoutInfo[i].lng, TripRoutInfo[i].lat);
        arrayPoints.push(point);
    }

    //Polyline的式样
    var styleOptions_inside = {
        strokeColor: "rgb(0, 0, 255)",        //中线颜色
        strokeWeight: 5,            //中线的宽度,以像素为单位
        strokeOpacity: 0.5,        //中线透明度,取值范围0 - 1
        strokeStyle: 'solid',        //中线的样式,solid或dashed
        enableClicking: false        //不响应点击事件
    };

    //创建车辆的TripRout
    var rout_inside = new createRout(mapObj, arrayPoints, styleOptions_inside);
    //显示TripRout
    rout_inside.show();

    for(var i = 0; i< TripRoutInfo.length; i++)
    {
        if( 0 == i )
        {
            var iconStr = '../../../Content/Vehicle/images/iconA.png';
            var width = 28;
            var height = 27;
            var Start = new createOneMaker(mapObj, TripRoutInfo[i].lng, TripRoutInfo[i].lat, iconStr, width, height, width / 2, height);
            //显示Start Marker
            Start.show();
        }
        else if ( (TripRoutInfo.length-1) == i )
        {
            var iconStr = '../../../Content/Vehicle/images/iconB.png';
            var width = 28;
            var height = 27;
            var End = new createOneMaker(mapObj, TripRoutInfo[i].lng, TripRoutInfo[i].lat, iconStr, width, height, width / 2, height);
            //显示End Marker
            End.show();
        }
        else
        {
            var iconStr = '../../../Content/Vehicle/images/waypoint.png';
            var width = 10;
            var height = 20;
            var mid = new createOneMaker(mapObj, TripRoutInfo[i].lng, TripRoutInfo[i].lat, iconStr, width, height, width / 2, height/2);
            //显示mid Marker
            mid.show();
        }
    }

    //让TripRout显示在最佳视野内
    //var style_ViewportOptions = { zoomFactor: -1 }; //地图级别的偏移量,您可以在方法得出的结果上减掉一个偏移值
    var Viewport = mapObj.getViewport(arrayPoints);
	mapObj.setZoom(Viewport.zoom);
    mapObj.setCenter(Viewport.center);
    
}

//普通地点检索（非车、非围栏）
//功能：commonSearch_1先对地图视野内进行关键字地点的检索，如果搜索到结果则执行searchCompleteCallback函数；commonSearch_2如果没有得到结果，则在整个中国地图上搜索，执行searchCompleteCallback函数。
//参数 BMapObj 地图对象
//参数 keyword 关键字
//参数 searchCompleteCallback 检索完了之后调用的函数，原型：searchCompleteCallback(result) 其中result的类型为results: LocalResult或Array<LocalResult> 参见百度地图api 参考类！
//写一个searchCompleteCallback的例子：
//var searchCompleteCallback = function(results){
//    console.log(results);
//    for (var i = 0; i < results.getCurrentNumPois(); i++){
//        console.log(results.getPoi(i));
//    }
//};
//通过上面的searchCompleteCallback函数就可以知道results.getPoi(i)返回的结构。
function commonSearch_1(BMapObj, keyword, searchCompleteCallback) {
    //console.log("commonSearch_1");

    //本地检索，属性设定（检索完成后的回调函数）
    var options = {
        onSearchComplete: searchCompleteCallback
    };

    var local = new BMap.LocalSearch(BMapObj.get_mapObj(), options);
    local.searchInBounds(keyword, BMapObj.get_mapObj().getBounds());
}

function commonSearch_2(BMapObj, keyword, searchCompleteCallback) {

    //console.log("commonSearch_2");

    //本地检索，属性设定（检索完成后的回调函数）
    var options = {
        onSearchComplete: searchCompleteCallback
    };

    var local = new BMap.LocalSearch(BMapObj.get_mapObj(), options);
    local.search(keyword);
}