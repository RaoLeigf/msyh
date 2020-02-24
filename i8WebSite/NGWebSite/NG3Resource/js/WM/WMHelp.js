Ext.define('Ext.ng.TagsHelpWindow', {
    extend: 'Ext.window.Window',
    title: '标签帮助',
    closable: true,
    resizable: false,
    modal: true,
    height: 400,
    width: 600,
    border: 0,
    help: "",
    layout: 'border',
    constrain: true,
    // helptype: "pcid_help",
    ORMMode: true,
    callback: null,
    selectedValue: "",
    //  dataUrl: 'PMS/PC/ProjectTable/GetProjectTableHelp',
    //  useDataUrl: 'PMS/PC/ProjectTable/GetCommonUseProjectTable',
    initComponent: function () {
        var me = this;
        var store;
        var toolbar = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            defaults: {
                margin: '0 5 0 5'
            },
            layout: {
                type: 'hbox'
            },
            items: [
                { xtype: 'textfield', width: 300, itemId: 'keyword' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '搜索',
                    handler: function () {
                        var cname = toolbar.queryById('keyword').getValue();
                        Ext.apply(store.proxy.extraParams, { 'searchkey': cname });
                        store.load();
                    }
                }
            ]
        });

        Ext.define('tagskindTree', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'PhId', type: 'string' },
                { name: 'text', type: 'string' },
                { name: 'CNo', type: 'string' },
                { name: 'CName', type: 'string' }
            ]
        });
        var tagskindTreeStore = Ext.create('Ext.data.TreeStore', {
            model: 'tagskindTree',
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'WM/Doc/TagsKind/GetTagsKindTreeNodes'
            }
        });
        //标签
        var tagskindTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            loaded: true,
            store: tagskindTreeStore,
            //hideHeaders: true,
            //multiSelect: true,
            //singleExpand: true,
            root: {
                text: "所有标签分类",
                expanded: true
            },
            //columns: [
            //    {
            //        xtype: 'treecolumn',
            //        text: '名称',
            //        flex: 2,
            //        sortable: true,
            //        dataIndex: 'text'
            //    }, {
            //        text: '主键',
            //        flex: 0,
            //        dataIndex: 'PhId',
            //        hidden: true,
            //        hideable: false
            //    }
            //],
            listeners: {
                'selectionchange': function (selModel, rcds, eOpts) {
                    var PhIdKind = rcds[0].get("PhId");
                    Ext.apply(store.proxy.extraParams, { 'PhIdKind': PhIdKind });
                    store.loadPage(1);
                }
            }
        });


        var tabPanel = Ext.create('Ext.TabPanel', {
            id: 'tabs',
            activeTab: 0,
            enableTabScroll: true,
            region: 'west',
            width: 180,
            items: [
                {
                    title: '标签分类树',
                    id: 'protab',
                    items: tagskindTree,
                    layout: 'border'
                }
            ]
        });
        //定义模型
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'Cname',
                    type: 'string',
                    mapping: 'Cname'
                }, {
                    name: 'Useflg',
                    type: 'int',
                    mapping: 'Useflg'
                }, {
                    name: 'NgInsertDt',
                    type: 'date',
                    mapping: 'NgInsertDt'
                }, {
                    name: 'NgUpdateDt',
                    type: 'date',
                    mapping: 'NgUpdateDt'
                }, {
                    name: 'NgRecordVer',
                    type: 'int',
                    mapping: 'NgRecordVer'
                }, {
                    name: 'Creator',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'Creator'
                }, {
                    name: 'Editor',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'Editor'
                }, {
                    name: 'CurOrgId',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'CurOrgId'
                }, {
                    name: 'Remark',
                    type: 'string',
                    mapping: 'Remark'
                }, {
                    name: 'Attribute',
                    type: 'string',
                    mapping: 'Attribute'
                }, {
                    name: 'PhidTagskind',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhidTagskind'
                }, {
                    name: 'PhidTagskind_EXName',
                    type: 'string',
                    mapping: 'PhidTagskind_EXName'
                }, {
                    name: 'Cstatus',
                    type: 'string',
                    mapping: 'Cstatus'
                }, {
                    name: 'PhIdRelationtags',
                    type: 'string',
                    mapping: 'PhIdRelationtags'
                }
            ]
        });

        store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 25,
            autoLoad: true,
            url: C_ROOT + 'WM/Doc/Tags/GetTagsList'
        });
        store.on('beforeload', function (store) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (me.help.outFilter2) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.help.outFilter2) });
            }
            if (me.help.likeFilter) {
                Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.help.likeFilter) });
            }
            if (me.help.leftLikeFilter) {
                Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.help.leftLikeFilter) });
            }
            if (me.help.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.help.clientSqlFilter });
            }

        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var tagsgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,    
            id: 'tagsgrid',
            stateful: true,
            stateId: 'nggrid',
            store: store,
            buskey: 'PhId', //对应的业务表主键属性               
            columnLines: true,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '标签名',
                    dataIndex: 'Cname',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '所属分类',
                    dataIndex: 'PhidTagskind_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '备注',
                    dataIndex: 'Remark',
                    width: 150,
                    sortable: false,
                    hidden: false
                }
            ],
            bbar: pagingbar
        });
        me.items = [
            toolbar, {
                xtype: 'panel',
                region: 'center',
                autoScroll: false,
                layout: 'border',
                border: 0,
                items: [tabPanel, tagsgrid]
            }
        ];
        me.buttons = [
            {
                text: '添加',
                handler: function () {
                    $OpenTab('标签-新增', C_ROOT + '/WM/Doc/Tags/TagsEdit?otype=add');

                }
            },
            '->',
            {
                text: '确定', handler: function () {
                    if (me.callback != null) {
                        me.gridDbClick(me.help, tagsgrid, this);
                    } else {

                        Ext.MessageBox.alert('', '请选择数据.');
                        return;
                    }
                }
            },
            { text: '取消', handler: function () { me.close(); } }
        ];
        me.callParent();

    },
    gridDbClick: function (help, grid, win) {
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var code = data[0].get(help.valueField);
            var name = data[0].get(help.displayField);
            for (var i = 1; i < data.length; i++) {
                code += "," + data[i].get(help.valueField);
                name += "," + data[i].get(help.displayField);
            }
            //if (!code) {
            //    var obj = data[0].data;
            //    //容错处理，model的字段有可能带表名获取不到值
            //    for (var p in obj) {

            //        var field = [];
            //        if (p.indexOf('.') > 0) {
            //            field = p.split('.');
            //        }

            //        if (field[1] === help.valueField) {
            //            code = obj[p];
            //        }
            //        if (field[1] === help.displayField) {
            //            name = obj[p];
            //        }

            //    }
            // }

            var obj = new Object();
            obj[help.valueField] = code;

            if (help.displayFormat) {
                obj[help.displayField] = Ext.String.format(help.displayFormat, code, name);
            } else {
                obj[help.displayField] = name;
            }

            Ext.define('richhelpModel', {
                extend: 'Ext.data.Model',
                fields: [{
                    name: help.valueField,
                    type: 'string',
                    mapping: help.valueField
                }, {
                    name: help.displayField,
                    type: 'string',
                    mapping: help.displayField
                }
                ]
            });

            var valuepair = Ext.create('richhelpModel', obj);
            help.setValue(valuepair); //必须这么设置才能成功
            //            help.setHiddenValue(code);
            //            help.setRawValue(name);
            this.hide();
            this.destroy();
            //if (me.isInGrid) {

            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = data[0].data;
            help.fireEvent('helpselected', pobj);
            //}
        }
    },
    getData: function () {
        var code = "";
        var name = "";
        var pobj = new Object();
        var df = "Cname"//this.displayField;
        var vf = "PhId"//this.valueField;
        var select = this.queryById("grid").store.data.items;

        if (select.length <= 0) {
            return null;
        }
        var dataarray = [];
        Ext.Array.each(select, function (model) {
            code = code + model.data[vf] + ",";
            name = name + model.data[df] + ",";
            dataarray.push(model.data);
        });
        code = code.substring(0, code.length - 1);
        name = name.substring(0, name.length - 1);

        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = dataarray;
        return pobj;
    }
});
Ext.define('Ext.ng.TagsHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngTagsHelp'],
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    selectMode: 'Single',
    multiSelect: false,
    needBlankLine: false,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    seletArr: null,
    valueField: "PhId",
    displayField: "Cname",
    userCodeField: "PhId",
    helpid: "fg3_tags",
    initComponent: function () {
        var me = this;

        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected');
        me.addEvents('beforetriggerclick');
        //me.getFirstRowData();
    },
    onTriggerClick: function () {
        var me = this;
        var win = me.createHelpWindow();
        win.help = me;

        me.fireEvent('beforetriggerclick', me);

        if (!Ext.isEmpty(me.rawValue) && me.value != null) {
            var arr = me.seletArr;
            Ext.Array.each(arr, function (model) {
                //var selectstore = win.queryById("weststore").store;
                //selectstore.add(model);
            });
        }
        //有数值时的初始化
        win.callback = function (grid) {
            var code = "";
            var name = "";
            var pobj = new Object();
            var df = me.displayField;
            var vf = me.valueField;
            //var select1 = win.queryById("westgrid").store.data.items;
            var select = grid.getSelectionModel().getSelection();
            if (select.length <= 0) {
                Ext.MessageBox.alert('', '请选择数据.');
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

            var valuepair = Ext.create('richhelpModel', obj);
            me.setValue(valuepair); //必须这么设置才能成功

            win.hide();
            win.destroy();

            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = select;
            me.seletArr = select;
            me.fireEvent('helpselected', pobj);
        };
        win.show();
    },
    createHelpWindow: function () {
        /////创建帮助弹窗
        var win = Ext.create("Ext.ng.TagsHelpWindow");
        return win;
    },
    showHelp: function () {
        this.onTriggerClick();
    }
});


Ext.define('Ext.ng.MultiTagsHelpWindow', {
    extend: 'Ext.window.Window',
    title: '标签帮助',
    closable: true,
    resizable: false,
    modal: true,
    height: 550,
    width: 800,
    border: 0,
    layout: 'border',
    constrain: true,
    help: "",
    //  helptype: "pcid_help",
    ORMMode: true,
    callback: null,
    //   dataUrl: 'PMS/PC/ProjectTable/GetProjectTableHelp',
    //   useDataUrl: 'PMS/PC/ProjectTable/GetCommonUseProjectTable',
    initComponent: function () {
        var me = this;

        var toolbar = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            defaults: {
                margin: '0 5 0 5'
            },
            layout: {
                type: 'hbox'
            },
            items: [
                { xtype: 'textfield', width: 300, itemId: 'keyword' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '搜索',
                    handler: function () {
                        var cname = toolbar.queryById('keyword').getValue();
                        Ext.apply(weststore.proxy.extraParams, { 'searchkey': cname });
                        weststore.load();
                    }
                }
            ]
        });
        Ext.define('tagskindTree', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'PhId', type: 'string' },
                { name: 'text', type: 'string' },
                { name: 'CNo', type: 'string' },
                { name: 'CName', type: 'string' }
            ]
        });
        var tagskindTreeStore = Ext.create('Ext.data.TreeStore', {
            model: 'tagskindTree',
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'WM/Doc/TagsKind/GetTagsKindTreeNodes'
            }
        });
        //标签
        var tagskindTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            loaded: true,
            store: tagskindTreeStore,
            //hideHeaders: true,
            //multiSelect: true,
            //singleExpand: true,
            root: {
                text: "所有标签分类",
                expanded: true
            },
            //columns: [
            //    {
            //        xtype: 'treecolumn',
            //        text: '名称',
            //        flex: 2,
            //        sortable: true,
            //        dataIndex: 'text'
            //    }, {
            //        text: '主键',
            //        flex: 0,
            //        dataIndex: 'PhId',
            //        hidden: true,
            //        hideable: false
            //    }
            //],
            listeners: {
                'selectionchange': function (selModel, rcds, eOpts) {
                    var PhIdKind = rcds[0].get("PhId");
                    Ext.apply(weststore.proxy.extraParams, { 'PhIdKind': PhIdKind });
                    weststore.loadPage(1);
                }
            }
        });
        var tabPanel = Ext.create('Ext.TabPanel', {
            id: 'tabs',
            activeTab: 0,
            enableTabScroll: true,
            region: 'west',
            width: 180,
            items: [
                {
                    title: '标签分类树',
                    id: 'protab',
                    items: tagskindTree,
                    layout: 'border'
                }
            ]
        });
        //定义模型
        Ext.define('tagsmodel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'Cname',
                    type: 'string',
                    mapping: 'Cname'
                }, {
                    name: 'Useflg',
                    type: 'int',
                    mapping: 'Useflg'
                }, {
                    name: 'NgInsertDt',
                    type: 'date',
                    mapping: 'NgInsertDt'
                }, {
                    name: 'NgUpdateDt',
                    type: 'date',
                    mapping: 'NgUpdateDt'
                }, {
                    name: 'NgRecordVer',
                    type: 'int',
                    mapping: 'NgRecordVer'
                }, {
                    name: 'Creator',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'Creator'
                }, {
                    name: 'Editor',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'Editor'
                }, {
                    name: 'CurOrgId',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'CurOrgId'
                }, {
                    name: 'Remark',
                    type: 'string',
                    mapping: 'Remark'
                }, {
                    name: 'Attribute',
                    type: 'string',
                    mapping: 'Attribute'
                }, {
                    name: 'PhidTagskind',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhidTagskind'
                }, {
                    name: 'PhidTagskind_EXName',
                    type: 'string',
                    mapping: 'PhidTagskind_EXName'
                }, {
                    name: 'Cstatus',
                    type: 'string',
                    mapping: 'Cstatus'
                }, {
                    name: 'PhIdRelationtags',
                    type: 'string',
                    mapping: 'PhIdRelationtags'
                }
            ]
        });
        ////列表加载前事件
        //weststore.on('beforeload', function (me, operation, eOpts) {
        //    //var form = queryPanel.getForm();
        //    //var data = form.getValues();
        //    //Ext.apply(queryfilter, data);
        //    //store.proxy.extraParams = { 'queryfilter': Ext.encode(queryfilter) };
        //    Ext.apply(weststore.proxy.extraParams, { 'queryfilter': Ext.encode(queryfilter) });
        //});
        var weststore = Ext.create('Ext.ng.JsonStore', {
            model: 'tagsmodel',
            pageSize: 25,
            autoLoad: true,
            url: C_ROOT + 'WM/Doc/Tags/GetTagsList'
        });
        weststore.on('beforeload', function (store) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (me.help.outFilter2) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.help.outFilter2) });
            }
            if (me.help.likeFilter) {
                Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.help.likeFilter) });
            }
            if (me.help.leftLikeFilter) {
                Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.help.leftLikeFilter) });
            }
            if (me.help.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.help.clientSqlFilter });
            }

        });
        var eaststore = Ext.create('Ext.ng.JsonStore', {
            id: 'eaststore',
            model: 'tagsmodel'
        });

        var westgridpanel = Ext.create('Ext.ng.GridPanel', {
            id: 'westgrid',
            region: 'west',
            autoScroll: true,
            frame: true,
            columnLines: true,
            overflowX: 'scroll',
            border: 0,
            width: 250,
            height: 400,
            store: weststore,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '标签名',
                    dataIndex: 'Cname',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '所属分类',
                    dataIndex: 'PhidTagskind_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '备注',
                    dataIndex: 'Remark',
                    width: 150,
                    sortable: false,
                    hidden: false
                }
            ]
        });

        var eastgridpanel = Ext.create('Ext.ng.GridPanel', {
            id: 'eastgrid',
            region: 'east',
            overflowX: 'scroll',
            columnLines: true,
            autoScroll: true,
            border: 0,
            width: 250,
            height: 400,
            stateful: true,
            store: eaststore,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '标签名',
                    dataIndex: 'Cname',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '所属分类',
                    dataIndex: 'PhidTagskind_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '备注',
                    dataIndex: 'Remark',
                    width: 150,
                    sortable: false,
                    hidden: false
                }
            ]
        });

        var choosebuttonpanel = Ext.create('Ext.form.Panel', {
            region: 'center',
            layout: 'border',
            width: 50,
            split: true,
            autoScroll: true,
            frame: true,
            border: 0,
            columnsPerRow: 4,
            fieldDefaults: {
                labelWidth: 60,
                anchor: '60%',
                margin: '0 10 5 0',
                msgTarget: 'side'
            },
            items: [
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'rightmove',
                    text: '>',
                    margin: '50 10 0 10',
                    handler: function () {
                        var selected = westgridpanel.getSelectionModel().getSelection();
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                var index = eaststore.find("PhId", model.data.PhId);
                                if (index < 0) {
                                    eaststore.add(model);
                                }
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'rightmoveall',
                    text: '>>',
                    margin: '20 10 0 10',
                    height: 20,
                    handler: function () {
                        var btn = this;
                        var selected = westgridpanel.store.data.items;
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                var index = eaststore.find("PhId", model.data.PhId);
                                if (index < 0) {
                                    eaststore.add(model);
                                }
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'leftmove',
                    text: '<',
                    margin: '50 10 0 10',
                    height: 20,
                    handler: function () {
                        var selected = eastgridpanel.getSelectionModel().getSelection();
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                eaststore.remove(model);
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'leftmoveall',
                    text: '<<',
                    margin: '20 10 0 10',
                    height: 20,
                    handler: function () {
                        eaststore.removeAll();
                    }
                }
            ]

        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: weststore
        });

        var datapanel = Ext.create('Ext.panel.Panel', {
            region: 'center',
            autoScroll: false,
            layout: 'border',
            border: 0,
            height: 400,
            width: 600,
            frame: false,
            items: [westgridpanel, choosebuttonpanel, eastgridpanel],
            bbar: pagingbar
        });

        var commonuseweststore = Ext.create('Ext.ng.JsonStore', {
            id: 'commonuseweststore',
            url: C_ROOT + 'WM/Doc/Tags/GetTagsList',
            model: 'tagsmodel'
        });

        //只能在这里写事件才能触发到
        commonuseweststore.on('beforeload', function (store) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (me.help.outFilter2) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.help.outFilter2) });
            }
            if (me.help.likeFilter) {
                Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.help.likeFilter) });
            }
            if (me.help.leftLikeFilter) {
                Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.help.leftLikeFilter) });
            }
            if (me.help.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.help.clientSqlFilter });
            }

        });

        var commonuseeaststore = Ext.create('Ext.ng.JsonStore', {
            id: 'commonuseeaststore',
            model: 'tagsmodel'
        });

        var commonusewest = Ext.create('Ext.ng.GridPanel', {
            id: 'uswest',
            region: 'west',
            autoScroll: true,
            frame: true,
            border: 0,
            columnLines: true,
            overflowX: 'scroll',
            width: 400,
            height: 400,
            store: commonuseweststore,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '标签名',
                    dataIndex: 'Cname',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '所属分类',
                    dataIndex: 'PhidTagskind_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '备注',
                    dataIndex: 'Remark',
                    width: 150,
                    sortable: false,
                    hidden: false
                }
            ]
        });

        var commonuseeast = Ext.create('Ext.ng.GridPanel', {
            id: 'useast',
            region: 'east',
            overflowX: 'scroll',
            border: 0,
            columnLines: true,
            autoScroll: true,
            width: 320,
            height: 400,
            stateful: true,
            store: commonuseeaststore,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '标签名',
                    dataIndex: 'Cname',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '所属分类',
                    dataIndex: 'PhidTagskind_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: '备注',
                    dataIndex: 'Remark',
                    width: 150,
                    sortable: false,
                    hidden: false
                }
            ]
        });

        var commonusechoosebutton = Ext.create('Ext.form.Panel', {
            region: 'center',
            layout: 'border',
            width: 100,
            split: true,
            autoScroll: true,
            frame: true,
            border: 0,
            columnsPerRow: 4,
            fieldDefaults: {
                labelWidth: 60,
                anchor: '60%',
                margin: '0 10 5 0',
                msgTarget: 'side'
            },
            items: [
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'usrightmove',
                    text: '>',
                    margin: '50 10 0 10',
                    handler: function () {
                        var selected = commonusewest.getSelectionModel().getSelection();
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                var index = commonuseeaststore.find("PhId", model.data.PhId);
                                if (index < 0) {
                                    commonuseeaststore.add(model);
                                }
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'usrightmoveall',
                    text: '>>',
                    margin: '20 10 0 10',
                    height: 20,
                    handler: function () {
                        var btn = this;
                        var selected = commonusewest.store.data.items;
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                var index = commonuseeaststore.find("PhId", model.data.PhId);
                                if (index < 0) {
                                    commonuseeaststore.add(model);
                                }
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'usleftmove',
                    text: '<',
                    margin: '50 10 0 10',
                    height: 20,
                    handler: function () {
                        var selected = commonuseeast.getSelectionModel().getSelection();
                        if (selected.length > 0) {
                            Ext.Array.each(selected, function (model) {
                                commonuseeaststore.remove(model);
                            });
                        }
                    }
                },
                {
                    region: 'north',
                    xtype: 'button',
                    id: 'usleftmoveall',
                    text: '<<',
                    margin: '20 10 0 10',
                    height: 20,
                    handler: function () {
                        commonuseeaststore.removeAll();
                    }
                }
            ]

        });

        var tabs = Ext.create('Ext.TabPanel', {
            activeTab: 0,
            enableTabScroll: true,
            region: 'center',
            border: 0,
            width: 800,
            height: 400,
            items: [
                {
                    title: '所有数据',
                    id: 'taball',
                    layout: 'border',
                    items: [
                        {
                            xtype: 'panel',
                            region: 'west',
                            autoScroll: false,
                            layout: 'border',
                            height: 400,
                            width: 200,
                            items: [tabPanel]
                        }, datapanel
                    ]
                }
                //}, {
                //    title: '常用数据',
                //    id: 'tabcommonuse',
                //    layout: 'border',
                //    items: [commonusewest, commonuseeast, commonusechoosebutton]
                //}
            ]
        });

        me.items = [toolbar, {
            xtype: 'panel',
            region: 'center',
            autoScroll: false,
            layout: 'border',
            border: 0,
            items: [tabs]
        }];
        me.buttons = [
            {
                text: '新增',
                handler: function () {
                    Ext.Ajax.request({
                        url: C_ROOT + '/WM/Doc/Tags/HasRight',
                        success: function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp.Status === "HasRight") {
                                $OpenTab('标签-新增', C_ROOT + '/WM/Doc/Tags/TagsEdit?otype=add');

                            } else {
                                Ext.MessageBox.alert('提示', resp.Msg);
                            }
                        }
                    });


                }
            },
            '->',
            {
                text: '确定', handler: function () {
                    //if (me.callback != null) {
                    me.callback("eastgrid");
                    //} else {

                    //        Ext.MessageBox.alert('', '请选择数据.');
                    //        return;
                    //}
                }
            },
            { text: '取消', handler: function () { me.close(); } }
        ];
        me.callParent();

        tabs.on('tabchange', function (tabpanel, nCard, oCard, eOpts) {
            if (nCard.id == 'tabcommonuse') {
                commonuseweststore.load();
                //commlistLoaded = true;
            }
        });

    },
    addCommonUseData: function (me, grid, commonUseStore) {
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var code = data[0].get("PhId");
            var index = commonUseStore.find("PhId", code); //去重
            if (index < 0) {
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/RichHelp/SaveCommonUseData',
                    params: { 'helpid': me.helptype, 'codeValue': code },
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                            commonUseStore.insert(commonUseStore.count(), data[0].data);
                        } else {
                            Ext.MessageBox.alert('保存失败', resp.status);
                        }
                    }
                });
            }
        }
    },
    delCommonUseData: function (me, commonUseGrid, commonUseStore) {
        var data = commonUseGrid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var code = data[0].get("PhId");
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/DeleteCommonUseData',
                params: { 'helpid': me.helptype, 'codeValue': code },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        commonUseStore.remove(data[0]); //移除
                    } else {
                        Ext.MessageBox.alert('删除失败!', resp.status);
                    }
                }
            });
        }
    },
    getData: function () {
        var code = "";
        var name = "";
        var pobj = new Object();
        var df = "Cname"//this.displayField;
        var vf = "PhId"//this.valueField;
        var select = this.queryById("eastgrid").store.data.items;

        if (select.length <= 0) {
            return null;
        }
        var dataarray = [];
        Ext.Array.each(select, function (model) {
            code = code + model.data[vf] + ",";
            name = name + model.data[df] + ",";
            dataarray.push(model.data);
        });
        code = code.substring(0, code.length - 1);
        name = name.substring(0, name.length - 1);

        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = dataarray;
        return pobj;
    }
});

Ext.define('Ext.ng.MultiTagsHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngMultiTagsHelp'],
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    selectMode: 'Multi',
    needBlankLine: false,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    seletArr: null,
    valueField: "PhId",
    displayField: "Cname",
    helpid: "fg3_tags",
    initComponent: function () {
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected');
        me.addEvents('beforetriggerclick');

        //多选不需做 实例而已
        /*me.on('render', function (combo, eOpts) {
            var input = this.el.down('input');
            if (input) {
                input.dom.ondblclick = function () {
                    var codeVal;
                    if (!Ext.isEmpty(me.getValue())) {
                        if (me.isInGrid) {
                            var grid;
                            if (me.grid) {
                                grid = me.grid;
                            } else if (me.gridID) {
                                grid = Ext.getCmp(me.gridID);
                            }

                            var data = grid.getSelectionModel().getSelection();
                            codeVal = data[0].get(me.codeColumn);
                        }
                        else {
                            codeVal = me.getValue();
                        }
                        alert(codeVal);
                        $OpenTab('供应商-查看', C_ROOT + 'DMC/Enterprise/SupplyFile/SupplyFileEdit?otype=view&id=' + codeVal);
                    }
                };
            }
        });*/
    },
    onTriggerClick: function () {
        var me = this;
        var win = me.createHelpWindow();
        var ids;
        win.help = me;
        me.fireEvent('beforetriggerclick', me);
        if (!Ext.isEmpty(me.rawValue) && me.value != null) {
            var arr = me.seletArr;
            Ext.Array.each(arr, function (model) {
                var selectstore = win.queryById("eastgrid").store;
                selectstore.add(model);
            });
            if (arr == null) {    //修改的时候在右框中出现数据
                if (me.selectedValue == null) {
                    ids = me.value;
                } else {
                    ids = me.selectedValue;
                }
                Ext.Ajax.request({
                    params: { 'ids': ids },
                    url: C_ROOT + 'WM/Doc/Tags/GetTagsByIds',
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        Ext.Array.each(resp.Record, function (model) {
                            var selectstore = win.queryById("eastgrid").store;
                            selectstore.add(model);
                        });
                    }
                });
            }
        }
        //有数值时的初始化
        win.callback = function (gridname) {
            var code = "";
            var name = "";
            var pobj = new Object();
            var df = me.displayField;
            var vf = me.valueField;
            var select = win.queryById(gridname).store.data.items;
            //if (select.length <= 0) {
            //    Ext.MessageBox.alert('', '请选择数据.');
            //    return;
            //}
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

            var valuepair = Ext.create('richhelpModel', obj);
            me.setValue(valuepair); //必须这么设置才能成功

            win.hide();
            win.destroy();

            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = select;
            me.seletArr = select;
            me.fireEvent('helpselected', pobj);
        };
        win.show();
    },
    createHelpWindow: function () {
        /////创建帮助弹窗
        var win = Ext.create("Ext.ng.MultiTagsHelpWindow");
        return win;
    },
    showHelp: function () {
        this.onTriggerClick();
    }
});


Ext.define('Ext.ng.SignetInfoHelp', {
    extend: 'Ext.window.Window',
    title: '印章信息帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    sqlwhere: "",
    width: 600,
    height: 400,
    layout: 'fit',
    items: [],
    initComponent: function () {
        var me = this; me.initParam();
        var sql = "";//当选择的是外借界面的印章使用，只能选择外借选择的印章
        var upphid = "";//当选择外借新增使用单，只能选择外借的印章
        if (me.isApplyForLend) {
            sql = "&ApplyIn=" + me.sqlForApply;
        } else {
            sql = "";
        }
        if (me.org != "") {
            sql = "&orgid=" + me.org;
        }
        if (me.isOutApply) {
            if (me.upPhId != "") {
                me.sqlForApply = "-1";
                Ext.Ajax.request({
                    params: { 'upphid': me.upPhId },
                    url: C_ROOT + 'WM/Signet/ESAppdtl/FindByUpPhId',
                    async: false, //同步请求
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        for (var i = 0; i < resp.Record.length; i++) {
                            me.sqlForApply += "," + resp.Record[i].PhidEsType;
                        }
                        sql = "&ApplyIn=" + me.sqlForApply;
                    }
                });
            }
        }
        me.addEvents('bntOk'); //定义值被选完的事件  
        Ext.define('SignetKindTree', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'PhId', type: 'string' },
                { name: 'text', type: 'string' },
                { name: 'CNo', type: 'string' },
                { name: 'CName', type: 'string' },
                { name: 'Path', type: 'string' }
            ]
        });
        var signetkindTreeStore = Ext.create('Ext.data.TreeStore', {
            model: 'SignetKindTree',
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'WM/Signet/EsKind/GetSignetKindTreeNodes'
            }
        });

        //印章类型
        var signetkindTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            loaded: true,
            store: signetkindTreeStore,
            //hideHeaders: true,
            //multiSelect: true,
            //singleExpand: true,
            root: {
                text: "所有印章类型",
                //   expanded: true
            },
            //columns: [
            //    {
            //        xtype: 'treecolumn',
            //        text: '名称',
            //        flex: 2,
            //        sortable: true,
            //        dataIndex: 'text'
            //    }, {
            //        text: '主键',
            //        flex: 0,
            //        dataIndex: 'PhId',
            //        hidden: true,
            //        hideable: false
            //    }
            //],
            listeners: {
                'selectionchange': function (selModel, rcds, eOpts) {
                    var path = rcds[0].get("Path");
                    Ext.apply(store.proxy.extraParams, { 'Path': path });
                    store.loadPage(1);
                }
            }
        });
        var tabPanel = Ext.create('Ext.TabPanel', {
            id: 'tabs',
            activeTab: 0,
            enableTabScroll: true,
            region: 'west',
            width: 180,
            items: [
                {
                    title: '类型树',
                    id: 'protab',
                    items: signetkindTree,
                    layout: 'border'
                }
            ]
        });
        //定义模型
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'CCode',
                    type: 'string',
                    mapping: 'CCode'
                }, {
                    name: 'CNo',
                    type: 'string',
                    mapping: 'CNo'
                }, {
                    name: 'CName',
                    type: 'string',
                    mapping: 'CName'
                }, {
                    name: 'PhIdOwnOrg',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdOwnOrg'
                }, {
                    name: 'PhIdOwnOrg_EXName',
                    type: 'string',
                    mapping: 'PhIdOwnOrg_EXName'
                }, {
                    name: 'Remarks',
                    type: 'string',
                    mapping: 'Remarks'
                }, {
                    name: 'PhIdFillOrg',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdFillOrg'
                }, {
                    name: 'PhIdFillOrg_EXName',
                    type: 'string',
                    mapping: 'PhIdFillOrg_EXName'
                }, {
                    name: 'PhIdFillEmp',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdFillEmp'
                }, {
                    name: 'PhIdFillEmp_EXName',
                    type: 'string',
                    mapping: 'PhIdFillEmp_EXName'
                }, {
                    name: 'FillDt',
                    type: 'date',
                    mapping: 'FillDt'
                }, {
                    name: 'CStatus',
                    type: 'string',
                    mapping: 'CStatus'
                }, {
                    name: 'Photo',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'Photo'
                }, {
                    name: 'PicPath',
                    type: 'string',
                    mapping: 'PicPath'
                }, {
                    name: 'PhIdOwnEmp',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdOwnEmp'
                }, {
                    name: 'PhIdOwnEmp_EXName',
                    type: 'string',
                    mapping: 'PhIdOwnEmp_EXName'
                }, {
                    name: 'PhIdType',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdType'
                }, {
                    name: 'PhIdType_EXName',
                    type: 'string',
                    mapping: 'PhIdType_EXName'
                }, {
                    name: 'PhIdEngrave',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdEngrave'
                }, {
                    name: 'PhIdEngrave_EXName',
                    type: 'string',
                    mapping: 'PhIdEngrave_EXName'
                }, {
                    name: 'PhIdMac',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhIdMac'
                }, {
                    name: 'MacSite',
                    type: 'string',
                    mapping: 'MacSite'
                }, {
                    name: 'MacStatus',
                    type: 'string',
                    mapping: 'MacStatus'
                }
            ]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 25,
            autoLoad: false,
            url: '../EsType/GetEsTypeList?CStatus=0' + sql
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            stateful: true,
            stateId: 'nggrid',
            store: store,
            buskey: 'PhId', //对应的业务表主键属性               
            columnLines: true,
            columns: [{
                xtype: 'rownumberer',
                text: '行号',
                stateId: 'grid_lineid',
                width: 35
            },
            {
                header: Lang.PicPath || '图片',
                dataIndex: 'PicPath',
                width: 100,
                sortable: false,
                hidden: false,
                renderer: function (v) {
                    if (v != "" && v != null) {
                        return "<div align='center'><img src='" + C_ROOT + v + "' width='20px' height='15px' /></div>";
                    } else {
                        return "";
                    }
                }
            }, {
                header: Lang.CName || '印章名称',
                dataIndex: 'CName',
                width: 100,
                sortable: false,
                hidden: false
            }, {
                header: Lang.PhIdType || '印章类型',
                dataIndex: 'PhIdType_EXName',
                width: 100,
                sortable: false,
                hidden: false
            }, {
                header: Lang.PhIdType || '印章类型',
                dataIndex: 'PhIdType',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.CStatus || '状态',
                dataIndex: 'CStatus',
                width: 100,
                sortable: false,
                hidden: true,
                renderer: function (val, cell) {
                    switch (val.toString()) {
                        case "0": return "可用"; break;
                        case "1": return "已借出"; break;
                        case "2": return "已销毁"; break;
                        case "3": return "销毁中"; break;
                        case "4": return "新增"; break;
                        case "5": return "审批中"; break;
                        case "6": return "封存中"; break;
                        case "7": return "已封存"; break;
                        case "8": return "启用中"; break;
                        default: return "新增"; break;
                    }
                }
            }, {
                header: Lang.PhIdOwnEmp || '保管人',
                dataIndex: 'PhIdOwnEmp',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdOwnEmp || '保管人',
                dataIndex: 'PhIdOwnEmp_EXName',
                width: 100,
                sortable: false,
                hidden: false
            }, {
                header: Lang.PhIdOwnOrg || '保管单位',
                dataIndex: 'PhIdOwnOrg',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdOwnOrg || '保管单位',
                dataIndex: 'PhIdOwnOrg_EXName',
                width: 100,
                sortable: false,
                hidden: false
            }, {
                header: Lang.Remarks || '当前借用人',
                dataIndex: 'LendEpm',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.Remarks || '借用时间',
                dataIndex: 'LendDate',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.Remarks || '备注',
                dataIndex: 'Remarks',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhId || '物理主键',
                dataIndex: 'PhId',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.CCode || '系统编号',
                dataIndex: 'CCode',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.CNo || '企业印章编号',
                dataIndex: 'CNo',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdFillOrg || '录入组织',
                dataIndex: 'PhIdFillOrg',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdFillEmp || '录入人',
                dataIndex: 'PhIdFillEmp',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.FillDt || '录入时间',
                dataIndex: 'FillDt',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdEngrave || '印章刻制的编号',
                dataIndex: 'PhIdEngrave',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.PhIdMac || '印控仪编号',
                dataIndex: 'PhIdMac',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.MacSite || '所处位置',
                dataIndex: 'MacSite',
                width: 100,
                sortable: false,
                hidden: true
            }, {
                header: Lang.MacStatus || '印章使用状态',
                dataIndex: 'MacStatus',
                width: 100,
                sortable: false,
                hidden: true
            }
            ],
            bbar: pagingbar
        });
        grid.on('celldblclick', function (view, td, cellIndex, record, tr, rowIndex, e, eOpts) {
            var data = grid.getSelectionModel().getSelection();
            me.fireEvent('bntOk', data);
            me.close();
        });
        function CloseWindow() {
            var currentWin = window;
            while (top != currentWin) {
                var parentExt = currentWin.parent.Ext;
                var fElement = parentExt.get(currentWin.frameElement);
                var windowElement = fElement.up('div.x-window');
                if (windowElement) {
                    var winId = windowElement.id;
                    var extWin = parentExt.getCmp(winId);
                    extWin.close();
                    return true;
                } else {
                    currentWin = currentWin.parent;
                }
            }
            alert("窗口关闭失败！");
            return false;
        }

        var toolbar = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            defaults: {
                margin: '0 5 0 5'
            },
            layout: {
                type: 'hbox'
            },
            items: [
                { xtype: 'label', text: '印章名称：' },
                { xtype: 'textfield', width: 150, itemId: 'keyword' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '搜索',
                    handler: function () {
                        var CName = toolbar.queryById('keyword').getValue();
                        Ext.apply(store.proxy.extraParams, { 'CName': CName });
                        store.load();
                    }
                }
            ]
        });
        //布局
        var viewport = Ext.create('Ext.panel.Panel', {
            id: "viewPort",
            layout: 'border',
            items: [
                {
                    id: 'myPanel',
                    xtype: 'panel',
                    region: 'center',
                    autoScroll: false,
                    overflowY: 'scroll',
                    layout: 'border',
                    border: true,
                    items: [toolbar, tabPanel, grid]
                }
            ]
        });
        me.items = viewport;
        me.buttons = [{
            text: '确认',
            handler: function () {
                var data = grid.getSelectionModel().getSelection();
                if (data.toString() != '') //  me.parent.Ext.getCmp('signetinfohelp').fireEvent('bntOk', data);
                {
                    me.fireEvent('bntOk', data);
                    me.close();
                }
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];
        store.load();
        me.callParent();
    },
    initParam: function () {
        var me = this;
        me.memory = {};
        me.gridConfig = {};
        me.helpType = "signetInfoHelp";
        me.gridConfig.border = true;
        me.gridConfig.region = 'center';
    },
    listeners: {

    }
});

Ext.define('Ext.wm.doc.TabPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.wmDocTabPanel',
    minHeight: 150,
    minWidth: 200,
    title: '企业文档',
    tabs: [],
    urls: [],
    tpls: [],
    handleCreate: undefined,
    handleMore: undefined,
    tabChanged: undefined,
    buttonName: '创建',
    toolbarId: 'wmdoctool',
    initComponent: function () {
        var me = this;
        me.tools = [{
            id: me.toolbarId,
            xtype: 'toolbar',
            ui: '',
            border: false,
            items: [{
                xtype: 'button',
                text: me.buttonName,
                handler: function () {
                    if (me.handleCreate == undefined)
                        $OpenTab('文档-录入', C_ROOT + 'WM/Doc/Document/DocumentEdit?otype=add');
                    else if (typeof me.handleCreate == 'function') {
                        me.handleCreate(me.activeTab);
                    }
                }
            }, '-', {
                xtype: 'button',
                text: '更多',
                handler: function () {
                    if (me.handleMore == undefined)
                        $OpenTab('文档列表', C_ROOT + 'WM/Doc/Document/DocumentList');
                    else if (typeof me.handleMore == "function") {
                        me.handleMore(me.activeTab);
                    }
                }
            }]
        }];
        me.defaults = { bodyPadding: 5 },
            me.items = function () {
                var items = [];
                Ext.Array.each(me.tabs, function (tabobj, index) {
                    var obj = {};
                    obj.itemId = tabobj.id;
                    obj.title = tabobj.name;
                    obj.tpl = me.tpls[index];
                    obj.url = me.urls[index];
                    obj.autoScroll = true;
                    items.push(obj);
                });
                return items;
            }();

        me.callParent(arguments);
        me.InitData();
        me.on('tabchange', function (tabPanel, newCard, oldCard, eOpts) {
            me.InitData(newCard);
            if (typeof me.tabChanged == 'function')
                me.tabChanged(newCard);
        });
    },
    hasLoaded: function (tabObj) {
        var me = this;
        if (tabObj == null || tabObj == undefined) {
            tabObj = me.activeTab;
        }
        var tmpData = tabObj.data;
        if (tmpData == undefined)
            return false;
        else
            return true;
    },
    RefreshData: function (tabObj) {
        var me = this;
        if (tabObj == null || tabObj == undefined) {
            tabObj = me.activeTab;
        }
        if (tabObj.url == undefined || tabObj.url == "")
            return;
        Ext.Ajax.request({
            //params: { 'attachguid': me.returnObj.guid, 'buscode': buscode },
            url: tabObj.url,
            tab: tabObj,
            success: function (response, opts) {
                var me = this;
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Record != null && resp.Record != undefined) {
                    opts.tab.update(resp.Record);
                } else {
                    Ext.MessageBox.alert('获取数据失败:' + resp.msg);
                }
            }
        });
    },
    InitData: function (tabObj) {
        var me = this;
        if (tabObj == null || tabObj == undefined) {
            tabObj = me.activeTab;
        }
        if (!me.hasLoaded(tabObj)) {
            me.RefreshData(tabObj);
        }
    }
});
Ext.define('Ext.ng.GRichHelp', {
    extend: 'Ext.ng.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngGRichHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    ORMMode: true,
    selectMode: 'Single', //multiple  
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    queryMode: 'remote',
    triggerAction: 'all', //'query'
    selectQueryProIndex: 0,
    initComponent: function () {
        //
        var me = this;


        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.helpType = 'RichHelp_' + me.helpid;
        me.bussType = me.bussType || 'all';

        //多选时，智能搜索记录存储
        var selectedRecords = [];


        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.listFields && me.listHeadTexts) {

            var listheaders = '';
            var listfields = '';

            var heads = me.listHeadTexts.split(','); //列头 
            var fields = me.listFields.split(','); //所有字段              

            var modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                } else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: temp, //fields[i],
                    type: 'string',
                    mapping: temp //fields[i]
                });

            }

            if (me.showAutoHeader) {

                for (var i = 0; i < heads.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                }
            }

            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                } else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            } else {
                temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;




        } else {
            //me.initialListTemplate(); //初始化下拉列表样式 
            var tempfield = me.valueField.split('.');//系统编码
            var valueField;
            if (tempfield.length > 1) {
                valueField = tempfield[1]; //去掉表名
            }
            else {
                valueField = me.valueField;
            }

            if (!me.userCodeField) {
                me.userCodeField = me.valueField;//容错处理
            }
            var uField = me.userCodeField.split('.');//用户编码
            var userCodeField;
            if (uField.length > 1) {
                userCodeField = uField[1];
            } else {
                userCodeField = me.userCodeField;
            }

            var dfield = me.displayField.split('.');
            var displayField;
            if (dfield.length > 1) {
                displayField = dfield[1]; //去掉表名
            }
            else {
                displayField = me.displayField;
            }

            var modelFields = [{
                name: valueField,
                type: 'string',
                mapping: valueField
            }, {
                name: userCodeField,
                type: 'string',
                mapping: userCodeField
            }, {
                name: displayField,
                type: 'string',
                mapping: displayField
            }]

            var listfields = '<td>{' + userCodeField + '}</td>';//显示用户代码
            listfields += '<td>{' + displayField + '}</td>';
            me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';

        }
        var store = Ext.create('Ext.data.Store', {
            //var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            fields: modelFields,
            cachePageData: true,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                reader: {
                    type: 'json',
                    root: 'Record',
                    totalProperty: 'totalRows'
                }
            }
        });

        me.bindStore(store);


        //只能在这里写事件才能触发到
        store.on('beforeload', function (store) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (me.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
            if (me.likeFilter) {
                Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
            }
            if (me.leftLikeFilter) {
                Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
            }
            if (me.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
            }

        });


        store.on('load', function (store, records, successful, eOpts) {

            var temp = store.data.items.concat(selectedRecords);
            //store.data.items = temp;
            store.loadData(temp);
            me.setValue(selectedRecords, false);
            //if (store.cachePageData) {

            //                    var allData = [];
            //                    for (var i = 0; i < store.dataContainer.items.length; i++) {
            //                        allData = allData.concat(store.dataContainer.items[i]);
            //                    }

            //                    allData.concat(store.data.items);
            //                    store.loadData(allData);                

            //              }

            if (me.needBlankLine) {
                //去掉表名
                var myValueFiled;
                var myDisplayField;
                var temp = me.valueField.split('.');
                if (temp.length > 1) {
                    myValueFiled = temp[1];
                } else {
                    myValueFiled = me.valueField;
                }

                temp = me.displayField.split('.');
                if (temp.length > 1) {
                    myDisplayField = temp[1];
                } else {
                    myDisplayField = me.displayField;
                }

                var emptydata = new Object();
                emptydata[myValueFiled] = '';
                emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                var rs = [emptydata];
                store.insert(0, rs);
            }

        });
        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');

        me.on('select', function (combo, records, eOpts) {
            if (me.multiSelect) {
                //判断是否存在
                var isExist = function (record) {
                    var flag = false;
                    for (var i = 0; i < selectedRecords.length; i++) {
                        var myRecord = selectedRecords[i];

                        if (record.data[me.valueField] == myRecord.data[me.valueField]) {
                            flag = true;
                            break;
                        }
                    }

                    return flag;
                };

                var tempRecords = [];
                for (var i = 0; i < records.length; i++) {
                    if (!isExist(records[i])) {
                        tempRecords.push(records[i]);
                    }
                }

                selectedRecords = selectedRecords.concat(tempRecords);
                me.setValue(selectedRecords, false);

            }
            var theField;

            var modelFileds;
            //构建
            if (me.listFields) {
                theField = [];
                modelFileds = []
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);


                    var obj = {
                        name: temp[i],
                        type: 'string',
                        mapping: temp[i]
                    };

                    modelFileds.push(obj);
                }
            } else {

                theField = [me.valueField, me.displayField];

                modelFileds = [{
                    name: me.valueField,
                    type: 'string',
                    mapping: me.valueField
                }, {
                    name: me.displayField,
                    type: 'string',
                    mapping: me.displayField
                }];
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: modelFileds//theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            //            var code = combo.getValue() || records[0].data[myValueFiled];
            //            var name = combo.getRawValue() || records[0].data[myDisplayField];

            var codeArr = [];
            var nameArr = [];
            for (var i = 0; i < records.length; i++) {
                codeArr.push(records[i].data[myValueFiled]);
                nameArr.push(records[0].data[myDisplayField]);
            }

            var code = codeArr.join();
            var name = nameArr.join();

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) { //嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            //select不需要设置value
            if (me.isInGrid) {
                me.setValue(valuepair); //必须这么设置才能成功
            }

            //debugger;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    pobj.data[theField[i]] = records[0].data[temp[1]];
                } else {
                    pobj.data[theField[i]] = records[0].data[theField[i]];
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {
            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                    case Ext.EventObject.LEFT:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                    case Ext.EventObject.RIGHT:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getRawValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });


        if (me.editable) {
            me.on('blur', function () {

                selectedRecords.length = 0; //清空数组

                if (me.getRawValue() == "") {
                    me.setValue('');
                }
                var value = me.getValue();
                if (value == "") {
                    me.setValue('');
                    return;
                }
                value = encodeURI(value);
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/RichHelp/ValidateData?helpid=' + me.helpid + '&inputValue=' + value + '&selectMode=' + me.selectMode,
                    params: { 'clientSqlFilter': this.clientSqlFilter, 'helptype': 'ngCommonHelp' },
                    async: false, //同步请求
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                            if (resp.Data == false) {
                                me.setValue('');
                            }
                        } else {
                            Ext.MessageBox.alert('取数失败', resp.status);
                        }
                    }
                });
            });
        }
    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (me.helpType === 'rich') { //用户自定义界面的模板 

                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;

                if (me.showAutoHeader) {
                    for (var i = 0; i < gridColumns.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + gridColumns[i].header + '</th>';
                    }
                }

                for (var i = 0; i < modelFields.length; i++) {
                    listfields += '<td>{' + modelFields[i]['name'] + '}</td>';
                }

            } else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头 

                if (me.showAutoHeader) {
                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    } else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp//fields[i]
                    });

                }
            }


            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                } else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });
            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }


            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            } else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/RichHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        } else {
            url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    } else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    },
    getValue: function () {
        var me = this;
        if (me.multiSelect && Ext.isArray(me.value)) {

            return me.value.join(','); //多选，数组转字符串
        }
        else {
            return me.value;
        }
    }
});

Ext.define('Ext.ng.GMultiRichHelp', {
    extend: 'Ext.ng.GRichHelp',
    alias: ['widget.ngGMultiRichHelp'],
    selectMode: 'Multi',
    multiSelect: true
});

Ext.define('Ext.ng.EmpCertSelectHelp', {
    extend: 'Ext.window.Window',
    title: '员工证书帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    mutli: false,
    width: 750,
    from: 'borrow',
    phids: '',
    height: 500,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        var ismutli = me.mutli === true ? "1" : "0";
        me.id = "empcertselecthelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/Certificate/EmployeeCert/EmpCertSelectHelp?mutli=' + ismutli + "&from=" + me.from + "&phids=" + me.phids;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.EntCertSelectHelp', {
    extend: 'Ext.window.Window',
    title: '企业证书帮助',
    autoScreen: true,
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    mutli: false,
    //draggable:false,
    width: 750,
    height: 500,
    from: 'borrow',
    phids: '',
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "entcertselecthelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        var ismutli = me.mutli == true ? "1" : "0";
        frame.src = C_ROOT + 'WM/Certificate/EnterpriseCert/EntCertSelectHelp?mutli=' + ismutli + "&from=" + me.from + "&phids=" + me.phids;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

//关闭Ext.window.Window窗口
var CloseWindow = function () {
    var currentWin = window;
    while (top != currentWin) {
        var parentExt = currentWin.parent.Ext;
        var fElement = parentExt.get(currentWin.frameElement);
        var windowElement = fElement.up('div.x-window');
        if (windowElement) {
            var winId = windowElement.id;
            var extWin = parentExt.getCmp(winId);
            extWin.close();
            return true;
        } else {
            currentWin = currentWin.parent;
        }
    }
    alert("窗口关闭失败！");
    return false;
}
Ext.define('Ext.ng.FileHelp', {
    extend: 'Ext.window.Window',
    title: '归档帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    width: 780,
    height: 550,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "filehelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/File/FileLibMain/FilePlatformHelp';
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.OrgFileHelp', {
    extend: 'Ext.window.Window',
    title: '归档帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    orgid: "",
    width: 780,
    height: 550,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "filehelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/File/FileLibMain/OrgFilePlatformHelp?orgid=' + me.orgid;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.ProFileHelp', {
    extend: 'Ext.window.Window',
    title: '归档帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    orgid: "",
    protypeid: "",
    projectid: "",
    //draggable:false,
    width: 780,
    height: 550,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "filehelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/File/FileLibMain/ProFilePlatformHelp?orgid=' + me.orgid + "&protypeid=" + me.protypeid + "&projectid=" + me.projectid;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.FileMuliSelectHelp', {
    extend: 'Ext.window.Window',
    title: '档案多选帮助',
    autoScreen: true,
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    width: 780,
    height: 550,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "filemuliselecthelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/File/FileLibMain/FileMutliSelectHelp';
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.FileSelectHelp', {
    extend: 'Ext.window.Window',
    title: '档案帮助',
    autoScreen: true,
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    width: 780,
    height: 550,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "fileselecthelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/File/FileLibMain/FileSelectHelp';
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.ArchiveToDocHelp', {
    extend: 'Ext.window.Window',
    title: '导出到文档库',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    constrain: true,//限制拖动范围
    maximizable: false,
    resizable: true,
    modal: true,
    //draggable:false,
    width: 400,
    height: 600,
    layout: 'fit',
    arcid: '',
    formType: '',
    openType: '',
    items: [],
    initComponent: function () {
        var me = this;
        var doccno = '';
        me.addEvents('bntOk'); //定义值被选完的事件  
        if (me.openType != 'batchexport') {
            Ext.Ajax.request({
                params: { 'arcid': me.arcid, 'formType': me.formType },
                url: C_ROOT + 'WM/Doc/Document/FindDocnoBySPhId',
                async: false, //同步请求
                success: function (response) {
                    //var resp = Ext.JSON.decode(response.responseText);
                    doccno = response.responseText;
                }
            });
        }
        Ext.define('DocLibTreeModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'PhId', type: 'string', mapping: 'id' },
                { name: 'Pphid', type: 'string' },
                { name: 'Cname', type: 'string' },
                { name: 'Cno', type: 'string' },
                { name: 'FromType', type: 'string' },
                { name: 'ProjectTypeName', type: 'string' }
            ]
        });
        var docLibStore = Ext.create('Ext.data.TreeStore', {
            model: 'DocLibTreeModel',
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'WM/Doc/DocLib/GetDocLibTreeNodes?libtype=1'
            }
        });

        //文档类别
        var doclibTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            rootVisible: false,
            store: docLibStore,
            multiSelect: true,
            width: '100%',
            height: 475,
            //singleExpand: true,
            columns: [{
                xtype: 'treecolumn',
                text: '名称',
                flex: 2,
                sortable: true,
                dataIndex: 'Cname'
            }, {
                text: '编号',
                flex: 1,
                dataIndex: 'Cno',
                sortable: true
            }, {
                text: '主键',
                flex: 0,
                dataIndex: 'PhId',
                hidden: true,
                hideable: false
            }]
        });

        var layout = new Object();
        if (me.openType == 'batchexport') {
            layout = {
                type: 'fit'
            };
        }
        else {
            layout = {
                type: 'vbox',
                align: 'center'
            };
        }

        var items = [doclibTree];

        if (me.openType != "batchexport") {
            items.push({
                xtype: 'label',
                forId: 'myarchiveShow',
                text: '如果公文已导入所选文档库，将更新该文档。',
                margin: '0 0 0 10'
            });
            items.push({
                xtype: 'textfield',
                name: 'name',
                fieldLabel: '文档编号',
                allowBlank: true,
                readOnly: doccno != "",
                value: doccno
            });
        }

        //panel
        var panel = Ext.create('Ext.panel.Panel', {
            name: "mp",
            layout: layout,
            items: items
        });


        me.items = panel;
        me.buttons = [{
            text: '确认',
            handler: function () {
                var data = doclibTree.getSelectionModel().getSelection();
                //  me.parent.Ext.getCmp('signetinfohelp').fireEvent('bntOk', data);
                var rd = new Object();
                if (data.length > 0) {
                    var pid = data[0].get("PhId");
                    if (pid == "" || pid == "0") {
                        alert("请选择对应文档库，不要选根目录");
                        return;
                    }
                    rd.pid = pid;
                    if (me.openType != "batchexport") {
                        rd.cno = me.items.items[0].items.items[2].value;
                    }
                    else {
                        rd.cno = '';
                    }
                    rd.fromC = doccno;
                    me.fireEvent('bntOk', rd);
                    me.close();
                } else {
                    alert("请选择对应文档库，不要选根目录");
                    return;
                }
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];

        me.callParent();
    },
    listeners: {

    }
});

Ext.define('Ext.ng.CalendarContentHelp', {
    extend: 'Ext.window.Window',
    title: '日程日志选择帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    projectid: '',
    //draggable:false,
    mutli: false,
    width: 500,
    from: 'borrow',
    height: 350,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        var ismutli = me.mutli === true ? "1" : "0";
        me.id = "calendarcontenthelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'WM/Affairs/MyWorklog/CalendarContentHelp?projectid="' + me.projectid;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});


//关闭Ext.window.Window窗口
var CloseWindow = function () {
    var currentWin = window;
    while (top != currentWin) {
        var parentExt = currentWin.parent.Ext;
        var fElement = parentExt.get(currentWin.frameElement);
        var windowElement = fElement.up('div.x-window');
        if (windowElement) {
            var winId = windowElement.id;
            var extWin = parentExt.getCmp(winId);
            extWin.close();
            return true;
        } else {
            currentWin = currentWin.parent;
        }
    }
    alert("窗口关闭失败！");
    return false;
}


Ext.define('Ext.ng.PlanSummaryHelp', {
    extend: 'Ext.window.Window',
    title: '计划内容帮助',
    autoScreen: true,
    //iconCls: 'icon-Attachment',
    closable: true,
    constrain: true,//限制拖动范围
    maximizable: false,
    resizable: true,
    modal: true,
    empid: '',
    projectid: '',
    draggable: false,
    width: 600,
    height: 400,
    layout: 'fit',
    items: [],
    initComponent: function () {
        var me = this;
        var empno = '';
        me.addEvents('bntOk'); //定义值被选完的事件  
        //if (me.openType != 'batchexport') {
        //    Ext.Ajax.request({
        //        params: { 'arcid': me.arcid, 'formType': me.formType },
        //        url: C_ROOT + 'WM/Doc/Document/FindDocnoBySPhId',
        //        async: false, //同步请求
        //        success: function (response) {
        //            //var resp = Ext.JSON.decode(response.responseText);
        //            doccno = response.responseText;
        //        }
        //    });
        //}
        Ext.define('rzModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'PhId', type: 'string' },
                {
                    name: 'Content', type: 'string',
                    mapping: 'Cname'
                },
                { name: 'Bdt', type: 'System.DateTime' },
                { name: 'Edt', type: 'System.DateTime' }
            ]
        });

        Ext.define('pdmodel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'Ccode',
                    type: 'string',
                    mapping: 'Ccode'
                }, {
                    name: 'Cbcode',
                    type: 'string',
                    mapping: 'Cbcode'
                }, {
                    name: 'Content',
                    type: 'string',
                    mapping: 'Content'
                }, {
                    name: 'Bdt',
                    type: 'date',
                    mapping: 'Bdt'
                }, {
                    name: 'Edt',
                    type: 'date',
                    mapping: 'Edt'
                }]
        });

        Ext.define('taskmodel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string',//因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'Content', type: 'string',
                    mapping: 'CName'
                }, {
                    name: 'LimitDate',
                    type: 'date',
                    mapping: 'LimitDate'
                }, {
                    name: 'FinishDt',
                    type: 'date',
                    mapping: 'FinishDt'
                }]
        });
        //计划
        var plstore = Ext.create('Ext.ng.JsonStore', {
            model: 'pdmodel',
            pageSize: 10,
            autoLoad: false,
            method: 'POST',
            url: '../PlanSummary/GetPlanSummaryDetail?empId=' + me.empid + "&projectid=" + me.projectid,

        });
        //日志
        var rzstore = Ext.create('Ext.ng.JsonStore', {
            model: 'rzModel',
            pageSize: 10,
            autoLoad: false,
            method: 'POST',
            url: '../MyWorklog/GetWorklogListForPlanHelp?projectid=' + me.projectid,

        });
        //任务
        var taskstore = Ext.create('Ext.ng.JsonStore', {
            model: 'taskmodel',
            pageSize: 10,
            autoLoad: false,
            method: 'POST',
            url: '../PbcTask/GetPbcTaskListAcceptAPP?empId=' + me.empid + "&projectid=" + me.projectid,

        });


        var taskselModel = Ext.create('Ext.selection.CheckboxModel');

        var taskpagingbar = Ext.create('Ext.ng.PagingBar', {
            store: taskstore
        });
        var taskgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            border: false,
            //frame: true,
            store: taskstore,
            bbar: taskpagingbar,
            selModel: taskselModel, //多选
            columnLines: true,
            buskey: 'PhId', //对应的子表主键属性
            columns: [
                {
                    header: Lang.PhId || '主键',
                    dataIndex: 'PhId',
                    width: 100,
                    sortable: false,
                    editor: { allowBlank: true },
                    hidden: true
                }, {
                    header: Lang.Content || '任务内容',
                    dataIndex: 'Content',
                    flex: 1,
                    sortable: false
                }, {
                    header: Lang.LimitDate || '完成期限',
                    dataIndex: 'LimitDate',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: Ext.util.Format.dateRenderer('Y-m-d')
                }

            ]
        });


        var selModel = Ext.create('Ext.selection.CheckboxModel');

        var rizhipagingbar = Ext.create('Ext.ng.PagingBar', {
            store: rzstore
        });
        var rizhigrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            border: false,
            //frame: true,
            store: rzstore,
            bbar: rizhipagingbar,
            selModel: selModel, //多选
            columnLines: true,
            buskey: 'PhId', //对应的子表主键属性
            columns: [
                {
                    header: Lang.PhId || '主键',
                    dataIndex: 'PhId',
                    width: 100,
                    sortable: false,
                    editor: { allowBlank: true },
                    hidden: true
                }, {
                    header: Lang.Content || '日志内容',
                    dataIndex: 'Content',
                    flex: 1,
                    sortable: false
                }, {
                    header: Lang.Bdt || '开始时间',
                    dataIndex: 'Bdt',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    //renderer: Ext.util.Format.dateRenderer('H:i')
                    renderer: Ext.util.Format.dateRenderer('Y-m-d')
                }, {
                    header: Lang.Edt || '结束时间',
                    dataIndex: 'Edt',
                    width: 100,
                    sortable: false,
                    renderer: Ext.util.Format.dateRenderer('Y-m-d')
                }
            ]
        });

        var selModel1 = Ext.create('Ext.selection.CheckboxModel');

        var planpagingbar = Ext.create('Ext.ng.PagingBar', {
            store: plstore
        });

        //计划grid
        var plgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            border: false,
            //frame: true,
            store: plstore,
            bbar: planpagingbar,
            selModel: selModel1, //多选
            columnLines: true,
            buskey: 'PhId', //对应的子表主键属性
            columns: [
                {
                    header: Lang.PhId || '主键',
                    dataIndex: 'PhId',
                    width: 100,
                    sortable: false,
                    editor: { allowBlank: true },
                    hidden: true
                }, {
                    header: Lang.Content || '计划内容',
                    dataIndex: 'Content',
                    flex: 1,
                    sortable: false
                }, {
                    header: Lang.Bdt || '开始时间',
                    dataIndex: 'Bdt',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    //renderer: Ext.util.Format.dateRenderer('H:i')
                    renderer: Ext.util.Format.dateRenderer('Y-m-d')
                }, {
                    header: Lang.Edt || '结束时间',
                    dataIndex: 'Edt',
                    width: 100,
                    sortable: false,
                    renderer: Ext.util.Format.dateRenderer('Y-m-d')
                }
            ]
        });

        var tabs = Ext.create('Ext.TabPanel', {
            itemId: 'tabs',
            activeTab: 0,
            enableTabScroll: true,
            region: 'center',
            layout: 'border',
            items: [
                {
                    title: '周',
                    itemId: 'week0',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [plgrid],
                }, {
                    title: '月',
                    itemId: 'mouth0',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [],
                }, {
                    title: '季',
                    itemId: 'season0',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [],
                }, {
                    title: '年',
                    itemId: 'year0',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [],
                },
                {
                    title: '日志',
                    itemId: 'tab2',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [],
                }, {
                    title: '任务',
                    itemId: 'tasktab',
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    border: true,
                    items: [],
                }
                //,
                //{
                //    title: '选择结果',
                //    id: 'tab3',
                //    xtype: 'panel',
                //    region: 'center',
                //    layout: 'border',
                //    border: true,
                //    items: [],
                //}
            ]
        });
        tabs.on('tabchange', function (tabPanel, newCard, oldCard, eOpts) {
            var proxy;
            if (oldCard.itemId == "tab2") {
                oldCard.items.remove(rizhigrid);
            } else {
                oldCard.items.remove(plgrid);
            }
            if (newCard.itemId == 'week0') {
                if (newCard.items.length < 1) {
                    newCard.add(plgrid);
                }
                plstore.cachePageData = false;
                plstore.removeAll();
                proxy = plstore.getProxy();
                proxy.setExtraParam('cycletype', 'GCWEEK');
                plstore.load();
            }
            else if (newCard.itemId == 'mouth0') {
                if (newCard.items.length < 1) {
                    newCard.add(plgrid);
                }
                plstore.cachePageData = false;
                plstore.removeAll();
                proxy = plstore.getProxy();
                proxy.setExtraParam('cycletype', 'GCMONTH');
                plstore.load();
            } else if (newCard.itemId == 'season0') {
                if (newCard.items.length < 1) {
                    newCard.add(plgrid);
                }
                plstore.cachePageData = false;
                plstore.removeAll();
                proxy = plstore.getProxy();
                proxy.setExtraParam('cycletype', 'GCSEASON');
                plstore.load();
            }
            else if (newCard.itemId == 'year0') {
                if (newCard.items.length < 1) {
                    newCard.add(plgrid);
                }
                plstore.cachePageData = false;
                plstore.removeAll();
                proxy = plstore.getProxy();
                proxy.setExtraParam('cycletype', 'GCYEAR');
                plstore.load();
            }
            else if (newCard.itemId == 'tab2') {
                if (newCard.items.length < 1) {
                    newCard.add(rizhigrid);
                }
                rzstore.cachePageData = false;
                rzstore.removeAll();
                // proxy = rzstore.getProxy();
                // proxy.setExtraParam('cycletype', 'GCWEEK');
                rzstore.load();
            } else if (newCard.itemId == 'tasktab') {
                if (newCard.items.length < 1) {
                    newCard.add(taskgrid);
                }
                taskstore.cachePageData = false;
                taskstore.removeAll();
                // proxy = rzstore.getProxy();
                // proxy.setExtraParam('cycletype', 'GCWEEK');
                taskstore.load();
            }
        });
        //布局
        var panel = Ext.create('Ext.panel.Panel', {

            region: 'center',
            autoScroll: false,
            layout: 'border',
            items: [tabs]

        });

        me.items = panel;
        me.buttons = [{
            text: '确认',
            handler: function () {
                var data = null;

                if (tabs.activeTab.itemId == 'tab2') {
                    data = rizhigrid.getSelectionModel().getSelection();
                } else if (tabs.activeTab.itemId == 'tasktab') {
                    data = taskgrid.getSelectionModel().getSelection();
                } else {
                    data = plgrid.getSelectionModel().getSelection();
                }
                //  me.parent.Ext.getCmp('signetinfohelp').fireEvent('bntOk', data);
                if (data.length > 0) {
                    me.fireEvent('bntOk', data);
                    me.close();
                } else {
                    me.close();
                }
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];
        plstore.getProxy().setExtraParam('cycletype', 'GCWEEK');
        plstore.load();
        me.callParent();
    },
    listeners: {

    }
});

Ext.define("Ext.ng.OfficeTemplateHelp", {
    extend: 'Ext.window.Window',
    title: '模板帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 500,
    height: 300,
    sdata: null,
    border: false,
    ORMMode: true,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;

        me.addEvents('helpSelected');
        me.addEvents('helpClosed');
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'PhId',
                type: 'string',
                mapping: 'PhId'
            }, {
                name: 'Cno',
                type: 'string',
                mapping: 'Cno'
            }, {
                name: 'Cname',
                type: 'string',
                mapping: 'Cname'
            }, {
                name: 'PhIdJingGe',
                type: 'string',
                mapping: 'PhIdJingGe'
            }, {
                name: 'Type',
                type: 'string',
                mapping: 'Type'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=WmTemplate&ORMMode=' + me.ORMMode
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        store.on('beforeload', function (store) {
            if (me.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'OfficeTemplateHelp',
            columnLines: true,
            store: store,
            hideHeaders: true,//隐藏表头
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '主键',
                    dataIndex: 'PhId',
                    flex: 1,
                    sortable: false,
                    hidden: true
                }, {
                    header: '编码',
                    dataIndex: 'Cno',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }, {
                    header: '模板名称',
                    dataIndex: 'Cname',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }
            ],
            bbar: pagingbar
        });

        grid.on('itemdblclick', function (_this, record, item, index) {
            var phid = record.get('PhId');
            var cname = record.get('Cname');

            var pobj = new Object();

            pobj.phid = phid;
            pobj.phid_jingge = record.get('PhIdJingGe');
            pobj.cname = cname;

            pobj.data = Ext.decode(record.data.row);
            me.fireEvent('helpselected', pobj);
            me.close();
        });

        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "ok", text: "确定", iconCls: 'add'
                },
                {
                    itemId: "cancle", text: "取消", iconCls: 'cross'
                }
            ]
        });


        me.items = [rolltoolbar, grid];
        me.callParent();

        rolltoolbar.items.get('ok').on('click', function () {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                var phid = data[0].get('PhId');
                var cname = data[0].get('Cname');

                var pobj = new Object();

                pobj.phid = phid;
                pobj.phid_jingge = data[0].get('PhIdJingGe');
                pobj.cname = cname;

                pobj.data = Ext.decode(data.row);
                me.fireEvent('helpselected', pobj);
            }
            else {
                Ext.MessageBox.alert('提示', '请选择模板！');
                return;
            }
            me.close();
        });
        rolltoolbar.items.get('cancle').on('click', function () {
            me.fireEvent('helpclosed');
            me.close();
        });
    }
});

//会议纪要模板帮助
Ext.define("Ext.ng.MeetSummaryModelHelp", {
    extend: 'Ext.window.Window',
    title: '会议纪要模板帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 500,
    height: 300,
    sdata: null,
    border: false,
    ORMMode: true,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;

        me.addEvents('helpSelected');
        me.addEvents('helpClosed');
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'PhId',
                type: 'string',
                mapping: 'PhId'
            }, {
                name: 'Cno',
                type: 'string',
                mapping: 'Cno'
            }, {
                name: 'Cname',
                type: 'string',
                mapping: 'Cname'
            }, {
                name: 'Cobject',
                type: 'string',
                mapping: 'Cobject'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=WM3_MeetSummaryModelHelp&ORMMode=' + me.ORMMode
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        store.on('beforeload', function (store) {
            if (me.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'MeetSummaryModelHelp',
            columnLines: true,
            store: store,
            hideHeaders: true,//隐藏表头
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '主键',
                    dataIndex: 'PhId',
                    flex: 1,
                    sortable: false,
                    hidden: true
                }, {
                    header: '编码',
                    dataIndex: 'Cno',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }, {
                    header: '模板名称',
                    dataIndex: 'Cname',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }
            ],
            bbar: pagingbar
        });

        grid.on('itemdblclick', function (_this, record, item, index) {
            var phid = record.get('PhId');
            var cname = record.get('Cname');

            var pobj = new Object();

            pobj.phid = phid;
            pobj.cobject = record.get('Cobject');
            pobj.cname = cname;

            pobj.data = Ext.decode(record.data.row);
            me.fireEvent('helpselected', pobj);
            me.close();
        });

        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "ok", text: "确定", iconCls: 'add'
                },
                {
                    itemId: "cancle", text: "取消", iconCls: 'cross'
                }
            ]
        });


        me.items = [rolltoolbar, grid];
        me.callParent();

        rolltoolbar.items.get('ok').on('click', function () {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                var phid = data[0].get('PhId');
                var cname = data[0].get('Cname');

                var pobj = new Object();

                pobj.phid = phid;
                pobj.cobject = data[0].get('Cobject');
                pobj.cname = cname;

                pobj.data = Ext.decode(data.row);
                me.fireEvent('helpselected', pobj);
            }
            else {
                Ext.MessageBox.alert('提示', '请选择模板！');
                return;
            }
            me.close();
        });
        rolltoolbar.items.get('cancle').on('click', function () {
            me.fireEvent('helpclosed');
            me.close();
        });
    }
});

//新闻正文模板帮助
Ext.define("Ext.ng.InformModelHelp", {
    extend: 'Ext.window.Window',
    title: '正文模板帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 500,
    height: 300,
    sdata: null,
    border: false,
    ORMMode: true,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;

        me.addEvents('helpSelected');
        me.addEvents('helpClosed');
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'PhId',
                type: 'string',
                mapping: 'PhId'
            }, {
                name: 'Cno',
                type: 'string',
                mapping: 'Cno'
            }, {
                name: 'Cname',
                type: 'string',
                mapping: 'Cname'
            }, {
                name: 'Cobject',
                type: 'string',
                mapping: 'Cobject'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=WM3_InformModelHelp&ORMMode=' + me.ORMMode
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        store.on('beforeload', function (store) {
            if (me.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'InformModelHelp',
            columnLines: true,
            store: store,
            hideHeaders: true,//隐藏表头
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '主键',
                    dataIndex: 'PhId',
                    flex: 1,
                    sortable: false,
                    hidden: true
                }, {
                    header: '编码',
                    dataIndex: 'Cno',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }, {
                    header: '模板名称',
                    dataIndex: 'Cname',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }
            ],
            bbar: pagingbar
        });

        grid.on('itemdblclick', function (_this, record, item, index) {
            var phid = record.get('PhId');
            var cname = record.get('Cname');

            var pobj = new Object();

            pobj.phid = phid;
            pobj.cobject = record.get('Cobject');
            pobj.cname = cname;

            pobj.data = Ext.decode(record.data.row);
            me.fireEvent('helpselected', pobj);
            me.close();
        });

        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "ok", text: "确定", iconCls: 'add'
                },
                {
                    itemId: "cancle", text: "取消", iconCls: 'cross'
                }
            ]
        });


        me.items = [rolltoolbar, grid];
        me.callParent();

        rolltoolbar.items.get('ok').on('click', function () {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                var phid = data[0].get('PhId');
                var cname = data[0].get('Cname');

                var pobj = new Object();

                pobj.phid = phid;
                pobj.cobject = data[0].get('Cobject');
                pobj.cname = cname;

                pobj.data = Ext.decode(data.row);
                me.fireEvent('helpselected', pobj);
            }
            else {
                Ext.MessageBox.alert('提示', '请选择模板！');
                return;
            }
            me.close();
        });
        rolltoolbar.items.get('cancle').on('click', function () {
            me.fireEvent('helpclosed');
            me.close();
        });
    }
});

//金格标签帮助
Ext.define("Ext.ng.OfficeTagHelp", {
    extend: 'Ext.window.Window',
    title: '标签帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 500,
    height: 300,
    sdata: null,
    border: false,
    ORMMode: false,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;

        me.addEvents('helpSelected');
        me.addEvents('helpclosed');
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'phid',
                type: 'string',
                mapping: 'phid'
            }, {
                name: 'code',
                type: 'string',
                mapping: 'code'
            }, {
                name: 'name',
                type: 'string',
                mapping: 'name'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=wm3_kinggrid_tag_help&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function (store) {
            if (me.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
        })
        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });
        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'OfficeTagHelp',
            columnLines: true,
            store: store,
            hideHeaders: true,//隐藏表头
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '主键',
                    dataIndex: 'phid',
                    flex: 1,
                    sortable: false,
                    hidden: true
                }, {
                    header: '编码',
                    dataIndex: 'code',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }, {
                    header: '模板名称',
                    dataIndex: 'name',
                    flex: 1,
                    sortable: false,
                    hidden: false,
                    menuDisabled: true
                }
            ],
            bbar: pagingbar
        });

        grid.on('itemdblclick', function (_this, record, item, index) {
            var phid = record.get('phid');
            var name = record.get('name');

            var pobj = new Object();
            pobj.phid = phid;
            pobj.code = record.get('code');
            pobj.name = name;
            pobj.data = Ext.decode(record.data.row);
            me.fireEvent('helpselected', pobj);
            me.close();
        });

        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "ok", text: "确定", iconCls: 'add'
                },
                {
                    itemId: "cancle", text: "取消", iconCls: 'cross'
                }
            ]
        });


        me.items = [rolltoolbar, grid];
        me.callParent();

        rolltoolbar.items.get('ok').on('click', function () {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                var phid = data[0].get('phid');
                var name = data[0].get('name');
                var code = data[0].get('code');
                var pobj = new Object();

                pobj.phid = phid;
                pobj.name = name;
                pobj.code = code;
                pobj.data = Ext.decode(data.row);
                me.fireEvent('helpselected', pobj);
            }
            else {
                Ext.MessageBox.alert('提示', '请选择模板！');
                return;
            }
            me.close();
        });
        rolltoolbar.items.get('cancle').on('click', function () {
            me.fireEvent('helpclosed');
            me.close();
        });
    }
});

Ext.define("Ext.ng.TranslateHistory", {
    extend: 'Ext.window.Window',
    title: '我的公文转发历史',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 700,
    height: 433,
    sdata: null,
    border: false,
    ORMMode: true,
    doctype: '',
    doccode: '',
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {

        var me = this;

        if (Ext.isEmpty(me.doccode) || Ext.isEmpty(me.doctype)) {
            Ext.MessageBox.alert('提示', '帮助初始化参数未设置！');
            return;
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'PhId',
                type: 'string',
                mapping: 'PhId'
            }, {
                name: 'Filler',
                type: 'string',
                mapping: 'Filler'
            }, {
                name: 'FillerUserName',
                type: 'string',
                mapping: 'FillerUserName'
            }, {
                name: 'Receiver',
                type: 'string',
                mapping: 'Receiver'
            }, {
                name: 'ReceiverUserName',
                type: 'string',
                mapping: 'ReceiverUserName'
            }, {
                name: 'Dt',
                type: 'string',
                mapping: 'Dt'
            }, {
                name: 'OName',
                type: 'string',
                mapping: 'OName'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'WM/Archive/ArcArchiveSend/GetArcArchiveSendList?doctype=' + me.doctype + '&doccode=' + me.doccode,
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'TranslateHistory',
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '转发人',
                    dataIndex: 'FillerUserName',
                    flex: 1,
                    sortable: false
                }, {
                    header: '接收人',
                    dataIndex: 'ReceiverUserName',
                    flex: 1,
                    sortable: false
                }, {
                    header: '职能机构',
                    dataIndex: 'OName',
                    flex: 1,
                    sortable: false
                }, {
                    header: '转发时间',
                    dataIndex: 'Dt',
                    flex: 1,
                    sortable: false
                }
            ],
            bbar: pagingbar
        });

        me.items = [grid];

        me.callParent();
    }
});

//收文管理来文文号帮助
Ext.define('Ext.ng.ArcReceiptCnoHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ArcReceiptCnoHelp'],
    triggerCls: 'x-form-help-trigger',//框帮助按钮的样式
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    //archiveType: 'DOCDISPATCH',
    initComponent: function () {

        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected'); //定义值被选完的事件



    },
    onTriggerClick: function (eOption, ignoreBeforeEvent) { //ignoreBeforeEvent为true能手动弹出帮助
        var me = this;

        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击        

        //查询
        var querypanel = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            bodyPadding: '5 10 5 10',
            layout: {
                type: 'hbox'
            },
            defaults: {
                margin: '0 5 0 5'
            },
            items: [
                { xtype: 'label', text: '关键字：', margin: '4 0 0 0' },
                { xtype: 'textfield', width: 150, itemId: 'searchValue' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '查询',
                    handler: function () {
                        var searchValue = querypanel.queryById('searchValue').getValue();
                        Ext.apply(store.proxy.extraParams, { 'keyword': searchValue });
                        store.load();
                    }
                }
            ]
        });

        //定义模型
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhidArc',
                    type: 'string',
                    mapping: 'PhidArc'
                }, {
                    name: 'Cno',
                    type: 'string',
                    mapping: 'Cno'
                }, {
                    name: 'Cname',
                    type: 'string',
                    mapping: 'Cname'
                }, {
                    name: 'Cnum',
                    type: 'string',
                    mapping: 'Cnum'
                }, {
                    name: 'CBoo',
                    type: 'string',
                    mapping: 'CBoo'
                }, {
                    name: 'BooName',
                    type: 'string',
                    mapping: 'BooName'
                }
            ]
        });


        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 10,
            autoLoad: true,
            url: C_ROOT + 'WM/Archive/MyArchive/GetMyArchiveListHelp?archivetype=' + me.archiveType
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            stateful: true,
            stateId: 'myarchivelisthelp',
            store: store,
            //buskey: 'PhidArc', //对应的业务表主键属性               
            columnLines: true,
            columns: [Ext.create('Ext.grid.RowNumberer', { text: '行号', width: 35 }),
            {
                header: '编号',
                dataIndex: 'PhidArc',
                flex: 1,
                sortable: false,
                hidden: true
            }, {
                header: '来文文号',
                dataIndex: 'Cnum',
                flex: 1,
                sortable: false,
                hidden: false
            }, {
                header: '来文名称',
                dataIndex: 'Cname',
                flex: 1,
                sortable: false,
                hidden: false
            }
            ],
            bbar: pagingbar
        });



        var buttons = [];

        buttons.push('->');
        buttons.push({ text: '确定', handler: function () { me.btnOk(me, grid, store, win); } });
        buttons.push({ text: '取消', handler: function () { win.close(); } });

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: '我的收文帮助', //'Hello',
            border: false,
            height: me.helpHeight,
            width: me.helpWidth,
            layout: 'border',
            modal: true,
            constrain: true,
            items: [querypanel, grid], //[toolbar, queryProperty, tabPanel],
            buttons: buttons
        });
        win.show();

        grid.on('itemdblclick', function () {
            me.gridDbClick(me, grid, win);
        });

    },
    showHelp: function (eOption, ignoreBeforeEvent) {
        this.onTriggerClick(eOption, ignoreBeforeEvent);//忽略beforetriggerclick事件，手动弹出帮助
    },
    btnOk: function (_this, grid, store, win) {
        var pobj = new Object();

        var code;
        var name;
        var cboo;
        var booname;
        var cnum;


        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            code = data[0].get("PhidArc");
            name = data[0].get("Cname");
            cboo = data[0].get("Cboo");
            booname = data[0].get("BooName");
            cnum = data[0].get("Cnum");
            pobj.data = data[0].data;
        }

        win.hide();
        win.destroy();


        pobj.code = code;
        pobj.name = name;
        pobj.cnum = cnum;
        pobj.cboo = cboo;
        pobj.booname = booname;
        pobj.type = 'fromhelp';
        _this.fireEvent("helpselected", pobj);

    },
    gridDbClick: function (help, grid, win) {
        var pobj = new Object();

        var code;
        var name;
        var cboo;
        var booname;
        var cnum;

        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            code = data[0].get("PhidArc");
            name = data[0].get("Cname");
            cboo = data[0].get("CBoo");
            cnum = data[0].get("Cnum");
            booname = data[0].get("BooName");
            pobj.data = data[0].data;
        }

        win.hide();
        win.destroy();


        pobj.code = code;
        pobj.name = name;
        pobj.cboo = cboo;
        pobj.cnum = cnum;
        pobj.booname = booname;
        pobj.type = 'fromhelp';
        help.fireEvent("helpselected", pobj);
    },

});

//公文查看情况帮助
Ext.define("Ext.ng.ArcViewQKHelp", {
    extend: 'Ext.window.Window',
    title: '公文查看情况帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 700,
    height: 433,
    sdata: null,
    border: false,
    ORMMode: true,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {

        var me = this;

        //查询
        var querypanel = Ext.create('Ext.form.Panel', {
            region: 'north',
            frame: true,
            split: true,
            minWidth: 400,
            autoScroll: true,
            //bodyStyle: 'padding:5,5,5,5',
            fieldDefaults: {
                labelWidth: 80,
                anchor: '100%',
                margin: '5 10 5 10',
                msgTarget: 'side'
            },
            layout: 'column',
            items: [
                {
                    xtype: 'ngText', fieldLabel: '用户信息', name: 'userMsg', itemId: 'userMsg', columnWidth: 0.5
                },
                {
                    xtype: 'ngRichHelp',
                    fieldLabel: '职能机构',
                    name: 'orgid',
                    itemId: 'orgid',
                    ORMMode: true,
                    helpid: 'fg_orglist',
                    valueField: 'PhId',
                    displayField: 'OName',
                    readOnly: false,
                    mustInput: false,
                    columnWidth: 0.5
                },
                {
                    xtype: 'ngText', itemId: 'readedrate', fieldLabel: '已查看率', readOnly: true, columnWidth: 0.5
                },
                {
                    xtype: "checkbox", boxLabel: '未阅读', name: 'status', inputValue: '1', id: 'status', columnWidth: 0.25
                },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '查询',
                    maxWidth: 50,
                    margin: '5 0 0 0',
                    columnWidth: 0.25,
                    handler: function () {

                        var userMsg = querypanel.queryById('userMsg').getValue();
                        var orgId = querypanel.queryById('orgid').getValue();
                        var status = querypanel.queryById('status').getSubmitValue();

                        var rate = me.getReadedRate(status);
                        querypanel.queryById('readedrate').setValue(rate);

                        Ext.apply(store.proxy.extraParams, { 'usermsg': userMsg, 'orgid': orgId, 'status': status });
                        store.currentPage = 1;
                        store.load();
                    }
                }
            ]
        });

        querypanel.queryById('status').on("change", function (_this, nVal, oVal, eOpts) {
            if (nVal == true) {
                querypanel.queryById('readedrate').setFieldLabel("未查看率");
            }
            else {
                querypanel.queryById('readedrate').setFieldLabel("已查看率");
            }

            var userMsg = querypanel.queryById('userMsg').getValue();
            var orgId = querypanel.queryById('orgid').getValue();
            var status = querypanel.queryById('status').getSubmitValue();

            var rate = me.getReadedRate(status);
            querypanel.queryById('readedrate').setValue(rate);

            Ext.apply(store.proxy.extraParams, { 'usermsg': userMsg, 'orgid': orgId, 'status': status });
            store.currentPage = 1;
            store.load();
        });

        var status = querypanel.queryById('status').getSubmitValue();
        var rate = me.getReadedRate(status);
        querypanel.queryById('readedrate').setValue(rate);

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'username',
                type: 'string',
                mapping: 'username'
            }, {
                name: 'userorg',
                type: 'string',
                mapping: 'userorg'
            }, {
                name: 'readtime',
                type: 'string',
                mapping: 'readtime'
            }, {
                name: 'firstdt',
                type: 'string',
                mapping: 'firstdt'
            }, {
                name: 'userdept',
                type: 'string',
                mapping: 'userdept'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'WM/Archive/MyArchive/GetReadList?archivestype=' + me.archivestype + '&arcid=' + me.arcId,
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'ArcViewQkHelp',
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                },
                {
                    header: '名称',
                    dataIndex: 'username',
                    flex: 1,
                    sortable: false
                }, {
                    header: '组织',
                    dataIndex: 'userorg',
                    flex: 1,
                    sortable: false
                }, {
                    header: '部门',
                    dataIndex: 'userdept',
                    flex: 1,
                    sortable: false
                }, {
                    header: '阅读时间',
                    dataIndex: 'readtime',
                    flex: 1,
                    sortable: false
                }, {
                    header: '首次阅读时间',
                    dataIndex: 'firstdt',
                    flex: 1,
                    sortable: false
                }
            ],
            bbar: pagingbar
        });


        var checkTime = function (value) {
            if (value.indexOf('0001') > -1) {
                return '';
            }
            else {
                return value;
            }
        }

        grid.getColumn('readtime').renderer = checkTime;
        grid.getColumn('firstdt').renderer = checkTime;

        me.items = [querypanel, grid];

        me.callParent();
    },
    getReadedRate: function (status) {
        var me = this;
        var rate = '';
        Ext.Ajax.request({
            params: { 'arcid': me.arcId, 'archivestype': me.archivestype, 'status': status },
            url: C_ROOT + "WM/Archive/MyArchive/GetReadeRate",
            async: false,
            success: function (response) {
                rate = response.responseText;
            }
        });

        return rate;
    }
});

//反馈意见帮助
Ext.define("Ext.ng.ArcFeedBackHelp", {
    extend: 'Ext.window.Window',
    title: '反馈意见帮助',
    closable: true,
    resizable: false,
    draggable: false,
    modal: true,
    width: 700,
    height: 433,
    sdata: null,
    border: false,
    ORMMode: true,
    userid: '',
    doctype: '',
    arcid: '',
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {

        var me = this;

        if (Ext.isEmpty(me.userid) || Ext.isEmpty(me.userid) || Ext.isEmpty(me.userid)) {
            Ext.MessageBox.alert("提示", "必要参数缺少！");
            return;
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'Remark',
                type: 'string',
                mapping: 'Remark'
            }, {
                name: 'Feedbackdt',
                type: 'string',
                mapping: 'Feedbackdt'
            }, {
                name: 'UserName',
                type: 'string',
                mapping: 'UserName'
            }, {
                name: 'DeptName',
                type: 'string',
                mapping: 'DeptName'
            }, {
                name: 'OrgName',
                type: 'string',
                mapping: 'OrgName'
            }]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            pageSize: 10,
            model: 'model',
            autoLoad: true,
            url: C_ROOT + 'WM/Archive/ArcFeedBack/GetArcFeedBackList?doctype=' + me.doctype + '&arcid=' + me.arcid + '&userid=' + me.userid,
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            buskey: 'PhId', //对应的业务表主键属性
            stateId: 'ArcFeedBackHelp',
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'rownumberer',
                    header: '行号',
                    width: 35
                }, {
                    header: '操作员',
                    dataIndex: 'UserName',
                    flex: 1,
                    sortable: false
                }, {
                    header: '反馈意见',
                    dataIndex: 'Remark',
                    flex: 1,
                    sortable: false
                }, {
                    header: '反馈时间',
                    dataIndex: 'Feedbackdt',
                    flex: 1,
                    sortable: false
                }, {
                    header: '部门',
                    dataIndex: 'DeptName',
                    flex: 1,
                    sortable: false
                }, {
                    header: '组织',
                    dataIndex: 'OrgName',
                    flex: 1,
                    sortable: false
                }
            ],
            bbar: pagingbar
        });


        var checkTime = function (value) {
            if (value.indexOf('0001') > -1) {
                return '';
            }
            else {
                return value;
            }
        }

        grid.getColumn('Feedbackdt').renderer = checkTime;

        me.items = [grid];

        me.callParent();
    }
});

//关联公文选择
Ext.define('Ext.ng.ArchiveHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ArchiveHelp'],
    triggerCls: 'x-form-help-trigger',//框帮助按钮的样式
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    //archiveType: 'DOCDISPATCH',
    initComponent: function () {

        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected'); //定义值被选完的事件
    },
    onTriggerClick: function (eOption, ignoreBeforeEvent) { //ignoreBeforeEvent为true能手动弹出帮助
        var me = this;

        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击        

        //查询
        var querypanel = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            bodyPadding: '5 10 5 10',
            layout: {
                type: 'hbox'
            },
            defaults: {
                margin: '0 5 0 5'
            },
            items: [
                { xtype: 'label', text: '关键字：', margin: '4 0 0 0' },
                { xtype: 'textfield', width: 150, itemId: 'searchValue' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '查询',
                    handler: function () {
                        var searchValue = querypanel.queryById('searchValue').getValue();
                        Ext.apply(store.proxy.extraParams, { 'keyword': searchValue });
                        store.load();
                    }
                }
            ]
        });

        //定义模型
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhidArc',
                    type: 'string',
                    mapping: 'PhidArc'
                }, {
                    name: 'ArchivesType',
                    type: 'string',
                    mapping: 'ArchivesType'
                }, {
                    name: 'Cnum',
                    type: 'string',
                    mapping: 'Cnum'
                }, {
                    name: 'Cname',
                    type: 'string',
                    mapping: 'Cname'
                }
            ]
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 10,
            autoLoad: true,
            url: C_ROOT + 'WM/Archive/MyArchive/GetMyArchiveList'
        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            stateful: true,
            stateId: 'ArchiveHelp',
            store: store,
            //buskey: 'PhidArc', //对应的业务表主键属性               
            columnLines: true,
            columns: [Ext.create('Ext.grid.RowNumberer', { text: '行号', width: 35 }),
            {
                header: '编号',
                dataIndex: 'ArchivesType',
                flex: 1,
                sortable: false,
                hidden: true
            }, {
                header: '文号',
                dataIndex: 'Cnum',
                flex: 1,
                sortable: false,
                hidden: false
            }, {
                header: '标题',
                dataIndex: 'Cname',
                flex: 1,
                sortable: false,
                hidden: false
            }
            ],
            bbar: pagingbar
        });

        var buttons = [];

        buttons.push('->');
        buttons.push({ text: '确定', handler: function () { me.btnOk(me, grid, store, win); } });
        buttons.push({ text: '取消', handler: function () { win.close(); } });

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: '关联公文选择', //'Hello',
            border: false,
            height: me.helpHeight,
            width: me.helpWidth,
            layout: 'border',
            modal: true,
            constrain: true,
            items: [querypanel, grid], //[toolbar, queryProperty, tabPanel],
            buttons: buttons
        });
        win.show();

        grid.on('itemdblclick', function () {
            me.gridDbClick(me, grid, win);
        });

    },
    showHelp: function (eOption, ignoreBeforeEvent) {
        this.onTriggerClick(eOption, ignoreBeforeEvent);//忽略beforetriggerclick事件，手动弹出帮助
    },
    btnOk: function (_this, grid, store, win) {
        var pobj = new Object();

        var phidarc, archivestype, cnum, cname;

        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            phidarc = data[0].get('PhidArc');
            archivestype = data[0].get("ArchivesType");
            cname = data[0].get("Cname");
            cnum = data[0].get("Cnum");
            pobj.data = data[0].data;
        }

        win.hide();
        win.destroy();

        pobj.phidarc = phidarc;
        pobj.archivestype = archivestype;
        pobj.cnum = cnum;
        pobj.cname = cname;
        pobj.type = 'fromhelp';
        _this.fireEvent("helpselected", pobj);
    },
    gridDbClick: function (help, grid, win) {
        var pobj = new Object();

        var phidarc, archivestype, cnum, cname;

        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            phidarc = data[0].get('PhidArc');
            archivestype = data[0].get("ArchivesType");
            cname = data[0].get("Cname");
            cnum = data[0].get("Cnum");
            pobj.data = data[0].data;
        }

        win.hide();
        win.destroy();

        pobj.phidarc = phidarc;
        pobj.archivestype = archivestype;
        pobj.cnum = cnum;
        pobj.cname = cname;
        pobj.type = 'fromhelp';
        help.fireEvent("helpselected", pobj);
    }
});

//基于数据库配置的通用帮助
Ext.define('Ext.ng.RoleCbooHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.RoleCbooHelp'],
    triggerCls: 'x-form-help-trigger',//框帮助按钮的样式
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    initComponent: function () {

        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected'); //定义值被选完的事件



    },
    onTriggerClick: function (eOption, ignoreBeforeEvent) { //ignoreBeforeEvent为true能手动弹出帮助
        var me = this;

        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击   
        //查询
        var querypanel = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            bodyPadding: '5 10 5 10',
            layout: {
                type: 'hbox'
            },
            defaults: {
                margin: '0 5 0 5'
            },
            items: [
                { xtype: 'label', text: '关键字：', margin: '4 0 0 0' },
                { xtype: 'textfield', width: 150, itemId: 'searchValue' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '查询',
                    handler: function () {
                        var searchValue = querypanel.queryById('searchValue').getValue();
                        continuousLocationTreeNodeFunc(w3ProceTree, searchValue, 'name');
                    }
                }
            ]
        });

        //定义模型
        Ext.define('ActorCbooModel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'id',
                    type: 'string',
                    mapping: 'id'
                }, {
                    name: 'text',
                    type: 'string',
                    mapping: 'text'
                }
            ]
        });

        var w3ProceStore = Ext.create('Ext.data.TreeStore', {
            model: 'W3ProceModel',
            autoLoad: true,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'DMC/Rights/Role/GetRoleCbooTreeData',
                extraParams: {
                    parentphid: ''
                }
            }, listeners: {
                'beforeload': function (store, opration, eOpts) {
                    store.proxy.extraParams.parentphid = opration.id;
                }
            }
        });

        var w3ProceTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            id: 'w3ProceTree',
            rootVisible: false,
            store: w3ProceStore,
            multiSelect: true,
            cascade: 'children',
            viewConfig: {
                loadMask: true
            },
            hideHeaders: true,
            columns: [{
                xtype: 'treecolumn',
                text: '角色-组织',
                flex: 2,
                sortable: true,
                dataIndex: 'text'
            }, {
                text: '编号',
                flex: 1,
                dataIndex: 'id',
                hidden: true,
                sortable: true
            }],
            listeners: {
                checkchange: function (node, check) {
                    if (check) {
                        if (node.data.leaf) {//叶子节点处理
                            checkParent(node.parentNode, check);
                        }
                        else {//选择上级节点
                            if (!node.hasChildNodes()) {//孩子节点未加载
                                w3ProceTree.expandNode(node, true, function (optional) {
                                    for (var j = 0; j < node.childNodes.length; j++) {
                                        node.childNodes[j].set('checked', check);
                                    }
                                });
                            }
                            else {
                                for (var j = 0; j < node.childNodes.length; j++) {
                                    node.childNodes[j].set('checked', check);
                                }
                            }
                        }
                    }
                    else {
                        if (!node.hasChildNodes()) {
                            var parentNode = node.parentNode;
                            var flag = false;
                            for (var j = 0; j < parentNode.childNodes.length; j++) {
                                var checkedNode = parentNode.childNodes[j].get('checked');
                                if (checkedNode) {
                                    flag = true;
                                }
                            }
                            if (flag == false) {
                                //checkParent(parentNode, false);
                                checkParent(parentNode, false)
                            }
                        }
                        else {
                            for (var j = 0; j < node.childNodes.length; j++) {
                                node.childNodes[j].set('checked', check);
                            }
                        }
                    }
                }
            }
        });

        var buttons = [];

        buttons.push('->');
        buttons.push({ text: '确定', handler: function () { me.btnOk(me, w3ProceTree, w3ProceStore, win); } });
        buttons.push({ text: '取消', handler: function () { win.close(); } });

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: '角色组织帮助', //'Hello',
            border: false,
            height: me.helpHeight,
            width: me.helpWidth,
            layout: 'border',
            modal: true,
            constrain: true,
            items: [querypanel, w3ProceTree], //[toolbar, queryProperty, tabPanel],
            buttons: buttons
        });
        win.show();


        //级联父节点
        var checkParent = function (nextnode, check) {
            nextnode.set('checked', check);
            if (check) {
                if (nextnode.parentNode != null) {
                    checkParent(nextnode.parentNode, check);
                }
            }
        }


    },
    showHelp: function (eOption, ignoreBeforeEvent) {
        this.onTriggerClick(eOption, ignoreBeforeEvent);//忽略beforetriggerclick事件，手动弹出帮助
    },
    btnOk: function (_this, w3ProceTree, w3ProceStore, win) {
        var selNodels = w3ProceTree.getChecked();
        var arr = [];
        Ext.Array.each(selNodels, function (rcd) {
            if (rcd.data.id.indexOf('$') > -1) {
                var roleId = rcd.data.parentId;
                var parentNode = w3ProceStore.getNodeById(roleId);
                var roleName = parentNode.data.text;//角色名称
                var obj = { "roleCbooId": rcd.data.id, "roleName": roleName, "cbooName": rcd.data.text };
                arr.push(obj);
            }
        });
        var texts = "";
        var roles = [];
        var ids = [];
        Ext.Array.each(arr, function (item) {
            var RevActorId = item.roleCbooId;
            var roleId = RevActorId.substring(0, RevActorId.indexOf('$', 0));
            if (roles.indexOf(roleId, 0) < 0) {
                texts += item.roleName;
                var cbooText = "";
                Ext.Array.each(arr, function (rcd) {
                    if (rcd.roleCbooId.indexOf(roleId, 0) > -1) {
                        cbooText += rcd.cbooName + ",";
                    }
                });
                if (cbooText.length > 0) {
                    cbooText = cbooText.substring(0, cbooText.length - 1);
                }
                texts += "{" + cbooText + "},";
                roles.push(roleId);
            }
            ids.push(RevActorId);
        });
        if (texts.length > 0) {
            texts = texts.substring(0, texts.length - 1);
        }

        win.hide();
        win.destroy();

        var pobj = new Object();
        pobj.cno = ids.join(',');
        pobj.name = texts;
        pobj.type = 'fromhelp';
        _this.fireEvent("helpselected", pobj);

    }
});


//递归算法定位树节点
function locationTreeNodeFunc(tree, node, value, locationtype) {
    for (var i = 0; i < node.childNodes.length; i++) {
        var subNode = node.childNodes[i];

        var text = '';
        if (locationtype == 'code')
            text = subNode.data.id;
        else if (locationtype == 'name')
            text = subNode.data.text;
        if (text == value) {
            var nodeArr = new Array();
            getNeedExpandNode(subNode, nodeArr);
            doNeedExpandNode(nodeArr);

            if (subNode.hasChildNodes())
                subNode.expand();

            tree.getSelectionModel().select(subNode);
            break;
        } else {
            if (subNode.hasChildNodes()) {
                locationTreeNodeFunc(tree, subNode, value, locationtype);
            }
        }
    }
}
//获取所有需要展开的父节点
function getNeedExpandNode(curNode, nodeArr) {
    var parentNode;
    if (curNode.parentNode != undefined && curNode.parentNode != null) {
        if ((curNode.parentNode.isExpanded() == false) && (curNode.parentNode.hasChildNodes())) {
            parentNode = curNode.parentNode;
            if (parentNode.data.text != 'Root')
                nodeArr.push(parentNode);
        }
    }
    //递归获取所有的上级节点 
    if ((parentNode != undefined) && (parentNode.parentNode.data.text != 'Root'))
        getNeedExpandNode(parentNode, nodeArr);
}
//展开需要的所有父节点
function doNeedExpandNode(nodeArr) {
    //展开的时候只能从上往下逐级展开,所以倒序循环 
    for (var i = nodeArr.length - 1; i >= 0; i--) {
        var needExpandNode = nodeArr[i];
        needExpandNode.expand();
    }
}

var findindex = 0;
var recordvalue = "";
//连续定位
function continuousLocationTreeNodeFunc(tree, locationtext, locationtype) {
    if (Ext.isEmpty(locationtext)) return;
    if (recordvalue != locationtext) { //若前后值不一，从头开始查
        recordvalue = locationtext;
        findindex = 0;
    }
    var rootNode = tree.getRootNode();
    var arr = locationTreeNodeByFuzzy(rootNode, locationtext, findindex, locationtype); //模糊定位 
    if (arr != null) {
        var node = arr[0];
        findindex = arr[1];
        if (node != null) {
            var nodeArr = new Array();
            getNeedExpandNode(node, nodeArr);
            doNeedExpandNode(nodeArr);

            if (node.hasChildNodes())
                node.expand();
            tree.getSelectionModel().select(node);
        }
    }
}

//模糊定位树节点
function locationTreeNodeByFuzzy(node, name, index, locationtype) {
    var arr = new Array();
    var c = -1;
    while (node) {
        c++;
        var temp = node;
        var showName = '';
        if (locationtype == 'code')
            showName = node.data.id;
        else if (locationtype == 'name')
            showName = node.data.text;
        if (showName.indexOf(name) < 0 || c <= index) {
            if (!node.lastChild) {
                if (node.nextSibling)
                    node = node.nextSibling;
                else if (!node.nextSibling && node.parentNode)
                    node = node.parentNode.nextSibling;
                else
                    node = node.nextSibling;
            } else {
                node = node.firstChild;
            }

            if (!node) {
                node = temp;
                while (node.parentNode && node.parentNode.parentNode && !node.nextSibling) {
                    node = node.parentNode;
                }
                node = node.nextSibling;
            }
        } else {
            arr.push(node);
            arr.push(c);
            return arr;
        }
    }
    findindex = 0;//循环差（最后一个停顿了）
    return null;
};
Ext.define('Ext.ng.OfficePurchaseHelp', {
    extend: 'Ext.window.Window',
    title: '采购单帮助',
    autoScreen: true,
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    mutli: false,
    //draggable:false,
    width: 700,
    height: 400,
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.id = "officepurchasehelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        var ismutli = me.mutli == true ? "1" : "0";
        frame.src = C_ROOT + 'WM/Office/OfficeMainPurchase/PurchaseHelp?mutli=' + ismutli;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    },
    listeners: {

    }
});

Ext.define('Ext.ng.OfficeItemdataHelpWindow', {
    extend: 'Ext.window.Window',
    title: '办公用品帮助',
    autoScreen: true,
    closable: true,
    maximizable: false,
    resizable: true,
    modal: true,
    mutli: false,
    type: "",
    width: 800,
    height: 450,
    layout: 'fit',
    outEmp: "",
    initComponent: function () {
        var me = this;
        me.id = "officeitemdatahelp";
        var frame = document.createElement("IFRAME");
        frame.id = "fileframe";
        frame.frameBorder = 0;
        var ismutli = me.mutli == true ? "1" : "0";
        var type = me.type;
        frame.src = C_ROOT + 'WM/Office/OfficeItemdata/OfficeItemdataHelp?mutli=' + ismutli + "&type=" + type + "&outemp=" + me.outEmp;
        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;
        me.callParent(arguments);
        frame.parentContainer = me;
        me.addEvents('bntOk'); //定义值被选完的事件
    }
});

Ext.define('Ext.ng.OfficeItemdataHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.OfficeItemdataHelp'],
    triggerCls: 'x-form-help-trigger',//框帮助按钮的样式
    editable: false,
    mustInput: true,
    bussType: '',
    mutli: false,
    outEmp: '',
    initComponent: function () {

        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('beforetriggerclick');
        me.addEvents('helpselected'); //定义值被选完的事件
    },
    onTriggerClick: function () {

        var me = this;

        me.fireEvent('beforetriggerclick', me);

        if (me.readOnly || arguments.length == 3) {
            return; //arguments.length == 3，输入框上点击     
        }

        var win = Ext.create("Ext.ng.OfficeItemdataHelpWindow", {
            mutli: me.mutli,
            type: me.bussType,
            outEmp: me.outEmp,
            listeners: {
                bntOk: function (record) {
                    me.fireEvent('helpselected', record);
                }
            }
        });

        win.show();
    }
});

Ext.define("Ext.ng.SignatureWin", {
    extend: 'Ext.window.Window',
    title: '签章选择',
    closable: true,
    resizable: false,
    modal: true,
    width: 500,
    height: 300,
    sdata: null,
    border: false,
    currData: null,
    taskId: null,
    callback: Ext.emptyFn,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        Ext.Ajax.request({
            async: false,
            url: C_ROOT + 'WM/Archive/WmIoSignature/GetSignatureListByCurrentUser',
            success: function (response) {
                if (response.responseText == null || response.responseText == "")
                    return;
                var resp = Ext.JSON.decode(response.responseText);
                if (response.statusText == "OK") {
                    me.sdata = resp.Record;
                    //修改url成绝对路径
                    for (var i = 0; i < me.sdata.length; i++) {
                        me.sdata[i].MarkPath = C_ROOT + me.sdata[i].MarkPath;
                        me.sdata[i].checkedPwd = false;
                    }
                }
                else {
                    $WorkFlow.msgAlert('获取签章数据出错', resp.errorMsg);
                }
            }
        });
        var baseInfoForm = Ext.create('Ext.ng.TableLayoutForm', {
            columnsPerRow: 5,
            region: 'center',
            margin: '0 -60 0 0',
            fieldDefaults: {
                labelWidth: 40,
                anchor: '100%',
                margin: '3 10 3 0',
                msgTarget: 'side'
            },
            fields: [{
                xtype: 'ngComboBox',
                name: 'Cname',
                fieldLabel: '签章',
                readOnly: false,
                valueField: "PhId",
                displayField: 'Cname',
                queryMode: 'local',
                itemId: "combS",
                data: me.sdata,
                tabIndex: 2,
                colspan: 2,
                listeners: {
                    change: function (ctr, newValue) {

                        var d = this.data.filter(function (o) { return o.PhId == newValue; });
                        if (d.length > 0) {
                            baseInfoForm.down('#txtPwd').setValue('');
                            me.currData = d[0];
                            if (me.currData.MarkPass != "") {
                                baseInfoForm.down("#img").setSrc('');
                                baseInfoForm.down("#viewBtn").enable();
                            } else {
                                me.currData.checkedPwd = true;
                                baseInfoForm.down("#img").setSrc(me.currData.MarkPath);
                                baseInfoForm.down("#viewBtn").disable();
                            }
                        }
                    }
                }
            },
            {
                xtype: 'ngText',
                name: 'pwd',
                fieldLabel: '密码',
                inputType: 'password',
                readOnly: false,
                itemId: "txtPwd",
                tabIndex: 2,
                colspan: 2
            }, {
                xtype: 'button',
                name: 'addSelect',
                itemId: 'viewBtn',
                text: '预览',
                margin: '3 0 0 0',
                width: 40,
                colspan: 1,
                handler: Ext.bind(function () {
                    if (!me.currData) {
                        $WorkFlow.msgAlert('提示', '请选择签章！');
                        return;
                    }
                    var phid = me.currData.PhId;
                    if (Ext.isEmpty(phid)) {
                        $WorkFlow.msgAlert('提示', '请选择签章！');
                        return;
                    }
                    var pwd = baseInfoForm.down('#txtPwd').getValue();
                    if (Ext.isEmpty(pwd)) {
                        $WorkFlow.msgAlert('提示', '请输入密码！');
                        return;
                    }
                    Ext.Ajax.request({
                        async: false,
                        url: C_ROOT + 'WM/Archive/WmIoSignature/GetSignatureInfogByPassword',
                        params: { 'id': phid, 'password': pwd },
                        success: function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (response.statusText == "OK") {
                                if (resp.Record) {
                                    me.currData.checkedPwd = true;
                                    baseInfoForm.down("#img").setSrc(me.currData.MarkPath);
                                    baseInfoForm.down("#viewBtn").disable();
                                }
                                else {
                                    $WorkFlow.msgAlert('提示', '提示密码错误');
                                }
                            }
                            else {
                                $WorkFlow.msgAlert('校验密码出错！', resp.errorMsg);
                            }
                        }
                    });
                })
            }, {
                xtype: 'panel',
                colspan: 5,
                style: 'border: 2px solid #A2A2A2',
                width: 460,
                height: 185,
                autoScroll: true,
                items: [{
                    xtype: 'image',
                    name: 'signature',
                    autoScroll: true,
                    region: 'south',
                    itemId: 'img'
                }]
            }]
        });
        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "help_query", text: "确定", iconCls: 'add'
                },
                {
                    itemId: "help_close", text: "取消", iconCls: 'cross'
                }
            ]
        });
        me.items = [rolltoolbar, baseInfoForm];
        me.callParent();

        rolltoolbar.items.get('help_query').on('click', function () {
            if (me.currData == null) {
                $WorkFlow.msgAlert('提示', '请选择签章！');
                return;
            }
            if (me.currData.checkedPwd == false) {
                $WorkFlow.msgAlert('提示', '请输入密码！');
                return;
            }
            me.callback(me.currData.PhId, me.currData.MarkPath);
            me.destroy();
        });
        rolltoolbar.items.get('help_close').on('click', function () {
            me.destroy();
        });
        me.on("beforeshow", $winBeforeShow);
        me.on("beforeclose", $winBeforeClose);
    }
});

//关闭Ext.window.Window窗口
var CloseWindow = function () {
    var currentWin = window;
    while (top != currentWin) {
        var parentExt = currentWin.parent.Ext;
        var fElement = parentExt.get(currentWin.frameElement);
        var windowElement = fElement.up('div.x-window');
        if (windowElement) {
            var winId = windowElement.id;
            var extWin = parentExt.getCmp(winId);
            extWin.close();
            return true;
        } else {
            currentWin = currentWin.parent;
        }
    }
    alert("窗口关闭失败！");
    return false;
}

Ext.define('Ext.ng.WMnumber', {
    extend: 'Ext.form.field.Number',
    alias: ['widget.ngWMnumber'],
    fieldStyle: "text-align:right",

    /**
     * 数值是否加入分节符，是以逗号分隔比如： 123,452,000.00
     */
    allowSection: false,
    /**
     * 数值分节符，为逗号(不要修改)
     */
    sectionSeparator: ',',

    /**
     * 零值是否显示，如果设置为true，则 0 值不显示。不显示的时候如果该字段不能为空，则通不过检测
     */
    zeroDisplayNone: false,

    /**
     * 是否是百分比
     * 
     * true : 数值0.56 会显示成 56% , 提交的时候仍然是0.56
     * 
     * 小数位数是指相对于原始值的，0.56的话小数位置是2，显示56%; 0.5678的小数位置为4,显示56.78%
     * 
     */
    isPercent: false,

    /**
     * 百分比符号，为%(不要修改)
     */
    percentSign: '%',

    /**
     * 对于百分比的数值，以下二个设置，都是对应于原始的小数值，因此对于12.34%，应设置成下面的样式：
     * 
     * decimalPrecision : 4,
     * 
     * step : 0.0001,
     * 
     * 对于 23％，应设置成
     * 
     * decimalPrecision : 2,
     * 
     * step : 0.01,
     * 
     */

    initComponent: function () {
        var me = this;
        //if (me.isPercent)
        //    me.allowSection = true;
        //if (me.allowSection)
        //    me.baseChars = me.baseChars + me.sectionSeparator;
        me.callParent();
    },

    /**
    * 
    */
    fixPrecision: function (value) {
        var me = this,
            nan = isNaN(value),
            precision = me.decimalPrecision;

        if (nan || !value) {
            return nan ? '' : value;
        } else if (!me.allowDecimals || precision <= 0) {
            precision = 0;
        }

        return parseFloat(Ext.Number.toFixed(parseFloat(value), precision));
    },

    beforeBlur: function () {
        var me = this,
            v = me.parseValue(me.getRawValue());

        if (!Ext.isEmpty(v)) {
            me.setValue(v);
        }
    },
    /**
     * 输入的数值转换成值
    */
    rawToValue: function (rawValue) {
        var value = this.fixPrecision(this.parseValue(rawValue));
        if (value === null) {
            value = rawValue || null;
        }
        if (this.isPercent)
            value /= 1.;
        // rawValue = value == '0' ? '' : value;
        if (value == '')
            return ' ';
        else {
            return value += ('' + this.percentSign);
        }
    },
    /*
     * 字符转换成录入的值
     */
    valueToRaw: function (value) {
        var me = this, decimalSeparator = me.decimalSeparator;
        value = me.parseValue(value);
        value = me.fixPrecision(value);
        if (me.isPercent)
            // value *= 100;
            value = Ext.isNumber(value) ? value : parseFloat(String(value).replace(
                decimalSeparator, '.'));
        //  if (this.allowSection)
        value = isNaN(value) ? '' : me.valueToRawWithSection(value).replace(
            '.', decimalSeparator);
        //else
        //    value = isNaN(value) ? '' : String(value).replace('.',
        //            decimalSeparator);
        return value;
    },

    /**
     * 将数值转换成有分节的字答卷串，百分比的话，加入 ' %'
     */
    valueToRawWithSection: function (value) {
        var me = this;
        if (me.zeroDisplayNone && !value)
            return '';
        if (this.allowDecimals) {
            if (me.isPercent)
                // 这里已经是除了100的数值，在转换的时候精度减少2位
                value = value.toFixed(Math.max(0, me.decimalPrecision - 2));
            // else
            // value = value.toFixed(me.decimalPrecision);
        }
        value = String(value);
        var ps = value.split('.'), whole = ps[0], sub = ps[1] ? '.' + ps[1]
            : '', r = /(\d+)(\d{3})/;
        while (r.test(whole)) {
            whole = whole.replace(r, '$1' + me.sectionSeparator + '$2');
        }
        value = whole + sub;
        if (me.isPercent && value != '' && value != '0')
            value += ('' + me.percentSign);
        return value;
    },

    getErrors: function (value) {
        value = arguments.length > 0 ? String(value).replace(/,/g, '').replace('%', '').replace(' ', '')
            : this.processRawValue(this.getRawValue());
        value = value == '' || isNaN(value) ? 0 : this.parseValue(value);// this.parseValue(value);
        return this.callParent([value]);
    },

    getSubmitValue: function () {
        var me = this, value = me.callParent();
        if (!me.submitLocaleSeparator) {
            value = value.replace(me.decimalSeparator, '.');
        }
        if (me.isPercent) {
            // 如果是百分比，提交的数据除以100/ 100.
            value = me.parseValue(value);
            value = String(value);
        }
        return value;
    },

    parseValue: function (value) {
        // 取消掉字符串里的 ,% 这二个字符。
        value = parseFloat(String(value).replace(this.decimalSeparator, '.')
            .replace(/,/g, '').replace('%', '').replace(' ', ''));
        return isNaN(value) ? null : value;
    },

    processRawValue: function (value) {
        var me = this, stripRe = me.stripCharsRe, mod, newValue;
        // 下面一句不加的话 bind 在有分节符或%的时候将不能正常工作
        value = value.replace(/,/g, '').replace('%', '');
        if (stripRe) {
            if (!stripRe.global) {
                mod = 'g';
                mod += (stripRe.ignoreCase) ? 'i' : '';
                mod += (stripRe.multiline) ? 'm' : '';
                stripRe = new RegExp(stripRe.source, mod);
            }
            newValue = value.replace(stripRe, '');
            if (newValue !== value) {
                // 这一句要去掉，不然分节符就没有了
                // me.setRawValue(newValue);
                if (me.lastValue === value) {
                    me.lastValue = newValue;
                }
                value = newValue;
            }
        }
        return value;
    }

});


Ext.define('Ext.ng.DianpHelp', {
    extend: 'Ext.window.Window',
    title: '点评',
    autoScreen: true,
    closable: true,
    constrain: true,//限制拖动范围
    maximizable: false,
    resizable: true,
    modal: true,
    detailId: '',
    tabtype: '',
    dianpCont: '',
    draggable: false,
    width: 300,
    height: 200,
    layout: 'fit',
    items: [],
    initComponent: function () {
        var me = this;
        var empno = '';
        me.addEvents('bntOk'); //定义值被选完的事件  
        //布局
        var txt = Ext.create('Ext.form.field.TextArea', {
            grow: true,
            disabled: me.tabtype == 'mysummarytab' || me.tabtype == 'myplantab',
            name: 'message',
            anchor: '100%'

        });
        if (me.dianpCont == '') {
            Ext.Ajax.request({
                params: { 'detailId': me.detailId, 'tabtype': me.tabtype },
                url: C_ROOT + "wm/Affairs/PlanSummary/GetDianP",
                async: false,
                success: function (response) {
                    txt.setValue(response.responseText);
                }
            });
        } else {
            txt.setValue(me.dianpCont)
        }
        me.items = txt;
        me.buttons = [{
            text: '确认',
            hidden: me.tabtype == 'mysummarytab' || me.tabtype == 'myplantab',
            handler: function () {
                Ext.Ajax.request({
                    params: { 'detailId': me.detailId, 'tabtype': me.tabtype, 'cont': encodeURI(txt.value) },
                    url: C_ROOT + "wm/Affairs/PlanSummary/DianP",
                    async: false,
                    success: function (response) {
                        me.fireEvent('bntOk', txt.value);
                        //Ext.MessageBox.alert('提示', resp.Msg);
                        me.close();
                    }
                });
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];
        me.callParent();
    },
    listeners: {

    }
});


Ext.define('Ext.ng.PlanTemlptWindow', {
    extend: 'Ext.window.Window',
    title: '总计计划模板帮助',
    closable: true,
    resizable: false,
    modal: true,
    height: 400,
    width: 600,
    border: 0,
    help: "",
    layout: 'border',
    constrain: true,
    callback: null,
    initComponent: function () {
        var me = this;
        var store;
        var toolbar = Ext.create('Ext.form.Panel', {
            region: 'north',
            border: false,
            frame: true,
            defaults: {
                margin: '0 5 0 5'
            },
            layout: {
                type: 'hbox'
            },
            items: [
                { xtype: 'textfield', width: 300, itemId: 'keyword' },
                {
                    xtype: 'button',
                    itemId: 'search',
                    text: '搜索',
                    handler: function () {
                        var cname = toolbar.queryById('keyword').getValue();
                        Ext.apply(store.proxy.extraParams, { 'searchkey': cname });
                        store.load();
                    }
                }
            ]
        });
        //定义模型
        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'PhId',
                    type: 'string', //因为页面不支持int64，所以这里做特殊处理
                    mapping: 'PhId'
                }, {
                    name: 'Cname',
                    type: 'string',
                    mapping: 'Cname'
                }, {
                    name: 'Cno',
                    type: 'string',
                    mapping: 'Cno'
                }, {
                    name: 'Stat',
                    type: 'string',
                    mapping: 'Stat'
                }, {
                    name: 'Ctype',
                    type: 'string',
                    mapping: 'Ctype'
                }
            ]
        });

        store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 25,
            autoLoad: true,
            url: C_ROOT + 'wm/Affairs/PlanTemplate/GetPlanTeml'
        });
        store.on('beforeload', function (store) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (me.help.outFilter) {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.help.outFilter) });
            } if (me.help.likeFilter) {
                Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.help.likeFilter) });
            }
            if (me.help.leftLikeFilter) {
                Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.help.leftLikeFilter) });
            }
            if (me.help.clientSqlFilter) {
                Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.help.clientSqlFilter });
            }

        });

        var pagingbar = Ext.create('Ext.ng.PagingBar', {
            store: store
        });

        var templgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            stateful: true,
            stateId: 'c784b7d0-3997-4601-bc00-309c9ebcbe1111172',
            store: store,
            forceFit: true,
            buskey: 'PhId', //对应的业务表主键属性               
            columnLines: true,
            columns: [
                {
                    xtype: 'rownumberer',
                    text: '行号',
                    stateId: 'grid_lineid',
                    width: 35
                },
                {
                    header: '模板编码',
                    dataIndex: 'Cno',
                    flex: 1,
                    sortable: false,
                    hidden: false
                }, {
                    header: '模板名称',
                    dataIndex: 'Cname',
                    flex: 2,
                    sortable: false,
                    hidden: false
                }
            ],
            bbar: pagingbar
        });
        me.items = [
            toolbar, {
                xtype: 'panel',
                region: 'center',
                autoScroll: false,
                layout: 'border',
                border: 0,
                items: [templgrid]
            }
        ];
        me.buttons = [
            '->',
            {
                text: '确定', handler: function () {
                    if (me.callback != null) {
                        me.gridDbClick(me.help, templgrid, this);
                    } else {

                        Ext.MessageBox.alert('', '请选择数据.');
                        return;
                    }
                }
            },
            { text: '取消', handler: function () { me.close(); } }
        ];

        templgrid.on('itemdblclick', function () {
            me.gridDbClick(me.help, templgrid, this);
        });
        me.callParent();

    },
    gridDbClick: function (help, grid, win) {
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var code = data[0].get(help.valueField);
            var name = data[0].get(help.displayField);
            for (var i = 1; i < data.length; i++) {
                code += "," + data[i].get(help.valueField);
                name += "," + data[i].get(help.displayField);
            }

            var obj = new Object();
            obj[help.valueField] = code;

            if (help.displayFormat) {
                obj[help.displayField] = Ext.String.format(help.displayFormat, code, name);
            } else {
                obj[help.displayField] = name;
            }

            Ext.define('richhelpModel', {
                extend: 'Ext.data.Model',
                fields: [{
                    name: help.valueField,
                    type: 'string',
                    mapping: help.valueField
                }, {
                    name: help.displayField,
                    type: 'string',
                    mapping: help.displayField
                }
                ]
            });

            var valuepair = Ext.create('richhelpModel', obj);
            help.setValue(valuepair);
            this.hide();
            this.destroy();

            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = data[0].data;
            help.fireEvent('helpselected', pobj);
        }
    }

});
Ext.define('Ext.ng.PlanTemplateHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngPlanTemplateHelp'],
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    selectMode: 'Single',
    multiSelect: false,
    needBlankLine: false,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    seletArr: null,
    valueField: "PhId",
    displayField: "Cname",
    userCodeField: "Cno",
    initComponent: function () {
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.addEvents('helpselected');
        me.addEvents('beforetriggerclick');
    },
    onTriggerClick: function () {
        var me = this;
        var win = me.createHelpWindow();
        win.help = me;

        me.fireEvent('beforetriggerclick', me);

        if (!Ext.isEmpty(me.rawValue) && me.value != null) {
            var arr = me.seletArr;
            Ext.Array.each(arr, function (model) {
            });
        }
        //有数值时的初始化
        win.callback = function (grid) {
            var code = "";
            var name = "";
            var pobj = new Object();
            var df = me.displayField;
            var vf = me.valueField;
            var select = grid.getSelectionModel().getSelection();
            if (select.length <= 0) {
                Ext.MessageBox.alert('', '请选择数据.');
                return;
            }
            Ext.Array.each(select, function (model) {
                code = code + model.data[vf] + ",";
                name = name + model.data[df] + ",";
            });
            code = code.substring(0, code.length - 1);
            name = name.substring(0, name.length - 1);
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

            var valuepair = Ext.create('richhelpModel', obj);
            me.setValue(valuepair); //必须这么设置才能成功

            win.hide();
            win.destroy();

            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = select;
            me.seletArr = select;
            me.fireEvent('helpselected', pobj);
        };
        win.show();
    },
    createHelpWindow: function () {
        /////创建帮助弹窗
        var win = Ext.create("Ext.ng.PlanTemlptWindow");
        return win;
    },
    showHelp: function () {
        this.onTriggerClick();
    }
});

//合并单元格
function WMmergeGrid(grid, colIndexArray, isAllSome, flag) {
    //isAllSome = isAllSome == undefined ? true : isAllSome; // 默认为true

    // 1.是否含有数据
    var gridView = document.getElementById(grid.getView().getId() + '-body');
    if (gridView == null) {
        return;
    }

    // 2.获取Grid的所有tr
    var trArray = [];
    if (grid.layout.type == 'table') { // 若是table部署方式，获取的tr方式如下
        trArray = gridView.childNodes;
    } else {
        trArray = gridView.getElementsByTagName('tr');
    }

    // 3.进行合并操作
    if (isAllSome) { // 3.1 全部列合并：只有相邻tr所指定的td都相同才会进行合并
        var lastTr = trArray[0]; // 指向第一行
        // 1)遍历grid的tr，从第二个数据行开始
        for (var i = 1, trLength = trArray.length; i < trLength; i++) {
            var thisTr = trArray[i];
            var isPass = true; // 是否验证通过
            // 2)遍历需要合并的列
            for (var j = 0, colArrayLength = colIndexArray.length; j < colArrayLength; j++) {
                var colIndex = colIndexArray[j];
                // 3)比较2个td的列是否匹配，若不匹配，就把last指向当前列
                if (lastTr.childNodes[colIndex].innerText != thisTr.childNodes[colIndex].innerText) {
                    lastTr = thisTr;
                    isPass = false;
                    break;
                }
            }

            // 4)若colIndexArray验证通过，就把当前行合并到'合并行'
            if (isPass) {
                for (var j = 0, colArrayLength = colIndexArray.length; j < colArrayLength; j++) {
                    var colIndex = colIndexArray[j];
                    // 5)设置合并行的td rowspan属性
                    if (lastTr.childNodes[colIndex].hasAttribute('rowspan')) {
                        var rowspan = lastTr.childNodes[colIndex].getAttribute('rowspan') - 0;
                        rowspan++;
                        lastTr.childNodes[colIndex].setAttribute('rowspan', rowspan);
                    } else {
                        lastTr.childNodes[colIndex].setAttribute('rowspan', '2');
                    }
                    // lastTr.childNodes[colIndex].style['text-align'] = 'center';; // 水平居中
                    lastTr.childNodes[colIndex].style['vertical-align'] = 'middle';; // 纵向居中
                    lastTr.childNodes[colIndex].setAttribute('rowspan', '2');
                    thisTr.childNodes[colIndex].style.display = 'none';
                }
            }
        }
    } else { // 3.2 逐个列合并：每个列在前面列合并的前提下可分别合并
        // 1)遍历列的序号数组
        for (var i = 0, colArrayLength = colIndexArray.length; i < colArrayLength; i++) {
            var colIndex = colIndexArray[i];
            var lastTr = trArray[0]; // 合并tr，默认为第一行数据
            // 2)遍历grid的tr，从第二个数据行开始
            for (var j = 1, trLength = trArray.length; j < trLength; j++) {
                var thisTr = trArray[j];
                // 3)2个tr的td内容一样
                if (lastTr.childNodes[colIndex].innerText == thisTr.childNodes[colIndex].innerText) {
                    // 4)若前面的td未合并，后面的td都不进行合并操作
                    if (i > 0 && thisTr.childNodes[colIndexArray[i - 1]].style.display != 'none') {
                        lastTr = thisTr;
                        continue;
                    } else {
                        // 5)符合条件合并td
                        if (lastTr.childNodes[colIndex].hasAttribute('rowspan')) {
                            var rowspan = lastTr.childNodes[colIndex].getAttribute('rowspan') - 0;
                            rowspan++;
                            lastTr.childNodes[colIndex].setAttribute('rowspan', rowspan);
                            if (flag) {
                                lastTr.childNodes[0].setAttribute('rowspan', rowspan);
                            }
                        } else {
                            lastTr.childNodes[colIndex].setAttribute('rowspan', '2');
                            if (flag) {
                                //lastTr.childNodes[0].removeAttribute('rowspan');
                                lastTr.childNodes[0].setAttribute('rowspan', '2');
                            }
                        }
                        // lastTr.childNodes[colIndex].style['text-align'] = 'center';; // 水平居中
                        lastTr.childNodes[colIndex].style['vertical-align'] = 'middle';; // 纵向居中
                        thisTr.childNodes[colIndex].style.display = 'none'; // 当前行隐藏
                        if (flag) {
                            thisTr.childNodes[0].style.display = 'none'; // 当前行隐藏

                        }
                    }
                } else {
                    // 5)2个tr的td内容不一样
                    lastTr = thisTr;
                }
            }
        }
    }
}

Ext.define('Ext.ng.WaterMarkWindow', {
    extend: 'Ext.window.Window',
    title: '插入水印',
    border: false,
    closeAction: 'hide',
    height: 400,
    width: 600,
    layout: 'border',
    modal: true,
    help: "",
    constrain: true,
    callback: null,
    selectedValue: "",
    initComponent: function () {
        var me = this;
        var sf = Ext.create('Ext.data.Store', {
            fields: ['code', 'name'],
            data: [
                { "code": "自动", "name": "自动" },
                { "code": "500%", "name": "500%" },
                { "code": "200%", "name": "200%" },
                { "code": "150%", "name": "150%" },
                { "code": "100%", "name": "100%" },
                { "code": "50%", "name": "50%" }
            ]
        });
        var fontStore = Ext.create('Ext.data.Store', {
            fields: ['code', 'name'],
            data: [
                { "code": "Arial Unicode MS", "name": "Arial Unicode MS" },
                { "code": "Batang", "name": "Batang" },
                { "code": "BatangChe", "name": "BatangChe" },
                { "code": "DFKai-SB", "name": "DFKai-SB" },
                { "code": "Dotum", "name": "Dotum" },
                { "code": "DotumChe", "name": "DotumChe" },
                { "code": "Malgun Gothic", "name": "Malgun Gothic" },
                { "code": "Meiryo", "name": "Meiryo" },
                { "code": "Microsoft JhengHei", "name": "Microsoft JhengHei" },
                { "code": "Microsoft JhengHei Light", "name": "Microsoft JhengHei Light" },
                { "code": "MingLiU", "name": "MingLiU" },
                { "code": "方正等线", "name": "方正等线" },
                { "code": "方正舒体", "name": "方正舒体" },
                { "code": "方正姚体", "name": "方正姚体" },
                { "code": "仿宋", "name": "仿宋" },
                { "code": "黑体", "name": "黑体" },
                { "code": "华文彩云", "name": "华文彩云" },
                { "code": "华文仿宋", "name": "华文仿宋" },
                { "code": "华文行楷", "name": "华文行楷" },
                { "code": "华文琥珀", "name": "华文琥珀" },
                { "code": "华文楷体", "name": "华文楷体" },
                { "code": "华文隶书", "name": "华文隶书" },
                { "code": "华文宋体", "name": "华文宋体" },
                { "code": "华文细黑", "name": "华文细黑" },
                { "code": "华文中宋", "name": "华文中宋" },
                { "code": "楷体", "name": "楷体" },
                { "code": "隶书", "name": "隶书" },
                { "code": "宋体", "name": "宋体" },
                { "code": "微软雅黑", "name": "微软雅黑" },
                { "code": "新宋体", "name": "新宋体" },
                { "code": "幼圆", "name": "幼圆" }
            ]
        });

        var sizeStore = Ext.create('Ext.data.Store', {
            fields: ['code', 'name'],
            data: [
                { "code": "36", "name": "36" },
                { "code": "40", "name": "40" },
                { "code": "44", "name": "44" },
                { "code": "48", "name": "48" },
                { "code": "54", "name": "54" },
                { "code": "60", "name": "60" },
                { "code": "66", "name": "66" },
                { "code": "72", "name": "72" },
                { "code": "80", "name": "80" },
                { "code": "90", "name": "90" },
                { "code": "96", "name": "96" },
                { "code": "105", "name": "105" },
                { "code": "120", "name": "120" },
                { "code": "144", "name": "144" }
            ]
        });
        var mstform = Ext.create('Ext.ng.TableLayoutForm', {
            region: 'north',
            frame: true,
            split: true,
            Width: 400,
            buskey: 'PhId',
            otype: 'add',
            autoScroll: true,
            columnsPerRow: 3,
            fieldDefaults: {
                labelWidth: 87,
                anchor: '100%',
                margin: '0 10 5 0',
                msgTarget: 'side'
            },
            fields: [
                {
                    xtype: 'radio',
                    boxLabel: '无水印',
                    name: 'Ctype',
                    inputValue: '0',
                    colspan: 3
                }, {
                    xtype: 'radio',
                    boxLabel: '图片水印',
                    name: 'Ctype',
                    inputValue: '1',
                    colspan: 3
                }, {
                    xtype: 'filefield',
                    fieldLabel: '选择图片',
                    langKey: 'PicPath',
                    name: 'PicPath',
                    itemId: 'PicPath',
                    id: 'PicPath',
                    buttonText: '选择图片上传',
                    colspan: 3
                }, {
                    xtype: 'combobox',
                    fieldLabel: '缩放',
                    itemId: 'Persent',
                    name: 'Persent',
                    store: sf,
                    queryMode: 'local',
                    valueField: 'code',
                    colspan: 1
                }, {
                    xtype: 'checkbox',
                    fieldLabel: '冲蚀',
                    itemId: 'IsCs',
                    name: 'IsCs',
                    colspan: 1
                }, {
                    xtype: 'hiddenfield',
                    fieldLabel: '',
                    colspan: 1
                }, {
                    xtype: 'radio',
                    boxLabel: '文字水印',
                    name: 'Ctype',
                    inputValue: '2',
                    colspan: 3
                }, {
                    xtype: 'ngText',
                    fieldLabel: '文字',
                    name: 'Cname',
                    itemId: 'Cname',
                    colspan: 3
                }, {
                    xtype: 'combobox',
                    fieldLabel: '字体',
                    itemId: 'FontStyle',
                    name: 'FontStyle',
                    store: fontStore,
                    queryMode: 'local',
                    valueField: 'code',
                    colspan: 3
                }, {
                    xtype: 'combobox',
                    fieldLabel: '字号',
                    itemId: 'FontSize',
                    name: 'FontSize',
                    store: sizeStore,
                    queryMode: 'local',
                    valueField: 'code',
                    colspan: 1
                }, {
                    xtype: 'colorpicker',
                    fieldLabel: '颜色',
                    itemId: 'FontColor',
                    name: 'FontColor',
                    colspan: 1
                }, {
                    xtype: 'checkbox',
                    fieldLabel: '半透明',
                    itemId: 'IsTm',
                    name: 'IsTm',
                    colspan: 1
                }, {
                    xtype: 'radiogroup',
                    fieldLabel: '板式',
                    columns: 2,
                    vertical: true,
                    items: [
                        { boxLabel: '对角线', name: 'Bs', inputValue: '1', checked: true },
                        { boxLabel: '水平', name: 'Bs', inputValue: '2' }
                    ]
                },
                {
                    xtype: 'container',
                    hidden: true,
                    items: [
                        {
                            xtype: 'hiddenfield',
                            fieldLabel: '主键',
                            name: 'PhId',
                            id: 'PhId'
                        }
                    ]
                }
            ]
        });

        me.items = [mstform];
        me.buttons = [
            '->',
            {
                text: '确定',
                handler: function () {
                    me.close();
                }
            },
            { text: '取消', handler: function () { me.close(); } }
        ];
        me.callParent();

    }
});