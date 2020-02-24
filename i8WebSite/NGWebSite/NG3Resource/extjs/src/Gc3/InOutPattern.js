//资产出入库方式帮助 Ext.InOutPattern.RichHelp
Ext.define('Ext.Gc3.InOutPattern', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcInOutPatternHelp',
    helpid: 'pmm3.pmm3_kc_in_out_pattern',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});