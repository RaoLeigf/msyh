Ext.define('Ext.Gc.BsDataGrid', {
    extend: 'Ext.Gc.EditGrid',
    
    deleterow: function () {
        var me = this;
        var data = me.getSelectionModel().getSelection();
        var str_ids = "";        
                
        if (data.length > 0) {            
            for (var i = 0; i < data.length; i++) {
                if (!Ext.isEmpty(data[i].data.PhId))
                    str_ids = str_ids + data[i].data.PhId + ";"
            }

            Ext.Ajax.request({
                params: {
                    'del_ids': str_ids
                },
                url: C_PATH + 'CheckBeforeDel',
                async: false, //同步请求
                success: function (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    if (resp.Status == "true") {
                        Ext.Array.each(data, function (record) {
                            me.store.remove(record); //前端删除
                        });
                    } else {
                        Ext.MessageBox.alert(CommLang.Notes || "提示", resp.Msg);
                    }
                }
            });

        } else {
            return;
        }
    },
})