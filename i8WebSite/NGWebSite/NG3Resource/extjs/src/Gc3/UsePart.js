//使用部位帮助    Ext.UsePart.RichHelp
Ext.define('Ext.Gc3.UsePart', {
    extend: 'Ext.ng.RichHelp',

    alias: 'widget.UsePartHelp',
    helpid: 'pmm3.usepart',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    userCodeField: 'code',//用户代码
    displayField: 'UsepartName',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: true,
    mustInput: false
});