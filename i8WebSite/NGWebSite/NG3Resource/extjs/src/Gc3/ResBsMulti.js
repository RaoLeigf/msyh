//Ext.GcResBs.MultiRichHelp
Ext.define('Ext.Gc3.ResBsMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.gcResBsMultiHelp',
    helpid: 'pmm3.res_bs',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});