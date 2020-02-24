//往来单位地址信息帮助
Ext.define('Ext.Gc3.EnterpriseAddressHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.EnterpriseAddressHelpField',
    helpid: 'pms3.enterprise_address',
    ORMMode: true,
    valueField: 'ent_id',
    displayField: 'address',
    MaxLength: 100,
    editable: true,
    mustInput: false
});