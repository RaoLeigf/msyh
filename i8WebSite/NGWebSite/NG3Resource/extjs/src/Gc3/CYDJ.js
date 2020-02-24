//创优等级帮助gcCYDJRichHelp  Ext.GcCYDJ.RichHelp
Ext.define('Ext.Gc3.CYDJ', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcCYDJRichHelp',
    helpid: 'pms3.cydj',
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