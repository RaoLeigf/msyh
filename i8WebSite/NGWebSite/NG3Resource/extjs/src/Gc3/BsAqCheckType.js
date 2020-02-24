//检查类型帮助    Ext.BsAqCheckType.RichHelp
Ext.define('Ext.Gc3.BsAqCheckType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsAqCheckTypeHelp',
    helpid: 'pms3.bs_aq_check_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});