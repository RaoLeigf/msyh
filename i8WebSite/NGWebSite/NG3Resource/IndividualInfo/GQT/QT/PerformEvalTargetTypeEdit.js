var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTPerformEvalTargetType",
      "desTitle": "mainPanel",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "上级代码",
          "itemId": "FParentCode",
          "name": "FParentCode",
          "maxLength": 30,
          "langKey": "FParentCode",
          "xtype": "ngText",
		  "hidden": true
        },
		{
          "fieldLabel": "父级",
          "itemId": "FParentName",
          "name": "FParentName",
          "maxLength": 30,
          "langKey": "FParentName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "代码",
          "itemId": "FCode",
          "name": "FCode",
          "maxLength": 30,
          "langKey": "FCode",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "名称",
          "itemId": "FName",
          "name": "FName",
          "maxLength": 255,
          "langKey": "FName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "备注",
          "itemId": "FRemark",
          "name": "FRemark",
          "maxLength": 100,
          "langKey": "FRemark",
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
    "PerformEvalTargetType": {
      "id": "PerformEvalTargetType",
      "buskey": "PhId",
      "bindtable": "Z_QTPerformEvalTargetType",
      "desTitle": "PerformEvalTargetType",
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
          "LangKey": "FCode",
          "dataIndex": "FCode",
          "width": 100,
          "header": "代码"
        },
        {
          "LangKey": "FName",
          "dataIndex": "FName",
          "width": 100,
          "header": "名称"
        },
        {
          "LangKey": "FRemark",
          "dataIndex": "FRemark",
          "width": 100,
          "header": "备注"
        },
        {
          "LangKey": "FParentCode",
          "dataIndex": "FParentCode",
          "width": 100,
          "header": "上级代码"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
