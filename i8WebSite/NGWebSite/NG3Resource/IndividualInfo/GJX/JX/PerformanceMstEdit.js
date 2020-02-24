var FActualAmount='';
var FBalanceAmount='';
var FImplRate='';
var individualConfigInfo = 
{
 "form": {
    "mainPanel": {
      "id": "mainPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceMst",
      "desTitle": "绩效管理",
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
              "ORMMode": false,
              "readOnly": true,
          "xtype": "ngRichHelp"
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
          "ORMMode": false,
            "readOnly": true,
          "xtype": "ngRichHelp"
        },
        {
            "id": "FProjName",
            "fieldLabel": "项目名称",
            "itemId": "FProjName",
            "name": "FProjName",
            "colspan": 2,
            "langKey": "FProjName",
            "helpid": "xm3_xmlist",
            "valueField": "PhId",
            "displayField": "FProjName",
            "userCodeField": "FProjCode",
            "colspan": 2,
            "acceptInput": true,
            "readOnly": true,
            "xtype": "ngRichHelp"
        },
        {
          "fieldLabel": "实施单位",
          "itemId": "FExploitingEntity",
          "name": "FExploitingEntity",
          "maxLength": 200,
          "langKey": "FExploitingEntity",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "负责人",
          "itemId": "FProjectLeader",
          "name": "FProjectLeader",
          "maxLength": 100,
          "langKey": "FProjectLeader",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "联系电话",
          "itemId": "FPhoneNumber",
          "name": "FPhoneNumber",
          "maxLength": 50,
          "langKey": "FPhoneNumber",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "联系地址",
          "itemId": "FContactAddress",
          "name": "FContactAddress",
          "maxLength": 200,
          "langKey": "FContactAddress",
          "xtype": "ngText"
        },
        {
          "fieldLabel": "项目属性",
          "itemId": "FProjAttr",
          "name": "FProjAttr",
          "langKey": "FProjAttr",
          "valueField": "code",
          "displayField": "name",
           "xtype": "ngComboBox",
           "readOnly": true,
          "queryMode": "local",
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
        },
        {
          "fieldLabel": "存续期限",
          "itemId": "FDuration",
          "name": "FDuration",
          "langKey": "FDuration",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
            "queryMode": "local",
            "readOnly": true,
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
          "fieldLabel": "开始日期",
          "itemId": "FStartDate",
          "name": "FStartDate",
            "langKey": "FStartDate",
            "readOnly": true,
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "结束日期",
          "itemId": "FEndDate",
          "name": "FEndDate",
            "langKey": "FEndDate",
            "readOnly": true,
          "xtype": "ngDate"
        },
        {
          "fieldLabel": "项目金额",
          "itemId": "FProjAmount",
          "name": "FProjAmount",
          "maxLength": 18,
            "langKey": "FProjAmount",
            "readOnly": true,
          "xtype": "ngNumber"
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
            "readOnly": true,
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
              "readOnly": true,
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
          "fieldLabel": "评价时间",
          "itemId": "FEvaluationDate",
          "name": "FEvaluationDate",
          "langKey": "FEvaluationDate",
          "xtype": "ngDate"
        },
        {
            "fieldLabel": "自评得分",
            "id": "FEvaluationScore",
            "itemId": "FEvaluationScore",
            "name": "FEvaluationScore",
            "langKey": "FEvaluationScore",
            "xtype": "numberfield",
            "maxValue": 100,
            "minValue": 0,
            "readOnly": true
        },
        {
            "fieldLabel": "评价结果",
            "id": "FEvaluationResult",
          "itemId": "FEvaluationResult",
          "name": "FEvaluationResult",
          "langKey": "FEvaluationResult",
          "valueField": "code",
          "displayField": "name",
          "xtype": "ngComboBox",
          "queryMode": "local",
          "readOnly": true,
          "data": [
            {
              "code": "1",
              "name": "好"
            },
            {
              "code": "2",
              "name": "较好"
            },
            {
              "code": "3",
              "name": "一般"
            },
            {
              "code": "4",
              "name": "差"
            }
          ]
        },
        {
            "fieldLabel": "填录人",
            "itemId": "FInformant",
            "name": "FInformant",
            "id": "FInformant",
            //"mustInput": true,
            "langKey": "FInformant",
            "helpid": "fg3_user",
            "valueField": "PhId",
            "displayField": "UserName",
            "userCodeField": "UserNo", 
            "xtype": "ngRichHelp",
            "readOnly": true
        },
        {
          "fieldLabel": "填录人名称",
          "itemId": "FInformantName",
          "name": "FInformantName",
          "langKey": "FInformantName",
          "xtype": "ngText",
		      //"hidden":true
        },
		{
          "fieldLabel": "指标类型",
          "itemId": "FTargetTypeCode",
          "name": "FTargetTypeCode",
          "maxLength": 50,
          "langKey": "FTargetTypeCode",
          "xtype": "ngText",
		  "hidden":true
        },
		{
          "fieldLabel": "指标类型_名称",
          "itemId": "FTargetTypeCode_EXName",
          "name": "FTargetTypeCode_EXName",
          "maxLength": 50,
          "langKey": "FTargetTypeCode_EXName",
          "xtype": "ngText",
		  "hidden":true
        },
        {
          "fieldLabel": "项目类型",
          "itemId": "FExpenseCategory",
          "name": "FExpenseCategory",
          "langKey": "FExpenseCategory",
          "helpid": "GHExpenseCategory",
          //"mustInput": true,
          "valueField": "dm",
          "displayField": "mc",
          "userCodeField": "dm",
          "ORMMode": false,
          "xtype": "ngRichHelp",
          "readOnly": true
      },
      {
        "fieldLabel": "邮编",
        "itemId": "FPostCode",
        "name": "FPostCode",
        "maxLength": 200,
        "langKey": "FPostCode",
        "xtype": "ngText"
      },
      {
        "fieldLabel": "评价部门",
        "itemId": "FEvaluationDept",
        "name": "FEvaluationDept",
        "langKey": "FEvaluationDept",
        "helpid": "ys_orglist",
        "valueField": "ocode",
        "displayField": "oname",
        "userCodeField": "ocode",
        "ORMMode": false,
          "readOnly": true,
        "xtype": "ngRichHelp"
      },
      {
        "fieldLabel": "单据类型",
        "itemId": "FType",
        "name": "FType",
        "langKey": "FType",
        "valueField": "code",
        "displayField": "name",
        "xtype": "ngComboBox",
        "queryMode": "local",
        "readOnly": true,
        "data": [
          {
            "code": "1",
            "name": "自评"
          },
          {
            "code": "2",
            "name": "抽评"
          }
        ]
      },
      {
        "fieldLabel": "绩效评价类型",
        "itemId": "FPerformType",
        "name": "FPerformType",
        "langKey": "FPerformType",
        "helpid": "GHPerformEvalTargetTypeTree",
        "valueField": "f_code",
        "displayField": "f_name",
        "userCodeField": "f_code",
        "ORMMode": true,
        "xtype": "ngRichHelp"
      },
      {
        "fieldLabel": "抽评得分",
        "id": "FCheckEvaluationScore",
        "itemId": "FCheckEvaluationScore",
        "name": "FCheckEvaluationScore",
        "langKey": "FCheckEvaluationScore",
        "xtype": "numberfield",
        "maxValue": 100,
        "minValue": 0,
        "readOnly": true
      },
      {
          "fieldLabel": "评价结果",
          "id": "FCheckEvaluationResult",
        "itemId": "FCheckEvaluationResult",
        "name": "FCheckEvaluationResult",
        "langKey": "FCheckEvaluationResult",
        "valueField": "code",
        "displayField": "name",
        "xtype": "ngComboBox",
        "queryMode": "local",
        "readOnly": true,
        "data": [
          {
            "code": "1",
            "name": "好"
          },
          {
            "code": "2",
            "name": "较好"
          },
          {
            "code": "3",
            "name": "一般"
          },
          {
            "code": "4",
            "name": "差"
          }
        ]
      },
      {
        "fieldLabel": "第三方评价",
        "itemId": "FThird",
        "name": "FThird",
        "langKey": "FThird",
        "valueField": "code",
        "displayField": "name",
        "xtype": "ngComboBox",
        "queryMode": "local",
        //"readOnly": true,
        "data": [
          {
            "code": "1",
            "name": "是"
          },
          {
            "code": "2",
            "name": "否"
          }
        ]
      },
      {
        "fieldLabel": "审批状态",
        "itemId": "FAuditStatus",
        "name": "FAuditStatus",
        "maxLength": 200,
        "langKey": "FAuditStatus",
        "xtype": "ngText",
        "hidden": true
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
                  "fieldLabel": "预算主表主键",
                  "name": "YSMstPhId"
              },
              {
                  "xtype": "hiddenfield",
                  "fieldLabel": "申报部门",
                  "name": "FDeclarationDept"
              },
            {
              "xtype": "hiddenfield",
              "fieldLabel": "记录版本",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
    },
    "DtlTextContPanel": {
      "id": "DtlTextContPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceDtl_TextCont",
      "desTitle": "说明",
       "columnsPerRow": 1,

      "fields": [
        {
            "itemId": "FOtherInstructions",
            "name": "FOtherInstructions",
            "xtype": "ngTextArea",
            "maxLength": 1000,
            "colspan": 1,
            "anchor":"110%",
            "emptyText": "请输入说明内容，（限500字）。",
            "height": 245
        },
        {
          "itemId": "FConclusion",
          "name": "FConclusion",
          "xtype": "ngTextArea",
          "maxLength": 1000,
          "hidden": true
        },
        {
          "itemId": "FCheckConclusion",
          "name": "FCheckConclusion",
          "xtype": "ngTextArea",
          "maxLength": 1000,
          "hidden": true
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
              "fieldLabel": "版本记录",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
     },
    "DtlEvalForm": {
         "id": "DtlEvalPanel",
         "buskey": "PhId",
         "bindtable": "jx3_PerformanceDtl_Eval",
         "desTitle": "绩效自评",
         "columnsPerRow": 8,
         "fields": [
             {
                 "html": "评价名称",
                 "height": 30,
                 "colspan": 2,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "html": "评价内容",
                 "height": 30,
                 "colspan": 4,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "html": "权重%",
                 "height": 30,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "html": "自评得分",
                 "height": 30,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "html": "项目完成情况",
                 "itemId": "FName",
                 "name": "FName",
                 "height": 70,
                 "colspan": 2,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "ngTextArea",
                 "itemId": "FContent",
                 "name": "FContent",
                 "maxLength": 500,
                 "langKey": "FContent",
                 "emptyText": "（限250字）。",
                 "height": 70,
                 "colspan": 4,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "numberfield",
                 "itemId": "FWeight",
                 "name": "FWeight",
                 "maxValue": 99,
                 "minValue": 0,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "numberfield",
                 "itemId": "FScore",
                 "name": "FScore",
                 "maxValue": 99,
                 "minValue": 0,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "html": "达到的效果",
                 "itemId": "FName",
                 "name": "FName",
                 "height": 70,
                 "colspan": 2,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "ngTextArea",
                 "itemId": "FContent",
                 "name": "FContent",
                 "maxLength": 500,
                 "langKey": "FContent",
                 "emptyText": "（限250字）。",
                 "colspan": 4,
                 "height": 70
             },
             {
                 "xtype": "numberfield",
                 "itemId": "FWeight",
                 "name": "FWeight",
                 "langKey": "FWeight",
                 "maxValue": 99,
                 "minValue": 0
             },
             {
                 "xtype": "numberfield",
                 "itemId": "FScore",
                 "name": "FScore",
                 "maxValue": 99,
                 "minValue": 0
             },
             {
                 "xtype": "container",
                 "colspan": 6,
                 "html": "合计",
                 "height": 30,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "container",
                 "itemId": "FWeight_Total",
                 "name": "FWeight_Total",
                 "html": "0",
                 "height": 30,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
             },
             {
                 "xtype": "container",
                 "itemId": "FScore_Total",
                 "name": "FScore_Total",
                 "html": "0",
                 "height": 30,
                 "style": {
                     "line-height": '30px',
                     "text-align": 'center'
                 }
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
                         "fieldLabel": "记录版本",
                         "name": "NgRecordVer"
                     }
                 ]
             }
         ]
     },
     "DtlConclusionTextContPanel": {
      "id": "DtlConclusionTextContPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceDtl_TextCont",
      "desTitle": "自评结论",
       "columnsPerRow": 1,

      "fields": [
        {
            "itemId": "FConclusion",
            "name": "FConclusion",
            "xtype": "ngTextArea",
            "maxLength": 1000,
            "colspan": 1,
            "anchor":"110%",
            "emptyText": "请输入结论内容，（限500字）。",
            "height": 245
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
              "fieldLabel": "版本记录",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
     },
     "DtlCheckConclusionTextContPanel": {
      "id": "DtlCheckConclusionTextContPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceDtl_TextCont",
      "desTitle": "抽评结论",
       "columnsPerRow": 1,

      "fields": [
        {
            "itemId": "FCheckConclusion",
            "name": "FCheckConclusion",
            "xtype": "ngTextArea",
            "maxLength": 1000,
            "colspan": 1,
            "anchor":"110%",
            "emptyText": "请输入结论内容，（限500字）。",
            "height": 245
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
              "fieldLabel": "版本记录",
              "name": "NgRecordVer"
            }
          ]
        }
      ]
     }
  },
 "grid": {
    "DtlEvalPanel": {
        "id": "DtlEvalGrid",
        "buskey": "PhId",
        "bindtable": "jx3_PerformanceDtl_Eval",
        "desTitle": "绩效自评",
        "columns": [
            {
                "xtype": "rownumberer",
                "stateId": "lineid",
                "text": "行号",
                "width": 50
            },
            {
                "dataIndex": "FName",
                "width": 400,
                "header": "评价名称"
            },
            {
                "dataIndex": "FContent",
                "width": 550,
                "header": "评价内容",
                "editor": {
                    "xtype": "ngTextArea"
                },
                "summaryType": 'count',
                "summaryRenderer": function (value, summaryData, dataIndex) {
                    return Ext.String.format('合计 {0}', ''); 
                }
            },
            {
                "dataIndex": "FWeight",
                "width": 250,
                "header": "权重%",
                "align": "center",
                "editor": {
                    "xtype": "numberfield",
                    "margin":"15 0 0 0",
                    "maxValue": 100,
                    "minValue": 0
                },
                "summaryType": 'sum'
            },
            {
                "dataIndex": "FScore",
                "width": 250,
                "header": "自评得分",
                "align": "center",
                "editor": {
                    "xtype": "numberfield",
                    "margin": "15 0 0 0",
                    "maxValue": 100,
                    "minValue": 0
                },
                "summaryType": function (records) {
                    var i = 0,
                        length = records.length,
                        total = 0,
                        record;

                    for (; i < length; ++i) {
                        record = records[i];
                        total += record.get('FScore');
                    }                  

                    var FEvaluationScore = Ext.getCmp('FEvaluationScore');
                    var FEvaluationResult = Ext.getCmp('FEvaluationResult');

                    FEvaluationScore.setValue(total);

                    /*
                        >=95(含)    好
                        95-85(含)		较好
                        85-60(含)		一般
                        <60			差
                     */

                    if (total >= 95) {
                        FEvaluationResult.setValue("1");
                    } else if (total >= 85 && total < 95) {
                        FEvaluationResult.setValue("2");
                    } else if (total >= 60 && total < 85) {
                        FEvaluationResult.setValue("3");
                    } else if (total < 60) {
                        FEvaluationResult.setValue("4");
                    }
                    return total;
                }
            }
        ]
    },
    "DtlBuDtlPanel": {
      "id": "DtlBuDtlPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceDtl_BuDtl",
      "desTitle": "支出明细",
      "columns": [
        {
              "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
              "width": 50
        },
        {
            "dataIndex": "DelPhid",
            "width": 200,
            "header": "预算主键",
            "hidden": true
        },
        {
            "dataIndex": "FDtlCode",
            "width": 200,
            "header": "支出明细编码",
            "align": "center",
            "hidden": false
        },
        {
            "dataIndex": "FName",
            "width": 400,
            "header": "支出明细内容",
            "hidden": false,
            "align": "center",
            "summaryType": 'count',
            "summaryRenderer": function (value, summaryData, dataIndex) {
                return Ext.String.format('合计 {0}', '');
            }
        },
        {
          "LangKey": "FSourceOfFunds",
          //"mustInput": true,
          "dataIndex": "FSourceOfFunds_EXName",
          "width": 200,
          "header": "资金来源",
          "readOnly": true,
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
            "dataIndex": "FBudgetAmount",
            "width": 200,
            "header": "核定预算数（元）",
            "align": "right",
            "summaryType": 'sum'
        },
        {
            "dataIndex": "FActualAmount",
            "width": 200,
            "header": "实际执行数（元）",
            "mustInput": true,
            "align": "right",
            //"summaryType": 'sum',
            "summaryType": 'count',
            "readOnly": true,
            "editor": {
                "xtype": "ngNumber",
                "listeners": {
                    "change": function (ext, newValue, oldValue) {
                        if (newValue !== oldValue) {
                            //debugger;
                            //var aa = me.DtlBuDtlPanel_store.data.items[0].data;

                            //console.log(newValue);
                        }
                    }
                }
            },
            "summaryRenderer": function (value, summaryData, dataIndex) {
                if(typeof ysmstphid !=='undefined' && FActualAmount==''){
                  Ext.Ajax.request({
                    params: { 'id': ysmstphid, 'tabtype': 'budgetmst' },
                      url: C_ROOT + 'GJX/JX/PerformanceMst/GetBudgetMstInfo',
                      async: false,
                      success: function (response) {
                          var resp = Ext.JSON.decode(response.responseText);
                          if (resp.Status === "success") {
                            FActualAmount=resp.Data.FActualAmount;
                            FBalanceAmount=resp.Data.FBalanceAmount;
                            FImplRate=resp.Data.FImplRate;
                          }
                      }
                  });
                }
                return FActualAmount;
            }
        },
        {
          "dataIndex": "FBalanceAmount",
          "width": 200,
          "header": "结余金额（元）",
          "align": "right",
          "mustInput": true,
          "readOnly": true,
          //"summaryType": 'sum',
          "summaryType": 'count',
          "editor": {
              "xtype": "ngNumber"
          },
          "summaryRenderer": function (value, summaryData, dataIndex) {
            if(typeof ysmstphid !=='undefined' && FBalanceAmount==''){
              Ext.Ajax.request({
                params: { 'id': ysmstphid, 'tabtype': 'budgetmst' },
                  url: C_ROOT + 'GJX/JX/PerformanceMst/GetBudgetMstInfo',
                  async: false,
                  success: function (response) {
                      var resp = Ext.JSON.decode(response.responseText);
                      if (resp.Status === "success") {
                        FActualAmount=resp.Data.FActualAmount;
                        FBalanceAmount=resp.Data.FBalanceAmount;
                        FImplRate=resp.Data.FImplRate;
                      }
                  }
              });
            }
            return FBalanceAmount;
          }
        },
        {
            "dataIndex": "FImplRate",
            "width": 200,
            "header": "执行率%",
			      "align": "right",
            "mustInput": true,
            "readOnly": true,
            "editor": {
                "xtype": "ngNumber"
            },
            /*"summaryType": function (records) {
                var length = records.length,
                    budgettotal = 0,
                    actualtotal = 0,
                    record;


                for (var i = 0; i < length; i++) {
                    record = records[i];
                    budgettotal += record.get('FBudgetAmount');  //核定预算数
                    actualtotal += record.get('FActualAmount');  //实际执行数
                } 

                return ((actualtotal / budgettotal) * 100).toFixed(2);
            },*/
            "summaryType": 'count',
            "summaryRenderer": function (value, summaryData, dataIndex) {
              if(typeof ysmstphid !=='undefined' && FImplRate==''){
                Ext.Ajax.request({
                  params: { 'id': ysmstphid, 'tabtype': 'budgetmst' },
                    url: C_ROOT + 'GJX/JX/PerformanceMst/GetBudgetMstInfo',
                    async: false,
                    success: function (response) {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp.Status === "success") {
                          FActualAmount=resp.Data.FActualAmount;
                          FBalanceAmount=resp.Data.FBalanceAmount;
                          FImplRate=resp.Data.FImplRate;
                        }
                    }
                });
              }
              return FImplRate;
            }

          },
      ]
     },
     "DtlTarImpl": {
         "id": "DtlTarImpl",
         "buskey": "PhId",
         "bindtable": "jx3_PerformanceDtl_TarImpl",
         "desTitle": "绩效目标实现情况",
         "columns": [
             {
                 "LangKey": "PhId",
                 "dataIndex": "PhId",
                 "width": 100,
                 "header": "主键",
                 "hidden": true
             },
             {
                 "LangKey": "XmPhid",
                 "dataIndex": "XmPhid",
                 "width": 100,
                 "header": "项目phid",
                 "hidden": true
             },
             {
                 "LangKey": "FTargetCode",
                 "dataIndex": "FTargetCode",
                 "width": 100,
                 "header": "指标代码",
                 "hidden": true
             },
             {
                 "header": "指标分类",
                 "columns": [
                     {
                         "LangKey": "FTargetTypeCode",
                         "dataIndex": "FTargetTypeCode",
                         "width": 100,
                         "header": "指标类型"
                     },
                     {
                         "LangKey": "FTargetClassCode",
                         "dataIndex": "FTargetClassCode",
                         "width": 100,
                         "header": "指标类别"
                     }
                 ]
             },
             {
                 "LangKey": "FTargetContent",
                 "dataIndex": "FTargetContent",
                 "width": 250,
                 "header": "指标内容",
                 "hidden": true
             },
             {
                 "LangKey": "FTargetName",
                 "dataIndex": "FTargetName",
                 "width": 200,
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
                 "LangKey": "FCompletionValue",
                 "dataIndex": "FCompletionValue",
                 "width": 200,
                 "header": "自评完成值",
                 "editor": {
                     "xtype": "ngTextArea"
                 }
             },
             {
                 "LangKey": "FScore",
                 "dataIndex": "FScore",
                 "width": 100,
                 "header": "自评得分",
                 "editor": {
                     "xtype": "ngNumber",
                     "decimalPrecision": 2
                 }
             },
             {
                  "LangKey": "FCheckCompletionValue",
                  "dataIndex": "FCheckCompletionValue",
                  "width": 200,
                  "header": "抽评完成值",
                  "editor": {
                      "xtype": "ngTextArea"
                  }
              },
              {
                  "LangKey": "FCheckScore",
                  "dataIndex": "FCheckScore",
                  "width": 100,
                  "header": "抽评得分",
                  "editor": {
                      "xtype": "ngNumber",
                      "decimalPrecision": 2
                  }
              },
             {
                 "LangKey": "FTargetDescribe",
                 "dataIndex": "FTargetDescribe",
                 "width": 400,
                 "header": "指标描述"
             }
         ]
     },
    "jxGridPanel": {
      "id": "jxGridPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceMst",
      "desTitle": "绩效列表",
      "columns": [
        {
          "xtype": "rownumberer",
          "stateId": "lineid",
          "text": "行号",
              "width": 50
        },
        {
          "dataIndex": "FProjName",
          "width": 200,
          "header": "项目名称"
        },
        {
          "dataIndex": "FProjAmount",
           "width": 200,
           "align": "right",
           "header": "项目金额",
            "renderer": function (val, cell) {
                var money = Ext.util.Format.usMoney(val);

                return money.replace('$', '￥');
            }
        },
        {
          "dataIndex": "FEvaluationDate",
          "width": 250,
            "header": "评价时间",
            "align": "center"
        },
        {
          "dataIndex": "FEvaluationScoreScore",
          "width": 150,
            "header": "自评得分",
            "align": "right"
        },
        {
          "dataIndex": "FEvaluationResult",
          "width": 200,
            "header": "自评结论",
            "align": "center"
            /*"renderer": function (val, cell) {
                var ret = "";
                if (val == '1') {
                    ret = "好";
                } else if (val == '2') {
                    ret = "较好";
                } else if (val == '3') {
                    ret = "一般";
                } else if (val == '4') {
                    ret = "差";
                }
                return ret;
            }*/
        },
        {
          "dataIndex": "FAuditStatus",
          "width": 100,
            "header": "单据状态",
            "align": "center",
          "renderer": function (val, cell) {
                var ret = "未审核";
                if (val == '2') {
                    ret = "已审核";
                } 
                return ret;
            }
        },
        {
          "dataIndex": "FDeclarationUnit_EXName",
            "width": 250,
            "align": "center",
          "header": "主管单位"
        },
        {
            "dataIndex": "FBudgetDept_EXName",
            "width": 250,
            "align": "center",
          "header": "部门"
        },
        {
          "dataIndex": "FExploitingEntity",
            "width": 250,
            "align": "center",
          "header": "实施单位"
        }
      ]
    },
    "ysGridPanel": {
      "id": "ysGridPanel",
      "buskey": "PhId",
      "bindtable": "ys3_BudgetMst",
      "desTitle": "预算列表",
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
          "width": 200,
          "header": "申报单位"
        },
        {
          "LangKey": "FDeclarationDept",
          "dataIndex": "FDeclarationDept_EXName",
          "width": 200,
          "header": "申报部门"
        },
        {
          "LangKey": "FBudgetDept",
          "dataIndex": "FBudgetDept_EXName",
          "width": 200,
          "header": "预算部门"
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 200,
          "header": "项目名称"
        },
        {
          "LangKey": "FPerformType",
          "dataIndex": "FPerformType_EXName",
          "width": 200,
          "header": "绩效评价类型"
        },
        {
          "dataIndex": "FProjAmount",
          "width": 200,
          "header": "核定预算数(元)",
          "align": "right"
        },
        {
          "dataIndex": "FActualAmount",
          "width": 200,
          "header": "实际执行数(元)",
          "align": "right"
        },
        {
          "dataIndex": "FBalanceAmount",
          "width": 200,
          "header": "结余金额(元)",
          "align": "right"
        },
        {
          "dataIndex": "FImplRate",
          "width": 200,
          "header": "预算执行率(%)",
          "align": "right"
        }
      ]
    },
    "jxselfGridPanel": {
      "id": "jxselfGridPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceMst",
      "desTitle": "绩效自评列表",
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
          "LangKey": "FEvaluationDate",
          "dataIndex": "FEvaluationDate",
          "width": 250,
          "header": "评价时间",
          "align": "center"
        },
        {
          "LangKey": "FEvaluationDept",
          "dataIndex": "FEvaluationDept_EXName",
          "width": 200,
          "header": "评价部门"
        },
        {
          "LangKey": "FInformantName",
          "dataIndex": "FInformantName",
          "width": 200,
          "header": "填录人"
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 200,
          "header": "项目名称"
        },
        {
          "LangKey": "FPerformType",
          "dataIndex": "FPerformType_EXName",
          "width": 200,
          "header": "绩效评价类型"
        },
        {
          "LangKey": "FExploitingEntity",
          "dataIndex": "FExploitingEntity",
          "width": 200,
          "header": "实施单位"
        },
        {
          "dataIndex": "FProjAmount",
          "width": 200,
          "header": "核定预算数(元)",
          "align": "right"
        },
        {
          "LangKey": "FAuditStatus",
          "dataIndex": "FAuditStatus",
          "width": 200,
          "header": "审批状态"
        },
        {
          "dataIndex": "FEvaluationScore",
          "width": 150,
          "header": "自评得分",
          "align": "right"
        },
        {
          "dataIndex": "FEvaluationResult",
          "width": 200,
          "header": "自评结果",
          "align": "center"
        }
      ]
    },
    "jxcheckGridPanel": {
      "id": "jxcheckGridPanel",
      "buskey": "PhId",
      "bindtable": "jx3_PerformanceMst",
      "desTitle": "绩效抽评列表",
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
          "LangKey": "FEvaluationDate",
          "dataIndex": "FEvaluationDate",
          "width": 250,
          "header": "评价时间",
          "align": "center"
        },
        {
          "LangKey": "FEvaluationDept",
          "dataIndex": "FEvaluationDept_EXName",
          "width": 200,
          "header": "评价部门"
        },
        {
          "LangKey": "FInformantName",
          "dataIndex": "FInformantName",
          "width": 200,
          "header": "填录人"
        },
        {
          "LangKey": "FProjName",
          "dataIndex": "FProjName",
          "width": 200,
          "header": "项目名称"
        },
        {
          "LangKey": "FPerformType",
          "dataIndex": "FPerformType_EXName",
          "width": 200,
          "header": "绩效评价类型"
        },
        {
          "LangKey": "FExploitingEntity",
          "dataIndex": "FExploitingEntity",
          "width": 200,
          "header": "实施单位"
        },
        {
          "dataIndex": "FProjAmount",
          "width": 200,
          "header": "核定预算数(元)",
          "align": "right"
        },
        {
          "LangKey": "FAuditStatus",
          "dataIndex": "FAuditStatus",
          "width": 200,
          "header": "审批状态"
        },
        {
          "dataIndex": "FEvaluationScore",
          "width": 150,
          "header": "自评得分",
          "align": "right"
        },
        {
          "dataIndex": "FEvaluationResult",
          "width": 200,
          "header": "自评结果",
          "align": "center"
        },
        {
          "dataIndex": "FCheckEvaluationScore",
          "width": 150,
          "header": "抽评得分",
          "align": "right"
        },
        {
          "dataIndex": "FCheckEvaluationResult",
          "width": 200,
          "header": "抽评结果",
          "align": "center"
        },
      ]
    }
  },
 "fieldSetForm": { },
  "tabPanel": {
    "DtlPanel": {
      "id": "DtlPanel",
      "desTitle": "详细信息",
      "items": [
          {
              "id": "TabPage1",
              "title": "支出明细情况",//绩效目标实现情况
              "langKey": "TabPage1"
          },
          {
              "id": "TabPage2",
              "title": "绩效目标实现情况",
              "langKey": "TabPage2"
          },
          {
              "id": "TabPage3",
              "title": "未完成原因分析及拟采取的措施",
              "langKey": "TabPage3"
          },
          {
            "id": "TabPage4",
            "title": "自评结论",
            "langKey": "TabPage4"
          },
          {
            "id": "TabPage5",
            "title": "抽评结论",
            "langKey": "TabPage5"
          } 
          
          //DtlSelfTextContPanel
      ]
    }
  }
}
