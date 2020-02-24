//银行帮助
Ext.define('Ext.Gc3.BankHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcBankHelp',
    ORMMode: true,
    helpid: 'ecc3_bank',
    valueField: 'PhId',
    displayField: 'BankName',
    MaxLength: 100,
    editable: false,
    mustInput: false
});