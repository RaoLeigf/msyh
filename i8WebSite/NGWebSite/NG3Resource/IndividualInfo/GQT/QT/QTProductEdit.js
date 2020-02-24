var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTProduct",
      "desTitle": "mainPanel",
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
          "fieldLabel": "产品标识",
          "itemId": "ProductBZ",
          "name": "ProductBZ",
          "maxLength": 20,
          "langKey": "ProductBZ",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "产品名称",
          "itemId": "ProductName",
          "name": "ProductName",
          "maxLength": 200,
          "langKey": "ProductName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "产品URL",
          "itemId": "ProductUrl",
          "name": "ProductUrl",
          "maxLength": 2000,
          "langKey": "ProductUrl",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "数据库类型",
          "itemId": "FSqlType",
          "name": "FSqlType",
          "maxLength": 100,
          "langKey": "FSqlType",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
          "data": [
            {
              "code": "sql",
              "name": "sql"
            },
            {
              "code": "oracle",
              "name": "oracle"
            }
          ],
          "editable":false
        },
        {
          "fieldLabel": "服务",
          "itemId": "FSqlServer",
          "name": "FSqlServer",
          "maxLength": 100,
          "langKey": "FSqlServer",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "账套",
          "itemId": "FSqlSource",
          "name": "FSqlSource",
          "maxLength": 100,
          "langKey": "FSqlSource",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "账号",
          "itemId": "FSqlDataName",
          "name": "FSqlDataName",
          "maxLength": 100,
          "langKey": "FSqlDataName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "密码",
          "itemId": "FSqlDataPwd",
          "name": "FSqlDataPwd",
          "maxLength": 100,
          "langKey": "FSqlDataPwd",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "操作员表名",
          "itemId": "FSqlUserTable",
          "name": "FSqlUserTable",
          "maxLength": 100,
          "langKey": "FSqlUserTable",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "操作员代码列名",
          "itemId": "FSqlUserTableCode",
          "name": "FSqlUserTableCode",
          "maxLength": 100,
          "langKey": "FSqlUserTableCode",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "操作员密码列名",
          "itemId": "FSqlUserTablePwd",
          "name": "FSqlUserTablePwd",
          "maxLength": 100,
          "langKey": "FSqlUserTablePwd",
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
              "fieldLabel": "记录版本",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    }
  },
  "grid": {
    "QTProduct": {
      "id": "QTProduct",
      "buskey": "PhId",
      "bindtable": "Z_QTProduct",
      "desTitle": "QTProduct",
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
          "LangKey": "ProductBZ",
          "dataIndex": "ProductBZ",
          "width": 100,
          "header": "产品标识"
        },
        {
          "LangKey": "ProductName",
          "dataIndex": "ProductName",
          "width": 200,
          "header": "产品名称"
        },
        {
          "LangKey": "ProductUrl",
          "dataIndex": "ProductUrl",
          "width": 500,
          "header": "产品URL"
        },
        {
          "LangKey": "FSqlType",
          "dataIndex": "FSqlType",
          "width": 100,
          "header": "数据库类型"
        },
        {
          "LangKey": "FSqlServer",
          "dataIndex": "FSqlServer",
          "width": 100,
          "header": "服务"
        },
        {
          "LangKey": "FSqlSource",
          "dataIndex": "FSqlSource",
          "width": 100,
          "header": "账套"
        },
        {
          "LangKey": "FSqlDataName",
          "dataIndex": "FSqlDataName",
          "width": 100,
          "header": "账号"
        },
        {
          "LangKey": "FSqlDataPwd",
          "dataIndex": "FSqlDataPwd",
          "width": 100,
          "header": "密码"
        },
        {
          "LangKey": "FSqlUserTable",
          "dataIndex": "FSqlUserTable",
          "width": 100,
          "header": "操作员表名"
        },
        {
          "LangKey": "FSqlUserTableCode",
          "dataIndex": "FSqlUserTableCode",
          "width": 100,
          "header": "操作员代码列名"
        },
        {
          "LangKey": "FSqlUserTablePwd",
          "dataIndex": "FSqlUserTablePwd",
          "width": 100,
          "header": "操作员密码列名"
        },
        {
          "LangKey": "BZ",
          "dataIndex": "BZ",
          "width": 100,
          "header": "备注"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
