//周材仓库帮助(有待修库位)gcWarehouseZcRichHelp    Ext.GcWarehouseZc.RichHelp
Ext.define('Ext.Gc3.WarehouseZc', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWarehouseZcRichHelp',
    helpid: 'pmm3.warehouse_zc',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Whname',
    userCodeField: "Whlocation",
    MaxLength: 100,
    editable: false,
    mustInput: false
});