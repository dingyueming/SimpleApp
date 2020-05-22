(function () {
    'use strict';
    //----------------//
    var vm = new Vue({
        el: '#vmContent',
        data: {
            //地图
            map: {},
            //设备列表
            deviceList: [],
            //最后定位数据
            lastLocatedData: [],
            //实时定位数据
            gpsDatas: [],
            //实时报警数据
            alarmDatas: [],
            //z-tree
            zTree: {
                treeData: {},
                setting: {
                    check: {
                        enable: true//checkbox
                    },
                    view: {
                        nameIsHTML: true, //允许name支持html
                        selectedMulti: false
                    },
                    edit: {
                        enable: false,
                        editNameSelectAll: false
                    },
                    data: {
                        simpleData: {
                            enable: true
                        }
                    },
                    callback: {
                        onDblClick: function (event, treeId, treeNode) {
                            if (treeNode != null) {
                                //给所有的marker的Title变成白色字体
                                vm.deviceList.forEach(function (value) {
                                    if (value.marker) {
                                        value.marker.setLabel({
                                            offset: new AMap.Pixel(0, 0),
                                            content: "<div style='color:#000000;' >" + value.carno + "</div>", //设置文本标注内容
                                            direction: value.lastTrackData.heading //设置文本标注方位
                                        });
                                    }
                                });
                                var device = vm.getDevice(treeNode.id.replace('car-', ''));
                                if (device && device.lastTrackData) {
                                    //设置中心点
                                    var destPoint = coordtransform.wgs84togcj02(device.lastTrackData.longitude, device.lastTrackData.latitude);
                                    //destPoint = new AMap.LngLat(destPoint[0], destPoint[1]);
                                    vm.map.setZoomAndCenter(16, destPoint);
                                    //设置title变红
                                    device.marker.setLabel({
                                        offset: new AMap.Pixel(0, 0),
                                        content: "<div style='color:#FF0000;' >" + device.carno + "</div>", //设置文本标注内容
                                        direction: device.lastTrackData.heading //设置文本标注方位
                                    });
                                    device.marker.show();
                                    device.marker.setTop(true);
                                }
                            }
                        }
                    }
                }
            }
        },
        methods: {
            //实例化地图
            initMap() {
                //实例化地图
                this.map = new AMap.Map('container', {
                    center: [104.251709, 30.570383],//中心点坐标
                    zoom: 12
                });
                //加载BasicControl，loadUI的路径参数为模块名中 'ui/' 之后的部分
                AMapUI.loadUI(['control/BasicControl'], function (BasicControl) {

                    //图层切换控件
                    vm.map.addControl(new BasicControl.LayerSwitcher({
                        position: 'rt' //right top，右上角
                    }));

                    //实时交通控件
                    vm.map.addControl(new BasicControl.Traffic({
                        position: 'lb',//left bottom, 左下角
                        open: false,
                    }));
                });
            },
            //实例化设备树
            initDeviceTree() {
                axios.post('QueryDeviceTree').then(function (response) {
                    vm.zTree.treeData = response.data;
                    $.fn.zTree.init($("#treeDemo"), vm.zTree.setting, vm.zTree.treeData);
                    fuzzySearch('treeDemo', '#key', null, true); //初始化模糊搜索方法
                }).catch(function (error) {
                    console.log(error);
                });
            },
            //实例化地图设备数据
            initData() {
                let axiosList = [axios.post('QueryDeviceList'), axios.post('QueryLastLocatedData')];
                axios.all(axiosList).then(function (response) {
                    vm.deviceList = response[0].data;//定位设备
                    vm.lastLocatedData = response[1].data;//最后定位数据
                    vm.lastLocatedData.forEach((value) => {
                        var device = vm.getDevice(value.carid);
                        if (device) {
                            //var minute = Number((new Date().getTime() - new Date(value.gnsstime).getTime()) / (1000 * 60));
                            device.lastTrackData = value;
                            var destPoint = coordtransform.wgs84togcj02(value.longitude, value.latitude);
                            //destPoint = new AMap.LngLat(destPoint[0], destPoint[1]);
                            var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(value);
                            var baseMarker = new AMap.Marker({
                                map: vm.map,
                                position: destPoint,
                                icon: iconUrl,
                                anchor: 'center',
                                offset: new AMap.Pixel(0, 0),
                                angle: value.heading,
                                topWhenClick: true,
                                title: device.license + ' ' + device.carno,
                                clickable: true,
                                label: { content: device.carno, direction: value.heading, offset: new AMap.Pixel(0, 0) },
                                extData: device,
                            });
                            device.marker = baseMarker;
                            if (iconUrl.indexOf('stop.png') > -1 || iconUrl.indexOf('run.png') > -1) {
                                device.marker.setzIndex(1000); //前置
                            }
                            //监听marker点击
                            device.marker.on('click', (e) => {
                                var device = e.target.getExtData();
                                var tmpArr = [
                                    "<tr><td>车牌号：</td><td>" + device.carno + "</td></tr>",
                                    //"<tr><td>单位：</td><td>" + device.unitname + "</td></tr>",
                                    "<tr><td>识别码：</td><td>" + device.mac + "</td></tr>",
                                    "<tr><td>时间：</td><td>" + device.lastTrackData.gnsstime + "</td></tr>",
                                    "<tr><td>速度：</td><td>" + device.lastTrackData.speed + "</td></tr>",
                                    "<tr><td>方向：</td><td>" + device.lastTrackData.headingStr + "</td></tr>",
                                    "<tr><td>状态：</td><td>" + device.lastTrackData.status_Str + "</td></tr>",
                                ];
                                var openedHtml = "<table style='text-align:left; line-height:22px; width:200px;'>" + tmpArr.join(' ') + "<table>";
                                // 创建 infoWindow 实例	
                                var infoWindow = new AMap.InfoWindow({
                                    content: openedHtml,
                                    anchor: 'bottom-left',
                                    offset: new AMap.Pixel(10, -10)
                                });
                                // 打开信息窗体
                                infoWindow.open(vm.map, device.marker.getPosition());
                            }, device);
                        }
                    });
                })
            },
            //启动定时器，更新车辆状态,位置
            initTimer() {
                setInterval(function () {
                    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    for (var i = 0; i < vm.deviceList.length; i++) {
                        var device = vm.deviceList[i];
                        var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(device.lastTrackData);
                        //更新设备图标
                        if (device.marker && device.marker.getIcon() != iconUrl) {
                            device.marker.setIcon(iconUrl);
                        }
                        //更新设备位置
                        if (device.lastTrackData) {
                            var destPoint = coordtransform.wgs84togcj02(device.lastTrackData.longitude, device.lastTrackData.latitude);
                            device.marker.setPosition(destPoint);
                        }
                        //更新树图标
                        if (treeObj && treeObj != null) {
                            var node = treeObj.getNodeByParam("id", "car-" + device.carid, null);
                            if (node && node != null) {
                                node.iconSkin = getCarTreeStateSkin(device.lastTrackData);
                                treeObj.updateNode(node);
                            }
                        }
                        //隐藏地图上不在线的车辆
                        if (iconUrl.indexOf("off") > -1 && device.marker) {
                            device.marker.hide();
                        }
                    }
                    //计算车辆在线数;
                    var nodes = treeObj.getNodes();
                    nodes.forEach(function (value) { vm.recusiveUnit(treeObj, value); })
                }, 5 * 1000);
            },
            //计算车辆在线数量
            recusiveUnit(treeObj, node) {
                var count = 0;
                if (node.children && Array.isArray(node.children)) {
                    if (node.children.length > 0) {
                        node.children.forEach(function (value) {
                            if (value.id.indexOf("unit") > -1) {
                                count += vm.recusiveUnit(treeObj, value);
                            }
                            if (value.id.indexOf("car") > -1 && (value.iconSkin == "white_car" || value.iconSkin == "yellow_car" || value.iconSkin == "green_car" || value.iconSkin == "green_phone")) {
                                count++;
                            }
                        });
                    }
                    var firstName = node.name.substring(0, node.name.indexOf("/"));
                    node.name = firstName + "/" + count + ")";
                    treeObj.updateNode(node);
                }
                return count;
            },
            //根据Mac或者carid获取设备
            getDevice(macOrId) {
                if (Array.isArray(this.deviceList)) {
                    for (var i = 0; i < this.deviceList.length; i++) {
                        if (this.deviceList[i].mac == macOrId || this.deviceList[i].carid == macOrId) {
                            return this.deviceList[i];
                        }
                    }
                }
            },
            //SignalR 通信
            initSignalR() {
                const connection = new signalR.HubConnectionBuilder()
                    .withUrl("@ViewBag.SignalrUrl")
                    .configureLogging(signalR.LogLevel.Information)
                    .build();
                connection.start().then(() => { }).catch(err => console.error(err.toString()));
                connection.on("UpdateMapData", function (data) {
                    if (data.gpsData != null) {
                        updateGpsData(data.mac, data.gpsData);
                    }
                    if (data.alarmData != null) {
                        updateAlarmTable(data.mac, data.alarmData);
                    }
                });
            },
            //更新实时定位数据
            updateGpsData(mac, gpsData) {
                var device = this.getDevice(mac);
                if (device) {
                    device.lastTrackData = gpsData;
                    //更新table
                    var list = vm.gpsDatas;
                    var isTableData = false;//是否在列表中
                    for (var i = 0; i < list.length; i++) {
                        if (list[i].mac == mac) {
                            isTableData = true;
                        }
                    }
                    if (!isTableData) {
                        var newRow = {};
                        newRow.license = device.license;
                        newRow.mac = device.mac;
                        newRow.sim = device.sim;
                        newRow.gnsstime = device.lastTrackData.gnsstime;
                        newRow.receive_time = device.lastTrackData.receive_time;
                        newRow.speed = device.lastTrackData.speed;
                        newRow.locate_str = device.lastTrackData.locateStr;
                        newRow.heading_str = device.lastTrackData.headingStr;
                        newRow.status = device.lastTrackData.status_Str;
                        newRow.locatemode_str = device.lastTrackData.locateMode;
                        newRow.mileage = device.lastTrackData.mileage;
                        newRow.position = device.lastTrackData.position;
                        newRow.longitude = device.lastTrackData.longitude;
                        newRow.latitude = device.lastTrackData.latitude;
                        this.gpsDatas.push(newRow);

                    } else {
                        for (var i = 0; i < list.length; i++) {
                            var track = list[i];
                            if (track.mac == device.lastTrackData.mac) {
                                track.gnsstime = device.lastTrackData.gnsstime;
                                track.receive_time = device.lastTrackData.receive_time;
                                track.speed = device.lastTrackData.speed;
                                track.locate_str = device.lastTrackData.locateStr;
                                track.heading_str = device.lastTrackData.headingStr;
                                track.status = device.lastTrackData.status_Str;
                                track.locatemode_str = device.lastTrackData.locateMode;
                                track.mileage = device.lastTrackData.mileage;
                                track.position = device.lastTrackData.position;
                                track.longitude = device.lastTrackData.longitude;
                                track.latitude = device.lastTrackData.latitude;
                                this.$set(vm.gpsDatas, i, track);
                            }
                        }
                    }
                }
            },
            //更新报警数据
            updateAlarmData(mac, alarmData) {
                var list = vmRealTime.alarmDatas;
                var isTableData = false;
                for (var i = 0; i < list.length; i++) {
                    if (list[i].mac == mac) {
                        isTableData = true;
                    }
                }
                if (!isTableData) {
                    var dev = this.getDevice(mac);
                    if (dev) {
                        var alarmRow = {};
                        alarmRow.license = dev.license;
                        alarmRow.mac = dev.mac;
                        alarmRow.sim = dev.sim;
                        alarmRow.gnsstime = alarmData.alarmTime;
                        alarmRow.alarmtype = alarmData.alarmType;
                        alarmRow.speed = alarmData.speed;
                        alarmRow.locate_str = alarmData.locateStr;
                        alarmRow.heading_str = alarmData.headingStr;
                        alarmRow.status = alarmData.status_Str;
                        alarmRow.locatemode_str = alarmData.locateMode;
                        alarmRow.mileage = alarmData.mileage;
                        alarmRow.position = alarmData.position;
                        alarmRow.longitude = alarmData.longitude;
                        alarmRow.latitude = alarmData.latitude;
                        this.alarmDatas.push(alarmRow);
                    }
                } else {
                    for (var i = 0; i < list.length; i++) {
                        var alarm = list[i];
                        if (alarm.mac == mac) {
                            alarm.gnsstime = alarmData.alarmTime;
                            alarm.alarmtype = alarmData.alarmType;
                            alarm.speed = alarmData.speed;
                            alarm.locate_str = alarmData.locateStr;
                            alarm.heading_str = alarmData.headingStr;
                            alarm.status = alarmData.status_Str;
                            alarm.locatemode_str = alarmData.locateMode;
                            alarm.mileage = alarmData.mileage;
                            alarm.position = alarmData.position;
                            alarm.longitude = alarmData.longitude;
                            alarm.latitude = alarmData.latitude;
                            this.$set(vmRealTime.alarmDatas, i, alarm);
                        }
                    }
                }
            },
        },
        mounted() {
            this.initMap();
            this.initDeviceTree();
            this.initData();
            this.initTimer();
        }
    });
    //----------------//
})();