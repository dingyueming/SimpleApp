
(function () {
    'use strict';
    var vmLbsAlarm = new Vue({
        el: '#vmLbsAlarm',
        data: {
            //地图对象
            map: {},
            //查询对象
            search: {
                timeValue: [new Date(new Date(new Date().toLocaleDateString()).getTime()), new Date(new Date(new Date().toLocaleDateString()).getTime() + 24 * 60 * 60 * 1000 - 1)],
                points: null,
                feature: null,
                drawType: '',
                checked: false,
            },
            //画框类型
            options: [{
                value: '',
                label: ''
            },
            {
                value: 'drawCircle',
                label: '圆'
            },
            {
                value: 'drawPolygon',
                label: '多边形'
            },
            {
                value: 'drawRect',
                label: '矩形'
            }],
            //设备列表
            deviceList: [],
            //signalConnection
            signalConnection: undefined,
        },
        methods: {
            //初始化地图
            initMap() {
                /**
                 * 这里的map为上文HTML中新建的地图容器ID号；
                 * 下面一行代码便可以加载地图到browser上,前提是
                 * 在EzMapAPI.js中设置了相应的参数（具体查看EzMapAPI.js）
                 */
                this.map = new EzMap("map");
                // 添加地图控件 (级别控制条，也可以选择不添加)
                this.map.showStandMapControl();
                //显示比例尺
                this.map.showScaleControl();
                //设置地图级别
                this.map.zoomTo(5);
                //设置中心点
                var centerCoord = new EzCoord(106.661876, 26.609144);
                this.map.centerAtLatlng(centerCoord);
                this.map.addMapEventListener(Ez.Event.MAP_CLICK, function (evt) {
                    var pixel = evt.pixel;
                    var coord = evt.coordinate;
                    var marker = vmLbsAlarm.map.forEachFeatureAtPixel(pixel, function (feature, layer) {
                        if (feature instanceof EzMarker) {
                            return feature;
                        }
                    });
                    if (marker) {
                        //通过单击marker时打开popup
                        marker.openInfoWindow(marker.openedHtml);
                    }
                });
            },
            //marker的popup
            getGpsInfoWinContent(title, gpsData) {
                //车牌号、所属单位、识别码、时间、速度、方向、状态
                var device = this.getDeviceByMac(gpsData.mac);
                if (device) {
                    var tmpArr = [
                        "<tr><td>单位：</td><td>" + device.unitname + "</td></tr>",
                        "<tr><td>识别码：</td><td>" + device.mac + "</td></tr>",
                        "<tr><td>时间：</td><td>" + gpsData.gnsstime + "</td></tr>",
                        "<tr><td>速度：</td><td>" + gpsData.speed + "</td></tr>",
                        "<tr><td>方向：</td><td>" + gpsData.headingStr + "</td></tr>",
                        "<tr><td>状态：</td><td>" + gpsData.status_Str + "</td></tr>",
                    ];
                    var openedHtml = "";
                    if (gpsData.mac.length == 8) {
                        openedHtml += "<tr><td>对讲机：</td><td>" + title + "</td></tr>";
                    } else {
                        openedHtml += "<tr><td>车牌号：</td><td>" + title + "</td></tr>";
                    }
                    return "<table style='text-align:left; line-height:22px;'>" + openedHtml + tmpArr.join(' ') + "<table>";
                }
                return "";
            },
            //初始化设备列表
            initDeviceList() {
                axios.post('../RealTimeMap/QueryDeviceList', Qs.stringify()).then((response) => {
                    vmLbsAlarm.deviceList = response.data;
                }).catch((error) => {
                    console.log(error);
                });
            },
            getDeviceByMac(mac) {
                if (Array.isArray(this.deviceList)) {
                    for (var i = 0; i < this.deviceList.length; i++) {
                        if (this.deviceList[i].mac === mac) {
                            return this.deviceList[i];
                        }
                    }
                }
            },
            //画框
            drawAction() {
                this.map.changeDragMode(vmLbsAlarm.search.drawType, function (/** feature为Ez.g.*要素类 */feature) {
                    /** 一般鼠标右键结束绘制,回调参数为动态绘制的要素,可以在回调中进行余下操作,例如，增加绘制要素到地图上. */
                    vmLbsAlarm.map.addOverlay(feature);
                    if (vmLbsAlarm.search.drawType == 'drawRect') {
                        vmLbsAlarm.search.points = feature.getPoints()[0];
                    }
                    vmLbsAlarm.search.feature = feature;
                    vmLbsAlarm.searchAction();
                });
            },
            //查询
            searchAction() {
                if (this.search.timeValue == null) {
                    this.$message.error('请选择时间');
                    return;
                }
                this.map.clear();
                //清除所有设备的marker
                for (var i = 0; i < this.deviceList.length; i++) {
                    var device = this.deviceList[i];
                    if (device.marker) {
                        device.marker = null;
                    }
                }
                const loading = this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                });
                axios.post('QueryLbsData', Qs.stringify({ dateTimes: this.search.timeValue, points: this.search.points })).then((response) => {
                    if (response.status == 200) {
                        var listAlarm = response.data;
                        if (listAlarm && Array.isArray(listAlarm)) {
                            listAlarm.forEach((alarm) => {
                                var flag = false;
                                if (vmLbsAlarm.search.drawType != '') {
                                    //判断点是否在多边形内
                                    if (vmLbsAlarm.search.drawType == 'drawPolygon' || vmLbsAlarm.search.drawType == 'drawRect') {
                                        var points = vmLbsAlarm.search.feature.getPoints()[0]
                                        flag = MapLib.pointInsidePolygon(points, [alarm.jd, alarm.wd]);
                                    }
                                    //判断点是否在圆内
                                    if (vmLbsAlarm.search.drawType == 'drawCircle') {
                                        var center = vmLbsAlarm.search.feature.getCenter();
                                        var radius = vmLbsAlarm.search.feature.getRadius();
                                        flag = MapLib.pointInsideCircle([alarm.jd, alarm.wd], center.coordinate_, radius);
                                    }
                                } else {
                                    flag = true;
                                }
                                var icon = new EzIcon({
                                    src: '../../plugins/ezMap/images/red.png',
                                    anchor: [0.5, 1],
                                    anchorXUnits: 'fraction',
                                    anchorYUnits: 'fraction',
                                    opacity: 1
                                });
                                var position = new EzCoord(alarm.jd, alarm.wd);
                                var marker = new EzMarker(position, icon);
                                //构造打开的html
                                var tmparr = [
                                    "<div style='margin-top:15px;;word-break:break-all;'>接警单编号：" + alarm.jjdbh + "</div>",
                                    "<div style='margin-top:15px;'>报警人：" + alarm.bjrxm + "</div>",
                                    "<div style='margin-top:15px;'>联系电话：" + alarm.lxdh + "</div>",
                                    "<div style='margin-top:15px;'>报警时间：" + alarm.bjsj + "</div>",
                                    "<div style='margin-top:15px;'>管辖单位：" + alarm.gxdw.unitname + "</div>",
                                    "<div style='margin-top:15px;'>接警单位：" + alarm.jjdw.unitname + "</div>",
                                ];
                                marker.openedHtml = "<div style='text-align:left;'>" + tmparr.join(' ') + "</div>";
                                if (flag) {
                                    this.map.addOverlay(marker);
                                }
                            });
                        }
                        this.search.points = null;
                        //启动signalR
                        this.startSignalR();
                        loading.close();
                    }
                }).catch((error) => {
                    loading.close();
                    console.log(error);
                });
            },
            //启动signalr
            startSignalR() {
                if (!this.signalConnection) {
                    this.signalConnection = new signalR.HubConnectionBuilder()
                        .withUrl("../../mapHub")
                        .configureLogging(signalR.LogLevel.Information)
                        .build();
                    this.signalConnection.start().then(() => { }).catch(err => console.error(err.toString()));
                    this.signalConnection.on("UpdateMapData", function (data) {
                        if (data.gpsData != null) {
                            var gpsData = data.gpsData;
                            gpsData.mac = data.mac;
                            gpsData.gnsstime = gpsData.gnssTime;
                            vmLbsAlarm.updateGpsData(gpsData);
                        }
                    });
                }
            },
            //更新设备位置
            updateGpsData(lastTrackData) {
                if (!this.search.checked) {
                    return;
                }
                if (!checkLoLa(lastTrackData.longitude, lastTrackData.latitude)) return;
                var flag = false;
                if (this.search.drawType != '') {
                    //判断点是否在多边形内
                    if ((this.search.drawType == 'drawPolygon' || this.search.drawType == 'drawRect')
                        && this.search.feature && (this.search.feature instanceof Polygon || this.search.feature instanceof Rectangle)) {
                        var points = this.search.feature.getPoints()[0]
                        flag = MapLib.pointInsidePolygon(points, [lastTrackData.longitude, lastTrackData.latitude]);
                    }
                    //判断点是否在圆内
                    if (this.search.drawType == 'drawCircle' && this.search.feature && this.search.feature instanceof Circle) {
                        var center = this.search.feature.getCenter();
                        var radius = this.search.feature.getRadius();
                        flag = MapLib.pointInsideCircle([lastTrackData.longitude, lastTrackData.latitude], center.coordinate_, radius);
                    }
                } else {
                    flag = true;
                }
                var device = this.getDeviceByMac(lastTrackData.mac);
                var newpos = new EzCoord(lastTrackData.longitude, lastTrackData.latitude);
                var icon = new EzIcon({
                    src: '../../plugins/ezMap/images/' + getCarStateIcon(lastTrackData),//item.iconImage
                    anchor: [0.5, 1],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    opacity: 1
                });
                if (!device) {
                    return;
                }
                if (device.marker) {
                    device.lastTrackData = lastTrackData;
                    device.marker.show();
                    device.marker.showTitle();
                    device.marker.setIcon(icon);
                    device.marker.setPoint(newpos);
                    device.marker.title.setPosition(newpos);
                    device.marker.title.setOffset([0, -40]);
                } else {
                    /**
                    * 通过marker添加Title到地图
                    */
                    var title = new EzTitle(device.license, {
                        "fontSize": 12,
                        "fontColor": "#FFFFFF",
                        "fillColor": "#00CDFF",
                        "isStroke": true,
                        "strokeColor": "#FFFFFF",
                        "strokeWidth": 1,
                        "lineHeight": 2.2,
                        "paddingH": 15,
                        "offset": [0, -40]
                    });
                    var marker = new EzMarker(newpos, icon, title);
                    if (flag) {
                        this.map.addOverlay(marker);
                    }
                    marker.showTitle();
                    device.marker = marker;
                    device.marker.title = title;
                    device.marker.lastTrackData = lastTrackData;
                    //通过单击marker时打开popup
                    var strHtml = this.getGpsInfoWinContent(device.license, lastTrackData);
                    device.marker.openedHtml = strHtml;
                }
            },
            //清除车辆图标
            clearCar(value, e) {
                if (!this.search.checked) {
                    //清除所有设备的marker
                    for (var i = 0; i < this.deviceList.length; i++) {
                        var device = this.deviceList[i];
                        if (device.marker) {
                            device.marker = null;
                        }
                    }
                }
            }
        },
        mounted() {
            this.initMap();
            this.initDeviceList();
        }
    });
})();
