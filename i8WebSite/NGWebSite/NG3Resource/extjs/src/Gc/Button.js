Ext.define('Ext.Gc.Button', {
    extend: 'Ext.button.Button',
    alias: 'widget.GcButton',

    forceDisable: false,////强制失效标识，初始化时设为true后，控件永远失效

    initComponent: function () {
        var me = this;

        if (me.forceDisable === true) {
            me.disabled = me.forceDisable;
        }

        me.callParent(arguments);
    },
    forcedisable: function () {
        ////强制失效按钮，失效后无法再生效，主要用于查看界面是对按钮初始状态进行设置
        var me = this;
        me.disable(true);
        me.forceDisable = true;
    },
    enable: function (silent) {
        var me = this;

        if (me.noright === true) {
            ////因为没有权限失效
            return me;
        }

        if (me.forceDisable === true) {
            return me;
        }

        me.callParent(arguments);
        return me;
    },
    forceenable: function () {
        ////强制生效按钮，此方法不受按钮权限和强制失效控制生效按钮
        ////除非特殊需求，一般不用
        var me = this;

        me.noright = false;
        me.forceDisable = false;

        me.enable(true);
    }
});