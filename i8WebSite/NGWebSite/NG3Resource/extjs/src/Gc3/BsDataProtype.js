//专业类型帮助gcBsDataProtypeRichHelp Ext.GcBsDataProtype.RichHelp
Ext.define('Ext.Gc3.BsDataProtype', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBsDataProtypeRichHelp',
    helpid: 'pmm3.bs_data_protype',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    userCodeField: "ItemNo",
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});