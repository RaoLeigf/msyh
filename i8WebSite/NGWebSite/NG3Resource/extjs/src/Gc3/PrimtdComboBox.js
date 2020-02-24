//计价方式帮助gcPrimtdComboBox    Ext.GcPrimtd.ComboBox
Ext.define('Ext.Gc3.PrimtdComboBox', {
    extend: 'Ext.ng.ComboBox',
    alias: 'widget.gcPrimtdComboBox',
    helpid: 'pms3.primtd',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Primtdname',
    listFields: 'Primtdname,PhId',
    listHeadTexts: 'Primtdname',
    MaxLength: 100,
    editable: false,
    mustInput: false
});