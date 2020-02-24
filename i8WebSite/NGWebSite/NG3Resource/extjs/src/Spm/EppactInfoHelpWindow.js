//进度帮助界面
Ext.define('Ext.Spm.EppactInfoHelpWindow', {
    extend: 'Ext.window.Window',
    width: 780,
    height: 550,
    title: '作业帮助',
    wbs: '',
    needrel: false,
    selfClose: false,
    wbslist: '',
    pcid: 0,
    isCheck: true,//是否只获取核准数据
    isFinish: false,//用于过滤已完工的作业
    isCal: true,//是否只获取已经计算的数据
    muilt: false,//允许多选
    initComponent: function () {
        var me = this;
        var winlang = "";

        me.addEvents('helpselected');//选中事件

        me.layout = 'fit';

        var panel = Ext.create('Ext.Spm.EppactInfoHelpPanel', {
            border: false,
            pcid: me.pcid,
            needrel: me.needrel,
            selfClose: me.selfClose,
            wbs: me.wbs,
            wbslist: me.wbslist,
            isCheck: me.isCheck,//是否只获取核准数据
            isFinish: me.isFinish,//用于过滤已完工的作业
            isCal: me.isCal,
            muilt: me.muilt,
            listeners: {
                helpselected: function (obj) {
                    me.fireEvent('helpselected', obj);
                    if (!me.selfClose)
                        me.close();
                },
                isready: function (panelLang) {
                    if (panelLang) {
                        winlang = panelLang;
                        me.title = winlang.SpmHelpWinTitle || '作业帮助';
                    }
                }
            }
        });
        me.items = panel;
        me.buttons = [
                        '->',
                        {
                            text: winlang.SpmHelpOk || '确定', handler: function () {
                                var ret = panel.getHelpData();
                                if (ret["status"] == -1) {
                                    Ext.MessageBox.alert(winlang.prompt || '提示', winlang.ChoLastNo || '请选择末级节点！');
                                }
                                if (ret["status"] == -2) {
                                    Ext.MessageBox.alert(winlang.prompt || '提示', winlang.ChoRelType || '请选择关系类型！');
                                }
                                else {
                                    if (ret["data"]) {
                                        me.fireEvent("helpselected", ret["data"]);
                                        if (!me.selfClose)
                                            me.close();
                                    } else {
                                        Ext.MessageBox.alert(winlang.prompt || '提示', winlang.ChoData || '请选择数据！');
                                    }
                                }
                            }
                        },
                            {
                                text: winlang.SpmHelpCancel || '取消', handler: function () { me.close(); }
                            }

        ];

        me.callParent();
    },
    listeners: {
        beforeshow: $winBeforeShow,
        beforeclose: $winBeforeClose
    }
});