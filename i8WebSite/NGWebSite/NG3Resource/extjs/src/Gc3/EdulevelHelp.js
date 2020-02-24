//学历帮助
Ext.define('Ext.Gc3.EdulevelHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcEdulevelHelp',
    ORMMode: true,
    helpid: 'edulevel',
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});