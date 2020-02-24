//设备帮助 
Ext.define('Ext.Gc3.YgEquipmentHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcYgEquipmentHelp',//别名xtype引用
    ORMMode: true,
    helpid: 'pms3.pms3_yg_equipment_m',//对应通用帮助
    valueField: 'PhId',//对应toolkit通用帮助
    displayField: 'EquipmentName',//对应toolkit通用帮助
    MaxLength: 100,
    editable: false,
    mustInput: false

});