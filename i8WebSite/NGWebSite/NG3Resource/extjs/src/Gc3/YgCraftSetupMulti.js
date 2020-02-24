//用工工种多选帮助 
Ext.define('Ext.Gc3.YgCraftSetupMulti', {
    extend: 'Ext.ng.MultiRichHelp',
    alias: 'widget.gcYgCraftSetupMultiHelp',//别名xtype引用
    ORMMode: true,
    helpid: 'pms3.pms3_yg_craft_setup',//对应通用帮助
    valueField: 'PhId',//对应toolkit通用帮助
    displayField: 'Name',//对应toolkit通用帮助
    MaxLength: 500,
    editable: false,
    mustInput: false

});