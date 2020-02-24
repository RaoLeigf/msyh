//工程项目帮助    Ext.ProjectTable_Js.RichHelp
Ext.define('Ext.Gc3.ProjectTableJs', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcProjectTableJsRichHelp',
    helpid: 'pms3.project_table',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ProjectName',
    MaxLength: 100,
    editable: true,
    mustInput: false
});