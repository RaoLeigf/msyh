
//辅助项专用帮助
//type为必传参数，cntmodel为打开合同帮助时必传参数，pc为打开WBS,CBS帮助时必传参数
Ext.define('Ext.Gfi.ExtendHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: ['widget.gfiExtendHelp'],
    valueField: 'code',
    displayField: 'name',
    type: '',
    codeType: '',
    cntmodel: 0,//合同必传模型参数
    pc: '',//wbs,cbs必传pc参数
    onTriggerClick: function () {
        var me = this;
        if (!me.fireEvent('beforetriggerclick',me)) {
            return false;
        }
        var help = {};
        switch (me.type) {
            case '1'://部门
                me.codeType = '001';
                help = Ext.create('Ext.ng.DeptHelp');
                help.showHelp();
                break;
            case '2'://员工
                me.codeType = '002';
                help = Ext.create('Ext.ng.EmpHelp');
                help.showHelp();
                break;
            case '3'://辅助项
                me.codeType = '003';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_fg_item',
                    ORMMode: false,
                    valueField: 'proj_code',
                    displayField: 'proj_name',
                    listFields: 'proj_code,proj_name',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '4'://往来单位
                me.codeType = '004';
                help = Ext.create('Ext.ng.EnterpriseHelp');
                help.showHelp();
                break;
            case '5'://客户
                me.codeType = '005';
                help = Ext.create('Ext.ng.CustomFileHelp');
                help.showHelp();
                break;
            case '6'://供应商
                me.codeType = '006';
                help = Ext.create('Ext.ng.SupplyFileHelp');
                help.showHelp();
                break;
            case '7':
                me.codeType = '001';
                help = Ext.create('Ext.Gfi.OtherHelp');
                help.show();
                break;
            case '8':
                me.codeType = '001';
                help = Ext.create('Ext.Gfi.OtherHelp');
                help.show();
                break;
            case '9'://币别
                me.codeType = '009';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_fg_fcur',
                    ORMMode: true,
                    valueField: 'FcCode',
                    displayField: 'FcName',
                    listFields: 'FcCode,FcName',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '10'://国别
                me.codeType = '010';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_nation',
                    ORMMode: true,
                    valueField: 'NationNo',
                    displayField: 'NatName',
                    listFields: 'NationNo,NatName',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '11'://省份
                me.codeType = '011';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_fg3_region',
                    ORMMode: true,
                    valueField: 'CNo',
                    displayField: 'CName',
                    listFields: 'CNo,CName',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '12'://资源
                me.codeType = '012';
                help = Ext.create('Ext.res.ItemDataHelpField');
                help.onTriggerClick();
                break;
            case '13'://项目信息
                me.codeType = '013';
                help = Ext.create('Ext.ng.ProjectHelp');
                help.showHelp();
                break;
            case '14'://
                me.codeType = '001';
                help = Ext.create('Ext.Gfi.OtherHelp');
                help.show();
                break;
            case '15':
                me.codeType = '001';
                help = Ext.create('Ext.Gfi.OtherHelp');
                help.show();
                break;
            case '16'://资源分类
                me.codeType = '016';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'pmm3.res_bs',
                    ORMMode: true,
                    valueField: 'PhId',
                    displayField: 'Name',
                    listFields: 'Code,Name',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '17'://WBS
                me.codeType = '017';
                help = Ext.create('Ext.wbs.WbsHelpField', { pc: me.pc });
                help.showHelp();
                break;
            case '18'://CBS
                me.codeType = '018';
                help = Ext.create('Ext.cbs.CbsHelpField', { pc: me.pc });
                help.showHelp();
                break;
            case '19':
                me.codeType = '001';
                help = Ext.create('Ext.ng.RichHelp');
                help.show();
                break;
            case '20'://工程收入类合同
                me.codeType = '020';
                help = Ext.create('Ext.cnt.CntInfoHelpField', { cntmodel: me.cntmodel });
                help.showHelp();
                break;
            case '21'://银行
                me.codeType = '021';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_uv_bank',
                    ORMMode: true,
                    valueField: 'bankno',
                    displayField: 'bankname',
                    listFields: 'bankno,bankname',
                    listHeadTexts: '代码,名称',
                });
                help.showHelp();
                break;
            case '22'://车辆信息
                me.codeType = '022';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'wm_uv_vehicle_main',
                    ORMMode: true,
                    valueField: 'cno',
                    displayField: 'vehicle_name',
                    listFields: 'cno,vehicle_name',
                    listHeadTexts: '代码,名称'
                });
                help.showHelp();
                break;
            case '23'://费用类型
                me.codeType = '023';
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_feetype',
                    ORMMode: true,
                    valueField: 'FeetypeId',
                    displayField: 'FeetypeName',
                    listFields: 'FeetypeId,FeetypeName',
                    listHeadTexts: '代码,名称'
                });
                help.showHelp();
                break;
            case '24':
                me.codeType = '001';
                help = Ext.create('Ext.ng.RichHelp');
                help.show();
                break;
            default:
                me.codeType = '999';//自定义辅助项类型
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'gfi_ast_proj',
                    ORMMode: true,
                    valueField: 'astprojno',
                    displayField: 'astprojname',
                    listFields: 'astprojno,astprojname',
                    listHeadTexts: '代码,名称',
                    outFilter: { 'GL_EXTEND_BINDOBJECT.id': me.type }
                });
                help.showHelp();
                break;
        }
        help.on('helpselected', function (data) {
            //Ext.define('richhelpModel', {
            //    extend: 'Ext.data.Model',
            //    fields: [{
            //        name: 'code',
            //        type: 'string',
            //        mapping: 'code'
            //    }, {
            //        name: 'name',
            //        type: 'string',
            //        mapping: 'name'
            //    }
            //    ]
            //});//定义模型
            //var obj = new Object();
            //obj['code'] = me.type + '@#' + data.code;//加类型之后的值
            //obj['name'] = data.code;
            //var valuepair = Ext.create('richhelpModel', obj);//模型赋值
            //me.setValue(valuepair); //控件赋值

            //data.code = me.type + '@#' + data.code;//加类型之后的值
            //data.oldVal = data.code;//加类型之前的原值
            me.fireEvent('helpselected', data);
        })
    },
    showHelp: function () {
        this.onTriggerClick();
    },
    //给控件赋type（打开帮助前必须赋值）
    setType: function (type) {
        this.type = String(type);
    },
    //wbs,cbs必传pc参数
    setPc: function (pc) {
        this.pc = pc
    },
    //合同必传模型参数
    setCntmodel: function (cntmodel) {
        this.cntmodel = cntmodel
    }
})
//mstform 
//buscode:业务类型
//containerid：容器id
//field：字段属性名
var SetExtendTypeByInfo = function (mstform, buscode, containerid, field) {
    Ext.Ajax.request({
        params: {
            'buscode': buscode,
            'containerid': containerid,
            'field': field
        },
        url: C_ROOT + 'GFI/GC/GL/Gfi3BusRegister/GetExtendType',
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            if (resp.success === "true") {
                mstform.queryById(field).setType(String(resp.type));
            }
        }
    });
}
//buscode:业务类型
//containerid：容器id
//field：字段属性名
var GetExtendTypeByInfo = function (buscode, containerid, field, callback) {
    Ext.Ajax.request({
        params: {
            'buscode': buscode,
            'containerid': containerid,
            'field': field
        },
        url: C_ROOT + 'GFI/GC/GL/Gfi3BusRegister/GetExtendType',
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            if (resp.success === "true") {
                if (callback) {
                    callback(String(resp.type));
                }
            }
        }
    });
}
//buscode:业务类型
var GetExtendTypeByBuscode = function (buscode, callback) {
    Ext.Ajax.request({
        params: {
            'buscode': buscode
        },
        url: C_ROOT + 'GFI/GC/GL/Gfi3BusRegister/GetExtendTypeByBuscode',
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            if (resp.success === "true") {
                if (callback) {
                    callback(resp.data);
                }
            }
        }
    });
}
//mstform
//buscode:业务类型
var SetExtendTypeByBuscode = function (mstform, buscode,callback) {
    Ext.Ajax.request({
        params: {
            'buscode': buscode
        },
        url: C_ROOT + 'GFI/GC/GL/Gfi3BusRegister/GetExtendTypeByBuscode',
        success: function (response) {
            var resp = Ext.JSON.decode(response.responseText);
            if (resp.success === "true") {
                if (resp.data.length > 0) {
                    Ext.each(resp.data, function (d) {
                        mstform.queryById(d.ctl_id).setType(d.phid_bindobj);
                    })
                }
                if (callback) {
                    callback(resp.data);
                }
            }
        }
    });
}