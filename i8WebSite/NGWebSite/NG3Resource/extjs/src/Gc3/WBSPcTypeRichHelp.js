//项目类型过滤WBS帮助  WBSPcTypeRichHelp
Ext.define('Ext.Gc3.WBSPcTypeRichHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcWBSPcTypeHelp',
    helpid: 'pmm3.wbs_help',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'WbsName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});