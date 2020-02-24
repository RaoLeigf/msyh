//通用项目组织关系树
Ext.define("Ext.Gc.CommonLeftLayout", {
    extend: 'Ext.panel.Panel',
    alias: 'widget.CommonLeftLayout',
    defaults: {
        split: true,
        bodyStyle: 'padding:1px'
    },
    layout: 'border',
    showTreeProj: true,//组织树上带出项目
    width: 200,
    split: true,
    border: false,
    cntmodel: '',
    orgfilter: '',
    typefilter: '',
    belongorg: '',
    belongproj: DefaultPc,
    showIncludeSub: true,
    funcKey: '',
    orgtree_d: '',//返回orgTree
    initComponent: function () {
        var me = this;
        me.initParam();
        //存储旧节点信息
        var lastnode;
        var lastsubnode;
        var lastsubprojnode;
        var lastparentnode;

        var orgTree = Ext.create('Ext.ng.BusOrgTreePanel', {
            id: 'orgTree',
            region: 'center',//默认布局
            split: false,//左右拖动
            collapsible: false,// 折叠
            showTreeProj: me.showTreeProj,
            autoSelectFirstNode: true,//第一次加载树的时候是否自动选择第一个节点
            autoSelectCurOrg: true,
            showIncludeSub: me.showIncludeSub,
            //第一次加载树的时候是否自动选择当前组织,优先级高于autoSelectFirstNode
            width: 220,
            funcKey: me.funcKey,
            listeners: {
                treeNodeChange: function (curNode, subOrgNode, subProjNode, parentNode) {
                    //若t为false表示grid已修改不允许切换
                    var org = this;
                    var t = true;
                    if (me.beforechange() == false) {
                        t = me.beforechange();
                    }
                    if (t == true) {
                        lastnode = curNode;
                        lastsubnode = subOrgNode;
                        lastsubprojnode = subProjNode;
                        lastparentnode = parentNode;
                    }
                    else {
                        curNode = lastnode;
                        subOrgNode = lastsubnode;
                        subProjNode = lastsubprojnode;
                        parentNode = lastparentnode;
                        org.queryById('treePanel').getSelectionModel().select(curNode);//切回原节点
                        return t;
                    }
                    var subOrg = "";
                    var subProj = "";
                    var curorg = "";
                    var curproj = "";
                    var parentId = "";
                    if (subOrgNode.length > 0) {
                        for (var i = 0 ; i < subOrgNode.length ; i++) {
                            if (subOrg == "") {
                                subOrg = subOrgNode[i].data['PhId'];
                            }
                            else {
                                subOrg += "," + subOrgNode[i].data['PhId'];
                            }
                        }
                    }
                    if (subProjNode.length > 0) {
                        for (var i = 0 ; i < subProjNode.length ; i++) {
                            if (subProj == "") {
                                subProj = subProjNode[i].data['PhId'];
                            }
                            else {
                                subProj += "," + subProjNode[i].data['PhId'];
                            }
                        }
                    }
                    if (curNode.raw.nodeType == 'PRO') {
                        curproj = curNode.raw.PhId;
                        me.belongproj = curproj;
                    }
                    else {
                        curorg = curNode.raw.PhId
                        me.belongproj = DefaultPc;
                        subProj = "";
                    }

                    if (parentNode != null) {
                        parentId = parentNode.data['PhId'];
                    }
                    if (curorg == "") {
                        me.belongorg = parentId;
                    } else {
                        me.belongorg = curorg;
                    }
                    var str = "#type:" + curNode.raw.nodeType + "#curOrg:" + curorg + "#subOrg:" + subOrg + "#curProj:" + curproj + "#cubProj:" + subProj + "#parentNode:" + parentId;
                    me.orgfilter = str;
                    me.changeCallBack();
                    

                    if (window.EPMEvent) {
                        window.EPMEvent.fireEvent(this, "ProjectChanged", {
                            org: me.belongorg,
                            proj: me.belongproj
                        });
                    }

                }
            }
        });
        me.orgtree_d = orgTree;
        if (me.cntmodel != '') {
            Ext.define('typemodel', {
                extend: 'Ext.data.Model',
                fields: [
                    {
                        name: 'PhId',
                        type: 'System.Int64',
                        mapping: 'PhId'
                    }, {
                        name: 'text',
                        type: 'System.String',
                        mapping: 'text'
                    }, {
                        name: 'id',
                        type: 'System.String',
                        mapping: 'id'
                    }
                ]
            });

            var typeStore = Ext.create('Ext.data.TreeStore', {
                model: typemodel,
                autoLoad: false,
                proxy: {
                    type: 'ajax',
                    url: '../CntType/GetCntTypeTreeData?model=' + me.cntmodel
                }
            });

            var typeTree = Ext.create('Ext.tree.TreePanel', {
                id: 'typetree',
                frame: true,
                split: true,
                width: 200,
                height: 150,
                region: 'north',
                store: typeStore,
                collapsible: false,
                useArrows: true,
                border: false,
                rootVisible: true,
                hideHeaders: true,
                autoScroll: true,
                columns: [{
                    text: '物理主键',
                    flex: 0,
                    sortable: false,
                    dataIndex: 'PhId',
                    hideable: false,
                    hidden: true
                }, {
                    text: '合同类型',
                    flex: 1,
                    xtype: 'treecolumn',
                    dataIndex: 'text',
                    hidden: false,
                    hideable: false,
                    align: 'left'
                }],
                root: {
                    PhId: -9999,
                    id: 'cnttype-*',
                    text: CommLang.TreeCntType || '合同类型',
                    expanded: true
                },
                listeners: {
                    selectionchange: function (m, selected, eOpts) {
                        var sel = selected[0].data['text'];
                        var id = selected[0].data['id'];
                        var phid = selected[0].data['PhId'];
                        var expanded = selected[0].data['expanded'];
                        var type = id.substring(0, 7);
                        var jsonStr = "{id:" + id + "," + "text:" + sel + "}" + phid + ":" + expanded + ":" + type;
                        //var sm = this.getSelectionModel();
                        var node = this.getStore().getNodeById(id);
                        var str = node.data['PhId'];
                        var childNode = node.findChildBy(function (node) {
                            str = str + ',' + node.data['PhId']
                        }, null, true);
                        me.typefilter = str;
                        //alert(str);
                        me.changeCallBack();

                    }
                }
            });

            me.items = [typeTree, orgTree];

        }
        else {
            me.items = [orgTree];
        }

        //me.addEvents(
        //    'selectionchange'
        //);

        me.callParent(arguments);

    },
    initParam: function () {
        var me = this;
    },
    //findNodeByKey: function (tree, value) {
    //    //console.info(value);
    //    if (value == "") { return; }
    //    var index = -1, firstFind = tree.nodeIndex == -1;

    //    var findNode = tree.getRootNode().findChildBy(function (node) {
    //        index++;
    //        //if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
    //        if (!node.data.root && index > tree.nodeIndex && (node.data['text'].indexOf(value) > -1)) {
    //            return true;
    //        }
    //    }, null, true);
    //    //
    //    tree.nodeIndex = index;
    //    if (findNode) {
    //        //
    //        tree.selectPath(findNode.getPath());
    //        var record = tree.getStore().getNodeById(findNode.data.id);
    //        tree.getSelectionModel().select(record);
    //    }
    //    else {
    //        if (firstFind) {
    //            Ext.MessageBox.alert('', '没有匹配的树节点.');
    //        }
    //        tree.nodeIndex = -1;
    //    }
    //},
    changeCallBack: function () {

    },
    beforechange: function () {

    }
});