//培训主题帮助gcTrainTitleRichHelp    Ext.GcTrainTitle.RichHelp
Ext.define('Ext.Gc3.TrainTitle', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcTrainTitleRichHelp',
    helpid: 'pms3.bs_aq_train_title',
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    ORMMode: true,
    editable: false
});