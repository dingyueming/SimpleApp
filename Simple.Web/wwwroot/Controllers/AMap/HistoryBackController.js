(function () {
    'use strict';
    //----------------//
    var vm = new Vue({
        el: '#vmContent',
        components: { 'treeselect': VueTreeselect.Treeselect },
        data: {
            //地图
            map: {},
            //设备列表
            deviceList: [],
            options: [],
            //回放车辆marker
            marker: {},
            //搜索条件
            search: {
                minspeed: 1,
                timeValue: [new Date(new Date(new Date().toLocaleDateString()).getTime()), new Date(new Date(new Date().toLocaleDateString()).getTime() + 24 * 60 * 60 * 1000 - 1)],
                deviceid: null,
            },
            playSpeed: 1,
            intervalIndex: 0,
            dataIndex: 0,
            newtracks: [],
            newtrackstb: [],
        },
        methods: {
            //实例化地图
            initMap() {
                //实例化地图
                this.map = new AMap.Map('container', {
                    center: [104.251709, 30.570383],//中心点坐标
                    zoom: 12
                });
                this.map.setFitView();
            },
            //实例化地图设备数据
            initData() {
                axios.post('../RealTimeMap/QueryDeviceList').then((response) => {
                    vm.deviceList = response.data;
                }).catch((error) => {
                    console.log(error);
                });
            },
            initDeviceTree() {
                axios.post('QueryDeviceTree')
                    .then(function (response) {
                        vm.options = response.data;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
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
            //实例化回放数据
            initNewtracks() {
                vm.newtracks = [];
                axios.post('QueryHistoryBackData', Qs.stringify(this.search))
                    .then(function (response) {
                        if (response.data && (response.data == null || response.data.length == 0)) {
                            vm.$message.warning('无可回放数据');
                            return;
                        }
                        vm.newtracks = response.data;
                        vm.newtracks.forEach((x) => {
                            //坐标转化
                            x.destPoint = coordtransform.wgs84togcj02(x.longitude, x.latitude);
                        });
                        var lineArr = [];
                        vm.newtracks.forEach((x) => {
                            lineArr.push(x.destPoint);
                        });
                        // 绘制轨迹
                        var polyline = new AMap.Polyline({
                            map: vm.map,
                            path: lineArr,
                            showDir: true,
                            strokeColor: "#28F",  //线颜色
                            // strokeOpacity: 1,     //线透明度
                            strokeWeight: 6,      //线宽
                            // strokeStyle: "solid"  //线样式
                        });
                        //设置起终点
                        var startMarker = new AMap.Marker({
                            map: vm.map,
                            position: lineArr[0],
                            offset: new AMap.Pixel(-18, -18),
                            icon: "../../plugins/amap/images/start.png",
                        });
                        var endMarker = new AMap.Marker({
                            map: vm.map,
                            position: lineArr[lineArr.length - 1],
                            offset: new AMap.Pixel(-18, -18),
                            icon: "../../plugins/amap/images/end.png",
                        });
                        //车辆图片
                        var imgUrl = "../../plugins/amap/images/" + (vm.newtracks[0].speed > 0 ? "run.png" : "stop.png");
                        vm.marker = new AMap.Marker({
                            map: vm.map,
                            position: lineArr[0],
                            icon: imgUrl,
                            offset: new AMap.Pixel(-4, -8),
                            autoRotation: true,
                            //angle: vm.newtracks[0].heading,
                        });
                        //设置中心点
                        vm.map.setZoomAndCenter(14, vm.newtracks[0].destPoint);
                        //打点播放
                        vm.speedChange();
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            play() {
                if (vm.dataIndex == vm.newtracks.length - 1) {
                    clearInterval(vm.intervalIndex);
                }
                var lastTrackData = vm.newtracks[vm.dataIndex];
                lastTrackData.mac = lastTrackData.device.mac;
                var dev = vm.getDevice(lastTrackData.carid);
                if (dev) {
                    var netracktbRow = {};
                    netracktbRow.license = dev.license + '(' + dev.license.carno + ')';
                    netracktbRow.mac = dev.mac;
                    netracktbRow.sim = dev.sim;
                    netracktbRow.gnsstime = lastTrackData.gnsstime;
                    netracktbRow.receive_time = lastTrackData.recorD_TIME;
                    netracktbRow.speed = lastTrackData.speed;
                    netracktbRow.locate_str = lastTrackData.locatE_STR;
                    netracktbRow.heading_str = lastTrackData.headinG_STR;
                    netracktbRow.status = lastTrackData.statusShow;
                    netracktbRow.locatemode_str = lastTrackData.locatemodE_STR;
                    netracktbRow.mileage = lastTrackData.kilometre;
                    netracktbRow.position = lastTrackData.position;
                    netracktbRow.longitude = lastTrackData.longitude;
                    netracktbRow.latitude = lastTrackData.latitude;
                    vm.newtrackstb.unshift(netracktbRow);
                }
                if (vm.dataIndex < vm.newtracks.length - 1) {
                    vm.dataIndex++;
                } else {
                    clearInterval(vm.intervalIndex);
                }
                //设置位置、方向以及背景图
                vm.marker.setPosition(lastTrackData.destPoint);
                //vm.marker.setAngle(lastTrackData.heading);
                var imgUrl = "../../plugins/amap/images/" + (lastTrackData.speed > 0 ? "run.png" : "stop.png");
                if (vm.marker.getIcon() != imgUrl) {
                    vm.marker.setIcon(imgUrl);
                }
            },
            //播放速度变化
            speedChange() {
                clearInterval(vm.intervalIndex);
                //打点播放
                vm.intervalIndex = setInterval(this.play, 1000 - ((this.playSpeed - 1) * 250));
            },
            //开始
            startAnimation() {
                if (!this.valadata()) {
                    return;
                }
                this.map.clearMap();
                this.newtrackstb = [];
                this.playSpeed = 1;
                this.dataIndex = 0;
                clearInterval(this.intervalIndex);
                this.initNewtracks();
            },
            //暂停
            pauseAnimation() {
                clearInterval(vm.intervalIndex);
            },
            //继续
            resumeAnimation() {
                this.speedChange();
            },
            //停止
            stopAnimation() {
                this.map.clearMap();
                this.playSpeed = 1;
                this.dataIndex = 0;
                this.newtrackstb = [];
                clearInterval(this.intervalIndex);
            },
            valadata() {
                if (this.search.deviceid == null) {
                    vm.$message.warning('请选择设备');
                    return false;
                }
                return true;
            },
            exportExcel() {
                const loading = this.$loading({
                    lock: true,
                    text: '正在导出数据',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                });
                axios({
                    method: 'post',
                    data: Qs.stringify(this.search),
                    url: 'ExportExcel',
                    responseType: 'blob'
                }).then(function (res) {
                    const blob = new Blob([res.data])
                    const fileName = '轨迹回放数据.xlsx'
                    if ('download' in document.createElement('a')) { // 非IE下载
                        const elink = document.createElement('a')
                        elink.download = fileName
                        elink.style.display = 'none'
                        elink.href = URL.createObjectURL(blob)
                        document.body.appendChild(elink)
                        elink.click()
                        URL.revokeObjectURL(elink.href) // 释放URL 对象
                        document.body.removeChild(elink)
                    } else { // IE10+下载
                        navigator.msSaveBlob(blob, fileName)
                    }
                    loading.close();
                }).catch(function (error) {
                    console.log(error);
                    loading.close();
                });
            },
        },
        mounted() {
            this.initData();
            this.initMap();
            this.initDeviceTree();
        }
    });
    //----------------//
})();