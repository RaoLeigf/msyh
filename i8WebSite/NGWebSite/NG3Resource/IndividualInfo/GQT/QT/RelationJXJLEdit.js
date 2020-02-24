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
              "width": 50
        },
        {
          "dataIndex": "PhId",
          "width": 100,
          "header": "主键",
		      "hidden": true
        },
        {
          "LangKey": "Dwdm",
          "dataIndex": "Dwdm",
          "width": 100,
          "header": "组织代码",
		      "hidden": true
        },
        {
          "dataIndex": "Dydm",
          "width": 100,
          "header": "组织phid",
		      "hidden": true
        },
        {
            "dataIndex": "Dylx",
            "width": 100,
            "hidden": true,
            "header": "对应类型"
        },
        {
            "dataIndex": "DefStr1",
            "width": 200,
            "header": "得分控制起始值(含)",
            "editor": {
              "xtype": "numberfield"
            }
        },
        {
          "dataIndex": "DefStr2",
          "width": 200,
          "header": "得分控制结束值",
          "editor": {
            "xtype": "numberfield"
          }
        },
        {
          "dataIndex": "DefStr3",
          "width": 200,
          "header": "对应绩效结论",
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
