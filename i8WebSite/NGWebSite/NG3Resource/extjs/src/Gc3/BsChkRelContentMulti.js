//检查内容子表帮助（多选）  Ext.BsChkRelContent.MultiRichHelp
Ext.define('Ext.Gc3.BsChkRelContentMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.gcBsChkRelContentMultiHelp',
    helpid: 'pms3.bs_chk_rel_content',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});