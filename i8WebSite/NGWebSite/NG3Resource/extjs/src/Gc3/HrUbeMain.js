//联盟体人才帮助gcRegionRichHelp   Ext.GcHrUbeMain.RichHelp
Ext.define('Ext.Gc3.HrUbeMain', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcHrUbeMainRichHelp',
    helpid: 'pmm3.hr_ube_main',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Cname',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1
});