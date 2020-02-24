//工程量类型帮助   Ext.ProjType.RichHelp
Ext.define('Ext.Gc3.ProjType', {
    extend: 'Ext.ng.RichHelp',
    name: 'PhidProjType',
    alias: 'widget.gcProjTypeRichHelp',
    helpid: 'pcm3.pcm3_cnt_projtype',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    listFields: 'Name,PhId',
    listHeadTexts: '名称', //智能搜索列头 
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: true,
    mustInput: false
});