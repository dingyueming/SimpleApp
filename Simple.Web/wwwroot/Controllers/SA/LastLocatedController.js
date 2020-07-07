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
                    //{ width: 60, titleAlign: 'center', columnAlign: 'center', type: 'selection' },
                    {
                        field: 'custome', title: '序号', width: 50, titleAlign: 'center', columnAlign: 'center',
                        formatter: function (rowData, index, pagingIndex) {
                            var currentIndex = index + pagingIndex;
                            return '<span style="color:red;font-weight: bold;">' + (currentIndex + 1) + '</span>';
                        }, isFrozen: false
                    },
                    {
                        field: 'car.carno', title: '车牌号', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true,
                        formatter: function (rowData, index, pagingIndex) {
                            return rowData.car.license + ' ' + rowData.car.carno;
                        }
                    },
                    {
                        field: 'car.carno', title: '单位', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true,
                        formatter: function (rowData, index, pagingIndex) {
                            return rowData.unit.unitname;
                        }
                    },
                    { field: 'gnsstime', title: '最后上线时间', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true },
                ],
                titleRows: [],
            },
            row: {},
            selectedRows: [],
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近一个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick(picker) {
                        const end = new Date();
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
                        picker.$emit('pick', [start, end]);
                    }
                }]
            },
            isSearchLocated: true
        },
        methods: {
            getTableData() {
                var search = { searchData: { dateTimes: this.timeValue, license: this.license, isSearchLocated: this.isSearchLocated }, pageIndex: this.pageIndex, pageSize: this.pageSize, where: '', orderBy: ' a.gnsstime desc' };
                axios.post('Query', Qs.stringify(search)).then(function (response) {
                    var pagination = response.data;
                    vm.tableConfig.tableData = pagination.data;
                    vm.total = pagination.total;
                    //console.log(pagination);
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
                this.getTableData();
            },
        },
        mounted: function () {
            this.getTableData();
        }
    });
})();