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
            //signalR链接
            conn: undefined,
            //实时定位数据
            gpsDatas: [],
            //实时报警数据
            alarmDatas: [],
            //短报文数据
            msgDatas: [],
            //z-tree
            zTree: {
                treeObj: {},
                treeData: {},
                selectNode: undefined,
                setting: {
                    check: {
                        //chkStyle: "radio",
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
                                vm.dbclickLocationTable(treeNode.id.replace('car-', ''));
                            }
                        },
                        beforeCheck: function (treeId, treeNode) {
                            //设置父节点不能被选择
                            if (treeNode.children) {
                                return false;
                            }
                            vm.zTree.selectNode = undefined;
                            vm.zTree.treeObj.checkAllNodes(false);
                            return true;
                        },
                        onCheck: function (e, treeId, treeNode) {
                            vm.zTree.selectNode = treeNode;
                        }
                    }
                }
            },
            //全屏
            isFullScreen: false,
            fullScreenStyle: {
                colmd10: 'col-md-10',
                colmd12: 'col-md-12',
                fullHeight: document.documentElement.clientHeight - 146,
                normalHeight: document.documentElement.clientHeight - 430
            },
            //其他数据
            otherData: {
                drawType: 1,//画图类型
                isShowUnits: false,
                units: [],//重点单位
                unitMarkers: [],
                powers: [],//执勤力量
                powerMarkers: [],
                xhs: [],//消火栓
                xhsMarkers: [],
                rectangleObj: null,
            },

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
                        position: 'lb' //right top，右上角
                    }));
                });
            },
            //实例化设备树
            initDeviceTree() {
                axios.post('QueryDeviceTree').then(function (response) {
                    vm.zTree.treeData = response.data;
                    vm.zTree.treeObj = $.fn.zTree.init($("#treeDemo"), vm.zTree.setting, vm.zTree.treeData);
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
                                label: { content: device.license, direction: value.heading, offset: new AMap.Pixel(0, 0) },
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
                                    "<tr><td>车辆编号：</td><td>" + device.license + "</td></tr>",
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
                            device.marker.show();
                        }
                        //更新设备位置
                        if (device.lastTrackData) {
                            var destPoint = coordtransform.wgs84togcj02(device.lastTrackData.longitude, device.lastTrackData.latitude);
                            if (device.marker) {
                                device.marker.setPosition(destPoint);
                            }
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
                        //去掉定位表格里的离线数据
                        //this.gpsData
                    }
                    //计算车辆在线数;
                    if (treeObj) {
                        var nodes = treeObj.getNodes();
                        nodes.forEach(function (value) { vm.recusiveUnit(treeObj, value); })
                    }
                }, 1 * 1000);
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
                    .withUrl("../../maphub")
                    .configureLogging(signalR.LogLevel.Information)
                    .build();
                connection.start().then(() => { }).catch(err => console.error(err.toString()));
                connection.on("UpdateMapData", function (data) {
                    if (data != null && data.gpsData != null) {
                        vm.updateGpsData(data.mac, data.gpsData);
                    }
                    if (data != null && data.alarmData != null) {
                        vm.updateAlarmTable(data.mac, data.alarmData);
                    }
                });
                connection.on("ShowCommandMsg", function (data) {
                    vm.$message.success(data);
                });
                this.conn = connection;
            },
            dbclickLocationTable(mac) {
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
                var device = vm.getDevice(mac);
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
                        this.gpsDatas.unshift(newRow);

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
                        this.alarmDatas.unshift(alarmRow);
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
            //初始化其他数据(重点单位，执勤力量，消火栓)
            initOtherData() {
                //重点单位
                axios.post('QueryAllUnit').then(function (response) {
                    vm.otherData.units = response.data;
                }).catch(function (error) {
                    console.log(error);
                });
                //执勤力量
                axios.post('QueryXfKeyUnit').then(function (response) {
                    vm.otherData.powers = response.data;
                }).catch(function (error) {
                    console.log(error);
                });
                //消火栓
                axios.post('QueryXfsyxx').then(function (response) {
                    vm.otherData.xhs = response.data;
                }).catch(function (error) {
                    console.log(error);
                });
            },
            //切换显示重点单位
            switchUnit() {
                if (!this.otherData.isShowUnits) {
                    this.otherData.units.forEach((value) => {
                        var destPoint = coordtransform.wgs84togcj02(value.gis_x, value.gis_y);
                        var unitMarker = new AMap.Marker({
                            map: vm.map,
                            position: destPoint,
                            icon: "../../plugins/amap/images/zddw.png",
                            anchor: 'center',
                            offset: new AMap.Pixel(0, 0),
                            //angle: value.heading,
                            topWhenClick: true,
                            title: value.unitname,
                            clickable: true,
                            label: { content: value.unitname, direction: 90, offset: new AMap.Pixel(0, 0) },
                        });
                        vm.otherData.unitMarkers.push(unitMarker);
                    });
                } else {
                    this.otherData.unitMarkers.forEach((x) => {
                        vm.map.remove(x);
                    });
                }
                this.otherData.isShowUnits = !this.otherData.isShowUnits;
            },
            locationQuery() {
                if (this.zTree.selectNode && this.conn) {
                    var device = this.getDevice(this.zTree.selectNode.id.replace('car-', ''));
                    this.conn.invoke("LocationQuery", device.mac, device.mtype, device.ctype).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
            drawRectangle(type) {
                this.otherData.drawType = type;
                if (this.otherData.drawType == 1 && this.otherData.powerMarkers.length > 0) {
                    this.otherData.powerMarkers.forEach((x) => {
                        vm.map.remove(x);
                    });
                    this.otherData.powerMarkers = [];
                }
                if (this.otherData.drawType == 2 && this.otherData.xhsMarkers.length > 0) {
                    this.otherData.xhsMarkers.forEach((x) => {
                        vm.map.remove(x);
                    });
                    this.otherData.xhsMarkers = [];
                }
                if (this.otherData.rectangleObj != null) {
                    this.map.remove(this.otherData.rectangleObj);
                }

                this.map.plugin(["AMap.MouseTool"], function () {
                    var mouseTool = new AMap.MouseTool(vm.map);
                    // 使用鼠标工具，在地图上画框
                    mouseTool.rectangle({
                        strokeColor: 'red',
                        strokeOpacity: 0.5,
                        strokeWeight: 2,
                        strokeStyle: 'solid',
                    });
                    //设置鼠标为十字
                    vm.map.setDefaultCursor("crosshair");
                    mouseTool.on('draw', function (event) {
                        // event.obj 为绘制出来的覆盖物对象
                        vm.otherData.rectangleObj = event.obj;
                        var bounds = event.obj.getBounds();

                        if (vm.otherData.drawType == 1) {
                            vm.otherData.powers.forEach((value) => {
                                var destPoint = coordtransform.wgs84togcj02(value.gis_x, value.gis_y);
                                destPoint = new AMap.LngLat(destPoint[0], destPoint[1]);
                                var isContains = bounds.contains(destPoint);
                                if (isContains) {
                                    var powerMarker = new AMap.Marker({
                                        map: vm.map,
                                        position: destPoint,
                                        icon: "../../plugins/amap/images/zqll.png",
                                        anchor: 'center',
                                        offset: new AMap.Pixel(0, 0),
                                        //angle: value.heading,
                                        topWhenClick: true,
                                        title: value.name,
                                        clickable: true,
                                        label: { content: value.name, direction: 90, offset: new AMap.Pixel(0, 0) },
                                        //extData: device,
                                    });
                                    vm.otherData.powerMarkers.push(powerMarker);
                                }
                            });
                        }

                        if (vm.otherData.drawType == 2) {
                            vm.otherData.xhs.forEach((value) => {
                                var destPoint = coordtransform.wgs84togcj02(value.gis_x, value.gis_y);
                                destPoint = new AMap.LngLat(destPoint[0], destPoint[1]);
                                var isContains = bounds.contains(destPoint);
                                if (isContains) {
                                    var xhsMarker = new AMap.Marker({
                                        map: vm.map,
                                        position: destPoint,
                                        icon: "../../plugins/amap/images/xhs.png",
                                        anchor: 'center',
                                        offset: new AMap.Pixel(0, 0),
                                        //angle: value.heading,
                                        topWhenClick: true,
                                        title: value.symc,
                                        label: { content: value.symc, direction: 90, },
                                        //extData: device,
                                    });
                                    vm.otherData.xhsMarkers.push(xhsMarker);
                                }
                            });
                        }
                        mouseTool.close();
                        vm.map.setDefaultCursor("default");
                    });
                });
            },
            setReturnInterval() {
                if (this.zTree.selectNode && this.conn) {
                    var device = this.getDevice(this.zTree.selectNode.id.replace('car-', ''));

                    this.$prompt('请输入间隔时间（S）', '设置回传间隔', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        inputPattern: /^\d+$/,
                        inputErrorMessage: '请输入数字'
                    }).then(({ value }) => {
                        this.conn.invoke("SetReturnInterval", device.mac, device.mtype, device.ctype, value).catch(function (err) {
                            return console.error(err.toString());
                        });
                    }).catch(() => {
                        
                    });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
        },
        mounted() {
            this.initMap();
            this.initDeviceTree();
            this.initData();
            this.initTimer();
            this.initSignalR();
            this.initOtherData();
        }
    });
    //----------------//
})();