//订单级别帮助gcPurOrderRanksRichHelp Ext.GcPurOrderRanks.RichHelp
Ext.define('Ext.Gc3.PurOrderRanks', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPurOrderRanksRichHelp',
    helpid: 'pmm3.pur_order_ranks',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});