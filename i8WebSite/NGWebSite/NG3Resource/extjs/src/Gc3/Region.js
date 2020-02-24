//区县帮助gcRegionRichHelp  Ext.GcRegion.RichHelp
Ext.define('Ext.Gc3.Region', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcRegionRichHelp',
    helpid: 'pmm3.region',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'RegionName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});