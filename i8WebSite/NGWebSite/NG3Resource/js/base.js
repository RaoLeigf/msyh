var $WinMgr = new Ext.WindowGroup();
//#region Common Function
function $$(o) {
    return Ext.getDom(o);
}

function $url(url) {
    //将 Ajax.request 的 url 中 @ 替换成 当前页的名称
    if (url.charAt(0) == '@') {
        url = $PageName + url.substr(1);
    };

    return url;
}

function $path(path) {
    //将 path 中 @ 替换成 当前程序路径
    if (path.indexOf("~/") == 0) {
        path = C_ROOT + path.substr(2);
    };

    return path;
}

function $mask(b, winCmp) {
    var arr = [];
    if (b) {
        findAllObj(window);
        if (winCmp) winCmp.objArr = arr;
        for (var i = 0; i < arr.length; i++) {
            var o = arr[i];
            if (o == null) continue;
            try {
                if (o.codeBase.indexOf("supcan") >= 0) {
                    if (o.isGrayWindow) {
                        arr[i] = null;
                    }
                    else {
                        o.func("GrayWindow", "true \r\n 220")
                        o.isGrayWindow = true;
                    }
                }
                else
                    o.style.visibility = 'hidden';
            } catch (e) { }
        }
    }
    else {
        arr = (winCmp && winCmp.objArr) ? winCmp.objArr : findAllObj(window);
        for (var i = 0; i < arr.length; i++) {
            var o = arr[i];
            if (o == null) continue;
            try {
                if (o.codeBase.indexOf("supcan") >= 0) {
                    if (o.isGrayWindow) {
                        o.func("GrayWindow", "false")
                        o.isGrayWindow = false;
                    }
                }
                else
                    o.style.visibility = 'visible';
            } catch (e) { }
        }
    }

    return arr;

    function findAllObj(w) {
        getObjs(w);
        var ifs = w.document.getElementsByTagName("iframe");
        for (var i = 0; i < ifs.length; i++) {
            var f = ifs[i];
            if (f.offsetHeight > 0 && (winCmp == undefined || f.winCmp != winCmp))
                findAllObj(f.contentWindow);
        }
        return arr;

        function getObjs(w) {
            var objs = w.document.getElementsByTagName("object");
            for (var i = 0; i < objs.length; i++) {
                arr.push(objs[i]);
            }
        }
    }
}
//#endregion