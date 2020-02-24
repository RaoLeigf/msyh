var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTZFFS",
      "desTitle": "支付方式编辑",
      "columnsPerRow": 2,
      "fields": [
        {
          "fieldLabel": "代码",
          "itemId": "Dm",
          "name": "Dm",
          "id":"DM",
          "maxLength": 30,
          "langKey": "Dm",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "名称",
          "itemId": "Mc",
          "name": "Mc",
          "maxLength": 255,
          "langKey": "Mc",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "备注",
          "itemId": "Bz",
          "name": "Bz",
          "maxLength": 100,
          "langKey": "Bz",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "主键",
          "itemId": "PhId",
          "name": "PhId",
          "hidden": true,
          "maxLength": 19,
          "langKey": "PhId",
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
    "zffsList": {
      "id": "zffsList",
      "buskey": "PhId",
      "bindtable": "Z_QTZFFS",
      "desTitle": "支付方式列表",
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
          "hidden": true,
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Dm",
          "dataIndex": "Dm",
          "width": 100,
          "header": "代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Mc",
          "dataIndex": "Mc",
          "width": 100,
          "header": "名称",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Bz",
          "dataIndex": "Bz",
          "width": 100,
          "header": "备注",
          "editor": {
            "xtype": "ngText"
          }
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
