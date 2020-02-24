//项目类型过滤CBS帮助 CBSPcTypeRichHelp
Ext.define('Ext.Gc3.CBSPcTypeRichHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcWBSPcTypeHelp',
    helpid: 'pco3.bd_cbs',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'CbsName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});