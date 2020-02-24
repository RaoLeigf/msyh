//整改方式帮助gcCorrTypeRichHelp  Ext.GcCorrType.RichHelp
Ext.define('Ext.Gc3.CorrType', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcCorrTypeRichHelp',
    helpid: 'pms3.bs_corr_type',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});