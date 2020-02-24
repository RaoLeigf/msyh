//仓库帮助gcWarehouseNonRightsRichHelp（无信息权限过滤）
Ext.define('Ext.Gc3.WarehouseNonRights', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWarehouseNonRightsRichHelp',
    helpid: 'pmm3.warehouse_zc',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Whname',
    userCodeField: "Warehouse",
    listFields: 'Warehouse,Whname,PhId',
    listHeadTexts: '编码,名称',
    MaxLength: 100,
    editable: true,
    mustInput: false,
    setPc: function (newValue) {
        ////设置按项目过滤
        var me = this;

        if (!Ext.isEmpty(newValue)) {
            me.setClientSqlFilter('phid in (select warehouseid from pc_warehouse where pc_id=\'' + newValue + '\')');
        } else {
            me.setClientSqlFilter('');
        }

        me.lastQuery = null;
    },
    getOnlyOneWarehouse: function () {
        var me = this;
        if (!me.listFields) {
            Ext.Msg.alert('提示', '请设置帮助的listFields属性！');
            return;
        }
        var fields = me.listFields.split(',');

        var modelFields = new Array();
        for (var i = 0; i < fields.length; i++) {

            var tempfield = fields[i].split('.');
            var temp;
            if (tempfield.length > 1) {
                temp = tempfield[1]; //去掉表名
            }
            else {
                temp = fields[i];
            }

            modelFields.push({
                name: fields[i],
                type: 'string',
                mapping: temp
            });
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: modelFields
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 2,
            autoLoad: false,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function () {

            if (me.outFilter) {
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
            if (me.firstRowFilter) { //加载第一行的过滤条件
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.firstRowFilter) });
            }
            if (me.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
            }
        });

        store.load(function () {
            if (store.data.items.length === 1) {
                var data = store.data.items[0].data;
                me.fireEvent('onlyOneWarehouseloaded', data);
            } else {
                //me.fireEvent('onlyOneWarehouseloaded', undefined);
            }
        });
    }
});