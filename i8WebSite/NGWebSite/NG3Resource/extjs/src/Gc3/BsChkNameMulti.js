//检查项名称帮助（多选）   Ext.BsChkName.MultiRichHelp
Ext.define('Ext.Gc3.BsChkNameMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.gcBsChkNameMultiHelp',
    helpid: 'pms3.bs_chk_name',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});