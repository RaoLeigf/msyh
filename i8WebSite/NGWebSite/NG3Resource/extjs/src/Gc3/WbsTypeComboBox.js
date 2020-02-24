//项目类型帮助gcWbsTypeComboBox   Ext.GcWbsType.ComboBox
Ext.define('Ext.Gc3.WbsTypeComboBox', {
    extend: 'Ext.ng.ComboBox',
    alias: 'widget.gcWbsTypeComboBox',
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