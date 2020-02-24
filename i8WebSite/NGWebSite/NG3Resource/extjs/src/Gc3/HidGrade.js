//隐患等级帮助gcHidGradeRichHelp  Ext.GcHidGrade.RichHelp
Ext.define('Ext.Gc3.HidGrade', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcHidGradeRichHelp',
    helpid: 'pms3.bs_hid_grade',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});