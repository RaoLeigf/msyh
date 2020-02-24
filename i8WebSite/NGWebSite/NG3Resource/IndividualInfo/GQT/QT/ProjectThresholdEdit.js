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
          "LangKey": "Orgcode",
          "dataIndex": "Orgcode",
          "width": 100,
          "header": "组织代码",
		      "hidden": true
        },
        {
          "dataIndex": "FOname",
          "width": 100,
          "header": "组织名称",
		      "hidden": true
        },
        {
            "dataIndex": "ProjTypeId",
            "width": 100,
            "hidden": true,
            "header": "所选项目类型ID"
        },
        {
            "dataIndex": "ProjTypeName",
            "width": 400,
            "header": "所选项目类型"
        },
        {
          "LangKey": "FThreshold",
          "dataIndex": "FThreshold",
          "width": 200,
          "header": "阈值",
          "editor": {
          "xtype": "numberfield"
          },
          "align": 'right'
		 
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
