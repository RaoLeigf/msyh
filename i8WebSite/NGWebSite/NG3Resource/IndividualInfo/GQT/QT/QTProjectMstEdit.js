var individualConfigInfo = 
{
  "form": {},
  "grid": {
    "grid": {
      "id": "grid",
      "buskey": "PhId",
      "bindtable": "qt3_projectmst",
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
          "hidden":true
        },
        {
          "LangKey": "FProcessstatus",
          "dataIndex": "FProcessstatus",
          "width": 100,
          "header": "时间进度"
        },
        /*{
          "LangKey": "FDtlstage",
          "dataIndex": "FDtlstage",
          "width": 100,
          "header": "明细阶段",
          "editor": {
						"xtype": "combobox",
						"store":storeDtlStage,
						"editable":false,
						"valueField":'Dm',
						"displayField":'Mc'
					}
        },*/
        {
          "LangKey": "FDtlstage",
          "dataIndex": "FDtlstage_EXName",
          "width": 100,
          "header": "明细阶段",
          "editor": {
            "helpid": "gh_DtlStage",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 300,
          "header": "项目名称"
        },
        {
          "LangKey": "FDeclarationDept",
          "dataIndex": "FDeclarationDept_EXName",
          "width": 150,
          "header": "申报部门",
          /*"editor": {
            "helpid": "ys_orglist2",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }*/
        },
        {
          "LangKey": "FBudgetDept",
          "dataIndex": "FBudgetDept_EXName",
          "width": 150,
          "header": "预算部门",
          /*"editor": {
            "helpid": "ys_orglist",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }*/
        },
        {
          "LangKey": "FTemporarydate",
          "dataIndex": "FTemporarydate",
          "width": 200,
          "header": "暂存时间"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
