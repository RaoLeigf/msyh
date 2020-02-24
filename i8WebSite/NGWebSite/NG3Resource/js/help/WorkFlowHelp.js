$WorkFlow = {
    isSetNGLang: false,
    NGLang: {},
    setNGLang: function (data) {
        if (Ext.isString(data)) {
            data = Ext.decode(data);
        }
        if (Ext.isObject(data)) {
            this.NGLang = data;
            this.isSetNGLang = true;
        }
    },
    // loadNGLangIfNotSet: function () {
    //     if (this.isSetNGLang)
    //         return;
    // },
    startFlow: function (bizType, bizPhid, callback, cancelback) {
        $WorkFlow.startFlowWithPdKey(bizType, bizPhid, null, callback, cancelback);
    },
    startFlowWithPdKey: function (bizType, bizPhid, pdkey, callback, cancelback) {
        if (Ext.isEmpty(bizType) || Ext.isEmpty(bizPhid)) {
            $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.paramError || "参数不正确！");
            return;
        }
        var url = C_ROOT + 'WorkFlow3/WorkFlow/GetStartFlowExecutionInfo?biztype=' + bizType + '&bizphid=' + bizPhid;
        if (!Ext.isEmpty(pdkey)) {
            if (Ext.isArray(pdkey)) {
                pdkey = pdkey.join(',');
            }
            url = url + '&pdkey=' + pdkey;
        }
        Ext.Msg.show({
            msg: 'loading...',
            wait: true,
            progress: true,
            dosable: true,
            waitConfig: {
                interval: 200
            },
            icon: Ext.Msg.INFO
        });
        Ext.Ajax.request({
            url: url,
            success: function (response) {
                Ext.Msg.close();
                var resp = Ext.JSON.decode(response.responseText);
                $WorkFlow.setNGLang(resp.NGLang);
                if (resp.success) {
                    var pdData = resp.data;
                    //单据有正在运行中的流程
                    if (pdData.hasRuningFlow) {
                        $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.bizHasRuningProcInst || "单据有正在运行中的流程，不允许送审！", function () {
                            if (cancelback && Ext.isFunction(cancelback)) {
                                cancelback();
                            }
                        });
                        return;
                    }
                    var win = Ext.create('Ext.ng.WorkFlowStartWin', {
                        bizType: bizType,
                        bizPhid: bizPhid,
                        data: pdData,
                        cancelback: cancelback,
                        callback: callback
                    });
                    Ext.useShims = true;
                    win.show();
                }
                else {
                    $WorkFlow.msgAlert($WF.NGLang.getPdListError || '获取流程定义出错', resp.errorMsg);
                }
            }
        });
    },
    showFlowInfo: function (bizType, bizPhid) {
        Ext.Ajax.request({
            url: C_ROOT + 'WorkFlow3/FlowManager/IsBizHasFlowHis?bizid=' + bizType + '&bizpk=' + bizPhid,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (!Ext.isObject(resp.NGLang)) {
                    resp.NGLang = {};
                }
                if (resp.wf) {
                    $OpenTab(resp.NGLang.ProcessTraceViewTitle || '流程追踪', C_ROOT + 'WorkFlow3/FlowManager/ProcessTraceView?bizid=' + bizType + '&bizpk=' + bizPhid);
                }
                else if (resp.af) {
                    var parm = "op_type=" + bizType + "@@**SourceFromType=WebFrom@@**primarystr=" + bizPhid;
                    window.external.ShowManagerWithParm("AppFlowHistoryViewManager", 1, parm);
                }
                else if (resp.oawf) {
                    $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', "查看老办公工作流未实现!");
                    return;
                }
                else if (resp.psoftwf) {
                    $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', "查看老办公工作流未实现!");
                    return;
                }
                else {
                    $WorkFlow.msgAlert(resp.NGLang.alertTitle || '提示', resp.NGLang.bizHasNoFlowInfo || "未找到该单据对应的流程信息!");
                    return;
                }
            }
        });
    },
    showPiDiagram: function (piid, curActId) {
        var url = 'WORKFLOW/ActivitiWebProxy.ashx?page=diagram-viewer&piid=' + piid;
        if (!Ext.isEmpty(curActId)) {
            url = url + "&curActI" + curActId;
        }
        $OpenTab($WF.NGLang.diagramViewerTitle || '流程查看', C_ROOT + 'WORKFLOW/ActivitiWebProxy.ashx?page=diagram-viewer&piid=' + piid);
    },
    showPdDiagram: function (pdid) {
        $OpenTab($WF.NGLang.diagramViewerTitle || '流程查看', C_ROOT + 'WORKFLOW/ActivitiWebProxy.ashx?page=diagram-viewer&pdid=' + pdid);
    },
    getHisGridColumns: function () {
        return [{
            header: $WF.NGLang.taskHisActor || '办理者',
            dataIndex: 'username',
            flex: 2,
            sortable: false,
            hidden: false
        }, {
            header: $WF.NGLang.taskHisMsg || '意见',
            dataIndex: 'msg',
            flex: 4,
            sortable: false,
            hidden: false
        }, {
            header: $WF.NGLang.taskHisTime || '办理时间',
            dataIndex: 'end_time',
            flex: 2,
            sortable: false,
            //renderer: Ext.util.Format.dateRenderer('Y-m-d H:i'),
            hidden: false
        }, {
            header: $WF.NGLang.taskHisNode || '节点',
            dataIndex: 'task_des',
            flex: 2,
            sortable: false,
            hidden: false
        }, {
            header: $WF.NGLang.taskHisTaskDes || '任务',
            dataIndex: 'actionname',
            flex: 2,
            sortable: false,
            hidden: false
        }, {
            header: $WF.NGLang.taskHisDuration || '停留时间',
            dataIndex: 'duration',
            flex: 1,
            sortable: false,
            hidden: false
        }, {
            header: $WF.NGLang.taskHisSignature || '签章',
            dataIndex: 'signature',
            flex: 1,
            sortable: false,
            renderer: function (value, metaData, record, colIndex, store, view) {
                if (!Ext.isEmpty(value)) {
                    var picPath = C_ROOT + value;
                    var ss = '<img src="' + picPath + '"  borer="0" />';
                    metaData.tdAttr = 'data-qtip="' + Ext.String.htmlEncode(ss) + '"';
                    return '<img src="' + picPath + '" width="12" height="12" borer="0" />';
                }
            },
            hidden: false
        }, {
            header: $WF.NGLang.attach || '附件',
            dataIndex: 'att_count',
            flex: 1,
            sortable: false,
            renderer: function (value, metaData, record, colIndex, store, view) {
                if (!Ext.isEmpty(value) && value > 0) {
                    var newValue = '<img src="' + C_ROOT + '\\NG3Resource\\icons\\attach.png" width="12" height="12" borer="0"  style="margin:0px 10px -2px 0px" />'
                    newValue += '<a class="delete-link" href="#attachment-record-' + record.data.taskid + '">' + value + '</a>';
                    return newValue;
                }
            },
            hidden: false
        }];
    },
    showTaskAttachment: function (record) {
        if (Ext.isEmpty(record.data.attGuid)) {
            //初始化附件控件
            var param = {
                product: "",//传相应产品，i6、i6s、i6P、A3、GE，可为空
                attachguid: "",//会话guid,可传空 生成方法 cs代码: Guid.NewGuid().ToString(),
                attachTName: "c_pfc_attachment",
                busTName: "act_hi_taskinst",//传相应业务表
                busid: record.data.taskid,//传相应产品业务单据phid
            };
            var result = LoadAttach.InitBeforeOpen(param);
            var obj = eval(result);
            if (!Ext.isEmpty(obj)) {
                if (obj.status == "success")//初始化成功
                {
                    record.data.attGuid = obj.msg;
                }
                else {
                    $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.attachCtlInitError || "附件初始化失败");
                    return;
                }
            } else {
                $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.attachCtlInitError || "附件初始化失败");
                return;
            }
        }
        var opt = {
            product: "",//传相应产品，可为空
            mode: "NG3",//固定传NG3
            openbymianframe: "1",//通过主框架打开附件 0或空 否  1 是 固定传1
            oper: "winfrom",//web、pb、winfrom、progress（进度条模式）,固定传 winfrom
            asr_tbl: "c_pfc_attachment",
            tbl: "act_hi_taskinst",//传相应业务表
            fill: $appinfo.userID,//传相应操作员id
            fillname: $appinfo.username,//传相应操作员姓名
            chkSign: "0",//默认传0
            chkCheckIn: "0",//默认传0
            btnAdd: "0",//新增按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            addserverstuts: "0",//导入按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnScan: "0",//扫描按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnDelete: "0",//删除按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnEdit: "0",//编辑按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnView: "1",//查看按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnDownload: "1",//下载按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnCancel: "1",//取消按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
            btnOk: "0",//确定按钮 oper非web时有效 0、禁用 1、显示 2、隐藏                    
            btnWebAdd: "2",//web新增按钮 oper为web时有效 0、禁用 1、显示 2、隐藏
            btnWebOk: "2",//web新增按钮 oper为web时有效 0、禁用 1、显示 2、隐藏
            archivestuts: "2",//归档按钮 oper非web时有效 0、禁用 1、显示 2、隐藏                    
            status: "view",//add 新增模式，view 查看模式， edit 编辑模式
            showlist: "1",//显示文件列表 0 不显示 1显示
            zip: "0", //附件压缩 0 不压缩 1 压缩
            filenum: "",//附件上传数量限制 0或空不限制附件上传数量
            filetype: "",//附件上传类型限制 目前仅支持传入"image",如果传了"image"附件控件就只能上传图片
            guid: record.data.attGuid //传第二步附件初始化获得的guid
        };
        //打开附件
        LoadAttach.Init(opt);
    },
    getNextTask: function (startdt) {
        var data = {};
        Ext.Ajax.request({
            async: false,
            url: C_ROOT + 'WorkFlow3/WorkFlow/GetNextPendingTaskByUser',
            params: { 'startDt': startdt },
            success: function (response) {
                data = Ext.JSON.decode(response.responseText);
            }
        });
        return data;
    },
    getOpenNextTask: function (startdt) {
        var openNext = false;
        if ($WorkFlow.isWebFram()) {
            try {
                openNext = window.parent.getIndividualSetting().IsMyWorkFlowSet;
            } catch (err) {

            }
        } else {
            if (window.external.GetWorkFlowFlag) {
                openNext = window.external.GetWorkFlowFlag() == '1';
            }
        }
        var data = {};
        if (openNext === true) {
            data = this.getNextTask(startdt);
        }
        return data;
    },
    openNextAppFlowTask: function (data, isProtal, isTaskList) {
        var isWeb = false, url;
        if (Ext.isEmpty(data.uitype) || data.uitype != 4) {
            isWeb = false;
        }
        else {
            isWeb = true;
            url = C_ROOT + data.url;
            if (isProtal == true) {
                url += '&isFromProtal=true';
            }
            if (isTaskList) {
                url += '&iswftasklist=true';
            }
        }
        if ($WorkFlow.isWebFram()) {
            if (isWeb) {
                window.location = url;
            } else {
                $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.openNotWebError || '不支持打开非web单据！', function () {
                    $CloseTab();
                });
            }
        } else {
            if (isWeb) {
                $OpenTab('', url);
            } else {
                var param = data.urlparam;
                if (isProtal == true) {
                    param += '@@**isFromProtal=true';
                }
                if (isTaskList == true) {
                    param += '@@**IsWFTaskList=true';
                }
                window.external.ShowManagerWithParm(data.url, -1, param);
                window.external.ActiveSelectedTabPageEx();
            }
            //关闭自己
            if (window.external) {
                setTimeout(function () {
                    window.external.CloseTabPageByUrl(window.location.href);
                }, 500);
            } else {
                setTimeout(function () {
                    window.close();
                }, 500);
            }
        }
    },
    createOrgTree: function () {
        var orgTree = Ext.create('Ext.ng.OrgTree', {
            region: 'center',
            border: false,
            autoScroll: true,
            rootVisible: false,
            useArrows: true,
            orgattr: '18',
            isIncludeDept: true,
            isLazyLoad: false,
            isRight: false,
            width: 180,
            minSize: 180,
            maxSize: 180
            //listeners: {
            //    selectionchange: selectionchange                 
            //}
        });
        //组织树特殊处理，增加未对应部门
        if (orgTree.store) {
            orgTree.store.on('load', function (store, records, successful, eOpts) {
                if (records.childNodes.length > 0) {
                    var newNode = Ext.create('Ext.data.NodeInterface', {
                        leaf: false
                    });
                    var parentNode = records.childNodes[0].parentNode;
                    var newNode = parentNode.createNode(newNode);
                    newNode.set('Text', $WF.NGLang.noDeptData || '未对应部门');
                    newNode.set('OCode', 'nodeptid');
                    newNode.set('id', 'nodeptid');
                    newNode.set('leaf', true);
                    parentNode.appendChild(newNode);
                }
            })
        }
        return orgTree;
    },
    showHoldAlterMsg: function (msg, hold, callback) {
        Ext.MessageBox.show({
            title: $WF.NGLang.alertTitle || '提示',
            msg: msg,
            width: 300,
            icon: Ext.MessageBox.INFO,
            modal: false
        });
        //this.Hold(hold);
        var h = 1;//10秒钟       
        if (hold != undefined) {
            h = hold;
        }
        setTimeout(function () {
            Ext.MessageBox.hide();
            if (Ext.isFunction(callback)) {
                callback();
            }
        }, h * 1000);
    },
    openWinFromPage: function (manager, param, withCallBack) {
        if ($WorkFlow.isWebFram()) {
            $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.openNotWebError || '不支持打开非web单据！');
        } else {
            if (withCallBack === true) {
                window.external.ShowManagerWithCallback('1', manager, 5, param);
            } else {
                window.external.ShowManagerWithParm(manager, -1, param);
            }
        }
    },
    msgAlert: function (title, msg, callback) {
        Ext.MessageBox.on('beforeshow', $winBeforeShow);
        var msgBox = Ext.MessageBox.alert(title, msg, function () {
            if (callback) {
                callback();
            }
            $winBeforeClose(msgBox);
        });
    },
    msgAlertNoOcx: function (title, msg, callback) {
        var dealOcx = Ext.MessageBox.dealOcx;
        if (dealOcx) {
            Ext.MessageBox.un('beforeshow', $winBeforeShow);
            Ext.MessageBox.un('beforeclose', $winBeforeClose);
        }
        var msgBox = Ext.MessageBox.alert(title, msg, function () {
            if (callback) {
                callback();
            }
            if (dealOcx) {
                Ext.MessageBox.on('beforeshow', $winBeforeShow);
                Ext.MessageBox.on('beforeclose', $winBeforeClose);
            }
        });
    },
    msgConfirm: function (title, msg, callback) {
        Ext.MessageBox.on('beforeshow', $winBeforeShow);
        var msgBox = Ext.MessageBox.confirm(title, msg, function (btn) {
            if (callback) {
                callback(btn);
            }
            $winBeforeClose(msgBox);
        })
    },
    isWebFram: function () {
        var f = $GetWFrame();
        if (f && f.Center) {
            return true;
        } else {
            return false;
        }
    },
    format: function () {
        var param = [];
        for (var i = 0, l = arguments.length; i < l; i++) {
            param.push(arguments[i]);
        }
        var statment = param[0]; // get the first element(the original statement)
        param.shift(); // remove the first element from array
        return statment.replace(/\{(\d+)\}/g, function (m, n) {
            return param[n];
        });
    }
};

var $WF = $WorkFlow;

Ext.define('Ext.wf.data.PagingMemoryProxy', {
    extend: 'Ext.data.proxy.Memory',
    // alias: 'proxy.pagingmemory',
    // alternateClassName: 'Ext.data.PagingMemoryProxy',
    read: function (operation, callback, scope) {
        var reader = this.getReader(),
            result = reader.read(this.data),
            sorters, filters, sorterFn, records;

        scope = scope || this;
        // filtering
        filters = operation.filters;
        if (filters.length > 0) {
            //at this point we have an array of  Ext.util.Filter objects to filter with,
            //so here we construct a function that combines these filters by ANDing them together
            records = [];

            Ext.each(result.records, function (record) {
                var isMatch = true,
                    length = filters.length,
                    i;

                for (i = 0; i < length; i++) {
                    var filter = filters[i],
                        fn = filter.filterFn,
                        scope = filter.scope;

                    isMatch = isMatch && fn.call(scope, record);
                }
                if (isMatch) {
                    records.push(record);
                }
            }, this);

            result.records = records;
            result.totalRecords = result.total = records.length;
        }

        // sorting
        sorters = operation.sorters;
        if (sorters.length > 0) {
            //construct an amalgamated sorter function which combines all of the Sorters passed
            sorterFn = function (r1, r2) {
                var result = sorters[0].sort(r1, r2),
                    length = sorters.length,
                    i;

                //if we have more than one sorter, OR any additional sorter functions together
                for (i = 1; i < length; i++) {
                    result = result || sorters[i].sort.call(this, r1, r2);
                }

                return result;
            };

            result.records.sort(sorterFn);
        }

        // paging (use undefined cause start can also be 0 (thus false))
        if (operation.start !== undefined && operation.limit !== undefined) {
            result.records = result.records.slice(operation.start, operation.start + operation.limit);
            result.count = result.records.length;
        }

        Ext.apply(operation, {
            resultSet: result
        });

        operation.setCompleted();
        operation.setSuccessful();

        Ext.Function.defer(function () {
            Ext.callback(callback, scope, [operation]);
        }, 10);
    }
});

Ext.define("Ext.ng.wf.SignatureWin", {
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
        me.title = $WF.NGLang.signatureWinTitle || me.title;
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
                fieldLabel: $WF.NGLang.taskHisSignature || '签章',
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
                fieldLabel: $WF.NGLang.pwd || '密码',
                inputType: 'password',
                readOnly: false,
                itemId: "txtPwd",
                //fieldStyle: 'border:1px solid #DDD;width:100%;background-color:#F5F5F5;color:#ACA899',
                tabIndex: 2,
                colspan: 2
            }, {
                xtype: 'button',
                name: 'addSelect',
                itemId: 'viewBtn',
                text: $WF.NGLang.perview || '预览',
                margin: '3 0 0 0',
                width: 40,
                colspan: 1,
                handler: Ext.bind(function () {
                    if (!me.currData) {
                        $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.choseSignature || '请选择签章！');
                        return;
                    }
                    var phid = me.currData.PhId;
                    if (Ext.isEmpty(phid)) {
                        $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.choseSignature || '请选择签章！');
                        return;
                    }
                    var pwd = baseInfoForm.down('#txtPwd').getValue();
                    if (Ext.isEmpty(pwd)) {
                        $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.noPwd || '请输入密码！');
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
                                    $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.pwdError || '密码错误');
                                }
                            }
                            else {
                                $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.pwdError || '密码错误');
                            }
                        }
                    });
                })
            }, {
                xtype: 'panel',
                //xtype:'form',
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
                    //autoShow: true,
                    //autoScroll: true,
                    //anchor: '100%',
                    itemId: 'img'
                }]
            }]
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
                    itemId: "help_query", text: $WF.NGLang.btnOk || "确定", iconCls: 'add'
                },
                {
                    itemId: "help_close", text: $WF.NGLang.btnCancel || "取消", iconCls: 'cross'
                }
            ]
        });
        me.items = [rolltoolbar, baseInfoForm];
        me.callParent();
        //if (me.currData.length > 0) {
        //    baseInfoForm.getForm().setValues(me.currData[1]);
        //}
        rolltoolbar.items.get('help_query').on('click', function () {
            if (me.currData == null) {
                $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.choseSignature || '请选择签章！');
                //$WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.choseSignature ||'请选择签章！');
                return;
            }
            if (me.currData.checkedPwd == false) {
                $WorkFlow.msgAlertNoOcx($WF.NGLang.alertTitle || '提示', $WF.NGLang.noPwd || '请输入密码！');
                //$WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示',  $WF.NGLang.noPwd ||'请输入密码！');
                return;
            }
            me.callback(me.currData.PhId, me.currData.MarkPath);
            me.close();
        });
        rolltoolbar.items.get('help_close').on('click', function () {
            me.close();
        });
        me.on("beforeshow", $winBeforeShow);
        me.on("beforeclose", $winBeforeClose);
    }
});

Ext.define("Ext.ng.wf.SignatureImage", {
    extend: 'Ext.Img',
    alias: 'widget.signatureImage',
    width: 200,
    padding: '15 0 15 0',
    border: true,
    signature: '',
    height: 150,
    style: 'border: 2px solid #A2A2A2;cursor: pointer',
    initComponent: function () {
        var me = this;
        if (Ext.isEmpty(me.src)) {
            me.src = C_ROOT + 'NG3Resource/pic/OtherHelp_Sign.jpg';
        }
        this.callParent();
        me.on('afterrender', function () {
            if (this.el) {
                this.el.on('dblclick', function () {
                    Ext.Msg.show({
                        msg: $WF.NGLang.getSignature || '获取签章数据' + '...',
                        wait: true,
                        progress: true,
                        dosable: true,
                        waitConfig: {
                            interval: 200
                        },
                        icon: Ext.Msg.INFO
                    });
                    Ext.Ajax.request({
                        // async: false,
                        url: C_ROOT + 'WM/Archive/WmIoSignature/GetSignatureListByCurrentUser',
                        success: Ext.bind(function (response) {
                            Ext.Msg.close();
                            if (response.responseText == null || response.responseText == "")
                                return;
                            if (response.statusText == "OK") {
                                var data = Ext.JSON.decode(response.responseText).Record;
                                if (data.length < 1) {
                                    $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.hasNoSignature || '未设置签章！');
                                    return;
                                }
                                //修改url成绝对路径
                                for (var i = 0; i < data.length; i++) {
                                    data[i].MarkPath = C_ROOT + data[i].MarkPath;
                                    data[i].checkedPwd = false;
                                }
                                var win = Ext.create('Ext.ng.wf.SignatureWin', {
                                    sdata: data,
                                    callback: Ext.bind(function (code, url) {
                                        this.signature = code;
                                        this.el.dom.src = url;
                                    }, this)
                                });
                                win.show();
                            }
                            else {
                                $WorkFlow.msgAlert($WF.NGLang.getSignatureError || '获取签章数据出错', resp.errorMsg);
                            }
                        }, this)
                    });
                }, this);
            }
        }, me);
    }
});

Ext.define('Ext.ng.WorkFlowPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ngWorkFlowPanel',
    autoHeight: true,
    frame: true,
    border: false,
    taskDealInfo: {},
    taskDynamicInfo: {},
    hasHis: false,
    hisGridStateId: Ext.emptyText,
    layout: 'border',
    fieldDefaults: {
        labelWidth: 80,
        anchor: '100%',
        margin: '0 5 0 0',
        msgTarget: 'side'
    },
    defaults: {
        anchor: '100%'
    },
    otype: 'edit',
    workFlowInfo: {},
    bizType: '',
    getAttachData: Ext.emptyFn,
    bizPhid: '',
    refreshTaskList: false,
    isFromProtal: false,
    showFlowHis: false,
    bizSaveFn: Ext.emptyFn,
    curOperates: Ext.emptyText,
    bizSaveAsync: false,
    showToolBarItems: [],
    showAllToolBar: false,
    hisGridMaxHeight: 150,
    initComponent: function () {
        var me = this;
        if (Ext.tip.QuickTipManager.isEnabled() === false) {
            Ext.tip.QuickTipManager.init();
        }
        if (!Ext.isEmpty(document.location.search) && document.location.search.concat('?')) {
            var qs = Ext.Object.fromQueryString(document.location.search.split('?')[1]) || {};
            if (!Ext.isEmpty(qs.iswftasklist) && qs.iswftasklist == 'true') {
                me.refreshTaskList = true;
            }
            if (!Ext.isEmpty(qs.isFromProtal) && qs.isFromProtal == 'true') {
                me.isFromProtal = true;
            }
        }
        if (Ext.isEmpty(me.workFlowInfo)) {
            me.workFlowInfo = {};
        }
        else if (Ext.isString(me.workFlowInfo)) {
            var info = Ext.htmlDecode(me.workFlowInfo);
            if (!Ext.isEmpty(info)) {
                me.workFlowInfo = Ext.decode(info);
            }
            else {
                me.workFlowInfo = {};
            }
        }
        //设置多语言
        $WF.setNGLang(me.workFlowInfo.NGLang);
        if (me.otype === "add" || (me.showFlowHis == false && Ext.isEmpty(me.workFlowInfo.wfpiid))) {
            me.hide();
            this.callParent();
            return;
        }
        me.addEvents('taskComplete');
        me.hisStore = Ext.create('Ext.data.Store', {
            storeId: 'hisStore',
            fields: ['taskid', 'task_des', 'username', 'actionname', 'msg', 'end_time', 'duration', 'signature', 'att_count']
        });
        var piid = me.workFlowInfo.wfpiid ? me.workFlowInfo.wfpiid : '';
        Ext.Ajax.request({
            url: C_ROOT + 'WorkFlow3/WorkFlow/GetWorkFlowHis?piid=' + piid + '&bizid=' + me.bizType + '&bizpk=' + me.bizPhid,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.success) {
                    if (Ext.isEmpty(me.workFlowInfo.wftaskid) && resp.hasHis) {
                        me.addFlowHisToolBar();
                        me.show();
                    }
                    me.hisStore.loadData(resp.data);
                } else {
                    $WorkFlow.msgAlert($WF.NGLang.getFlowHisError || '取数流程历史失败', resp.errorMsg);
                }
            }
        });
        if (Ext.isEmpty(me.workFlowInfo.wftaskid)) { //非工作流界面进入
            me.items = me.buildHisLayout();
            me.hide();
        } else {
            me.AddWFToolBar();
            me.items = me.buildWFLayout();
        }
        me.workFlowInfo.isBizApproved = me.workFlowInfo.isBizApproved || false;
        this.callParent();
        me.on("afterrender", function () {
            var me = this;
            if (me.workFlowInfo.wftaskid) {
                if (me.otype == 'view') {
                    me.down("#wf_remark").userSetReadOnly(false);
                    me.down("#system_message").setReadOnly(false);
                    me.down("#mobile_message").setReadOnly(false);
                    me.down("#messageContent").setReadOnly(false);
                    me.down("#messageUsers").setReadOnly(false);
                }
                if (me.workFlowInfo.uiConstraint) {
                    $SetWorkFlowUIState(me.workFlowInfo.uiConstraint);
                    //var uiConstraint = [{ "m1#Cname": '2' }];
                    //$SetWorkFlowUIState(uiConstraint);
                }
            }
        });
    },
    //在toolbar上增加工作流相关按钮
    AddWFToolBar: function () {
        var me = this;
        if (me.toolbar && !Ext.isEmpty(me.workFlowInfo.wftaskid)) {
            //隐藏业务单据toolbar按钮
            if (me.showAllToolBar == false) {
                var index = (function () {
                    var keys = me.toolbar.items.keys;
                    for (var i = 0; i < keys.length; i++) {
                        if (keys[i].indexOf("tbfill-") === 0) {
                            return i + 1;
                        }
                    }
                    return keys.length;
                })();
                if (index > 0) index = index - 1;
                //把单据上的按钮隐藏掉
                for (var i = 0; i < index; i++) {
                    var btn = me.toolbar.items.items[i];
                    if (btn && btn.itemId) {
                        if (me.showToolBarItems.indexOf(btn.itemId) < 0)
                            me.toolbar.hiddeBtn(btn.itemId);
                    }
                }
            }
            //me.toolbar.insert(0, {
            //    id: 'wfbtn_nextTask',
            //    iconCls: "icon-NextRec",
            //    text: $WF.NGLang.btnNextTask || "跳过",
            //    handler: function () {
            //        var nextTaskData = $WorkFlow.getNextTask(me.workFlowInfo.createTime);
            //        if (Ext.isEmpty(nextTaskData.id)) {
            //            $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.hasNoNextTask || '当前已是最后一个任务!');
            //            return;
            //        } else {
            //            $WorkFlow.openNextAppFlowTask(nextTaskData, me.isFromProtal, me.refreshTaskList);
            //        }
            //    }
            //});
            me.toolbar.insert(0, {
                id: 'wfbtn_flowDiagram',
                iconCls: "icon-History",
                text: $WF.NGLang.btnFlowDiagram || "流程图",
                handler: function () {
                    $WorkFlow.showPiDiagram(me.workFlowInfo.wfpiid, me.workFlowInfo.activityId);
                }
            });
            if (me.workFlowInfo.canAttach) {
                //me.toolbar.insert(0, {
                //    id: 'wfbtn_attachment',
                //    iconCls: "icon-Attachment",
                //    text: $WF.NGLang.btnAttach || "工作流附件",
                //    handler: function () {
                //        if (Ext.isEmpty(me.attachGuid)) {
                //            //初始化附件控件
                //            var param = {
                //                product: "",//传相应产品，i6、i6s、i6P、A3、GE，可为空
                //                attachguid: "",//会话guid,可传空 生成方法 cs代码: Guid.NewGuid().ToString(),
                //                attachTName: "c_pfc_attachment",
                //                busTName: "act_hi_taskinst",//传相应业务表
                //                busid: me.workFlowInfo.wftaskid,//传相应产品业务单据phid
                //            };
                //            var result = LoadAttach.InitBeforeOpen(param);
                //            var obj = eval(result);
                //            if (!Ext.isEmpty(obj)) {
                //                if (obj.status == "success")//初始化成功
                //                {
                //                    me.attachGuid = obj.msg;
                //                }
                //                else {
                //                    $WorkFlow.msgAlert($WF.NGLang.alertTitle, $WF.NGLang.attachCtlInitError);
                //                    return;
                //                }
                //            } else {
                //                $WorkFlow.msgAlert($WF.NGLang.alertTitle, $WF.NGLang.attachCtlInitError);
                //                return;
                //            }
                //        }
                //        var opt = {
                //            product: "",//传相应产品，可为空
                //            mode: "NG3",//固定传NG3
                //            openbymianframe: "1",//通过主框架打开附件 0或空 否  1 是 固定传1
                //            oper: "winfrom",//web、pb、winfrom、progress（进度条模式）,固定传 winfrom
                //            asr_tbl: "c_pfc_attachment",
                //            tbl: "act_hi_taskinst",//传相应业务表
                //            fill: $appinfo.userID,//传相应操作员id
                //            fillname: $appinfo.username,//传相应操作员姓名
                //            chkSign: "0",//默认传0
                //            chkCheckIn: "0",//默认传0
                //            btnAdd: "1",//新增按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            addserverstuts: "0",//导入按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnScan: "1",//扫描按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnDelete: "1",//删除按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnEdit: "1",//编辑按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnView: "1",//查看按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnDownload: "1",//下载按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnCancel: "1",//取消按钮 oper非web时有效 0、禁用 1、显示 2、隐藏
                //            btnOk: "1",//确定按钮 oper非web时有效 0、禁用 1、显示 2、隐藏                    
                //            btnWebAdd: "2",//web新增按钮 oper为web时有效 0、禁用 1、显示 2、隐藏
                //            btnWebOk: "2",//web新增按钮 oper为web时有效 0、禁用 1、显示 2、隐藏
                //            archivestuts: "2",//归档按钮 oper非web时有效 0、禁用 1、显示 2、隐藏                    
                //            status: "add",//add 新增模式，view 查看模式， edit 编辑模式
                //            showlist: "1",//显示文件列表 0 不显示 1显示
                //            zip: "0", //附件压缩 0 不压缩 1 压缩
                //            filenum: "",//附件上传数量限制 0或空不限制附件上传数量
                //            filetype: "",//附件上传类型限制 目前仅支持传入"image",如果传了"image"附件控件就只能上传图片
                //            guid: me.attachGuid //传第二步附件初始化获得的guid
                //        };
                //        //打开附件
                //        LoadAttach.Init(opt);
                //    }
                //});
            }
            if (me.workFlowInfo.canTermination) {
                me.toolbar.insert(0, {
                    id: 'wfbtn_terminate',
                    iconCls: "icon-Cancel",
                    text: $WF.NGLang.btnAbort || "终止",
                    handler: function () {
                        me.flowTerminate(1);
                    }
                });
            }
            if (me.workFlowInfo.canAddTis) {
                //me.toolbar.insert(0, {
                //    id: 'wfbtn_addsubtis',
                //    iconCls: "icon-Assign",
                //    text: $WF.NGLang.btnAddsubtis || "加签",
                //    handler: function () {
                //        me.addSubTasks(1);
                //    }
                //});
            }
            if (me.workFlowInfo.canTransmit) {
                //me.toolbar.insert(0, {
                //    id: 'wfbtn_transmit',
                //    iconCls: "icon-EditRow",
                //    text: $WF.NGLang.btntransmit || "转签",
                //    handler: function () {
                //        me.taskReassign(1);
                //    }
                //});
            }
            if (me.workFlowInfo.canUndo) {
                me.toolbar.insert(0, {
                    id: 'wfbtn_rollback',
                    iconCls: "icon-PriorRec",
                    text: $WF.NGLang.btnrollback || "驳回",
                    handler: function () {
                        me.rollBack(0);
                    }
                });
            }
            me.toolbar.insert(0, {
                id: 'wfbtn_taskcomplete',
                iconCls: "icon-Confirm",
                text: $WF.NGLang.btnSubmit || "提交",
                handler: function () {
                    me.taskComplete(0);
                }
            });
        }
    },
    //在toolbar上增加工作流历史按钮
    addFlowHisToolBar: function () {
        var me = this;
        if (me.toolbar) {
            var index = (function () {
                var keys = me.toolbar.items.keys;
                for (var i = 0; i < keys.length; i++) {
                    if (keys[i].indexOf("tbfill-") === 0) {
                        return i + 1;
                    }
                }
                return keys.length;
            })();
            if (index > 0) index = index - 1;
            me.toolbar.insert(index, {
                id: 'wfbtn_flowinfo',
                iconCls: "icon-History",
                text: "流程信息",
                handler: function () {
                    $WorkFlow.showFlowInfo(me.bizType, me.bizPhid);
                }
            });
        }
    },
    buildHisLayout: function () {
        var me = this;
        me.layout = 'hbox';
        return [{
            xtype: 'ngGridPanel',
            name: 'lzhisShow',
            flex: 1,
            padding: '0 0 0 5',
            stateId: me.hisGridStateId,
            maxHeight: me.hisGridMaxHeight,
            //height: 150,
            store: me.hisStore,
            columns: $WorkFlow.getHisGridColumns(),
            listeners: {
                cellclick: function (view, cell, cellIndex, record, row, rowIndex, e) {
                    if (cellIndex == 7 && e.target.tagName == 'A') {
                        $WorkFlow.showTaskAttachment(record);
                    }
                }
            }
        }];
    },
    //设置控件
    buildWFLayout: function () {
        var me = this, imgSrc;
        if (!Ext.isEmpty(me.workFlowInfo.defaultSignature)) {
            imgSrc = C_ROOT + me.workFlowInfo.signatureUrl;
        }
        me.height = 170;
        return [{
            xtype: 'panel',
            region: 'center',
            layout: 'border',
            border: 0,
            //height: 170,
            items: [{
                xtype: 'panel',
                region: 'west',
                layout: 'border',
                border: 0,
                //height: 170,
                width: 250,
                items: [{
                    xtype: 'panel',
                    region: 'center',
                    layout: 'border',
                    items: [{
                        xtype: 'ngTextArea',//ngUsefulTextArea
                        region: 'center',
                        controlid: 'wf_remark',
                        itemId: "wf_remark",
                        name: 'SuggestValue',
                        emptyText: $WF.NGLang.emptyRemarkInfo || '请输入办理意见',
                        width: 150,
                        height: 140,
                        //height:'100%'
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER && me.down("#wf_remark").getValue() == "") {
                                    if (me.bizType == "GHProject") {
                                        Ext.Ajax.request({
                                            params: { "id": me.bizPhid, "tabtype": "projectmst" },
                                            async: false,
                                            url: C_ROOT + 'GXM/XM/ProjectMst/GetProjectMstInfo',
                                            success: function (response) {
                                                var resp = Ext.JSON.decode(response.responseText);
                                                SelectYJK(me, resp.Data.FBudgetDept, resp.Data.FDeclarationUnit);
                                            }
                                        });
                                    }
                                    if (me.bizType == "GHBudget") {
                                        Ext.Ajax.request({
                                            params: { "id": me.bizPhid, "tabtype": "budgetmst" },
                                            async: false,
                                            url: C_ROOT + 'GYS/YS/BudgetMst/GetBudgetMstInfo',
                                            success: function (response) {
                                                var resp = Ext.JSON.decode(response.responseText);
                                                SelectYJK(me, resp.Data.FBudgetDept, resp.Data.FDeclarationUnit);
                                            }
                                        });
                                    }
                                    if (me.bizType == "GHExpense") {
                                        Ext.Ajax.request({
                                            params: { "id": me.bizPhid, "tabtype": "expensemst" },
                                            async: false,
                                            url: C_ROOT + 'GYS/YS/ExpenseMst/GetExpenseMstInfo',
                                            success: function (response) {
                                                var resp = Ext.JSON.decode(response.responseText);
                                                SelectYJK(me, resp.Data.FBudgetDept, resp.Data.FDeclarationunit);
                                            }
                                        });
                                    }

                                    if (me.bizType == "GHSubject") {
                                        Ext.Ajax.request({
                                            params: { "id": me.bizPhid, "tabtype": "ghsubject" },
                                            async: false,
                                            url: C_ROOT + 'GYS/YS/GHSubject/GetGHSubjectInfo',
                                            success: function (response) {
                                                var resp = Ext.JSON.decode(response.responseText);
                                                SelectYJK(me, resp.Data.FDeclarationDept, resp.Data.FDeclarationUnit);
                                            }
                                        });
                                    }

                                }
                            }
                        }
                    }, {
                        xtype: 'signatureImage',
                        region: 'east',
                        name: 'signature',
                        itemId: "signature",
                        signature: me.workFlowInfo.defaultSignature || '',
                        src: imgSrc,
                        width: 100,
                        padding: '15 0 15 0',
                        border: true
                        //height: 150
                    }]
                }, {
                    xtype: 'panel',
                    region: 'south',
                    border: 0,
                    layout: 'hbox',
                    items: [{
                        xtype: 'checkbox',
                        boxLabel: $WF.NGLang.msgTypeSys || '消息自由呼通知',
                        name: 'system_message',
                        itemId: 'system_message',
                        margin: '0 20 0 5',
                        listeners: {
                            change: function (ctl, newValue) {
                                me.noticeUiSetting();
                            }
                        },
                        inputValue: 0
                    }, {
                        xtype: 'checkbox',
                        boxLabel: $WF.NGLang.msgTypeSMS || '移动消息通知',
                        itemId: 'mobile_message',
                        name: 'mobile_message',
                        listeners: {
                            change: function (ctl, newValue) {
                                me.noticeUiSetting();
                            }
                        },
                        inputValue: 0
                    }]
                }
                ]
            }, {
                xtype: 'ngGridPanel',
                region: 'center',
                name: 'lzhisShow',
                flex: 1,
                padding: '0 0 0 5',
                stateId: me.hisGridStateId,
                //height: 150,
                store: me.hisStore,
                columns: $WorkFlow.getHisGridColumns(),
                listeners: {
                    cellclick: function (view, cell, cellIndex, record, row, rowIndex, e) {
                        if (cellIndex == 7 && e.target.tagName == 'A') {
                            $WorkFlow.showTaskAttachment(record);
                        }
                    }
                }
            }]
        }, {
            xtype: 'panel',
            region: 'south',
            itemId: 'noticePanel',
            border: 0,
            height: 50,
            layout: 'column',
            hidden: true,
            items: [{
                xtype: 'ngTextArea',
                itemId: "messageContent",
                name: 'message',
                margin: '5 5 0 5',
                emptyText: $WF.NGLang.msgContent || '消息',
                columnWidth: 0.6,
                height: 40
            }, {
                xtype: 'WFNoticeUseerTrigger',
                margin: '12 2 0 0',
                itemId: 'messageUsers',
                fieldLabel: $WF.NGLang.msgReceiver || '消息接收人',
                editable: false,
                //width: 400
                columnWidth: 0.4
            }]
        }];
    },
    setYJ: function (text) {
        me = this;
        me.down("#wf_remark").setValue(text);
    },
    //判断是否输入办理意见
    isValid: function (ignoreRemark) {
        var me = this;
        var valid = true;
        if (me.workFlowInfo.compType != 4) {
            me.taskDealInfo.ignoreCurUserProtalRefresh = true
        }
        me.taskDealInfo.taskid = me.workFlowInfo.wftaskid;
        me.taskDealInfo.piid = me.workFlowInfo.wfpiid;
        me.taskDealInfo.biztype = me.bizType;
        me.taskDealInfo.bizphid = me.bizPhid;
        me.taskDealInfo.dynamicUser = {};
        me.taskDealInfo.attguid = me.attachGuid;
        if (!Ext.isEmpty(me.workFlowInfo.compId))
            me.taskDealInfo.compId = me.workFlowInfo.compId;
        if (!Ext.isEmpty(me.workFlowInfo.compType))
            me.taskDealInfo.compType = me.workFlowInfo.compType;
        var remark = me.down("#wf_remark").getValue();
        var minRemarkLen = 0;
        if (!Ext.isEmpty(me.workFlowInfo.minCommentLen)) {
            minRemarkLen = me.workFlowInfo.minCommentLen;
        }
        if (Ext.isEmpty(remark)) {
            if (Ext.isEmpty(ignoreRemark) || ignoreRemark === false || minRemarkLen > 0) {
                Ext.create('Ext.ng.MessageBox').Error($WF.NGLang.hasNoremark || '请输入办理意见', 1);
                return false;
            }
            else {
                me.taskDealInfo.remark = "";
            }
        }
        else {
            if (remark.length < minRemarkLen) {
                Ext.create('Ext.ng.MessageBox').Error($WF.format($WF.NGLang.reamrkHasMore || '请至少输入{0}字的意见！', minRemarkLen), 1);
                return false;
            }
            else {
                me.taskDealInfo.remark = remark;
            }
        }
        //判断是否选择签章        
        var singature = '';
        if (!Ext.isEmpty(me.down('#signature'))) {
            singature = me.down('#signature').signature
        }
        if (Ext.isEmpty(singature)) {
            singature = '';
        }
        me.taskDealInfo.signature = singature;
        if (me.workFlowInfo.needsignature && Ext.isEmpty(me.taskDealInfo.signature)) {
            Ext.create('Ext.ng.MessageBox').Error($WF.NGLang.mustSignature || '必需签章！', 1);
            return false;
        }
        //判断是否发送消息通知
        var isSystemMsg = me.down('#system_message').getValue();
        var isMobileMsg = me.down('#mobile_message').getValue();
        if (isSystemMsg || isMobileMsg) {
            var msgText = me.down('#messageContent').getValue();
            if (Ext.isEmpty(msgText)) {
                Ext.create('Ext.ng.MessageBox').Error($WF.NGLang.hasNoMsgContent || '请输入消息内容', 1);
                return false;
            }
            var msgUser = me.down('#messageUsers').data;
            if (Ext.isEmpty(msgUser)) {
                Ext.create('Ext.ng.MessageBox').Error($WF.NGLang.hasNoMsgReceiver || '请输入消息接收者', 1);
                return false;
            }
            var tempdata = [];
            Ext.Array.forEach(msgUser, function (item) {
                tempdata.push(item.userId);
            });
            me.taskDealInfo.noticeInfo = { 'systemMsg': isSystemMsg, 'mobileMsg': isMobileMsg, 'msgText': msgText, 'users': tempdata };
        }
        return valid;
    },
    //判断是否输入办理意见及调用业务单据保存
    validAndSaveBiz: function (oper) {
        var me = this;
        if (!me.isValid())
            return false;
        //如果单据已审批则不再调用保存
        if (me.workFlowInfo.isBizApproved == true) {
            return true;
        }
        //调用js方法进行单据保存，如果失败则终止操作    
        if (me.bizSaveFn != Ext.emptyFn) {
            if (me.bizSaveAsync) {
                me.bizSaveFn(function () {
                    me.bizSaveCallBack(oper);
                }, oper);
                return false;
            }
            else if (me.bizSaveFn() === false) {
                return false;
            }
        }
        return true;
    },
    setBizAttachData: function (oper) {
        var me = this;
        if (me.getAttachData && me.getAttachData != Ext.emptyFn) {
            var tempData = me.getAttachData(oper);
            if (Ext.isObject(tempData)) {
                me.taskDealInfo.bizAttachData = tempData;
            }
        }
    },
    //单据保存后回调
    bizSaveCallBack: function (oper) {
        var me = this;
        //调用业务组件附加数据
        me.setBizAttachData(oper);
        if (oper == "taskComplete") {
            me.taskComplete(1);
        }
        else if (oper == "rollBack") {
            me.rollBack(1);
        }
        else if (oper == "addSubTasks") {
            me.addSubTasks(2);
        }
        else if (oper == "flowTerminate") {
            me.flowTerminate(2);
        }
        else if (oper == "taskReassign") {
            me.taskReassign(2);
        }
    },
    //转签操作
    taskReassign: function (step) {
        var me = this;
        if (step == 1) {
            if (!me.validAndSaveBiz("taskReassign")) {
                return;
            }
            me.taskReassign(2);
        }
        else if (step == 2) {
            var userHelpWin = Ext.create('Ext.ng.UserHelpWindow', {
                listeners: {
                    close: function () {
                        me.reloadPage();
                    },
                    beforeshow: $winBeforeShow,
                    beforeclose: $winBeforeClose
                },
                callback: function () {
                    var d = userHelpWin.getData();
                    me.taskDealInfo.user = d.code;
                    Ext.Ajax.request({
                        params: { 'data': JSON.stringify(me.taskDealInfo) },
                        async: false,
                        url: C_ROOT + 'WorkFlow3/WorkFlow/TaskReassign',
                        success: function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp.success) {
                                me.successAlterAndClose($WF.format($WF.NGLang.taskReassignTo || '任务已转签给:{0}', d.name));
                            } else {
                                $WorkFlow.msgAlert($WF.NGLang.taskReassignError || '转签操作失败', resp.errorMsg);
                            }
                        }
                    });
                }
            });
            userHelpWin.show();
        }
    },
    //流程终止
    flowTerminate: function (step) {
        var me = this;
        if (step == 1) {
            if (!me.validAndSaveBiz("flowTerminate")) {
                return;
            }
            me.flowTerminate(2);
        }
        else if (step == 2) {
            Ext.Ajax.request({
                params: { 'data': JSON.stringify(me.taskDealInfo) },
                async: false,
                url: C_ROOT + 'WorkFlow3/WorkFlow/FlowTerminate',
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.success) {
                        me.successAlterAndClose($WF.NGLang.terminateSuccess || '流程终止成功');
                    } else {
                        $WorkFlow.msgAlert($WF.NGLang.terminateFailure || '流程终止失败', resp.errorMsg);
                    }
                }
            });
        }
    },
    //加签
    addSubTasks: function (step) {
        var me = this;
        if (step == 1) {
            if (!me.validAndSaveBiz("addSubTasks")) {
                return;
            }
            me.addSubTasks(2);
        }
        else if (step == 2) {
            var userHelpWin = Ext.create('Ext.ng.MultiUserHelpWindow', {
                listeners: {
                    close: function () {
                        me.reloadPage();
                    },
                    beforeshow: $winBeforeShow,
                    beforeclose: $winBeforeClose
                },
                callback: function () {
                    var d = userHelpWin.getData();
                    me.taskDealInfo.users = d.code;
                    Ext.Ajax.request({
                        params: { 'data': JSON.stringify(me.taskDealInfo) },
                        async: false,
                        url: C_ROOT + 'WorkFlow3/WorkFlow/AddSubTasks',
                        success: function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp.success) {
                                me.successAlterAndClose($WF.NGLang.addSubTasksSuccess || '加签操作成功');
                            } else {
                                $WorkFlow.msgAlert($WF.NGLang.addSubTasksFailure || '加签操作失败', resp.errorMsg);
                            }
                        }
                    });
                }
            });
            userHelpWin.show();
        }
    },
    //任务办理
    taskComplete: function (step) {
        var me = this;
        if (step == 0) {
            if (!me.isValid(true)) {
                return;
            }
            //非自定义类型组件调用单据保存
            if (me.workFlowInfo.isBizApproved == false && me.workFlowInfo.compType < 4 && me.bizSaveFn != Ext.emptyFn) {
                if (me.bizSaveAsync) {
                    me.bizSaveFn(function () {
                        me.bizSaveCallBack("taskComplete");
                    }, "taskComplete");
                    return;
                }
                else if (me.bizSaveFn() == false) {
                    return;
                }
            }
            me.taskComplete(1);
        }
        else if (step == 1) {
            var isApprove = me.workFlowInfo.compType == 3 ? '1' : '0';
            if (me.workFlowInfo.hasParentTask == false) {
                //获取节点动态指派数据
                Ext.Msg.show({
                    msg: $WF.NGLang.getTaskDataInfo || '获取任务办理相关数据' + '...',
                    wait: true,
                    progress: true,
                    dosable: true,
                    waitConfig: {
                        interval: 200
                    },
                    icon: Ext.Msg.INFO
                });
                Ext.Ajax.request({
                    url: C_ROOT + 'WorkFlow3/WorkFlow/GetTaskDynamicInfo?taskid=' + me.workFlowInfo.wftaskid + '&piid' + me.workFlowInfo.wfpiid + '&biztype=' + me.bizType + '&pks=' + me.bizPhid + '&isApproveTask=' + isApprove,
                    success: function (response) {
                        Ext.Msg.close();
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.success) {
                            me.taskDynamicInfo = resp.data;
                            if (me.workFlowInfo.compType == 3) {
                                if (resp.checkApprove.result == 2) {
                                    $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.checkApproveFailure || '审批检测失败:' + resp.checkApprove.msg);
                                    return;
                                }
                                else if (resp.checkApprove.result == 1) {
                                    $WorkFlow.msgConfirm($WF.NGLang.alertTitle || '提示', resp.checkApprove.msg, function (id) {
                                        if (id.toString() == 'yes') {
                                            me.taskComplete(2);
                                        }
                                    });
                                }
                                else {
                                    me.taskComplete(2);
                                }
                            } else {
                                me.taskComplete(2);
                            }
                        }
                        else {
                            $WorkFlow.msgAlert($WF.NGLang.getTaskDynamicError || '获取节点动态指派数据出错', resp.errorMsg);
                        }
                    }
                });
            } else {
                me.taskComplete(4);
            }
        }
        else if (step == 2) { //判断是否需要动态选择分支
            if (me.taskDynamicInfo.needDynamicBranch) {
                var win = Ext.create('Ext.ng.WFDynamicBranchWin', {
                    branchData: me.taskDynamicInfo.nextTrans,
                    cancelback: me.reloadPage,
                    callBack: function (data) {
                        me.taskDealInfo.dynamicNode = data;
                        me.taskComplete(3);
                    }
                });
                win.show();
            }
            else {
                me.taskComplete(3);
            }
        }
        else if (step == 3) { //分支选择完成、判断是否需要动态选择人员
            var win, nodeData = [];
            if (me.taskDynamicInfo.needDynamicUsers) {
                //根据动态分支计算哪些节点是需要动态设置人员的                
                for (var i = 0; i < me.taskDynamicInfo.dynamicUserNodes.length; i++) {
                    var d = me.taskDynamicInfo.dynamicUserNodes[i], needDynamic = true;
                    if (!Ext.isEmpty(me.taskDealInfo.dynamicNode) && !Ext.isEmpty(d.needDynamicNodes)) {
                        for (var dindex = 0; dindex < d.needDynamicNodes.length; dindex++) {
                            var result = me.taskDealInfo.dynamicNode.filter(function (o) { return o.destActId == d.needDynamicNodes[dindex]; });
                            if (result && result.length < 1) {
                                needDynamic = false;
                                break;
                            }
                        }
                    }
                    if (needDynamic) {
                        nodeData.push({ "nodeId": d.id, "nodeName": d.name, "exists_user": d.exists_user, "users": d.users, "dynamicAny": d.dynamicAny, "dispVal": "" });
                    }
                }
            }
            if (nodeData.length == 1) {
                var dynamicType = [];
                if (nodeData[0].exists_user) {
                    dynamicType.push(1);
                    if (nodeData[0].dynamicAny) {
                        dynamicType.push(2);
                    }
                } else {
                    dynamicType.push(2);
                }
                win = Ext.create('Ext.ng.WFDynamicNodeUserWin', {
                    radioItems: dynamicType,
                    psnUserData: nodeData[0].users,
                    actNodeId: nodeData[0].nodeId,
                    activityName: nodeData[0].nodeName,
                    callbackSimplyData: true,
                    cancelback: me.reloadPage,
                    callback: function (data) {
                        me.taskDealInfo.dynamicUser = data;
                        me.taskComplete(4);
                    }
                });
                win.show();
            }
            else if (nodeData.length > 1) {
                win = Ext.create('Ext.ng.WFNodeUserSetingWin', {
                    data: nodeData,
                    cancelback: me.reloadPage,
                    callback: function (data) {
                        Ext.apply(me.taskDealInfo.dynamicUser, data);
                        me.taskComplete(4);
                    }
                });
                win.show();
            }
            else { //不需要选择动态人员
                me.taskComplete(4);
            }
        }
        else if (step == 4) {
            // console.log(me.taskDealInfo);
            if (!Ext.isEmpty(me.taskDynamicInfo.dynamicUserCacheKey)) {
                me.taskDealInfo.dynamicUserCacheKey = me.taskDynamicInfo.dynamicUserCacheKey;
            }
            if (!Ext.isEmpty(me.taskDynamicInfo.dynamicBranchCacheKey)) {
                me.taskDealInfo.dynamicBranchCacheKey = me.taskDynamicInfo.dynamicBranchCacheKey;
            }
            if (me.workFlowInfo.compType == 4) {
                me.fireEvent('taskComplete', me.workFlowInfo.compId, me.taskDealInfo);
            } else {
                Ext.Msg.show({
                    msg: $WF.NGLang.submitTaskComplete || '提交任务办理' + '...',
                    wait: true,
                    progress: true,
                    dosable: true,
                    waitConfig: {
                        interval: 200
                    },
                    icon: Ext.Msg.INFO
                });
                Ext.Ajax.request({
                    params: { 'data': JSON.stringify(me.taskDealInfo) },
                    // async: false,
                    url: C_ROOT + 'WorkFlow3/WorkFlow/taskComplete',
                    success: function (response) {
                        // Ext.Msg.close();
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.success) {
                            //var TaskName = me.taskDynamicInfo.dynamicUserNodes[0].activityName;  //节点名称
                            //var listId = me.taskDealInfo.bizphid;                                //单据phid
                            //var biztypeName = me.biztype;                                       //业务类型
                            //Ext.Ajax.request({
                            //    params: { 'ID': listId, 'activeName': TaskName, 'biztypeName': biztypeName },
                            //    url: C_ROOT + 'GXM/XM/ProjectMst/SaveNextApprove',
                            //    async: false
                            //});

                            if (Ext.isEmpty(resp.flowErrorMsg)) {
                                me.successAlterAndClose($WF.NGLang.TaskCompleteSuccess || '任务办理成功');
                            }
                            else {
                                me.successAlterAndClose('任务办理成功,但' + resp.flowErrorMsg);
                            }
                        } else {
                            $WorkFlow.msgAlert($WF.NGLang.TaskCompleteError || '任务办理失败', resp.errorMsg);
                        }
                    }
                });
            }
        }
    },
    //回退操作
    rollBack: function (step) {
        var me = this;
        var RollbackName = ""; //回退节点名称
        if (step == 0) {
            if (!me.validAndSaveBiz("rollBack")) {
                return;
            }
            me.rollBack(1);
        }
        else if (step == 1) {
            if (me.workFlowInfo.compType == 3) {
                Ext.Msg.show({
                    msg: $WF.NGLang.checkCancelApprove || '检测单据是否允许取消审批' + '...',
                    wait: true,
                    progress: true,
                    dosable: true,
                    waitConfig: {
                        interval: 200
                    },
                    icon: Ext.Msg.INFO
                });
                Ext.Ajax.request({
                    // async: false,
                    url: C_ROOT + 'WorkFlow3/WorkFlow/CheckCancelApproveValid?taskid=' + me.workFlowInfo.wftaskid + '&piid=' + me.workFlowInfo.wfpiid + '&bizid=' + me.bizType + '&bizpk=' + me.bizPhid,
                    success: function (response) {
                        Ext.Msg.close();
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.success) {
                            if (resp.result == 2) {
                                $WorkFlow.msgAlert($WF.NGLang.canNotCancelApprove || '单据不允许取消审批', resp.msg);
                                return;
                            }
                            else if (resp.result == 1) {
                                $WorkFlow.msgConfirm($WF.NGLang.alertTitle || '提示', resp.msg, function (id) {
                                    if (id.toString() == 'yes') {
                                        me.rollBack(2);
                                    }
                                });
                            }
                            else {
                                me.rollBack(2);
                            }
                        }
                        else {
                            $WorkFlow.msgAlert($WF.NGLang.checkCancelApproveError || '检测是否允许取消审批出错！', resp.errorMsg);
                        }
                    }
                });
            }
            else {
                me.rollBack(2);
            }
        }
        else if (step == 2) {
            //设置单据附加数据（审批节点驳回时）
            if (me.workFlowInfo.isBizApproved == true) {
                me.setBizAttachData('rollBack');
            }
            var win = Ext.create('Ext.ng.RollBackNodeWin', {
                taskId: me.workFlowInfo.wftaskid,
                cancelback: me.reloadPage,
                callback: function (data, dyUser, rollName) {
                    RollbackName = rollName;  //节点名称
                    me.taskDealInfo.nodeid = data;
                    if (dyUser) {
                        me.rollBack(3);
                    }
                    else {
                        me.rollBack(4);
                    }
                }
            });
            win.show();
        }
        else if (step == 3) {
            //重新指派人员
            Ext.Ajax.request({
                params: { 'taskid': me.workFlowInfo.wftaskid, 'nodeid': me.taskDealInfo.nodeid },
                //async: false,
                url: C_ROOT + 'WorkFlow3/WorkFlow/GetNodeDynamicUser',
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    var dynamicType = [];
                    if (resp.exists_user) {
                        dynamicType.push(1);
                        if (resp.dynamicAny) {
                            dynamicType.push(2);
                        }
                    } else {
                        dynamicType.push(2);
                    }
                    if (resp.dynamicUser) {
                        win = Ext.create('Ext.ng.WFDynamicNodeUserWin', {
                            radioItems: dynamicType,
                            psnUserData: resp.users,
                            actNodeId: me.taskDealInfo.nodeid,
                            activityName: resp.name,
                            cancelback: me.reloadPage,
                            callbackSimplyData: true,
                            callback: function (data) {
                                me.taskDealInfo.dynamicUser = data;
                                me.rollBack(4);
                            }
                        });
                        win.show();
                    } else {
                        me.rollBack(4);
                    }
                }
            });
        }
        else if (step == 4) {
            if (!Ext.isEmpty(me.workFlowInfo.sc_rejectReason)) {
                var win = Ext.create('Ext.ng.SC3RejectReason', {
                    cancelback: me.reloadPage,
                    reasonData: me.workFlowInfo.sc_rejectReason,
                    callback: function (id) {
                        me.taskDealInfo.sc3RejectReason = id;
                        me.rollBack(5);
                    }
                });
                win.show();
            } else {
                me.rollBack(5);
            }
        }
        else if (step == 5) {
            //调用后端方法处理回退
            Ext.Msg.show({
                msg: $WF.NGLang.submitRollBack || '提交驳回操作' + '...',
                wait: true,
                progress: true,
                dosable: true,
                waitConfig: {
                    interval: 200
                },
                icon: Ext.Msg.INFO
            });
            Ext.Ajax.request({
                params: { 'data': JSON.stringify(me.taskDealInfo) },
                // async: false,
                url: C_ROOT + 'WorkFlow3/WorkFlow/RollBack',
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.success) {
                        //var listPhid = me.taskDealInfo.bizphid; //单据phid
                        //var taskDealName = RollbackName;//节点名称
                        //var biztypeName = me.biztype;    //业务类型
                        //Ext.Ajax.request({
                        //    params: { 'ID': listPhid, 'activeName': taskDealName, 'biztypeName': biztypeName },
                        //    url: C_ROOT + 'GXM/XM/ProjectMst/SaveNextApprove',
                        //    async: false
                        //});

                        //修改状态为已退回
                        Ext.Ajax.request({
                            params: { 'id': me.taskDealInfo.bizphid, 'biztypeName': me.taskDealInfo.biztype },
                            async: false,
                            url: C_ROOT + 'GXM/XM/ProjectMst/SaveRollBack',
                            success: function (response) {
                                var resp = Ext.JSON.decode(response.responseText);
                                if (resp.Status === "success") {

                                }
                            }
                        });

                        me.successAlterAndClose($WF.NGLang.rollBackSuccess || '驳回操作成功');
                    } else {
                        $WorkFlow.msgAlert($WF.NGLang.rollBackError || '驳回操作失败', resp.errorMsg);
                    }
                }
            });
        }
    },
    noticeUiSetting: function () {
        var me = this;
        var isSystemMsg = me.down('#system_message').getValue();
        var isMobileMsg = me.down('#mobile_message').getValue();
        if (isSystemMsg || isMobileMsg) {
            me.setHeight(220);
            me.down('#noticePanel').show();
        } else {
            me.setHeight(170);
            me.down('#noticePanel').hide();
        }
    },
    reloadPage: function () {
        window.location.reload();
    },
    successAlterAndClose: function (msg) {
        $NG3Refresh();
        var me = this;
        //刷新portal        
        RefreshPortal("WorkFlowTask", '0');
        RefreshPortal("MyParticipantRunningFlow", '0');
        //获取下个任务id
        var nextTaskData = {};
        if (me.isFromProtal == true) { //portal进入且设置了连续办理
            nextTaskData = $WorkFlow.getOpenNextTask(me.workFlowInfo.createTime);
        }
        if (!Ext.isEmpty(nextTaskData.id)) { //需要打开下个任务
            $WorkFlow.showHoldAlterMsg(msg + ',' + ($WF.NGLang.openNextTask || '准备打开下个任务！'), 1.5, function () {
                $WorkFlow.openNextAppFlowTask(nextTaskData, true, false);
            });
        } else {
            $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', msg, function () {
                //刷新工作流任务列表界面
                if (me.refreshTaskList == true && window.external.RefreshWebListPage) {
                    window.external.RefreshWebListPage();
                }
                //关闭页面
                if ($WorkFlow.isWebFram() == true || window.external.RefreshWebListPage) {
                    $CloseTab();
                }
                else {
                    window.open("about:blank", "_self", ""); //解决chrome关不掉问题
                    window.close();
                }
            });
        }
    }
});

Ext.define("Ext.ng.wf.baseWindow", {
    extend: 'Ext.window.Window',
    cancelback: Ext.emptyFn,
    initComponent: function () {
        var me = this;
        me.callParent();
        me.on("beforeshow", $winBeforeShow);
        me.on("beforeclose", $winBeforeClose);
        me.on("close", Ext.bind(function (p, opts) {
            me.invokeCancelback();
        }, me));
    },
    invokeCancelback: function () {
        var me = this;
        if (!Ext.isEmpty(me.cancelback)) {
            me.cancelback();
        }
    }
});

Ext.define("Ext.ng.RollBackNodeWin", {
    extend: 'Ext.ng.wf.baseWindow',
    title: '节点选择',
    closable: true,
    resizable: false,
    modal: true,
    width: 600,
    height: 400,
    gridStore: null,
    border: false,
    taskId: null,
    callback: Ext.emptyFn,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        me.title = $WF.NGLang.rollBackNodeWinTitle || me.title;
        Ext.define('rollmodel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'id',
                type: 'System.String',
                mapping: 'id'
            }, {
                name: 'name',
                type: 'System.String',
                mapping: 'name'
            }, {
                name: 'hisUsers',
                type: 'System.String',
                mapping: 'hisUsers'
            }, {
                name: 'canDynamicUser',
                type: 'bool',
                mapping: 'canDynamicUser'
            }]
        });
        var rollstore = Ext.create('Ext.data.Store', {
            model: 'rollmodel',
            autoLoad: true,
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'workflow3/workflow/GetRollBackNode?taskid=' + me.taskId,
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });
        var rollgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: false,
            //border: false,
            store: rollstore,
            frame: true,
            height: '100%',
            width: '100%',
            columnLines: true,
            forceFit: true,
            columns: [{
                header: $WF.NGLang.nodeName || '节点名称',
                dataIndex: 'name',
                flex: 1.5,
                sortable: false,
                hidden: false
            }, {
                header: $WF.NGLang.taskHisActor || '办理者',
                dataIndex: 'hisUsers',
                flex: 2.3,
                sortable: false
            }, {
                header: $WF.NGLang.nodeId || '节点编码',
                dataIndex: 'id',
                flex: 1,
                sortable: false,
                hidden: true
            }],
            plugins: ['wfShowConditionalToolTip']
        });
        var rolltoolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: [{
                xtype: "checkbox", itemId: "cbDyUser", boxLabel: $WF.NGLang.reSetUser || "重新指派人员", hidden: true, align: "left"
            }, "->",
            {
                itemId: "help_query", text: $WF.NGLang.btnOk || "确定", iconCls: 'add'
            },
            {
                itemId: "help_close", text: $WF.NGLang.btnCancel || "取消", iconCls: 'cross'
            }
            ]
        });
        me.items = [rolltoolbar, rollgrid];
        me.callParent();
        rolltoolbar.items.get('help_query').on('click', function () {
            var id = '';
            var data = rollgrid.getSelectionModel().getSelection();

            if (data.length < 1) {
                $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.hasNoRollBakNode || '请选择回退节点');
                return;
            }
            me.callback(data[0].data.id, rolltoolbar.down("#cbDyUser").getValue(), data[0].data.name); //增加回退节点名称
            me.destroy();
        });
        rolltoolbar.items.get('help_close').on('click', function () {
            me.destroy();
            me.invokeCancelback();
        });
        rollgrid.on("selectionchange", function (control, selected) {
            rolltoolbar.down("#cbDyUser").hide();
            rolltoolbar.down("#cbDyUser").reset();
            if (!Ext.isEmpty(selected)) {
                if (selected[0].data.canDynamicUser) {
                    rolltoolbar.down("#cbDyUser").show();
                }
            }
        });
    }
});

Ext.define("Ext.ng.SC3RejectReason", {
    extend: 'Ext.ng.wf.baseWindow',
    title: '驳回原因',
    closable: true,
    resizable: false,
    modal: true,
    width: 600,
    height: 400,
    //gridStore: null,
    border: false,
    callback: Ext.emptyFn,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        var store = Ext.create('Ext.data.Store', {
            fields: ['phid', 'name'],
            autoLoad: true,
            data: me.reasonData
        });
        var rollgrid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            store: store,
            frame: true,
            height: '100%',
            width: '100%',
            columnLines: true,
            forceFit: true,
            columns: [{
                header: 'ID',
                dataIndex: 'phid',
                flex: 2.3,
                sortable: false
            }, {
                header: '原因',
                dataIndex: 'name',
                flex: 1,
                sortable: false,
                hidden: false
            }],
            plugins: ['wfShowConditionalToolTip']
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
                    itemId: "help_query", text: $WF.NGLang.btnOk || "确定", iconCls: 'add'
                },
                {
                    itemId: "help_close", text: $WF.NGLang.btnCancel || "取消", iconCls: 'cross'
                }
            ]
        });
        me.items = [rolltoolbar, rollgrid];
        me.callParent();
        rolltoolbar.items.get('help_query').on('click', function () {
            var id = '';
            var data = rollgrid.getSelectionModel().getSelection();

            if (data.length < 1) {
                $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', '请选择驳回原因');
                return;
            }
            me.callback(data[0].data.phid);
            me.destroy();
        });
        rolltoolbar.items.get('help_close').on('click', function () {
            me.destroy();
            me.invokeCancelback();
        });
    }
});

Ext.define('Ext.ng.WFDynamicBranchWin', {
    extend: 'Ext.ng.wf.baseWindow',
    title: '指派下级分支',
    resizable: false,
    modal: true,
    height: 400,
    width: 400,
    border: false,
    layout: 'border',
    constrain: true,
    branchData: [],
    callBack: Ext.emptyFn,
    cancelback: Ext.emptyFn,
    initComponent: function () {
        var me = this;
        me.title = $WF.NGLang.dynamicBranchWinTitle || me.title;
        var toolbar = Ext.create('Ext.Toolbar', {
            region: 'south',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            width: '100%',
            items: ["->",
                {
                    itemId: "help_ok", text: $WF.NGLang.btnOk || "确定", iconCls: 'icon-Confirm',
                },
                {
                    itemId: "help_close", text: $WF.NGLang.btnCancel || "取消", iconCls: 'icon-Close'
                }
            ]
        });

        Ext.define('dynamicBranchModel', {
            extend: 'Ext.data.Model',
            fields: [
                {
                    name: 'id',
                    type: 'string',
                    mapping: 'id'
                }, {
                    name: 'needDynamic',
                    type: 'bool',
                    mapping: 'needDynamic'
                }, {
                    name: 'sourceId',
                    type: 'string',
                    mapping: 'sourceId'
                }, {
                    name: 'sourceName',
                    type: 'string',
                    mapping: 'sourceName'
                }, {
                    name: 'destinationId', type: 'string',
                    mapping: 'destinationId'
                }, {
                    name: 'destinationName', type: 'string',
                    mapping: 'destinationName'
                }, {
                    name: 'destinationType', type: 'string',
                    mapping: 'destinationType'
                }
            ]
        });

        var branchStore = Ext.create('Ext.data.TreeStore', {
            model: 'dynamicBranchModel',
            root: {
                text: $WF.NGLang.curNodeName || "当前节点",
                sourceName: $WF.NGLang.curNodeName || "当前节点",
                expanded: true,
                destinationType: 1,
                children: me.branchData
            }
            // proxy: {
            //    type: 'ajax',
            //    url: C_ROOT+'@Url.Content("~/WM/Archive/W3BaTreedatabase/GetArchiveTypeTreeNodes")'
            //}
            //root: {
            //    text: "当前节点",
            //    sourceName: "当前节点",
            //    expanded: true,
            //    destinationType: 1,
            //    children: [{
            //        "id": "1",
            //        "needDynamic": false,
            //        "sourceId": "01",
            //        "sourceName": "当前节点",
            //        "destinationId": "02",
            //        "destinationName": "排它网关",
            //        "destinationType": 16,
            //        "expanded": "true",
            //        "children": [
            //          {
            //              "id": "11",
            //              "needDynamic": true,
            //              "sourceId": "02",
            //              "sourceName": "排它网关",
            //              "destinationId": "03",
            //              "destinationName": "任务1",
            //              "destinationType": 1,
            //              "expanded": "true",
            //              "checked": false,
            //              "children": []
            //          },
            //          {
            //              "id": "12",
            //              "needDynamic": true,
            //              "sourceId": "02",
            //              "sourceName": "排它网关",
            //              "destinationId": "04",
            //              "destinationName": "并行网关",
            //              "destinationType": 32,
            //              "expanded": "true",
            //              "checked": false,
            //              "children": [
            //                {
            //                    "id": "121",
            //                    "needDynamic": false,
            //                    "sourceId": "04",
            //                    "sourceName": "并行网关",
            //                    "destinationId": "05",
            //                    "destinationName": "排它网关",
            //                    "destinationType": 16,
            //                    "expanded": "true",
            //                    "children": [
            //                      {
            //                          "id": "1211",
            //                          "needDynamic": true,
            //                          "sourceId": "05",
            //                          "sourceName": "排它网关",
            //                          "destinationId": "06",
            //                          "destinationName": "任务2",
            //                          "destinationType": 1,
            //                          "expanded": "true",
            //                          "checked": false,
            //                          "children": []
            //                      },
            //                      {
            //                          "id": "1212",
            //                          "needDynamic": true,
            //                          "sourceId": "05",
            //                          "sourceName": "排它网关",
            //                          "destinationId": "07",
            //                          "destinationName": "任务4",
            //                          "destinationType": 1,
            //                          "expanded": "true",
            //                          "checked": false,
            //                          "children": []
            //                      }
            //                    ]
            //                },
            //                {
            //                    "id": "122",
            //                    "needDynamic": false,
            //                    "sourceId": "04",
            //                    "sourceName": "并行网关",
            //                    "destinationId": "08",
            //                    "destinationName": "包容网关",
            //                    "destinationType": 64,
            //                    "expanded": "true",
            //                    "children": [
            //                      {
            //                          "id": "1221",
            //                          "needDynamic": true,
            //                          "sourceId": "08",
            //                          "sourceName": "包容网关",
            //                          "destinationId": "09",
            //                          "destinationName": "任务5",
            //                          "destinationType": 1,
            //                          "expanded": "true",
            //                          "checked": false,
            //                          "children": []
            //                      },
            //                      {
            //                          "id": "1222",
            //                          "needDynamic": true,
            //                          "sourceId": "08",
            //                          "sourceName": "包容网关",
            //                          "destinationId": "10",
            //                          "destinationName": "任务6",
            //                          "destinationType": 1,
            //                          "expanded": "true",
            //                          "checked": false,
            //                          "children": []
            //                      }
            //                    ]
            //                }
            //              ]
            //          }
            //        ]
            //    }
            //    ]
            //}
        });

        var branchTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            collapsible: false,
            useArrows: true,
            rootVisible: false,
            store: branchStore,
            multiSelect: true,
            //singleExpand: true,       
            listeners: {
                //设置不能选择的节点
                afterrender: function (tree, opts) {
                    //setNodeCheckBoxDisable(tree,tree.getRootNode().childNodes);
                }
                //load: function (sender, nodes, records) {
                //    Ext.each(records, function (record, index) {
                //        if (record.data.needDynamic == false) {    //  这是我自己后台数据的一个属性，不要被名字误导
                //            console.log(record.data.id);
                //            var node = this.getView().getNode(record);
                //            var d = Ext.fly(node);
                //            var input = Ext.fly(node).down('input');
                //            input.set({ disabled: 'disable' });
                //            d.addCls('x-form-item-default x-form-type-text x-field-default x-item-disabled');
                //        }
                //    }, this);
                //}
            },
            columns: [{
                xtype: 'treecolumn',
                text: $WF.NGLang.nodeName || '名称',
                flex: 2,
                sortable: true,
                dataIndex: 'destinationName'
            }, {
                text: '编号',
                flex: 1,
                dataIndex: 'destinationId',
                hidden: true,
                sortable: true
            }, {
                text: '需要动态选择',
                flex: 0,
                dataIndex: 'needDynamic',
                hidden: true,
                hideable: false
            }, {
                text: '源ID',
                flex: 0,
                dataIndex: 'sourceId',
                hidden: true,
                hideable: false
            }, {
                text: '类型',
                flex: 0,
                dataIndex: 'destinationType',
                hidden: true,
                hideable: false
            }, {
                text: '连接线id',
                flex: 0,
                dataIndex: 'id',
                hidden: true,
                hideable: false
            }]
        });

        me.items = [toolbar, branchTree];

        me.callParent();

        branchTree.on('checkchange', function (node, check) {
            if (check) {
                if (node.parentNode != null) {
                    if (node.parentNode.data.destinationType < 32) {
                        checkBrother(node, false);
                    }
                    checkParent(node.parentNode, check);
                }
            }
            else {
                node.cascadeBy(function (thisnode) {
                    if (thisnode.data.needDynamic) {
                        thisnode.set('checked', false);
                    }
                });
            }
        });

        toolbar.items.get('help_ok').on('click', function () {
            if (isValid(branchTree.getRootNode()) == false)
                return;
            var checkedNode = branchTree.getChecked();
            var callBackData = [];
            for (var i = 0; i < checkedNode.length; i++) {
                var d = {
                    sourceId: checkedNode[i].data.sourceId,
                    transitionId: checkedNode[i].data.id,
                    destActId: checkedNode[i].data.destinationId
                }
                callBackData.push(d);
            }
            if (me.callBack != Ext.emptyFn) {
                me.destroy();
                me.callBack(callBackData);
            }
        });

        toolbar.items.get('help_close').on('click', function () {
            me.destroy();
            me.invokeCancelback();
        });

        function setNodeCheckBoxDisable(tree, nodes) {
            Ext.Array.forEach(nodes, function (item, inddex) {
                if (item.data.needDynamic == false) {
                    item.disabled = true;
                    setNode(tree, item, false);
                }
                if (item.childNodes.length > 0) {
                    setNodeCheckBoxDisable(tree, item.childNodes);
                }
            });
        }

        function setNode(tree, node, value) {
            var checkbox = getCheckbox(tree, node);
            //checkbox.disabled = value;
            checkbox.dom.style.backgroundColor = "#ccc";
            checkbox.dom.style.opacity = "0.5";
            checkbox.dom.style.zIndex = "100";
        };

        function getCheckbox(tree, node) {
            var view = tree.getView();     // 这样就获得了节点
            var n = view.getNode(node);
            var input = Ext.fly(n).down('input'); // checkbox   // 获取节点下的checkbox
            return input;
        };

        function checkParent(nextnode, check) {
            if (nextnode.data.sourceName == ($WF.NGLang.curNodeName || "当前节点"))
                return;
            if (nextnode.data.needDynamic) {
                nextnode.set('checked', check);
            }
            if (check) {
                if (nextnode.parentNode != null) {
                    if (nextnode.parentNode.data.destinationType < 32) {
                        checkBrother(nextnode, false);
                    }
                    checkParent(nextnode.parentNode, true);
                }
            }
        };

        function checkBrother(checknode, check) {
            if (checknode.parentNode != null) {
                var array = checknode.parentNode.childNodes;
                Ext.Array.forEach(array, function (item, inddex, array) {
                    if (item.data.id != checknode.data.id) {
                        item.cascadeBy(function (itemnode) {
                            if (itemnode.data.needDynamic) {
                                itemnode.set('checked', false);
                            }
                        });
                    }
                })
            }
        };

        function isValid(node) {
            var valid = true;
            if (node.childNodes.length < 1) {
                return true;
            }
            var checkedSubNodes = getCheckedSubNodes(node);
            if (checkedSubNodes.length < 1) {
                var msg = $WF.format($WF.NGLang.nodeMustHasChild || '节点【{0}】必需选择下级分支！', node.data.destinationName);
                Ext.create('Ext.ng.MessageBox').Error(msg);
                return false;
            }
            if (node.data.destinationType < 32) {
                if (checkedSubNodes.length > 1) {
                    var msg = $WF.format($WF.NGLang.nodeMustOnlyOneChild || '节点【{0}】不能选择多个分支！', node.data.destinationName);
                    Ext.create('Ext.ng.MessageBox').Error(msg);
                    return false;
                }
            }
            for (var i = 0; i < checkedSubNodes.length; i++) {
                valid = valid && isValid(checkedSubNodes[i]);
            }
            return valid;
        }

        function getCheckedSubNodes(node) {
            var list = [];
            for (var i = 0; i < node.childNodes.length; i++) {
                var subNode = node.childNodes[i];
                if (subNode.data.needDynamic == false || subNode.data.checked) {
                    list.push(subNode)
                }
            }
            return list;
        }
    }

});

Ext.define("Ext.ng.WFDynamicNodeUserWin", {
    extend: 'Ext.ng.wf.baseWindow',
    title: '节点人员指派',
    closable: true,
    resizable: false,
    modal: true,
    width: 800,
    height: 520,
    nodeIndex: -1,
    actNodeId: null,
    activityName: null,
    psnUserData: [],
    gridStore: null,
    border: false,
    callback: null,
    callbackSimplyData: false,
    radioItems: [1, 2], //待选人员、用户-组织
    orgTreeSelctValue: '',
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        if (!Ext.isEmpty(me.activityName)) {
            me.title = me.activityName + ($WF.NGLang.taskHisNode || '节点') + '--' + ($WF.NGLang.dynamicUsers || '人员指派')
        } else {
            me.title = $WF.NGLang.dynamicUsers || me.title;
        }
        me.radioSelectValue = me.radioItems[0];
        me.initParam();
        var pageSize = 12;
        if (me.radioItems.length == 1) {
            pageSize = 14;
        }
        var psnUserDataWarrp = {
            items: me.psnUserData,
            total: me.psnUserData.length
        };
        var memoryProxy = Ext.create("Ext.wf.data.PagingMemoryProxy", {
            reader: {
                type: 'json',
                root: 'items',
                totalProperty: 'total'
            }
        });
        var storeField = [{
            name: 'userId',
            mapping: 'userId',
            type: 'string'
        }, {
            name: 'userNo',
            mapping: 'userNo',
            type: 'string'
        }, {
            name: 'userName',
            mapping: 'userName',
            type: 'string'
        }, {
            name: 'deptName',
            mapping: 'deptName',
            type: 'string'
        }, {
            name: 'orgName',
            mapping: 'orgName',
            type: 'string'
        }];
        me.memoryStore = Ext.create('Ext.data.Store', {
            fields: storeField,
            pageSize: pageSize,
            proxy: memoryProxy,
            data: psnUserDataWarrp
        });
        var store = Ext.create('Ext.ng.JsonStore', {
            autoLoad: false,
            pageSize: pageSize,
            fields: storeField,
            url: C_ROOT + 'WorkFlow3/WorkFlow/GetUserList'
        }),
            pageBar = Ext.create('Ext.toolbar.Paging', {
                store: store,
                displayMsg: '共 {2} 条数据'
            }),
            memoryPagebar = Ext.create('Ext.toolbar.Paging', {
                store: me.memoryStore,
                displayMsg: '共 {2} 条数据'
            }),
            resultStore = Ext.create('Ext.ng.JsonStore', {
                fields: storeField
            });
        if (me.radioItems.length > 1 || me.radioSelectValue == 2) {
            var orgTree = $WorkFlow.createOrgTree();
            var left = {
                //title: "人力资源树",
                autoScroll: false,
                //collapsible: true,
                //split: true,
                weight: 50,
                itemId: 'orgTreePanel',
                region: 'west',
                width: 220,
                minSize: 220,
                maxSize: 220,
                border: true,
                layout: 'border',
                items: [{
                    region: 'north',
                    height: 26,
                    layout: 'border',
                    border: false,
                    items: [{
                        region: 'center',
                        xtype: "textfield",
                        allowBlank: true,
                        fieldLabel: '',
                        emptyText: $WF.NGLang.treeSeachEmptyText || '输入关键字，定位树节点',
                        margin: '2 0 2 2',
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER) {
                                    me.findNodeByFuzzy(orgTree, el.getValue());
                                    el.focus();
                                    return false;
                                } else {
                                    me.nodeIndex = -1;
                                }
                            }
                        }
                    }, {
                        region: 'east',
                        xtype: 'button',
                        text: '',
                        iconCls: 'icon-Location',
                        width: 21,
                        margin: '2 5 2 5',
                        handler: function () {
                            var el = arguments[0].prev();
                            me.findNodeByFuzzy(orgTree, el.getValue());
                            el.focus();
                        }
                    }]
                }, orgTree]
            };
        }
        var gridLeft = Ext.create('Ext.grid.Panel', {
            flex: 5,
            store: me.memoryStore,
            autoScroll: true,
            columnLines: true,
            columns: [{
                header: $WF.NGLang.userNo || '编号',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userNo'
            }, {
                header: $WF.NGLang.userName || '姓名',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userName'
            }, {
                header: $WF.NGLang.userDept || '部门',
                flex: 1,
                itemId: 'deptName',
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'deptName'
            }, {
                header: $WF.NGLang.userOrg || '组织',
                flex: 1,
                itemId: 'orgName',
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'orgName'
            }],
            listeners: {
                'itemdblclick': function (item, record, it, index, e, eOpts) {
                    me.copyData([record], resultStore);
                }
            },
            viewConfig: {
                loadMask: false
            }
        });
        var gridLeft2 = Ext.create('Ext.grid.Panel', {
            flex: 5,
            store: store,
            autoScroll: true,
            columnLines: true,
            columns: [{
                header: $WF.NGLang.userNo || '编号',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userNo'
            }, {
                header: $WF.NGLang.userName || '姓名',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userName'
            }],
            listeners: {
                'itemdblclick': function (item, record, it, index, e, eOpts) {
                    me.copyData([record], resultStore);
                }
            }
        });
        var gridRight = Ext.create('Ext.ng.GridPanel', {
            flex: 5,
            store: resultStore,
            autoScroll: true,
            columnLines: true,
            columns: [{
                header: $WF.NGLang.userNo || '编号',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userNo'
            }, {
                header: $WF.NGLang.userName || '姓名',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'userName'
            }],
            listeners: {
                'itemdblclick': function (item, record, it, index, e, eOpts) {
                    me.removeData([record], resultStore);
                }
            }
        });
        me.items = [{
            region: 'north',
            height: 30,
            layout: 'border',
            border: true,
            style: 'margin-bottom: 4px;',
            items: [{
                xtype: 'radiogroup',
                layout: 'column',
                defaults: {
                    style: 'margin-right: 30px;'
                },
                items: me.radioItems,
                listeners: {
                    change: function (obj, newValue, oldValue, eOpts) {
                        me.rbChange(newValue.rb);
                    }
                }
            }]
        }, {
            region: 'center',
            layout: 'border',
            border: false,
            items: [left, {
                region: 'center',
                layout: 'border',
                border: false,
                items: [{
                    region: 'north',
                    xtype: '',
                    height: 27,
                    border: false,
                    layout: 'border',
                    items: [{
                        xtype: 'ngwfsearchfield',
                        region: 'center',
                        width: 160,
                        fieldLabel: '',
                        itemId: "seachtext",
                        emptyText: $WF.NGLang.userSeachEmptyText || '输入编号/姓名，回车查询',
                        maxHeight: 22,
                        seachFn: function (key) {
                            me.searchData({
                                "seachtext": key
                            });
                        },
                        clearSeachFn: function () {
                            me.searchData({
                                "seachtext": ''
                            });
                        }
                    }]
                }, {
                    region: 'center',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        flex: 5,
                        layout: 'card',
                        itemId: 'leftGridPanel',
                        border: false,
                        items: [gridLeft, gridLeft2]
                    }, {
                        width: 80,
                        layout: 'absolute',
                        border: false,
                        frame: true,
                        items: [{
                            xtype: 'button',
                            name: 'addSelect',
                            text: '&gt;',
                            x: 6,
                            y: 90,
                            width: 60,
                            handler: Ext.bind(function () {
                                var data = me.getActiveLeftGrid().getSelectionModel().getSelection();
                                me.copyData(data, resultStore);
                            })
                        }, {
                            xtype: 'button',
                            name: 'selectAll',
                            text: '&gt;&gt;',
                            x: 6,
                            y: 120,
                            width: 60,
                            handler: Ext.bind(function () {
                                var data = me.getActiveLeftGrid().store.data.items;
                                me.copyData(data, resultStore);
                            })
                        }, {
                            xtype: 'button',
                            name: 'removeSelect',
                            text: '&lt;',
                            x: 6,
                            y: 150,
                            width: 60,
                            handler: Ext.bind(function () {
                                var data = gridRight.getSelectionModel().getSelection();
                                me.removeData(data, resultStore);
                            })
                        }, {
                            xtype: 'button',
                            name: 'removeAll',
                            text: '&lt;&lt;',
                            x: 6,
                            y: 180,
                            width: 60,
                            handler: Ext.bind(function () {
                                me.removeData(null, resultStore, true);
                            })
                        }]
                    }, gridRight]
                }, {
                    region: 'south',
                    layout: 'card',
                    itemId: 'pageBarPanel',
                    border: false,
                    items: [memoryPagebar, pageBar]
                }]
            }]
        }];
        me.buttons = ['->', {
            text: $WF.NGLang.btnOk || '确认',
            iconCls: 'icon-Confirm',
            handler: function () {
                if (me.callback) {
                    var data = me.currData,
                        tmpdata = {
                            "id": me.actNodeId,
                            users: []
                        };
                    if (data.value.length > 0) {
                        if (me.callbackSimplyData) {
                            tmpdata = {};
                            tmpdata[me.actNodeId] = [];
                            resultStore.each(function (record) {
                                tmpdata[me.actNodeId].push(record.data.userId);
                            });
                        } else {
                            resultStore.each(function (record) {
                                tmpdata.users.push(record.data);
                            });
                        }
                    } else {
                        $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.hasNoSelectData || "未选择数据");
                        return;
                    }
                    me.destroy();
                    me.callback(tmpdata);
                }
            }
        }, {
                text: $WF.NGLang.btnCancel || '取消',
                iconCls: 'icon-Close',
                margin: '0 5 8 0',
                handler: function () {
                    me.destroy();
                    me.invokeCancelback();
                }
            }];
        me.callParent();
        if (me.radioItems.length == 1) {
            me.items.items[0].hide();
        }
        if (orgTree) {
            orgTree.on('selectionchange', function (m, selected, eOpts) {
                if (selected.length > 0 && !selected[0].data.root) {
                    me.orgTreeSelctValue = selected[0].data.id;
                    me.down('#seachtext').setValue('');
                    me.searchData({
                        "seachtext": ''
                    });
                }
            });
        }
        me.gridStore = store;
        me.leftGrid = gridLeft;
        me.leftGrid2 = gridLeft2;
        me.rightGrid = gridRight;
        me.tree = orgTree;
        me.pageBarPanel = me.down('#pageBarPanel');
        me.leftGridPanel = me.down('#leftGridPanel');
        me.rbChange(me.radioSelectValue);
    },
    initParam: function () {
        var me = this,
            rItems = [],
            rLabels = ['', $WF.NGLang.pendingUser || '待选人员', $WF.NGLang.allUsers || '用户-组织'];
        //me.width = 720;
        //me.height = 520;
        me.currData = {
            value: [],
            text: []
        };
        //待定
        me.memory = {};
        me.gridConfig = {};

        var tmpArr = me.radioItems;
        for (var i = 0; i < tmpArr.length; i++) {
            rItems.push({
                boxLabel: rLabels[tmpArr[i]],
                name: 'rb',
                inputValue: tmpArr[i],
                checked: i == 0
            });
        }
        me.radioItems = rItems;
    },
    getActiveLeftGrid: function () {
        var me = this;
        if (me.radioSelectValue == 1) {
            return me.leftGrid;
        } else {
            return me.leftGrid2;
        }
    },
    getTreeType: function () {
        var treeType = ['', 'psn', 'hr'];
        return treeType[Number(this.radioSelectValue)];
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") {
            return;
        }
        var me = this,
            index = -1,
            firstFind = me.nodeIndex == -1;
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.Text.indexOf(value) > -1 || node.data.OCode.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        } else {
            if (firstFind) {
                $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', "没有匹配的树节点");
            }
            me.nodeIndex = -1;
        }
    },
    searchData: function (param) {
        var me = this;
        if (me.radioSelectValue == 1) {
            me.memoryStore.clearFilter(false);
            if (!Ext.isEmpty(param.seachtext)) {
                var codeFilter = new Ext.util.Filter({
                    filterFn: function (item) {
                        return item.get('userNo').indexOf(param.seachtext) > -1 || item.get('userName').indexOf(param.seachtext) > -1;
                    }
                });
                me.memoryStore.addFilter([codeFilter]);
            }
            me.memoryStore.load();
        } else {
            param.deptid = me.orgTreeSelctValue;
            me.gridStore.currentPage = 1;
            Ext.apply(me.gridStore.proxy.extraParams, param);
            me.gridStore.load();
        }
    },
    copyData: function (selectData, resultStore) {
        var me = this;
        var dataLen = selectData.length,
            index = resultStore.getCount(),
            tmpArr = me.currData.value,
            tmpData = [];
        for (var i = 0; i < dataLen; i++) {
            var sourceData = selectData[i].data;
            if (Ext.Array.indexOf(tmpArr, sourceData.userId) < 0) {
                me.currData.value.push(sourceData.userId);
                me.currData.text.push(sourceData.userName);
                tmpData.push(sourceData);
            }
        }
        resultStore.insert(index, tmpData);
    },
    removeData: function (data, resultStore, isAll) {
        var me = this;
        if (isAll) {
            resultStore.removeAll();
            me.currData.value = [];
            me.currData.text = [];
        } else {
            resultStore.remove(data);
            for (var i = 0; i < data.length; i++) {
                var posIndex = Ext.Array.indexOf(data[i].data.userId);
                Ext.Array.erase(me.currData.value, posIndex, 1);
                Ext.Array.erase(me.currData.text, posIndex, 1);
            }
        }
    },
    rbChange: function (value) {
        var me = this;
        me.radioSelectValue = value;
        me.down('#seachtext').setValue('');
        var treePanel = me.down('#orgTreePanel');
        if (value == 1) {
            me.leftGridPanel.flex = 6.5;
            me.rightGrid.flex = 3.5;
            if (treePanel) {
                treePanel.hide();
            }
        } else {
            me.leftGridPanel.flex = 5;
            me.rightGrid.flex = 5;
            if (treePanel) {
                treePanel.show();
            }
        }
        me.leftGridPanel.getLayout().setActiveItem(value - 1);
        me.pageBarPanel.getLayout().setActiveItem(value - 1);
        me.searchData({
            "seachtext": ''
        });
    }
});

Ext.define('Ext.ng.WFNodeUserTrigger', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.WFNodeUserTrigger',
    triggerCls: 'x-form-help-trigger',
    userType: [1, 2],
    psnUserData: [],
    actNodeId: null,
    activityName: null,
    callback: null,
    onTriggerClick: function (a, b) {
        var me = this;
        var win = Ext.create('Ext.ng.WFDynamicNodeUserWin', {
            radioItems: me.userType,
            psnUserData: me.psnUserData,
            actNodeId: me.actNodeId,
            activityName: me.activityName,
            callback: function (data) {
                me.callback(data);
            }
        });
        win.show();
    }
});

Ext.define("Ext.ng.WFNodeUserSetingWin", {
    extend: 'Ext.ng.wf.baseWindow',
    title: '节点人员指派',
    closable: true,
    resizable: false,
    modal: true,
    width: 650,
    height: 350,
    gridStore: null,
    //data: [{
    //    "nodeId": "1", "nodeName": "节点1","exists_user":false,
    //    "users": [{ "userId": "003002", "userNo": "111", "userName": "徐成生" }, { "userId": "111", "userNo": "222", "userName": "徐成生2222" }]
    //}],
    border: false,
    callback: null,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        this.title = $WF.NGLang.nodeDynamicUsers || me.title;
        me.gridStore = Ext.create('Ext.data.Store', {
            fields: ['nodeId', 'nodeName', 'users', 'dispVal', 'exists_user']
        });
        me.gridStore.loadData(me.data);
        me.grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            height: 300,
            store: me.gridStore,
            autoScroll: true,
            columnLines: true,
            border: false,
            selModel: { mode: "SIMPLE" },
            plugins: Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                listeners: {
                    beforeedit: function (editor, e, eOpts) {
                        if (e.field == "dispVal") {
                            var dynamicType = [];
                            if (e.record.data.exists_user) {
                                dynamicType.push(1);
                                if (e.record.data.dynamicAny) {
                                    dynamicType.push(2);
                                }
                            } else {
                                dynamicType.push(2);
                            }
                            e.column.setEditor(Ext.create('Ext.ng.WFNodeUserTrigger', {
                                userType: dynamicType,
                                psnUserData: e.record.data.users,
                                actNodeId: e.record.data.nodeId,
                                activityName: e.record.data.nodeName,
                                callback: function (data) {
                                    e.record.data.selectData = data.users;
                                    var tempArr = [], value = '';
                                    Ext.Array.forEach(e.record.data.selectData, function (item, inddex) {
                                        tempArr.push(item.userName);
                                    });
                                    value = tempArr.join(",");
                                    var data = me.grid.getSelectionModel().getSelection();
                                    data[0].set('dispVal', value);
                                }
                            }));
                        }
                    }
                }
            }),
            columns: [{
                header: $WF.NGLang.nodeId || '节点编号',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                hidden: true,
                draggable: false,
                dataIndex: 'nodeId'
            }, {
                header: $WF.NGLang.nodeName || '节点名称',
                flex: 1,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'nodeName'
            }, {
                header: $WF.NGLang.user || '人员',
                flex: 3,
                sortable: false,
                menuDisabled: true,
                draggable: false,
                dataIndex: 'dispVal'
            }]
        });
        me.items = [me.grid];
        me.buttons = ['->', {
            text: $WF.NGLang.btnOk || '确认',
            iconCls: 'icon-Confirm',
            handler: function () {
                if (me.callback) {
                    var isValid = true, tmpdata = {};
                    me.gridStore.each(function (record) {
                        if (record.data.selectData && record.data.selectData.length > 0) {
                            var userArray = [];
                            for (var i = 0; i < record.data.selectData.length; i++) {
                                userArray.push(record.data.selectData[i].userId);
                            }
                            tmpdata[record.data.nodeId] = userArray;
                            //tmpdata.push({ "id": record.data.nodeId, "users": record.data.selectData });
                        }
                        else {
                            isValid = false;
                        }
                    });
                    if (!isValid) {
                        $WorkFlow.msgAlert($WF.NGLang.alertTitle || '提示', $WF.NGLang.notAllNodeSetUser || "不是所有节点都已指派人员");
                        return;
                    }
                    me.destroy();
                    me.callback(tmpdata);
                }
            }
        }, {
                text: $WF.NGLang.btnCancel || '取消',
                margin: '0 5 8 0',
                iconCls: 'icon-Close',
                handler: function () {
                    me.destroy();
                    me.invokeCancelback();
                }
            }];
        me.callParent();
    }
});

Ext.define("Ext.ng.wf.UserSelectWin", {
    extend: 'Ext.window.Window',
    title: '人员选择',
    closable: true,
    resizable: false,
    modal: true,
    width: 650,
    height: 520,
    nodeIndex: -1,
    gridStore: null,
    currData: { value: [], text: [] },
    lastSelectData: [], //上次选中数据
    border: false,
    callback: null,
    callbackSimplyData: false,
    radioItems: [1, 2], //用户-组织、流程人员
    orgTreeSelctValue: '',
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        me.initRES();
        me.title = me.RES.title;
        me.flowUsers = [{
            "userId": "WF$initiator",
            "userNo": "WF$initiator",
            "userName": me.RES.flowUsers.initiator
        }, {
            "userId": "WF$nextappman",
            "userNo": "WF$nextappman",
            "userName": me.RES.flowUsers.nextappman
        }, {
            "userId": "WF$allappman",
            "userNo": "WF$allappman",
            "userName": me.RES.flowUsers.allappman
        }];
        var store = Ext.create('Ext.ng.JsonStore', {
            autoLoad: false,
            pageSize: 15,
            fields: [{
                name: 'userId',
                mapping: 'userId',
                type: 'string'
            }, {
                name: 'userNo',
                mapping: 'userNo',
                type: 'string'
            }, {
                name: 'userName',
                mapping: 'userName',
                type: 'string'
            }],
            url: C_ROOT + 'WorkFlow3/WorkFlow/GetUserList'
        }),
            pageBar = Ext.create('Ext.ng.PagingBar', {
                store: store,
                itemId: 'UserSelectWinPagingBar1',
                displayMsg: '共 {2} 条数据'
            }),
            resultStore = Ext.create('Ext.ng.JsonStore', {
                fields: [{
                    name: 'userId',
                    mapping: 'userId',
                    type: 'string'
                }, {
                    name: 'userNo',
                    mapping: 'userNo',
                    type: 'string'
                }, {
                    name: 'userName',
                    mapping: 'userName',
                    defaultValue: '',
                    type: 'string'
                }]
            }),
            orgTree = $WorkFlow.createOrgTree(),
            left = {
                autoScroll: false,
                itemId: 'orgTreePanel',
                weight: 50,
                region: 'west',
                width: 180,
                minSize: 180,
                maxSize: 180,
                border: true,
                layout: 'border',
                items: [{
                    region: 'north',
                    height: 26,
                    layout: 'border',
                    border: false,
                    items: [{
                        region: 'center',
                        xtype: "textfield",
                        allowBlank: true,
                        fieldLabel: '',
                        emptyText: me.RES.orgTree.seachEmptyText,
                        margin: '2 0 2 2',
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER) {
                                    me.findNodeByFuzzy(orgTree, el.getValue());
                                    el.focus();
                                    return false;
                                }
                                else {
                                    me.nodeIndex = -1;
                                }
                            }
                        }
                    }, {
                        region: 'east',
                        xtype: 'button',
                        text: '',
                        iconCls: 'icon-Location',
                        width: 21,
                        margin: '2 5 2 5',
                        handler: function () {
                            var el = arguments[0].prev();
                            me.findNodeByFuzzy(orgTree, el.getValue());
                            el.focus();
                        }
                    }]
                }, orgTree]
            },
            gridLeft = Ext.create('Ext.ng.GridPanel', {
                columnWidth: .5,
                height: 450,
                store: store,
                autoScroll: true,
                columnLines: true,
                border: false,
                //selModel: { mode: "SIMPLE" },
                columns: [{
                    header: me.RES.gridColumn.userNo,
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'userNo'
                }, {
                    header: me.RES.gridColumn.userName,
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'userName'
                }],
                listeners: {
                    'itemdblclick': function (item, record, it, index, e, eOpts) {
                        me.copyData([record], resultStore);
                    }
                },
                viewConfig: {
                    style: {
                        overflowX: 'hidden !important'
                    }
                },
                bodyStyle: {
                    top: '24px !important'
                }
            }),
            gridRight = Ext.create('Ext.ng.GridPanel', {
                columnWidth: .5,
                store: resultStore,
                height: 450,
                autoScroll: true,
                columnLines: true,
                border: false,
                //selModel: { mode: "SIMPLE" },
                columns: [{
                    header: me.RES.gridColumn.userNo,
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'userNo'
                }, {
                    header: me.RES.gridColumn.userName,
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'userName'
                }],
                listeners: {
                    'itemdblclick': function (item, record, it, index, e, eOpts) {
                        resultStore.remove([record]);
                        Ext.Array.remove(me.currData.value, record.data.cno);
                        Ext.Array.remove(me.currData.text, record.data.cname);
                    }
                },
                viewConfig: {
                    style: {
                        overflowX: 'hidden !important'
                    }
                },
                style: {
                    "margin-right": "-1px !important"
                },
                bodyStyle: {
                    top: '24px !important'
                }
            }),
            btnPanel = {
                height: 450,
                width: 80,
                layout: 'absolute',
                border: false,
                frame: true,
                style: { marginTop: "-4px" },
                items: [{
                    xtype: 'button',
                    name: 'addSelect',
                    text: '&gt;',
                    x: 6,
                    y: 90,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = gridLeft.getSelectionModel().getSelection();
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'selectAll',
                    text: '&gt;&gt;',
                    x: 6,
                    y: 120,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = store.data.items;
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeSelect',
                    text: '&lt;',
                    x: 6,
                    y: 150,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = gridRight.getSelectionModel().getSelection();
                        me.removeData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeAll',
                    text: '&lt;&lt;',
                    x: 6,
                    y: 180,
                    width: 60,
                    handler: Ext.bind(function () {
                        me.removeData(null, resultStore, true);
                    })
                }]
            },
            center = {
                region: 'center',
                border: true,
                layout: 'column',
                items: [gridLeft, btnPanel, gridRight],
                bbar: pageBar
            },
            top = {
                region: 'north',
                //xtype: '',
                itemId: 'seachPanel',
                height: 27,
                border: false,
                layout: 'border',
                items: [{
                    region: 'center',
                    width: 160,
                    fieldLabel: '',
                    itemId: "seachtext",
                    emptyText: me.RES.userSeachEmptyText,
                    xtype: 'textfield',
                    maxHeight: 22,
                    style: {
                        marginRight: '0px !important',
                        marginTop: '1px !important'
                    },
                    enableKeyEvents: true,
                    listeners: {
                        'keydown': function (el, e, eOpts) {
                            if (e.getKey() == e.ENTER) {
                                var key = el.getValue();
                                if (me.radioSelectValue == 1) {
                                    me.searchData({
                                        "seachtext": key,
                                        "deptid": me.orgTreeSelctValue
                                    });
                                }
                                el.focus();
                                return false;
                            }
                        }
                    }
                }],
                style: {
                    backgroundColor: 'transparent !important'
                }
            };
        me.items = [{
            region: 'north',
            height: 30,
            layout: 'border',
            border: true,
            style: 'margin-bottom: 4px;',
            items: [{
                xtype: 'radiogroup',
                layout: 'column',
                defaults: {
                    style: 'margin-right: 30px;'
                },
                items: [{
                    boxLabel: me.RES.userType.allUsers,
                    name: 'rb',
                    inputValue: 1,
                    checked: true
                }, {
                    boxLabel: me.RES.userType.flowUser,
                    name: 'rb',
                    inputValue: 2,
                    checked: false
                }],
                listeners: {
                    change: function (obj, newValue, oldValue, eOpts) {
                        me.rbChange(newValue.rb);
                    }
                }
            }]
        }, {
            region: 'center',
            layout: 'border',
            border: false,
            items: [left, top, center]
        }];
        me.buttons = ['->', {
            text: me.RES.btn.ok,
            iconCls: 'icon-Confirm',
            handler: function () {
                if (me.callback) {
                    var data = me.currData, tmpdata = [];
                    if (data.value.length > 0) {
                        resultStore.each(function (record) {
                            tmpdata.push(record.data);
                        });
                    }
                    else {
                        $WorkFlow.msgAlert(me.RES.MSG.alertTitle, me.RES.MSG.hasNoSelectData);
                        return;
                    }
                    me.destroy();
                    me.callback(tmpdata);
                }
            }
        }, {
                text: me.RES.btn.cancel,
                iconCls: 'icon-Close',
                margin: '0 5 8 0',
                handler: function () {
                    me.destroy();
                }
            }];
        me.callParent();
        me.gridStore = store;
        me.pageBar = pageBar;
        orgTree.on('selectionchange', function (m, selected, eOpts) {
            if (selected.length > 0 && !selected[0].data.root) {
                me.orgTreeSelctValue = selected[0].data.id;
                me.down('#seachtext').setValue('');
                me.searchData({
                    "seachtext": '',
                    "deptid": me.orgTreeSelctValue
                });
            }
        });
        if (!Ext.isEmpty(me.lastSelectData)) {
            resultStore.loadData(me.lastSelectData);
            Ext.Array.forEach(me.lastSelectData, function (i) {
                me.currData.value.push(i.userId);
                me.currData.text.push(i.userName);
            });
        }
        me.rbChange(1);
        me.on("beforeshow", $winBeforeShow);
        me.on("beforeclose", $winBeforeClose);
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = this, index = -1, firstFind = me.nodeIndex == -1;
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.Text.indexOf(value) > -1 || node.data.OCode.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                $WorkFlow.msgAlert(me.RES.MSG.alertTitle, me.RES.MSG.notFindTreeNode);
            }
            me.nodeIndex = -1;
        }
    },
    searchData: function (param) {
        var me = this;
        me.gridStore.currentPage = 1;
        Ext.apply(me.gridStore.proxy.extraParams, param);
        me.gridStore.load();
    },
    copyData: function (selectData, resultStore) {
        var me = this;
        var dataLen = selectData.length,
            index = resultStore.getCount(),
            tmpArr = me.currData.value,
            tmpData = [];
        for (var i = 0; i < dataLen; i++) {
            var sourceData = selectData[i].data;
            if (Ext.Array.indexOf(tmpArr, sourceData.userId) < 0) {
                me.currData.value.push(sourceData.userId);
                me.currData.text.push(sourceData.userName);
                tmpData.push(sourceData);
            }
        }
        resultStore.insert(index, tmpData);
    },
    removeData: function (data, resultStore, isAll) {
        var me = this;
        if (isAll) {
            resultStore.removeAll();
            me.currData.value = [];
            me.currData.text = [];
        }
        else {
            resultStore.remove(data);
            for (var i = 0; i < data.length; i++) {
                var posIndex = Ext.Array.indexOf(me.currData.value, data[i].data.userId);
                Ext.Array.erase(me.currData.value, posIndex, 1);
                Ext.Array.erase(me.currData.text, posIndex, 1);
            }
        }
        me.pageBar.updateInfo();
    },
    rbChange: function (value) {
        var me = this;
        me.radioSelectValue = value;
        me.down('#seachtext').setValue('');
        if (value == 2) {
            me.gridStore.removeAll();
            me.gridStore.loadData(me.flowUsers);
            me.down('#orgTreePanel').hide();
            me.pageBar.hide();
            me.down('#seachPanel').hide();
        }
        else {
            me.gridStore.removeAll();
            me.searchData({
                "seachtext": '',
                "deptid": me.orgTreeSelctValue
            });
            me.down('#orgTreePanel').show();
            me.pageBar.show();
            me.down('#seachPanel').show();
        }
    },
    initRES: function () {
        var me = this;
        me.RES = {};
        me.RES.title = $WF.NGLang.userSelectWinTitle || '人员选择';
        me.RES.userSeachEmptyText = $WF.NGLang.userSeachEmptyText || '输入编号/姓名，回车查询';
        me.RES.flowUsers = {
            'initiator': $WF.NGLang.flowStartUser || '发起人',
            'nextappman': $WF.NGLang.nextAppName || '下级节点办理人',
            'allappman': $WF.NGLang.allFlowUser || '所有已办人员'
        };
        me.RES.orgTree = {
            'seachEmptyText': $WF.NGLang.treeSeachEmptyText || '输入关键字，定位树节点'
        };
        me.RES.gridColumn = {
            'userNo': $WF.NGLang.userNo || '编号',
            'userName': $WF.NGLang.userName || '姓名'
        };
        me.RES.userType = {
            'allUsers': $WF.NGLang.allUsers || '用户-组织',
            'flowUser': $WF.NGLang.flowUser || '流程人员'
        };
        me.RES.btn = {
            'ok': $WF.NGLang.btnOk || '确认',
            'cancel': $WF.NGLang.btnCancel || '取消'
        };
        me.RES.MSG = {
            'alertTitle': $WF.NGLang.alertTitle || '提示',
            'hasNoSelectData': $WF.NGLang.hasNoSelectData || '未选择数据',
            'notFindTreeNode': $WF.NGLang.notFindTreeNode || '没有匹配的树节点'
        };
    }
});

Ext.define('Ext.ng.wf.NoticeUseerTrigger', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.WFNoticeUseerTrigger',
    triggerCls: 'x-form-help-trigger',
    data: [],
    onTriggerClick: function (a, b) {
        var me = this;
        var win = Ext.create('Ext.ng.wf.UserSelectWin', {
            lastSelectData: me.data,
            callback: function (d) {
                var names = [];
                me.data = d;
                Ext.Array.forEach(me.data, function (item, inddex) {
                    names.push(item.userName);
                });
                me.setValue(names.join(","));
            }
        });
        win.show();
    }
});

Ext.define("Ext.ng.WorkFlowStartWin", {
    extend: 'Ext.window.Window',
    title: '流程发起',
    closable: false,
    resizable: false,
    modal: true,
    width: 650,
    height: 480,
    margin: '0 0 0 0',
    padding: '0 0 0 0',
    layout: {
        type: 'border',
        padding: 4
    },
    border: false,
    gridStore: null,
    data: null,
    bizType: null,
    bizPhid: null,
    flowDesc: null,
    selectPdid: null,
    startFlowParam: {},
    callback: null,
    cancelback: null,
    initComponent: function () {
        var me = this;
        me.initRES();
        me.title = me.RES.title;
        me.gridStore = Ext.create('Ext.data.Store', {
            fields: ['id', 'name', 'description']
        });
        me.gridStore.loadData(me.data.pdlist);
        me.grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            height: 280,
            width: '100%',
            store: me.gridStore,
            autoScroll: true,
            columnLines: true,
            needfocusRow: true,
            border: false,
            listeners: {
                'afterrender': function () {
                    if (me.gridStore.getCount() > 0) {
                        me.grid.getView().select(0);
                    }
                }
            },
            columns: [{
                header: me.RES.gridColumn.pdId,
                flex: 1,
                sortable: false,
                hidden: true,
                draggable: false,
                dataIndex: 'id'
            }, {
                header: me.RES.gridColumn.pdName,
                flex: 2,
                sortable: false,
                draggable: false,
                dataIndex: 'name'
            }, {
                xtype: 'actioncolumn',
                width: 50,
                items: [{
                    iconCls: 'icon-View',
                    tooltip: me.RES.btn.view,
                    handler: function (grid, rowIndex, colIndex) {
                        var rec = grid.getStore().getAt(rowIndex);
                        $WorkFlow.showPdDiagram(rec.get('id'));
                    }
                }]
            }]
        });
        me.mstform = Ext.create('Ext.ng.TableLayoutForm', {
            region: 'north',
            frame: true,
            split: false,
            autoHeight: true,
            padding: '0 0 0 0',
            width: '100%',
            margin: '0 -35 0 0',
            autoScroll: true,
            buskey: 'PhId', //对应的业务表主键属性
            otype: 'edit', //操作类型,add||edit||view
            columnsPerRow: 2,
            fieldDefaults: {
                labelWidth: 35,
                anchor: '100%',
                margin: '0 3 0 0',
                msgTarget: 'side'
            },
            fields: [{
                xtype: 'ngTextArea',
                fieldLabel: me.RES.form.flowDesc,
                name: 'flowdesc',
                itemId: 'txtFlowDesc',
                height: 80,
                readOnly: false,
                mustInput: false,
                colspan: 1
            }, {
                xtype: 'ngTextArea',
                fieldLabel: me.RES.form.remark,
                itemId: 'txtRemark',
                height: 80,
                name: 'remark',
                readOnly: false,
                mustInput: false,
                colspan: 1,
                enableKeyEvents: true,
				listeners: {
					'keydown': function (el, e, eOpts) {
						if (e.getKey() == e.ENTER && me.mstform.queryById("txtRemark").getValue()=="") {
							if(me.bizType=="GHProject"){
								Ext.Ajax.request({
									params: { "id": me.bizPhid, "tabtype": "projectmst"},
									async:false,
									url: C_ROOT + 'GXM/XM/ProjectMst/GetProjectMstInfo',
									success: function (response) {
										var resp = Ext.JSON.decode(response.responseText);
										SelectYJK(me,resp.Data.FBudgetDept,resp.Data.FDeclarationUnit);
									}
								});
							}
							if(me.bizType=="GHBudget"){
								Ext.Ajax.request({
									params: { "id": me.bizPhid, "tabtype": "budgetmst"},
									async:false,
									url: C_ROOT + 'GYS/YS/BudgetMst/GetBudgetMstInfo',
									success: function (response) {
										var resp = Ext.JSON.decode(response.responseText);
										SelectYJK(me,resp.Data.FBudgetDept,resp.Data.FDeclarationUnit);
									}
								});
							}
							if(me.bizType=="GHExpense"){
								Ext.Ajax.request({
									params: { "id": me.bizPhid, "tabtype": "expensemst"},
									async:false,
									url: C_ROOT + 'GYS/YS/ExpenseMst/GetExpenseMstInfo',
									success: function (response) {
										var resp = Ext.JSON.decode(response.responseText);
										SelectYJK(me,resp.Data.FBudgetDept,resp.Data.FDeclarationunit);
									}
								});
							}
							
							if(me.bizType=="GHSubject"){
								Ext.Ajax.request({
									params: { "id": me.bizPhid, "tabtype": "ghsubject"},
									async:false,
									url: C_ROOT + 'GYS/YS/GHSubject/GetGHSubjectInfo',
									success: function (response) {
										var resp = Ext.JSON.decode(response.responseText);
										SelectYJK(me,resp.Data.FDeclarationDept,resp.Data.FDeclarationUnit);
									}
								});
							}
						}
					}
				}
            }, {
                xtype: 'panel',
                title: me.RES.form.procDefin,
                margin: '0 4 0 0',
                //name: 'remark',
                readOnly: false,
                mustInput: false,
                items: [me.grid],
                colspan: 2
            }]
        });
        me.items = [me.mstform];
        me.buttons = ['->', {
            text: me.RES.btn.ok,
            itemId: 'okBtn',
            iconCls: 'icon-Confirm',
            handler: function () {
                me.validDataAndStartFlow();
            }
        }, {
                text: me.RES.btn.cancel,
                margin: '0 5 8 0',
                iconCls: 'icon-Close',
                handler: function () {
                    if (me.cancelback && Ext.isFunction(me.cancelback)) {
                        me.cancelback();
                    }
                    me.close();
                    //me.destroy();
                }
            }];
        me.callParent();
        me.mstform.down("#txtFlowDesc").setValue(me.data.flowdesc);
        me.on("beforeshow", $winBeforeShow);
        me.on("beforeclose", $winBeforeClose);
        me.on("show", function () {
            var me = this;
            if (me.data.autostart == true && me.gridStore.getCount() == 1) {
                var pdid = me.gridStore.getAt(0).get("id");
                me.autoStartFlowByOnlyOne(pdid, me.data.flowdesc);
            }
        });
    },
    setYJ: function (text) {
        me = this;
        me.mstform.queryById("txtRemark").setValue(text);
    },
    startFlow: function (step) {
        me = this;
        if (step == 1) {
            //获取节点动态指派数据
            Ext.Msg.show({
                msg: 'loading...',
                wait: true,
                progress: true,
                dosable: true,
                waitConfig: {
                    interval: 200
                },
                icon: Ext.Msg.INFO
            });
            Ext.Ajax.request({
                // async: false,
                url: C_ROOT + 'WorkFlow3/WorkFlow/GetTaskDynamicInfo?pdid=' + me.startFlowParam.pdid + '&biztype=' + me.bizType + '&pks=' + me.bizPhid,
                success: function (response) {
                    Ext.Msg.close();
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.success) {
                        me.dynamicInfo = resp.data;
                        if (!Ext.isEmpty(me.dynamicInfo.hasNextTrans) && me.dynamicInfo.hasNextTrans == false) {
                            $WorkFlow.msgAlert(me.RES.MSG.alertTitle, me.RES.MSG.createFlowErrorByNoChildNode);
                            return;
                        }
                        else {
                            me.startFlow(2);
                        }
                    }
                    else {
                        $WorkFlow.msgAlert(me.RES.MSG.getTaskDynamicError, resp.errorMsg);
                    }
                }
            });
        }
        else if (step == 2) { //判断是否需要动态选择分支
            if (me.dynamicInfo.needDynamicBranch) {
                var win = Ext.create('Ext.ng.WFDynamicBranchWin', {
                    branchData: me.dynamicInfo.nextTrans,
                    callBack: function (data) {
                        me.startFlowParam.dynamicNode = data;
                        me.startFlow(3);
                    }
                });
                win.show();
            }
            else {
                me.startFlow(3);
            }
        }
        else if (step == 3) { //分支选择完成、判断是否需要动态选择人员
            var win, nodeData = [];
            if (me.dynamicInfo.needDynamicUsers) {
                //根据动态分支计算哪些节点是需要动态设置人员的                
                for (var i = 0; i < me.dynamicInfo.dynamicUserNodes.length; i++) {
                    var d = me.dynamicInfo.dynamicUserNodes[i], needDynamic = true;
                    if (!Ext.isEmpty(me.startFlowParam.dynamicNode) && !Ext.isEmpty(d.needDynamicNodes)) {
                        for (var dindex = 0; dindex < d.needDynamicNodes.length; dindex++) {
                            var result = me.startFlowParam.dynamicNode.filter(function (o) { return o.destActId == d.needDynamicNodes[dindex]; });
                            if (result && result.length < 1) {
                                needDynamic = false;
                                break;
                            }
                        }
                    }
                    if (needDynamic) {
                        nodeData.push({ "nodeId": d.id, "nodeName": d.name, "exists_user": d.exists_user, "users": d.users, "dynamicAny": d.dynamicAny, "dispVal": "" });
                    }
                }
            }
            if (nodeData.length == 1) {
                var dynamicType = [];
                if (nodeData[0].exists_user) {
                    dynamicType.push(1);
                    if (nodeData[0].dynamicAny) {
                        dynamicType.push(2);
                    }
                } else {
                    dynamicType.push(2);
                }
                win = Ext.create('Ext.ng.WFDynamicNodeUserWin', {
                    radioItems: dynamicType,
                    psnUserData: nodeData[0].users,
                    actNodeId: nodeData[0].nodeId,
                    activityName: nodeData[0].nodeName,
                    callbackSimplyData: true,
                    callback: function (data) {
                        me.startFlowParam.dynamicUser = data;
                        me.startFlow(4);
                    }
                });
                win.show();
            }
            else if (nodeData.length > 1) {
                win = Ext.create('Ext.ng.WFNodeUserSetingWin', {
                    data: nodeData,
                    callback: function (data) {
                        Ext.apply(me.startFlowParam.dynamicUser, data);
                        me.startFlow(4);
                    }
                });
                win.show();
            }
            else { //不需要选择动态人员
                me.startFlow(4);
            }
        }
        else if (step == 4) {
            //me.hide();
            if (!Ext.isEmpty(me.dynamicInfo.dynamicUserCacheKey)) {
                me.startFlowParam.dynamicUserCacheKey = me.dynamicInfo.dynamicUserCacheKey;
            }
            if (!Ext.isEmpty(me.dynamicInfo.dynamicBranchCacheKey)) {
                me.startFlowParam.dynamicBranchCacheKey = me.dynamicInfo.dynamicBranchCacheKey;
            }
            Ext.Msg.show({
                msg: me.RES.MSG.createFlow + '...',
                wait: true,
                progress: true,
                dosable: true,
                waitConfig: {
                    interval: 200
                },
                icon: Ext.Msg.INFO
            });
            Ext.Ajax.request({
                url: C_ROOT + 'WorkFlow3/WorkFlow/CreateProcInst',
                params: { 'data': JSON.stringify(me.startFlowParam) },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.success) {
                        //var activeName = me.dynamicInfo.dynamicUserNodes[0].activityName; //节点名称
                        //var listId = me.startFlowParam.bizphid;                           //单据phid
                        //var biztypeName = me.startFlowParam.biztype;    //业务类型
                        //Ext.Ajax.request({
                        //    params: { 'ID': listId, 'activeName': activeName, 'biztypeName': biztypeName },
                        //    url: C_ROOT + 'GXM/XM/ProjectMst/SaveNextApprove',
                        //    async: false
                        //});
                        $NG3Refresh();
                        me.close();
                        $WorkFlow.msgAlert(me.RES.MSG.alertTitle, me.RES.MSG.createFlowSuccess);
                        if (me.callback && Ext.isFunction(me.callback)) {
                            me.callback(resp.piid);
                        }
                    }
                    else {
                        me.close();
                        $WorkFlow.msgAlert(me.RES.MSG.createFlowError, resp.errorMsg);
                        if (me.cancelback && Ext.isFunction(me.cancelback)) {
                            me.cancelback();
                        }
                    }
                }
            });
        }
    },
    validDataAndStartFlow: function () {
        var me = this;
        var records = me.grid.getSelectionModel().getSelection();
        if (records.length < 1) {
            $WorkFlow.msgAlert(me.RES.MSG.alertTitle, me.RES.MSG.selectOnePd);
            return false;
        }
        me.startFlowParam.pdid = records[0].data.id;
        me.startFlowParam.flowdesc = me.mstform.down("#txtFlowDesc").getValue();
        me.startFlowParam.remark = me.mstform.down("#txtRemark").getValue();
        me.startFlowParam.biztype = me.bizType;
        me.startFlowParam.bizphid = me.bizPhid;
        me.startFlowParam.dynamicUser = {};
        me.startFlowParam.dynamicNode = [];
        me.startFlow(1);
    },
    autoStartFlowByOnlyOne: function (id, flowDesc) {
        var me = this;
        me.startFlowParam.pdid = id;
        me.startFlowParam.flowdesc = flowDesc;
        me.startFlowParam.biztype = me.bizType;
        me.startFlowParam.bizphid = me.bizPhid;
        me.startFlowParam.dynamicUser = {};
        me.startFlowParam.dynamicNode = [];
        me.startFlow(1);
        //    me.close();
        //    Ext.MessageBox.alert('aa','bbb');
    },
    initRES: function () {
        var me = this;
        me.RES = {};
        me.RES.title = $WF.NGLang.createFlowWinTitile || '流程发起';
        me.RES.form = {
            'flowdesc': $WF.NGLang.flowKeyWord || '流程描述',
            'remark': $WF.NGLang.taskRemark || '办理意见',
            'procDefin': $WF.NGLang.procDefin || '流程定义'
        };
        me.RES.gridColumn = {
            'pdId': $WF.NGLang.pdId || '编号',
            'pdName': $WF.NGLang.pdName || '流程名称'
        };
        me.RES.btn = {
            'ok': $WF.NGLang.btnOk || '确认',
            'cancel': $WF.NGLang.btnCancel || '取消',
            'view': $WF.NGLang.btnView || '查看'
        };
        me.RES.MSG = {
            'alertTitle': $WF.NGLang.alertTitle || '提示',
            'createFlowErrorByNoChildNode': $WF.NGLang.createFlowErrorByNoChildNode || '不存在符合条件的下级分支，不能创建流程实例！',
            'getTaskDynamicError': $WF.NGLang.getTaskDynamicError || '获取节点动态指派数据出错',
            'createFlow': $WF.NGLang.createFlow || '创建流程实例',
            'createFlowSuccess': $WF.NGLang.createFlowSuccess || '流程发起成功',
            'createFlowError': $WF.NGLang.createFlowError || '创建流程出错',
            'selectOnePd': $WF.NGLang.selectOnePd || '请选择流程定义？'
        };
    }
});

Ext.define('Apps.plugin.wfShowConditionalToolTip', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.wfShowConditionalToolTip',
    init: function (grid) {
        grid.on('columnresize', function () {
            grid.getView().refresh();
        });
        grid.on('render', function () {
            var tm = new Ext.util.TextMetrics();
            Ext.Array.each(grid.columns, function (column) {
                if (column.hasCustomRenderer == true) {
                    // This column already has a renderer applied to it
                    // so we will be adding only the tooltip after the
                    // custom renderer has formatted the data.
                    column.renderer = Ext.Function.createSequence(column.renderer, function (a, b, c, d, e, f, g) {
                        // There could be instances where the column actually has no value, such as
                        // row expander, etc.. check for that and only apply the tooltip if the column
                        // has data.
                        if (a) {
                            // Check to see if the entire data string is visible in the cell, if it is then disregard
                            // otherwise add the tooltip
                            if ((g.ownerCt.columns[e].getEl().getWidth() || 10) <= (((tm.getSize(a).width + 15) || 0))) {
                                if (a.indexOf('<img') == -1) {  // not on icons !!!
                                    b.tdAttr += 'data-qtip="' + a + '"';
                                }
                            }
                        }
                    });
                } else {
                    // Here we do the same as above, just w/o the sequence as there is no existing renderer
                    column.renderer = function (a, b, c, d, e, f, g) {
                        if (a) {
                            if ((g.ownerCt.columns[e].getEl().getWidth() || 10) <= (((tm.getSize(a).width + 15) || 0))) {
                                if (a.indexOf('<img') == -1) { // not on icons !!!
                                    b.tdAttr += 'data-qtip="' + a + '"';
                                }
                            }
                            return a;
                        }
                    }
                }
            });
        });
    }
});

Ext.define("Ext.ng.wf.FlowClewWin", {
    extend: 'Ext.window.Window',
    title: '流程催办',
    closable: true,
    resizable: false,
    modal: true,
    width: 500,
    height: 220,
    NGLang: {},
    border: false,
    piid: null,
    bizKey: null,
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        me.RES = {
            'title': me.NGLang.winTitile || '流程催办',
            'msgLabel': me.NGLang.msgLabel || '消息',
            'sendSMS': me.NGLang.sendSMS || '发送手机短信',
            'clewMsgInfo': me.NGLang.clewMsgInfo || '您有一条流程描述为【{0}】的待办工作流任务，请尽快进行办理！',
            'btnSend': me.NGLang.btnSend || '发 送',
            'btnClose': me.NGLang.btnClose || '关 闭',
            'alertTitle': me.NGLang.alertTitle || '提示',
            'msgIsEmpty': me.NGLang.msgIsEmpty || '请输入消息内容！',
            'sendSuccess': me.NGLang.sendSuccess || '发送催办成功！',
            'sendError': me.NGLang.sendError || '发送催办失败！'
        }
        me.title = me.RES.title;
        var baseInfoForm = Ext.create('Ext.ng.TableLayoutForm', {
            columnsPerRow: 2,
            region: 'center',
            margin: '0 -20 0 0',
            fieldDefaults: {
                labelWidth: 40,
                anchor: '100%',
                margin: '3 10 3 0',
                msgTarget: 'side'
            },
            fields: [
                {
                    xtype: 'label',
                    text: me.RES.msgLabel + ':',
                    colspan: 2
                }, {
                    xtype: 'textareafield',
                    name: "msg",
                    itemId: 'msg',
                    height: 100,
                    colspan: 2
                }, {
                    xtype: 'ngCheckbox',
                    name: 'isSms',
                    itemId: 'isSms',
                    labelWidth: 80,
                    inputValue: 1,
                    fieldLabel: me.RES.sendSMS,
                    colspan: 2
                }]
        });
        var data = {};
        if (!Ext.isEmpty(me.bizKey)) {
            data.msg = $WF.format(me.RES.clewMsgInfo, me.bizKey);
            //'您有一条流程描述为【' + me.bizKey + '】的待办工作流任务，请尽快进行办理！';
        }
        baseInfoForm.getForm().setValues(data);
        me.items = [baseInfoForm];
        me.buttons = [
            "->",
            {
                text: me.RES.btnSend,
                handler: function () {
                    var data = baseInfoForm.getForm().getValues();
                    if (Ext.isEmpty(data.msg)) {
                        $WorkFlow.msgAlert(me.RES.alertTitle, me.RES.msgIsEmpty);
                        return;
                    }
                    var isSms = 0;
                    if (!Ext.isEmpty(data.isSms)) {
                        isSms = data.isSms;
                    }
                    Ext.Ajax.request({
                        async: false, //同步请求
                        params: { 'piid': me.piid, issms: isSms, msg: data.msg },
                        url: C_ROOT + 'WorkFlow3/FlowManager/TaskPromptWithMsg',
                        success: function (response) {
                            var result = Ext.JSON.decode(response.responseText);
                            if (result.success) {
                                $WorkFlow.msgAlert(me.RES.alertTitle, me.RES.sendSuccess);
                                me.destroy();
                            }
                            else {
                                $WorkFlow.msgAlert(me.RES.alertTitle, me.RES.sendError);
                            }
                        }
                    });
                }
            },
            {
                text: me.RES.btnClose,
                handler: function () {
                    //me.close();
                    me.destroy();
                }
            }
        ]
        me.callParent();
    }
});

Ext.define("Ext.ng.wf.ProcessDirectoryTree", {
    extend: 'Ext.ng.TreePanel',
    region: 'west',
    split: true,
    autoScroll: true,
    rootVisible: false,
    border: false,
    lines: false,
    width: 160,
    treeFields: [{ name: 'type', type: 'string' },
    { name: 'text', type: 'string' }, { name: 'id', type: 'string' }
    ],
    url: C_ROOT + 'WorkFlow3/FlowManager/GetProcessDirectoryTree?id=1',
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    getAllChildren: function () {
        var me = this;
        var d = [];
        var recores = me.getSelectionModel().getSelection();
        if (!Ext.isEmpty(recores)) {
            if (recores[0].data.type == 'root') {
                return JSON.stringify(d);
            }
            me.getChildrenIds(d, recores[0]);
            if (Ext.isEmpty(d)) {
                d.push("noData");
            }
        }
        return JSON.stringify(d);
    },
    getChildrenIds: function (list, node) {
        var me = this;
        if (node.data.type == 'modeler') {
            list.push(node.data.id);
        }
        if (!Ext.isEmpty(node.childNodes)) {
            for (var i = 0; i < node.childNodes.length; i++) {
                me.getChildrenIds(list, node.childNodes[i]);
            }
        }
    }
});

Ext.define('Ext.ng.form.WFSearchField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.ngwfsearchfield',
    trigger1Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    hasSearch: false,
    seachFn: Ext.emptyFn,
    clearSeachFn: Ext.emptyFn,
    //paramName: 'query',
    initComponent: function () {
        var me = this;
        me.callParent(arguments);
        me.on('specialkey', function (f, e) {
            if (e.getKey() == e.ENTER) {
                me.onTrigger2Click();
            }
        });
    },

    afterRender: function () {
        this.callParent();
        this.triggerCell.item(0).setDisplayed(false);
    },

    onTrigger1Click: function () {
        var me = this;
        if (me.hasSearch) {
            me.setValue('');
            me.clearSeachFn();
            me.hasSearch = false;
            me.triggerCell.item(0).setDisplayed(false);
            me.updateLayout();
        }
    },

    onTrigger2Click: function () {
        var me = this,
            value = me.getValue();
        if (value.length > 0) {
            me.seachFn(value);
            me.hasSearch = true;
            me.triggerCell.item(0).setDisplayed(true);
            me.updateLayout();
        } else if (me.hasSearch) {
            me.onTrigger1Click();
        }
    }
});