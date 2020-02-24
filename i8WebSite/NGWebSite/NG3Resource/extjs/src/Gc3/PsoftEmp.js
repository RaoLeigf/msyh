//劳务工帮助 Ext.PsoftEmp.RichHelp
Ext.define('Ext.Gc3.PsoftEmp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcPsoftEmpHelp',
    helpid: 'pms3.psoft_emp',
    ORMMode: true,
    valueField: 'EmpId',
    displayField: 'SName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});