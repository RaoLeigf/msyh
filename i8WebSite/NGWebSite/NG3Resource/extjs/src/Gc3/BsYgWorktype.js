//工种帮助  Ext.BsYgWorktype.RichHelp
Ext.define('Ext.Gc3.BsYgWorktype', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsYgWorktypeHelp',
    helpid: 'pms3.bs_yg_worktype',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});