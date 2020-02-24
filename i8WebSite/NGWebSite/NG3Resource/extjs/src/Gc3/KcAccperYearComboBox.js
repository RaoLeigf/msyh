//核算期年帮助gcKcAccperYear  Ext.GcKcAccperYear.ComboBox
Ext.define('Ext.Gc3.KcAccperYearComboBox', {
    extend: 'Ext.ng.ComboBox',
    alias: 'widget.gcKcAccperYear',
    ORMMode: true,
    helpid: 'pmm3.kc_accper_year',
    valueField: 'Uyear',
    displayField: 'Uyear',
    listFields: 'Uyear,PhId',
    listHeadTexts: 'Uyear',
    MaxLength: 100,
    editable: false,
    mustInput: false
});