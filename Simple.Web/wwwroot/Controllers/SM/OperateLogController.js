(function () {
    'use strict';
    var vm = new Vue({
        el: '#vmBox',
        data: {
            where: {
                appName: ""
            },
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
                    { field: 'loginname', title: '登录名', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'realname', title: '实际名', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'ip', title: '登录ip', width: 100, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'operatetypeShow', title: '操作', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'operatetime', title: '时间', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'modelname', title: '对象', width: 100, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'remark', title: '备注', width: 240, titleAlign: 'center', columnAlign: 'center', isResize: true },
                ],
                titleRows: [],
            },
            row: {},
            selectedRows: [],
        },
        methods: {
            getTableData() {
                var search = { pageIndex: this.pageIndex, pageSize: this.pageSize, where: ' and a.loginname like \'%' + this.where.appName + '%\'', orderBy: 'a.operatetime desc,a.loginname,a.logid desc' };
                axios.post('Query', Qs.stringify(search)).then(function (response) {
                    var pagination = response.data;
                    vm.tableConfig.tableData = pagination.data;
                    vm.total = pagination.total;
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