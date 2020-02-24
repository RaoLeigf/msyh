//班组类型帮助    Ext.ClassType.RichHelp
Ext.define('Ext.Gc3.ClassType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.ClassTypeHelp',
    helpid: 'pmm3.class_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});