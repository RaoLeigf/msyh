//违章类型帮助    Ext.BsAqPecType.RichHelp
Ext.define('Ext.Gc3.BsAqPecType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsAqPecTypeHelp',
    helpid: 'pms3.bs_aq_pec_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});