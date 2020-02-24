//采购级别帮助gcPurRanksRichHelp  Ext.GcPurRanks.RichHelp
Ext.define('Ext.Gc3.PurRanks', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPurRanksRichHelp',
    helpid: 'pmm3.pur_ranks',
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