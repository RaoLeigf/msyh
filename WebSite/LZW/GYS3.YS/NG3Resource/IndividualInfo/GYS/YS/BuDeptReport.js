﻿var individualConfigInfo = 
{
  "form": {
    "editPanel": {
      "id": "editPanel",
      "buskey": "PhId",
      "bindtable": "ys3_BudgetMst",
      "desTitle": "查询面板",
      "columnsPerRow": 3,
      "fields": [
        {
            "fieldLabel": "项目年度",
            "itemId": "FYear",
            "name": "FYear",
            "maxLength": 4,
            "langKey": "FYear",
            "xtype": "ngText"
        },
        {
          "fieldLabel": "申报单位",
          "itemId": "FDeclarationUnit",
          "name": "FDeclarationUnit",
          "langKey": "FDeclarationUnit",
          "helpid": "sb_orglist",
          "valueField": "ocode",
          "displayField": "OName",
          "userCodeField": "ocode",
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "预算部门",
          "itemId": "FBudgetDept",
          "name": "FBudgetDept",
          "langKey": "FBudgetDept",
          "helpid": "ys_orglist",
          "valueField": "ocode",
          "displayField": "oname",
          "userCodeField": "ocode",
          "xtype": "ngRichHelp"
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
    "dtlPanel": {
      "id": "dtlPanel",
      "buskey": "PhId",
      "bindtable": "ys3_BudgetDtl_BudgetDtl",
      "desTitle": "明细",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
            "LangKey": "f_year",
            "dataIndex": "f_year",
            "width": 80,
            "header": "年度",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DeclarationUnit",
            "dataIndex": "f_DeclarationUnit",
            "width": 100,
            "header": "申报单位代码",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DeclarationUnitName",
            "dataIndex": "f_DeclarationUnitName",
            "width": 180,
            "header": "申报单位",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_BudgetDept",
            "dataIndex": "f_BudgetDept",
            "width": 100,
            "header": "预算部门代码",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_BudgetDeptName",
            "dataIndex": "f_BudgetDeptName",
            "width": 180,
            "header": "预算部门",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjCode",
            "dataIndex": "f_ProjCode",
            "width": 120,
            "header": "项目代码",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjName",
            "dataIndex": "f_ProjName",
            "width": 180,
            "header": "项目名称",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjAmount",
            "dataIndex": "f_ProjAmount",
            "width": 100,
            "header": "项目金额",
            "editor": {
                "xtype": "ngNumber"
            }
        },
        {
            "LangKey": "f_BudgetAmount",
            "dataIndex": "f_BudgetAmount",
            "width": 100,
            "header": "预算金额",
            "editor": {
                "xtype": "ngNumber"
            }
        },
        {
            "LangKey": "f_DtlCode",
            "dataIndex": "f_DtlCode",
            "width": 120,
            "header": "明细项目代码",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DtlName",
            "dataIndex": "f_DtlName",
            "width": 150,
            "header": "明细项目",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_Amount",
            "dataIndex": "f_Amount",
            "width": 100,
            "header": "明细金额",
            "hidden": true,
            "editor": {
                "xtype": "ngNumber"
            }
        },
        {
            "LangKey": "f_DtlBuAmount",
            "dataIndex": "f_DtlBuAmount",
            "width": 100,
            "header": "明细预算金额",
            "hidden": true,
            "editor": {
                "xtype": "ngNumber"
            }
        },
        {
            "LangKey": "f_ExpensesChannel",
            "dataIndex": "f_ExpensesChannel",
            "width": 150,
            "header": "支出渠道代码",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ExpensesChannelName",
            "dataIndex": "f_ExpensesChannelName",
            "width": 150,
            "header": "支出渠道",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_Description",
            "dataIndex": "f_Description",
            "width": 150,
            "header": "说明",
            "editor": {
                "xtype": "ngText"
            }
        }
      ]
    },
    "gridPanel": {
      "id": "gridPanel",
      "buskey": "PhId",
      "bindtable": "ys3_BudgetMst",
      "desTitle": "列表",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },        
        {
            "LangKey": "f_year",
            "dataIndex": "f_year",
            "width": 50,
            "header": "年度",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DeclarationUnit",
            "dataIndex": "f_DeclarationUnit",
            "width": 100,
            "header": "申报单位代码",
            "hidden": true,
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DeclarationUnitName",
            "dataIndex": "f_DeclarationUnitName",
            "width": 150,
            "header": "申报单位",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_BudgetDept",
            "dataIndex": "f_BudgetDept",
            "width": 100,
            "header": "预算部门代码",
            "hidden": true,
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_BudgetDeptName",
            "dataIndex": "f_BudgetDeptName",
            "width": 120,
            "header": "预算部门",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjCode",
            "dataIndex": "f_ProjCode",
            "width": 120,
            "header": "项目代码",
            "hidden": true,
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjName",
            "dataIndex": "f_ProjName",
            "width": 160,
            "header": "项目名称",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ProjAmount",
            "dataIndex": "f_ProjAmount",
            "width": 90,
            "header": "项目金额",
            "editor": {
                "xtype": "ngNumber"
            },
            "style": "text-alien:center;",
            "align": "right"
        },
        {
            "LangKey": "f_BudgetAmount",
            "dataIndex": "f_BudgetAmount",
            "width": 90,
            "header": "预算金额",
            "editor": {
                "xtype": "ngNumber"
            },
            "style": "text-alien:center;",
            "align": "right"
        },
        {
            "LangKey": "f_DtlCode",
            "dataIndex": "f_DtlCode",
            "width": 120,
            "header": "明细项目代码",
            "hidden": true,
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_DtlName",
            "dataIndex": "f_DtlName",
            "width": 150,
            "header": "明细项目",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_Amount",
            "dataIndex": "f_Amount",
            "width": 90,
            "header": "明细金额",
            "format": "$0,0.00",
            "editor": {
                "xtype": "ngNumber"
            },
            "style": "text-alien:center;",
            "align": "right"
        },
        {
            "LangKey": "f_DtlBuAmount",
            "dataIndex": "f_DtlBuAmount",
            "width": 90,
            "header": "明细预算金额",
            "format": "$0,0.00",
            "editor": {
                "xtype": "ngNumber"
            },
            "style": "text-alien:center;",
            "align": "right"
        },
        {
            "LangKey": "f_ExpensesChannel",
            "dataIndex": "f_ExpensesChannel",
            "width": 150,
            "header": "支出渠道代码",
            "hidden": true,
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_ExpensesChannelName",
            "dataIndex": "f_ExpensesChannelName",
            "width": 160,
            "header": "支出渠道",
            "editor": {
                "xtype": "ngText"
            }
        },
        {
            "LangKey": "f_Description",
            "dataIndex": "f_Description",
            "width": 150,
            "header": "说明",
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
