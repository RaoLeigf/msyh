//资源需求单帮助gcProjResMRichHelp Ext.GcProjResM.RichHelp
Ext.define('Ext.Gc3.ProjResM', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcProjResMRichHelp',
    helpid: 'pms3.proj_res_m_bill',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'BillNo',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});