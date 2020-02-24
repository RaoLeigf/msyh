var individualConfigInfo = 
{
  "form": {
    "form": {
      "id": "form",
      "buskey": "PhId",
      "bindtable": "z_qtdtlstage",
      "desTitle": "form",
      "columnsPerRow": 1,
      "fields": [
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
          "fieldLabel": "代码",
          "itemId": "Dm",
          "name": "Dm",
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
          "maxLength": 400,
          "langKey": "Bz",
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
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    }
  },
  "grid": {
    "grid": {
      "id": "grid",
      "buskey": "PhId",
      "bindtable": "z_qtdtlstage",
      "desTitle": "grid",
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
          "hidden": true
        },
        {
          "LangKey": "Dm",
          "dataIndex": "Dm",
          "width": 100,
          "header": "代码"
        },
        {
          "LangKey": "Mc",
          "dataIndex": "Mc",
          "width": 100,
          "header": "名称"
        },
        {
          "LangKey": "Bz",
          "dataIndex": "Bz",
          "width": 100,
          "header": "备注"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
