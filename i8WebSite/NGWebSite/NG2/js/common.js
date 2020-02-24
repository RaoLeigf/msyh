///<reference path="../vswd-ext.js" />

Ext.require([
    'Ext.Ajax.*',
    'Ext.window.*',
    'Ext.util.*',
    'Ext.panel.*'
]);

var $ajaxInfo, $ajaxData, _isIE = navigator.userAgent.indexOf("MSIE") >= 0, w = window,
$args = w.lastArguments,

VMODE = {};  //new Ext.ux.plugin.VisibilityMode({ hideMode: 'nosize' });

Ext.useShims = true;
Ext.Ajax.timeout = 300000;

//var App = new Ext.App();
var Cookie = new Ext.state.CookieProvider({
    path: "/",
    expires: new Date(new Date().getTime() + (1000 * 60 * 60 * 24 * 30)) //30 days
});


//#region AF
function $AF(o) {
    return $AF.ctor.call($AF, o);
}

$AF.toParam = function (obj) {
    var pm = obj;
    if (Ext.isObject(obj)) {
        var arr = [];
        for (var p in obj) {
            //对include进行特殊处理,自动获取handle
            if (p.toLowerCase() == 'include' && obj[p].$AF) {
                obj[p] = obj[p].func("GetHandle", "");
            }

            arr.push(p);
            arr.push("=");
            //args.push(typeof aij[p] == 'string' ? aij[p].replace(/\r\n/g, '\\r\\n') : aij[p]);
            arr.push(obj[p]);
            arr.push(";");
        }
        pm = arr.join('');
    }
    return pm;
};

$AF.create = function (pobj) {
    var o, path = location.href.substr(0, location.href.indexOf(C_ROOT, 7)) + C_ROOT + "NG2/supcan/",
        cfg = pobj.ObjConfig,
        zipurl = path + "BCV1.bin",
        typeid = _isIE ? 'CLASSID="clsid:619F1AC0-2644-40D3-9EB1-22F81C5FE097" Codebase="' + path + 'supcan2.cab#Version=1,0,0,3"' : 'type="application/supcan-plugin" Codebase="' + path + 'supcan.xpi"';

    if (cfg.type == "LuxForm")
        zipurl += "," + path + "LuxForm.bin";
    else if (cfg.type.indexOf("BCV4") >= 0)
        zipurl += "," + path + "BCV4.bin";

    try {
        cfg.ver = SupCanVersion;
    } catch (e) { }

    cfg.param = 'Border=0;';


    if ($sessionid) {
        document.cookie = 'ASP.NET_SessionId=' + escape($sessionid);
    }

    $$(pobj).innerHTML = ['<Object id=', cfg.id, ' Width=100% height=100% ', typeid, '>',
             '<param Name="CtlName" Value="', cfg.type, '">',
             '<param Name="CtlVersion" Value="', cfg.ver, '">',
             '<param Name="ZipUrl" Value="', zipurl, '">',
             '<param Name="id" Value="', cfg.id, '">',
             '<param Name="Cookie" Value="', document.cookie, '">',
             '<param Name="CtlPara" Value="dynaloadVersion=2;', cfg.param, '"></Object>'
            ].join('');
    o = $$(pobj).firstChild;
    //}
    o.Template = pobj.Template;
    o.Cfg = cfg;
    return o;
}

$AF.ctor = function (o) {
    var o = $$(o);
    if (!o) alert("Error: $AF(o) == null");
    if (!o.isReady || o.$AF) return o;

    try {
        var ps = o.func('reflectProps', ''), fs = o.func('reflectFuncs', '');

        if (ps.length + fs.length == 0) return;
        ps = ps.split(',');
        fs = fs.split(',');


        _initFuncs(fs);

        _initProps(ps);

        //初始化
        o.init = function () {
        }

        //静态添加一个get函数 返回的是子控件的id
        o.get = function (id) {
            $AF.Controls = $AF.Controls || {};
            var sub = $AF.Controls[id];
            if (!sub) {
                var type = o.func('GetObjectType', id);
                if (type) {
                    var ps2 = o.func('reflectProps', id).split(','),
                            fs2 = o.func('reflectFuncs', id).split(',');

                    sub = $AF.Controls[id] = { id: id, xtype: type, AF: o };
                    //注入子对象的方法和属性
                    _initFuncs(fs2, sub);
                    _initProps(ps2, sub);
                }
            }
            return sub;
        };

        _ext();

        o.$AF = true;
    }
    catch (e) { }

    return o;

    //对OBJECT的自定义扩展
    function _ext() {
        o.isChanged = IsChanged = o.getChanged;
        switch (o.Cfg.type) {
            case 'BCV1.TreeList':
                setFunc('getChanged', function (opt) {
                    opt = opt || {};
                    if (opt.level === undefined) opt.level = 1;
                    var r = o.func('toJson', o.GetChangedXML($AF.toParam(opt)));
                    if (r == 0) r = '';
                    return r;
                });
                //返回选中的行
                setFunc('getCheckedRows', function (rowname) {
                    rowname = rowname || 'checked';
                    return o.FindAll(rowname + '=1').split(',');
                });
                //获取主键的值
                setFunc('getKeyValue', function () {
                    var vs = [], a = arguments;
                    for (var i = 0; i < a.length; i++) {
                        if (Ext.isArray(a[i])) {
                            for (var j = 0; j < a[i].length; j++) {
                                _getV(a[i][j]);
                            }
                        }
                        else {
                            _getV(a[i]);
                        }
                    }

                    return vs.length == 1 ? vs[0] : vs.length == 0 ? '' : vs;

                    function _getV(n) {
                        if (n != '' && n != null) {
                            n = Number(n);
                            if (!isNaN(n))
                                n = o.GetRowKey(n);

                            if (n || n != '')
                                vs.push(n);
                        }
                    }
                });
                //获取Query
                setFunc('getQuery', function (bar) {
                    var h = this.GetHandle(bar || "topBar");
                    var xml = o.func(h + "Export", "content=asData;includeEmpty=False");
                    return xml == '' ? '' : o.func("toJSON", xml + "\r\n recordset");
                });
                break;
            case 'BCV1.FreeForm':
                setFunc('getChanged', function (opt) {
                    opt = opt || {};
                    if (opt.level === undefined) opt.level = 1;
                    var r = o.func('toJson', o.GetChangedXML(opt));
                    if (r == 0) r = '';
                    return r;
                });
                //获取查询数据
                setFunc('getQuery', function (h) {
                    h = h || '';
                    var xml = o.func(h + "Export", "content=asData;includeEmpty=False");
                    return xml == '' ? '' : o.func("toJSON", xml + "\r\n recordset");
                });
                break;
            case 'BCV1.Tree':
                break;
            case 'LuxForm':
                break;
            case 'BCV1.Chart':
                break;
            default:
                alert('对OBJECT的自定义扩展未实现' + o.GetCtlName());
                break;
        }
        o._OpenLoadMask = o.OpenLoadMask;
        setFunc('OpenLoadMask', function (ms) {
            o._OpenLoadMask(ms || 500);
        });

        function setFunc(fname, func) {
            var s1 = fname.slice(0, 1), s2 = fname.slice(1);
            o[s1.toUpperCase() + s2] = o[s1.toLowerCase() + s2] = func;
        }
    }

    function _initProps(props, obj) {
        obj = obj || o;
        _each(props, function (item) {
            if (item === 'Title') return; //跳过title属性，解决treelist的tooltip效果显示代码块的问题
            var s1 = item.slice(0, 1), s2 = item.slice(1);
            if (obj[s1.toUpperCase() + s2] || obj[s1.toLowerCase() + s2]) s1 = '$' + s1;
            obj[s1.toUpperCase() + s2] = obj[s1.toLowerCase() + s2] = function (v) {
                if (arguments.length == 0) {//get
                    return obj == o ? o.GetProp(item) : o.GetObjectProp(obj.id, item);
                }
                else {
                    obj == obj == o ? o.SetProp(item, v) : o.SetObjectProp(obj.id, item, v);
                }
            }
        });
    }

    function _initFuncs(funcs, obj) {
        obj = obj || o;
        _each(funcs, function (item) {

            var s1 = item.slice(0, 1), s2 = item.slice(1);
            //if (obj[s1.toUpperCase() + s2] || obj[s1.toLowerCase() + s2]) alert(obj.id + '方法重复:' + item);
            obj[s1.toUpperCase() + s2] = obj[s1.toLowerCase() + s2] = function () {
                return o.func(item, obj == o ? _args(arguments) : _args(obj.id, arguments));
            };
        });
    }

    function _args() {
        var a = arguments, args = [];
        for (var i = 0; i < a.length; i++) {
            if (typeof a[i] == 'object' && typeof a[i].length == 'number') {
                for (var j = 0; j < a[i].length; j++) {
                    var aij = a[i][j];
                    if (typeof aij == 'string') {
                        args.push(aij);
                    }
                    else if (typeof aij == 'object') {
                        //将object转换成 supcan 能认识的参数
                        //object 自动拆分成  p1=value1;p2=value2 的形式
                        args.push($AF.toParam(aij));
                    }
                    else {
                        args.push(a[i][j]);
                    }
                }
            }
            else {
                args.push(a[i]);
            }
        }
        return args.join("\r\n");
    }

    function _each(arr, func) {
        var i, tmp;
        for (i = 0; i < arr.length; i++) {
            tmp = arr[i];
            if (tmp) {
                func.call(o, tmp, i);
            }
        }
    }
}

//必需的函数(控件会反向调用，用途:切换焦点)
function focusIE(o) {
    //debugger;
    var ae = document.activeElement;
    if (ae) {
        if (!_isIE) {
            ae.blur();
            return;
        }

        if (typeof (o) == 'object') {
            if (ae == o) return;
            o.focus();
        }
        else {
            if (ae.id == o) return;
            var o = $$(o);
            try {
                if (o != null) o.focus();
            }
            catch (err) {
                //alert("focus异常:" + err.message  );
            }
        }
    }
}

function OnReady(id) {
    var o = $$(id);
    if (o) {
        if (!o.$AF) {
            o.Template = o.Template || '1';
            o.isReady = true;
            o = $AF(o);
            //console.log(id + ' try to onready');
        }

        if (o && o.$AF) {
            //console.log(id + ' try to Build Width:'+o.offsetWidth);
            if (o.Template && o.Template.length > 1) {
                o.Template = o.func("Build", o.Template);
            }
            if (o.OnReady) {
                try { o.OnReady(); } catch (e) { }
                o.OnReady = null;
            }
            if (o.bakPNode && o.offsetWidth == 1 && o.offsetHeight == 1) {
                o.style.width = o.bakWidth || '100%';
                o.style.height = o.bakHeight || '100%';
                o.bakPNode.appendChild(o);
                //console.log('ieAF:' + o.id);
            }
        }
    }
}

var __allReadyInterval;

window.onload = function () {

    __allReadyInterval = __allReadyInterval || setInterval(function () {
        var objs = document.getElementsByTagName("object"), r = true; all = 0; allready = 0;
        for (var i = 0; i < objs.length; i++) {

            var obj = objs[i];

            //debugger;
            //alert(obj.id);
            if (obj.codeBase.indexOf("supcan") >= 0 && obj.Cfg && !obj.$AF) {
                if (_isIE && (obj.offsetWidth == 0 || obj.offsetHeight == 0)) {
                    obj.bakPNode = obj.parentNode;
                    obj.bakWidth = obj.style.width;
                    obj.bakHeight = obj.style.height;
                    obj.style.width = obj.style.height = "1px";
                    (document.body || document.documentElement).insertBefore(obj);
                }

                try { OnReady(obj.id); } catch (e) { }
                r = false;
            }
        }

        if (r) {
            clearInterval(__allReadyInterval);
            //console.log('all ready');
            AllReady();
        }
    }, 200);
}
function AllReady() { }

function OnEvent(id, Event, p1, p2, p3, p4) {
    //console.log('id=' + id + ' EventName=' + Event + ' | ' + p1 + ' | ' + p2 + ' | ' + p3 + ' | ' + p4);
    //假如执行如下js
    //alert('id:' + id + '\nEvent:' + Event);
    var o = $AF(id);
    if (o) {
        if (Event.slice(0, 2) != 'On') Event = 'On' + Event;
        var evt = o[Event];
        if (o.GetCtlName().toLowerCase() == 'freeform') {
            //'FreeForm'中 p1 为子控件的 id
            var sub = o.get(p1);
            if (sub) {
                evt = sub[Event];
                o = sub;
            }
        }

        if (typeof evt == 'function')
            evt.call(o, p1, p2, p3, p4);
    }
}
//#endregion

//#region 对Ext的扩展

//QueryString
Ext.getUrlParam = function (param) {
    var params = Ext.urlDecode(location.search.substring(1));
    return param ? params[param] : params;
};

//将 Ajax.request 的 url 中 @ 替换成 当前页的名称
Ext.Ajax.on('beforerequest', function (conn, opts) {

    opts.url = $url(opts.url);
    //opts.params 的一级子对象转换为字符串
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
    if (res.status == 0)
        Ext.Msg.alert('requestexception', 'url:' + opts.url + '<br/>' + res.statusText);
});
//#endregion

//#region Panel 扩展
Ext.Panel.prototype.updateIframe = function (url) {
    this.html = "<iframe src='" + url + "' width=100% height=100% frameborder=0></iframe>";
    this.update(this.html);
}
//#endregion

//#region Window 扩展


Ext.override(Ext.window.Window, {

    //为Window添加一个 waitIframe 方法
    waitIframe: function (url, func, funcMustDo) {

        this.isIframeInited = this.isIframeInited || false;
        if (!this.rendered) {
            this.render(Ext.getBody());
        }
        if (this.isIframeInited) {
            //必须执行的函数
            if (this.isIframeReady && funcMustDo) funcMustDo.call(this);
        }
        else {
            this.updateIframe(url);
            var iframe = this.body.dom.childNodes[0];
            this.dw = iframe.contentWindow;
            this.dw.winCmp = this;
            iframe.winCmp = this;
            var count = 0;

            //debugger;

            //            var interval = interval ||
            //			setInterval(function () {
            //			    if (!this.dw.isReady) {
            //			        if (++count >= 6000) {
            //			            clearInterval(interval);
            //			        }
            //			        return;
            //			    }
            //			    clearInterval(interval);

            //			    this.isIframeReady = true;
            //			    func.call(this);

            //			    //必须执行的函数
            //			    if (funcMustDo) funcMustDo.call(this);
            //			} .createDelegate(this), 50);

            var interval = interval ||
			 setInterval(Ext.Function.bind(function () {
			     if (!this.dw.isReady) {
			         if (++count >= 6000) {
			             clearInterval(interval);
			         }
			         return;
			     }
			     clearInterval(interval);

			     this.isIframeReady = true;
			     func.call(this);

			     //必须执行的函数
			     if (funcMustDo) funcMustDo.call(this);
			 }, this), 50);

            this.isIframeInited = true;
        }
    },

    //centerToScreen
    centerToScreen: function () {
        var xy = this.el.getAlignToXY(this.container, 'c-c');
        this.setPagePosition(xy[0], xy[1] - xy[1] * 0.25);
        return this;
    },

    //#region Cover ActiveX
    maskObj: function (b) {
        this.objArr = $mask(b, this);
    },

    //        beforeShow: Ext.Window.prototype.beforeShow.createSequence(function () {
    //            if (this.autoScreen) this.centerToScreen();

    //            if (this.modal) this.maskObj(true);
    //        }),
    //        hide: Ext.Window.prototype.hide.createSequence(function () {
    //            if (this.modal) {
    //                this.maskObj(false);
    //            }
    //        })

    beforeShow: Ext.Function.createSequence(Ext.Window.prototype.beforeShow, function () {
        if (this.autoScreen) this.centerToScreen();

        if (this.modal) this.maskObj(true);
    }, this),

    hide: Ext.Function.createSequence(Ext.Window.prototype.hide, function () {
        if (this.modal) {
            this.maskObj(false);
        }
    }, this)


    //这个弹出窗口隐藏不掉
    //    beforeShow: function () {

    //        if (this.autoScreen) this.centerToScreen();

    //        if (this.modal) this.maskObj(true);       
    //    },
    //    hide: function () { 
    //        if (this.modal) {
    //            this.maskObj(false);
    //        }       
    //    }

    //#endregion

});

//#endregion

//#region 自定义扩展

Ext.FitDiv = function (id) {
    var obj = $$(id), w = window, doc = document, de = 'documentElement',
        r = obj.getAttribute('right'), b = obj.getAttribute('bottom'),
        rr = Math.abs(parseInt(r, 10)), bb = Math.abs(parseInt(b, 10));

    Ext.EventManager.addListener(w, 'resize', initSize);
    initSize();
    return obj;

    function initSize() {
        //debugger;
        extobj = Ext.get(obj);
        width = w.innerWidth ? w.innerWidth : (doc[de] && doc[de].clientWidth) ? doc[de].clientWidth : doc.body.offsetWidth;
        height = w.innerHeight ? w.innerHeight : (doc[de] && doc[de].clientHeight) ? doc[de].clientHeight : doc.body.offsetHeight;

        if (r != null) {
            if (r.indexOf('%') >= 0)
                extobj.setWidth(width * (100 - rr) / 100 - extobj.getLeft() - 1);
            else
                extobj.setWidth(width - rr - extobj.getLeft() - 1);
        }

        if (b != null) {
            height = b.indexOf('%') >= 0 ? height * (100 - bb) / 100 - extobj.getTop() - 1 : height - bb - extobj.getTop() - 1;
            extobj.setHeight(height);
            if (_isIE && extobj.first())
                extobj.first().setHeight(height);
        }

    }
}

//#region $ShowHelp
function $ShowHelp(opts) {
    if (!Ext.isObject(opts)) {
        alert('$ShowHelp: opts must be a object');
        return;
    }
    opts.$params = opts.$params || {};
    opts.$params.callback = opts.$params.callback || opts.callback;
    opts.$params.ctrl = opts.$params.ctrl || opts.ctrl;
    opts.$params.codeField = opts.$params.codeField;
    opts.$params.nameField = opts.$params.nameField;

    debugger;

    opts.$params.id = opts.$params.id || opts.$params.helpid || opts.id || opts.helpid
    opts.$params.selectMode = opts.$params.selectMode//单选:0,多选：1

    if (opts.$params.id == null || opts.$params.id == '') {
        alert('$ShowHelp: id is null or empty');
        return;
    }
    delete opts.id;

    //opts.url = C_ROOT + "SUP/Base/CommHelp.aspx?id=" + opts.$params.id + "&codeField=" + opts.$params.codeField + "&nameField=" + opts.$params.nameField;

    opts.url = C_ROOT + "SUP/CommonHelp?helpid=" + opts.$params.id + "&selectMode=" + opts.$params.selectMode
                               + "&codeField=" + opts.$params.codeField + "&nameField=" + opts.$params.nameField;
    $ShowDialog(opts)
}
//#endregion

//#region $ShowDialog
//url 地址  isWFrame是否是框架级 modal是否模态  width height
function $createDialog(opts) {
    debugger;
    if (Ext.isString(opts)) opts = { url: opts };
    if (Ext.isEmpty(opts.url)) {
        alert('$ShowDialog:url is null');
        return;
    }
    var autoId = 'dialog_' + opts.url.replace(/[\/\.\?=]/g, '');
    //console.log(autoId);

    //    var winCmp = $WinMgr.get(autoId), backupClosable = opts.closable;
    //    if (winCmp != null && !winCmp.isIframeReady) {
    //        winCmp.destroy();
    //        winCmp = null;
    //    }     

    var winCmp = null, backupClosable = opts.closable;
    //保证winCmp是可关闭的,便于在载入失败时用户可以关闭窗口      
    opts.closable = true;

    if (winCmp == null) {
        winCmp = new Ext.Window(Ext.apply({
            //id: autoId, //id不能重复
            //plugins: VMODE,
            closeAction: 'hide',
            modal: true,
            autoScreen: true,
            title: '&nbsp;',
            initHidden: true,
            width: 500,
            height: 300,
            layout: 'fit',
            open: function () {
                //显示窗口
                this.show();

                //不让窗体标题被隐藏
                if (this.y < 0) this.setPagePosition(this.x, 1);

                this.waitIframe(this.url,
	            function () {
	                //debugger;
	                //this.buttons[0].destroy();
	                this.getDockedItems()[1].getComponent(0).destroy(); //ext4
	                if (!backupClosable) {

	                    //this.getTool('close').hide();
	                    //this.tools.close.hide();
	                    this.getDockedItems()[0].getComponent(1).hide(); //ext4
	                }
	                var dw = this.dw;
	                if (dw.init) dw.init.call(this, this);
	                //if (dw.hideAction) this.on('hide', this.dw.hideAction.createDelegate(winCmp, [winCmp.$params]));
	                if (dw.hideAction) this.on('hide', Ext.Function.bind(this.dw.hideAction, winCmp, [winCmp.$params]));
	            },
	            function () {
	                var dw = this.dw;
	                if (dw && dw.dealAction) {
	                    dw.dealAction.call(winCmp, winCmp.$params);
	                }
	            }
            );
            },
            listeners: {
                move: function (me, x, y) {
                    //不让窗体标题被隐藏
                    if (y < 0) me.setPagePosition(me.x, 1);
                }
            },
            buttons: [{ text: '正在载入...'}]
        }, opts));

        //$WinMgr.register(winCmp);
    }
    else {
        winCmp.$params = opts.$params;
    }

    return winCmp;
}

function $ShowDialog(opts) {
    $createDialog(opts).open();
}
//#endregion

//判断是否是字段对象
function $IsField(obj) {
    return obj.isFormField;
}

//items : Container.items
//validate: 是否验证表单
//idRegex: id1|id2  匹配的正则表达式
//return: params
function $BuildFieldParmas(items, validate, idRegex) {
    var params = {};
    _buildValues(items);
    return params;

    function _buildValues(items) {
        if (items === undefined) {
            alert('$BuildFieldParmas: 请确保container.items有值');
            return {};
        }
        items.each(function () {
            if (this.items) {
                _buildValues(this.items);
            }

            var id = this.getId();
            if (checkID(id) && $IsField(this)) {
                if (validate && !this.validate()) {
                    //验证失败
                    params = false;
                    return;
                }
                params[id] = this.getValue();
            }
        });
    }

    function checkID(id) {
        if (idRegex == null) return true;
        return idRegex.test(id);
    }
}

function $ValidateResponse(xhr) {
    //debugger;       
    if (xhr.valid === undefined) {
        var info = $ajaxInfo = $GetAjaxInfo(xhr);
        if (!$ajaxInfo) return true;
        $ajaxData = $ajaxInfo.data;
        if ($ajaxData && $ajaxData.NeedLogin) {

            //显示登陆窗口
            var f = parent.WFrame || w.WFrame;
            if (f) {
                var opts = xhr.options;
                if (opts.url && opts.url.charAt(0) != '/') {
                    opts.url = $path(C_PATH + opts.url);
                }
                f.Login.open(opts);
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

//response: ResponseText
//validate: 是否对返回的json进行验证,如果错误则提示,默认=false
function $DecodeResponse(response, validate) {
    //ss(response);
    var rtn = response.valid;
    if (validate && !$ValidateResponse(response)) {//需要验证时 而且 验证失败时,直接返回,不进行转换
        rtn = 0;
    }
    else {
        //responseText 为空时,返回1 即:true
        rtn = Ext.decode(response.responseText || rtn);
    }

    return rtn;
}

function $GetAjaxInfo(response) {

    //必须加上trim,IE中的BUG,会在后面加一个空格
    return Ext.decode(response.getResponseHeader("AjaxInfo"));
}

function $GetWFrame() {
    try {
        var cw = w;
        while (cw != top) {
            if (cw.WFrame)
                break;
            else
                cw = cw.parent;
        }
    } catch (e) { }
    return cw.WFrame;
}

function $OpenTab(title, url, params) {
    var f = $GetWFrame();
    if (f && f.Center) {
        f.Center.openTab(title, url, params);
    }
    else {
        var nw = window.open($path(url), title, 'toolbar=no, menubar=no, scrollbars=no,resizable=no,location=no, status=no');

        nw.lastArguments = params || {};
    }
}

function $CloseTab(n) {
    var f = $GetWFrame();
    if (f && f.Center) {

        //debugger;

        //f.Checker.fireEvent('checkin', f.Checker);//触发签入操作

        //var tab = f.Center.get(n) || f.Center.getActiveTab();
        var tab = f.Center.getComponent(n) || f.Center.getActiveTab();

        f.Center.remove(tab);
    }
    else {
        w.close();
    }
}

//#endregion

//#region 业务扩展
//#region 按钮状态控制
//UIType NotControl=默认不控制,View=查看,Add=新增,Edit=编辑,Verify=审核,组合的时候可以一起写,如AddEdit=表示新增编辑
function $SetUIState(cont, state, hide) {
    var funcName;
    if (cont) {
        if (cont.cascade) {
            funcName = hide ? 'setVisible' : 'setDisabled';
            cont.cascade(function (item) {
                if (item[funcName] && item.UIType && !item.UIType.match(/NotControl/i)) {
                    var b = item.UIType.match(new RegExp(state, "i"));
                    if (!hide) b = !b;
                    item[funcName](b);
                }
            });
        }
        else if (cont.GetObjectID) {
            funcName = hide ? 'Visible' : 'Enable';
            for (var i = 0; i < 9999; i++) {
                var id = cont.GetObjectID(i);
                if (id) {
                    var uitype = cont.GetObjectProp(id, 'UIType');
                    if (uitype && !uitype.match(/NotControl/i)) {
                        cont.SetObjectProp(id, funcName, item.UIType.match(new RegExp(state, "i")));
                    }
                }
                else
                    break;
            }
        }
    }
}
//#endregion
//#endregion

//#region Util

function HTMLDecode(strHtml) {

    var div = document.createElement('div');
    div.innerHTML = strHtml;

    var str = div.firstChild.nodeValue;
    return str;

    //           if (div.innerText == undefined) {
    //              return  div.TextContent;//firefox
    //           }
    //           else {
    //               return div.innerText;//IE
    //           }

}

//#endregion

//function ss(o) {
//    var s = '';
//    for (var p in o) {
//        if (o[p] != null) {
//            s += p + ': ' + o[p] + '\n'
//        }
//    }
//    alert(s);
//}

//var console = console;
//if (!console || console && !console.log) {
//    console = console || {};
//    var c = 0;
//    console.log = function (s) {
//        if (c++ < 30) {
//            alert(s);
//        }
//    }
//}

//#region 在线帮助
function $OpenOnlineHelp(sitepath, pageName, HelpBizType) {
    if (typeof (HelpBizType) != "undefined") {
        OpenWebForm(sitepath + "PCS/Help/HelpOnline.aspx?filename=" + pageName + "&btype=" + HelpBizType, 0, 0, 1024, 768);
    }
    else {
        OpenWebForm(sitepath + "PCS/Help/HelpOnline.aspx?filename=" + pageName + "&btype=", 0, 0, 1024, 768);
    }
}

//打开Web页面
function OpenWebForm(asPageName, aiMenu, aiScrollbar, aiWidth, aiHeight) {
    var lLeft, lTop, lWidth, lHeight;

    lWidth = WindowWidthDeal(aiWidth);
    lHeight = WindowHeightDeal(aiHeight);

    lLeft = WindowLeftDeal(-1, lWidth);
    lTop = WindowTopDeal(-1, lHeight);

    OpenWebFormByAll(asPageName, aiMenu, aiScrollbar, lWidth, lHeight, lLeft, lTop);
}

//在指定宽度、高度、上边距、左边距情况下打开打开Web页面
function OpenWebFormByAll(asPageName, aiMenu, aiScrollbar, lWidth, lHeight, lLeft, lTop) {
    var sFeatures;

    sFeatures = "scrollbars=" + aiScrollbar + ",resizable=0" + "menubar=" + aiMenu + ",toolbar=" + aiMenu + ",status=1,location=" + aiMenu;
    sFeatures += ",top=" + lTop + ",left=" + lLeft + ",width=" + lWidth + ",height=" + lHeight;

    window.open(asPageName, "_blank", sFeatures);
}

//页面高度处理
function WindowHeightDeal(aiHeight) {
    var allHeight, retHeight;

    allHeight = window.screen.availHeight;
    retHeight = aiHeight;


    //如果没有传入高度
    if (retHeight <= 0 || retHeight > allHeight) {
        if (allHeight < 650)
            retHeight = allHeight * 0.91;  //800*600时的高度为屏幕高度 * 0.91
        else {
            retHeight = allHeight - 50; //allHeight - 15;// 600; //1024*768时候为600
        }
    }

    //modify by niga
    /*
    if (allHeight < 650)
    retHeight = allHeight * 0.91;  //800*600时的高度为屏幕高度 * 0.91
    else
    {
    retHeight = allHeight - 50;//allHeight - 15;// 600; //1024*768时候为600
    }
    */

    return retHeight;
}

//页面宽度处理
function WindowWidthDeal(aiWidth) {
    var allWidth, retWidth;

    allWidth = window.screen.availWidth;
    retWidth = aiWidth;


    //如果没有传入宽度，则指定宽度为800
    if (aiWidth <= 0 || aiWidth > allWidth) {
        retWidth = allWidth - 5; // 800;
    }

    //modify by niga

    return retWidth;
}

//页面左边距处理

function WindowLeftDeal(aiLeft, aiWidth) {
    var allWidth, retLeft;

    allWidth = window.screen.availWidth;
    retLeft = aiLeft;

    //小于0或者超过屏幕宽度则居中打开		
    if (retLeft < 0 || retLeft + aiWidth > allWidth)
        retLeft = (allWidth - aiWidth) / 2;

    return retLeft;
}

//页面上边距处理

function WindowTopDeal(aiTop, aiHeight) {
    var allHeight, retTop;

    allHeight = window.screen.availHeight;
    retTop = aiTop;

    //小于0或者超过屏幕宽度则居中打开	
    if (retTop < 0 || retTop + aiHeight > allHeight)
        retTop = 5; // (allHeight - aiHeight)/2;

    return retTop; // - 20;

}

//#endregion