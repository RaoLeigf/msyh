Ext.define('Ext.Gc.AddFormWindow', {
    extend: 'Ext.Gc.TableFormWindow',
    title: CommLang.AddWin||"新增窗口",

    saveFunc: null,
    initComponent: function () {
        var me = this;

        if (me.formConfig) {
            Ext.apply(me.formConfig, {
                buskey: 'PhId', //对应的业务表主键属性
                otype: 'add' //操作类型,add||edit||view
            });

            Ext.applyIf(me.formConfig, {
                columnsPerRow: 2
            });
        }

        me.buttons = [
            {
                xtype: "checkbox",
                name: "cb_continue",
                boxLabel: CommLang.ContinueFill||"连续录入",
                align: "left",
                handler: function (rd, val) {
                    me.IsContinue = val;
                }
            },
            "->",
            {
                text: CommLang.SaveSpace || '保 存',
                handler: function () {
                    me.saveFunc(me, me.IsContinue);
                }
            }, 'close'
        ];

        this.callParent();
    }
});