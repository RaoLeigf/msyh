//品牌帮助  BrandRichHelp
Ext.define('Ext.Gc3.Brand', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.BrandHelp',
    helpid: 'pmm3.brand',  
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});
