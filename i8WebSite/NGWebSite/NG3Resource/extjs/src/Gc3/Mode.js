//采购方式帮助gcModeRichHelp  Ext.GcMode.RichHelp
Ext.define('Ext.Gc3.Mode', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcModeRichHelp',
    helpid: 'pmm3.cg_mode',
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