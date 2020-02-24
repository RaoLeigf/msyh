//作业活动帮助gcTrainTypeRichHelp Ext.GcBsActivityWork.RichHelp
Ext.define('Ext.Gc3.BsActivityWork', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBsActivityWorkRichHelp',
    helpid: 'pms3.bs_aq_activity_work',
    valueField: 'PhId',
    displayField: 'ItemName',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});