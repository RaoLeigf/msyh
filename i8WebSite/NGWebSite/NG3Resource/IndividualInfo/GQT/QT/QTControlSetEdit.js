var individualConfigInfo = 
{
  "form": {
    "mainform": {
      "id": "mainform",
      "buskey": "PhId",
      "bindtable": "Z_QTControlSet",
      "desTitle": "mainform",
      "columnsPerRow": 1,
      "fields": [
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
          "fieldLabel": "控制对象",
          "itemId": "ControlObject",
          "name": "ControlObject",
          "maxLength": 300,
          "langKey": "ControlObject",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "功能点标志",
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
    "grid": {
      "id": "grid",
      "buskey": "PhId",
      "bindtable": "Z_QTControlSet",
      "desTitle": "grid",
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
          "LangKey": "ControlObject",
          "dataIndex": "ControlObject",
          "width": 500,
          "header": "控制对象"
        },
        {
          "LangKey": "BZ",
          "dataIndex": "BZ",
          "width": 200,
          "header": "功能点标志"
        },
        {
          "LangKey": "ControlOrgName",
          "dataIndex": "ControlOrgName",
          "width": 400,
          "header": "控制组织名称"
        },
        {
          "LangKey": "ControlOrNot",
          "dataIndex": "ControlOrNot",
          "width": 100,
          "header": "是否控制",
            "editor": {
                /*"xtype": "ngComboBox",
                "valueField": "code",
                "displayField": "name",
                "QueryMode": "local",
                "valueType": "int",
                "store": [[1, '是'], [2, '否']]*/
                /*"valueField": "code",
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
                ]*/
                "valueField": "code",
                "displayField": "name",
                "xtype": "ngComboBox",
                "queryMode": "local",
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
                ],
                "editable": false
            },

          },
          {
              "LangKey": "BEGINFDATE",
              "dataIndex": "BEGINFDATE",
              "width": 200,
              "format": "Y-m-d",
              "header": "启用日期",
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
