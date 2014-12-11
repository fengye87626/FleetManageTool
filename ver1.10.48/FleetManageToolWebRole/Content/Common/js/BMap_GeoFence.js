/*****************************************************
 *        ihpleD Map API v0.1
 *        Copyright 2014-2015, ABCSoft
 *        Date: Jan 2014
 *        by:   YueQQ ZhangBo(ak:4nrWxxMUEThqwE4sFlW1qTRP)
******************************************************/

//////////////////////////////////////////////////////
//式样格式说明
//////////////////////////////////////////////////////
/*****************************************************
 *CircleOptions式样：
 *    String:strokeColor            圆形边线颜色
 *    String:fillColor            圆形填充颜色。当参数为空时,圆形将没有填充效果
 *    Number:strokeWeight            圆形边线的宽度,以像素为单位
 *    Number:strokeOpacity        圆形边线透明度,取值范围0 - 1。
 *    Number:fillOpacity            圆形填充的透明度,取值范围0 - 1
 *    String:strokeStyle            圆形边线的样式,solid或dashed
 *    Boolean:enableMassClear        是否在调用map.clearOverlays清除此覆盖物,默认为true
 *    Boolean:enableEditing        是否启用线编辑,默认为false
 *    Boolean:enableClicking        是否响应点击事件,默认为true
******************************************************/
/*****************************************************
 *InfoBox式样：
 *    Size:offset                infoBox的偏移量
 *    String:boxClass                定义infoBox的class
 *    Json:boxStyle                定义infoBox的style,此项会覆盖boxClass
 *    String:closeIconMargin                    关闭按钮的margin
 *    String:closeIconUrl                关闭按钮的url地址 
 *    Boolean:enableAutoPan        是否启动自动平移功能 
 *    Number:align    基于哪个位置进行定位，取值为[INFOBOX_AT_TOP,INFOBOX_AT_BOTTOM]
******************************************************/

//地图缩放级别：3~18
var Map_zoom = {
    Zoom3: 3,
    Zoom4: 4,
    Zoom5: 5,
    Zoom6: 6,
    Zoom7: 7,
    Zoom8: 8,
    Zoom9: 9,
    Zoom10: 10,
    Zoom11: 11,
    Zoom12: 12,
    Zoom13: 13,
    Zoom14: 14,
    Zoom15: 15,
    Zoom16: 16,
    Zoom17: 17,
    Zoom18: 18
}

//常用GeoFence画面的Setting数据
var Settings = {

    mapCircleCenterIcon: { width: 21, height: 32, fileName: "../../../Content/Common/images/iconLocation.png", anchor: new BMap.Size(10, 31) },
    mapCircleDragIcon: { width: 29, height: 18, fileName: "../../../Content/Common/images/iconDrag.png", anchor: new BMap.Size(14, 9) },

    //GeoFence 视图Mode
    viewModeActiveGeoFence: 'ActiveGeoFence',                       //激活状态的GeoFence
    viewModeSelectedGeoFence: 'SelectedGeoFence',                   //激活状态且是被选中中的GeoFence
    viewModeInactiveGeoFence: 'InactiveGeoFence',                   //停用状态的GeoFence
    viewModeSelectedInactiveGeoFence: 'SelectedInactiveGeoFence',   //停用状态且是被选中中的GeoFence
    viewModeEditGeoFence: 'editGeoFence',                           //编辑中的GeoFence

    //Circle式样定义
    getCircleStyleOptions: function (viewMode) {

        var circle_styleOptions = null;

        switch (viewMode)
        {
            case Settings.viewModeActiveGeoFence:    //active
                circle_styleOptions = {
                    strokeColor: "blue",        //边线颜色。
                    fillColor: "blue",          //填充颜色。当参数为空时,圆形将没有填充效果。
                    strokeWeight: 1,            //边线的宽度,以像素为单位。
                    strokeOpacity: 0.5,         //边线透明度,取值范围0 - 1。
                    fillOpacity: 0.3,           //填充的透明度,取值范围0 - 1。
                    strokeStyle: 'solid'        //边线的样式,solid或dashed。
                };
                break;
            case Settings.viewModeSelectedGeoFence:    //selected active
                circle_styleOptions = {
                    strokeColor: "red",         //边线颜色。
                    fillColor: "blue",          //填充颜色。当参数为空时,圆形将没有填充效果。
                    strokeWeight: 1.5,          //边线的宽度,以像素为单位。
                    strokeOpacity: 0.8,         //边线透明度,取值范围0 - 1。
                    fillOpacity: 0.3,           //填充的透明度,取值范围0 - 1。
                    strokeStyle: 'solid'        //边线的样式,solid或dashed。
                };
                break;
            case Settings.viewModeInactiveGeoFence://inactive
                circle_styleOptions = {
                    strokeColor: "gray",        //边线颜色。
                    fillColor: "gray",          //填充颜色。当参数为空时,圆形将没有填充效果。
                    strokeWeight: 1,            //边线的宽度,以像素为单位。
                    strokeOpacity: 0.5,         //边线透明度,取值范围0 - 1。
                    fillOpacity: 0.6,           //填充的透明度,取值范围0 - 1。
                    strokeStyle: 'solid'        //边线的样式,solid或dashed。
                };
                break;
            case Settings.viewModeSelectedInactiveGeoFence://selected inactive
                circle_styleOptions = {
                    strokeColor: "red",         //边线颜色。
                    fillColor: "gray",          //填充颜色。当参数为空时,圆形将没有填充效果。
                    strokeWeight: 1.5,          //边线的宽度,以像素为单位。
                    strokeOpacity: 0.8,         //边线透明度,取值范围0 - 1。
                    fillOpacity: 0.6,           //填充的透明度,取值范围0 - 1。
                    strokeStyle: 'solid'        //边线的样式,solid或dashed。
                };
                break;
            case Settings.viewModeEditGeoFence:
                circle_styleOptions = {
                    strokeColor: "blue",        //边线颜色。
                    fillColor: "blue",          //填充颜色。当参数为空时,圆形将没有填充效果。
                    strokeWeight: 1,            //边线的宽度,以像素为单位。
                    strokeOpacity: 0.5,         //边线透明度,取值范围0 - 1。
                    fillOpacity: 0.3,           //填充的透明度,取值范围0 - 1。
                    strokeStyle: 'solid'        //边线的样式,solid或dashed。
                };
                break;
            default:
                break;
        }
        return circle_styleOptions;
    },

    //circle上需要添加的线
    getLineStyleOptionsForCircleRadius:function () {

        line_styleOptions= {
            strokeColor:"blue",                 //颜色
            strokeWeight:6,                     //线的宽度
            strokeOpacity:0.5,                  //线的透明度
            strokeStyle:"dashed"                //虚线
        };
        return line_styleOptions;
    },

    //编辑电子围栏，当半径适中或大的状态时，Lable的样式
    getRadiusLableOptionsBig: function (p) {
        var opts = {
            position: p,      // 指定文本标注所在的地理位置
            offset: new BMap.Size(-35, -25)          //设置文本偏移量
        }
        return opts;
    },

    //编辑电子围栏，当半径很小状态时，Lable的样式
    getRadiusLableOptionsSmall: function (p) {
        var opts = {
            position: p,      // 指定文本标注所在的地理位置
            offset: new BMap.Size(-35, -25)          //设置文本偏移量
        }
        return opts;
    },

    //编辑电子围栏，当半径适中或大的状态时，Lable内部文字的样式
    getRadiusLableWordsOptionsBig: function () {
        var opts = {
            color: "white",
            fontSize: "10pt",
            height: "10px",
            lineHeight: "20px",
            fontFamily: "微软雅黑",
            border: "",
            background: ""
        }
        return opts;
    },

    //编辑电子围栏，当半径很小状态时，Lable内部文字的样式
    getRadiusLableWordsOptionsSmall: function () {
        var opts = {
            color: "blue",
            fontSize: "10pt",
            height: "10px",
            lineHeight: "20px",
            fontFamily: "微软雅黑",
            border: "",
            background: ""
        }
        return opts;
    },

    //InfoBox样式定义
    getInfoBoxStyleOptions: function (viewMode) {

        var infobox_styleOptions = null;

        switch (viewMode)
        {
            case Settings.viewModeActiveGeoFence:    //active
            case Settings.viewModeSelectedGeoFence:    //selected active
                infobox_styleOptions = {
                    boxStyle:
                        {background:"url('../../../Content/GeoFence/images/GeoFenceInfo_bk.png')",width: "313px",height: "203px"},
                    closeIconUrl: "../../../Content/Common/images/InfoClose.png",
                    closeIconMargin: "8px 8px 0px 0px",
                    enableAutoPan: true,
                    align: INFOBOX_AT_TOP,
                    offset: new BMap.Size(0, 19)
                };
                break;
            case Settings.viewModeInactiveGeoFence://inactive
            case Settings.viewModeSelectedInactiveGeoFence://selected inactive
                infobox_styleOptions = {
                    boxStyle:
                        { background: "url('../../../Content/GeoFence/images/GeoFenceInfo_Inactive_bk.png')", width: "313px", height: "99px" },
                    closeIconUrl: "../../../Content/Common/images/InfoClose.png",
                    closeIconMargin: "8px 8px 0px 0px",
                    enableAutoPan: true,
                    align: INFOBOX_AT_TOP,
                    offset: new BMap.Size(0, 19)
                }
                break;
            default:
                break;
        }
        return infobox_styleOptions;
    },
}


//自定义的地图圆形覆盖物类
// MapCircle 类接口
// 构造函数:MapCircle
// 参数列表:
// 参数Object:mapObj            地图对象的Object
function MapCircle(mapObj, lng, lat, radius, infoBox_content) {

    //console.log("MapCircle1(lat", lat, "lng: ", lg);
    //NOTE - DO NOT REFERENCE PROPERTIES DIRECTLY, USE GETTER/SETTER METHODS
    this.mapObj = mapObj;
    this.longitude = lng;
    this.latitude = lat;
    this.radius = radius;

    this.viewMode = null;
    this.circle = null;
    this.centerMarker = null;
    this.outerMarker = null;

    //this.hasInfoBox = false;
    this.infoBox = null;//infoBox_content, infoBox_opts
    this.infoBox_content = infoBox_content;

    //显示半径文言的lable
    this.lable = null;

    //描画半径用的线
    this.lineForRadius = null;

    /**
     * Draw the circle
     */
    this.draw = function (viewMode) {
        //console.log("MapCircle.draw(lat", this.latitude, "lng: ", this.longitude);
        this.viewMode = viewMode;
         
        if (viewMode == Settings.viewModeActiveGeoFence)
        {
            this.drawCircleActiveMode();
        }
        else if (viewMode == Settings.viewModeInactiveGeoFence)
        {
            this.drawCircleInactiveMode();
        }
        else if (viewMode == Settings.viewModeEditGeoFence)
        {
            this.drawCircleEditMode();
        }
    }

    /**
     * removes circle from map
     */
    this.remove = function () {
        //not required, GeoFence Line: 290  Map.Reset(true) clears all layers
        this.circle.remove();
    }

    /**
     * Returns center as BMap.Point
     */
    this.getCenter = function () {
        return new BMap.Point(this.longitude, this.latitude);
    }

    /**
     * Sets the center of this map circle
     */
    this.setCenter = function (point) {

        if ((point == undefined)
            || ((point.lng == 0) && (point.lat == 0)))
        {
            return;
        }

        //设定圆形覆盖的中心位置
        this.circle.setCenter(point);

        //保存中心位置信息
        this.longitude = point.lng;
        this.latitude = point.lat;
    }

    /**
    * Returns the radius of this map circle
    */
    this.getRadius = function () {
        return this.radius;
    }

    /**
    * Resizes the radius of this map circle
    */
    this.setRadius = function (radius) {
        if (radius != undefined) {
            this.radius = radius;
            this.circle.setRadius(radius);
        }
    }

    /**
    * 取得circle的Bounds信息
    */
    this.getBounds = function () {

        var tmp_SouthWest = new BMap.Point(0, 0);
        var tmp_NorthEast = new BMap.Point(0, 0);

        //circle所在矩形区域的西南角
        tmp_SouthWest = this.circle.getBounds().getSouthWest();
        //circle所在矩形区域的东北角
        tmp_NorthEast = this.circle.getBounds().getNorthEast();

        var tmp_Bounds = { sw: tmp_SouthWest, ne: tmp_NorthEast };

        return tmp_Bounds;
    }

    /**
     * Gets the radius according to the map current zoom:单位为米
     */
    this.getRadiusFromZoom = function () {

        var zoom = this.mapObj.getZoom();
        var radius = 200;

        if (zoom >= Map_zoom.Zoom18)
        {
            radius = 200;
        }
        else if (zoom < Map_zoom.Zoom3)
        {
            radius = 3151677;
        }
        else
        {
            switch (zoom)
            {
                case Map_zoom.Zoom17:
                    radius = 400;
                    break;
                case Map_zoom.Zoom16:
                    radius = 800;
                    break;
                case Map_zoom.Zoom15:
                    radius = 1500;
                    break;
                case Map_zoom.Zoom14:
                    radius = 3000;
                    break;
                case Map_zoom.Zoom13:
                    radius = 6000;
                    break;
                case Map_zoom.Zoom12:
                    radius = 12000;
                    break;
                case Map_zoom.Zoom11:
                    radius = 24000;
                    break;
                case Map_zoom.Zoom10:
                    radius = 48000;
                    break;
                case Map_zoom.Zoom9:
                    radius = 96000;
                    break;
                case Map_zoom.Zoom8:
                    radius = 192000;
                    break;
                case Map_zoom.Zoom7:
                    radius = 384000;
                    break;
                case Map_zoom.Zoom6:
                    radius = 768000;
                    break;
                case Map_zoom.Zoom5:
                    radius = 1536000;
                    break;
                case Map_zoom.Zoom4:
                    radius = 3072000;
                    break;
                case Map_zoom.Zoom3:
                    radius = 3151677;
                    break;
                default:
                    break;
            }
        }
        return radius;
    }

    /**
     * Draws the circle in view mode
     */
    this.drawCircleActiveMode = function () {

        //（this.radius）的单位为米
        var circleRadius = this.radius;
        //取得Circel描画的所指定的样式
        var styleOptions = Settings.getCircleStyleOptions(this.viewMode);
        
        // 创建指定的圆形覆盖物
        this.circle = new BMap.Circle(this.getCenter(), circleRadius, styleOptions);
        // 将圆形覆盖物添加到地图中
        this.mapObj.addOverlay(this.circle);
        // 允许覆盖物在map.clearOverlays方法中被清除。
        this.circle.enableMassClear();

        //取得InfoBox的所指定的样式
        var infoBox_opts = Settings.getInfoBoxStyleOptions(this.viewMode);

        // 创建信息窗口对象
        this.infoBox = new BMapLib.InfoBox(this.mapObj, this.infoBox_content, infoBox_opts);
    }

    /**
     * Draws the circle in inactive mode
     */
    this.drawCircleInactiveMode = function () {

        //（this.radius）的单位为米
        var circleRadius = this.radius;
        //取得Circel描画的所指定的样式
        var styleOptions = Settings.getCircleStyleOptions(this.viewMode);

        // 创建指定的圆形覆盖物
        this.circle = new BMap.Circle(this.getCenter(), circleRadius, styleOptions);
        // 将圆形覆盖物添加到地图中
        this.mapObj.addOverlay(this.circle);
        // 允许覆盖物在map.clearOverlays方法中被清除。
        this.circle.enableMassClear();

        //取得InfoBox的所指定的样式
        var infoBox_opts = Settings.getInfoBoxStyleOptions(this.viewMode);

        // 创建信息窗口对象
        this.infoBox = new BMapLib.InfoBox(this.mapObj, this.infoBox_content, infoBox_opts);
    }

    /**
     * Draws the circle in edit mode
     */
    this.drawCircleEditMode = function () {

        var isNew = false;

        if (this.radius < 0) 
        {
            //it means that this is a new geofence.
            //see: GeoFence: else if (state == 'createNewFence') - if it is a new fence, the radius is set to -1
            isNew = true;
            this.radius = this.getRadiusFromZoom();
        }
 
        //（this.radius）的单位为米
        var circleRadius = this.radius;
        // 创建指定的圆形覆盖物
        var styleOptions = Settings.getCircleStyleOptions(this.viewMode);
        var circle = new BMap.Circle(this.getCenter(), circleRadius, styleOptions);
        // 将圆形覆盖物添加到地图中
        this.mapObj.addOverlay(circle);
        circle.enableMassClear();

        this.circle = circle;

        //以圆形覆盖物的中心，设定地图中心和调整地图的显示比例尺
        if (!isNew)
        {
            this.setZoomForRadius();
        }

        //显示半径文言以及描画半径
        this.setShowRadiusInit(circleRadius);

        //创建圆形覆盖物边界上的Marker
        var tmp_lng = circle.getBounds().getNorthEast().lng;
        var tmp_lat = circle.getCenter().lat;
        var tmp_point = new BMap.Point(tmp_lng, tmp_lat);

        var tmp_icon = Settings.mapCircleDragIcon;
        var outer_Icon = new BMap.Icon(tmp_icon.fileName, new BMap.Size(tmp_icon.width, tmp_icon.height), { anchor: tmp_icon.anchor });    // 创建标Marker使用的图标
        this.outerMarker = new BMap.Marker(tmp_point, { icon: outer_Icon });
        this.outerMarker.enableDragging();
        this.mapObj.addOverlay(this.outerMarker);

        //cannot reference this in the events so assign to temp variable
        var Me_Mapcircle = this;
        //Outer marker dragging event - this is the small white circle
        this.outerMarker.addEventListener("dragging", function (Me) {
        
            return function (e) {
                //force the outer marker to move only on the x axis
                if (e.point.lat != Me.circle.getCenter().lat)
                {
                    var tmp_point = new BMap.Point(e.point.lng, e.point.lat);
                    Me.outerMarker.setPosition(tmp_point);
                }

                //计算圆形覆盖物新的半径
                var lng1 = e.point.lng;
                var lng2 = Me.circle.getCenter().lng;
                var newRadius = (lng1 - lng2) * 86000;

                //console.log("new radius: ", newRadius);
                if (newRadius > 3151677)
                {
                    newRadius = 3151677;
                }

                //radius must be at least 10 metres
                if (newRadius >= 20)
                {
                    //修改MapCircle对象的半径
                    Me.setRadius(newRadius);
                }

                //force the outermarker to remain fixed at the north east position of the circle.
                lng = Me.circle.getBounds().getNorthEast().lng;
                lat = Me.circle.getCenter().lat;
                point = new BMap.Point(lng, lat);
                Me.outerMarker.setPosition(point);

                //缩放过程中半径文言以及描画半径
                Me.setShowRadiusS();
            }
        }(Me_Mapcircle));

        //创建圆形覆盖物中心点的Marker
        tmp_lng = this.circle.getCenter().lng;
        tmp_lat = this.circle.getCenter().lat;
        tmp_point = new BMap.Point(tmp_lng, tmp_lat);

        tmp_icon = Settings.mapCircleCenterIcon;
        var center_Icon = new BMap.Icon(tmp_icon.fileName, new BMap.Size(tmp_icon.width, tmp_icon.height), { anchor: tmp_icon.anchor });    // 创建标Marker使用的图标

        this.centerMarker = new BMap.Marker(tmp_point, {icon: center_Icon });
        this.centerMarker.enableDragging();
        this.mapObj.addOverlay(this.centerMarker);
        this.centerMarker.show();

        //为Center Marker 添加拖拽事件
        this.centerMarker.addEventListener("dragging", function (Me) {
            return function (e) {
                //以CenterMarker的位置为中心，设置圆形的中心点坐标，并更新MapCircle对象的数据成员
                var tmp_point = new BMap.Point(e.point.lng, e.point.lat);
                //修改MapCircle对象的中心点
                Me.setCenter(tmp_point);

                //更新圆形覆盖物边界Marker的位置
                var lng = Me.circle.getBounds().getNorthEast().lng;
                var lat = Me.circle.getCenter().lat;
                var point = new BMap.Point(lng, lat);
                Me.outerMarker.setPosition(point);

                //平移过程中半径文言以及描画半径
                Me.setShowRadiusP();
            }
        }(Me_Mapcircle));

        //为Center Marker 添加拖拽结束事件
        this.centerMarker.addEventListener("dragend", function (Me) {
            return function (e) {
                //获得dragend事件结束时，地点位置
                var tmp_point = new BMap.Point(e.point.lng, e.point.lat);

                //实时更新Location信息
                updateCurrentLocation(tmp_point);
            }
        }(Me_Mapcircle));

        this.mapObj.addEventListener("zoomend", function (Me) {
            return function () {

                //缩放过程中半径文言以及描画半径
                Me.setShowRadiusS();
            }
        }(Me_Mapcircle));
    }

    /**
    * Change the circle draw style in viewMode
    */
    this.changeCircleStyle = function (viewMode) {

        //判断入参：viewMode
        switch (viewMode)
        {
            case Settings.viewModeActiveGeoFence:
            case Settings.viewModeSelectedGeoFence:
            case Settings.viewModeInactiveGeoFence:
            case Settings.viewModeSelectedInactiveGeoFence:
                //变更this的viewMode
                this.viewMode = viewMode;

                //取得Circel描画的所指定的样式
                var styleOptions = Settings.getCircleStyleOptions(viewMode);
                //修改circle的当前描绘样式
                this.circle.setStrokeColor(styleOptions.strokeColor);
                this.circle.setFillColor(styleOptions.fillColor);
                this.circle.setStrokeOpacity(styleOptions.strokeOpacity);
                this.circle.setStrokeWeight(styleOptions.strokeWeight);
                this.circle.setFillOpacity(styleOptions.fillOpacity);
                this.circle.setStrokeStyle(styleOptions.strokeStyle);
                break;
            default:
                break;
        }
    }

    /**
    * Open the infoBox
    */
    this.openInfoBox = function () {
        this.infoBox.open(this.circle.getCenter());
        this.infoBox.setContent(this.infoBox_content);
    };

    /**
    * Close the infoBox
    */
    this.closeInfoBox = function () {
        this.infoBox.close();
    };

    /**
    * 给InfoBox添加close事件：关闭infoBox时，派发事件的接口
    */
    this.addCloseInfoBoxEvent = function (closeEventFunc) {
        if(null != this.infoBox){
            this.infoBox.addEventListener("close", closeEventFunc);
        }
    };
    
    /**
    * Change the infoBox of object
    */
    this.changeInfoBox = function (infoBox_content) {

        //取得InfoBox的所指定的样式
        var infoBox_opts = Settings.getInfoBoxStyleOptions(this.viewMode);

        // 创建信息窗口对象
        this.infoBox_content = infoBox_content;
        this.infoBox = new BMapLib.InfoBox(this.mapObj, this.infoBox_content, infoBox_opts);
    }

    /**
    * Change the content of infoBox
    */
    this.changeInfoBoxContent = function (infoBox_content) {

        if ((infoBox_content == undefined) || (typeof infoBox_content == undefined))
        {
            return;
        }
        //变更infoBox_content
        this.infoBox_content = infoBox_content;

        //判读是否存在InfoBox/InfoBox是否打开状态：关闭或者不存在时，.setContext方法会失败
        if ((null == this.infoBox) || (false ==this.infoBox.isOpen()))
        {
            return;
        }

        //变更infoBox的显示的内容
        this.infoBox.setContent(this.infoBox_content);
    }

    /**
    * Change the point of outMarker and centerMarker
    */
    this.moveCirle_EditMode = function (lng, lat) {
        if ((undefined == lng) || (undefined == lat)
            || ((0 == lng) && (0 == lat))) {
            return;
        }

        var center_point = new BMap.Point(lng, lat);
        //设定Circle的中心点
        this.setCenter(center_point);

        //移动centerMarker
        this.centerMarker.setPosition(center_point);

        //移动outMarker
        var bound = this.getBounds();
        var out_point = new BMap.Point(bound.ne.lng, center_point.lat);
        this.outerMarker.setPosition(out_point);

        //实时更新Location信息
        updateCurrentLocation(center_point);

        //平移过程中半径文言以及描画半径
        this.setShowRadiusP();
    }


    /**
     * Sets the zoom based on the circle radius
     */
    this.setZoomForRadius = function () {

        //取得circle的Bound信息
        var tmp_SouthWest = new BMap.Point(0, 0);
        var tmp_NorthEast = new BMap.Point(0, 0);
        var arrayCircleBounds = new Array();

        tmp_SouthWest = this.circle.getBounds().getSouthWest();
        tmp_NorthEast = this.circle.getBounds().getNorthEast();
        arrayCircleBounds.push(tmp_SouthWest);
        arrayCircleBounds.push(tmp_NorthEast);

        var Viewport = this.mapObj.getViewport(arrayCircleBounds);
        this.mapObj.setZoom(Viewport.zoom);
    }

    //初始时，显示半径文言以及描画半径
    this.setShowRadiusInit = function (circleRadius) {

        //取得圆形覆盖物中心的坐标
        var cPoint = new BMap.Point(this.circle.getCenter().lng, this.circle.getCenter().lat);

        //取得圆形覆盖物边界上最右边的坐标
        var rPoint = new BMap.Point(this.circle.getBounds().getNorthEast().lng, this.circle.getCenter().lat);

        //取得半径中间位置的坐标
        var ccPoint = new BMap.Point((this.circle.getBounds().getNorthEast().lng + this.circle.getCenter().lng) / 2, this.circle.getCenter().lat);

        //描画半径
        var lineForRadius = new BMap.Polyline([
                cPoint,
                rPoint
        ], Settings.getLineStyleOptionsForCircleRadius());
        this.mapObj.addOverlay(lineForRadius);

        //显示当前半径的值
        var radiusVal = new BMap.Label(Math.round(circleRadius) + "m", Settings.getRadiusLableOptionsBig(ccPoint));
        radiusVal.setStyle(Settings.getRadiusLableWordsOptionsBig());
        this.mapObj.addOverlay(radiusVal);
        this.lable = radiusVal;
        this.lineForRadius = lineForRadius;
    }

    //缩放过程中半径文言以及描画半径
    this.setShowRadiusS = function () {

        var circleRadius = Math.round(this.radius);

        //取得圆形覆盖物中心的坐标,以及将点转成像素
        var cPoint = new BMap.Point(this.circle.getCenter().lng, this.circle.getCenter().lat);
        var x1 = this.mapObj.pointToPixel(cPoint).x;

        //取得圆形覆盖物边界上最右边的坐标,以及将点转成像素
        var rPoint = new BMap.Point(this.circle.getBounds().getNorthEast().lng, this.circle.getCenter().lat);
        var x2 = this.mapObj.pointToPixel(rPoint).x;

        //取得圆形覆盖物边界上最右上的坐标,以及将点转成像素
        var nePoint = new BMap.Point(this.circle.getBounds().getNorthEast().lng, this.circle.getBounds().getNorthEast().lat);
        var x3 = this.mapObj.pointToPixel(nePoint).x;
        var y3 = this.mapObj.pointToPixel(nePoint).y;

        //取得圆形覆盖物边界上最右上角并且向右100px的坐标
        var x4 = x3 + 100;
        var y4 = y3;
        var neePoint = this.mapObj.pixelToPoint(new BMap.Pixel(x4, y4));

        //取得圆形覆盖物边界上最右上角并且向右50px的坐标
        var x5 = x3 + 50;
        var y5 = y3;
        var neePoint50 = this.mapObj.pixelToPoint(new BMap.Pixel(x5, y5));

        //取得半径中间位置的坐标
        var ccPoint = new BMap.Point((this.circle.getBounds().getNorthEast().lng + this.circle.getCenter().lng) / 2, this.circle.getCenter().lat);

        //对当前半径的像素进行判断，形成两种样式 ①半径适中或大  ②半径很小
        if ((x2 - x1) > 90) {
            //更新描画半径
            this.lineForRadius.setPath([cPoint, rPoint]);

            //更新显示当前半径的值
            this.lable.setContent(circleRadius + "m");
            this.lable.setPosition(ccPoint);
            this.lable.setStyle(Settings.getRadiusLableWordsOptionsBig());

        } else {
            //更新描画半径
            this.lineForRadius.setPath([rPoint, cPoint, nePoint, neePoint]);

            //更新显示当前半径的值
            this.lable.setContent(circleRadius + "m");
            this.lable.setPosition(neePoint50);
            this.lable.setStyle(Settings.getRadiusLableWordsOptionsSmall());
        }
    }

    //平移过程中半径文言以及描画半径
    this.setShowRadiusP = function () {

        var circleRadius = Math.round(this.radius);

        //取得圆形覆盖物中心的坐标,以及将点转成像素
        var cPoint = new BMap.Point(this.circle.getCenter().lng, this.circle.getCenter().lat);
        var x1 = this.mapObj.pointToPixel(cPoint).x;

        //取得圆形覆盖物边界上最右边的坐标,以及将点转成像素
        var rPoint = new BMap.Point(this.circle.getBounds().getNorthEast().lng, this.circle.getCenter().lat);
        var x2 = this.mapObj.pointToPixel(rPoint).x;

        //取得圆形覆盖物边界上最右上的坐标,以及将点转成像素
        var nePoint = new BMap.Point(this.circle.getBounds().getNorthEast().lng, this.circle.getBounds().getNorthEast().lat);
        var x3 = this.mapObj.pointToPixel(nePoint).x;
        var y3 = this.mapObj.pointToPixel(nePoint).y;

        //取得圆形覆盖物边界上最右上角并且向右100px的坐标
        var x4 = x3 + 100;
        var y4 = y3;
        var neePoint = this.mapObj.pixelToPoint(new BMap.Pixel(x4, y4));

        //取得圆形覆盖物边界上最右上角并且向右50px的坐标
        var x5 = x3 + 50;
        var y5 = y3;
        var neePoint50 = this.mapObj.pixelToPoint(new BMap.Pixel(x5, y5));

        //取得半径中间位置的坐标
        var ccPoint = new BMap.Point((this.circle.getBounds().getNorthEast().lng + this.circle.getCenter().lng) / 2, this.circle.getCenter().lat);

        //对当前半径的像素进行判断，形成两种样式 ①半径适中或大  ②半径很小
        if ((x2 - x1) > 90) {
            //更新描画半径
            this.lineForRadius.setPath([cPoint, rPoint]);

            //更新显示当前半径的值
            this.lable.setContent(circleRadius + "m");
            this.lable.setPosition(ccPoint);
            this.lable.setStyle(Settings.getRadiusLableWordsOptionsBig());

        } else {
            //更新描画半径
            this.lineForRadius.setPath([rPoint, cPoint, nePoint, neePoint]);

            //更新显示当前半径的值
            this.lable.setContent(circleRadius + "m");
            this.lable.setPosition(neePoint50);
            this.lable.setStyle(Settings.getRadiusLableWordsOptionsSmall());
        }
    }
}


/////////////////////////////////////////////////////////////////////////////////////
/**
* GeoFence constructor function
*/
function GeoFence(mapObj, longitude, latitude, radius, geoId, viewMode, name, infoBox_content) {

    //继承MapCircle
    MapCircle.call(this, mapObj, longitude, latitude, radius, infoBox_content);

    var Me_GeoFence = this;

    this.geoId = geoId;
    this.name = name;
    //this.isNewFence = isNew; //true if it's created during this page's existence

    //判断ViewMode
    if (viewMode == Settings.viewModeActiveGeoFence
        || viewMode == Settings.viewModeInactiveGeoFence
        || viewMode == Settings.viewModeEditGeoFence)
    {
        //call out to MapCircle.draw..
        this.draw(viewMode);

        // 为circle对象添加click事件:弹出InfoBox信息窗口、变更circle的描绘样式
        this.circle.addEventListener("click", function (geofence) {
            return function (e) {

                //判断当前geofence的InfoBox是不是打开状态
                if ((null != geofence.infoBox) && (false == geofence.infoBox.isOpen()))
                {
                    //Click事件处理:
                    //把地图上其他被选中的GeoFence样式，变更为未选中的样式
                    //把地图上打开的InfoBox关闭
                    GeoFenceCollection.click_ManagerProcess();

                    //打开geofence的InfoBox信息窗口
                    geofence.openInfoBox();
                    //给InfoBox添加Close事件
                    geofence.addCloseInfoBoxEvent(GeoFenceCollection.click_ManagerProcess);

                    //变更被点击的GeoFence的ViewMode和Circle的描绘式样为被选中状态
                    //（同时变更存储的viewMode）
                    if (geofence.viewMode == Settings.viewModeActiveGeoFence)
                    {
                        geofence.changeCircleStyle(Settings.viewModeSelectedGeoFence);
                    } else if (geofence.viewMode == Settings.viewModeInactiveGeoFence)
                    {
                        geofence.changeCircleStyle(Settings.viewModeSelectedInactiveGeoFence);
                    }

                } else {
                    //当前的geofence的InfoBox是打开的，什么也不做
                    //nothing to do
                }

            }
        }( Me_GeoFence));

    }

    //停用GeoFence==》变更GeoFence的circle描绘式样和InfoBox的InfoBox的式样及内容
    this.DeactiveGeoFence = function (infoBox_content) {
        //消去当前打开的InfoBox
        this.closeInfoBox();

        //变更circle的显示为Inactive样式（同时变更存储的viewMode）
        this.changeCircleStyle(Settings.viewModeInactiveGeoFence);
        //变更显示的InfoBox为Inactive样式和内容
        this.changeInfoBox(infoBox_content);
    }

    //激活GeoFence==》变更GeoFence的circle描绘式样和InfoBox的式样及内容
    this.ActivateGeoFence = function (infoBox_content) {

        //消去当前打开的InfoBox
        this.closeInfoBox();

        //变更circle的显示为Active样式（同时变更存储的viewMode）
        this.changeCircleStyle(Settings.viewModeActiveGeoFence);
        //变更显示的InfoBox为Inactive样式和内容
        this.changeInfoBox(infoBox_content);

    }

    //删除GeoFence
    this.DeleteGeoFence = function () {
        this.remove();
    }

}


//GeoFence容器
var GeoFenceCollection = {

    _collection: [],
    currFence: null, //geofence under construction

    length: function () {
        return this._collection.length;
    },

    push: function (geoFence) {

        var exists = false;
        for (var i = 0; i < this._collection.length; i++)
        {
            if (this._collection[i].geoId == geoFence.geoId)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            this._collection.push(geoFence);
        }
    },

    getAtIndex: function (idx) {
        if (idx < 0 || idx > this._collection.length)
        {
            //超出数组范围
            return -1;
        }
        return this._collection[idx];
    },

    getByGeoFenceId: function (geoId) {
        var fence = null;
        for (var i = 0; i < this._collection.length; i++)
        {
            if (this._collection[i].geoId == geoId)
            {
                fence = this._collection[i];
                break;
            }
        }
        if (i >= this._collection.length) {
            //没有查找到匹配的GeoId时，返回-1
            return -1;
        }
        else {
            return fence;
        }
    },

    removeGeoFence: function (GeoFenceId) {
        for (var i = 0; i < this._collection.length; i++)
        {
            gf = this._collection[i];
            if (gf.geoId == GeoFenceId)
            {
                this._collection[i].closeInfoBox();
                this._collection[i].DeleteGeoFence();
                this._collection.splice(i, 1);
                break;
            }
        }
    },

    getSelectedGeoFenceId: function () {
        var geofenceid = null;
        //循环处理：找到当前的ViewMode为'SelectedGeoFence'或者'SelectedInactiveGeoFence'的GeoFenceID
        for (var i = 0; i < this._collection.length; i++)
        {
            var geofence = this._collection[i];
            if ((geofence.viewMode == Settings.viewModeSelectedGeoFence) 
                || (geofence.viewMode == Settings.viewModeSelectedInactiveGeoFence)) {
                geofenceid = geofence.geoId;
                break;
            }
        }
        if (i >= this._collection.length)
        {
            //当前不存在被选中的GeoFence时，返回-1
            return -1;
        }
        else {
            return geofenceid;
        }
    },

    getSelectedGeoFence: function () {

        //循环处理：找到当前的ViewMode为'SelectedGeoFence'或者'SelectedInactiveGeoFence'的GeoFenceID
        for (var i = 0; i < this._collection.length; i++)
        {
            var geofence = this._collection[i];
            if ((geofence.viewMode == Settings.viewModeSelectedGeoFence) 
                || (geofence.viewMode == Settings.viewModeSelectedInactiveGeoFence)) {
                break;
            }
        }

        if (i >= this._collection.length)
        {
            //当前不存在被选中的GeoFence时，返回-1
            return -1;
        }
        else {
            return geofence;
        }
    },

    //Click响应函数：地图上的其他处理(关闭InfoBox、修改Circle的描画式样)
    click_ManagerProcess: function () {
        //查找当前被选中的GeoFence
        var selectedGeofence = GeoFenceCollection.getSelectedGeoFence();

        //当前存在被选中的GeoFence
        if (-1 != selectedGeofence)
        {
            //关闭选中的GeoFence的InfoBox
            selectedGeofence.closeInfoBox();
        }

        if (Settings.viewModeSelectedGeoFence == selectedGeofence.viewMode)
        {
            //修改ViewMode为未选中的ViewMode
            selectedGeofence.viewMode = Settings.viewModeActiveGeoFence;
            ///变更circle显示为未选中的样式（同时变更存储的viewMode）
            selectedGeofence.changeCircleStyle(Settings.viewModeActiveGeoFence);
        }
        else if (Settings.viewModeSelectedInactiveGeoFence == selectedGeofence.viewMode)
        {
            //修改ViewMode为未选中的ViewMode
            selectedGeofence.viewMode = Settings.viewModeInactiveGeoFence;
            ///变更circle显示为未选中的样式（同时变更存储的viewMode）
            selectedGeofence.changeCircleStyle(Settings.viewModeInactiveGeoFence);
        }
    }
}

//ihpleD FleetManageTool GPS坐标变换后,描绘指定的GeoFence
function ihpleD_DisplayGeoFences(mapObj, GeoFencesInfo, b_ClearMap, b_ChangeViewport, isfirst) {

    //存放circle的Bound信息
    var arrayCircleBounds = new Array();
    var tmp_viewMode = null;

    //清除地图上的覆盖物
    if (true == b_ClearMap) {
        mapObj.clearOverlays();
    }

    //清空GeoFences容器
    GeoFenceCollection._collection = [];
    //描绘每一个GeoFence,并且追加到GeoFence容器中
    for (var i = 0; i < GeoFencesInfo.length; i++) {

        //0:Inactive， 1：active
        if ( 1 == GeoFencesInfo[i].stateKind)
        {
            tmp_viewMode = 'ActiveGeoFence';
        }else if (0 == GeoFencesInfo[i].stateKind){
            tmp_viewMode = 'InactiveGeoFence';
        }
        
        //创建一个GeoFence对象
        //(mapObj, longitude, latitude, radius, geoId, viewMode, name, isNew, infoBox_content)
        var geofence = new GeoFence(mapObj, GeoFencesInfo[i].lng, GeoFencesInfo[i].lat,GeoFencesInfo[i].radius,
                        GeoFencesInfo[i].geoId, tmp_viewMode, GeoFencesInfo[i].name, GeoFencesInfo[i].info_content);
        //挨个把GeoFence追加到GeoFenceCollection容器中
        GeoFenceCollection.push(geofence);
        
        //取得GeoFence的边界信息(西南角、东北角)
        var tmp_geofenceBound = geofence.getBounds();
        arrayCircleBounds.push(tmp_geofenceBound.sw);
        arrayCircleBounds.push(tmp_geofenceBound.ne);
    }

    //判断是否进行最佳视图范围的调整
    if (true == b_ChangeViewport) {
        
        //地图级别的偏移量
        var style_ViewportOptions = { zoomFactor: 0 }; //当zoomFactor为-1：,可以在方法得出的结果上减掉一个偏移值

        //让circle显示在最佳视野内
        var Viewport = mapObj.getViewport(arrayCircleBounds, style_ViewportOptions);
        mapObj.setCenter(Viewport.center);
        mapObj.setZoom(Viewport.zoom);
    }

    //地图添加点击事件，点击地图时关闭信息窗口
    mapObj.addEventListener("click", function (e) {
        if (e.overlay) {
            //alert('你点击的是覆盖物：' + e.overlay.toString());
            //点击的是覆盖物时，直接返回
            return;
        } else {
            //alert('你点击的是地图');
            //点击的是地图时,Click事件处理:
            //把地图上其他被选中的GeoFence样式，变更为未选中的样式
            //把地图上打开的InfoBox关闭
            GeoFenceCollection.click_ManagerProcess();

        }
    });

    if (isfirst == true) {
        setTimeout(function () {
            $("#geofence_BMap")[0].style.zIndex = "";
            $("#geofence_map_for_hide").hide();
        }, 400);
    }
}

//实时更新EditGeoFence画面的current location信息
function updateCurrentLocation(Point) {

    //通过经纬度进行反地址解析
    var LocationInfo = GeoShowAddress(Point.lng, Point.lat, "geofenceEditShapes_Location");
}