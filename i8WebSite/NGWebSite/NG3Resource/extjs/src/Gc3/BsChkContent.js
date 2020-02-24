//检查内容帮助    Ext.BsChkContent.RichHelp
Ext.define('Ext.Gc3.BsChkContent', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsChkContentHelp',
    helpid: 'pms3.bs_chk_content',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});