Ext.define('Ext.Gc.SimpledataWindow', {
    extend: 'Ext.window.Window',
    width: 550,
    height: 350,
    title: CommLang.NeedWebWinName||'需要传递窗口名',
    muilt: true,//需要传递是否单选和多选
    url: '',//需要传递url
    model: '',//需要传递store的model
    columns: [],//需要传递grid列
    queryobj: '',//外部条件{id:0001}
    layout: 'fit',
    initComponent: function () {
        var me = this;

        me.addEvents('helpselected');//选中事件

        var panel = Ext.create('Ext.Gc.SimpledataPanel', {
            border: false,
            muilt: me.muilt,
            url: me.url,
            model: me.model,
            columns: me.columns,
            queryobj: me.queryobj,
            listeners: {
                helpselected: function (obj) {
                    me.fireEvent('helpselected', obj);
                    me.close();
                }
            }
        });
        me.panel = panel;
        me.items = panel;
        me.buttons = [
                        '->',
                        {
                            text: CommLang.CommitTo||'确定', handler: function () {
                                var data = panel.getHelpData();
                                if (data) {
                                    me.fireEvent("helpselected", data);
                                    me.close();
                                } else {
                                    Ext.MessageBox.alert('', CommLang.ChooseData||'请选择数据');
                                }
                            }
                        },
                            {
                                text: '取消', handler: function () { me.close(); }
                            }

        ];

        me.callParent();
    },
    setqueryobj: function (obj) {
        me.panel.queryobj = obj;
    }
});