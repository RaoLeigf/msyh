//作业帮助  Ext.ActId.RichHelp
Ext.define('Ext.Gc3.ActId', {
    extend: 'Ext.ng.RichHelp',
    name: 'PhidActId',
    helpid: 'pms3.spm3_eppact',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Activityname',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});