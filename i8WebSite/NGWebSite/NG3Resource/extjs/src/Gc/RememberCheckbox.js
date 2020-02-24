Ext.define('Ext.Gc.RememberCheckbox', {
    extend: 'Ext.form.field.Checkbox',
    alias: 'widget.GcRememberCheckbox',
    uniqueCode: '',
    initComponent: function () {
        var me = this;

        me.callParent(arguments);

        if (!Ext.isEmpty(me.uniqueCode)) {
            me.on('afterrender', function () {
                me.on('show', function () {
                    me.getCheckInfo();
                });
                
                me.on('change', function () {
                    me.saveCheckInfo();
                });
            });
        }
    },
    getCheckInfo: function () {
        var me = this;
        if (me.isHidden() === false) {
            Ext.Ajax.request({
                params: {
                    'uniqueCode': 'iscontinue_' + me.uniqueCode
                },
                url: C_ROOT + 'PMS/Common/Remember/GetRememberInfo',
                success: function (response) {
                    if (response.responseText === "1") {
                        me.setValue(true);
                    } else {
                        me.setValue(false);
                    }
                }
            });
        }
    },
    saveCheckInfo: function () {
        var me = this;
        if (me.isHidden() === false) {
            var remInfo = "";
            if (me.getValue() === true) {
                remInfo = "1";
            }
            Ext.Ajax.request({
                params: {
                    'uniqueCode': 'iscontinue_' + me.uniqueCode,
                    'remInfo': remInfo,
                    'isMemo': 1
                },
                url: C_ROOT + 'PMS/Common/Remember/SaveRememberInfo',
                success: function (response) {
                }
            });
        }
    }
});