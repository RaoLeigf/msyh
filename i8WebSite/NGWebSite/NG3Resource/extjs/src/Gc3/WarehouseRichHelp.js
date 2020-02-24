//仓库帮助(有过滤条件)gcWarehouseRichHelp   Ext.Warehouse.RichHelp
Ext.define('Ext.Gc3.WarehouseRichHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWarehouseRichHelp',
    helpid: 'pmm3.warehouse_help',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Whname',
    userCodeField: "Warehouse",
    listFields: 'Warehouse,Whname,Isautowhlo,DefInt2,PhId',
    listHeadTexts: '编码,名称',
    MaxLength: 100,
    editable: true,
    mustInput: false,
    setPc: function (newValue) {
        ////设置按项目过滤
        var me = this;

        if (!Ext.isEmpty(newValue)) {
            me.setClientSqlFilter('phid in (select warehouseid from pc_warehouse where pc_id=\'' + newValue + '\')');
        } else {
            me.setClientSqlFilter('');
        }

        me.lastQuery = null;
    }
});