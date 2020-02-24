//交货地点帮助gcDeliverNameRichHelp   Ext.GcDeliverName.RichHelp
Ext.define('Ext.Gc3.DeliverName', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcDeliverNameRichHelp',
    helpid: 'pmm3.deliver_name',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'DeliverName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});