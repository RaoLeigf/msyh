Ext.define('Ext.Gc.ExcelImport', {
    extend: 'Ext.window.Window',
    width: 800,
    height: 500,
    constrain: true,
    title: CommLang.ExcelImport || 'Excel导入',//ExcelImport
    layout: {
        type: 'border',
        padding: 4
    },
    modal: true,
    common: true,//是否通用导入,false则在业务点可加工数据
    busiType: "excelimport",//区分业务点
    busiParm: "",//业务点个性化参数
    busiGrid: null,//源grid,用于获取字段列表
    beforeCreate: Ext.emptyFn,
    removeColumns: [],//移除列
    iseform:false,//是否自定义表单调用
    initComponent: function () {
        var me = this;

        me.on( 'beforeshow', $winBeforeShow );
        me.on( 'beforeclose', $winBeforeClose );

        me.addEvents('helpselected');

        me.importError = "0"
        me.importWarn = "0";

        /* 工具栏 */
        var toolBar = Ext.create('Ext.ng.Toolbar', {
            region: 'north',
            ngbuttons: [
                 { itemId: 'importdata', langkey:'importdata',text: '导入', iconCls: 'icon-Import' },
                 'deleterow',
                 { itemId: 'selected', langkey: 'selected', text: '确定', iconCls: 'icon-create' },
                '->', { itemId: "winclose", langkey: 'winclose', text: "关闭", iconCls: "icon-Close" }
            ]
        });
        toolBar.get('winclose').on('click', function () {
            me.close();
        });
        toolBar.get('importdata').on('click', function () {
            colPanel.setCache();
            if (!filePanel.queryById('filename').value) {
                Ext.Msg.alert(CommLang.Notes || "提示", CommLang.SelFile || "请先选择文件"); //SelFile
                return false;
            }
            if (!filePanel.queryById('sheetname').value) {
                Ext.Msg.alert(CommLang.Notes || "提示", CommLang.SelSheet || "请先选择工作表");
                return false;
            }
            var items = colPanel.grid.store.data.items;
            var isempty = true;
            for (var i = 0; i < items.length; i++) {
                if (items[i].data['excelName'] && items[i].data['excelName'] != "") {
                    isempty = false;
                    break;
                }
            }
            if (isempty) {
                Ext.Msg.alert(CommLang.Notes || "提示", CommLang.SelCell || "请至少选择一列进行对应");
                return false;
            }

            Ext.MessageBox.wait(CommLang.Importing || '正在导入数据,请稍后...', CommLang.Notes || '提示');
            Ext.Ajax.request({
                params: {
                    'busiType': me.busiType,
                    'fileName': filePanel.filename,
                    'excelSheet': filePanel.queryById('sheetname').value,
                    'excelSheetRow': filePanel.queryById('sheetrow').value,
                    'excelGrid': colPanel.grid.getAllGridData(),
                    'busiParm': me.busiParm
                },
                url: me.common ? C_ROOT + 'PMS/Common/Common/GetImportDataFromExcel' : C_PATH + 'GetImportDataFromExcel',
                success: function (response) {
                    var data = Ext.JSON.decode(response.responseText);
                    if (data.Status == "success") {
                        var list = Ext.decode(data.listData);
                        //me.beforeLoadData(list.Record);
                        datPanel.grid.getStore().loadData(list.Record);
                        datPanel.grid.getView().refresh();

                        me.importError = data.importError;
                        me.importWarn = data.importWarn;

                        Ext.Msg.alert(CommLang.Notes || '提示', CommLang.ImportSec || '导入成功');
                    }
                    else
                        Ext.Msg.alert(CommLang.Notes || '提示', data.Msg);
                }
            });
        });
        toolBar.get('deleterow').on('click', function () {
            var data = datPanel.grid.getSelectionModel().getSelection();
            var store = datPanel.grid.getStore();
            if (data.length > 0) {
                Ext.Array.each(data, function (record) {
                    store.remove(record);
                });
            }

        });
        toolBar.get('selected').on('click', function () {

            var okImport = function () {
                var pobj = new Object();
                //pobj.code = code;
                //pobj.name = name;
                pobj.type = 'excelimport';
                pobj.data = datPanel.grid.getStore().data.items;
                me.fireEvent('helpselected', pobj);
                me.close();
            }


            if (datPanel.grid.getStore().find('sysImportRowStatus', '1') > -1) {
                Ext.Msg.alert(CommLang.Notes || '提示', CommLang.importError || '导入数据存在异常,请更正!');
                return;
            }

            if (datPanel.grid.getStore().find('sysImportRowStatus', '9') > -1) {
                Ext.MessageBox.confirm(CommLang.Notes || '提示', CommLang.importWarn || '存在异常数据,是否继续?', function (callBack) {
                    if (callBack.toString() == 'yes') {
                        okImport();
                    }
                });
            } else {
                okImport();
            }
        });

        /* 工作表列表 */
        var initSheet = function (store, sheets) {
            if (sheets) {
                store.removeAll();
                store.add({ code: '', name: '&nbsp;' })//&nbsp;
                Ext.each(sheets, function (value) { store.add({ code: value, name: value }); });
            }
        }
        var storeSheet = Ext.create('Ext.data.Store', {
            fields: [{ name: 'code', type: 'string' }, { name: 'name', type: 'string' }],
            data: [
            ]
        });
        var filePanel = Ext.create('Ext.form.Panel', {
            region: 'north',
            layout: 'column',
            frame: true,
            broder: 0,
            defaults: { margin: '2,2,2,4' },
            items: [{
                xtype: 'filefield',
                itemId: 'filename',
                name: 'filename',
                fieldLabel: CommLang.FilePath || '文件路径',
                labelWidth: 60,
                msgTarget: 'side',
                allowBlank: false,
                columnWidth: .5,
                buttonText: CommLang.Seling || '选择...',
                validator: function (value) {
                    if (value) {
                        if (Ext.String.endsWith(value, ".xls", true) || Ext.String.endsWith(value, ".xlsx", true)) {
                            return true;
                        }
                        else
                            Ext.Msg.alert(CommLang.Notes || "提示", CommLang.SelExcel || "请选择Excle文件");
                    }
                    return false;
                },
                listeners: {
                    'change': function (ff, value, eOpts) {
                        var form = this.up('form').getForm();
                        if (form.isValid()) {
                            form.submit({
                                params: { 'busiType': me.busiType },
                                url: C_ROOT + 'PMS/Common/Common/UploadFile',
                                waitMsg: CommLang.UpLoading || '正在上传文件...',
                                method: 'post',
                                success: function (fp, o) {
                                    fp.findField('filename').inputEl.dom.value = fp.findField('filename').value;
                                    Ext.Msg.alert(CommLang.Notes || "提示", CommLang.UpLoadSec || '上传成功')
                                    filePanel.filename = o.result.filename;
                                    storeSheet.on("load", function () { initSheet(storeSheet, o.result.excelsheets); });
                                    initSheet(storeSheet, o.result.excelsheets);
                                },
                                failure: function (form, o) {
                                    Ext.Msg.alert(CommLang.Notes || '提示', o.result.Msg);
                                }
                            });
                        }
                    }
                }
            },
            {
                xtype: 'ngComboBox',
                fieldLabel: CommLang.Sheet || '工作表',
                itemId: "sheetname",
                labelWidth: 50,
                columnWidth: .3,
                displayField: 'name',
                valueField: 'code',
                valueType: 'string',
                store: storeSheet,
                listeners: {
                    'select': function (combo, records, eOpts) { if (records[0].data.code == '') combo.setRawValue('') }
                }
            }, {
                xtype: 'ngText',
                fieldLabel: CommLang.ExcelRowRange || 'Excel行范围',
                itemId: "sheetrow",
                labelWidth: 75,
                value: "1-1000",
                columnWidth: .2
            }
            ]
        });

        /* excel字段对应 */
        Ext.define('modelexcel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'colName', type: 'string' },
                { name: 'colTitle', type: 'string' },
                { name: 'excelName', type: 'string' },
                { name: 'helpId', type: 'string' }
            ]
        });
        var cellData = [];
        //cellData.push({ code: '', name: '&nbsp;' });
        Ext.each([
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                'AA', 'AB', 'AC', 'AD', 'AE', 'AF', 'AG', 'AH', 'AI', 'AJ', 'AK', 'AL', 'AM', 'AN', 'AO', 'AP', 'AQ', 'AR', 'AS', 'AT', 'AU', 'AV', 'AW', 'AX', 'AY', 'AZ',
                'BA', 'BB', 'BC', 'BD', 'BE', 'BF', 'BG', 'BH', 'BI', 'BJ', 'BK', 'BL', 'BM', 'BN', 'BO', 'BP', 'BQ', 'BR', 'BS', 'BT', 'BU', 'BV', 'BW', 'BX', 'BY', 'BZ',
                'CA', 'CB', 'CC', 'CD', 'CE', 'CF', 'CG', 'CH', 'CI', 'CJ', 'CK', 'CL', 'CM', 'CN', 'CO', 'CP', 'CQ', 'CR', 'CS', 'CT', 'CU', 'CV', 'CW', 'CX', 'CY', 'CZ'
        ],
            function (value) { cellData.push({ code: value, name: value }); });

        var gridcolumns = [];
        var index=0;
        if (me.busiGrid) {
            Ext.each(me.busiGrid.columns, function (col) {
                if (col.dataIndex && col.text != '行号' && col.text != '序号') {
                    //暂只支持最多两层多表头
                    if (me.busiGrid.columns[index].items.items.length > 0) {
                        Ext.each(me.busiGrid.columns[index].items.items, function (ccol) {

                            gridcolumns.push({
                                dataIndex: ccol.dataIndex,
                                header: ccol.text,
                                helpId: '',
                                width: ccol.width,
                                excelName: '',
                                hidden: ccol.hidden == true
                            });
                        });
                    } else {
                        gridcolumns.push({
                            dataIndex: col.dataIndex,
                            header: col.text,
                            helpId: '',
                            width: col.width,
                            excelName: '',
                            hidden: col.hidden == true
                        });
                    }
                }
                index++;
            });
        }
        else
            Ext.Msg.alert(CommLang.BusiGridCanNotNull || "busiGrid不能为空");

        me.beforeCreate(gridcolumns);

        if (!Ext.isEmpty(me.removeColumns)) {
            var ary = [];
            Ext.Array.forEach(gridcolumns, function (item) {
                if (Ext.Array.contains(me.removeColumns, item.dataIndex)) {
                    ary.push(item);
                }
            });

            Ext.Array.forEach(ary, function (item) {
                Ext.Array.remove(gridcolumns, item);
            });
        }

        //增加导入状态信息列
        gridcolumns.push({
            dataIndex: 'sysImportRowStatus', header: CommLang.ImportStat || '导入状态', helpId: '', width: 70, excelName: '', hidden: me.iseform,
            renderer: function (val) {
                switch (val.toString()) {
                    case "0":
                        return CommLang.ImportNormal||"正常";
                    case "1":
                        return CommLang.ImportPrompt || "提示";
                    case "9":
                        return CommLang.ImportFalse||"错误";
                    default:
                        return val.toString();
                }
            }
        });
        gridcolumns.push({
            dataIndex: 'sysImportRowMsg', header: CommLang.ImportMsg || '导入提示', helpId: '', width: 200, excelName: '', hidden: me.iseform
        });

        /* 映射 */
        var createColumnPanel = function (gridcolumns) {
            var columndata = [];
            Ext.each(gridcolumns, function (col) {
                if (!col.hidden && col.xtype != 'rownumberer' && col.dataIndex != 'sysImportRowStatus' && col.dataIndex != 'sysImportRowMsg')
                    columndata.push({
                        colName: col.dataIndex,
                        colTitle: col.header,
                        helpId: col.helpId,
                        excelName: col.excelName
                    });
            });

            var columnStore = Ext.create('Ext.data.Store', {
                model: 'modelexcel',
                data: columndata
            });
            var columnGrid = Ext.create("Ext.ng.GridPanel", {
                store: columnStore,
                border: 1,
                columnWidth: .5,
                columnLines: true,
                forceFit: true,
                columns: [
                    { text: CommLang.ColName || '列名', dataIndex: 'colTitle' },
                    {
                        text: CommLang.CellName || '单元格',
                        dataIndex: 'excelName',
                        editor: {
                            name: 'excelcol',
                            itemId: 'excelcol',
                            xtype: 'ngComboBox',
                            queryMode: 'local',
                            editable: false,
                            needBlankLine: true,
                            datasource: 'default',
                            data: cellData,
                            listeners: {
                                'select': function (combo, records, eOpts) {
                                    if (combo.getValue() == '')
                                        combo.setRawValue('');
                                },
                                'focus': function (combo, eOpts) {
                                    if (combo.getValue() == '')
                                        combo.setRawValue('');
                                }
                            }
                        }
                    }
                ],
                plugins: [
                    Ext.create('Ext.grid.plugin.CellEditing', {
                        clicksToEdit: 1
                    })
                ]
            });
            var columnPanel = Ext.create('Ext.form.Panel', {
                region: 'west',
                layout: 'fit',
                width: 200,
                frame: true,
                border: 0,
                items: [columnGrid]
            });

            columnPanel.grid = columnGrid;
            columnPanel.setCache = function () {
                var cacheData = [];
                for (var i = 0; i < this.grid.getStore().getCount() ; i++) {
                    var record = this.grid.getStore().getAt(i);
                    if (record.data.excelName)
                        cacheData.push({ "col": record.data.colName, "val": record.data.excelName });
                }
                sessionStorage.setItem("GcExcelImport." + me.busiType, Ext.JSON.encode(cacheData));
            };
            columnPanel.getCache = function () {
                var json = sessionStorage.getItem("GcExcelImport." + me.busiType);
                var cacheData = [];
                if (!Ext.isEmpty(json))
                    cacheData = Ext.JSON.decode(json)
                if (cacheData.length == 0) return;
                for (var i = 0; i < this.grid.getStore().getCount() ; i++) {
                    var record = this.grid.getStore().getAt(i);
                    Ext.Array.each(cacheData, function (item) {
                        if (item.col == record.data.colName)
                            record.data.excelName = item.val;
                    });
                }

            }
            return columnPanel;
        }

        /* 数据 */
        var createDataPanel = function (gridcolumns, modelName) {
            var cols = []
            //var storefields = []
            Ext.each(gridcolumns, function (col) {
                cols.push({
                    header: col.header,
                    dataIndex: col.dataIndex,
                    width: col.width,
                    hidden: col.hidden,
                    renderer: col.renderer
                });
                //storefields.push({
                //    name: col.dataIndex, type: 'string'
                //});
            });
            var dataGrid = Ext.create("Ext.ng.GridPanel", {
                //store: Ext.create('Ext.data.Store', { model: Ext.clone(me.busiGrid.getStore().model), autoLoad: false }),
                store: Ext.create('Ext.data.Store', { model: modelName, autoLoad: false }),
                border: 1,
                region: 'center',
                height: 100,
                columnLines: true,
                columns: cols
            });
            //me.afterCreateDataGrid(dataGrid);
            var dataPanel = Ext.create('Ext.form.Panel', {
                region: 'center',
                layout: 'border',
                frame: true,
                split: true,
                border: 0,
                items: [dataGrid]
            });
            dataPanel.grid = dataGrid;
            return dataPanel;
        }

        var colPanel = createColumnPanel(gridcolumns);

        var modelName=me.busiGrid.store.model.getName();

        Ext.define('excelimportconverdata', {
            extend: modelName,
            fields: [
                {
                    name: 'sysImportRowStatus',
                    type: 'string',
                    mapping: 'sysImportRowStatus'
                }, {
                    name: 'sysImportRowMsg',
                    type: 'string',
                    mapping: 'sysImportRowMsg'
                }
            ]
        });

        var datPanel = createDataPanel(gridcolumns, 'excelimportconverdata');

        colPanel.getCache();
        me.items = [toolBar, filePanel, colPanel, datPanel];
        me.callParent();
    }
});