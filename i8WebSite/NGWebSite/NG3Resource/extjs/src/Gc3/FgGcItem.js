//辅助项表帮助gcFgGcItemRichHelp  Ext.GcFgGcItem.RichHelp
Ext.define('Ext.Gc3.FgGcItem', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcFgGcItemRichHelp',
    helpid: 'pmm3.fg_gc_item',
    ORMMode: true,
    valueField: 'ProjCode',
    displayField: 'ProjName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});