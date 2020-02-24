var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTYSXZ",
        "desTitle": "项目类型",
      "columnsPerRow": 2,
      "fields": [
        {
          "fieldLabel": "代码",
          "itemId": "Dm",
          "name": "Dm",
          "maxLength": 30,
          "langKey": "Dm",
          "xtype": "ngText",
          "id":"Dm"
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
          "maxLength": 40,
          "langKey": "Bz",
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
    "zclbList": {
      "id": "zclbList",
      "buskey": "PhId",
      "bindtable": "Z_QTYSXZ",
        "desTitle": "项目类型列表",
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
          "width": 200,
          "header": "主键",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Dm",
          "dataIndex": "Dm",
          "width": 200,
          "header": "代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Mc",
          "dataIndex": "Mc",
          "width": 200,
          "header": "名称",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Bz",
          "dataIndex": "Bz",
          "width": 200,
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
