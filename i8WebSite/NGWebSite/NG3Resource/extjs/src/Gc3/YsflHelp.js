//预算分类帮助    Ext.GcYsflHelp.RichHelp
Ext.define('Ext.Gc3.YsflHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcYsflHelp',
    ORMMode: true,
    helpid: 'pms3.bs_ys_type',
    valueField: 'phid',
    displayField: 'name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});