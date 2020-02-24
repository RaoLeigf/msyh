//清单帮助窗口    Ext.BoqM.RichHelp
Ext.define('Ext.Gc3.BoqM', {
    extend: 'Ext.ng.BoqHelp',
    alias: 'widget.gcBoqMRichHelp',
    helpid: 'pms3.pms3_boq_m',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Cname',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    listoritem: 'M',
    setPc: function (newValue) {
        ////设置按项目过滤
        var me = this;

        if (!Ext.isEmpty(newValue)) {
            me.phidpc = newValue;
        } else {
            me.phidpc = 0;
        }

        me.lastQuery = null;
    }
});