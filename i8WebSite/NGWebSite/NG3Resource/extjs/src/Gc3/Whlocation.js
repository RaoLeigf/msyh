//库位帮助gcWhlocationRichHelp  Ext.Whlocation.RichHelp
Ext.define('Ext.Gc3.Whlocation', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcWhlocationRichHelp',
    helpid: 'pmm3.whlocation',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Whname',
    userCodeField: "Whlocation",
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: true,
    mustInput: false,
    setWarehouse: function (newValue) {
        ////设置按仓库过滤
        var me = this;
        if (!Ext.isEmpty(newValue)) {
            if (me.outFilter) {
                Ext.apply(me.outFilter, {
                    warehouseid: newValue
                });
            } else {
                me.outFilter = {
                    warehouseid: newValue
                }
            }
        } else {
            if (me.outFilter) {
                delete me.outFilter.warehouseid;
            } else {
                me.outFilter = {}
            }
        }
        me.lastQuery = null;
    }
});