//检查类别帮助    Ext.BsSpotctlType.RichHelp
Ext.define('Ext.Gc3.BsSpotctlType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsSpotctlTypeHelp',
    helpid: 'pms3.bs_spotctl_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});