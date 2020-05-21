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
            //实时数据
            realTimeData: {},
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
                                deviceList.forEach(function (value) {
                                    if (value.marker) {
                                        value.marker.title_.defaultStyle_.fontColor = "#FFFFFF";
                                    }
                                });
                                var device = getCarByCarID(parseInt(treeNode.id.replace('car-', '')));
                                if (device && device.lastTrackData) {
                                    //设置中心点
                                    var centerCoord = new EzCoord(device.lastTrackData.longitude, device.lastTrackData.latitude);
                                    map.centerAndZoom(centerCoord, 14);
                                    device.marker.title_.defaultStyle_.fontColor = "#FF0000";
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
                        var device = vm.getCarByCarID(value.carid);
                        if (device) {
                            var minute = Number((new Date().getTime() - new Date(value.gnsstime).getTime()) / (1000 * 60));
                            device.lastTrackData = value;
                            var destPoint = coordtransform.wgs84togcj02(value.longitude, value.latitude);
                            destPoint = new AMap.LngLat(destPoint[0], destPoint[1]);
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
                                label: { content: device.carno, direction: value.heading, offset: new AMap.Pixel(5, 5) },
                                //extData: car.CARID,
                            });
                            device.marker = baseMarker;
                            if (iconUrl.indexOf('stop.png') > -1 || iconUrl.indexOf('run.png') > -1) {
                                device.marker.setzIndex(1000); //前置
                            }
                        }
                    });
                })
            },
            //启动定时器，更新车辆状态
            initTimer() {
                setInterval(function () {
                    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
                    for (var i = 0; i < vm.deviceList.length; i++) {
                        var device = vm.deviceList[i];
                        if (device.marker) {
                            var iconUrl = "../../plugins/amap/images/" + getCarStateIcon(device.lastTrackData);
                            if (device.marker.getIcon() != iconUrl)
                                device.marker.setIcon(iconUrl);
                        }
                        if (treeObj && treeObj != null) {
                            var node = treeObj.getNodeByParam("id", "car-" + device.carid, null);
                            if (node && node != null) {
                                node.iconSkin = getCarTreeStateSkin(device.lastTrackData);
                                treeObj.updateNode(node);
                            }
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
            //根据Mac获取设备
            getDeviceByMac(mac) {
                if (Array.isArray(this.deviceList)) {
                    for (var i = 0; i < this.deviceList.length; i++) {
                        if (this.deviceList[i].mac === mac) {
                            return this.deviceList[i];
                        }
                    }
                }
            },
            //根据carid获取车辆
            getCarByCarID(carid) {
                if (Array.isArray(this.deviceList)) {
                    for (var i = 0; i < this.deviceList.length; i++) {
                        if (this.deviceList[i].carid === carid) {
                            return this.deviceList[i];
                        }
                    }
                }
            }
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