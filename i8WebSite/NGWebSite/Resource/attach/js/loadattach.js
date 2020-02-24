var LoadAttach = {
    GUID: String,
    STATUS: String,

    idGuid: Object,
    idStatus: Object,

    FileSrvUrl: String,
    OptGuid: String,
    OptMode: String,
    OpenByMianFrame: String,
    closeNG3Container: Function,
    PopupDiv: String,
    AttachIframe: String,
    PureWeb: String,//是否特变纯web版本 1 是 0 否
    Oper: String,
    AsrTable: String,//业务表名
    AsrWidth: Number, //NG3附件框宽度
    Win: Object, //记录被这个弹窗隐藏的金格控件
    Language: String,//语言
    UCode: String,//账套
    LangInfo:Object,//多语言列表

    //iframe_attach:Object,

    //功能描述：初始化
    Init: function (opt) {
        //初始化前校验
        if (!LoadAttach.CheckParams(opt)) { return; }

        LoadAttach.PureWeb = "0";
        LoadAttach.OptGuid = opt.guid || "";
        LoadAttach.OptMode = opt.mode || "";
        LoadAttach.OpenByMianFrame = opt.openbymianframe || "0";
        LoadAttach.GUID = "";
        LoadAttach.STATUS = "";
        LoadAttach.Oper = opt.oper || "";
        LoadAttach.AsrTable = opt.tbl || "";
        LoadAttach.AsrWidth = opt.asrwidth || "706";
        LoadAttach.Language = opt.language || "";
        LoadAttach.UCode = opt.ucode || "";
        LoadAttach.LangInfo = {};
        
        try {
            var producttmp = window.external.GetProduct();

            if (producttmp) {
                opt.product = producttmp;
            }
        }
        catch (ex) {
            opt.product = LoadAttach.GetProduct();
            if (LoadAttach.OpenByMianFrame == "1") {
                LoadAttach.PureWeb = "1";
            }
        }

        if (LoadAttach.OptMode == "NG3") {
            LoadAttach.closeNG3Container = opt.closeContainer;
        }
        else {
            LoadAttach.SetValue(LoadAttach.OptGuid + "&guid", "");
            LoadAttach.SetValue(LoadAttach.OptGuid + "&status", "");
        }

        LoadAttach.idGuid = document.getElementById("idGuid") || null;
        LoadAttach.idStatus = document.getElementById("idStatus") || null;

        LoadAttach.FileSrvUrl = LoadAttach.GetFileSrvUrl(opt);        

        var url = LoadAttach.GetClientUrl(opt);
        opt.src = url;

        if (LoadAttach.PureWeb == "0") {
            LoadAttach.LoadClient(opt);
        }
        else {
            LoadAttach.PopupDiv = "iframewapper";
            LoadAttach.AttachIframe = "asr_iframe";
            LoadAttach.ShowAttach(url, opt.product);
        }
    },

    //功能描述：校验参数
    CheckParams: function (opt) {
        if (opt == null) {
            var msg1 = LoadAttach.fnLangGet('loadattachmsg1');
            alert(msg1 + "!");
            //alert("参数未传递!");
            return false;
        }

        if (opt.openbymianframe != "1") {
            if (document.getElementById("iframewapper") == null) {
                var msg2 = LoadAttach.fnLangGet('loadattachmsg2');
                alert(msg2 + "!");
                //alert("容器对象不存在!");
                return false;
            }
        }

        //打开方式
        if ((opt.oper || "") == "") {
            var msg3 = LoadAttach.fnLangGet('loadattachmsg3');
            alert(msg3 + "!");
            //alert("参数oper未传递!");
            return false;
        }

        //会话GUID
        if ((opt.guid || "") == "") {
            var msg4 = LoadAttach.fnLangGet('loadattachmsg4');
            alert(msg4 + "!");
            //alert("参数guid未传递!");
            return false;
        }

        return true;
    },

    //功能描述：获取附件站点URL
    GetFileSrvUrl: function (opt) {
        var href = window.location.href;
        var pathname = window.location.pathname || "";
        var host_url = href.replace(pathname, "").replace(/\?.*/, "");
        var filrsrv_url = host_url + "/" + (opt.product || "I6p") + "FileSrv/";

        return filrsrv_url;
    },

    //功能描述：获取附件站点URL
    GetProductUrl: function (product) {
        var href = window.location.href;
        var pathname = window.location.pathname || "";
        var host_url = href.replace(pathname, "").replace(/\?.*/, "");
        var filrsrv_url = host_url + "/" + (product || "I6p") + "/";

        return filrsrv_url;
    },

    //功能描述：获取产品
    GetProduct: function () {
        var product = '';
        var pathname = window.location.pathname || "";

        if (LoadAttach.StartWith(pathname, '/')) {

        }
        else {
            pathname = "/" + pathname;
        }

        if (pathname.toLowerCase().indexOf('/i8/') >= 0) {
            product = "i8";
        }
        else if (pathname.toLowerCase().indexOf('/i6/') >= 0) {
            product = "i6";
        }
        else if (pathname.toLowerCase().indexOf('/i6s/') >= 0) {
            product = "i6s";
        }
        else if (pathname.toLowerCase().indexOf('/a3/') >= 0) {
            product = "a3";
        }
        else if (pathname.toLowerCase().indexOf('/ge/') >= 0) {
            product = "ge";
        }
        else if (pathname.toLowerCase().indexOf('/mini/') >= 0) {
            product = "mini";
        }
        else if (pathname.toLowerCase().indexOf('/g6h/') >= 0) {
            product = "g6h";
        }
        else {
            product = 'i8';
        }
        //if (pathname.startsWith('/'))
        //{
        //    if (pathname.length > 1) {
        //        pathname.substring(1, pathname.length - 1);
        //    }
        //}

        //var index = pathname.indexOf('/');
        //if (index > 0)
        //{
        //    pathname = pathname.substring

        //}        

        return product;
    },

    StartWith: function (sourcestr, startstr) {
        if (startstr == null || startstr == "" || sourcestr.length == 0 || startstr.length > sourcestr.length) {
            return false;
        }

        if (sourcestr.substring(0, startstr.length) == startstr) {
            return true;
        }
        else {
            return false;
        }
    },

    EndWith: function (sourcestr, startstr) {
        if (startstr == null || startstr == "" || sourcestr.length == 0 || startstr.length > sourcestr.length) {
            return false;
        }

        if (sourcestr.substring(sourcestr.length - startstr.length, sourcestr.length) == startstr) {
            return true;
        }
        else {
            return false;
        }
    },

    //功能描述：获取附件控件URL
    GetClientUrl: function (opt) {
        var url = LoadAttach.FileSrvUrl + "NGClient.aspx";
        url += "?oper=" + (opt.oper || "");
        url += "&mode=" + (opt.mode || "");
        url += "&product=" + (opt.product || "I6p");
        url += "&fill=" + (opt.fill || "");
        url += "&fillname=" + (opt.fillname || "");
        url += "&chk=" + (opt.chkSign || "0");
        url += (opt.chkCheckIn || "0");
        url += "&btn=" + (opt.btnAdd || "0");
        url += (opt.btnScan || "0");
        url += (opt.btnDelete || "0");
        url += (opt.btnEdit || "0");
        url += (opt.btnView || "0");
        //if (LoadAttach.PureWeb == "1") {
        //    url += "0";
        //}
        //else {
        //    url += (opt.btnView || "0");
        //}

        url += (opt.btnDownload || "0");
        url += (opt.btnOk || "0");
        url += (opt.btnWebAdd || "0");
        url += (opt.btnCancel || "0");

        if (!opt.btnWebOk && opt.btnWebOk != "0") {
            url += ("2");
        }
        else {
            url += (opt.btnWebOk);
        }

        url += "&status=" + (opt.status || "view");
        url += "&showlist=" + (opt.showlist || "1");
        url += "&showstatus=" + (opt.showstatus || "1");
        url += "&showborder=" + (opt.showborder || "1");
        url += "&guid=" + (opt.guid || "");
        url += "&zip=" + (opt.zip || "1");
        url += "&maxheight=" + (opt.maxheight || "0");
        url += "&archivestuts=" + (opt.archivestuts || "2");
        url += "&addserverstuts=" + (opt.addserverstuts || "2");
        url += "&templatestuts=" + (opt.templatestuts || "2");
        url += "&autosave=" + (opt.autosave || "0");

        if (opt.filenum) {
            url += "&filenum=" + (opt.filenum || "0");
        }
        if (opt.filesize) {
            url += "&filesize=" + opt.filesize;
        }

        if (opt.filetype) {
            url += "&fileType=" + opt.filetype;
        }

        url += "&pureweb=" + LoadAttach.PureWeb;
        url += "&dkbtnstuts=" + (opt.dkbtnstuts || "0"); //项目文档、项目知识、企业文档和维基百科按钮

        if (opt.addexstatus) {
            url += "&addexstatus=" + (opt.addexstatus || "1");//新增下拉按钮是否显示 1显示 0隐藏
        }

        if (opt.tscolstatus) {
            url += "&tscolstatus=" + (opt.tscolstatus || "1");//标签、共享方式和共享给列是否显示 1显示 0隐藏
        }

        LoadAttach.fnCurLanguageUCodeGet();
        
        if (LoadAttach.PureWeb == "1") { //纯web
            if (opt.selpath) {
                var filepath = encodeURIComponent(opt.selpath);
                url += "&selpath=" + filepath;//文件选择框打开时定位的文件目录
            }

            if ((LoadAttach.Language || "") == "") {

            }

            if ((LoadAttach.UCode || "") == "") {

            }

        }
        else {
            if (opt.selpath) {
                url += "&selpath=" + (opt.selpath || "");//文件选择框打开时定位的文件目录
            }

            if ((LoadAttach.Language || "") == "") {
                try {
                    LoadAttach.Language = window.external.GetLanguage();
                } catch (ex) {
                }
            }

            if ((LoadAttach.UCode || "") == "") {
                try {
                    LoadAttach.UCode = window.external.GetUCode();
                } catch(ex) {
                }
            }            
        }

        url += "&language=" + (LoadAttach.Language || "");
        url += "&ucode=" + (LoadAttach.UCode || "");

        return url;
    },

    //功能描述：获取当前语言和账套
    fnCurLanguageUCodeGet: function () {

        var curwindow = window;
        var finded = false;
        var i = 0;
        while (!finded) {
            i += 1;

            if (i == 10) {
                break;
            }

            try {
                var windowtmp = curwindow;

                if (!LoadAttach.fnIsEmpty(windowtmp.C_Language) && !LoadAttach.fnIsEmpty(windowtmp.C_DataBase)) {
                    LoadAttach.Language = windowtmp.C_Language;
                    LoadAttach.UCode = windowtmp.C_DataBase;
                    finded = true;
                }
                else {
                    curwindow = curwindow.parent;
                }
            }
            catch (e) {
                curwindow = curwindow.parent;
                //window.open(url);
            }
        }        
    },

    //功能描述：判断是否为空
    fnIsEmpty: function (value) {
        if (value == null || value == "" || value == "undefined" || value.length == 0) {
            return true;
        }
        else {
            return false;
        }
    },

    //功能描述：判断Object是否为空
    fnIsEmptyObj: function (obj) {
        for (var key in obj)
        {
            if (key)
            {
                return false;
            }
        }

        return true;
    },

    //功能描述：装载附件客户端
    LoadClient: function (opt) {
        if (LoadAttach.OpenByMianFrame == "1") {
            //NG3页面通过主框架打开附件            
            var msg = escape("formopen@@**" + opt.src);
            window.external.NG3Attach(msg);
        }
        else {
            var iframe = document.getElementById("asr_iframe");
            var iframewapper = document.getElementById("iframewapper");
            if (iframe) {
                iframe.src = opt.src || "";
                iframe.width = "100%";
                iframe.height = "100%";
            } else {
                iframe = document.createElement("iframe");
                iframe.id = "asr_iframe";
                iframe.src = opt.src || "";
                iframe.width = "100%";
                iframe.height = "100%";
                iframe.setAttribute("frameborder", "no");
                iframe.setAttribute("border", "0");
                iframe.setAttribute("marginwidth", "0");
                iframe.setAttribute("marginheight", "0");
                iframe.setAttribute("scrolling", "no");
                iframe.setAttribute("allowtransparency", "yes");
                iframewapper.appendChild(iframe);
            }

            var height = opt.height || 0;
            if (height > 0) {
                iframe.height = opt.height;
            }
        }
    },

    //功能描述：接收返回值
    ReturnValue: function (key, value) {

        if (LoadAttach.PureWeb == "1") {
            try {
                var valuetmp = decodeURI(value);
                AttachReturnValue(key, valuetmp);
            }
            catch (e) {
            }

            if (key == "closeNG3Container") {
                var Idiv = document.getElementById(LoadAttach.PopupDiv);
                Idiv.style.display = "none";

                var body = document.getElementsByTagName("body");
                var mybg = document.getElementById("ngattachbg");

                body[0].removeChild(mybg);

                try {
                    $winBeforeClose(LoadAttach.Win, null); //报表页面楼工控件处理
                }
                catch (e) {
                }
            }
        }
        else {
            switch (key) {
                case "guid":
                    LoadAttach.GUID = value;
                    if (LoadAttach.idGuid != null) {
                        LoadAttach.idGuid.value = value;
                    }
                    break;
                case "status":
                    LoadAttach.STATUS = value;
                    if (LoadAttach.idStatus != null) {
                        LoadAttach.idStatus.value = value;
                    }
                    break;
                case "del":
                    alert(value);
                case "closeNG3Container":
                    LoadAttach.closeNG3Container();
                    break;
            }
        }
    },

    //功能描述：发送消息到附件控件
    SendMsg: function (msg) {
        if (LoadAttach.OpenByMianFrame == "1") {
            //NG3页面通过主框架打开附件
            window.external.NG3Attach(msg);
        }
        else {
            var iframe = document.getElementById("proxy_iframe");
            if (iframe && iframe.parentNode) {
                iframe.parentNode.removeChild(iframe);
            }

            iframe = document.createElement("iframe");
            iframe.id = "proxy_iframe";
            iframe.width = "0";
            iframe.height = "0";
            iframe.src = LoadAttach.FileSrvUrl + "proxy_filesrv.htm#" + msg;
            iframe.style.display = "none";
            iframe.setAttribute("border", "no");
            iframe.setAttribute("border", "0");
            iframe.setAttribute("marginwidth", "0");
            iframe.setAttribute("marginheight", "0");
            iframe.setAttribute("scrolling", "0");
            iframe.setAttribute("allowtransparency", "yes");
            document.body.appendChild(iframe);
        }
    },

    //功能描述：读取消息
    GetValue: function (key) {
        var result = "-1";

        try {
            result = window.external.GetValue(LoadAttach.OptGuid + "&" + key);
        } catch (ex) {
            alert(ex.message);
        }

        return result;
    },

    //功能描述: 写入消息
    SetValue: function (key, value) {
        try {
            window.external.SetValue(key, value);
        } catch (ex) {
            //输出调试信息
            //alert(ex.message);
        }
    },

    //功能描述：获取状态
    GetStatus: function () {
        var status = "";
        if (LoadAttach.OptMode == "test" || LoadAttach.OptMode == "NG3") {
            status = LoadAttach.STATUS;
        } else {
            status = LoadAttach.GetValue("status");
        }
        return status;
    },

    //功能描述：获取GUID
    GetGuid: function () {
        var guid = "";
        if (LoadAttach.OptMode == "test" || LoadAttach.OptMode == "NG3") {
            guid = LoadAttach.GUID;
        } else {
            guid = LoadAttach.GetValue("guid");
        }
        return guid;
    },

    //功能描述：附件初始化
    InitBeforeOpen: function (param) {
        var result = "";
        var bustypecode = "";
        var orgid = "0";
        var busurl = "";
        var phidsfilter = "";

        try {
            var producttmp = window.external.GetProduct();

            if (producttmp) {
                param.product = producttmp;
            }
        }
        catch (ex) {

            param.product = LoadAttach.GetProduct();
        }

        try {
            bustypecode = param.bustypecode;
        }
        catch (ex) {
        }

        if (bustypecode == null) {
            bustypecode = "";
        }

        try {
            orgid = param.orgid;
        }
        catch (ex) {
        }

        if (orgid == null) {
            orgid = "0";
        }

        try {
            busurl = param.busurl;
        }
        catch (ex) {
        }

        if (busurl == null) {
            busurl = "";
        }

        try {
            phidsfilter = param.phidsfilter;
        }
        catch (ex) {
        }

        if (phidsfilter == null) {
            phidsfilter = "";
        }


        var url = LoadAttach.GetProductUrl(param.product) + 'SUP/Attachment/AttachmentInit';

        LoadAttach.CreateAjax(url, "&attachguid=" + param.attachguid + "&attachTName=" + param.attachTName + "&busTName=" + param.busTName + "&busid=" + param.busid + "&bustypecode=" + bustypecode + "&busurl=" + escape(busurl) + "&orgid=" + orgid + "&phidsfilter=" + escape(phidsfilter), function (data) {
            result = data;
        });

        if ((result || "") == "") {
            result = "{Status:\"error\",msg:\"\"}";
        }

        result = "(" + result + ")";
        return result;
    },

    //功能描述：附件保存
    Save: function (product, attachguid, buscode) {

        var result = "";
        try {
            var producttmp = window.external.GetProduct();

            if (producttmp) {
                product = producttmp;
            }
        }
        catch (ex) {
            product = LoadAttach.GetProduct();
        }
        var url = LoadAttach.GetProductUrl(product) + 'SUP/Attachment/Save';

        LoadAttach.CreateAjax(url, "&attachguid=" + attachguid + "&buscode=" + buscode, function (data) {
            result = data;
        });

        if ((result || "") == "") {
            result = "{Status:\"error\",msg:\"\"}";
        }

        result = "(" + result + ")";

        return result;
    },

    //功能描述：清空附件缓存
    ClearTempData: function (product, attachguid) {
        var result = "";

        try {
            var producttmp = window.external.GetProduct();

            if (producttmp) {
                product = producttmp;
            }
        }
        catch (ex) {
            product = LoadAttach.GetProduct();
        }

        var url = LoadAttach.GetProductUrl(product) + 'SUP/Attachment/ClearTempData';

        LoadAttach.CreateAjax(url, "&attachguid=" + attachguid, function (data) {
            result = data;
        });

        if ((result || "") == "") {
            result = "{Status:\"error\",msg:\"\"}";
        }

        result = "(" + result + ")";

        return result;
    },

    //功能描述：获取附件信息
    GetAttachmentInfo: function (product, attachguid) {
        var result = "";

        try {
            var producttmp = window.external.GetProduct();

            if (producttmp) {
                product = producttmp;
            }
        }
        catch (ex) {
            product = LoadAttach.GetProduct();
        }

        var url = LoadAttach.GetProductUrl(product) + 'SUP/Attachment/GetAttachmentInfo';

        LoadAttach.CreateAjax(url, "&guid=" + attachguid, function (data) {
            result = data;
        });

        if ((result || "") == "") {
            result = "{Status:\"error\",msg:\"\"}";
        }

        result = "(" + result + ")";

        return result;
    },

    //功能描述：创建Ajax Post异步请求
    CreateAjax: function (url, params, fn) {
        var ajax = LoadAttach.GetXmlRequest();

        url = url + "?d=" + (new Date()).toString();

        //ajax.open
        ajax.open("POST", url, false);

        //ajax.setRequestHeader 定义传输的文件HTTP头信息
        ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        //ajax.onreadystatechange
        ajax.onreadystatechange = function () {
            if (ajax.readyState == 4 && ajax.status == 200) {
                var data = ajax.responseText.toString();
                fn(data);
            }
        }

        //ajax.send
        ajax.send(params);
    },

    //功能描述：创建XMLHttpRequest
    GetXmlRequest: function () {
        var xmlRequest;

        try {
            xmlRequest = new XMLHttpRequest();
        } catch (e) {

            try {
                xmlRequest = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {

                try {
                    xmlRequest = new ActiveXObject("Micorsoft.XMLHTTP");
                } catch (e) {
                    xmlRequest = false;
                }
            }
        }

        if (!xmlRequest) {
            var msg = LoadAttach.fnLangGet('loadattachmsg5');
            alert(msg + "XMLHTTPRequest!");
            //alert("当前浏览器不支持XMLHTTPRequest!");
        }

        return xmlRequest;
    },

    //功能描述：关闭附件框
    CloseAttach: function () {
        var iframe_attach = document.getElementById(LoadAttach.AttachIframe);
        var filelist = iframe_attach.contentWindow.GetAttachFileList();

        try {
            AttachReturnValue("closeNG3Container", filelist);
        }
        catch (e) {
        }

        var Idiv = document.getElementById(LoadAttach.PopupDiv);
        Idiv.style.display = "none";

        var body = document.getElementsByTagName("body");
        var mybg = document.getElementById("ngattachbg");

        body[0].removeChild(mybg);

        try {
            $winBeforeClose(LoadAttach.Win, null); //报表页面楼工控件处理
        }
        catch (e)
        { }
    },

    //功能描述：打开附件框
    ShowAttach: function (url, product) {
        try {
            $winBeforeShow(LoadAttach.Win, null); //报表页面楼工控件处理
        }
        catch (e)
        { }
        var Idiv = document.getElementById(LoadAttach.PopupDiv);
        var iframe_attach = document.getElementById(LoadAttach.AttachIframe);

        var imgattach = document.getElementById("ngattachimgdiv");
        var attachtext = document.getElementById("ngattachtextdiv");
        var imgclose = document.getElementById("ngattachclosediv");
        var filelistwidth = LoadAttach.AsrWidth - 18;


        if (Idiv == null) {
            Idiv = document.createElement("div");
            Idiv.id = "iframewapper";
            Idiv.style.borderRadius = "5px";
            Idiv.style.width = LoadAttach.AsrWidth + "px";
            Idiv.style.height = "422px";
            Idiv.style.display = "block";
            Idiv.style.position = "absolute";
            Idiv.style.zIndex = "100000";
            Idiv.style.background = "#97c5f0";//"#E4E4E4";
            //Idiv.style.border = "2px solid #ffffff";
            //Idiv.style.background = "#ffffff";
            //Idiv.style.backgroundImage = "url(../../pic/product.png)";
            document.body.appendChild(Idiv);

            var backgroundiframe = document.createElement("iframe");
            backgroundiframe.id = "backgroundiframe";
            backgroundiframe.frameBorder = "0";
            backgroundiframe.scrolling = "no";
            backgroundiframe.style.backgroundColor = "transparent";
            backgroundiframe.style.position = "absolute";
            backgroundiframe.style.zIndex = "-1";
            backgroundiframe.style.width = "100%";
            backgroundiframe.style.height = "100%";
            backgroundiframe.style.top = "0px";
            backgroundiframe.style.left = "0px";
            Idiv.appendChild(backgroundiframe);

            imgattach = document.createElement("div");
            imgattach.id = "ngattachimgdiv";
            imgattach.style.width = "8px";
            imgattach.style.height = "25px";
            imgattach.style.display = "block";
            imgattach.style.position = "absolute";
            imgattach.style.zIndex = "100000";
            //imgattach.style.whiteSpace = "nowrap";
            //imgattach.style.verticalAlign = "middle";
            imgattach.innerHTML = '<img id="ngattachimg" src="' + LoadAttach.GetProductUrl(product) + 'RESOURCE/attach/image/attach.png" style="width: 8px; height: 14px;margin-left:10px;margin-top:6px;"/>';
            Idiv.appendChild(imgattach);

            var uploadfiletitle = LoadAttach.fnLangGet('uploadfiletitle');

            attachtext = document.createElement("div");
            attachtext.id = "ngattachtextdiv";
            attachtext.style.width = "40px";//25
            attachtext.style.height = "25px";
            attachtext.style.display = "block";
            attachtext.style.position = "absolute";
            attachtext.style.zIndex = "100000";
            attachtext.style.marginLeft = "25px";
            attachtext.style.paddingTop = "4px";//7
            attachtext.innerHTML = uploadfiletitle;//'附件';
            Idiv.appendChild(attachtext);

            imgclose = document.createElement("div");
            imgclose.id = "ngattachclosediv";
            imgclose.style.width = "10px";
            imgclose.style.height = "25px";
            imgclose.style.display = "block";
            imgclose.style.position = "absolute";
            imgclose.style.zIndex = "100000";
            imgclose.innerHTML = '<img id="ngattachimg" onclick="LoadAttach.CloseAttach();" src="' + LoadAttach.GetProductUrl(product) + 'RESOURCE/attach/image/close.png" style="width: 10px; height: 8px;margin-left:' + filelistwidth + 'px;margin-top:8px;"/>';
            Idiv.appendChild(imgclose);

            iframe_attach = document.createElement("iframe");
            iframe_attach.id = "asr_iframe";
            Idiv.appendChild(iframe_attach);
            //('<div id="iframewapper" style=" width:900px;height:360px;display: none; 
            //position: absolute;z-index: 1000;border: 2px solid #ffffff;background: #ffffff;">
            //<iframe id="asr_iframe"></iframe></div>');
        }
        else {
            Idiv.style.display = "block";
        }

        var divleft = (document.documentElement.clientWidth - Idiv.clientWidth) / 2 + document.documentElement.scrollLeft + "px";
        var divtop = (document.documentElement.clientHeight - Idiv.clientHeight) / 2 + document.documentElement.scrollTop + 32;

        if (divtop > 70 && document.documentElement.scrollTop <= 0) {
            divtop = 70;
        }

        divtop += "px";

        //以下部分要将弹出层居中显示 
        Idiv.style.left = divleft;
        Idiv.style.top = divtop;

        //附件图片
        imgattach.style.left = "0px";
        imgattach.style.top = "0px";

        //附件文字
        attachtext.style.left = "0px";
        attachtext.style.top = "0px";

        //附件关闭图片
        imgclose.style.left = "0px";
        imgclose.style.top = "0px";


        //以下部分使整个页面至灰不可点击 
        var procbg = document.createElement("div"); //首先创建一个div 
        procbg.setAttribute("id", "ngattachbg"); //定义该div的id 
        procbg.style.backgroundColor = "rgba(255,255,255,0.5)";
        procbg.style.width = "100%";
        procbg.style.height = "100%";
        procbg.style.position = "fixed";
        procbg.style.top = "0";
        procbg.style.left = "0";
        procbg.style.zIndex = "500";
        procbg.style.opacity = "0.6";
        procbg.style.filter = "Alpha(opacity=70)";

        if (iframe_attach == null) {
            iframe_attach = document.createElement("iframe");
            iframe_attach.id = "asr_iframe";
            iframe_attach.width = filelistwidth + "px";
            iframe_attach.height = "386px";
            iframe_attach.style.marginLeft = "9px";
            iframe_attach.style.marginTop = "25px";
            iframe_attach.style.marginRight = "9px";
            iframe_attach.style.marginBottom = "9px";
            iframe_attach.setAttribute("frameborder", "no");
            iframe_attach.setAttribute("border", "0");
            iframe_attach.setAttribute("marginwidth", "0");
            iframe_attach.setAttribute("marginheight", "0");
            iframe_attach.setAttribute("scrolling", "no");
            iframe_attach.setAttribute("allowtransparency", "yes");
            iframe_attach.src = url;
            Idiv.appendChild(iframe_attach);
        }
        else {
            iframe_attach.width = filelistwidth + "px";
            iframe_attach.height = "386px";
            iframe_attach.style.marginLeft = "9px";
            iframe_attach.style.marginTop = "25px";
            iframe_attach.style.marginRight = "9px";
            iframe_attach.style.marginBottom = "9px";
            iframe_attach.setAttribute("frameborder", "no");
            iframe_attach.setAttribute("border", "0");
            iframe_attach.setAttribute("marginwidth", "0");
            iframe_attach.setAttribute("marginheight", "0");
            iframe_attach.setAttribute("scrolling", "no");
            iframe_attach.setAttribute("allowtransparency", "yes");
            iframe_attach.src = url;
        }

        //以上部分也可以用csstext代替 
        // procbg.style.cssText="background:#000000;width:100%;height:100%;position:fixed;top:0;left:0;zIndex:500;opacity:0.6;filter:Alpha(opacity=70);"; 

        //背景层加入页面 
        document.body.appendChild(procbg);
        //document.body.style.overflow = "hidden"; //取消滚动条 

        //以下部分实现弹出层的拖拽效果 
        //LoadAttach.DragFunc(LoadAttach.PopupDiv);
        var posX;
        var posY;

        Idiv.onmousedown = function (e) {
            if (!e) e = window.event; //IE 
            posX = e.clientX - parseInt(Idiv.style.left);
            posY = e.clientY - parseInt(Idiv.style.top);
            document.onmousemove = mousemove;
        }

        document.onmouseup = function () {
            document.onmousemove = null;
        }

        function mousemove(ev) {
            if (ev == null) ev = window.event; //IE 
            Idiv.style.left = (ev.clientX - posX) + "px";
            Idiv.style.top = (ev.clientY - posY) + "px";
        }

    },

    //通用帮助相关函数
    ReturnCommHelpDataForAttach: function (helptype, data) {
        var iframe_attach = document.getElementById("asr_iframe");

        if (iframe_attach && helptype != 'template') {
            iframe_attach.contentWindow.CommHelpReturn(helptype, data);
        }

        LoadAttach.CloseCommHelpDivForAttach();
    },

    CloseCommHelpDivForAttach: function () {
        var Idiv = document.getElementById("dvAttachCommHelp");

        if (Idiv) {

            var body = document.getElementsByTagName("body");
            body[0].removeChild(Idiv);
            //Idiv.style.display = "none";
        }
    },

    ShowCommHelpDivForAttach: function (helptype, title, width, height, url) {

        var Idiv = document.getElementById("dvAttachCommHelp");
        var iframe_attach = document.getElementById("ifeAttachCommHelp");

        var attachtext = document.getElementById("ngAttachCommHelpTextdiv");
        var imgclose = document.getElementById("ngAttachCommHelpClosediv");

        var divhelpheight = height + 34;
        var divhelpwidth = width + 18;

        if (Idiv == null) {
            var product = LoadAttach.GetProduct();
            var producturl = LoadAttach.GetProductUrl(product);

            Idiv = document.createElement("div");
            Idiv.id = "dvAttachCommHelp";
            Idiv.style.borderRadius = "5px";
            Idiv.style.width = divhelpwidth + "px";
            Idiv.style.height = divhelpheight + "px";
            Idiv.style.display = "block";
            Idiv.style.position = "absolute";
            Idiv.style.zIndex = "9999";
            Idiv.style.background = "#97c5f0"//"#E4E4E4";
            //Idiv.style.border = "2px solid #ffffff";
            //Idiv.style.background = "#ffffff";
            //Idiv.style.backgroundImage = "url(../../pic/product.png)";
            document.body.appendChild(Idiv);

            attachtext = document.createElement("div");
            attachtext.id = "ngAttachCommHelpTextdiv";
            attachtext.style.width = "90px";
            attachtext.style.height = "25px";
            attachtext.style.display = "block";
            attachtext.style.position = "absolute";
            attachtext.style.zIndex = "9999";
            attachtext.style.marginLeft = "10px";
            attachtext.style.paddingTop = "7px";
            attachtext.innerHTML = title;
            Idiv.appendChild(attachtext);

            imgclose = document.createElement("div");
            imgclose.id = "ngAttachCommHelpClosediv";
            imgclose.style.width = "10px";
            imgclose.style.height = "25px";
            imgclose.style.display = "block";
            imgclose.style.position = "absolute";
            imgclose.style.zIndex = "9999";
            imgclose.innerHTML = '<img id="ngattachimg" onclick="LoadAttach.CloseCommHelpDivForAttach();" src="' + producturl + 'RESOURCE/attach/image/close.png" style="width: 10px; height: 8px;margin-left:' + width + 'px;margin-top:8px;"/>';
            Idiv.appendChild(imgclose);

            iframe_attach = document.createElement("iframe");
            iframe_attach.id = "ifeAttachCommHelp";
            iframe_attach.name = "ifeAttachCommHelpName";
            Idiv.appendChild(iframe_attach);
            //('<div id="iframewapper" style=" width:900px;height:360px;display: none; 
            //position: absolute;z-index: 1000;border: 2px solid #ffffff;background: #ffffff;">
            //<iframe id="asr_iframe"></iframe></div>');
        }
        else {
            Idiv.style.display = "block";
        }

        var divleft = 0;
        var divtop = 0;

        if (document.documentElement.clientWidth == 0) {
            divleft = (document.body.clientWidth - Idiv.clientWidth) / 2 + document.body.scrollLeft + "px";
            divtop = (document.body.clientHeight - Idiv.clientHeight) / 2 + document.body.scrollTop + 32;
        }
        else {
            divleft = (document.documentElement.clientWidth - Idiv.clientWidth) / 2 + document.documentElement.scrollLeft + "px";
            divtop = (document.documentElement.clientHeight - Idiv.clientHeight) / 2 + document.documentElement.scrollTop + 32;
        }

        if (helptype.toLowerCase() == "tagshelp" || helptype.toLowerCase() == "alluserhelp" || helptype.toLowerCase() == "depthelp"
            || helptype.toLowerCase() == "projdochelp" || helptype.toLowerCase() == "prowikilisthelp" || helptype.toLowerCase() == "entdochelp"
            || helptype.toLowerCase() == "wikientlisthelp") {
            divtop = 20 + "px";
        } else if (helptype == "template") {
            divtop = 68 + "px";
        }
        else if (helptype == "myuploadfile") {//我的附件
            divtop = 40 + "px";
        }

        //以下部分要将弹出层居中显示 
        Idiv.style.left = divleft;
        Idiv.style.top = divtop;

        //通用帮助名称
        attachtext.style.left = "0px";
        attachtext.style.top = "0px";

        //通用帮助关闭图片
        imgclose.style.left = "0px";
        imgclose.style.top = "0px";

        if (iframe_attach == null) {
            iframe_attach = document.createElement("iframe");
            iframe_attach.id = "ifeAttachCommHelp";
            iframe_attach.name = "ifeAttachCommHelpName";
            iframe_attach.width = width + "px";
            iframe_attach.height = height + "px";
            iframe_attach.style.marginLeft = "9px";
            iframe_attach.style.marginTop = "25px";
            iframe_attach.style.marginRight = "9px";
            iframe_attach.style.marginBottom = "9px";
            iframe_attach.setAttribute("frameborder", "no");
            iframe_attach.setAttribute("border", "0");
            iframe_attach.setAttribute("marginwidth", "0");
            iframe_attach.setAttribute("marginheight", "0");
            iframe_attach.setAttribute("scrolling", "no");
            iframe_attach.setAttribute("allowtransparency", "yes");
            iframe_attach.src = url;
            Idiv.appendChild(iframe_attach);
        }
        else {
            iframe_attach.width = width + "px";
            iframe_attach.height = height + "px";
            iframe_attach.style.marginLeft = "9px";
            iframe_attach.style.marginTop = "25px";
            iframe_attach.style.marginRight = "9px";
            iframe_attach.style.marginBottom = "9px";
            iframe_attach.setAttribute("frameborder", "no");
            iframe_attach.setAttribute("border", "0");
            iframe_attach.setAttribute("marginwidth", "0");
            iframe_attach.setAttribute("marginheight", "0");
            iframe_attach.setAttribute("scrolling", "no");
            iframe_attach.setAttribute("allowtransparency", "yes");
            iframe_attach.src = url;
        }

        //以下部分实现弹出层的拖拽效果 
        //LoadAttach.DragFunc('dvAttachCommHelp');

        //以下部分实现弹出层的拖拽效果 
        var posX;
        var posY;

        Idiv.onmousedown = function (e) {
            if (!e) e = window.event; //IE 
            posX = e.clientX - parseInt(Idiv.style.left);
            posY = e.clientY - parseInt(Idiv.style.top);
            document.onmousemove = mousemove;
        }

        document.onmouseup = function () {
            document.onmousemove = null;
        }

        function mousemove(ev) {
            if (ev == null) ev = window.event; //IE 
            Idiv.style.left = (ev.clientX - posX) + "px";
            Idiv.style.top = (ev.clientY - posY) + "px";
        }

    },

    //div任意拖动
    DragFunc: function (id) {
        var drag = document.getElementById(id);
        drag.onmousedown = function (event) {
            var ev = event || window.event;
            event.stopPropagation();
            var disX = EV.clientX - drag.offsetLeft;
            var disY = EV.clientY - drag.offsetTop;

            document.onmousemove = function (event) {
                var ev = event || window.event;
                drag.style.left = ev.clientX - disX + "px";
                drag.style.top = ev.clientY - disY + "px";
                drag.style.cursor = "move";
            };
        };
        drag.onmouseup = function () {
            document.onmousemove = null;
            this.style.cursor = "default";
        }
    },

    //功能描述：获取多语言
    fnLangGet: function (langkey) {
        var langvalue = '';
        var curwindow = window;
        var finded = false;
        var i = 0;

        if (LoadAttach.fnIsEmptyObj(LoadAttach.LangInfo)) {
            while (!finded) {
                i += 1;

                if (i == 10) {
                    break;
                }

                try {
                    var windowtmp = curwindow;

                    if (!LoadAttach.fnIsEmpty(windowtmp.langInfo)) {
                        LoadAttach.LangInfo = eval("[" + windowtmp.langInfo + "]");

                        if (LoadAttach.LangInfo.length > 0)
                        {
                            finded = true;
                        }                        
                    }
                    else {
                        curwindow = curwindow.parent;
                    }
                }
                catch (e) {
                    curwindow = curwindow.parent;
                    //window.open(url);
                }
            }
        }
        else {
            finded = true;
        }

        if (finded) {
            switch (langkey) {
                case "uploadfiletitle":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].uploadfiletitle, "附件");
                    break;
                case "loadattachmsg1":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].loadattachmsg1, "参数未传递");
                    break;
                case "loadattachmsg2":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].loadattachmsg2, "容器对象不存在");
                    break;
                case "loadattachmsg3":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].loadattachmsg3, "参数oper未传递");
                    break;
                case "loadattachmsg4":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].loadattachmsg4, "参数guid未传递");
                    break;
                case "loadattachmsg5":
                    langvalue = LoadAttach.fnLangReplace(LoadAttach.LangInfo[0].loadattachmsg5, "当前浏览器不支持");
                    break;
            }
        }
        else {
            var newvalue = '';
            try {
                newvalue = window.external.LangGet(langkey);
            } catch (e) {

            }

            switch (langkey) {
                case "uploadfiletitle":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "附件");
                    break;
                case "loadattachmsg1":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "参数未传递");
                    break;
                case "loadattachmsg2":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "容器对象不存在");
                    break;
                case "loadattachmsg3":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "参数oper未传递");
                    break;
                case "loadattachmsg4":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "参数guid未传递");
                    break;
                case "loadattachmsg5":
                    langvalue = LoadAttach.fnLangReplace(newvalue, "当前浏览器不支持");
                    break;
            }
        }

        return langvalue;
    },

    fnLangReplace: function (newvalue, oldvalue) {

        if (newvalue == null || newvalue == "" || newvalue == "undefined" || newvalue.length == 0) {
            return oldvalue;
        }
        else {
            return newvalue;
        }

    }
}


