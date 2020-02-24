//分包范围帮助    Ext.Fbfw.RichHelp
Ext.define('Ext.Gc3.Fbfw', {
    extend: 'Ext.ng.RichHelp',
    name: 'PhidFbfw',
    helpid: 'pcm3.pcm3_cnt_fbfwbase',
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