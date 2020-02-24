w = window;

var loginpop = false;

Ext.Ajax.timeout = 90000;

//ie 浮动
Ext.useShims = true;

//允许打印编辑
var PrintDesignTime = false;


var Cookie = Ext.create('Ext.state.CookieProvider', {
    path: "/",
    expires: new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)) //30 days
});

//操作类型
var $Otype = { ADD: 'add', EDIT: 'edit', VIEW: 'view' };


function $path(path) {
    //将 path 中 @ 替换成 当前程序路径
    if (path.indexOf("~/") == 0) {
        path = C_ROOT + path.substr(2);
    };

    return path;
}

//获取主界面对象
function $GetWFrame() {
    try {
        var cw = window;
        while (cw != top) {
            if (cw.MainFrame)
                break;
            else
                cw = cw.parent;
        }
    } catch (e) { }

    //跨域访问不允许
    try {
        return cw.MainFrame;
    } catch (e) {
        return undefined;
    }

};

//注册SUPCAN控件
function $RegSupcan(ctrls, showFlg) {
    if (!Ext.isChrome) return;
    var WF = $GetWFrame();
    if (WF) {
        if (!WF.SUPCAN) {
            WF.SUPCAN = {};
        }
        var tabId = WF.Center.getActiveTab().myTabId;
        if (!WF.SUPCAN[tabId]) {
            WF.SUPCAN[tabId] = { 'ctrls': ctrls, 'showFlg': showFlg };
        } else {
            var arr = new Array();
            Ext.each(WF.SUPCAN[tabId]['ctrls'], function (ctrl) {
                if (ctrl && ctrl.func) {
                    arr.push(ctrl);
                }
            });
            WF.SUPCAN[tabId]['ctrls'] = arr;
            Ext.each(ctrls, function (ctrl) {
                WF.SUPCAN[tabId]['ctrls'].push(ctrl);
            });
        }
    }
}

//修改SUPCAN状态
function $SetSupcan(showFlg) {
    if (!Ext.isChrome) return;
    var WF = $GetWFrame();
    if (WF && WF.SUPCAN) {
        if (WF.Center.getActiveTab()) {
            var tabId = WF.Center.getActiveTab().myTabId;
            var regObj = WF.SUPCAN[tabId];
            if (regObj) {
                Ext.each(regObj['ctrls'], function (af) {
                    if (af && af.func) {
                        if (!showFlg) {
                            af.func("grayWindow", "1\r\n backcolor=#FFFFFF;alpha=120");
                        }
                        else {
                            af.func("grayWindow", "");
                        }
                    }
                });
                regObj['showFlg'] = showFlg;
            }
        }
    }
}

//注册金格对象
function $RegKG(kgObj, showFlg) {
    if (!Ext.isChrome) return;
    var WF = $GetWFrame();
    var tabId = WF.Center.getActiveTab().myTabId;
    if (!WF.KG[tabId]) {
        WF.KG[tabId] = { 'kgObj': kgObj, 'showFlg': showFlg };
    }
}

//修改状态
function $SetKG(showFlg) {
    if (!Ext.isChrome) return;
    var WF = $GetWFrame();
    var tabId = WF.Center.getActiveTab().myTabId;
    var regObj = WF.KG[tabId];
    regObj['showFlg'] = showFlg;
}

//win弹窗隐藏金格,SUPCAN
function $winBeforeShow(win, opt) {
    if (Ext.isChrome) {
        var WF = $GetWFrame();

        if (WF) {//web框
            $SetSupcan(false);
            if (!WF.Center) return;//供应商门户无tab页
            if (WF.Center.getActiveTab() == null) return;//直接打开用款计划时返回
            var tabId = WF.Center.getActiveTab().myTabId;
            var regObj = WF.KG[tabId];
            if (regObj) {
                if (regObj['showFlg']) {
                    regObj['kgObj'].HidePlugin(0);
                    win.kgHideObj = regObj['kgObj']; //记录被这个弹窗隐藏的金格控件
                }
            }
        }
        else {//win框

        }
    }
}

//关闭显示金格,SUPCAN
function $winBeforeClose(win, opt) {
    if (Ext.isChrome) {
        if (win.kgHideObj) {
            win.kgHideObj.HidePlugin(1);
        }
        $SetSupcan(true);
    }
}



function $OpenTab(title, url, params, allowedClose) {
    var f = $GetWFrame();
    if (f && f.Center) {
        f.Center.openTab(title, url, params, allowedClose);
    }
    else {

        var encodeTitle = encodeURIComponent(title);
        //debugger;
        //var nw = window.open($path(url), title, 'toolbar=no, menubar=no, scrollbars=no,resizable=no,location=no, status=no');
        if (url.indexOf('?') > 0) {
            url += '&AppTitle=' + encodeTitle;
        }
        else {
            url += '?AppTitle=' + encodeTitle;
        }
        var fullUrl = window.location.protocol + "//" + window.location.host + $path(url);
        //window.external.ShowExternalWebForm(fullUrl);//主界面中打开
        if (window.external.ShowExternalWebForm) {
            window.external.ShowExternalWebForm(fullUrl);//主界面中打开
        } else {
            window.open(fullUrl);
        }
        //        var nw = window.open();
        //        nw.location.href = url;

        //        if (nw) {
        //            nw.lastArguments = params || {};
        //        }


        //tab页集成
        if (window.parent) {
            //alert('opentab');
            var autoId = '';
            if (params) {
                for (var p in params) {
                    var pv = params[p];
                    if (pv != null) {
                        autoId += p + (Ext.isObject(pv) ? Ext.urlEncode(pv) : pv + '').replace(/\W/g, '').substr(0, 30);
                        ;
                    }
                }
            }
            autoId = url.replace(/[\/\.]/g, '') + autoId;

            var obj = {};
            obj.id = autoId;
            obj.msgtype = "opentab";
            obj.title = title;
            obj.url = url;
            var json = JSON.stringify(obj);
            window.parent.postMessage(json, "*");
        }
    }
}

//关闭tab页
function $CloseTab(n) {
    var f = $GetWFrame();
    if (f && f.Center) {

        //debugger;
        //f.Checker.fireEvent('checkin', f.Checker);//触发签入操作

        var tab = f.Center.getComponent(n) || f.Center.getActiveTab();

        f.Center.remove(tab);
    }
    else {
        //w.close();
        window.external.CloseSelectedTabPage();//调用winform关闭tab页面，解决IE是否关闭弹出窗问题
    }
};

function $ShowDialog(title, url, params, allowClose) {


    if (url) {
        var closeable = allowClose === undefined ? true : allowClose;
        var autoId = '';
        if (params) {
            for (var p in params) {
                var pv = params[p];
                if (pv != null) {
                    autoId += p + (Ext.isObject(pv) ? Ext.urlEncode(pv) : pv + '').replace(/\W/g, '').substr(0, 30);
                    ;
                }
            }
        }
        autoId = url.replace(/[\/\.]/g, '') + autoId;

        var frame = document.createElement("IFRAME");
        frame.scrolling = "auto";
        frame.frameBorder = 0;
        frame.src = url;
        frame.height = "100%";
        frame.width = "100%";

        var height = (params && params.Height) || 400;
        var width = (params && params.Width) || 600;

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: title, //'Hello',
            border: false,
            height: height,
            width: width,
            layout: 'border',
            modal: true,
            contentEl: frame
        });

        win.show();
        frame.parentContainer = win;

        var myMask = new Ext.LoadMask(win, { msg: "Please wait..." });
        myMask.show();
        frame.onload = frame.onreadystatechange = function () {
            if (this.readyState && this.readyState != "complete") {
                return;
            }
            else {
                myMask.hide();
            }
        }
    }
};

//#region Ext.Ajax处理

//将 Ajax.request 的 url 中 @ 替换成 当前页的名称
Ext.Ajax.on('beforerequest', function (conn, opts) {
    //opts.url = $url(opts.url);
    //opts.params 的一级子对象转换为字符串
    var f = $GetWFrame();
    if (f) {
        Ext.apply(opts.params, { 'ng3_logid': f.LogID }); //传登录id，验证是否因为使用同一个会话导致已经被踢掉
    }
    if (Ext.isObject(opts.params)) {
        for (var p in opts.params) {
            if (Ext.isArray(opts.params[p]) || Ext.isObject(opts.params[p])) {
                opts.params[p] = Ext.encode(opts.params[p]);
            }
        }
    }
});
//对Ajax返回串进行有效性验证
Ext.Ajax.on('requestcomplete', function (conn, res, opts) {
    res.options = opts;

    res.text = res.responseText
    try {
        $ValidateResponse(res);
    }
    catch (e) {
        Ext.Msg.alert(e.message, res.responseText);
    }
});
//当发生错误时,统一处理
Ext.Ajax.on('requestexception', function (conn, res, opts) {
    if (res.status == 0) {
        Ext.Msg.alert('requestexception', 'url:' + opts.url + '<br/>' + res.statusText);
    }
    else {
        $ValidateResponse(res);
    }
});
//#endregion

function $ValidateResponse(xhr) {
    if (xhr.valid === undefined) {
        var info = $ajaxInfo = $GetAjaxInfo(xhr);
        if (!$ajaxInfo) return true;
        $ajaxData = $ajaxInfo.data;
        if ($ajaxData) {
            if ($ajaxData.NeedLogin) {
                //显示登陆窗口
                var f = parent.MainFrame || w.MainFrame; //parent.WFrame || w.WFrame;
                if (f) {
                    var opts = xhr.options;
                    if (opts.url && opts.url.charAt(0) != '/') {
                        opts.url = $path(C_PATH + opts.url);
                    }
                    if (!f.loginpop) {
                        f.PopLogin(opts);
                        f.loginpop = true;
                    }
                    f.NotCompleteRequest.push(opts);
                    return true;
                }
            }
            if ($ajaxData.AjaxException) {

                //Ext.MessageBox.show({
                //    title: unescape(info.msgtitle),
                //    msg: "<p>" + unescape(info.msg) + "</p>",
                //    buttons: Ext.MessageBox.OK,
                //    animateTarget: 'body'
                //});

                if (info.msgtype === 'Alert') {
                    NGMsg.Info(unescape(info.msg));
                }
                else {
                    NGMsg.Error(unescape(info.msg));
                }
                return true;
            }
        }
        if (info.msgtype && info.msg) {
            var msg = unescape(info.msg);
            var title = unescape(info.msgtitle);
            //var icon = Ext.MessageBox[info.msgicon.toUpperCase()];
            switch (info.msgtype) {
                case 'Alert':
                    alert(msg);
                    break;
                default:
                    alert(info.msgtype + ' 未实现,在common.js');
                    break;
            }
        }
        xhr.valid = info.type == 'Succ';
    }
    return xhr.valid;
}

function $GetAjaxInfo(response) {


    //必须加上trim,IE中的BUG,会在后面加一个空格
    if (response.getResponseHeader && response.getResponseHeader("AjaxInfo")) {

        return Ext.decode(response.getResponseHeader("AjaxInfo"));
    }
    else {
        return '';
    }
}

//处理自定义信息
function $DealIndividualInfo(info, languageInfo) {

    var temp = {};
    if (!Ext.isObject(info)) {//自定义传过来string串
        if (info.length > 0) {
            temp = Ext.decode(info); //自定义对象		
        }
    }
    else {
        temp = info;//无自定义传对象过来
    }

    //多语言处理
    if (languageInfo) {
        //form
        for (var panelId in temp.form) {

            if (panelId) {
                var formPanel = temp.form[panelId];
                var fields = formPanel.fields;
                for (var i = 0; i < fields.length; i++) {
                    var filed = fields[i];

                    if (filed.xtype === 'checkboxgroup' || filed.xtype === 'radiogroup') {
                        var items = filed.items;
                        for (var j = 0; j < items.length; j++) {
                            if (items[j].langKey && items[j].boxLabel) {
                                if (!Ext.isEmpty(languageInfo[items[j].langKey])) {
                                    items[j].boxLabel = languageInfo[items[j].langKey];
                                }
                            }
                        }
                    }

                    if (filed.langKey && filed.fieldLabel) {
                        if (!Ext.isEmpty(languageInfo[filed.langKey])) {
                            filed.fieldLabel = languageInfo[filed.langKey];
                        }
                    }

                    if (filed.langKey && filed.boxLabel) {
                        if (!Ext.isEmpty(languageInfo[filed.langKey])) {
                            filed.boxLabel = languageInfo[filed.langKey];
                        }
                    }

                    //正则表达式
                    if (filed.regex) {
                        filed.regex = Ext.decode(filed.regex);
                    }

                }

                if (formPanel.langKey && formPanel.title) {
                    if (!Ext.isEmpty(languageInfo[formPanel.langKey])) {
                        formPanel.title = languageInfo[formPanel.langKey];
                    }
                }
            }
        }

        //fieldSetForm
        for (var panelId in temp.fieldSetForm) {

            if (panelId) {
                var formPanel = temp.fieldSetForm[panelId];
                var fieldsets = formPanel.fieldSets;
                for (var i = 0; i < fieldsets.length; i++) {
                    var fieldset = fieldsets[i];
                    var fields = fieldset.allfields;
                    for (var k = 0; k < fields.length; k++) {
                        var filed = fields[k];

                        if (filed.xtype === 'checkboxgroup' || filed.xtype === 'radiogroup') {
                            var items = filed.items;
                            for (var j = 0; j < items.length; j++) {
                                if (items[j].langKey && items[j].boxLabel) {
                                    if (!Ext.isEmpty(languageInfo[items[j].langKey])) {
                                        items[j].boxLabel = languageInfo[items[j].langKey];
                                    }
                                }
                            }
                        }

                        if (filed.langKey && filed.fieldLabel) {
                            if (!Ext.isEmpty(languageInfo[filed.langKey])) {
                                filed.fieldLabel = languageInfo[filed.langKey];
                            }
                        }

                        if (filed.langKey && filed.boxLabel) {
                            if (!Ext.isEmpty(languageInfo[filed.langKey])) {
                                filed.boxLabel = languageInfo[filed.langKey];
                            }
                        }

                        //正则表达式
                        if (filed.regex) {
                            filed.regex = Ext.decode(filed.regex);
                        }
                    }

                    if (fieldset.langKey && fieldset.title) {
                        if (!Ext.isEmpty(languageInfo[fieldset.langKey])) {
                            fieldset.title = languageInfo[fieldset.langKey];
                        }
                    }
                }

                if (formPanel.langKey && formPanel.title) {
                    if (!Ext.isEmpty(languageInfo[formPanel.langKey])) {
                        formPanel.title = languageInfo[formPanel.langKey];
                    }
                }
            }
        }

        //tab
        for (var panelId in temp.tabPanel) {
            var tabPanel = temp.tabPanel[panelId];
            var tabs = tabPanel.items;
            for (var i = 0; i < tabs.length; i++) {
                var tab = tabs[i];
                if (tab.langKey && tab.title) {
                    if (!Ext.isEmpty(languageInfo[tab.langKey])) {
                        tab.title = languageInfo[tab.langKey];
                    }

                }
            }
        }
    }

    //grid
    for (var panelId in temp.grid) {
        if (panelId) {

            var gridConfig = temp.grid[panelId];
            for (var i = 0; i < gridConfig.columns.length; i++) {
                var col = gridConfig.columns[i];
                if (languageInfo) {
                    if (col.langKey && col.header) {
                        if (!Ext.isEmpty(languageInfo[col.langKey])) {
                            col.header = languageInfo[col.langKey];
                        }
                    }
                }

                var renderer = col.renderer;
                if (renderer && Ext.isString(renderer)) {
                    col.renderer = Ext.decode(renderer); //字符串转成Function对象                      
                }

                var editor = col.editor;
                if (editor) {
                    var listener = editor.listeners;
                    if (listener) {
                        for (var index in listener) {
                            if (index) {
                                var s = listener[index];
                                if (Ext.isString(s)) {
                                    listener[index] = Ext.decode(s);
                                }
                            }
                        }
                    }
                }
            }


        }
    }

    return temp;
}

//处理列表自定义列信息
function $DealIndividualColumn(info) {
    var individualInfo = Ext.isEmpty(info) ? { models: [], columns: [] } : Ext.decode(info); //自定义列

    for (var i = 0; i < individualInfo.columns.length; i++) {
        var col = individualInfo.columns[i];
        if (col.renderer) {
            col.renderer = Ext.decode(col.renderer);//string 转换成对象
        }
    }

    return individualInfo;
}

//合并自定义列的model
function $MergIndividualModelCol(fields, individualInfo, gridID) {

    if (!individualInfo.grid) return fields;

    var gridInfo = individualInfo.grid[gridID];
    for (var i = 0; i < gridInfo.columns.length; i++) {
        var col = gridInfo.columns[i];
        if (col.dataIndex) {
            //if (col.dataIndex.startsWith('user_')) {//ie不支持

            if (Ext.String.startsWith(col.dataIndex, 'user_', true)) {

                var dataType = 'string';
                var dateFormat = null;
                if (col.editor && col.editor.xtype) {
                    if (col.editor.xtype === 'ngDate') {
                        dataType = 'date';
                    }
                    else if (col.editor.xtype === 'ngDateTime') {
                        dataType = 'date';
                        dateFormat = 'Y-m-d H:i:s';//IE要设置这个
                    }
                    else if (col.editor.xtype === 'ngNumber') {
                        dataType = 'int';
                    }
                }

                if (dateFormat) {
                    fields.push({
                        name: col.dataIndex,
                        type: dataType,
                        dateFormat: dateFormat,
                        mapping: col.dataIndex
                    });
                }
                else {
                    fields.push({
                        name: col.dataIndex,
                        type: dataType,
                        mapping: col.dataIndex
                    });
                }

            }//if
        }
    }

    return fields;
}

//回车替换tab页
Ext.onReady(function () {

    Ext.getBody().on("keydown", function () {

        var e = window.event || arguments[0];
        var keyCode = e.charCode || e.keyCode || e.which;

        var el = e.srcElement || e.target
        switch (keyCode) {
            case Ext.EventObject.ENTER: //回车键
                try {
                    if (el.tagName.toUpperCase() == "TEXTAREA" || el.tagName.toUpperCase() == "DIV") {
                        return true;
                    }
                }
                catch (e) {
                }

                try {
                    if (Ext.isIE) {
                        window.event.keyCode = 9;
                        //return e.keyCode = e.charCode = e.which = 9;
                    }
                    else {
                        e.which.keyCode = 9;
                        //                        var next = nextCtl(el);
                        //                        if (next) {
                        //                            next.focus();
                        //                            e.preventDefault();
                        //                        }
                    }

                }
                catch (e) {
                }
                return true;

                //            case Ext.EventObject.F12: //*帮助
                //                //OpenOnlineHelp();
                //                event.returnValue = false;
                //                return true;
        }
    });

    function nextCtl(ctl) {
        var form = ctl.form;
        for (var i = 0; i < form.elements.length - 1; i++) {
            if (ctl == form.elements[i]) {
                return form.elements[i + 1];
            }
        }
    };
});

//设置工作流ui的状态
function $SetWorkFlowUIState(info) {
    if (Ext.isEmpty(info)) return;

    if (Ext.isString(info)) {
        info = Ext.decode(info);
    }
    for (index in info) {//数组
        if (index) {

            var item = info[index];

            for (field in item) {
                var ids = field.split('#');
                if (ids.length > 1) {
                    var containerID = ids[0];
                    var container = Ext.getCmp(containerID);
                    var ctlOption = item[field];//控制项
                    if (container.xtype === 'ngTableLayoutForm' || container.xtype === 'ngFieldSetForm') {
                        //var ctl = container.query("[name='" + ids[1] + "']")[0]; 
                        var ctl = container.queryById(ids[1]);//itemId
                        if (ctl) {
                            if (ctlOption == '0' && ctl.userSetReadOnly) {
                                ctl.userSetReadOnly(true);//比较晚调用有效果
                                ctl.readOnly = true;//工作流调用得比较早有效果
                            }
                            if (ctlOption == '1' && ctl.userSetReadOnly) {
                                ctl.userSetReadOnly(false);
                                ctl.readOnly = false;//工作流调用得比较早
                            }

                            //不可见
                            if (ctlOption == '2') {
                                ctl.hide();
                            }

                            //必输项
                            if (ctlOption == '3') {
                                ctl.on('afterrender', function (fieldCtl) {
                                    fieldCtl.allowBlank = false;
                                    var label = fieldCtl.el.down('label.x-form-item-label');
                                    label.setStyle({ color: 'OrangeRed' });
                                })
                            }
                        }              
                    }
                    else if (container.xtype === 'ngGridPanel') {
                        var colName = ids[1];
                        if (ctlOption == '0' && container.setReadOnlyCol) {
                            container.setReadOnlyCol(colName, true);//只读                                
                        }
                        if (ctlOption == '1' && container.setReadOnlyCol) {
                            container.setReadOnlyCol(colName, false);//可编辑
                        }
                        //不可见
                        if (ctlOption == '2' && container.hideColumn) {
                            container.hideColumn(colName, true);
                        }
                        //必输项
                        if (ctlOption == '3' && container.setMustInputCol) {
                            container.setMustInputCol(colName, true);
                        }
                    }
                    else {
                        NGMsg.Info('无法识别容器类型：' + container.xtype);
                    }
                }
            }
        }
    }
};

//设置IMP传过来的UI的状态
function $SetIMPUIState(containerID, info) {
    for (index in info) {
        if (index) {
            var container = info[index];
            if (containerID === container.containerID) {
                if (container.type === "form") {
                    var fields = container.items;
                    for (findex in fields) {
                        var fieldItem = fields[findex];
                        //var field = Ext.getCmp(containerID).getComponent(fieldItem.ctlID);
                        //var field = Ext.getCmp(containerID).query("[name='" + fieldItem.ctlID + "']")[0];
                        var ctl = Ext.getCmp(containerID).queryById(fieldItem.ctlID);//itemId
                        if (field) {
                            if (fieldItem.mu === '1') {
                                if (field.userSetMustInput) {
                                    field.userSetMustInput(true);
                                }
                            }
                            if (fieldItem.re === '1') {
                                if (field.userSetReadOnly) {
                                    field.userSetReadOnly(true);
                                }
                            }
                            if (fieldItem.hi === '1') {
                                if (field.hide) {
                                    field.hide(true);
                                }
                            }
                            if (fieldItem.ma === '1') {
                                if (field.userSetMask) {
                                    field.userSetMask(true);
                                }
                            }

                        }
                    }
                } else if (container.type === "grid") {
                    var cols = container.items;
                    for (cindex in cols) {
                        var colItem = cols[cindex];
                        var grid = Ext.getCmp(containerID);
                        if (colItem.mu === '1') {
                            grid.setMustInputCol(colItem.ctlID);
                        }
                        if (colItem.re === '1') {
                            grid.setReadOnlyCol(colItem.ctlID);
                        }
                        if (colItem.hi === '1') {
                            grid.hideColumn(colItem.ctlID);
                        }
                        if (colItem.ma === '1') {
                            //grid.setReadOnlyCol(colItem.ctlID);
                            grid.setMaskCol(colItem.ctlID);
                        }
                    }

                }

            }
        }
    }
}

//设置列权限的UI状态
function $SetFieldRightUIState(containerID, info) {

    if (Ext.isEmpty(info)) return;

    if (Ext.isString(info)) {
        info = Ext.decode(info);
    }

    for (index in info) {
        if (index) {
            var container = info[index];
            if (containerID === container.ContainerId) {
                if (container.ContainerType === "0") {//form
                    var fields = container.Items;
                    for (ctlID in fields) {
                        var flag = fields[ctlID]; //标记位的值                      
                        //var field = Ext.getCmp(containerID).query("[name='" + fieldItem.ctlID + "']")[0];
                        var ctl = Ext.getCmp(containerID).queryById(ctlID);//itemId
                        if (ctl) {
                            if (0 == flag && ctl.userSetReadOnly) {//只读
                                ctl.userSetReadOnly(true);
                            }
                            else if (1 == flag && ctl.userSetReadOnly) {//可编辑
                                ctl.userSetReadOnly(false);
                            }
                            else if (2 == flag && ctl.userSetMask) {//掩码                                
                                ctl.userSetMask(true);
                            }
                        }
                    }
                } else if (container.ContainerType === "1") {//grid
                    var cols = container.Items;
                    for (ctlID in cols) {
                        var flag = cols[ctlID];
                        var grid = Ext.getCmp(containerID);
                        if (0 == flag) {//只读
                            grid.setReadOnlyCol(ctlID, true);
                        }
                        else if (1 == flag) {//可编辑
                            grid.setReadOnlyCol(ctlID, false);
                        }
                        else if (2 == flag) {//掩码
                            grid.setMaskCol(ctlID);
                        }

                    }

                }

            }
        }
    }
}


//送审
function CreateAppFlow(bizid, pks, flowdesc) {
    var parm = "0@#" + bizid + "@#" + pks + "@#SourceFromType=WebFrom;SendDescription=" + flowdesc;
    window.external.ShowManagerWithCallback(pks, "WorkFlowStartManager", 1, parm);
}
//流程历史
function ShowAppFlowHis(bizid, pks) {
    var parm = "op_type=" + bizid + "@@**SourceFromType=WebFrom@@**primarystr=" + pks;
    window.external.ShowManagerWithCallback(pks, "FlowHistoryViewManager", 1, parm);
}

//#region 打印方法
//打开打印页面
function $OpenPrintForm(title, url, params, printObj) {
    function GetDateString(d1) {
        var y = d1.getFullYear().toString();
        var m = d1.getMonth() < 10 ? '0' + d1.getMonth() : d1.getMonth().toString();
        var d = d1.getDate() ? '0' + d1.getDate() : d1.getDate().toString();
        var h = d1.getHours() ? '0' + d1.getHours() : d1.getHours().toString();
        var min = d1.getMinutes() ? '0' + d1.getMinutes() : d1.getMinutes().toString();
        var s = d1.getSeconds() ? '0' + d1.getSeconds() : d1.getSeconds().toString();
        return y + m + d + h + min + s;
    }
    var printId = printObj.printid;
    printId = printId ? printId : "";
    var pageid = printObj.pageid || '';
    var filename = pageid + GetDateString(new Date());
    var urlParam = "?pageid=" + printObj.pageid + "&filename=" + filename;
    if (printId && printId != "") {
        var typeFile = printObj.typefile, moudleType = printObj.moudletype;
        var mTitle = printObj.title;
        var previeweditflg = printObj.previeweditflg;
        var showpreview = printObj.showpreview;
        urlParam += "&printId=" + printId;
        urlParam += "&typeFile=" + typeFile;
        urlParam += "&moudleType=" + moudleType;
        urlParam += "&mTitle=" + mTitle;
        urlParam += "&previeweditflg=" + previeweditflg;
        urlParam += "&showpreview=" + showpreview;
    }
    $CreatePrintInfo(printObj, filename);
    $OpenTab(title, url + urlParam, params);
}

//生成打印模板及数据文件
function $CreatePrintInfo(printObj, filename) {
    var fds = new Object(), clss = new Object(), jsonD = new Object(), parms = new Object();
    var mTitle = printObj.title, dTitle = printObj.titles;
    var mForms = printObj.forms, myGrids = printObj.grids;
    var pageid = printObj.pageid, typeFile = printObj.typefile, moudleType = printObj.moudletype;//模板打印相关
    var printId = printObj.printid; printId = printId ? printId : "";
    var gridprintmode = printObj.gridprintmode;//连续打印相关
    var gridprintstore = new Object();

    function GetAllFormFields() { //获取所有Form的表单字段
        var _mFields = [];
        Ext.Array.each(mForms, function (f) {
            mform = f.fields;
            if (!mform) {  //如果不是Ext.ng.TableLayoutForm
                GetFormField(f.items, _mFields);
            }
            else {
                _mFields = _mFields.concat(mform);
            }
        });
        return _mFields;
    }
    function GetFormField(items, fields) { //递归查找表单字段
        if (items.items) {
            GetFormField(items.items, fields);
        }
        else if (items.length > 0) {
            Ext.Array.each(items, function (item) {
                GetFormField(item, fields);
            });
        }
        else if (items.fieldLabel || items.boxLabel) {
            fields.push(items);
        }
    }
    function GetAllFormFieldItems() { //获取表单字段的数据信息
        var _mItems = [];
        Ext.Array.each(mForms, function (f) {
            _mItems = _mItems.concat(f.getForm().getFields().items);
        });
        return _mItems;
    }
    function GetImgUrl(imgUrl) {
        if (imgUrl.indexOf("..") < 0) {
            return imgUrl;
        }
        else {
            var port = window.location.port;
            port = port.length > 0 ? (":" + port) : "";
            return window.location.protocol + "//" + window.location.host + port + window.C_ROOT + imgUrl.substring(3);
        }
    }

    var oW = window.document.documentElement.offsetWidth - 45;

    var hasForm = mForms ? true : false;
    parms["hasform"] = hasForm;
    parms["mtitle"] = mTitle || '';
    var hasGrid = myGrids && myGrids.length > 0 ? true : false;
    parms["hasgrid"] = hasGrid;
    //parms["pageName"] = pageName;   
    parms["dtitle"] = dTitle ? dTitle.join("$jkd$") : '';
    parms["width"] = oW;
    if (hasForm) {
        var mForm = mForms[0];
        var mform = GetAllFormFields();
        var cols = mForm.columnsPerRow ? (mForm.columnsPerRow > 5 ? 4 : mForm.columnsPerRow) : 4;
        var totalColumns = 0;
        var mF = (function () {
            var newform = [];
            Ext.Array.each(mform, function (f) {
                var fd = new Object();
                var flabel = "";
                if (f.xtype == "ngFormPanel") {
                    for (var pf = 0; pf < f.items.length; pf++) {
                        fd = new Object();
                        flabel = "";
                        var nf = f.items[pf];
                        if (nf.xtype != "hiddenfield" && nf.xtype != "image" && !nf.hidden) {
                            if (nf.xtype == "container") {
                                if (nf.items && nf.items.length > 0) {
                                    flabel = nf.items[0].fieldLabel;
                                    fd["name"] = nf.items[0].name;
                                }
                            }
                            else {
                                flabel = nf.fieldLabel || nf.boxLabel;
                                fd["name"] = nf.name;
                            }
                            if (flabel) {
                                fd["label"] = flabel.lastIndexOf(":") == flabel.length - 1 ? flabel.substring(0, flabel.length - 1) : flabel;
                                if (nf.colspan) {
                                    totalColumns += nf.colspan;
                                    fd["colspan"] = nf.colspan;
                                }
                                else {
                                    totalColumns++;
                                    fd["colspan"] = 1;
                                }
                                newform.push(fd);
                            }
                        }
                    }

                }
                else if (f.xtype != "hiddenfield" && f.xtype != "image" && !f.hidden) {
                    if (f.xtype == "container") {
                        if (f.items && f.items.length > 0) {
                            flabel = f.items[0].fieldLabel;
                            fd["name"] = f.items[0].name;
                        }
                    }
                    else {
                        flabel = f.fieldLabel || f.boxLabel;
                        fd["name"] = f.name;
                    }
                    if (flabel) {
                        fd["label"] = flabel.lastIndexOf(":") == flabel.length - 1 ? flabel.substring(0, flabel.length - 1) : flabel;
                        if (f.colspan) {
                            totalColumns += f.colspan;
                            fd["colspan"] = f.colspan;
                        }
                        else {
                            totalColumns++;
                            fd["colspan"] = 1;
                        }
                        newform.push(fd);
                    }
                }
            });
            return JSON.stringify(newform);
        })();
        if (mForm.imgObj && mForm.imgObj.length > 0) {
            parms["hasImg"] = true;
            var imgWidth = 0;
            parms["imgObj"] = (function () {
                var imgObj = [];
                Ext.Array.each(mForm.imgObj, function (img) {
                    var sizeW = typeof (img.getSize) === "function" ? img.getSize().width : 160;  //img对象可能是组装的对象
                    sizeW = sizeW < 2 ? 160 : sizeW;  //firefox 取不到宽度 纠结
                    var tmpImg = new Object();
                    tmpImg["width"] = sizeW;
                    imgWidth += sizeW;
                    tmpImg["src"] = img.src;
                    tmpImg["name"] = img.name;
                    tmpImg["title"] = img.title;
                    jsonD[img.name] = GetImgUrl(img.src);
                    imgObj.push(tmpImg);
                });
                return imgObj;
            })();
            if (imgWidth > oW / 2) {
                imgWidth = oW / 2;
            }
            parms["imgWidth"] = imgWidth < 2 ? 40 : imgWidth;
        }
        var rows = Math.ceil(totalColumns / cols);
        parms["mform"] = mF;
        parms["rows"] = rows;
        parms["cols"] = cols;

        var mFields = GetAllFormFieldItems();
        var idUsed = ",";
        Ext.Array.each(mFields, function (f) {
            var mfid = f.id;
            if (f.itemId) {
                mfid = f.itemId
            }
            var fieldname = f.name;
            var classname = f.alternateClassName;
            if (idUsed.indexOf("," + mfid + ",") < 0) {
                idUsed += mfid + ",";
                if (f.xtype === "checkboxgroup") {
                    var cbValue = [];
                    Ext.Array.each(f.items.items, function (cb) {
                        if (cb.checked) {
                            cbValue.push(cb.boxLabel);
                            //idUsed += mfid + ",";
                        }
                    });
                    jsonD[fieldname] = cbValue.length > 0 ? cbValue.join(",") : "";
                }
                else if (classname === 'Ext.form.field.Checkbox' || classname === 'Ext.form.Checkbox') {
                    jsonD[fieldname] = f.checked;
                }
                else if (f.inputType === "radio") {
                    if (!jsonD[fieldname] || jsonD[fieldname] == "") {
                        jsonD[fieldname] = f.checked ? f.boxLabel : "";
                    }
                }
                else {
                    jsonD[fieldname] = f.rawValue ? f.rawValue : f.value;
                }
            }
        });
        parms["buskey"] = mForm.buskey;
        var keyObj = mForm.getForm().findField(mForm.buskey);
        jsonD[mForm.buskey] = keyObj ? keyObj.value : "";
    }
    else if (printObj.buskey) {
        var buskey = printObj.buskey;
        jsonD[buskey.id] = buskey.value;
        parms["buskey"] = buskey.id;
    }
    if (hasGrid) {
        jsonD["Grids"] = {};
        var gridCls = "gridcls_";
        var MaxCols = 0;;
        //#region 构造列信息
        for (var i = 0; i < myGrids.length; i++) {
            var cols = 0;
            gridCls = "gridcls_" + i;
            var mygrid = myGrids[i];
            var gridIdCls = "gridcls_";
            if (mygrid.id) {
                gridIdCls = "gridcls_" + mygrid.id;
            }
            clss[i] = new Object();
            fds[i] = [];
            var mycls = (function () {
                var newdata = [];
                function getGridDataColumns(gridCols) {
                    Ext.Array.each(gridCols, function (cl) {
                        if (cl.items && cl.items.length > 0) {
                            getGridDataColumns(cl.items.items || cl.items);
                        }
                            //if (cl.columns) {//多层表头处理
                            //    getGridDataColumns(cl.columns);
                            //}
                        else {
                            if (cl.dataIndex != "" && !(cl.hidden) && cl.xtype != "rownumberer") {
                                cols++;
                                var cls = new Object();
                                cls["dataIndex"] = cl.dataIndex;
                                cls["text"] = cl.text;
                                newdata.push(cls);
                                fds[i].push(cl.dataIndex);
                                if (cl.picColumn) { //图片列
                                    cls["picflg"] = 1;
                                    clss[i][cl.dataIndex] = "picColumn";
                                    parms[gridCls + "Height"] = 80;
                                }
                                else if (cl.xtype == "ngcheckcolumn") {
                                    cls["picflg"] = 0;
                                    clss[i][cl.dataIndex] = "checkColumn";
                                }
                                else {
                                    cls["picflg"] = 0;
                                    if (typeof (cl.renderer) === "function") {
                                        clss[i][cl.dataIndex] = cl;
                                    }
                                }
                                if (cl.printRenderer) {
                                    clss[i][cl.dataIndex] = cl;
                                }
                            }
                        }
                    });
                }
                getGridDataColumns(mygrid.columns);
                return JSON.stringify(newdata)
            })();
            parms[gridCls] = mycls;
            if (cols > MaxCols) {
                MaxCols = cols;
            }
            var all = mygrid.store.getRange();
            var data = [];
            var dataIndex = 0;
            Ext.Array.each(all, function (record) {
                var tmpd = record.data;
                var tmp = new Object();
                Ext.Array.each(fds[i], function (f) { //代码转名称
                    try {
                        if (clss[i][f] && clss[i][f].printRenderer) {
                            tmp[f] = clss[i][f].printRenderer(tmpd[f], this);
                        }
                        else if (typeof (clss[i][f]) === "undefined") {
                            tmp[f] = tmpd[f];
                        }
                        else if (clss[i][f] === "checkColumn") { //
                            if (printId == "") {
                                if (tmpd[f] == "1") {
                                    tmp[f] = "√";
                                }
                            }
                            else {
                                tmp[f] = tmpd[f];
                            }
                        }
                        else if (clss[i][f] === "picColumn") { //图片列
                            tmp[f] = GetImgUrl(tmpd[f]);
                        }
                        else {
                            var colindex = clss[i][f].getIndex();// mygrid.getView().getNodes()[0].cells[3];
                            tmp[f] = clss[i][f].renderer(tmpd[f], this, record, dataIndex, colindex, mygrid.store);
                            //tmp[f] = clss[i][f].renderer(tmpd[f], this);
                        }
                    }
                    catch (e) {
                        //代码转名称出错，使用原始值
                        tmp[f] = tmpd[f];
                    }
                });
                data.push(tmp);
                dataIndex++;
            });
            jsonD["Grids"][gridCls] = data;
            if (gridIdCls != "gridcls_" && !jsonD["Grids"][gridIdCls]) {
                jsonD["Grids"][gridIdCls] = data;
            }
        }
        //#endregion
        parms["gridwidth"] = parseInt(oW / MaxCols);
        parms["maxcols"] = MaxCols;
    }
    if (gridprintmode) {
        gridprintstore["extraParams"] = myGrids[0].store.proxy.extraParams;
        gridprintstore["url"] = myGrids[0].store.proxy.url;
        gridprintstore["currentPage"] = myGrids[0].store.currentPage;
        gridprintstore["pageSize"] = myGrids[0].store.pageSize;
        gridprintstore["pageCount"] = myGrids[0].initialConfig.bbar.getPageData().pageCount;
        gridprintstore["gridprintmode"] = gridprintmode;
        parms["gridprintmode"] = gridprintmode;
        parms["gridprintstore"] = JSON.stringify(gridprintstore);
    }//连续打印
    if (printObj.formdata) {
        if (typeof (printObj.formdata) == "object") {
            jsonD = printObj.formdata;
        }
        else {
            jsonD = Ext.JSON.encode(printObj.formdata);
        }
    }
    if (printObj.griddatas) {
        jsonD["Grids"] = {};
        gridCls = "gridcls_";
        for (var gid in printObj.griddatas) {
            gridCls = "gridcls_" + gid;
            if (typeof (printObj.griddatas[gid]) == "object") {
                jsonD["Grids"][gridCls] = printObj.griddatas[gid];
            }
            else {
                jsonD["Grids"][gridCls] = Ext.JSON.encode(printObj.griddatas[gid]);
            }
        }
    }
    parms["data"] = JSON.stringify(jsonD);
    parms["filename"] = filename;
    parms["printid"] = printId;
    Ext.Ajax.request({
        params: parms,
        url: C_ROOT + 'SUP/Print/CreatePrintInfo',
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            if (resp.status === "ok") {
            } else {
            }
        }
    });
}

//通过Grid打印列表(直接打印当前页)
function $Print(pageid, grid, title) {
    if (!grid) {
        Ext.Msg.alert("Error", "Cannot Get Grid Object!");
        return false;
    }
    else if (grid.store.getRange().length < 1) {
        Ext.Msg.alert("提示", "没有数据!");
        return false;
    }
    var printObj = { pageid: pageid, grids: [grid], title: title };
    $OpenPrintForm(title, C_ROOT + 'SUP/Print/Index', "Print", printObj);
}

//通过Grid打印列表(连续打印所有页)
function $PrintEx(pageid, grid, title, gridprintmode) {
    if (!grid) {
        Ext.Msg.alert("Error", "Cannot Get Grid Object!");
        return false;
    }
    else if (grid.store.getRange().length < 1) {
        Ext.Msg.alert("提示", "没有数据!");
        return false;
    }
    if (grid.query("ngPagingBar").length == 1) {
        var printObj = { pageid: pageid, grids: [grid], title: title, gridprintmode: gridprintmode || '0' };
        $OpenPrintForm(title, C_ROOT + 'SUP/Print/PrintEx', "PrintEx", printObj);
    }
    else {
        $Print(pageid, grid, title)
    }
}

//打印多个grid
function $PrintGrids(pageid, buskey, grids, titles) {   //buskey = { "id": "", "value": "" };
    if (!grids || grids.length == 0) {
        Ext.Msg.alert("Error", "Cannot Get Grids!");
        return false;
    }
    if (!buskey) {
        Ext.Msg.alert("Error", "Cannot Get buskey!");
        return false;
    }
    var printObj = { page: pageid, grids: grids, titles: titles, title: titles[0], forms: null, buskey: buskey, DesignTime: PrintDesignTime };
    $OpenPrintForm(titles[0], C_ROOT + 'SUP/Print/PrintDetail', "PrintDetail", printObj);
}

//打印明细页面Form单据(或者grid，可以多个grid)
function $PrintForm(pageid, form, grids, titles) {
    if (!form) {
        Ext.Msg.alert("Error", "Cannot Get Form Object!");
        return false;
    }
    if (!form.length) {
        form = [form];
    }
    if (!form[0].buskey) {
        Ext.Msg.alert("Error", "Cannot Get buskey!");
        return false;
    }
    $PrintDetail(pageid, form, grids, titles, "", "", "");
}

//此方法不建议直接调用
function $PrintDetail(pageid, form, grids, titles, printid, typefile, moudletype) {
    if (form && !form.length) {
        form = [form];
    }
    var printObj = { pageid: pageid, grids: grids, titles: titles, title: titles[0], forms: form, printid: printid, typefile: typefile, moudletype: moudletype, DesignTime: PrintDesignTime };
    $OpenPrintForm(titles[0], C_ROOT + 'SUP/Print/PrintDetail', "PrintDetail", printObj);
}



//套打方法调用
function $PrintHelp(pageid, typefile, form, grids, titles) {
    $OpenPrintTemplateSelect(typefile, function (printid, moudletype, previeweditflg, showpreview) {
        if (form && !form.length) {
            form = [form];
        }
        var printObj = { pageid: pageid, grids: grids, titles: titles, title: titles[0], forms: form, printid: printid, typefile: typefile, moudletype: moudletype, previeweditflg: previeweditflg, showpreview: showpreview, DesignTime: PrintDesignTime };
        $OpenPrintForm(titles[0], C_ROOT + 'SUP/Print/PrintDetail', "PrintDetail", printObj);
    });
}

////套打方法调用
//function $PrintHelp(pageid, typefile, form, grids, titles, appflowbiz, appflowpk) {
//    $OpenPrintTemplateSelect(typefile, function (printid, moudletype, previeweditflg, showpreview) {
//        if (form && !form.length) {
//            form = [form];
//        }
//        var printObj = { pageid: pageid, grids: grids, titles: titles, title: titles[0], forms: form, printid: printid, typefile: typefile, moudletype: moudletype, previeweditflg: previeweditflg, showpreview: showpreview, DesignTime: PrintDesignTime, appflowbiz: appflowbiz, appflowpk: appflowpk };
//        $OpenPrintForm(titles[0], C_ROOT + 'SUP/Print/PrintDetail', "PrintDetail", printObj);
//    });
//}

//套打方法调用直接传递数据
function $PrintHelpData(pageid, typefile, formdata, griddatas, titles) {
    $OpenPrintTemplateSelect(typefile, function (printid, moudletype, previeweditflg, showpreview) {
        var printObj = { pageid: pageid, titles: titles, title: titles[0], formdata: formdata, griddatas: griddatas, printid: printid, typefile: typefile, moudletype: moudletype, previeweditflg: previeweditflg, showpreview: showpreview, DesignTime: PrintDesignTime };
        $OpenPrintForm(titles[0], C_ROOT + 'SUP/Print/PrintDetail', "PrintDetail", printObj);
    });
}
function $OpenPrintTemplateSelect(typefile, callback) {
    var printWindowHelp;
    var tmpStore;
    Ext.Ajax.request({
        url: C_ROOT + 'SUP/Print/GetFmtTemplateFromDb?typefile=' + typefile,
        success: function (res, opts) {
            var data = Ext.JSON.decode(res.responseText);
            if (data.Record && data.Record.length == 1 && !PrintDesignTime) {
                var printId = data.Record[0]['printid'];    //模板编号
                var moudleType = data.Record[0]['def_int2'];  //模板类型
                var def_int1 = data.Record[0]['def_int1'];  //直接打印
                var previeweditflg = data.Record[0]['previeweditflg'];  //预览可编辑
                if (callback) callback(printId, moudleType, previeweditflg || "0", def_int1 || "1");
            }
            else {
                ShowPrintHelp(typefile, callback);
            }
        }
    });
    function ShowPrintHelp(typefile, callback) {
        if (!printWindowHelp) {
            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [{
                    id: "help_print",
                    text: "打印",
                    iconCls: 'icon-Print',
                    handler: function () {
                        var data = grid.getSelectionModel().getSelection();
                        if (data.length > 0) {
                            var printId = data[0].get('printid');    //模板编号
                            var moudleType = data[0].get('def_int2');  //模板类型
                            var def_int1 = data[0].get('def_int1');  //直接打印
                            var previeweditflg = data[0].get('previeweditflg');  //预览可编辑
                            printWindowHelp.hide();
                            printWindowHelp.destroy();
                            if (callback) callback(printId, moudleType, previeweditflg || "0", def_int1 || "1");
                            //$PrintDetail(pageid, form, grids, titles, printId, typefile, moudleType);
                        }
                        else {
                            Ext.MessageBox.alert("提示", "请先选择打印模板.");
                        }
                    }
                }, {
                    id: "help_printex",
                    text: PrintDesignTime ? "模板设计" : "直接打印",
                    iconCls: 'icon-Print',
                    hidden: !PrintDesignTime,
                    handler: function () {
                        printWindowHelp.hide();
                        printWindowHelp.destroy();
                        $PrintDetail(pageid, form, grids, titles, "", typefile, "");
                    }
                }, {
                    id: "help_delete",
                    text: "删除模板",
                    hidden: !PrintDesignTime,
                    iconCls: 'icon-delete',
                    handler: function () {
                        var data = grid.getSelectionModel().getSelection();
                        if (data.length > 0) {
                            var moudleType = data[0].get('def_int2');  //模板类型
                            if (moudleType == "2") {
                                var printId = data[0].get('printid');    //模板编号
                                Ext.Msg.confirm("提示信息", "你确认删除 " + grid.getSelectionModel().getSelection()[0].get("billname") + " 模板",
                                function (btn) {
                                    if (btn == "yes") {
                                        Ext.Ajax.request({
                                            params: { printid: printId },
                                            url: C_ROOT + "SUP/Print/DeleteModule",
                                            success: function (response) {
                                                var resp = Ext.JSON.decode(response.responseText);
                                                if (resp.status === "ok") {
                                                    //                                                Ext.MessageBox.alert("提示", resp.msg);
                                                    if (resp.count > 0) {
                                                        tmpStore.cachePageData = false;
                                                        tmpStore.load();
                                                        tmpStore.cachePageData = true;
                                                    }
                                                } else {
                                                    Ext.MessageBox.alert("提示", resp.msg);
                                                }
                                            }
                                        });
                                    }
                                });
                            }
                            else {
                                Ext.MessageBox.alert("提示", "只能删除用户自定义的模板数据.");
                            }
                        }
                        else {
                            Ext.MessageBox.alert("提示", "请先选择要删除的模板.");
                        }
                    }
                }, "->", {
                    id: "help_close",
                    text: "关闭",
                    iconCls: 'icon-Close',
                    handler: function () {
                        printWindowHelp.hide();
                        printWindowHelp.destroy();
                    }
                }]
            });
            Ext.define('TemplateModel', {  //定义模型
                extend: 'Ext.data.Model',
                fields: [{
                    name: 'printid',
                    type: 'string',
                    mapping: 'printid'
                }, {
                    name: 'typefile',
                    type: 'string',
                    mapping: 'typefile'
                }, {
                    name: 'billname',
                    type: 'string',
                    mapping: 'billname'
                }, {
                    name: 'moduleno',
                    type: 'string',
                    mapping: 'moduleno'
                }, {
                    name: 'filename',
                    mapping: 'filename',
                    type: 'string'
                }, {
                    name: 'dateflg',
                    mapping: 'dateflg',
                    type: 'string'
                }, {
                    name: 'def_int2',
                    mapping: 'def_int2',
                    type: 'string'
                }, {
                    name: 'previeweditflg',
                    mapping: 'previeweditflg',
                    type: 'string'
                }, {
                    name: 'def_int1',
                    mapping: 'def_int1',
                    type: 'string'
                }, {
                    name: 'remarks',
                    mapping: 'remarks',
                    type: 'string'
                }]
            });
            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'TemplateModel',
                autoLoad: true,
                url: C_ROOT + 'SUP/Print/GetFmtTemplateFromDb?typefile=' + typefile,
                listeners: {
                    load: function (m, records, successful, eOpts) {
                        if (records.length == 1 && !PrintDesignTime) {
                            var printId = records[0].get('printid');    //模板编号
                            var moudleType = records[0].get('def_int2');  //模板类型
                            var def_int1 = records[0].get('def_int1');  //直接打印
                            var previeweditflg = records[0].get('previeweditflg');  //预览可编辑
                            printWindowHelp.hide();
                            printWindowHelp.destroy();
                            if (callback) callback(printId, moudleType, previeweditflg || "0", def_int1 || "1");
                            //$PrintDetail(pageid, form, grids, titles, printId, typefile, moudleType);
                        }
                    }
                }
            });
            tmpStore = store;
            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                store: store,
                columnLines: true,
                autoScroll: true,
                forceFit: true,
                scroll: "vertical",
                layout: {
                    //                align: 'stretch',
                    //                type: 'hbox'
                },
                viewConfig: {
                    scrollOffset: 0
                },
                columns: [{
                    header: '编号',
                    flex: 1,
                    sortable: true,
                    dataIndex: 'printid'
                }, {
                    header: '单据类型',
                    flex: 1.5,
                    sortable: true,
                    dataIndex: 'typefile',
                    renderer: function (val, meta) {
                        if (meta) {
                            meta.tdAttr = "title='" + val + "'";
                        }
                        return val;
                    }
                }, {
                    header: '模板名称',
                    flex: 1.5,
                    sortable: true,
                    dataIndex: 'billname',
                    renderer: function (val, meta) {
                        if (meta) {
                            meta.tdAttr = "title='" + val + "'";
                        }
                        return val;
                    }
                }, {
                    header: '文件名称',
                    flex: 2,
                    titleAlign: 'center',
                    sortable: true,
                    dataIndex: 'filename',
                    renderer: function (val, meta) {
                        if (meta) {
                            meta.tdAttr = "title='" + val + "'";
                        }
                        return val;
                    }
                }, {
                    header: '类型',
                    flex: 1,
                    sortable: true,
                    dataIndex: 'def_int2',
                    renderer: function (val) {
                        switch (val) {
                            case "0":
                                return '用户';
                            case "1":
                                return '系统';
                            case "3":
                                return '系统_PDF';
                            default:
                                return '自定义';
                        }
                    }
                }, {
                    header: '预览编辑',
                    flex: 1,
                    sortable: true,
                    dataIndex: 'previeweditflg',
                    renderer: function (val) {
                        switch (val) {
                            case "0":
                                return '否';
                            case "1":
                                return '是';
                            default:
                                return '否';
                        }
                    }
                }, {
                    header: '直接预览',
                    flex: 1,
                    sortable: true,
                    dataIndex: 'def_int1',
                    renderer: function (val) {
                        switch (val) {
                            case "0":
                                return '否';
                            case "1":
                                return '是';
                            default:
                                return '是';
                        }
                    }
                }, {
                    header: '备注',
                    flex: 1.5,
                    titleAlign: 'center',
                    sortable: true,
                    dataIndex: 'remarks',
                    hidden: true
                }]
            });
            grid.on('itemdblclick', function () {
                var data = grid.getSelectionModel().getSelection();
                if (data.length > 0) {
                    var printId = data[0].get('printid');    //模板编号
                    var moudleType = data[0].get('def_int2');  //模板类型
                    var def_int1 = data[0].get('def_int1');  //直接打印
                    var previeweditflg = data[0].get('previeweditflg');  //预览可编辑
                    printWindowHelp.hide();
                    printWindowHelp.destroy();
                    if (callback) callback(printId, moudleType, previeweditflg || "0", def_int1 || "1");
                    //$PrintDetail(pageid, form, grids, titles, printId, typefile, moudleType);
                }
            });
            var y = (window.document.documentElement.offsetHeight - 300) / 3;
            printWindowHelp = Ext.create('Ext.window.Window', {
                title: "选择套打模板",
                border: false,
                height: 300,
                closable: false,
                width: 600,
                y: y,
                layout: 'border',
                modal: true,
                resizable: false,
                draggable: false,
                items: [toolbar, grid]
            });
        }
        else {
            tmpStore.cachePageData = false;
            tmpStore.load();
            tmpStore.cachePageData = true;
        }
        printWindowHelp.show();
    }

}
//#endregion


function NG3Ajaxrequest(option, msg) {

    var myMask = new Ext.LoadMask(document.body, { msg: msg || "正在处理..." });
    myMask.show();

    Ext.Ajax.request(option);

    Ext.Ajax.on('requestcomplete', function (conn, res, opts) {
        myMask.hide();
    });
    Ext.Ajax.on('requestexception', function (conn, res, opts) {
        myMask.hide();
    });

}


function unzip(b64Data) {
    var strData = atob(b64Data);
    var data = pako.inflate(strData, { to: 'string' });//直接解开
    return data;
}

function zip(str) {
    var binaryString = pako.gzip(str, { to: 'string' });
    //var binaryString = pako.deflate(str, { to: 'string' });

    return btoa(binaryString);
}

function $GetLength(str) {
    if (Ext.isEmpty(str))
    {
        return 0;
    }
    var blen = 0;
    for (var i = 0; i < str.length; i++) {
        if ((str.charCodeAt(i) & 0xff00) != 0) {
            blen++;
        }
        blen++;
    }
    return blen;
    //return str.replace(/[\u0391-\uFFE5]/g, "aa").length;//先把中文替换成两个英文，再计算长度
}


//#region imp
//方案选择帮助
function $IMPShcmeHelp(targetBuz, calback, parms, beforeFunc) {
    var clientSqlFilter = " bustype='" + targetBuz + "' and isuse=1 and sysflg<>1";
    if (parms) {
        parms.clientSqlFilter = clientSqlFilter;
    }
    else {
        parms = { "clientSqlFilter": clientSqlFilter };
    }
    ///检测是否只有一个方案，是则直接返回
    NG3Ajaxrequest({
        url: C_ROOT + 'IMP/Engine/Engine/GetSchemeId',
        params: { outqueryfilter: JSON.stringify(parms) },
        success: function (res, opts) {
            if (beforeFunc) beforeFunc();
            if (res.responseText == '') {
                Ext.MessageBox.alert('提示', '没有可用的自定义方案，请联系管理员进行配置！');
                return;
            }
            var resp = Ext.JSON.decode(res.responseText);
            if (res.responseText == "" || (Object.prototype.toString.call(resp) === '[object Array]' && resp.length > 0)) {
                var grid_TplHelp = Ext.create('Ext.ng.RichHelp', {
                    allowBlank: false,
                    ORMMode: false,
                    showCommonUse: false,
                    helpid: 'imp3_scheme',
                    editable: false,
                    readOnly: false,
                    mustInput: false,
                    outFilter: parms,
                    clientSqlFilter: clientSqlFilter
                });
                grid_TplHelp.on('helpselected', function (obj) {
                    if (calback) calback(obj.data.phid || obj.data["imp3_scheme.phid"]);
                });
                grid_TplHelp.showHelp();
            }
            else {
                if (calback) calback(res.responseText);
            }
        }
    });
}
//源选择帮助
function $IMPSourceHelp(schemephid, tabSign, callback, custom, iswin) {
    var clientSqlFilter = " schemephid=" + schemephid + " and business_sign='" + tabSign + "'";
    var parms = { "clientSqlFilter": clientSqlFilter };
    ///检测是否只有一个源，是则直接跳到数据选择
    NG3Ajaxrequest({
        url: C_ROOT + 'IMP/Engine/Engine/GetSourceId',
        params: { schemephid: schemephid, business_sign: tabSign },
        success: function (res, opts) {
            if (res.responseText == "") {
                var grid_TplHelp = Ext.create('Ext.ng.RichHelp', {
                    allowBlank: false,
                    showCommonUse: false,
                    ORMMode: false,
                    helpid: 'imp3_schemesource',
                    editable: false,
                    readOnly: false,
                    mustInput: false,
                    outFilter: parms,
                    clientSqlFilter: clientSqlFilter
                });
                grid_TplHelp.on('helpselected', function (obj) {
                    $IMPSourceDataHelp(schemephid, obj.data.phid || obj.data["imp3_schemesource.phid"], callback, custom, iswin);
                });
                grid_TplHelp.showHelp();
            }
            else {
                $IMPSourceDataHelp(schemephid, res.responseText, callback, custom, iswin);
            }
        }
    });
}
//源数据选择帮助
function $IMPSourceDataHelp(schemephid, sourcephid, callback, custom, iswin) {

    var closeflg = iswin == 1 ? false : true; //iswin==1 表示win的关闭销毁由业务组控制
    var grid_TplHelp = Ext.create('Ext.ng.ImpCommonHelp', {
        allowBlank: false,
        ORMMode: false,
        schemephid: schemephid,
        sourcephid: sourcephid,
        editable: false,
        readOnly: false,
        mustInput: false,
        isclose: closeflg
    });
    if (custom) {
        grid_TplHelp.setCustom(custom);
    }
    grid_TplHelp.on('helpselected', function (obj) {
        NG3Ajaxrequest({
            url: C_ROOT + 'IMP/Engine/Engine/GetMappingData?schemephid=' + schemephid + '&sourcephid=' + sourcephid,
            params: { data: JSON.stringify(obj.data) },
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    if (callback) callback(resp.data, grid_TplHelp.sourcewin);
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    });
    grid_TplHelp.sourcewin = grid_TplHelp.showHelp();



}

//上查下查(单主键)
function $IMPUpDownSearch(buscode, temppkitem, temppkvalue, type) {
    var text = '当前单据';
    if (typeof (busTitle) != 'undefined' && busTitle != '') {
        text = busTitle;
    }
    $OpenTab("IMP穿透", C_ROOT + 'SUP/IMPPenetration/IMPPenetration?buscode=' + buscode + '&temppkitem=' + temppkitem + '&temppkvalue=' + temppkvalue + '&text=' + text);
}
//获取方案信息
function $IMPSchemeInfo(schemephid) {
    var schemeinfo = {};
    if (!isNaN(schemephid) && schemephid) {
        NG3Ajaxrequest({
            params: { phid: schemephid, needdetail: 1 },
            url: C_ROOT + 'IMP/Template/Imp3Scheme/FindImp3Scheme',
            async: false,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.totalRows > 0) {
                    schemeinfo = resp.Record[0];
                }
            }
        });
    }
    return schemeinfo;
}
//IMP帮助列格式化公共方法
function $IMPMaskPublicMethod(funcname) {
    function ToPercent(data, digits) {
        return Number(data * 100).toFixed(digits) + "%";
    }
    if (funcname == "ToPercent") {
        return ToPercent;
    }
}
//#endregion

//创建灵动菜单条
function $CreateFloatMenu(code) {

    Ext.Ajax.request({
        url: C_ROOT + 'SUP/CustomFloatMenu/GetFloatMenuByCode',
        params: { 'code': code },
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            var items = resp.items;
            if (items.length > 0) {
                var myTabs = [];
                var floatMenuWindow = Ext.create('Ext.window.Window', {
                    title: '灵动菜单条',
                    width: 350,
                    height: 60,
                    plain: false,
                    resizable: false,
                    modal: false,
                    closable: false,
                    collapsible: true,
                    draggable: true,
                    tbar: Ext.create('Ext.toolbar.Toolbar', {
                        xtype: 'tooltar',
                        id: 'floatmenutoolbar',
                        layout: {
                            overflowHandler: 'Menu'
                        }
                    }),
                    listeners: {
                        'move': function (window, x, y, eOpts) {
                            if (y < 0) {
                                window.showAt(x, 0);
                            } else if (y > document.body.offsetHeight - 60) {
                                window.showAt(x, document.body.offsetHeight - 60);
                            } else if (x < 0) {
                                window.showAt(0, y);
                            } else if (x > document.body.offsetWidth - 350) {
                                window.showAt(document.body.offsetWidth - 350, y);
                            }
                        }
                    }
                });

                myTabs.push({
                    iconCls: "icon-Setup",
                    border: false,
                    handler: function () {
                        $OpenTab('菜单条设置', C_ROOT + 'SUP/CustomFloatMenu?code=' + code);
                    }
                });
                myTabs.push('-');

                for (var i = 0; i < items.length; i++) {
                    myTabs.push({
                        text: items[i].Name,
                        name: items[i].Url,
                        height: 24,
                        handler: function () {
                            var url = this.name;
                            var index = url.indexOf('?');
                            if ((index == -1 && url.length < 5) || (index > -1 && index < 5)) {
                                Ext.MessageBox.alert('提示', '商业对象树配置的列表Url少于5个字符，请联系相关开发员!');
                                return;
                            }

                            var strs1 = url.split('[');
                            var strs2 = url.split(']');
                            if (strs1.length > 1 && strs2.length > 1) {
                                for (var n = 1; n < strs1.length; n++) {
                                    var str = strs1[n].substring(0, strs1[n].indexOf(']'));
                                    var strs = str.split('#');
                                    var value = null;
                                    try {
                                        value = Ext.getCmp(strs[0]).queryById(strs[1]).getValue();
                                    } catch (e) {
                                        
                                    }
                                    if (value != null) {
                                        url = url.replace('[' + str + ']', value);
                                    } else {
                                        if (url.indexOf("RW/DesignFrame/ReportView") > -1) {
                                            url = url.replace('[' + str + ']', '');
                                        } else {
                                            var start = url.lastIndexOf(',', url.indexOf(str));
                                            var end = url.indexOf(str) + str.length + 1;
                                            if (start == -1) {
                                                start = url.lastIndexOf('{', url.indexOf(str)) + 1;
                                                end++;
                                            }
                                            var temp = url.substring(start, end + 1);
                                            url = url.replace(temp, '');
                                        }
                                    }
                                }

                                if (url.charAt(url.length - 1) != '}') {
                                    url += '}';
                                }
                            }
                            
                            $OpenTab(this.text, C_ROOT + url);
                        }
                    });
                }

                Ext.getCmp("floatmenutoolbar").add(myTabs);
                floatMenuWindow.showAt(document.body.offsetWidth - 415, 0);
            }
        }
    });
  
}

//系统表单二次开发调用服务端封装
function execServer(funcName, paramObj, sucFunc) {

    if (funcName == '' || paramObj == '') {
        Ext.MessageBox.alert('提示', '功能名和参数不能为空');
        return;
    }

    Ext.Ajax.request({
        params: paramObj,
        url: C_ROOT + 'Addin/ExtendFunc/Action/' + funcName,
        async: false, //同步请求
        success: function (response) {         
            res = Ext.JSON.decode(response.responseText);
            sucFunc(res);
        }
    });
}

//grid日期格式化
function $ngDatetimeRenderer(val) {
    if (val) {
        var str = val;
        if (val.indexOf('00:00:00') > 0 || val.length < 11) {
            var str = Ext.util.Format.date(val, 'Y-m-d');//不能带H:i:s时分秒，否则自动加8个小时
        }
        else {
            var str = Ext.util.Format.date(val, 'Y-m-d H:i:s');
        }
        return str;
    } else {
        return '';
    }
};


var NGCalc = NGCalc || {};//乘除法对象
(function (me) {
    me.div = function (arg1, arg2) {
        var t1 = 0, t2 = 0, r1, r2;
        try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
        try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
        with (Math) {
            r1 = Number(arg1.toString().replace(".", ""));
            r2 = Number(arg2.toString().replace(".", ""));
            return (r1 / r2) * pow(10, t2 - t1);
        }
    }
    me.mul = function (arg1, arg2) {
        var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
        try { m += s1.split(".")[1].length } catch (e) { }
        try { m += s2.split(".")[1].length } catch (e) { }
        return Number(s1, replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    }
})(NGCalc);


//#region 在线帮助
function $OpenOnlineHelp(pageName, HelpBizType) {
    var sFeatures = "scrollbars=" + 0 + ",resizable=0" + "menubar=" + 0 + ",toolbar=" + 0 + ",status=1,location=" + 0;
    sFeatures += ",top=" + 100 + ",left=" + 100 + ",width=" + 1024 + ",height=" + 768;
        
    var asPageName;
    if (typeof (HelpBizType) != "undefined") {       
        asPageName = C_ROOT + "PCS/Help/HelpOnline.aspx?filename=" + pageName + "&btype=" + HelpBizType;
    }
    else {       
        asPageName = C_ROOT + "PCS/Help/HelpOnline.aspx?filename=" + pageName + "&btype=";
    }
    window.open(asPageName, "_blank", sFeatures);
}

//#region 页面打开的时间统计
function getPerformanceTiming() {
    var performance = window.performance, times = {};
    if (!performance) {
        // 当前浏览器不支持
        //console.log('你的浏览器不支持 performance 接口');
        return;
    }

    var t = performance.timing;
    times.url = window.location.href;
    //【重要】重定向的时间
    //【原因】拒绝重定向！比如，http://example.com/ 就不该写成 http://example.com
    times.redirectTime = t.redirectEnd - t.redirectStart;

    //【重要】DNS 查询时间
    //【原因】DNS 预加载做了么？页面内是不是使用了太多不同的域名导致域名查询的时间太长？
    // 可使用 HTML5 Prefetch 预查询 DNS ，见：[HTML5 prefetch](http://segmentfault.com/a/1190000000633364)            
    times.lookupDnsTime = t.domainLookupEnd - t.domainLookupStart;

    //【重要】读取页面第一个字节的时间
    //【原因】这可以理解为用户拿到你的资源占用的时间，加异地机房了么，加CDN 处理了么？加带宽了么？加 CPU 运算速度了么？
    // TTFB 即 Time To First Byte 的意思
    // 维基百科：https://en.wikipedia.org/wiki/Time_To_First_Byte
    times.ttfb = t.responseStart - t.navigationStart;

    //【重要】内容加载完成的时间
    //【原因】页面内容经过 gzip 压缩了么，静态资源 css/js 等压缩了么？
    times.requestTime = t.responseEnd - t.requestStart;
    // TCP 建立连接完成握手的时间
    times.tcpTime = t.connectEnd - t.connectStart;


    //onReady事件未开始执行
    if (Ext.isEmpty(Ext._beforeReadyTime)) {
        times.loadPageTime = -1;
        times.commonJsTime = -1;
    } else {        //by cwc    
        times.commonJsTime = Ext._beforeReadyTime - t.responseEnd;
    }

    //onReady事件函数已开始未完成
    if (Ext.isEmpty(Ext._afterReadytime)) {
        times.loadPageTime = "-1";
    } else {
        //【重要】页面加载完成的时间
        //【原因】这几乎代表了用户等待页面可用的时间
        //times.loadPage = t.loadEventEnd - t.navigationStart;     
        times.loadPageTime = Ext._afterReadytime - t.navigationStart;
    }

    Ext.Ajax.request({
        url: C_ROOT + '/Log/LogPage/PushPageLog',
        params: { data: JSON.stringify(times) }
    });

}

//3.5秒后收集性能数据
setTimeout("getPerformanceTiming()", 3500);
//#endregion

function $NG3Refresh() {
    var f = $GetWFrame();
    if (f && f.Center) {
        var obj = f.Center.items.items;
        for (var i = 0 ; i < obj.length; i++) {
            try {
                obj[i].contentEl.contentWindow.NG3Refresh();
            }
            catch (e) { }
        }
    }
}

//页面间回刷数据
function $FireRefreshEvent(busType, data) {
    var frame = $GetWFrame();
    if (frame) {
        var listref = frame.ListObserver.get(busType);
        listref.fireEvent('refreshlist', data);
    } else {
        window.external.RefreshWebListPage();
    }
}
//注册页面回刷事件
function $OnRefreshEvent(busType, callback) {
    var frame = $GetWFrame();
    if (frame) {
        var listref = Ext.create('Ext.ng.ListRefresher');
        frame.ListObserver.add(busType, listref);
        listref.on('refreshlist', callback)
    }
}

//页面刷新portal方法
function RefreshPortal(portalId, count) {
    if (window.external.IsInWebBrowser == undefined) {
        if (window.parent != null && window.parent != undefined && window.parent.frame.Center.queryById('mainHome') != null && window.parent.frame.Center.queryById('mainHome') != undefined) {
            window.parent.frame.Center.queryById('mainHome').contentEl.contentWindow.refreshPortal(portalId, count);
        }
    } else {
        window.external.RefreshPortal(portalId, count);
    }
}