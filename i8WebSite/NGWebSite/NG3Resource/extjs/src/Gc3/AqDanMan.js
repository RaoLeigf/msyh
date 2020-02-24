//危险源管理帮助   Ext.AqDanMan.RichHelp
Ext.define('Ext.Gc3.AqDanMan', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcAqDanManHelp',
    helpid: 'pms3.pms3_aq_dan_man',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'dansourcename',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});