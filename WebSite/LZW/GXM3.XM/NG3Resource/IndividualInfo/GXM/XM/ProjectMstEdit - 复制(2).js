var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "xm3_ProjectMst",
      "desTitle": "项目基础信息",
      "columnsPerRow": 4,
      "labelWidth": "85px",
      "fields": [
        {
          "fieldLabel": "申报单位",
          "itemId": "FDeclarationUnit",
          "name": "FDeclarationUnit",
          "mustInput": true,
          "langKey": "FDeclarationUnit",
          "helpid": "sb_orglist",
          "valueField": "ocode",
          "displayField": "oname",
          "userCodeField": "ocode",
          "ORMMode": false,
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "预算部门",
          "itemId": "FBudgetDept",
          "name": "FBudgetDept",
          "mustInput": true,
          "langKey": "FBudgetDept",
          "helpid": "ys_orglist",
          "valueField": "ocode",
          "displayField": "oname",
          "userCodeField": "ocode",
          "ORMMode": false,
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "项目属性",
          "itemId": "FProjAttr",
          "name": "FProjAttr",
          "mustInput": true,
          "langKey": "FProjAttr",
          "valueField": "code",
          "displayField": "name",
          "userCodeField": "test1",
          "xtype": "ngComboBox",
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "存续期限",
          "itemId": "FDuration",
          "name": "FDuration",
          "mustInput": true,
          "langKey": "FDuration",
          "valueField": "code",
          "displayField": "name",
          "userCodeField": "test22",
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
          "fieldLabel": "项目名称",
          "itemId": "FProjName",
          "name": "FProjName",
          "mustInput": true,
          "colspan": 2,
          "maxLength": 100,
          "langKey": "FProjName",
		  "labelStyle":"color:OrangeRed",
           //"xtype": "ngText"
          "xtype": "triggerfield"
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
          "ORMMode": false,
          "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "项目状态",
          "itemId": "FProjStatus",
          "name": "FProjStatus",
          "readOnly": true,
          "langKey": "FProjStatus",
          "valueField": "code",
          "displayField": "name",
          "userCodeField": "test66",
          "xtype": "ngComboBox",
          "queryMode": "local",
          "valueType": "int",
          "data": [
            {
              "code": 0,
              "name": ""
            },
            {
              "code": 1,
                "name": "预立项"
            },
            {
              "code": 2,
                "name": "项目立项"
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
          "readOnly": true,
          "maxLength": 18,
          "langKey": "FProjAmount",
          "xtype": "ngNumber"
          },
          {
              "fieldLabel": "预算金额",
              "itemId": "FBudgetAmount",
              "name": "FBudgetAmount",
              "readOnly": true,
              "maxLength": 18,
              "langKey": "FBudgetAmount",
              "xtype": "ngNumber"
          },
        
        {
          "fieldLabel": "项目年度",
          "itemId": "FYear",
          "name": "FYear",
          "mustInput": true,
          "maxLength": 4,
          "langKey": "FYear",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "开始日期",
          "itemId": "FStartDate",
            "name": "FStartDate",
            "mustInput": true,
          "langKey": "FStartDate",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "结束日期",
          "itemId": "FEndDate",
            "name": "FEndDate",
            "mustInput": true,
          "langKey": "FEndDate",
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "纪要编号",
          "itemId": "FMeetiingSummaryNo",
          "name": "FMeetiingSummaryNo",
          "maxLength": 100,
          "langKey": "FMeetiingSummaryNo",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "会议日期",
          "itemId": "FMeetingTime",
          "name": "FMeetingTime",
          "langKey": "FMeetingTime",
          "xtype": "ngDate"
        },
        {
            "fieldLabel": "申报日期",
            "itemId": "FDateofDeclaration",
            "name": "FDateofDeclaration",
            "readOnly": true,
            "langKey": "FDateofDeclaration",
            "xtype": "ngDate"
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
            "fieldLabel": "绩效评价",
            "itemId": "FIfPerformanceAppraisal",
            "name": "FIfPerformanceAppraisal",
            "langKey": "FIfPerformanceAppraisal",
            "valueField": "code",
            "displayField": "name",
            "userCodeField": "code",
            "xtype": "ngComboBox",
            "queryMode": "local",
            "valueType": "int",
            "data": [
              {
                  "code": 0,
                  "name": ""
              },
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
            "fieldLabel": "绩效评价类型",
            "itemId": "FPerformEvalType",
            "name": "FPerformEvalType",
            "langKey": "FPerformEvalType",
            "helpid": "GHPerformEvalType",            
            "valueField": "FCode",
            "displayField": "FName",
            "userCodeField": "FCode",
            "ORMMode": true,
            "xtype": "ngRichHelp"
        },
        {
            "fieldLabel": "绩效项目类型",
            "itemId": "FPerformType",
            "name": "FPerformType",
            "langKey": "FPerformType",
            "helpid": "GHPerformEvalTargetTypeTree",
            "valueField": "f_code",
            "displayField": "f_name",
            "userCodeField": "f_code",
            "ORMMode": false,
            "xtype": "ngRichHelp"
        },
        {
            "fieldLabel": "重点评价",
            "itemId": "FIfKeyEvaluation",
            "name": "FIfKeyEvaluation",
            "langKey": "FIfKeyEvaluation",
            "valueField": "code",
            "displayField": "name",
            "userCodeField": "test333",
            "xtype": "ngComboBox",
            "queryMode": "local",
            "valueType": "int",
            "data": [
              {
                  "code": 0,
                  "name": ""
              },
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
            "fieldLabel": "是否集中采购",
            "itemId": "FIfPurchase",
            "name": "FIfPurchase",
            "hidden": true,
            "langKey": "FIfPurchase",
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
              "fieldLabel": "是否集中采购",
              "itemId": "FIfPurchase",
              "name": "FIfPurchase",
              "hidden": true,
              "maxLength": 19,
              "langKey": "FIfPurchase",
              "xtype": "ngText"
          },
        {
            "fieldLabel": "项目代码",
            "itemId": "FProjCode",
            "name": "FProjCode",
            "hidden": true,
            "maxLength": 40,
            "langKey": "FProjCode",
            "xtype": "ngText"
          },
          {
              "id": "FApproveStatus",
              "fieldLabel": "审批状态",
              "itemId": "FApproveStatus",
              "name": "FApproveStatus",
              "hidden": true,
              "maxLength": 2,
              "langKey": "FApproveStatus",
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
          "itemId": "FFunctionalOverview",
          "name": "FFunctionalOverview",
          "maxLength": 1000,
          "langKey": "FFunctionalOverview",
          "xtype": "ngTextArea",
          "emptyText": "请输入部门职能概述（限500字）。",
          "height": 245,
          "colspan": 1
        },
        {
          "fieldLabel": "项目概况",
          "itemId": "FProjOverview",
          "name": "FProjOverview",
          "hidden": true,
          "maxLength": 1000,
          "langKey": "FProjOverview1",
          "xtype": "ngTextArea"
        },
        {
          "fieldLabel": "立项依据",
          "itemId": "FProjBasis",
          "name": "FProjBasis",
          "hidden": true,
          "maxLength": 500,
          "langKey": "FProjBasis1",
          "xtype": "ngTextArea"
        },
        {
          "fieldLabel": "可行性",
          "itemId": "FFeasibility",
          "name": "FFeasibility",
          "hidden": true,
          "maxLength": 500,
          "langKey": "FFeasibility1",
          "xtype": "ngTextArea"
        },
        {
          "fieldLabel": "必要性",
          "itemId": "FNecessity",
          "name": "FNecessity",
          "hidden": true,
          "maxLength": 500,
          "langKey": "FNecessity1",
          "xtype": "ngTextArea"
        },
        {
          "fieldLabel": "总体绩效目标",
          "itemId": "FLTPerformGoal",
          "name": "FLTPerformGoal",
          "hidden": true,
          "maxLength": 1000,
          "langKey": "FLTPerformGoal1",
          "xtype": "ngTextArea"
        },
        {
          "fieldLabel": "年度绩效目标",
          "itemId": "FAnnualPerformGoal",
          "name": "FAnnualPerformGoal",
          "hidden": true,
          "maxLength": 1000,
          "langKey": "FAnnualPerformGoal1",
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
          "itemId": "FProjOverview",
          "name": "FProjOverview",
          "maxLength": 1000,
          "langKey": "FProjOverview",
          "xtype": "ngTextArea",
          "emptyText": "请输入项目概况和主要内容（限500字）。",
          "height": 245,
          "colspan": 1
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
        "desTitle": "总体绩效目标",
      "columnsPerRow": 1,
      "fields": [
        {
          "itemId": "FLTPerformGoal",
          "name": "FLTPerformGoal",
          "maxLength": 1000,
          "langKey": "FLTPerformGoal",
          "xtype": "ngTextArea",
              "emptyText": "请输入总体绩效目标（限500字）。",
          "height": 245,
          "colspan": 1
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
          "itemId": "FAnnualPerformGoal",
          "name": "FAnnualPerformGoal",
          "maxLength": 1000,
          "langKey": "FAnnualPerformGoal",
          "xtype": "ngTextArea",
          "emptyText": "请输入年度绩效目标（限500字）。",
          "height": 245,
          "colspan": 1
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
    "projectStartInfoPanel": {
        "id": "projectStartInfoPanel",
        "buskey": "PhId",
        "bindtable": "ys3_BudgetDtl_TextContent",
        "desTitle": "项目立项情况",
        "columnsPerRow": 1,
        "fields": [
            {
                "id": "FProjBasis",
                "fieldLabel": "立项依据",
                "itemId": "FProjBasis",
                "name": "FProjBasis",
                "maxLength": 500,
                "langKey": "FProjBasis",
                "xtype": "ngTextArea",
                "emptyText": "请输入项目立项依据（限250字）。",
                "height": 70,
                "colspan": 1
            },
            {
                "id": "FFeasibility",
                "fieldLabel": "可行性",
                "itemId": "FFeasibility",
                "name": "FFeasibility",
                "maxLength": 500,
                "langKey": "FFeasibility",
                "xtype": "ngTextArea",
                "emptyText": "请输入项目可行性分析（限250字）。",
                "height": 70,
                "colspan": 1
            },
            {
                "id": "FNecessity",
                "fieldLabel": "必要性",
                "itemId": "FNecessity",
                "name": "FNecessity",
                "maxLength": 500,
                "langKey": "FNecessity",
                "xtype": "ngTextArea",
                "emptyText": "请输入项目必要性分析（限250字）。",
                "height": 70,
                "colspan": 1
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
                        "fieldLabel": "当前版本",
                        "name": "NgRecordVer"
                    }
                ]
            }
        ]
      },
      "PurchaseDtlPanel": {
          "id": "PurchaseDtlPanel",
          "buskey": "PhId",
          "bindtable": "xm3_ProjectDtl_PurchaseDtl",
          "desTitle": "采购明细",
          "columnsPerRow": 3,
          "fields": [
              {
                  "fieldLabel": "采购内容",
                  "itemId": "FContent",
                  "name": "FContent",
                  "mustInput": true,
                  "colspan": 3,
                  "maxLength": 200,
                  "langKey": "FContent",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "采购目录代码",
                  "itemId": "FCatalogCode",
                  "name": "FCatalogCode",
                  "langKey": "FCatalogCode",
                  "helpid": "GHProcurementCatalog",
                  "valueField": "f_Code",
                  "displayField": "f_Name",
                  "userCodeField": "f_Code",
                  "xtype": "ngRichHelp"
              },
              {
                  "fieldLabel": "采购类型代码",
                  "itemId": "FTypeCode",
                  "name": "FTypeCode",
                  "langKey": "FTypeCode",
                  "mustInput": true,
                  "helpid": "GHProcurementType",
                  "valueField": "f_Code",
                  "displayField": "f_Name",
                  "userCodeField": "f_Code",
                  "xtype": "ngRichHelp"
              },
              {
                  "fieldLabel": "采购程序代码",
                  "itemId": "FProcedureCode",
                  "name": "FProcedureCode",
                  "langKey": "FProcedureCode",
                  "mustInput": true,
                  "helpid": "GHProcurementProcedures",
                  "valueField": "f_Code",
                  "displayField": "f_Name",
                  "userCodeField": "f_Code",
                  "xtype": "ngRichHelp"
              },
              {
                  "fieldLabel": "采购数量",
                  "itemId": "FQty",
                  "name": "FQty",
                  "maxLength": 18,
                  "langKey": "FQty",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "计量单位",
                  "itemId": "FMeasUnit",
                  "name": "FMeasUnit",
                  "maxLength": 10,
                  "langKey": "FMeasUnit",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "预计单价",
                  "itemId": "FPrice",
                  "name": "FPrice",
                  "maxLength": 18,
                  "langKey": "FPrice",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "总计金额",
                  "itemId": "FAmount",
                  "name": "FAmount",
                  "maxLength": 18,
                  "langKey": "FAmount",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "主表phid",
                  "itemId": "MstPhid",
                  "name": "MstPhid",
                  "hidden": true,
                  "maxLength": 19,
                  "langKey": "MstPhid",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "明细项目代码",
                  "itemId": "FDtlCode",
                  "name": "FDtlCode",
                  "hidden": true,
                  "maxLength": 255,
                  "langKey": "FDtlCode",
                  "xtype": "ngText"
              },
              {
                  "fieldLabel": "技术参数及配置标准",
                  "itemId": "FSpecification",
                  "name": "FSpecification",
                  "hidden": true,
                  "maxLength": 500,
                  "langKey": "FSpecification1",
                  "xtype": "ngTextArea"
              },
              {
                  "fieldLabel": "备注",
                  "itemId": "FRemark",
                  "name": "FRemark",
                  "hidden": true,
                  "maxLength": 500,
                  "langKey": "FRemark1",
                  "xtype": "ngTextArea"
              },
              {
                  "fieldLabel": "预计采购时间",
                  "itemId": "FEstimatedPurTime",
                  "name": "FEstimatedPurTime",
                  "hidden": true,
                  "langKey": "FEstimatedPurTime1",
                  "xtype": "ngDate"
              },
              {
                  "fieldLabel": "是否绩效评价",
                  "itemId": "FIfPerformanceAppraisal",
                  "name": "FIfPerformanceAppraisal",
                  "hidden": true,
                  "langKey": "FIfPerformanceAppraisal1",
                  "valueField": "code",
                  "displayField": "name",
                  "xtype": "ngComboBox",
                  "queryMode": "local",
                  "data": [
                      {
                          "code": "1",
                          "name": "是"
                      },
                      {
                          "code": "2",
                          "name": "否"
                      }
                  ]
              },
              {
                  "fieldLabel": "明细项目代码",
                  "itemId": "FName",
                  "name": "FName",
                  "hidden": true,
                  "maxLength": 500,
                  "langKey": "FName",
                  "xtype": "ngText"
              },
              {
                  "xtype": "container",
                  "name": "hiddenContainer",
                  "hidden": true,
                  "items": [
                      {
                          "xtype": "hiddenfield",
                          "fieldLabel": "phid",
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
      "PurchaseDtlPanelText": {
          "id": "PurchaseDtlPanelText",
          "buskey": "PhId",
          "bindtable": "xm3_ProjectDtl_PurchaseDtl",
          "desTitle": "采购明细文字区",
          "columnsPerRow": 2,
          "fields": [
              {
                  "fieldLabel": "技术参数及配置标准",
                  "itemId": "FSpecification",
                  "name": "FSpecification",
                  "colspan": 2,
                  "maxLength": 500,
                  "langKey": "FSpecification",
                  "xtype": "ngTextArea"
              },
              {
                  "fieldLabel": "备注",
                  "itemId": "FRemark",
                  "name": "FRemark",
                  "colspan": 2,
                  "maxLength": 500,
                  "langKey": "FRemark",
                  "xtype": "ngTextArea"
              },
              {
                  "fieldLabel": "预计采购时间",
                  "itemId": "FEstimatedPurTime",
                  "name": "FEstimatedPurTime",
                  "langKey": "FEstimatedPurTime",
                  "xtype": "ngDate"
              },
              {
                  "fieldLabel": "是否绩效评价",
                  "itemId": "FIfPerformanceAppraisal",
                  "name": "FIfPerformanceAppraisal",
                  "langKey": "FIfPerformanceAppraisal",
                  "valueField": "code",
                  "displayField": "name",
                  "userCodeField": "test444",
                  "xtype": "ngComboBox",
                  "mustInput": true,
                  "queryMode": "local",
                  "valueType": "int",
                  "data": [
                      {
                          "code": "1",
                          "name": "是"
                      },
                      {
                          "code": "2",
                          "name": "否"
                      }
                  ]
              },
              {
                  "xtype": "container",
                  "name": "hiddenContainer",
                  "hidden": true,
                  "items": [
                      {
                          "xtype": "hiddenfield",
                          "fieldLabel": "phid",
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
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 51,
          "header": "主键",
          "hidden": true
        },
        {
          "LangKey": "MstPhid",
          "dataIndex": "MstPhid",
          "width": 66,
          "header": "主表phid",
          "hidden": true
        },
        {
          "LangKey": "FDtlCode",
          "dataIndex": "FDtlCode",
            "width": 100,
            "hidden": true,
          "header": "明细项目代码",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "FName",
          "mustInput": true,
          "dataIndex": "FName",
          "width": 226,
          "header": "明细项目名称",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "FAmount",
          "mustInput": true,
          "dataIndex": "FAmount",
          "width": 100,
            "header": "金额",
            "align": 'right',
          "editor": {
            "xtype": "ngNumber"
            }
        },
        {
          "LangKey": "FSourceOfFunds",
          "mustInput": true,
          "dataIndex": "FSourceOfFunds_EXName",
          "width": 100,
          "header": "资金来源",
          "editor": {
            "helpid": "GHSourceOfFunds",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "ORMMode": false,
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FBudgetAmount",
          "dataIndex": "FBudgetAmount",
          "width": 100,
            "header": "预算金额",
            "align": 'right',
          "hidden": true,
          "editor": {
            "xtype": "ngNumber"
          }
        },
        {
          "LangKey": "FPaymentMethod",
          "dataIndex": "FPaymentMethod_EXName",
          "width": 100,
          "header": "支付方式",
          "editor": {
            "helpid": "GHPaymentMethod",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "ORMMode": false,
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
          },
          {
              "LangKey": "FBudgetAccounts",
              "dataIndex": "FBudgetAccounts_EXName",
              "width": 100,
              "header": "预算科目",
              "editor": {
                  "helpid": "GHBudgetAccounts",
                  "valueField": "kmdm",
                  "displayField": "kmmc",
                  "userCodeField": "kmdm",
                  "ORMMode": false,
                  "isInGrid": true,
                  "helpResizable": true,
                  "xtype": "ngRichHelp"
              }
          },
          {
              "LangKey": "FExpensesChannel",
              "dataIndex": "FExpensesChannel_EXName",
              "width": 100,
              "header": "支出渠道",
              "editor": {
                  "helpid": "GHExpensesChannel",
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
              "LangKey": "FIfPurchase",
              "dataIndex": "FIfPurchase",
              "width": 100,
              "header": "是否集中采购",
              "editor": {
                  "xtype": "ngComboBox",
                  "valueField": "code",
                  "displayField": "name",
                  "QueryMode": "local",
                  "valueType": "int",
                  "data": [
                      {
                          "code": "1",
                          "name": "是"
                      },
                      {
                          "code": "2",
                          "name": "否"
                      }
                  ]
              }
          },
          //{
          //    "LangKey": "FIfPurchase",
          //    "dataIndex": "FIfPurchase",
          //    "width": 100,
          //    "header": "集中采购",
          //    "editor": {
          //        "xtype": "ngComboBox",
          //        "data": [
          //            {
          //                "code": 1,
          //                "name": "是"
          //            },
          //            {
          //                "code": 2,
          //                "name": "否"
          //            }
          //        ]
          //    }
          //}, 
        {
          "LangKey": "FFeedback",
          "dataIndex": "FFeedback",
            "width": 170,
            "hidden": true,
          "header": "反馈意见",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "FOtherInstructions",
          "dataIndex": "FOtherInstructions",
          "width": 376,
          "header": "测算过程及其他说明的事项",
          "editor": {
            "xtype": "ngText"
          }
          },
          {
              "LangKey": "FMidEdit",
              "dataIndex": "FMidEdit",
              "width": 100,
              "hidden": true,
              "header": "年中调整判断",
              "editor": {
                  "xtype": "ngText"
              }
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
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 69,
          "header": "主键",
          "hidden": true
        },
        {
          "LangKey": "MstPhid",
          "dataIndex": "MstPhid",
          "width": 77,
          "header": "主表phid",
          "hidden": true
        },
        {
          "LangKey": "FSourceOfFunds",
          "dataIndex": "FSourceOfFunds_EXName",
          "width": 429,
          "header": "资金来源",
          "readOnly":true,
          "editor": {
            "helpid": "GHSourceOfFunds",
            "valueField": "dm",
            "displayField": "mc",
            "userCodeField": "dm",
            "ORMMode": false,
            "isInGrid": true,
            "helpResizable": true,
            "xtype": "ngRichHelp"
          }
        },
        {
          "LangKey": "FAmount",
          "dataIndex": "FAmount",
          "width": 153,
            "header": "金额(元)",
            "align": 'right',
          "readOnly": true,
          "editor": {
            "xtype": "ngNumber"
          }
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
          "LangKey": "PhId",
          "dataIndex": "PhId",
          "width": 57,
          "header": "主键",
          "hidden": true
        },
        {
          "LangKey": "MstPhid",
          "dataIndex": "MstPhid",
          "width": 73,
          "header": "主表phid",
          "hidden": true
        },
        {
          "LangKey": "FImplContent",
          "mustInput": true,
          "dataIndex": "FImplContent",
          "width": 395,
          "header": "实施内容",
          "editor": {
            "xtype": "ngText"
          }
        },
        {
          "LangKey": "FStartDate",
          "dataIndex": "FStartDate",
          "width": 100,
          "format": "Y-m-d",
          "header": "开始日期",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "FEndDate",
          "dataIndex": "FEndDate",
          "width": 100,
          "format": "Y-m-d",
          "header": "结束日期",
          "editor": {
            "xtype": "ngDate"
          }
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
          "header": "主键",
          "hidden": true
        },
        {
          "LangKey": "FDeclarationUnit",
          "dataIndex": "FDeclarationUnit_EXName",
          "width": 100,
          "header": "申报单位"
        },
        {
          "LangKey": "FBudgetDept",
          "dataIndex": "FBudgetDept_EXName",
          "width": 100,
          "header": "预算部门"
        },
        {
          "LangKey": "FDeclarationDept",
          "dataIndex": "FDeclarationDept_EXName",
          "width": 100,
          "header": "申报部门",
          "hidden": true
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
          "dataIndex": "FExpenseCategory_EXName",
          "width": 100,
            "header": "项目类型"
        },
        {
          "LangKey": "FDuration",
          "dataIndex": "FDuration",
          "width": 100,
          "header": "存续期限"
        },
        {
          "LangKey": "FProjAttr",
          "dataIndex": "FProjAttr",
          "width": 100,
          "header": "项目属性"
        },
        {
          "LangKey": "FProjStatus",
          "dataIndex": "FProjStatus",
          "width": 100,
          "header": "项目状态"
          },
          {
              "LangKey": "FApproveStatus",
              "dataIndex": "FApproveStatus",
              "width": 100,
              "header": "单据状态"
          },
        {
          "LangKey": "FIfPerformanceAppraisal",
          "dataIndex": "FIfPerformanceAppraisal",
          "width": 100,
          "header": "绩效评价"
        },
        {
          "LangKey": "FProjAmount",
          "dataIndex": "FProjAmount",
            "width": 100,
            "align": 'right',
          "header": "项目金额"
        },
        
        {
          "LangKey": "FStartDate",
          "dataIndex": "FStartDate",
          "width": 100,
          "format": "Y-m-d",
          "header": "开始日期"
        },
        {
          "LangKey": "FEndDate",
          "dataIndex": "FEndDate",
          "width": 100,
          "format": "Y-m-d",
          "header": "结束日期"
        },
        {
          "LangKey": "FDateofDeclaration",
          "dataIndex": "FDateofDeclaration",
          "width": 100,
          "format": "Y-m-d",
          "header": "申报日期"
        },
        {
          "LangKey": "FDeclarer",
          "dataIndex": "FDeclarer",
          "width": 100,
          "header": "申报人"
        },
        {
          "LangKey": "FApprover",
          "dataIndex": "FApprover_EXName",
          "width": 100,
          "header": "审批人"
        },
        {
          "LangKey": "FApproveDate",
          "dataIndex": "FApproveDate",
          "width": 100,
          "format": "Y-m-d",
          "header": "审批日期"
        }
      ]
    },
    "PerformTargetPanel": {
        "id": "PerformTargetPanel",
        "buskey": "PhId",
        "bindtable": "xm3_ProjectDtl_PerformTar",
        "desTitle": "绩效目标分解",
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
              "hidden": true
          },
          {
              "LangKey": "MstPhid",
              "dataIndex": "MstPhid",
              "width": 100,
              "header": "主表phid",
              "hidden": true
          },
          {
              "header": "指标类别",
              "columns": [
                {
                    "LangKey": "FTargetTypeCode",
                    "dataIndex": "FTargetTypeCode_EXName",
                    "width": 100,
                    "header": "", //类型
                    "editor": {
                        "helpid": "GHPerformEvalTargetType",
                        "valueField": "f_Code",
                        "displayField": "f_Name",
                        "userCodeField": "f_Code",
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                },
                {
                    "LangKey": "FTargetClassCode",
                    "dataIndex": "FTargetClassCode_EXName",
                    "width": 100,
                    "header": "", //类别
                    "editor": {
                        "helpid": "GHPerformEvalTargetClass",
                        "valueField": "f_Code",
                        "displayField": "f_Name",
                        "userCodeField": "f_Code",
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                }
              ]
          },
          {
              "LangKey": "FTargetCode",
              "dataIndex": "FTargetCode",
              "width": 100,
              "header": "指标代码",
              "editor": {
                  "helpid": "GHPerformEvalTarget",
                  "valueField": "f_targetcode",
                  "displayField": "f_targetcode",
                  "userCodeField": "f_targetcode",
                  "ORMMode": false,
                  "isInGrid": true,
                  "helpResizable": true,
                  "xtype": "ngRichHelp",
                  "editable": false
              }
          },
          {
              "LangKey": "FTargetName",
              "dataIndex": "FTargetName",
              "width": 162,
              "header": "指标名称",
              "editor": {
                  "xtype": "ngText"
              }
          },
          {
              "LangKey": "FTargetValue",
              "dataIndex": "FTargetValue",
              "width": 100,
              "header": "指标值",
              "editor": {
                  "xtype": "ngText"
              }
          },
          {
              "LangKey": "FTargetWeight",
              "dataIndex": "FTargetWeight",
              "width": 79,
              "header": "指标权重",
              "editor": {
                  "xtype": "ngNumber",
                  "maxValue": 100.0,
                  "minValue":0
              }
          },
          {
              "LangKey": "FTargetDescribe",
              "dataIndex": "FTargetDescribe",
              "width": 359,
              "header": "指标描述",
              "editor": {
                  "xtype": "ngText"
              }
          },
          {
              "LangKey": "FIfCustom",
              "dataIndex": "FIfCustom",
              "width": 100,
              "header": "是否用户增加",
              "hidden": true,
              "editor": {
                  "xtype": "ngComboBox",
                  "valueField": "code",
                  "displayField": "name",
                  "QueryMode": "local",
                  "valueType": "int",
                  "data": [
                    {
                        "code": 0,
                        "name": ""
                    },
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
          }
        ]
      },
      "PurDtl4SOFGrid": {
          "id": "PurDtl4SOFGrid",
          "buskey": "PhId",
          "bindtable": "xm3_ProjectDtl_PurDtl4SOF",
          "desTitle": "采购明细-资金来源",
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
                  "readOnly": true,
                  "width": 100,
                  "header": "资金来源"
              },
              {
                  "LangKey": "FAmount",
                  "dataIndex": "FAmount",
                  "width": 100,
                  "header": "金额"
              },
              {
                  "LangKey": "MstPhid",
                  "dataIndex": "MstPhid",
                  "width": 100,
                  "header": "主表phid",
                  "hidden": true
              },
              
              {
                  "LangKey": "FDtlCode",
                  "dataIndex": "FDtlCode",
                  "width": 100,
                  "header": "明细项目代码",
                  "hidden": true
              },
              {
                  "LangKey": "FName",
                  "dataIndex": "FName",
                  "width": 100,
                  "header": "明细项目名称",
                  "hidden": true
              }
          ]
      }
  },
  "fieldSetForm": {

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
            "title": "总体绩效目标",
          "langKey": "TabPage7"
        },
        {
          "id": "TabPage8",
          "title": "年度绩效目标",
          "langKey": "TabPage8"
        },
        {
            "id": "TabPage9",
            "title": "绩效目标分解",
            "langKey": "TabPage9"
        }
      ]
      },
      "PurDtl4SOFTab": {
          "id": "PurDtl4SOFTab",
          "desTitle": "资金来源",
          "items": [
              {
                  "id": "TabPage1",
                  "title": "资金来源",
                  "langKey": "TabPage1"
              }
          ]
      }
  }
}
