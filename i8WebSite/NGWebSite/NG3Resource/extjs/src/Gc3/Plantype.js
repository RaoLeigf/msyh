//计划类型帮助gcPlantypeRichHelp  Ext.GcPlantype.RichHelp
Ext.define('Ext.Gc3.Plantype', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPlantypeRichHelp',
    helpid: 'pmm3.plantype',
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