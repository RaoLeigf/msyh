var storeZBLB= Ext.create('Ext.ng.JsonStore', {
	fields: [{

  
    "name": "FCode",
    "type": "string",
    "mapping": "FCode"
  },
  {
    "name": "FName",
    "type": "string",
    "mapping": "FName"
				}],
				url: C_ROOT + 'GQT/QT/PerformEvalTargetClass/GetPerformEvalTargetClassList',
});

var individualConfigInfo = 
{
  "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "Z_QTPerformEvalTarget",
      "desTitle": "mainPanel",
      "columnsPerRow": 1,
      "fields": [
        {
          "fieldLabel": "指标类型代码",
          "itemId": "FTargetTypeCode",
          "name": "FTargetTypeCode",
		  "hidden": true,
          "maxLength": 30,
          "langKey": "FTargetTypeCode",
          "xtype": "ngText"
        },
		{
          "fieldLabel": "分类",
          "itemId": "FTargetTypeName",
          "name": "FTargetTypeName",
          "maxLength": 30,
          "langKey": "FTargetTypeName",
          "xtype": "ngText"
        },
        /*{
          "fieldLabel": "指标类别代码",
          "itemId": "FTargetClassCode",
          "name": "FTargetClassCode",
          "maxLength": 30,
          "langKey": "FTargetClassCode",
          "xtype": "ngText",
		  
        },*/
		{
			"fieldLabel": "类别",
			"itemId": "FTargetClassCode",
			"name": "FTargetClassCode",
			"xtype":"combo",
			//"store":[['no','无加密'],['wep','wep加密']],
			"store":storeZBLB,
			"editable":false,
			"valueField":'FCode',
			"displayField":'FName'
		},
        {
          "fieldLabel": "代码",
          "itemId": "FTargetCode",
          "name": "FTargetCode",
          "maxLength": 30,
          "langKey": "FTargetCode",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "名称",
          "itemId": "FTargetName",
          "name": "FTargetName",
          "maxLength": 255,
          "langKey": "FTargetName",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "值",
          "itemId": "FTargetValue",
          "name": "FTargetValue",
          "maxLength": 100,
          "langKey": "FTargetValue",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "权重",
          "itemId": "FTargetWeight",
          "name": "FTargetWeight",
          "maxLength": 18,
          "langKey": "FTargetWeight",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "描述",
          "itemId": "FTargetDescribe",
          "name": "FTargetDescribe",
          "maxLength": 300,
          "langKey": "FTargetDescribe",
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
    "list1": {
      "id": "list1",
      "buskey": "PhId",
      "bindtable": "Z_QTPerformEvalTarget",
      "desTitle": "list1",
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
          "LangKey": "FTargetClassCode",
          "dataIndex": "FTargetClassCode",
          "width": 100,
          "header": "指标类别代码"
        },
        {
          "LangKey": "FTargetCode",
          "dataIndex": "FTargetCode",
          "width": 100,
          "header": "指标代码"
        },
        {
          "LangKey": "FTargetName",
          "dataIndex": "FTargetName",
          "width": 100,
          "header": "指标名称"
        },
        {
          "LangKey": "FTargetValue",
          "dataIndex": "FTargetValue",
          "width": 100,
          "header": "指标值"
        },
        {
          "LangKey": "FTargetWeight",
          "dataIndex": "FTargetWeight",
          "width": 100,
          "header": "指标权重"
        },
        {
          "LangKey": "FTargetDescribe",
          "dataIndex": "FTargetDescribe",
          "width": 300,
          "header": "指标描述"
        }
      ]
    }
  },
  "fieldSetForm": {},
  "tabPanel": {}
}
