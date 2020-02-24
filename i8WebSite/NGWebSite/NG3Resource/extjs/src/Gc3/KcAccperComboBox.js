//核算期帮助gcKcAccperlHelp  Ext.GcKcAccper.ComboBox
Ext.define('Ext.Gc3.KcAccperComboBox', {
    extend: 'Ext.ng.ComboBox',
    alias: 'widget.gcKcAccperlHelp',
    ORMMode: true,
    helpid: 'pmm3.kc_accper',
    valueField: 'PhId',
    displayField: 'Accper',
    listFields: 'Uyear,Accper,PhId',
    listHeadTexts: '核算年,核算期',
    showHeader: true,
    MaxLength: 100,
    editable: false,
    mustInput: false,
    setUyear: function (newValue) {
        ////设置按核算年过滤
        var me = this;

        if (!Ext.isEmpty(newValue)) {
            me.setOutFilter({ uyear: newValue });
        }
    }
});