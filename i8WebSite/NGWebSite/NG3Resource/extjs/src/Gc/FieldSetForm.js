Ext.define('Ext.Gc.FieldSetForm', {
    extend: 'Ext.ng.FieldSetForm',

    hiddenFields: [], //不显示列
    forceDisable: false, ////强制失效标识，初始化时设为true后，控件永远失效
    fieldRightUIInfo: '',
    initComponent: function () {
        var me = this;

        ////隐藏字段
        var hiddenFields = me.hiddenFields;
        if (hiddenFields.length > 0) {
            for (var i = 0; i < me.fieldSets.length; i++) {
                var fieldset = me.fieldSets[i];
                if (me.isBuildByRows) {
                    if (fieldset.fieldRows && fieldset.fieldRows.length > 0) {
                        if (fieldset.fieldRows && fieldset.fieldRows.length > 0) {
                            fieldset.fieldRows = me.dealHiddenFields(hiddenFields, fieldset.fieldRows);
                        }
                    }
                } else {
                    if (fieldset.allfields && fieldset.allfields.length > 0) {
                        fieldset.allfields = me.dealHiddenFields(hiddenFields, fieldset.allfields);
                    }
                }
            }
        }

        //设置小数位
        for (var i = 0; i < me.fieldSets.length; i++) {
            var fieldset = me.fieldSets[i];
            if (me.isBuildByRows) {
                if (fieldset.fieldRows && fieldset.fieldRows.length > 0) {
                    if (fieldset.fieldRows && fieldset.fieldRows.length > 0) {
                        SetFieldFormat(fieldset.fieldRows);
                    }
                }
            } else {
                if (fieldset.allfields && fieldset.allfields.length > 0) {
                    SetFieldFormat(fieldset.allfields);
                }
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

                if (field.userSetReadOnly) {
                    if (field.gcUserReadOnly === false) {
                        field.setReadOnly(false);
                    }
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