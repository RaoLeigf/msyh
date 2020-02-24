var individualConfigInfo = 
{
  "form": {
    "main": {
      "id": "main",
      "buskey": "PhId",
      "bindtable": "z_qtaccount",
      "desTitle": "main",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "账套",
          "itemId": "Dm",
          "name": "Dm",
          "maxLength": 30,
          "langKey": "Dm",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "数据库连接串",
          "itemId": "FConn",
          "name": "FConn",
          "maxLength": 255,
          "langKey": "FConn",
          "xtype": "ngText"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "PhId"
            },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    }
  },
  "grid": {
    "main": {
      "id": "main",
      "buskey": "PhId",
      "bindtable": "z_qtaccount",
      "desTitle": "main",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "Dm",
          "dataIndex": "Dm",
          "width": 200,
          "header": "核算账套"
        },
        {
          "LangKey": "FConn",
          "dataIndex": "FConn",
          "width": 500,
          "header": "数据库连接串"
        },
        {
          "LangKey": "IsDefault",
          "dataIndex": "IsDefault",
          "width": 200,
          "header": "是否默认",
          // "editor": {
          //     "xtype": "ngCheckbox"
          // }
          "xtype": 'ngcheckcolumn',
          'checkedVal': 1,
          'unCheckedVal': 0
        },
        {
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 100,
          "header": "主键",
          "hidden":true
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
