var individualConfigInfo = 
{
  "form": {
	  "mainPanel": {
		  "id": "mainPanel",
            "buskey": "PhId",
            //"bindtable": "xm3_ProjectMst",
            //"desTitle": "项目基础信息",
            "columnsPerRow": 4,
            "labelWidth": "130px",
			"fields": [
			  {
					"fieldLabel": "申报单位",
					"xtype": "ngRichHelp",
					"helpid": "sb_orglist",
					"valueField": "ocode",
					"displayField": "oname",
					"userCodeField": "ocode",
					"ORMMode": false,
					//isInGrid: true,
					//helpResizable: true,
					"itemId": "FDeclarationUnit",
					"name": "FDeclarationUnit",
					"editable": false
				},
				{
					"fieldLabel": "预算部门",
					"xtype": "ngRichHelp",
					"helpid": "ys_orglist",
					"valueField": "ocode",
					"displayField": "oname",
					"userCodeField": "ocode",
					"ORMMode": false,
					//isInGrid: true,
					//helpResizable: true,
					"itemId": "FBudgetDept",
					"name": "FBudgetDept",
					"editable": false
				},
				{
					"fieldLabel": "项目年度",
					"xtype": "ngComboBox",
					"valueField": "code",
					"displayField": "name",
					"userCodeField": "code",
					"queryMode": "local",
					"itemId": "FYear",
					"name": "FYear",
					"valueType": "int",
					"data":[
						{
							"code": new Date().getFullYear()-2,
							"name": new Date().getFullYear()-2
						},
						{
							"code": new Date().getFullYear()-1,
							"name": new Date().getFullYear()-1
						},
						{
							"code": new Date().getFullYear(),
							"name": new Date().getFullYear()
						},
						{
							"code": new Date().getFullYear()+1,
							"name": new Date().getFullYear()+1
						},
						{
							"code": new Date().getFullYear()+2,
							"name": new Date().getFullYear()+2
						}
					]
				},
				{
					"fieldLabel": "项目名称",
					"xtype": "ngRichHelp",
					"helpid": "GHXMName_xm3",//  GHXMName
					"valueField": "f_projcode",
					"displayField": "f_projname",
					"userCodeField": "f_projcode",
					"ORMMode": false,
					//isInGrid: true,
					//helpResizable: true,
					"itemId": "FProjName",
					"name": "FProjName",
					"editable": false
				}
			  ]
	  }
  },
  "grid": {
    "ModifyGrid": {
      "id": "ModifyGrid",
      "buskey": "PhId",
      "bindtable": "Z_QTModify",
      "desTitle": "列表",
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
		  "hidden":true
        },
        {
          "LangKey": "UserCode",
          "dataIndex": "UserCode",
          "width": 150,
          "header": "用户编码"
        },
        {
          "LangKey": "UserName",
          "dataIndex": "UserName",
          "width": 200,
          "header": "用户名称"
        },
        {
          "LangKey": "IP",
          "dataIndex": "IP",
          "width": 150,
          "header": "用户IP"
        },
        {
          "LangKey": "ModifyField",
          "dataIndex": "ModifyField",
          "width": 250,
          "header": "修改字段"
        },
        {
          "LangKey": "BeforeValue",
          "dataIndex": "BeforeValue",
          "width": 300,
          "header": "修改前"
        },
        {
          "LangKey": "AfterValue",
          "dataIndex": "AfterValue",
          "width": 300,
          "header": "修改后"
        },
		{
          "LangKey": "NgInsertDt",
          "dataIndex": "NgInsertDt",
          "width": 250,
          "header": "修改时间"
        },
        {
          "LangKey": "FProjCode",
          "dataIndex": "FProjCode",
          "width": 100,
          "header": "单据代码",
		  "hidden":true
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 100,
          "header": "单据名称",
		  "hidden":true
        },
        {
          "LangKey": "TabName",
          "dataIndex": "TabName",
          "width": 100,
          "header": "修改处tab名称",
		  "hidden":true
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
