//劳务工信息帮助
Ext.define('Ext.Gc3.CraftInfoHelp', {
    extend: 'Ext.ng.RichHelp',
    alias: 'widget.CraftInfoHelp',
    helpid: 'pms3.pms3_yg_craft_info',
    valueField: 'PhId',
    displayField: 'CraftName',
    listFields: 'CraftNo,CraftName,PhId,CraftNo,CraftName,PhidTeamsgrId,PhidCrafttype,TeamsName,Name',
    listHeadTexts: '劳务工号,劳务工名称',
    editable: false,
    ORMMode: true,
    needExtend:false,
    setPc: function (pcid) {
        ////设置按核算年过滤
        var me = this;

        if (!Ext.isEmpty(pcid)) {
            me.setOutFilter({ phid_pc: pcid });
        }
    },
    initComponent: function () {
        var me = this;
        me.addEvents(
            'helpselectedEx'
        );
        me.on('helpselected', function (data) {
            var you = this;
            you.data = data;
            if (true) {
                Ext.Ajax.request({
                    params: { 'craftid': data.code },
                    url: C_ROOT + "PMS/PMS/YgCraftInfo/GetNewestEnterExit",
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.length > 0) {
                            var ExData = you.data;
                            ExData.data.PhidCraftComp = resp[0].PhidCraftComp;
                            ExData.data.PhidPc = resp[0].PhidPc;
                            ExData.data.PhidCraftTeamId = resp[0].PhidCraftTeamId;
                            ExData.data.PhidTeamsgrId = resp[0].PhidTeamsgrId;
                            me.fireEvent('helpselectedEx', ExData);
                        } else {
                            //Ext.MessageBox.alert('取数失败', resp.Msg);
                            me.fireEvent('helpselectedEx', you.data);
                        }
                    }
                });
            }
        });
        me.callParent();
    }
});