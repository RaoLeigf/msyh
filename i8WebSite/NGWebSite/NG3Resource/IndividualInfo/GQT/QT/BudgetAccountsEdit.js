var individualConfigInfo =
{
    "form": {
        "mainPanel": {
            "id": "mainPanel",
            "buskey": "PhId",
            "bindtable": "Z_QTYSKM",
            "desTitle": "预算科目",
            "columnsPerRow": 2,
            "fields": [
                {
                    "fieldLabel": "科目代码",
                    "itemId": "KMDM",
                    "name": "KMDM",
                    "maxLength": 20,
                    "langKey": "KMDM",
                    "xtype": "ngText",
                    "id": "KMDM"
                },
                {
                    "fieldLabel": "科目名称",
                    "itemId": "KMMC",
                    "name": "KMMC",
                    "maxLength": 100,
                    "langKey": "KMMC",
                    "xtype": "ngText"
                },
                {
                    "fieldLabel": "科目类别",
                    "itemId": "KMLB",
                    "name": "KMLB",
                    "langKey": "KMLB",
                    "valueField": "code",
                    "displayField": "name",
                    "xtype": "ngComboBox",
                    "queryMode": "local",
                    "data": [
                        {
                            "code": "0",
                            "name": "收入"
                        },
                        {
                            "code": "1",
                            "name": "支出"
                        }
                    ]
                },
                {
                    "fieldLabel": "预备费科目",
                    "itemId": "HB",
                    "name": "HB",
                    "maxLength": 100,
                    "inputValue": "1",
                    "langKey": "HB",
                    "xtype": "checkbox"
                },
                {
                    "fieldLabel": "主键",
                    "itemId": "PhId",
                    "name": "PhId",
                    "maxLength": 19,
                    "langKey": "PhId",
                    "xtype": "hiddenfield"
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
        "yskmList": {
            "id": "yskmList",
            "buskey": "PhId",
            "bindtable": "Z_QTYSKM",
            "desTitle": "预算科目列表",
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
                    "width": 200,
                    "header": "主键",
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "KMDM",
                    "dataIndex": "KMDM",
                    "width": 200,
                    "header": "科目代码",
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "KMMC",
                    "dataIndex": "KMMC",
                    "width": 200,
                    "header": "科目名称",
                    "editor": {
                        "xtype": "ngText"
                    }
                },
                {
                    "LangKey": "KMLB",
                    "dataIndex": "KMLB",
                    "width": 200,
                    "header": "科目类别",
                    "editor": {
                        "xtype": "ngComboBox",
                        "valueField": "code",
                        "displayField": "name",
                        "QueryMode": "local",
                        "valueType": "int"
                    }
                },
                {
                    "LangKey": "HB",
                    "dataIndex": "HB",
                    "width": 100,
                    "header": "预备费",
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
