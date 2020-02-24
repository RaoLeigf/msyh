//劳务工进场信息帮助(取进场表)
Ext.define('Ext.Gc3.YgEnterExitHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.YgEnterExitHelp',
    helpid: 'pms3.yg.enter_exit',
    valueField: 'PhId',
    displayField: 'CraftName',
    editable: false,
    ORMMode: true,
    setPc: function (pcid) {
        ////设置按项目过滤
        var me = this;

        if (!Ext.isEmpty(pcid)) {
            me.setOutFilter({ 'pms3_yg_enter_exit.phid_pc': pcid });
        }
    }
});