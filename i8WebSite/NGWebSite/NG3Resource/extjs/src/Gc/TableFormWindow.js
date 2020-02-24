Ext.define('Ext.Gc.TableFormWindow', {
    extend: 'Ext.window.Window',
    modal: true,
    width: 650,
    closable: true,
    resizable: false,
    closeAction: "hide",
    constrain: true,

    initComponent: function () {
        var me = this;
        if (me.formConfig) {

            ////设置字段宽度
            var labelWidth = 60;
            if (me.formConfig.fields && me.formConfig.fields.length > 0) {
                Ext.Array.forEach(me.formConfig.fields, function (field) {
                    if (field.fieldLabel) {
                        if (labelWidth < field.fieldLabel.length * 12 + 6) {
                            labelWidth = field.fieldLabel.length * 12 + 6;
                        }
                    }
                });
            }
            if (me.formConfig.fieldDefaults) {
                me.formConfig.fieldDefaults.labelWidth = labelWidth;
            } else {
                me.formConfig.fieldDefaults = {
                    labelWidth: labelWidth,
                    anchor: '100%',
                    margin: '3 10 3 0',
                    msgTarget: 'side'
                };
            }


            var form = Ext.create('Ext.ng.TableLayoutForm', me.formConfig);
            me.tableForm = form;
            me.items = [form];

            ////设置窗口高度
            if (me.height) {
                var h = form.items.length * 28 + 80;
                if (h > 250) {
                    me.height = h;
                } else {
                    me.height = 250;
                }
            }

        }

        if (me.buttons) {
            for (var i = 0; i < me.buttons.length; i++) {
                var btn = me.buttons[i];
                if (btn == 'close') {
                    me.buttons[i] = {
                        text: CommLang.CloseSpace||'关 闭',
                        handler: function () {
                            me.hide();
                        }
                    };
                }
            }
        }

        this.callParent();
    },
    getForm: function () {
        var me = this;
        return me.tableForm.getForm();
    },
    setData: function (data) {
        var me = this;

        var form = me.tableForm.getForm();

        form.reset();
        form.setValues(data);
    }
});