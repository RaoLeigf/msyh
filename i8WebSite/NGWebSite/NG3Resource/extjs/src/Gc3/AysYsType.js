//成本类型帮助gcAysYsTypeRichHelp Ext.GcAysYsType.RichHelp
Ext.define('Ext.Gc3.AysYsType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcAysYsTypeRichHelp',
    helpid: 'pmm3.ays_ys_type',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});