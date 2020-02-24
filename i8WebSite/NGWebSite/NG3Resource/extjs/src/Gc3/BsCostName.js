//费用名称帮助   Ext.BsCostName.RichHelp
Ext.define('Ext.Gc3.BsCostName', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsCostNameHelp',
    helpid: 'pms3.bs_costname',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});