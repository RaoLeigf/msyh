Ext.define('Ext.Gc.SimpledataPanel', {
    extend: 'Ext.form.Panel',
    layout: 'border',
    valueField: 'PhId',
    displayField: 'Name',
    width: 550,
    height: 350,
    muilt: true,//需要传递是否单选和多选
    url: '',
    model: '',
    columns: [],//需要传递grid列
    queryobj: '',
    initComponent: function () {
        var me = this;

        me.addEvents('helpselected');

        var store = Ext.create('Ext.ng.JsonStore', {
            model: me.model,
            pageSize: 25,
            autoLoad: true,
            url: me.url,
            queryObj: {}
        });

        store.on('beforeload', function (store, operation, eOpts) {
            if (me.queryobj)
                Ext.apply(store.proxy.extraParams, me.queryobj);
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        if (me.muilt)
            var selModel = Ext.create('Ext.selection.CheckboxModel');
        else
            var selModel = '';

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: false,
            selModel: selModel, //多选
            //stateId: 'nggrid',
            store: store,
            columnLines: true,
            columns: me.columns,
            bbar: pagingbar
        });

        me.grid = grid;

        me.items = [
            {
                region: 'center',
                xtype: 'container',
                layout: 'border',
                split: true,
                border: false,
                items: grid
            }
        ];

        this.callParent(arguments);

        ////双击选中
        grid.on('itemdblclick', function (grd, record, item, index, e, eOpts) {
            me.fireEvent("helpselected", me.getHelpData())
        });
    },
    getHelpData: function () {

        var me = this;

        var code = "";
        var name = "";
        var pobj = new Object();
        var df = me.displayField;
        var vf = me.valueField;
        //var select1 = win.queryById("westgrid").store.data.items;
        var select = me.grid.getSelectionModel().getSelection();
        if (select.length <= 0) {
            return;
        }
        Ext.Array.each(select, function (model) {
            code = code + model.data[vf] + ",";
            name = name + model.data[df] + ",";
        });
        code = code.substring(0, code.length - 1);
        name = name.substring(0, name.length - 1);
        //pobj.data = Ext.decode(select[0].data.row);
        var obj = new Object();
        obj[vf] = code;
        obj[df] = name;

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

        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = select;
        me.seletArr = select;
        return pobj;
    }

});