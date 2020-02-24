//班组人员设置多选通用帮助  Ext.TeamsEmpData.MultiRichHelp
Ext.define('Ext.Gc3.TeamsEmpDataMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.TeamsEmpDataHelp',
    helpid: 'pmm3.teamsemp',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'EmpName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});