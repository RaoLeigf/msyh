//检查内容帮助（多选）    Ext.BsChkContent.MultiRichHelp
Ext.define('Ext.Gc3.BsChkContentMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.gcBsChkContentMultiHelp',
    helpid: 'pms3.bs_chk_content',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});