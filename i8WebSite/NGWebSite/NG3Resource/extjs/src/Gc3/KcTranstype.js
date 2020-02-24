//出入库单业务类型帮助gcWarehouseRichHelp Ext.KcTranstype.RichHelp
Ext.define('Ext.Gc3.KcTranstype', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcKcTranstypeRichHelp',
    helpid: 'pmm3.kc_transtype',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Transname',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});