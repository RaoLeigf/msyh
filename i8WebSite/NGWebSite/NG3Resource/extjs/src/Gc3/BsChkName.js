//检查项名称帮助   Ext.BsChkName.RichHelp
Ext.define('Ext.Gc3.BsChkName', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsChkNameHelp',
    helpid: 'pms3.bs_chk_name',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});