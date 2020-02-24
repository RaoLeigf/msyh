var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTGKXM",
      "desTitle": "编辑面板",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "项目代码",
          "itemId": "DM",
          "name": "DM",
          "maxLength": 255,
          "langKey": "DM",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "项目名称",
          "itemId": "MC",
          "name": "MC",
          "maxLength": 250,
          "langKey": "MC",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "父节点代码",
          "itemId": "DEFSTR2",
          "name": "DEFSTR2",
          "maxLength": 255,
          "langKey": "DEFSTR2",
          "xtype": "ngText"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "主键",
              "name": "PhId"
            },
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
    "listPanel": {
      "id": "listPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTGKXM",
      "desTitle": "列表",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "DM",
          "dataIndex": "DM",
          "width": 153,
          "header": "项目代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "MC",
          "dataIndex": "MC",
          "width": 232,
          "header": "项目名称",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "DEFSTR2",
          "dataIndex": "DEFSTR2",
          "width": 216,
          "header": "父节点代码",
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
