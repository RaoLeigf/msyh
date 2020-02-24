var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTZCGNFl",
      "desTitle": "编辑",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "科目代码",
          "itemId": "KMDM",
          "name": "KMDM",
          "maxLength": 20,
          "langKey": "KMDM",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "科目名称",
          "itemId": "KMMC",
          "name": "KMMC",
          "maxLength": 100,
          "langKey": "KMMC",
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
    "KMList": {
      "id": "KMList",
      "buskey": "PhId",
      "bindtable": "Z_QTZCGNFl",
      "desTitle": "科目列表",
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
          "hidden": true
        },
        {
          "LangKey": "KMDM",
          "dataIndex": "KMDM",
          "width": 100,
          "header": "科目代码"
        },
        {
          "LangKey": "KMMC",
          "dataIndex": "KMMC",
          "width": 100,
          "header": "科目名称"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
