//成本核算通用帮助  Ext.AysItem.RichHelp
Ext.define('Ext.Gc3.AysItem', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.AysItemHelp',
    helpid: 'pms3.ays_item_m',
    ORMMode: true,
    valueField: 'Code',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});