///<reference path="../vswd-ext.js" />
Ext.ns('Ext.ux');
Ext.ns('Ext.ux.form');
Ext.ns('Ext.ux.grid');
Ext.ns('Ext.ux.plugin');

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
                        handler: this.closeWin.createDelegate(this, this.win, true),
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

    initComponent: function () {
        var me = this;
        var pagingItems = [this.first = new Ext.Toolbar.Button({
            tooltip: this.firstText,
            overflowText: this.firstText,
            iconCls: 'x-tbar-page-first',
            disabled: true,
            handler: this.moveFirst,
            scope: this
        }), this.prev = new Ext.Toolbar.Button({
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
                pagingItems.push(this.afterTextItem = new Ext.Toolbar.TextItem({
                    text: String.format(this.afterPageText, 1)
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
                            return String.format('<b>{0}</b> / <b>{1}</b>', thumb.value, thumb.slider.maxValue);
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

        pagingItems.push(this.next = new Ext.Toolbar.Button({
            tooltip: this.nextText,
            overflowText: this.nextText,
            iconCls: 'x-tbar-page-next',
            disabled: true,
            handler: this.moveNext,
            scope: this
        }));
        pagingItems.push(this.last = new Ext.Toolbar.Button({
            tooltip: this.lastText,
            overflowText: this.lastText,
            iconCls: 'x-tbar-page-last',
            disabled: true,
            handler: this.moveLast,
            scope: this
        }));
        pagingItems.push('-');
        pagingItems.push(this.refresh = new Ext.Toolbar.Button({
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
            this.AF.OnSort = function () {                
                Ext.apply(this.load.params, { custom: this.AF.getCustom() });
                this.doRefresh();
            } .createDelegate(this);
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
                String.format(
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
                this.afterTextItem.setText(String.format(this.afterPageText, pages));
                this.inputItem.setValue(activePage);
                break;
            case 2:
                var startP = Math.min(activePage - Math.floor(this.buttonNum / 2), pages - this.buttonNum + 1);
                startP = Math.max(startP, 1);
                for (var bi = 0; bi < this.buttonNum; bi++) {
                    currBtn = this.buttonGroup.get(bi);
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
        this.updateInfo();
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
        this.doLoad(((page - 1) * this.pageSize).constrain(0, this.AF.totalCount || 0));
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

            this.onLoad(this.load.params)
        } .createDelegate(this);
        if (this.fireEvent('beforechange', this, this.load.params) !== false) {
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
        if(p != pageData.activePage)
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
Ext.reg('suppaging', Ext.ux.SupcanPager);
    
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
Ext.onReady(function () {
    /* This important rule solves many of the <object/iframe>.reInit issues encountered
    * when setting display:none on an upstream(parent) element (on all Browsers except IE).
    * This default rule enables the new Panel:hideMode 'nosize'. The rule is designed to
    * set height/width to 0 cia CSS if hidden or collapsed.
    * Additional selectors also hide 'x-panel-body's within layouts to prevent
    * container and <object, img, iframe> bleed-thru.
    */
    var CSS = Ext.util.CSS;
    if (CSS) {
        CSS.getRule('.x-hide-nosize') || //already defined?
            CSS.createStyleSheet('.x-hide-nosize{height:0px!important;width:0px!important;border:none!important;zoom:1;}.x-hide-nosize * {height:0px!important;width:0px!important;border:none!important;zoom:1;}');
        CSS.refreshCache();
    }

});

(function () {

    var El = Ext.Element, A = Ext.lib.Anim, supr = El.prototype;
    var VISIBILITY = "visibility",
        DISPLAY = "display",
        HIDDEN = "hidden",
        NONE = "none";

    var fx = {};

    fx.El = {

        /**
        * Sets the CSS display property. Uses originalDisplay if the specified value is a boolean true.
        * @param {Mixed} value Boolean value to display the element using its default display, or a string to set the display directly.
        * @return {Ext.Element} this
        */
        setDisplayed: function (value) {
            var me = this;
            me.visibilityCls ? (me[value !== false ? 'removeClass' : 'addClass'](me.visibilityCls)) :
                        supr.setDisplayed.call(me, value);
            return me;
        },

        /**
        * Returns true if display is not "none" or the visibilityCls has not been applied
        * @return {Boolean}
        */
        isDisplayed: function () {
            return !(this.hasClass(this.visibilityCls) || this.isStyle(DISPLAY, NONE));
        },
        // private
        fixDisplay: function () {
            var me = this;
            supr.fixDisplay.call(me);
            me.visibilityCls && me.removeClass(me.visibilityCls);
        },

        /**
        * Checks whether the element is currently visible using both visibility, display, and nosize class properties.
        * @param {Boolean} deep (optional) True to walk the dom and see if parent elements are hidden (defaults to false)
        * @return {Boolean} True if the element is currently visible, else false
        */
        isVisible: function (deep) {
            var vis = this.visible ||
                                    (!this.isStyle(VISIBILITY, HIDDEN) &&
                        (this.visibilityCls ?
                            !this.hasClass(this.visibilityCls) :
                                !this.isStyle(DISPLAY, NONE))
                      );

            if (deep !== true || !vis) {
                return vis;
            }

            var p = this.dom.parentNode,
                      bodyRE = /^body/i;

            while (p && !bodyRE.test(p.tagName)) {
                if (!Ext.fly(p, '_isVisible').isVisible()) {
                    return false;
                }
                p = p.parentNode;
            }
            return true;

        },
        //Assert isStyle method for Ext 2.x
        isStyle: supr.isStyle || function (style, val) {
            return this.getStyle(style) == val;
        }

    };

    //Add basic capabilities to the Ext.Element.Flyweight class
    Ext.override(El.Flyweight, fx.El);

    /**
    * @class Ext.ux.plugin.VisibilityMode
    * @version 1.3.1
    * @author Doug Hendricks. doug[always-At]theactivegroup.com
    * @copyright 2007-2009, Active Group, Inc.  All rights reserved.
    * @license <a href="http://www.gnu.org/licenses/gpl.html">GPL 3.0</a>
    * @donate <a target="tag_donate" href="http://donate.theactivegroup.com"><img border="0" src="http://www.paypal.com/en_US/i/btn/x-click-butcc-donate.gif" border="0" alt="Make a donation to support ongoing development"></a>
    * @singleton
    * @static
    * @desc This plugin provides an alternate mechanism for hiding Ext.Elements and a new hideMode for Ext.Components.<br />
    * <p>It is generally designed for use with all browsers <b>except</b> Internet Explorer, but may used on that Browser as well.
    * <p>If included in a Component as a plugin, it sets it's hideMode to 'nosize' and provides a new supported
    * CSS rule that sets the height and width of an element and all child elements to 0px (rather than
    * 'display:none', which causes DOM reflow to occur and re-initializes nested OBJECT, EMBED, and IFRAMES elements)
    * @example 
    var div = Ext.get('container');
    new Ext.ux.plugin.VisibilityMode().extend(div);
    //You can override the Element (instance) visibilityCls to any className you wish at any time
    div.visibilityCls = 'my-hide-class';
    div.hide() //or div.setDisplayed(false);
      
    // In Ext Layouts:      
    someContainer.add({
    xtype:'flashpanel',
    plugins: [new Ext.ux.plugin.VisibilityMode() ],
    ...
    });
    
    // or, Fix a specific Container only and all of it's child items:
    // Note: An upstream Container may still cause Reflow issues when hidden/collapsed
    
    var V = new Ext.ux.plugin.VisibilityMode({ bubble : false }) ;
    new Ext.TabPanel({
    plugins     : V,
    defaults    :{ plugins: V },
    items       :[....]
    });
    */
    Ext.ux.plugin.VisibilityMode = function (opt) {

        Ext.apply(this, opt || {});

        var CSS = Ext.util.CSS;

        if (CSS && !Ext.isIE && this.fixMaximizedWindow !== false && !Ext.ux.plugin.VisibilityMode.MaxWinFixed) {
            //Prevent overflow:hidden (reflow) transitions when an Ext.Window is maximize.
            CSS.updateRule('.x-window-maximized-ct', 'overflow', '');
            Ext.ux.plugin.VisibilityMode.MaxWinFixed = true;  //only updates the CSS Rule once.
        }

    };


    Ext.extend(Ext.ux.plugin.VisibilityMode, Object, {

        /**
        * @cfg {Boolean} bubble If true, the VisibilityMode fixes are also applied to parent Containers which may also impact DOM reflow.
        * @default true
        */
        bubble: true,

        /**
        * @cfg {Boolean} fixMaximizedWindow If not false, the ext-all.css style rule 'x-window-maximized-ct' is disabled to <b>prevent</b> reflow
        * after overflow:hidden is applied to the document.body.
        * @default true
        */
        fixMaximizedWindow: true,

        /**
        *
        * @cfg {array} elements (optional) A list of additional named component members to also adjust visibility for.
        * <br />By default, the plugin handles most scenarios automatically.
        * @default null
        * @example ['bwrap','toptoolbar']
        */

        elements: null,

        /**
        * @cfg {String} visibilityCls A specific CSS classname to apply to Component element when hidden/made visible.
        * @default 'x-hide-nosize'
        */

        visibilityCls: 'x-hide-nosize',

        /**
        * @cfg {String} hideMode A specific hideMode value to assign to affected Components.
        * @default 'nosize'
        */
        hideMode: 'nosize',

        ptype: 'uxvismode',
        /**
        * Component plugin initialization method.
        * @param {Ext.Component} c The Ext.Component (or subclass) for which to apply visibilityMode treatment
        */
        init: function (c) {

            var hideMode = this.hideMode || c.hideMode,
            plugin = this,
            bubble = Ext.Container.prototype.bubble,
            changeVis = function () {

                var els = [this.collapseEl, this.actionMode].concat(plugin.elements || []);

                Ext.each(els, function (el) {
                    plugin.extend(this[el] || el);
                }, this);

                var cfg = {
                    visFixed: true,
                    animCollapse: false,
                    animFloat: false,
                    hideMode: hideMode,
                    defaults: this.defaults || {}
                };

                cfg.defaults.hideMode = hideMode;

                Ext.apply(this, cfg);
                Ext.apply(this.initialConfig || {}, cfg);

            };

            c.on('render', function () {

                // Bubble up the layout and set the new
                // visibility mode on parent containers
                // which might also cause DOM reflow when
                // hidden or collapsed.
                if (plugin.bubble !== false && this.ownerCt) {

                    bubble.call(this.ownerCt, function () {
                        this.visFixed || this.on('afterlayout', changeVis, this, { single: true });
                    });
                }

                changeVis.call(this);

            }, c, { single: true });

        },
        /**
        * @param {Element/Array} el The Ext.Element (or Array of Elements) to extend visibilityCls handling to.
        * @param {String} visibilityCls The className to apply to the Element when hidden.
        * @return this
        */
        extend: function (el, visibilityCls) {
            el && Ext.each([].concat(el), function (e) {

                if (e && e.dom) {
                    if ('visibilityCls' in e) return;  //already applied or defined?
                    Ext.apply(e, fx.El);
                    e.visibilityCls = visibilityCls || this.visibilityCls;
                }
            }, this);
            return this;
        }

    });

    Ext.preg && Ext.preg('uxvismode', Ext.ux.plugin.VisibilityMode);
    /** @sourceURL=<uxvismode.js> */
    Ext.provide && Ext.provide('uxvismode');
})();
//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion

//#region extux

//#endregion