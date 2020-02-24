storeProduct = Ext.create('Ext.ng.JsonStore',{
				autoLoad:false,
				fields: [
                    {
                        "name": "PhId",
                        "type": "int",
                        "mapping": "PhId"
                    },
                    {
                        "name": "ProductBZ",
                        "type": "string",
                        "mapping": "ProductBZ"
                    }
                ],
				url: C_ROOT + 'GQT/QT/QTProduct/GetQTProductList'
			});
			
var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTProductUser",
      "desTitle": "mainPanel",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "主键",
          "itemId": "PhId",
          "name": "PhId",
          "maxLength": 19,
          "langKey": "PhId",
          "xtype": "ngText",
		  "hidden":true
        },
        {
          "fieldLabel": "产品主键",
          "itemId": "ProductPhid",
          "name": "ProductPhid",
          "maxLength": 19,
          "langKey": "ProductPhid",
          "xtype": "ngText",
		  "hidden":true
        },
        {
          "fieldLabel": "产品标识",
          "itemId": "ProductBZ",
          "name": "ProductBZ",
          "maxLength": 20,
          "langKey": "ProductBZ",
          "xtype": "ngComboBox",
		  "store":storeProduct,
		  "editable":false,
		  "valueField":'ProductBZ',
		  "displayField":'ProductBZ',
		  "mustInput": true
        },
        {
          "fieldLabel": "操作员代码",
          "itemId": "ProductUserCode",
          "name": "ProductUserCode",
          "maxLength": 50,
          "langKey": "ProductUserCode",
          "xtype": "ngText",
		  "mustInput": true
        },
        {
          "fieldLabel": "操作员名称",
          "itemId": "ProductUserName",
          "name": "ProductUserName",
          "maxLength": 100,
          "langKey": "ProductUserName",
          "xtype": "ngText",
		  "mustInput": true
        },
        {
          "fieldLabel": "操作员密码",
          "itemId": "ProductUserPwd",
          "name": "ProductUserPwd",
          "maxLength": 200,
          "langKey": "ProductUserPwd",
          "xtype": "ngText"
		  //"mustInput": true,
        },
        {
          "fieldLabel": "备注",
          "itemId": "BZ",
          "name": "BZ",
          "maxLength": 200,
          "langKey": "BZ",
          "xtype": "ngText"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "记录版本",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    }
  },
  "grid": {
    "QTProductUser": {
      "id": "QTProductUser",
      "buskey": "PhId",
      "bindtable": "Z_QTProductUser",
      "desTitle": "QTProductUser",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 100,
          "header": "主键",
		  "hidden":true
        },
        {
          "LangKey": "ProductPhid",
          "dataIndex": "ProductPhid",
          "width": 100,
          "header": "产品主键",
		  "hidden":true
        },
        {
          "LangKey": "ProductBZ",
          "dataIndex": "ProductBZ",
          "width": 100,
          "header": "产品标识"
        },
        {
          "LangKey": "ProductUserCode",
          "dataIndex": "ProductUserCode",
          "width": 100,
          "header": "操作员代码"
        },
        {
          "LangKey": "ProductUserName",
          "dataIndex": "ProductUserName",
          "width": 100,
          "header": "操作员名称"
        },
        {
          "LangKey": "ProductUserPwd",
          "dataIndex": "ProductUserPwd",
          "width": 100,
          "header": "操作员密码"
        },
        {
          "LangKey": "BZ",
          "dataIndex": "BZ",
          "width": 100,
          "header": "备注"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
