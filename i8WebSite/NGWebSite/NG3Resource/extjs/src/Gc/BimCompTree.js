//构件分类树
Ext.define('Ext.Gc.BimCompTree', {
    extend: 'Ext.Gc.TreePanel',
    alias: 'widget.gcBimCompTree',
    region: 'west',
    width: 220,
    split: true,
    rootVisible: true,
    hideHeaders: true,
    root: { text: "全部" },
    initComponent: function () {
        var me = this;
        me.autoLoad = false;
        me.treeFields = [
            { name: 'text', type: 'string' },
            { name: 'PhId', type: 'string' },
            { name: 'Floor', type: 'string' },
            { name: 'Prof', type: 'Prof' },
            { name: 'Class', type: 'Class' },
            { name: 'Subclass', type: 'Subclass' }
        ],
        me.columns = [{
            text: Lang.PhId || '物理主键',
            flex: 0,
            sortable: false,
            dataIndex: 'PhId',
            hideable: false,
            hidden: true
        }, {
            text: Lang.Name || '名称',
            flex: 1,
            xtype: 'treecolumn',
            dataIndex: 'text',
            hidden: false,
            hideable: false,
            align: 'left'
        }],

        me.store = Ext.create('Ext.data.TreeStore', {
            fields: me.treeFields,
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'PMS/PMS/BimComp/GetBimCompTreeData'
            }
        });

        this.callParent(arguments);
    },
    loaddata: function (phidpc) {
        var me = this;

        busphidpc = phidpc;
        me.getRootNode().collapse(true);
        me.getRootNode().removeAll();
        Ext.apply(me.getStore().proxy.extraParams, { 'phidpc': phidpc });
        me.getStore().load();
        me.getView().refresh();
    },
    getCheckedNode: function () {
        //var records = this.getChecked();
        var records = [];
        var treedata = this.getSelectionModel().getSelection();
        if (treedata.length > 0)
            records.push(treedata[0]);
        var arr = [];

        Ext.Array.each(records, function (record) {
            if (1 == 1 || !Ext.isEmpty(record.data.Subclass)) {
                var newobj = {};
                newobj.PhId = record.data.PhId;
                newobj.Floor = record.data.Floor;
                newobj.Prof = record.data.Prof;
                newobj.Class = record.data.Class;
                newobj.Subclass = record.data.Subclass;
                arr.push({ data: newobj });
            }
        });

        return arr;
    },
    listeners: {
        //'checkchange': function (node, checked, eOpts) {
        //    var me = this;
        //    if (checked === true) {
        //        //me.checkedWithParentNode(node);
        //        me.checkedWithChild(node);
        //    } else {
        //        me.uncheckedWithParentNode(node);
        //        me.uncheckedWithChild(node);
        //    }
        //    setTimeout(function () {
        //        store.load();
        //    },500);
        //}

        'selectionchange': function (tree, selected, eOpts) {
            setTimeout(function () {
                store.load();
            }, 500);
        }
    }
});