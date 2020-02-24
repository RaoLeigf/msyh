//检查结果帮助    Ext.BsCheckResult.RichHelp
Ext.define('Ext.Gc3.BsCheckResult', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsCheckResultHelp',
    helpid: 'pms3.bs_check_result',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});