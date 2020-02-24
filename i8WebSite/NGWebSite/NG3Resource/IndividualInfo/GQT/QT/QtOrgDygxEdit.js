var individualConfigInfo = 
{
  "form": {},
  "grid": {
    "ORGDYGX": {
      "id": "ORGDYGX",
      "buskey": "PhId",
      "bindtable": "z_qtorgdygx",
      "desTitle": "项目库对应G6H设置",
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
          "LangKey": "Xmorg",
          "dataIndex": "Xmorg_EXName",
          "width": 300,
          "header": "项目库组织",
          "editor": {
              "helpid": "gxm_orglist_all",
              "valueField": "ocode",
            "displayField": "oname",
              "userCodeField": "ocode",
              "ORMMode": false,
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "Oldorg",
          "dataIndex": "Oldorg",
          "width": 200,
            "header": "老G6H组织代码",
            "editor": {
                "xtype": "ngText"
            }
          },
          {
              "LangKey": "Oldbudget",
              "dataIndex": "Oldbudget",
              "width": 200,
              "header": "老G6H部门代码",
              "editor": {
                  "xtype": "ngText"
              }
          },
        {
            "LangKey": "IfCorp",
            "dataIndex": "IfCorp",
              "width": 100,
            "header": "类型",
            "hidden": true
          },
          {
              "LangKey": "ParentOrgId",
              "dataIndex": "ParentOrgId",
              "width": 100,
              "header": "部门归属组织",
              "hidden": true
          }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
