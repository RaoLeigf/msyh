var individualConfigInfo = 
{
  "form": {
    "editmemo": {
      "id": "editmemo",
      "buskey": "phid",
      "bindtable": "z_qteditmemo",
      "desTitle": "批注记录edit",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "批注单据主键",
          "itemId": "Memophid",
          "name": "Memophid",
          "maxLength": 19,
          "langKey": "Memophid",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "用户编码",
          "itemId": "UserCode",
          "name": "UserCode",
          "maxLength": 255,
          "langKey": "UserCode",
          "xtype": "ngText"
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
    "billList": {
      "id": "billList",
      "buskey": "PhId",
      "bindtable": "z_qteditmemo",
      "desTitle": "批注记录list",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
          "width": 35
        },
        {
          "LangKey": "Memophid",
          "dataIndex": "Memophid",
          "width": 100,
          "header": "批注单据主键"
        },
        {
          "LangKey": "UserCode",
          "dataIndex": "UserCode",
          "width": 100,
          "header": "用户编码"
        },
        {
          "LangKey": "UserName",
          "dataIndex": "UserName",
          "width": 100,
          "header": "用户名称"
        },
        {
          "LangKey": "MemoTime",
          "dataIndex": "MemoTime",
          "width": 100,
          "header": "批注时间",
          "editor": {
            "xtype": "ngDate"
          }
        },
        {
          "LangKey": "IP",
          "dataIndex": "IP",
          "width": 100,
          "header": "用户ip"
        },
        {
          "LangKey": "MemoCode",
          "dataIndex": "MemoCode",
          "width": 100,
          "header": "批注字段代码",
          "hidden": true
        },
        {
          "LangKey": "MenoName",
          "dataIndex": "MenoName",
          "width": 100,
          "header": "批注字段名称"
        },
        {
          "LangKey": "MemoArea",
          "dataIndex": "MemoArea",
          "width": 100,
          "header": "批注区域"
        },
        {
          "LangKey": "BeforeCode",
          "dataIndex": "BeforeCode",
          "width": 100,
          "header": "批注前代码",
          "hidden": true
        },
        {
          "LangKey": "BeforeName",
          "dataIndex": "BeforeName",
          "width": 100,
          "header": "批注前名称"
        },
        {
          "LangKey": "AfterCode",
          "dataIndex": "AfterCode",
          "width": 100,
          "header": "批注后代码",
          "hidden": true
        },
        {
          "LangKey": "AfterName",
          "dataIndex": "AfterName",
          "width": 100,
          "header": "批注后名称"
        },
        {
          "LangKey": "IfChoose",
          "dataIndex": "IfChoose",
          "width": 100,
          "header": "是否引用"
        },
        {
          "LangKey": "FProjCode",
          "dataIndex": "FProjCode",
          "width": 100,
          "header": "项目代码",
          "hidden": true
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 100,
          "header": "项目名称"
        },
        {
          "LangKey": "TabName",
          "dataIndex": "TabName",
          "width": 100,
          "header": "NGColumn16",
          "hidden": true
        },
        {
          "LangKey": "DEFSTR1",
          "dataIndex": "DEFSTR1",
          "width": 100,
          "header": "NGColumn17",
          "hidden": true
        },
        {
          "LangKey": "DEFSTR2",
          "dataIndex": "DEFSTR2",
          "width": 100,
          "header": "NGColumn18",
          "hidden": true
        },
        {
          "LangKey": "DEFSTR3",
          "dataIndex": "DEFSTR3",
          "width": 100,
          "header": "NGColumn19",
          "hidden": true
        },
        {
          "LangKey": "DEFINT1",
          "dataIndex": "DEFINT1",
          "width": 100,
          "header": "NGColumn20",
          "hidden": true
        },
        {
          "LangKey": "DEFINT2",
          "dataIndex": "DEFINT2",
          "width": 100,
          "header": "NGColumn21",
          "hidden": true
        },
        {
          "LangKey": "DEFNUM1",
          "dataIndex": "DEFNUM1",
          "width": 100,
          "header": "NGColumn22",
          "hidden": true
        },
        {
          "LangKey": "DEFNUM2",
          "dataIndex": "DEFNUM2",
          "width": 100,
          "header": "NGColumn23",
          "hidden": true
        },
        {
          "LangKey": "DEFDate1",
          "dataIndex": "DEFDate1",
          "width": 100,
          "header": "NGColumn24",
          "hidden": true
        },
        {
          "LangKey": "DEFDate2",
          "dataIndex": "DEFDate2",
          "width": 100,
          "header": "NGColumn25"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
