//业务点帮助gcGcCPfcRegisterRichHelp Ext.GcMetBus.RichHelp
Ext.define('Ext.Gc3.MetBus', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcGcMetBusRichHelp',
    helpid: 'pms3.metadata_bustree',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'BusinessName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});