var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTJFQD",
      "desTitle": "zjlywh1",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "代码",
          "itemId": "DM",
          "name": "DM",
		  "id":"DM",
          "maxLength": 30,
          "langKey": "DM",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "名称",
          "itemId": "MC",
          "name": "MC",
		  "id":"MC",
          "maxLength": 255,
          "langKey": "MC",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "备注",
          "itemId": "BZ",
          "name": "BZ",
          "maxLength": 40,
          "langKey": "BZ",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "主键",
          "itemId": "PhId",
          "name": "PhId",
          "maxLength": 19,
          "langKey": "PhId",
          "xtype": "hiddenfield"
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
    "资金来源表": {
      "id": "资金来源表",
      "buskey": "PhId",
      "bindtable": "Z_QTJFQD",
      "desTitle": "ZJLYlist",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
              "width": 50
        },
        {
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 100,
          "header": "主键",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "DM",
          "dataIndex": "DM",
          "width": 200,
          "header": "代码"
        },
        {
          "LangKey": "MC",
          "dataIndex": "MC",
          "width": 200,
          "header": "名称"
        },
        {
          "LangKey": "BZ",
          "dataIndex": "BZ",
          "width": 500,
          "header": "备注"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
