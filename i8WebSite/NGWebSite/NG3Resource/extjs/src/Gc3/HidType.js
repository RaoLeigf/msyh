//隐患类型帮助gcHidTypeRichHelp   Ext.GcHidType.RichHelp
Ext.define('Ext.Gc3.HidType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcHidTypeRichHelp',
    helpid: 'pms3.bs_hid_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});