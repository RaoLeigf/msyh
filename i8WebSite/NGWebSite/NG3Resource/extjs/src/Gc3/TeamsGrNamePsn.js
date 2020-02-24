//班组帮助（包括负责人）   Ext.TeamsGrNamePsn.RichHelp
Ext.define('Ext.Gc3.TeamsGrNamePsn', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcTeamsGrNamePsnHelp',
    helpid: 'pms3.teams_gr_name_psn',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TeamsName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});