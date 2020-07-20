(function () {
    'use strict';
    //----------------//
    var vm = new Vue({
        el: '#vmContent',
        data: {
            //地图
            map: {},
            //marker图层
            labelsLayer: {},
            //设备列表
            deviceList: [],
            //实时更新的设备
            realTimeDevice: undefined,
            //跟踪车辆
            followDevice: undefined,
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
            //命令状态数据
            commandDatas: [],
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
                    }
                }
            },
            //全屏
            isFullScreen: false,
            fullScreenStyle: {
                colmd10: 'col-md-10 ',
                colmd12: 'col-md-12 ',
                fullHeight: document.documentElement.clientHeight - 146,
                normalHeight: 633
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
            //导航点
            dhd: {
                map: undefined,
                longitude: null,
                laitude: null,
                path: 0,
                name: null,
            },
            //实战演练
            szyl: {
                map: undefined,
                longitude: null,
                laitude: null,
                name: null,
            },
            currentMarker: undefined,
            //导航路线
            directionLine: [],
            directionMarker: [],
        },
        methods: {
            //实例化地图
            initMap() {
                //实例化地图
                var amap = new AMap.Map('container', {
                    center: [104.066642, 30.656279],//中心点坐标
                    zoom: 12,
                    resizeEnable: true
                });
                this.map = amap;
                // 创建一个 LabelsLayer 实例来承载 LabelMarker，[LabelsLayer 文档](https://lbs.amap.com/api/jsapi-v2/documentation#labelslayer)
                this.labelsLayer = new AMap.LabelsLayer({
                    zooms: [3, 20],
                    //zIndex: 1000,
                    // 关闭标注避让，默认为开启，v1.4.15 新增属性
                    collision: false,
                    // 开启标注淡入动画，默认为开启，v1.4.15 新增属性
                    animation: true,
                });
                // 将 LabelsLayer 添加到地图上
                this.map.add(this.labelsLayer);
                //加载BasicControl，loadUI的路径参数为模块名中 'ui/' 之后的部分
                AMapUI.loadUI(['control/BasicControl'], function (BasicControl) {

                    //图层切换控件
                    vm.map.addControl(new BasicControl.LayerSwitcher({
                        position: 'lb' //right top，右上角
                    }));
                });
                AMap.plugin(['AMap.PlaceSearch', 'AMap.AutoComplete'], function () {
                    var auto = new AMap.AutoComplete({
                        input: "tipinput"
                    });
                    var placeSearch = new AMap.PlaceSearch({
                        map: amap
                    });  //构造地点查询类
                    auto.on("select", select);//注册监听，当选中某条记录时会触发
                    function select(e) {
                        placeSearch.setCity(e.poi.adcode);
                        placeSearch.search(e.poi.name);  //关键字查询查询
                    }
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
                const loading = this.$loading({
                    lock: true,
                    text: '正在初始化数据',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                });
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
                            var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(value);
                            var labelTitle = device.license + ' ' + (device.tecH_PARAMETERS_BRIEF == null ? "" : device.tecH_PARAMETERS_BRIEF);

                            var icon = {
                                // 图标类型，现阶段只支持 image 类型
                                type: 'image',
                                // 图片 url
                                image: iconUrl,
                                // 图片尺寸
                                size: [64, 64],
                                // 图片相对 position 的锚点，默认为 bottom-center
                                anchor: 'center',
                            };

                            var text = {
                                // 要展示的文字内容
                                content: labelTitle,
                                // 文字方向，有 icon 时为围绕文字的方向，没有 icon 时，则为相对 position 的位置
                                direction: 'top',
                                // 在 direction 基础上的偏移量
                                //offset: [-20, -5],
                                // 文字样式
                                style: {
                                    // 字体大小
                                    fontSize: 12,
                                    // 字体颜色
                                    fillColor: '#000000',
                                    // 描边颜色
                                    strokeColor: '#fff',
                                    // 描边宽度
                                    strokeWidth: 2,
                                    //背景颜色
                                    backgroundColor: '#CCCCCC',
                                }
                            };

                            var labelMarker = new AMap.LabelMarker({
                                name: device.mac, // 此属性非绘制文字内容，仅最为标识使用
                                position: destPoint,
                                map: vm.map,
                                zIndex: 16,
                                // 将第一步创建的 icon 对象传给 icon 属性
                                icon: icon,
                                // 将第二步创建的 text 对象传给 text 属性
                                text: text,
                                //标注显示级别范围， 可选值： [2,20]
                                zooms: [2, 20],
                                extData: device,
                            });

                            var baseMarker = new AMap.Marker({
                                //map: vm.map,
                                position: destPoint,
                                icon: iconUrl,
                                anchor: 'center',
                                offset: new AMap.Pixel(0, 0),
                                //angle: value.heading,
                                topWhenClick: true,
                                title: device.license + ' ' + device.carno,
                                clickable: true,
                                label: { content: labelTitle, direction: value.heading, offset: new AMap.Pixel(0, 0) },
                                extData: device,
                            });
                            vm.labelsLayer.add(labelMarker);
                            device.marker = labelMarker;
                            if (iconUrl.indexOf('stop.png') > -1 || iconUrl.indexOf('run.png') > -1) {
                                device.marker.setzIndex(1000); //前置
                            }
                            //监听marker点击
                            device.marker.on('click', (e) => {
                                var device = e.target.getExtData();
                                //console.log(device);
                                var jscs = "", cllb = "";
                                if (device.tecH_PARAMETERS != null) {
                                    jscs = device.tecH_PARAMETERS;
                                }
                                if (device.usageStr != null) {
                                    cllb = device.usageStr;
                                }
                                var tmpArr = [
                                    "<tr><td style='width:120px;'>车辆编号：</td><td  style='width:180px;'>" + device.license + "</td></tr>",
                                    "<tr><td>车牌号：</td><td>" + device.carno + "</td></tr>",
                                    "<tr><td>车辆类别：</td><td>" + cllb + "</td></tr>",
                                    "<tr><td>技术参数：</td><td>" + jscs + "</td></tr>",
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
                                infoWindow.open(vm.map, e.target.getPosition());
                            }, device);
                        }
                    });
                    loading.close();
                    vm.UpdatePage();
                })
            },
            //更新车辆状态,位置
            UpdatePage() {
                var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
                var realTimeDevice = this.realTimeDevice;
                if (realTimeDevice) {
                    var device = realTimeDevice;
                    var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(device.lastTrackData);
                    //更新设备图标
                    if (device.marker && device.marker.getIcon() != iconUrl) {
                        device.marker.setIcon(iconUrl);
                        //device.marker.show();
                    }
                    if (vm.followDevice && vm.followDevice.mac == realTimeDevice.mac) {
                        //设置中心点
                        var destPoint = coordtransform.wgs84togcj02(device.lastTrackData.longitude, device.lastTrackData.latitude);
                        vm.map.setZoomAndCenter(20, destPoint);
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
                            if (node.iconSkin != getCarTreeStateSkin(device.lastTrackData)) {
                                node.iconSkin = getCarTreeStateSkin(device.lastTrackData);
                                treeObj.updateNode(node);
                                //计算车辆在线数;
                                if (treeObj) {
                                    var nodes = treeObj.getNodes();
                                    nodes.forEach(function (value) { vm.recusiveUnit(treeObj, value); })
                                }
                            }
                        }
                    }
                } else {
                    for (var i = 0; i < vm.deviceList.length; i++) {
                        var device = vm.deviceList[i];
                        var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(device.lastTrackData);
                        //更新设备图标
                        if (device.marker && device.marker.getIcon() != iconUrl) {
                            device.marker.setIcon(iconUrl);
                            //device.marker.show();
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
                                if (node.iconSkin != getCarTreeStateSkin(device.lastTrackData)) {
                                    node.iconSkin = getCarTreeStateSkin(device.lastTrackData);
                                    treeObj.updateNode(node);
                                }
                            }
                        }
                        //隐藏地图上不在线的车辆
                        //if (iconUrl.indexOf("off") > -1 && device.marker) {
                        //    device.marker.hide();
                        //}
                    }
                    //计算车辆在线数;
                    if (treeObj) {
                        var nodes = treeObj.getNodes();
                        nodes.forEach(function (value) { vm.recusiveUnit(treeObj, value); })
                    }
                }
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
                connection.onclose(async () => {
                    connection.start().then(() => { }).catch(err => console.error(err.toString()));
                });
                connection.on("UpdateMapData", function (data) {
                    if (data != null && data.gpsData != null) {
                        vm.updateGpsData(data.mac, data.gpsData);
                    }
                    if (data != null && data.alarmData != null) {
                        vm.updateAlarmTable(data.mac, data.alarmData);
                    }
                });
                connection.on("UpdateMsgData", function (data) {
                    if (data != null) {
                        var device = vm.getDevice(data.mac);
                        vm.msgDatas.push({ unitname: device.unit.unitname, mac: data.mac, license: device.license, sender: "终端", receiver: "平台", msg: data.content.msg, time: new Date().Format("yyyy-MM-dd hh:mm:ss") });
                    }
                });
                connection.on("ShowCommandMsg", function (data) {
                    var device = vm.getDevice(data.mac);
                    vm.commandDatas.push({ unitname: device.unit.unitname, license: device.license, time: new Date().Format("yyyy-MM-dd hh:mm:ss"), cmdname: data.content.cmdStr, status: data.content.showMsg });
                    vm.$message.success(data.content.showMsg);

                });
                connection.on("DrawDirLine", function (path, directionData) {
                    //判断是否存在此导航点
                    var existDire = false;
                    var existMarker, existPolyline;
                    vm.directionMarker.forEach((value) => {
                        if (value.extData == directionData.id) {
                            existDire = true;
                            existMarker = value;
                        }
                    });
                    vm.directionLine.forEach((value) => {
                        if (value.extData == directionData.id) {
                            existPolyline = value;
                        }
                    });

                    //绘制终点
                    var markerOption = {
                        icon: "http://webapi.amap.com/images/0.png",
                        position: path[path.length - 1],
                        offset: [-10, -35],
                        label: { content: directionData.name, direction: 'top', offset: new AMap.Pixel(0, 0) },
                        extData: directionData.id
                    };
                    var direMarker = new AMap.Marker(markerOption);
                    direMarker.setMap(vm.map);
                    AMapUI.load(['ui/misc/PathSimplifier'], function (PathSimplifier) {

                        if (!PathSimplifier.supportCanvas) {
                            alert('当前环境不支持 Canvas！');
                            return;
                        }
                        var pathSimplifierIns;
                        //如果存在此线路
                        if (existDire) {
                            pathSimplifierIns = existPolyline;
                        } else {
                            //创建组件实例
                            pathSimplifierIns = new PathSimplifier({
                                zIndex: 100,
                                map: vm.map, //所属的地图实例
                                getPath: function (pathData, pathIndex) {
                                    //返回轨迹数据中的节点坐标信息，[AMap.LngLat, AMap.LngLat...] 或者 [[lng|number,lat|number],...]
                                    return pathData.path;
                                },
                                getHoverTitle: function (pathData, pathIndex, pointIndex) {
                                    //返回鼠标悬停时显示的信息
                                    if (pointIndex >= 0) {
                                        //鼠标悬停在某个轨迹节点上
                                        //return pathData.name + '，点:' + pointIndex + '/' + pathData.path.length;
                                        return "距离目的地：" + pathData.distance + "千米,预计用时" + pathData.duration + "分钟";
                                    }
                                    //鼠标悬停在节点之间的连线上
                                    //return pathData.name + '，点数量' + pathData.path.length;
                                    return "距离目的地：" + pathData.distance + "千米,预计用时" + pathData.duration + "分钟";
                                },
                                renderOptions: {
                                    //轨迹线的样式
                                    pathLineStyle: {
                                        strokeStyle: 'green',
                                        lineWidth: 6,
                                        dirArrowStyle: true
                                    }
                                }
                            });
                            pathSimplifierIns.extData = directionData.id;
                            vm.directionLine.push(pathSimplifierIns)
                        }

                        //设置数据
                        pathSimplifierIns.setData([{
                            name: '路线0',
                            path: path,
                            distance: directionData.distance,
                            duration: directionData.duration,
                        }]);

                    });

                    //如果存在此导航
                    if (existDire) {
                        //移除现有的marker
                        vm.map.remove(existMarker);
                    } else {
                        vm.directionMarker.push(direMarker)
                    }
                });
                connection.on("MapHubException", function (data) {
                    console.log(data);
                });

                this.conn = connection;
            },
            dbclickLocationTable(mac) {
                //给所有的marker的Title变成白色字体
                vm.deviceList.forEach(function (value) {
                    if (value.marker) {
                        var labelTitle = value.license + ' ' + (value.tecH_PARAMETERS_BRIEF == null ? "" : value.tecH_PARAMETERS_BRIEF);
                        value.marker.setText({
                            // 要展示的文字内容
                            content: labelTitle,
                            // 文字方向，有 icon 时为围绕文字的方向，没有 icon 时，则为相对 position 的位置
                            direction: 'top',
                            // 在 direction 基础上的偏移量
                            //offset: [-20, -5],
                            // 文字样式
                            style: {
                                // 字体大小
                                fontSize: 12,
                                // 字体颜色
                                fillColor: '#000000',
                                // 描边颜色
                                strokeColor: '#fff',
                                // 描边宽度
                                strokeWidth: 2,
                                //背景颜色
                                backgroundColor: '#CCCCCC',
                            }
                        });
                    }
                });
                var device = vm.getDevice(mac);
                if (device && device.lastTrackData) {
                    vm.followDevice = device;
                    //设置中心点
                    var destPoint = coordtransform.wgs84togcj02(device.lastTrackData.longitude, device.lastTrackData.latitude);
                    vm.map.setZoomAndCenter(20, destPoint);
                    device.marker.setTop(true);
                    var labelTitle = device.license + ' ' + (device.tecH_PARAMETERS_BRIEF == null ? "" : device.tecH_PARAMETERS_BRIEF);
                    //设置title变红FF0000
                    device.marker.setText({
                        // 要展示的文字内容
                        content: labelTitle,
                        // 文字方向，有 icon 时为围绕文字的方向，没有 icon 时，则为相对 position 的位置
                        direction: 'top',
                        // 在 direction 基础上的偏移量
                        //offset: [-20, -5],
                        // 文字样式
                        style: {
                            // 字体大小
                            fontSize: 12,
                            // 字体颜色
                            fillColor: '#FF0000',
                            // 描边颜色
                            strokeColor: '#fff',
                            // 描边宽度
                            strokeWidth: 2,
                            //背景颜色
                            backgroundColor: '#CCCCCC',
                        }
                    });
                    //device.marker.show();
                    device.marker.setTop(true);
                }
            },
            //更新实时定位数据
            updateGpsData(mac, gpsData) {
                var device = this.getDevice(mac);
                if (device) {
                    device.lastTrackData = gpsData;
                    //if (device.marker) {
                    //    device.marker.setMap(this.map)
                    //}
                    //更新地图上的位置和图标
                    this.realTimeDevice = device;
                    this.UpdatePage();
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
                            if (track.mac == mac) {
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
                //执勤力量
                axios.post('QueryAllUnit').then(function (response) {
                    vm.otherData.units = response.data;
                    vm.otherData.units.forEach((value) => {
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
                        unitMarker.hide();
                        vm.otherData.unitMarkers.push(unitMarker);
                        //监听marker点击
                        unitMarker.on('click', (e) => {
                            var tmpArr = [
                                "<tr><td style='width:100px;'>单位名称：</td><td>" + value.unitname + "</td></tr>",
                                "<tr><td>值班电话：</td><td>" + value.dutyphone + "</td></tr>",
                                "<tr><td>负责人：</td><td>" + value.principal + "</td></tr>",
                                "<tr><td>执勤人数：</td><td>" + value.ondutycount + "</td></tr>",
                                "<tr><td>执勤车辆：</td><td>" + value.ondutycar + "</td></tr>",
                            ];
                            var openedHtml = "<table style='text-align:left; line-height:22px; width:400px;'>" + tmpArr.join(' ') + "<table>";
                            // 创建 infoWindow 实例	
                            var infoWindow = new AMap.InfoWindow({
                                content: openedHtml,
                                anchor: 'bottom-left',
                                autoMove: false,
                                offset: new AMap.Pixel(10, -10)
                            });
                            // 打开信息窗体
                            infoWindow.open(vm.map, unitMarker.getPosition());
                        });
                    });
                }).catch(function (error) {
                    console.log(error);
                });
                //重点单位
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
                    this.otherData.unitMarkers.forEach((x) => {
                        x.show();
                    });
                } else {
                    this.otherData.unitMarkers.forEach((x) => {
                        x.hide();
                    });
                }
                this.otherData.isShowUnits = !this.otherData.isShowUnits;
            },
            locationQuery() {
                var nodes = this.zTree.treeObj.getCheckedNodes();
                if (nodes.length > 0 && this.conn) {
                    nodes.forEach((x) => {
                        var device = this.getDevice(x.id.replace('car-', ''));
                        if (device) {
                            this.conn.invoke("LocationQuery", device.mac, device.mtype, device.ctype).catch(function (err) {
                                return console.error(err.toString());
                            });
                        }
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
                                    //监听marker点击
                                    powerMarker.on('click', (e) => {
                                        var tmpArr = [
                                            "<tr><td style='width:100px;'>单位名称：</td><td>" + value.name + "</td></tr>",
                                            "<tr><td>单位地址：</td><td>" + value.address + "</td></tr>",
                                            "<tr><td>单位类型：</td><td>" + value.unit_type + "</td></tr>",
                                            "<tr><td>所属中队：</td><td>" + value.fire_brigade + "</td></tr>",
                                            "<tr><td>层数：</td><td>" + value.building_storey + "</td></tr>",
                                            "<tr><td>行驶路线：</td><td>" + value.driving_route + "</td></tr>",
                                            "<tr><td>详细信息：</td><td><a href='../../keyunit/" + value.name + ".htm' target='_blank'>" + value.name + "</a></td></tr>",
                                        ];
                                        var openedHtml = "<table style='text-align:left; line-height:22px; width:400px;'>" + tmpArr.join(' ') + "<table>";
                                        // 创建 infoWindow 实例	
                                        var infoWindow = new AMap.InfoWindow({
                                            content: openedHtml,
                                            anchor: 'bottom-left',
                                            autoMove: false,
                                            offset: new AMap.Pixel(10, -10)
                                        });
                                        // 打开信息窗体
                                        infoWindow.open(vm.map, powerMarker.getPosition());
                                    });
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
                                    //监听marker点击
                                    xhsMarker.on('click', (e) => {
                                        var tmpArr = [
                                            "<tr><td style='width:100px;'>名称：</td><td>" + value.symc + "</td></tr>",
                                            "<tr><td>地址：</td><td>" + value.sydz + "</td></tr>",
                                            "<tr><td>消火栓形式：</td><td>" + value.xhsxs + "</td></tr>",
                                            "<tr><td>管网形式：</td><td>" + value.gwxs + "</td></tr>",
                                            "<tr><td>压力（pa）：</td><td>" + value.yl + "</td></tr>",
                                            "<tr><td>管径（mm）：</td><td>" + value.gj + "</td></tr>",
                                            //"<tr><td>详细信息：</td><td><a href='../../" + value.name + ".htm' target='_blank'>详细信息</a></td></tr>",
                                        ];
                                        var openedHtml = "<table style='text-align:left; line-height:22px; width:300px;'>" + tmpArr.join(' ') + "<table>";
                                        // 创建 infoWindow 实例	
                                        var infoWindow = new AMap.InfoWindow({
                                            content: openedHtml,
                                            anchor: 'bottom-left',
                                            autoMove: false,
                                            offset: new AMap.Pixel(10, -10)
                                        });
                                        // 打开信息窗体
                                        infoWindow.open(vm.map, xhsMarker.getPosition());
                                    });
                                }
                            });
                        }
                        mouseTool.close();
                        vm.map.setDefaultCursor("default");
                    });
                });
            },
            setReturnInterval() {
                var nodes = this.zTree.treeObj.getCheckedNodes();
                if (nodes.length > 0 && this.conn) {
                    this.$prompt('请输入间隔时间（秒）', '设置回传间隔', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        inputPattern: /^\d+$/,
                        inputErrorMessage: '请输入数字',
                        inputValidator: function (value) {
                            if (value < 10) {
                                return "请输入不小于10的整数";
                            }
                        }
                    }).then(({ value }) => {
                        nodes.forEach((x) => {
                            var device = this.getDevice(x.id.replace('car-', ''));
                            if (device) {
                                this.conn.invoke("SetReturnInterval", device.mac, device.mtype, device.ctype, value).catch(function (err) {
                                    return console.error(err.toString());
                                });
                            }
                        });
                    }).catch(() => {

                    });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
            xfdbwen() {
                var nodes = this.zTree.treeObj.getCheckedNodes();
                if (nodes.length > 0 && this.conn) {
                    this.$prompt('请输入内容', '发送文字消息', {
                        confirmButtonText: '发送',
                        cancelButtonText: '取消',
                        //inputPattern: /^\d+$/,
                        //inputErrorMessage: '请输入数字'
                    }).then(({ value }) => {
                        nodes.forEach((x) => {
                            var device = this.getDevice(x.id.replace('car-', ''));
                            if (device) {
                                vm.msgDatas.push({ unitname: device.unit.unitname, mac: device.mac, license: device.license, sender: "平台", receiver: "终端", msg: value, time: new Date().Format("yyyy-MM-dd hh:mm:ss") });
                                this.conn.invoke("Xfdbwen", device.mac, device.mtype, device.ctype, value).catch(function (err) {
                                    return console.error(err.toString());
                                });
                            }
                        });
                    }).catch(() => {

                    });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
            xfdhd() {
                //实例化导航点地图
                this.dhd.map = new AMap.Map('dhdmap', {
                    center: [104.066642, 30.656279],//中心点坐标
                    zoom: 12
                });
                this.dhd.map.on('click', (e) => {
                    this.dhd.longitude = e.lnglat.getLng();
                    this.dhd.laitude = e.lnglat.getLat();
                    //var text = '您在 [ ' + e.lnglat.getLng() + ',' + e.lnglat.getLat() + ' ] 的位置单击了地图！';
                    //console.log(text);
                });
                //$('#myModal').modal({ backdrop: 'static' });
                var nodes = this.zTree.treeObj.getCheckedNodes();
                if (nodes.length > 0 && this.conn) {
                    $('#myModal').modal({ backdrop: 'static' });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
            tjdhd() {
                if (this.dhd.longitude == null || this.dhd.laitude == null || this.dhd.name == null) {
                    vm.$message.warning('请完整填写信息');
                    return;
                }
                var nodes = this.zTree.treeObj.getCheckedNodes();
                nodes.forEach((x) => {
                    var device = this.getDevice(x.id.replace('car-', ''));
                    if (device) {
                        this.conn.invoke("Xfdhd", device.mac, device.mtype, device.ctype, this.dhd.longitude, this.dhd.laitude, this.dhd.name, this.dhd.path).catch(function (err) {
                            return console.error(err.toString());
                        });
                    }
                });
                $('#myModal').modal('hide');
            },
            szszyl() {
                //实例化导航点地图
                this.szyl.map = new AMap.Map('szylmap', {
                    center: [104.066642, 30.656279],//中心点坐标
                    zoom: 12
                });

                this.szyl.map.on('click', function (e) {
                    if (vm.currentMarker)
                        vm.szyl.map.remove(vm.currentMarker);

                    var lngX = e.lnglat.getLng();
                    var latY = e.lnglat.getLat();
                    var markerOption = {
                        map: vm.szyl.map,
                        icon: "http://webapi.amap.com/images/0.png",
                        position: new AMap.LngLat(lngX, latY),
                        offset: [-10, -35]
                    };
                    vm.currentMarker = new AMap.Marker(markerOption);
                    vm.szyl.longitude = e.lnglat.getLng();
                    vm.szyl.laitude = e.lnglat.getLat();
                });

                var nodes = this.zTree.treeObj.getCheckedNodes();
                if (nodes.length > 0 && this.conn) {
                    $('#szylModal').modal({ backdrop: 'static' });
                } else {
                    vm.$message.warning('请选择车辆');
                }
            },
            tjszyl() {
                if (this.szyl.longitude == null || this.szyl.laitude == null || this.szyl.name == null) {
                    vm.$message.warning('请完整填写信息');
                    return;
                }
                var nodes = this.zTree.treeObj.getCheckedNodes();
                nodes.forEach((x) => {
                    if (x.id.indexOf("car") > -1) {
                        var device = this.getDevice(x.id.replace('car-', ''));
                        if (!device.marker) {
                            vm.$message.warning('车辆' + device.license + ':无法导航');
                            return;
                        }
                        if (device && device.marker) {
                            this.conn.invoke("Tjszyl", device.mac, device.marker.getPosition().lng, device.marker.getPosition().lat, this.szyl.longitude, this.szyl.laitude, this.szyl.name).catch(function (err) {
                                return console.error(err.toString());
                            });
                        }
                    }
                });
                $('#szylModal').modal('hide');
            },
            jsszyl() {
                //实战演练
                this.directionMarker.forEach((x) => {
                    x.setMap(null);
                });
                this.directionMarker = [];
                this.directionLine.forEach((x) => {
                    x.setData(null);
                });
                this.directionLine = [];
            },
            clearMarker() {
                this.otherData.powerMarkers.forEach((x) => {
                    vm.map.remove(x);
                });
                this.otherData.powerMarkers = [];


                this.otherData.xhsMarkers.forEach((x) => {
                    vm.map.remove(x);
                });
                this.otherData.xhsMarkers = [];

                if (this.otherData.rectangleObj != null) {
                    this.map.remove(this.otherData.rectangleObj);
                };

                this.otherData.unitMarkers.forEach((x) => {
                    x.hide();
                });
            },
        },
        mounted() {
            this.initMap();
            this.initDeviceTree();
            this.initData();
            setInterval(() => {
                this.realTimeDevice = undefined;
                this.UpdatePage();
            }, 180 * 1000);
            this.initSignalR();
            this.initOtherData();
        }
    });
    //----------------//
})();