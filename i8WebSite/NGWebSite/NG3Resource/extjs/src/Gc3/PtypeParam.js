//合同预收预付款项  Ext.GcPtypeParam.RichHelp
Ext.define('Ext.Gc3.PtypeParam', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcPtypeParam',
    helpid: 'pcm3.pcm3_cnt_ptypeparam',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'NameNew',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});