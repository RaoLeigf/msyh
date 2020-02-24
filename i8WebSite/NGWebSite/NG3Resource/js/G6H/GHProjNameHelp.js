
//工会预算项目名称帮助
Ext.define('Ext.ng.GHProjNameHelp', {
    extend: 'Ext.form.field.ComboBox',
    mixins: { base: 'Ext.ng.form.field.Base' },
    requires: ['Ext.ng.form.field.Base'],
    alias: ['widget.GHProjNameHelp'],
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
    maxFlexColumns:5,
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

        var existQueryProperty = false;
        var queryPropertyItems;
        var showTree;
        var richQueryItem;
        var richQueryFilter;
                
        ShowHelp = function () {

            var queryItems;
            var modelFields;
            var gridColumns;

            if (!allfield) {
                NGMsg.Error('获取帮助信息失败，请检查数据库通用帮助配置信息是否正确!');
                me.isShowing = false;
                return;
            }

            var fields = allfield.split(','); //所有字段
            var heads = headText.split(','); //列头

            queryItems = new Array();
            for (var i = 0; i < heads.length; i++) {
                var tempfield = fields[i].split('.');
                var temp = fields[i];
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
                var ar = temp.split(' ');//取别名
                if (ar.length > 1) {
                    temp = ar[ar.length - 1].trim();
                }

                modelFields.push({
                    name: temp, //fields[i], //不去掉表名
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
                var ar = temp.split(' ');//取别名
                if (ar.length > 1) {
                    temp = ar[ar.length - 1].trim();
                }
                
                if (heads.length > me.maxFlexColumns) {
                    gridColumns.push({
                        header: heads[i],
                        //flex: 1,
                        width: 200,
                        //sortable: true,
                        dataIndex: temp //fields[i] 去掉表名
                    });
                }
                else {
                    gridColumns.push({
                        header: heads[i],
                        flex: 1,                        
                        //sortable: true,
                        dataIndex: temp //fields[i] 去掉表名
                    });
                }
            }


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
                                    toolbar.queryById('ch-treerem').setVisible(true);

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
                        if (activeTab.id === 'treeStyle'){
                            var data = tree.getSelectionModel().selected.items[0].data;
                            var code = data.id;
                            if (data && !Ext.isEmpty(code)) {                               
                                me.addCommonUseData(me, code, commonUseStore);
                            }
                        }
                        else{
                            var data = grid.getSelectionModel().getSelection();
                            if (data.length > 0) {

                                var valField = help.valueField;
                                var temp = help.valueField.split('.');//多表关联的时候带表名
                                if (temp.length > 1) {
                                    valField = temp[1];//去表名
                                }
                                var code = data[0].get(valField);
                                me.addCommonUseData(me, code, commonUseStore);
                            }
                        }                        
                    }
                });
                buttons.push({ itemId: 'richhelp_del', text: '删除常用', disabled: true, handler: function () { me.delCommonUseData(me, commonUseGrid, commonUseStore) } });
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
                        commonUseStore.insert(commonUseStore.count(), data[0].data);
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