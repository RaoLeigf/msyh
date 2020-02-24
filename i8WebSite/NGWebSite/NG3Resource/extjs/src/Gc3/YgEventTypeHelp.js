//事件类别帮助 
Ext.define('Ext.Gc3.YgEventTypeHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcYgEventTypeHelp',//别名xtype引用
    ORMMode: true,
    helpid: 'pms3.yg_eventtype',//对应通用帮助
    valueField: 'PhId',//对应toolkit通用帮助
    displayField: 'Name',//对应toolkit通用帮助
    MaxLength: 100,
    editable: false,
    mustInput: false

});