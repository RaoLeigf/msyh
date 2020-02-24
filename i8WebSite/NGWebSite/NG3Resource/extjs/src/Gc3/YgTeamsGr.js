//用工班组帮助   Ext.YgTeamsGr.RichHelp
Ext.define('Ext.Gc3.YgTeamsGr', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcYgTeamsGrHelp',
    helpid: 'pms3.yg.teams_gr',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TeamsName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});