//种类gcMecTypeComboBox   Ext.GcMecType.ComboBox
Ext.define('Ext.Gc3.MecTypeComboBox', {
    extend: 'Ext.ng.ComboBox',
    alias: 'widget.gcMecTypeComboBox',
    helpid: 'pmm3.mec_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});