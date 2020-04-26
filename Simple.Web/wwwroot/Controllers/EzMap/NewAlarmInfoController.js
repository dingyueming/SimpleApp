(function () {
    'use strict';
    var vm = new Vue({
        el: '#vmBox',
        data: {
            timeValue: [new Date(new Date(new Date().toLocaleDateString()).getTime()), new Date(new Date(new Date().toLocaleDateString()).getTime() + 24 * 60 * 60 * 1000 - 1)],
            license: '',
            total: 0,
            pageIndex: 1,
            pageSize: 15,
            tableConfig: {
                multipleSort: false,
                tableData: [],
                columns: [
                    { width: 60, titleAlign: 'center', columnAlign: 'center', type: 'selection' },
                    {
                        field: 'custome', title: '序号', width: 50, titleAlign: 'center', columnAlign: 'center',
                        formatter: function (rowData, index, pagingIndex) {
                            var currentIndex = index + pagingIndex;
                            return '<span style="color:red;font-weight: bold;">' + (currentIndex + 1) + '</span>';
                        }, isFrozen: false
                    },
                    {
                        field: 'license', title: '车牌号', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true,
                        formatter: function (rowData, index, pagingIndex) {
                            return rowData.car.license;
                        }
                    },
                    { field: 'gnsstime', title: '时间', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'longitude', title: '经度', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'latitude', title: '纬度', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'headingshow', title: '方向', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'speed', title: '速度', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'statusshow', title: '状态', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'locateshow', title: '定位', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'locatemodeshow', title: '定位模式', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'alarmshow', title: '报警', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'record_time', title: '记录时间', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    {
                        field: 'area', title: '关联区域', width: 180, titleAlign: 'center', columnAlign: 'center', isResize: true,
                        formatter: function (rowData, index, pagingIndex) {
                            return rowData.area.areaname;
                        }
                    },
                ],
                titleRows: [],
            },
            row: {},
            selectedRows: [],
        },
        methods: {
            getTableData() {
                var search = { searchData: { dateTimes: this.timeValue, license: this.license }, pageIndex: this.pageIndex, pageSize: this.pageSize, where: '', orderBy: ' a.gnsstime desc' };
                axios.post('Query', Qs.stringify(search)).then(function (response) {
                    var pagination = response.data;
                    vm.tableConfig.tableData = pagination.data;
                    vm.total = pagination.total;
                    console.log(pagination);
                }).catch(function (error) {
                    console.log(error);
                });
            },
            pageChange(pageIndex) {
                this.pageIndex = pageIndex;
                this.getTableData();
            },
            pageSizeChange(pageSize) {

                this.pageIndex = 1;
                this.pageSize = pageSize;
                this.getTableData();
            },
            sortChange(params) {
                if (params.height.length > 0) {
                    this.tableConfig.tableData.sort(function (a, b) {

                        if (params.height === 'asc') {

                            return a.height - b.height;
                        } else if (params.height === 'desc') {

                            return b.height - a.height;
                        } else {

                            return 0;
                        }
                    });
                }
            },
            selectALL(selection) {
                this.selectedRows = selection;
            },
            selectChange(selection, rowData) {
                this.selectedRows = selection;
            },
            selectGroupChange(selection) {
            },
            select() {
                this.pageIndex = 1;
                this.getTableData();
            },
        },
        mounted: function () {
            this.getTableData();
        }
    });
})();