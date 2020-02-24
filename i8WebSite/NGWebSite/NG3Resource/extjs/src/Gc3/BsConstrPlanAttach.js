//施工平面图直接获取唯一附件帮助   Ext.BsConstrPlanAttach.RichHelp
Ext.define('Ext.Gc3.BsConstrPlanAttach', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.GcBsConstrPlanAttachHelp',
    helpid: 'pms3.bs_constr_plan_attach',
    ORMMode: true,
    valueField: 'phid',
    displayField: 'asr_name',
    MaxLength: 100,
    editable: false,
    mustInput: false
});