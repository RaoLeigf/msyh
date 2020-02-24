//检查内容子表帮助  Ext.BsChkRelContent.RichHelp
Ext.define('Ext.Gc3.BsChkRelContent', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsChkRelContentHelp',
    helpid: 'pms3.bs_chk_rel_content',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});