//订购方式帮助gcPurWayRichHelp    Ext.GcPurWay.RichHelp
Ext.define('Ext.Gc3.PurWay', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPurWayRichHelp',
    helpid: 'pmm3.pur_way',
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