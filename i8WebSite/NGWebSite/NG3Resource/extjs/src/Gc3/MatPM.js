//采购计划弹出帮助字段    Ext.MatPM.RichHelp
Ext.define('Ext.Gc3.MatPM', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.MatPMHelpField',
    helpid: 'pmm3.mat_p_m',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'UserCode',
    MaxLength: 100,
    editable: false,
    mustInput: false
});