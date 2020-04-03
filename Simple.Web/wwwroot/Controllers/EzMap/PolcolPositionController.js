(function () {
    'use strict';
    var vmLbsAlarm = new Vue({
        el: '#vmLbsAlarm',
        data: {
            map: {},//地图对象
            search: {//查询
                timeValue: [new Date(new Date(new Date().toLocaleDateString()).getTime()), new Date(new Date(new Date().toLocaleDateString()).getTime() + 24 * 60 * 60 * 1000 - 1)],
                points: null,
                nameOrPoliceCode: "",
                feature: null,
            },
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
            //画框
            drawAction() {
                this.map.changeDragMode('drawRect', function (/** feature为Ez.g.*要素类 */feature) {
                    /** 一般鼠标右键结束绘制,回调参数为动态绘制的要素,可以在回调中进行余下操作,例如，增加绘制要素到地图上. */
                    vmLbsAlarm.map.addOverlay(feature);
                    vmLbsAlarm.search.points = feature.getPoints()[0];
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
                const loading = this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                });
                axios.post('QueryPositionData', Qs.stringify({ dateTimes: this.search.timeValue, points: this.search.points, nameOrPoliceCode: this.search.nameOrPoliceCode })).then((response) => {
                    if (response.status == 200) {
                        var listAlarm = response.data;
                        if (listAlarm && Array.isArray(listAlarm)) {
                            listAlarm.forEach((alarm) => {
                                var icon = new EzIcon({
                                    src: '../../plugins/ezMap/images/policeman.png',
                                    anchor: [0.5, 1],
                                    anchorXUnits: 'fraction',
                                    anchorYUnits: 'fraction',
                                    opacity: 1
                                });
                                //title
                                var title = new EzTitle(alarm.name + "(" + alarm.code + ")", {
                                    "fontSize": 12,
                                    "fontColor": "#FFFFFF",
                                    "fillColor": "#00CDFF",
                                    "isStroke": true,
                                    "strokeColor": "#FFFFFF",
                                    "strokeWidth": 1,
                                    "lineHeight": 2.2,
                                    "paddingH": 15,
                                    "offset": [0, -20]
                                });
                                var position = new EzCoord(alarm.jd, alarm.wd);
                                var marker = new EzMarker(position, icon, title);
                                //构造打开的html
                                var tmparr = [
                                    "<div style='margin-top:0px;'>姓名：" + alarm.name + "</div>",
                                    "<div style='margin-top:15px;'>警号：" + alarm.code + "</div>",
                                    "<div style='margin-top:15px;'>考勤时间：" + alarm.check_Time + "</div>",
                                    "<div style='margin-top:15px;'>部门名称：" + alarm.deptname + "</div>",
                                    "<div style='margin-top:15px;'>经度：" + alarm.jd + "</div>",
                                    "<div style='margin-top:15px;'>纬度：" + alarm.wd + "</div>"
                                ];
                                marker.openedHtml = "<div style='text-align:left;'>" + tmparr.join(' ') + "</div>";
                                this.map.addOverlay(marker);
                                marker.showTitle();
                            });
                        }
                        this.search.points = null;
                        loading.close();
                    }
                }).catch((error) => {
                    loading.close();
                    console.log(error);
                });
            }
        },
        mounted() {
            this.initMap();
        }
    });
})();
