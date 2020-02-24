//记账类型帮助gcFinTypeHelp   Ext.FinType.RichHelp
Ext.define('Ext.Gc3.FinType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcFinTypeHelp',
    helpid: 'pmm3.cwfintype',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TypeName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});