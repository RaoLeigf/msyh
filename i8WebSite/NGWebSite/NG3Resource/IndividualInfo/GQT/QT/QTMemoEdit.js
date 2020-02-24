var individualConfigInfo = 
{
  "form": {
    "form": {
      "id": "form",
      "buskey": "PhId",
      "bindtable": "Z_QTMemo",
      "desTitle": "form",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "主键",
          "itemId": "PhId",
          "name": "PhId",
          "maxLength": 18,
          "langKey": "PhId",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "状态",
          "itemId": "MenoStatus",
          "name": "MenoStatus",
          "maxLength": 10,
          "langKey": "MenoStatus",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "标题",
          "itemId": "MenoName",
          "name": "MenoName",
          "maxLength": 200,
          "langKey": "MenoName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "提醒方式",
          "itemId": "MenoRemind",
          "name": "MenoRemind",
          "maxLength": 10,
          "langKey": "MenoRemind",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "金格主键",
          "itemId": "WordPhid",
          "name": "WordPhid",
          "maxLength": 18,
          "langKey": "WordPhid",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "备注",
          "itemId": "BZ",
          "name": "BZ",
          "maxLength": 200,
          "langKey": "BZ",
          "xtype": "ngText"
        },
        {
          "xtype": "container",
          "name": "hiddenContainer",
          "hidden": true,
          "items": [
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
    "Grid": {
      "id": "Grid",
      "buskey": "PhId",
      "bindtable": "Z_QTMemo",
      "desTitle": "Grid",
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
		  "hidden":true
        },
        {
          "LangKey": "MenoStatus",
          "dataIndex": "MenoStatus",
          "width": 100,
          "header": "状态",
		  "editor": {
                "valueField": "code",
                "displayField": "name",
                "xtype": "ngComboBox",
                "queryMode": "local",
                "data": [
                    {
                        "code": '0',
                        "name": "未处置"
                    },
                    {
                        "code": '1',
                        "name": "已处置"
                    }
                ],
                "editable": false
            },
        },
        {
          "LangKey": "MenoName",
          "dataIndex": "MenoName",
          "width": 500,
          "header": "待办事项标题",
		  "editor": {
                "xtype": "ngText"
            }
        },
        {
          "LangKey": "MenoRemind",
          "dataIndex": "MenoRemind",
          "width": 650,
          "header": "提醒方式",
		  "editor": {
                "valueField": "code",
                "displayField": "name",
                "xtype": "ngComboBox",//ngComboBox
                "queryMode": "local",
				"selectMode":'Multi',//设置为多选
                "data": [
                    {
                        "code": '项目送审时',
                        "name": "项目送审时"
                    },
                    {
                        "code": '项目审批时',
                        "name": "项目审批时"
                    },
					{
                        "code": '年中调整送审时',
                        "name": "年中调整送审时"
                    },
					{
                        "code": '年中调整审批时',
                        "name": "年中调整审批时"
                    }
                ],
                "editable": false
            },
        },
        {
          "LangKey": "WordPhid",
          "dataIndex": "WordPhid",
          "width": 100,
          "header": "金格主键",
		  "hidden":true
        },
        {
          "LangKey": "BZ",
          "dataIndex": "BZ",
          "width": 400,
          "header": "备注",
		  "editor": {
                "xtype": "ngText"
            }
        },
        {
          "LangKey": "Creator",
          "dataIndex": "Creator",
          "width": 100,
          "header": "操作员phid",
		  "hidden":true
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
