var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectMst",
      "desTitle": "项目基础信息",
      "columnsPerRow": 4,
      "fields": [
        {
          "fieldLabel": "申报单位",
          "itemId": "FDeclarationUnit",
          "name": "FDeclarationUnit",
          "langKey": "FDeclarationUnit",
          "helpid": "cboo",
          "valueField": "PhId",
          "displayField": "OName",
          "userCodeField": "OCode",
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "预算部门",
          "itemId": "FBudgetDept",
          "name": "FBudgetDept",
          "langKey": "FBudgetDept",
          "helpid": "dept",
          "valueField": "PhId",
          "displayField": "OName",
          "userCodeField": "OCode",
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "项目名称",
          "itemId": "FProjName",
          "name": "FProjName",
          "maxLength": 100,
          "langKey": "FProjName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "项目属性",
          "itemId": "FProjAttr",
          "name": "FProjAttr",
          "langKey": "FProjAttr",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
          "valueType": "string",
          "data": [
            {
              "code": "1",
              "name": "延续项目"
            },
            {
              "code": "2",
              "name": "经常性项目"
            }
          ]
        },
        {
          "fieldLabel": "存续期限",
          "itemId": "FDuration",
          "name": "FDuration",
          "langKey": "FDuration",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "支出类别",
          "itemId": "FExpenseCategory",
          "name": "FExpenseCategory",
          "maxLength": 20,
          "langKey": "FExpenseCategory",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "项目状态",
          "itemId": "FProjStatus",
          "name": "FProjStatus",
          "langKey": "FProjStatus",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "项目金额",
          "itemId": "FProjAmount",
          "name": "FProjAmount",
          "maxLength": 18,
          "langKey": "FProjAmount",
          "xtype": "ngNumber"
        },
        {
          "fieldLabel": "项目年度",
          "itemId": "FYear",
          "name": "FYear",
          "maxLength": 4,
          "langKey": "FYear",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "开始日期",
          "itemId": "FStartDate",
          "name": "FStartDate",
          "langKey": "FStartDate",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "结束日期",
          "itemId": "FEndDate",
          "name": "FEndDate",
          "langKey": "FEndDate",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "绩效评价",
          "itemId": "FIfPerformanceAppraisal",
          "name": "FIfPerformanceAppraisal",
          "langKey": "FIfPerformanceAppraisal",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "重点评价",
          "itemId": "FIfKeyEvaluation",
          "name": "FIfKeyEvaluation",
          "langKey": "FIfKeyEvaluation",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "会议日期",
          "itemId": "FMeetingTime",
          "name": "FMeetingTime",
          "langKey": "FMeetingTime",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "会议纪要编号",
          "itemId": "FMeetiingSummaryNo",
          "name": "FMeetiingSummaryNo",
          "maxLength": 100,
          "langKey": "FMeetiingSummaryNo",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "申报人",
          "itemId": "FDeclarer",
          "name": "FDeclarer",
          "maxLength": 20,
          "langKey": "FDeclarer",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "申报日期",
          "itemId": "FDateofDeclaration",
          "name": "FDateofDeclaration",
          "langKey": "FDateofDeclaration",
          "xtype": "ngDate"
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
    },
    "FunctionalOvervPanel": {
      "id": "FunctionalOvervPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_TextContent",
      "desTitle": "部门职能概述",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "部门职能概述",
          "itemId": "FFunctionalOverview",
          "name": "FFunctionalOverview",
          "maxLength": 1000,
          "langKey": "FFunctionalOverview",
          "xtype": "ngTextArea"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "PhId"
            },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    },
    "ProjOverviewPanel": {
      "id": "ProjOverviewPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_TextContent",
      "desTitle": "项目概况",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "项目概况",
          "itemId": "FProjOverview",
          "name": "FProjOverview",
          "maxLength": 1000,
          "langKey": "FProjOverview",
          "xtype": "ngTextArea"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "PhId"
            },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    },
    "longTargetPanel": {
      "id": "longTargetPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_TextContent",
      "desTitle": "长期绩效目标",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "长期绩效目标",
          "itemId": "FLTPerformGoal",
          "name": "FLTPerformGoal",
          "maxLength": 1000,
          "langKey": "FLTPerformGoal",
          "xtype": "ngTextArea"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "PhId"
            },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    },
    "yearTargetPanel": {
      "id": "yearTargetPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_TextContent",
      "desTitle": "年度绩效目标",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "年度绩效目标",
          "itemId": "FAnnualPerformGoal",
          "name": "FAnnualPerformGoal",
          "maxLength": 1000,
          "langKey": "FAnnualPerformGoal",
          "xtype": "ngTextArea"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "PhId"
            },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    }
  },
  "grid": {
    "BudgetDtlPanel": {
      "id": "BudgetDtlPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_BudgetDtl",
      "desTitle": "预算明细",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "FDtlCode",
          "dataIndex": "FDtlCode",
          "width": 100,
          "header": "明细项目代码"
        },
        {
          "LangKey": "FName",
          "dataIndex": "FName",
          "width": 100,
          "header": "明细项目名称"
        },
        {
          "LangKey": "FAmount",
          "dataIndex": "FAmount",
          "width": 100,
          "header": "金额"
        },
        {
          "LangKey": "FBudgetAmount",
          "dataIndex": "FBudgetAmount",
          "width": 100,
          "header": "预算金额"
        },
        {
          "LangKey": "FSourceOfFunds",
          "dataIndex": "FSourceOfFunds",
          "width": 100,
          "header": "资金来源"
        },
        {
          "LangKey": "FPaymentMethod",
          "dataIndex": "FPaymentMethod",
          "width": 100,
          "header": "支付方式"
        },
        {
          "LangKey": "FOtherInstructions",
          "dataIndex": "FOtherInstructions",
          "width": 100,
          "header": "其他说明"
        },
        {
          "LangKey": "FFeedback",
          "dataIndex": "FFeedback",
          "width": 100,
          "header": "反馈意见"
        }
      ]
    },
    "FundApplPanel": {
      "id": "FundApplPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_FundAppl",
      "desTitle": "资金申请",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "FSourceOfFunds",
          "dataIndex": "FSourceOfFunds",
          "width": 373,
          "header": "资金来源"
        },
        {
          "LangKey": "FAmount",
          "dataIndex": "FAmount",
          "width": 153,
          "header": "金额"
        }
      ]
    },
    "ImplPlanPanel": {
      "id": "ImplPlanPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_ImplPlan",
      "desTitle": "实施计划",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "FImplContent",
          "dataIndex": "FImplContent",
          "width": 395,
          "header": "实施内容"
        },
        {
          "LangKey": "FStartDate",
          "dataIndex": "FStartDate",
          "width": 100,
          "header": "开始日期"
        },
        {
          "LangKey": "FEndDate",
          "dataIndex": "FEndDate",
          "width": 100,
          "header": "结束日期"
        }
      ]
    },
    "billList": {
      "id": "billList",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectMst",
      "desTitle": "单据列表",
      "isBillList": true,
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
          "header": "主键"
        },
        {
          "LangKey": "FDeclarationUnit",
          "dataIndex": "FDeclarationUnit_EXName",
          "width": 100,
          "header": "申报单位",
          "editor": {
            "helpid": "cboo",
            "valueField": "PhId",
            "displayField": "OName",
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
            "helpid": "dept",
            "valueField": "ocode",
            "displayField": "oname",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FDeclarationDept",
          "dataIndex": "FDeclarationDept_EXName",
          "width": 100,
          "header": "申报部门",
          "editor": {
            "helpid": "dept",
            "valueField": "ocode",
            "displayField": "oname",
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FProjCode",
          "dataIndex": "FProjCode",
          "width": 100,
          "header": "项目编码"
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 100,
          "header": "项目名称"
        },
        {
          "LangKey": "FExpenseCategory",
          "dataIndex": "FExpenseCategory",
          "width": 100,
          "header": "支出类别"
        },
        {
          "LangKey": "FProjAttr",
          "dataIndex": "FProjAttr",
          "width": 100,
          "header": "项目属性",
          "editor": {
            "xtype": "ngComboBox",
            "data": [
              {
                "code": "1",
                "name": "延续项目"
              },
              {
                "code": "2",
                "name": "经常性项目"
              }
            ]
          },
          "renderer": function (val, cell) {
              switch (val) {
                  case "1":
                      return "延续项目";
                  case "2":
                      return "经常性项目";
              }
          }
        },
        {
          "LangKey": "FDuration",
          "dataIndex": "FDuration",
          "width": 100,
          "header": "存续期限",
          "editor": {
            "xtype": "ngComboBox",
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
          },
          "renderer": function (val, cell) {
              switch (val) {
                  case "1":
                      return "一次性项目";
                  case "2":
                      return "经常性项目";
                  case "3":
                      return "跨年度项目";
              }
          }
        },
        {
          "LangKey": "FProjStatus",
          "dataIndex": "FProjStatus",
          "width": 100,
          "header": "项目状态",
          "editor": {
            "xtype": "ngComboBox",
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
          },
          "renderer": function (val, cell) {
              switch (val) {
                  case 1:
                      return "单位备选";
                  case 2:
                      return "纳入预算";
                  case 3:
                      return "项目执行";
                  case 4:
                      return "项目调整";
                  case 5:
                      return "项目暂停";
                  case 6:
                      return "项目终止";
                  case 7:
                      return "项目关闭";
              }
          }
        },
        {
          "LangKey": "FIfPerformanceAppraisal",
          "dataIndex": "FIfPerformanceAppraisal",
          "width": 100,
          "header": "绩效评价",
          "editor": {
            "xtype": "ngComboBox",
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
          "header": "项目金额",
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FApproveStatus",
          "dataIndex": "FApproveStatus",
          "width": 100,
          "header": "单据状态",
          "editor": {
            "xtype": "ngComboBox",
            "data": [
              {
                "code": 1,
                "name": "待上报"
              },
              {
                "code": 2,
                "name": "审批中"
              },
              {
                "code": 3,
                "name": "已审批"
              }
            ]
          }
        },
        {
          "LangKey": "FStartDate",
          "dataIndex": "FStartDate",
          "width": 100,
          "header": "开始日期",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FEndDate",
          "dataIndex": "FEndDate",
          "width": 100,
          "header": "结束日期",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FDateofDeclaration",
          "dataIndex": "FDateofDeclaration",
          "width": 100,
          "header": "申报日期",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FDeclarer",
          "dataIndex": "FDeclarer",
          "width": 100,
          "header": "申报人"
        }
      ]
    }
  },
  "fieldSetForm": {
    "projectStartInfoPanel": {
      "id": "projectStartInfoPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectDtl_TextContent",
      "desTitle": "项目立项情况",
      "fieldSets": [
        {
          "itemId": "FProjBasisGroup",
          "desTitle": "立项依据",
          "columnsPerRow": 1,
          "allfields": [
            {
              "fieldLabel": "立项依据",
              "itemId": "FProjBasis",
              "name": "FProjBasis",
              "maxLength": 500,
              "langKey": "FProjBasis",
              "xtype": "ngTextArea"
            },
            {
              "xtype": "container",
              "name": "hiddenContainer",
              "hidden": true,
              "items": [
                {
                  "xtype": "hiddenfield",
                  "fieldLabel": "",
                  "name": "PhId"
                },
                {
                  "xtype": "hiddenfield",
                  "fieldLabel": "",
                  "name": "NgRecordVer"
                }
              ]
            }
          ]
        },
        {
          "itemId": "FFeasibilityGroup",
          "desTitle": "可行性",
          "columnsPerRow": 1,
          "allfields": [
            {
              "fieldLabel": "可行性",
              "itemId": "FFeasibility",
              "name": "FFeasibility",
              "maxLength": 500,
              "langKey": "FFeasibility",
              "xtype": "ngTextArea"
            }
          ]
        },
        {
          "itemId": "FNecessityGroup",
          "desTitle": "必要性",
          "columnsPerRow": 1,
          "allfields": [
            {
              "fieldLabel": "必要性",
              "itemId": "FNecessity",
              "name": "FNecessity",
              "maxLength": 500,
              "langKey": "FNecessity",
              "xtype": "ngTextArea"
            }
          ]
        }
      ]
    }
  },
  "tabPanel": {
    "DtlPanel": {
      "id": "DtlPanel",
      "desTitle": "详细信息",
      "items": [
        {
          "id": "TabPage1",
          "title": "部门职能概述",
          "langKey": "TabPage1"
        },
        {
          "id": "TabPage2",
          "title": "项目概况",
          "langKey": "TabPage2"
        },
        {
          "id": "TabPage3",
          "title": "项目立项情况",
          "langKey": "TabPage3"
        },
        {
          "id": "TabPage4",
          "title": "项目预算明细",
          "langKey": "TabPage4"
        },
        {
          "id": "TabPage5",
          "title": "项目资金申请",
          "langKey": "TabPage5"
        },
        {
          "id": "TabPage6",
          "title": "实施计划",
          "langKey": "TabPage6"
        },
        {
          "id": "TabPage7",
          "title": "长期绩效目标",
          "langKey": "TabPage7"
        },
        {
          "id": "TabPage8",
          "title": "年度绩效目标",
          "langKey": "TabPage8"
        }
      ]
    }
  }
}
