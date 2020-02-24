//民族帮助
Ext.define('Ext.Gc3.FolkHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcFolkHelp',
    ORMMode: true,
    helpid: 'fg_simple_data_folk',
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});