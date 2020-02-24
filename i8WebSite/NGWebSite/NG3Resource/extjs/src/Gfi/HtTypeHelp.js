
Ext.define('Ext.Gfi.HtTypeHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: ['widget.gfiHtTypeHelp'],
    valueField: 'code',
    displayField: 'name',
    type: '',
    cntmodel: 0,//合同必传模型参数
    pc: '',//wbs,cbs必传pc参数
    compNo: '',
    pcname: '',
    initComponent: function () {
        var me = this;
        var help = {};
        me.addEvents('beforetriggerclick');
        this.callParent();
    },
    onTriggerClick: function () {
        var me = this;
        if (!me.fireEvent('beforetriggerclick', me)) { return false };
        //var help = {};
        switch (me.type) {
            case '1'://I8
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'ecc_ht_type_i8',
                    ORMMode: true,
                    valueField: 'PhId',
                    displayField: 'Name',
                    listFields: 'PhId,Remarks,Name',
                    listHeadTexts: '主键,备注,名称',
                });
                help.showHelp();
                break;
            case '2'://I6S I6
                help = Ext.create('Ext.ng.RichHelp', {
                    helpid: 'ecc_ht_type_i6s',
                    ORMMode: true,
                    valueField: 'id',
                    displayField: 'subtypename',
                    listFields: 'id,subtypecode,subtypename',
                    listHeadTexts: '主键,合同类型,类型名称',
                });
                help.showHelp();
                break;
            case '3'://工程收入类合同
                help = Ext.create('Ext.cnt.CntInfoHelpField', { cntmodel: me.cntmodel, lang: { 'angd': '222' } });
                help.showHelp();
                break;
            case '4'://SCM合同
                var win = Ext.create('Ext.ng.ContractCostHelpWindow', {
                    exceptend: true,
                    compNo: me.compNo,
                    pc: me.pc,
                    pcname: me.pcname
                });
                win.show();
                //var leftBar = document.getElementsByClassName('x-component x-header-text-container x-window-header-text-container x-window-header-text-container-default x-box-item x-component-default')[0];
                var rightBar = document.getElementsByClassName('x-tool x-box-item x-tool-default x-tool-after-title')[0];
                var left = rightBar.offsetLeft - 5;
                rightBar.setAttribute('style', 'width: 15px; height: 15px; right: auto; left: ' + left + 'px; top: 0px; margin: 0px;')
                win.on('helpselected', function (obj, status) {
                    me.fireEvent('helpselected', obj, status);
                });
                return;
        }
        if (!me.fireEvent('beforetriggerclick', me)) return;

        help.on('helpselected', function (data) {
            Ext.define('richhelpModel', {
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
            });//定义模型
            var obj = new Object();
            obj['code'] = data.code;//加类型之后的值
            obj['name'] = data.name;
            var valuepair = Ext.create('richhelpModel', obj);//模型赋值
            me.setValue(valuepair); //控件赋值
            me.fireEvent('helpselected', data);
        });
    },
    showHelp: function () {
        this.onTriggerClick();
    },
    //给控件赋type（打开帮助前必须赋值）
    setType: function (type) {
        this.type = type;
    },
    //wbs,cbs必传pc参数
    setPc: function (pc) {
        this.pc = pc
    },
    setClientSqlFilter: function (sql) {
        help.setClientSqlFilter(sql);
    },
    //合同必传模型参数
    setCntmodel: function (cntmodel) {
        this.cntmodel = cntmodel
    }
})