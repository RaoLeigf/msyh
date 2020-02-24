//标准计量单位帮助窗口gcUnitRichHelp
Ext.define('Ext.Gc3.Unit', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcUnitRichHelp',
    name: 'Unit',
    helpid: 'pms3.msunit',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Msname',
    listFields: 'Msunit,Msname,PhId',
    listHeadTexts: '编码,名称',
    MaxLength: 100,
    editable: true,
    mustInput: false
});