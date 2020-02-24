//资源大类  Ext.GcResBs.RichHelp
Ext.define('Ext.Gc3.ResBs', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcResBsHelp',
    helpid: 'pmm3.res_bs',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});