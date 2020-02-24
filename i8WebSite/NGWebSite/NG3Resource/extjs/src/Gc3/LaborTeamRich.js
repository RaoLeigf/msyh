//班组类型帮助   Ext.Gc3.LaborTeamRich
Ext.define('Ext.Gc3.LaborTeamRich', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.LaborTeamRichHelp',
    helpid: 'pms3.pms3_yg_labor_team',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});