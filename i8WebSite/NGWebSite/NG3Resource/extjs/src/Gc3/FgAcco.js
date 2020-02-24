//科目帮助gcFgAccoRichHelp  Ext.GcFgAcco.RichHelp
Ext.define('Ext.Gc3.FgAcco', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcFgAccoRichHelp',
    helpid: 'pms3.fg_acco',
    ORMMode: true,
    valueField: 'AcCode',
    displayField: 'AcName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});