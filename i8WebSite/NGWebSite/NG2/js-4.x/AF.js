///<reference path="../vswd-ext.js" />
var $ajaxInfo, $ajaxData, _isIE = navigator.userAgent.indexOf("MSIE") >= 0, w = window,

//#region AF
function $AF(o) {
    return $AF.ctor.call($AF,o);
}

$AF.toParam = function (obj) {
    var pm = obj;
    if (Ext.isObject(obj)) {
        var arr = [];
        for (var p in obj) {
            //��include�������⴦��,�Զ���ȡhandle
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
    var o;
    //if (_isIE) {
    //    o = $('object', pobj)[0];
    //}
    //else {
    var path = "http://localhost" + C_ROOT + "NG2/supcan/",
        cfg = pobj.ObjConfig,
        zipurl = path + "BCV1.bin",
        typeid = _isIE ? 'CLASSID="clsid:619F1AC0-2644-40D3-9EB1-22F81C5FE097" Codebase="' + path + 'supcan2.cab#Version=1,0,0,3"' : 'type="application/supcan-plugin" Codebase="' + path + 'supcan.xpi"';

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
    return o;
}

$AF.ctor = function (o) {
    var o = $$(o);
    if (!o) alert("Error: $AF(o) == null");
    if (!o.isReady || o.$AF) return o;

    try {
        var ps = o.func('reflectProps', '').split(','),
            fs = o.func('reflectFuncs', '').split(',');

        _initFuncs(fs);

        _initProps(ps);

        //��ʼ��
        o.init = function () {
        }

        //��̬���һ��get���� ���ص����ӿؼ���id
        o.get = function (id) {
            $AF.Controls = $AF.Controls || {};
            var sub = $AF.Controls[id];
            if (!sub) {
                var type = o.func('GetObjectType', id);
                if (type) {
                    var ps2 = o.func('reflectProps', id).split(','),
                            fs2 = o.func('reflectFuncs', id).split(',');

                    sub = $AF.Controls[id] = { id: id, xtype: type, AF: o };
                    //ע���Ӷ���ķ���������
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

    //��OBJECT���Զ�����չ
    function _ext() {
        switch (o.GetCtlName()) {
            case 'TreeList':
                o.GetChanged = function (opt) {
                    opt = opt || {};
                    if (opt.level === undefined) opt.level = 1;
                    var r = o.func('toJson', o.GetChangedXML($AF.toParam(opt)));
                    if (r == 0) r = '';
                    return r;
                };
                //����ѡ�е���
                o.GetCheckedRows = function (rowname) {
                    rowname = rowname || 'checked';
                    var exp = rowname + '=1', rs = [], pos = -1, len = o.getRows();
                    do {
                        pos = o.find(exp, pos - 0 + 1);
                        if (pos == -1)
                            break;
                        else
                            rs.push(pos);

                    } while (true);

                    return rs;
                };
                o.GetKeyValue = function () {
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
                        n = Number(n);
                        if (!isNaN(n))
                            n = o.GetRowKey(n);

                        if (n || n != '')
                            vs.push(n);
                    }
                }
                break;
            case 'FreeForm':
                o.GetChanged = function (opt) {
                    opt = opt || {};
                    if (opt.level === undefined) opt.level = 1;
                    var r = o.func('toJson', o.GetChangedXML(opt));
                    if (r == 0) r = '';
                    return r;
                }
                break;
            case 'Tree':
                break;
            default:
                alert('��OBJECT���Զ�����չδʵ��' + o.GetCtlName());
                break;
        }
    }

    function _initProps(props, obj) {
        obj = obj || o;
        _each(props, function (item) {
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
            //if (obj[s1.toUpperCase() + s2] || obj[s1.toLowerCase() + s2]) alert(obj.id + '�����ظ�:' + item);
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
                        //��objectת���� supcan ����ʶ�Ĳ���
                        //object �Զ���ֳ�  p1=value1;p2=value2 ����ʽ
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

//����ĺ���(�ؼ��ᷴ����ã���;:�л�����)
function focusIE(o) {    
    if (!_isIE) {
        document.activeElement.blur();
        return;
    }

    if (typeof (o) == 'object') {
        if (document.activeElement == o) return;
        o.focus();
    }
    else {
        if (document.activeElement.id == o) return;
        var o = $$(o);
        if (o != null) o.focus();
    }
}

function OnReady(id) {
    var o = $$(id);
    if (o) {
        if (!o.$AF) {          
            o.isReady = true;  
            o = $AF(o);
        }
    }
}
var __allReadyInterval;var sss = '';
window.onload = function () {
    __allReadyInterval = __allReadyInterval || setInterval(function () {
        var objs = document.getElementsByTagName("object"), r = true; all = 0; allready = 0;
        for (var i = 0; i < objs.length; i++) {
            var obj = objs[i];
            obj.Template = obj.Template || '1';
            if (obj.codeBase.indexOf("supcan") >= 0) {
                if (obj.$AF) {
                    if (obj.Template && obj.Template.length > 1) {
                        obj.Template = obj.func("Build", obj.Template);
                    }
                    if (obj.OnReady) {
                        try { obj.OnReady(); } catch (e) { }
                    }
                }
                else {
                    r = false;
                }
            }
        }

        if (r) {
            clearInterval(__allReadyInterval);
            AllReady();
        }
    }, 30);
}
function AllReady() { }

function OnEvent(id, Event, p1, p2, p3, p4) {
    //����ִ������js
    //alert('id:' + id + '\nEvent:' + Event);
    var o = $AF(id);
    if (o) {
        if (Event.slice(0, 2) != 'On') Event = 'On' + Event;
        var evt = o[Event];
        if (o.GetCtlName() == 'FreeForm') {
            //'FreeForm'�� p1 Ϊ�ӿؼ��� id
            var sub = o.get(p1);
            if (sub) {
                evt = sub[Event];
                o = sub;
            }
        }

        if(typeof evt == 'function')
            evt.call(o, p1, p2, p3, p4);
    }
}
//#endregion

//#region ��Ext����չ
