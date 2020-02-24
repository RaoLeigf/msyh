//统计周期帮助gcWorkCycleRichHelp(周)  Ext.WorkCycleWeek.RichHelp
Ext.define('Ext.Gc3.WorkCycleWeek', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWorkCycleWeekRichHelp',
    name: 'PhidCycle',
    itemId: 'PhidCycle',
    helpid: 'WorkCycle',
    outFilter: { CType: 'GCWEEK' },
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'CName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1,
    setUppid: function (newValue) {
        ////按主表周期设置过滤
        var me = this;

        if (!Ext.isEmpty(newValue) && newValue > 0) {
            me.setOutFilter({ upphid: newValue });
        }
    },
    setDate: function (newValue) {
        var me = this;
        me.setClientSqlFilter(newValue);
    }
});