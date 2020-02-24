//退货原因帮助gcThyyRichHelp  Ext.GcThyy.RichHelp
Ext.define('Ext.Gc3.Thyy', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcThyyRichHelp',
    helpid: 'pmm3.thyy',
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