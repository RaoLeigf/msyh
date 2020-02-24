//单项工程帮助    Ext.BsIndividProject.RichHelp
Ext.define('Ext.Gc3.BsIndividProject', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsIndividProjectHelp',
    helpid: 'pms3.bs_individ_project',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});