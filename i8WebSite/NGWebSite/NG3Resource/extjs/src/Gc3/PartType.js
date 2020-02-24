//检查部位帮助    Ext.PartType.RichHelp
Ext.define('Ext.Gc3.PartType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcPartTypeHelp',
    helpid: 'pms3.bs_chk_part',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});