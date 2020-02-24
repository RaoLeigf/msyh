Ext.define('Ext.Gc.EditFormWindow', {
    extend: 'Ext.Gc.TableFormWindow',
    title: CommLang.EditWin||"编辑窗口",

    saveFunc: null,

    initComponent: function () {
        var me = this;

        if (me.formConfig) {
            Ext.apply(me.formConfig, {
                buskey: 'PhId', //对应的业务表主键属性
                otype: 'edit' //操作类型,add||edit||view
            });

            Ext.applyIf(me.formConfig, {
                columnsPerRow: 2
            });
        }

        me.buttons = [
            "->",
            {
                text: CommLang.SaveSpace||'保 存',
                handler: function () {
                    me.saveFunc(me);
                }
            }, 'close'
        ];

        this.callParent();
    }
});