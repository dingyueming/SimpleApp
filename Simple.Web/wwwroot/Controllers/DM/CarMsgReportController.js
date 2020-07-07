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
                vm.row = JSON.parse(JSON.stringify(this.rowData));
                vm.row.carid = "car-" + vm.row.carid;
                $('#myModal').modal({ backdrop: 'static', keyboard: false });
                // 参数根据业务场景随意构造
                let params = { type: 'edit', index: this.index, rowData: this.rowData };
                this.$emit('on-custom-comp', params);
            },
            deleteRow() {
                vm.selectedRows.push(this.rowData);
                vm.deleteSelect();
                // 参数根据业务场景随意构造
                let params = { type: 'delete', index: this.index };
                this.$emit('on-custom-comp', params);
            },
        }
    });
    var vm = new Vue({
        el: '#vmBox',
        components: { 'treeselect': VueTreeselect.Treeselect },
        data: {
            where: {
                carid: null
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
                    {
                        field: 'car', title: '报备车辆', width: 150, titleAlign: 'center', columnAlign: 'center', isResize: true, formatter: function (rowData, index, pagingIndex) {
                            if (rowData.car != null) {
                                return rowData.car.license + ' ' + rowData.car.carno;
                            }
                            return "";
                        }
                    },
                    { field: 'approver', title: '审批人', width: 150, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'sendtime', title: '出动时间', width: 150, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'backtime', title: '返回时间', width: 150, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'content', title: '任务内容', width: 200, titleAlign: 'center', columnAlign: 'center', isResize: true },
                    { field: 'custome-adv', title: '操作', width: 100, titleAlign: 'center', columnAlign: 'center', componentName: 'table-operation', isResize: true },
                ],
                titleRows: [],
            },
            row: {},
            selectedRows: [],
        },
        methods: {
            getTableData() {
                var search = { pageIndex: this.pageIndex, pageSize: this.pageSize, where: '', orderBy: '' };
                if (this.where.carid != null) {
                    search.where += ' and a.carid =' + this.where.carid.replace("car-", "");
                }
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
                this.row = { id: 0, carid: null };
                $('#myModal').modal({ backdrop: 'static' });
            },
            select() {
                this.pageIndex = 1;
                this.getTableData();
            },
            deleteSelect() {
                if (Array.isArray(this.selectedRows) && this.selectedRows.length > 0) {
                    if (confirm('确认删除吗')) {
                        axios.post('BatchDelete', Qs.stringify({ exEntities: this.selectedRows })).then(function (response) {
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
            initDeviceTree() {
                axios.post('QueryDeviceTree')
                    .then(function (response) {
                        vm.options = response.data;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            saveData() {
                debugger;
                //数据校验
                if (this.row.carid == null) {
                    this.$message.error('请选择报备车辆');
                    return;
                }
                if ($.trim(this.row.approver) == '') {
                    this.$message.error('请填写审批人');
                    return;
                }
                if ($.trim(this.row.sendtime) == null) {
                    this.$message.error('请填写出动时间');
                    return;
                }
                var url;
                if (this.row.id == 0) {
                    url = 'add';
                } else {
                    url = 'update';
                }
                this.row.carid = this.row.carid.replace("car-", "");
                axios.post(url, Qs.stringify({ exEntity: this.row })).then(function (response) {
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
        },
        mounted: function () {
            this.getTableData();
            this.initDeviceTree();
        }
    });
})();