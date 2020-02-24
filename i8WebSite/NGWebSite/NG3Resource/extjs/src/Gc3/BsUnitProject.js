//单位工程帮助gcBsUnitProjectRichHelp Ext.GcBsUnitProject.RichHelp
Ext.define('Ext.Gc3.BsUnitProject', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBsUnitProjectRichHelp',
    helpid: 'pms3.bs_unit_project',
    valueField: 'PhId',
    displayField: 'ItemName',
    editable: false,
    ORMMode: true,
    MaxLength: 100
});