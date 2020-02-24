//进度帮助字段
Ext.define('Ext.Spm.EppactInfoHelpField', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.SpmEppactInfoHelpField',
    helpid: 'pms3.spm3_eppact',
    valueField: 'PhId',
    displayField: 'Name',
    listFields: 'UserDefinedActcode,Name,PhId',
    listHeadTexts: '作业编码,作业名称', //智能搜索列头 
    pcid: 0,
    wbs: '',
    wbslist: '',
    isCheck: true,
    isFinish: false,
    isCal: true,
    ORMMode: true,
    muilt: false,//允许多选
    matchFieldWidth: false,
    initComponent: function () {
        var me = this;

        me.callParent();

        me.store.on('beforeload', function (store) {

            me.defaultclientSqlFilter = " mstid = " + me.pcid;

            me.clientSqlFilter = me.defaultclientSqlFilter;

            Ext.apply(me.store.proxy.extraParams, { 'clientSqlFilter': me.clientSqlFilter });

        });
    },
    onTriggerClick: function () {
        var me = this;
        if (me.readOnly) return;

        me.fireEvent('beforetriggerclick', me);

        var help = Ext.create('Ext.Spm.EppactInfoHelpWindow', { pcid: me.pcid, wbs: me.wbs, wbslist: me.wbslist, isCheck: me.isCheck, isFinish: me.isFinish, isCal: me.isCal });
        help.on('helpselected', function (data) {

            help.close();
            me.fireEvent('helpselected', data);
        }
        );
        help.show();
    }
});