///<reference path="../../extjs/ext-all-debug.js">

Ext.define('Ext.ng.Toolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.ngToolbar', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建         
    pageSize: 20,
    itemWidth: 60,
    showArrowBtn: false,//是否显示toolbar最右边的下拉button列表
    rightName: "", //权限名称
    inPopWin:false,//toobar在弹出窗口,独立控制权限
    ngbuttons: [],   //按钮
    //候选按钮
    candidateButtons: [{ itemId: "add", text: "新增", width: this.itemWidth, iconCls: "icon-New", langkey: "add" },
							   { itemId: "edit", text: "修改", width: this.itemWidth, iconCls: "icon-Edit", langkey: "edit" },
							   { itemId: "delete", text: "删除", width: this.itemWidth, iconCls: "icon-delete", langkey: "delete" },
							   { itemId: "view", text: "查看", width: this.itemWidth, iconCls: "icon-View", langkey: "view" },
							   { itemId: "save", text: "保存", width: this.itemWidth, iconCls: "icon-save", langkey: "save" },
							   { itemId: "query", text: "查询", width: this.itemWidth, iconCls: "icon-Query", langkey: "query" },
							   { itemId: "refresh", text: "刷新", width: this.itemWidth, iconCls: "icon-Refresh", langkey: "refresh" },
							   { itemId: "clear", text: "清空", width: this.itemWidth, iconCls: "icon-Clear", langkey: "clear" },
							   { itemId: "copy", text: "复制", width: this.itemWidth, iconCls: "icon-Copy", langkey: "copy" },
							   { itemId: "import", text: "导入", width: this.itemWidth, iconCls: "icon-Import", langkey: "import" },
							   { itemId: "export", text: "导出", width: this.itemWidth, iconCls: "icon-Export", langkey: "export" },
							   { itemId: "create", text: "生成", width: this.itemWidth, iconCls: "icon-create", langkey: "create" },
							   { itemId: "verify", text: "审核", width: this.itemWidth, iconCls: "icon-Verify", langkey: "verify" },
							   { itemId: "valid", text: "审批", width: this.itemWidth, iconCls: "icon-Valid", langkey: "valid" },
							   { itemId: "unvalid", text: "去审批", width: this.itemWidth, iconCls: "icon-Unvalid", langkey: "unvalid" },
							   { itemId: "addrow", text: "增行", width: this.itemWidth, iconCls: "icon-AddRow", langkey: "addrow" },
							   { itemId: "deleterow", text: "删行", width: this.itemWidth, iconCls: "icon-DeleteRow", langkey: "deleterow" },
							   { itemId: "assign", text: "分配", width: this.itemWidth, iconCls: "icon-Assign", langkey: "assign" },
							   { itemId: "config", text: "配置", width: this.itemWidth, iconCls: "icon-Setup", langkey: "config" },
							   { itemId: "compute", text: "计算", width: this.itemWidth, iconCls: "icon-Compute", langkey: "compute" },
							   { itemId: "location", text: "定位", width: this.itemWidth, iconCls: "icon-Location", langkey: "location" },
							   { itemId: "subbill", text: "下级业务", width: this.itemWidth, iconCls: "icon-Backbill", langkey: "subbill" },
							   { itemId: "relabill", text: "相关单据", width: this.itemWidth, iconCls: "icon-Relabill", langkey: "relabill" },
							   { itemId: "check", text: "送审", width: this.itemWidth, iconCls: "icon-Check", langkey: "check" },
							   { itemId: "checkview", text: "送审查看", width: this.itemWidth, iconCls: "icon-CheckView", langkey: "checkview" },
							   { itemId: "history", text: "送审追踪", width: this.itemWidth, iconCls: "icon-History", langkey: "history" },
							   { itemId: "ok", text: "确认", width: this.itemWidth, iconCls: "icon-Confirm", langkey: "ok" },
							   { itemId: "cancel", text: "取消", width: this.itemWidth, iconCls: "icon-Cancel", langkey: "cancel" },
							   { itemId: "help", text: "帮助", width: this.itemWidth, iconCls: "icon-Help", langkey: "help" },
							   { itemId: "print", text: "打印", width: this.itemWidth, iconCls: "icon-Print", langkey: "print" },
							   { itemId: "exit", text: "退出", width: this.itemWidth, iconCls: "icon-Exit", langkey: "exit" },
							   { itemId: "back", text: "返回", width: this.itemWidth, iconCls: "icon-back", langkey: "back" },
							   { itemId: "editrow", text: "修改", width: this.itemWidth, iconCls: "icon-EditRow", langkey: "editrow" },
							   { itemId: "first", text: "首", width: this.itemWidth, iconCls: "icon-Firstrec", langkey: "first" },
							   { itemId: "pre", text: "前", width: this.itemWidth, iconCls: "icon-PriorRec", langkey: "pre" },
							   { itemId: "next", text: "后", width: this.itemWidth, iconCls: "icon-NextRec", langkey: "next" },
							   { itemId: "last", text: "尾", width: this.itemWidth, iconCls: "icon-LastRec", langkey: "last" },
							   { itemId: "deal", text: "处理", width: this.itemWidth, iconCls: "icon-Operate", langkey: "deal" },
							   { itemId: "note", text: "记事本", width: this.itemWidth, iconCls: "icon-Note", langkey: "note" },
							   { itemId: "orgselect", text: "组织选择", width: this.itemWidth, iconCls: "icon-Boo", langkey: "orgselect" },
							   { itemId: "addbrother", text: "同级增加", width: this.itemWidth, iconCls: "icon-AddBrother", langkey: "addbrother" },
							   { itemId: "addchild", text: "下级增加", width: this.itemWidth, iconCls: "icon-AddChild", langkey: "addchild" },
							   { itemId: "attachment", text: "附件", width: this.itemWidth, iconCls: "icon-Attachment", langkey: "attachment" },
							   { itemId: "hide", text: "隐藏", width: this.itemWidth, iconCls: "icon-Close", langkey: "hide" },
							   { itemId: "close", text: "关闭", width: this.itemWidth, iconCls: "icon-Close", langkey: "close" }
                               ],
    initComponent: function () {
        var me = this;
        this.border = false;
        if (!me.height) {
            me.height = 26;
            me.minSize = 26;
            me.maxSize = 26;
        }
        //Ext.Toolbar.superclass.initComponent.call(this);
        this.callParent();

        me.addEvents('buttonready'); //toolbar 按钮权限处理完成

        this.on("beforerender", this.beforeRender); //控制权限
        //-------------判断toolbar多语言-------------
        //2017-12-23更新，注释掉这个，直接在findbutton里新增
        //if (typeof ($ToolbarLang) !== "undefined") {
        //    var ToolbarLanghtml = Ext.htmlDecode($ToolbarLang);
        //    var ToolbarLang = Ext.decode(ToolbarLanghtml);
        //    //再判断$ToolbarLang是不是一个空对象
        //    //if (Object.keys(ToolbarLang).length != 0) { //这种写法ie8不支持
        //    if (JSON.stringify(ToolbarLang) != '{}') { //要换成这种写法
        //        for (var i in ToolbarLang) {
        //            var itemstoolbarlength = this.candidateButtons.length;
        //            var itemstoolbar = this.candidateButtons;
        //            for (var j = 0; j < itemstoolbarlength; j++) {
        //                if (i == itemstoolbar[j].langkey) {
        //                    itemstoolbar[j].text = ToolbarLang[i]
        //                }
        //            }
        //        }
        //    }
        //}
        //-------------判断toolbar多语言END-------------
        this.createButton(); //创建按钮   
        
        var clostbtn = this.get('close');
        if (clostbtn) {
            this.get('close').addEvents('beforeclose'); //添加关闭前事件
            this.get('close').on('click', this.closeHanler);
        }
    },
    createButton: function () {
        var me = this;
        var menus = [], pageId = window.location.pathname.replace(/[\/\.]/g, ''), hideBtns = {}, chkflg = true;
        
        if (this.showArrowBtn) {
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/LayoutLog/GetToolBarData',
                async: false,
                params: { PageId: pageId },
                success: function (res, opts) {
                    if (res.responseText.length > 0) {
                        hideBtns = Ext.JSON.decode(Ext.JSON.decode(res.responseText).Value);
                    }
                }
            });
        }
        for (var i = 0; i < this.ngbuttons.length; i++) {
            chkflg = true;
            var button = this.ngbuttons[i];
            var tmpBtn;
            if (button === '->') {
                this.add({ xtype: 'tbfill' });
                menus.push({ xtype: 'menuseparator' });
                continue;
            }
            else if (button === '-') {
                this.add({ xtype: 'tbseparator' });
                menus.push({ xtype: 'menuseparator' });
                continue;
            }

            var stdbutton;
            if (typeof (button) == "object") {

                if (button.groupitem) {//是下拉分组
                    var menu = Ext.create('Ext.menu.Menu', {});
                    var mu = [];
                    for (var j = 0; j < button.items.length; j++) {
                        chkflg = true;
                        var innerBtn;
                        var subbotton = button.items[j];
                        if (subbotton.itemId) {
                            tempbutton = this.findButton(subbotton.itemId);
                            if (tempbutton) {                               
                                if (subbotton.text) {
                                    tempbutton.text = subbotton.text;
                                }
                                if (subbotton.langkey) {
									tempbuttonText = this.findToorbarlang(subbotton.langkey);
                                    if (tempbuttonText) {
										tempbutton.text = tempbuttonText;
									}
								}  
                                if (subbotton.iconCls) {
                                    tempbutton.iconCls = subbotton.iconCls;
                                }
                                tempbutton.itemId = subbotton.itemId;
                                innerBtn = menu.add(tempbutton);
                                if (hideBtns[innerBtn.itemId] === 1) { innerBtn.hide(); chkflg = false; }
                                mu.push({ text: tempbutton.text, checked: chkflg, targetBtnID: innerBtn.itemId, checkHandler: this.onItemClick });
                            }
                            else {//object,自定义的按钮
                                if (typeof (subbotton) == "object") {
                                    //var tempbutton = subbotton;//new Object();
                                    //if (subbotton.text) {
                                    //    tempbutton.text = subbotton.text;
                                    //}
                                    if (subbotton.langkey) {
                                        var langText = this.findToorbarlang(subbotton.langkey);
                                        if (langText) {
                                            subbotton.text = langText;
                                        }
                                    }
                                    //if (subbotton.iconCls) {
                                    //    tempbutton.iconCls = subbotton.iconCls;
                                    //}
                                    //tempbutton.itemId = subbotton.itemId;
									tmpBtn = menu.add(subbotton);
                                    if (hideBtns[tmpBtn.itemId] === 1) { tmpBtn.hide(); chkflg = false; }
                                    mu.push({ text: subbotton.text, checked: chkflg, targetBtnID: tmpBtn.itemId, checkHandler: this.onItemClick });
                                }
                            }
                        }
                        else {
                            tempbutton = this.findButton(subbotton); //ngbotton的id                           
                            innerBtn = menu.add(tempbutton);
                            innerBtn.itemId = subbotton;
                            if (hideBtns[innerBtn.itemId] === 1) { innerBtn.hide(); chkflg = false; }
                            mu.push({ text: tempbutton.text, checked: chkflg, targetBtnID: innerBtn.itemId, checkHandler: this.onItemClick });
                        }
                    }
                    
                    //多语言
                    if (button.langkey) {
                        var langText = this.findToorbarlang(button.langkey);
                        if (langText) {
                            button.text = langText;
                        }
                    }
                    
                    button.menu = menu;
                    tmpBtn = this.add(button);
                    if (hideBtns[tmpBtn.itemId] === 1) { tmpBtn.hide(); chkflg = false; }
                    menus.push({ text: button.text, checked: chkflg, targetBtnID: tmpBtn.itemId, checkHandler: this.onItemClick, menu: mu });
                }
                else {
                    //if (button.itemId) {
                    //    stdbutton = this.findButton(button.itemId);
                    //    if (stdbutton) {
                    //        if (button.text) {
                    //            stdbutton.text = button.text;
                    //        }
					//		if(button.langkey){
                    //            stdbuttonText = this.findToorbarlang(button.langkey);
                    //            if(stdbuttonText){
                    //                stdbutton.text = stdbuttonText;
                    //            }
                    //        }
                    //        if (button.iconCls) {
                    //            stdbutton.iconCls = button.iconCls;
                    //        }
                    //    }
                    //}

                    //stdbutton = button;
                    //if (stdbutton) {
                    //    tmpBtn = this.add(stdbutton);
                    //    if (hideBtns[tmpBtn.itemId] === 1) { tmpBtn.hide(); chkflg = false; }
                    //    menus.push({ text: stdbutton.text, checked: chkflg, targetBtnID: tmpBtn.itemId, checkHandler: this.onItemClick });
                    //    stdbutton = undefined;
                    //}
                    //else {
                    if (button.langkey) {
                            stdbuttonText = this.findToorbarlang(button.langkey);
                        if (stdbuttonText) {
                                button.text = stdbuttonText;
                            }
                        }
                        tmpBtn = this.add(button); //标准按钮列表没找到
                        if (hideBtns[tmpBtn.itemId] === 1) { tmpBtn.hide(); chkflg = false; }
                        menus.push({ text: button.text, checked: chkflg, targetBtnID: tmpBtn.itemId, checkHandler: this.onItemClick });
                    //}
                }
            }
            else {
                stdbutton = this.findButton(button); //字符串

                if (stdbutton) {
                    tmpBtn = this.add(stdbutton);
                    if (hideBtns[tmpBtn.itemId] === 1) { tmpBtn.hide(); chkflg = false; }
                    menus.push({ text: stdbutton.text, checked: chkflg, targetBtnID: tmpBtn.itemId, checkHandler: this.onItemClick });
                    stdbutton = undefined;
                }
            }
        }

        if (this.showArrowBtn) {
        if (this.items.length > 0) {
                var mCtl = this.add({
                    text: '', width: 14, style: "Opacity:0", margin: "-2 0 -2 -8", padding: "5 5 5 0", menu: { items: menus }, listeners: {
                mouseover: function () {
                    this.el.setOpacity(100);
                },
                mouseout: function () {
                    if (!this.menu.isVisible()) {
                        this.el.setOpacity(0);
                    }
                }
            }
            });
            me.tipMenu = mCtl.menu;//toolbar的下拉变量
            mCtl.menu.mCtl = mCtl;
            mCtl.menu.pageId = pageId;
            mCtl.menu.rawHideBtns = hideBtns;
            mCtl.menu.on("beforehide", this.onBeforeMenuHide);
        }
        }

       
    },

    findButton: function (buttonid) {
        var items = this.candidateButtons;
        if (typeof ($ToolbarLang) !== "undefined") {
            var ToolbarLanghtml = Ext.htmlDecode($ToolbarLang);
            var ToolbarLang = Ext.decode(ToolbarLanghtml);
        } else {
            var ToolbarLang = {};
        }
        for (var i = 0; i < items.length; i++) {
            var btn = items[i];
            if ((buttonid === btn.itemId) || ((btn.itemId == undefined) && (buttonid === btn))) {
                if (buttonid in ToolbarLang) {
                    btn.text = ToolbarLang[buttonid]
                }
                return btn;
            }
        }
    },
	//找多语言的button
    findToorbarlang: function (langkey) {
        if (typeof ($ToolbarLang) !== "undefined") {
            var ToolbarLanghtml = Ext.htmlDecode($ToolbarLang);
            var ToolbarLang = Ext.decode(ToolbarLanghtml);
            var items = Object.keys(ToolbarLang);
            var itemslength = Object.keys(ToolbarLang).length;
            for (var i = 0; i < itemslength; i++) {
                //如果langkey和ToolbarLang的key相等，则返回ToolbarLang的value
                if (langkey == Object.keys(ToolbarLang)[i]) {
                    var toolbarkey = Object.keys(ToolbarLang)[i];
                    return ToolbarLang[toolbarkey];
                }
            }
        }

    },

    beforeRender: function (toolbar) {      
        var me = this;  
        if (!toolbar) return; //这个事件会被触发两次

        if (!Ext.isEmpty(me.rightName) && $toolbarRightInfo && !me.inPopWin) {
            var temp = Ext.htmlDecode($toolbarRightInfo);
            if (!Ext.isEmpty(temp)) {
                var disablebtn = Ext.decode(temp)[me.rightName];
                var arr = disablebtn.split(',');
                for (var i = 0; i < arr.length; i++) {
                    var btn = me.items.get(arr[i]);
                    if (btn) {
                        if (btn.disable) {
                            btn.noright = true;//业务点判断是因为权限引起的失效
                            btn.disable();
                        }
                    }
                    else {
                        if (!arr[i]) continue;
                        btn = me.queryById(arr[i]);//Ext.getCmp(arr[i]); //分组下面的按钮
                        if (btn) {
                            if (btn.disable) {
                                btn.noright = true;
                                btn.disable();
                            }
                        }
                    }
                }
                me.fireEvent('buttonready', me); //toolbar 按钮权限处理完成
            }
        }
        else {
            me.refreshRight($appinfo.orgID);
        }

    },

    doLoad: function (start) {
        this.AF.OpenLoadMask(500);
        this.load.params = this.load.params || {};
        this.load.params.rows = this.pageSize;
        this.load.params.startRow = start;
        Ext.applyIf(this.load.params, { custom: this.AF.getCustom() });
        var oldsecc = this.load.success;

        this.load.success = function (res, opts) {

            oldsecc(res, opts);
            this.AF.CloseLoadMask();
            var obj = Ext.decode(res.responseText);
            if (obj.totalRows)
                this.AF.totalCount = obj.totalRows;
            else if (obj.Record)
                this.AF.totalCount = obj.Record.length;

            //            this.onLoad(this.load.params)
        }.createDelegate(this);

        if (this.fireEvent('beforechange', this, this.load.params) !== false) {
            Ext.Ajax.request(this.load);
        }
    },

    get: function (itemId) {

        //if (itemId.indexOf('#') > 0) {
        //    var id = itemId.split('#');
        //    var topItem = id[0];
        //    var secItem = id[1];

        //    button = this.items.get(topItem).menu.items.get(secItem);
        //}
        //else {

        //    var button = this.items.get(itemId);
        //    if (!button) {
        //        button = Ext.getCmp(itemId);
        //    }
        //}

        var button = this.queryById(itemId);
        //var button = this.query("[itemId='" + itemId + "']")[0];
        return button;
    },

    closeHanler: function () {

        if (window.parent) {
            var obj = {};
            obj.msgtype = "closetab";
            //obj.title = '';
            //obj.url =  '';
            window.parent.postMessage(obj, "*");
        }

        if (this.fireEvent('beforeclose')) {
            $CloseTab();
        }

    },

    onItemClick: function (el, tf) {
        if (el.targetBtnID) {
            var btn = Ext.getCmp(el.targetBtnID);
            if (btn) {
                btn.setVisible(tf);
                if (tf) {
                    if (el.parentMenu && el.parentMenu.parentItem) {
                        if (el.parentMenu.parentItem.checked != tf) {
                            el.parentMenu.parentItem.setChecked(tf);
                        }
                    }
                }
                else {
                    if (el.parentMenu && el.parentMenu.parentItem) {
                        if (el.parentMenu.query("[checked=true]").length == 0) {
                            if (el.parentMenu.parentItem.checked != tf) {
                                el.parentMenu.parentItem.setChecked(tf);
                            }
                        }
                    }
                }
                if (el.menu) {
                    if (tf && el.menu.query("[checked=true]").length > 0) { return; }
                    Ext.Array.each(el.menu.items.items, function (c) {
                        if (c.checked != tf) {
                            c.setChecked(tf);
                        }
                    });
                }
            }
        }
    },

    onBeforeMenuHide: function (m) {
        m.mCtl.el.setOpacity(0);
        var hideBtns = new Object();
        var tmpBtns = m.query("[checked=false]");
        Ext.Array.each(tmpBtns, function (btn) {
            if (btn.targetBtnID) {
                hideBtns[btn.targetBtnID] = 1;
            }
        });
        if (JSON.stringify(m.rawHideBtns) === JSON.stringify(hideBtns)) {
            //            alert("当前值与原始值相同");
            return;
        }
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/LayoutLog/SetToolBarData',
            params: { PageId: m.pageId, HideBtns: JSON.stringify(hideBtns) },
            success: function (res, opts) {
            }
        });
    },
    
    hiddeBtn: function (itemId) {
        var me = this;
        me.get(itemId).hide();

        var findItem = null;

        if (!me.tipMenu) return;
        var menu = me.tipMenu;//Ext.getCmp('arrowMenu');
        var menuitems = menu.query('menucheckitem');        
        for (var i = 0; i < menuitems.length; i++) {
            var item = menuitems[i];
            if (item.targetBtnID === itemId) {
               findItem = item;
               break;
            }
            
        }
        if (findItem) {
           //menu.remove(findItem);
           findItem.hide();
        }
    },
    showBtn: function (itemId) {
        var me = this;
        me.get(itemId).show();
        var findItem = null;
        if (!me.tipMenu) return;

        var menu = me.tipMenu;//Ext.getCmp('arrowMenu');
        var menuitems = menu.query('menucheckitem');        
        for (var i = 0; i < menuitems.length; i++) {
            var item = menuitems[i];
            if (item.targetBtnID === itemId) {
               findItem = item;
               break;
            }
            
        }
        if (findItem) {
           //menu.remove(findItem);
           findItem.show();
        }
    },
    refreshRight: function (orgid) {
        var me = this;        

        if (me.rightName) {
            //按钮权限控制
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/PageRight/GetNoRightsButtons?pageName=' + me.rightName + '&orgid=' + orgid,
                params: { rightname: me.rightName },
                success: function (response, opts) {

                    var disablebtn = response.responseText;              

                    //先放开
                    for (var i = 0; i < length; i++) {
                        var btn = me.items.items[i];
                        btn.enable();
                    }

                    var arr = disablebtn.split(',');
                    for (var i = 0; i < arr.length; i++) {
                        var btn = me.items.get(arr[i]);
                        if (btn) {
                            if (btn.disable) {
                                btn.noright = true;//业务点判断是因为权限引起的失效
                                btn.disable();
                            }
                        }
                        else {
                            if (!arr[i]) continue;
                            btn = me.queryById(arr[i]);//Ext.getCmp(arr[i]); //分组下面的按钮
                            if (btn) {
                                if (btn.disable) {
                                    btn.noright = true;
                                    btn.disable();
                                }
                            }
                        }                        
                    }
                    me.fireEvent('buttonready', me); //toolbar 按钮权限处理完成
                }
            });
        }

    },
    enableBtn: function (itemId, flg) {
        var me = this;
        var btn = me.get(itemId);
        if (btn.noright) return;
        if (flg) {
            btn.enable();
        }
        else {
            btn.disable();
        }
    },
});

Ext.define('Ext.ng.FormPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ngFormPanel', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建         
    initComponent: function () {
        this.callParent();
        var me = this;
        var otype = me.otype;

        if (!me.border) {
            me.style = { borderColor: 'transparent', backgroundColor: 'transparent' };
        }

        //form变为只读
        if (otype === 'view') {
            var fields = me.getForm().getFields().items;
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                field.readOnly = true;
                //                if (field.setReadOnly) {
                //                    field.setReadOnly(true);
                //                    //field.setDisabled(true);                       
                //                    field.el.down('input').setStyle({ backgroundImage: 'none', backgroundColor: '#eaeaea' });
                //                }
            }
        }

        me.addEvents('dataready'); //数据准备成功，form设置值setValue完成后触发,供二次开发调用

    },
    getFormData: function (serial) {
        var me = this;
        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }
        var json = GetExtJsFormData(me, me.buskey, me.otype, serialflag);

        return json;
    },
    isValid: function () {
        var me = this;
        var valid = true;

        if (!me.otype) {
          NGMsg.Error("formpanel的otype属性未设置，请设置为add或者edit");
          return false;
        }

        var fields = me.getForm().getFields().items;
        for (var i = 0; i < fields.length; i++) {
            var field = fields[i];
            //
            if (field.isValid() == false) {
                valid = false;            

                if (!field.activeErrors) {
                    NGMsg.Error('[' + field.fieldLabel + ']输入不合法:该输入项为必输项');
                }
                else {
                    NGMsg.Error('[' + field.fieldLabel + ']输入不合法:' + field.activeErrors);
                }

                field.focus();
                break;
            }
        }

        return valid;
    },
    getItem: function (itemId) {
        return this.queryById(itemId);
    },
    setFormReadOnly: function (flag) {
        var me = this;
        var fields = me.getForm().getFields().items;
        for (var i = 0; i < fields.length; i++) {
            var field = fields[i];
            //field.readOnly = true;
            if (field.setReadOnly) {
                if (flag) {
                    field.originReadOnly = field.readOnly;//记忆readonly的初始值
                    field.setReadOnly(true);
                }
                else {
                    if (!field.originReadOnly) {//初始值为readonly，不能放开
                        field.setReadOnly(false);
                    }
                }
            }
        }
    },
    setOriginValue: function (val) {
        this.OriginValue = val;
    },
    hasModifyed: function () {
        var me = this;
        if (me.OriginValue === JSON.stringify(me.getForm().getValues())) {
            return false;
        }
        return true;
    }    
});

Ext.define('Ext.ng.QueryPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ngQueryPanel',
    region: 'north',
    autoHeight: true,
    frame: true,
    border: false,
    columnsPerRow: 4,
    hidePanel: true,
    queryAutoLoad: true, //自动load
    bodyStyle: 'padding:10px 10px 0px 10px',
    fieldDefaults: {
        labelWidth: 80,
        anchor: '100%',
        margin: '0 10 5 0',
        msgTarget: 'side'
    },
    beforeSearch: Ext.emptyFn,
    defaults: {
        anchor: '100%'
    },
    buttons: [{ xtype: "checkbox", boxLabel: '记忆搜索', name: 'QueryPanel_checkbox_rembname', inputValue: '1', margin: '0 0 0 6' }, "->",
			  { text: "查 询", iconCls: 'icon-Query' }, { text: "清 空", iconCls: 'icon-Clear' },
			  { xtype: 'label', text: '', margin: '0 0 0 20' }],
    initComponent: function () {
        var me = this;
        var store = new Object();
        if (!me.pageid) {
            me.pageid = window.location.pathname.replace(/[\/\.]/g, '');
        }
        //#region button 区域
        me.buttons[3].handler = function () {
            var formdata = me.getForm();
            var items = formdata.getFields().items;
            Ext.Array.each(items, function (f) {
                if (f.name == "QueryPanel_checkbox_rembname") { //记忆搜索选择框
                    f.setValue("0");
                }
                else {
                    f.setValue("");
                }
            });
        };
        me.buttons[2].handler = function () {
            if (me.beforeSearch() === false) {
                return;
            }
            me.searchEvent(store);
        };
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
            me.toolbar.insert(index, {
                itemId: 'hidden_Query',
                iconCls: me.hidePanel ? 'icon-unfold' : 'icon-fold',
                text: me.hidePanel ? '显示' : '隐藏',
                handler: function () {
                    if (this.iconCls == 'icon-unfold') {
                        me.show();
                        this.setIconCls('icon-fold');
                        this.setText("隐藏");
                        if (!me.isBindCombox) {
                            me.isBindCombox = true;
                            BatchBindCombox(me.getForm().getFields().items);
                        }
                    }
                    else {
                        me.hide();
                        this.setIconCls('icon-unfold');
                        this.setText("显示");
                    }
                }
            });
            if (me.hidePanel) { me.hide(); }
        }
        //#endregion

        //#region 生成查询区
        var columns = new Array();
        var totalColumns = 0;

        var newColumns = [];
        if (me.grid) {
            newColumns = Ext.clone(me.grid.columns);
            store = me.grid.store;
        }

        //乱写的
        //if (me.queryCtl) {
        //    var queryCtlCount = me.queryCtl.length;
        //    for (var i = 0; i < queryCtlCount; i++) {
        //        var col = Ext.clone(me.queryCtl[i]);
        //        var column = new Object();
        //        column.queryCtl = col;
        //        column.header = col.header;
        //        column.dataIndex = col.dataIndex;
        //        column.isNeedQueryField = true;
        //        delete column.queryCtl.header;
        //        delete column.queryCtl.dataIndex;
        //        newColumns.push(column);
        //    }
        //}

        if (newColumns.length > 0) {
            //预处理列
            var columnsCount = newColumns.length;
            for (var i = 0; i < columnsCount; i++) {
                var column = newColumns[i];

                if (column.queryCtl && column.queryCtl.colspan) {
                    totalColumns += column.queryCtl.colspan;
                }
                else {
                    totalColumns++;
                }

                //if (column.hidden || !column.isNeedQueryField || column.$className === 'Ext.grid.RowNumberer') {
                if (!column.isNeedQueryField || column.$className === 'Ext.grid.RowNumberer') {
                    continue; //隐藏列,行号，或者非查询列跳过
                }

                var flag = false;
                if (column.queryCtl) {
                    if ((column.queryCtl.xtype === 'datefield' || column.queryCtl.xtype === 'numberfield') && column.queryCtl.regionQeury != false) {
                        flag = true;
                    }
                    else {
                        flag = column.queryCtl.regionQeury;
                    }
                }
                //数字或者日期列
                if (flag) {

                    var lowColumn = new Object(); //Ext.clone(column);
                    for (var p in column) {

                        if (p === 'text') {
                            lowColumn.text = column.text + '(下限)';
                        }
                        else if (p === 'dataIndex') {
                            if (column.queryCtl.xtype === 'datefield') {
                                lowColumn.dataIndex = column.dataIndex + '*date*ngLow'; //修改上限字段名称,日期字段:date
                            }
                            else if (column.queryCtl.xtype === 'numberfield') {
                                lowColumn.dataIndex = column.dataIndex + '*num*ngLow'; //修改上限字段名称，数字字段:num
                            }
                            else {
                                lowColumn.dataIndex = column.dataIndex + '*char*ngLow'; //修改上限字段名称，字符字段:num
                            }
                        }
                        else {
                            lowColumn[p] = column[p];
                        }
                    }
                    columns.push(lowColumn);

                    var upColumn = new Object(); //Ext.clone(column);
                    for (var p in column) {

                        if (p === 'text') {
                            upColumn.text = column.text + '(上限)';
                        }
                        else if (p === 'dataIndex') {
                            if (column.queryCtl.xtype === 'datefield') {
                                upColumn.dataIndex = column.dataIndex + '*date*ngUP'; //修改上限字段名称
                            }
                            else if (column.queryCtl.xtype === 'numberfield') {
                                upColumn.dataIndex = column.dataIndex + '*num*ngUP'; //修改上限字段名称
                            }
                            else {
                                upColumn.dataIndex = column.dataIndex + '*char*ngUP'; //修改上限字段名称
                            }
                        }
                        else {
                            upColumn[p] = column[p];
                        }
                    }
                    columns.push(upColumn);
                }
                else {
                    columns.push(column);
                }
            }
        }

        var cols; //按一行三列分开
        var columnWith;

        //默认的
        if (!me.columnsPerRow) {
            cols = 4; //按一行四列分开
            columnWith = .25;
            if (columns.length < 6) {
                cols = 2; //小于6行就两列
                columnWith = .45;
            }
        }
        else {
            cols = me.columnsPerRow;
            switch (cols) {
                case 1:
                case 2: columnWith = 0.498; break;
                case 3: columnWith = 0.333; break;
                case 4: columnWith = 0.249; break;
                default: columnWith = 0.249;
            }
        }

        var rows = Math.ceil(totalColumns / cols); //计算行数

        var index = 0;
        var outarr = new Array();
        for (var i = 0; i < rows; i++) {
            var outobj = new Object();
            outobj.xtype = 'container';
            outobj.layout = 'column';
            outobj.border = false;
            var inarr = new Array();

            for (var j = 0; j < cols;) {
                if (index >= columns.length) {
                    break; //超界
                }

                var column = columns[index];
                index++;
                var inItems = new Object();

                var tempColumnWith = columnWith;

                if (column.queryCtl && column.queryCtl.colspan) {
                    tempColumnWith *= column.queryCtl.colspan;
                    j += column.queryCtl.colspan;
                }
                else {
                    j++;
                }

                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                inItems.layout = 'anchor', //'form';
				inItems.border = false

                var field = new Object();

                if (column.queryCtl) {
                    field = Ext.clone(column.queryCtl); //深拷贝
                }
                else {
                    field.xtype = 'ngText'; //默认的控件是文本框
                }

                var theName = column.dataIndex;
                if (column.queryCtl && column.queryCtl.name) {
                    theName = column.queryCtl.name;
                }

                var obj = { fieldLabel: column.header || column.text, name: theName };
                Ext.apply(field, obj); //修正名称,字段值      

                inItems.items = [field];
                inarr.push(inItems);
            }
            outobj.items = inarr;
            outarr.push(outobj);
        }
        me.items = outarr;
        //#endregion

        this.callParent();

        //#region 根据记忆给查询区赋值
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/QueryPanel/GetQueryPanelData',
            async: true,
            params: { PageId: me.pageid },
            success: function (res, opts) {
                if (Ext.isEmpty(res.responseText)) {
                    return;
                }
                var data = Ext.JSON.decode(res.responseText);
                var formdata = me.getForm();
                var items = formdata.getFields().items;
                if (!data.reembersql || data.reembersql.length == 0) {

                    if (me.queryAutoLoad) {
                        store.load();
                    }

                    Ext.Array.each(items, function (f) {
                        me.bindKeyEvent(f.id, store);
                    });
                }
                else {
                    var rembstr = Ext.JSON.decode(data.remeberstr);
                    store.queryObj = { 'queryfilter': data.remeberstr };
                    if (me.queryAutoLoad) {
                        store.load();
                    }
                    Ext.Array.each(items, function (f) {
                        if (f.name == "QueryPanel_checkbox_rembname") { //记忆搜索选择框
                            f.setValue("1");
                        }
                        else {
                            if (rembstr[f.name].length > 0) {
                                f.setValue(rembstr[f.name]);
                            }
                            me.bindKeyEvent(f.id, store);
                        }
                    });
                    if ((!me.hidePanel || !me.toolbar) && !me.isBindCombox) {
                        me.isBindCombox = true;
                        BatchBindCombox(me.getForm().getFields().items);
                    }
                }
            }
        });
        //#endregion
    },
    searchEvent: function (store) {
        var me = this;
        var chk = me.getForm().findField("QueryPanel_checkbox_rembname");//Ext.getCmp("QueryPanel_checkbox_rember");
        var data = me.getForm().getValues();
        delete data.QueryPanel_checkbox_rembname;;   //过滤掉记忆搜索
        store.currentPage = 1;
        if (store.queryObj) {
            Ext.apply(store.queryObj, { 'queryfilter': JSON.stringify(data) });
        }
        else {
            store.queryObj = { 'queryfilter': JSON.stringify(data) };
        }

        if (store.cachePageData) {
            store.cachePageData = false;
            store.load();
            store.cachePageData = true;
        }
        else {
            store.load();
        }

        if (chk.checked) {
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/QueryPanel/SetQueryPanelData',
                params: { PageId: me.pageid, ClientJsonString: JSON.stringify(data) },
                success: function (res, opts) {
                }
            });
        }
    },
    bindKeyEvent: function (cid, store) {
        var me = this;
        new Ext.KeyMap(cid, [{
            key: [10, 13],
            fn: function () { me.searchEvent(store); }
        }]);
    }
});

Ext.define('Ext.ng3.QueryPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.ng3QueryPanel',
    region: 'north',
    autoHeight: true,
    frame: true,
    border: false,
    clientSqlFilter: "",      //过滤写法: '{"No*str*like":"1","Name*str*like":"2","Contype*str*eq":"8"}'
    columnsHidden: [],
    columnsPerRow: 4,
    hidePanel: false,
    queryAutoLoad: true, //自动load
    ignoreRemStr: false,//忽略记忆值
    inPopWin: false,//在弹出窗口,独立发请求
    bodyStyle: 'padding:10px 10px 0px 10px',
    fieldDefaults: {
        labelWidth: 80,
        anchor: '100%',
        margin: '0 10 5 0',
        msgTarget: 'side'
    },
    beforeSearch: Ext.emptyFn,
    defaults: {
        anchor: '100%'
    },
    buttons: [{ xtype: "checkbox", boxLabel: '记忆搜索', name: 'QueryPanel_checkbox_rembname', inputValue: '1', margin: '0 0 0 6' }, "->",
              { text: "查 询", itemId: 'query' }, { text: "清 空" }, { xtype: 'button', text: '设置', margin: '0 3 0 0', itemId: 'queryset' },
			  { xtype: 'label', text: '', margin: '0 0 0 20' }],
    initComponent: function () {
        var me = this;
        if (!me.pageid) {
            me.pageid = window.location.pathname.replace(/[\/\.]/g, '');
        }
        //#region button 区域

        //处理勾选之后要发请求到后端去保存勾选框的数据
        me.buttons[0].handler = function () {

            if (this.getValue() == true) {
                var ischeck = '1'
            } else {
                var ischeck = '0'
            }
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/QueryPanel/SaveCheckData',
                params: { pageid: me.pageid, ischeck: ischeck },
                success: function (res, opts) {
                }
            });
        };
        
        //me.buttons[2].handler = function () {
        //    if (me.beforeSearch() === false) {
        //        return;
        //    }
        //    me.searchEvent(me.grid.store, false);
        //};
        me.buttons[2].handler = me.debounceImmediate(function a() {
            if (me.beforeSearch() === false) {
                return;
            }
            me.searchEvent(me.grid.store, false);
        }, 600,true);

        me.buttons[3].handler = function () {
            var formdata = me.getForm();
            var items = formdata.getFields().items;
            Ext.Array.each(items, function (f) {
                //if (f.id == "QueryPanel_checkbox_rember") { //记忆搜索选择框
                if (f.name == "QueryPanel_checkbox_rembname") { //记忆搜索选择框
                    f.setValue("0");
                }
                else {
                    f.setValue("");
                }
            });
        };

        me.buttons[4].handler = function () {
            var callback = function () {
                if (me.items && me.items.length >= 0) {  //从>变为>=。是为了解决内嵌查询控件全部不勾的时候,再次勾选数据出不来的问题
                    me.removeAll();
                    me.getForm().getFields().removeAll(); //removeAll之后，还需要把fields给清空，否则getForm().getValues()有问题

                    me.add(me.buildLayout(false));

                    if (me.queryAutoLoad) {
                        me.searchEvent(me.grid.store);
                    }
                }
            };
            me.setQueryInfo(me.pageid, callback);
        };

        me.items = me.buildLayout(true);//me.buildTableLayout();

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

            var showFlg = me.langInfo.Show || '显示';
            var hideFlg = me.langInfo.Hide || '隐藏';
            me.toolbar.insert(index, {
                itemId: 'hidden_Query',
                iconCls: me.hidePanel ? 'icon-unfold' : 'icon-fold',
                text: me.hidePanel ? showFlg : hideFlg,
                handler: function () {
                    if (this.iconCls == 'icon-unfold') {
                        me.show();
                        this.setIconCls('icon-fold');
                        this.setText(hideFlg);
                        if (!me.isBindCombox) {
                            me.isBindCombox = true;
                            BatchBindCombox(me.getForm().getFields().items);
                        }
                    }
                    else {
                        me.hide();
                        this.setIconCls('icon-unfold');
                        this.setText(showFlg);
                    }
                }
            });
            if (me.hidePanel) { me.hide(); }
        }
        //#endregion
       
        this.callParent();
        me.addEvents('beforesearch');//查询之前
        BatchBindCombox(me.getForm().getFields().items);

        if (me.queryAutoLoad) {
            me.searchEvent(me.grid.store, true);
        }
        
        //Ext.getCmp("QueryPanel_checkbox_rember").setValue("1");
        //var searcheBtn = me.query("[name='QueryPanel_checkbox_rembname']")[0];
        //console.log(me.ischeck);
        var searcheBtn = me.getForm().findField("QueryPanel_checkbox_rembname").setValue(me.ischeck);

        //me.on('afterrender', function () {
        //    var span = me.queryById('query').el.down('span');
        //    span.setStyle({ backgroundColor: '#03A9F4', color: '#fbfbfb' });
        //});
       
    },
    debounce: function (func, delay) {
        return function (args) {
            var _that = this;
            var _args = args;
            clearTimeout(func.id);
            func.id = setTimeout(function () {
                func.call(_that, _args)
            }, delay);
        }
    },
    debounceImmediate: function (func, wait, immediate) {
        var timeout, result;
        return function () {
            var context = this;
            var args = arguments;
            if (timeout) clearTimeout(timeout);
            if (immediate) {
                var callNow = !timeout;
                timeout = setTimeout(function () {
                    timeout = null;
                }, wait);
                if (callNow) result = func.apply(context, args);
            } else {
                timeout = setTimeout(function () {
                    func.apply(context, args)
                }, wait)
            }
            return result;
        }
    },
    setQueryInfo: function (pageid, callback) {

        var toolbar = Ext.create('Ext.Toolbar', {
            region: 'north',
            border: false,
            height: 36,
            minSize: 26,
            maxSize: 26,
            items: [{ id: "query_save", text: "保存", width: this.itemWidth, iconCls: "icon-save" },
                           { id: "query_addrow", text: "增行", width: this.itemWidth, iconCls: "icon-AddRow" },
                           { id: "query_deleterow", text: "删行", width: this.itemWidth, iconCls: "icon-DeleteRow" },
                            '->',
                            { id: "query_close", text: "关闭", width: this.itemWidth, iconCls: "icon-Close", handler: function () { win.close(); } }
                           ]
        });

        //定义模型
        Ext.define('queryInfoModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'ccode',
                mapping: 'ccode',
                type: 'string'
            }, {
                name: 'pageid',
                mapping: 'pageid',
                type: 'string'
            }, {
                name: 'searchtable',
                mapping: 'searchtable',
                type: 'string'
            }, {
                name: 'searchfield',
                mapping: 'searchfield',
                type: 'string'
            }, {
                name: 'fname_chn',
                mapping: 'fname_chn',
                type: 'string'
            }, {
                name: 'fieldtype',
                mapping: 'fieldtype',
                type: 'string'
            }, {
                name: 'combflg',
                mapping: 'combflg',
                type: 'string'
            }, {
                name: 'defaultdata',
                mapping: 'defaultdata',
                type: 'string'
            }, {
                name: 'displayindex',
                mapping: 'displayindex',
                type: 'number'
            }, {
                name: 'isplay',
                mapping: 'isplay',
                type: 'int'
                //type: 'boolean'
            }, {
                name: 'definetype',
                mapping: 'definetype',
                type: 'string'
            }, {
                name: 'sortmode', //排序方式
                mapping: 'sortmode',
                type: 'int'
            }, {
                name: 'sortorder', //排序顺序
                mapping: 'sortorder',
                type: 'number'
            }
            //添加这个，为了防止以后又出现让新增删除按钮的情况
            //, {
            //    name: 'sysflg',
            //    mapping: 'sysflg',
            //    type: 'int'
            //}
            ]
        });

        var richQueryStore = Ext.create('Ext.ng.JsonStore', {
            model: 'queryInfoModel',
            autoLoad: true,
            pageSize: 50,
            url: C_ROOT + 'SUP/QueryPanel/GetIndividualQueryPanelInfo?pageId=' + pageid
        });

        var queryPanelCellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
            clicksToEdit: 1
        });

        var operatorType = Ext.create('Ext.ng.ComboBox', {
            valueField: "code",
            displayField: 'name',
            queryMode: 'local',                           //local指定为本地数据  如果是后台传输  值为remote     
            name: 'mode',
            datasource: 'default',
            data: [{             //编辑状态下,状态列的下拉菜单的 data
                "code": "eq",
                "name": "="
            }, {
                "code": "gt",
                "name": ">"
            }, {
                "code": "lt",
                "name": "<"
            }, {
                "code": "ge",
                "name": ">="
            }, {
                "code": "le",
                "name": "<="
            }, {
                "code": "like",
                "name": "%*%"
            }, {
                "code": "LLike",
                "name": "*%"
            }, {
                "code": "RLike",
                "name": "%*"
            }]
        });
        
        
        var sortType = Ext.create('Ext.ng.ComboBox', {
            valueField: "code",
            displayField: 'name',
            queryMode: 'local',                           //local指定为本地数据  如果是后台传输  值为remote
            valueType:'int',
            name: 'mode',
            data: [{             //编辑状态下,状态列的下拉菜单的 data
                "code": 0,
                "name": "不排序"
            }, {
                "code": 1,
                "name": "升序"
            }, {
                "code": 2,
                "name": "降序"
            }]
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            width: 1400,
            stateful: true,
            //stateId: 'sysgrid',
            store: richQueryStore,
            otype: 'edit',
            buskey: 'ccode', //对应的业务表主键               
            columnLines: true,
            columns: [{
                header: '代 码',
                flex: 1,
                sortable: false,
                dataIndex: 'ccode',
                hidden: true
            }, {
                header: '字段类型',
                flex: 1,
                sortable: false,
                dataIndex: 'fieldtype',
                hidden: true
            }, {
                header: '业务点标志',
                flex: 1,
                sortable: false,
                dataIndex: 'pageid',
                hidden: true
            }, {
                header: '表名',
                flex: 2,
                sortable: false,
                dataIndex: 'searchtable'
            }, {
                header: '字段',
                width: 100,
                sortable: false,
                dataIndex: 'searchfield'
            }, {
                header: '字段名称',
                width: 100,
                sortable: false,
                dataIndex: 'fname_chn',
                //editor: {}
            }, {
                header: '运算符',
                flex: 1,
                sortable: false,
                dataIndex: 'combflg',
                //editor: operatorType,
                renderer: function (val) {
                    var ret;
                    var index = operatorType.getStore().find('code', val);
                    var record = operatorType.getStore().getAt(index);
                    if (record) {
                        ret = record.data.name;
                        return ret;
                    } else {
                        ret = val;
                        return ret;
                    }
                }
            }, {
                header: '默认值',
                flex: 1,
                sortable: false,
                dataIndex: 'defaultdata',
                editor: {}
            }, {
                header: '布局顺序',
                flex: 1,
                sortable: false,
                dataIndex: 'displayindex',
                editor: { xtype: 'numberfield', minValue: 0 }
            }, {
                xtype: 'ngcheckcolumn',                          
                header: '是否显示',
                flex: 1,
                //width:80,
                sortable: false,
                dataIndex: 'isplay',
                checkedVal: 1,
                unCheckedVal: 0//,
                //readerer: function (val) {
                //    //debugger;
                //    if (val == 1) {
                //        return true;
                //    }
                //    return false;
                //}
            }, {
                header: '定义类型',
                //flex: 1,
                width:80,
                sortable: false,
                dataIndex: 'definetype'
            }, {
                //0默认为空 || 1升序 || 2降序 
                header: '排序方式',
                //flex: 1,
                width:60,
                sortable: false,
                dataIndex: 'sortmode',
                editor: sortType,                
                renderer: function (val) {
                    if (val == 0 || val == '' || val == null) return "不排序";
                    if (val == 1) return "升序";
                    if (val == 2) return "降序";
                },
            }, {
                header: '排序顺序',
                //flex: 1,
                width:60,
                sortable: false,
                dataIndex: 'sortorder',
                editor: { xtype: 'numberfield', minValue: 0 }
            }
            //添加这个，为了防止以后又出现让新增删除按钮的情况
            //, {
            //    header: '是否系统内置',
            //    flex: 1,
            //    sortable: false,
            //    dataIndex: 'sysflg',
            //    hidden: true
            //}
            ],
            plugins: [queryPanelCellEditing]
        });

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: '查询条件设置',
            border: false,
            height: 400,
            width: 860,
            constrainHeader: true,
            layout: 'border',
            modal: true,
            items: [grid],
            buttons: [
                    //新增的时候，也会调用save方法，为了防止添加的时候直接点新增，从而覆盖系统内置数据的情况
                    //{ text: '新增', handler: function () { Save(); GetQuerySetting(); win.close(); } },
                    //{ text: '删除', handler: function () { DelOneQuerySettingData(); } },
                    '->',
                    { text: '恢复默认', handler: function () { RestoreDefault();} },
                    { text: '确定', handler: function () { Save(); win.close(); } },
                    { text: '取消', handler: function () { win.close(); } }]
        });
        win.show();

        //-------------新增内嵌查询插件弹出窗口新增----------------------
        function GetQuerySetting() {
            $OpenTab('内嵌查询新增', C_ROOT + "MDP/BusObj/QuerySetting/QuerySettingEdit?otype=add&pageid=" + pageid);
        }
        //-------------End

        //----------------内嵌查询恢复默认按钮--------------------------
        function RestoreDefault() {
            Ext.MessageBox.confirm('提示', '恢复默认将会删除该页面内嵌查询自己修改过的所有数据，确认恢复默认吗？', cb);

            function cb(id) {
                if (id.toString() == "no") {
                    return;
                } else {
                    Ext.Ajax.request({
                        url: C_ROOT + 'SUP/QueryPanel/RestoreDefault?pageid=' + pageid,
                        success: function (resp) {
                            Ext.MessageBox.alert('提示', '恢复默认设置成功,请重新打开该页面!', function () {
                                setTimeout(function () { win.close(); }, 100);
                            });

                        },
                        failure: function (resp) {
                            Ext.MessageBox.alert('恢复失败', '请联系管理员');
                        }
                    });
                }
            }

        }
        //----------------End

        //-----------内嵌查询面板删除方法---------------
        //function DelOneQuerySettingData() {
        //    var records = grid.getSelectionModel().getSelection();
        //    if (records.length == 0) {
        //        Ext.Msg.alert("提示", "未选择行");
        //        return;
        //    }

        //    var ccode = records[0].get('ccode');

        //    console.log(records);
        //}
        //-------------End

            function Save() {
                var griddata = grid.getAllGridData(); //grid.getChange();
                var gridchange = grid.getChange();
                Ext.Ajax.request({
                        url: C_ROOT + 'SUP/QueryPanel/SaveQueryInfo?pageid=' +pageid,
                        params: { 'griddata': griddata, 'gridchange': gridchange
                },
                    callback: callback,
                    success: function (response) {

                    if (Ext.isEmpty(response.responseText)) return;
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        richQueryStore.commitChanges();
                    } else {
                        Ext.MessageBox.alert('保存失败', resp.status);
                        name = 'error';
                }
            }
        });
        }

        toolbar.items.get('query_save').on('click', function () {
            Save();
        });

        var data =[{             //编辑状态下,状态列的下拉菜单的 data
            "code": "eq",
            "name": "="
            }, {
            "code": "gt",
            "name": ">"
            }, {
            "code": "lt",
            "name": "<"
            }, {
            "code": "ge",
            "name": ">="
            }, {
            "code": "le",
            "name": "<="
            }, {
            "code": "like",
            "name": "%*%"
            }, {
            "code": "LLike",
            "name": "*%"
            }, {
            "code": "RLike",
            "name": "%*"
        }];

        var otherData =[{             //编辑状态下,状态列的下拉菜单的 data
            "code": "eq",
            "name": "="
            }, {
            "code": "gt",
            "name": ">"
            }, {
            "code": "lt",
            "name": "<"
            }, {
            "code": "ge",
            "name": ">="
            }, {
            "code": "le",
            "name": "<="
        }];

        grid.on('itemclick', function (grid, record, item, index, e, eOpts) {

            var ftype = record.data['fieldtype'];
            if (ftype === 'Number' || ftype === 'Date' || ftype === 'Byte' || ftype === 'Int16'
                || ftype === 'Int' || ftype === 'Int32') {

                if (operatorType.datasource === 'default') {
                    operatorType.getStore().loadData(otherData);
                    operatorType.datasource = 'other';
            }
            }
            else {
                if (operatorType.datasource === 'other') {
                    operatorType.getStore().loadData(data);
                    operatorType.datasource = 'default';
        }
        }
        });

    },
    searchEvent: function (store, isFirstLoad) {
        var me = this;
        if (!me.fireEvent('beforesearch', me))//查询之前
        {
            return;
        }

        var chk = me.getForm().findField("QueryPanel_checkbox_rembname");//Ext.getCmp("QueryPanel_checkbox_rember");
        var data = me.getForm().getValues();
        var newdata = new Object();
        var rememberSaveData = new Object(); //最终要去存的记忆值
        for (var obj in data) {
            if (data[obj] == null || data[obj] == "") {
                continue;
            }
            else {
                //日期处理，<=date情况下，需要设置为 < date+1,否则数据库带时分秒，当天就搜索不出来了
                if (obj.indexOf('date*le') > 0) {
                    var temp = data[obj];
                    if (temp.length > 11) {//带时分秒自动加一天
                        var dt = new Date(temp);
                        var newStr = obj.replace('date*le', 'date*lt');
                        newdata[newStr] = Ext.Date.format(Ext.Date.add(dt, Ext.Date.DAY, 1), 'Y-m-d');
                    } else {
                        newdata[obj] = data[obj];
                    }
                }
                else {
                    newdata[obj] = data[obj];
                }
            }
            rememberSaveData[obj] = data[obj];  //要存入的记忆值(保持不变)
        }

        delete newdata.QueryPanel_checkbox_rembname;;   //过滤掉记忆搜索

        //添加自定义过滤
        if (me.clientSqlFilter != null && me.clientSqlFilter.length != null && me.clientSqlFilter.length > 0) {
            var clientobj = JSON.parse(me.clientSqlFilter);
            Ext.applyIf(newdata, clientobj);
        }
        //待审批预算
        if (me.clientSqlFilter != null && me.pageid == 'Web:WorkFlowTaskList') {
            Ext.applyIf(newdata, me.clientSqlFilter);
        }

        //发文管理
        if (me.clientSqlFilter != null) {
            Ext.applyIf(newdata, me.clientSqlFilter);
        }

        store.currentPage = 1;
        var str = JSON.stringify(newdata);
        var rememberSaveStr = JSON.stringify(rememberSaveData)
        //str = str.replace(/年/g, '/').replace(/月/g, '/').replace(/日/g, '');

        if (store.queryObj) {
            Ext.apply(store.queryObj, { 'queryfilter': str, 'sortinfo': me.sortinfo });
        }
        else {
            store.queryObj = { 'queryfilter': str, 'sortinfo': me.sortinfo };
        }

        if (store.cachePageData) {
            store.cachePageData = false;
            store.load();
            store.cachePageData = true;
        }
        else {
            store.load();
        }

        if (!isFirstLoad) {
            if (chk && !chk.checked) {
                str = '';//不记忆，置空
            }
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/QueryPanel/SetQueryPanelData',
                params: { PageId: me.pageid, ClientJsonString: rememberSaveStr },
                success: function (res, opts) {
                }
            });
        }
    },
    bindKeyEvent: function (cid, store) {
        var me = this;
        new Ext.KeyMap(cid, [{
            key: [10, 13],
            fn: function () { me.searchEvent(store); }
        }]);
    },   
    buildLayout: function (isFirstLoad) {
        var me = this;
        if (me.labelWidth) {
            Ext.apply(me.fieldDefaults, { labelWidth: me.labelWidth });
        }
        var columnsPerRow = me.columnsPerRow;//4;
        var fields = [];

        if ($queryPanelInfo && isFirstLoad && !me.inPopWin) {
            var panelInfo = Ext.htmlDecode($queryPanelInfo);
            if (!Ext.isEmpty(panelInfo)) {
                var json = Ext.decode(panelInfo);
                var objstr = json[me.pageid];//根据pageid获取查询信息
                if (!objstr) {
                    NGMsg.Warn("内嵌查询pageid:[" + me.pageid + "]的信息未找到，与Controller层配置的QueryPanelIDs属性不匹配!");
                    me.setLangInfo({});
                    return;
                }

                var resp = objstr["list"];
                var rememberstr = objstr["rememberstr"];

                var sortinfo = objstr["sortfilter"];
                me.sortinfo = sortinfo;
                //在这里设置搜索框的值
                var ischeck = objstr["ischeck"];
                me.ischeck = ischeck;

                totalColumns = resp.length;
                for (var i = 0; i < resp.length; i++) {
                    var name = resp[i].name;

                    if (me.ignoreRemStr) continue; //忽略记忆值
                    if (me.ischeck == "1") {
                        if (rememberstr != null && (rememberstr[name] != null || rememberstr[name] != "")) {
                            rememflag = true;
                            resp[i].value = rememberstr[name];   //记忆搜索
                        }
                    }


                    fields.push(resp[i]);
                }
                var langInfo = objstr["langInfo"];


                if (isFirstLoad) {
                    me.setLangInfo(langInfo);
                    //me.setQueryPanelChecked(ischeck);
                }



            }
        }
        else {
            //alert("内嵌查询需要改造！");
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/QueryPanel/GetIndividualQueryPanel?pageid=' + me.pageid,
                async: false, //同步请求
                success: function (response) {
                    if (Ext.isEmpty(response.responseText)) return;
                    var objstr = Ext.decode(response.responseText);
                    var resp = Ext.JSON.decode(objstr["list"]);
                    var rememberstr = Ext.JSON.decode(objstr["rememberstr"]);

                    var sortinfo = objstr["sortfilter"];
                    me.sortinfo = sortinfo;

                    var ischeck = objstr["ischeck"];
                    me.ischeck = ischeck;

                    totalColumns = resp.length;
                    for (var i = 0; i < resp.length; i++) {
                        var name = resp[i].name;

                        if (me.ignoreRemStr) continue; //忽略记忆值
                        if (me.ischeck == "1") {
                            if (rememberstr != null && (rememberstr[name] != null || rememberstr[name] != "")) {
                                rememflag = true;
                                resp[i].value = rememberstr[name];   //记忆搜索
                            }
                        }


                        fields.push(resp[i]);
                    }

                    var langInfo = Ext.JSON.decode(objstr["langInfo"]);
                    if (isFirstLoad) {
                        me.setLangInfo(langInfo);
                    }
                }
            });

            //me.langInfo = {};
            //return [];
        }

        var items = fields; //me.fields; //me.items; //所有控件
        var totalColumns = 0;

        var hiddenfields = []; //隐藏字段集
        for (var i = 0; i < fields.length; i++) {

            var tempItem = fields[i];
            if (!tempItem) continue;
            //if (readOnly) {
            //    tempItem.readOnly = true;
            //}
            if (tempItem.hidden) {
                hiddenfields.push(tempItem);
                continue;
            }
            if (tempItem.colspan) {
                totalColumns += tempItem.colspan;
            }
            else {
                totalColumns++;
            }
            //items.push(tempItem);
        }

        var cols; //默认按一行三列分开
        var columnWith;
        if (!columnsPerRow) {

            cols = 3; //默认按一行三列分开
            columnWith = .3;
            if (items) {
                if (items.length < 6) {
                    cols = 2; //小于6行就两列
                    columnWith = .45;
                }
            }
        }
        else {

            cols = columnsPerRow; //me.columnsPerRow;
            switch (cols) {
                case 1: columnWith = 0.9; break;
                case 2: columnWith = 0.49; break;
                case 3: columnWith = 0.33; break;
                case 4: columnWith = 0.249; break;
                case 5: columnWith = 0.199; break;
                case 6: columnWith = 0.16; break;
                case 7: columnWith = 0.14; break;
                case 8: columnWith = 0.124; break;
                default: columnWith = 0.33;
            }
        }

        var rows = Math.ceil(totalColumns / cols); //计算行数

        var index = 0;
        var outarr = new Array();
        for (var i = 0; i < rows; i++) {

            var outobj = new Object();
            outobj.xtype = 'container';//背景不是是白的
            outobj.layout = 'column';
            outobj.border = false;

            // outobj.defaults = {
            //     style: { paddingRight: '10px' }
            //  };

            var inarr = new Array();
            for (var j = 0; j < cols;) {

                if (index >= items.length) {
                    break; //超界
                }
                var item = items[index];
                index++;
                if (!item) continue;
                if (item.hidden) continue;

                var tempColumnWith = columnWith;
                if (item && item.colspan) {
                    tempColumnWith *= item.colspan;
                    j += item.colspan;
                }
                else {
                    j++;
                }

                var inItems = new Object();
                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                inItems.layout = 'anchor', //'form';
                //inItems.layout = 'form', //'';
				inItems.border = false

                inItems.items = [item];
                inarr.push(inItems);
            }
            outobj.items = inarr;
            outarr.push(outobj);
        }
        //me.items = outarr;

        //处理隐藏字段，放最后
        if (hiddenfields.length > 0) {
            var hid = new Object();
            hid.xtype = 'container';
            hid.border = false;
            hid.items = hiddenfields;
            outarr.push(hid);
        }

        return outarr;
    },
    setLangInfo:function(langInfo){
        var me = this;
        me.langInfo = langInfo;

        if (langInfo.RememberSearch) {
            me.buttons[0].boxLabel = langInfo.RememberSearch;
        }
        if (langInfo.Query) {
            me.buttons[2].text = langInfo.Query;
        }
        if (langInfo.Clear) {
            me.buttons[3].text = langInfo.Clear;
        }
        if (langInfo.Setting) {
            me.buttons[4].text = langInfo.Setting;
        }
    },
    setQueryPanelChecked: function (ischeck) {
        var me = this;
        me.ischeck = ischeck;
    }
});

Ext.define('Ext.ng.TableLayoutForm', {
    extend: 'Ext.ng.FormPanel', //'Ext.form.Panel',
    alias: 'widget.ngTableLayoutForm', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    region: 'north',
    autoHeight: true,
    frame: true,
    border: false,
    bodyStyle: 'padding:5px',
    //layout: 'form',//默认是auto    
    fieldDefaults: {
        //labelAlign: 'right', //'top',
        labelWidth: 60,
        anchor: '100%',
        margin: '3 10 3 0',
        msgTarget: 'side'
    },
    isBuildByRows: false,//传入二维数组按行构建布局
    initComponent: function () {
        var me = this;
        if (me.isBuildByRows) {
            me.items = me.buildLayoutByRow(me.fieldRows, me.columnsPerRow);
        }
        else {
            if (me.fields && me.fields.length > 0) {
                me.items = me.buildTableLayout(me.fields, me.columnsPerRow);
            }
        }

        this.callParent();

    },
    buildTableLayout: function (fields, columnsPerRow, readOnly) {
        var me = this;

        if (me.labelWidth) {
            Ext.apply(me.fieldDefaults, { labelWidth: me.labelWidth });
        }

        if (me.layout && me.layout.type === 'absolute')//自定义表单绝对布局，直接返回
        {
            var items = [];
            for (var i = 0; i < fields.length; i++) {
                items.push(fields[i]);
            }
            return items;
        }
        //debugger;

        var items = fields; //me.fields; //me.items; //所有控件
        var totalColumns = 0;

        var hiddenfields = []; //隐藏字段集
        for (var i = 0; i < fields.length; i++) {
          
            var tempItem = fields[i];
            if (!tempItem) continue;
            if (readOnly) {
                tempItem.readOnly = true;
            }
            if (tempItem.hidden) {
                hiddenfields.push(tempItem);
                continue;
            }
            if (tempItem.colspan) {
                totalColumns += tempItem.colspan;
            }
            else {
                totalColumns++;
            }
            //items.push(tempItem);
        }

        var cols; //默认按一行三列分开
        var columnWith;
        if (!columnsPerRow) {

            cols = 3; //默认按一行三列分开
            columnWith = .3;
            if (items) {
                if (items.length < 6) {
                    cols = 2; //小于6行就两列
                    columnWith = .45;
                }
            }
        }
        else {

            cols = columnsPerRow; //me.columnsPerRow;
            switch (cols) {
                case 1: columnWith = 0.9; break;
                case 2: columnWith = 0.49; break;
                case 3: columnWith = 0.33; break;
                case 4: columnWith = 0.249; break;
                case 5: columnWith = 0.199; break;
                case 6: columnWith = 0.16; break;
                case 7: columnWith = 0.14; break;
                case 8: columnWith = 0.124; break;
                default: columnWith = 0.33;
            }
        }

        var rows = Math.ceil(totalColumns / cols); //计算行数

        var index = 0;
        var outarr = new Array();
        for (var i = 0; i < rows; i++) {

            var outobj = new Object();
            outobj.xtype = 'container';//背景不是是白的
            outobj.layout = 'column';
            outobj.border = false;

            // outobj.defaults = {
            //     style: { paddingRight: '10px' }
            //  };

            var inarr = new Array();
            for (var j = 0; j < cols;) {

                if (index >= items.length) {
                    break; //超界
                }
                var item = items[index];
                index++;
                if (!item) continue;
                if (item.hidden) continue;

                var tempColumnWith = columnWith;
                if (item && item.colspan) {
                    tempColumnWith *= item.colspan;
                    j += item.colspan;
                }
                else {
                    j++;
                }

                var inItems = new Object();
                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                inItems.layout = 'anchor', //'form';
                //inItems.layout = 'form', //'';
				inItems.border = false

                inItems.items = [item];
                inarr.push(inItems);
            }
            outobj.items = inarr;
            outarr.push(outobj);
        }
        //me.items = outarr;

        //处理隐藏字段，放最后
        if (hiddenfields.length > 0) {
            var hid = new Object();
            hid.xtype = 'container';
            hid.border = false;
            hid.items = hiddenfields;
            outarr.push(hid);
        }

        return outarr;
    },
    buildLayoutByRow: function (fieldRows, columnsPerRow) {
        
        var columnWith;
        cols = columnsPerRow; 
        switch (cols) {
            case 1: columnWith = 0.9; break;
            case 2: columnWith = 0.49; break;
            case 3: columnWith = 0.33; break;
            case 4: columnWith = 0.249; break;
            case 5: columnWith = 0.199; break;
            case 6: columnWith = 0.16; break;
            case 7: columnWith = 0.14; break;
            case 8: columnWith = 0.124; break;
            default: columnWith = 0.33;
        }

        var outarr = new Array();
        for (var i = 0; i < fieldRows.length; i++) {

            var outobj = new Object();
            outobj.xtype = 'container';
            outobj.layout = 'column';
            outobj.border = false;
            
            var row = fieldRows[i];//行
            var inarr = new Array();
            for (var j = 0; j < row.length; j++) {

                var item = row[j];
                var tempColumnWith = columnWith;
                if (item && item.colspan) {
                    tempColumnWith *= item.colspan;                    
                }                

                var inItems = new Object();
                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                inItems.layout = 'anchor', //'form';               
				inItems.border = false

                inItems.items = [item];
                inarr.push(inItems);
            }
            outobj.items = inarr;
            outarr.push(outobj);
        }

        return outarr;
    }
    
});

Ext.define('Ext.ng.FieldSetForm', {
    extend: 'Ext.ng.FormPanel',
    alias: 'widget.ngFieldSetForm', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    region: 'north',
    frame: true,
    border: false,
    layout: 'auto', //支持自适应  
    fieldDefaults: {
        labelWidth: 60,
        margin: '4 10 4 0',
        anchor: '100%',
        msgTarget: 'side'
    },
    isBuildByRows: false,//传入二维数组按行构建布局
    initComponent: function () {
        var me = this;           
        me.items = me.buildLayout(me.fieldSets);
        this.callParent();
    },
    buildLayout: function (fieldSets, readOnly) {
        var me = this;       

        var arr = [];
        for (var i = 0; i < fieldSets.length; i++) {

            var fieldset = fieldSets[i];
            if (me.isBuildByRows) {

                if (fieldset.xtype != 'fieldset')//自定义表单第一层绝对布局，传入可能不是fieldset
                {
                    arr.push(fieldset);
                    continue;
                }
                if (fieldset.fieldRows && fieldset.fieldRows.length > 0) {                   

                    //if (fieldset.layout && fieldset.layout.type === 'absolute')//自定义表单绝对布局，直接返回
                    if (fieldset.isAbsoluteLayout) {
                        arr.push(fieldset);//先放fieldset,否则会遮住控件
                        var fieldsetItems = [];
                        for (var j = 0; j < fieldset.fieldRows.length; j++) {
                            var row = fieldset.fieldRows[j];
                            for (var k = 0; k < row.length; k++) {
                                //fieldsetItems.push(row[k]);
                                arr.push(row[k]);
                            }
                        }

                        //fieldset.items = fieldsetItems;
                    }
                    else {
                        fieldset.items = me.buildFieldSetLayoutByRow(fieldset.fieldRows, fieldset.columnsPerRow);
                        arr.push(fieldset);
                    }
                }               
            }
            else {
                if (fieldset.allfields && fieldset.allfields.length > 0) {
                    fieldset.items = me.buildFieldSetLayout(fieldset, fieldset.columnsPerRow, readOnly);
                }
                arr.push(fieldset);
            }
            //arr.push(fieldset);
        }
        return arr;
    },
    buildFieldSetLayout: function (fieldset, columnsPerRow, readOnly) {
        var me = this;

        if (fieldset.labelWidth) {
            if (fieldset.fieldDefaults) {
                Ext.apply(fieldset.fieldDefaults, { labelWidth: fieldset.labelWidth });
            }
            else {
                fieldset.fieldDefaults = { labelWidth: fieldset.labelWidth };
            }
        }
        else if (me.labelWidth) {
            if (me.fieldDefaults) {
                Ext.apply(me.fieldDefaults, { labelWidth: me.labelWidth });
            }
            else {
                fieldset.fieldDefaults = { labelWidth: me.labelWidth };
            }
        }

        var fields = fieldset.allfields;
        var items = fieldset.allfields; //me.fields; //me.items; //所有控件
        var totalColumns = 0;

        var hiddenfields = []; //隐藏字段集
        for (var i = 0; i < fields.length; i++) {
            
            var tempItem = fields[i];

            if (readOnly) {
                tempItem.readOnly = true;
            }
            if (tempItem.hidden) {
                hiddenfields.push(tempItem);
                continue;
            }

            if (tempItem.colspan) {
                totalColumns += tempItem.colspan;
            }
            else {
                totalColumns++;
            }
            //items.push(tempItem);
        }

        var cols; //默认按一行三列分开
        var columnWith;
        if (!columnsPerRow) {

            cols = 3; //默认按一行三列分开
            columnWith = .3;
            if (items) {
                if (items.length < 6) {
                    cols = 2; //小于6行就两列
                    columnWith = .45;
                }
            }
        }
        else {

            cols = columnsPerRow; //me.columnsPerRow;
            switch (cols) {
                case 1: columnWith = 0.99; break;
                case 2: columnWith = 0.49; break;
                case 3: columnWith = 0.33; break;
                case 4: columnWith = 0.249; break;
                case 5: columnWith = 0.199; break;
                default: columnWith = 0.33;
            }
        }

        var rows = Math.ceil(totalColumns / cols); //计算行数
        var index = 0;
        var outarr = new Array();
        for (var i = 0; i < rows; i++) {

            var outobj = new Object();
            outobj.xtype = 'container';
            outobj.layout = 'column';
            outobj.border = false;
            //outobj.defaults = {
            //    style: { paddingRight: '10px'}//,paddingBottom: '5px' }
            //};

            var inarr = new Array();

            for (var j = 0; j < cols;) {

                if (index >= items.length) {
                    break; //超界
                }

                var item = items[index];
                index++;
                if (item.hidden) continue;//隐藏跳过

                var tempColumnWith = columnWith;
                if (item && item.colspan) {
                    tempColumnWith *= item.colspan;
                    j += item.colspan;
                }
                else {
                    j++;
                }

                var inItems = new Object();

                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                // inItems.layout = 'form';
                inItems.layout = 'anchor';
                inItems.border = false;

                inItems.items = [item];
                inarr.push(inItems);
            }
            outobj.items = inarr;

            outarr.push(outobj);
        }

        //处理隐藏字段，放最后
        if (hiddenfields.length > 0) {
            var hid = new Object();
            hid.xtype = 'container';
            hid.border = false;
            hid.items = hiddenfields;
            outarr.push(hid);
        }

        return outarr;
    },
    buildFieldSetLayoutByRow: function (fieldRows, columnsPerRow) {

        var columnWith;
        cols = columnsPerRow;
        switch (cols) {
            case 1: columnWith = 0.9; break;
            case 2: columnWith = 0.49; break;
            case 3: columnWith = 0.33; break;
            case 4: columnWith = 0.249; break;
            case 5: columnWith = 0.199; break;
            case 6: columnWith = 0.16; break;
            case 7: columnWith = 0.14; break;
            case 8: columnWith = 0.124; break;
            default: columnWith = 0.33;
        }

        var outarr = new Array();
        for (var i = 0; i < fieldRows.length; i++) {
           
            var outobj = new Object();
            outobj.xtype = 'container';
            outobj.layout = 'column';
            outobj.border = false;            
            var inarr = new Array();
            var row = fieldRows[i];//行
            for (var j = 0; j < row.length; j++) {

                var item = row[j];
                var tempColumnWith = columnWith;
                if (item && item.colspan) {
                    tempColumnWith *= item.colspan;
                }

                var inItems = new Object();
                inItems.xtype = 'container';
                inItems.columnWidth = tempColumnWith; //.3;
                inItems.layout = 'anchor', //'form';               
                inItems.border = false

                inItems.items = [item];
                inarr.push(inItems);
            }
            outobj.items = inarr;
            outarr.push(outobj);
        }

        return outarr;
    }
});

Ext.define('Ext.ng.PagingBar', {
    extend: 'Ext.toolbar.Paging',
    alias: 'widget.ngPagingBar', //别名,可通过设置xtype构建 
    border: false,
    displayInfo: true,
    displayMsg: '第{0}-{1}条 共{2}条',
    emptyMsg: "没有数据",
    beforePageText: "第",
    afterPageText: "/{0}页",
    firstText: "首页",
    prevText: "上一页",
    nextText: "下一页",
    lastText: "尾页",
    refreshText: "刷新",
    showRefresh: true,
    initComponent: function () {
        var me = this;
        combo = Ext.create('Ext.form.ComboBox', {
            name: 'pagesize',
            //id: 'pagesize',
            itemId: 'pagesize',
            hiddenName: 'pagesize',
            store: new Ext.data.ArrayStore({
                fields: ['text', 'value'],
                data: [['20', 20], ['25', 25],['30', 30], ['40', 40], ['50', 50]]
            }),
            valueField: 'value',
            displayField: 'text',
            editable:false, //不允许用户自己编辑
            //emptyText: 20,
            width: 50
        });
        me.items = ['-',combo];
        this.callParent();    

        //me.store.pageSize = Ext.state.Manager.getProvider().state.Pagesize;  //直接在这里赋值pagesize，不然别的地方取不到,但是在构造函数里store的pagesize直接赋值，没法找到父容器。
        var stateArr = [];
        if ($gridStateInfo) {
            var stateInfo = Ext.htmlDecode($gridStateInfo);
            if (!Ext.isEmpty(stateInfo)) {
                var json = Ext.decode(stateInfo);//从viewBag获取,性能优化
                //var stateArr = [];
                if (json && json.length > 0) {
                    for (var i = 0; i < json.length; i++) {
                        var state = {};
                        if (json[i].Pagesize != "") {
                            state["Pagesize"] = json[i].Pagesize;
                        }
                        if (json[i].Gid != "") {
                            state["Gid"] = json[i].Gid;
                        }
                        if (json[i].Bustype != "") {
                            state["stateID"] = json[i].Bustype;
                        }
                        stateArr.push(state);

                    }
                }
            }
        }
        //用户没有配置gridStateID，那么第一次加载直接默认去取Ext.state.Manager里的值
        if (me.gridStateID == void 0) {
            if (Ext.state.Manager.getProvider().state.Pagesize != void 0) {
                me.store.pageSize = Ext.state.Manager.getProvider().state.Pagesize;
            }            
        } else {
            //如果配置了gridStateID，就从之前stateArr里循环读取
            for (var i = 0; i < stateArr.length; i++) {
                if (me.gridStateID == stateArr[i].stateID) {
                    if (stateArr[i].Pagesize != void 0) {
                        me.store.pageSize = stateArr[i].Pagesize;
                    }
                    
                } else {
                    //me.store.pageSize = Ext.state.Manager.getProvider().state.Pagesize;  //直接默认jsonstore里的25？
                }
            }
        }

        //这里注释掉，因为在初始化里找不到父元素，没有渲染出来,放到afterrender里
        //combo.on("select", function (comboBox,me) {
        //    var pagingToolbar = Ext.ComponentQuery.query('ngPagingBar',me.container); //查找当前容器页面上的PagingBar
        //    //pagingToolbar.pageSize = parseInt(comboBox.getValue());
        //    itemPerPage = parseInt(comboBox.getValue());
        //    pagingToolbar[0].store.pageSize = itemPerPage;
        //    pagingToolbar[0].store.loadPage(1);  //无论在第几页，切换的时候都回到第一页

        //});

       
    },
    listeners: {
        render: function (me, eOpts) {
            if (!me.showRefresh) {
                me.items.items[10].hide();
                me.items.items[9].hide();
            }
        },
        afterrender: function (me, ele) {

            var _self = me;
            var combo = me.queryById('pagesize');
            var fatherContainer = me.findParentByType('ngGridPanel');
            combo.on('select', function (comboBox, me) {
                var pagingToolbar = Ext.ComponentQuery.query('ngPagingBar', fatherContainer); //查找当前容器页面上的PagingBar

                var whichPagingbar = pagingToolbar.length - 1; //如果没有给itemID，则需要给一个默认值,测试来看，后出现的会排在后面
                if (_self.itemId != void 0) {
                    for (var i = 0; i < pagingToolbar.length; i++) {
                        if (_self.itemId == pagingToolbar[i].itemId) {
                            whichPagingbar = i;
                        }
                    }
                }
                itemPerPage = parseInt(comboBox.getValue());
                // pagingToolbar[0].store.pageSize = itemPerPage;
                // pagingToolbar[0].store.loadPage(1);  //无论在第几页，切换的时候都回到第一页     
                pagingToolbar[whichPagingbar].store.pageSize = itemPerPage;
                pagingToolbar[whichPagingbar].store.loadPage(1);  //无论在第几页，切换的时候都回到第一页                               
            });

            var pagingBarPagesize;
            var gid;
            var ngGrid = me.findParentByType('ngGridPanel'); //找到ngGrid父容器
            var ContainerStateID;

            //兼容没有父容器的和没有stateId的情况,直接return
            if (ngGrid == void 0) {
                me.queryById('pagesize').setValue(25);
                return;
            } else {
                if (ngGrid.stateId == void 0) {
                    me.queryById('pagesize').setValue(25);
                    return;
                }
            }
            var ContainerStateID = ngGrid.stateId;
            //if (ngGrid != void 0) {
            //    if (ngGrid.stateId == void 0) {
            //        ContainerStateID = "";
            //    } else {
            //        ContainerStateID = ngGrid.stateId;
            //    }
            //} else {
            //    ContainerStateID = "";
            //}


            var stateArr = [];
            if ($gridStateInfo) {
                var stateInfo = Ext.htmlDecode($gridStateInfo);
                if (!Ext.isEmpty(stateInfo)) {
                    var json = Ext.decode(stateInfo);//从viewBag获取,性能优化
                    //var stateArr = [];
                    if (json && json.length > 0) {
                        for (var i = 0; i < json.length; i++) {
                            var state = {};
                            if (json[i].Pagesize != "") {
                                state["Pagesize"] = json[i].Pagesize;
                            }
                            if (json[i].Gid != "") {
                                state["Gid"] = json[i].Gid;
                            }
                            if (json[i].Bustype != "") {
                                state["stateID"] = json[i].Bustype;
                            }
                            stateArr.push(state);

                        }
                    }
                }
            }

            //如果用户没有配置gridStateID,就默认页面上只有一个grid,就直接从Ext.state.Manager里取值
            if (me.gridStateID == void 0) {
                gid = Ext.state.Manager.getProvider().state.Gid;
                if (Ext.state.Manager.getProvider().state.Pagesize != void 0) {
                    pagingBarPagesize = Ext.state.Manager.getProvider().state.Pagesize;
                } else {
                    pagingBarPagesize = me.store.pageSize //没有找到就直接默认
                }
            } else {
                //如果用户配置了，就循环去取stateArr数组里的pagesize
                for (var i = 0; i < stateArr.length; i++) {
                    if (ContainerStateID == stateArr[i].stateID) {
                        if (stateArr[i].Pagesize != void 0) {
                            pagingBarPagesize = stateArr[i].Pagesize;
                            gid = stateArr[i].Gid;
                        } else {
                            pagingBarPagesize = me.store.pageSize //没有找到就直接默认
                            gid = stateArr[i].Gid;
                        }
                    } else {                        
                        pagingBarPagesize = me.store.pageSize //没有找到就直接默认
                        gid = Ext.state.Manager.getProvider().state.Gid || '';
                    }
                }
            }

            //me.items.items[12].setValue(pagingBarPagesize);
            me.queryById('pagesize').setValue(pagingBarPagesize);
            //me.store.loadPage(1);

            //在这里发出Ajax请求，去更新数据库里的pagesize
            me.queryById('pagesize').on("select", function (comboBox, me) {
                var pagesize = comboBox.value;
                //var gid = Ext.state.Manager.getProvider().state.Gid;
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/LayoutLog/savePagesize',
                    async: false,
                    params: {"gid":gid,"pagesize":pagesize},
                    success: function (res, opts) {

                    }
                });
                
            })
        }
    }
});

Ext.define('Ext.ng.GridPanel', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.ngGridPanel', //别名,可通过设置xtype构建 
    viewConfig: {
        enableTextSelection: true//grid支持文本选中        
    },
    curRow: 0,//当前行
    DbClickToView: true,//双击查看
    needfocusRow: false,//IE下默认不需要调用focusRow方法
    initComponent: function () {

        var me = this;
        var otype = me.otype;

        var bbar = me.bbar; //callParent方法后bbar属性置为空
        //this.callParent(arguments);

        //if (Ext.isIE) {//IE11是false
       if (!Ext.isChrome) {
            if (!me.needfocusRow) {
                if (me.viewConfig) {
                    Ext.apply(me.viewConfig, {
                        enableTextSelection: true,//grid支持文本选中
                        focusRow: Ext.emptyFn//grid中在数据行最右边输入，ie横向滚动条自动跳到最前位置
                    });
                }
                else {
                    me.viewConfig = {
                        enableTextSelection: true,//grid支持文本选中
                        focusRow: Ext.emptyFn//grid中在数据行最右边输入，ie横向滚动条自动跳到最前位置
                    }
                }
            }
        }

        //列标题对齐方式处理
        for (var i = 0; i < me.columns.length; i++) {
            var column = me.columns[i];
            if (column) {
                if (!column.hidden) {
                    if (column.titleAlign) {
                        column.style = "text-align:" + column.titleAlign;
                    }
                    else {
                        column.style = "text-align:center"; //默认列头居中
                    }
                    column.hideable = true;//允许勾选
                    //必输列控制
                    if (column.mustInput) {
                        column.style += ";color:OrangeRed";
                    }
                } else {
                    column.hideable = false;//彻底隐藏，不让勾选
                }

                //子列居中,动态设置
                if (column.columns) {
                    for (var k = 0; k < column.columns.length; k++) {
                        var col = column.columns[k];
                        if (col.titleAlign) {
                            col.style = "text-align:" + col.titleAlign;
                        }
                        else {
                            col.style = "text-align:center"; //默认列头居中
                        }
                    }
                }              
            }

            //处理stateId，解决界面经常乱掉的问题
            if (column.dataIndex) {
                column.stateId = column.dataIndex;  //处理stateid
                //以下代码弹帮助grid会错位
                //if (!column.itemId) {
                //    column.itemId = column.dataIndex;
                //}
            }
            else {
                if (column.xtype == 'rownumberer' && !column.stateId) {
                    column.stateId = 'grid_lineid';
                }
            }
        }

        this.callParent(arguments); //locked:true，居中不起作用，下移

        //grid变为只读
        me.on('beforeedit', function (editor, e, eOpts) {            
                if (e.column.readOnly) {//列是只读的
                    return false;
                }
                else {
                    if (e.column.readOnly === false) {
                        return true;
                    }
                    if (me.otype === 'view') {//查看状态设置为只读
                        return false;
                    }
                    else {
                        return true;
                    }
                }            
        });
        
        //调整分页条的页号
        if (bbar) {
            me.store.on('load', function (store, records) {
                //debugger;
                //修正页号
                if (!bbar.items) return;
                var item = bbar.items.get('inputItem');
                var pageindex = item.getValue(); //页号

                if (pageindex > 0) {
                    if (Math.ceil(store.totalCount / store.pageSize) < pageindex) {
                        if (store.totalCount > 0) {
                            item.setValue(1);
                            store.currentPage = 1;
                        }
                        else {
                            item.setValue(0);
                            store.currentPage = 0;
                        }
                    }
                }
                else {
                    item.setValue(0);
                }

            })
        }

        var dataIndex; //记录当前列
        var indexArray = [];
        var haveSearch = false;//表示是否找过，不是找到的意思     
        me.headerCt.on("headertriggerclick", function (container, header, e, t) {
            dataIndex = header.dataIndex;

            //hideColumn方法读取不到columnItem，这里补偿处理
            var columnMenus = container.getMenu().queryById('columnItem').menu;
            var allColumns = container.getGridColumns();
            for (var i = 0; i < allColumns.length; i++) {
                var col = allColumns[i];
                if (col.hideable == false) {
                    var menuItem = columnMenus.query('[text=' + col.text + ']');;
                    if (menuItem.length > 0) {
                        menuItem[0].hide();//列选择不允许勾选
                    }
                }
            }

        });

        //有lock列的grid必须在事件里面写添加按钮
        var menu = me.headerCt.on("menucreate", function (container, m) {
            m.add([{
                itemId: 'location',
                text: '定位',
                iconCls: 'icon-Location',
                handler: function () {

                    var findCount = 0;//找到次数
                    var preValue = '';//上一个值

                    var pageCount = 1;

                    var pbar = me.query('pagingtoolbar');
                    var hasPagingbar = false;

                    if (pbar.length > 0) {
                        hasPagingbar = true;
                        pageCount = Math.ceil(me.getStore().getTotalCount() / me.getStore().pageSize);
                    }
                    //显示弹出窗口
                    var win = Ext.create('Ext.window.Window', {
                        title: '定位',
                        border: false,
                        constrain: true,
                        resizable: false,
                        height: 60,
                        width: 185,
                        //layout: 'hbox',
                        y: 100,
                        layout: 'absolute',
                        modal: false,
                        fieldDefaults: {
                            labelWidth: 80,
                            anchor: '100%',
                            margin: '5 10 5 0',
                            msgTarget: 'side'
                        },
                        items: [{
                            itemId: 'loactionValue',
                            xtype: 'ngText',
                            x: 5,
                            y: 5,
                            enableKeyEvents: true,
                            listeners: {
                                'keydown': function (combo, e, eOpts) {
                                    //回车
                                    if (e.keyCode == Ext.EventObject.ENTER) {
                                        win.queryById('locationBtn').handler();
                                    }
                                }
                            }
                        }, {
                            xtype: 'button',
                            itemId: 'locationBtn',
                            iconCls: 'icon-Location',
                            x: 148,
                            y: 5,
                            handler: function () {

                                var searchValue = this.up('window').queryById('loactionValue').getValue(); //Ext.getCmp('loactionValue').getValue();

                                if (searchValue != preValue) {
                                    findCount = 0;//两次的值不一样，清零
                                    haveSearch = false;
                                    indexArray.length = 0;//换关键字了，清空数组
                                }
                                preValue = searchValue;

                                var value = {};
                                value[dataIndex] = searchValue
                                //alert(dataIndex);
                                var index = 0;

                                if (!haveSearch) {//没有找过
                                    Ext.Array.each(me.getStore().data.items, function (record) {

                                        var obj = record.data;
                                        if (obj[dataIndex].indexOf(searchValue) > -1) {
                                            indexArray.unshift(index); //入列
                                            //indexArray.push(index); //入列，到结尾

                                            findCount++;
                                        }

                                        index++;
                                    });
                                    haveSearch = true;
                                }

                                if (indexArray.length > 0) {
                                    if (me.getView().focusRow) {
                                        //me.getView().focusRow(indexArray.pop());//IE下needFocusRow为false，focusRow方法被置空，不起效果
                                        me.getView().select(indexArray.pop());
                                    }
                                    else if (me.getView().normalView) {
                                        me.getView().normalView.focusRow(indexArray.pop());
                                    }
                                    else {
                                        me.getView().select(indexArray.pop());
                                    }
                                }
                                else {

                                    //if (me.getStore().currentPage < me.getStore().getTotalCount()) {
                                    if (me.getStore().currentPage < pageCount) {
                                        me.getStore().loadPage(me.getStore().currentPage + 1);
                                    }
                                    else {
                                        if (findCount == 0) {
                                            //Ext.create('Ext.ng.MessageBox').Info('定位不到数据');
                                            //alert('定位不到数据');
                                            Ext.Msg.alert('提示', '定位不到数据');
                                        }
                                        var pbar = me.query('pagingtoolbar');
                                        if (hasPagingbar) {
                                        //if (me.bbar) {
                                            me.getStore().loadPage(1);
                                        }
                                    }

                                    haveSearch = false;

                                }

                                win.queryById('loactionValue').focus();
                                //if (me.getStore().currentPage == pageCount) {
                                //    me.getStore().currentPage == 0;                                   
                                //}

                            }
                        }]
                    });

                    win.show();
                    setTimeout(function () { win.queryById('loactionValue').focus(); }, 200);
                }
               
            }]);

        });
                
        me.on('boxready', function (cmp) {
            //增加布局菜单
            if (me.stateful && me.stateId != undefined) {
                var menu = me.headerCt.getMenu();

                menu.add([{
                    text: '默认布局',
                    //id: 'defaultlaout',
                    iconCls: 'icon-Clear',
                    handler: function () {
                        Ext.state.Manager.clear(me.stateId);
                        window.location.reload();
                    }
                }]);
            }

        });

        me.on('columnmove', function () {
            //            //增加菜单
            //            if (me.stateful && me.stateId != undefined) {
            //                var menu = me.headerCt.getMenu();
            //                
            //                menu.add([{
            //                    text: '默认布局',
            //                    iconCls: 'icon-Clear',
            //                    handler: function () {
            //                        Ext.state.Manager.clear(me.stateId);
            //                        window.location.reload();
            //                    }
            //                }]);
            //            }
        });

        //列表增加双击查看功能
        me.on('itemdblclick', function (view, record, item, index, e, eOpts) {

            if (me.DbClickToView) {
                var tbar = me.previousNode("ngToolbar");
                if (tbar) {
                    var btn_view = tbar.items.get('view');
                    if (btn_view) {
                        btn_view.fireEvent('click');
                    }
                }
            }

        });
        
        //双击查看基础数据详细信息
        me.on('celldblclick', function (view, td, cellIndex, record, tr, rowIndex, e, eOpts) {
            //alert('row: ' + rowIndex);
            me.curRow = rowIndex;

            //me.columns[cellIndex].dataIndex
            if(me.columns[cellIndex]){
                if (me.columns[cellIndex].helpid && me.columns[cellIndex].codecolumn) {
                    alert(record.data[me.columns[cellIndex].codecolumn]);
                }
            }
        });

        //tooltip浮动窗口
        me.on('itemmouseenter', function (view, record, item, index, e, eOpts) {

            var cols = view.getGridColumns();
            var col = cols[e.getTarget(view.cellSelector).cellIndex]
            var data = record.data[col.dataIndex];
            if (!Ext.isEmpty(data)) {
                if (view.tip == null) {
                    view.tip = Ext.create('Ext.tip.ToolTip', {
                        target: view.el,
                        delegate: view.itemSelector,
                        trackMouse: true,
                        //dismissDelay: 10000,
                        autoHide: false,
                        renderTo: Ext.getBody(),
                        listeners: {
                            beforeshow: function updateTipBody(tip) {
                                if (Ext.isEmpty(tip.html)) return false;
                            }
                        }
                    });
                }

                if (e.target.scrollWidth <= e.target.clientWidth)
                    data = "";
                view.el.clean();
                view.tip.update(data);
                view.tip.hide();
            }
            else {
                if (view.tip != null) {//空的单元格，不出tip
                    view.el.clean();
                    view.tip.update(data);
                    view.tip.hide();
                }
            }

        });

    },
    getChange: function (serial) {

        if (this.plugins != null && this.plugins.length > 0 && this.plugins[0]) {
            this.plugins[0].completeEdit();
        }

        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }

        var me = this;
        var json = '';
        var data = GetExtJsGridData(me.store, me.buskey, true);//所有字段都传，二开可能会需要处理

        if (serialflag) {
            json = JSON.stringify(data);
            return json;
        }
        else {
            return data;
        }
    },
    getChangeForEntity: function (serial) {

        if (this.plugins != null && this.plugins.length > 0 && this.plugins[0]) {
            this.plugins[0].completeEdit();
        }

        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }

        var me = this;
        var json = '';
        var data = GetExtJsGridData(me.store, me.buskey, true);

        if (serialflag) {
            json = JSON.stringify(data);
            return json;
        }
        else {
            return data;
        }
    },
    getAllGridData: function () {
        var json = GetAllGridData(this.store, this.buskey); //所有行状态都是新增状态
        return json;
    },
    getAllData: function (serial) {

        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }

        var json = '';
        var data = GetExtJsGridAllData(this.store, this.buskey); //全取，各种行状态都有

        if (serialflag) {
            json = JSON.stringify(data);
            return json;
        }
        else {
            return data;
        }
    },
    isValid: function () {
        var me = this;
        return ValidGridData(me);
    },
    hasModifyed: function () {

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
    getColumn: function (colname) {
        var me = this;
        var obj;

        //查找子列
        function getInnerCol(column, colname) {
            var col;
            for (var i = 0; i < column.items.items.length; i++) {
                var curCol = column.items.items[i];
                if (colname === curCol.dataIndex) {
                    col = curCol; break;
                }
                if (curCol.items.items.length > 0) {
                    col = getInnerCol(curCol, colname);//递归获取
                }
            }
            return col;
        }

        if (me.lockable) {
            for (var i = 0; i < me.lockedGrid.columns.length; i++) {
                if (colname === me.lockedGrid.columns[i].dataIndex) {
                    obj = me.lockedGrid.columns[i]; break;
                }

                if (me.lockedGrid.columns[i].items.items.length > 0) {
                    var temp = getInnerCol(me.lockedGrid.columns[i], colname);
                    if (temp) {
                        obj = temp;break;
                    }
                }
            }

            if (obj) return obj;

            for (var i = 0; i < me.normalGrid.columns.length; i++) {
                if (colname === me.normalGrid.columns[i].dataIndex) {
                    obj = me.normalGrid.columns[i]; break;
                }
                if (me.normalGrid.columns[i].items.items.length > 0) {
                    var temp = getInnerCol(me.normalGrid.columns[i], colname);
                    if (temp) {
                        obj = temp; break;
                    }
                }
            }
            return obj;
        } else {
            for (var i = 0; i < me.columns.length; i++) {
                if (colname === me.columns[i].dataIndex) {
                    obj = me.columns[i]; break
                }
                if (me.columns[i].items.items.length > 0) {
                    var temp = getInnerCol(me.columns[i], colname);
                    if (temp) {
                        obj = temp; break;
                    }
                }
            }
            return obj;
        }
    },
    getRow: function () {

        var me = this;
        var select = me.getSelectionModel().getSelection();

        var data = { 'key': me.buskey };
        data['unchange'] = select;
        var d = GetTableData(data);

        return JSON.stringify(d);
    },
    resetPagingBar: function (pagingBar) {
        var me = this;
        if (pagingBar) {

            var textItem = pagingBar.items.get('inputItem');
            textItem.setValue(1);
            me.store.currentPage = 1;
        }
    },
    setReadOnlyCol: function (colname, flag) {
        var col = this.getColumn(colname);

        if (flag || typeof (flag) == "undefined") {
            col.readOnly = true;//与grid的beforeedit事件配合完成
        }
        else {
            col.readOnly = false;
        }
        //这个写法有问题
        //if (flag || typeof (flag) == "undefined") {          
        //    col.myEditor = col.getEditor();//暂存
        //    col.setEditor(null);//设置只读   
        //}
        //else {           
        //    col.setEditor(col.myEditor);//设置只读   
        //}        
    },
    setMustInputCol: function (colname, flag) {
        
        var col = this.getColumn(colname);
        if (flag || typeof (flag) == "undefined") {
            col.addCls("mustInput");
            var editor = col.getEditor();
            editor.allowBlank = false;//必输
        }
        else {
            col.removeCls("mustInput");
            var editor = col.getEditor();
            editor.allowBlank = true;//必输
        }
    },
    hideColumn: function (colname, flag) {
        var col = this.getColumn(colname); 
        if (flag || typeof (flag) == "undefined") {
             
            //var menuItem = this.headerCt.menu.queryById('columnItem').menu.query('[text=' + col.text + ']');           
            //if (menuItem.length > 0) {
            //    menuItem[0].hide();//列选择不允许勾选
            //}
            col.hideable = false;//菜单中也隐藏           
            col.hide();//隐藏            
        } else {
            col.hideable = true;//菜单中显示            
            col.show();//显示            
        }
    },
    setMaskCol: function (colname) {
        var col = this.getColumn(colname);
        this.setReadOnlyCol(colname, true);
        col.renderer = function (val) { if (!Ext.isEmpty(val)) { return "***"; } };//掩码
    },
    setGridReadOnly: function (flag) {
        var me = this;
        if (flag) {
            me.otype = 'view';            
        }
        else {
            me.otype = 'edit';
        }

        for (var i = 0; i < me.columns.length; i++) {
            var col = me.columns[i];
            if (col.xtype === 'ngcheckcolumn') {
                if (flag) {
                    col.originReadOnly = col.readOnly;//初始值
                    col.userSetReadOnly(true);
                } else {
                    if (!col.originReadOnly) {
                        col.userSetReadOnly(false);
                    }
                }
                
            }
        }
    }
});

Ext.define('Ext.ng.GridExpandPanel', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.ngGridExpandPanel',
    plugins: [{
        ptype: 'rowexpander',
        rowBodyTpl: ['<div class="ux-row-expander-box"></div>'],
        expandOnRender: true,
        expandOnDblClick: false
    }],
    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.addEvents('prepareparam'); //准备参数
        me.addEvents('detailitemclick'); //明细列表点击

        me.getView().on('expandbody', function (node, record, eNode) {
            var element = Ext.get(eNode).down('.ux-row-expander-box');
            //IE用offsetWidth
            element.dom.style.width = (eNode.clientWidth > 0 ? eNode.clientWidth : eNode.offsetWidth) + "px";

            var detailStore = Ext.create('Ext.data.Store', {
                model: me.storeInfo.model,
                pageSize: 20,
                autoLoad: false,
                proxy: {
                    type: 'ajax',
                    url: me.storeInfo.url,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            var grid = Ext.create('Ext.grid.Panel', {
                hideHeaders: false,
                border: false,
                columns: me.detailColumns,
                store: detailStore
            });

            me.fireEvent('prepareparam', detailStore, record);

            //            detailStore.on('beforeload', function (store, operation, eOpts) {
            //                Ext.apply(store.proxy.extraParams, { 'masterID': record.data[me.masterID] });
            //            });

            detailStore.load();

            grid.on('itemclick', function (dgrid, record, item, index, e, eOpts) {
                me.getView().getSelectionModel().deselectAll();

                me.fireEvent('detailitemclick', dgrid, record, item, index, e, eOpts);
            });

            //防止事件冒泡，吞掉事件
            element.swallowEvent(['click', 'mousedown', 'mouseup', 'dblclick'], true);

            grid.render(element);

        });
        me.getView().on('collapsebody', function (node, record, eNode) {
            Ext.get(eNode).down('.ux-row-expander-box').down('div').destroy();
        });

        me.on('resize', function (grid, width, height, oldWidth, oldHeight, eOpts) {
            var element = me.el.query('div.ux-row-expander-box[id]');

            //Ext.suspendLayouts();
            for (var i = 0; i < element.length; i++) {
                if (!element[i].firstChild) continue;

                element[i].style.width = width + "px";

                Ext.getCmp(element[i].firstChild.id).doLayout();
            }
            // Ext.resumeLayouts(true);

        });
    }
});

Ext.define('Ext.ng.TabPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.ngTabPanel',
    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        if (me.tabPosition === 'left') {

            var isNeptune = document.getElementById("themestyle").href.indexOf('neptune') > 0 ? true : false; //海王星主题
            for (var i = 0; i < me.items.length; i++) {


                var oldtitle = ' ' + me.tabBar.items.items[i].text; //me.items.items[i].title;

                if (oldtitle) {
                    var newtitle = oldtitle.split('').join('<br>'); //默认是IE的标题                    

                    if (!Ext.isIE || Ext.isIE10) {

                        var width = oldtitle.length * 15;//oldtitle.length * 14 + 6;
                        var left = oldtitle.length * 9;
                        //firefox
                        if (!Ext.isChrome) {
                            width = oldtitle.length * 18; //宽度变高度
                            left = oldtitle.length * 7.5; //靠左变靠上
                        }
                        if (Ext.isIE10) {
                            var width = oldtitle.length * 16;
                            var left = oldtitle.length * 6;
                        }

                        var height = '15px';
                        if (isNeptune) {
                            height = '16px';
                        }
                        newtitle = "<div style='-webkit-transform: rotate(90deg);-moz-transform:rotate(90deg);transform:rotate(90deg);width:" + width + "px;height:" + height + ";left:" + left + "px !important;position: relative;padding: 8px 8px 8px 8px;'>"
													 + newtitle + "</div>";
                    }
                    //me.items.items[i].title = newtitle;

                    me.tabBar.items.items[i].text = newtitle;
                }
            }

            if (Ext.isIE && !Ext.isIE10) {

                var width = '20px';
                if (isNeptune) {
                    width = '30px';
                }
                for (var i = 0; i < me.tabBar.items.items.length; i++) {
                    var tab = me.tabBar.items.items[i];
                    var height = 0; //tab.text.split('<br>').length * 20;
                    //控制宽度，去掉旋转，去掉背景,圆角处理            
                    tab.style = "width:" + width + ";filter:none;background-image:none;border-bottom-left-radius:4px;border-bottom-right-radius:0px;border-top-right-radius:0px";

                    tab.on('activate', function (bar, e, eOpts) {
                        bar.el.setStyle({ backgroundColor: 'white', borderRightWidth: '0px' });
                    });

                    tab.on('deactivate', function (bar, e, eOpts) {
                        if (bar.el) {
                            bar.el.setStyle({ backgroundColor: '' });
                        }
                    });
                }

                me.on('afterrender', function () {
                    //激活第一个tab
                    me.tabBar.items.items[0].el.setStyle({ backgroundColor: 'white', borderRightWidth: '0px' });

                    for (var i = 0; i < me.tabBar.items.items.length; i++) {
                        var tab = me.tabBar.items.items[i];
                        var height = tab.text.split('<br>').length * 15 + 12;
                        tab.el.down('div.x-tab-wrap').setStyle({ width: '20px', height: height + 'px', position: 'relative', top: '7px' });
                        tab.el.down('span.x-tab-inner').setStyle({ marginLeft: '-10px' });
                    }
                });
            } //if isIE

        } //if left


    } //initComponent
});

Ext.define('Ext.we.TabPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.weTabPanel',
    cls: 'verticaltab',
    tabBar: {
        width: 100,
        minTabWidth: 100,
        maxTabWidth: 100,
        height: 15,
        orientation: 'vertical'//'horizontal'
    },
    tabPosition: 'left'
})

Ext.define('Ext.ng.JsonStore', {
    extend: 'Ext.data.Store',
    pageSize: 35,
    alias: 'widget.ngJsonStore', //别名,可通过设置xtype构建
    cachePageData: false, //缓存分页数据
    constructor: function (config) {
        tempconfig = Ext.apply({
            proxy: {
                type: 'ajax',
                url: config.url,
                actionMethods: {
                    create: 'POST',
                    read: 'POST',//read: 'GET',
                    update: 'POST',
                    destroy: 'POST'
                },
                reader: {
                    type: 'json',
                    root: 'Record',
                    totalProperty: 'totalRows'
                }
            }
        }, config);

        this.callParent([tempconfig]);

        var me = this;

        me.dataContainer = Ext.create('Ext.util.MixedCollection'); //数据缓存

        me.on('beforeload', function (store, operation, eOpts) {

            Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
            if (this.queryObj) {
                Ext.apply(store.proxy.extraParams, this.queryObj); //查询窗口的条件   
            }

            //            if (this.outQueryObj) {
            //                Ext.apply(store.proxy.extraParams, this.outQueryObj); //外部过滤条件 
            //            }

            if (store.cachePageData) {
                if (store.dataContainer.containsKey(store.currentPage)) {

                    var records = store.dataContainer.get(store.currentPage);
                    store.loadData(records);

                    //store.loadRawData(store.dataContainer.get(store.currentPage));//会修改总页数,分页条会变成只有1页

                    //var result = store.proxy.reader.read(store.dataContainer.get(store.currentPage));
                    //var records = result.records;
                    //store.loadRecords(records, { addRecords: false });

                    store.fireEvent('load', me, records, true); //触发load事件，更新分页条的页号

                    return false; //防止ajax请求去服务端读取数据
                }
            }

        });

        me.on('load', function (store, records, successful, eOpts) {


            if (records && store.cachePageData) {
                //debugger;
                var pageIndex = store.currentPage;
                store.dataContainer.add(pageIndex, records);
            }

        });

        if (me.cachePageData) {

            me.on('add', function (store, eOpts) {

                if (this.dataContainer) {
                    this.dataContainer.clear();
                }
            });

            me.on('remove', function (store, eOpts) {
                if (this.dataContainer) {
                    this.dataContainer.clear();
                }
            })
        }

    },
    clearPageData: function () {
        var me = this;
        if (me.dataContainer) {
            me.dataContainer.clear();
        }
    }

});

Ext.define('Ext.ng.TreePanel', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.ngTreePanel',
    //autoScroll: true,
    //enableTabScroll: true,
    rootVisible: false,
    lines: true,
    useArrows: true,
    //width: 220,
    minSize: 220,
    maxSize: 220,
    controlCheckState: false,//是否需要控制树的check的只读状态
    sumByLevel: false,//是否级次汇总    
    initComponent: function () {
        var me = this;
        me.setColumnAlign(me);

        if (me.controlCheckState) {
            //Ext.apply(me.treeFields, { name: 'disabled', type: 'bool' });//
            me.treeFields.push({ name: 'disabled', type: 'bool' });
            me.addNodeFunc();//添加disable,enable方法
        }
        var thestore = Ext.create('Ext.data.TreeStore', {
            autoLoad: me.autoLoad,
            root: {
                expanded: me.autoLoad
            },
            fields: me.treeFields,
            proxy: {
                type: 'ajax',
                url: me.url
            },
            sorters: me.sorters
        });
        me.store = thestore;
        me.callParent(arguments);

        var dataIndex; //记录当前列
        var indexArray = [];
        var findFlag = false;
        var findCount = 0;
        me.headerCt.on("headertriggerclick", function (container, header, e, t) {
            dataIndex = header.dataIndex;
        });

        //有lock列的grid必须在事件里面写添加按钮
        var menu = me.headerCt.on("menucreate", function (container, m) {
            m.add([{
                itemId: 'location',
                text: '定位',
                iconCls: 'icon-Location',
                handler: function () {


                    var pageCount = 1;
                    if (me.bbar) {
                        pageCount = Math.ceil(me.getStore().getTotalCount() / me.getStore().pageSize);
                    }
                    //显示弹出窗口
                    var win = Ext.create('Ext.window.Window', {
                        title: '定位',
                        border: false,
                        constrain: true,
                        height: 60,
                        width: 185,
                        layout: 'hbox',
                        modal: false,
                        fieldDefaults: {
                            labelWidth: 80,
                            anchor: '100%',
                            margin: '0 10 5 0',
                            msgTarget: 'side'
                        },
                        items: [{
                            itemId: 'loactionValue',
                            xtype: 'ngText'
                        }, {
                            xtype: 'button',
                            iconCls: 'icon-Location',
                            handler: function () {

                                var searchValue = this.up('window').queryById('loactionValue').getValue(); //Ext.getCmp('loactionValue').getValue();
                                var value = {};
                                value[dataIndex] = searchValue
                                //alert(dataIndex);
                                var index = 0;

                                if (!findFlag) {
                                    Ext.Array.each(me.getStore().data.items, function (record) {

                                        var obj = record.data;
                                        if (obj[dataIndex].indexOf(searchValue) > -1) {
                                            indexArray.unshift(index); //入列
                                            //indexArray.push(index); //入列，到结尾

                                            findCount++;
                                        }

                                        index++;
                                    });
                                    findFlag = true;
                                }

                                if (indexArray.length > 0) {
                                    if (me.getView().focusRow) {
                                        me.getView().focusRow(indexArray.pop());
                                    }
                                    else if (me.getView().normalView) {
                                        me.getView().normalView.focusRow(indexArray.pop());
                                    }
                                }
                                else {

                                    //if (me.getStore().currentPage < me.getStore().getTotalCount()) {
                                    if (me.getStore().currentPage < pageCount) {
                                        me.getStore().loadPage(me.getStore().currentPage + 1);
                                    }
                                    else {
                                        if (findCount == 0) {
                                            NGMsg.Info('定位不到数据');
                                        }
                                        me.getStore().loadPage(1);
                                    }

                                    findFlag = false;

                                }

                                //if (me.getStore().currentPage == pageCount) {
                                //    me.getStore().currentPage == 0;                                   
                                //}

                            }
                        }]
                    });

                    win.show();
                    setTimeout(function () { win.queryById('loactionValue').focus(); }, 200);
                }
            }]);
        });

        me.store.on('load', function () {
            var tree = me;
            if (tree.controlCheckState) {  
                setTimeout(function () { me.setCheckUI() }, 500);//0.5秒后设置UI
            }

        });

        me.on('checkchange', function (node, state) {

            var tree = me.store.ownerTree;
            if (tree.controlCheckState) {
                if (node.data.disabled) {
                    node.set('checked', !state);//不可选，设置回原来的值
                }
                //样式
                var checkBox = tree.getView().getNode(node).firstChild.firstChild.getElementsByTagName('input')[0];
                if (checkBox && node.data.disabled) {
                    checkBox.className += ' checkTree-mask';
                }
            }
        });
        
        //级次汇总        
        me.on('beforeedit', function (editor, e, eOpts) {

            //查看状态设置为只读
            if (me.otype === 'view') {
                return false;
            }
            else {
                if (e.column.readOnly) {//列是只读的
                    return false;
                }               
            }

            if (me.sumByLevel) {
                if (e.record.data.leaf) {
                    return true;
                }
                return false;
            }
            return true;
        });
       
        me.on('edit', function (editor, e, eOpts) {
            
            var fieldName = e.field;
            if (me.sumByLevelCols) {
                if (me.sumByLevelCols.indexOf(fieldName) > -1) {
                    var changeVal = e.value - e.originalValue;//变化值
                    ChangeParentNode(e.record.parentNode);
                    //级次汇总，递归更新父节点
                    function ChangeParentNode(record) {
                        if (!record.parentNode) return;//忽略root
                        var newVal = Number(record.data[fieldName]) + Number(changeVal);
                        //record.set(fieldName, newVal.toFixed(2));//调用set方法会出现红色提示，层数，列数多了就很卡,
                        record.data[fieldName] = newVal.toFixed(2);
                        if (record.parentNode) {
                            ChangeParentNode(record.parentNode);
                        }
                    }

                    me.getView().refresh();
                }
            }
        });
    },
    setColumnAlign: function (me) {
        if (me.columns) {
            //列标题对齐方式处理
            for (var i = 0; i < me.columns.length; i++) {
                var column = me.columns[i];
                if (column) {
                    if (!column.hidden) {
                        if (column.titleAlign) {
                            column.style = "text-align:" + column.titleAlign;
                        }
                        else {
                            column.style = "text-align:center"; //默认列头居中
                        }
                        column.hideable = true;//允许勾选
                        //必输列控制
                        if (column.mustInput) {
                            column.style += ";color:OrangeRed";
                        }
                    } else {
                        column.hideable = false;//彻底隐藏，不让勾选
                    }
                }

                //子列居中
                if (column.columns) {
                    for (var j = 0; j < column.columns.length; j++) {
                        var col = column.columns[j];
                        if (col.titleAlign) {
                            col.style = "text-align:" + col.titleAlign;
                        }
                        else {
                            col.style = "text-align:center"; //默认列头居中
                        }
                    }
                }
            }
        }
    },
    setCheckUI: function () {
        var me = this;

        var rootNode = me.getRootNode();
        findChildnode(rootNode);
        function findChildnode(node) {
            var childNodes = node.childNodes;
            for (var i = 0; i < childNodes.length; i++) {
                var currentNode = childNodes[i];

                //
                var checkBox = me.getView().getNode(currentNode).firstChild.firstChild.getElementsByTagName('input')[0];
                if (checkBox) {
                    if (currentNode.data.disabled) {//不可用个，设置样式为灰色
                        checkBox.className += ' checkTree-mask';
                    }
                }

                if (currentNode.childNodes.length > 0) {
                    findChildnode(currentNode);//递归
                }
            }
        };
    },
    getAllData: function (busKey) {
        var me = this;
        var newR = me.getStore().getNewRecords();
        if (newR.length == 0)
        { newR = [] }
        var updateR = me.getStore().getUpdatedRecords();

        var allRow = updateR.concat(newR);

        for (var i = 0; i < allRow.length; i++) {
            me.delUnUseProperty(allRow[i].data);
        }

        //格式化
        var data = GetDatatableData(allRow, [], [], [], busKey);
        
        return JSON.stringify(data);
    },
    getChange: function (serial) {
        var me = this;

        if (this.plugins != null && this.plugins.length > 0 && this.plugins[0]) {
            this.plugins[0].completeEdit();
        }

        var serialflag = serial;
        if (typeof (serial) == "undefined") {
            serialflag = true; //不传参数默认值是true，序列化
        }

        var treeStore = me.getStore();
        var newR = treeStore.getNewRecords();
        var updateR = treeStore.getUpdatedRecords();
        var deleteR = treeStore.getRemovedRecords();
        if (newR.length == 0)
        { newR = [] }
        if (updateR.length == 0)
        { updateR = [] }
        if (deleteR.length == 0)
        { deleteR = [] }


        for (var i = 0; i < newR.length; i++) {
            me.delUnUseProperty(newR[i].data);
        }

        for (var i = 0; i < updateR.length; i++) {
            if (updateR[i].data['id'] === 'root') {
                updateR.splice(i, 1);//忽略root节点数据
                continue;
            }
            me.delUnUseProperty(updateR[i].data);
        }

        //格式化
        var data = GetDatatableData(newR, updateR, deleteR, [], me.buskey);

        if (serialflag) {
            return JSON.stringify(data);
        }
        else {
            return data;
        }
    },
    hasModifyed: function () {
        var me = this;
        if (this.plugins != null && this.plugins.length > 0 && this.plugins[0]) {
            this.plugins[0].completeEdit();
        }
        var treeStore = me.getStore();       
        var newR = treeStore.getNewRecords();
        var updateR = treeStore.getUpdatedRecords();
        var deleteR = treeStore.getRemovedRecords();

        if (newR.length > 0 || updateR.length > 0 || deleteR.length > 0) {
            return true;
        }
        return false;
    },
    loadData: function (treeData) {
        var me = this;
        var rootNode = me.getRootNode();
        rootNode.removeAll();
        builNode(rootNode, treeData);

        function builNode(currentNode, children) {           
            if (children && children.length > 0) {
                for (var i = 0; i < children.length; i++) {
                    var nodeData = children[i];
                    var currentNode = rootNode.appendChild(nodeData);
                    builNode(currentNode, nodeData.children);
                }             
            }
        }
    },
    loadByUrl: function (url) {
        var me = this;
        me.getRootNode().removeAll();
        me.getStore().setProxy(
             {
                 type: 'ajax',
                 url: url
             });
        me.getStore().load();
    },
    setReadOnlyCol: function (colname, flag) {
        var col = this.getColumn(colname);

        if (flag || typeof (flag) == "undefined") {
            col.readOnly = true;//与grid的beforeedit事件配合完成
        }
        else {
            col.readOnly = false;
        }     
    },
    getColumn: function (colname) {
        var me = this;
        var obj;
        if (me.lockable) {
            for (var i = 0; i < me.lockedGrid.columns.length; i++) {
                if (colname === me.lockedGrid.columns[i].dataIndex) {
                    obj = me.lockedGrid.columns[i]; break
                }
            }

            if (obj) return obj;

            for (var i = 0; i < me.normalGrid.columns.length; i++) {
                if (colname === me.normalGrid.columns[i].dataIndex) {
                    obj = me.normalGrid.columns[i]; break
                }
            }
            return obj;
        } else {
            for (var i = 0; i < me.columns.length; i++) {
                if (colname === me.columns[i].dataIndex) {
                    obj = me.columns[i]; break
                }
            }
            return obj;
        }

    },
    addNodeFunc: function () {

        //扩展node节点的功能
        if (!Ext.data.Model.prototype.enable) {
            Ext.apply(Ext.data.Model.prototype, {
                disable: function () {
                    var me = this;
                    me.set('disabled', true);

                    var tree = me.store.ownerTree;
                    var checkBox = tree.getView().getNode(me).firstChild.firstChild.getElementsByTagName('input')[0];
                    checkBox.className = checkBox.className.replace(' checkTree-mask', '') + ' checkTree-mask';
                },
                enable: function () {
                    var me = this;
                    me.set('disabled', false);
                    var tree = me.store.ownerTree;
                    var checkBox = tree.getView().getNode(me).firstChild.firstChild.getElementsByTagName('input')[0];
                    checkBox.className = checkBox.className.replace(' checkTree-mask', '');
                }
            });
        }
    },
    delUnUseProperty: function (data) {//树节点特有属性不需要传到服务端
        delete data.allowDrag;
        delete data.allowDrop;
        delete data.children;
        delete data.index;
        delete data.depth;
        delete data.expanded;
        delete data.expandable;
        //delete data.checked;
        delete data.leaf;
        delete data.cls;
        delete data.iconCls;
        delete data.icon;
        delete data.root;
        delete data.isLast;
        delete data.isFirst;
        delete data.loaded;
        delete data.href;
        delete data.hrefTarget;
        delete data.qtip;
        delete data.qtitle;
        delete data.qshowDelay;
        delete data.qtitle;
    },
    isValid: function ()
    {
        return true;
        //var me = this;
        //return ValidTreeGridData(me);
    }
});

Ext.define('Ext.ng.AttachMent', {
    extend: 'Ext.window.Window',
    title: '附件',
    autoScreen: true,
    iconCls: 'icon-Attachment',
    closable: true,
    maximizable: false,
    resizable: true,
    //modal: true,
    //draggable:false,
    width: 835,
    height: 335,
    layout: 'fit',
    attachTName: 'c_pfc_attachment',
    btnAdd: '1',
    btnWebOk: '2', //不显示确定按钮，单据提交的时候才保存附件
    btnScan: '0',
    btnDelete: '0',
    btnEdit: '0',
    btnView: '0',
    btnDownload: '0',
    autoSave: '1',
    status: 'edit',
    busTName: '',
    busID: '',
    initComponent: function () {
        var me = this;

        if (me.busTName.length == 0) {
            alert("请设置业务表名属性busTName！");
            me.callParent(arguments);
            return;
        }
        //        if (me.busID.length == 0) {
        //            alert("请设置业务主键属性busID！");
        //            me.callParent(arguments);
        //            return;
        //        }

        var attachguid = '';
        if (me.attachGuid) {
            attachguid = me.attachGuid;
        } else {
            //attachguid = me.busID;//不同的单据phid一样的，单据之间的附件乱串了
        }


        var frame = document.createElement("IFRAME");
        frame.id = "frame1";
        frame.frameBorder = 0;
        frame.src = C_ROOT + 'SUP/Attachment/Index?attachTName=' + me.attachTName + '&btnAdd='
									 + me.btnAdd + '&status=' + me.status + '&busTName=' + me.busTName
									 + '&busid=' + me.busID + '&attachguid=' + attachguid
									 + '&btnWebOk=' + me.btnWebOk + '&autoSave=' + me.autoSave
                                     + '&btnScan=' + me.btnScan + '&btnDelete=' + me.btnDelete
                                     + '&btnEdit=' + me.btnEdit + '&btnView=' + me.btnView
                                     + '&btnDownload=' + me.btnDownload;
        if (me.fileNum) {
            frame.src += "&fileNum=" + (me.fileNum || "0");
        }

        frame.height = "100%";
        frame.width = "100%";
        me.contentEl = frame;

        me.callParent(arguments);

        frame.parentContainer = me;
        me.on('beforeclose', function () {

            var att = me.contentEl.contentWindow.LoadAttach;
            me.returnObj = { guid: att.GUID, status: att.STATUS };

        });
    },
    listeners: {
        show: function (me, eOpts) {
            me.resizable = false;//active控件拖动消失，初始化时是true禁止拖动
        }
    },
    Save: function (buscode) {

        var me = this;

        Ext.Ajax.request({
            params: { 'attachguid': me.returnObj.guid, 'buscode': buscode },
            url: C_ROOT + 'SUP/Attachment/Save',
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {


                } else {
                    Ext.MessageBox.alert('保存失败:' + resp.msg);
                }
            }
        });

    },
    ClearTempData: function () {
        var me = this;
        Ext.Ajax.request({
            params: { 'attachguid': me.returnObj.guid },
            url: C_ROOT + 'SUP/Attachment/ClearTempData',
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {


                } else {
                    Ext.MessageBox.alert('保存失败:' + resp.msg);
                }
            }
        });
    },
    GetAttachmentInfo: function (callback) {
        var me = this;
        Ext.Ajax.request({
            params: {
                'guid': me.returnObj.guid
            },
            url: C_ROOT + 'SUP/Attachment/GetAttachmentInfo',
            success: function (response) {
                callback(response);
            }
        });
    }
});

Ext.define('Ext.ng.MessageBox', {

    /*******************************
    错误对话框,提示性,非模态,一定时间后自动消失
    msg:对话框消息体
    hold:对话框显示持续时间
    ********************************/
    Error: function (msg, hold, fuc) {
        //this.Show('错误', msg, hold, 'alert-error');
        this.Show('错误', msg, hold, Ext.MessageBox.ERROR, fuc);
    },

    /*******************************
    消息对话框,提示性,非模态,一定时间后自动消失
    msg:对话框消息体
    hold:对话框显示持续时间
    ********************************/
    Info: function (msg, hold, fuc) {
        //this.Show('提示', msg, hold, 'alert-information');
        this.Show('提示', msg, hold, Ext.MessageBox.INFO,fuc);
    },

    /*******************************
    警告对话框,提示性,非模态,一定时间后自动消失
    msg:对话框消息体
    hold:对话框显示持续时间
    ********************************/
    Warn: function (msg, hold, fuc) {
        //this.Show('警告', msg, hold, 'alert-warn');
        this.Show('警告', msg, hold, Ext.MessageBox.WARNING, fuc);
    },

    /*******************************
    成功对话框,提示性,非模态,一定时间后自动消失
    msg:对话框消息体
    hold:对话框显示持续时间
    ********************************/
    Success: function (msg, hold, fuc) {
        //this.Show('提示', msg, hold, 'alert-success');
        this.Show('提示', msg, hold, Ext.MessageBox.INFO,fuc);
    },

    /*******************************
    确认对话框,提示性,模态,不会自动消失
    返回值 yes/no
    msg:对话框消息体
    ********************************/
    Confirm: function (msg,fn, scope) {
        Ext.MessageBox.confirm('Confirm', msg, fn, scope);
        this.ajustPosition();
    },

    /*******************************
    带输入的对话框,提示性,模态,不会自动消失
    返回值 输入的value
    title:标题栏
    msg:提示性信息
    ********************************/
    Prompt: function (title, msg, fn, scope, multiline, value) {
        Ext.MessageBox.prompt(title, msg, fn, scope, multiline, value);
        this.ajustPosition();
    },

    /*******************************
    带多行输入的对话框,提示性,模态,不会自动消失
    返回值 输入的value
    title:标题栏
    msg:提示性信息
    width:消息框宽度
    ********************************/
    MultilinePrompt: function (title, msg, width) {
        Ext.MessageBox.show({
            title: title,
            msg: msg,
            width: width,
            buttons: Ext.MessageBox.OKCANCEL,
            multiline: true,
            icon: 'alert-information'
        });
    },

    /*******************************
    带按钮的对话框,提示性,模态,不会自动消失
    返回值 点击的按钮对应的值
    title:标题栏
    msg:提示性信息
    buttons:要显示的按钮,如Ext.MessageBox.YESNOCANCEL,
    ********************************/
    ButtonDialog: function (title, msg, buttons) {
        Ext.MessageBox.show({
            title: title,
            msg: msg,
            buttons: buttons,
            icon: 'alert-information'
        });
    },

    /*******************************
    进度条对话框,提示性,模态,处理完成后自动消失
    返回值 
    title:标题栏
    msg:提示性信息
    progressText:进度条上要显示的信息
    ********************************/
    ProgressDialog: function (title, msg, progressText) {
        Ext.MessageBox.show({
            title: title,
            msg: msg,
            progressText: progressText,
            width: 300,
            progress: true,
            closable: false,
            icon: 'alert-information'
        });

        // this hideous block creates the bogus progress
        var f = function (v) {
            return function () {
                if (v == 12) {
                    Ext.MessageBox.hide();
                    Ext.example.msg('Done', 'Your fake items were loaded!');
                }
                else {
                    var i = v / 11;
                    Ext.MessageBox.updateProgress(i, Math.round(100 * i) + '% completed');
                }
            };
        };
        for (var i = 1; i < 13; i++) {
            setTimeout(f(i), i * 500);
        }
    },

    /*******************************
    等待对话框,提示性,模态,不会自动消失
    返回值 点击的按钮对应的值
    msg:提示性信息
    progressText:进度条上要显示的信息
    ********************************/
    WaitDialog: function (msg, progressText) {
        Ext.MessageBox.show({
            msg: msg,
            progressText: progressText,
            width: 300,
            wait: true,
            waitConfig: { interval: 200 },
            icon: 'alert-information'
        });
    },

    /*******************************
    提示对话框,提示性,非模态,一定时间后自动消失
    title:对话框标题
    msg:对话框消息体
    hold:对话框显示持续时间
    icon:要显示的图片
    ********************************/
    Show: function (title, msg,hold, icon,fuc) {

        Ext.MessageBox.show({
            buttons: Ext.Msg.OK,
            title: title,
            msg: msg,
            width: 300,
            icon: icon,
            fn: function (btn) {               
                if ('ok' === btn) {
                    if (fuc) {
                        fuc();
                    }
                    Ext.MessageBox.close();                   
                }
            },
            modal: false          
        });

        //ocx控件处理
        Ext.MessageBox.on('beforeshow', $winBeforeShow);
        Ext.MessageBox.on('beforeclose', $winBeforeClose);

        this.ajustPosition();                 

        //var msgBox = Ext.create('Ext.window.MessageBox', {          
        //    listeners: {
        //        beforeshow: $winBeforeShow,
        //        beforeclose: $winBeforeClose
        //    }
        //});

        //msgBox.show({
        //    buttons: Ext.Msg.OK,
        //    title: title,
        //    msg: msg,
        //    width: 300,
        //    icon: icon,
        //    modal: false
        //});

        //msgBox.setXY([msgBox.getX(), msgBox.getY() - 80]);

        this.Hold(hold, Ext.MessageBox);
    },
    Hold: function Hold(hold, msgBox) {
        var h = 10;//10秒钟       
        if (hold != undefined) {
            h = hold;
        }       
        setTimeout(function () {
            //Ext.MessageBox.hide();
            msgBox.close();
        }, h * 1000);
    },
    ajustPosition: function () {
        Ext.MessageBox.setXY([Ext.MessageBox.getX(), Ext.MessageBox.getY() - 80]);//设置位置    
    }
}, function () {    
    NGMsg = new Ext.ng.MessageBox();//初始化NGMsg对象
});

//#region 布局记忆
Ext.define('Ext.ng.state.HttpProvider',
	{
	    extend: 'Ext.state.Provider',

	    //构造函数
	    constructor: function (config) {
	        config = config || {};
	        var me = this;
	        Ext.apply(me, config);

	        this.superclass.constructor.call(this);
	        this.state = this.readValues();
	    },

	    //缓存地址
	    url: '',

	    // private
	    set: function (name, value) {
	        if (typeof value == "undefined" || value === null) {
	            this.clear(name);
	            return;
	        }

	        if (this.state[name] != value) {
	            this.setValue(name, value);
	        }

	        this.superclass.set.call(this, name, value);
	    },

	    // 清空数据
	    clear: function (name) {
	        this.clearValue(name);
	        this.superclass.clear.call(this, name);
	    },

	    // 读取指定用户的全部布局数据
	    readValues: function () {
	        var state = {};
	        var me = this;

	        if ($gridStateInfo) {
	            var stateInfo = Ext.htmlDecode($gridStateInfo);
	            if (!Ext.isEmpty(stateInfo)) {
	                var json = Ext.decode(stateInfo);//从viewBag获取,性能优化
	                if (json && json.length > 0) {

	                    for (var i = 0; i < json.length; i++) {
	                        if (json[i].Bustype != "") {
	                            state[json[i].Bustype] = me.decodeValue(json[i].Value);
	                        }

                            //获取stateid对应的pagesize,读取布局数据会不会对之前的有影响??
	                        if (json[i].Pagesize != "") {
	                            state["Pagesize"] = json[i].Pagesize;
	                        }
	                        if (json[i].Gid != "") {
	                            state["Gid"] = json[i].Gid;
	                        }

	                    }
	                }
	            }
	        }
	        else {
	            //Ext.Ajax.request({
	            //    url: C_ROOT + 'SUP/LayoutLog/ReadLayout', //this.url + '/ReadLayout',
	            //    async: false,
	            //    params: {},
	            //    success: function (res, opts) {
	            //        if (res.responseText) {
	            //            var json = Ext.decode(res.responseText);
	            //            if (json && json.length > 0) {

	            //                for (var i = 0; i < json.length; i++) {
	            //                    if (json[i].Bustype != "") {
	            //                        state[json[i].Bustype] = me.decodeValue(json[i].Value);
	            //                    }

	            //                }
	            //            }
	            //        }
	            //    }
	            //});
	        }

	        return state;
	    },

	    // 保存数据
	    setValue: function (name, value) {	       
	        var me = this;
	        Ext.Ajax.request({
	            url: C_ROOT + 'SUP/LayoutLog/SaveLayout', //this.url + '/SaveLayout',
	            params: { bustype: name, layoutValue: me.encodeValue(value) },
	            success: function (res, opts) {
	            }
	        });
	    },

	    // 清空数据
	    clearValue: function (name) {
	        Ext.Ajax.request({
	            async: false, //同步请求，解决恢复默认布局不起效果
	            url: C_ROOT + 'SUP/LayoutLog/ClearLayout', //this.url + '/ClearLayout',
	            params: { bustype: name },
	            success: function (res, opts) {
	            }
	        });
	    }
	});

//暂时不启动

Ext.state.Manager.setProvider(Ext.create('Ext.ng.state.HttpProvider',{ url: '../Sup' }));

//#endregion

// #region Ext.ng.form.field

//基类
Ext.define('Ext.ng.form.field.Base', {
    extend: 'Ext.form.field.Base',
    alias: ['widget.ngFieldBase'],
    textChange: false,
    //cls:'ng-x-form-text',
    initComponent: function () {
        var me = this;
        me.callParent();
        
        if (me.mustInput)//必输项
        {
            //me.labelStyle = 'color:RoyalBlue';
            me.labelStyle = 'color:OrangeRed'; //'color:RoyalBlue';
            me.allowBlank = false;
        }

        me.on('afterrender', function (panel, eOpts) {

            //checkbox不能变色,否则只读时勾勾看不见
            if ('ngCheckbox' === me.xtype) return;
            if ('ngRadio' === me.xtype) return;
            
            if (me.readOnly) {
                //me.setReadOnly(true);
                var input = me.el.down('input') || me.el.down('textarea');
                //input.setStyle({ backgroundImage: 'none', backgroundColor: '#eaeaea' });
                input.setStyle({ backgroundImage: 'none' });
                me.preventMark = true;

                //处理控件变短
                var errmsg = me.el.down('div.x-form-error-msg')
                if (errmsg) {
                    errmsg.up('td').setStyle({ display: 'none' });
                }
            }
            
            //样式
            //var input = this.el.down('input');
            //if (input && input.dom.type === 'text' && Theme != 'neptune') {
            //    input.dom.setAttribute('class', 'x-form-field ngx-form-text');
            //}

        });

        me.addEvents('itemchanged'); //模仿pb做itemchanged事件
        me.on('change', function () {
            me.textChange = true;
        });
        me.on('blur', function () {
            if (me.textChange) {

                me.fireEvent('itemchanged');
                me.textChange = false;
            }

            //if (!me.readOnly) {
            //    var input = this.el.down('input');
            //    if (input && input.dom.type === 'text' && Theme != 'neptune') {
            //        input.dom.setAttribute('class', 'x-form-field ngx-form-text');
            //    }
            //}
        });

        //add by ljy 20161215 避免打开编辑窗口时就触发itemchanged事件
        me.on('focus', function () {
            me.textChange = false;

            //if (!me.readOnly) {
            //    //设置样式
            //    var input = this.el.down('input');
            //    if (input && input.dom.type === 'text' && Theme != 'neptune') {
            //        input.dom.setAttribute('class', 'x-form-text-focus');
            //    }
            //}
        });

    },
    userSetReadOnly: function (flag) {
        this.setReadOnly(flag);
        this.preventMark = flag;
        if (this.el) {
            var input = this.el.down('input') || this.el.down('textarea');
            if ('ngCheckbox' != this.xtype && 'ngRadio' != this.xtype) {//checkBox和radio不控制颜色
                if (flag) {
                    input.setStyle({ backgroundImage: 'none' });
                }
                else {
                    input.setStyle({ backgroundColor: 'white' });
                }
            }
            //处理控件变短
            var errmsg = this.el.down('div.x-form-error-msg')
            if (errmsg) {
                errmsg.up('td').setStyle({ display: 'none' });
            }
        }
    },
    userSetMustInput: function (flag) {
        var input = this.el.down('label');

        if (flag) {
            input.setStyle({ color: 'OrangeRed' });
            this.allowBlank = false;
        }
        else {
            input.setStyle({ color: 'Black' });
            this.allowBlank = true;
        }
    },
    userSetMask: function (flag) {
        var me = this;        
        if (typeof (flag) == "undefined") {
            flag = true;
        }
        
        me.userSetReadOnly(flag);
        var input = this.el.down('input') || this.el.down('textarea');       
        if (flag) {
            if (!me.isMask) {               
                //input.dom.type = 'password';//借用密码框
                //input.dom.style.zIndex = 0;
                //input.dom.style.color = "#ffffff";//数字控件靠右显示，点击span，数字会显示出来
                //input.dom.style.textIndent = "-999px";

                //var span = document.createElement("span");
                //span.className = "text-Mask";
                //span.innerHTML = "***";
                //input.dom.parentNode.insertBefore(span, input.dom);
                input.dom.style.display = 'none';

                var maskInput = document.createElement("input");
                maskInput.className = input.dom.className;
                maskInput.style.width = '100%';
                maskInput.readOnly = true;
                maskInput.value = '***';
                maskInput.name = "maskInput";
                if (me.isMarginRight) {
                    maskInput.style.textAlign = "right";
                }
                //input.dom.parentNode.insertBefore(maskInput, input.dom);
                input.dom.parentNode.appendChild(maskInput);
                me.isMask = true;
            }
        }
        else {
            if (me.isMask) {
                input.dom.style.display = 'inline';
                //input.dom.type = 'text';//放开密码框
                //input.dom.style.zIndex = 1;
                //input.dom.style.textIndent = "0px";
                //input.dom.style.color = "#333f49";//字体颜色弄回来  

                //var span = this.el.down('span');
                //input.dom.parentNode.removeChild(span.dom);
                var maskInput = this.el.down('[name=maskInput]');
                input.dom.parentNode.removeChild(maskInput.dom);
                me.isMask = false;
            }
        }
    }
})

//基于xml配置的通用帮助
Ext.define('Ext.ng.CommonHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngCommonHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 600, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    selectMode: 'Single', //multiple  
    ORMMode: false,
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    triggerCls: 'x-form-help-trigger',
    queryMode: 'remote',
    triggerAction: 'all', //'query'
    myCodeVal: '',
    initComponent: function () {
        //
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.editable) {
            if (me.listFields && me.listHeadTexts) {

                var listheaders = '';
                var listfields = '';

                var heads = me.listHeadTexts.split(','); //列头 
                var fields = me.listFields.split(','); //所有字段              

                var modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp //fields[i]
                    });

                }

                if (me.showAutoHeader) {

                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    listfields += '<td>{' + temp + '}</td>';
                }

                var temp;
                if (me.showAutoHeader) {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                } else {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }
                me.tpl = temp;


            }
            else {
                //me.initialListTemplate(); //初始化下拉列表样式 
                var tempfield = me.valueField.split('.');
                var valueField;
                if (tempfield.length > 1) {
                    valueField = tempfield[1]; //去掉表名
                }
                else {
                    valueField = me.valueField;
                }

                var dfield = me.displayField.split('.');
                var displayField;
                if (dfield.length > 1) {
                    displayField = dfield[1]; //去掉表名
                }
                else {
                    displayField = me.displayField;
                }

                var modelFields = [{
                    name: valueField,
                    type: 'string',
                    mapping: valueField
                }, {
                    name: displayField,
                    type: 'string',
                    mapping: displayField
                }]

                var listfields = '<td>{' + valueField + '}</td>';
                listfields += '<td>{' + displayField + '}</td>';
                me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';

            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10,
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }
        }

        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');

        me.on('select', function (combo, records, eOpts) {

            var theField;

            //构建
            if (me.listFields) {
                theField = [];
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);

                    //                        var tempfield = temp[i].split('.');
                    //                        if (tempfield.length > 1) {
                    //                            theField.push(tempfield[1]);
                    //                        }
                    //                        else {
                    //                            theField.push(temp[i]);
                    //                        }
                }
            }
            else {
                //                    theField = [];
                //                    var temp = me.valueField.split('.');
                //                    if (temp.length > 1) {
                //                        theField.push(temp[1]);
                //                    } else {
                //                        theField.push(me.valueField);
                //                    }

                //                    temp = me.displayField.split('.');
                //                    if (temp.length > 1) {
                //                        theField.push(temp[1]);
                //                    } else {
                //                        theField.push(me.displayField);
                //                    }

                theField = [me.valueField, me.displayField];
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            var code = combo.getValue() || records[0].data[myValueFiled];
            var name = combo.getRawValue() || records[0].data[myDisplayField];

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = (name === '&nbsp;') ? '' : name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            me.setValue(valuepair); //必须这么设置才能成功

            //debugger;

            me.myCodeVal = code;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    var str = records[0].data[temp[1]];
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
                else {
                    var str = records[0].data[theField[i]]
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {


            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });

        //合法验证
        me.on('blur', function () {
        	var value = me.getValue();
            if (value == "")
                return;
            value = encodeURI(value);
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/CommonHelp/ValidateData?helpid=' + me.helpid + '&inputValue=' + value + '&selectMode=' + me.selectMode,
        	    params: { 'clientSqlFilter': this.clientSqlFilter, 'helptype': 'ngCommonHelp' },
        	    async: false, //同步请求
        	    success: function (response) {
        	        var resp = Ext.JSON.decode(response.responseText);
        	        if (resp.Status === "success") {
        	            if (resp.Data == false) {
        	                me.setValue('');
        	            }        	          
        	        } else {
        	            Ext.MessageBox.alert('取数失败', resp.status);
        	        }
        	    }
        	});
        });

        //双击打开基础数据窗口
        me.on('render', function (combo, eOpts) {
            var input = this.el.down('input');
            if (input) {
                input.dom.ondblclick = function () {
                    
                    var codeVal;
                    if (!Ext.isEmpty(me.getValue())) {
                        if (me.isInGrid) {
                            var grid;
                            if (me.grid) {
                                grid = me.grid;
                            } else if (me.gridID) {
                                grid = Ext.getCmp(me.gridID);
                            }

                            var data = grid.getSelectionModel().getSelection();
                            codeVal = data[0].get(me.codeColumn);
                        }
                        else {
                            codeVal = me.getValue();
                        }
                        //alert(codeVal);
                    }

                };               

            }
        });
    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (me.helpType === 'rich') {//用户自定义界面的模板 

                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;

                if (me.showAutoHeader) {
                    for (var i = 0; i < gridColumns.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + gridColumns[i].header + '</th>';
                    }
                }

                for (var i = 0; i < modelFields.length; i++) {
                    listfields += '<td>{' + modelFields[i]['name'] + '}</td>';
                }

            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头 

                if (me.showAutoHeader) {
                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp//fields[i]
                    });

                }
            }


            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }
        else {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }


        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });



    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击           

        if (Ext.isEmpty(me.helpid)) return;
        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;

        ShowHelp = function () {

            var queryItems;
            var modelFields;
            var gridColumns;

            if (me.helpType === 'rich') {//用户自定义界面的模板            
                queryItems = template.Template.QueryItems;
                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;
            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头


                queryItems = new Array();
                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp = fields[i];;
                    //                    if (tempfield.length > 1) {
                    //                        temp = tempfield[1]; //去掉表名
                    //                    }
                    //                    else {
                    //                        temp = fields[i];
                    //                    }

                    queryItems.push({
                        xtype: 'textfield',
                        fieldLabel: heads[i],
                        name: temp //fields[i]
                        //anchor: '95%'
                    });
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: fields[i], //不去掉表名
                        type: 'string',
                        mapping: temp
                    });
                }

                gridColumns = new Array();
                //                for (var i = 0; i < fields.length; i++) {

                //                    var tempfield = fields[i].split('.');
                //                    var temp;
                //                    if (tempfield.length > 1) {
                //                        temp = tempfield[1]; //去掉表名
                //                    }
                //                    else {
                //                        temp = fields[i];
                //                    }

                //                    gridColumns.push({
                //                        header: heads[i],
                //                        flex: 1,
                //                        //sortable: true,
                //                        dataIndex: temp//fields[i]
                //                    });
                //                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    gridColumns.push({
                        header: heads[i],
                        flex: 1,
                        //sortable: true,
                        dataIndex: fields[i]
                    });
                }
            }

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    id: "help_query",
								    text: "查询",
								    iconCls: 'add'
								},
								{
								    id: 'help_show',
								    text: '隐藏',
								    iconCls: 'icon-fold',
								    handler: function () {
								        if (this.iconCls == 'icon-unfold') {
								            this.setIconCls('icon-fold');
								            this.setText("隐藏");
								            querypanel.show();
								        } else {
								            this.setIconCls('icon-unfold');
								            this.setText("显示");
								            querypanel.hide();
								        }
								    }
								},
								 "->",
							   {
							       id: "help_close",
							       text: "关闭",
							       iconCls: 'cross'
							   }
							 ]
            });

            querypanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'north',
                //hidden: true,
                bodyStyle: 'padding:3px',
                fields: queryItems
            })

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'model',
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            //store.load();//这里load，IE的界面会扭掉

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store
            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid;
            //多选
            if (me.selectMode === 'Multi') {

                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    selModel: selModel, //多选
                    columnLines: true,
                    columns: gridColumns,
                    bbar: pagingbar
                });
            }
            else {//单选
                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    //autoScroll:true,
                    columnLines: true,
                    //                    layout: {
                    //                        type: 'hbox', //这个会出现横向滚动条，难看
                    //                        align: 'stretch'
                    //                    },
                    columns: gridColumns,
                    bbar: pagingbar
                });
            }

            grid.on('itemdblclick', function () {

                //
                var data = grid.getSelectionModel().getSelection();

                if (data.length > 0) {

                    var valarr = me.valueField.split('.');
                   var valtemp = me.valueField;
                   if (valarr.length > 1) {
                       valtemp = valarr[1];
                   }

                   var namearr = me.displayField.split('.');
                   var nametemp = me.displayField;
                   if (namearr.length > 1) {
                       nametemp = namearr[1];
                   }

                   var code = data[0].get(valtemp);
                   var name = data[0].get(nametemp);

                    var obj = new Object();
                    obj[valtemp] = code;

                    if (me.displayFormat) {
                        obj[nametemp] = Ext.String.format(me.displayFormat, code, name);
                    } else {
                        obj[nametemp] = (name === '&nbsp;') ? '' : name;
                    }

                    var valuepair = Ext.ModelManager.create(obj, 'model');

                    me.setValue(valuepair); //必须这么设置才能成功

                    win.hide();
                    win.close();
                    win.destroy();

                    //if (me.isInGrid) {

                    me.myCodeVal = code;
                    var pobj = new Object();
                    pobj.code = code;
                    pobj.name = name;
                    pobj.type = 'fromhelp';
                    pobj.data = data[0].data;

                    //空值修正
                    for (var p in pobj.data) {
                        if (pobj.data[p] && pobj.data[p] === '&nbsp;') {
                            pobj.data[p] = '';
                        }
                    }

                    me.fireEvent('helpselected', pobj);
                    //}

                }
            }, this)

            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,                
                constrain: true,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                modal: true,
                items: [toolbar, querypanel, grid]
            });

            win.show();

            //store.load();//手工调不会触发beforeload事件


            toolbar.items.get('help_query').on('click', function () {
                store.loadPage(1);
            })

            toolbar.items.get('help_close').on('click', function () {

                win.hide();
                win.close();
                win.destroy();

            })

            store.on('beforeload', function () {
                var formdata = querypanel.getForm();
                var data = formdata.getValues();

                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }

                //debugger;
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data), 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                else {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data) });
                }

                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

                //return true;
            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

        };

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }
        else {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }


        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {



                    if (me.helpType === 'rich') {
                        title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });


    },
    showHelp: function () {
        this.onTriggerClick();
    },
    bindData: function () {
        var me = this;

        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);

        return;

        //        Ext.Ajax.request({
        //            params: { 'outqueryfilter': me.outFilter },
        //            url: C_ROOT + 'SUP/CommonHelp/GetName?helpid=' + me.helpid + '&code=' + me.getValue(),
        //            success: function (response) {

        //                var resp = Ext.JSON.decode(response.responseText);
        //                if (resp.status === "ok") {

        //                    Ext.define('model', {
        //                        extend: 'Ext.data.Model',
        //                        fields: [{
        //                            name: me.valueField, //'code',
        //                            type: 'string',
        //                            mapping: me.valueField//'code'
        //                        }, {
        //                            name: me.displayField, //'name',
        //                            type: 'string',
        //                            mapping: me.displayField//'name'
        //                        }
        //                     ]
        //                    });

        //                    if (!Ext.isEmpty(resp.name)) {
        //                        var obj = new Object();
        //                        obj[me.valueField] = me.getValue();

        //                        if (me.displayFormat) {
        //                            obj[me.displayField] = Ext.String.format(me.displayFormat, me.getValue(), resp.name);
        //                        } else {
        //                            obj[me.displayField] = resp.name; //显示值
        //                        }

        //                        var provincepair = Ext.ModelManager.create(obj, 'model');
        //                        me.setValue(provincepair);
        //                    }

        //                } else {
        //                    Ext.MessageBox.alert('取数失败', resp.status);
        //                }
        //            }
        //        });

    }, //bindData
    getCodeName: function (value) {
        var me = this;
        var name;

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/CommonHelp/GetName?helptype=Single&helpid=' + me.helpid + '&code=' + value,
            async: false, //同步请求
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    name = resp.name; //显示值                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                    name = 'error';
                }
            }
        });

        return name;
    },
    setOutFilter: function (obj) {
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        this.clientSqlFilter = str;
    },
    getFirstRowData: function () {
        var me = this;
        var fields = me.listFields.split(',');

        var modelFields = new Array();
        for (var i = 0; i < fields.length; i++) {

            var tempfield = fields[i].split('.');
            var temp;
            if (tempfield.length > 1) {
                temp = tempfield[1]; //去掉表名
            }
            else {
                temp = fields[i];
            }

            modelFields.push({
                name: fields[i],
                type: 'string',
                mapping: temp
            });
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: modelFields
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 20,
            autoLoad: false,
            url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function () {

            //            var data = new Object();
            //            data[me.valueField] = value;

            if (me.outFilter) {
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
            if (me.firstRowFilter) {
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.firstRowFilter) });
            }

        })

        store.load(function () {
            var data = store.data.items[0].data;
            me.fireEvent('firstrowloaded', data);
        });


    }
});

//自定义表单帮助
Ext.define('Ext.ng.CustomFormHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngCustomFormHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 600, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    selectMode: 'Single', //multiple  
    ORMMode: false,
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    triggerCls: 'x-form-help-trigger',
    queryMode: 'remote',
    triggerAction: 'all', //'query'  
    initComponent: function () {
        //
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.editable) {
            if (me.listFields && me.listHeadTexts) {

                var listheaders = '';
                var listfields = '';

                var heads = me.listHeadTexts.split(','); //列头 
                var fields = me.listFields.split(','); //所有字段              

                var modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp //fields[i]
                    });

                }

                if (me.showAutoHeader) {

                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    listfields += '<td>{' + temp + '}</td>';
                }

                var temp;
                if (me.showAutoHeader) {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                } else {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }
                me.tpl = temp;                                

            }
            else {
                //me.initialListTemplate(); //初始化下拉列表样式 

                var tempfield = me.valueField.split('.');
                var valueField;
                if (tempfield.length > 1) {
                    valueField = tempfield[1]; //去掉表名
                }
                else {
                    valueField = me.valueField;
                }

                var dfield = me.displayField.split('.');
                var displayField;
                if (dfield.length > 1) {
                    displayField = dfield[1]; //去掉表名
                }
                else {
                    displayField = me.displayField;
                }

                var modelFields = [{
                    name: valueField,
                    type: 'string',
                    mapping: valueField
                }, {
                    name: displayField,
                    type: 'string',
                    mapping: displayField
                }]

                var listfields = '<td>{' + valueField + '}</td>';
                listfields += '<td>{' + displayField + '}</td>';
                me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10,
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CustomHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }
        }

        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');
        me.addEvents('beforetriggerclick');
        me.addEvents('beforehelpclose');

        me.on('select', function (combo, records, eOpts) {

            var theField;

            //构建
            if (me.listFields) {
                theField = [];
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);

                    //                        var tempfield = temp[i].split('.');
                    //                        if (tempfield.length > 1) {
                    //                            theField.push(tempfield[1]);
                    //                        }
                    //                        else {
                    //                            theField.push(temp[i]);
                    //                        }
                }
            }
            else {
                //                    theField = [];
                //                    var temp = me.valueField.split('.');
                //                    if (temp.length > 1) {
                //                        theField.push(temp[1]);
                //                    } else {
                //                        theField.push(me.valueField);
                //                    }

                //                    temp = me.displayField.split('.');
                //                    if (temp.length > 1) {
                //                        theField.push(temp[1]);
                //                    } else {
                //                        theField.push(me.displayField);
                //                    }

                theField = [me.valueField, me.displayField];
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            var code = combo.getValue() || records[0].data[myValueFiled];
            var name = combo.getRawValue() || records[0].data[myDisplayField];

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = (name === '&nbsp;') ? '' : name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            me.setValue(valuepair); //必须这么设置才能成功

            //debugger;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    var str = records[0].data[temp[1]];
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
                else {
                    var str = records[0].data[theField[i]]
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {


            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });

        me.on('change', function () {

        });


        //合法验证
        if (me.editable) {
            me.on('blur', function () {
                var value = me.getValue();
                if (value == "")
                    return;
                value = encodeURI(value);
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/CustomHelp/ValidateData?helpid=' + me.helpid + '&inputValue=' + value + '&selectMode=' + me.selectMode,
                    params: { 'clientSqlFilter': this.clientSqlFilter },
                    async: false, //同步请求
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                            if (resp.Data == false) {
                                me.setValue('');
                            }
                        } else {
                            Ext.MessageBox.alert('取数失败', resp.status);
                        }
                    }
                });
            });
        }

    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (!allfield) return;

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头 

            if (me.showAutoHeader) {
                for (var i = 0; i < heads.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                }
            }

            modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: temp, //fields[i],
                    type: 'string',
                    mapping: temp//fields[i]
                });

            }

            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CustomHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };
       
        url = C_ROOT + 'SUP/CustomHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });



    },
    onTriggerClick: function (eOption, ignoreBeforeEvent) {
        var me = this;
        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击           

        if (Ext.isEmpty(me.helpid)) return;
        
        if (!ignoreBeforeEvent) {//不忽略beforetriggerclick事件
            if (!me.fireEvent('beforetriggerclick', me)) return;
        }
        //me.fireEvent('beforetriggerclick');

        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;

        ShowHelp = function () {

            var queryItems;
            var modelFields;
            var gridColumns;
                        
            
            if (!allfield) return;

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头
                
            queryItems = new Array();
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp = fields[i];;
                //                    if (tempfield.length > 1) {
                //                        temp = tempfield[1]; //去掉表名
                //                    }
                //                    else {
                //                        temp = fields[i];
                //                    }

                queryItems.push({
                    xtype: 'textfield',
                    fieldLabel: heads[i],
                    name: temp //fields[i]
                    //anchor: '95%'
                });
            }

            modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: fields[i], //不去掉表名
                    type: 'string',
                    mapping: temp
                });
            }

            gridColumns = new Array();
   
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                gridColumns.push({
                    header: heads[i],
                    flex: 1,
                    //sortable: true,
                    dataIndex: fields[i]
                });
            }
           

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    id: "help_query",
								    text: "查询",
								    iconCls: 'add'
								},
								{
								    id: 'help_show',
								    text: '隐藏',
								    iconCls: 'icon-fold',
								    handler: function () {
								        if (this.iconCls == 'icon-unfold') {
								            this.setIconCls('icon-fold');
								            this.setText("隐藏");
								            querypanel.show();
								        } else {
								            this.setIconCls('icon-unfold');
								            this.setText("显示");
								            querypanel.hide();
								        }
								    }
								},
								 "->",
							   {
							       id: "help_close",
							       text: "关闭",
							       iconCls: 'cross'
							   }
							 ]
            });

            querypanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'north',
                hidden: false,
                bodyStyle: 'padding:3px',
                fields: queryItems
            })

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'model',
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/CustomHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            //store.load();//这里load，IE的界面会扭掉

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store
            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid;
            //多选
            if (me.selectMode === 'multiple') {

                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    selModel: selModel, //多选
                    columnLines: true,
                    columns: gridColumns,
                    bbar: pagingbar
                });
            }
            else {//单选
                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    //autoScroll:true,
                    columnLines: true,
                    //                    layout: {
                    //                        type: 'hbox', //这个会出现横向滚动条，难看
                    //                        align: 'stretch'
                    //                    },
                    columns: gridColumns,
                    bbar: pagingbar
                });
            }

            grid.on('itemdblclick', function () {

                //
                var data = grid.getSelectionModel().getSelection();

                if (data.length > 0) {
                    var code = data[0].get(me.valueField);
                    var name = data[0].get(me.displayField);

                    var obj = new Object();
                    obj[me.valueField] = code;

                    if (me.displayFormat) {
                        obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
                    } else {
                        obj[me.displayField] = (name === '&nbsp;') ? '' : name;
                    }

                    var valuepair = Ext.ModelManager.create(obj, 'model');

                    me.setValue(valuepair); //必须这么设置才能成功

                    win.hide();
                    win.close();
                    win.destroy();

                    //if (me.isInGrid) {

                    var pobj = new Object();
                    pobj.code = code;
                    pobj.name = name;
                    pobj.type = 'fromhelp';
                    pobj.data = data[0].data;

                    //空值修正
                    for (var p in pobj.data) {
                        if (pobj.data[p] && pobj.data[p] === '&nbsp;') {
                            pobj.data[p] = '';
                        }
                    }

                    me.fireEvent('helpselected', pobj);
                    //}

                }
            }, this)

            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                constrain: true,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                modal: true,
                items: [toolbar, querypanel, grid]
            });

            win.show();

            //store.load();//手工调不会触发beforeload事件


            toolbar.items.get('help_query').on('click', function () {             
                store.loadPage(1); 
            })

            toolbar.items.get('help_close').on('click', function () {

                win.hide();
                win.close();
                win.destroy();

            })

            store.on('beforeload', function () {
                var formdata = querypanel.getForm();
                var data = formdata.getValues();

                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }

                //debugger;
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data), 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                else {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data) });
                }

                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

                //return true;
            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

        };
        
        url = C_ROOT + 'SUP/CustomHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {



                    if (me.helpType === 'rich') {
                        title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    },
    showHelp: function () {
        this.onTriggerClick();
    },
    bindData: function () {
        var me = this;
        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);
        return;
    }, //bindData
    getCodeName: function (value) {
        var me = this;
        var name;

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/CommonHelp/GetName?helptype=Single&helpid=' + me.helpid + '&code=' + value,
            async: false, //同步请求
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    name = resp.name; //显示值                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                    name = 'error';
                }
            }
        });

        return name;
    },
    setOutFilter: function (obj) {
        Ext.apply(this.store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(obj) });
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        this.clientSqlFilter = str;
    },
    getFirstRowData: function () {
        var me = this;
        var fields = me.listFields.split(',');

        var modelFields = new Array();
        for (var i = 0; i < fields.length; i++) {

            var tempfield = fields[i].split('.');
            var temp;
            if (tempfield.length > 1) {
                temp = tempfield[1]; //去掉表名
            }
            else {
                temp = fields[i];
            }

            modelFields.push({
                name: fields[i],
                type: 'string',
                mapping: temp
            });
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: modelFields
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 20,
            autoLoad: false,
            url: C_ROOT + 'SUP/CustomHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function () {
            if (me.outFilter) {

                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            } else {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(data) });
            }

        });

        store.load(function () {
            var data = store.data.items[0].data;
            me.fireEvent('firstrowloaded', data);
        });


    }
});

//自定义表单多选帮助
Ext.define('Ext.ng.CustFormMultiHelp', {
    extend: 'Ext.ng.CustomFormHelp',
    alias: ['widget.ngCustFormMutilHelp'],
    selectMode: 'Multi',
    multiSelect: false,//智能搜索为多选再设置为true,getValue()会得到数组
    showCommonUse: true, //是否显示常用
    showSelectedData: true,//是否显示已选数据
    onTriggerClick: function (eOp, ignoreBeforeEvent) {
        var me = this;
        if (me.readOnly) return;

        if (!ignoreBeforeEvent) {//不忽略beforetriggerclick事件
            if (!me.fireEvent('beforetriggerclick', me)) return;
        }

        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;

        ShowHelp = function () {

            var queryItems;
            var modelFields;
            var gridColumns;

            if (!allfield) return;

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头名称

            queryItems = new Array();
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp = fields[i];;
                //                    if (tempfield.length > 1) {
                //                        temp = tempfield[1]; //去掉表名
                //                    }
                //                    else {
                //                        temp = fields[i];
                //                    }

                queryItems.push({
                    xtype: 'textfield',
                    fieldLabel: heads[i],
                    name: temp //fields[i]                            
                });
            }

            modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: fields[i], //不去掉表名
                    type: 'string',
                    mapping: temp
                });
            }

            gridColumns = new Array();
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                gridColumns.push({
                    header: heads[i],
                    flex: 1,
                    //sortable: true,
                    dataIndex: fields[i]
                });
            }

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [
							    {
							        id: "help_query",
							        text: "查询",
							        iconCls: 'add'
							    },
							    {
							        id: 'help_show',
							        text: '显示',
							        iconCls: 'icon-unfold',
							        handler: function () {
							            if (this.iconCls == 'icon-unfold') {
							                this.setIconCls('icon-fold');
							                this.setText("隐藏");
							                querypanel.show();
							            } else {
							                this.setIconCls('icon-unfold');
							                this.setText("显示");
							                querypanel.hide();
							            }
							        }
							    },
								    "->",
							    {
							        id: "help_close",
							        text: "关闭",
							        iconCls: 'cross'
							    }
                ]
            });

            var querypanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'north',
                hidden: true,
                fields: queryItems,
                style: { borderColor: 'transparent', backgroundColor: 'transparent' }
            })

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'model',
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/CustomHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            //store.load();//这里load，IE的界面会扭掉

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store,
                displayMsg: '共 {2} 条数据',
                //emptyMsg: "没有数据",
                beforePageText: "第",
                afterPageText: "/{0} 页",
                style: { backgroundImage: 'none', backgroundColor: 'transparent' }
            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                //frame: true,
                border: false,
                store: store,
                selModel: { mode: "SIMPLE" }, //MULTI, //多选
                columnLines: true,
                columns: gridColumns
                //bbar: pagingbar
            });

            var resultStore = Ext.create('Ext.ng.JsonStore', {
                model: 'model'
            });

            var selectedLoaded = false;
            //已选值记忆
            store.on('load', function () {

                if (!me.showSelectedData) return;
                if (!Ext.isEmpty(me.value)) {
                    if (!selectedLoaded) {
                        var rows = 0;
                        var selectData = [];
                        var vals = me.value.split(',');
                        for (var i = 0; i < vals.length; i++) {
                            var index = store.find(me.valueField, vals[i]);
                            if (index == (-1)) {
                                index = store.find(me.displayField, vals[i]);
                            }
                            var record = store.getAt(index);
                            if (record) {
                                selectData.push(record);
                                rows++;
                            }
                        }

                        if (rows == vals.length) {
                            resultStore.insert(0, selectData); //批量插入
                        }
                        else {

                            Ext.Ajax.request({
                                params: { 'helpid': me.helpid, 'codes': me.value },
                                url: C_ROOT + 'SUP/CommonHelp/GetSelectedData',
                                success: function (response) {
                                    var resp = Ext.JSON.decode(response.responseText);
                                    if (resp.Record.length > 0) {
                                        resultStore.insert(0, resp.Record);
                                    } else {
                                        Ext.MessageBox.alert('获取失败');
                                    }
                                }
                            });
                        }
                        selectedLoaded = true;
                    } //if
                }

            });

            var resultGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'east',
                //frame: true,
                width: 235,
                border: false,
                store: resultStore,
                selModel: { mode: "SIMPLE" }, //多选
                columnLines: true,
                columns: gridColumns
            });

            var btnPanel = Ext.create('Ext.panel.Panel', {
                region: 'east',
                width: 80,
                layout: 'absolute',
                border: false,
                frame: true,
                padding: 0,
                style: { borderColor: 'transparent', backgroundColor: 'transparent' }, //backgroundColor: 'transparent !important',marginTop: '22px',
                items: [{
                    xtype: 'button',
                    name: 'addSelect',
                    text: '&gt;',
                    x: 9,
                    y: 120,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = grid.getSelectionModel().getSelection();
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'selectAll',
                    text: '&gt;&gt;',
                    x: 9,
                    y: 150,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = store.data.items;
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeSelect',
                    text: '&lt;',
                    x: 9,
                    y: 180,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = resultGrid.getSelectionModel().getSelection();
                        resultStore.remove(data);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeAll',
                    text: '&lt;&lt;',
                    x: 9,
                    y: 210,
                    width: 60,
                    handler: Ext.bind(function () {
                        resultStore.removeAll();
                    })
                }]
            });

            var panel = Ext.create('Ext.panel.Panel', {
                region: 'center',
                //frame: true,
                border: false,
                layout: 'border',
                items: [grid, btnPanel, resultGrid]
            });

            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                constrain: true,
                modal: true,
                items: [toolbar, querypanel, panel],
                buttons: [pagingbar, '->', { text: '确定', handler: function () { me.btnOk(me, resultStore, win); } }, { text: '取消', handler: function () { win.close(); } }],
                layout: {
                    type: 'border',
                    padding: 1
                }
            });

            win.show();

            grid.on('itemdblclick', function (grid, record, item) {
                var data = [];
                data = grid.getSelectionModel().getSelection();
                if (data.length == 0) {//直接双击取不到选中的行
                    data.push(record);
                }
                me.copyData(data, resultStore);
            }, this)

            toolbar.items.get('help_query').on('click', function () {
                store.loadPage(1);  
            })

            toolbar.items.get('help_close').on('click', function () {

                win.hide();
                win.close();
                win.destroy();

            })

            store.on('beforeload', function () {
                var formdata = querypanel.getForm();
                var data = formdata.getValues();

                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }

                //debugger;
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data), 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                else {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data) });
                }

                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

                //return true;
            })
        };

        url = C_ROOT + 'SUP/CustomHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {



                    if (me.helpType === 'rich') {
                        title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },
    copyData: function (selectData, resultStore) {

        var me = this;
        var dataLen = selectData.length;
        var count = resultStore.getCount();
        var index = count;
        if (count > 0) {
            for (var i = 0; i < dataLen; i++) {
                var sourceData = selectData[i].data[me.valueField];
                var hit = false;
                for (var j = 0; j < count; j++) {
                    var selectedData = resultStore.data.items[j].data[me.valueField];
                    if (sourceData === selectedData) {
                        hit = true;
                    }
                }
                if (!hit) {
                    resultStore.insert(index, selectData[i]);
                    index++;
                }
            }
        } else {
            resultStore.insert(0, selectData); //批量插入
        }
    },
    btnOk: function (me, resultStore, win) {

        var values = new Array();
        var names = new Array();

        var arr = resultStore.data.items;
        for (var i = 0; i < arr.length; i++) {

            values.push(arr[i].data[me.valueField]);
            names.push(arr[i].data[me.displayField]);
        }

        var code = values.join(',');
        var name = names.join(',');

        var obj = new Object();
        obj[me.valueField] = code;

        if (me.displayFormat) {
            obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
        } else {
            obj[me.displayField] = name;
        }

        var valuepair = Ext.ModelManager.create(obj, 'model');
        me.setValue(valuepair); //必须这么设置才能成功

        win.hide();
        win.close();
        win.destroy();

        //if (me.isInGrid) {

        var pobj = new Object();
        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = arr;

        me.fireEvent('helpselected', pobj);

    },
    showHelp: function () {
        this.onTriggerClick();
    },
    getValue: function () {
        var me = this;
        if (me.multiSelect && me.value.length > 0) {
            return me.value[0];//数组转成string返回，否则保存数据库值为["01,03"]
        }
        else {
            return me.value;
        }
    }
});

//基于xml配置的多选通用帮助
Ext.define('Ext.ng.MultiHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngMutilHelp'],
    pageSize: 10,
    minChars: 100, //定义输入最少多少个字符的时候智能搜锁获取数据,设100来禁止智能搜索
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 600, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    ORMMode: true,
    selectMode: 'Multi', //multiple  
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: false, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    queryMode: 'remote',
    triggerAction: 'all', //'query'
    //triggerCls: 'icon-ComHelp',        
    initComponent: function () {
        //
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.listFields && me.listHeadTexts) {

            var listheaders = '';
            var listfields = '';

            var heads = me.listHeadTexts.split(','); //列头 
            var fields = me.listFields.split(','); //所有字段              

            var modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: temp, //fields[i],
                    type: 'string',
                    mapping: temp //fields[i]
                });

            }

            if (me.showAutoHeader) {

                for (var i = 0; i < heads.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                }
            }

            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }
                listfields += '<td>{' + temp + '}</td>';
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            } else {
                temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10,
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

        }
        else {
            me.initialListTemplate(); //初始化下拉列表样式 
        }

        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');

        me.on('select', function (combo, records, eOpts) {

            var theField;

            //构建
            if (me.listFields) {
                theField = [];
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);
                }
            }
            else {
                theField = [me.valueField, me.displayField];
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            var code = combo.getValue() || records[0].data[myValueFiled];
            var name = combo.getRawValue() || records[0].data[myDisplayField];

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            me.setValue(valuepair); //必须这么设置才能成功

            //debugger;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    pobj.data[theField[i]] = records[0].data[temp[1]];
                }
                else {
                    pobj.data[theField[i]] = records[0].data[theField[i]];
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.remove(autoPagingbar.items.items[10]);
            autoPagingbar.remove(autoPagingbar.items.items[9]);

        });

        me.on('keydown', function (combo, e, eOpts) {


            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });

    },
    listeners: {
        render: function (me, eOpts) {
            me.el.down('input').dom.readOnly = true; //禁用输入框
        }
    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (me.helpType === 'rich') {//用户自定义界面的模板 

                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;

                if (me.showAutoHeader) {
                    for (var i = 0; i < gridColumns.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + gridColumns[i].header + '</th>';
                    }
                }

                for (var i = 0; i < modelFields.length; i++) {
                    listfields += '<td>{' + modelFields[i]['name'] + '}</td>';
                }

            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头 

                if (me.showAutoHeader) {
                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp//fields[i]
                    });

                }
            }


            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {
                
                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }
        else {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly) return;

        if (Ext.isEmpty(me.helpid)) return;
        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;

        ShowHelp = function () {
            var queryItems;
            var modelFields;
            var gridColumns;

            if (me.helpType === 'rich') {//用户自定义界面的模板            
                queryItems = template.Template.QueryItems;
                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;
            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头

                queryItems = new Array();
                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp = fields[i];;
                    //                    if (tempfield.length > 1) {
                    //                        temp = tempfield[1]; //去掉表名
                    //                    }
                    //                    else {
                    //                        temp = fields[i];
                    //                    }

                    queryItems.push({
                        xtype: 'textfield',
                        fieldLabel: heads[i],
                        name: temp //fields[i]                   
                    });
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: fields[i], //不去掉表名，有些sql场景带表名
                        type: 'string',
                        mapping: temp  //  fields[i] ,mapping字段不能带表名，不能带点号
                    });
                }

                gridColumns = new Array();
                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    gridColumns.push({
                        header: heads[i],
                        width: 168,
                        //sortable: true,
                        dataIndex: fields[i] // 与model的name对应
                    });
                }
            }

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    id: "help_query",
								    text: "查询",
								    iconCls: 'add'
								},
								{
								    id: 'help_show',
								    text: '显示',
								    iconCls: 'icon-unfold',
								    handler: function () {
								        if (this.iconCls == 'icon-unfold') {
								            this.setIconCls('icon-fold');
								            this.setText("隐藏");
								            querypanel.show();
								        } else {
								            this.setIconCls('icon-unfold');
								            this.setText("显示");
								            querypanel.hide();
								        }
								    }
								},
								 "->",
							   {
							       id: "help_close",
							       text: "关闭",
							       iconCls: 'cross'
							   }
							 ]
            });

            var querypanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'north',
                hidden: true,
                fields: queryItems,
                style: { borderColor: 'transparent', backgroundColor: 'transparent' }
            })

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'model',
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            //store.load();//这里load，IE的界面会扭掉

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store,
                displayMsg: '共 {2} 条数据',
                //emptyMsg: "没有数据",
                beforePageText: "第",
                afterPageText: "/{0} 页",
                style: { backgroundImage: 'none', backgroundColor: 'transparent' }
            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');
            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                //frame: true,
                border: false,
                store: store,
                selModel: { mode: "SIMPLE" }, //MULTI, //多选
                columnLines: true,
                columns: gridColumns
                //bbar: pagingbar
            });

            var resultStore = Ext.create('Ext.ng.JsonStore', {
                model: 'model'
            });

            var selectedLoaded = false;
            //已选值记忆
            store.on('load', function () {

                if (!Ext.isEmpty(me.value)) {
                    if (!selectedLoaded) {
                        var rows = 0;
                        var selectData = [];
                        var vals = me.value.split(',');
                        for (var i = 0; i < vals.length; i++) {
                            var index = store.find(me.valueField, vals[i]);
                            var record = store.getAt(index);
                            if (record) {
                                selectData.push(record);
                                rows++;
                            }
                        }

                        if (rows == vals.length) {
                            resultStore.insert(0, selectData); //批量插入
                        }
                        else {

                            Ext.Ajax.request({
                                params: { 'helpid': me.helpid, 'codes': me.value, 'mode': me.ORMMode },
                                url: C_ROOT + 'SUP/CommonHelp/GetSelectedData',
                                success: function (response) {
                                    var resp = Ext.JSON.decode(response.responseText);
                                    if (resp.Record.length > 0) {
                                        //resultStore.insert(0, resp.Record); 

                                        //处理数据集，可能modelfield不一致，grid的field带点号，model不能带
                                        var arr = [];
                                        var fields = resultStore.model.getFields();
                                        for (var i = 0; i < resp.Record.length; i++) {
                                           
                                            var record = resp.Record[i];
                                            var obj = {};
                                            for (var k = 0; k < fields.length; k++) {
                                                 //if(fields[k].name.indexOf('.') > 0)
                                                obj[fields[k].name] = record[fields[k].mapping];//数据以name为准                                              
                                            }
                                            arr.push(obj);
                                        }
                                        resultStore.insert(0, arr); 
                                                                              
                                    } else {
                                        Ext.MessageBox.alert('获取失败');
                                    }
                                }
                            });
                        }
                        selectedLoaded = true;
                    } //if
                }

            });

            var resultGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'east',
                //frame: true,
                width: 235,
                border: false,
                store: resultStore,
                selModel: { mode: "SIMPLE" }, //多选
                columnLines: true,
                columns: gridColumns
            });

            var btnPanel = Ext.create('Ext.panel.Panel', {
                region: 'east',
                width: 80,
                layout: 'absolute',
                border: false,
                frame: true,
                padding: 0,
                style: { borderColor: 'transparent', backgroundColor: 'transparent' }, //backgroundColor: 'transparent !important',marginTop: '22px',
                items: [{
                    xtype: 'button',
                    name: 'addSelect',
                    text: '&gt;',
                    x: 9,
                    y: 120,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = grid.getSelectionModel().getSelection();
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'selectAll',
                    text: '&gt;&gt;',
                    x: 9,
                    y: 150,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = store.data.items;
                        me.copyData(data, resultStore);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeSelect',
                    text: '&lt;',
                    x: 9,
                    y: 180,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = resultGrid.getSelectionModel().getSelection();
                        resultStore.remove(data);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeAll',
                    text: '&lt;&lt;',
                    x: 9,
                    y: 210,
                    width: 60,
                    handler: Ext.bind(function () {
                        resultStore.removeAll();
                    })
                }]
            });

            var panel = Ext.create('Ext.panel.Panel', {
                region: 'center',
                //frame: true,
                border: false,
                layout: 'border',
                items: [grid, btnPanel, resultGrid]
            });

            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                constrain: true,
                modal: true,
                items: [toolbar, querypanel, panel],
                buttons: [pagingbar, '->', { text: '确定', handler: function () { me.btnOk(me, resultStore, win); } }, { text: '取消', handler: function () { win.close(); } }],
                layout: {
                    type: 'border',
                    padding: 1
                }
            });

            win.show();

            //pagingbar.el.down('.x-box-inner').setStyle({ backgroundColor: 'transparent' }); //#dfe8f6

            //store.load();//手工调不会触发beforeload事件

            grid.on('itemdblclick', function (grid, record, item) {
                var data = [];
                data = grid.getSelectionModel().getSelection();
                if (data.length == 0) {//直接双击取不到选中的行
                    data.push(record);
                }
                me.copyData(data, resultStore);
            }, this)

            toolbar.items.get('help_query').on('click', function () {               
                store.loadPage(1);               
            })

            toolbar.items.get('help_close').on('click', function () {
                win.hide();
                win.close();
                win.destroy();
            })

            store.on('beforeload', function () {
                var formdata = querypanel.getForm();
                var data = formdata.getValues();

                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }

                //debugger;
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data), 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                else {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data) });
                }

                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

                //return true;
            })
        };

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }
        else {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },
    showHelp: function () {
        this.onTriggerClick();
    },
    copyData: function (selectData, resultStore) {       
        var me = this;
        var dataLen = selectData.length;
        var count = resultStore.getCount();
        var index = count;
        if (count > 0) {
            for (var i = 0; i < dataLen; i++) {
                var sourceData = selectData[i].data[me.valueField];
                var hit = false;
                for (var j = 0; j < count; j++) {
                    var selectedData = resultStore.data.items[j].data[me.valueField];
                    if (sourceData === selectedData) {
                        hit = true;
                    }
                }
                if (!hit) {
                    resultStore.insert(index, selectData[i]);
                    index++;
                }
            }
        } else {
            resultStore.insert(0, selectData); //批量插入
        }
    },
    btnOk: function (me, resultStore, win) {

        var values = new Array();
        var names = new Array();

        var arr = resultStore.data.items;
        for (var i = 0; i < arr.length; i++) {

            values.push(arr[i].data[me.valueField]);
            names.push(arr[i].data[me.displayField]);
        }

        var code = values.join(',');
        var name = names.join(',');

        var obj = new Object();
        obj[me.valueField] = code;

        if (me.displayFormat) {
            obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
        } else {
            obj[me.displayField] = name;
        }

        var valuepair = Ext.ModelManager.create(obj, 'model');
        me.setValue(valuepair); //必须这么设置才能成功

        win.hide();
        win.close();
        win.destroy();

        //if (me.isInGrid) {

        var pobj = new Object();
        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = arr;

        me.fireEvent('helpselected', pobj);

    },
    bindData: function () {
        var me = this;

        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);

        return;
    }, //bindData
    getCodeName: function (value) {
        var me = this;
        var name;
        //

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/CommonHelp/GetName?helptype=Multi&helpid=' + me.helpid + '&code=' + value,
            async: false, //同步请求
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    name = resp.name; //显示值                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                    name = 'error';
                }
            }
        });

        return name;
    },
    setOutFilter: function (obj) {
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        this.clientSqlFilter = str;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    }
});

//基于数据库配置的通用帮助
Ext.define('Ext.ng.RichHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngRichHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    //helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    ORMMode: true,
    selectMode: 'Single', //multiple  
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    triggerCls: 'x-form-help-trigger',
    queryMode: 'remote',
    triggerAction: 'all', //'query'
    selectQueryProIndex: 0,
    isShowing: false,
    editable: true,
    codeIsNum: true,//代码列为数值型, phid的valueType不能设置为int,设置这个属性为true，代码转名称空值不显示0
    showCommonUse: true, //是否显示常用
    showRichQuery: false,//是否显示高级
    infoRightUIContainerID: '',//信息权限UI容器id
    busCode:'',//信息权限所属业务类型
    acceptInput: false,//接受用户自由输入的值
    helpDraggable: true,
    helpResizable: true,
    helpMaximizable: false,
    helpTitle:undefined,//帮助标题
    matchFieldWidth: false,
    ignoreOutFilter: false,//代码转名称时忽略外部条件
    maxFlexColumns: 5,
    onlyLeafValid:false,//仅叶子节点有效
    initComponent: function () {
        //
        var me = this;

        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.helpType = 'RichHelp_' + me.helpid;
        me.bussType = me.bussType || 'all';

        //多选时，智能搜索记录存储
        var selectedRecords = [];

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.editable) {
            if (me.listFields && me.listHeadTexts) {

                var listheaders = '';
                var listfields = '';

                var heads = me.listHeadTexts.split(','); //列头 
                var fields = me.listFields.split(','); //所有字段              

                var modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp //fields[i]
                    });

                }

                if (me.showAutoHeader) {

                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    listfields += '<td>{' + temp + '}</td>';
                }

                var temp;
                if (me.showAutoHeader) {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                } else {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }
                me.tpl = temp;

            }
            else {
                //me.initialListTemplate(); //初始化下拉列表样式 

                var tempfield = me.valueField.split('.');//系统编码
                var valueField;
                if (tempfield.length > 1) {
                    valueField = tempfield[1]; //去掉表名
                }
                else {
                    valueField = me.valueField;
                }

                if (!me.userCodeField) {
                    me.userCodeField = me.valueField;//容错处理
                }
                var uField = me.userCodeField.split('.');//用户编码
                var userCodeField;
                if (uField.length > 1) {
                    userCodeField = uField[1];
                } else {
                    userCodeField = me.userCodeField;
                }

                var dfield = me.displayField.split('.');
                var displayField;
                if (dfield.length > 1) {
                    displayField = dfield[1]; //去掉表名
                }
                else {
                    displayField = me.displayField;
                }

                var modelFields = [{
                    name: valueField,
                    type: 'string',
                    mapping: valueField
                }, {
                    name: userCodeField,
                    type: 'string',
                    mapping: userCodeField
                }, {
                    name: displayField,
                    type: 'string',
                    mapping: displayField
                }]
                                
                var listfields = '<td>{' + userCodeField + '}</td>';//显示用户代码
                listfields += '<td>{' + displayField + '}</td>';
                me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';

            }

            var store = Ext.create('Ext.data.Store', {
                //var store = Ext.create('Ext.ng.JsonStore', {
                pageSize: 10,
                fields: modelFields,
                cachePageData: true,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode, //+ '&UIContainerID=' + me.infoRightUIContainerID,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);
            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
                Ext.apply(store.proxy.extraParams, { 'UIContainerID': me.infoRightUIContainerID,'BusCode':me.busCode });
               

            });
            store.on('load', function (store, records, successful, eOpts) {

                if (me.multiSelect && selectedRecords.length > 0) {
                    var temp = store.data.items.concat(selectedRecords);
                    //store.data.items = temp;
                    store.loadData(temp);
                    me.setValue(selectedRecords, false);
                }


                if (me.needBlankLine) {
                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                }

            });
        }

        me.addEvents('beforehelpselected'); //定义值被选完的事件
        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');
        me.addEvents('beforetriggerclick');
        me.addEvents('beforehelpclose');        

        me.on('beforeselect', function (combo, record, index, eOpts) {           
            me.oldVal = me.getValue();//旧的值
        });

        me.on('select', function (combo, records, eOpts) {

            if (me.multiSelect) {
                //判断是否存在
                var isExist = function (record) {
                    var flag = false;
                    for (var i = 0; i < selectedRecords.length; i++) {
                        var myRecord = selectedRecords[i];

                        if (record.data[me.valueField] == myRecord.data[me.valueField]) {
                            flag = true;
                            break;
                        }
                    }

                    return flag;
                }

                var tempRecords = [];
                for (var i = 0; i < records.length; i++) {
                    if (!isExist(records[i])) {
                        tempRecords.push(records[i]);
                    }
                }

                selectedRecords = selectedRecords.concat(tempRecords);
                me.setValue(selectedRecords, false);
            }

            var theField;//所有列
            var modelFileds;
            //构建model
            if (me.listFields) {
                theField = [];
                modelFileds = []
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);


                    var obj = {
                        name: temp[i],
                        type: 'string',
                        mapping: temp[i]
                    }

                    modelFileds.push(obj);
                }
            }
            else {

                theField = [me.valueField, me.displayField];

                modelFileds = [{
                    name: me.valueField,
                    type: 'string',
                    mapping: me.valueField
                }, {
                    name: me.displayField,
                    type: 'string',
                    mapping: me.displayField
                }];

            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: modelFileds//theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }
            
            //            var code = combo.getValue() || records[0].data[myValueFiled];
            //            var name = combo.getRawValue() || records[0].data[myDisplayField];

            var codeArr = [];
            var nameArr = [];
            for (var i = 0; i < records.length; i++) {
                codeArr.push(records[i].data[myValueFiled]);
                nameArr.push(records[0].data[myDisplayField]);
            }

            var code = codeArr.join();
            var name = nameArr.join();

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid || me.acceptInput) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = name;
            }

           
            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            //select不需要设置value
            //if (me.isInGrid) {//grid特殊处理,valueField也是name
                if (!me.multiSelect) {
                    me.setValue(valuepair); //不是多选，在这里设置
                }
            //}
           
            var pobj = new Object();
            pobj.oldVal = me.oldVal;
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';           
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');//去掉表名
                if (temp.length > 1) {
                    pobj.data[theField[i]] = records[0].data[temp[1]];
                }
                else {
                    pobj.data[theField[i]] = records[0].data[theField[i]];
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {
            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                    case Ext.EventObject.LEFT:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                    case Ext.EventObject.RIGHT:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });

        me.on('render', function (combo, eOpts) {
            var input = this.el.down('input');
            if (input) {
                //input.dom.ondblclick = function () { alert(combo.getValue());};
            }
        });

        if (me.editable && !me.isInGrid) {//grid中名称列是假列，不验了
            me.on('blur', function () {

                if (me.acceptInput) {//接受输入值                   
                    me.setValue(me.rawValue); //让grid有值                  
                } else {
                    selectedRecords.length = 0; //清空数组
                    var value = me.getRawValue();
                    if (Ext.isEmpty(value)) {
                        me.setValue('');
                        return;
                    }
                       
                    value = encodeURI(value);
                    Ext.Ajax.request({
                        url: C_ROOT + 'SUP/RichHelp/ValidateData?helpid=' + me.helpid + '&inputValue=' + value + '&selectMode=' + me.selectMode,
                        params: { 'clientSqlFilter': this.clientSqlFilter, 'helptype': 'ngRichHelp' },
                        async: false, //同步请求
                        success: function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp.Status === "success") {
                                if (resp.Data == false) {
                                    me.setValue('');
                                }
                            }
                            else {
                                Ext.MessageBox.alert('取数失败', resp.status);
                            }
                        }
                    });
                }


            });
        }

    },
    getValue: function () {
        // If the user has not changed the raw field value since a value was selected from the list,
        // then return the structured value from the selection. If the raw field value is different
        // than what would be displayed due to selection, return that raw value.
        var me = this,
            picker = me.picker,
            rawValue = me.getRawValue(), //current value of text field
            value = me.value; //stored value from last selection or setValue() call

        if (me.getDisplayValue() !== rawValue) {
            if (me.acceptInput) {//接受输入值  
                value = rawValue;//通用帮助选好之后，在后面输入字符再删除，value就会变rawValue
                me.value = me.displayTplData = me.valueModels = null;
            }
            if (picker) {
                me.ignoreSelection++;
                picker.getSelectionModel().deselectAll();
                me.ignoreSelection--;
            }
        }

        return value;
    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';


            if (!allfield) return;

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头 

            if (me.showAutoHeader) {
                for (var i = 0; i < heads.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                }
            }

            modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: temp, //fields[i],
                    type: 'string',
                    mapping: temp//fields[i]
                });

            }
            
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    //title = resp.data.Title;
                    allfield = resp.data.AllField;
                    headText = resp.data.HeadText;
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    },
    onTriggerClick: function (eOption, ignoreBeforeEvent) { //ignoreBeforeEvent为true能手动弹出帮助
        var me = this;
        me.selectQueryProIndex = 0;
        if (!ignoreBeforeEvent) {//不忽略beforetriggerclick事件
            if (!me.fireEvent('beforetriggerclick', me)) return;
        }
        if (me.isShowing) return;

        me.isShowing = true;
        if (me.readOnly || arguments.length == 3) {
            me.isShowing = false;
            return; //arguments.length == 3，输入框上点击     
        }

        if (Ext.isEmpty(me.helpid)) {
            me.isShowing = false;
            return;
        }
           
        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var model;
        var columns;
        var newUrl;
        var existQueryProperty = false;
        var queryPropertyItems;
        var showTree;
        var richQueryItem;
        var richQueryFilter;
                
        ShowHelp = function () {

            //var queryItems;
            var modelFields;
            var gridColumns;

            if (!model.length) {
                NGMsg.Error('获取帮助信息失败，请检查数据库通用帮助配置信息是否正确!');
                me.isShowing = false;
                return;
            }

            //var fields = allfield.split(','); //所有字段
            //var heads = headText.split(','); //列头

            //queryItems = new Array();
            //for (var i = 0; i < heads.length; i++) {
            //    var tempfield = fields[i].split('.');
            //    var temp = fields[i];
            //    queryItems.push({
            //        xtype: 'textfield',
            //        fieldLabel: heads[i],
            //        name: temp //fields[i]                            
            //    });
            //}

            modelFields = model;//new Array();

            //for (var i = 0; i < fields.length; i++) {
            //    var tempfield = fields[i].split('.');
            //    var temp;
            //    if (tempfield.length > 1) {
            //        temp = tempfield[1]; //去掉表名                  
            //    }
            //    else {
            //        temp = fields[i];
            //    }
            //    var ar = temp.split(' ');//取别名
            //    if (ar.length > 1) {
            //        temp = ar[ar.length - 1].trim();
            //    }
            //    modelFields.push({
            //        name: temp, //fields[i], //不去掉表名
            //        type: 'string',
            //        mapping: temp
            //    });
            //}

            for (var i = 0; i < columns.length; i++) {
                var renderer = columns[i].renderer;
                if (renderer) {
                    columns[i].renderer = Ext.decode(renderer);
                }
            }

            gridColumns = columns; //new Array();

            //for (var i = 0; i < heads.length; i++) {
            //    var tempfield = fields[i].split('.');
            //    var temp;
            //    if (tempfield.length > 1) {
            //        temp = tempfield[1]; //去掉表名                   
            //    }
            //    else {
            //        temp = fields[i];
            //    }
            //    var ar = temp.split(' ');//取别名
            //    if (ar.length > 1) {
            //        temp = ar[ar.length - 1].trim();
            //    }
                
            //    if (heads.length > me.maxFlexColumns) {
            //        gridColumns.push({
            //            header: heads[i],
            //            //flex: 1,
            //            width: 200,
            //            //sortable: true,
            //            dataIndex: temp //fields[i] 去掉表名
            //        });
            //    }
            //    else {
            //        gridColumns.push({
            //            header: heads[i],
            //            flex: 1,                        
            //            //sortable: true,
            //            dataIndex: temp //fields[i] 去掉表名
            //        });
            //    }
            //}


            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                weight: 20,
                height: 36,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    xtype: 'textfield',
								    itemId: "searchkey",
								    width: 200								    
								},
								{
								    itemId: 'richhelp_query',
								    iconCls: 'icon-View'
								},
                                {
                                    itemId: 'richhelp_refresh',
                                    iconCls: 'icon-Refresh'
                                }, {
                                    xtype: 'checkboxfield',
                                    boxLabel: '在结果中搜索',
                                    width: 100,
                                    itemId: 'ch-searchInResult',
                                    inputValue: '01'
                                },
                                '->',
							     {
							         xtype: 'checkboxgroup',
							         name: 'hobby',
							         items: [                                       
                                        {
                                            boxLabel: '树记忆', width: 60, itemId: 'ch-treerem', inputValue: '02',hidden:true, handler: function (chk) {
                                                me.saveTreeMemory(leftTree, chk.getValue());
                                                var k = 0;
                                            }
                                        }
							         ]
							     }
                ]
            });

            var searcheArr = [];
            var searchIndex = {}; //索引
            toolbar.queryById('ch-searchInResult').on('change', function (me, nvalue, ovalue, eOpts) {

                if (false == nvalue) {
                    searcheArr.length = 0; //清空条件列表
                    searchIndex = {}; //清空索引
                }

            });

            toolbar.queryById('richhelp_query').on('click', function () {

                var searchkey;
                var key = toolbar.queryById('searchkey').getValue();
                var activeTab = tabPanel.getActiveTab();
                if (activeTab.id === 'listStyle') {//列表搜索
                    if (toolbar.queryById('ch-searchInResult').getValue()) {
                        if (!searchIndex[key]) {
                            searcheArr.push(key);
                            searchIndex[key] = key;
                        }
                        searchkey = searcheArr;
                    }
                    else {
                        searcheArr.length = 0;
                        searcheArr.push(key);
                    }
                    Ext.apply(store.proxy.extraParams, { 'searchkey': searcheArr });                  
                    store.loadPage(1);
                }
               
                //树定位
                if (activeTab.id === 'treeStyle') {
                    me.findNodeByFuzzy(tree, key);
                    toolbar.queryById('searchkey').focus();
                }
             
            });

            toolbar.queryById('richhelp_refresh').on('click', function () {
                toolbar.queryById('searchkey').setValue('');

                if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                    delete store.proxy.extraParams.searchkey;
                    delete store.proxy.extraParams.treesearchkey;
                    delete store.proxy.extraParams.treerefkey;
                    store.load();
                }
            });

            toolbar.on('afterrender', function () {
                toolbar.queryById('searchkey').getEl().on('keypress', function (e, t, eOpts) {
                    //回车
                    if (e.keyCode == Ext.EventObject.ENTER) {                       
                        toolbar.queryById('richhelp_query').fireEvent('click');
                    }

                });
            });
            

            var propertyCode = queryPropertyItems[me.selectQueryProIndex].code;
            var propertyID = queryPropertyItems[me.selectQueryProIndex].inputValue;
            queryPropertyItems[me.selectQueryProIndex].checked = true;

            var queryProperty = Ext.create('Ext.container.Container', {
                region: 'north',
                //frame: true,
                weight: 20,
                border: false,
                //layout: 'auto', //支持自适应 	              
                items: [{
                    xtype: 'fieldset', //'fieldcontainer',
                    title: '查询属性', //fieldLabel: 'Size',
                    defaultType: 'radiofield',
                    defaults: {
                        flex: 1
                    },
                    layout: 'column',
                    fieldDefaults: {
                        margin: '0 10 0 0'
                    },
                    items: [{
                        id: 'radioQueryPro',
                        xtype: 'radiogroup',
                        layout: 'column',
                        fieldDefaults: {
                            margin: '0 10 3 0'
                        },
                        activeItem: 0,
                        items: queryPropertyItems,
                        listeners: {
                            'change': function (radioCtl, nvalue, ovalue) {

                                var select = radioCtl.getChecked();
                                if (select.length > 0) {

                                    leftPanel.setTitle(select[0].boxLabel);
                                    var code = select[0].code; //加载树的搜索id
                                    propertyCode = code;
                                    propertyID = select[0].inputValue;

                                    Ext.Ajax.request({
                                        //params: { 'id': busid },
                                        url: C_ROOT + 'SUP/RichHelp/GetListExtendInfo?code=' + propertyCode,
                                        //callback: ShowHelp,
                                        success: function (response) {
                                            var resp = Ext.JSON.decode(response.responseText);
                                            var extFields = resp.extfields; //扩展字段
                                            var extHeader = resp.extheader; //扩展列头

                                            var fields = Ext.clone(modelFields);
                                            var columns = Ext.clone(gridColumns);

                                            if (extHeader && extHeader != '') {
                                                var tempfs = extFields.split(',');
                                                var cols = extHeader.split(',');
                                                for (var i = 0; i < tempfs.length; i++) {
                                                    fields.push({
                                                        name: tempfs[i],
                                                        type: 'string',
                                                        mapping: tempfs[i]
                                                    });

                                                    columns.push({
                                                        header: cols[i],
                                                        flex: 1,
                                                        dataIndex: tempfs[i]
                                                    });
                                                }
                                            }

                                            //使用外部的store
                                            store = Ext.create('Ext.ng.JsonStore', {
                                                fields: fields,
                                                pageSize: 20,
                                                autoLoad: true,
                                                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                                                listeners: {
                                                    'beforeload': function () {
                                                        var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                                                        Ext.apply(store.proxy.extraParams, data);
                                                        if (me.likeFilter) {
                                                            Ext.apply(data, me.likeFilter);
                                                        }
                                                        if (me.outFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                                                        }
                                                        if (me.leftLikeFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                                                        }
                                                        if (me.clientSqlFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                                                        }
                                                    } //beforeload function
                                                }//listeners
                                            });
                                            //重新配置grid
                                            grid.reconfigure(store, columns);
                                            pagingbar.bind(store);
                                        }
                                    });
                                }

                                if (nvalue.property === 'all') {

                                    toolbar.queryById('ch-treerem').hide();

                                    leftPanel.setVisible(false);
                                    //leftTree.setVisible(false);
                                    if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                                        delete store.proxy.extraParams.searchkey;
                                        delete store.proxy.extraParams.treesearchkey;
                                        delete store.proxy.extraParams.treerefkey;
                                        store.load();
                                    }
                                    return;
                                } else {

                                    me.initParam();
                                    if (toolbar.queryById('ch-treerem')) {
                                        toolbar.queryById('ch-treerem').setVisible(true);
                                    }

                                    var rootNode = leftTree.getRootNode();
                                    if (leftTree.isFirstLoad) {
                                        rootNode.expand(); //expand会自动调用load
                                        leftTree.isFirstLoad = false;
                                    }
                                    else {
                                        leftTree.getStore().load();
                                    }
                                    leftPanel.setVisible(true);
                                } //else

                            } //function
                        }//listeners
                    }]
                }]
            });
            
            var leftTree = Ext.create('Ext.ng.TreePanel', {
                //title: queryPropertyItems[0].boxLabel,
                autoLoad: false,
                //collapsible: true,
                split: true,
                //hidden: true,
                width: 180,
                region: 'west',
                isFirstLoad: true,
                treeFields: [{ name: 'text', type: 'string' },
                   { name: 'treesearchkey', string: 'string' },
                   { name: 'treerefkey', type: 'string' }//我的自定义属性                
                ],
                url: C_ROOT + "SUP/RichHelp/GetQueryProTree",
                listeners: {
                    selectionchange: function (m, selected, eOpts) {
                        me.memory.eOpts = "selectionchange";

                        //刷列表数据
                        var record = selected[0];
                        if (record) {
                            if (!Ext.isEmpty(record.data.treesearchkey) && !Ext.isEmpty(record.data.treerefkey)) {
                                Ext.apply(store.proxy.extraParams, { 'treesearchkey': record.data.treesearchkey, 'treerefkey': record.data.treerefkey });
                                store.load();
                            }
                            //设置选中
                            toolbar.queryById('ch-treerem').setValue(me.memory.IsMemo && me.memory.FoucedNodeValue == selected[0].getPath());
                            me.memory.eOpts = "";
                        }
                    },
                    viewready: function (m, eOpts) {

                        if (me.memory) {

                            if (!Ext.isEmpty(me.memory.FoucedNodeValue)) {
                                leftTree.selectPath(me.memory.FoucedNodeValue, null, null, function () {
                                    if (Ext.isIE) {
                                        window.setTimeout(function () {
                                            var selectNode = m.view.body.query("tr." + m.view.selectedItemCls);
                                            if (selectNode) {
                                                selectNode[0].scrollIntoView(true);
                                            }
                                        }, 500);
                                    }
                                });
                            }
                            else {
                                store.load();
                            }
                        }
                    }
                }
            });

            leftTree.getStore().on('beforeload', function (store, operation, eOpts) {
                operation.params.code = propertyCode; //树添加参数	                
            });
                    

            var leftPanel = Ext.create('Ext.panel.Panel', {
                title: "查询属性数据",
                autoScroll: false,
                collapsible: true,
                split: true,
                hidden: true,
                region: 'west',
                weight: 10,
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
                        emptyText: '输入关键字，定位树节点',
                        margin: '2 0 2 2',
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER) {
                                    me.findNodeByFuzzy(leftTree, el.getValue());
                                    el.focus();
                                    return false;
                                }
                                else {
                                    me.nodeIndex = -1;
                                }
                            }
                        }
                    }, {
                        region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5',
                        handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(leftTree, el.getValue()); el.focus(); }
                    }]
                }, leftTree]
            });

            var tree = Ext.create('Ext.ng.TreePanel', {
                //collapsible: true,
                //split: true,
                //width: 180,
                region: 'center',
                autoLoad: false,
                treeFields: [{ name: 'text', type: 'string' },
                   { name: 'row', type: 'string' }//我的自定义属性                            
                ],
                url: C_ROOT + "SUP/RichHelp/GetTreeList?helpid=" + me.helpid + '&ORMMode=' + me.ORMMode
            });

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode + '&UIContainerID=' + me.infoRightUIContainerID
            });

            tree.on('itemdblclick', function (treepanel, record, item, index, e, eOpts) {

                var code = record.data.id;
                var name = record.data.text;

                var obj = new Object();
                obj[me.valueField] = code;

                if (me.displayFormat) {
                    obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
                } else {
                    obj[me.displayField] = name;
                }

                var valuepair = Ext.ModelManager.create(obj, 'model');
                me.setValue(valuepair); //必须这么设置才能成功                
                win.hide();
                win.close();
                win.destroy();

                var pobj = new Object();
                pobj.code = code;
                pobj.name = name;
                pobj.type = 'fromhelp';

                var index = store.find(me.valueField, code);
                pobj.data = Ext.decode(record.data.row);
                me.fireEvent('helpselected', pobj);

            });

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store
            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                //frame: false,
                //border: false,
                store: store,
                //autoScroll:true,                    
                columnLines: true,
                columns: gridColumns,
                bbar: pagingbar
            });

            var commonUseStore = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                //pageSize: 20,
                autoLoad: false,
                url: C_ROOT + 'SUP/RichHelp/GetCommonUseList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });
            //常用数据
            var commonUseGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                columnLines: true,
                columns: gridColumns,
                store: commonUseStore
            });

            var richqueryStore = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                pageSize: 20,
                autoLoad: false,
                url: C_ROOT + 'SUP/RichHelp/GetRichQueryList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode + '&UIContainerID=' + me.infoRightUIContainerID
            });

            var richqueryPagingbar = Ext.create('Ext.ng.PagingBar', {
                store: richqueryStore
            });
            //高级查询列表
            var richqueryGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                columnLines: true,
                columns: gridColumns,
                store: richqueryStore,
                bbar: richqueryPagingbar
            });
            //查询面板
            var queryPanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'east',
                //frame:false,            
                //title: '查询条件',
                split: true,
                width: 260,
                //minWidth: 100,
                autoScroll: true,
                columnsPerRow: 2,
                fieldDefaults: {
                    //labelAlign: 'right', //'top',
                    labelWidth: 30,
                    anchor: '100%',
                    margin: '3 5 3 0',
                    msgTarget: 'side'
                },
                fields: richQueryItem,
                dockedItems: [{
                    xtype: 'toolbar',
                    dock: 'bottom',
                    ui: 'footer',
                    items: ['->', { xtype: 'button', text: '保存', handler: function () { me.saveQueryFilter(me.helpid, queryPanel); } },
                                      { xtype: 'button', text: '设置', handler: function () { me.setQueryInfo(me.helpid); } },
                                      { xtype: 'button', text: '搜索', handler: function () { me.richQuerySearch(queryPanel, richqueryStore); } },
                                      { xtype: 'button', text: '清空', handler: function () { queryPanel.getForm().reset(); } }
                    ]
                }]

            });

            var tabItems = [];

            tabItems.push({ layout: 'border', title: '列表', id: 'listStyle', items: [grid] });
            if (showTree) {
                tabItems.push({ layout: 'border', title: '树型', id: 'treeStyle', items: [tree] });
            }
            if (me.showCommonUse) {
                tabItems.push({ layout: 'border', title: '常用', id: 'commonData', items: [commonUseGrid] });
            }
            if (me.showRichQuery) {
                tabItems.push({ layout: 'border', title: '高级', id: 'richquery', items: [richqueryGrid, queryPanel] });
            }

            var tabPanel = Ext.create('Ext.tab.Panel', {
                layout: 'border',
                region: 'center',
                deferredRender: false,
                plain: true,
                activeTab: 0,
                tabBar: {
                    height: 28
                },
                defaults: { bodyStyle: 'padding:3px' },
                items: tabItems
            });

            var commlistLoaded = false; //已经加载标记
            tabPanel.on('tabchange', function (tabpanel, nCard, oCard, eOpts) {

                if (nCard.id === 'treeStyle') {
                    tree.getRootNode().expand();
                    if (win.queryById('richhelp_add')) {
                        win.queryById('richhelp_add').enable(true);
                        win.queryById('richhelp_del').disable(true);
                    }                   
                }
                if (nCard.id === 'commonData') {
                    if (win.queryById('richhelp_add')) {
                        win.queryById('richhelp_add').disable(true);
                        win.queryById('richhelp_del').enable(true);
                    }
                    if (!commlistLoaded) {
                        commonUseStore.load();
                        commlistLoaded = true;
                    }
                }
                if (nCard.id === 'listStyle') {
                    if (win.queryById('richhelp_add')) {
                        win.queryById('richhelp_del').disable(true);
                        win.queryById('richhelp_add').enable(true);
                    }
                }
                if (nCard.id === 'richquery') {
                    me.richQuerySearch(queryPanel, richqueryStore);
                    if (win.queryById('richhelp_add')) {
                        win.queryById('richhelp_del').disable(true);
                        win.queryById('richhelp_add').enable(true);
                    }
                }
            });

            grid.on('itemdblclick', function () {
                me.gridDbClick(me, grid, win);
            });

            commonUseGrid.on('itemdblclick', function () {
                me.gridDbClick(me, commonUseGrid, win);
            });

            richqueryGrid.on('itemdblclick', function () {
                me.gridDbClick(me, richqueryGrid, win);
            });

            queryPanel.on('afterrender', function () {
                queryPanel.getForm().setValues(richQueryFilter); //设置值
                BatchBindCombox(queryPanel.getForm().getFields().items); //代码转名称
            });

            var winItems = [];
            if (existQueryProperty) {
                toolbar.queryById('ch-treerem').show();//显示树记忆
                winItems.push(toolbar);
                winItems.push(queryProperty);
                winItems.push(leftPanel);
                winItems.push(tabPanel);
            }
            else {
                winItems.push(toolbar);
                winItems.push(tabPanel);
               
            }

            var buttons = [];

            if (me.showCommonUse) {
                buttons.push({
                    itemId: 'richhelp_add', text: '添加常用', handler: function () {

                        var activeTab = tabPanel.getActiveTab();
                        if (activeTab.id === 'treeStyle') {
                            if (tree.getSelectionModel().selected.items.length > 0) {
                                var data = tree.getSelectionModel().selected.items[0].data;

                                if (me.onlyLeafValid && !data.leaf) {
                                    NGMsg.Warn("不是叶子节点不能加入常用");
                                    return;
                                }
                                var code = data.id;
                                if (data && !Ext.isEmpty(code)) {
                                    me.addCommonUseData(me, code, commonUseStore);
                                }
                            }
                        }
                        else{
                            var data = grid.getSelectionModel().getSelection();
                            if (data.length > 0) {

                                var valField = me.valueField;
                                var temp = me.valueField.split('.');//多表关联的时候带表名
                                if (temp.length > 1) {
                                    valField = temp[1];//去表名
                                }
                                var code = data[0].get(valField);
                                me.addCommonUseData(me, code, commonUseStore);
                            }
                        }                        
                    }
                });
                buttons.push({
                    itemId: 'richhelp_del', text: '删除常用', disabled: true, handler: function () {
                        me.delCommonUseData(me, commonUseGrid, commonUseStore)
                    }
                });
            }

            if (newUrl) {
                buttons.push({
                    itemId: 'newEdit', text: '新增', handler: function () {
                        $OpenTab('新增', C_ROOT + newUrl);
                    }
                });
            }
            buttons.push('->');
            buttons.push({ text: '确定', handler: function () { me.btnOk(me, grid, tree, tabPanel, commonUseGrid, richqueryGrid, win); } });
            buttons.push({ text: '取消', handler: function () { win.close(); } });


            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: me.helpTitle || title,
                border: false,               
                //style:{
                //    opacity: '0.85'                   
                //},
                height: me.helpHeight,
                width: me.helpWidth,
                draggable: me.helpDraggable,
                resizable: me.helpResizable,
                maximizable: me.helpMaximizable,
                layout: 'border',
                y: 100,
                modal: true,
                //constrain: true,
                constrainHeader: true,
                items: winItems, //[toolbar, queryProperty, tabPanel],
                buttons: buttons,
                listeners: {
                    beforeshow: $winBeforeShow,
                    beforeclose: $winBeforeClose
                }
            });
            win.show();

            //触发选择改变事件，加载左边树            
            if (me.selectQueryProIndex != 0) {
                var radioGroup = Ext.getCmp('radioQueryPro');
                radioGroup.fireEvent('change', radioGroup);
            }

            me.isShowing = false;
            //store.load();//手工调不会触发beforeload事件

            store.on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                Ext.apply(store.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });
            
            tree.getStore().on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                var treeStore = tree.getStore();
                Ext.apply(treeStore.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(treeStore.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }            
                if (me.clientSqlFilter) {
                    Ext.apply(treeStore.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });

            store.on('load', function (store, records, successful, eOpts) {
                
                if (me.needBlankLine) {
                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    //emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                }

            });

            richqueryStore.on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                Ext.apply(richqueryStore.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });

        };
        
        var url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    title = me.title || resp.data.Title;
                    allfield = resp.data.AllField;
                    headText = resp.data.HeadText;
                    model = resp.data.model;
                    columns = resp.data.columns;
                    existQueryProperty = resp.data.existQueryProp;
                    queryPropertyItems = Ext.JSON.decode(resp.data.queryProperty);
                    showTree = (resp.data.showTree == '1');
                    richQueryItem = Ext.JSON.decode(resp.data.richQueryItem);
                    richQueryFilter = Ext.JSON.decode(resp.data.queryFilter);
                    newUrl = resp.data.newUrl;
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },
    showHelp: function (eOption, ignoreBeforeEvent) {
        this.onTriggerClick(eOption, ignoreBeforeEvent);//忽略beforetriggerclick事件，手动弹出帮助
    },
    bindData: function () {
        var me = this;
        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);
        return;
    }, //bindData
    btnOk: function (help, grid, tree, tabPanel, commonUseGrid, richqueryGrid, win) {

        var activeTab = tabPanel.getActiveTab();
        var code;
        var name;
        var pobj = new Object();

        var valField = help.valueField;
        var temp = help.valueField.split('.');//多表关联的时候带表名,否则智能搜索报错
        if (temp.length > 1) {
            valField = temp[1];//去表名
        }

        var nameField = help.displayField;
        var temp = help.displayField.split('.');//多表关联的时候带表名
        if (temp.length > 1) {
            nameField = temp[1];//去表名
        }

        if (activeTab.id === 'listStyle') {
            var data = grid.getSelectionModel().getSelection();
            if (data.length > 0) {
                code = data[0].get(valField);
                name = data[0].get(nameField);
                if (!code) {
                    var obj = data[0].data;
                    //容错处理，带表名获取不到值
                    for (var p in obj) {

                        var field = [];
                        if (p.indexOf('.') > 0) {
                            field = p.split('.');
                        }

                        if (field[1] === valField) {
                            code = obj[p];
                        }
                        if (field[1] === nameField) {
                            name = obj[p];
                        }

                    }
                }

                pobj.data = data[0].data;
            }
        }
        if (activeTab.id === 'commonData') {
            var data = commonUseGrid.getSelectionModel().getSelection();
            if (data.length > 0) {
                code = data[0].get(valField);
                name = data[0].get(nameField);
                pobj.data = data[0].data;
            }
        }
        if (activeTab.id === 'richquery') {
            var data = richqueryGrid.getSelectionModel().getSelection();
            if (data.length > 0) {
                code = data[0].get(valField);
                name = data[0].get(nameField);
                pobj.data = data[0].data;
            }
        }
        if (activeTab.id === 'treeStyle') {
            var selectM = tree.getSelectionModel()
            var select = selectM.getSelection();

            code = select[0].data.id;
            name = select[0].data.text;
            pobj.data = Ext.decode(select[0].data.row);
        }


        var obj = new Object();
        //obj[valField] = code;

        if (help.acceptInput) {//接受用户输入
            obj[valField] = name; 
        } else {
            obj[valField] = code;
        }
        if (help.displayFormat) {
            obj[nameField] = Ext.String.format(help.displayFormat, code, name);
        } else {
            obj[nameField] = name;
        }

        Ext.define('richhelpModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: valField,
                type: 'string',
                mapping: valField
            }, {
                name: nameField,
                type: 'string',
                mapping: nameField
            }
			     ]
        });

        //        var valuepair = Ext.ModelManager.create(obj, 'richhelpModel');

        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';       
        if (!help.fireEvent('beforehelpselected', pobj)) return;

        var valuepair = Ext.create('richhelpModel', obj);
        help.setValue(valuepair); //必须这么设置才能成功
        //        help.setHiddenValue(code);
        //        help.setRawValue(name);

        win.hide();
        win.close();
        win.destroy();
               
        help.fireEvent('helpselected', pobj);

    },
    addCommonUseData: function (help, code, commonUseStore) {
     
        var index = commonUseStore.find(help.valueField, code); //去重
        if (index < 0) {

            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/SaveCommonUseData',
                params: { 'helpid': help.helpid, 'codeValue': code },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        //commonUseStore.insert(commonUseStore.count(), data[0].data);\
                        commonUseStore.load();
                        NGMsg.Info("成功添加常用");
                    } else {
                        Ext.MessageBox.alert('保存失败', resp.Msg, resp.status);
                    }
                }
            });
        }
        
    },
    delCommonUseData: function (help, commonUseGrid, commonUseStore) {
        var data = commonUseGrid.getSelectionModel().getSelection();
        if (data.length > 0) {

            var valField = help.valueField;
            var temp = help.valueField.split('.');//多表关联的时候带表名
            if (temp.length > 1) {
                valField = temp[1];//去表名
            }
            var code = data[0].get(valField);
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/DeleteCommonUseData',
                params: { 'helpid': help.helpid, 'codeValue': code },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        commonUseStore.remove(data[0]); //移除
                    } else {
                        Ext.MessageBox.alert('删除失败!', resp.status);
                    }
                }
            });
        }
    },
    gridDbClick: function (help, grid, win) {
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {

            var valField = help.valueField;
            var temp = help.valueField.split('.');//多表关联的时候带表名
            if (temp.length > 1) {
                valField = temp[1];//去表名
            }

            var nameField = help.displayField;
            var temp = help.displayField.split('.');//多表关联的时候带表名
            if (temp.length > 1) {
                nameField = temp[1];//去表名
            }

            var code = data[0].get(valField);
            var name = data[0].get(nameField);

            if (!code) {
                var obj = data[0].data;
                //容错处理，model的字段有可能带表名获取不到值
                for (var p in obj) {

                    var field = [];
                    if (p.indexOf('.') > 0) {
                        field = p.split('.');
                    }

                    if (field[1] === valField) {
                        code = obj[p];
                    }
                    if (field[1] === nameField) {
                        name = obj[p];
                    }

                }
            }

            var obj = new Object();
            //obj[valField] = code;

            if (help.acceptInput) {//接受用户输入
                obj[valField] = name;
            } else {
                obj[valField] = code;
            }
            if (help.displayFormat) {
                obj[nameField] = Ext.String.format(help.displayFormat, code, name);
            } else {
                obj[nameField] = name;
            }

            Ext.define('richhelpModel', {
                extend: 'Ext.data.Model',
                fields: [{
                    name: valField,
                    type: 'string',
                    mapping: valField
                }, {
                    name: nameField,
                    type: 'string',
                    mapping: nameField
                }
			     ]
            });

            //            var valuepair = Ext.ModelManager.create(obj, 'richhelpModel');
            
            var pobj = new Object();

            pobj.oldVal = oldVal;
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = data[0].data;
            if (!help.fireEvent('beforehelpselected', pobj)) return;
            
            var oldVal = help.getValue();//旧的值
            var valuepair = Ext.create('richhelpModel', obj);
            help.setValue(valuepair); //必须这么设置才能成功
            //            help.setHiddenValue(code);
            //            help.setRawValue(name);
            win.hide();
            win.close();
            win.destroy();
            //if (me.isInGrid) {

            help.fireEvent('helpselected', pobj);
            //}

        }
    },
    initParam: function () {
        var me = this;
        me.memory = {};
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/RichHelp/GetTreeMemoryInfo',
            async: false,
            params: { type: me.helpType, busstype: me.bussType },
            success: function (response, opts) {
                me.memory = Ext.JSON.decode(response.responseText);
            }
        });
    },
    saveTreeMemory: function (tree, checked) {
        var me = this;
        if (!me.memory) { return; }
        if (me.memory.eOpts == "selectionchange") { return; }
        var sd = tree.getSelectionModel().getSelection();
        if (sd.length > 0) {
            me.memory.FoucedNodeValue = sd[0].getPath();
            me.memory.IsMemo = checked;
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/UpdataTreeMemory',
                async: true,
                params: { type: me.helpType, busstype: me.bussType, foucednodevalue: me.memory.FoucedNodeValue, ismemo: checked },
                success: function (response, opts) {
                }
            });
        }
    },
    richQuerySearch: function (queryPanel, richqueryStore) {
        var query = JSON.stringify(queryPanel.getForm().getValues());
        Ext.apply(richqueryStore.proxy.extraParams, { 'query': query });
        richqueryStore.load();
    },
    setQueryInfo: function (helpid) {

        var toolbar = Ext.create('Ext.Toolbar', {
            region: 'north',
            border: false,
            height: 36,
            minSize: 26,
            maxSize: 26,
            items: [{ id: "query_save", text: "保存", width: this.itemWidth, iconCls: "icon-save" },
                           { id: "query_addrow", text: "增行", width: this.itemWidth, iconCls: "icon-AddRow" },
                           { id: "query_deleterow", text: "删行", width: this.itemWidth, iconCls: "icon-DeleteRow" },
                            '->',
                            { id: "query_close", text: "关闭", width: this.itemWidth, iconCls: "icon-Close", handler: function () { win.close(); } }
                           ]
        });

        //定义模型
        Ext.define('queryInfoModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'code',
                mapping: 'code',
                type: 'string'
            }, {
                name: 'tablename',
                mapping: 'tablename',
                type: 'string'
            }, {
                name: 'field',
                mapping: 'field',
                type: 'string'
            }, {
                name: 'fname_chn',
                mapping: 'fname_chn',
                type: 'string'
            }, {
                name: 'fieldtype',
                mapping: 'fieldtype',
                type: 'string'
            }, {
                name: 'operator',
                mapping: 'operator',
                type: 'string'
            }, {
                name: 'defaultdata',
                mapping: 'defaultdata',
                type: 'string'
            }, {
                name: 'displayindex',
                mapping: 'displayindex',
                type: 'number'
            }, {
                name: 'definetype',
                mapping: 'definetype',
                type: 'string'
            }, ]
        });

        var richQueryStore = Ext.create('Ext.ng.JsonStore', {
            model: 'queryInfoModel',
            autoLoad: true,
            pageSize: 50,
            url: C_ROOT + 'SUP/RichHelp/GetRichQueryUIInfo?helpid=' + helpid
        });

        var richQueryCellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
            clicksToEdit: 1
        });

        var operatorType = Ext.create('Ext.ng.ComboBox', {
            valueField: "code",
            displayField: 'name',
            queryMode: 'local',                           //local指定为本地数据  如果是后台传输  值为remote     
            name: 'mode',
            datasource: 'default',
            data: [{             //编辑状态下,状态列的下拉菜单的 data
                "code": "eq",
                "name": "="
            }, {
                "code": "gt",
                "name": ">"
            }, {
                "code": "lt",
                "name": "<"
            }, {
                "code": "ge",
                "name": ">="
            }, {
                "code": "le",
                "name": "<="
            }, {
                "code": "like",
                "name": "%*%"
            }, {
                "code": "LLike",
                "name": "*%"
            }, {
                "code": "RLike",
                "name": "%*"
            }]
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            width: 200,
            stateful: true,
            //stateId: 'sysgrid',
            store: richQueryStore,
            otype: otype,
            buskey: 'code', //对应的业务表主键               
            columnLines: true,
            columns: [{
                header: '代 码',
                flex: 1,
                sortable: false,
                dataIndex: 'code',
                hidden: true
            }, {
                header: '字段类型',
                flex: 1,
                sortable: false,
                dataIndex: 'fieldtype',
                hidden: true
            }, {
                header: '表名',
                flex: 1,
                sortable: false,
                dataIndex: 'tablename'
            }, {
                header: '字段',
                flex: 1,
                sortable: false,
                dataIndex: 'field'
            }, {
                header: '字段名称',
                flex: 1,
                sortable: false,
                dataIndex: 'fname_chn'
            }, {
                header: '运算符',
                flex: 1,
                sortable: false,
                dataIndex: 'operator',
                editor: operatorType,
                renderer: function (val) {
                    var ret;
                    var index = operatorType.getStore().find('code', val);
                    var record = operatorType.getStore().getAt(index);
                    if (record) {
                        ret = record.data.name;
                    }
                    return ret;
                }
            }, {
                header: '默认值',
                flex: 1,
                sortable: false,
                dataIndex: 'defaultdata',
                editor: {}
            }, {
                header: '排序号',
                flex: 1,
                sortable: false,
                dataIndex: 'displayindex',
                editor: { xtype: 'numberfield' }
            }, {
                header: '定义类型',
                flex: 1,
                sortable: false,
                dataIndex: 'definetype',
                renderer: function (val) {
                    if (val === '1') {
                        return "用户定义";
                    }
                    else {
                        return "系统定义";
                    }
                }
            }],
            plugins: [richQueryCellEditing]
        });

        //显示弹出窗口
        var win = Ext.create('Ext.window.Window', {
            title: '查询条件设置',
            border: false,
            height: 400,
            width: 600,
            layout: 'border',
            modal: true,
            items: [grid],
            buttons: ['->',
                    { text: '确定', handler: function () { Save(); win.close(); } },
                    { text: '取消', handler: function () { win.close(); } }]
        });
        win.show();

        function Save() {
            var griddata = grid.getAllGridData(); //grid.getChange();
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/SaveQueryInfo?helpid=' + helpid,
                params: { 'griddata': griddata },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        richQueryStore.commitChanges();
                    } else {
                        Ext.MessageBox.alert('保存失败', resp.status);
                        name = 'error';
                    }
                }
            });
        }

        toolbar.items.get('query_save').on('click', function () {
            Save()
        });

        var data = [{             //编辑状态下,状态列的下拉菜单的 data
            "code": "eq",
            "name": "="
        }, {
            "code": "gt",
            "name": ">"
        }, {
            "code": "lt",
            "name": "<"
        }, {
            "code": "ge",
            "name": ">="
        }, {
            "code": "le",
            "name": "<="
        }, {
            "code": "like",
            "name": "%*%"
        }, {
            "code": "LLike",
            "name": "*%"
        }, {
            "code": "RLike",
            "name": "%*"
        }];

        var otherData = [{             //编辑状态下,状态列的下拉菜单的 data
            "code": "eq",
            "name": "="
        }, {
            "code": "gt",
            "name": ">"
        }, {
            "code": "lt",
            "name": "<"
        }, {
            "code": "ge",
            "name": ">="
        }, {
            "code": "le",
            "name": "<="
        }];

        grid.on('itemclick', function (grid, record, item, index, e, eOpts) {

            var ftype = record.data['fieldtype'];
            if (ftype === 'Number' || ftype === 'Date') {

                if (operatorType.datasource === 'default') {
                    operatorType.getStore().loadData(otherData);
                    operatorType.datasource = 'other';
                }
            }
            else {
                if (operatorType.datasource === 'other') {
                    operatorType.getStore().loadData(data);
                    operatorType.datasource = 'default';
                }
            }
        });

    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = tree, index = -1;
        var firstFind = false;
        if (isNaN(me.nodeIndex) || me.nodeIndex == null || me.value != value) {
            me.nodeIndex = -1;
            me.value = value;
        }
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
            }
            me.nodeIndex = -1;
        }
    },
    saveQueryFilter: function (helpid, qpanel) {
        var data = JSON.stringify(qpanel.getForm().getValues());

        if (data === '{}') return; //值为空

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/RichHelp/SaveQueryFilter',
            async: true,
            params: { 'helpid': helpid, 'data': data },
            success: function (response, opts) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status === "success") {
                    Ext.MessageBox.alert('保存成功!');
                }
            }
        });
    },
    getCodeName: function (value) {
        var me = this;
        var name;

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/RichHelp/GetName?helptype=Single&helpid=' + me.helpid + '&code=' + value,
            async: false, //同步请求
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    name = resp.name; //显示值                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                    name = 'error';
                }
            }
        });

        return name;
    },
    setOutFilter: function (obj) {        
        Ext.apply(this.store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(obj) });
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {       
        Ext.apply(this.store.proxy.extraParams, { 'clientSqlFilter': str });
        this.clientSqlFilter = str;
    },
    getFirstRowData: function () {
        var me = this;
        if (!me.listFields) {
            Ext.Msg.alert('提示', '请设置帮助的listFields属性！');
            return;
        }
        var fields = me.listFields.split(',');

        var modelFields = new Array();
        for (var i = 0; i < fields.length; i++) {

            var tempfield = fields[i].split('.');
            var temp;
            if (tempfield.length > 1) {
                temp = tempfield[1]; //去掉表名
            }
            else {
                temp = fields[i];
            }

            modelFields.push({
                name: fields[i],
                type: 'string',
                mapping: temp
            });
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: modelFields
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 20,
            autoLoad: false,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function () {

            //            var data = new Object();
            //            data[me.valueField] = value;

            if (me.outFilter) {
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
            if (me.firstRowFilter) {//加载第一行的过滤条件
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.firstRowFilter) });
            }

        })

        store.load(function () {
            if (store.data.items.length > 0) {
                var data = store.data.items[0].data;
                me.fireEvent('firstrowloaded', data);
            }
            else {
                me.fireEvent('firstrowloaded', undefined);
            }
        });

    },
    alignPicker: function () {
        var me = this;
        var picker = me.getPicker();
        var fieldWidth = me.bodyEl.getWidth();     
        if (320 < fieldWidth) {
            picker.setWidth(fieldWidth);
        }
        else {//解决分页条出不来
            picker.setWidth(320);
        }
        me.callParent();
    },
    reConfigHelp: function (config) {
        this.helpid = config.helpid;
        this.valueField = config.valueField;
        this.displayField = config.displayField;
    }
});

//基于数据库配置的多选通用帮助
Ext.define('Ext.ng.MultiRichHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: ['widget.ngMultiRichHelp'],
    selectMode: 'Multi',
    multiSelect: false,//智能搜索为多选再设置为true,getValue()会得到数组
    showCommonUse: true, //是否显示常用
    showSelectedData: true,//是否显示已选数据
    helpWidth: 880, //帮助宽度
    helpHeight: 400, //帮助高度
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击           

        if (Ext.isEmpty(me.helpid)) return;
        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;
        var model;
        var columns;

        var existQueryProperty = false;
        var queryPropertyItems;
        var showTree;
        var richQueryItem;
        var richQueryFilter;

        if (!me.fireEvent('beforetriggerclick', me)) return;      

        ShowHelp = function () {

            //var queryItems;
            var modelFields;
            var gridColumns;

            if (!model.length) {
                NGMsg.Error('获取帮助信息失败，请检查数据库通用帮助配置信息是否正确!');
                me.isShowing = false;
                return;
            }

            //var fields = allfield.split(','); //所有字段
            //var heads = headText.split(','); //列头

            modelFields = model; //new Array();

            //for (var i = 0; i < fields.length; i++) {
            //    var tempfield = fields[i].split('.');
            //    var temp;
            //    if (tempfield.length > 1) {
            //        temp = tempfield[1]; //去掉表名
            //    }
            //    else {
            //        temp = fields[i];
            //    }
            //    var ar = temp.split(' ');//取别名
            //    if (ar.length > 1) {
            //        temp = ar[ar.length - 1].trim();
            //    }
            //    modelFields.push({
            //        name: fields[i], //temp //不去掉表名
            //        type: 'string',
            //        mapping: temp //不能带表名，带点号grid数据展示不出来
            //    });
            //}

            for (var i = 0; i < columns.length; i++) {
                var renderer = columns[i].renderer;
                if (renderer) {
                    columns[i].renderer = Ext.decode(renderer);
                }
            }
            gridColumns = columns; //new Array();

            //for (var i = 0; i < heads.length; i++) {
            //    var tempfield = fields[i].split('.');
            //    var temp;
            //    if (tempfield.length > 1) {
            //        temp = tempfield[1]; //去掉表名
            //    }
            //    else {
            //        temp = fields[i];
            //    }
            //    if (heads.length > 5) {
            //        gridColumns.push({
            //            header: heads[i],                       
            //            width: 200,
            //            dataIndex: fields[i] //与model的name属性对应
            //        });
            //    }
            //    else {
            //        gridColumns.push({
            //            header: heads[i],
            //            flex: 1,                     
            //            dataIndex: fields[i] //与model的name属性对应
            //        });
            //    }              
            //}


            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 36,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    xtype: 'textfield',
								    itemId: "searchkey",
								    width: 200
								},
								{
								    itemId: 'richhelp_query',
								    iconCls: 'icon-View'
								},
                                {
                                    itemId: 'richhelp_refresh',
                                    iconCls: 'icon-Refresh'
                                }, {
                                    xtype: 'checkboxfield',
                                    boxLabel: '在结果中搜索',
                                    width: 100,
                                    itemId: 'ch-searchInResult',
                                    inputValue: '01'
                                },
                                '->',
							     {
							         xtype: 'checkboxgroup',
							         name: 'hobby',
							         items: [                                       
                                        {
                                            boxLabel: '树记忆', width: 60, itemId: 'ch_m_treerem', inputValue: '02', hidden: true, handler: function (chk) {
                                                me.saveTreeMemory(leftTree, chk.getValue());
                                                var k = 0;
                                            }
                                        }
							         ]
							     }
                ]
            });

            var searcheArr = [];
            var searchIndex = {}; //索引
            toolbar.queryById('ch-searchInResult').on('change', function (me, nvalue, ovalue, eOpts) {

                if (false == nvalue) {
                    searcheArr.length = 0; //清空条件列表
                    searchIndex = {}; //清空索引
                }

            });

            toolbar.queryById('richhelp_query').on('click', function () {
                var searchkey;
                var key = toolbar.queryById('searchkey').getValue();
                if (toolbar.queryById('ch-searchInResult').getValue()) {
                  

                    if (!searchIndex[key]) {
                        searcheArr.push(key);
                        searchIndex[key] = key;
                    }

                    searchkey = searcheArr;
                }
                else {
                    searcheArr.length = 0;
                    searcheArr.push(key);
                }

                Ext.apply(store.proxy.extraParams, { 'searchkey': searcheArr });
                store.loadPage(1);

            });

            toolbar.queryById('richhelp_refresh').on('click', function () {
                toolbar.queryById('searchkey').setValue('');

                if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                    delete store.proxy.extraParams.searchkey;
                    delete store.proxy.extraParams.treesearchkey;
                    delete store.proxy.extraParams.treerefkey;
                    store.load();
                }
            });

            var propertyCode = queryPropertyItems[0].code;
            var propertyID = queryPropertyItems[0].inputValue;
            var queryProperty = Ext.create('Ext.container.Container', {
                region: 'north',
                //frame: true,
                border: false,
                //layout: 'auto', //支持自适应 	                                  
                items: [{
                    xtype: 'fieldset', //'fieldcontainer',
                    title: '查询属性', //fieldLabel: 'Size',
                    defaultType: 'radiofield',
                    defaults: {
                        flex: 1
                    },
                    layout: 'column',
                    fieldDefaults: {
                        margin: '0 10 0 0'
                    },
                    items: [{
                        xtype: 'radiogroup',
                        layout: 'column',
                        fieldDefaults: {
                            margin: '0 10 3 0'
                        },
                        activeItem: 0,
                        items: queryPropertyItems,
                        listeners: {
                            'change': function (radioCtl, nvalue, ovalue) {

                                var select = radioCtl.getChecked();
                                if (select.length > 0) {

                                    leftPanel.setTitle(select[0].boxLabel);
                                    var code = select[0].code; //加载树的搜索id
                                    propertyCode = code;
                                    propertyID = select[0].inputValue;

                                    Ext.Ajax.request({
                                        //params: { 'id': busid },
                                        url: C_ROOT + 'SUP/RichHelp/GetListExtendInfo?code=' + propertyCode,
                                        //callback: ShowHelp,
                                        success: function (response) {
                                            var resp = Ext.JSON.decode(response.responseText);
                                            var extFields = resp.extfields; //扩展字段
                                            var extHeader = resp.extheader; //扩展列头

                                            var fields = Ext.clone(modelFields);
                                            var columns = Ext.clone(gridColumns);

                                            if (extHeader && extHeader != '') {
                                                var tempfs = extFields.split(',');
                                                var cols = extHeader.split(',');
                                                for (var i = 0; i < tempfs.length; i++) {
                                                    fields.push({
                                                        name: tempfs[i],
                                                        type: 'string',
                                                        mapping: tempfs[i]
                                                    });

                                                    columns.push({
                                                        header: cols[i],
                                                        flex: 1,
                                                        dataIndex: tempfs[i]
                                                    });
                                                }
                                            }

                                            //使用外部的store
                                            store = Ext.create('Ext.ng.JsonStore', {
                                                fields: fields,
                                                pageSize: 20,
                                                autoLoad: true,
                                                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                                                listeners: {
                                                    'beforeload': function () {
                                                        var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                                                        Ext.apply(store.proxy.extraParams, data);
                                                        if (me.likeFilter) {
                                                            Ext.apply(data, me.likeFilter);
                                                        }
                                                        if (me.outFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                                                        }
                                                        if (me.leftLikeFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                                                        }
                                                        if (me.clientSqlFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                                                        }
                                                    } //beforeload function
                                                }//listeners
                                            });
                                            //重新配置grid
                                            grid.reconfigure(store, columns);
                                            pagingbar.bind(store);
                                        }
                                    });
                                }

                                if (nvalue.property === 'all') {

                                    toolbar.queryById('ch_m_treerem').hide();

                                    leftPanel.setVisible(false);
                                    //leftTree.setVisible(false);
                                    if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                                        delete store.proxy.extraParams.searchkey;
                                        delete store.proxy.extraParams.treesearchkey;
                                        delete store.proxy.extraParams.treerefkey;
                                        store.load();
                                    }
                                    return;
                                } else {

                                    me.initParam();
                                    toolbar.queryById('ch_m_treerem').setVisible(true);

                                    var rootNode = leftTree.getRootNode();
                                    if (leftTree.isFirstLoad) {
                                        rootNode.expand(); //expand会自动调用load
                                        leftTree.isFirstLoad = false;
                                    }
                                    else {
                                        leftTree.getStore().load();
                                    }
                                    leftPanel.setVisible(true);
                                } //else
                            } //change function
                        }//listeners
                    }]
                }]
            });

            var leftTree = Ext.create('Ext.ng.TreePanel', {
                //title: queryPropertyItems[0].boxLabel,
                autoLoad: false,
                //collapsible: true,
                split: true,
                width: 180,
                region: 'west',
                //hidden: true,
                isFirstLoad: true,
                treeFields: [{ name: 'text', type: 'string' },
                   { name: 'treesearchkey', string: 'string' },
                   { name: 'treerefkey', type: 'string' }//我的自定义属性                
                ],
                url: C_ROOT + "SUP/RichHelp/GetQueryProTree",
                listeners: {
                    selectionchange: function (m, selected, eOpts) {
                        me.memory.eOpts = "selectionchange";

                        //刷列表数据
                        var record = selected[0];
                        if (record) {
                            if (!Ext.isEmpty(record.data.treesearchkey) && !Ext.isEmpty(record.data.treerefkey)) {
                                Ext.apply(store.proxy.extraParams, { 'treesearchkey': record.data.treesearchkey, 'treerefkey': record.data.treerefkey });
                                store.load();
                            }
                            //设置选中
                            if (toolbar.queryById('ch_m_treerem')) {
                                toolbar.queryById('ch_m_treerem').setValue(me.memory.IsMemo && me.memory.FoucedNodeValue == selected[0].getPath());
                            }
                            me.memory.eOpts = "";
                        }
                    },
                    viewready: function (m, eOpts) {
                        if (!Ext.isEmpty(me.memory.FoucedNodeValue)) {
                            leftTree.selectPath(me.memory.FoucedNodeValue, null, null, function () {
                                if (Ext.isIE) {
                                    window.setTimeout(function () {
                                        var selectNode = m.view.body.query("tr." + m.view.selectedItemCls);
                                        if (selectNode) {
                                            selectNode[0].scrollIntoView(true);
                                        }
                                    }, 500);
                                }
                            });
                        }
                        else {
                            store.load();
                        }
                    }
                }
            });

            leftTree.getStore().on('beforeload', function (store, operation, eOpts) {
                operation.params.code = propertyCode; //树添加参数
            });

            //leftTree.getStore().load(); //手动load，不然beforeload不起效果
            //leftTree.getRootNode().expand(); //expand会自动调用load

            var leftPanel = Ext.create('Ext.panel.Panel', {
                title: "人力资源树",
                autoScroll: false,
                collapsible: true,
                split: true,
                hidden: true,
                region: 'west',
                weight: 10,
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
                        emptyText: '输入关键字，定位树节点',
                        margin: '2 0 2 2',
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER) {
                                    me.findNodeByFuzzy(leftTree, el.getValue());
                                    el.focus();
                                    return false;
                                }
                                else {
                                    me.nodeIndex = -1;
                                }
                            }
                        }
                    }, {
                        region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5',
                        handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(leftTree, el.getValue()); el.focus(); }
                    }]
                }, leftTree]
            });

            var tree = Ext.create('Ext.ng.TreePanel', {
                //collapsible: true,
                //split: true,
                //width: 180,
                region: 'center',
                autoLoad: false,
                treeFields: [{ name: 'text', type: 'string' },
                   { name: 'row', type: 'string' }//我的自定义属性                            
                ],
                url: C_ROOT + "SUP/RichHelp/GetTreeList?helpid=" + me.helpid + '&ORMMode=' + me.ORMMode
            });

            //过滤条件处理
            tree.getStore().on('beforeload', function (store, operation, eOpts) {
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                pageSize: me.pageSize || 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store
            });

            tree.on('itemdblclick', function (treepanel, record, item, index, e, eOpts) {

                var code = record.data.id;
                var name = record.data.text;

                var obj = new Object();
                obj[me.valueField] = code;

                if (me.displayFormat) {
                    obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
                } else {
                    obj[me.displayField] = name;
                }

                var valuepair = Ext.ModelManager.create(obj, 'model');
                me.setValue(valuepair); //必须这么设置才能成功
                win.hide();
                win.close();
                win.destroy();

                var pobj = new Object();
                pobj.code = code;
                pobj.name = name;
                pobj.type = 'fromhelp';

                var index = store.find(me.valueField, code);
                pobj.data = Ext.decode(record.data.row);
                me.fireEvent('helpselected', pobj);

            });

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                //frame: false,
                //border: false,
                store: store,
                selModel: { mode: "SIMPLE" }, //多选                    
                columnLines: true,
                columns: gridColumns,
                bbar: pagingbar
            });

            var commonUseStore = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                //pageSize: 20,
                autoLoad: false,
                url: C_ROOT + 'SUP/RichHelp/GetCommonUseList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            var commonUseGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                columnLines: true,
                columns: gridColumns,
                store: commonUseStore,
                selModel: { mode: "SIMPLE" } //多选
            });

            var richqueryStore = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                pageSize: 20,
                autoLoad: false,
                url: C_ROOT + 'SUP/RichHelp/GetRichQueryList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            var richqueryPagingbar = Ext.create('Ext.ng.PagingBar', {
                store: richqueryStore
            });

            var richqueryGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                columnLines: true,
                columns: gridColumns,
                store: richqueryStore,
                selModel: { mode: "SIMPLE" }, //多选
                bbar: richqueryPagingbar
            });

            var queryPanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'east',
                //frame:false,            
                //title: '查询条件',
                split: true,
                width: 260,
                //minWidth: 100,
                autoScroll: true,
                columnsPerRow: 2,
                fieldDefaults: {
                    //labelAlign: 'right', //'top',
                    labelWidth: 30,
                    anchor: '100%',
                    margin: '3 5 3 0',
                    msgTarget: 'side'
                },
                fields: richQueryItem,
                dockedItems: [{
                    xtype: 'toolbar',
                    dock: 'bottom',
                    ui: 'footer',
                    items: ['->', { xtype: 'button', text: '保存', handler: function () { me.saveQueryFilter(me.helpid, queryPanel); } },
                                      { xtype: 'button', text: '设置', handler: function () { me.setQueryInfo(me.helpid); } },
                                      { xtype: 'button', text: '搜索', handler: function () { me.richQuerySearch(queryPanel, richqueryStore); } },
                                      { xtype: 'button', text: '清空', handler: function () { queryPanel.getForm().reset(); } }
                    ]
                }]

            });

            queryPanel.on('afterrender', function () {
                queryPanel.getForm().setValues(richQueryFilter); //设置值
                BatchBindCombox(queryPanel.getForm().getFields().items); //代码转名称
            });

            var tabItems = [{ layout: 'border', title: '列表', id: 'listStyle', items: [grid] }];

            if (showTree) {
                tabItems.push({ layout: 'border', title: '树型', id: 'treeStyle', items: [tree] });
            }
            if (me.showCommonUse) {
                tabItems.push({ layout: 'border', title: '常用', id: 'commonData', items: [commonUseGrid] });
            }
            if (me.showRichQuery) {
                tabItems.push({ layout: 'border', title: '高级', id: 'richquery', items: [richqueryGrid, queryPanel] });
            }

            var tabPanel = Ext.create('Ext.tab.Panel', {
                layout: 'border',
                region: 'center',
                deferredRender: false,
                plain: true,
                activeTab: 0,
                //minHeight: 360,
                //minWidth: 600,//不要设置，grid滚动条不出来
                defaults: { bodyStyle: 'padding:3px' },
                items: tabItems
            });

            var commlistLoaded = false; //已经加载标记
            tabPanel.on('tabchange', function (tabpanel, nCard, oCard, eOpts) {

                if (nCard.id === 'treeStyle') {
                    tree.getRootNode().expand();
                    if (win.queryById('mutilhelp_del')) {
                        win.queryById('mutilhelp_add').enable(true);
                        win.queryById('mutilhelp_del').disable(true);
                    }
                }
                if (nCard.id === 'commonData') {

                    if (win.queryById('mutilhelp_del')) {
                        win.queryById('mutilhelp_add').disable(true);
                        win.queryById('mutilhelp_del').enable(true);
                    }

                    if (!commlistLoaded) {
                        commonUseStore.load();
                        commlistLoaded = true;
                    }
                }
                if (nCard.id === 'listStyle') {

                    if (win.queryById('mutilhelp_del')) {
                        win.queryById('mutilhelp_del').disable(true);
                        win.queryById('mutilhelp_add').enable(true);
                    }

                }
                if (nCard.id === 'richquery') {
                    me.richQuerySearch(queryPanel, richqueryStore);
                }
            });

            grid.on('itemdblclick', function () {
                me.gridDbClick(me, grid, win);
            });

            commonUseGrid.on('itemdblclick', function () {
                me.gridDbClick(me, commonUseGrid, win);
            });

            richqueryGrid.on('itemdblclick', function () {
                me.gridDbClick(me, richqueryGrid, win);
            });

            var selectedLoaded = false;
            //已选值记忆
            store.on('load', function () {

                if (!me.showSelectedData) return;
                if ((!Ext.isEmpty(me.value) && me.value != '') || !Ext.isEmpty(me.selectedValue)) {
                    if (!selectedLoaded) {
                        var rows = 0;
                        var selectData = [];                      
                        
                        var tempValue = me.value;
                        if (!Ext.isEmpty(me.selectedValue)) {
                            tempValue = me.selectedValue;//grid里面的多选，把已选值传进来，如果通过value传入，点取消会显示编码
                        }

                        var vals = "";
                        if (Ext.isArray(tempValue)) {
                            vals = tempValue;//tempValue[0].split(',');
                        }
                        else {
                            vals = tempValue.split(',');
                        }
                        for (var i = 0; i < vals.length; i++) {
                            var index = store.find(me.valueField, vals[i]);
                            if (index == (-1)) {
                                index = store.find(me.displayField, vals[i]);
                            }
                            var record = store.getAt(index);
                            if (record) {
                                selectData.push(record);
                                rows++;
                            }
                        }

                        if (rows == vals.length) {
                            resultStore.insert(0, selectData); //批量插入
                        }
                        else {

                            Ext.Ajax.request({
                                params: { 'helpid': me.helpid, 'codes': me.selectedValue || me.value, 'mode': me.ORMMode },
                                url: C_ROOT + 'SUP/RichHelp/GetSelectedData',
                                success: function (response) {
                                    var resp = Ext.JSON.decode(response.responseText);
                                    if (resp.Record.length > 0) {
                                        //resultStore.insert(0, resp.Record);

                                        //处理数据集，可能modelfield不一致，grid的field带点号，model不能带
                                        var arr = [];
                                        var fields = resultStore.model.getFields();
                                        for (var i = 0; i < resp.Record.length; i++) {

                                            var record = resp.Record[i];
                                            var obj = {};
                                            for (var k = 0; k < fields.length; k++) {
                                                //if(fields[k].name.indexOf('.') > 0)
                                                obj[fields[k].name] = record[fields[k].mapping]; //数据以name为准                                              
                                            }
                                            arr.push(obj);
                                        }
                                        resultStore.insert(0, arr);
                                    } else {
                                        Ext.MessageBox.alert('获取失败');
                                    }
                                }
                            });
                        }
                        selectedLoaded = true;
                    } //if
                }
            });

            var resultStore = Ext.create('Ext.ng.JsonStore', {
                model: 'model'
            });

            var resultGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'east',
                //frame: true,
                width: 235,
                border: false,
                style: {
                    marginTop: '27px'
                },
                store: resultStore,
                selModel: { mode: "SIMPLE" }, //多选
                columnLines: true,
                columns: gridColumns
            });

            var btnPanel = Ext.create('Ext.panel.Panel', {
                region: 'east',
                width: 80,
                layout: 'absolute',
                border: false,
                frame: true,
                padding: 0,
                style: { borderColor: 'transparent', backgroundColor: 'transparent' }, //backgroundColor: 'transparent !important',marginTop: '22px',
                items: [{
                    xtype: 'button',
                    name: 'addSelect',
                    text: '&gt;',
                    x: 9,
                    y: 90,
                    width: 60,
                    handler: Ext.bind(function () {

                        var activeTab = tabPanel.getActiveTab();
                        var data;
                        if (activeTab.id === 'listStyle') {
                            data = grid.getSelectionModel().getSelection();
                        } else if (activeTab.id === 'commonData') {
                            data = commonUseGrid.getSelectionModel().getSelection();
                        } else if (activeTab.id === 'richquery') {
                            data = richqueryGrid.getSelectionModel().getSelection();
                        } else {
                            var selectM = tree.getSelectionModel()
                            var select = selectM.getSelection();
                            data = Ext.decode(select[0].data.row);
                        }
                        if (data) {
                            me.copyData(data, resultStore);
                        }
                    })
                }, {
                    xtype: 'button',
                    name: 'selectAll',
                    text: '&gt;&gt;',
                    x: 9,
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
                    x: 9,
                    y: 150,
                    width: 60,
                    handler: Ext.bind(function () {
                        var data = resultGrid.getSelectionModel().getSelection();
                        resultStore.remove(data);
                    })
                }, {
                    xtype: 'button',
                    name: 'removeAll',
                    text: '&lt;&lt;',
                    x: 9,
                    y: 180,
                    width: 60,
                    handler: Ext.bind(function () {
                        resultStore.removeAll();
                    })
                }]
            });

            var panel = Ext.create('Ext.panel.Panel', {
                region: 'center',
                //frame: true,
                border: false,
                layout: 'border',
                items: [tabPanel, btnPanel, resultGrid]
            });

            var winItems = [];
            if (existQueryProperty) {
                toolbar.queryById('ch_m_treerem').show();//显示树记忆
                winItems.push(toolbar);
                winItems.push(queryProperty);
                winItems.push(leftPanel);
                winItems.push(panel);
            }
            else {
                winItems.push(toolbar);
                winItems.push(panel);
            }


            var buttons = [];

            if (me.showCommonUse) {
                buttons.push({
                    itemId: 'mutilhelp_add', text: '添加常用', handler: function () {
                        var activeTab = tabPanel.getActiveTab();
                        if (activeTab.id === 'treeStyle') {
                            if (tree.getSelectionModel().selected.items.length > 0) {
                                var data = tree.getSelectionModel().selected.items[0].data;

                                if (me.onlyLeafValid && !data.leaf) {
                                    NGMsg.Warn("不是叶子节点不能加入常用");
                                    return;
                                }
                                var code = data.id;
                                if (data && !Ext.isEmpty(code)) {
                                    var codeArr = [];
                                    codeArr.push(code);
                                    me.addCommonUseData(me, codeArr, commonUseStore);
                                }
                            }
                        }
                        else {
                            var data = grid.getSelectionModel().getSelection();
                            if (data.length > 0) {

                                var codeArr = [];
                                for (var i = 0; i < data.length; i++) {
                                    var valField = me.valueField;
                                    var temp = me.valueField.split('.');//多表关联的时候带表名
                                    if (temp.length > 1) {
                                        valField = temp[1];//去表名
                                    }
                                    var code = data[i].get(valField);
                                    codeArr.push(code);
                                }
                                me.addCommonUseData(me, codeArr, commonUseStore);
                            }
                        }
                    }
                });
                buttons.push({
                    itemId: 'mutilhelp_del', text: '删除常用', disabled: true, handler: function () {
                        me.delCommonUseData(me, commonUseGrid, commonUseStore)
                    }
                });
            }
            buttons.push('->');
            buttons.push({ text: '确定', handler: function () { me.btnOk(me, resultStore); } });
            buttons.push({ text: '取消', handler: function () { win.close(); } });

            //显示弹出窗口
            win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                modal: true,
                //constrain: true,
                constrainHeader:true,
                items: winItems, //[toolbar, queryProperty, tabPanel],
                buttons: buttons,
                listeners: {
                    beforeshow: $winBeforeShow,
                    beforeclose: $winBeforeClose
                }
            });
            win.show();
            win.on('beforeclose', function () { me.fireEvent("beforehelpclose", me); });

            //store.load();//手工调不会触发beforeload事件

            store.on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                Ext.apply(store.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
                //return true;
            });

            richqueryStore.on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                Ext.apply(richqueryStore.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(richqueryStore.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });

        };


        var url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    title = resp.data.Title;
                    allfield = resp.data.AllField;
                    headText = resp.data.HeadText;
                    model = resp.data.model;
                    columns = resp.data.columns;
                    existQueryProperty = resp.data.existQueryProp;
                    queryPropertyItems = Ext.JSON.decode(resp.data.queryProperty);
                    showTree = (resp.data.showTree == '1');
                    richQueryItem = Ext.JSON.decode(resp.data.richQueryItem);
                    richQueryFilter = Ext.JSON.decode(resp.data.queryFilter);

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    },
    copyData: function (selectData, resultStore, tabPanel) {

        var me = this;

        var count = resultStore.getCount();
        var index = count;
        if (count > 0) {

            var valField = me.valueField;
            var arr = me.valueField.split('.');
            if (arr.length > 1) {
                valField = arr[1];//去掉表名
            }
            
            if (Ext.isArray(selectData)) {
                var dataLen = selectData.length;
                for (var i = 0; i < dataLen; i++) {
                    var sourceData = selectData[i].data[valField];
                    var hit = false;
                    for (var j = 0; j < count; j++) {
                        var selectedData = resultStore.data.items[j].data[valField];
                        if (sourceData === selectedData) {
                            hit = true;
                        }
                    }
                    if (!hit) {
                        resultStore.insert(index, selectData[i]);
                        index++;
                    }
                }
            }
            else {
                var sourceData = selectData[valField];
                var hit = false;
                for (var j = 0; j < count; j++) {
                    var selectedData = resultStore.data.items[j].data[valField];
                    if (sourceData === selectedData) {
                        hit = true;
                    }
                }
                if (!hit) {
                    resultStore.insert(index, selectData);
                }
            }

        } else {
            resultStore.insert(0, selectData); //批量插入
        }
    },
    btnOk: function (me, resultStore) {

        var values = new Array();
        var userCodes = new Array();
        var names = new Array();
        var arr = resultStore.data.items;
        for (var i = 0; i < arr.length; i++) {

            var code = arr[i].data[me.valueField];
            var name = arr[i].data[me.displayField];
            var userCode = arr[i].data[me.userCodeField];

            if (!code) {
                var obj = arr[i].data;
                //容错处理，带表名获取不到值
                for (var p in obj) {

                    var field = [];
                    if (p.indexOf('.') > 0) {
                        field = p.split('.');
                    }

                    if (field[1] === me.valueField) {
                        code = obj[p];
                    }
                    if (field[1] === me.displayField) {
                        name = obj[p];
                    }
                    if (field[1] === me.userCodeField) {
                        userCode = obj[p];
                    }

                }
            }

            values.push(code);
            names.push(name);
            userCodes.push(userCode);

        }
        var code = values.join(',');
        var name = names.join(',');
        var userCode = userCodes.join(',');

        var obj = new Object();
        obj[me.valueField] = code;
        if (me.displayFormat) {
            obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
        } else {
            obj[me.displayField] = name;
        }

        Ext.define('multimodel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: me.valueField,
                type: 'string',
                mapping: me.valueField
            }, {
                name: me.displayField,
                type: 'string',
                mapping: me.displayField
            }
			]
        });

        var valuepair = Ext.ModelManager.create(obj, 'multimodel');
        me.setValue(valuepair); //必须这么设置才能成功

        win.hide();
        win.close();
        win.destroy();
        //if (me.isInGrid) {

        var pobj = new Object();
        pobj.code = code;
        pobj.name = name;
        pobj.userCode = userCode;
        pobj.type = 'fromhelp';
        pobj.data = arr;

        me.fireEvent('helpselected', pobj);

    },
    addCommonUseData: function (help, codeArr, commonUseStore) {

        var toAddArr = [];
        for (var i = 0; i < codeArr.length; i++) {
            var index = commonUseStore.find(help.valueField, codeArr[i]); //去重
            if (index < 0) {
                toAddArr.push(codeArr[i]);
            }
        }
       
        if (toAddArr.length > 0) {

            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/SaveCommonUseData',
                params: { 'helpid': help.helpid, 'codeValue': toAddArr.join() },
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status === "success") {
                        //commonUseStore.insert(commonUseStore.count(), data[0].data);\
                        commonUseStore.load();
                        NGMsg.Info("成功添加常用");
                    } else {
                        Ext.MessageBox.alert('保存失败', resp.Msg, resp.status);
                    }
                }
            });
        }
        else {
            NGMsg.Info("无可添加数据");
        }

    },
    delCommonUseData: function (help, commonUseGrid, commonUseStore) {
        var data = commonUseGrid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var codeArr = [];
            for (var i = 0; i < data.length; i++) {
                var valField = help.valueField;
                var temp = help.valueField.split('.');//多表关联的时候带表名
                if (temp.length > 1) {
                    valField = temp[1];//去表名
                }
                var code = data[i].get(valField);
                codeArr.push(code);
            }         
            if (codeArr.length > 0) {
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/RichHelp/DeleteCommonUseData',
                    params: { 'helpid': help.helpid, 'codeValue': codeArr.join() },
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                            for (var i = 0; i < data.length; i++) {
                                commonUseStore.remove(data[i]); //移除
                            }
                        } else {
                            Ext.MessageBox.alert('删除失败!', resp.status);
                        }
                    }
                });
            }
        }
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = tree, index = -1;
        var firstFind = false;
        if (isNaN(me.nodeIndex) || me.nodeIndex == null || me.value != value) {
            me.nodeIndex = -1;
            me.value = value;
        }
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
            }
            me.nodeIndex = -1;
        }
    },
    showHelp: function () {
        this.onTriggerClick();
    },
    getValue: function () {
        var me = this;
        if (me.multiSelect && me.value.length > 0) {
            return me.value[0];//数组转成string返回，否则保存数据库值为["01,03"]
        }
        else {
            return me.value;
        }
    }
});

//基于数据库配置的通用帮助
Ext.define('Ext.ng.MasterSlaveHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngMSHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 750, //帮助宽度
    helpHeight: 400, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    ORMMode: true,
    selectMode: 'Multi', //  
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    queryMode: 'remote',
    triggerAction: 'all', //'query'
    selectQueryProIndex: 0,
    isShowing: false,
    editable: false,
    initComponent: function () {
        //
        var me = this;

        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换
        me.helpType = 'RichHelp_' + me.helpid;
        me.bussType = me.bussType || 'all';

        var selectedRecords = [];

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.editable) {
            if (me.listFields && me.listHeadTexts) {

                var listheaders = '';
                var listfields = '';

                var heads = me.listHeadTexts.split(','); //列头 
                var fields = me.listFields.split(','); //所有字段              

                var modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp //fields[i]
                    });

                }

                if (me.showAutoHeader) {

                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    listfields += '<td>{' + temp + '}</td>';
                }

                var temp;
                if (me.showAutoHeader) {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                } else {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }
                me.tpl = temp;

                


            }
            else {
                //me.initialListTemplate(); //初始化下拉列表样式 

                var modelFields = [{
                    name: me.valueField,
                    type: 'string',
                    mapping: me.valueField
                }, {
                    name: me.displayField,
                    type: 'string',
                    mapping: me.displayField
                }]

                var listfields = '<td>{' + me.valueField + '}</td>';
                listfields += '<td>{' + me.displayField + '}</td>';
                me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }

            var store = Ext.create('Ext.data.Store', {
                //var store = Ext.create('Ext.ng.JsonStore', {
                pageSize: 10,
                fields: modelFields,
                cachePageData: true,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);


            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });

            store.on('load', function (store, records, successful, eOpts) {

                var temp = store.data.items.concat(selectedRecords);
                //store.data.items = temp;
                store.loadData(temp);
                me.setValue(selectedRecords, false);               

                if (me.needBlankLine) {
                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                }

            });
        }

        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');
        me.addEvents('beforetriggerclick');

        me.on('select', function (combo, records, eOpts) {
                        
            //判断是否存在
            var isExist = function (record) {
                var flag = false;
                for (var i = 0; i < selectedRecords.length; i++) {
                    var myRecord = selectedRecords[i];

                    if (record.data[me.valueField] == myRecord.data[me.valueField]) {
                        flag = true;
                        break;
                    }
                }

                return flag;
            }

            var tempRecords = [];
            for (var i = 0; i < records.length; i++) {
                if (!isExist(records[i])) {
                    tempRecords.push(records[i]);
                }
            }

            selectedRecords = selectedRecords.concat(tempRecords);
            me.setValue(selectedRecords, false);

            var theField;

            var modelFileds;
            //构建
            if (me.listFields) {
                theField = [];
                modelFileds = []
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);


                    var obj = {
                        name: temp[i],
                        type: 'string',
                        mapping: temp[i]
                    }

                    modelFileds.push(obj);
                }
            }
            else {

                theField = [me.valueField, me.displayField];

                modelFileds = [{
                    name: help.valueField,
                    type: 'string',
                    mapping: help.valueField
                }, {
                    name: help.displayField,
                    type: 'string',
                    mapping: help.displayField
                }];

            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: modelFileds//theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            //            var code = combo.getValue() || records[0].data[myValueFiled];
            //            var name = combo.getRawValue() || records[0].data[myDisplayField];

            var codeArr = [];
            var nameArr = [];
            for (var i = 0; i < records.length; i++) {
                codeArr.push(records[i].data[myValueFiled]);
                nameArr.push(records[0].data[myDisplayField]);
            }

            var code = codeArr.join();
            var name = nameArr.join();

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            //select不需要设置value
            if (me.isInGrid) {//grid特殊处理,valueField也是name
                me.setValue(valuepair); //必须这么设置才能成功
            }

            //debugger;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    pobj.data[theField[i]] = records[0].data[temp[1]];
                }
                else {
                    pobj.data[theField[i]] = records[0].data[theField[i]];
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {
            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                    case Ext.EventObject.LEFT:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                    case Ext.EventObject.RIGHT:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });

        if (!me.readOnly) {
            me.on('blur', function () {

                selectedRecords.length = 0; //清空数组


                var value = me.getValue();
                if (value == "" || value == 'null')
                    return;
                value = encodeURI(value);
                Ext.Ajax.request({
                    url: C_ROOT + 'SUP/RichHelp/ValidateData?helpid=' + me.helpid + '&inputValue=' + value + '&selectMode=' + me.selectMode,
                    params: { 'clientSqlFilter': this.clientSqlFilter, 'helptype': 'ngMSHelp' },
                    async: false, //同步请求
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                            if (resp.Data == false) {
                                me.setValue('');
                            }
                        }
                        else {
                            Ext.MessageBox.alert('取数失败', resp.status);
                        }
                    }
                });
            });
        }

    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;        

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';
            
            if (!allfield) return;

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头 

            if (me.showAutoHeader) {
                for (var i = 0; i < heads.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                }
            }

            modelFields = new Array();
            for (var i = 0; i < fields.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                modelFields.push({
                    name: temp, //fields[i],
                    type: 'string',
                    mapping: temp//fields[i]
                });

            }
            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    //title = resp.data.Title;
                    allfield = resp.data.AllField;
                    headText = resp.data.HeadText;

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    },
    onTriggerClick: function () {
        var me = this;
        //if (me.isShowing) return;

        me.fireEvent('beforetriggerclick');

        me.isShowing = true;
        if (me.readOnly || arguments.length == 3) {
            me.isShowing = false;
            return; //arguments.length == 3，输入框上点击     
        }

        if (Ext.isEmpty(me.helpid)) {
            me.isShowing = false;
            return;
        }

        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var model;
        var columns;
        var detailModel;
        var detailColumns;

        var allDetailFields;
        var detailHeadText;
        var masterTableKey;

        var existQueryProperty = false;
        var queryPropertyItems;

        ShowHelp = function () {

           if (!allfield) return;
           if (!allDetailFields) return;

            //var queryItems;
            var modelFields;
            var gridColumns;

            var detailModelFields;
            var detailGridColumns;
            
                //var fields = allfield.split(','); //所有字段
                //var heads = headText.split(','); //列头
              
                //queryItems = new Array();
                //for (var i = 0; i < heads.length; i++) {
                //    var tempfield = fields[i].split('.');
                //    var temp = fields[i];
                //    queryItems.push({
                //        xtype: 'textfield',
                //        fieldLabel: heads[i],
                //        name: temp                  
                //    });
                //}

                modelFields = model;//new Array();

                //for (var i = 0; i < fields.length; i++) {
                //    var tempfield = fields[i].split('.');
                //    var temp;
                //    if (tempfield.length > 1) {
                //        temp = tempfield[1]; //去掉表名
                //    }
                //    else {
                //        temp = fields[i];
                //    }
                //    modelFields.push({
                //        name: temp,//fields[i], //不去掉表名
                //        type: 'string',
                //        mapping: temp
                //    });
                //}

                for (var i = 0; i < columns.length; i++) {
                    var renderer = columns[i].renderer;
                    if (renderer) {
                        columns[i].renderer = Ext.decode(renderer);
                    }
                }
                gridColumns = columns;//new Array();

                //for (var i = 0; i < heads.length; i++) {
                //    var tempfield = fields[i].split('.');
                //    var temp;
                //    if (tempfield.length > 1) {
                //        temp = tempfield[1]; //去掉表名
                //    }
                //    else {
                //        temp = fields[i];
                //    }
                //    gridColumns.push({
                //        header: heads[i],
                //        flex: 1,
                //        //width:200,
                //        //sortable: true,
                //        dataIndex: temp//fields[i]
                //    });
                //}
          
                //var detailFields = allDetailFields.split(','); //所有字段
                //var detailHeads = detailHeadText.split(','); //列头

                detailModelFields = detailModel; //new Array();

                //for (var i = 0; i < detailFields.length; i++) {
                //    var tempfield = detailFields[i].split('.');
                //    var temp;
                //    if (tempfield.length > 1) {
                //        temp = tempfield[1]; //去掉表名
                //    }
                //    else {
                //        temp = detailFields[i];
                //    }
                //    detailModelFields.push({
                //        name: temp,//detailFields[i], //不去掉表名
                //        type: 'string',
                //        mapping: temp
                //    });
                //}

                for (var i = 0; i < detailColumns.length; i++) {
                    var renderer = detailColumns[i].renderer;
                    if (renderer) {
                        detailColumns[i].renderer = Ext.decode(renderer);
                    }
                }
                detailGridColumns = detailColumns; //new Array();

                //for (var i = 0; i < detailHeads.length; i++) {
                //    var tempfield = detailFields[i].split('.');
                //    var temp;
                //    if (tempfield.length > 1) {
                //        temp = tempfield[1]; //去掉表名
                //    }
                //    else {
                //        temp = detailFields[i];
                //    }
                //    detailGridColumns.push({
                //        header: detailHeads[i],
                //        flex: 1,                       
                //        dataIndex: temp//detailFields[i]
                //    });
                //}

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                weight: 20,
                height: 36,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    xtype: 'textfield',
								    id: "searchkey",
								    width: 200
								},
								{
								    id: 'richhelp_query',
								    iconCls: 'icon-View'
								},
                                {
                                    id: 'richhelp_refresh',
                                    iconCls: 'icon-Refresh'
                                }, '->',
							     {
							         xtype: 'checkboxgroup',
							         name: 'hobby',
							         items: [
                                        { boxLabel: '在结果中搜索', width: 100, id: 'ch_ms_searchInResult', inputValue: '01' },
                                        {
                                            boxLabel: '树记忆', width: 60, id: 'ch_ms_treerem', hidden: true, inputValue: '02', handler: function (chk) {
                                                me.saveTreeMemory(leftTree, chk.getValue());
                                                var k = 0;
                                            }
                                        }
							         ]
							     }
                ]
            });

            var searcheArr = [];
            var searchIndex = {}; //索引
            Ext.getCmp('ch_ms_searchInResult').on('change', function (me, nvalue, ovalue, eOpts) {

                if (false == nvalue) {
                    searcheArr.length = 0; //清空条件列表
                    searchIndex = {}; //清空索引
                }

            });

            Ext.getCmp('richhelp_query').on('click', function () {
                var searchkey;
                if (Ext.getCmp('ch_ms_searchInResult').getValue()) {
                    var key = Ext.getCmp('searchkey').getValue();

                    if (!searchIndex[key]) {
                        searcheArr.push(Ext.getCmp('searchkey').getValue());
                        searchIndex[key] = key;
                    }

                    searchkey = searcheArr;
                }
                else {
                    searcheArr.length = 0;
                    searcheArr.push(Ext.getCmp('searchkey').getValue());
                }

                Ext.apply(store.proxy.extraParams, { 'searchkey': searcheArr });
                store.load();

            });

            Ext.getCmp('richhelp_refresh').on('click', function () {
                Ext.getCmp('searchkey').setValue('');

                if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                    delete store.proxy.extraParams.searchkey;
                    delete store.proxy.extraParams.treesearchkey;
                    delete store.proxy.extraParams.treerefkey;
                    store.load();
                }
            });

            var propertyCode = queryPropertyItems[me.selectQueryProIndex].code;
            var propertyID = queryPropertyItems[me.selectQueryProIndex].inputValue;
            queryPropertyItems[me.selectQueryProIndex].checked = true;

            var queryProperty = Ext.create('Ext.container.Container', {
                region: 'north',
                //frame: true,
                weight: 20,
                border: false,
                //layout: 'auto', //支持自适应 	              
                items: [{
                    xtype: 'fieldset', //'fieldcontainer',
                    title: '查询属性', //fieldLabel: 'Size',
                    defaultType: 'radiofield',
                    defaults: {
                        flex: 1
                    },
                    layout: 'column',
                    fieldDefaults: {
                        margin: '0 10 0 0'
                    },
                    items: [{
                        id: 'radioQueryPro',
                        xtype: 'radiogroup',
                        layout: 'column',
                        fieldDefaults: {
                            margin: '0 10 3 0'
                        },
                        activeItem: 0,
                        items: queryPropertyItems,
                        listeners: {
                            'change': function (radioCtl, nvalue, ovalue) {

                                var select = radioCtl.getChecked();
                                if (select.length > 0) {

                                    leftPanel.setTitle(select[0].boxLabel);
                                    var code = select[0].code; //加载树的搜索id
                                    propertyCode = code;
                                    propertyID = select[0].inputValue;

                                    Ext.Ajax.request({
                                        //params: { 'id': busid },
                                        url: C_ROOT + 'SUP/RichHelp/GetListExtendInfo?code=' + propertyCode,
                                        //callback: ShowHelp,
                                        success: function (response) {
                                            var resp = Ext.JSON.decode(response.responseText);
                                            var extFields = resp.extfields; //扩展字段
                                            var extHeader = resp.extheader; //扩展列头

                                            var fields = Ext.clone(modelFields);
                                            var columns = Ext.clone(gridColumns);

                                            if (extHeader && extHeader != '') {
                                                var tempfs = extFields.split(',');
                                                var cols = extHeader.split(',');
                                                for (var i = 0; i < tempfs.length; i++) {
                                                    fields.push({
                                                        name: tempfs[i],
                                                        type: 'string',
                                                        mapping: tempfs[i]
                                                    });

                                                    columns.push({
                                                        header: cols[i],
                                                        flex: 1,
                                                        dataIndex: tempfs[i]
                                                    });
                                                }
                                            }

                                            //使用外部的store
                                            store = Ext.create('Ext.ng.JsonStore', {
                                                fields: fields,
                                                pageSize: 20,
                                                autoLoad: true,
                                                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                                                listeners: {
                                                    'beforeload': function () {
                                                        var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                                                        Ext.apply(store.proxy.extraParams, data);
                                                        if (me.likeFilter) {
                                                            Ext.apply(data, me.likeFilter);
                                                        }
                                                        if (me.outFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                                                        }
                                                        if (me.leftLikeFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                                                        }
                                                        if (me.clientSqlFilter) {
                                                            Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                                                        }
                                                    } //beforeload function
                                                }//listeners
                                            });
                                            //重新配置grid
                                            grid.reconfigure(store, columns);
                                            pagingbar.bind(store);
                                        }
                                    });
                                }

                                if (nvalue.property === 'all') {

                                    Ext.getCmp('ch_ms_treerem').hide();

                                    leftPanel.setVisible(false);
                                    //leftTree.setVisible(false);
                                    if (store.proxy.extraParams.searchkey || store.proxy.extraParams.treesearchkey || store.proxy.extraParams.treerefkey) {
                                        delete store.proxy.extraParams.searchkey;
                                        delete store.proxy.extraParams.treesearchkey;
                                        delete store.proxy.extraParams.treerefkey;
                                        store.load();
                                    }
                                    return;
                                } else {

                                    me.initParam();
                                    Ext.getCmp('ch_ms_treerem').setVisible(true);

                                    var rootNode = leftTree.getRootNode();
                                    if (leftTree.isFirstLoad) {
                                        rootNode.expand(); //expand会自动调用load
                                        leftTree.isFirstLoad = false;
                                    }
                                    else {
                                        leftTree.getStore().load();
                                    }
                                    leftPanel.setVisible(true);
                                } //else

                            } //function
                        }//listeners
                    }]
                }]
            });
            
            var leftTree = Ext.create('Ext.ng.TreePanel', {
                //title: queryPropertyItems[0].boxLabel,
                autoLoad: false,
                //collapsible: true,
                split: true,
                //hidden: true,
                width: 180,
                region: 'west',
                isFirstLoad: true,
                treeFields: [{ name: 'text', type: 'string' },
                   { name: 'treesearchkey', string: 'string' },
                   { name: 'treerefkey', type: 'string' }//我的自定义属性                
                ],
                url: C_ROOT + "SUP/RichHelp/GetQueryProTree",
                listeners: {
                    selectionchange: function (m, selected, eOpts) {
                        me.memory.eOpts = "selectionchange";

                        //刷列表数据
                        var record = selected[0];
                        if (record) {
                            if (!Ext.isEmpty(record.data.treesearchkey) && !Ext.isEmpty(record.data.treerefkey)) {
                                Ext.apply(store.proxy.extraParams, { 'treesearchkey': record.data.treesearchkey, 'treerefkey': record.data.treerefkey });
                                store.load();
                            }
                            //设置选中
                            Ext.getCmp('ch_ms_treerem').setValue(me.memory.IsMemo && me.memory.FoucedNodeValue == selected[0].getPath());
                            me.memory.eOpts = "";
                        }
                    },
                    viewready: function (m, eOpts) {

                        if (me.memory) {

                            if (!Ext.isEmpty(me.memory.FoucedNodeValue)) {
                                leftTree.selectPath(me.memory.FoucedNodeValue, null, null, function () {
                                    if (Ext.isIE) {
                                        window.setTimeout(function () {
                                            var selectNode = m.view.body.query("tr." + m.view.selectedItemCls);
                                            if (selectNode) {
                                                selectNode[0].scrollIntoView(true);
                                            }
                                        }, 500);
                                    }
                                });
                            }
                            else {
                                store.load();
                            }
                        }
                    }
                }
            });

            leftTree.getStore().on('beforeload', function (store, operation, eOpts) {
                operation.params.code = propertyCode; //树添加参数	                
            });

            var leftPanel = Ext.create('Ext.panel.Panel', {
                title: "人力资源树",
                autoScroll: false,
                collapsible: true,
                split: true,
                hidden: true,
                region: 'west',
                weight: 10,
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
                        emptyText: '输入关键字，定位树节点',
                        margin: '2 0 2 2',
                        enableKeyEvents: true,
                        listeners: {
                            'keydown': function (el, e, eOpts) {
                                if (e.getKey() == e.ENTER) {
                                    me.findNodeByFuzzy(leftTree, el.getValue());
                                    el.focus();
                                    return false;
                                }
                                else {
                                    me.nodeIndex = -1;
                                }
                            }
                        }
                    }, {
                        region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5',
                        handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(leftTree, el.getValue()); el.focus(); }
                    }]
                }, leftTree]
            });

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                fields: modelFields,
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store
            });

            //var selModel = Ext.create('Ext.selection.CheckboxModel');

            var grid = Ext.create('Ext.ng.GridPanel', {
                region: 'north',
                height: 150,
                //frame: false,
                //border: false,
                store: store,                            
                columnLines: true,
                columns: gridColumns,
                bbar: pagingbar
            });
         
            var detailStore = Ext.create('Ext.ng.JsonStore', {
                fields: detailModelFields,
                pageSize: 20,
                autoLoad: false,
                url: C_ROOT + 'SUP/RichHelp/GetDetailList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
            });

            var detailGrid = Ext.create('Ext.ng.GridPanel', {
                region: 'center',
                //frame: false,
                //border: false,
                store: detailStore,
                selModel: { mode: "SIMPLE" }, //多选                            
                columnLines: true,
                columns: detailGridColumns                
            });

            var centerPanel = Ext.create('Ext.panel.Panel', {
                id: 'weipanel',
                autoScroll: false,               
                split: true,               
                region: 'center',            
                minSize: 180,
                maxSize: 180,
                border: true,
                layout: 'border',
                items: [grid, detailGrid]
            });

            grid.on('itemclick', function () {

                var data = grid.getSelectionModel().getSelection();
                if (data.length > 0) {
                    var masterCode = data[0].get(masterTableKey);
                    Ext.apply(detailStore.proxy.extraParams, { 'masterCode': masterCode });
                    detailStore.load();
                }

            });

            detailGrid.on('itemdblclick', function () {
                me.gridDbClick(me, detailGrid, win);
            });

            var winItems = [];
            if (existQueryProperty) {
                winItems.push(toolbar);
                winItems.push(queryProperty);
                winItems.push(leftPanel);
                winItems.push(centerPanel);
            }
            else {
                winItems.push(toolbar);
                winItems.push(centerPanel);
            }

            var buttons = [];                      
            buttons.push('->');
            buttons.push({ text: '确定', handler: function () { me.btnOk(me, detailGrid, win); } });
            buttons.push({ text: '取消', handler: function () { win.close(); } });


            //显示弹出窗口
            var win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                modal: true,
                //constrain: true,
                constrainHeader: true,
                items: winItems, //[toolbar, queryProperty, tabPanel],
                buttons: buttons
            });
            win.show();

            //触发选择改变事件，加载左边树            
            if (me.selectQueryProIndex != 0) {
                var radioGroup = Ext.getCmp('radioQueryPro');
                radioGroup.fireEvent('change', radioGroup);
            }

            me.isShowing = false;
            //store.load();//手工调不会触发beforeload事件

            store.on('beforeload', function () {
                var data = { 'propertyID': propertyID, 'propertyCode': propertyCode };
                Ext.apply(store.proxy.extraParams, data);
                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
                //return true;
            })
        };

        var url = C_ROOT + 'SUP/RichHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;

        Ext.Ajax.request({
            //params: { 'id': busid },
            url: url,
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    title = resp.data.Title;
                    allfield = resp.data.AllField;
                    headText = resp.data.HeadText;
                    model = resp.data.model;
                    columns = resp.data.columns;
                    detailModel = resp.data.detailModel;
                    detailColumns = resp.data.detailColumns

                    allDetailFields = resp.data.detailTableFields;
                    detailHeadText = resp.data.detailHeadText;
                    masterTableKey = resp.data.masterTableKey;

                    existQueryProperty = resp.data.existQueryProp;
                    queryPropertyItems = Ext.JSON.decode(resp.data.queryProperty);
                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },
    showHelp: function () {
        this.onTriggerClick();
    },
    bindData: function () {
        var me = this;
        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);
        return;
    }, //bindData
    btnOk: function (help, grid, win) {
               
        var code;
        var name;
        var pobj = new Object();
        var values = new Array();
        var names = new Array();
               
        var data = grid.getSelectionModel().getSelection();
        for (var i = 0; i < data.length; i++) {
                code = data[i].get(help.valueField);
                name = data[i].get(help.displayField);

                if (!code) {
                    var obj = data[0].data;
                    //容错处理，带表名获取不到值
                    for (var p in obj) {

                        var field = [];
                        if (p.indexOf('.') > 0) {
                            field = p.split('.');
                        }

                        if (field[1] === help.valueField) {
                            code = obj[p];
                        }
                        if (field[1] === help.displayField) {
                            name = obj[p];
                        }

                    }
                }

                values.push(code);
                names.push(name);
                
            }

        var code = values.join(',');
        var name = names.join(',');

        var obj = new Object();
        obj[help.valueField] = code;

        if (help.displayFormat) {
            obj[help.displayField] = Ext.String.format(help.displayFormat, code, name);
        } else {
            obj[help.displayField] = name;
        }

        Ext.define('richhelpModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: help.valueField,
                type: 'string',
                mapping: help.valueField
            }, {
                name: help.displayField,
                type: 'string',
                mapping: help.displayField
            }
            ]
        });

        //        var valuepair = Ext.ModelManager.create(obj, 'richhelpModel');
        var valuepair = Ext.create('richhelpModel', obj);
        help.setValue(valuepair); //必须这么设置才能成功
        //        help.setHiddenValue(code);
        //        help.setRawValue(name);

        win.hide();
        win.close();
        win.destroy();

        pobj.code = code;
        pobj.name = name;
        pobj.data = data;
        pobj.type = 'fromhelp';
        help.fireEvent('helpselected', pobj);

    },   
    gridDbClick: function (help, grid, win) {
        var data = grid.getSelectionModel().getSelection();
        if (data.length > 0) {
            var code = data[0].get(help.valueField);
            var name = data[0].get(help.displayField);

            if (!code) {
                var obj = data[0].data;
                //容错处理，model的字段有可能带表名获取不到值
                for (var p in obj) {

                    var field = [];
                    if (p.indexOf('.') > 0) {
                        field = p.split('.');
                    }

                    if (field[1] === help.valueField) {
                        code = obj[p];
                    }
                    if (field[1] === help.displayField) {
                        name = obj[p];
                    }

                }
            }

            var obj = new Object();
            obj[help.valueField] = code;

            if (help.displayFormat) {
                obj[help.displayField] = Ext.String.format(help.displayFormat, code, name);
            } else {
                obj[help.displayField] = name;
            }

            Ext.define('richhelpModel', {
                extend: 'Ext.data.Model',
                fields: [{
                    name: help.valueField,
                    type: 'string',
                    mapping: help.valueField
                }, {
                    name: help.displayField,
                    type: 'string',
                    mapping: help.displayField
                }
                ]
            });

            //            var valuepair = Ext.ModelManager.create(obj, 'richhelpModel');

            var valuepair = Ext.create('richhelpModel', obj);
            help.setValue(valuepair); //必须这么设置才能成功
            //            help.setHiddenValue(code);
            //            help.setRawValue(name);
            win.hide();
            win.close();
            win.destroy();
            //if (me.isInGrid) {

            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'fromhelp';
            pobj.data = data[0].data;
            help.fireEvent('helpselected', pobj);
            //}

        }
    },
    initParam: function () {
        var me = this;
        me.memory = {};
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/RichHelp/GetTreeMemoryInfo',
            async: false,
            params: { type: me.helpType, busstype: me.bussType },
            success: function (response, opts) {
                me.memory = Ext.JSON.decode(response.responseText);
            }
        });
    },
    saveTreeMemory: function (tree, checked) {
        var me = this;
        if (me.memory.eOpts == "selectionchange") { return; }
        var sd = tree.getSelectionModel().getSelection();
        if (sd.length > 0) {
            me.memory.FoucedNodeValue = sd[0].getPath();
            me.memory.IsMemo = checked;
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/RichHelp/UpdataTreeMemory',
                async: true,
                params: { type: me.helpType, busstype: me.bussType, foucednodevalue: me.memory.FoucedNodeValue, ismemo: checked },
                success: function (response, opts) {
                }
            });
        }
    },  
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = this, index = -1, firstFind = me.nodeIndex == -1;
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            //if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
            }
            me.nodeIndex = -1;
        }
    },    
    getCodeName: function (value) {
        var me = this;
        var name;

        Ext.Ajax.request({
            url: C_ROOT + 'SUP/RichHelp/GetName?helptype=Single&helpid=' + me.helpid + '&code=' + value,
            async: false, //同步请求
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    name = resp.name; //显示值                    
                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                    name = 'error';
                }
            }
        });

        return name;
    },
    setOutFilter: function (obj) {
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        this.clientSqlFilter = str;
    },
    getFirstRowData: function () {
        var me = this;
        var fields = me.listFields.split(',');

        var modelFields = new Array();
        for (var i = 0; i < fields.length; i++) {

            var tempfield = fields[i].split('.');
            var temp;
            if (tempfield.length > 1) {
                temp = tempfield[1]; //去掉表名
            }
            else {
                temp = fields[i];
            }

            modelFields.push({
                name: fields[i],
                type: 'string',
                mapping: temp
            });
        }

        Ext.define('model', {
            extend: 'Ext.data.Model',
            fields: modelFields
        });

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'model',
            pageSize: 20,
            autoLoad: false,
            url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode
        });

        store.on('beforeload', function () {

            //            var data = new Object();
            //            data[me.valueField] = value;

            if (me.outFilter) {
                //Ext.apply(me.outFilter, data);
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
            }
            else {
                Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(data) });
            }

        })

        store.load(function () {
            var data = store.data.items[0].data;
            me.fireEvent('firstrowloaded', data);
        });

    }
});

//下拉帮助
Ext.define('Ext.ng.ComboBox', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngComboBox'],
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    typeAhead: true,
    typeAheadDelay: 500,
    triggerAction: "all",    
    ORMMode: true,
    selectMode: 'Single',//多选:Multi
    needBlankLine: false,//是否需要空行
    showHeader: false,//显示列头
    codeIsNum: true,//代码列是数值类型
    initComponent: function () {
        var me = this;

        if (me.selectMode == 'Multi') {
            me.multiSelect = true;
        }
            

        //设置默认值
        me.valueField = me.valueField ? me.valueField : 'code';
        me.displayField = (me.displayField && me.displayField != 'text') ? me.displayField : 'name';

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';

        var store;

        if (me.queryMode === 'remote') {

            if (!(Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField))) {
                if (me.listFields) {
                    var listheaders = '';
                    var listfields = '';

                    var heads = [];
                    if (me.listHeadTexts) {
                        heads = me.listHeadTexts.split(','); //列头 
                    }

                    var fields = me.listFields.split(','); //所有字段              

                    var modelFields = new Array();
                    for (var i = 0; i < fields.length; i++) {

                        var tempfield = fields[i].split('.');
                        var temp;
                        if (tempfield.length > 1) {
                            temp = tempfield[1]; //去掉表名
                        }
                        else {
                            temp = fields[i];
                        }


                        //值字段，按照数据类型设置,保证setValue设置成功
                        if (temp == me.valueField) {
                            var dataType = me.valueType ? me.valueType : 'string';
                            modelFields.push({
                                name: temp, //fields[i],
                                type: dataType,
                                mapping: temp //fields[i]
                            });
                        }
                        else {//其他字段都用string
                            modelFields.push({
                                name: temp, //fields[i],
                                type: 'string',
                                mapping: temp //fields[i]
                            });
                        }

                    }

                    if (me.showHeader) {
                        for (var i = 0; i < heads.length; i++) {
                            listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                        }
                    }

                    for (var i = 0; i < heads.length; i++) {

                        var tempfield = fields[i].split('.');
                        var temp;
                        if (tempfield.length > 1) {
                            temp = tempfield[1]; //DataTable取数模式去掉表名
                        }
                        else {
                            temp = fields[i];
                        }

                        listfields += '<td>{' + temp + '}</td>';
                    }

                    var temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                    me.tpl = temp;
                }
                else {
                    //me.initialListTemplate();

                    var tempfield = me.valueField.split('.');//系统编码
                    var valueField;
                    if (tempfield.length > 1) {
                        valueField = tempfield[1]; //去掉表名
                    }
                    else {
                        valueField = me.valueField;
                    }

                    if (!me.userCodeField) {
                        me.userCodeField = me.valueField;//容错处理
                    }
                    var uField = me.userCodeField.split('.');//用户编码
                    var userCodeField;
                    if (uField.length > 1) {
                        userCodeField = uField[1];
                    } else {
                        userCodeField = me.userCodeField;
                    }

                    var dfield = me.displayField.split('.');
                    var displayField;
                    if (dfield.length > 1) {
                        displayField = dfield[1]; //去掉表名
                    }
                    else {
                        displayField = me.displayField;
                    }

                    var modelFields = [{
                        name: valueField,
                        type: 'string',
                        mapping: valueField
                    }, {
                        name: userCodeField,
                        type: 'string',
                        mapping: userCodeField
                    }, {
                        name: displayField,
                        type: 'string',
                        mapping: displayField
                    }]

                    var listfields = '<td>{' + userCodeField + '}</td>';//显示用户代码
                    listfields += '<td>{' + displayField + '}</td>';
                    me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }

                store = Ext.create('Ext.data.Store', {
                    pageSize: 50,
                    fields: modelFields,
                    proxy: {
                        type: 'ajax',
                        url: C_ROOT + 'SUP/RichHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                        reader: {
                            type: 'json',
                            root: 'Record',
                            totalProperty: 'totalRows'
                        }
                    }
                });

                me.bindStore(store);
            }
        }
        else {

            var dataType = me.valueType ? me.valueType : 'string';

          
            store = Ext.create('Ext.data.Store', {
                pageSize: 50,
                fields: [
				{ name: me.valueField, type: dataType },
				{ name: me.displayField, type: 'string' }
                ],
                data: me.data
            });

            if (me.needBlankLine) {            
                var emptydata = {};
                if (me.valueType == 'int') {
                    emptydata[me.valueField] = 0;
                }
                else {
                    emptydata[me.valueField] = '';
                }
                emptydata[me.displayField] = '&nbsp;'; //空html标记 下拉才能显示空行
                var rs = [emptydata];
                store.insert(0, rs);
            }

            me.bindStore(store);

        }

        me.callParent(); //store加载成功后调用，保证setValue没问题
        this.mixins.base.initComponent.call(me);

        if (store && me.queryMode === 'remote') {
            //处理外部条件
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }

                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }
            });
            
            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    var emptydata = new Object();

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    if (me.valueType == 'int') {
                        emptydata[myValueFiled] = 0;
                    }
                    else {
                        emptydata[myValueFiled] = '';
                    }
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记 下拉才能显示空行

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }
        }

        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('beforetriggerclick');//下拉之前事件

        me.on('select', function (combo, records, eOpts) {

            var theField;

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }

            var dataType = me.valueType ? me.valueType : 'string';
            //构建
            if (me.listFields) {
                theField = [];
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {                                       
                    if (temp[i] == me.valueField) {
                        theField.push({
                            name: temp[i],
                            type: dataType,
                            mapping: temp[i]
                        });

                    } else {
                        theField.push({
                            name: temp[i],
                            type: 'string',
                            mapping: temp[i]
                        });
                    }                                       
                }
            }
            else {               
                theField = [{
                    name: myValueFiled, 
                    type: dataType,
                    mapping: myValueFiled 
                }, {
                    name: myDisplayField, 
                    type: 'string',
                    mapping: myDisplayField
                }]                
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: theField
            });
                      

            var code = combo.getValue() || records[0].data[myValueFiled];
            var name = combo.getRawValue() || records[0].data[myDisplayField];

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {              
                    obj[me.valueField] = code;
                                
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                if (me.valueType == 'int' && code == 0) {
                    obj[me.displayField] = '';//当类型是int的时候，控件显示&nbsp;类型为string能正常显示空
                }
                else {                    
                    obj[me.displayField] = name;
                }
            }

            if (!Ext.isEmpty(name)) {
                var valuepair = Ext.ModelManager.create(obj, 'themodel');
                //combox local不需要setValue
                if (me.queryMode === 'remote') {
                    me.setValue(valuepair); //必须这么设置才能成功
                }
            }
            else {
                me.clearValue();//选择空行，清空
            }
           

            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].name.split('.');
                if (temp.length > 1) {
                    pobj.data[theField[i]] = records[0].data[temp[1]];
                }
                else {
                    pobj.data[theField[i].name] = records[0].data[theField[i].name];
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        //0显示成空
        me.on('change', function (combo, newVal, oldVal) {
            if (me.ignoreZero) {
                if (newVal == 0) {
                    me.setValue('');
                }
            }
        });

        //处理清空问题
        me.on('afterrender', function () {
            var input = this.el.down('input');
            if (input) {                
                //input.dom.onchange = function () {
                //    var newVal = me.inputEl.dom.value
                //};

                //input值被清空，把value也清空
                me.inputEl.on('change', function (e, t, eOpts) {
                    //alert(me.inputEl.dom.value);
                    if (Ext.isEmpty(me.inputEl.dom.value)) {
                        //me.clearValue();//会得到null值
                        me.setValue('');
                    }
                });
            }
        });

    },
    initialListTemplate: function (store) {
        var me = this;

        if (Ext.isEmpty(me.helpid)) return;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (me.helpType === 'rich') {//用户自定义界面的模板 

                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;

                for (var i = 0; i < gridColumns.length; i++) {
                    listheaders += '<th class="x-column-header-inner x-column-header-over">' + gridColumns[i].header + '</th>';
                }

                for (var i = 0; i < modelFields.length; i++) {
                    listfields += '<td>{' + modelFields[i]['name'] + '}</td>';
                }
            }
            else {

                var heads = headText.split(','); //列头 
                var fields = allfield.split(','); //所有字段              

                if (me.showHeader) {
                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {
                    modelFields.push({
                        name: fields[i],
                        type: 'string',
                        mapping: fields[i]
                    });

                    listfields += '<td>{' + fields[i] + '}</td>';
                }
            }

           
            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }


            })


            if (me.showHeader) {
                var temp = '<div><table width="100%" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                me.tpl = temp;
            }
        };
              

        var url;
        if (me.helpType === 'rich') {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpTemplate?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }
        else {
            url = C_ROOT + 'SUP/CommonHelp/GetHelpInfo?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode;
        }

        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });

    },   
    onTriggerClick:function () {
            var me = this;
            me.fireEvent('beforetriggerclick');
            me.callParent();           
        },
    bindData: function () {
        var me = this;

        BindCombox(me, me.valueField, me.displayField, me.helpid, me.getValue(), me.selectMode);
        return;

    },
    setOutFilter: function (obj) {
        Ext.apply(this.store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(obj) });
        this.outFilter = obj;
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        Ext.apply(this.store.proxy.extraParams, { 'clientSqlFilter': str });
        this.clientSqlFilter = str;
    },
    getValue: function () {
        var me = this;
        if (me.multiSelect && Ext.isArray(me.value)) {

            return me.value.join(','); //多选，数组转字符串
        }
        else {
            return me.value;
        }
    }
});

//树形下拉帮助
Ext.define('Ext.ng.TreeComboBox', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.ngTreeComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    store: new Ext.data.ArrayStore({ fields: [], data: [[]] }),
    editable: false,
    //resizable: true,
    minWidth: 100,
    selectMode: 'Single',
    //maxWidth: 350,
    labelAlign: 'right',
    readOnly: false,
    minChars: 1, //输入一个就弹出下拉
    //typeAhead: true,
    triggerAction: 'all',
    //matchFieldWidth: false,
    initComponent: function () {

        var me = this;
        this.callParent(arguments);
        this.mixins.base.initComponent.call(me);
        this.treeRenderId = Ext.id();

        var height = me.treePanel.height || 300;
        if (me.treeMinHeight) {
            height = (me.treePanel.height < me.treeMinHeight) ? me.treeMinHeight : me.treePanel.height;
        }
        var width = me.pickerWidth || 300;
        if (me.treeMinWidth) {
            width = (me.treePanel.width < me.treeMinWidth) ? me.treeMinWidth : me.treePanel.width;
        }
        //必须要用这个定义tpl
        this.tpl = new Ext.XTemplate('<tpl for="."><div style="height:' + height + 'px;width:' + width + '"><div style="height:' + height + 'px;" id="' + this.treeRenderId + '"></div></div></tpl>');
        //this.tpl = new Ext.XTemplate('<div style="height:' + me.treePanel.height + 'px;" id="' + this.treeRenderId + '"></div>');
        //this.tpl = new Ext.XTemplate('<div style="height:150px;" id="' + this.treeRenderId + '"></div>');

        Ext.define('treemodel', {
            extend: 'Ext.data.Model',
            fields: [
							{
							    name: me.valueField,
							    type: 'string',
							    mapping: me.valueField
							}, {
							    name: me.displayField,
							    type: 'string',
							    mapping: me.displayField
							}]
        });

        me.treePanel.border = false, //去掉边框
		me.treePanel.width = width;
        if (!me.matchFieldWidth) {
            me.getPicker().setWidth(width);
        }

        var treeObj = me.treePanel;

        treeObj.on('itemclick', function (view, rec) {
            if (rec) {


                var isleaf = rec.get('leaf');
                if (me.effectiveNodeType === 'leaf' && (isleaf == false)) {
                    return; //不是叶子节点
                }

                var code = rec.get(me.treeValueField); //rec.get('id');
                var name = rec.get('text');

                var obj = new Object();
                obj[me.valueField] = code;
                obj[me.displayField] = name;

                var valuepair = Ext.ModelManager.create(obj, 'treemodel');
                me.setValue(valuepair); //必须这么设置才能成功

                me.collapse();

            }
        });

        this.on({
            'expand': function () {


            //			var tempdiv = Ext.query('.x-panel-body');       
            //			for (var i = 0; i < tempdiv.length; i++) {
            //				if (Ext.isEmpty(tempdiv[i].innerHTML)) {
            //					tempdiv[i].remove();
            //				}
            //			}

            if (!treeObj.rendered && treeObj && !this.readOnly) {
                //if (treeObj && !this.readOnly) {
                Ext.defer(function () {
                    treeObj.render(this.treeRenderId);
                }, 100, this);
            }

        }
        });
    },
    doQuery: function () {
        var me = this;
        me.expand();
    }
});

Ext.define('Ext.ng.AutoComplete', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: 'widget.ngAutoComplete',
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    typeAhead: true,
    typeAheadDelay: 500,
    triggerAction: "all",
    hideTrigger: true,
    initComponent: function () {
        var me = this;
        me.callParent();
        this.mixins.base.initComponent.call(me);

        var rootPath = '../';
        if (me.rootPath) {
            rootPath = me.rootPath;
        }
        store = Ext.create('Ext.data.Store', {
            pageSize: 50,
            fields: [{ name: me.valueField, type: 'string' },
				{ name: me.displayField, type: 'string' }],
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid,
                reader: {
                    type: 'json',
                    root: 'Record',
                    totalProperty: 'totalRows'
                }
            }
        });

        me.bindStore(store);
    }
});

//Text
Ext.define('Ext.ng.Text', {
    extend: 'Ext.form.field.Text',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngText', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

        me.on('blur', function (textCtl) {
            if (Ext.isEmpty(textCtl.getValue().trim())) {
                textCtl.setValue('');//处理空格
            }
        });
    },
    validateValue: function (value) {
        var me = this;
        var realLen = $GetLength(value);//实际字符数
        if (realLen > me.maxLength) {
            me.setActiveError('输入字符超过最大长度(中文算两个字符):' + me.maxLength);
            me.setValue(value.substring(0, me.maxLength));
            return false;
        }
        return me.callParent(arguments);
    }
});

Ext.define('Ext.ng.TextArea', {
    extend: 'Ext.form.field.TextArea',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngTextArea', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

        //me.on('afterrender', function (panel, eOpts) {
        //    //样式
        //    var input = this.el.down('textarea');
        //    if (input && input.dom.type === 'textarea') {
        //        input.dom.setAttribute('class', 'x-form-field ng-textarea x-form-textarea');
        //    }
        //});

        //me.on('blur', function () {  
        //    if (!me.readOnly) {
        //        var input = this.el.down('textarea');
        //        if (input && input.dom.type === 'textarea') {
        //            //input.dom.setAttribute('class', 'x-form-field ngx-form-textarea x-form-textarea');
        //            input.dom.setAttribute('class', 'x-form-field ng-textarea x-form-textarea');
        //        }
        //    }
        //});

        me.on('focus', function () {
            me.textChange = false;

            //if (!me.readOnly) {
            //    //设置样式
            //    var input = this.el.down('textarea');
            //    if (input && input.dom.type === 'textarea') {
            //        input.dom.setAttribute('class', 'ng-textarea-focus');
            //    }
            //}
        });

        me.on('blur', function (textCtl) {
            if (Ext.isEmpty(textCtl.getValue().trim())) {
                textCtl.setValue('');//处理空格
            }
        });
        
    },
    validateValue: function (value) {
        var me = this;
        var realLen = $GetLength(value);//实际字符数
        if (realLen > me.maxLength) {
            me.setActiveError('输入字符超过最大长度(中文算两个字符):' + me.maxLength);
            me.setValue(value.substring(0, me.maxLength));
            return false;
        }
        return me.callParent(arguments);
    }
});

Ext.define('Ext.ng.Number', {
    extend: 'Ext.form.field.Number',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngNumber', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    showPercent: false, //是否显示百分比数值
    isMarginRight: true,
    initComponent: function () {
        var me = this;
        if (me.isMarginRight) {
            me.fieldStyle = { textAlign: 'right' };
        }
        this.mixins.base.initComponent.call(me);
        me.callParent();

        me.on('focus', function (number) {
            if (number.value == 0) {
                var input = this.el.down('input');
                if (input) {
                    input.dom.select();//0值，自动选中方便输入
                }
                number.inflag = true;             
                //number.setValue(null);//设置为null,change事件会崩溃                
                //number.setCursorPosition(0);
            }
        });
        
        me.on('blur', function (number) {
            if (!number.value) {
                number.inflag = false;
                number.setValue(0);               
            }
        });

        //定位到第一个位置
        //me.on('afterrender', function (number) {
        //    number.getEl().on('mouseover', function () {
        //        number.setCursorPosition(0);
        //    });
        //});

    },
    setValue: function (v) {
        var me = this;
        this.callParent(arguments);
      
       
        var havePercent = false;//传进来的值是否存在%号
        if (this.showPercent) {
            if (v == undefined) { //显示百分比数值时undefined转为0，不转换的话界面为%，转了为0.00%
                v = 0;
            }
            if (String(v).indexOf("%") >= 0) { //删除百分号
                v = String(v).replace("%", "");
                havePercent = true;
            }
        }
        v = typeof (v) == 'number' ? v : String(v).replace(this.decimalSeparator, ".").replace(/,/g, "");

        //---beg防止上下滚动失效
        if (v == this.minValue) {
            this.setSpinDownEnabled(false);
        } else if (!this.spinDownEnabled) {
            this.setSpinDownEnabled(true);
        }
        if (v == this.maxValue) {
            this.setSpinUpEnabled(false);
        } else if (!this.spinUpEnabled) {
            this.setSpinUpEnabled(true);
        }
        //---end防止上下滚动失效

        if (this.showPercent && !havePercent) { //显示百分比数值，传进来的值不带百分号，显示RawValue的时候要*100,如：0.01->1%
            v *= 100;
        }

        if (this.allowDecimals) {

            var zCount = Ext.String.repeat('0', this.decimalPrecision);//小数位后0的个数
            if (this.showPercent) {
                zCount = Ext.String.repeat('0', this.decimalPrecision - 2);//带%，截掉后两位
            }            
            
            //手动截取,防止输入超过了四舍五入，1.115却是1.11
            var str = v.toString();
            var indexOfDot = str.indexOf('.');
            if (indexOfDot > 0) {
                var totalLength = indexOfDot + zCount.length + 1;
                var subVal = str.substring(0,totalLength);
                var addBit = str.substring(totalLength, totalLength + 1);//检测多出来的一位需不需要四舍五入
                if (Number(addBit) > 4) {
                    v = Number(subVal) + 1 / Math.pow(10, zCount.length);
                }
                else {
                    v = Number(subVal);
                }              
            } 

            v = Ext.util.Format.number(String(v), "0,000." + zCount);//自动四舍五入，1.115却是1.11

            //if (this.decimalPrecision == 6) {
            //    if (this.showPercent) {
            //        v = Ext.util.Format.number(String(v), "0,000.0000");//带%，截掉后两位
            //    } else {
            //        v = Ext.util.Format.number(String(v), "0,000.000000");
            //    }
            //}
            //else {                 
            //    v = Ext.util.Format.number(String(v), "0,000.00");
            //}

        } else {
            v = Ext.util.Format.number(String(v), "0");
        }
        if (this.showPercent) { //显示百分比数值，需要末尾加%号
            v += "%";
        }
           
        this.setRawValue(v);         
    },
    validateValue: function (value) {
        var me = this;
        if (Ext.isEmpty(value)) {
            value = '';//传入null，下面me.callParent(arguments)会报错
        }
        if (!value) {
           return me.callParent(arguments);//必输项需要校验
        }

        if (value.length < 1) {
            return me.callParent(arguments);
        }
        var havePercent = false; //传进来的值是否存在%号
        if (this.showPercent) {
            if (String(value).indexOf("%") >= 0) { //删除百分号
                value = String(value).replace("%", "");
                havePercent = true;
            }
        }
        value = String(value).replace(this.decimalSeparator, ".").replace(/,/g, "");

        //显示百分比数值，传进来的值带百分号，显示setValue的时候要/100,如：1%->0.01
        if (this.showPercent && havePercent) {
            value = value / 100;
        }
        if (isNaN(value)) {
            this.markInvalid(Ext.String.format(this.nanText, value));
            return false;
        }

        var num = this.parseValue(value);
        if (num < this.minValue) {
            this.markInvalid(Ext.String.format(this.minText, this.minValue));
            return false;
        }
        if (num > this.maxValue) {
            this.markInvalid(Ext.String.format(this.maxText, this.maxValue));
            return false;
        }
        //数值正确，清除错误信息
        //this.clearInvalid();
        //return true;
                
        var val = me.callParent(arguments);
        return val;
    },
    parseValue: function (value) {
        var havePercent = false; //传进来的值是否存在%号
        if (this.showPercent) {  //删除百分号
            if (String(value).indexOf("%") >= 0) {
                value = String(value).replace("%", "");
                havePercent = true;
            }
        }
        value = parseFloat(String(value).replace(this.decimalSeparator, ".").replace(/,/g, ""));
        //显示百分比数值，传进来的值带百分号，显示setValue的时候要/100,如：1%->0.01
        if (this.showPercent && havePercent) {
            value = value / 100;
        }
        return isNaN(value) ? '' : value;
    },   
    getSubmitData: function () {

        data = {};
        data[this.getName()] = this.getValue();
        return data;
    },
    setCursorPosition: function (pos) {
        var input = this.el.down('input');
        if (input) {
            var tobj = input.dom;
            if (pos < 0) {
                pos = tobj.value.length;
            }

            if (tobj.setSelectionRange) {//chrome
                setTimeout(function () {
                    tobj.setSelectionRange(pos, pos);
                    //tobj.focus();
                }, 0);
            }
            else if (tobj.createTextRange) {//ie
                var rng = tobj.creatTextRange();
                rng.move('character', pos);
                rng.select();
            }

        }
    }
});


//百分比控件
Ext.define('Ext.ng.RateNumber', {
    extend: 'Ext.ng.Number',
    alias: 'widget.ngRateNumber', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    decimalPrecision: 6,
    maxText: '该输入项的最大值是100',

        step: 0.0001,
        maxValue: 1,
        minValue: 0,
        initComponent: function () {
        var me = this;
        if (me.isMarginRight) {
            me.fieldStyle = {
                textAlign: 'right'
        };
        }
        this.mixins.base.initComponent.call(me);
        me.callParent();

},
        transformRawValue: function (v) {
        if (v == "")
                return "";

            v = String(Number(v) * 100)

            var s = v.split('.');

        if (s[1]) {
            if (s[1] === 0)
                v = Ext.util.Format.number(v, "0");
            else
                    v = Ext.util.Format.number(v, "0.00");
            }
        else
            v = Ext.util.Format.number(v, "0");

        return v;
        },
                getRawValue: function () {
                    var me = this,
v = (me.inputEl ? me.inputEl.getValue() : Ext.value(me.rawValue, ''));

        v = Number(v);
        me.rawValue = v;
        return v / 100;
        }
});


Ext.define('Ext.ng.Date', {
    extend: 'Ext.form.field.Date',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngDate', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建  
    format: 'Y-m-d',
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

    },
    setValue: function (value) {
        var me = this;
        if (value) {
            if (value.length > 11) {//数据库字段设计成带时分秒的，截取掉时分秒
                value = value.substring(0, 10);
                me.setValue(value);
                return;
            }          
        }       
        me.callParent(arguments);        
    },
    getErrors: function () {
        var me = this;
        var errors = me.callParent(arguments);

        if (!(me.CompareID && me.CompareChar)) {
            return errors;
        }
        var first = me.getValue();
        var second = Ext.getCmp(me.CompareID).getValue(); //比较对象     
        if (Ext.isEmpty(first) || Ext.isEmpty(first)) {
            return errors;
        }

        var ret = "";
        if (me.CompareChar == ">") {
            if (Date.parse(first) < Date.parse(second)) {
                ret = "[" + me.fieldLabel + "] 不能小于 [" + Ext.getCmp(me.CompareID).fieldLabel + "]";
            }
        }
        else if (me.CompareChar == "<") {
            if (Date.parse(first) > Date.parse(second)) {
                ret = "[" + me.fieldLabel + "] 不能大于 [" + Ext.getCmp(me.CompareID).fieldLabel + "]";
            }
        }
        else if (me.CompareChar == "=") {
            if (Date.parse(first) != Date.parse(second)) {
                ret = me.fieldLabel + " 不等于 " + Ext.getCmp(me.CompareID).fieldLabel;
            }
        }
        else {
            ret = "CompareChar属性值[" + me.CompareChar + "]不合法，值必须是>、<或者=";
        }

        //me.activeErrors = ret;
        if (!Ext.isEmpty(ret)) {
            errors.push(ret);
        }
        return errors;

    }

});

Ext.define('Ext.ng.Time', {
    extend: 'Ext.form.field.Time',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngTime', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

    }
});

Ext.define('Ext.ng.Checkbox', {
    extend: 'Ext.form.field.Checkbox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngCheckbox', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

        me.addEvents('beforechange');
    },
    getCustomValue: function () {
        if (this.value) {
            return 1;
        }
        else {
            return 0;
        }
    },
    setValue: function (checked) {
        var me = this,
         boxes, i, len, box;

        //添加事件，可以做是否勾选判断
        if (!me.fireEvent('beforechange', me, checked)) {
            return;
        }
        //me.value = value;
        //me.checkChange();
        //return me;

        if (Ext.isArray(checked)) {
            boxes = me.getManager().getByName(me.name, me.getFormId()).items;
            len = boxes.length;

            for (i = 0; i < len; ++i) {
                box = boxes[i];
                box.setValue(Ext.Array.contains(checked, box.inputValue));
            }
        } else {
            me.callParent(arguments);
        }

        return me;
    }
});

Ext.define('Ext.ng.Radio', {
    extend: 'Ext.form.field.Radio',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngRadio', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();

    }
});

// #endregion

// #region observe

Ext.define('Ext.ng.ListRefresher', {
    mixins: {
        observable: 'Ext.util.Observable'
    },
    constructor: function (config) {
        this.mixins.observable.constructor.call(this, config);

        this.addEvents('refreshlist');
    }
});


Ext.define('Ext.ng.Checker', {
    mixins: {
        observable: 'Ext.util.Observable'
    },
    constructor: function (config) {
        this.mixins.observable.constructor.call(this, config);

        this.addEvents('checkin');
    }
});


Ext.define('Ext.ng.CheckColumn', {
    extend: 'Ext.grid.column.Column',
    alternateClassName: 'Ext.ux.CheckColumn',
    alias: 'widget.ngcheckcolumn',
    align: 'center',
    stopSelection: true,
    checkedVal: true,
    unCheckedVal: false,
    tdCls: Ext.baseCSSPrefix + 'grid-cell-checkcolumn',
    innerCls: Ext.baseCSSPrefix + 'grid-cell-inner-checkcolumn',
    clickTargetName: 'el',
    constructor: function () {
        this.addEvents(
            'beforecheckchange',
            'checkchange'
        );
        this.scope = this;
        this.callParent(arguments);


        if (this.readOnly === true) {
            this.on('beforecheckchange', function () {
                return false;
            });
        }
    },
    processEvent: function (type, view, cell, recordIndex, cellIndex, e, record, row) {
        var me = this,
            key = type === 'keydown' && e.getKey(),
            mousedown = type == 'mousedown';

        if (!me.disabled && (mousedown || (key == e.ENTER || key == e.SPACE))) {
            var dataIndex = me.dataIndex;
            var checked;
            var val = record.get(dataIndex);
            if (val === me.checkedVal) {
                checked = true;
            } else {
                checked = false;
            }

            checked = !checked;////点一次要反一下
            //checked = !record.get(dataIndex);

            // Allow apps to hook beforecheckchange
            if (me.fireEvent('beforecheckchange', me, recordIndex, checked) !== false) {

                if (checked) {
                    record.set(dataIndex, me.checkedVal);
                } else {
                    record.set(dataIndex, me.unCheckedVal);
                }

                me.fireEvent('checkchange', me, recordIndex, checked);

                // Mousedown on the now nonexistent cell causes the view to blur, so stop it continuing.
                if (mousedown) {
                    e.stopEvent();
                }

                // Selection will not proceed after this because of the DOM update caused by the record modification
                // Invoke the SelectionModel unless configured not to do so
                if (!me.stopSelection) {
                    view.selModel.selectByPosition({
                        row: recordIndex,
                        column: cellIndex
                    });
                }

                // Prevent the view from propagating the event to the selection model - we have done that job.
                return false;
            } else {
                // Prevent the view from propagating the event to the selection model if configured to do so.
                return !me.stopSelection;
            }
        } else {
            return me.callParent(arguments);
        }
    },
    onEnable: function (silent) {
        var me = this;

        me.callParent(arguments);
        me.up('tablepanel').el.select('.' + Ext.baseCSSPrefix + 'grid-cell-' + me.id).removeCls(me.disabledCls);
        if (!silent) {
            me.fireEvent('enable', me);
        }
    },
    onDisable: function (silent) {
        var me = this;

        me.callParent(arguments);
        me.up('tablepanel').el.select('.' + Ext.baseCSSPrefix + 'grid-cell-' + me.id).addCls(me.disabledCls);
        if (!silent) {
            me.fireEvent('disable', me);
        }
    },
    renderer: function (value, meta) {
        var me = this;

        var cssPrefix = Ext.baseCSSPrefix,
            cls = [cssPrefix + 'grid-checkcolumn'];

        if (this.disabled) {
            meta.tdCls += ' ' + this.disabledCls;
        }

        if (value == me.checkedVal) {
            cls.push(cssPrefix + 'grid-checkcolumn-checked');
        }

        return '<img class="' + cls.join(' ') + '" src="' + Ext.BLANK_IMAGE_URL + '"/>';
    },
    readOnlyFunction: function () {
        return false;
    },
    userSetReadOnly: function (flag) {
        var me = this;
        if (flag) {
            me.on('beforecheckchange', me.readOnlyFunction);
        }
        else {
            me.un('beforecheckchange', me.readOnlyFunction);
        }
    }
});

Ext.define('Ext.ng.RadioColumn', {
    extend: 'Ext.grid.column.CheckColumn',
    alternateClassName: 'Ext.ux.RadioColumn',
    alias: 'widget.ngRadioColumn',
    gridstore: undefined,
    constructor: function () {
        var me = this;
        this.callParent(arguments);
        this.on('beforecheckchange', function (column, recordIndex, checked) {

            if (checked == false) {
                return false;
            }
            else {
                var record = me.gridstore.findRecord(column.dataIndex, true);
                if (record != null) {
                    record.set(column.dataIndex, false)
                }
                return true;
            }


        });
    },

    /**
     * @private
     * Process and refire events routed from the GridView's processEvent method.
     */
    processEvent: function (type, view, cell, recordIndex, cellIndex, e, record, row) {
        var me = this;
        me.callParent(arguments);
        me.gridstore = view.store;
    },
    // Note: class names are not placed on the prototype bc renderer scope
    // is not in the header.
    renderer: function (value, meta) {
        var cssPrefix = Ext.baseCSSPrefix,
            //cls = [cssPrefix + 'grid-checkcolumn'];
			cls = ['x-form-radio'];

        if (this.disabled) {
            meta.tdCls += ' ' + this.disabledCls;
        }
        if (value) {
            cls.push(cssPrefix + 'grid-checkcolumn-checked');
        }
        return '<img class="' + cls.join(' ') + '" src="' + Ext.BLANK_IMAGE_URL + '"/>';
    }
});

// #endregion

// #region NG.Util


//-------获取Ext.form.Panel对象的数据-----
//
//form: 表单Ext.form.Panel对象
//key: 单据的主键,多主键以","号为分隔符
//optype:操作类型, 新增：new,修改：edit
//返回json格式数据
//----------------------------------------------
function GetExtJsFormData(form, key, optype, serialflag) {


    if (optype === 'copy' || optype === 'add') {
        optype = 'newRow';
    }
    else {
        optype = 'modifiedRow';
    }

    var data = DealFormData(form);  

    //处理主键，fastdp是多主键
    var keys = key.split(',');
    if (!data.hasOwnProperty(keys[0])) {
        NGMsg.Error("busKey属性设置不正确！");
    }

    if (Ext.isEmpty(data[keys[0]])) {//主键值为空，则为新增
        optype = 'newRow';
    }

    var temp = '';
    for (var i = 0; i < keys.length; i++) {

        var keytemp = keys[i];
        var arr = keys[i].split('.');
        if (arr.length > 1) {
            keytemp = arr[1];
        }

        if (i < (keys.length - 1)) {
            temp += data[keytemp] + ",";
        }
        else {
            temp += data[keytemp];
        }

    }
    data["key"] = temp;  //data[key];//主键列的值

    var obj = new Object();
    obj['key'] = key; //主键列
    obj[optype] = data;
    data = { 'form': obj };

    if (serialflag) {
        var json = JSON.stringify(data); //Ext.encode(data);
        return json;
    }
    return data;
}

//Ext4.0版本，复合控件不能清空值处理
//Ext4.2已经修正
function DealFormData(form) {

    var fields = form.getForm().getFields().items;
    var toDeleteArr = [];//待删除的行
    for (var i = 0; i < fields.length; i++) {
        if (fields[i].inEditor) {         
            toDeleteArr.push(i);
        }
    }
    for (var i = toDeleteArr.length - 1; i >= 0; i--) {
        fields.splice(toDeleteArr[i], 1);//删
    }

    var formdata = form.getForm();
    var data = formdata.getValues();

    //处理那些值为空导致getValues()取不到值，不能传值的对象
    for (var i = 0; i < fields.length; i++) {

        var field = fields[i];
        var classname = field.alternateClassName;
        var fieldname = field.name; //字段

        //combox控件处理
        if (classname === 'Ext.form.field.ComboBox' || classname === 'Ext.form.ComboBox') {
            if (!data.hasOwnProperty(fieldname)) {
                data[fieldname] = ''; //空值
            }
        }

        //Checkbox控件处理
        if (classname === 'Ext.form.field.Checkbox' || classname === 'Ext.form.Checkbox') {
            if (!data.hasOwnProperty(fieldname)) {
                data[fieldname] = ''; //空值
            }
        }

        //Radio控件处理
        if (classname === 'Ext.form.field.Radio' || classname === 'Ext.form.Radio') {
            if (!data.hasOwnProperty(fieldname)) {
                data[fieldname] = ''; //空值
            } else if (Ext.isArray(data[fieldname])) {
                var temppp = data[fieldname].filter(function (n) { return n != '' });
                if (temppp.length > 0) {
                    data[fieldname] = temppp[0];
                }
            }
        }
    }

    return data;
}

//-------合并Ext.form.Panel对象的数据-----
//
//forms: 表单Ext.form.Panel对象数组
//key: 单据的主键,多主键以","号为分隔符
//optype:操作类型, 新增：new,修改：edit
//返回json格式数据
//----------------------------------------------
function MergeFormData(forms, key, optype) {

    if (optype === 'copy' || optype === 'add') {
        optype = 'newRow';
    }
    else {
        optype = 'modifiedRow';
    }

    if (forms.length > 0) {


        var data = DealFormData(forms[0]);

        for (var i = 1; i < forms.length; i++) {

            var obj = DealFormData(forms[i]);

            for (var p in obj) {
                if (!data.hasOwnProperty(p)) {
                    data[p] = obj[p]; //键值对加入data中
                }
            }
        }

        //处理主键
        var keys = key.split(',');
        var temp = '';
        for (var i = 0; i < keys.length; i++) {

            var keytemp = keys[i];
            var arr = keys[i].split('.');
            if (arr.length > 1) {
                keytemp = arr[1];
            }

            if (i < (keys.length - 1)) {
                temp += data[keytemp] + ",";
            }
            else {
                temp += data[keytemp];
            }

        }
        data["key"] = temp;  //data[key];//主键列的值

        var obj = new Object();
        obj['key'] = key; //主键列id
        obj[optype] = data;
        data = { 'form': obj };

        var json = JSON.stringify(data); //Ext.encode(data);
        return json;

    }
}

//----------获取ExtJs.Grid对象的数据----------
//store : Ext.data.Store对象
//key : 主键列，多主键以","为分隔符
//
//返回json格式数据
//------------------------------------------------
function GetExtJsGridData(store, key, forEntity) {

    var flag = false;
    var newRecords = store.getNewRecords(); //获得新增行  
    var modifyRecords = store.getUpdatedRecords(); // 获取修改的行的数据，无法获取幻影数据 
    var removeRecords = store.getRemovedRecords(); //获取移除的行

    var newdata = [];
    Ext.Array.each(newRecords, function (record) {

        //
        var newobj = record.data;
        newobj["key"] = null;
        newobj = { 'row': newobj }; //行标记
        newdata.push(newobj);
    });

    var modifydata = [];
    Ext.Array.each(modifyRecords, function (record) {

        //
        var modifyobj = new Object(); //record.modified;

        //处理主键
        var keys = key.split(',');
        var values = '';
        for (i = 0; i < keys.length; i++) {
            if (i < (keys.length - 1)) {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }

                values += ",";
            }
            else {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }
            }
        }

        if (forEntity) {
            var modified = new Object();
            //处理修改的字段,entity修改行要全传               
            record.data["key"] = values;
            modifyobj = { 'row': record.data }; //行标记
            modifydata.push(modifyobj);
        }
        else {
            //处理修改的字段,datatable只传修改的部分,减少数据传输量
            var modified = new Object();
            modified["key"] = values;
            for (var p in record.modified) {
                modified[p] = record.data[p];
            }
            modifyobj = { 'row': modified }; //行标记

            modifydata.push(modifyobj);
        }

    });

    var removedata = [];
    Ext.Array.each(removeRecords, function (record) {
        //

        var object = new Object();
        //object[key] = record.get(key);

        var keys = key.split(',');
        var values = "";
        for (i = 0; i < keys.length; i++) {
            if (i < (keys.length - 1)) {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }

                values += ",";
            }
            else {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }
            }
        }
        object["key"] = values;
        object = { 'row': object }; //行标记
        removedata.push(object);
    });

    var data = new Object();
    data['key'] = key;

    //
    if (newdata.length > 0) {
        data['newRow'] = newdata;
        flag = true;
    }
    if (modifydata.length > 0) {
        data['modifiedRow'] = modifydata;
        flag = true;
    }
    if (removedata.length > 0) {
        data['deletedRow'] = removedata;
        flag = true;
    }
    data = { 'table': data };

    if (flag) {
        data['isChanged'] = true;
    }
    return data;
    //    var json = JSON.stringify(data);//Ext.encode(data);
    //    return json;

}

//获取所有的数据，包含行状态,modify行也包含全部数据
function GetExtJsGridAllData(store, key) {

    var newRecords = store.getNewRecords(); //获得新增行  
    var modifyRecords = store.getUpdatedRecords(); // 获取修改的行的数据，无法获取幻影数据 
    var removeRecords = store.getRemovedRecords(); //获取移除的行

    var keyIndex = {};//记录修改、删除行

    var keys = key.split(','); //多主键  

    Ext.Array.each(newRecords, function (record) {
        var allValue = '';
        for (var i = 0; i < keys.length; i++) {
            allValue += record.data[keys[i]];
        }
        if (!Ext.isEmpty(allValue)) {
            keyIndex[allValue] = allValue;//新增行的key也可能有值的
        }
    });

    Ext.Array.each(modifyRecords, function (record) {
        var allValue = '';
        for (var i = 0; i < keys.length; i++) {
             allValue += record.data[keys[i]];
        }
        keyIndex[allValue] = allValue;       
    });
    Ext.Array.each(removeRecords, function (record) {
        //keyIndex[record.data[key]] = record.data[key];
        var allValue = '';
        for (var i = 0; i < keys.length; i++) {
             allValue += record.data[keys[i]];
        }
        keyIndex[allValue] = allValue;  
    });

    var unchangeRecords = [];
    for (var i = 0; i < store.data.items.length; i++) {

        //if (!Ext.isEmpty(store.data.items[i].data[key]) && !keyIndex[store.data.items[i].data[key]]) {
        //    unchangeRecords.push(store.data.items[i]);
        //}

        var allValue = '';
        for (var j = 0; j < keys.length; j++) {
             allValue += store.data.items[i].data[keys[j]];
        }
        
        //不是新增、修改、删除行，视为未修改行
         if (!Ext.isEmpty(allValue) && !keyIndex[allValue]) {
            unchangeRecords.push(store.data.items[i]);
        }
    }

    return GetDatatableData(newRecords, modifyRecords, removeRecords, unchangeRecords, key);

}

//----------获取DataTable的json数据----------
//新增:newRecords,修改:modifyRecords,删除:removeRecords,不变:unchangeRecords
// 数据类型都是Ext.data.Model[]
// key：业务主键
//------------------------------------------------
function GetDatatableData(newRecords, modifyRecords, removeRecords, unchangeRecords, key) {

    var flag = false;
    var newdata = [];
    Ext.Array.each(newRecords, function (record) {

        //
        var newobj = record.data;
        newobj["key"] = null;
        newobj = { 'row': newobj }; //行标记
        newdata.push(newobj);
    });

    var modifydata = [];
    Ext.Array.each(modifyRecords, function (record) {

        //
        var modifyobj = new Object(); //record.modified;

        //处理主键
        var keys = key.split(',');
        var values = '';
        for (i = 0; i < keys.length; i++) {
            if (i < (keys.length - 1)) {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }

                values += ",";
            }
            else {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }
            }
        }

        //处理修改的字段，全传
        var modified = new Object();
        record.data["key"] = values;
        //        for (var p in record.modified) {
        //            modified[p] = record.data[p];
        //        }       
        modifyobj = { 'row': record.data }; //行标记
        modifydata.push(modifyobj);
    });

    var removedata = [];
    Ext.Array.each(removeRecords, function (record) {
        //

        var object = new Object();
        //object[key] = record.get(key);

        var keys = key.split(',');
        var values = "";
        for (i = 0; i < keys.length; i++) {
            if (i < (keys.length - 1)) {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }

                values += ",";
            }
            else {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }
            }
        }
        object["key"] = values;
        object = { 'row': object }; //行标记
        removedata.push(object);
    });

    var unChangedData = [];
    Ext.Array.each(unchangeRecords, function (record) {

        //处理主键
        var keys = key.split(',');
        var values = '';
        for (i = 0; i < keys.length; i++) {
            if (i < (keys.length - 1)) {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }

                values += ",";
            }
            else {
                if (record.data[keys[i]]) {
                    values += record.data[keys[i]];
                }
                else {
                    values += "";
                }
            }
        }

        record.data['key'] = values;
        var obj = { 'row': record.data }; //行标记
        unChangedData.push(obj);
    });

    var data = new Object();
    data['key'] = key;

    if (newdata.length > 0) {
        data['newRow'] = newdata;
        flag = true;
    }
    if (modifydata.length > 0) {
        data['modifiedRow'] = modifydata;
        flag = true;
    }
    if (removedata.length > 0) {
        data['deletedRow'] = removedata;
        flag = true;
    }
    if (unChangedData.length > 0) {
        data['unChangedRow'] = unChangedData;
        flag = true;
    }

    data = { 'table': data };
    if (flag) {
        data['isChanged'] = true;
    }
    return data;
};

//----------获取DataTable的json数据----------
//新增:data.add,修改:data.modify,删除:data.remove,不变:data.unchange
// 数据类型都是Ext.data.Model[]
// data.key：业务主键
//------------------------------------------------
function GetTableData(data) {

    var newdata = data.add || [];
    var modifydata = data.modify || [];
    var removedata = data.remove || [];
    var unChangedData = data.unchange || [];

    return GetDatatableData(newdata, modifydata, removedata, unChangedData, data.key);
};

//----------获取ExtJs.Grid对象的所有数据----------
//store : Ext.data.Store对象
//key : 主键列，多主键以","为分隔符
//返回行状态全是新增的json格式数据
//------------------------------------------------
function GetAllGridData(store, key) {

    var all = store.getRange();

    var newdata = [];
    Ext.Array.each(all, function (record) {

        var newobj = record.data;
        newobj["key"] = null;
        newobj = { 'row': newobj }; //行标记
        newdata.push(newobj);
    });

    var data = new Object();
    data['key'] = key;

    if (newdata.length > 0) {
        data['newRow'] = newdata;
    }

    data = { 'table': data };

    var json = JSON.stringify(data); //Ext.encode(data);
    return json;

}

//----------校验ExtJs.Grid对象的数据----------
//store : Ext.grid.Panel对象
//通过:true,失败:false
//------------------------------------------------
function ValidGridData(grid) {

    var store = grid.store;
    var newRecords = store.getNewRecords(); //获得新增行  
    var modifyRecords = store.getUpdatedRecords(); // 获取修改的行的数据，无法获取幻影数据
    var removeRecords = store.getRemovedRecords(); //获取移除的行

    for (var i = 0; i < store.data.items.length; i++) {
        var curRow = store.data.items[i].data;

        var j = 0;
        for (property in curRow) {
            var name = property; //列名
            var value = curRow[property]; //值

            var colIndex = -1;
            var findColumn = false;
            var isInLockGrid = false;//是否在锁定列中

            if (grid.lockable) {
                //查找是第几列
                for (var k = 0; k < grid.lockedGrid.columns.length; k++) {
                    var column = grid.lockedGrid.columns[k];
                    if (name === column.dataIndex) {
                        colIndex = k;
                        findColumn = true;
                        isInLockGrid = true;
                        break;
                    }
                }

                if (!isInLockGrid) {
                    for (var k = 0; k < grid.normalGrid.columns.length; k++) {
                        var column = grid.normalGrid.columns[k];
                        if (name === column.dataIndex) {
                            colIndex = k;
                            findColumn = true;                           
                            break;
                        }
                    }
                }

            }
            else {
                //查找是第几列
                for (var k = 0; k < grid.columns.length; k++) {
                    var column = grid.columns[k];
                    if (name === column.dataIndex) {
                        colIndex = k;
                        findColumn = true;
                        break;
                    }
                }
            }

            if (!findColumn) continue;
            //if (!grid.columns[colIndex].getEditor) continue; //容错,有锁定列,获取不到真正的列，的从lockedGrid或者normalGrid获取  
            var curColumn = grid.getColumn(name);
            if (!curColumn.getEditor) continue;
            j++;
            
            var editor = curColumn.getEditor();
            if (editor) {

                var isMustInput = curColumn.initialConfig.mustInput;//必输项单独处理               
                //if (!editor.validateValue(value) || (!editor.allowBlank && Ext.isEmpty(value))) {
                if (!editor.validateValue(value) || (isMustInput && Ext.isEmpty(value))) {

                    var errorMsg = !editor.validateValue(value) ? editor.activeErrors : "该项为必输项";
                    errorMsg = '第' + (i + 1) + '行， [' + curColumn.text + ']列输入不合法：' + errorMsg;

                    var msg = Ext.Msg.show(
								{
								    title: '提示',
								    msg: errorMsg,
								    closable: false,
								    buttons: Ext.Msg.OK,
								    icon: Ext.Msg.ERROR,
								    animateTarget: grid.getView().getNode(store.data.items[i]).childNodes[j - 1].id
								}
							);

                    var readGrid = grid;
                    if (grid.lockable) {
                        if (isInLockGrid) {
                            readGrid = grid.lockedGrid;
                        }
                        else {
                            readGrid = grid.normalGrid;
                        }                        
                    }               
                    setTimeout(function () {
                        msg.close();
                        readGrid.plugins[0].startEdit(i, (colIndex));//grid.plugins[0].startEdit(i, (j));
                    }, 5000);

                    //grid.plugins[0].startEditByPosition({ row: i, column: (j+1) });
                    //grid.plugins[0].startEdit(i, (j));
                    return false;
                }
            }
        }
    }

    //列内容的唯一性验证
    if (grid.uniqueColumn) {

        for (var i = 0; i < grid.uniqueColumn.length; i++) {
            var temp = grid.uniqueColumn[i];
            if (temp.col.indexOf(',') > 0) {
                var obj = {};
                var arr = temp.col.split(',');
                for (var j = 0; j < store.data.items.length; j++) {

                    var data = store.data.items[j].data;
                    var value = '';
                    for (var k = 0; k < arr.length; k++) {
                        if (data[arr[k]]) {
                            value += data[arr[k]];
                        }
                    }

                    if (obj.hasOwnProperty(value)) {//已经存在

                        NGMsg.Error('第' + (j + 1) + '行输入数据重复!');
                        return false;
                    } else {
                        if (!Ext.isEmpty(value)) {
                            obj[value] = value;
                        }
                    }
                }
            }
            else {
                var obj = {};
                for (var j = 0; j < store.data.items.length; j++) {
                    var data = store.data.items[j].data;
                    var value = temp.col;
                    if (obj.hasOwnProperty(data[value])) {//已经存在

                        var findColumn = false;
                        var colIndex = -1;
                        //查找是第几列
                        for (var k = 0; k < grid.columns.length; k++) {
                            var column = grid.columns[k];
                            if (value === column.dataIndex) {
                                colIndex = k;
                                findColumn = true;
                                break;
                            }
                        }
                        if (!findColumn) continue;

                        grid.plugins[0].startEdit(j, colIndex);
                        NGMsg.Error('第' + (j + 1) + '行输入数据重复!');
                       

                        return false;
                    } else {
                        if (!Ext.isEmpty(data[value])) {
                            obj[data[value]] = data[value];
                        }
                    }
                }
            }
        }
    }

    return true;
};
function ValidTreeGridData(grid) {
    
        var finalArr = []; //递归store，存储为一个数组
        var storeRoot = grid.store.tree.root;
    
        var gridAll = grid.down().getGridColumns();  //获取所有的columns,包含嵌套的
        //grid.expandAll();
        //【1】先全部递归试试，包含root节点
        var recursiveStore = function (storeRoot) {
            if (storeRoot.childNodes.length != 0) {
                finalArr.push(storeRoot)
                for (var i = 0; i < storeRoot.childNodes.length; i++) {
                    var child = storeRoot.childNodes[i];
                    recursiveStore(child)
                }
            } else {
                finalArr.push(storeRoot)
            }
        }
        recursiveStore(storeRoot);
        //【2】去除finalArr中的第一个Root节点
        finalArr = finalArr.slice(1);
        //【3】循环数组
        for (var i = 0; i < finalArr.length; i++) {
            var curRow = finalArr[i].data;
    
            //var j = 0;
            for (property in curRow) {
                var name = property; //列名
                var value = curRow[property]; //值
                //为0 怎么处理？
                //if (value == 0) {
                //    value = '';
                //}
    
                var colIndex = -1;
                var findColumn = false;
                //查找是第几列
                for (var k = 0; k < gridAll.length; k++) {
                    var column = gridAll[k];
                    if (name === column.dataIndex) {
                        colIndex = k;
                        findColumn = true;
                        break;
                    }
                }
    
                if (!findColumn) continue;
                if (!gridAll[colIndex].getEditor) continue; //容错
                //j++;
    
                var editor = gridAll[colIndex].getEditor();
    
                if (editor) {
    
                    var isMustInput = gridAll[colIndex].initialConfig.mustInput;//必输项单独处理               
                    //if (!editor.validateValue(value) || (!editor.allowBlank && Ext.isEmpty(value))) {
                    if (!editor.validateValue(value) || (isMustInput && Ext.isEmpty(value))) {
                        //console.log(finalArr[i])
                        //console.log(grid.getView().getNode(finalArr[i]))
                        var errorMsg = !editor.validateValue(value) ? editor.activeErrors : "该项为必输项";
                        if (grid.getView().getNode(finalArr[i]) != null && finalArr[i].childNodes.length == 0) {
                            errorMsg = '第' + (i + 1) + '行， [' + column.text + ']列输入不合法：' + errorMsg;
                        } else {
                            errorMsg = '[' + column.text + ']列输入不合法：' + errorMsg;
                        }
                        var msg = Ext.Msg.show(
                                    {
                                        title: '提示',
                                        msg: errorMsg,
                                        closable: false,
                                        buttons: Ext.Msg.OK,
                                        icon: Ext.Msg.ERROR,
                                        //animateTarget: grid.getView().getNode(store.data.items[i]).childNodes[j - 1].id
                                    }
                                );
    
                        setTimeout(function () {
                            msg.close();
                            grid.plugins[0].startEdit(i, (colIndex));//grid.plugins[0].startEdit(i, (j));
                        }, 1000);
    
    
                        return false;
                    }
                }
            }
        }
    
        //列内容的唯一性验证
        //if (grid.uniqueColumn) {
    
        //    for (var i = 0; i < grid.uniqueColumn.length; i++) {
        //        var temp = grid.uniqueColumn[i];
        //        if (temp.col.indexOf(',') > 0) {
        //            var obj = {};
        //            var arr = temp.col.split(',');
        //            for (var j = 0; j < finalArr.length; j++) {
    
        //                var data = finalArr[j].data;
        //                var value = '';
        //                for (var k = 0; k < arr.length; k++) {
        //                    if (data[arr[k]]) {
        //                        value += data[arr[k]];
        //                    }
        //                }
    
        //                if (obj.hasOwnProperty(value)) {//已经存在
    
        //                    //NGMsg.Error('第' + (j + 1) + '行输入数据重复!');
        //                    var errorMsg = '第' + (j + 1) + '行输入数据重复!';
        //                    var msg = Ext.Msg.show(
        //                        {
        //                            title: '提示',
        //                            msg: errorMsg,
        //                            closable: true,
        //                            buttons: Ext.Msg.OK,
        //                            icon: Ext.Msg.ERROR,
        //                            //animateTarget: grid.getView().getNode(finalArr[i]).id
        //                        }
        //                    );
        //                    return false;
        //                } else {
        //                    if (!Ext.isEmpty(value)) {
        //                        obj[value] = value;
        //                    }
        //                }
        //            }
        //        }
        //        else {
        //            var obj = {};
        //            for (var j = 0; j < finalArr.length; j++) {
        //                var data = finalArr[j].data;
        //                var value = temp.col;
        //                if (obj.hasOwnProperty(data[value])) {//已经存在
    
        //                    var findColumn = false;
        //                    var colIndex = -1;
        //                    //查找是第几列
        //                    for (var k = 0; k < gridAll.length; k++) {
        //                        var column = gridAll[k];
        //                        if (value === column.dataIndex) {
        //                            colIndex = k;
        //                            findColumn = true;
        //                            break;
        //                        }
        //                    }
        //                    if (!findColumn) continue;
    
        //                    grid.plugins[0].startEdit(j, colIndex);
        //                    //NGMsg.Error('第' + (j + 1) + '行输入数据重复!');
        //                    var errorMsg = '第' + (j + 1) + '行输入数据重复!';
        //                    var msg = Ext.Msg.show(
        //                        {
        //                            title: '提示',
        //                            msg: errorMsg,
        //                            closable: true,
        //                            buttons: Ext.Msg.OK,
        //                            icon: Ext.Msg.ERROR,
        //                            //animateTarget: grid.getView().getNode(finalArr[i]).id
        //                        }
        //                    );
        //                    return false;
        //                } else {
        //                    if (!Ext.isEmpty(data[value])) {
        //                        obj[data[value]] = data[value];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    
        return true;
    };

function FormatToDate(CurrTime, format) {

    //"Mon Jan 7 15:07:37 UTC+0800 2013";

    //"Thu Jan 03 2013 00:00:00 GMT+0800"

    //

    CurrTime = CurrTime + '';
    var date = '';
    var month = new Array();
    month["Jan"] = 1;
    month["Feb"] = 2;
    month["Mar"] = 3;
    month["Apr"] = 4;
    month["May"] = 5;
    month["Jun"] = 6;
    month["Jul"] = 7;
    month["Aug"] = 8;
    month["Sep"] = 9;
    month["Oct"] = 10;
    month["Nov"] = 11;
    month["Dec"] = 12;

    var str = CurrTime.split(" ");

    if (CurrTime.indexOf('UTC') > 0) {
        if (format === "yyyy-mm-dd") {
            date = str[5] + '-' + month[str[1]] + '-' + str[2];
        }
        else if (format.toLocaleLowerCase() === "yyyy-mm-dd hh:mm:ss") {
            date = str[5] + '-' + month[str[1]] + '-' + str[2] + " " + str[3];
        }
    }
    else if (CurrTime.indexOf('GMT') > 0) {
        if (format.toLocaleLowerCase() === "yyyy-mm-dd") {
            date = str[3] + '-' + month[str[1]] + '-' + str[2];
        }
        else if (format.toLocaleLowerCase() === "yyyy-mm-dd hh:mm:ss") {
            date = str[3] + '-' + month[str[1]] + '-' + str[2] + " " + str[4];
        }
    }


    return date;
}

function BindCombox(combox, code, name, helpid, codeValue, helptype) {

    Ext.Ajax.request({
        //params: { 'id': busid },
        url: C_ROOT + 'SUP/CommonHelp/GetName?helptype=' + helptype + '&helpid=' + helpid + '&code=' + codeValue,
        success: function (response) {

            var resp = Ext.JSON.decode(response.responseText);
            if (resp.status === "ok") {

                Ext.define('model', {
                    extend: 'Ext.data.Model',
                    fields: [{
                        name: code, //'code',
                        type: 'string',
                        mapping: code//'code'
                    }, {
                        name: name, //'name',
                        type: 'string',
                        mapping: name//'name'
                    }
					 ]
                });

                var obj = new Object();
                obj[code] = codeValue;
                if (combox.displayFormat) {
                    obj[name] = Ext.String.format(combox.displayFormat, codeValue, resp.name);
                } else {
                    obj[name] = resp.name;
                }
                var provincepair = Ext.ModelManager.create(obj, 'model');
                combox.setValue(provincepair);

            } else {
                Ext.MessageBox.alert('取数失败', resp.status);
            }
        }
    });
}

//----------批量做帮助控件的代码名称转换----------
//comboxs : Ext.ng.CommonHelp,Ext.form.ComboBox对象数组
//
//------------------------------------------------
function BatchBindCombox(comboxs, callbackFunc) {
       
    var codeobj = [];
    var comboxColls = [];
    for (var i = 0; i < comboxs.length; i++) {
        if (!comboxs[i]) continue;

        if (comboxs[i].alternateClassName === 'Ext.ng.CommonHelp'
		|| (comboxs[i].alternateClassName === 'Ext.form.ComboBox' && comboxs[i].queryMode === 'remote')	
		|| comboxs[i].alternateClassName === 'Ext.ng.MultiHelp') {


            if (!Ext.isEmpty(comboxs[i].getValue())) {
                //var config = comboxs[i].initialConfig;
                var obj = {};
                obj['HelpID'] = comboxs[i].helpid;
                if (obj['HelpID'] == "" && comboxs[i].queryMode == "local") {
                    Ext.Array.each(comboxs[i].data, function (record) {
                        if (record.code == comboxs[i].getValue()) {
                            Ext.define('model', {
                                extend: 'Ext.data.Model',
                                fields: [{
                                    name: 'code',
                                    type: 'string',
                                    mapping: 'code'
                                }, {
                                    name: 'name',
                                    type: 'string',
                                    mapping: 'name'
                                }
					             ]
                            });

                            var obj2 = new Object();
                            obj2['code'] = comboxs[i].getValue();
                            if (comboxs[i].displayFormat) {
                                obj2['name'] = Ext.String.format(comboxs[i].displayFormat, comboxs[i].getValue(), record.name);
                            } else {
                                obj2['name'] = record.name;
                            }
                            var provincepair = Ext.ModelManager.create(obj2, 'model');
                            comboxs[i].setValue(provincepair);
                        }
                    });
                    continue;
                }
                obj['Code'] = comboxs[i].getValue();
                obj['SelectMode'] = comboxs[i].selectMode;
                obj['HelpType'] = comboxs[i].xtype;
                obj['Name'] = '';
                if (comboxs[i].ignoreOutFilter) {//代码转名称忽略外部条件
                    obj['OutJsonQuery'] = '';
                }
                else {
                    obj['OutJsonQuery'] = comboxs[i]['outFilter'] ? comboxs[i]['outFilter'] : '';
                }
                codeobj.push(obj);

                comboxColls.push(comboxs[i]);
            }

            //ComboBox多选设置值
            if (comboxs[i].alternateClassName === 'Ext.form.ComboBox' && comboxs[i].queryMode === 'local' && comboxs[i].multiSelect) {
               comboxs[i].setValue(comboxs[i].getValue().split(','));//转成数组
           }
        }
    }

    if (codeobj.length > 0) {
        Ext.Ajax.request({
            params: { 'valueobj': Ext.encode(codeobj) },
            url: C_ROOT + 'SUP/CommonHelp/GetAllNames',
            callback: callbackFunc,
            success: function (response) {

                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    for (var i = 0; i < comboxColls.length; i++) {


                        var config = comboxColls[i].initialConfig;
                        var code = config.valueField || comboxColls[i].valueField;
                        var name = config.displayField || comboxColls[i].displayField;

                        //去掉表名
                        //var myValueFiled;
                        //var myDisplayField;
                        //var temp = code.split('.');
                        //if (temp.length > 1) {
                        //    myValueFiled = temp[1];
                        //} else {
                        //    myValueFiled = config.valueField;
                        //}

                        //temp = name.split('.');
                        //if (temp.length > 1) {
                        //    myDisplayField = temp[1];
                        //} else {
                        //    myDisplayField = config.displayField;
                        //}

                        var valtype = comboxColls[i].valueType ? comboxColls[i].valueType : 'string';
                        Ext.define('model', {
                            extend: 'Ext.data.Model',
                            fields: [{
                                name: code, //,'myValueFiled',
                                type: valtype,
                                mapping: code //'myValueFiled'
                            }, {
                                name: name, //'myDisplayField',
                                type: 'string',
                                mapping: name//'myDisplayField'
                            }
                            ]
                        });

                        var nameValue = resp.name[i]['Name'];

                        if (!Ext.isEmpty(nameValue)) {
                            var obj = new Object();
                            //obj[myValueFiled] = comboxColls[i].getValue();
                            obj[code] = comboxColls[i].getValue();
                            if (comboxColls[i].displayFormat) {
                                //obj[myDisplayField] = Ext.String.format(comboxColls[i].displayFormat, comboxColls[i].getValue(), nameValue);
                                obj[name] = Ext.String.format(comboxColls[i].displayFormat, comboxColls[i].getValue(), nameValue);
                            } else {
                                //obj[myDisplayField] = nameValue;
                                obj[name] = nameValue;
                            }
                            var provincepair = Ext.ModelManager.create(obj, 'model');
                            comboxColls[i].setValue(provincepair);
                        } else {

                            var val = comboxColls[i].getValue();
                            //if (valtype == 'int' && val == 0) {//默认值0设置为空
                            if (comboxColls[i].codeIsNum && val == 0) { //代码列为数值型， 由于phid截断的原因，不能通过设置valueType来判断
                                comboxColls[i].setValue('');
                            }
                        }

                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });
    }

}

// #endregion

// #region 业务组件之人员帮助
Ext.define("Ext.ng.EmpHelp", {
    extend: 'Ext.window.Window',
    alias: 'widget.emphelp',
    title: '员工帮助',
    closable: true,
    resizable: false,
    modal: true,
    width: 600,
    height: 400,
    nodeIndex: -1,
    border: false,
    callback: null,  //点击确定后的回调函数
    allowBank: true, //允许返回空值
    bussType: "all", //业务类型
    gridStore: null,
    sqlfilter: "",
    isMulti: false, //默认单选
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this, center = {}, resultStore;
        me.initParam();
        var store = Ext.create('Ext.ng.JsonStore', {
            autoLoad: false,
            pageSize: 15,
            fields: [{
                name: 'cno',
                mapping: 'cno',
                type: 'string'
            }, {
                name: 'cname',
                mapping: 'cname',
                type: 'string'
            }, {
                name: 'assigntype',
                mapping: 'assigntype',
                type: 'string'
            }],
            url: C_ROOT + 'SUP/Person/GetEmpList?gettype=emplist&sqlfilter=' + me.sqlfilter
        }),
			empTree = Ext.create('Ext.ng.TreePanel', {
			    region: 'center',
			    border: false,
			    autoScroll: true,
			    rootVisible: false,
			    useArrows: true,
			    width: 180,
			    minSize: 180,
			    maxSize: 180,
			    treeFields: [{ name: 'text', type: 'string' },
						 { name: 'id', type: 'string' },
						 { name: 'pid', type: 'string' },
						 { name: 'relatindex', type: 'string' },
						 { name: 'bopomofo', type: 'string' }],
			    url: C_ROOT + 'SUP/Person/LoadTree?ctype=hr',
			    listeners: {
			        selectionchange: function (m, selected, eOpts) {
			            me.memory.eOpts = "selectionchange";
			            me.searchData({ leaf: selected[0].data.leaf, ocode: selected[0].data.id, relatindex: selected[0].data.relatindex });
			            me.getDockedItems(":last")[0].query("checkboxfield:first")[0].setValue(me.memory.IsMemo && me.memory.FoucedNodeValue == selected[0].getPath());
			            me.memory.eOpts = "";
			        },
			        itemclick: function (m, record, item, index, e, eOpts) {
			            me.nodeIndex = -1;
			        },
			        load: function (m, node, records, successful, eOpts) {
			            Ext.Ajax.request({
			                url: C_ROOT + 'SUP/Person/GetTreeMemoryInfo',
			                params: { type: me.helpType, busstype: me.bussType },
			                success: function (response, opts) {
			                    me.memory = Ext.JSON.decode(response.responseText);
			                    if (!Ext.isEmpty(me.memory.FoucedNodeValue)) {
			                        empTree.selectPath(me.memory.FoucedNodeValue, null, null, function () {
			                            if (Ext.isIE) {
			                                window.setTimeout(function () {
			                                    var selectNode = m.view.body.query("tr." + m.view.selectedItemCls);
			                                    if (selectNode) {
			                                        selectNode[0].scrollIntoView(true);
			                                    }
			                                }, 500);
			                            }
			                        });
			                    }
			                    else {
			                        store.load();
			                    }
			                }
			            });
			        }
			    }
			}),
			left = {
			    title: "人力资源树",
			    autoScroll: false,
			    collapsible: true,
			    split: true,
			    region: 'west',
			    weight: 50,
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
			            emptyText: '输入关键字，定位树节点',
			            margin: '2 0 2 2',
			            enableKeyEvents: true,
			            listeners: {
			                'keydown': function (el, e, eOpts) {
			                    if (e.getKey() == e.ENTER) {
			                        me.findNodeByFuzzy(empTree, el.getValue());
			                        el.focus();
			                        return false;
			                    }
			                    else {
			                        me.nodeIndex = -1;
			                    }
			                }
			            }
			        }, { region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5', handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(empTree, el.getValue()); el.focus(); } }]
			    }, empTree]
			},
			gridLeft = Ext.create('Ext.ng.GridPanel', Ext.apply({
			    store: store,
			    autoScroll: true,
			    columnLines: true,
			    columns: [{
			        header: '编号',
			        flex: 1,
			        sortable: false,
			        menuDisabled: true,
			        draggable: false,
			        dataIndex: 'cno'
			    }, {
			        header: '姓名',
			        flex: 1,
			        sortable: false,
			        menuDisabled: true,
			        draggable: false,
			        dataIndex: 'cname',
			        renderer: function (value, parm, record) {
			            return me.getEmpName(value, record.data.assigntype);
			        }
			    }],
			    listeners: {
			        'itemdblclick': function (item, record, it, index, e, eOpts) {
			            if (me.isMulti) {
			                var data = gridLeft.getSelectionModel().getSelection();
			                me.copyData(data, resultStore);
			                gridLeft.getSelectionModel().deselectAll();
			            }
			            else if (me.callback) {
			                me.close();
			                me.callback({ cno: record.data.cno, cname: record.data.cname });
			            }
			        }
			    },
			    viewConfig: {
			        style: {
			            overflowX: 'hidden !important'
			        }
			    },
			    bbar: me.isMulti ? null : Ext.create('Ext.ng.PagingBar', {
			        store: store,
			        displayMsg: '共 {2} 条数据',
			        showRefresh: false
			    })
			}, me.gridConfig)), pageBar;
        if (me.isMulti) {
            resultStore = Ext.create('Ext.ng.JsonStore', {
                fields: [{
                    name: 'cno',
                    mapping: 'cno',
                    type: 'string'
                }, {
                    name: 'cname',
                    mapping: 'cname',
                    type: 'string'
                }, {
                    name: 'assigntype',
                    mapping: 'assigntype',
                    type: 'string'
                }]
            });
            var gridRight = Ext.create('Ext.ng.GridPanel', {
                columnWidth: .5,
                store: resultStore,
                height: 341,
                autoScroll: true,
                columnLines: true,
                border: false,
                selModel: { mode: "SIMPLE" },
                columns: [{
                    header: '编号',
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'cno'
                }, {
                    header: '姓名',
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    draggable: false,
                    dataIndex: 'cname'
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
				    height: 350,
				    width: 80,
				    layout: 'absolute',
				    border: true,
				    frame: true,
				    style: { marginTop: "-4px" },
				    items: [{
				        xtype: 'button',
				        name: 'addSelect',
				        text: '&gt;',
				        x: 6,
				        y: 120,
				        width: 60,
				        handler: Ext.bind(function () {
				            var data = gridLeft.getSelectionModel().getSelection();
				            me.copyData(data, resultStore);
				            gridLeft.getSelectionModel().deselectAll();
				        })
				    }, {
				        xtype: 'button',
				        name: 'selectAll',
				        text: '&gt;&gt;',
				        x: 6,
				        y: 150,
				        width: 60,
				        handler: Ext.bind(function () {
				            var data = store.data.items;
				            me.copyData(data, resultStore);
				            gridLeft.getSelectionModel().deselectAll();
				        })
				    }, {
				        xtype: 'button',
				        name: 'removeSelect',
				        text: '&lt;',
				        x: 6,
				        y: 180,
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
				        y: 210,
				        width: 60,
				        handler: Ext.bind(function () {
				            me.removeData(null, resultStore, true);
				        })
				    }]
				},
				pageBar = Ext.create('Ext.ng.PagingBar', {
				    store: store,
				    showRefresh: false,
				    displayMsg: '共 {2} 条数据&nbsp;&nbsp;&nbsp;已选 {3} 条数据',
				    updateInfo: function () {
				        var p = this;
				        var displayItem = p.child('#displayItem'),
					pageData = p.getPageData(), msg;
				        if (displayItem) {
				            if (p.store.getCount() === 0) {
				                msg = p.emptyMsg;
				            }
				            else {
				                msg = Ext.String.format(
								 p.displayMsg,
								 pageData.fromRecord,
								 pageData.toRecord,
								 pageData.total,
								 me.currData.value.length
						);
				            }
				            displayItem.setText(msg);
				        }
				    }
				});
            center = {
                region: 'center',
                border: true,
                layout: 'column',
                items: [gridLeft, btnPanel, gridRight],
                bbar: pageBar
            };
        }
        else {
            center = gridLeft;
        }
        var top = {
            region: 'north',
            xtype: '',
            height: 27,
            border: false,
            layout: 'border',
            items: [{
                region: 'west',
                xtype: "combobox",
                store: Ext.create('Ext.ng.JsonStore', {
                    autoLoad: false,
                    pageSize: 50,
                    fields: [
				{ name: 'ccode', mapping: 'ccode' },
				{ name: 'cname', mapping: 'cname' }
				],
                    proxy: {
                        type: 'ajax',
                        url: C_ROOT + 'SUP/Person/GetEmpStatus',
                        reader: {
                            type: 'json',
                            root: 'Record',
                            totalProperty: 'totalRows'
                        }
                    },
                    listeners: {
                        load: function (m, records, successful, eOpts) {
                            m.insert(0, [{ ccode: '', cname: '&nbsp;' }]);
                        }
                    }
                }),
                width: 100,
                labelWidth: 28,
                allowBlank: true,
                editable: false,
                fieldLabel: '',
                emptyText: '状态',
                valueField: 'ccode',
                displayField: 'cname',
                listeners: {
                    select: function (combo, records, eOpts) {
                        var code = combo.getValue() || records[0].data.emptype;
                        if (Ext.isEmpty(code)) {
                            combo.clearValue();
                        }
                        me.searchData({ "empstatus": code });
                    }
                }
            }, {
                region: 'west',
                xtype: "combobox",
                store: Ext.create('Ext.ng.JsonStore', {
                    autoLoad: false,
                    fields: [
				{ name: 'emptype', mapping: 'emptype' },
				{ name: 'typename', mapping: 'typename' }
				],
                    proxy: {
                        type: 'ajax',
                        url: C_ROOT + 'SUP/Person/GetEmpType',
                        reader: {
                            type: 'json',
                            root: 'Record',
                            totalProperty: 'totalRows'
                        }
                    },
                    listeners: {
                        load: function (m, records, successful, eOpts) {
                            m.insert(0, [{ emptype: '', typename: '&nbsp;' }]);
                        }
                    }
                }),
                width: 100,
                labelWidth: 28,
                allowBlank: true,
                editable: false,
                fieldLabel: '',
                emptyText: '类型',
                valueField: 'emptype',
                displayField: 'typename',
                style: { marginLeft: '10px' },
                listeners: {
                    select: function (combo, records, eOpts) {
                        var code = combo.getValue() || records[0].data.emptype;
                        if (Ext.isEmpty(code)) {
                            combo.clearValue();
                        }
                        me.searchData({ "emptype": code });
                    }
                }
            }, { region: 'center', xtype: 'container' }, {
                region: 'east',
                width: 160,
                fieldLabel: '',
                emptyText: '输入编号/姓名，回车查询',
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
                            var param = { "searchtxt": "" };
                            if (!Ext.isEmpty(key)) {
                                if (/\d+/.test(key)) {
                                    param = { "searchtxt": "cno like '%" + key + "%'" };
                                }
                                else if (/\W+/.test(key)) {
                                    param = { "searchtxt": "cname like '%" + key + "%'" };
                                }
                                else {
                                    param = { "searchtxt": "(cno like '%" + key + "%' or cname like '%" + key + "%' or bopomofo like '%" + key + "%')" };
                                }
                            }
                            me.searchData(param);
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
        me.items = [left, top, center];
        me.buttons = [{
            xtype: 'checkboxfield',
            boxLabel: '记忆树选中状态',
            handler: function (chk) {
                me.updataTreeMemory(empTree, chk.getValue());
            }
        }, {
            xtype: 'checkboxfield',
            boxLabel: '显示兼职',
            handler: function (chk) {
                me.searchData({ "partmark": chk.getValue() ? "1" : "" });
            }
        }, {
            xtype: 'checkboxfield',
            boxLabel: '显示代理',
            handler: function (chk) {
                me.searchData({ "proymark": chk.getValue() ? "1" : "" });
            }
        }, '->', {
            text: '确认',
            handler: function () {
                if (me.callback) {
                    var data = me.isMulti ? me.currData.value : gridLeft.getSelectionModel().getSelection(), tmpdata = {};
                    if (data.length > 0) {
                        if (me.isMulti) {
                            tmpdata = me.currData;
                            tmpdata = { cno: tmpdata.value.join(","), cname: tmpdata.text.join(",") };
                        }
                        else {
                            tmpdata = data[0].data;
                            tmpdata = { cno: tmpdata.cno, cname: tmpdata.cname };
                        }
                    }
                    else {
                        if (me.allowBank) {
                            tmpdata = { cno: '', cname: '' };
                        }
                        else {
                            Ext.MessageBox.alert('', '未选择数据.');
                            return;
                        }
                    }
                    me.close();
                    me.callback(tmpdata);
                }
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];
        me.callParent();
        me.show();
        me.gridStore = store;
        me.pageBar = pageBar;
    },
    initParam: function () {
        var me = this;
        me.memory = {};
        me.gridConfig = {};
        if (me.isMulti) {
            me.currData = { value: [], text: [] };
            me.helpType = "empmulti";
            me.width = 700;
            me.height = 473;
            me.gridConfig.columnWidth = .5;
            me.gridConfig.height = 341;
            me.gridConfig.border = false;
            me.gridConfig.selModel = { mode: "SIMPLE" };
            me.gridConfig.bodyStyle = {
                top: '24px !important'
            };
        }
        else {
            me.helpType = "empsingle";
            me.gridConfig.border = true;
            me.gridConfig.region = 'center';
        }
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = this, index = -1, firstFind = me.nodeIndex == -1;
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
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
    updataTreeMemory: function (tree, checked) {
        var me = this;
        if (me.memory.eOpts == "selectionchange") { return; }
        var sd = tree.getSelectionModel().getSelection();
        if (sd.length > 0) {
            me.memory.FoucedNodeValue = sd[0].getPath();
            me.memory.IsMemo = checked;
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/Person/UpdataTreeMemory',
                async: true,
                params: { type: me.helpType, busstype: me.bussType, foucednodevalue: me.memory.FoucedNodeValue, ismemo: checked },
                success: function (response, opts) {
                }
            });
        }
    },
    getEmpName: function (name, assigntype) {
        if (assigntype == "0" || assigntype == "") { return name; }
        else { return name + "<font color='blue'>[" + (assigntype == "1" ? "兼" : "代") + "]</font>"; }
    },
    copyData: function (selectData, resultStore) {
        var me = this;
        var dataLen = selectData.length,
			index = resultStore.getCount(),
			tmpArr = me.currData.value,
			tmpData = [];
        for (var i = 0; i < dataLen; i++) {
            var sourceData = selectData[i].data;
            if (Ext.Array.indexOf(tmpArr, sourceData.cno) < 0) {
                me.currData.value.push(sourceData.cno);
                me.currData.text.push(sourceData.cname);
                tmpData.push(sourceData);
            }
        }
        resultStore.insert(index, tmpData);
        me.pageBar.updateInfo();
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
                var posIndex = Ext.Array.indexOf(me.currData.value, data[i].data.cno);
                Ext.Array.erase(me.currData.value, posIndex, 1);
                Ext.Array.erase(me.currData.text, posIndex, 1);
            }
        }
        me.pageBar.updateInfo();
    }
});

//返回值说明：1_员工，2_用户，3_联盟体，4_外部联系人，5_客户，6_UIC会员,7_分销商
Ext.define("Ext.ng.PersonHelp", {
    extend: 'Ext.window.Window',
    alias: 'widget.personhelp',
    title: '人员帮助',
    closable: true,
    resizable: false,
    modal: true,
    width: 700,
    height: 480,
    nodeIndex: -1,
    border: false,
    callback: null,  //点击确定后的回调函数,//1是员工，2是用户
    allowBank: true, //允许返回空值
    bussType: "all", //业务类型
    gridStore: null,
    defaultHeight: 50,
    radioItems: [1, 2, 3, 4, 5, 6], //员工、用户-角色、用户-用户组、自定义联系人、在线人员、外部人员
    layout: {
        type: 'border',
        padding: 4
    },
    initComponent: function () {
        var me = this;
        me.radioSelectValue = me.radioItems[0];
        me.initParam();
        var store = Ext.create('Ext.ng.JsonStore', {
            autoLoad: false,
            pageSize: 15,
            fields: [{
                name: 'cno',
                mapping: 'cno',
                type: 'string'
            }, {
                name: 'cname',
                mapping: 'cname',
                type: 'string'
            }, {
                name: 'assigntype',
                mapping: 'assigntype',
                defaultValue: '',
                type: 'string'
            }, {
                name: 'ctype',
                mapping: 'ctype',
                type: 'string'
            }],
            url: C_ROOT + 'SUP/Person/GetPersonList'
        }),
		pageBar = Ext.create('Ext.ng.PagingBar', {
		    store: store,
		    showRefresh: false,
		    displayMsg: '共 {2} 条数据&nbsp;&nbsp;&nbsp;已选 {3} 条数据',
		    updateInfo: function () {
		        var p = this;
		        var displayItem = p.child('#displayItem'),
					pageData = p.getPageData(), msg;
		        if (displayItem) {
		            if (p.store.getCount() === 0) {
		                msg = p.emptyMsg;
		            }
		            else {
		                msg = Ext.String.format(
								 p.displayMsg,
								 pageData.fromRecord,
								 pageData.toRecord,
								 pageData.total,
								 me.currData.value.length
						);
		            }
		            displayItem.setText(msg);
		        }
		    }
		}),
		resultStore = Ext.create('Ext.ng.JsonStore', {
		    fields: [{
		        name: 'cno',
		        mapping: 'cno',
		        type: 'string'
		    }, {
		        name: 'cname',
		        mapping: 'cname',
		        type: 'string'
		    }, {
		        name: 'assigntype',
		        mapping: 'assigntype',
		        defaultValue: '',
		        type: 'string'
		    }, {
		        name: 'ctype',
		        mapping: 'ctype',
		        type: 'string'
		    }]
		}),
		empTree = Ext.create('Ext.ng.TreePanel', {
		    region: 'center',
		    border: false,
		    autoScroll: true,
		    rootVisible: false,
		    useArrows: true,
		    width: 180,
		    minSize: 180,
		    maxSize: 180,
		    treeFields: [{ name: 'text', type: 'string' },
						 { name: 'id', type: 'string' },
						 { name: 'pid', type: 'string' },
						 { name: 'relatindex', type: 'string' },
						 { name: 'bopomofo', type: 'string' }],
		    url: C_ROOT + 'SUP/Person/LoadTree?ctype=' + me.getTreeType(),
		    listeners: {
		        selectionchange: function (m, selected, eOpts) {
		            if (selected.length > 0 && !selected[0].data.root) {
		                me.memory.eOpts = "selectionchange";
		                me.searchData({
		                    leaf: selected[0].data.leaf,
		                    ocode: selected[0].data.id,
		                    relatindex: selected[0].data.relatindex,
		                    gettype: me.getListType(),
		                    group: me.getTreeType()
		                });
		                me.getDockedItems(":last")[0].query("checkboxfield:first")[0].setValue(me.memory.IsMemo && me.memory.FoucedNodeValue == selected[0].getPath());
		                me.memory.eOpts = "";
		            }
		        },
		        itemclick: function (m, record, item, index, e, eOpts) {
		            me.nodeIndex = -1;
		        },
		        load: function (m, node, records, successful, eOpts) {
		            empTree.selectPath(empTree.getRootNode().getPath());
		            var firstChild = empTree.getRootNode().firstChild;
		            if (!firstChild) {
		                me.gridStore && me.gridStore.removeAll();
		                me.getDockedItems(":last")[0].query("checkboxfield:first")[0].setValue(false);
		                return;
		            }
		            Ext.Ajax.request({
		                url: C_ROOT + 'SUP/Person/GetTreeMemoryInfo',
		                params: { type: me.helpType, busstype: me.bussType },
		                success: function (response, opts) {
		                    me.memory = Ext.JSON.decode(response.responseText);
		                    if (!Ext.isEmpty(me.memory.FoucedNodeValue)) {
		                        empTree.selectPath(me.memory.FoucedNodeValue, null, null, function () {
		                            if (Ext.isIE) {
		                                window.setTimeout(function () {
		                                    var selectNode = m.view.body.query("tr." + m.view.selectedItemCls);
		                                    if (selectNode) {
		                                        selectNode[0].scrollIntoView(true);
		                                    }
		                                }, 500);
		                            }
		                        });
		                    }
		                    else {
		                        empTree.selectPath(empTree.getRootNode().firstChild.getPath());
		                    }
		                }
		            });
		        }
		    }
		}),
		left = {
		    title: "人力资源树",
		    autoScroll: false,
		    collapsible: true,
		    split: true,
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
		            emptyText: '输入关键字，定位树节点',
		            margin: '2 0 2 2',
		            enableKeyEvents: true,
		            listeners: {
		                'keydown': function (el, e, eOpts) {
		                    if (e.getKey() == e.ENTER) {
		                        me.findNodeByFuzzy(empTree, el.getValue());
		                        el.focus();
		                        return false;
		                    }
		                    else {
		                        me.nodeIndex = -1;
		                    }
		                }
		            }
		        }, { region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5', handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(empTree, el.getValue()); el.focus(); } }]
		    }, empTree]
		},
		gridLeft = Ext.create('Ext.ng.GridPanel', {
		    columnWidth: .5,
		    height: 310,
		    store: store,
		    autoScroll: true,
		    columnLines: true,
		    border: false,
		    selModel: { mode: "SIMPLE" },
		    columns: [{
		        header: '编号',
		        flex: 1,
		        sortable: false,
		        menuDisabled: true,
		        draggable: false,
		        dataIndex: 'cno'
		    }, {
		        header: '姓名',
		        flex: 1,
		        sortable: false,
		        menuDisabled: true,
		        draggable: false,
		        dataIndex: 'cname',
		        renderer: function (value, parm, record) {
		            if (record.data.ctype == "1") {
		                return me.getEmpName(value, record.data.assigntype);
		            }
		            return value;
		        }
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
		    height: 310,
		    autoScroll: true,
		    columnLines: true,
		    border: false,
		    selModel: { mode: "SIMPLE" },
		    columns: [{
		        header: '编号',
		        flex: 1,
		        sortable: false,
		        menuDisabled: true,
		        draggable: false,
		        dataIndex: 'cno'
		    }, {
		        header: '姓名',
		        flex: 1,
		        sortable: false,
		        menuDisabled: true,
		        draggable: false,
		        dataIndex: 'cname'
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
		    height: 320,
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
		    xtype: '',
		    height: 27,
		    border: false,
		    layout: 'border',
		    items: [{
		        region: 'east',
		        xtype: "combobox",
		        store: Ext.create('Ext.ng.JsonStore', {
		            autoLoad: false,
		            pageSize: 50,
		            fields: [
				{ name: 'ccode', mapping: 'ccode' },
				{ name: 'cname', mapping: 'cname' }
				],
		            proxy: {
		                type: 'ajax',
		                url: C_ROOT + 'SUP/Person/GetEmpStatus',
		                reader: {
		                    type: 'json',
		                    root: 'Record',
		                    totalProperty: 'totalRows'
		                }
		            },
		            listeners: {
		                load: function (m, records, successful, eOpts) {
		                    m.insert(0, [{ ccode: '', cname: '&nbsp;' }]);
		                }
		            }
		        }),
		        width: 100,
		        labelWidth: 28,
		        allowBlank: true,
		        editable: false,
		        fieldLabel: '',
		        emptyText: '状态',
		        valueField: 'ccode',
		        displayField: 'cname',
		        listeners: {
		            select: function (combo, records, eOpts) {
		                var code = combo.getValue() || records[0].data.emptype;
		                if (Ext.isEmpty(code)) {
		                    combo.clearValue();
		                }
		                me.searchData({ "empstatus": code });
		            }
		        }
		    }, {
		        region: 'east',
		        xtype: "combobox",
		        store: Ext.create('Ext.ng.JsonStore', {
		            autoLoad: false,
		            fields: [
				{ name: 'emptype', mapping: 'emptype' },
				{ name: 'typename', mapping: 'typename' }
				],
		            proxy: {
		                type: 'ajax',
		                url: C_ROOT + 'SUP/Person/GetEmpType',
		                reader: {
		                    type: 'json',
		                    root: 'Record',
		                    totalProperty: 'totalRows'
		                }
		            },
		            listeners: {
		                load: function (m, records, successful, eOpts) {
		                    m.insert(0, [{ emptype: '', typename: '&nbsp;' }]);
		                }
		            }
		        }),
		        width: 100,
		        labelWidth: 28,
		        allowBlank: true,
		        editable: false,
		        fieldLabel: '',
		        emptyText: '类型',
		        valueField: 'emptype',
		        displayField: 'typename',
		        style: { marginLeft: '10px', marginRight: '10px' },
		        listeners: {
		            select: function (combo, records, eOpts) {
		                var code = combo.getValue() || records[0].data.emptype;
		                if (Ext.isEmpty(code)) {
		                    combo.clearValue();
		                }
		                me.searchData({ "emptype": code });
		            }
		        }
		    }, {
		        region: 'east',
		        width: 160,
		        fieldLabel: '',
		        emptyText: '输入编号/姓名，回车查询',
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
		                    var param = { "searchtxt": "" };
		                    if (!Ext.isEmpty(key)) {
		                        if (/\d+/.test(key)) {
		                            param = { "searchtxt": "cno like '%" + key + "%'" };
		                        }
		                        else if (/\W+/.test(key)) {
		                            param = { "searchtxt": "cname like '%" + key + "%'" };
		                        }
		                        else {
		                            param = { "searchtxt": "(cno like '%" + key + "%' or cname like '%" + key + "%' or bopomofo like '%" + key + "%')" };
		                        }
		                    }
		                    me.searchData(param);
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
        me.gridStore = store;
        me.tree = empTree;
        me.pageBar = pageBar;
        me.items = [{
            region: 'north', height: 30, layout: 'border', border: true, style: 'margin-bottom: 4px;',
            items: [
            {
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
        }, { region: 'center', layout: 'border', border: false, items: [left, top, center] }];
        me.buttons = [{
            xtype: 'checkboxfield',
            boxLabel: '记忆树选中状态',
            handler: function (chk) {
                me.updataTreeMemory(empTree, chk.getValue());
            }
        }, {
            xtype: 'checkboxfield',
            boxLabel: '显示兼职',
            handler: function (chk) {
                me.searchData({ "partmark": chk.getValue() ? "1" : "" });
            }
        }, {
            xtype: 'checkboxfield',
            boxLabel: '显示代理',
            handler: function (chk) {
                me.searchData({ "proymark": chk.getValue() ? "1" : "" });
            }
        }, {
            xtype: 'checkboxfield',
            boxLabel: '显示禁用',
            hidden: true,
            handler: function (chk) {
                me.searchData({ "lgsign": chk.getValue() ? "1" : "" });
            }
        }, '->', {
            text: '确认',
            handler: function () {
                if (me.callback) {
                    var data = me.currData, tmpdata = {};
                    if (data.value.length > 0) {
                        tmpdata = { cno: data.value.join(","), cname: data.text.join(",") };
                    }
                    else {
                        if (me.allowBank) {
                            tmpdata = { cno: '', cname: '' };
                        }
                        else {
                            Ext.MsgBox.sliDown("提示", "未选择数据", me.el.dom, 200);
                            return;
                        }
                    }
                    me.close();
                    me.callback(tmpdata);
                }
            }
        }, {
            text: '取消',
            margin: '0 5 8 0',
            handler: function () {
                me.close();
            }
        }];
        me.callParent();
        me.show();
        me.treePanel = me.items.items[1].items.items[0];
    },
    initParam: function () {
        var me = this, rItems = [],
            rLabels = ['', '员工-组织', '用户-角色', '用户-用户组', '自定义联系人', '在线人员', '外部人员'];
        me.memory = {};
        me.gridConfig = {};
        me.currData = { value: [], text: [] };
        me.helpType = "person_" + me.getTreeType();
        me.width = 700;
        me.height = 473;

        var tmpArr = me.radioItems;
        for (var i = 0; i < tmpArr.length; i++) {
            rItems.push({ boxLabel: rLabels[tmpArr[i]], name: 'rb', inputValue: tmpArr[i], checked: i == 0 });
        }
        me.radioItems = rItems;
    },
    getListType: function () {
        var listType = ['', 'emplist', 'userlist', 'userlist', 'selfgrouplist', 'onlinelist', 'outerlist'];
        return listType[Number(this.radioSelectValue)];
    },
    getTreeType: function () {
        var treeType = ['', 'hr', 'actor', 'ugroup', 'selfgroup', 'online', 'outer'];
        return treeType[Number(this.radioSelectValue)];
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var me = this, index = -1, firstFind = me.nodeIndex == -1;
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MsgBox.sliDown("提示", "没有匹配的树节点", me.el.dom, 200);
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
    updataTreeMemory: function (tree, checked) {
        var me = this;
        if (me.memory.eOpts == "selectionchange") { return; }
        var sd = tree.getSelectionModel().getSelection();
        if (sd.length > 0) {
            me.memory.FoucedNodeValue = sd[0].getPath();
            me.memory.IsMemo = checked;
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/Person/UpdataTreeMemory',
                async: true,
                params: { type: me.helpType, busstype: me.bussType, foucednodevalue: me.memory.FoucedNodeValue, ismemo: checked },
                success: function (response, opts) {
                }
            });
        }
    },
    getEmpName: function (name, assigntype) {
        if (assigntype == "0" || assigntype == "") { return name; }
        else { return name + "<font color='blue'>[" + (assigntype == "1" ? "兼" : "代") + "]</font>"; }
    },
    copyData: function (selectData, resultStore) {
        var me = this;
        var dataLen = selectData.length,
			index = resultStore.getCount(),
			tmpArr = me.currData.value,
			tmpData = [];
        for (var i = 0; i < dataLen; i++) {
            var sourceData = selectData[i].data;
            if (Ext.Array.indexOf(tmpArr, sourceData.ctype + '|' + sourceData.cno) < 0) {
                me.currData.value.push(sourceData.ctype + '|' + sourceData.cno);
                me.currData.text.push(sourceData.cname);
                if (tmpArr.length < me.defaultHeight) {
                    tmpData.push(sourceData);
                }
            }
        }
        resultStore.insert(index, tmpData);
        me.pageBar.updateInfo();
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
                var posIndex = Ext.Array.indexOf(me.currData.value, data[i].data.ctype + '|' + data[i].data.cno);
                Ext.Array.erase(me.currData.value, posIndex, 1);
                Ext.Array.erase(me.currData.text, posIndex, 1);
            }
        }
        me.pageBar.updateInfo();
    },
    rbChange: function (value) {
        var me = this,
            cmb = me.query("combobox"),
            cbx = me.getDockedItems(":last")[0].query("checkboxfield");

        me.radioSelectValue = value;
        me.helpType = "person_" + me.getTreeType();
        me.treePanel.setTitle(['', '人力资源树', '角色', '用户组', '自定义联系人组', '', '外部分类'][Number(value)]);
        me.treePanel.show();
        cbx[0].show();
        if (value == "1") {
            cmb[0].show();
            cmb[1].show();

            cbx[1].show();
            cbx[2].show();
            cbx[3].hide();
        }
        else if (value == "2" || value == "3") {
            cmb[0].hide();
            cmb[1].hide();

            cbx[1].hide();
            cbx[2].hide();
            cbx[3].show();
        }
        else {
            cmb[0].hide();
            cmb[1].hide();

            cbx[1].hide();
            cbx[2].hide();
            cbx[3].hide();
        }

        if (value == "5") {
            me.treePanel.hide();
            cbx[0].hide();
            me.searchData({
                leaf: true,
                ocode: "",
                relatindex: "",
                gettype: me.getListType(),
                group: me.getTreeType()
            });
        }
        else {
            me.tree.store.proxy.url = C_ROOT + 'SUP/Person/LoadTree?ctype=' + me.getTreeType();
            me.tree.store.load();
        }
    }
});
// #endregion

//#region ngUsefulComboBox常用语组件
Ext.define('Ext.ng.UsefulComboBox', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.ngUsefulComboBox', //别名,可通过设置xtype构建 
    controlid: '',
    hideTrigger: true,
    fieldLabel: '',
    queryMode: 'local',
    displayField: 'names',
    valueField: 'names',
    initComponent: function () {
        this.callParent();
        this.changeValue = false;
        this.canUpdate = true;
        this.on("change", this.LoadStore);
        this.on("blur", this.SaveNames);
        this.on("afterrender", this.Afterrender);
    },
    LoadStore: function () {
        var me = this;
        var data = me.getStore().proxy.reader.jsonData;
        Ext.Ajax.request({
            url: C_ROOT + 'SUP/UsefulControl/GetList',
            params: { 'controlid': me.controlid, 'names': me.getValue() },
            async: false,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                me.AllStore = Ext.create('Ext.data.Store', {
                    fields: ['names']
                });
                me.AllStore.loadRawData(resp);
                me.bindStore(me.AllStore);
                me.changeValue = true;
            }
        });
    },
    SaveNames: function () {
        var me = this;
        var names = me.getValue();
        if (me.changeValue == true) {
            me.changeValue = false;
            me.UpdateNames(names);
        }
    },
    UpdateNames: function (names) {
        var me = this;
        if (names != null && names.length > 0 && me.canUpdate == true) {
            Ext.Ajax.request({
                url: C_ROOT + 'SUP/UsefulControl/Update',
                params: { 'controlid': me.controlid, 'names': names },
                async: false,
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                }
            });
        }
    },
    Afterrender: function () {
        if (this.controlid == '') {
            throw '请设置UsefulComboBox controlid';
        }
    },
    userSetReadOnly: function (flag) {
        this.setReadOnly(flag);
        this.preventMark = flag;
        var input = this.el.down('input') || this.el.down('textarea');
        if (flag) {
            input.setStyle({ backgroundImage: 'none' });
        }
        else {
            input.setStyle({ backgroundColor: 'white' });
        }
        //处理控件变短
        var errmsg = this.el.down('div.x-form-error-msg')
        if (errmsg) {
            errmsg.up('td').setStyle({ display: 'none' });
        }
    }
});
//#endregion

//#region ngUsefulTextArea常用语组件
Ext.define('Ext.ng.UsefulTextArea', {
    extend: 'Ext.container.Container',
    alias: 'widget.ngUsefulTextArea', //别名,可通过设置xtype构建 
    controlid: '',
    emptyText: '按下回车键显示常用语',
    height: 60,
    layout: 'absolute',
    initComponent: function () {
        this.callParent();
        if (this.controlid == '') {
            throw '请设置ngUsefulTextArea controlid';
        }
        //this.items.first().controlid = this.controlid;
        this.items.get('ngUsefulComboBox').controlid = this.controlid;
        this.items.get('ngUsefulComboBox').canUpdate = false;
        this.items.get('ngTextArea').height = this.height;
        this.items.get('ngTextArea').IsENTER = false;
        this.items.get('ngTextArea').emptyText = this.emptyText;
    },
    userSetReadOnly: function (val) {
        this.items.get('ngTextArea').userSetReadOnly(val);
        this.items.get('ngUsefulComboBox').userSetReadOnly(val);
    },
    getValue: function () {
        return this.items.get('ngTextArea').getValue();
    },
    setValue: function (val) {
        this.items.get('ngTextArea').setValue(val);
    },
    items: [
        {
            controlid: '',
            xtype: 'ngUsefulComboBox',
            itemId: 'ngUsefulComboBox',
            editable: false,
            hideTrigger: true,
            enableKeyEvents: true,
            fieldStyle: 'border:0;color:transparent;background:transparent',
            style: 'z-index:-1;',
            listeners: {
                afterrender: function (me, eOpts) {
                    me.setY(me.nextSibling().height - 11);
                },
                collapse: function (me, eOpts) {
                    var textareafield = me.nextSibling();
                    var task = new Ext.util.DelayedTask(function () {
                        textareafield.focus();
                        textareafield.IsENTER = false;
                    });
                    task.delay(100);
                },
                select: function (me, records, eOpts) {
                    var val = me.getValue();
                    val = val.replace(/\\n/g, "\n");//替换全部换行
                    var textareafield = me.nextSibling();
                    textareafield.setValue(val);
                },
                focus: function (me, The, eOpts) {
                    var textareafield = me.nextSibling();
                    textareafield.focus();
                },
                keypress: function (me, e, eOpts) {
                    var textareafield = me.nextSibling();
                    textareafield.focus();
                }
            }
        },
        {
            xtype: 'ngTextArea',//textareafield ngTextArea
            itemId: 'ngTextArea',
            region: 'center',
            height: 60,
            enableKeyEvents: true,
            listeners: {
                blur: function (me, The, eOpts) {
                    var val = me.getValue();
                    val = val.replace(/\n/g, "\\n");//替换全部换行
                    var ngUsefulComboBox = me.previousSibling();
                    ngUsefulComboBox.canUpdate = true;
                    if (!me.IsENTER) {
                        ngUsefulComboBox.UpdateNames(val);
                    }
                    ngUsefulComboBox.canUpdate = false;
                    me.IsENTER = false;
                },
                Keydown: function (me, e, eOpts) {
                    if (e.getKey() == e.ENTER) {
                        me.IsENTER = true;
                        var val = me.getValue();
                        val = val.replace(/\n/g, "\\n");//替换全部换行
                        var ngUsefulComboBox = me.previousSibling();
                        ngUsefulComboBox.setValue(val);
                    }
                },
                keyup: function (me, e, eOpts) {
                    if (e.getKey() == e.ENTER) {
                        var ngUsefulComboBox = me.previousSibling();
                        ngUsefulComboBox.onTriggerClick();
                    }
                }
            }
        }
    ]
});
//#endregion

//#region imp数据选择帮助
Ext.define('Ext.ng.ImpCommonHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.ngImpHelp'],
    pageSize: 10,
    minChars: 1, //定义输入最少多少个字符的时候获取数据
    helpType: 'simple', //默认是simple,自定义界面：rich
    helpWidth: 800, //帮助宽度
    helpHeight: 500, //帮助高度
    showAutoHeader: false,
    //outFilter: {}, //外部查询条件,精确条件
    //likeFilter: {}, //外部模糊查询条件，like条件
    selectMode: 'Single', //multiple  
    ORMMode: false,
    needBlankLine: false,
    //forceSelection: true,
    autoSelect: false, //不要自动选择第一行
    enableKeyEvents: true, //允许key事件
    selectOnFoucus: true,
    typeAhead: true, //延时查询
    typeAheadDelay: 500, //延迟500毫秒，默认是250
    //valueNotFoundText: 'Select a Country!!...',
    pagedata: {},//页面数据
    maskdata: {}, //数据格式化
    datasourceData: {},
    queryMode: 'remote',
    triggerAction: 'all', //'query'   
    helpMaximizable: true, // 最大化支持
    initComponent: function () {
        //
        var me = this;
        this.callParent();
        this.mixins.base.initComponent.call(me); //与callParent方法不可调换

        //me.tpl = '<div><table width="100%" ><tr><th class="x-column-header-inner x-column-header-over" >代码</th><th class="x-column-header-inner x-column-header-over">名称</th></tr><tpl for="."><tr class="x-boundlist-item"><td>{' + this.valueField + '}</td><td>{' + this.displayField + '}<td></tr></tpl></table></div>';
        if (Ext.isEmpty(me.helpid) || Ext.isEmpty(me.displayField) || Ext.isEmpty(me.valueField)) return;

        if (me.editable) {
            if (me.listFields && me.listHeadTexts) {

                var listheaders = '';
                var listfields = '';

                var heads = me.listHeadTexts.split(','); //列头 
                var fields = me.listFields.split(','); //所有字段              

                var modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp //fields[i]
                    });

                }

                if (me.showAutoHeader) {

                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    listfields += '<td>{' + temp + '}</td>';
                }

                var temp;
                if (me.showAutoHeader) {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                } else {
                    temp = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
                }
                me.tpl = temp;


            }
            else {
                //me.initialListTemplate(); //初始化下拉列表样式 
                var tempfield = me.valueField.split('.');
                var valueField;
                if (tempfield.length > 1) {
                    valueField = tempfield[1]; //去掉表名
                }
                else {
                    valueField = me.valueField;
                }

                var dfield = me.displayField.split('.');
                var displayField;
                if (dfield.length > 1) {
                    displayField = dfield[1]; //去掉表名
                }
                else {
                    displayField = me.displayField;
                }

                var modelFields = [{
                    name: valueField,
                    type: 'string',
                    mapping: valueField
                }, {
                    name: displayField,
                    type: 'string',
                    mapping: displayField
                }]

                var listfields = '<td>{' + valueField + '}</td>';
                listfields += '<td>{' + displayField + '}</td>';
                me.tpl = '<div><table width="100%" style="border-spacing:0px;" ><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';

            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10,
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'SUP/CommonHelp/GetHelpList?helpid=' + me.helpid + '&ORMMode=' + me.ORMMode,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });

            me.bindStore(store);

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            });

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }
        }


        me.addEvents('helpselected'); //定义值被选完的事件
        me.addEvents('firstrowloaded');

        me.on('select', function (combo, records, eOpts) {

            var theField;

            //构建
            if (me.listFields) {
                theField = [];
                var temp = me.listFields.split(',');
                for (var i = 0; i < temp.length; i++) {
                    theField.push(temp[i]);


                }
            }
            else {
                theField = [me.valueField, me.displayField];
            }

            Ext.define('themodel', {
                extend: 'Ext.data.Model',
                fields: theField
            });

            //去掉表名
            var myValueFiled;
            var myDisplayField;
            var temp = me.valueField.split('.');
            if (temp.length > 1) {
                myValueFiled = temp[1];
            } else {
                myValueFiled = me.valueField;
            }

            temp = me.displayField.split('.');
            if (temp.length > 1) {
                myDisplayField = temp[1];
            } else {
                myDisplayField = me.displayField;
            }


            var code = combo.getValue() || records[0].data[myValueFiled];
            var name = combo.getRawValue() || records[0].data[myDisplayField];

            if (Ext.isEmpty(code)) {
                name = '';
            }

            var obj = new Object();
            if (me.isInGrid) {//嵌在grid中
                obj[me.valueField] = name; //欺骗,grid那边显示有问题
            } else {
                obj[me.valueField] = code;
            }
            if (me.displayFormat) {
                obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
            } else {
                obj[me.displayField] = (name === '&nbsp;') ? '' : name;
            }

            var valuepair = Ext.ModelManager.create(obj, 'themodel');
            me.setValue(valuepair); //必须这么设置才能成功

            //debugger;
            var pobj = new Object();
            pobj.code = code;
            pobj.name = name;
            pobj.type = 'autocomplete';
            //pobj.data = records[0].data;
            pobj.data = {};
            for (var i = 0; i < theField.length; i++) {
                var temp = theField[i].split('.');
                if (temp.length > 1) {
                    var str = records[0].data[temp[1]];
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
                else {
                    var str = records[0].data[theField[i]]
                    pobj.data[theField[i]] = (str === '&nbsp;') ? '' : str;
                }
            }

            me.fireEvent('helpselected', pobj);

        });

        me.on('expand', function (field, opt) {

            //刷新按钮去掉
            var autoPagingbar = me.getPicker().pagingToolbar;
            autoPagingbar.items.items[10].hide();
            autoPagingbar.items.items[9].hide();

        });

        me.on('keydown', function (combo, e, eOpts) {


            if (me.isExpanded) {

                //回车
                if (e.keyCode == Ext.EventObject.ENTER) {
                    if (me.picker.el.query('.' + me.picker.overItemCls).length > 0) return false;
                    me.onTriggerClick();
                }

                //翻页
                switch (e.keyCode) {
                    case Ext.EventObject.PAGE_UP:
                        me.getPicker().pagingToolbar.movePrevious();
                        return true;
                    case Ext.EventObject.PAGE_DOWN:
                        me.getPicker().pagingToolbar.moveNext();
                        return true;
                    case Ext.EventObject.HOME:
                        me.getPicker().pagingToolbar.moveFirst();
                        return true;
                    case Ext.EventObject.END:
                        me.getPicker().pagingToolbar.moveLast();
                        return true;
                }

                if (!Ext.isEmpty(me.getValue())) {
                    if (e.keyCode == Ext.EventObject.BACKSPACE || e.keyCode == Ext.EventObject.DELETE) {

                    }
                }
            }
        });
    },
    initialListTemplate: function (store) {
        var me = this;

        var allfield;
        var headText;
        var initTpl;
        var template;

        initTpl = function () {

            var modelFields;
            var gridColumns;

            var listheaders = '';
            var listfields = '';

            if (me.helpType === 'rich') {//用户自定义界面的模板 

                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;

                if (me.showAutoHeader) {
                    for (var i = 0; i < gridColumns.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + gridColumns[i].header + '</th>';
                    }
                }

                for (var i = 0; i < modelFields.length; i++) {
                    listfields += '<td>{' + modelFields[i]['name'] + '}</td>';
                }

            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头 

                if (me.showAutoHeader) {
                    for (var i = 0; i < heads.length; i++) {
                        listheaders += '<th class="x-column-header-inner x-column-header-over">' + heads[i] + '</th>';
                    }
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: temp, //fields[i],
                        type: 'string',
                        mapping: temp//fields[i]
                    });

                }
            }


            for (var i = 0; i < heads.length; i++) {

                var tempfield = fields[i].split('.');
                var temp;
                if (tempfield.length > 1) {
                    temp = tempfield[1]; //去掉表名
                }
                else {
                    temp = fields[i];
                }

                listfields += '<td>{' + temp + '}</td>';
            }

            var store = Ext.create('Ext.data.Store', {
                pageSize: 10, //这个决定页大小                
                fields: modelFields,
                proxy: {
                    type: 'ajax',
                    url: C_ROOT + 'IMP3/Engine/GetHelpList?schemephid=' + me.schemephid + '&sourcephid=' + me.sourcephid,
                    reader: {
                        type: 'json',
                        root: 'Record',
                        totalProperty: 'totalRows'
                    }
                }
            });
            //me.bindStore(store); //动态绑定store
            me.store = store;

            //只能在这里写事件才能触发到
            store.on('beforeload', function (store) {

                Ext.apply(store.proxy.extraParams, { 'page': store.currentPage - 1 }); //修改pageIndex为从0开始
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                if (me.likeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(me.likeFilter) });
                }
                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });
                }

            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

            var temp;
            if (me.showAutoHeader) {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tr>' + listheaders + '</tr><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            else {
                temp = '<div><table width="100%" style="border-spacing:0px;"><tpl for="."><tr class="x-boundlist-item">' + listfields + '</tr></tpl></table></div>';
            }
            me.tpl = temp;

        };

        var url = C_ROOT + 'IMP3/Engine/Engine/GetHelpInfo?schemephid=' + me.schemephid + '&sourcephid=' + me.sourcephid;



        Ext.Ajax.request({
            url: url,
            callback: initTpl,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {

                    if (me.helpType === 'rich') {
                        //title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        //title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });



    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly || arguments.length == 3) return; //arguments.length == 3，输入框上点击           

        if (Ext.isEmpty(me.sourcephid)) return;
        //
        var title;
        var allfield;
        var headText;
        var ShowHelp;
        var template;

        ShowHelp = function () {

            var queryItems;
            var modelFields;
            var gridColumns;

            if (me.helpType === 'rich') {//用户自定义界面的模板            
                queryItems = template.Template.QueryItems;
                modelFields = template.Template.Model.fields;
                gridColumns = template.Template.GridColumns;
            }
            else {

                if (!allfield) return;

                var fields = allfield.split(','); //所有字段
                var heads = headText.split(','); //列头


                queryItems = new Array();
                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp = fields[i];;

                    queryItems.push({
                        xtype: 'textfield',
                        fieldLabel: heads[i],
                        name: temp //fields[i]
                        //anchor: '95%'
                    });
                }

                modelFields = new Array();
                for (var i = 0; i < fields.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    modelFields.push({
                        name: fields[i], //不去掉表名
                        type: 'string',
                        mapping: temp
                    });
                }

                gridColumns = new Array();

                for (var i = 0; i < heads.length; i++) {

                    var tempfield = fields[i].split('.');
                    var temp;
                    if (tempfield.length > 1) {
                        temp = tempfield[1]; //去掉表名
                    }
                    else {
                        temp = fields[i];
                    }

                    gridColumns.push({
                        header: heads[i],
                        flex: 1,
                        minWidth: 90,
                        //sortable: true,
                        dataIndex: fields[i],
                        renderer: function (value, metaData, record) {                            
                            try {
                                var mask;
                                var datasource;
                                try {
                                    mask = me.maskdata[metaData.column.dataIndex];
                                }
                                catch (e) {
                                    mask = null;
                                }
                                try {
                                    datasource = me.datasourceData[metaData.column.dataIndex];
                                }
                                catch (e) {
                                    datasource = null;
                                }
                                if (mask) {
                                    if (mask.indexOf("FormatDate") == 0) {
                                        // 截取第二个参数并去除收尾空格
                                        var start = mask.indexOf(',') + 1;
                                        var end = mask.lastIndexOf(')');
                                        var attr = mask.slice(start, end).replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                                        var format = attr.slice(1, -1);
                                        return Ext.Date.format(new Date(value), format);
                                    }
                                    if (mask.indexOf("ToPercent") == 0) {
                                        var digits = 0;
                                        //存在第二个参数
                                        if (mask.indexOf(',') > 0) {
                                            // 截取第二个参数并去除收尾空格
                                            var start = mask.indexOf(',') + 1;
                                            var end = mask.lastIndexOf(')');
                                            var digits = mask.slice(start, end).replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                                        }
                                        return $IMPMaskPublicMethod("ToPercent")(value, digits);
                                    }
                                    if (mask.indexOf("FormatNumber") == 0) {
                                        var start = mask.indexOf(',') + 1;
                                        var end = mask.lastIndexOf(')');
                                        var attr = mask.slice(start, end).replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                                        var format = attr.slice(1, -1);
                                        return Ext.util.Format.number(Number(value), format);
                                    }                                    
                                    var result = mask.replace("{data}", value);
                                    return eval(result);
                                    
                                }
                                else if (datasource) {
                                    var datas = datasource.split(';');
                                    for (var i = 0; i < datas.length; i++) {
                                        if (datas[i].split('|')[0] == value) {
                                            return datas[i].split('|')[1];
                                        }
                                    }
                                }
                            }
                            catch (err) {
                                return value;
                            }
                            return value;
                        }
                    });
                }
            }

            var toolbar = Ext.create('Ext.Toolbar', {
                region: 'north',
                border: false,
                //split: true,
                height: 26,
                minSize: 26,
                maxSize: 26,
                items: [
								{
								    id: "help_query",
								    text: "查询",
								    iconCls: 'add'
								},
								{
								    id: 'help_show',
								    text: '显示',
								    iconCls: 'icon-unfold',
								    handler: function () {
								        if (this.iconCls == 'icon-unfold') {
								            this.setIconCls('icon-fold');
								            this.setText("隐藏");
								            querypanel.show();
								        } else {
								            this.setIconCls('icon-unfold');
								            this.setText("显示");
								            querypanel.hide();
								        }
								    }
								},
                                {
                                    id: 'help_clear',
                                    text: '清空',
                                    iconCls: "icon-Clear",
                                    handler: function () {
                                        querypanel.getForm().reset();
                                    }
                                },
								 "->",
							   {
							       id: "help_close",
							       text: "关闭",
							       iconCls: 'cross'
							   }
                ]
            });

            querypanel = Ext.create('Ext.ng.TableLayoutForm', {
                region: 'north',
                hidden: true,
                bodyStyle: 'padding:3px',
                fields: queryItems,
                maxHeight: 150,
                autoScroll: true
            })

            Ext.define('model', {
                extend: 'Ext.data.Model',
                fields: modelFields
            });

            var store = Ext.create('Ext.ng.JsonStore', {
                model: 'model',
                pageSize: 20,
                autoLoad: true,
                url: C_ROOT + 'IMP/Engine/Engine/GetHelpList?schemephid=' + me.schemephid + '&sourcephid=' + me.sourcephid + '&sort=' + me.primaryKey
            });

            //store.load();//这里load，IE的界面会扭掉

            var pagingbar = Ext.create('Ext.ng.PagingBar', {
                store: store,
                displayMsg: '共 {2} 条数据',
                //emptyMsg: "没有数据",
                beforePageText: "第",
                afterPageText: "/{0} 页",
                style: { backgroundImage: 'none', backgroundColor: 'transparent' }
            });

            //var pagingbar = Ext.create('Ext.ng.PagingBar', {
            //    store: store
            //});

            var selModel = Ext.create('Ext.selection.CheckboxModel');

            var winItems = [];

            winItems.push(toolbar);
            winItems.push(querypanel);

            var grid;
            var resultStore = Ext.create('Ext.ng.JsonStore', {
                model: 'model'
            });
            var winBtns = [];
            var win;
            //多选
            if (me.selectMode === 'Multi') {

                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    selModel: { mode: "SIMPLE" },
                    //selModel: selModel, //多选
                    columnLines: true,
                    columns: gridColumns
                    //bbar: pagingbar
                });
                grid.on('itemdblclick', function (grid, record, item) {
                    var data = [];
                    data = grid.getSelectionModel().getSelection();
                    if (data.length == 0) {//直接双击取不到选中的行
                        data.push(record);
                    }
                    me.copyData(data, resultStore);
                }, this);

                var resultGrid = Ext.create('Ext.ng.GridPanel', {
                    region: 'east',
                    //frame: true,
                    width: 235,
                    border: false,
                    store: resultStore,
                    selModel: { mode: "SIMPLE" }, //多选
                    columnLines: true,
                    columns: gridColumns
                });

                var btnPanel = Ext.create('Ext.panel.Panel', {
                    region: 'east',
                    width: 80,
                    layout: 'absolute',
                    border: false,
                    frame: true,
                    padding: 0,
                    style: { borderColor: 'transparent', backgroundColor: 'transparent' }, //backgroundColor: 'transparent !important',marginTop: '22px',
                    items: [{
                        xtype: 'button',
                        name: 'addSelect',
                        text: '&gt;',
                        x: 9,
                        y: 120,
                        width: 60,
                        handler: Ext.bind(function () {
                            var data = grid.getSelectionModel().getSelection();
                            me.copyData(data, resultStore);
                        })
                    }, {
                        xtype: 'button',
                        name: 'selectAll',
                        text: '&gt;&gt;',
                        x: 9,
                        y: 150,
                        width: 60,
                        handler: Ext.bind(function () {
                            var data = store.data.items;
                            me.copyData(data, resultStore);
                        })
                    }, {
                        xtype: 'button',
                        name: 'removeSelect',
                        text: '&lt;',
                        x: 9,
                        y: 180,
                        width: 60,
                        handler: Ext.bind(function () {
                            var data = resultGrid.getSelectionModel().getSelection();
                            resultStore.remove(data);
                        })
                    }, {
                        xtype: 'button',
                        name: 'removeAll',
                        text: '&lt;&lt;',
                        x: 9,
                        y: 210,
                        width: 60,
                        handler: Ext.bind(function () {
                            resultStore.removeAll();
                        })
                    }]
                });

                var panel = Ext.create('Ext.panel.Panel', {
                    region: 'center',
                    //frame: true,
                    border: false,
                    layout: 'border',
                    items: [grid, btnPanel, resultGrid]
                });
                winItems.push(panel);

                winBtns = [pagingbar, '->', { text: '确定', handler: function () { me.btnOk(me, resultStore, win); } }, { text: '取消', handler: function () { win.close(); } }];

            }
            else {//单选
                grid = Ext.create('Ext.ng.GridPanel', {
                    region: 'center',
                    frame: true,
                    border: false,
                    store: store,
                    //autoScroll:true,
                    columnLines: true,
                    columns: gridColumns,
                    bbar: pagingbar
                });
                grid.on('itemdblclick', function () {
                    var data = grid.getSelectionModel().getSelection();

                    if (data.length > 0) {

                        var valarr = me.valueField.split('.');
                        var valtemp = me.valueField;
                        if (valarr.length > 1) {
                            valtemp = valarr[1];
                        }

                        var namearr = me.displayField.split('.');
                        var nametemp = me.displayField;
                        if (namearr.length > 1) {
                            nametemp = namearr[1];
                        }

                        var code = data[0].get(valtemp);
                        var name = data[0].get(nametemp);

                        var obj = new Object();
                        obj[valtemp] = code;

                        if (me.displayFormat) {
                            obj[nametemp] = Ext.String.format(me.displayFormat, code, name);
                        } else {
                            obj[nametemp] = (name === '&nbsp;') ? '' : name;
                        }

                        var valuepair = Ext.ModelManager.create(obj, 'model');

                        me.setValue(valuepair); //必须这么设置才能成功

                        if (me.isclose) {
                            win.hide();
                            win.close();
                            win.destroy();
                        }

                        var pobj = new Object();
                        pobj.code = code;
                        pobj.name = name;
                        pobj.type = 'fromhelp';
                        var formData = data[0].data;
                        //空值修正
                        for (var p in formData) {
                            if (formData[p] && formData[p] === '&nbsp;') {
                                formData[p] = '';
                            }
                        }
                        //处理主键
                        var keys = me.primaryKey.split(',');
                        var temp = '';
                        for (var i = 0; i < keys.length; i++) {

                            var keytemp = keys[i];
                            var arr = keys[i].split('.');
                            if (arr.length > 1) {
                                keytemp = arr[1];
                            }

                            if (i < (keys.length - 1)) {
                                temp += formData[keytemp] + ",";
                            }
                            else {
                                temp += formData[keytemp];
                            }

                        }
                        formData["key"] = temp;  //data[key];//主键列的值

                        var obj = new Object();
                        obj['key'] = me.primaryKey; //主键列
                        obj['unChangedRow'] = formData;
                        pobj.data = { 'form': obj };;
                        me.fireEvent('helpselected', pobj);
                        //}

                    }
                }, this);
                winItems.push(grid);
                winBtns = ['->', { text: '确定', handler: function () { grid.fireEvent('itemdblclick'); } }, { text: '取消', handler: function () { win.close(); } }];
            }
            //显示弹出窗口
            win = Ext.create('Ext.window.Window', {
                title: title, //'Hello',
                border: false,
                constrain: true,
                height: me.helpHeight,
                width: me.helpWidth,
                layout: 'border',
                modal: true,
                items: winItems,
                buttons: winBtns,
                maximizable: me.helpMaximizable,
                layout: {
                    type: 'border',
                    padding: 1
                }
            });
            me.sourcewin = win;
            win.show();

            toolbar.items.get('help_query').on('click', function () {
                store.loadPage(1);           
            })

            toolbar.items.get('help_close').on('click', function () {
                win.hide();
                win.close();
                win.destroy();
            })

            store.on('beforeload', function () {
                var formdata = querypanel.getForm();
                var data = formdata.getValues();

                if (me.likeFilter) {
                    Ext.apply(data, me.likeFilter);
                }

                //debugger;
                if (me.outFilter) {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data), 'outqueryfilter': JSON.stringify(me.outFilter) });
                }
                else {
                    Ext.apply(store.proxy.extraParams, { 'queryfilter': JSON.stringify(data) });
                }

                if (me.leftLikeFilter) {
                    Ext.apply(store.proxy.extraParams, { 'leftLikefilter': JSON.stringify(me.leftLikeFilter) });
                }
                if (me.clientSqlFilter) {
                    Ext.apply(store.proxy.extraParams, {
                        'clientSqlFilter': encodeURI(me.clientSqlFilter)
                    });
                }

                //return true;
            })

            if (me.needBlankLine) {
                store.on('load', function (store, records, successful, eOpts) {

                    //去掉表名
                    var myValueFiled;
                    var myDisplayField;
                    var temp = me.valueField.split('.');
                    if (temp.length > 1) {
                        myValueFiled = temp[1];
                    } else {
                        myValueFiled = me.valueField;
                    }

                    temp = me.displayField.split('.');
                    if (temp.length > 1) {
                        myDisplayField = temp[1];
                    } else {
                        myDisplayField = me.displayField;
                    }

                    var emptydata = new Object();
                    emptydata[myValueFiled] = '';
                    emptydata[myDisplayField] = '&nbsp;'; //空html标记          

                    var rs = [emptydata];
                    store.insert(0, rs);
                });
            }

        };

        var url = C_ROOT + 'IMP/Engine/Engine/GetHelpInfo?schemephid=' + me.schemephid + '&sourcephid=' + me.sourcephid;
        Ext.Ajax.request({
            url: url,
            params: {
                data: me.pagedata
            },
            callback: ShowHelp,
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.status === "ok") {
                    if (me.helpType === 'rich') {
                        title = resp.Title;
                        template = resp.template; //界面模板
                    }
                    else {
                        title = resp.data.Title;
                        allfield = resp.data.AllField;
                        headText = resp.data.HeadText;
                        me.valueField = resp.data.codeField;
                        me.displayField = resp.data.nameField;
                        me.selectMode = resp.data.isMulti == "1" ? 'Multi' : 'Single';
                        //me.selectMode = 'Multi';
                        me.primaryKey = resp.data.primaryKey;
                        me.maskdata = resp.data.MaskData;
                        me.datasourceData = resp.data.DatasourceData;
                        if (resp.data.clientSqlFilter && resp.data.clientSqlFilter != '') {
                            if (me.clientSqlFilter) {
                                me.clientSqlFilter += ' and ' + resp.data.clientSqlFilter
                            }
                            else {
                                me.clientSqlFilter = resp.data.clientSqlFilter;
                            }

                        }
                    }

                } else {
                    Ext.MessageBox.alert('取数失败', resp.status);
                }
            }
        });


    },
    showHelp: function () {
        this.onTriggerClick();
        return this.sourcewin;
    },
    btnOk: function (me, resultStore, win) {
        var values = new Array();
        var names = new Array();

        var arr = resultStore.data.items;
        for (var i = 0; i < arr.length; i++) {

            values.push(arr[i].data[me.valueField]);
            names.push(arr[i].data[me.displayField]);
        }

        var code = values.join(',');
        var name = names.join(',');

        var obj = new Object();
        obj[me.valueField] = code;

        if (me.displayFormat) {
            obj[me.displayField] = Ext.String.format(me.displayFormat, code, name);
        } else {
            obj[me.displayField] = name;
        }

        var valuepair = Ext.ModelManager.create(obj, 'model');
        me.setValue(valuepair); //必须这么设置才能成功

        if (me.isclose) {
            win.hide();
            win.close();
            win.destroy();
        }


        var unChangedData = [];
        Ext.Array.each(resultStore.data.items, function (record) {
            //处理主键
            var keys = me.primaryKey.split(',');
            var values = '';
            for (i = 0; i < keys.length; i++) {
                if (i < (keys.length - 1)) {
                    if (record.data[keys[i]]) {
                        values += record.data[keys[i]];
                    }
                    else {
                        values += "";
                    }

                    values += ",";
                }
                else {
                    if (record.data[keys[i]]) {
                        values += record.data[keys[i]];
                    }
                    else {
                        values += "";
                    }
                }
            }

            record.data['key'] = values;
            var obj = {
                'row': record.data
            }; //行标记
            unChangedData.push(obj);
        });

        var data = new Object();
        data['key'] = me.primaryKey;
        if (unChangedData.length > 0) {
            data['unChangedRow'] = unChangedData;
        }
        data = {
            'table': data
        };
        //if (me.isInGrid) {

        var pobj = new Object();
        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = data;

        me.fireEvent('helpselected', pobj);

    },
    copyData: function (selectData, resultStore) {
        var me = this;
        var dataLen = selectData.length;
        var count = resultStore.getCount();
        var index = count;
        if (count > 0) {
            for (var i = 0; i < dataLen; i++) {
                var sourceData = selectData[i].data[me.valueField];
                var hit = false;
                for (var j = 0; j < count; j++) {
                    var selectedData = resultStore.data.items[j].data[me.valueField];
                    if (sourceData === selectedData) {
                        hit = true;
                    }
                }
                if (!hit) {
                    resultStore.insert(index, selectData[i]);
                    index++;
                }
            }
        } else {
            resultStore.insert(0, selectData); //批量插入
        }
    },
    setOutFilter: function (obj) {
        this.outFilter = obj;
    },
    setCustom: function (custom) {
        if (custom && custom.width) {
            this.helpWidth = custom.width;
        }
        if (custom && custom.height) {
            this.helpHeight = custom.height;
        }
        if (custom && custom.data) {
            this.pagedata = custom.data;
        }
        if (custom && custom.clientSqlFilter) {
            this.clientSqlFilter = custom.clientSqlFilter;
        }
    },
    setLikeFilter: function (obj) {
        this.likeFilter = obj;
    },
    setLeftLikeFilter: function (obj) {
        this.leftLikeFilter = obj;
    },
    setClientSqlFilter: function (str) {
        this.clientSqlFilter = str;
    }
});
//#endregion


//#region imp ngImpMessageBox详细提示框
Ext.define('Ext.ng.ImpMessageBox', {
    extend: 'Ext.window.Window',
    alias: 'widget.ngImpMessageBox', //别名,可通过设置xtype构建 
    modal: true,
    closable: false,
    draggable: true,
    resizable: false,
    maximizable: false,
    //minimizable: true,
    height: 100,
    maxWidth: 600,
    minWidth: 270,
    maxHeight: 500,
    layout: {
        type: 'border',
        align: 'middle'
    },
    items: [{
        region: 'north',
        height: 34,
        itemId: 'text',
        xtype: 'text',
        text: '',
        padding: '10 10 10 10'
    }, {
        region: 'center',
        itemId: 'textarea',
        xtype: 'textarea'
    }],
    buttonAlign: 'center',
    buttons: [
        {
            xtype: 'button', text: '是', itemId: "yes", handler: function () {
                this.ownerCt.ownerCt.callback('是');
                this.ownerCt.ownerCt.close();
            }
        },
        {
            xtype: 'button', text: '否', itemId: "no", handler: function () {
                this.ownerCt.ownerCt.callback('否');
                this.ownerCt.ownerCt.close();
            }
        },
        {
            xtype: 'button', text: '详细信息', itemId: "remark", handler: function () {
                this.ownerCt.ownerCt.callback('详细信息');
                if (this.ownerCt.ownerCt.width == 600) {
                    this.ownerCt.ownerCt.setWidth(this.ownerCt.ownerCt.lenwidth);
                    this.ownerCt.ownerCt.setHeight(100);
                } else {
                    this.ownerCt.ownerCt.setWidth(600);
                    this.ownerCt.ownerCt.setHeight(400);
                }
                this.ownerCt.ownerCt.center();
            }
        }
    ],
    bodyStyle: "background-color:transparent;border:0px;",
    initComponent: function () {
        this.callParent();
    },
    show: function (title, msg, fn) {
        var len = this.strlen(msg.substring(0, 30));
        this.lenwidth = 270 + (len - 30) * 4;
        this.setWidth(this.lenwidth);
        this.setHeight(100);
        this.center();
        this.title = title;
        this.callback = fn;
        this.callParent();
        this.getComponent('text').setText(msg.substring(0, 30));
        this.getComponent('textarea').setValue(msg);
    },
    strlen: function (str) {
        var len = 0;
        for (var i = 0; i < str.length; i++) {
            var c = str.charCodeAt(i);
            //单字节加1 
            if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
                len++;
            }
            else {
                len += 2;
            }
        }
        return len;
    },
    hide: function () {
        this.callParent();
    },
    close: function () {
        this.callParent();
    }
}, function () {
    Ext.ngImpMessageBox = Ext.ngImpMsg = new this();
});
//#endregion

//#region 业务类型树
Ext.define('Ext.ng.BusTreePanel', {
    extend: 'Ext.panel.Panel',
    alias: ['widget.ngBusTreePanel'],
    title: "业务类型",
    autoScroll: false,
    collapsible: true,
    split: true,
    region: 'west',
    weight: 10,
    width: 190,
    minSize: 180,
    maxSize: 180,
    isNeedLazyLoad: true,
    isRemoveNotExistNode: true,
    border: 0,
    enTree: false,//是否用企业功能树
    isSearching: false, //判断搜索状态，默认false（没有搜索），当按下搜索按钮的时候变为true
    searchCondition: '', //搜索的字符串，保存下来，可能会用到
    layout: 'border',
    isNeedLazy: true,
    initComponent: function () {
        var me = this;
        this.callParent();

        if (!me.tablename) {
            me.tablename = '';
        }
        var thestore = Ext.create('Ext.data.TreeStore', {
            autoLoad: true,
            nodeParam: 'nodeid',
            root: {
                expanded: true
            },
            fields: [{ name: 'text', type: 'string' },
            { name: 'phid', type: 'string' },
            { name: 'Suite', type: 'string' },
            { name: 'bustype', string: 'string' },
            { name: 'nodetype', string: 'string' },
            ],
            proxy: {
                type: 'ajax',
                //url: C_ROOT + 'SUP/IndividualProperty/LoadBusTree'
                url: C_ROOT + 'MDP/BusObj/SysMenu/SysMenuTree?product=' + me.product + "&tablename=" + me.tablename + "&enTree=" + me.enTree + "&isNeedLazyLoad=" + me.isNeedLazy + "&isRemoveNotExistNode=" + me.isRemoveNotExistNode
            }
        });

        var busTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            itemId: 'busTree',
            rootVisible: false,
            lines: false,
            store: thestore
        });
        if (!me.isNeedLazy) {
            Ext.Array.each(me.getDockedItems(), function (component) {
                component.hide();
            });
            me.add({
                region: 'north',
                height: 26,
                layout: 'border',
                border: false,
                items: [{
                    region: 'center',
                    xtype: "ngText",
                    allowBlank: true,
                    cls: 'x-form-text-focus',
                    fieldLabel: '',
                    emptyText: '输入关键字，定位树节点',
                    margin: '2 0 2 2',
                    enableKeyEvents: true,
                    listeners: {
                        'keydown': function (el, e, eOpts) {
                            if (e.getKey() == e.ENTER) {
                                me.findNodeByFuzzy(busTree, el.getValue());
                                el.focus();
                                return false;
                            }
                            else {
                                busTree.nodeIndex = -1;
                            }
                        }
                    }
                }, {
                    region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5',
                    handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(busTree, el.getValue()); el.focus(); }
                }]
            });
        }
        me.add(busTree);

        me.on('afterrender', function () {
            var titlePanels = Ext.query("*[class=x-tool x-box-item x-tool-default]");

            for (var i = 0; i < titlePanels.length; i++) {
                titlePanels[i].style.margin = "0px 2px";//图标被遮住1个像素     
            }

            var collaspeTitle = Ext.query("*[class=x-tool x-box-item x-tool-default x-top x-tool-top x-tool-default-top x-tool-before-title]");
            for (var i = 0; i < collaspeTitle.length; i++) {
                collaspeTitle[i].style.margin = "0px 2px";//图标被遮住1个像素     
            }

        })
    },
    dockedItems: [{
        xtype: 'toolbar',
        //width: 300,
        //height: 26,
        height: 30,
        dock: 'top',
        layout: 'border',
        minWidth: 100,
        items: [
            {
                region: 'center',
                xtype: 'textfield',
                itemId: 'query',
                name: 'queryname',
                emptyText: '搜索内容',
                enableKeyEvents: true,
                listeners: {
                    'keydown': function (el, e, eOpts) {
                        if (e.getKey() == e.ENTER) {
                            var condition = this.ownerCt.queryById('query').getValue();
                            //alert(condition);
                            if (condition == '' || condition == null) {
                                return;
                            };
                            var busFuncTree = this.findParentByType('ngBusTreePanel');//找到根节点
                            if (!busFuncTree.tablename) {
                                busFuncTree.tablename = '';
                            }
                            //var itemsLength = busFuncTree.items.items.length;
                            //busFuncTree.setActiveTab(busFuncTree.items.items[itemsLength - 1]);
                            var tree = busFuncTree.getTree();

                            //按下搜索按钮或者回车的时候，变为true
                            busFuncTree.isSearching = true;

                            busFuncTree.searchCondition = condition;

                            //this.ownerCt.queryById('query').setValue(''); //输入框为空
                            tree.getRootNode().removeAll();
                            tree.getStore().setProxy(
                                {
                                    type: 'ajax',
                                    //url: C_ROOT + 'SUP/IndividualProperty/LoadBusTree'
                                    url: C_ROOT + 'MDP/BusObj/SysMenu/Query?condition=' + condition + "&tablename=" + busFuncTree.tablename + "&isRemoveNotExistNode=" + busFuncTree.isRemoveNotExistNode + "&enTree=" + busFuncTree.enTree,
                                }
                            );

                            tree.getStore().load();
                            tree.getView().refresh();
                            //tree.getRootNode().expand();
                        }
                    }
                }
                //minWidth: 80             
            }, {
                region: 'east',
                //width: 40,
                tooltip: '搜索',
                iconCls: 'icon-Query',
                handler: function () {
                    var condition = this.ownerCt.queryById('query').getValue();
                    //alert(condition);
                    if (condition == '' || condition == null) {
                        //alert('请输入搜索内容');
                        return;
                    };
                    var busFuncTree = this.findParentByType('ngBusTreePanel');//找到根节点
                    if (!busFuncTree.tablename) {
                        busFuncTree.tablename = '';
                    }
                    //var itemsLength = busFuncTree.items.items.length;
                    //busFuncTree.setActiveTab(busFuncTree.items.items[itemsLength - 1]);
                    var tree = busFuncTree.getTree();

                    //按下搜索按钮或者回车的时候，变为true
                    busFuncTree.isSearching = true;
                    busFuncTree.searchCondition = condition;

                    //this.ownerCt.queryById('query').setValue(''); //输入框为空
                    tree.getRootNode().removeAll();
                    tree.getStore().setProxy(
                        {
                            type: 'ajax',
                            //url: C_ROOT + 'SUP/IndividualProperty/LoadBusTree'
                            url: C_ROOT + 'MDP/BusObj/SysMenu/Query?condition=' + condition + "&tablename=" + busFuncTree.tablename + "&isRemoveNotExistNode=" + busFuncTree.isRemoveNotExistNode + "&enTree=" + busFuncTree.enTree,
                        }
                    );

                    tree.getStore().load();
                    tree.getView().refresh();
                    //tree.getRootNode().expand();

                }
            }, {
                region: 'east',
                //width: 40,
                tooltip: '刷新',
                iconCls: 'icon-Refresh',
                itemId: 'refreshTree',
                anchor: '100%',
                handler: function () {
                    //alert('刷新');
                    var busFuncTree = this.findParentByType('ngBusTreePanel');//找到根节点
                    var tree = busFuncTree.getTree();
                    var _self = this;
                    this.ownerCt.queryById('query').setValue(''); //输入框为空
                    //Ext.getCmp('query').setValue('');
                    //tree.removeAll();//无效果
                    this.disable();//先禁用刷新按钮

                    //刷新的时候变为false
                    busFuncTree.isSearching = false;

                    busFuncTree.searchCondition = '';

                    tree.getRootNode().removeAll();
                    tree.getStore().setProxy(
                        {
                            type: 'ajax',
                            url: C_ROOT + 'MDP/BusObj/SysMenu/SysMenuTree?' + "tablename=" + busFuncTree.tablename + "&enTree=" + busFuncTree.enTree + "&isNeedLazyLoad=" + busFuncTree.isNeedLazy + "&isRemoveNotExistNode=" + busFuncTree.isRemoveNotExistNode
                        }
                    );
                    //tree.getStore().load();
                    tree.getStore().load({
                        //回调函数里再解除禁用
                        callback: function () {
                            _self.findParentByType('ngBusTreePanel').queryById('refreshTree').enable();
                        }
                    });
                }
            }]
    }],
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var firstFind = false;
        var me = this, index = -1;
        if (!me.nodeIndex) {
            me.nodeIndex = -1;
        }
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            //if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
            }
            me.nodeIndex = -1;
        }
    },
    getTree: function () {
        var me = this;
        return me.query("[itemId='busTree']")[0];
    }
});
//#region 业务类型树

//#region FastDP专用业务类型树
Ext.define('Ext.ng.BusTreePanelAll', {
    extend: 'Ext.panel.Panel',
    alias: ['widget.ngBusTreePanelAll'],
    title: "业务类型",
    autoScroll: false,
    collapsible: true,
    split: true,
    region: 'west',
    weight: 10,
    width: 190,
    minSize: 180,
    maxSize: 180,
    border: 0,
    layout: 'border',
    initComponent: function () {
        var me = this;
        this.callParent();

        Ext.define('bustypeModel', {
            extend: 'Ext.data.Model',
            fields: [{
                name: 'bustype',
                type: 'string',
                mapping: 'bustype'
            }, {
                name: 'busname',
                type: 'string',
                mapping: 'busname'
            }, {
                name: 'url',
                type: 'string',
                mapping: 'url'
            }]
        });
        if (typeof (me.tablename) == "undefined") {
            me.tablename = ""
        }
        var thestore = Ext.create('Ext.data.TreeStore', {
            autoLoad: true,
            root: {
                expanded: true
            },
            fields: [{ name: 'text', type: 'string' },
                { name: 'bustype', string: 'string' },
                { name: 'phid', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: C_ROOT + 'SUP/IndividualProperty/LoadBusTree?tablename=' + me.tablename
            }
        });

        var busTree = Ext.create('Ext.tree.Panel', {
            region: 'center',
            itemId: 'busTree',
            rootVisible: false,
            lines: false,
            store: thestore
        });
        
        me.add({
            region: 'north',
            height: 26,
            layout: 'border',
            border: false,
            items: [{
                region: 'center',
                xtype: "ngText",
                allowBlank: true,
                cls: 'x-form-text-focus',
                fieldLabel: '',
                emptyText: '输入关键字，定位树节点',
                margin: '2 0 2 2',
                enableKeyEvents: true,
                listeners: {
                    'keydown': function (el, e, eOpts) {
                        if (e.getKey() == e.ENTER) {
                            me.findNodeByFuzzy(busTree, el.getValue());
                            el.focus();
                            return false;
                        }
                        else {
                            busTree.nodeIndex = -1;
                        }
                    }
                }
            }, {
                region: 'east', xtype: 'button', text: '', iconCls: 'icon-Location', width: 21, margin: '2 5 2 5',
                handler: function () { var el = arguments[0].prev(); me.findNodeByFuzzy(busTree, el.getValue()); el.focus(); }
            }]
        });

        me.add(busTree);
    },
    findNodeByFuzzy: function (tree, value) {
        if (value == "") { return; }
        var firstFind = false;
        var me = this, index = -1;
        if (!me.nodeIndex) {
            me.nodeIndex = -1;
        }
        var findNode = tree.getRootNode().findChildBy(function (node) {
            index++;
            //if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1 || node.data.bopomofo.indexOf(value.toUpperCase()) > -1)) {
            if (!node.data.root && index > me.nodeIndex && (node.data.text.indexOf(value) > -1)) {
                return true;
            }
        }, null, true);
        me.nodeIndex = index;
        if (findNode) {
            tree.selectPath(findNode.getPath());
        }
        else {
            if (firstFind) {
                Ext.MessageBox.alert('', '没有匹配的树节点.');
            }
            me.nodeIndex = -1;
        }
    },
    getTree: function () {
        var me = this;
        return me.query("[itemId='busTree']")[0];
    }
});
//#region FastDP专用业务类型树

//#region 图片
Ext.define('Ext.ng.ImagePanel', {
    extend: 'Ext.form.Panel',
    //extend: 'Ext.container.Container',
    //layout: 'form',
    alias: 'widget.ngImagePanel',
    initComponent: function () {

        var me = this;
        var filefield = Ext.create('Ext.form.field.File', {
            labelWidth: 50,
            msgTarget: 'side',
            //width: 300,
            fieldLabel: '海报',
            hideLabel: true,
            anchor: '100%',
            buttonText: '选择图片',
            hidden: true,
            validator: function (value) {
                //jpg gif bmp png
                if (!Ext.isEmpty(value)) {
                    var arr = value.split('.');
                    var tp = false;
                    switch (arr[arr.length - 1].toLowerCase()) {
                        case 'jpg':
                        case 'gif':
                        case 'bmp':
                        case 'png': tp = true;
                    }
                    if (tp) {
                        return true;
                    }
                    return "请选择图片";
                }
                return true;
            },
            listeners: {
                change: function () {
                    var form = me.getForm();
                    //var form = me.up('form').getForm();
                    form.submit({
                        url: C_ROOT + "FileUpload/ImageUpload",
                        waitMsg: '创建预览中...',
                        success: function (fp, o) {
                            var path = o.result.path;
                            me.queryById('hiddenVal').setValue(path);
                            img.setSrc(C_ROOT + path);
                        }
                    });
                }
            }
        });

        var img = Ext.create('Ext.Img', {
            alt: '暂无图片',
            //height: 400,
            //width: 589,
            autoShow: true,
            autoScroll: true,
            anchor: '100%',
            itemId: 'ActiovityPosterImg',
            src: C_ROOT + '1'
            //contentEl: '暂无图片'
        });
        //隐藏域用于保存url
        var hiddenField = Ext.create('Ext.form.field.Hidden', {
            itemId: 'hiddenVal',
            name: me.name,
            listeners: {
                'change': function (text, newVal, oldVal) {
                    img.setSrc(C_ROOT + newVal);
                }
            }
        })

        me.items = [filefield, img, hiddenField];

        this.callParent();

        me.on('afterrender', function () {

            me.getEl().on('mouseover', function () {
                //alert('onmouseover');
                filefield.setVisible(true);
            })

            me.getEl().on('mouseout', function () {
                //alert('onmouseover');
                filefield.setVisible(false);
            })
        })

    }
});
//#endregion

//#region 内嵌查询专用数字控件
Ext.define('Ext.ng.QueryNumber', {
    extend: 'Ext.form.field.Number',
    mixins: { base: 'Ext.ng.form.field.Base' },
    alias: 'widget.ngQueryNumber', //别名,可通过设置xtype构建,或者通过Ext.widget()方法构建 
    emptyText: '请输入数字',//显示空白字符
    decimalPrecision: 8,//小数点后允许的最大精度
    initComponent: function () {
        var me = this;
        this.mixins.base.initComponent.call(me);
        me.callParent();
        me.on('blur', function (number) {
            //要用rawValue获取原始值
            if (this.rawValue == 0) {
                return true;
            }
            //必须下面这样写，不然还是会出错误提示    
            if (isNaN(this.rawValue)) {
                this.setValue(null);
            }

        });
        //输入的时候监听，非数字直接置空
        me.on('change', function (a) {
            if (isNaN(this.rawValue)) {
                this.setValue(null);
            }
        });
    },
});
//#endregion
                                                                                                                                                                                                                          
