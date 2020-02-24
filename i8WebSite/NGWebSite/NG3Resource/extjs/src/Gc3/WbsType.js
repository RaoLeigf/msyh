//项目类型帮助gcWbsTypeRichHelp   Ext.GcWbsType.RichHelp
Ext.define('Ext.Gc3.WbsType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWbsTypeRichHelp',
    helpid: 'pms3.wbs_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TypeName',
    listFields: 'TypeName,PhId',
    listHeadTexts: 'TypeName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});