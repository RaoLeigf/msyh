//可以按仓库过滤项目，信息权限不参与过滤
Ext.define('Ext.Gc3.ProjectWarehouseKc', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcProjectWarehouseKcRichHelp',
    width: 100,
    height: 25,
    ORMMode: true,
    helpid: 'pmm3.project_warehouse',
    listHeadTexts: "编码,名称",
    listFields: "PcNo,ProjectName,Pc,PhId",
    outFilter: { 'app_status': '1' },
    //ignoreOutFilter: true,//代码转名称时忽略外部条件
    valueField: 'PhId',
    displayField: 'ProjectName',
    MaxLength: 100,
    mustInput: false,
    initComponent: function () {
        var me = this;

        me.callParent();

        //me.on('helpselected', function(obj) {
        //    if (obj && obj.data && !Ext.isEmpty(obj.data.PcNo)) {
        //        obj.name = obj.name + '(' + obj.data.PcNo + ')';
        //        me.setRawValue(obj.name);
        //    }
        //});
    },
    setWarehouse: function (newValue) {
        ////设置按仓库过滤
        var me = this;

        if (!Ext.isEmpty(newValue)) {
            //me.setOutFilter({ warehouseid: newValue });

            me.setClientSqlFilter("project_table.phid in (select pc_id from pc_warehouse where warehouseid='" +
                newValue + "')");
        } else {
            //me.setOutFilter({});
            me.setClientSqlFilter();
        }
        me.lastQuery = null;
    }
});