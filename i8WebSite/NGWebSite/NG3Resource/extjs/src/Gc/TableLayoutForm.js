Ext.define('Ext.Gc.TableLayoutForm', {
    extend: 'Ext.ng.TableLayoutForm',

    hiddenFields: [], //不显示列
    forceDisable: false, ////强制失效标识，初始化时设为true后，控件永远失效
    fieldRightUIInfo: '',
    initComponent: function () {
        var me = this;


        ////隐藏字段
        var hiddenFields = me.hiddenFields;
        if (hiddenFields.length > 0) {

            if (me.isBuildByRows) {
                for (var i = 0; i < me.fieldRows.length; i++) {
                    var row = me.fieldRows[i]; //行
                    me.fieldRows[i] = me.dealHiddenFields(hiddenFields, row);
                }
            } else {
                if (me.fields && me.fields.length > 0) {
                    me.fields = me.dealHiddenFields(hiddenFields, me.fields);
                }
            }
        }

        //设置小数位
        if (me.isBuildByRows) {
            for (var i = 0; i < me.fieldRows.length; i++) {
                var row = me.fieldRows[i]; //行
                SetFieldFormat(row);
            }
        } else {
            if (me.fields && me.fields.length > 0) {
                SetFieldFormat(me.fields);
            }
        }

        this.callParent();

        me.userSetReadOnly(me.forceDisable);

        me.on('afterrender', function () {
            if (me.fieldRightUIInfo) {
                $SetFieldRightUIState(me.id, me.fieldRightUIInfo);
            }
        });
    },
    dealHiddenFields: function (hiddenFields, fields) {
        var fs = [];
        var hc = {
            xtype: 'container',
            hidden: true,
            items: [
            ]
        };

        Ext.Array.forEach(fields, function (item) {
            if (Ext.Array.contains(hiddenFields, item.itemId) === false) {
                fs.push(item);
            } else {
                hc.items.push(item);
            }
        });

        fs.push(hc);
        return fs;
    },

    /////设置控件只读
    ////flag true表示设置控件只读，false取消控件只读
    ////array 可选参数，不传设置全部字段，传一个字符串数组，则特殊设置数组中对应的itemId字段的只读属性
    userSetReadOnly: function (flag, array) {
        var me = this;

        var fields = [];
        var fs = me.getForm().getFields().items;
        if (array) {
            Ext.Array.forEach(fs, function (f) {
                if (Ext.Array.contains(array, f.itemId)) {
                    fields.push(f);
                }
            });
        } else {
            fields = fs;
        }


        if (flag) {
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];

                if (field.userSetReadOnly) {

                    if (field.readOnly !== true) {
                        ////字段当前是非只读的设置字段只读
                        field.gcUserReadOnly = field.readOnly; /////记录一下控件当前的只读状态
                        field.setReadOnly(true);
                    }
                }
            }
        } else {

            ////已强制失效
            if (me.forceDisable === true) {
                return;
            }

            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                if (field.gcUserReadOnly === false) {
                    field.setReadOnly(false);
                }
            }
        }
    },
    forcedisable: function () {
        ////强制失效按钮，失效后无法再生效，主要用于查看界面是对按钮初始状态进行设置
        var me = this;
        me.userSetReadOnly(true);
        me.forceDisable = true;
    }
});