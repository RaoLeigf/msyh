//巡检点类型帮助   Ext.BsAqWatch.RichHelp
Ext.define('Ext.Gc3.BsAqWatch', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsAqWatchHelp',
    helpid: 'pms3.bs_aq_watch',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});