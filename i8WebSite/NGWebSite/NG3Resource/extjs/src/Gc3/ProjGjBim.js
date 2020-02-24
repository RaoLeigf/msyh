//构件帮助  Ext.ProjGjBim.RichHelp
Ext.define('Ext.Gc3.ProjGjBim', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcProjGjBimHelp',
    helpid: 'pms3.proj_gj_bim',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'CompName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});