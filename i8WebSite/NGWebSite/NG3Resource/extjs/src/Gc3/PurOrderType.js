//订单类型帮助gcPurOrderTypeRichHelp  Ext.GcPurOrderType.RichHelp
Ext.define('Ext.Gc3.PurOrderType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPurOrderTypeRichHelp',
    helpid: 'pmm3.pur_ordertype',
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