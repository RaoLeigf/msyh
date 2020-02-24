var individualConfigInfo =
{
    "form": {
        "GHSubject": {
            "id": "GHSubject",
            "buskey": "PhId",
            "bindtable": "ys3_Subject",
            "desTitle": "基本支出",
            "columnsPerRow": 4,
            "fields": [
                {
                    "fieldLabel": "申报单位",
                    "itemId": "FDeclarationUnit",
                    "name": "FDeclarationUnit",
                    "langKey": "FDeclarationUnit",
                    "helpid": "sb_orglist",
                    "valueField": "ocode",
                    "displayField": "oname",
                    "userCodeField": "ocode",
                    "readOnly": true,
                    "xtype": "ngRichHelp"
                },
                {
                    "fieldLabel": "填报部门",
                    "itemId": "FDeclarationDept",
                    "name": "FDeclarationDept",
                    "langKey": "FDeclarationDept",
                    //"helpid": "dept4ocode",
					"helpid": "ys_orglist",
                    "valueField": "ocode",
                    "displayField": "oname",
                    "userCodeField": "ocode",
                    //"readOnly": true,
                    "xtype": "ngRichHelp"
                },
                {
                    "fieldLabel": "申报日期",
                    "itemId": "FDateofDeclaration",
                    "name": "FDateofDeclaration",
                    //"hidden": true,
                    "langKey": "FDateofDeclaration",
                    "xtype": "ngDate"
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
                    "hidden": true,
                    "readOnly": true,
                    "xtype": "ngRichHelp"
                },
                {
                    "fieldLabel": "申报人",
                    "itemId": "FDeclarer",
                    "name": "FDeclarer",
                    "maxLength": 20,
                    "langKey": "FDeclarer",
                    "readOnly": true,
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "项目年度",
                    "itemId": "FYear",
                    "name": "FYear",
                    "hidden": true,
                    "maxLength": 4,
                    "langKey": "FYear",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "项目代码",
                    "itemId": "FProjCode",
                    "name": "FProjCode",
                    "hidden": true,
                    "maxLength": 255,
                    "langKey": "FProjCode",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "任务名称",
                    "itemId": "FProjName",
                    "name": "FProjName",
                    "mustInput": true,
                    //"hidden": true,
                    "colspan": 2,
                    "maxLength": 100,
                    "langKey": "FProjName",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "项目属性",
                    "itemId": "FProjAttr",
                    "name": "FProjAttr",
                    "hidden": true,
                    "langKey": "FProjAttr",
                    "valueField": "code",
                    "displayField": "name",
                    "xtype": "ngComboBox",
                    "queryMode": "local",
                    "data": [
                        {
                            "code": "1",
                            "name": "支出"
                        },
                        {
                            "code": "2",
                            "name": "收入"
                        }
                    ]
                },
                {
                    "fieldLabel": "项目状态",
                    "itemId": "FProjStatus",
                    "name": "FProjStatus",
                    "hidden": true,
                    "maxLength": 10,
                    "langKey": "FProjStatus",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "开始日期",
                    "itemId": "FStartDate",
                    "name": "FStartDate",
                    "hidden": true,
                    "langKey": "FStartDate",
                    "xtype": "ngDate"
                },
                {
                    "fieldLabel": "结束日期",
                    "itemId": "FEndDate",
                    "name": "FEndDate",
                    "hidden": true,
                    "langKey": "FEndDate",
                    "xtype": "ngDate"
                },

                {
                    "fieldLabel": "项目金额",
                    "itemId": "FProjAmount",
                    "name": "FProjAmount",
                    "hidden": true,
                    "maxLength": 18,
                    "langKey": "FProjAmount",
                    "xtype": "ngNumber"
                },
                {
                    "fieldLabel": "预算金额",
                    "itemId": "FBudgetAmount",
                    "name": "FBudgetAmount",
                    "hidden": true,
                    "maxLength": 18,
                    "langKey": "FBudgetAmount",
                    "xtype": "ngNumber"
                },
                {
                    "fieldLabel": "单据类型",
                    "itemId": "FType",
                    "name": "FType",
                    "hidden": true,
                    "langKey": "FType",
                    "valueField": "code",
                    "displayField": "name",
                    "xtype": "ngComboBox",
                    "queryMode": "local",
                    "data": [
                        {}
                    ]
                },
                {
                    "fieldLabel": "审批状态",
                    "itemId": "FApproveStatus",
                    "name": "FApproveStatus",
                    "hidden": true,
                    "langKey": "FApproveStatus",
                    "valueField": "code",
                    "displayField": "name",
                    "xtype": "ngComboBox",
                    "queryMode": "local",
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
                            "name": "纳入预算"
                        }
                    ]
                },
                {
                    "fieldLabel": "结转标志",
                    "itemId": "FCarryOver",
                    "name": "FCarryOver",
                    "hidden": true,
                    "maxLength": 10,
                    "langKey": "FCarryOver",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "审批人",
                    "itemId": "FApprover",
                    "name": "FApprover",
                    "hidden": true,
                    "langKey": "FApprover",
                    "helpid": "fg3_user",
                    "valueField": "PhId",
                    "displayField": "UserName",
                    "userCodeField": "UserNo",
                    "xtype": "ngRichHelp"
                },
                {
                    "fieldLabel": "审批时间",
                    "itemId": "FApproveDate",
                    "name": "FApproveDate",
                    "hidden": true,
                    "langKey": "FApproveDate",
                    "xtype": "ngDate"
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
                            "fieldLabel": "",
                            "name": "NgRecordVer"
                        }
                    ]
                }
            ]
        }
    },
    "grid": {
        "GHSubjectMst": {
            "id": "GHSubjectMst",
            "buskey": "PhId",
            "bindtable": "ys3_SubjectMst",
            "desTitle": "基本支出科目",
            "columns": [
                {
                    "xtype": "rownumberer",
                    "stateId": "lineid",
                    "text": "行号",
                    "width": 50
                },
                {
                    "LangKey": "FSubjectCode",
                    "dataIndex": "FSubjectCode",
                    "width": 150,
                    "readOnly": true,
                    "sortable": false,
                    "header": "科目编码"
                },
                {
                    "LangKey": "FSubjectName",
                    "dataIndex": "FSubjectName",
                    "width": 200,
                    "readOnly": true,
                    "sortable": false,
                    "header": "科目名称"
                },
                {
                    "LangKey": "FProjName",
                    "dataIndex": "FProjName",
                    "width": 200,
                    "readOnly": true,
                    "sortable": false,
                    "header": "子科目名称"
                },
                {
                    "LangKey": "FProjAmount",
                    "dataIndex": "FProjAmount",
                    "width": 200,
                    "readOnly": true,
                    "sortable": false,
                    "header": "金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FAmountEdit",
                    "dataIndex": "FAmountEdit",
                    "width": 200,
                    "readOnly": true,
                    "header": "调整金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FAmountAfterEdit",
                    "dataIndex": "FAmountAfterEdit",
                    "width": 200,
                    "readOnly": true,
                    "header": "调整后金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FFillDept",
                    "dataIndex": "FFillDept_EXName",
                    "width": 200,
                    "header": "填报部门",
                    "readOnly": true,
                    "sortable": false,
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
                    "LangKey": "FYear",
                    "dataIndex": "FYear",
                    "width": 100,
                    "header": "项目年度",
                    "hidden": true
                },
                {
                    "LangKey": "FDeclarationUnit",
                    "dataIndex": "FDeclarationUnit_EXName",
                    "width": 100,
                    "header": "申报单位",
                    "hidden": true,
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
                    "LangKey": "PhId",
                    "dataIndex": "PhId",
                    "width": 100,
                    "header": "主键",
                    "hidden": true
                },
                {
                    "LangKey": "FBudgetDept",
                    "dataIndex": "FBudgetDept",
                    "width": 100,
                    "header": "预算部门",
                    "hidden": true
                    //"editor": {
                    //    "xtype": "ngComboBox",
                    //    "valueField": "code",
                    //    "displayField": "name",
                    //    "QueryMode": "local",
                    //    "valueType": "int"
                    //}
                },
                {
                    "LangKey": "FDateofDeclaration",
                    "dataIndex": "FDateofDeclaration",
                    "width": 100,
                    "header": "申报日期",
                    "hidden": true
                },
                {
                    "LangKey": "FProjCode",
                    "dataIndex": "FProjCode",
                    "width": 100,
                    "header": "项目编码",
                    "hidden": true
                },
                {
                    "LangKey": "FKMLB",
                    "dataIndex": "FKMLB",
                    "width": 100,
                    "header": "科目属性",
                    "hidden": true
                },
                {
                    "LangKey": "Mstphid",
                    "dataIndex": "Mstphid",
                    "width": 100,
                    "header": "主表主键",
                    "hidden": true
                },
                {
                    "LangKey": "FDeclarer",
                    "dataIndex": "FDeclarer",
                    "width": 100,
                    "header": "申报人",
                    "hidden": true
                }
            ]
        },
        "GHSubjectMstBudgetDtl": {
            "id": "GHSubjectMstBudgetDtl",
            "buskey": "PhId",
            "bindtable": "ys3_SubjectMst_BudgetDtl",
            "desTitle": "基本支出科目项目明细",
            "columns": [
                {
                    "xtype": "rownumberer",
                    "stateId": "lineid",
                    "text": "行号",
                    "width": 50
                },
                {
                    "LangKey": "FDtlName",
                    "dataIndex": "FDtlName",
                    "width": 200,
                    "header": "明细项目名称",
                    "editor": {
                        "xtype": "ngText"
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "Fqty",
                    "dataIndex": "Fqty",
                    "width": 100,
                    "header": "数量",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FMeasUnit",
                    "dataIndex": "FMeasUnit",
                    "width": 100,
                    "header": "计量单位",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "FPrice",
                    "dataIndex": "FPrice",
                    "width": 100,
                    "header": "单价",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FAmount",
                    "dataIndex": "FAmount",
                    "width": 200,
                    "header": "金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FAmountEdit",
                    "dataIndex": "FAmountEdit",
                    "width": 200,
                    "header": "调整金额",
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FAmountAfterEdit",
                    "dataIndex": "FAmountAfterEdit",
                    "width": 200,
                    "header": "调整后金额",
                    "readOnly": true,
                    "align": 'right',
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FExpensesChannel",
                    "dataIndex": "FExpensesChannel_EXName",
                    "width": 200,
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
                    },
                    "mustInput": true
                },
                {
                    "LangKey": "FOtherInstructions",
                    "dataIndex": "FOtherInstructions",
                    "width": 300,
                    "header": "测算过程及其他需要说明的事项",
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "PhId",
                    "dataIndex": "PhId",
                    "width": 100,
                    "header": "主键",
                    "hidden": true
                },
                {
                    "LangKey": "Mstphid",
                    "dataIndex": "Mstphid",
                    "width": 100,
                    "header": "主表主键",
                    "hidden": true
                },
                {
                    "LangKey": "FProjCode",
                    "dataIndex": "FProjCode",
                    "width": 100,
                    "header": "项目编码",
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
                    "header": "项目名称",
                    "hidden": true
                },
                {
                    "LangKey": "FBudgetset",
                    "dataIndex": "FBudgetset",
                    "width": 100,
                    "header": "分部门填报",
                    "hidden": true
                }
            ]
        },
        "billList": {
            "id": "billList",
            "buskey": "PhId",
            "bindtable": "ys3_Subject",
            "desTitle": "列表信息",
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
                    "LangKey": "FDeclarationUnit",
                    "dataIndex": "FDeclarationUnit_EXName",
                    "width": 100,
                    "header": "申报单位",
                    "hidden": true,
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
                    "LangKey": "FDeclarationDept",
                    "dataIndex": "FDeclarationDept_EXName",
                    "width": 200,
                    "header": "填报部门",
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
                    "LangKey": "FBudgetDept",
                    "dataIndex": "FBudgetDept_EXName",
                    "width": 100,
                    "header": "预算部门",
                    "hidden": true,
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
                    "LangKey": "FProjCode",
                    "dataIndex": "FProjCode",
                    "width": 100,
                    "hidden": true,
                    "header": "项目代码"
                },
                {
                    "LangKey": "FProjName",
                    "dataIndex": "FProjName",
                    "width": 300,
                    "header": "任务名称"
                },
                {
                    "LangKey": "FType",
                    "dataIndex": "FType",
                    "width": 100,
                    "header": "申报进度"
                },
                {
                    "LangKey": "FYear",
                    "dataIndex": "FYear",
                    "width": 100,
                    "hidden": true,
                    "header": "项目年度"
                },
                {
                    "LangKey": "FProjAmount",
                    "dataIndex": "FProjAmount",
                    "width": 100,
                    "header": "项目金额",
                    "hidden": true,
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "FApproveStatus",
                    "dataIndex": "FApproveStatus",
                    "width": 100,
                    "header": "审批状态"
                    //"editor": {
                    //    "xtype": "ngComboBox",
                    //    "valueField": "code",
                    //    "displayField": "name",
                    //    "QueryMode": "local",
                    //    "valueType": "int"
                    //}
                },
                {
                    "LangKey": "FDeclarer",
                    "dataIndex": "FDeclarer",
                    "width": 200,
                    "header": "填报人"
                },
                {
                    "LangKey": "FDateofDeclaration",
                    "dataIndex": "FDateofDeclaration",
                    "width": 200,
                    "format": "Y-m-d",
                    "header": "申报日期"
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
                    "LangKey": "FApproveDate",
                    "dataIndex": "FApproveDate",
                    "width": 200,
                    "header": "审批时间",
                    "editor": {
                        "xtype": "ngDate"
                    }
                }
            ]
        }
    },
    "fieldSetForm": {},
    "tabPanel": {}
}
