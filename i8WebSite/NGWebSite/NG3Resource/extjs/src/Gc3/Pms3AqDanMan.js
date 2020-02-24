//关联危险源帮助gcTrainTypeRichHelp    Ext.GcPms3AqDanMan.RichHelp
Ext.define('Ext.Gc3.Pms3AqDanMan', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPms3AqDanManRichHelp',
    helpid: 'pms3.pms3_aq_dan_man',
    valueField: 'PhId',
    displayField: 'Title',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});