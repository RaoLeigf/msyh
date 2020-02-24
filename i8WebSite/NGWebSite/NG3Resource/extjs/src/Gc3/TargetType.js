//管理层级帮助gcTargetTypeRichHelp    Ext.GcTargetType.RichHelp
Ext.define('Ext.Gc3.TargetType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcTargetTypeRichHelp',
    helpid: 'pms3.bs_target_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});