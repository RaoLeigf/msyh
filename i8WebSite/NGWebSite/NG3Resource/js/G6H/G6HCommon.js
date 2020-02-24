

var GiACommon = {};

(function (me) {
    //删除grid一行
    //
    //var grid = Ext.getCmp('DimensioGrid');
    //{ 'id': id }
    me.$moneyRend = function (value) {
        var moneyRend = Ext.util.Format.usMoney;
        var newvalue = moneyRend(value);
        return newvalue.replace('$', '');
    }

    me.$DeleteRow = function (grid, url, cptable) {
        if (grid == null || url == null || url == "") return;
        //得到焦点行
        var sModel = grid.getSelectionModel();
        var rs = sModel.getSelection();

        if (rs.length > 0) {
            Ext.MessageBox.confirm('提示', '你确定要删除这一行?',
            function (btn) {
                if (btn == 'yes') {
                    var PhId = rs[0].get('PhId'); //获得id

                    Ext.MessageBox.wait("正在处理，请稍后...", "等待");
                    Ext.Ajax.request({
                        params: { 'busid': PhId, 'cptable': cptable },
                        url: url,
                        success: function (response, opts) {
                            Ext.MessageBox.close();
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp.status == 'Success' || resp.Status == 'success') {
                                sModel.selectNext();
                                grid.store.remove(rs[0]); //前端删除
                                Ext.MessageBox.alert("提示", "删除成功", me.$TreeRefresh);
                            }
                            else {
                                Ext.MessageBox.alert("提示", "删除失败:" + resp.Msg);
                            }
                        },
                        failure: function (responese, opts) {
                            var resp = Ext.JSON.decode(responese.responseText);
                            Ext.MessageBox.alert("提示", resp.error);
                        }
                    });
                }
            });
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }
    //编辑状态下的增行
    me.$AddRow = function (grid) {
        if (grid == null) return;
        var store = grid.getStore();
        var model = grid.getSelectionModel();
        store.insert(store.getCount(), model);
    }
    //编辑状态下的删行
    me.$DelRow = function (grid) {
        if (grid == null) return;
        var sModel = grid.getSelectionModel();
        var rs = sModel.getSelection();

        if (rs.length > 0) {
            sModel.selectNext();
            grid.store.remove(rs[0]); //前端删除
        }
    }
    //编辑状态下的插行
    me.$IstRow = function (grid) {
        if (grid == null) return;
        var store = grid.getStore();
        var model = grid.getSelectionModel();
        store.insert(model.getCount(), model);
    }
    me.$CommTreePhid = function (getParams) {
        if (getParams == undefined || !getParams)
            return Phid;
        var Params = new Array();
        Params = Phid.split("&");
        var CYear;
        var Sysphid;
        var Playphid;
        var Orgphid;
        var itemphid;
        Ext.Array.each(Params, function (value) {
            if (value != "" && value != undefined) {
                var temp = new Array();
                temp = value.split("=");
                switch (temp[0]) {
                    case 'CYear': CYear = temp[1]; break;
                    case 'Sysphid': Sysphid = temp[1]; break;
                    case 'Playphid': Playphid = temp[1]; break;
                    case 'Orgphid': Orgphid = temp[1]; break;
                    case 'itemphid': itemphid = temp[1]; break;
                }
            }
        });
        var param = {
            'CYear': CYear,
            'Sysphid': Sysphid,
            'Playphid': Playphid,
            'Orgphid': Orgphid,
            'itemphid': itemphid
        };
        return param;
    }
    me.$CommTreeParam = function () {
        var Params = new Array();
        Params = Phid.split("&");
        var CYear;
        var Sysphid;
        var Playphid;
        var Orgphid;
        Ext.Array.each(Params, function (value) {
            if (value != "" && value != undefined) {
                var temp = new Array();
                temp = value.split("=");
                switch (temp[0]) {
                    case 'CYear': CYear = temp[1]; break;
                    case 'Sysphid': Sysphid = temp[1]; break;
                    case 'Playphid': Playphid = temp[1]; break;
                    case 'Orgphid': Orgphid = temp[1]; break;
                }
            }
        });
        var param = {
            'CYear': CYear,
            'Sysphid': Sysphid,
            'Playphid': Playphid,
            'Orgphid': Orgphid
        };
        return param;
    }
    me.$CommTreeText = function () {
        return encodeURI(treeText);
    }
    me.$CommTreeisleft = function () {
        return isleft;
    }
    me.$getDgmbcellback = function (ctempformat, otype) {

        //底稿格式的选择
        if (ctempformat == 0) {
            //excel
            Ext.getCmp('word').setValue(false);
            Ext.getCmp('excel').setValue(true);
        }
        else {
            //word
            Ext.getCmp('word').setValue(true);
            Ext.getCmp('excel').setValue(false);
        }

        if (otype == $Otype.VIEW) {
            //rich.readOnly = true;
            Ext.getCmp('word').readOnly = true;
            Ext.getCmp('excel').readOnly = true
        }
    };
    //底稿模板选择
    //dgType 底稿类型
    me.$Dgmbpanel = function (ctemplib, ctempformat) {

        var panel = Ext.widget("panel", {
            title: '模板选择',
            region: 'north',
            //autoScroll: true,
            overflowY: 'scroll',
            bodyStyle: "padding-right:20px",
            //layout: 'border',
            height: 80,
            layout: {
                type: 'hbox',
                align: 'center'
            },
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    autoScroll: true,
                    layout: 'column',
                    overflowY: 'scroll',
                    border: false,
                    flex: 1,
                    style: 'margin-top:15px;',
                    items: [{
                        xtype: 'checkboxgroup',
                        columnWidth: 1,
                        columns: 2,
                        items: [{
                                boxLabel: 'Excel表格',
                                name: 'topping',
                                id: 'Excel',
                                flex: 1,
                                listeners: {
                                    change: function (me, newValue, oldValue) {
                                        Ext.getCmp('Word').setValue(oldValue);
                                        Ext.getCmp('CAudittypeName').helpid = 'gia3_audit_temp_excel';
                                        ctempformat.setValue(0);
                                    }
                                }

                            },{
                               boxLabel: 'Word文档',
                               name: 'topping',
                               id: 'Word',
                               checked: true,
                               flex: 1,
                               listeners: {
                                   change: function (me, newValue, oldValue) {
                                       Ext.getCmp('Excel').setValue(oldValue);
                                       Ext.getCmp('CAudittypeName').helpid = 'gia3_audit_temp_doc';
                                       ctempformat.setValue(1);
                                   }
                               }
                           }
                        ]
                    }
                    ]
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '模板文件',
                    name: 'CAudittypeName',
                    id: 'CAudittypeName',
                    flex: 2,
                    style: 'margin-top:15px;',
                    //gia3_audit_temp_doc
                    //gia3_audit_temp_excel
                    helpid: 'gia3_audit_temp_doc',
                    editable: false,
                    mustInput: true,
                    ORMMode: true,
                    valueField: 'Gia3BaseAudittemplib.PhId',
                    displayField: 'Gia3BaseAudittemplib.CName',
                    queryMode: 'remote',
                    listeners: {
                        helpselected: function (obj) {
                            ctemplib.setValue(obj.code);
                        }
                    }
                }
            ]
        });
        //me.$getDgmbcellback = function (ctempformat, ctemplib, otype) {
        //    //底稿格式的选择
        //    if (ctempformat == 0) {
        //        Ext.getCmp('Word').setValue(true);
        //        Ext.getCmp('Excel').setValue(false);
        //    }
        //    else {
        //        Ext.getCmp('Word').setValue(false);
        //        Ext.getCmp('Excel').setValue(true);
        //    }

        //    //底稿模板的选择
        //    var rich = Ext.getCmp('CAudittypeName');
        //    rich.setValue(ctemplib);

        //    var codectl = [rich];
        //    BatchBindCombox(codectl);
        //    if (otype == $Otype.VIEW) {
        //        rich.readOnly = true;
        //        Ext.getCmp('Word').readOnly = true;
        //        Ext.getCmp('Excel').readOnly = true
        //    }
        //};
        return panel
    }

    var Phid = -1;
    var treeText = "";
    //用于树面板  区别月点击叶子 也显示grid
    var isleft = false;
    var UseNgTree;
    me.$TreeRefresh = function () {
        if (UseNgTree != undefined)
            UseNgTree.getStore().reload();
    }
    //审计工作计划 树
    //nodeif 树节点双击时的条件过滤  
    me.$CommTreePanel = function (title, url, grid, ngToolbar, nodeif, myShow) {
        var widthT = 235;
        var ngTree = Ext.create('Ext.ng.TreePanel', {
            id: 'panellist',
            region: 'center',
            width: widthT,
            title: title,
            //items: [ngToolbar],
            treeFields: [
                { name: 'text', type: 'string' }
            ],
            url: url,
            columns: [
                {
                    xtype: 'treecolumn',
                    flex: 1,
                    dataIndex: 'text'
                }
            ],
            listeners: {
                'cellclick': function () {
                    panelr.hide();
                    var Selectnode = ngTree.getSelectionModel().getSelection();
                    if (Selectnode[0] == null) return;
                    if (nodeif(Selectnode[0].data.leaf)) {
                        isleft = Selectnode[0].data.leaf;
                        if (!isleft) {
                            Phid = Selectnode[0].data.id;
                            treeText = Selectnode[0].data.text;
                        }
                        else {
                            Phid = Selectnode[0].parentNode.data.id;
                            treeText = Selectnode[0].parentNode.data.text;
                        }
                        panelr.show();
                    }
                }
            }
        });
        UseNgTree = ngTree;
        var panelLift = Ext.create('Ext.ng.FormPanel', {
            width: widthT,
            region: 'west',
            layout: 'border',
            items: [ngTree]
        });
        var panelr = Ext.create("Ext.ng.FormPanel", {
            region: 'center',
            id: 'GirdPanel',
            layout: 'border',
            hidden: true,
            items: [ngToolbar, grid],
            listeners: {
                show: function () {
                    myShow(ngTree.getSelectionModel().getSelection(), grid);
                }
            }
        });
        var viewport = Ext.create('Ext.container.Viewport', {
            id: "viewPort",
            layout: 'border',
            items: [panelLift, panelr]
        });
        return viewport;
    }

    //审计方案树 树面板（按几次加载加载树节点）
    //nodeif 树节点双击时的条件过滤  
    //url 构造树的地址  params 得到根节点条件
    me.$CommTreePanelPart = function (title, url, params, grid, ngToolbar, nodeif, myShow) {
        var widthT = 235;
        var ngTree = Ext.create('Ext.ng.TreePanel', {
            id: 'panellist',
            region: 'center',
            width: widthT,
            title: title,
            //items: [ngToolbar],
            treeFields: [
                { name: 'text', type: 'string' },
                { name: 'nodeParams', type: 'string' }
            ],
            url: url + '?' + params,
            columns: [
                {
                    xtype: 'treecolumn',
                    flex: 1,
                    dataIndex: 'text'
                }
            ],
            listeners: {
                'cellclick': function () {
                    panelr.hide();
                    var a = ngTree.getSelectionModel().getSelection();
                    if (a[0] == null) return;
                    var value = nodeif(a[0].data.leaf, a[0]);
                    if (value > 0) {
                        Phid = a[0].data.id;
                        treeText = a[0].data.text;
                        if (value == 2) isleft = false;
                        if (value == 1) isleft = true;
                        panelr.show();
                    }
                }


            }
        });
        UseNgTree = ngTree;
        //ngTree.getStore().on('beforeload', function (store, operation, eOpts) {
        //    var root = operation.node;
        //    if (root == null || root.data.leaf) return;
        //    var Url = url + root.data.nodeParams;
        //    store.proxy.url = Url;
        //});
        var panelLift = Ext.create('Ext.ng.FormPanel', {
            width: widthT,
            region: 'west',
            layout: 'border',
            items: [ngTree]
        });
        var panelr = Ext.create("Ext.ng.FormPanel", {
            region: 'center',
            id: 'GirdPanel',
            layout: 'border',
            hidden: true,
            items: [ngToolbar, grid],
            listeners: {
                show: function () {
                    myShow(ngTree.getSelectionModel().getSelection(), grid);
                }
            }
        });
        var viewport = Ext.create('Ext.container.Viewport', {
            id: "viewPort",
            layout: 'border',
            items: [panelLift, panelr]
        });
        return viewport;
    }

    //通用 审核/取消审核 
    me.$VerifyOP = function (grid, url, CAppMan, CAppState, seccessfu, failfu) {
        if (grid == null) return;
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var PhId = data[0].get('PhId');
            if (data[0].raw.CAppState == CAppState) {
                var message = "";
                if (CAppState == 1) {
                    message = '此条记录已经审核，无需再次审核！';
                }
                else {
                    message = '此条记录尚未审核，无法取消审核！';
                }
                Ext.MessageBox.alert('提示', message);
                return;
            }
            var CAppDtDate = new Date();
            var CAppDt = CAppDtDate.getFullYear() + '/' + (CAppDtDate.getMonth() + 1) + "/" + CAppDtDate.getDate() + " " + CAppDtDate.getHours() + ":" + CAppDtDate.getMinutes() + ":" + CAppDtDate.getSeconds();
            Ext.MessageBox.wait("正在处理，请稍后...", "等待");
            Ext.Ajax.request({
                params: { 'PhId': PhId, 'CAppMan': CAppMan, 'CAppState': CAppState, 'CAppDt': CAppDt },
                url: url,
                async: false, //同步请求
                success: function (response) {
                    Ext.MessageBox.close();
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        seccessfu();
                    } else {
                        failfu(resp.Msg);
                    }
                }
            });
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }
    //通用 启用/去启用 
    me.$StateOP = function (grid, url, CState, seccessfu, failfu) {
        if (grid == null) return;
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var PhId = data[0].get('PhId');

            Ext.MessageBox.wait("正在处理，请稍后...", "等待");
            Ext.Ajax.request({
                params: {
                    'PhId': PhId, 'CState': CState
                },
                url: url,
                success: function (response) {
                    Ext.MessageBox.close();
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status == "success") {
                        seccessfu();
                    } else {
                        failfu(resp.Msg);
                    }
                }
            });
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }
    //通用 按钮启用停用/审核未审核/下达取消下达 显示判断
    me.$changButtonState = function (work, unwork, state) {
        if (state == "启用" || state == "审核" || state == "审批" || state == "下达" || state == "1" || state == "2") {
            work.hide();
            unwork.show();
        }
        else {
            work.show();
            unwork.hide();
        }
    }
    //通用       调取金格控件的方法
    //otype      编辑状态add||edit||view
    //tablename  当前表单的表名
    //cPid       当前表单的主键 (如果是新增的状态，必须先赋值一个临时主键（Guid.NewGuid().ToString()）)
    //ctype       调取的金格控件的类型（.doc||.xls）
    me.$CommBaseFile = function (otype, tablename, cPid, ctype) {
        var height = document.body.clientHeight;
        var title = "正文";

        if (otype == "view") {
            title += "查看";
        } else if (otype == "edit") {
            title += "编辑";
        } else if (otype == "add") {
            title += "新增";
        };

        $OpenTab(title, C_ROOT + 'G6/GiA/Gia3BaseFile/Gia3BaseFile?otype=' + otype + '&c_ptable=' + tablename + '&c_pid=' + cPid + '&c_type=' + ctype + '&height=' + height);
        //$OpenTab(title, C_ROOT + 'G6/GiA/Gia3BaseFile/Gia3BaseFile2003?otype=' + otype + '&c_ptable=' + tablename + '&c_pid=' + cPid + '&c_type=' + ctype + '&height=' + height);
        
        //openfile(C_ROOT + 'G6/GiA/Gia3BaseFile/Gia3BaseFile?otype=' + otype + '&c_ptable=' + tablename + '&c_pid=' + cPid + '&c_type=' + ctype ,title);
        
    }

      function openfile(url,title){
  	    var mhtmlHeight = window.screen.availHeight;//获得窗口的垂直位置;
	    var mhtmlWidth = window.screen.availWidth; //获得窗口的水平位置; 
	    var iTop = 0; //获得窗口的垂直位置;
	    var iLeft = 0; //获得窗口的水平位置;
        window.open(url,title,'height='+mhtmlHeight+',width='+mhtmlWidth+',top='+iTop+',left='+iLeft+',toolbar=no,menubar=yes,scrollbars=no,resizable=yes, location=no,status=no');  
      }

    //通用       附件调取的方法
    //id         当前表单的主键
    //otype      编辑状态add||edit||view
    //tablename  当前表单的表名
    me.$AttachMent = function (id, otype, tablename) {
        if (id != null || id != "") {
            if (otype == "edit" || otype == "view") {
                var att = Ext.create('Ext.ng.AttachMent', {
                    attachTName: 'c_pfc_attachment',
                    btnAdd: '1',
                    btnWebOk: '1',
                    status: otype,
                    busTName: tablename,
                    busID: id
                });
                att.show();

            } else {
                //var att = Ext.create('Ext.ng.AttachMent', {
                //    btnWebOk: '1',
                //    status: otype,
                //    busTName: tablename,
                //    busID: id
                //});
                //att.show();
                Ext.MessageBox.alert("提示", "保存完后再添加附件！");
            }
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }


    //通用       附件调取的方法
    //id         当前表单的主键
    //otype      编辑状态add||edit||view
    //tablename  当前表单的表名
    me.$AttachMentNew = function (id, otype, tablename, root_path) {
        if (id != null || id != "") {
            if (otype == "edit" || otype == "view") {
                var addAble = true;
                if (otype == "view") {
                    addAble = false;
                }

                var panel = Ext.create('Ext.ux.uploadPanel.UploadPanel', {
                    header: false,
                    addFileBtnText: '新增',
                    uploadBtnText: '上传',
                    removeBtnText: '移除所有',
                    cancelBtnText: '取消上传',
                    file_upload_limit: 20, //MB
                    width: 600,
                    height: 305,
                    upload_url: 'UploadAction.actionXXX',
					root_path: root_path,
                    attachTableName: 'c_pfc_attachment',
                    status: otype,
                    busTName: tablename,
                    busID: id,
                    addBtnEnable: addAble,
                    uploadBtnEnable: addAble
                });
                att = Ext.widget('window', {
                    title: '文件上传',
                    closeAction: 'destroy',
                    layout: 'fit',
                    resizable: false,
                    modal: true,
                    items: panel
                });
                att.show();
            } else {
                //var att = Ext.create('Ext.ng.AttachMent', {
                //    btnWebOk: '1',
                //    status: otype,
                //    busTName: tablename,
                //    busID: id
                //});
                //att.show();
                Ext.MessageBox.alert("提示", "保存完后再添加附件！");
            }
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }


    //otype         编辑状态add||edit||view
    //newc_pid      当前表单的主键 
    //newc_ptable   当前表单的表名
    //ctype         调取的金格控件的类型（.doc||.xls）
    //c_pid         模板表单的主键 
    //c_ptable      模板表单的表名
    me.$CommBaseFileAdd = function (otype, newc_pid, newc_ptable, ctype, c_pid, c_ptable, url) {

        //有模板就打开原来的，没有就新增再打开
        Ext.MessageBox.wait("正在处理，请稍后...", "等待");
        Ext.Ajax.request({
            params: { 'otype': otype, 'c_ptable': c_ptable, 'c_pid': c_pid, 'newc_pid': newc_pid, 'newc_ptable': newc_ptable, 'c_type': ctype },
            url: url,
            success: function (response) {
                Ext.MessageBox.close();
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status === "success" || resp.Status === "haves") {
                    me.$CommBaseFile(otype, resp.Data.newc_ptable, resp.Data.newc_pid, resp.Data.ctype);

                } else {
                    Ext.MessageBox.alert('模板打开失败', resp.Msg);
                }
            }
        });

    };

    //otype         编辑状态add||edit||view
    //newc_pid      当前表单的主键 
    //newc_ptable   当前表单的表名
    me.$CommBaseFileView = function (otype, newc_pid, newc_ptable, url) {

        //有模板就打开原来的，没有就新增再打开
        Ext.MessageBox.wait("正在处理，请稍后...", "等待");
        Ext.Ajax.request({
            params: { 'otype': otype, 'newc_pid': newc_pid, 'newc_ptable': newc_ptable },
            url: url,
            success: function (response) {
                Ext.MessageBox.close();
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status === "success") {
                    me.$CommBaseFile(otype, resp.Data.newc_ptable, resp.Data.newc_pid, resp.Data.ctype);

                } else {
                    Ext.MessageBox.alert('模板打开失败', resp.Msg);
                }
            }
        });

    };

    //通用 复制数据
    me.$CopyData = function (grid, url, seccessfu, failfu) {
        if (grid == null) return;
        var data = grid.getSelectionModel().getSelection();

        if (data.length > 0) {
            var PhId = data[0].get('PhId');
            Ext.MessageBox.wait("正在处理，请稍后...", "等待");
            Ext.Ajax.request({
                params: {
                    'PhId': PhId
                },
                url: url,
                success: function (response) {
                    Ext.MessageBox.close();
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status == "success") {
                        seccessfu();
                    } else {
                        failfu(resp.Msg);
                    }
                }
            });
        }
        else {
            Ext.MessageBox.alert("提示", "请先选择数据行！");
        }
    }

    me.$WindowMessage = function (value, callback) {

        var text = Ext.isEmpty(value) ? "" : value;
        //创建window
        var win = Ext.create("Ext.window.Window", {
            id: "myWin",
            title: "输入",
            width: 300,
            height: 180,
            layout: "fit",
            items: [
                {
                    xtype: "form",
                    defaultType: 'textfield',
                    defaults: {
                        anchor: '100%'
                    },
                    fieldDefaults: {
                        labelWidth: 80,
                        labelAlign: "left",
                        flex: 1,
                        margin: 5
                    },
                    items: [
                        {
                            xtype: "container",
                            layout: "vbox",
                            items: [
                                { xtype: "label", height: 20,margin:'10', text: "请输入您定制的方法名:" },
                                { xtype: "ngText", id: "contentarea", hideLabel: true, height: 40,margin:'10', anchor: "95%", width: "90%", value: text }
                            ]
                        }
                    ]
                }
            ],
            buttons: [
                {
                    xtype: "button", text: "确定", handler: function () {
                        var txt = Ext.getCmp('contentarea').getValue();
                        if (txt == null || txt == '') {
                            Ext.Msg.alert('提示', '请输入名称！');
                        } else {
                            callback(txt);
                            this.up("window").close();
                        }
                    }
                },
                { xtype: "button", text: "取消", handler: function () { this.up("window").close(); } }
            ]
        });

        win.show();
    }

})(GiACommon);
