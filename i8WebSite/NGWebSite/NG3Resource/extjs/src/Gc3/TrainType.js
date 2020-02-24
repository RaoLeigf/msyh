//培训类型帮助gcTrainTypeRichHelp Ext.GcTrainType.RichHelp
Ext.define('Ext.Gc3.TrainType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcTrainTypeRichHelp',
    helpid: 'pms3.bs_aq_train_type',
    valueField: 'PhId',
    displayField: 'ItemName',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});