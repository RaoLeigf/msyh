var individualConfigInfo =
{
    "form": {
        "mainPanel": {
            "id": "mainPanel",
            "buskey": "PhId",
            "bindtable": "ys3_expensemst",
            "desTitle": "用款计划基础信息",
            "columnsPerRow": 3,
            "labelWidth": 120,
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
                    "ORMMode": false,
                    "xtype": "ngRichHelp",
                    "hidden": true
                },
                {
                    "fieldLabel": "申报部门",
                    "itemId": "FBudgetDept",
                    "name": "FBudgetDept",
                    //"mustInput": true,
                    "langKey": "FBudgetDept",
                    "helpid": "ys_orglist",
                    "valueField": "ocode",
                    "displayField": "oname",
                    "userCodeField": "ocode",
                    "ORMMode": false,
                    "xtype": "ngRichHelp",
                    "mustInput": true,
					"editable": false,
					"readOnly": true
                },
				{
                    "fieldLabel": "预算部门",
                    "itemId": "FDeclarationDept",
                    "name": "FDeclarationDept",
                    //"mustInput": true,
                    "langKey": "FDeclarationDept",
                    "helpid": "ys_orglist",
                    "valueField": "ocode",
                    "displayField": "oname",
                    "userCodeField": "ocode",
                    "ORMMode": false,
                    "xtype": "ngRichHelp",
                    "mustInput": true,
					"editable": false
                },
                {
                    "fieldLabel": "申报日期",
                    "itemId": "FDateofdeclaration",
                    "name": "FDateofdeclaration",
                    "langKey": "FDateofdeclaration",
                    "xtype": "ngDate",
                    "mustInput": true,
					"editable": false
                },
                {
                    "fieldLabel": "项目代码",
                    "itemId": "FProjcode",
                    "name": "FProjcode",
                    "hidden": true,
                    "maxLength": 40,
                    "langKey": "FProjcode",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "项目名称",
                    "itemId": "FProjname",
                    "name": "FProjname",
                    "langKey": "FProjname",
                    "helpid": "GHXMName",
                    "valueField": "f_projcode",
                    "displayField": "f_projname",
                    "userCodeField": "f_projcode",
                    "ORMMode": false,
                    "xtype": "ngRichHelp",
                    "editable": false,
                    "mustInput": true
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
                    "xtype": "ngRichHelp",
                    "readOnly": true
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
                    "xtype": "ngNumber",
                    "readOnly": true,
                    //"labelStyle": "background-color:OrangeRed",
                },
				{
                    "fieldLabel": "已编报数",
                    "itemId": "Useamount",
                    "name": "Useamount",
                    "maxLength": 18,
                    "langKey": "Useamount",
                    "xtype": "ngNumber",
                    "readOnly": true
                },
                {
                    "fieldLabel": "剩余可编报数",
                    "itemId": "FPlayamount",
                    "name": "FPlayamount",
                    "maxLength": 18,
                    "langKey": "FPlayamount",
                    "xtype": "ngNumber",
                    "readOnly": true
                },
                {
                    "fieldLabel": "预计支出金额",
                    "itemId": "FSurplusamount",
                    "name": "FSurplusamount",
                    "maxLength": 18,
                    "langKey": "FSurplusamount",
                    "xtype": "ngNumber",
                    "readOnly": true,
					"hidden": true
                },
                {
                    "fieldLabel": "预计返回金额",
                    "itemId": "FReturnamount",
                    "name": "FReturnamount",
                    "maxLength": 18,
                    "langKey": "FReturnamount",
                    "xtype": "ngNumber",
                    "readOnly": true
                },
				{
                    "fieldLabel": "账务实际发生数",
                    "itemId": "CWamount",
                    "name": "CWamount",
                    "maxLength": 18,
                    "langKey": "CWamount",
                    "xtype": "ngNumber",
                    "readOnly": true
                },
                {
                    "fieldLabel": "是否额度返还",
                    "itemId": "FIfpurchase",
                    "name": "FIfpurchase",
                    "hidden": true,
                    "maxLength": 19,
                    "langKey": "FIfpurchase",
                    "xtype": "ngText"
                },
                {
                    "id": "FApprovestatus",
                    "fieldLabel": "审批状态",
                    "itemId": "FApprovestatus",
                    "name": "FApprovestatus",
                    "hidden": true,
                    "maxLength": 2,
                    "langKey": "FApprovestatus",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "申报人",
                    "itemId": "FDeclarer",
                    "name": "FDeclarer",
                    "maxLength": 20,
                    "langKey": "FDeclarer",
                    "xtype": "ngText",
                    "hidden": true
                },
				{
                    "fieldLabel": "是否额度核销（0否1是）",
                    "itemId": "FIfKeyEvaluation",
                    "name": "FIfKeyEvaluation",
                    "maxLength": 20,
                    "langKey": "FIfKeyEvaluation",
                    "xtype": "ngText",
                    "hidden": true
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
                    "fieldLabel": "单据号",
                    "itemId": "FPerformevaltype",
                    "name": "FPerformevaltype",
                    "hidden": true,
                    "langKey": "FPerformevaltype",
                    "xtype": "ngText"
                },
                {
                    "xtype": "container",
                    "name": "hiddenContainer",
                    "hidden": true,
                    "items": [
                        //{
                        //    "xtype": "hiddenfield",
                        //    "fieldLabel": "主键",
                        //    "name": "PhId"
                        //},
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
                    "width": 50
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
                    "width": 326,
                    "header": "明细费用名称",
                    "editor": {
                        "xtype": "ngText"
                    },
                    "mustInput": true
                },
				{
                    "LangKey": "FDtlName",
                    "dataIndex": "FDtlName",
                    "width": 326,
                    "header": "明细项目名称",
                    "editor": {
                        "helpid": "ghYSBudgetDtl",
                        "valueField": "f_DtlCode",
                        "displayField": "f_Name",
                        "userCodeField": "f_DtlCode",
                        "isInGrid": true,
                        "helpResizable": true,
                        "ORMMode": false,
                        "xtype": "ngRichHelp"
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "FSourceoffunds",
                    "dataIndex": "FSourceoffunds_EXName",
                    "width": 260,
                    "header": "资金来源",
                    "editor": {
                        "helpid": "GHSourceOfFunds",
                        "valueField": "dm",
                        "displayField": "mc",
                        "userCodeField": "dm",
                        "isInGrid": true,
                        "helpResizable": true,
                        "ORMMode": false,
                        "xtype": "ngRichHelp"
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "FBudgetaccounts",
                    "dataIndex": "FBudgetaccounts_EXName",
                    "width": 260,
                    "header": "预算科目",
                    "editor": {
                        "helpid": "GHBudgetAccounts",
                        "valueField": "kmdm",
                        "displayField": "kmmc",
                        "userCodeField": "kmdm",
                        "isInGrid": true,
                        "helpResizable": true,
                        "ORMMode": false,
                        "xtype": "ngRichHelp"
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "FExpenseschannel",
                    "dataIndex": "FExpenseschannel_EXName",
                    "width": 260,
                    "header": "支出渠道",
                    "editor": {
                        "helpid": "GHExpensesChannel",
                        "valueField": "ocode",
                        "displayField": "oname",
                        "userCodeField": "ocode",
                        "isInGrid": true,
                        "helpResizable": true,
                        "ORMMode": false,
                        "xtype": "ngRichHelp"
                    },
                    "mustInput": true
                },
                /*{
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
                },*/
                {
                    "LangKey": "FAmount",
                    "dataIndex": "FAmount",
                    "width": 220,
                    "header": "预计支出金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "FReturnamount",
                    "dataIndex": "FReturnamount",
                    "width": 220,
                    "header": "预计返还金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FOtherinstructions",
                    "dataIndex": "FOtherinstructions",
                    "width": 400,
                    "header": "测算过程及其他需要说明的事项",
                    "editor": {
                        "xtype": "ngText"
                    },
                    "mustInput": true
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
                    "width": 50
                },
				{
                    "LangKey": "FPerformevaltype",
                    "dataIndex": "FPerformevaltype",
                    "width": 200,
                    "header": "单据号"
                },
                {
                    "LangKey": "FDeclarationunit",
                    "dataIndex": "FDeclarationunit_EXName",
                    "width": 200,
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
                    "width": 200,
                    "header": "申报部门",
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
                    "LangKey": "FDeclarationDept",
                    "dataIndex": "FDeclarationDept_EXName",
                    "width": 200,
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
                    "LangKey": "FProjcode",
                    "dataIndex": "FProjcode",
                    "width": 150,
                    "header": "项目编码",
                    //"hidden": true
                },
                {
                    "LangKey": "FProjname",
                    "dataIndex": "FProjname",
                    "width": 200,
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
                            },
                            {
                                "code": "4",
                                "name": "额度返还"
                            }
                        ]
                    }
                },
                {
                    "LangKey": "FNextApprove",
                    "dataIndex": "FNextApprove",
                    "width": 150,
                    "header": "下一审批岗位"
                },
                {
                    "LangKey": "FDeclarer",
                    "dataIndex": "FDeclarer",
                    "width": 200,
                    "header": "申报人"
                },
                {
                    "LangKey": "FDateofdeclaration",
                    "dataIndex": "FDateofdeclaration",
                    "width": 200,
                    "header": "申报日期",
                    "editor": {
                        "xtype": "ngDate"
                    }
                },
                {
                    "LangKey": "FApprover",
                    "dataIndex": "FApprover_EXName",
                    "width": 200,
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
                    "width": 200,
                    "header": "审批时间",
                    "editor": {
                        "xtype": "ngDate"
                    }
                },
                {
                    "LangKey": "FYear",
                    "dataIndex": "FYear",
                    "width": 150,
                    "header": "项目年度",
                    "hidden": true
                },
                {
                    "LangKey": "FProjAttr",
                    "dataIndex": "FProjAttr",
                    "width": 150,
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
                    "width": 150,
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
                    "width": 200,
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
                    "width": 200,
                    "header": "开始日期",
                    "hidden": true,
                    "editor": {
                        "xtype": "ngDate"
                    }
                },
                {
                    "LangKey": "FDeclarationDept",
                    "dataIndex": "FDeclarationDept_EXName",
                    "width": 200,
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
                    "width": 200,
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
                    "width": 200,
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
                    "width": 200,
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
                    "width": 200,
                    "header": "可编报数",
                    "hidden": true,
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FServiceDept",
                    "dataIndex": "FServiceDept",
                    "width": 200,
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
                    "width": 200,
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
