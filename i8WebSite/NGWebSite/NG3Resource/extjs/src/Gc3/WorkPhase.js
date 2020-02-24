//工作阶段帮助    Ext.WorkPhase.RichHelp
Ext.define('Ext.Gc3.WorkPhase', {
    extend: 'Ext.ng.RichHelp',
    name: 'PhidWorkPhase',
    helpid: 'pmm3.mbs',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});