Ext.define('Ext.Gc.TreePanel', {
    extend: 'Ext.tree.Panel',

    hiddenColumns: [], //不显示列


    initComponent: function() {
        var me = this;

        ////移除隐藏列
        if (me.hiddenColumns.length > 0) {
            var columns = [];
            Ext.Array.forEach(me.columns, function(col) {
                if (Ext.Array.contains(me.hiddenColumns, col.dataIndex) === false) {
                    columns.push(col);
                }
            });
            me.columns = columns;
        }

        //设置小数位
        SetColumnFormat(me.columns);

        //列标题对齐方式处理
        for (var i = 0; i < me.columns.length; i++) {
            var column = me.columns[i];
            if (!column.hidden) {
                if (column.titleAlign) {
                    column.style = "text-align:" + column.titleAlign;
                } else {
                    column.style = "text-align:center"; //默认列头居中
                }

                //必输列控制
                if (column.mustInput) {
                    column.style += ";color:OrangeRed";
                }
            }
        }

        this.callParent(arguments);
    },
    checkAll: function() {
        var me = this;
        var root = me.getRootNode();
        me.checkedWithChild(root);
    },
    uncheckAll: function() {
        var me = this;
        var root = me.getRootNode();
        me.uncheckedWithChild(root);
    },
    checkedWithParentNode: function(n) {
        var me = this;
        n.set('checked', true);
        if (n.parentNode) {
            me.checkedWithParentNode(n.parentNode);
        }
    },
    uncheckedWithParentNode: function(n) {
        var me = this;
        n.set('checked', false);
        if (n.parentNode) {
            var c = false;
            var childNodes = n.parentNode.childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                if (childNodes[i].get('checked') == true) {
                    c = true;
                }
            }
            if (c == false) {
                n.parentNode.set('checked', false);
                me.uncheckedWithParentNode(n.parentNode);
            }
        }
    },
    checkedWithChild: function(n) {
        var me = this;
        n.set('checked', true);
        if (n.childNodes.length > 0) {
            var childNodes = n.childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                me.checkedWithChild(childNodes[i]);
            }
        }
    },
    uncheckedWithChild: function(n) {
        var me = this;
        n.set('checked', false);
        if (n.childNodes.length > 0) {
            var childNodes = n.childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                me.uncheckedWithChild(childNodes[i]);
            }
        }
    },
    getSelectedRecord: function() {
        var me = this;
        var selectionModel = me.getSelectionModel();
        if (!selectionModel)
            return;
        var records = selectionModel.getSelection();
        if (!records || records.length == 0)
            return;
        var record = records[0];
        return record;
    },
    locationInGrid: function(searchFunc) {
        var tree = this;

        var eq = searchFunc;

        var moveNext = function(node) {
            var rn;
            if (node.childNodes.length > 0) {
                rn = node.childNodes[0];
            } else {
                rn = moveUpNext(node);
            }
            if (rn) {
                if (rn.isExpanded() == false) {
                    rn.expand();
                }
            }
            return rn;
        };
        /////递归向上搜索
        var moveUpNext = function(node) {
            if (node.nextSibling) {
                return node.nextSibling;
            }

            if (node.parentNode) {
                return moveUpNext(node.parentNode);
            }
            return false;
        };

        var n;
        var selecteds = tree.getSelectionModel().getSelection();
        if (selecteds.length > 0) {
            n = selecteds[0];
        } else {
            n = tree.getRootNode();
        }

        var record = moveNext(n);
        if (record == false) {
            ////如果没有下一个节点从根节点开始搜索
            record = tree.getRootNode();
        }

        if (record) {
            while (eq(record) == false) {
                record = moveNext(record);
                if (record == false) {
                    break;
                }
            }

            if (record) {
                tree.getSelectionModel().select(record);
            } else {
                record = tree.getRootNode();
                while (eq(record) == false) {
                    record = moveNext(record);
                    if (record == false) {
                        break;
                    }
                }
                if (record) {
                    tree.getSelectionModel().select(record);
                }
            }
        }
    },

    getChange: function(serial) {
        var me = this;

        var newRecords = me.store.getNewRecords(); //获得新增行  
        var modifyRecords = me.store.getUpdatedRecords(); // 获取修改的行的数据，无法获取幻影数据 
        var removeRecords = me.store.getRemovedRecords(); //获取移除的行

        var keyIndex = {}; //记录修改、删除行

        var keys = me.buskey.split(','); //多主键  

        Ext.Array.each(newRecords, function(record) {
            var allValue = '';
            for (var i = 0; i < keys.length; i++) {
                allValue += record.data[keys[i]];
            }
            if (!Ext.isEmpty(allValue)) {
                keyIndex[allValue] = allValue; //新增行的key也可能有值的
            }
        });
        Ext.Array.each(modifyRecords, function(record) {
            var allValue = '';
            for (var i = 0; i < keys.length; i++) {
                allValue += record.data[keys[i]];
            }
            keyIndex[allValue] = allValue;
        });
        Ext.Array.each(removeRecords, function(record) {
            //keyIndex[record.data[key]] = record.data[key];
            var allValue = '';
            for (var i = 0; i < keys.length; i++) {
                allValue += record.data[keys[i]];
            }
            keyIndex[allValue] = allValue;
        });


        var data = GetDatatableData(newRecords, modifyRecords, removeRecords, [], me.buskey);


        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }

        if (serialflag) {
            json = JSON.stringify(data);
            return json;
        } else {
            return data;
        }
    },
    hasModifyed: function() {

        if (this.plugins != null && this.plugins.length > 0 && this.plugins[0]) {
            this.plugins[0].completeEdit();
        }
        var me = this;
        var newRecords = me.store.getNewRecords(); //获得新增行  
        var modifyRecords = me.store.getUpdatedRecords(); // 获取修改的行的数据，无法获取幻影数据 
        var removeRecords = me.store.getRemovedRecords(); //获取移除的行

        if (newRecords.length > 0 || modifyRecords.length > 0 || removeRecords.length > 0) {
            return true;
        }
        return false;
    },
    isValid: function() {
        var me = this;

        var root = me.store.getRootNode();

        var checkFun = function(record) {

            var curRow = record.data;

            for (property in curRow) {
                var name = property; //列名
                var value = curRow[property]; //值

                var colIndex = -1;
                var findColumn = false;
                //查找是第几列
                for (var k = 0; k < me.columns.length; k++) {
                    var column = me.columns[k];
                    if (name === column.dataIndex) {
                        colIndex = k;
                        findColumn = true;
                        break;
                    }
                }

                if (!findColumn) continue;
                if (!me.columns[colIndex].getEditor) continue; //容错

                var editor = me.columns[colIndex].getEditor();

                if (editor) {
                    if (!editor.validateValue(value) || (!editor.allowBlank && Ext.isEmpty(value))) {

                        var errorMsg = !editor.validateValue(value) ? editor.activeErrors : CommLang.ThisIsMustInPut || "该项为必输项";
                        errorMsg = '[' + column.text + ']' + CommLang.ColumnNotValid || '列输入不合法' + ':' + errorMsg;

                        var i = me.getView().indexOf(record);

                        var msg = Ext.Msg.show(
                            {
                                title: CommLang.prompt || '提示',
                                msg: errorMsg,
                                closable: false,
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR,
                                animateTarget: i
                            }
                        );

                        setTimeout(function() {
                            msg.close();
                            me.plugins[0].startEdit(i, colIndex);
                        }, 1000);

                        return false;
                    }
                }
            }
        };

        var node = root.findChildBy(checkFun, null, true);

        if (node) {
            return false;
        } else {
            return true;
        }
    },
    filterBy: function (filterFunc) {
        var me = this;

        me.clearFilter();

        me.getSelectionModel().deselectAll();
        var view = this.getView(),
            nodesAndParents = [];

        var rootNode = this.getRootNode();
        nodesAndParents.push(rootNode.id);////根节点始终不隐藏

        this.getRootNode().cascadeBy(function(tree, view) {
            var currNode = this;
            if (currNode) {
                if (filterFunc(currNode)) {
                    me.expandPath(currNode.getPath());
                    while (currNode.parentNode) {
                        nodesAndParents.push(currNode.id);
                        currNode = currNode.parentNode;
                    }
                }
            }
        }, null, [this, view]);

        this.getRootNode().cascadeBy(function(tree, view) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode && !Ext.Array.contains(nodesAndParents, this.id))
                Ext.get(uiNode).setDisplayed('none');
        }, null, [this, view]);
    },
    clearFilter: function() {
        var view = this.getView();
        this.getRootNode().cascadeBy(function(tree, view) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode)
                Ext.get(uiNode).setDisplayed('table-row');
        }, null, [this, view]);
    }
});