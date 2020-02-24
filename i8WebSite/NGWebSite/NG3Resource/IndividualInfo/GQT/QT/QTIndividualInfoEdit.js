var individualConfigInfo =
{
    "form": {
        "QTindividualinfoEdit": {
            "id": "QTindividualinfoEdit",
            "buskey": "PhId",
            "bindtable": "Z_QTindividualinfo",
            "desTitle": "edit",
            "columnsPerRow": 1,
            "fields": [
                {
                    "fieldLabel": "主键",
                    "itemId": "PhId",
                    "name": "PhId",
                    "maxLength": 19,
                    "langKey": "PhId",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "自定义表单主键",
                    "itemId": "IndividualinfoPhid",
                    "name": "IndividualinfoPhid",
                    "maxLength": 19,
                    "langKey": "IndividualinfoPhid",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "自定义表单名称",
                    "itemId": "IndividualinfoName",
                    "name": "IndividualinfoName",
                    "maxLength": 200,
                    "langKey": "IndividualinfoName",
                    "xtype": "ngText"
                },
                {
                    "xtype": "container",
                    "name": "hiddenContainer",
                    "hidden": true,
                    "items": [
                        {
                            "xtype": "hiddenfield",
                            "fieldLabel": "备用",
                            "name": "NgRecordVer"
                        }
                    ]
                }
            ]
        }
    },
    "grid": {
        "QTindividualinfolist": {
            "id": "QTindividualinfolist",
            "buskey": "PhId",
            "bindtable": "Z_QTindividualinfo",
            "desTitle": "list",
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
                    "hidden": true,
                    "header": "主键"
                },
                {
                    "LangKey": "IndividualinfoName",
                    "dataIndex": "IndividualinfoName",
                    "width": 100,
                    "hidden": true,
                    "header": "自定义表单名称"
                },
                {
                    "LangKey": "IndividualinfoBustype",
                    "dataIndex": "IndividualinfoBustype",
                    "width": 200,
                    "header": "自定义表单类型",
                    "editor": {
                        "xtype": "ngComboBox",
                        "valueField": "code",
                        "displayField": "name",
                        "QueryMode": "local",
                        "valueType": "int",
                        "store": [["GHBudgetYLX", '预立项'], ["GHBudgetXMLX", '项目立项'], ["GHProjectBegin", '年中调整']/*, ["GHExpenseMst", '项目支出预算审批'], ["GHPerformanceMst", '绩效评价']*/]
                        
                    }
                },
                {
                    "LangKey": "IndividualinfoBustypeName",
                    "dataIndex": "IndividualinfoBustypeName",
                    "width": 200,
                    "header": "自定义表单类型名称",
                    "editor": {
                        "xtype": "ngComboBox",
                        "valueField": "code",
                        "displayField": "name",
                        "QueryMode": "local",
                        "valueType": "int",
                        "store": [["GHBudgetYLX", '预立项'], ["GHBudgetXMLX", '项目立项'], ["GHProjectBegin", '年中调整']/*, ["GHExpenseMst", '项目支出预算审批'], ["GHPerformanceMst", '绩效评价']*/]

                    }
                },
				{
                    "LangKey": "DEFINT1",
                    "dataIndex": "DEFINT1",
                    "width": 100,
                    //"hidden": true,
                    "header": "显示顺序",
                    "editor": {
                        "xtype": "ngNumber",
						"allowDecimals":false
                    }
                },
                {
                    "LangKey": "DEFSTR9",
                    "dataIndex": "DEFSTR9",
                    "width": 100,
                    //"hidden": true,
                    "header": "组织代码"
                },
                {
                    "LangKey": "DEFSTR10",
                    "dataIndex": "DEFSTR10",
                    "width": 100,
                    //"hidden": true,
                    "header": "组织名称"
                },
				{
                    "LangKey": "IndividualinfoAmount1",
                    "dataIndex": "IndividualinfoAmount1",
                    "width": 200,
                    "mustInput": true,
                    "header": "金额控制开始金额",
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "IndividualinfoAmount2",
                    "dataIndex": "IndividualinfoAmount2",
                    "width": 200,
                    "mustInput": true,
                    "header": "金额控制结束金额",
                    "editor": {
                        "xtype": "ngNumber"
                    }
                },
                {
                    "LangKey": "BZ",
                    "dataIndex": "BZ",
                    "width": 400,
                    //"hidden": true,
                    "header": "模板说明",
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "IndividualinfoPhid",
                    "dataIndex": "IndividualinfoPhid_EXName",
                    "width": 200,
                    "hidden": true,
                    "header": "自定义表单名称",
                    "editor": {
                        "helpid": "GHQTindividualinfo",
                        "valueField": "phid",
                        "displayField": "name",
                        "userCodeField": "phid",
                        "ORMMode": false,
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                },
                {
                    "LangKey": "YLXPhid",
                    "dataIndex": "YLXPhid_EXName",
                    "width": 200,
                    "header": "预立项模板名称",
                    "editor": {
                        "helpid": "GHQTindividualinfo",
                        "valueField": "phid",
                        "displayField": "name",
                        "userCodeField": "phid",
                        "ORMMode": false,
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                },
                {
                    "LangKey": "XMLXPhid",
                    "dataIndex": "XMLXPhid_EXName",
                    "width": 200,
                    "header": "项目立项模板名称",
                    "editor": {
                        "helpid": "GHQTindividualinfo",
                        "valueField": "phid",
                        "displayField": "name",
                        "userCodeField": "phid",
                        "ORMMode": false,
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                },
                {
                    "LangKey": "NZTXPhid",
                    "dataIndex": "NZTXPhid_EXName",
                    "width": 200,
                    "header": "年中调整模板名称",
                    "editor": {
                        "helpid": "GHQTindividualinfo",
                        "valueField": "phid",
                        "displayField": "name",
                        "userCodeField": "phid",
                        "ORMMode": false,
                        "isInGrid": true,
                        "helpResizable": true,
                        "xtype": "ngRichHelp"
                    }
                },
                {
                    "LangKey": "DEFSTR4",
                    "dataIndex": "DEFSTR4",
                    "width": 200,
                    "header": "支出预算分类",
                    "editor": {
                        "xtype": "ngComboBox",
                        "valueField": "code",
                        "displayField": "name",
                        "QueryMode": "local",
                        "valueType": "string",
                        "store": [["1", '项目支出'], ["2", '基本支出-公用经费'], ["3", '基本支出-人员经费']]

                    }
                }
                
            ]
        }
    },
    "fieldSetForm": {},
    "tabPanel": {}
}
