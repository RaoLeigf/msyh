Ext.define('Ext.Gc.Toolbar', {
    extend: 'Ext.ng.Toolbar',
    showArrowBtn: false,//是否显示toolbar最右边的下拉button列表
    defaults: {
        xtype: 'GcButton'
    },
    setableAll:
        ///操作状态 View 状态忽略able参数
        ///可用,不可用 true,false
        ///按钮数组 
        ///当前操作不影响的按钮数组 只处理buttonarr为空的情况
        ///是否永久失效  true,false
        function (otype, able, buttonarr, buttonarrnot, fordisable) {
            if (buttonarr == "")
                buttonarr = undefined;
            if (buttonarrnot == "")
                buttonarrnot = undefined;

            var me = this;

            if (otype == 'View') {
                buttonarr = undefined;
                able = false;
                fordisable = true;
            }

            if (buttonarr) {
                for (var i = 0; i < buttonarr.length; i++) {
                    if (me.getComponent(buttonarr[i])) {

                        if (me.getComponent(buttonarr[i]).xtype != "GcButton")
                            continue;

                        if (able)
                            me.getComponent(buttonarr[i]).enable(able);
                        else {
                            if (fordisable)
                                me.getComponent(buttonarr[i]).forcedisable();
                            else
                                me.getComponent(buttonarr[i]).disable();
                        }
                    }
                }
                return;
            }

            var keys = me.items.keys;

            for (var i = 0; i < keys.length; i++) {

                //附件,关联单据,送审追踪,关闭,帮助,打印
                if (keys[i] == "applycheck"|| keys[i] == "attachment" || keys[i] == "relabill" || keys[i] == "checkview" || keys[i] == "close"
                    || keys[i] == "help" || keys[i] == "print" || keys[i].indexOf( "tbfill-" ) === 0 || keys[i] == "impupdown" || keys[i] == "impinfoview" )
                    continue

                //不设置工作流的字段
                if (keys[i].substring(0, 5) == 'wfbtn')
                    continue;

                //取消审核按钮反向控制
                if (keys[i] == "unverify") {
                    if (able)
                        me.getComponent(keys[i]).disable();
                    else
                        me.getComponent(keys[i]).enable(true);

                    continue;
                }

                //if (keys[i] == "applycheck")//申请去审按钮 在gcfun.js里处理
                //    continue;

                if (me.getComponent(keys[i])) {

                    if (me.getComponent(keys[i]).xtype != "GcButton")
                        continue;

                    if (buttonarrnot)
                        if (buttonarrnot.indexOf(keys[i]) >= 0)
                            continue
                        else {
                            if (able)
                                me.getComponent(keys[i]).enable(able);
                            else {
                                if (fordisable)
                                    me.getComponent(keys[i]).forcedisable();
                                else
                                    me.getComponent(keys[i]).disable();
                            }
                        }
                    else {
                        if (able)
                            me.getComponent(keys[i]).enable(able);
                        else {
                            if (fordisable)
                                me.getComponent(keys[i]).forcedisable();
                            else
                                me.getComponent(keys[i]).disable();
                        }
                    }
                }
            }

        }
});