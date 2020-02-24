//所有计量单位帮助窗口gcPUnitRichHelp
Ext.define('Ext.Gc3.PUnit', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcPUnitRichHelp',
    name: 'Unit',
    helpid: 'pms3.p_msunit',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'Msname',
    MaxLength: 100,
    editable: false,
    mustInput: false
});
