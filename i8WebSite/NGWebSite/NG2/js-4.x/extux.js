///<reference path="../vswd-ext.js" />
Ext.ns('Ext.ux');
Ext.ns('Ext.ux.form');
Ext.ns('Ext.ux.grid');
Ext.ns('Ext.ux.plugin');

//debugger;

//#region Taskbar
Ext.ux.Taskbar = Ext.extend(Ext.Toolbar, {
    height: 26,
    //分隔符的位置
    getNotifyIndex: function () {
        var index = -1;
        this.items.each(function (item, i) {
            if (item.id == 'notify_split') {
                index = i;
                return true;
            }
        });
        return index;
    },
    //任务栏的开始位置
    getTaskStartIndex: function () {
        return 0;
    },
    //任务栏结束位置
    getTaskEndIndex: function () {
        return this.getNotifyIndex() - 1;
    },
    //返回所有的任务栏按钮
    getAllTaskButton: function () {
        var start = this.getTaskStartIndex(), end = this.getTaskEndIndex(), arr = [];
        for (; start < end; start++) {
            arr[start] = this.items.items[start];
        }
        return arr;
    },
    initComponent: function () {
        this.add('->', { id: 'notify_split', xtype: 'tbseparator' });
    },
    getTaskButton: function (win) {
        var found;
        this.items.each(function (item) {
            if (item.win == win) {
                found = item;
                item.show();
                return true;
            }
        });
        if (!found) {
            found = this.addTaskButton(win);
        }

        return found;
    },
    addNotify: function (obj) {
        return this.addButton(obj);
    },
    addTaskButton: function (win) {
        var btn = new Ext.ux.Taskbar.TaskButton(win);
        this.insertButton(this.getTaskEndIndex(), btn);
        this.doLayout();
        return btn;
    },
    remove: function (win) {
        win.taskButton.hide();
        this.doLayout();
    },
    setActiveButton: function (btn) {
        btn.toggle(true, true);
    },
    setInactiveButton: function (btn) {
        btn.toggle(false, true);
    }
});

Ext.ux.Taskbar.TaskButton = Ext.extend(Ext.Button, {
    enableToggle: true,
    iconCls: 'im-talk',
    maxWidth: 80,
    constructor: function (win) {
        Ext.ux.Taskbar.superclass.constructor.call(this, {
            win: win,
            iconCls: 'im-talk',
            text: Ext.util.Format.ellipsis(win.info.text, 12),
            handler: function () {
                if (win.minimized || win.hidden) {
                    win.show();
                } else if (win == win.manager.getActive()) {
                    win.minimize();
                } else {
                    win.toFront();
                }
            },
            clickEvent: 'mousedown',
            onRender: function () {
                Ext.ux.Taskbar.TaskButton.superclass.onRender.apply(this, arguments);

                this.cmenu = new Ext.menu.Menu({
                    items: [{
                        text: '\u8FD8\u539F',
                        handler: function () {
                            if (!this.win.isVisible()) {
                                this.win.show();
                            } else {
                                this.win.restore();
                            }
                        },
                        scope: this
                    }, {
                        text: '\u6700\u5C0F\u5316',
                        handler: this.win.minimize,
                        scope: this.win
                    }, {
                        text: '\u6700\u5927\u5316',
                        handler: this.win.maximize,
                        scope: this.win
                    }, '-', {
                        text: '\u5173\u95ED',
                        //handler: this.closeWin.createDelegate(this, this.win, true),
                        handler:Ext.Function.bind(this.closeWin,this, this.win, true),

                        scope: this.win
                    }]
                });

                this.cmenu.on('beforeshow', function () {
                    var items = this.cmenu.items.items;
                    var w = this.win;
                    items[0].setDisabled(w.maximized !== true && w.hidden !== true);
                    items[1].setDisabled(w.minimized === true);
                    items[2].setDisabled(w.maximized === true || w.hidden === true);
                }, this);

                this.el.on('contextmenu', function (e) {
                    e.stopEvent();
                    if (!this.cmenu.el) {
                        this.cmenu.render();
                    }
                    var xy = e.getXY();
                    xy[1] -= this.cmenu.el.getHeight();
                    this.cmenu.showAt(xy);
                }, this);
            },
            closeWin: function (cMenu, e, win) {
                if (!win.isVisible()) {
                    win.show();
                } else {
                    win.restore();
                }
                win.hide();
            }
        });
    }
});
//#endregion

//#region extux SupcanPager
Ext.ux.SupcanPager = Ext.extend(Ext.Toolbar, {
    //0 none 1:input pager 2:number pager 3:slider pager
    pagerMode: 1,
    sliderWidth: 197,
    buttonNum: 5,
    pageSize: 20,
    prependButtons: true,
    displayMsg: '显示 {0} - {1} / 共 {2} 条记录',
    emptyMsg: 'No data',
    firstText: 'First Page',
    afterPageText: '/ {0}',
    prevText: 'Previous Page',
    nextText: 'Next Page',
    lastText: 'Last Page',
    refreshText: 'Refresh',

    first: {},
    prev: {},
    next: {},
    last: {},
    refresh: {},
    afterTextItem: {},

    initComponent: function () {
        //debugger;
        var me = this;
        var pagingItems = [this.first = new Ext.button.Button({
            tooltip: this.firstText,
            overflowText: this.firstText,
            iconCls: 'x-tbar-page-first',
            disabled: true,
            handler: this.moveFirst,
            scope: this
        }), this.prev = new Ext.button.Button({
            tooltip: this.prevText,
            overflowText: this.prevText,
            iconCls: 'x-tbar-page-prev',
            disabled: true,
            handler: this.movePrevious,
            scope: this
        })];

        switch (this.pagerMode) {
            case 1:
                pagingItems.push(' ');
                pagingItems.push(this.inputItem = new Ext.form.NumberField({
                    cls: 'x-tbar-page-number',
                    allowDecimals: false,
                    allowNegative: false,
                    enableKeyEvents: true,
                    selectOnFocus: true,
                    submitValue: false,
                    listeners: {
                        scope: this,
                        keydown: this.onPagingKeyDown,
                        blur: this.onPagingBlur
                    }
                }));
                //debugger;
                pagingItems.push(this.afterTextItem = new Ext.toolbar.TextItem({
                    text: Ext.String.format(this.afterPageText, 1)
                }));
                pagingItems.push(' ');
                break;
            case 2:
                this.buttonGroup = new Ext.ButtonGroup({ frame: false, columns: this.buttonNum });
                for (var bi = 1; bi <= this.buttonNum; bi++) {
                    this.buttonGroup.add({ width: 25, disabled: true, enableToggle: true, text: bi, handler: function () { if (this.allowDepress == true) me.changePage(this.text - 0); } });
                }
                pagingItems.push(this.buttonGroup);
                break;
            case 3:
                pagingItems.push(this.slider = new Ext.Slider({
                    width: this.sliderWidth,
                    minValue: 1,
                    maxValue: 1,
                    plugins: new Ext.slider.Tip({
                        getText: function (thumb) {
                            return Ext.String.format('<b>{0}</b> / <b>{1}</b>', thumb.value, thumb.slider.maxValue);
                        }
                    }),
                    listeners: {
                        changecomplete: {
                            scope: this,
                            fn: function (s, v) {
                                this.changePage(v);
                            }
                        }
                    }
                }));
                break;
        }

        pagingItems.push(this.next = new Ext.button.Button({
            tooltip: this.nextText,
            overflowText: this.nextText,
            iconCls: 'x-tbar-page-next',
            disabled: true,
            handler: this.moveNext,
            scope: this
        }));
        pagingItems.push(this.last = new Ext.button.Button({
            tooltip: this.lastText,
            overflowText: this.lastText,
            iconCls: 'x-tbar-page-last',
            disabled: true,
            handler: this.moveLast,
            scope: this
        }));
        pagingItems.push('-');
        pagingItems.push(this.refresh = new Ext.button.Button({
            tooltip: this.refreshText,
            overflowText: this.refreshText,
            iconCls: 'x-tbar-loading',
            handler: this.doRefresh,
            scope: this
        }));

        var userItems = this.items || this.buttons || [];
        if (this.prependButtons) {
            this.items = userItems.concat(pagingItems);
        } else {
            this.items = pagingItems.concat(userItems);
        }
        delete this.buttons;
        if (this.displayInfo) {
            this.items.push('->');
            this.items.push(this.displayItem = new Ext.Toolbar.TextItem({}));
        }

        if (this.load)
            this.load.params = this.load.params || {};

        Ext.PagingToolbar.superclass.initComponent.call(this);
        if (this.AF && !this.AF.OnSort && this.AF.IsRemoteSort() == '1') {
            //            this.AF.OnSort = function () {
            //                Ext.apply(this.load.params, { custom: this.AF.getCustom() });
            //                this.doRefresh();
            //                        } .createDelegate(this);

            this.AF.OnSort = Ext.Function.bind(function () {
                Ext.apply(this.load.params, { custom: this.AF.getCustom() });
                this.doRefresh();
            }, this);

        }


        this.addEvents(
            'change',
            'beforechange'
        );
        this.on('afterlayout', this.onFirstLayout, this, { single: true });
        this.cursor = 0;
        if (this.autoLoad != false)
            this.doRefresh();
    },

    // private
    onFirstLayout: function () {
        if (this.dsLoaded) {
            this.onLoad.apply(this, this.dsLoaded);
        }
    },

    // private
    updateInfo: function () {
        if (this.displayItem) {
            var count = Number(this.AF.GetRows());
            var msg = count == 0 ?
                this.emptyMsg :
                Ext.String.format(
                    this.displayMsg,
                    this.cursor + 1, this.cursor + count, this.AF.totalCount
                );
            this.displayItem.setText(msg);
        }
    },

    // private
    pagerOnLoad: function (activePage, pages) {
        switch (this.pagerMode) {
            case 1:
                this.afterTextItem.setText(Ext.String.format(this.afterPageText, pages));
                this.inputItem.setValue(activePage);
                break;
            case 2:
                var startP = Math.min(activePage - Math.floor(this.buttonNum / 2), pages - this.buttonNum + 1);
                startP = Math.max(startP, 1);
                for (var bi = 0; bi < this.buttonNum; bi++) {
                    //debugger;
                    //currBtn = this.buttonGroup.get(bi);
                    currBtn = this.buttonGroup.getComponent(bi);
                    if (pages < startP + bi) {
                        currBtn.disable();
                    }
                    else {
                        currBtn.setText(startP + bi);
                        if (activePage == startP + bi) {
                            currBtn.allowDepress = false;
                            currBtn.toggle(true);
                        }
                        else {
                            currBtn.toggle(false);
                            currBtn.allowDepress = true;
                        }
                        currBtn.enable();
                    }
                }
                break;
            case 3:
                this.slider.setMaxValue(pages);
                this.slider.setValue(activePage);
                break;
        }
    },

    // private
    onLoad: function (o) {
        if (!this.rendered) {
            this.dsLoaded = [o];
            return;
        }

        this.cursor = o.startRow || 0;
        var d = this.getPageData(), ap = d.activePage, ps = d.pages;

        this.pagerOnLoad(ap, ps);

        this.first.setDisabled(ap == 1);
        this.prev.setDisabled(ap == 1);
        this.next.setDisabled(ap == ps);
        this.last.setDisabled(ap == ps);
        this.refresh.enable();

        //this.updateInfo();

        this.fireEvent('change', this, d);
    },

    // private
    getPageData: function () {
        var total = this.AF.totalCount;
        return {
            total: total,
            activePage: Math.ceil((this.cursor + this.pageSize) / this.pageSize),
            pages: total < this.pageSize ? 1 : Math.ceil(total / this.pageSize)
        };
    },

    changePage: function (page) {
        //debugger;
        //this.doLoad(((page - 1) * this.pageSize).constrain(0, this.AF.totalCount || 0));//扩展方法constrain方法不能用


//        var temp = (page - 1) * this.pageSize;
//        if (temp < 0) {
//            temp = 0;
//        }
//        else if (temp > this.AF.totalCount) {
//            temp = this.AF.totalCount;
//        }

        var temp = Ext.Number.constrain((page - 1) * this.pageSize, 0, this.AF.totalCount || 0);

        this.doLoad(temp);
    },

    // private
    onLoadError: function () {
        if (!this.rendered) {
            return;
        }
        this.refresh.enable();
    },

    // private
    readPage: function (d) {
        var v = this.inputItem.getValue(), pageNum;
        if (!v || isNaN(pageNum = parseInt(v, 10))) {
            this.inputItem.setValue(d.activePage);
            return false;
        }
        return pageNum;
    },

    // private
    beforeLoad: function () {
        if (this.rendered && this.refresh) {
            this.refresh.disable();
        }
    },

    // private
    doLoad: function (start) {
        //debugger;
        this.AF.OpenLoadMask(500);
        this.load.params = this.load.params || {};
        this.load.params.rows = this.pageSize;
        this.load.params.startRow = start;
                
        Ext.applyIf(this.load.params, { custom: this.AF.getCustom() });

        var flag = false;

        var oldsecc = this.load.success;

        //        this.load.success = function (res, opts) {
        //            oldsecc(res, opts);

        //            if (!flag || this.pagerMode===3 ) {  
        //                         
        //                this.AF.CloseLoadMask();
        //                var obj = Ext.decode(res.responseText);
        //                if (obj.totalRows)
        //                    this.AF.totalCount = obj.totalRows;
        //                else if (obj.Record)
        //                    this.AF.totalCount = obj.Record.length;

        //                this.onLoad(this.load.params)

        //                flag = true;
        //            }
        //        } .createDelegate(this);

        this.load.success = Ext.Function.bind(function (res, opts) {
            oldsecc(res, opts);

            if (!flag || this.pagerMode === 3) {

                this.AF.CloseLoadMask();

                try {

                    var obj = Ext.decode(res.responseText);
                    if (obj.totalRows)
                        this.AF.totalCount = obj.totalRows;
                    else if (obj.Record)
                        this.AF.totalCount = obj.Record.length;

                    this.onLoad(this.load.params)

                    flag = true;

                } catch (e) {}
             
            }
        }, this);


        if (this.fireEvent('beforechange', this, this.load.params) !== false) {
            //debugger;          
            Ext.Ajax.request(this.load);
        }
    },

    moveFirst: function () {
        this.doLoad(0);
    },

    movePrevious: function () {
        this.doLoad(Math.max(0, this.cursor - this.pageSize));
    },

    moveNext: function () {
        this.doLoad(this.cursor + this.pageSize);
    },

    moveLast: function () {
        var total = this.AF.totalCount,
            extra = total % this.pageSize;

        this.doLoad(extra ? (total - extra) : total - this.pageSize);
    },

    doRefresh: function () {
        this.doLoad(this.cursor);
    },

    // private
    onDestroy: function () {
        Ext.PagingToolbar.superclass.onDestroy.call(this);
    },

    onPagingFocus: function () {
        this.inputItem.select();
    },

    //private
    onPagingBlur: function (e) {
        var pageData = this.getPageData();
        var p = Math.min(this.inputItem.value - 0, pageData.pages);
        if (p != pageData.activePage)
            this.changePage(p);
    },

    // private
    onPagingKeyDown: function (field, e) {
        var k = e.getKey(), d = this.getPageData(), pageNum;
        if (k == e.RETURN) {
            e.stopEvent();
            pageNum = this.readPage(d);
            if (pageNum !== false) {
                pageNum = Math.min(Math.max(1, pageNum), d.pages) - 1;
                this.doLoad(pageNum * this.pageSize);
            }
        } else if (k == e.HOME || k == e.END) {
            e.stopEvent();
            pageNum = k == e.HOME ? 1 : d.pages;
            field.setValue(pageNum);
        } else if (k == e.UP || k == e.PAGEUP || k == e.DOWN || k == e.PAGEDOWN) {
            e.stopEvent();
            if ((pageNum = this.readPage(d))) {
                var increment = e.shiftKey ? 10 : 1;
                if (k == e.DOWN || k == e.PAGEDOWN) {
                    increment *= -1;
                }
                pageNum += increment;
                if (pageNum >= 1 & pageNum <= d.pages) {
                    field.setValue(pageNum);
                }
            }
        }
    }
});

//Ext.reg('suppaging', Ext.ux.SupcanPager);//weizj
    
//#endregion

//#region Ext.ux.IntelliMask
/**
* @class Ext.ux.IntelliMask
* @version 1.0.2
* @author Doug Hendricks. doug[always-At]theactivegroup.com
* @copyright 2007-2010, Active Group, Inc.  All rights reserved.
* @donate <a target="tag_donate" href="http://donate.theactivegroup.com"><img border="0" src="http://www.paypal.com/en_US/i/btn/x-click-butcc-donate.gif" border="0" alt="Make a donation to support ongoing development"></a>
* @constructor
* Create a new LoadMask
* @desc A custom utility class for generically masking elements while providing delayed function
* execution during masking operations.
*/
Ext.ux.IntelliMask = function (el, config) {

    Ext.apply(this, config || { msg: this.msg });
    this.el = Ext.get(el);

};

Ext.ux.IntelliMask.prototype = {

    /**
    * @cfg {String} msg The default text to display in a centered loading message box
    * @default 'Loading Media...'
    */
    msg: 'Loading...',
    /**
    * @cfg {String} msgCls
    * The CSS class to apply to the loading message element.
    * @default "x-mask-loading"
    */
    msgCls: 'x-mask-loading',


    /** @cfg {Number} zIndex The optional zIndex applied to the masking Elements
    */
    zIndex: null,

    /**
    * Read-only. True if the mask is currently disabled so that it will not be displayed (defaults to false)
    * @property {Boolean} disabled
    * @type Boolean
    */
    disabled: false,

    /**
    * Read-only. True if the mask is currently applied to the element.
    * @property {Boolean} active
    * @type Boolean
    */
    active: false,

    /**
    * @cfg {Boolean/Integer} autoHide  True or millisecond value hides the mask if the {link #hide} method is not called within the specified time limit.
    */
    autoHide: false,

    /**
    * Disables the mask to prevent it from being displayed
    */
    disable: function () {
        this.disabled = true;
    },

    /**
    * Enables the mask so that it can be displayed
    */
    enable: function () {
        this.disabled = false;
    },

    /**
    * Show this Mask over the configured Element.
    * @param {String/ConfigObject} msg The message text do display during the masking operation
    * @param {String} msgCls The CSS rule applied to the message during the masking operation.
    * @param {Function} fn The callback function to be invoked after the mask is displayed.
    * @param {Integer} fnDelay The number of milleseconds to wait before invoking the callback function
    * @return {Object} A hash containing Element references to the mask container element and 
    *   the element containing the message text.
    *   { mask : Element,
    *     msgEl : Element
    *   }
    * @example
    mask.show({autoHide:3000});   //show defaults and hide after 3 seconds.
    * @example
    mask.show('Loading Content', null, loadContentFn); //show msg and execute fn
    * @example
    mask.show({
    msg: 'Loading Content',
    msgCls : 'x-media-loading',
    fn : loadContentFn,
    fnDelay : 100,
    scope : window,
    autoHide : 2000   //remove the mask after two seconds.
    });
    */
    show: function (msg, msgCls, fn, fnDelay) {

        var opt = {}, autoHide = this.autoHide, mask, maskMsg;
        fnDelay = parseInt(fnDelay, 10) || 20; //ms delay to allow mask to quiesce if fn specified

        if (Ext.isObject(msg)) {
            opt = msg;
            msg = opt.msg;
            msgCls = opt.msgCls;
            fn = opt.fn;
            autoHide = typeof opt.autoHide != 'undefined' ? opt.autoHide : autoHide;
            fnDelay = opt.fnDelay || fnDelay;
        }
        if (!this.active && !this.disabled && this.el) {
            msg = msg || this.msg;
            msgCls = msgCls || this.msgCls;
            mask = this.el.mask(msg, msgCls);

            if (this.active = !!mask) {
                maskMsg = this.el.child('.' + msgCls);
                if (this.zIndex) {
                    mask.setStyle("z-index", this.zIndex);
                    if (maskMsg) {
                        maskMsg.setStyle("z-index", this.zIndex + 1);
                    }
                }
            }
        } else { fnDelay = 0; }

        //passed function is called regardless of the mask state.
        if (typeof fn === 'function') {
            fn.defer(fnDelay, opt.scope || null);
        } else { fnDelay = 0; }

        if (autoHide && (autoHide = parseInt(autoHide, 10) || 2000)) {
            this.hide.defer(autoHide + (fnDelay || 0), this);
        }

        return this.active ? { mask: mask, msgEl: maskMsg} : null;
    },

    /**
    * Hide this Mask.
    * @param {Boolean} remove  True to remove the mask element from the DOM after hide.
    */
    hide: function () {
        this.el && this.el.unmask();
        this.active = false;
        return this;
    },

    // private
    destroy: function () { this.hide(); this.el = null; }
};

if (Ext.provide) { Ext.provide('uxmask'); }
//#endregion

//#region Ext.ux.plugin.VisibilityMode

//#endregion

//#region Ext.ux.Toolbar

Ext.define('Ext.ux.Toolbar', {
    extend: 'Ext.Toolbar',
    pageSize: 20,
    itemWidth: 60,
    rightName: "", //权限名称
    ngbuttons: [],   //按钮
    //候选按钮
    candidateButtons: [{ id: "add", text: "新增", width: this.itemWidth, iconCls: "icon-New" },
                               { id: "edit", text: "修改", width: this.itemWidth, iconCls: "icon-Edit" },
                               { id: "delete", text: "删除", width: this.itemWidth, iconCls: "icon-delete" },
                               { id: "view", text: "查看", width: this.itemWidth, iconCls: "icon-View" },
                               { id: "save", text: "提交", width: this.itemWidth, iconCls: "icon-save" },
                               { id: "query", text: "查询", width: this.itemWidth, iconCls: "icon-Query" },
                               { id: "refresh", text: "刷新", width: this.itemWidth, iconCls: "icon-Refresh" },
                               { id: "clear", text: "清空", width: this.itemWidth, iconCls: "icon-Clear" },
                               { id: "copy", text: "数据拷贝", width: this.itemWidth, iconCls: "icon-Copy" },
                               { id: "import", text: "导入", width: this.itemWidth, iconCls: "icon-Import" },
                               { id: "export", text: "导出", width: this.itemWidth, iconCls: "icon-Export" },
                               { id: "create", text: "生成", width: this.itemWidth, iconCls: "icon-create" },
                               { id: "verify", text: "审核", width: this.itemWidth, iconCls: "icon-Verify" },
                               { id: "valid", text: "审批", width: this.itemWidth, iconCls: "icon-Valid" },
                               { id: "unvalid", text: "去审批", width: this.itemWidth, iconCls: "icon-Unvalid" },
                               { id: "addrow", text: "增行", width: this.itemWidth, iconCls: "icon-AddRow" },
                               { id: "deleterow", text: "删行", width: this.itemWidth, iconCls: "icon-DeleteRow" },
                               { id: "assign", text: "分配", width: this.itemWidth, iconCls: "icon-Assign" },
                               { id: "config", text: "配置", width: this.itemWidth, iconCls: "icon-Setup" },
                               { id: "compute", text: "计算", width: this.itemWidth, iconCls: "icon-Compute" },
                               { id: "location", text: "定位", width: this.itemWidth, iconCls: "icon-Location" },
                               { id: "subbill", text: "下级业务", width: this.itemWidth, iconCls: "icon-Backbill" },
                               { id: "relabill", text: "相关单据", width: this.itemWidth, iconCls: "icon-Relabill" },
                               { id: "check", text: "送审", width: this.itemWidth, iconCls: "icon-Check" },
                               { id: "checkview", text: "送审查看", width: this.itemWidth, iconCls: "icon-CheckView" },
                               { id: "history", text: "送审历史", width: this.itemWidth, iconCls: "icon-History" },
                               { id: "ok", text: "确认", width: this.itemWidth, iconCls: "icon-Confirm" },
                               { id: "cancel", text: "取消", width: this.itemWidth, iconCls: "icon-Cancel" },
                               { id: "help", text: "帮助", width: this.itemWidth, iconCls: "icon-Help" },
                               { id: "print", text: "打印", width: this.itemWidth, iconCls: "icon-Print" },
                               { id: "exit", text: "退出", width: this.itemWidth, iconCls: "icon-Exit" },
                               { id: "back", text: "返回", width: this.itemWidth, iconCls: "icon-back" },
                               { id: "editrow", text: "修改", width: this.itemWidth, iconCls: "icon-EditRow" },
                               { id: "first", text: "首", width: this.itemWidth, iconCls: "icon-Firstrec" },
                               { id: "pre", text: "前", width: this.itemWidth, iconCls: "icon-PriorRec" },
                               { id: "next", text: "后", width: this.itemWidth, iconCls: "icon-NextRec" },
                               { id: "last", text: "尾", width: this.itemWidth, iconCls: "icon-LastRec" },
                               { id: "deal", text: "处理", width: this.itemWidth, iconCls: "icon-Operate" },
                               { id: "note", text: "记事本", width: this.itemWidth, iconCls: "icon-Note" },
                               { id: "orgselect", text: "组织选择", width: this.itemWidth, iconCls: "icon-Boo" },
                               { id: "addbrother", text: "同级增加", width: this.itemWidth, iconCls: "icon-AddBrother" },
                               { id: "addchild", text: "下级增加", width: this.itemWidth, iconCls: "icon-AddChild" },
                               { id: "attachment", text: "附件", width: this.itemWidth, iconCls: "icon-Attachment" },
                               { id: "hide", text: "关闭", width: this.itemWidth, iconCls: "icon-Close" },
                               { id: "close", text: "关闭", width: this.itemWidth, iconCls: "icon-Close", handler: function () { $CloseTab(); } }
                               ],

    

    initComponent: function () {
        this.border = false;
        this.height = 26;
        this.minSize = 26;
        this.maxSize = 26;

        //Ext.Toolbar.superclass.initComponent.call(this);
        this.callParent();

        //this.on("beforerender", this.beforeRender); //控制权限

        this.createButton(); //创建按钮        
    },

//    listerners:
//    {
//        'beforerender': function () {
//            alert('before');
//        }
//    },

    createButton: function () {
        for (var i = 0; i < this.ngbuttons.length; i++) {
            var button = this.ngbuttons[i];
            var stdbutton;

            if (typeof (button) == "object") {
                if (button.id) {
                    stdbutton = this.findButton(button.id);
                    if (stdbutton) {
                        if (button.text) {
                            stdbutton.text = button.text;
                        }
                        if (button.iconCls) {
                            stdbutton.iconCls = button.iconCls;
                        }
                    }
                }
            }
            else {
                stdbutton = this.findButton(button); //字符串
            }

            if (stdbutton) {
                this.add(stdbutton);
            }
            else {

                if (button === '->') {
                    this.add({ xtype: 'tbfill' });
                }
                else if (button === '-') {
                    this.add({ xtype: 'tbseparator' });
                }
                else {
                    this.add(button);
                }
            }
        }
    },

    findButton: function (buttonid) {
        var items = this.candidateButtons;
        for (var i = 0; i < items.length; i++) {
            var btn = items[i];

            if ((buttonid === btn.id) || ((btn.id == undefined) && (buttonid === btn))) {
                return btn;
            }
        }
    },

    beforeRender: function (toolbar) {
        //alert(this.items.keys.toString());

        var me = this;

        //debugger;

        //if (!toolbar) return; //这个事件会被触发两次

//        Ext.Ajax.request({
//            url: 'SendList/GetRight', //?rightname=' + me.rightName,
//            params: { rightname: me.rightName },
//            success: function (response, opts) {

//                var disablebtn = response.responseText;

//                var arr = disablebtn.split(',');
//                for (var i = 0; i < arr.length; i++) {
//                    var btn = me.items.get(arr[i]);
//                    if (btn) {
//                        //btn.disable();
//                    }

//                    //this.get(arr[i]).disable();
//                }
//            }
//        });

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
        } .createDelegate(this);

        if (this.fireEvent('beforechange', this, this.load.params) !== false) {
            Ext.Ajax.request(this.load);
        }
    },

    get: function (id) {
        return this.items.get(id);
    }

})


//#endregion

//#region Ext.ux.Checker

Ext.define('Ext.ux.Checker',{
    mixins: {
        observable: 'Ext.util.Observable'
    },
    constructor: function (config) {
        this.mixins.observable.constructor.call(this, config);

        this.addEvents('checkin');
    }
});

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion