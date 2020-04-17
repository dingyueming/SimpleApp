(function () {
    'use strict';
    // 自定义列组件
    Vue.component('table-operation', {
        template: `<span>
        <a href="" v-on:click.stop.prevent="update(rowData,index)">编辑</a>&nbsp;
        <a href="" v-on:click.stop.prevent="deleteRow(rowData,index)">删除</a>
        </span>`,
        props: {
            rowData: {
                type: Object
            },
            field: {
                type: String
            },
            index: {
                type: Number
            }
        },
        methods: {
            update() {
                vm.row = this.rowData;
                $('#myModal').modal({ backdrop: 'static', keyboard: false });
                // 参数根据业务场景随意构造
                let params = { type: 'edit', index: this.index, rowData: this.rowData };
                this.$emit('on-custom-comp', params);
            },
            deleteRow() {
                vm.row = this.rowData;
                vm.pageMode = "delete";
                vm.operateData(vm.row);
                // 参数根据业务场景随意构造
                let params = { type: 'delete', index: this.index };
                this.$emit('on-custom-comp', params);
            },
        }
    });
    var vm = new Vue({
        el: '#vmBox',
        data: {
            where: {
                appName: ""
            },
            options: [],
            pageMode: 'add',
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
                    { field: 'app_Name', title: '应用名称', width: 100, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'password', title: '密码', width: 100, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'ip', title: 'IP', width: 120, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'port', title: '端口', width: 40, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'int_Type', title: '接口类型', width: 40, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'ter_Type', title: '终端类型', width: 40, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'app_Id', title: '状态', width: 40, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'status', title: '运行状态', width: 40, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'run_Status', title: '接口标准', width: 80, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    //{
                    //    field: 'user.usersName', title: '创建人', width: 150, titleAlign: 'center', columnAlign: 'center', isResize: true,
                    //    formatter: function (rowData, index, pagingIndex) { return rowData.user.usersName; }
                    //},
                    //{ field: 'remark', title: '备注', width: 150, titleAlign: 'center', columnAlign: 'left', isResize: true },
                    { field: 'custome-adv', title: '操作', width: 100, titleAlign: 'center', columnAlign: 'center', componentName: 'table-operation', isResize: true },
                ],
                titleRows: [],
            },
            row: {},
            selectedRows: [],
        },
        methods: {
            getTableData() {
                var search = { pageIndex: this.pageIndex, pageSize: this.pageSize, where: ' and a.app_name like \'%' + this.where.appName + '%\'', orderBy: '' };
                axios.post('Query', Qs.stringify(search)).then(function (response) {
                    var pagination = response.data;
                    console.log(pagination);
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
            add() {
                this.row = {};
                $('#myModal').modal({ backdrop: 'static' });
            },
            select() {
                this.pageIndex = 1;
                this.getTableData();
            },
            deleteSelect() {
                if (Array.isArray(this.selectedRows) && this.selectedRows.length > 0) {
                    if (confirm('确认删除吗')) {
                        axios.post('@Url.Action("BatchDelete")', Qs.stringify({ exEntities: this.selectedRows })).then(function (response) {
                            var commonResult = response.data;
                            if (commonResult.isSuccess) {
                                $('#myModal').modal('hide');
                                vm.getTableData();
                            } else {
                                vm.$message.error(commonResult.message);
                            }
                        }).catch(function (error) {
                            console.log(error);
                        });
                    }
                }
            },
            saveData() {
                axios.post('SaveData', Qs.stringify({ exEntity: this.row })).then(function (response) {
                    var commonResult = response.data;
                    if (commonResult.isSuccess) {
                        $('#myModal').modal('hide');
                        vm.getTableData();
                    } else {
                        vm.$message.error(commonResult.message);
                    }
                }).catch(function (error) {
                    console.log(error);
                });
            },
        },
        mounted: function () {
            this.getTableData();
        }
    });
})();