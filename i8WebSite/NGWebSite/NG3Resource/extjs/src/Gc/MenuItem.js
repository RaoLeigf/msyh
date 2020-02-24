Ext.define('Ext.Gc.MenuItem', {
    extend: 'Ext.menu.Item',
    alias: 'widget.GcMenuItem',

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

        if (me.forceDisable === true) {
            return me;
        }

        me.callParent(arguments);
        return me;
    }
});