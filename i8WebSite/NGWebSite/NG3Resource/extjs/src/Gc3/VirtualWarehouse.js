//虚拟仓库帮助gcVirtualWarehouseRichHelp  Ext.GcVirtualWarehouse.RichHelp
Ext.define('Ext.Gc3.VirtualWarehouse', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcVirtualWarehouseRichHelp',
    helpid: 'pmm3.virtualwarehouse',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Whname',
    userCodeField: "Warehouse",
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});