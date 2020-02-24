//劳务工信息弹出帮助字段-单选
Ext.define('Ext.Gc3.YgCraftInfoHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.YgCraftInfoHelp',
    helpid: 'pms3.pms3_yg_craft_info',
    valueField: 'PhId',
    displayField: 'CraftName',
    ORMMode: true,
    editable: false,
    pcid: 0,
    muilt: false,//允许多选
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly) return;

        if (me.fireEvent('beforetriggerclick', me) == false)
            return;

        var help = Ext.create('Ext.Gc3.YgCraftInfoHelpWindow', { pcid: me.pcid });
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