//班组帮助gcTeamsGrRichHelp Ext.TeamsGr.RichHelp
Ext.define('Ext.Gc3.TeamsGr', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.gcTeamsGrRichHelp',
    helpid: 'pms3.teams_gr',
    ORMMode: true,
    valueField: 'PhId',
    displayField: 'TeamsName',
    userCodeField: "Code",
    //listFields: '',
    //listHeadTexts: '',
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1,
    setPc: function (pcid) {
        ////设置按项目过滤
        var me = this;

        if (!Ext.isEmpty(pcid)) {
            me.setOutFilter({ pcid: pcid });
        }
    }
});