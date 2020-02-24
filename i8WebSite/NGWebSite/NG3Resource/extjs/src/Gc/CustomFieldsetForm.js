Ext.define('Ext.Gc.CustomFieldsetForm', {
    extend: 'Ext.form.FieldSet',
    title: CommLang.TjAndKzInfo || '统计及控制信息',//TjAndKzInfo
    collapsed: true,
    columnsPerRow: 4,
    border: true,
    collapsible: true,
    allfields: [
    ],
    schemeid: 0,
    containerArry: [],
    container: '',//单个grid的刷新
    mcontainer: '',//form容器ID
    initComponent: function () {
        var me = this;

        if (me.containerArry) {
            Ext.Array.forEach(me.containerArry, function (item) {
                if (item) {
                    if (item.isXType) {
                        if (item.isXType('ngGridPanel')) {
                            item.on('selectionchange', function (grd, selected, eOpts) {

                                if (selected.length === 0) {
                                    return;
                                } else if (item.curSelectedRow !== selected[0]) {

                                    item.curSelectedRow = selected[0];
                                    me.impTargetOthers(item);
                                }

                                //me.impTargetOthers(item);
                            });
                        }
                    }
                }
            });
        }

        this.callParent(arguments);
    },
    listeners: {
        expand: function () {
            var me = this;

            //控制只请求一次-去掉单次请求
            if (me.items.items.length > 0) {
                //未勾选实时则不发请求
                if (Ext.getCmp('impRereieve').getValue() == false)
                    return;
            }

            ImpTargetOthers(
                me.schemeid,
                me.containerArry,
                me,
                me.mcontainer
            );
        },
        collapse: function () {
        }
    },
    setSchemeid: function (schemeid) {
        var me = this;
        this.schemeid = schemeid;
    },
    impTargetOthers: function (grid)//用于grid切换行刷新
    {
        var me = this;

        if (me.collapsed)
            return;

        //未勾选实时则不发请求
        if (Ext.getCmp('impRereieve').getValue() == false)
            return;

        ImpTargetOthers(me.schemeid, me.containerArry, me, me.mcontainer, grid);
    },
    setContainerArry: function (array) {
        var me = this;
        me.containerArry = array;
    }
}
);