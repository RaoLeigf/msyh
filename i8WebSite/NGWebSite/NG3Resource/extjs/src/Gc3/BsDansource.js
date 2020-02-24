//危险源帮助gcTrainTypeRichHelp  Ext.GcBsDansource.RichHelp
Ext.define('Ext.Gc3.BsDansource', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBsDansourceRichHelp',
    helpid: 'pms3.bs_dansource',
    valueField: 'PhId',
    displayField: 'ItemName',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});