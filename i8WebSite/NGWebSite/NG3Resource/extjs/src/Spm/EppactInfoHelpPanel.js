Ext.define('Ext.Spm.EppactInfoHelpPanel', {
    extend: 'Ext.form.Panel',
    layout: 'border',
    pcid: 0,
    wbs: '',
    needrel: false,
    wbslist: '',
    isCheck: true,//是否只获取核准数据
    isFinish: false,//用于过滤已完工的作业
    isCal: true,
    valueField: 'PhId',
    displayField: 'Name',
    grid: 0,
    width: 700,
    muilt: false,//允许多选
    height: 350,
    selectMode: 'Single', //multiple
    initComponent: function () {
        var me = this;

        //多语言
        Ext.Ajax.request({
            url: C_ROOT + 'PMS/PCM/CntM/GetBaseInfo',
            async: false, //同步请求
            params: { 'bustype': 'JD_Auth' },
            success: function (response) {
                var resp = Ext.JSON.decode(response.responseText);
                if (resp.Status == "success")
                    me.Lang = Ext.isEmpty(resp) ? {} : Ext.decode(resp.Lang);
            }
        });

        me.addEvents('helpselected');
        me.addEvents('isready');//设置window上的多语言信息

        var store = Ext.create('Ext.ng.JsonStore', {
            model: 'eppactmodel',
            pageSize: 25,
            autoLoad: false,
            url: C_ROOT + 'Schedules/SpmEppact/GetSpmEppacHelp'
        });

        var ngToolbar = Ext.create('Ext.ng.Toolbar', {
            region: 'north',
            ngbuttons: [
                '->'
            ]
        });

        store.on('beforeload', function (_this, operation, eOpts) {
            //if (Ext.isEmpty(me.pcid)) return false;
            //Ext.apply(store.proxy.extraParams, { 'queryfilter': Ext.JSON.encode({ MstId: me.pcid,WbsId:me.wbs,WbsHelp:me.wbslist, Type: 0 }) });
            Ext.apply(store.proxy.extraParams, { 'clientfilter': Ext.JSON.encode({ MstId: me.pcid, WbsId: me.wbs, WbsHelp: me.wbslist, Type: 0, isCheck: me.isCheck, isFinish: me.isFinish, isCal: me.isCal }) });
        });

        //var pagingbar = Ext.create('Ext.ng.PagingBar', {
        //    store: store
        //});

        if (me.muilt)
            var selModel = Ext.create('Ext.selection.CheckboxModel');
        else
            var selModel = '';

        var cellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
            clicksToEdit: 1
        });

        var grid = Ext.create('Ext.ng.GridPanel', {
            region: 'center',
            //frame: true,                  
            stateful: true,
            plugins: [cellEditing],
            stateId: 'SPM3_EPPACT_HELP_NEW',
            store: store,
            selModel: selModel,
            //otype: 'view',
            buskey: 'PhId', //对应的业务表主键属性               
            columnLines: true,
            columns: [Ext.create('Ext.grid.RowNumberer', { text: me.Lang.rownumberer || '行号', width: 35 }),
                {
                    header: me.Lang.PhId || '主键',
                    dataIndex: 'PhId',
                    width: 100,
                    sortable: false,
                    hidden: true
                }, {
                    header: me.Lang.MstId || '外键',
                    dataIndex: 'MstId',
                    width: 100,
                    sortable: false,
                    hidden: true
                },
                //{
                //    header: me.Lang.PPhId || '上级作业Id',
                //    dataIndex: 'PPhId',
                //    width: 100,
                //    sortable: false,
                //    hidden: true,
                //},
                {
                    header: me.Lang.PPhId || '上级作业',
                    dataIndex: 'PPhId_EXName',
                    width: 100,
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.UserDefinedActcode || '作业编码',
                    dataIndex: 'UserDefinedActcode',
                    width: 120,
                    sortable: false,
                    hidden: false
                },
                //{
                //    header: me.Lang.Activityname || '作业名称',
                //    dataIndex: 'Activityname',
                //    width: 220,
                //    sortable: false,
                //    hidden: true
                //},
                {
                    header: me.Lang.TaskName || '作业名称',
                    dataIndex: 'Name',
                    width: 220,
                    sortable: false,
                    hidden: false
                }, {
                    header: Lang.Lagtype || '关系类型',
                    width: 100,
                    sortable: false,
                    dataIndex: 'Lagtype',
                    value: 0,
                    hidden:!me.needrel,
                    editable: true,
                    editor: {
                        xtype: 'ngComboBox',
                        id: 'Lagtype',
                        readOnly: false,
                        mustInput: true,
                        ORMMode: true,
                        valueField: 'code',
                        displayField: 'value',
                        listFields: '',
                        listHeadTexts: '',
                        queryMode: 'local', //local指定为本地数据,如果是后台传输则值为remote
                        //outFilter: {},
                        helpid: '',
                        data: [
                               { "code": 0, "value": (Lang.FS || '完工-开工(FS)') },
                               { "code": 1, "value": (Lang.SS || '开工-开工(SS)') },
                               { "code": 2, "value": (Lang.FF || '完工-完工(FF)') },
                               { "code": 3, "value": (Lang.SF || '开工-完工(SF)') }

                        ]
                    },
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        var reltype = "";
                        switch (parseInt(value)) {
                            case 0: reltype = Lang.FS || '完工-开工(FS)';
                                break;
                            case 1: reltype = Lang.SS || '开工-开工(SS)';
                                break;
                            case 2: reltype = Lang.FF || '完工-完工(FF)';
                                break;
                            case 3: reltype = Lang.SF || '开工-完工(SF)';

                        }
                        return reltype;
                    }
                }, {
                    header: me.Lang.Es || '最早开始',
                    dataIndex: 'Es',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.Ls || '最晚开始',
                    dataIndex: 'Ls',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.Ef || '最早完成',
                    dataIndex: 'Ef',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.Lf || '最晚完成',
                    dataIndex: 'Lf',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.As || '实际开始',
                    dataIndex: 'As',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    	var date = Ext.util.Format.date( value, 'Y-m-d' );
                    	if ( date == "1900-01-01" )
                    		return "";
                    	else
                    		return date;
                    }
                }, {
                    header: me.Lang.Af || '实际完成',
                    dataIndex: 'Af',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    	var date = Ext.util.Format.date( value, 'Y-m-d' );
                    	if ( date == "1900-01-01" )
                    		return "";
                    	else
                    		return date;                        
                    }
                }, {
                    xtype: 'checkcolumn',
                    header: me.Lang.Asflag || '开始',
                    dataIndex: 'Asflag',
                    width: 60,
                    sortable: false,
                    hidden: false
                }, {
                    xtype: 'checkcolumn',
                    header: me.Lang.Afflag || '完成',
                    dataIndex: 'Afflag',
                    width: 60,
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.Pct || '完成百分比',
                    dataIndex: 'Pct',
                    align: 'right',
                    width: 80,
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.Remdur || '尚需工期',
                    dataIndex: 'Remdur',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.Actsort || '工序排序',
                    dataIndex: 'Actsort',
                    width: 100,
                    sortable: false,
                    hidden: true
                }, {
                    header: me.Lang.OrgBeginDt || '原始开始',
                    dataIndex: 'OrgBeginDt',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.OrgEndDt || '原始完工',
                    dataIndex: 'OrgEndDt',
                    width: 100,
                    sortable: false,
                    hidden: false,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return Ext.util.Format.date(value, 'Y-m-d');
                    }
                }, {
                    header: me.Lang.GclTotal || '工程总量',
                    dataIndex: 'GclTotal',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: false
                },
                //{
                //    header: me.Lang.GclUnit || '工程量单位Id',
                //    dataIndex: 'GclUnit',
                //    width: 80,
                //    sortable: false,
                //    hidden: true
                //},
                {
                    header: me.Lang.GclUnit || '工程量单位',
                    dataIndex: 'GclUnit_EXName',
                    width: 80,
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.GclPrice || '工程量单价',
                    dataIndex: 'GclPrice',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.GclPeriod || '本期完成量',
                    dataIndex: 'GclPeriod',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: true
                }, {
                    header: me.Lang.GclRemain || '剩余工程量',
                    dataIndex: 'GclRemain',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.GclFinish || '已完成工程量',
                    dataIndex: 'GclFinish',
                    width: 80,
                    align: 'right',
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.GclDrvFlag || '驱控',
                    dataIndex: 'GclDrvFlag',
                    width: 80,
                    sortable: false,
                    hidden: false,
                    renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                        if (data == 0) return (me.Lang.GclDrvFlag1 || '无驱控');
                        if (data == 1) return (me.Lang.GclDrvFlag2 || '工程量驱控');
                        if (data == 2) return (me.Lang.GclDrvFlag3 || '资源驱控');
                    }
                }, {
                    header: me.Lang.Remark || '备注',
                    dataIndex: 'Remark',
                    width: 380,
                    sortable: false,
                    hidden: false
                }, {
                    header: me.Lang.IsLeaf || '是否叶子',
                    dataIndex: 'IsLeaf',
                    width: 100,
                    sortable: false,
                    hidden: true
                }
            ]
            //bbar: pagingbar
        });

        me.grid = grid;

        var queryPanel = Ext.create('Ext.ng3.QueryPanel', {
            toolbar: ngToolbar,
            pageid: "Web:SpmEppactList", //对应内嵌查询业务点标识
            grid: me.grid,
            inPopWin:true,
            columnsPerRow: 4 //每行3列
        });

        me.items = [
            ngToolbar,
            {
                region: 'center',
                xtype: 'container',
                layout: 'border',
                split: true,
                border: false,
                items: [queryPanel, grid]
            }
        ];

        this.callParent(arguments);
        me.fireEvent("isready", me.Lang);
    },
    getHelpData: function () {

        var ret = new Object()

        var me = this;

        var code = "";
        var name = "";
        var pobj = new Object();
        var df = me.displayField;
        var vf = me.valueField;
        //var select1 = win.queryById("westgrid").store.data.items;
        var select = me.grid.getSelectionModel().getSelection();
        if (select.length <= 0) {
            ret['status'] = 0
            return ret;
        }
        Ext.Array.each(select, function (model) {
            if (model.data["IsLeaf"] == "0") {
                ret['status'] = -1
            }
            if (me.needrel) {
                if (Ext.isEmpty(model.data["Lagtype"]))
                    ret['status'] = -2
            };
                
            code = code + model.data[vf] + ",";
            name = name + model.data[df] + ",";
        });
        code = code.substring(0, code.length - 1);
        name = name.substring(0, name.length - 1);
        //pobj.data = Ext.decode(select[0].data.row);
        var obj = new Object();
        obj[vf] = code;
        obj[df] = name;

        Ext.define('richhelpModel', {
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

        //var valuepair = Ext.create('richhelpModel', obj);
        //me.setValue(valuepair); //必须这么设置才能成功

        pobj.code = code;
        pobj.name = name;
        pobj.type = 'fromhelp';
        pobj.data = select;
        me.seletArr = select;

        if (!ret['status'])
            ret['status'] = 1
        ret['data'] = pobj

        return ret;
    }
});