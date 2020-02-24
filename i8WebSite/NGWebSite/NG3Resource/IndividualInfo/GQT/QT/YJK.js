function MyYJK(gzl,Dept,Org){
	var InsertYJ=[];
	var DeleteYJ=[];
	var ChangeYJ=[];
	var storeYJ = Ext.create('Ext.ng.JsonStore', {
		autoLoad: false,
		fields: [{

	"name": "PhId",
	"type": "string",
	"mapping": "PhId"
	},
	{
	"name": "Text",
	"type": "string",
	"mapping": "Text"
		}],
		url: C_ROOT + 'GQT/QT/QtYJK/GetQtYJKList'
	});
	var pagingbarYJ = Ext.create('Ext.ng.PagingBar', {
		store: storeYJ
	});
	var YJKToolbar = Ext.create('Ext.ng.Toolbar', {
		region: 'north',
		ngbuttons: [
			'addrow','deleterow','save', { itemId: "close1", text: "关闭", width: this.itemWidth, iconCls: "icon-Close" }
		]
	});
	var gridYJ = Ext.create('Ext.ng.GridPanel', {
		columnWidth: .5,
		height:394,
		//height: 390,
		store: storeYJ,
		autoScroll: true,
		columnLines: true,
		border: false,
		bbar: [pagingbarYJ],
		//selModel: { mode: "SIMPLE" },
		columns: [{
			header: '主键',
			flex: 1,
			sortable: false,
			menuDisabled: true,
			draggable: false,
			dataIndex: 'PhId',
			hidden:true
		}, {
			header: '',
			flex: 1,
			sortable: false,
			menuDisabled: true,
			draggable: false,
			dataIndex: 'Text',
			editor: {
				xtype: "textfield"
			}
		}],
		listeners: {
			
		},
		viewConfig: {
			style: {
				overflowX: 'hidden !important'
			}
		},
		plugins: [
			Ext.create('Ext.grid.plugin.CellEditing', {
				clicksToEdit: 1,    //单击编辑，单元格修改
				autoEncode: false  //不解析成html
			})
		]
	});

	var YJKwin = Ext.create('Ext.window.Window', {
		title: '意见库维护',
		height: 500,
		width: 600,
		modal: true,
		//closable:true,
		//closeAction:'hide',
		items: [
			YJKToolbar,
			gridYJ
		],
		buttons: [
			{
				xtype: "button",
				text: "确认",
				handler: function () {
					YJKwin.close();
					SelectYJK(gzl,Dept,Org);
				}
			},
			{
				xtype: "button",
				text: "取消",
				handler: function () {
					YJKwin.close();
				}
			}
		]

	});
	storeYJ.load();
	YJKwin.show();
	
	YJKToolbar.items.get('addrow').on('click', function () {
		var YJData = gridYJ.getSelectionModel();
		storeYJ.insert(storeYJ.getCount(), YJData);
	});
	
	YJKToolbar.items.get('deleterow').on('click', function () {
		var selection = gridYJ.getSelectionModel().getSelection();
		var phid = selection[0].data.PhId;
		if (!phid || phid.length == 0) {

		} else {
			DeleteYJ.push(phid);
		}
		storeYJ.remove(selection);
	});
	
	YJKToolbar.items.get('save').on('click', function () {
		for(var i=0;i<storeYJ.getModifiedRecords().length;i++){
			var phid=storeYJ.getModifiedRecords()[i].data.PhId;
			var text=storeYJ.getModifiedRecords()[i].data.Text;
			if(phid==""){
				InsertYJ.push({'Text':text,'Usercode':$user.id,'Dept':Dept,'Org':Org});
			}else{
				ChangeYJ.push({'PhId':phid,'Text':text});
			}
		}
		Ext.Ajax.request({
			params: { "DeleteYJ": DeleteYJ, "ChangeYJ": ChangeYJ,"InsertYJ":InsertYJ},
			url: C_ROOT + 'GQT/QT/QtYJK/Update1',
			success: function (response) {
				var resp = Ext.JSON.decode(response.responseText);
				if (resp.Status === "success") {
					storeYJ.load();
					InsertYJ=[];
					DeleteYJ=[];
					ChangeYJ=[];
					Ext.MessageBox.alert('提示', "保存成功");
				} else {
					Ext.MessageBox.alert('保存失败', resp.Msg);
				}
			}
		});
		
	});
	
	YJKToolbar.items.get('close1').on('click', function () {
		YJKwin.close();
	});
}

function SelectYJK(gzl,Dept,Org){
	var storeYJ = Ext.create('Ext.ng.JsonStore', {
		autoLoad: false,
		fields: [{

	"name": "PhId",
	"type": "string",
	"mapping": "PhId"
	},
	{
	"name": "Text",
	"type": "string",
	"mapping": "Text"
		}],
		url: C_ROOT + 'GQT/QT/QtYJK/GetQtYJKList'
	});
	var pagingbarYJ = Ext.create('Ext.ng.PagingBar', {
		store: storeYJ
	});
	var gridYJ = Ext.create('Ext.ng.GridPanel', {
		columnWidth: .5,
		height:434,
		//height: 390,
		store: storeYJ,
		autoScroll: true,
		columnLines: true,
		border: false,
		bbar: [pagingbarYJ],
		//selModel: { mode: "SIMPLE" },
		columns: [{
			header: '主键',
			flex: 1,
			sortable: false,
			menuDisabled: true,
			draggable: false,
			dataIndex: 'PhId',
			hidden:true
		}, {
			header: '',//意见
			flex: 1,
			sortable: false,
			menuDisabled: true,
			draggable: false,
			dataIndex: 'Text',
			editor: {
				xtype: "textfield"
			}
		}],
		listeners: {
			'itemdblclick': function (item, record, it, index, e, eOpts) {
				Ext.Ajax.request({
					params: { "id": record.get('PhId')},
					url: C_ROOT + 'GQT/QT/QtYJK/UpdateNum',
					async:false,
					success: function (response) {
						
					}
				});
				YJKwin.close();
				gzl.setYJ(record.get('Text'));
			}
		},
		viewConfig: {
			style: {
				overflowX: 'hidden !important'
			}
		}
	});

	var YJKwin = Ext.create('Ext.window.Window', {
		title: '我的意见库',
		height: 500,
		width: 600,
		modal: true,
		//closable:true,
		//closeAction:'hide',
		items: [
			gridYJ
		],
		buttons: [
			{
				xtype: "button",
				text: "维护",
				margin:'0 340 0 0',
				handler: function () {
					YJKwin.close();
					MyYJK(gzl,Dept,Org);
				}
			},
			{
				xtype: "button",
				text: "确认",
				handler: function () {
					var select = gridYJ.getSelectionModel().getSelection();
					Ext.Ajax.request({
						params: { "id": select[0].data.PhId},
						url: C_ROOT + 'GQT/QT/QtYJK/UpdateNum',
						async:false,
						success: function (response) {
							
						}
					});
					YJKwin.close();
					gzl.setYJ(select[0].data.Text);
				}
			},
			{
				xtype: "button",
				text: "取消",
				handler: function () {
					YJKwin.close();
				}
			}
		]

	});
	storeYJ.load();
	YJKwin.show();

}


