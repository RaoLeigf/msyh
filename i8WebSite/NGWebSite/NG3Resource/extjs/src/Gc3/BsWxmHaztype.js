//危险源类别帮助gcTrainTypeRichHelp    Ext.GcBsWxmHaztype.RichHelp
Ext.define('Ext.Gc3.BsWxmHaztype', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBsWxmHaztypeRichHelp',
    helpid: 'pms3.bs_wxm_haztype',
    valueField: 'PhId',
    displayField: 'ItemName',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});