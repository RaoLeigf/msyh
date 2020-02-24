//检查阶段帮助gcChkMomentRichHelp Ext.GcChkMoment.RichHelp
Ext.define('Ext.Gc3.ChkMoment', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcChkMomentRichHelp',
    helpid: 'pms3.bs_chk_moment',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'ItemName',
    MaxLength: 100,
    editable: false,
    mustInput: false,
});