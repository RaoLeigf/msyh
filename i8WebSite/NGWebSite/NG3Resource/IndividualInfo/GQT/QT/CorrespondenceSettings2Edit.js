var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTDYGX2",
      "desTitle": "GX2",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "单位代码",
          "itemId": "Dwdm",
          "name": "Dwdm",
          "maxLength": 255,
          "langKey": "Dwdm",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "对应代码",
          "itemId": "Dydm",
          "name": "Dydm",
          "maxLength": 255,
          "langKey": "Dydm",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "对应类型",
          "itemId": "Dylx",
          "name": "Dylx",
          "maxLength": 10,
          "langKey": "Dylx",
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
    "DYZCLB": {
      "id": "DYZCLB",
      "buskey": "PhId",
      "bindtable": "Z_QTDYGX2",
      "desTitle": "DYZCLB",
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
          "hidden": true,
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Dwdm",
          "dataIndex": "Dwdm",
          "width": 100,
          "header": "单位代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Dydm",
          "dataIndex": "Dydm",
          "width": 100,
          "header": "对应代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "Dylx",
          "dataIndex": "Dylx",
          "width": 100,
          "header": "对应类型",
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
