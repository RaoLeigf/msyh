
Ext.Loader.setConfig({
    enabled: true,
    paths: {
        'Ext.ux': '../NG3Resource/extjs/ux/statusbar',
        'Ext.ux.aceeditor': '../NG3Resource/extjs/ux/aceeditor'
    }
});

Ext.require(['Ext.ux.aceeditor.Panel', 'Ext.ux.StatusBar']);

//属性中文
var PropertyName = {
    id:'ID标识',
    xtype:'类型',
    fieldLabel: '标签',
    labelWidth: '标签宽度',
    name: '字段名',
    mustInput: '必输项',
    RunReadOnly:'只读',
    hidden: '是否隐藏',
    colspan: '列宽',
    maxLength: '最大长度',
    decimalPrecision: '小数点位数',
    decimalSeparator: '千分位符号',
    valueField: '代码列',
    displayField: '名称列',
    helpid: '帮助id',
    matchFieldWidth:'匹配控件宽度',
    boxLabel: '标签',
    columnsPerRow: '每行列数',
    title: '标题',
    collapsible: '是否可折叠',
    header: '列标题',
    dataIndex: '字段名',
    editor: '编辑器',
    width: '列宽',
    bindtable: '绑定表' 
}

//控件最基本的属性
var baseCtlInfo = {
    xtype: 'ngText',
    fieldLabel: 'Text',
    name: 'Text',
    mustInput: false,
    RunReadOnly: false,
    hidden: false,
    colspan: 1
}
//form field设计时信息
var designInfo = {
    ngText: {
        xtype: 'ngText',
        fieldLabel: 'Text',
        name: 'Text',
        mustInput: false,
        maxLength: 100,
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    ngTextArea: {
        xtype: 'ngTextArea',
        fieldLabel: 'Text Area',
        name: 'Text Area',
        mustInput: false,
        maxLength: 200,
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    ngDate: {
        xtype: 'ngDate',
        fieldLabel: 'Date',
        name: 'Date',
        mustInput: false,
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    ngDateTime: {
        xtype: 'ngDateTime',
        fieldLabel: 'DateTime',
        name: 'DateTime',
        mustInput: false,
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    ngNumber: {
        xtype: 'ngNumber',
        fieldLabel: 'Number',
        name: 'Number',
        mustInput: false,
        decimalPrecision: 2, //小数点位数
        decimalSeparator: '.', //小数点符号
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    ngComboBox: {
        xtype: 'ngComboBox',
        fieldLabel: 'ComboBox',
        name: 'ComboBox',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: '',
        colspan: 1,
        RunReadOnly: false,
        hidden: false,
        matchFieldWidth: true
    },
    ngAutoComplete: {
        xtype: 'ngAutoComplete',
        name: 'ngAutoComplete',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: '',
        colspan: 1,
        RunReadOnly: false,
        hidden: false,
        matchFieldWidth: true
    },
    ngCommonHelp: {
        xtype: 'ngCommonHelp',
        fieldLabel: 'CommonHelp',
        name: 'CommonHelp',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: '',
        colspan: 1,
        RunReadOnly: false,
        hidden: false,
        matchFieldWidth: true
    },
    ngRichHelp: {
        xtype: 'ngRichHelp',
        fieldLabel: 'ngRichHelp',
        name: 'ngRichHelp',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: '',
        colspan: 1,
        RunReadOnly: false,
        hidden: false,
        matchFieldWidth: true
    },
    ngRadio: {
        xtype: 'ngRadio',
        boxLabel: 'Radio',
        name: 'Radio',
        mustInput: false,
        hidden: false,
        colspan: 1
    },
    ngCheckbox: {
        xtype: 'ngCheckbox',
        boxLabel: 'Checkbox',
        name: 'Checkbox',
        mustInput: false,
        RunReadOnly: false,
        hidden: false,
        colspan: 1
    },
    fieldset: {
        columnsPerRow: 3,
        title: '',
        collapsible: true
    },
    panel: {
        id: '',
        title: '',
        hidden: false
    },
    checkboxgroup: {
        fieldLabel: '',
        colspan: 1
    },
    radiogroup: {
        fieldLabel: '',
        colspan: 1
    }    
}

//grid列控件设计时信息
var colCtlDesignInfo = {
    ngText: {
        xtype: 'ngText',
        name: 'Text',
        mustInput: false,
        maxLength: 100
    },
    ngDate: {
        xtype: 'ngDate',
        name: 'Date',
        mustInput: false
    },
    ngDateTime: {
        xtype: 'ngDateTime',
        name: 'DateTime',
        mustInput: false
    },
    ngNumber: {
        xtype: 'ngNumber',
        name: 'Number',
        mustInput: false,
        decimalPrecision: 2, //小数点位数
        decimalSeparator: '.' //小数点符号
    },
    ngComboBox: {
        xtype: 'ngComboBox',
        name: 'ComboBox',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: ''
    },
    ngAutoComplete: {
        xtype: 'ngAutoComplete',
        name: 'ngAutoComplete',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: ''
    },
    ngCommonHelp: {
        xtype: 'ngCommonHelp',
        name: 'CommonHelp',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: ''
    },
    ngRichHelp: {
        xtype: 'ngRichHelp',
        name: 'ngRichHelp',
        mustInput: false,
        valueField: '',
        displayField: '',
        helpid: ''
    }
}

//列设计时信息
var gridColumnInfo = {    
    header: '',
    //flex: 1,
    hidden:false,
    width: 120,
    sortable: true,
    mustInput: false, //必输列
    dataIndex: '',
    editor: {}
};

//拖拽功能定义
Ext.define('Ext.ux.dd.PanelFieldDragZone', {
    extend: 'Ext.dd.DragZone',
    constructor: function (cfg) {
        cfg = cfg || {};
        if (cfg.ddGroup) {
            this.ddGroup = cfg.ddGroup;
        }
    },
    init: function (panel) {
        if (panel.nodeType) {
            Ext.ux.dd.PanelFieldDragZone.superclass.init.apply(this, arguments);
        }
        else {
            if (panel.rendered) {
                Ext.ux.dd.PanelFieldDragZone.superclass.constructor.call(this, panel.getEl());
            } else {
                panel.on('afterrender', this.init, this, { single: true });
            }
        }
    },
    scroll: false,
    getDragData: function (e) {
        var targetLabel = e.getTarget('label', null, true), oldMark, field, dragEl;
        if (targetLabel) {
            field = Ext.getCmp(targetLabel.up('.' + Ext.form.Labelable.prototype.formItemCls).id);
            return {
                field: field,
                ddel: field.getEl().dom,
                moveField: true,
                pos: "left"
            };
        }
    },
    getRepairXY: function () {
        return this.dragData.field.getEl().getXY();
    },
    onMouseUp: function (e) {
        if (!e) { return; }
        if (this.canDelete && !e.canDroped) {
            if (this.dragData.field.from === "devdefine") { //默认字段不允许删除
                return true;
            }
            this.getDragEl().style.display = "none"
            var panel = designContainer.getLayout().getActiveItem();
            var topNode = GetCurrParentTreeNode(panel.id);
            FindTreeNode(topNode, this.dragData.field).remove(true);
            componentTree.getView().select(topNode); //重排布局
            RefreshPropGrid(topNode);
        }
        e.canDroped = false;
        this.canDelete = false;
        this.removeDropStyle();
    },
    afterDragOut: function (target, e, id) {
        this.canDelete = true;
    },
    afterDragEnter: function (target, e, id) {
        this.canDelete = false;
    },
    onDrag: function (e, id) {
        this.removeDropStyle();
        var targetLabel = e.getTarget('label', null, true), field;
        if (targetLabel) {
            field = Ext.getCmp(targetLabel.up("." + Ext.form.Labelable.prototype.formItemCls).id);
            if (field && field.id != this.dragData.field.id) {
                this.LastEL = field.el;
                this.dragData.pos = "left";
                this.addDropStyle("left");
            }
        }
        else {
            var targetInput = e.getTarget('', null, true);
            if (targetInput) {
                targetInput = targetInput.up("." + Ext.form.Labelable.prototype.formItemCls);
                if (targetInput && targetInput.id) {
                    field = Ext.getCmp(targetInput.id);
                    if (field && field.id != this.dragData.field.id) {
                        this.LastEL = field.el;
                        this.dragData.pos = "right";
                        this.addDropStyle("right");
                    }
                }
            }
        }
        return true;
    },
    addDropStyle: function (pos) {
        if (this.LastEL && this.LastEL.dom) {
            this.LastEL.up("").setStyle({ "backgroundColor": "#FFFEE6", "paddingLeft": pos == "left" ? "10px" : "" });
        }
    },
    removeDropStyle: function () {
        if (this.LastEL && this.LastEL.dom) {
            this.LastEL.up("").setStyle({ "backgroundColor": "", "paddingLeft": "" });
        }
    }
});

Ext.define('Editor.Panel.WithToolbar', {
    extend: 'Ext.ux.aceeditor.Panel',
    alias: 'widget.AceEditor.WithToolbar',
    initComponent: function () {
        var me = this, toolbar = [
        //                   {
        //                       text: 'Save',
        //                       handler: function () {
        //                           alert(me.editor.getSession().getValue());
        //                       },
        //                       scope: me
        //                   },
{
    text: 'Undo',
    handler: me.undo,
    scope: me
},
{
    text: 'Redo',
    handler: me.redo,
    scope: me
},
'->',
{
    text: 'Settings',
    // iconCls: 'user',
    menu: {
        xtype: 'menu',
        plain: true,
        items: [{
            text: 'Show Invisibles',
            handler: function () {
                me.showInvisible = (me.showInvisible) ? false : true;
                me.editor.setShowInvisibles(me.showInvisible);
            },
            checked: (me.showInvisible),
            scope: me
        },
        {
            text: 'Wrap Lines',
            handler: function () {
                me.useWrapMode = (me.useWrapMode) ? false : true;
                me.editor.getSession().setUseWrapMode(me.useWrapMode);
            },
            checked: (me.useWrapMode),
            scope: me
        },
        {
            text: 'Code Folding',
            handler: function () {
                me.codeFolding = (me.codeFolding) ? false : true;
                me.editor.setShowFoldWidgets(me.codeFolding);
            },
            checked: (me.codeFolding),
            scope: me
        },
        {
            text: 'Highlight Active Line',
            handler: function () {
                me.highlightActiveLine = (me.highlightActiveLine) ? false : true;
                me.editor.setHighlightActiveLine(me.highlightActiveLine);
            },
            checked: (me.highlightActiveLine),
            scope: me
        },
        {
            text: 'Show Line Numbers',
            handler: function () {
                me.showGutter = (me.showGutter) ? false : true;
                me.editor.renderer.setShowGutter(me.showGutter);
            },
            checked: (me.showGutter),
            scope: me
        },
        {
            text: 'Highlight Selected Word',
            handler: function () {
                me.highlightSelectedWord = (me.highlightSelectedWord) ? false : true;
                me.editor.setHighlightSelectedWord(me.highlightSelectedWord);
            },
            checked: (me.highlightSelectedWord),
            scope: me
        },
        {
            xtype: 'menuseparator'
        },
        Ext.create('Ext.container.Container', {
            layout: {
                type: 'hbox'
            },
            items: [{
                xtype: 'menuitem',
                text: 'Font Size',
                handler: function () {
                },
                flex: 1,
                checked: (me.highlightSelectedWord),
                scope: me
            },
            {
                fieldStyle: 'text-align: right',
                hideLabel: true,
                xtype: 'numberfield',
                value: me.fontSize,
                minValue: 6,
                maxValue: 72,
                width: 50,
                flex: 0,
                height: 12,
                plain: true,
                listeners: {
                    change: function (field, value) {
                        me.fontSize = value;
                        me.setFontSize(me.fontSize + "px");
                    }
                }
            }]
        }),
        Ext.create('Ext.container.Container', {
            layout: {
                type: 'hbox'
            },
            width: 200,
            items: [{
                xtype: 'menuitem',
                text: 'Show Print Margin',
                handler: function () {
                },
                flex: 1,
                checked: (me.highlightSelectedWord),
                scope: me
            },
            {
                fieldStyle: 'text-align: right',
                hideLabel: true,
                xtype: 'numberfield',
                value: me.printMarginColumn,
                minValue: 1,
                maxValue: 200,
                width: 50,
                flex: 0,
                height: 12,
                plain: true,
                listeners: {
                    change: function (field, value) {
                        me.printMarginColumn = value;
                        me.editor.setPrintMarginColumn(me.printMarginColumn);
                    }
                }
            }]
        }),
        {
            xtype: 'menuseparator'
        },
        {
            xtype: 'container',
            layout: {
                type: 'hbox'
            },
            width: 240,
            items: [{
                xtype: 'menuitem',
                text: 'Theme'
            },
            {
                xtype: 'combo',
                mode: 'local',
                flex: 1,
                value: me.theme,
                triggerAction: 'all',
                editable: false,
                name: 'Theme',
                displayField: 'name',
                valueField: 'value',
                queryMode: 'local',
                store: Ext.create('Ext.data.Store', {
                    fields: ['name',
                            'value'],
                    data: [{
                        value: 'ambiance',
                        name: 'Ambiance'
                    },
                    {
                        value: 'chrome',
                        name: 'Chrome'
                    },
                    {
                        value: 'clouds',
                        name: 'Clouds'
                    },
                    {
                        value: 'clouds_midnight',
                        name: 'Clouds Midnight'
                    },
                    {
                        value: 'cobalt',
                        name: 'Cobalt'
                    },
                    {
                        value: 'crimson_editor',
                        name: 'Crimson Editor'
                    },
                    {
                        value: 'dawn',
                        name: 'Dawn'
                    },
                    {
                        value: 'dreamweaver',
                        name: 'Dreamweaver'
                    },
                    {
                        value: 'eclipse',
                        name: 'Eclipse'
                    },
                    {
                        value: 'idle_fingers',
                        name: 'idleFingers'
                    },
                    {
                        value: 'kr_theme',
                        name: 'krTheme'
                    },
                    {
                        value: 'merbivore',
                        name: 'Merbivore'
                    },
                    {
                        value: 'merbivore_soft',
                        name: 'Merbivore Soft'
                    },
                    {
                        value: 'mono_industrial',
                        name: 'Mono Industrial'
                    },
                    {
                        value: 'monokai',
                        name: 'Monokai'
                    },
                    {
                        value: 'pastel_on_dark',
                        name: 'Pastel on dark'
                    },
                    {
                        value: 'solarized_dark',
                        name: 'Solarized Dark'
                    },
                    {
                        value: 'solarized_light',
                        name: 'Solarized Light'
                    },
                    {
                        value: 'textmate',
                        name: 'TextMate'
                    },
                    {
                        value: 'twilight',
                        name: 'Twilight'
                    },
                    {
                        value: 'tomorrow',
                        name: 'Tomorrow'
                    },
                    {
                        value: 'tomorrow_night',
                        name: 'Tomorrow Night'
                    },
                    {
                        value: 'tomorrow_night_blue',
                        name: 'Tomorrow Night Blue'
                    },
                    {
                        value: 'tomorrow_night_bright',
                        name: 'Tomorrow Night Bright'
                    },
                    {
                        value: 'tomorrow_night_eighties',
                        name: 'Tomorrow Night 80s'
                    },
                    {
                        value: 'vibrant_ink',
                        name: 'Vibrant Ink'
                    }]
                }),
                listeners: {
                    change: function (field, value) {
                        me.theme = value;
                        me.setTheme(me.theme);
                    }
                }
            }]
        }]
    }
}];

        var wordCount = Ext.create('Ext.toolbar.TextItem', { text: 'Position: 0' }),
    lineCount = Ext.create('Ext.toolbar.TextItem', { text: 'Line: 0' });


        Ext.apply(me, {
            tbar: toolbar,
            bbar: Ext.create('Ext.ux.StatusBar', {
                //defaultText: 'Default status',
                //statusAlign: 'right', // the magic config
                items: [lineCount, wordCount]
            })
        });

        me.on('editorcreated', function () {
            me.editor.selection.on("changeCursor", function (e) {
                var c = me.editor.selection.getCursor(),
                            l = c.row + 1;

                wordCount.update('Position: ' + c.column);
                lineCount.update('Line: ' + l);
            }, me);
        });

        me.callParent(arguments);
    }
});

Ext.define('Ext.ng.ColumnEditorTrigger', {
    extend: 'Ext.form.field.Trigger',
    initComponent: function () {
        var me = this;

        me.addEvents('setproperty');

    },
    onTriggerClick: function () {
        var me = this;

        var data = {};
        var val = me.getValue();
        if (me.getValue()) {
            var data = Ext.decode(me.getValue());
        }

        var colXtypeStore = Ext.create('Ext.data.Store', {
            model: 'XtypeModel',
            data: [{ value: 'ngCommonHelp', text: '帮助' }, { value: 'ngComboBox', text: '下拉' },
        { value: 'ngText', text: '单行文本' }, { value: 'ngNumber', text: '数字' },
        { value: 'ngDate', text: '日期' }, { value: 'ngDateTime', text: '时间' }
            ]
        });

        //列控件类型
        var colxtypeCombo = Ext.create('Ext.ng.PropertyCombo', {
            fieldLabel: '控件类型',
            labelAlign: 'right',
            labelWidth: 60,
            readOnly: true,//暂时只读
            store: colXtypeStore,
            value: data.xtype
        });

        var propertyGrid = Ext.create('Ext.grid.property.Grid', {
            region: 'center',
            //title: 'Properties',
            //height: 600,
            width: 300,
            source: data,
            customEditors: {
                //xtype: colxtypeCombo
            },
            customRenderers: {
                //                           xtype: function (v) {
                //                               var combValue = colXtypeStore.getById(v);
                //                               return combValue ? combValue.get('text') : '';
                //                           }
            },
            propertyNames: {
                //xtype : '控件类型'
            }
        })

        var toolbar = Ext.create('Ext.Toolbar',
        {
            region: 'north',
            border: false,
            //split: true,
            height: 26,
            minSize: 26,
            maxSize: 26,
            items: [colxtypeCombo]
        });

        win = Ext.create('Ext.window.Window', {
            title: 'Editor Properties',
            border: false,
            height: 300,
            width: 400,
            layout: 'border',
            modal: true,
            items: [toolbar, propertyGrid],
            buttons: [{
                text: '确定', handler: function () {

                    var val = Ext.encode(propertyGrid.getSource());
                    me.setValue(val);
                    me.fireEvent('setproperty', val);

                    win.close()
                }
            },
           { text: '取消', handler: function () { win.close() } }]
        });

        win.show();

        colxtypeCombo.on('change', function (comb, newValue, oldValue, eOpts) {
            var source = propertyGrid.getSource();
            var template = Ext.clone(colCtlDesignInfo[newValue]);

            for (var pro in template) {
                if (pro) {
                    if (source[pro]) {
                        template[pro] = source[pro];
                    }
                }
            }
            template.xtype = newValue;
            propertyGrid.setSource(template);

            //设置render
            var outSource = propGrid.source; //propGrid.getSource()
            //if ('ngCommonHelp' === newValue || 'ngCombox' === newValue) {
            //    if (!outSource.renderer) {
            //        outSource.renderer = function (val, metaData) {
            //            return metaData.column.getEditor().getCodeName(val);
            //        }
            //    }
            //    outSource.renderer = outSource.renderer.toString();
            //    propGrid.SetPropStore(outSource);
            //}
            //else {
            if (outSource.renderer) {
                delete outSource.renderer;
                propGrid.SetPropStore(outSource);
            }
            // }
        });

        propertyGrid.on('beforeedit', function (editor, e, eOpts) {
            return false;//暂时只读
            //var name = e.record.data.name;
            //if (name === 'xtype') {
            //    return false;
            //}

        });

    }
});

Ext.define('Ext.ng.RendererTrigger', {
    extend: 'Ext.form.field.Trigger',
    initComponent: function () {
        var me = this;

        me.addEvents('setproperty');

    },
    onTriggerClick: function () {
        var me = this;

        var data = {};
        var val = me.getValue();

        //                   if (me.getValue()) {
        //                       var data = Ext.decode(me.getValue());
        //                   }

        var editorbak = Ext.create('Ext.form.field.TextArea', {
            width: 400,
            height: 240,
            bodyPadding: 10,
            value: val
        });


        var aceeditor = Ext.create('Editor.Panel.WithToolbar', {
            //xtype: 'AceEditor.WithToolbar',
            //title: 'Javascript',
            //sourceEl: 'pre_1',
            theme: 'Chrome',
            parser: 'javascript',
            showInvisible: true,
            printMargin: true
        });

        var win = Ext.create('Ext.window.Window', {
            title: 'Renderer',
            border: false,
            height: 400,
            width: 700,
            layout: 'fit', //'border',
            modal: true,
            items: [aceeditor],
            buttons: [{
                text: '确定', handler: function () {

                    var val = aceeditor.getValue();
                    //me.fireEvent('setproperty', val);//暂时不设置值
                    win.close()
                }
            },
           { text: '取消', handler: function () { win.close() } }]
        });

        win.show();

        aceeditor.getSession().setValue(Ext.htmlDecode(val));

    }
});