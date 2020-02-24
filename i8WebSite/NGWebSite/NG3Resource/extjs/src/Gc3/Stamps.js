//印花税税目帮助gcStampsRichHelp   Ext.GcStamps.RichHelp
Ext.define('Ext.Gc3.Stamps', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcStampsRichHelp',
    helpid: 'itm3_stamps',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});