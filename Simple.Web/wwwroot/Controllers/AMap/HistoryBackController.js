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
            value: [],
            options: [{
                id: 'fruits',
                label: 'Fruits',
                children: [{
                    id: 'apple',
                    label: 'Apple 🍎',
                    isNew: true,
                }, {
                    id: 'grapes',
                    label: 'Grapes 🍇',
                }, {
                    id: 'pear',
                    label: 'Pear 🍐',
                }, {
                    id: 'strawberry',
                    label: 'Strawberry 🍓',
                }, {
                    id: 'watermelon',
                    label: 'Watermelon 🍉',
                }],
            }, {
                id: 'vegetables',
                label: 'Vegetables',
                children: [{
                    id: 'corn',
                    label: 'Corn 🌽',
                }, {
                    id: 'carrot',
                    label: 'Carrot 🥕',
                }, {
                    id: 'eggplant',
                    label: 'Eggplant 🍆',
                }, {
                    id: 'tomato',
                    label: 'Tomato 🍅',
                }],
            }],
            //搜索条件
            search: {
                minspeed: 0,
                zerospeed: true,
                timeValue: [new Date(new Date(new Date().toLocaleDateString()).getTime()), new Date(new Date(new Date().toLocaleDateString()).getTime() + 24 * 60 * 60 * 1000 - 1)],
                deviceid: null,
                playSpeed: 1,
            },
            operateType: "pause",
            //options: [],
            marker: undefined,
            intervalIndex: 0,
            dataIndex: 0,
            newtracks: [],
        },
        methods: {
            //实例化地图
            initMap() {
                //实例化地图
                this.map = new AMap.Map('container', {
                    center: [104.251709, 30.570383],//中心点坐标
                    zoom: 12
                });
            },
            //实例化地图设备数据
            initData() {
                axios.post('../RealTimeMap/QueryDeviceList').then((response) => {
                }).catch((error) => {
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

        },
        mounted() {
            this.initData();
            this.initMap();
            this.initDeviceTree();
            this.initTimer();
        }
    });
    //----------------//
})();