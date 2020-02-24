//事务特性组帮助gcPropertyGroupRichHelp
Ext.define('Ext.Gc3.PropertyGroup', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPropertyGroupRichHelp',
    helpid: 'pms3.pms3_item_property_group_m',
    //outFilter: {},
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Name',
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false
});