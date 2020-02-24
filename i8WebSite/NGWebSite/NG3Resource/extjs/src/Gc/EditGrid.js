Ext.define('Ext.Gc.EditGrid', {
    extend: 'Ext.ng.GridPanel',
    region: 'center',
    border: false,
    frame: true,
    commonview: '',
    selModel: {
        mode: "MULTI" 
    },
    buskey: 'PhId', //对应的业务表主键属性
    columnLines: true,
    hiddenColumns: {}, //不显示列
    clickedColumn: "",//单击双击列
    viewtype: "list",//list:列表界面的grid  edit：编辑界面的grid
    fieldRightUIInfo: '',
    fieldRightIsMatchTable: false,////信息域权限根据表名匹配
    rightClkCol: {},
    mustInputCol:[],
    initComponent: function () {
        var me = this;

        ////移除隐藏列
        if (me.hiddenColumns.length > 0) {
            var columns = [];
            Ext.Array.forEach(me.columns, function (col) {
                if (Ext.Array.contains(me.hiddenColumns, col.dataIndex) === false) {
                    columns.push(col);
                }
            });
            me.columns = columns;
        }

        //设置小数位
        SetColumnFormat(me.columns);

        //设置sortable
        //设置列必输(此设置为通过Imp配置控制项分组字段为必输项(不输入导致IMP检索报错))
        Ext.Array.forEach(me.columns, function (col){
            col.sortable = true;
            if (me.mustInputCol.length > 0)
            {
                if (me.mustInputCol.indexOf(col.dataIndex) > -1)
                    col.mustInput = true;
                
                if (col.dataIndex.indexOf("_EXName") > -1)
                {
                    if (me.mustInputCol.indexOf(col.dataIndex.substring(0,col.dataIndex.indexOf("_EXName"))) > -1)
                        col.mustInput = true;
                }
            }
        });

        this.callParent(arguments);

        me.on('cellclick', function (_this, td, cellIndex, record, tr, rowIndex, e, eOpts) {
            if (me.columns[cellIndex]) {
                ////合并表头的grid会有问题
                me.clickedColumn = me.columns[cellIndex].dataIndex;
            }
        });

        me.on('beforeitemdblclick', function (_this, record, item, index, e, eOpt) {
            var retvalue = false;

            if (me.commonview) {
                if (me.clickedColumn == "WfFlg")
                    me.commonview.CheckViewFn();
                else
                    if (me.clickedColumn == "AsrFlg")
                        if (me.viewtype == "list") {
                            me.commonview.AttachmentFn();
                        } else {
                            me.commonview.AttachmentGridFn(me.tablename);
                        }

                    else
                        retvalue = true;
            }
            else
                return true;

            return retvalue;
        });

        me.userSetReadOnly(me.forceDisable);

        me.on('afterrender', function() {
            if (Ext.isEmpty(me.fieldRightUIInfo)) {
                return;
            }

            var info;
            if (Ext.isString(me.fieldRightUIInfo)) {
                info = Ext.decode(me.fieldRightUIInfo);
            }

            var index = Ext.Array.findBy(info, function(item) {
                return item.ContainerId === me.id;
            });

            if (!index) {
                /////没找到id相同的容器
                Ext.Array.forEach(info, function(item) {
                    if (item.ContainerType === "0" && item.TableName === me.bindtable) {
                        ////如果容器是form并且绑定表与当前容器相同
                        item.ContainerType = "1";
                        item.ContainerId = me.id;
                    }

                    if (me.fieldRightIsMatchTable) {
                        if (item.ContainerType === "1" && item.TableName === me.bindtable) {
                            ////如果容器是grid并且绑定表与当前容器相同
                            item.ContainerType = "1";
                            item.ContainerId = me.id;
                        }
                    }
                });
            }

            ////找到id相同的容器
            ////处理信息域权限代码转名称列
            Ext.Array.forEach(info, function (item) {
                if (item.ContainerId === me.id && item.ContainerType === "1") {
                    var fields = item.Items;
                    for (ctlID in fields) {
                        if (!me.getColumn(ctlID)) {
                            if (!me.getColumn(ctlID + '_EXName')) {
                            } else {
                                fields[ctlID + '_EXName'] = fields[ctlID];
                            }
                            delete fields[ctlID];
                        }
                    }
                }
            });

            $SetFieldRightUIState(me.id, info);
        });

        //tooltip
        me.on('itemmouseenter', function (view, record, item, index, e, eOpts) {
            if (view.tip == null) {
                view.tip = Ext.create('Ext.tip.ToolTip', {
                    target: view.el,
                    delegate: view.itemSelector,
                    trackMouse: true,
                    renderTo: Ext.getBody(),
                    listeners: {
                        beforeshow: function updateTipBody(tip) {
                            if (Ext.isEmpty(tip.html)) return false;
                        }
                    }
                });
            }
            var cols = view.getGridColumns();
            var col = cols[e.getTarget(view.cellSelector).cellIndex]
            var data = record.data[col.dataIndex];
            if (e.target.scrollWidth <= e.target.clientWidth)
                data = "";
            view.el.clean();
            view.tip.update(data);
            view.tip.hide();
        })

        var fieldTemp_EXName = '';
        var fieldTemp = '';
        var oriVal='';
        var rowindex = 0;

        //右键菜单
        var contextMenuList = new Ext.menu.Menu({
            items: [
                {
                    text: CommLang.ClearCurr||'清空当前',
                    handler: function (menu) {
                        var fieldTemp_EXNameDefVal = me.getCurrTyprDefaultVal(me.store.data.items[rowindex].get(fieldTemp_EXName));
                        oriVal = me.store.data.items[rowindex].get(fieldTemp_EXName);

                        if (!Ext.isEmpty(fieldTemp)) {
                            var fieldTempDefVal = me.getCurrTyprDefaultVal(me.store.data.items[rowindex].get(fieldTemp));
                            oriVal = me.store.data.items[rowindex].get(fieldTemp);
                            if (me.beforeBatchSetData(rowindex, fieldTemp, fieldTempDefVal, "clearCurr")) {
                                me.store.data.items[rowindex].set(fieldTemp, fieldTempDefVal);
                                me.store.data.items[rowindex].set(fieldTemp_EXName, fieldTemp_EXNameDefVal);
                                me.afterBatchSetData(rowindex, fieldTemp, oriVal,fieldTempDefVal, "clearCurr");
                            }
                        } else {
                                if (me.beforeBatchSetData(rowindex, fieldTemp_EXName, fieldTemp_EXNameDefVal, "clearCurr")) {
                                    me.store.data.items[rowindex].set(fieldTemp_EXName, fieldTemp_EXNameDefVal);
                                    me.afterBatchSetData(rowindex, fieldTemp_EXName,oriVal, fieldTemp_EXNameDefVal, "clearCurr");
                                }
                        }
                    }
                }, {
                    text: CommLang.ClearAll||'清空所有',
                    handler: function (menu) {
                        var fieldTemp_EXNameDefVal = me.getCurrTyprDefaultVal(me.store.data.items[rowindex].get(fieldTemp_EXName));

                        if (!Ext.isEmpty(fieldTemp)) {
                            var fieldTempDefVal = me.getCurrTyprDefaultVal(me.store.data.items[rowindex].get(fieldTemp));
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                if (me.beforeBatchSetData(row, fieldTemp, fieldTempDefVal, "clearAll")) {
                                    var oriVal = item.get(fieldTemp);
                                    item.set(fieldTemp, fieldTempDefVal);
                                    item.set(fieldTemp_EXName, fieldTemp_EXNameDefVal);
                                    me.afterBatchSetData(row, fieldTemp, oriVal,fieldTempDefVal, "clearAll");
                                } row++;
                            })
                        } else {
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                if (me.beforeBatchSetData(row, fieldTemp_EXName, fieldTemp_EXNameDefVal, "clearAll")) {
                                    var oriVal = item.get(fieldTemp_EXName);
                                    item.set(fieldTemp_EXName, fieldTemp_EXNameDefVal);
                                    me.afterBatchSetData(row, fieldTemp_EXName, oriVal,fieldTemp_EXNameDefVal, "clearAll");
                                } row++;
                            })
                        }
                    }
                }, {
                    text: CommLang.ResetByThisNull||'以此重置为空的',
                    handler: function (menu) {
                        var currfieldTemp_EXNameVal = me.store.data.items[rowindex].get(fieldTemp_EXName);

                        if (!Ext.isEmpty(fieldTemp)) {  ////当该字段为非通用帮助字段时，fieldTemp==''
                            var currFieldTempVal = me.store.data.items[rowindex].get(fieldTemp);////当fieldTemp==''时，currFieldTempVal==undefined
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                var otherFieldVal = item.get(fieldTemp);
                                //var defTypeVal = me.getCurrTyprDefaultVal(otherFieldVal);////根据当前返回值判定是否为空
                                if (Ext.isEmpty(otherFieldVal) || otherFieldVal == 0 || otherFieldVal == "") {
                                    if (me.beforeBatchSetData(row, fieldTemp, currFieldTempVal, "resetNull")) {
                                        var oriVal = item.get(fieldTemp);
                                        item.set(fieldTemp, currFieldTempVal);
                                        item.set(fieldTemp_EXName, currfieldTemp_EXNameVal);
                                        me.afterBatchSetData(row, fieldTemp, oriVal,currFieldTempVal, "resetNull");
                                    }
                                } row++;
                            });
                        }
                        else
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                var otherField_EXNameVal = item.get(fieldTemp_EXName);
                                //var defTypeVal = me.getCurrTyprDefaultVal(otherField_EXNameVal);////根据当前返回值判定是否为空
                                if (Ext.isEmpty(otherField_EXNameVal) || otherField_EXNameVal == 0 || otherField_EXNameVal == "") {
                                    if (me.beforeBatchSetData(row, fieldTemp_EXName, currfieldTemp_EXNameVal, "resetNull")) {
                                        var oriVal = item.get(fieldTemp_EXName);
                                        item.set(fieldTemp_EXName, currfieldTemp_EXNameVal);
                                        me.afterBatchSetData(row, fieldTemp_EXName, oriVal,currfieldTemp_EXNameVal, "resetNull");
                                    }
                                } row++;
                            });
                    }
                }, {
                    text: CommLang.ResetAll||'以此重置所有的',
                    handler: function (menu) {
                        var currfieldTemp_EXNameVal = me.store.data.items[rowindex].get(fieldTemp_EXName);
                        if (!Ext.isEmpty(fieldTemp)) {
                            var currFieldTempVal = me.store.data.items[rowindex].get(fieldTemp);
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                if (me.beforeBatchSetData(row, fieldTemp, currFieldTempVal, "resetAll")) {
                                    var oriVal = item.get(fieldTemp);
                                    item.set(fieldTemp, currFieldTempVal);
                                    item.set(fieldTemp_EXName, currfieldTemp_EXNameVal);
                                    me.afterBatchSetData(row, fieldTemp, oriVal,currFieldTempVal, "resetAll");
                                } row++;
                            })
                        } else {
                            var row = 0;
                            Ext.Array.forEach(me.store.data.items, function (item) {
                                if (me.beforeBatchSetData(row, fieldTemp_EXName, currfieldTemp_EXNameVal, "resetAll")) {
                                    var oriVal = item.get(fieldTemp_EXName);
                                    item.set(fieldTemp_EXName, currfieldTemp_EXNameVal);
                                    me.afterBatchSetData(row, fieldTemp_EXName, oriVal,currfieldTemp_EXNameVal, "resetAll");
                                } row++;
                            })
                        }
                    }
                }
            ]
        });

        me.on('cellcontextmenu', function (grd, td, cellIndex, record, tr, rowIndex, e, eOpts) {
            if (me.otype != $Otype.VIEW) {
                var fieldName = grd.getHeaderAtIndex(cellIndex).dataIndex;//获取编辑列的属性
                fieldTemp_EXName = fieldName;
                rowindex = rowIndex;
                var text = grd.getHeaderAtIndex(cellIndex).text;//获取编辑列的属性名称

                if (Ext.isObject(me.rightClkCol)) {
                    for (var field in me.rightClkCol) {
                        if (field === fieldName) {
                            var i = 0;
                            var itemText = '';
                            Ext.Array.forEach(contextMenuList.items.items, function (item) {
                                //contextMenuList.initialConfig.items[i].text;
                                itemText = contextMenuList.initialConfig.items[i].text + text;
                                item.setText(itemText);
                                i++;
                            })
                            contextMenuList.showAt(e.getXY());

                            if (!Ext.isEmpty(me.rightClkCol[field])) {
                                fieldTemp = me.rightClkCol[field];
                            }
                        }
                    }
                }
            }
        });
    },
    //orderField: 'Orders',

    forceDisable: false,////强制失效标识，初始化时设为true后，控件永远失效

    addrow: function (newobj) {
        var me = this;
        if (newobj == null) {
            newobj = {};
        }
        var index = me.store.getCount();

        if (Ext.isArray(newobj) === false) {
            ////单个对象
            if (me.orderField) {
                if (index === 0) {
                    newobj[me.orderField] = index;
                } else {
                    newobj[me.orderField] = me.store.max(me.orderField) + 1;
                }
            }
            me.store.insert(index, newobj);
        } else {
            ////数组
            if (me.orderField) {
                var orderid = 0;
                if (index === 0) {
                    orderid = index;
                } else {
                    orderid = me.store.max(me.orderField) + 1;
                }

                Ext.Array.forEach(newobj, function(item) {
                    item[me.orderField] = orderid;
                    orderid++;
                });
            }
            me.store.insert(index, newobj);
        }
    },
    deleterow: function () {
        var me = this;
        var data = me.getSelectionModel().getSelection();

        if (data.length > 0) {
            Ext.Array.each(data, function (record) {
                me.store.remove(record); //前端删除
            });
        }
    },
    itemup: function () {
        var me = this;

        var record = me.getSelectionModel().getSelection();
        if (record.length > 0) {
            var index = me.store.indexOf(record[0]);
            if (index > 0) {
                ////交换排序字段
                var pid = me.store.getAt(index - 1).get(me.orderField);
                var orders = record[0].get(me.orderField);

                me.store.getAt(index - 1).set(me.orderField, orders);
                record[0].set(me.orderField, pid);

                me.setOrder(); ////重新排序

                me.getView().refresh();
                me.getSelectionModel().select(index - 1);
            }
        }
    },
    itemdown: function () {
        var me = this;

        var record = me.getSelectionModel().getSelection();
        if (record.length > 0) {
            var index = me.store.indexOf(record[0]);
            if (index < me.store.getCount() - 1) {
                ////交换排序字段
                var pid = me.store.getAt(index + 1).get(me.orderField);
                var orders = record[0].get(me.orderField);

                me.store.getAt(index + 1).set(me.orderField, orders);
                record[0].set(me.orderField, pid);

                me.setOrder(); ////重新排序

                me.getView().refresh();
                me.getSelectionModel().select(index + 1);
            }
        }
    },

    setOrder: function () {
        var me = this;

        me.store.sort([
            {
                property: me.orderField,
                direction: 'ASC'
            }
        ]);

        var i = 0;
        me.store.each(function (record) {
            record.data[me.orderField] = i + 1;
            i++;
        });
    },

    saveData: function (url) {
        var me = this;

        me.plugins[0].completeEdit(); //GRID失去焦点

        if (!me.isValid()) {
            return;
        }

        var mstformData = me.getChange();

        Ext.Ajax.request({
            params: {
                'mstformData': mstformData
            },
            url: url,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status === "success") {
                    Ext.MessageBox.alert('提示', CommLang.SaveSuccess||"保存成功", function () {
                        //me.store.commitChanges();
                        me.store.refreshCache();
                        me.store.reload();
                    });
                } else {
                    Ext.MessageBox.alert(CommLang.SaveFailed||'保存失败', resp.Msg);
                }
            }
        });
    },

    getSelectedRecord: function () {
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
    locationInGrid: function (searchFunc) {
        var grid = this;

        if (grid.store.getCount() === 0) {
            return;
        }
        //确定起始行
        var selmodel = grid.getSelectionModel();
        var records = selmodel.getSelection();
        var bgnindx;
        if (records.length === 0) {
            bgnindx = 0;
        } else {
            bgnindx = grid.store.indexOf(records[0]) + 1;
        }
        var findindex = grid.store.findBy(searchFunc, grid.store, bgnindx);
        if (findindex >= 0) {
            selmodel.select(findindex);
        } else {
            bgnindx = 0;
            findindex = grid.store.findBy(searchFunc, grid.store, bgnindx);
            if (findindex >= 0) {
                selmodel.select(findindex);
            }
        }
    },

    /////设置控件只读
    ////flag true表示设置控件只读，false取消控件只读
    ////array 可选参数，不传设置全部列，传一个字符串数组，则特殊设置数组中对应的列的只读属性
    userSetReadOnly: function (flag, array) {
        var me = this;

        if (!me.userSetReadOnlyFun) {
            ////初始化
            me.enablecolumns = [];
            me.disablecolumns = [];
            me.defcolumnsflag = true;

            me.userSetReadOnlyFun = function (editor, e, eOpts) {
                ////已强制失效
                if (me.forceDisable === true) {
                    return false;
                }

                if (me.defcolumnsflag === true) {
                    ////默认有效状态下
                    if (me.disablecolumns.length > 0) {
                        ////部分失效
                        if (Ext.Array.contains(me.disablecolumns, e.field)) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    ////默认失效状态下
                    if (me.enablecolumns.length > 0) {
                        ////部分有效
                        if (Ext.Array.contains(me.enablecolumns, e.field)) {
                            return true;
                        }
                    }
                    return false;
                }
            };
            me.on('beforeedit', me.userSetReadOnlyFun);
        }

        if (flag) {
            if (array) {
                ////部分失效
                me.disablecolumns = Ext.Array.merge(me.disablecolumns, array);

                Ext.Array.forEach(array, function (a) {
                    me.enablecolumns = Ext.Array.remove(me.enablecolumns, a);
                });
            } else {
                ////全部失效
                me.enablecolumns = [];
                me.defcolumnsflag = false;
            }
        } else {
            if (array) {
                ////部分生效
                me.enablecolumns = Ext.Array.merge(me.enablecolumns, array);

                Ext.Array.forEach(array, function (a) {
                    me.disablecolumns = Ext.Array.remove(me.disablecolumns, a);
                });
            } else {
                ////全部生效
                me.disablecolumns = [];
                me.defcolumnsflag = true;
            }
        }
    },
    forcedisable: function () {
        ////强制失效按钮，失效后无法再生效，主要用于查看界面是对按钮初始状态进行设置
        var me = this;
        me.userSetReadOnly(true);
        me.forceDisable = true;
    },
    print: function (title) {
        var me = this;
        $Print(me.id, me, title);
    },
    listdbclick: function (dbcallback, commonview) {
        var me = this;
        if (me.clickedColumn == "WfFlg")
            commonview.CheckViewFn();
        else
            if (me.clickedColumn == "AsrFlg")
                commonview.AttachmentFn();
            else
                dbcallback();
    },
    setCommonView: function (view) {
        var me = this;
        me.commonview = view;
    },
    getCurrTyprDefaultVal:function(currVal) {
        var typeToString = Ext.typeOf(currVal);
        switch (typeToString) {
            case 'string':
                return '';
                break;
            case 'date':
                return null;
                break;
            case 'boolean':
                return false;
                break;
            case 'number':
                return 0;
                break;
        }
    },
    beforeBatchSetData: function (row,field,value,type) { return true; },
    afterBatchSetData: function (row, field, orivalue, value, type) { },
    scrollToRow: function (rowIndex) {
        var selmodel = this.getSelectionModel();
        selmodel.select(rowIndex);

        //ie浏览器定位滚动
        if (!Ext.isChrome) {
            //window.setTimeout(function () {
            //    var selectNode = gridpanel.view.body.query("tr." + gridpanel.view.selectedItemCls);
            //    if (selectNode) {
            //        selectNode[0].scrollIntoView(true);
            //    }
            //}, 500);

            var selectNode = this.view.body.query("tr." + this.view.selectedItemCls);
            if (selectNode) {
                selectNode[0].scrollIntoView(true);
            }
        }
    }
});