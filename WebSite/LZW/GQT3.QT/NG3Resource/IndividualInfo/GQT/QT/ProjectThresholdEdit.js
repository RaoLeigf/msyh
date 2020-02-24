var individualConfigInfo = 
{
  "form": {},
  "grid": {
    "list2": {
      "id": "list2",
      "buskey": "PhId",
      "bindtable": "Z_QTProjectThreshold",
      "desTitle": "list2",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "Orgcode",
          "dataIndex": "Orgcode",
          "width": 100,
          "header": "组织代码"
        },
        {
          "dataIndex": "FOname",
          "width": 100,
          "header": "组织名称"
          },
          {
              "dataIndex": "ProjTypeId",
              "width": 100,
              "hidden": true,
              "header": "所选项目类型ID"
          },
          {
              "dataIndex": "ProjTypeName",
              "width": 100,
              "header": "所选项目类型"
          },
        {
          "LangKey": "FThreshold",
          "dataIndex": "FThreshold",
          "width": 100,
          "header": "阈值"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
