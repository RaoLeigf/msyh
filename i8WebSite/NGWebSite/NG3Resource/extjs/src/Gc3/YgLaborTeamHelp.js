//劳务队帮助 
Ext.define('Ext.Gc3.YgLaborTeamHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcYgLaborTeamHelp',//别名xtype引用
    ORMMode: true,
    helpid: 'pms3.pms3_yg_labor_team',//对应通用帮助
    valueField: 'PhId',//对应toolkit通用帮助
    displayField: 'Name',//对应toolkit通用帮助
    MaxLength: 100,
    editable: false,
    mustInput: false

});