//运输方式帮助gcShipTypeRichHelp  Ext.GcShipType.RichHelp
Ext.define('Ext.Gc3.ShipType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcShipTypeRichHelp',
    helpid: 'pmm3.shiptype',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ShipName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});