var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "phid",
      "bindtable": "ys3_expensemst",
      "desTitle": "用款计划基础信息",
      "columnsPerRow": 4,
      "fields": [
        {
          "fieldLabel": "申报单位",
          "itemId": "FDeclarationunit",
          "name": "FDeclarationunit",
          "langKey": "FDeclarationunit",
          "helpid": "sb_orglist",
          "valueField": "ocode",
          "displayField": "oname",
          "userCodeField": "ocode",
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "申报日期",
          "itemId": "FDateofdeclaration",
          "name": "FDateofdeclaration",
          "langKey": "FDateofdeclaration",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "项目名称",
          "itemId": "FProjname",
          "name": "FProjname",
          "maxLength": 100,
          "langKey": "FProjname",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "项目类型",
          "itemId": "FExpenseCategory",
          "name": "FExpenseCategory",
          "langKey": "FExpenseCategory",
          "helpid": "GHExpenseCategory",
          "valueField": "dm",
          "displayField": "mc",
          "userCodeField": "dm",
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "服务意向单位",
          "itemId": "FServiceDept",
          "name": "FServiceDept",
          "maxLength": 200,
          "langKey": "FServiceDept",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "经费标准",
          "itemId": "FFundStandard",
          "name": "FFundStandard",
          "maxLength": 200,
          "langKey": "FFundStandard",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "年初预算金额",
          "itemId": "FProjAmount",
          "name": "FProjAmount",
          "maxLength": 18,
          "langKey": "FProjAmount",
          "xtype": "ngNumber"
        },
        {
          "fieldLabel": "可编报数",
          "itemId": "FPlayamount",
          "name": "FPlayamount",
          "maxLength": 18,
          "langKey": "FPlayamount",
          "xtype": "ngNumber"
        },
        {
          "fieldLabel": "预计支出金额",
          "itemId": "FSurplusamount",
          "name": "FSurplusamount",
          "maxLength": 18,
          "langKey": "FSurplusamount",
          "xtype": "ngNumber"
        },
        {
          "fieldLabel": "预计返回金额",
          "itemId": "FReturnamount",
          "name": "FReturnamount",
          "maxLength": 18,
          "langKey": "FReturnamount",
          "xtype": "ngNumber"
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
    "ExpensedtlGrid": {
      "id": "ExpensedtlGrid",
      "buskey": "PhId",
      "bindtable": "ys3_expensedtl",
      "desTitle": "用款计划明细",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "MstPhid",
          "dataIndex": "MstPhid",
          "width": 100,
          "header": "主表主键",
          "hidden": true
        },
        {
          "LangKey": "FDtlcode",
          "dataIndex": "FDtlcode",
          "width": 100,
          "header": "明细项目代码",
          "hidden": true
        },
        {
          "LangKey": "FName",
          "dataIndex": "FName",
          "width": 100,
          "header": "明细项目名称"
        },
        {
          "LangKey": "FSourceoffunds",
          "dataIndex": "FSourceoffunds_EXName",
          "width": 100,
          "header": "资金来源",
          "editor": {
            "helpid": "GHSourceOfFunds",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FBudgetaccounts",
          "dataIndex": "FBudgetaccounts_EXName",
          "width": 100,
          "header": "预算科目",
          "editor": {
            "helpid": "GHBudgetAccounts",
            "valueField": "kmdm",
            "displayField": "kmmc",
            "userCodeField": "kmdm",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FExpenseschannel",
          "dataIndex": "FExpenseschannel_EXName",
          "width": 100,
          "header": "支出渠道",
          "editor": {
            "helpid": "GHExpensesChannel",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FQty",
          "dataIndex": "FQty",
          "width": 100,
          "header": "天数",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FPrice",
          "dataIndex": "FPrice",
          "width": 100,
          "header": "人数",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FQty2",
          "dataIndex": "FQty2",
          "width": 100,
          "header": "单价",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FAmount",
          "dataIndex": "FAmount",
          "width": 100,
          "header": "预计金额",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FReturnamount",
          "dataIndex": "FReturnamount",
          "width": 100,
          "header": "返还金额",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FOtherinstructions",
          "dataIndex": "FOtherinstructions",
          "width": 100,
          "header": "明细使用说明"
        }
      ]
    },
    "billList": {
      "id": "billList",
      "buskey": "PhId",
      "bindtable": "ys3_expensemst",
      "desTitle": "单据列表",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "FDeclarationunit",
          "dataIndex": "FDeclarationunit_EXName",
          "width": 100,
          "header": "申报单位",
          "editor": {
            "helpid": "sb_orglist",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FBudgetDept",
          "dataIndex": "FBudgetDept_EXName",
          "width": 100,
          "header": "预算部门",
          "editor": {
            "helpid": "ys_orglist",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FProjname",
          "dataIndex": "FProjname",
          "width": 100,
          "header": "项目名称"
        },
        {
          "LangKey": "FApprovestatus",
          "dataIndex": "FApprovestatus",
          "width": 100,
          "header": "审批状态",
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": "1",
                "name": "待上报"
              },
              {
                "code": "2",
                "name": "审批中"
              },
              {
                "code": "3",
                "name": "已审批"
              }
            ]
          }
        },
        {
          "LangKey": "FDeclarer",
          "dataIndex": "FDeclarer",
          "width": 100,
          "header": "申报人"
        },
        {
          "LangKey": "FDateofdeclaration",
          "dataIndex": "FDateofdeclaration",
          "width": 100,
          "header": "申报日期",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FApprover",
          "dataIndex": "FApprover_EXName",
          "width": 100,
          "header": "审批人",
          "editor": {
            "helpid": "fg3_user",
            "valueField": "PhId",
            "displayField": "UserName",
            "userCodeField": "UserNo",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FApprovedate",
          "dataIndex": "FApprovedate",
          "width": 100,
          "header": "审批时间",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FYear",
          "dataIndex": "FYear",
          "width": 100,
          "header": "项目年度",
          "hidden": true
        },
        {
          "LangKey": "FProjcode",
          "dataIndex": "FProjcode",
          "width": 100,
          "header": "项目编码",
          "hidden": true
        },
        {
          "LangKey": "FProjAttr",
          "dataIndex": "FProjAttr",
          "width": 100,
          "header": "项目属性",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": "1",
                "name": "延续项目"
              },
              {
                "code": "2",
                "name": "新增项目"
              }
            ]
          }
        },
        {
          "LangKey": "FDuration",
          "dataIndex": "FDuration",
          "width": 100,
          "header": "存续期限",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": "1",
                "name": "一次性项目"
              },
              {
                "code": "2",
                "name": "经常性项目"
              },
              {
                "code": "3",
                "name": "跨年度项目"
              }
            ]
          }
        },
        {
          "LangKey": "FExpenseCategory",
          "dataIndex": "FExpenseCategory_EXName",
          "width": 100,
          "header": "支出类别",
          "hidden": true,
          "editor": {
            "helpid": "GHExpenseCategory",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FProjstatus",
          "dataIndex": "FProjstatus",
          "width": 100,
          "header": "项目状态",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": 1,
                "name": "单位备选"
              },
              {
                "code": 2,
                "name": "纳入预算"
              },
              {
                "code": 3,
                "name": "项目执行"
              },
              {
                "code": 4,
                "name": "项目调整"
              },
              {
                "code": 5,
                "name": "项目暂停"
              },
              {
                "code": 6,
                "name": "项目终止"
              },
              {
                "code": 7,
                "name": "项目关闭"
              }
            ]
          }
        },
        {
          "LangKey": "FStartdate",
          "dataIndex": "FStartdate",
          "width": 100,
          "header": "开始日期",
          "hidden": true,
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FDeclarationDept",
          "dataIndex": "FDeclarationDept_EXName",
          "width": 100,
          "header": "申报部门",
          "hidden": true,
          "editor": {
            "helpid": "dept4ocode",
            "valueField": "ocode",
            "displayField": "oname",
            "userCodeField": "ocode",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FEnddate",
          "dataIndex": "FEnddate",
          "width": 100,
          "header": "结束日期",
          "hidden": true,
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FIfperformanceappraisal",
          "dataIndex": "FIfperformanceappraisal",
          "width": 100,
          "header": "是否绩效评价",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": 1,
                "name": "是"
              },
              {
                "code": 2,
                "name": "否"
              }
            ]
          }
        },
        {
          "LangKey": "FIfKeyEvaluation",
          "dataIndex": "FIfKeyEvaluation",
          "width": 100,
          "header": "是否重点评价",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": 1,
                "name": "是"
              },
              {
                "code": 2,
                "name": "否"
              }
            ]
          }
        },
        {
          "LangKey": "FProjAmount",
          "dataIndex": "FProjAmount",
          "width": 100,
          "header": "年初预算金额",
          "hidden": true,
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FType",
          "dataIndex": "FType",
          "width": 100,
          "header": "单据类型",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": "1",
                "name": "年初"
              },
              {
                "code": "2",
                "name": "年中新增"
              },
              {
                "code": "3",
                "name": "专项"
              }
            ]
          }
        },
        {
          "LangKey": "FBudgetamount",
          "dataIndex": "FBudgetamount",
          "width": 100,
          "header": "预算金额",
          "hidden": true,
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FLifeCycle",
          "dataIndex": "FLifeCycle",
          "width": 100,
          "header": "版本标识",
          "hidden": true,
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FVerno",
          "dataIndex": "FVerno",
          "width": 100,
          "header": "调整版本号",
          "hidden": true
        },
        {
          "LangKey": "FPlayamount",
          "dataIndex": "FPlayamount",
          "width": 100,
          "header": "可编报数",
          "hidden": true,
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FServiceDept",
          "dataIndex": "FServiceDept",
          "width": 100,
          "header": "服务意向单位",
          "hidden": true
        },
        {
          "LangKey": "FCarryover",
          "dataIndex": "FCarryover",
          "width": 100,
          "header": "结转标志",
          "hidden": true,
          "editor": {
            "xtype": "ngComboBox",
            "valueField": "code",
            "displayField": "name",
            "QueryMode": "local",
            "valueType": "int",
            "data": [
              {
                "code": 1,
                "name": "未结转"
              },
              {
                "code": 2,
                "name": "已结转"
              }
            ]
          }
        },
        {
          "LangKey": "FFundStandard",
          "dataIndex": "FFundStandard",
          "width": 100,
          "header": "经费标准",
          "hidden": true
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {
    "DtlPanel": {
      "id": "DtlPanel",
      "desTitle": "明细信息",
      "items": [
        {
          "id": "TabPage1",
          "title": "项目支出预算明细",
          "langKey": "TabPage1"
        }
      ]
    }
  }
}
