var individualConfigInfo =
    {
        "form": {
            "mainPanel": {
                "id": "mainPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetMst",
                "desTitle": "预算基础信息",
                "columnsPerRow": 4,
                "fields": [
                    {
                        "id": "FDeclarationUnit",
                        "fieldLabel": "申报单位",
                        "itemId": "FDeclarationUnit",
                        "name": "FDeclarationUnit",
                        "mustInput": true,
                        "langKey": "FDeclarationUnit",
                        "helpid": "cboo",
                        "valueField": "PhId",
                        "displayField": "OName",
                        "userCodeField": "OCode",
                        "xtype": "ngRichHelp"
                    },
                    {
                        "id": "FBudgetDept",
                        "fieldLabel": "预算部门",
                        "itemId": "FBudgetDept",
                        "name": "FBudgetDept",
                        "mustInput": true,
                        "langKey": "FBudgetDept",
                        "helpid": "dept",
                        "valueField": "PhId",
                        "displayField": "OName",
                        "userCodeField": "OCode",
                        "xtype": "ngRichHelp",
                        "listeners": {
                            "beforetriggerclick": function (obj) {
                                var FDeclarationUnit = Ext.getCmp('FDeclarationUnit').getValue();
                                //var FBudgetDept = Ext.getCmp('FBudgetDept').getValue();
                                if (!FDeclarationUnit) {
                                    Ext.MessageBox.alert("'提示'", '申报单位不能为空');
                                    return false;
                                }
                                //if (!FBudgetDept) {
                                //    Ext.MessageBox.alert("'提示'", '预算部门不能为空');
                                //    return false;
                                //}

                                //obj.setOutFilter({ f_DeclarationUnit: FDeclarationUnit, f_BudgetDept: FBudgetDept  });
                            }
                        }
                    },
                    {
                        "fieldLabel": "项目名称",
                        "itemId": "FProjName",
                        "name": "FProjName",
                        "colspan": 2,
                        "mustInput": true,
                        "langKey": "FProjName",
                        "helpid": "xm3_xmlist",
                        "valueField": "PhId",
                        "displayField": "FProjName",
                        "userCodeField": "FProjCode",
                        "acceptInput": true,
                        "xtype": "ngRichHelp",
                        "listeners": {
                            "helpselected": function (str) {
                                var PhId = str.data["PhId"];
                                var FProjName = str.data["FProjName"];

                                dataFromXmk(PhId);
                                // Ext.getCmp('FProjName').setValue(FProjName);
                            },
                            "beforetriggerclick": function (obj) {
                                var otype = tableOtype();
                                if (otype == "edit") {
                                    Ext.MessageBox.alert("提示", '修改时不能重新引用项目!');
                                    return false;
                                }
                                var FDeclarationUnit = Ext.getCmp('FDeclarationUnit').getValue();
                                var FBudgetDept = Ext.getCmp('FBudgetDept').getValue();
                                if (!FDeclarationUnit) {
                                    Ext.MessageBox.alert("'提示'", '申报单位不能为空');
                                    return false;
                                }
                                if (!FBudgetDept) {
                                    Ext.MessageBox.alert("'提示'", '预算部门不能为空');
                                    return false;
                                }

                                obj.setOutFilter({ f_DeclarationUnit: FDeclarationUnit, f_BudgetDept: FBudgetDept });
                            }
                        }
                        //"fieldLabel": "项目名称",
                        //"itemId": "FProjName",
                        //  "name": "FProjName",
                        //  "mustInput": true,
                        //"maxLength": 100,
                        //"colspan": 2,
                        //"langKey": "FProjName",
                        //  "helpid": "xm3_xmlist",
                        //  "valueField": "PhId",
                        //  "displayField": "OName",
                        //  "userCodeField": "OCode",
                        //  "xtype": "ngRichHelp"

                        //"fieldLabel": "项目名称",
                        // "itemId": "FProjName",
                        // "name": "FProjName",
                        // "mustInput": true,
                        // "maxLength": 100,
                        // "colspan": 2,
                        // "langKey": "FProjName",
                        // "helpid": "cboo",
                        // "valueField": "PhId",
                        // "displayField": "OName",
                        // "userCodeField": "OCode",
                        // "editable": true,
                        // "showCommonUse": false, //是否显示常用
                        // "acceptInput": true,//接受用户自由输入的值
                        // "listFields": "OCode,OName",
                        // "listHeadTexts": "1,2",
                        // //"outFilter": "1243",
                        // // "showTreeProj": true, 
                        // "xtype": "ngRichHelp"
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
                                "name": "经常性项目"
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
                        "fieldLabel": "项目年度",
                        "itemId": "FYear",
                        "name": "FYear",
                        "maxLength": 4,
                        "langKey": "FYear",
                        "xtype": "ngText"
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
                        "fieldLabel": "会议时间",
                        "itemId": "FMeetingTime",
                        "name": "FMeetingTime",
                        "langKey": "FMeetingTime",
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
                        "id": "PhId",
                        "fieldLabel": "主键",
                        "itemId": "PhId",
                        "name": "PhId",
                        "hidden": true,
                        "maxLength": 19,
                        "langKey": "PhId",
                        "xtype": "ngText"
                    },
                    {
                        "id": "XmMstPhid",
                        "fieldLabel": "项目表主键",
                        "itemId": "XmMstPhid",
                        "name": "XmMstPhid",
                        "hidden": true,
                        "maxLength": 19,
                        "langKey": "XmMstPhid",
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
                "bindtable": "ys3_BudgetDtl_TextContent",
                "desTitle": "部门职能概述",
                "columnsPerRow": 1,
                "fields": [
                    {
                        "id": "FFunctionalOverview",
                        "fieldLabel": "部门职能概述",
                        "itemId": "FFunctionalOverview",
                        "name": "FFunctionalOverview",
                        "maxLength": 1000,
                        "langKey": "FFunctionalOverview",
                        "xtype": "ngTextArea",
                        "width": "100%",
                        "height": "100%"
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
                        "fieldLabel": "项目表phid",
                        "itemId": "XmPhid",
                        "name": "XmPhid",
                        "hidden": true,
                        "maxLength": 19,
                        "langKey": "XmPhid",
                        "xtype": "ngText"
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
                        "fieldLabel": "长期绩效目标",
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
                                "fieldLabel": "当前版本",
                                "name": "NgRecordVer"
                            }
                        ]
                    }
                ]
            },
            "ProjOverviewPanel": {
                "id": "ProjOverviewPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetDtl_TextContent",
                "desTitle": "项目概况",
                "columnsPerRow": 1,
                "fields": [
                    {
                        "id": "FProjOverview",
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
                                "fieldLabel": "当前版本",
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
                        "xtype": "ngTextArea"
                    },
                    {
                        "id": "FFeasibility",
                        "fieldLabel": "可行性",
                        "itemId": "FFeasibility",
                        "name": "FFeasibility",
                        "maxLength": 500,
                        "langKey": "FFeasibility",
                        "xtype": "ngTextArea"
                    },
                    {
                        "id": "FNecessity",
                        "fieldLabel": "必要性",
                        "itemId": "FNecessity",
                        "name": "FNecessity",
                        "maxLength": 500,
                        "langKey": "FNecessity",
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
                                "fieldLabel": "当前版本",
                                "name": "NgRecordVer"
                            }
                        ]
                    }
                ]
            },
            "longTargetPanel": {
                "id": "longTargetPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetDtl_TextContent",
                "desTitle": "长期绩效目标",
                "columnsPerRow": 1,
                "fields": [
                    {
                        "id": "FLTPerformGoal",
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
                                "fieldLabel": "当前版本",
                                "name": "NgRecordVer"
                            }
                        ]
                    }
                ]
            },
            "yearTargetPanel": {
                "id": "yearTargetPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetDtl_TextContent",
                "desTitle": "年度绩效目标",
                "columnsPerRow": 1,
                "fields": [
                    {
                        "id": "FAnnualPerformGoal",
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
                                "fieldLabel": "当前版本",
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
                "bindtable": "ys3_BudgetDtl_BudgetDtl",
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
                        "width": 100,
                        "hidden": true,
                        "header": "主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "MstPhid",
                        "dataIndex": "MstPhid",
                        "width": 100,
                        "hidden": true,
                        "header": "主表主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "FDtlCode",
                        "dataIndex": "FDtlCode",
                        "width": 100,
                        "header": "明细项目代码",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "TName",
                        "mustInput": true,
                        "dataIndex": "TName",
                        "width": 100,
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
                        "header": "金额(元)",
                        "editor": {
                            "xtype": "ngNumber"
                        }
                    },
                    {
                        "LangKey": "FSourceOfFunds",
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
                        "LangKey": "FBudgetAccounts",
                        "dataIndex": "FBudgetAccounts",
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
                    //{
                    //    "LangKey": "FBudgetAccounts",
                    //    "dataIndex": "FBudgetAccounts",
                    //    "width": 100,
                    //    "header": "预算科目",
                    //    "editor": {
                    //        "xtype": "ngText"
                    //    }
                    //},
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
                        "LangKey": "FExpensesChannel",
                        "dataIndex": "FExpensesChannel",
                        "width": 100,
                        "header": "支出渠道",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "FOtherInstructions",
                        "dataIndex": "FOtherInstructions",
                        "width": 100,
                        "header": "其他说明",
                        "editor": {
                            "xtype": "ngText"
                        }
                    }
                ]
            },
            "FundApplPanel": {
                "id": "FundApplPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetDtl_FundAppl",
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
                        "width": 100,
                        "hidden": true,
                        "header": "主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "MstPhid",
                        "dataIndex": "MstPhid",
                        "width": 100,
                        "hidden": true,
                        "header": "主表主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "FSourceOfFunds",
                        "dataIndex": "FSourceOfFunds_EXName",
                        "width": 429,
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
                        "LangKey": "FAmount",
                        "dataIndex": "FAmount",
                        "width": 100,
                        "header": "金额(元)",
                        "editor": {
                            "xtype": "ngNumber"
                        }
                    }
                ]
            },
            "ImplPlanPanel": {
                "id": "ImplPlanPanel",
                "buskey": "PhId",
                "bindtable": "ys3_BudgetDtl_ImplPlan",
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
                        "width": 100,
                        "hidden": true,
                        "header": "主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "MstPhid",
                        "dataIndex": "MstPhid",
                        "width": 100,
                        "hidden": true,
                        "header": "主表主键",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "FImplContent",
                        "dataIndex": "FImplContent",
                        "mustInput": true,
                        "width": 317,
                        "header": "实施内容",
                        "editor": {
                            "xtype": "ngText"
                        }
                    },
                    {
                        "LangKey": "FStartDate",
                        "dataIndex": "FStartDate",
                        "width": 150,
                        "format": "Y-m-d",
                        "header": "开始日期",
                        "editor": {
                            "xtype": "ngDate"
                        }
                    },
                    {
                        "LangKey": "FEndDate",
                        "dataIndex": "FEndDate",
                        "width": 150,
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
                "bindtable": "ys3_BudgetMst",
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
                        "hidden": true,
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
                        "LangKey": "FProjAttr",
                        "dataIndex": "FProjAttr",
                        "width": 100,
                        "header": "项目属性",
                        "editor": {
                            "xtype": "ngComboBox",
                            "data": [
                                {
                                    "code": 1,
                                    "name": "延续项目"
                                },
                                {
                                    "code": 2,
                                    "name": "经常性项目"
                                }
                            ]
                        }
                    },
                    {
                        "LangKey": "FProjName",
                        "dataIndex": "FProjName",
                        "width": 100,
                        "header": "项目名称"
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
                        }
                    },
                    {
                        "LangKey": "FExpenseCategory",
                        "dataIndex": "FExpenseCategory",
                        "width": 100,
                        "header": "项目类型",
                        "editor": {
                            "xtype": "ngText"
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
                        "LangKey": "FProjCode",
                        "dataIndex": "FProjCode",
                        "width": 100,
                        "header": "项目编码"
                    },
                    {
                        "LangKey": "FProjStatus",
                        "dataIndex": "FProjStatus",
                        "width": 100,
                        "header": "项目状态",
                        "editor": {
                            "xtype": "ngComboBox"
                        }
                    },
                    {
                        "LangKey": "FApproveStatus",
                        "dataIndex": "FApproveStatus",
                        "width": 100,
                        "header": "审批状态",
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
                    },
                    {
                        "LangKey": "FVerNo",
                        "dataIndex": "FVerNo",
                        "width": 100,
                        "header": "调整版本号"
                    },
                    {
                        "LangKey": "FDateofDeclaration",
                        "dataIndex": "FDateofDeclaration",
                        "width": 100,
                        "format": "Y-m-d",
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
                    },
                    {
                        "LangKey": "FApprover",
                        "dataIndex": "FApprover",
                        "width": 100,
                        "header": "审批人"
                    },
                    {
                        "LangKey": "FApproveDate",
                        "dataIndex": "FApproveDate",
                        "format": "Y-m-d",
                        "width": 100,
                        "header": "审批时间",
                        "editor": {
                            "xtype": "ngDate"
                        }
                    }
                ]
            }
        },
        "fieldSetForm": {},
        "tabPanel": {
            "DtlPanel": {
                "id": "DtlPanel",
                "desTitle": "详细信息",
                "items": [
                    {
                        "id": "TabPage1",
                        "title": "职能部门概述",
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
