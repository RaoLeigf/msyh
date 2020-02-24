//仓库帮助gcWarehouseHelp   Ext.Warehouse.Help
Ext.define('Ext.Gc3.WarehouseHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWarehouseHelp',
    helpid: 'pmm3.warehouse',
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