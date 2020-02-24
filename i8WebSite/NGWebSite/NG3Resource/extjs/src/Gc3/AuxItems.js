//弹性辅助项帮助gcAuxItemsRichHelp Ext.AuxItems.RichHelp
Ext.define('Ext.Gc3.AuxItems', {
    extend: 'Ext.Gfi.ExtendHelp',
    alias: 'widget.gcAuxItemsRichHelp',
    ORMMode: true,
    MaxLength: 100,
    editable: false,
    mustInput: false,
    colspan: 1,

    initComponent: function() {
        var me = this;

        this.callParent();
    },
    onTriggerClick:function() {
        var me = this;

        if (me.readOnly === true) {
            return;
        }

        if (!me.isInitType) {
            Ext.Ajax.request({
                params: {
                    'buscode': me.bustype,
                    'containerid': me.containerid,
                    'field': me.fieldName
                },
                async: false, //同步请求
                url: C_ROOT + 'GFI/GC/GL/Gfi3BusRegister/GetExtendType',
                success: function(response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.success === "true") {
                        me.setType(String(resp.type));
                        me.isInitType = true;
                    }
                }
            });
        }

        this.callParent();
    }
});