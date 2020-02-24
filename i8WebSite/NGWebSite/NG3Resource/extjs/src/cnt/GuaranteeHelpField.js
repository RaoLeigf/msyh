//合同保函弹出帮助字段-单选
Ext.define('Ext.cnt.GuaranteeHelpField', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GuaranteeHelpField',
    helpid: 'pcm3.pcm3_cnt_guarantee',
    valueField: 'PhId',
    displayField: 'BillNo',
    editable: false,
    cntmodel: 0,
    ORMMode: true,
    muilt: false,//允许多选
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly) return;
        var help = Ext.create('Ext.cnt.GuaranteeHelpWindow', { cntmodel: me.cntmodel });
        help.on('helpselected', function (data) {
            if (data) {
                var obj = new Object();
                var name = data.name;
                var code = data.code;
                obj[me.valueField] = code;

                if (me.displayFormat) {
                    obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
                } else {
                    obj[me.displayField] = name;
                }

                Ext.define('richhelpModel', {
                    extend: 'Ext.data.Model',
                    fields: [{
                        name: me.valueField,
                        type: 'string',
                        mapping: me.valueField
                    }, {
                        name: me.displayField,
                        type: 'string',
                        mapping: me.displayField
                    }
                    ]
                });

                var valuepair = Ext.create('richhelpModel', obj);
                me.setValue(valuepair); //必须这么设置才能成功
                help.close();
                me.fireEvent('helpselected', data);
            }
        });
        help.show();
    }
});