Ext.define('Ext.Gc.ViewFormWindow', {
    extend: 'Ext.Gc.TableFormWindow',
    title:CommLang.ViewWin|| "查看窗口",

    initComponent: function () {
        var me = this;

        if (me.formConfig) {
            Ext.apply(me.formConfig, {
                buskey: 'PhId', //对应的业务表主键属性
                otype: 'view' //操作类型,add||edit||view
            });

            Ext.applyIf(me.formConfig, {
                columnsPerRow: 2
            });
        }

        me.buttons = ["->", 'close'];

        this.callParent();
    }
});