﻿var individualConfigInfo = {
    form: {
        dispatchpanel: {
            id: 'dispatchpanel',
            buskey: 'PhId',
            frame: true,
            split: true,
            columnsPerRow: 3,
            minWidth: 400,
            bindtable: 'hr_arc_dispatch',
            desTitle: '主信息',
            autoScroll: true,
            fieldDefaults: {
                labelWidth: 87,
                anchor: '100%',
                margin: '0 10 5 0',
                msgTarget: 'side'
            },
            fields: [
                {
                    xtype: 'ngText',
                    fieldLabel: '标题',
                    name: 'Cname',
                    itemId: 'Cname',
                    langKey: 'Cname',
                    readOnly: false,
                    mustInput: true,
                    colspan: 3
                }, {
                    xtype: 'ngText',
                    fieldLabel: '编号',
                    name: 'Cno',
                    itemId: 'Cno',
                    langKey: 'Cno',
                    readOnly: false,
                    mustInput: true,
                    // hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '发文类型',
                    name: 'DocType',
                    id: 'DocType',
                    langKey: 'DocType',
                    helpid: 'wm3_dispatch_type_cj',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'cname',
                    listFields: 'cno,cname,phid',
                    listHeadTexts: '编码,名称',
                    MaxLength: 100,
                    mustInput: false,
                    showCommonUse: false,
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '组织',
                    langKey: 'Cboo',
                    name: 'Cboo',
                    id: 'Cboo',
                    helpid: 'fg_orglist',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    MaxLength: 100,
                    // mustInput: true,
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngText',
                    fieldLabel: '组织简称',
                    langKey: 'ShortcBoo',
                    name: 'ShortcBoo',
                    itemId: 'ShortcBoo',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngText',
                    fieldLabel: '主题词',
                    langKey: 'Topic',
                    name: 'Topic',
                    itemId: 'Topic',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngText',
                    fieldLabel: '关键字',
                    langKey: 'Subject',
                    name: 'Subject',
                    itemId: 'Subject',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '缓急程度',
                    langKey: 'Emergency',
                    name: 'Emergency',
                    itemId: 'Emergency',
                    helpid: 'emergency_help',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'cname',
                    MaxLength: 100,
                    editable: true,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '密级类型',
                    langKey: 'ArcseCrettype',
                    name: 'ArcseCrettype',
                    itemId: 'ArcseCrettype',
                    helpid: 'arcscrettype_help',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'c_name',
                    MaxLength: 100,
                    editable: true,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '保密期限',
                    langKey: 'ArcseCretterm',
                    name: 'ArcseCretterm',
                    itemId: 'ArcseCretterm',
                    helpid: 'arcscretterm_help',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'c_name',
                    MaxLength: 100,
                    editable: true,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '联合发文组织',
                    langKey: 'UnionBoo',
                    name: 'UnionBoo',
                    itemId: 'UnionBoo',
                    helpid: 'fg_orglist',
                    colspan: 3,
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '主送',
                    langKey: 'MainSend',
                    name: 'MainSend',
                    itemId: 'MainSend',
                    helpid: 'fg_orglist',
                    colspan: 3,
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '抄送',
                    langKey: 'CopySend',
                    name: 'CopySend',
                    itemId: 'CopySend',
                    helpid: 'fg_orglist',
                    colspan: 3,
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '抄报',
                    langKey: 'CopyReport',
                    name: 'CopyReport',
                    itemId: 'CopyReport',
                    helpid: 'fg_orglist',
                    colspan: 3,
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '会签部门',
                    langKey: 'CountersignDept',
                    name: 'CountersignDept',
                    itemId: 'CountersignDept',
                    helpid: 'dept',
                    colspan: 3,
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    editable: false,
                    hidden: true
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '印发组织',
                    langKey: 'StampCboo',
                    name: 'StampCboo',
                    itemId: 'StampCboo',
                    helpid: 'fg_orglist',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'oname',
                    listFields: 'ocode,oname',
                    listHeadTexts: '编码,名称',
                    hidden: true,
                    MaxLength: 100,
                    editable: true
                }, {
                    xtype: 'ngDateTime',
                    fieldLabel: '印发时间',
                    langKey: 'StampDate',
                    name: 'StampDate',
                    itemId: 'StampDate',
                    readOnly: false,
                    hidden: true,
                    mustInput: false
                }, {
                    xtype: 'ngNumber',
                    fieldLabel: '打印份数',
                    langKey: 'PrintNum',
                    name: 'PrintNum',
                    itemId: 'PrintNum',
                    readOnly: false,
                    mustInput: false,
                    minValue: 0,
                    value: 1,
                    hidden: true,
                    allowDecimals: false,//设置整数
                    colspan: 1
                }, {
                    xtype: 'ngNumber',
                    fieldLabel: '页数',
                    langKey: 'PageNum',
                    name: 'PageNum',
                    itemId: 'PageNum',
                    readOnly: false,
                    mustInput: false,
                    minValue: 0,
                    hidden: true,
                    allowDecimals: false,//设置整数
                    colspan: 1
                }, {
                    xtype: 'ngText',
                    fieldLabel: '流程文件',
                    langKey: 'Instance',
                    name: 'Instance',
                    itemId: 'Instance',
                    readOnly: true,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngText',
                    fieldLabel: '受控文件',
                    name: 'ControledFile',
                    langKey: 'ControledFile',
                    itemId: 'ControledFile',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngText',
                    fieldLabel: '发文状态',
                    langKey: 'Cstatus',
                    name: 'CstatusDec',
                    itemId: 'CstatusDec',
                    readOnly: true,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngRichHelp',
                    fieldLabel: '拟稿人',
                    langKey: 'DrawEmp',
                    name: 'DrawEmp',
                    itemId: 'DrawEmp',
                    readOnly: false,
                    mustInput: false,
                    colspan: 1,
                    helpid: 'secuser',
                    ORMMode: false,
                    valueField: 'phid',
                    displayField: 'u_name',
                    hidden: true,
                    editable: true
                }, {
                    xtype: 'ngDateTime',
                    fieldLabel: '拟稿日期',
                    langKey: 'DrawDt',
                    name: 'DrawDt',
                    itemId: 'DrawDt',
                    readOnly: false,
                    hidden: true,
                    mustInput: false
                }, {
                    xtype: 'checkbox',
                    boxLabel: '是否需要盖章',
                    langKey: 'IsNeedSignature',
                    name: 'IsNeedSignature',
                    inputValue: '1',
                    hidden: true,
                    checked: false
                }, {
                    xtype: 'ngText',
                    fieldLabel: '关联单据',
                    langKey: 'RelateUrl',
                    name: 'RelateUrl',
                    itemId: 'RelateUrl',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 1
                }, {
                    xtype: 'ngTextArea',
                    fieldLabel: '备注',
                    langKey: 'Ctext',
                    name: 'Ctext',
                    minHeight: 50,
                    itemId: 'Ctext',
                    readOnly: false,
                    mustInput: false,
                    hidden: true,
                    colspan: 3
                }, {
                    xtype: 'container',
                    hidden: true,
                    items: [
                        {
                            xtype: 'hiddenfield',
                            fieldLabel: '流水号',
                            name: 'Ccode',
                            itemId: 'Ccode'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '会签确认人',
                            name: 'MutilauditDoner',
                            itemId: 'MutilauditDoner'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '会签确认时间',
                            name: 'MutilauditDoneDt',
                            itemId: 'MutilauditDoneDt'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '会签确认标志',
                            name: 'MutilauditDone',
                            itemId: 'MutilauditDone'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '版本信息',
                            name: 'Edition',
                            itemId: 'Edition'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '删除操作',
                            name: 'IsDelete',
                            itemId: 'IsDelete'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '业务编码',
                            name: 'SerialNo',
                            itemId: 'SerialNo'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '转发标志',
                            name: 'TransferFlag',
                            itemId: 'TransferFlag'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '转发文编码',
                            name: 'Transfercode',
                            itemId: 'Transfercode'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '是否包含稿纸',
                            name: 'IsPaper',
                            itemId: 'IsPaper'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '导出标志',
                            name: 'ExportdocFlag',
                            itemId: 'ExportdocFlag'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '封回标志，1封回，2封回已修改',
                            name: 'ForceOut',
                            itemId: 'ForceOut',
                            readOnly: false,
                            mustInput: false,
                            colspan: 1
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '签发人',
                            name: 'IssUer',
                            itemId: 'IssUer'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '签发日期',
                            name: 'IssueDt',
                            itemId: 'IssueDt'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '打印次数',
                            name: 'PrintCount',
                            itemId: 'PrintCount'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '主键',
                            name: 'PhId',
                            itemId: 'PhId'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '附件数',
                            name: 'Attachment',
                            itemId: 'Attachment'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '登记人',
                            name: 'Booker',
                            itemId: 'Booker'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '登记日期',
                            name: 'BookDt',
                            itemId: 'BookDt'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '归档人',
                            name: 'PigeonholeEmp',
                            itemId: 'PigeonholeEmp'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '归档日期',
                            name: 'PigeonholeDt',
                            itemId: 'PigeonholeDt'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '共享标志',
                            name: 'ShareFlg',
                            itemId: 'ShareFlg'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '共享列表',
                            name: 'RightList',
                            itemId: 'RightList'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '印发人员',
                            name: 'stamper',
                            itemId: 'stamper'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: 'stampdt(wu)',
                            name: 'Stampdt',
                            itemId: 'Stampdt'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '是否已归档到posft档案库',
                            name: 'PsoftDossierFlag',
                            itemId: 'PsoftDossierFlag'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '封回人',
                            name: 'FouthuMan',
                            itemId: 'FouthuMan'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '发文类型编码',
                            name: 'DocTypeCno',
                            itemId: 'DocTypeCno'
                        }
                    ]
                }
            ]
        },
        sendobjpanel: {
            id: 'sendobjpanel',
            region: 'north',
            frame: true,
            split: true,
            minWidth: 400,
            buskey: 'PhId',
            bindtable: 'hr_arc_dispatch_obj,hr_arc_dispatch_actorobj',
            desTitle: '发送对象',
            autoScroll: true,
            columnsPerRow: 5,
            fieldDefaults: {
                labelWidth: 87,
                anchor: '100%',
                margin: '0 10 5 0',
                msgTarget: 'side'
            },
            fields: [
                {
                    xtype: 'ngMultiRichHelp',
                    fieldLabel: '常用发送对象',
                    name: 'RevUsualSendObj',
                    itemId: 'RevUsualSendObj',
                    ORMMode: true,
                    helpid: 'sendobj_help',
                    valueField: 'PhId',
                    displayField: 'Cname',
                    readOnly: false,
                    mustInput: false,
                    colspan: 4
                }, {
                    xtype: 'hiddenfield', //无实际意义，占位
                    fieldLabel: '占位',
                    name: 'blank',
                    itemId: 'blank',
                    colspan: 1
                }, {
                    xtype: 'ngTextArea',
                    fieldLabel: '接收人员',
                    itemId: 'EmpName',
                    name: 'EmpName',
                    rowspan: 2,
                    minHeight: 100,
                    colspan: 4,
                    editable: false
                }, {
                    xtype: 'ngTableLayoutForm',
                    region: 'north',
                    frame: true,
                    split: true,
                    name: 'EmpHelpForm',
                    itemId: 'EmpHelpForm',
                    minWidth: 50,
                    padding: '0 0 0 0',
                    autoHeight: false,
                    width: '50px',
                    bodyStyle: {
                        'padding': '0px'
                    },
                    columnsPerRow: 1,
                    fieldDefaults: {
                        labelWidth: 87,
                        anchor: '100%',
                        msgTarget: 'side'
                    },
                    fields: [
                        {
                            xtype: 'button',
                            itemId: 'SelectEmp',
                            text: '选择人员'
                        }, {
                            xtype: 'button',
                            itemId: 'ClearEmp',
                            text: '清除人员'
                        }
                    ]
                }, {
                    xtype: 'ngMultiOrgHelp',
                    fieldLabel: '接收组织',
                    name: 'CbooId',
                    itemId: 'CbooId',
                    colspan: 4
                }, {
                    xtype: 'checkbox',
                    fieldLabel: '包含下级组织',
                    name: 'IncludeDown',
                    itemId: 'IncludeDown',
                    inputValue: '1',
                    colspan: 1
                }, {
                    xtype: 'RoleCbooHelp',
                    fieldLabel: '接收角色',
                    name: 'RoleCbooName',
                    itemId: 'RoleCbooName',
                    readOnly: false,
                    mustInput: false,
                    valueField: 'id',
                    displayField: 'text',
                    colspan: 4
                }, {
                    xtype: 'container',
                    hidden: true,
                    name: 'hiddenContainer',
                    items: [
                        {
                            xtype: 'hiddenfield',
                            fieldLabel: '角色组织编号',
                            name: 'RoleCbooId',
                            itemId: 'RoleCbooId'
                        }, {
                            xtype: 'hiddenfield',
                            fieldLabel: '主键',
                            name: 'PhId',
                            itemId: 'PhId'
                        },
                        {
                            xtype: 'hiddenfield',
                            fieldLabel: '人员编码',
                            name: 'EmpId',
                            itemId: 'EmpId'
                        }
                    ]
                }
            ]
        }
    },
    grid: {
        gridinfo: {
            id: 'gridinfo',
            region: 'center',
            buskey: 'PhId',
            frame: true,
            desTitle: '发文列表',
            langKey: 'indetail',
            bindtable: 'hr_arc_dispatch',
            columnLines: true,
            stateful: true,
            stateId: 'wm3_dispatch_stateid',
            columns: [{ xtype: 'rownumberer', header: '行号', langKey: 'RowNumber', width: 50 },
            {
                header: '主键',
                dataIndex: 'PhId',
                langKey: 'PhId',
                width: 100,
                sortable: true,
                hidden: true,
                hideable: false
            }, {
                header: '编号',
                dataIndex: 'Cno',
                langKey: 'Cno',
                width: 200,
                sortable: true,
                hidden: false
            }, {
                header: '标题',
                dataIndex: 'Cname',
                langKey: 'Cname',
                width: 300,
                sortable: true,
                hidden: false
            }, {
                header: '关键字',
                dataIndex: 'Subject',
                langKey: 'Subject',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '发文类型',
                dataIndex: 'DocTypeDec',
                langKey: 'DocType',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '已归档',
                dataIndex: 'PsoftDossierFlag',
                langKey: 'PsoftDossierFlag',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '已导出',
                dataIndex: 'ExportdocFlag',
                langKey: 'ExportdocFlag',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '发文状态',
                langKey: 'Cstatus',
                dataIndex: 'CstatusDec',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '流程状态',
                dataIndex: 'flow_status_name',
                langKey: 'flow_status_name',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '待办节点',
                dataIndex: 'curnodes',
                langKey: 'curnodes',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '待办人',
                dataIndex: 'pendingusers',
                langKey: 'pendingusers',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '工作流名称',
                langKey: 'pd_name',
                dataIndex: 'pd_name',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '阅读情况',
                dataIndex: 'ViewQk',
                langKey: 'ViewQk',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '附件',
                dataIndex: 'Attachment',
                langKey: 'Attachment',
                width: 100,
                sortable: true,
                align: 'center',
                hidden: true
            }, {
                header: '发文组织',
                dataIndex: 'CbooDec',
                langKey: 'Cboo',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '拟稿日期',
                dataIndex: 'DrawDt',
                langKey: 'DrawDt',
                width: 200,
                sortable: true,
                hidden: false
            }, {
                header: '拟稿人',
                dataIndex: 'DrawEmpDec',
                langKey: 'DrawEmp',
                width: 200,
                sortable: true,
                hidden: false
            }, {
                header: '缓急程度',
                dataIndex: 'EmergencyDec',
                langKey: 'Emergency',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '分发人',
                dataIndex: 'BookerDec',
                langKey: 'Booker',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '分发日期',
                dataIndex: 'BookDt',
                langKey: 'BookDt',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '归档人',
                dataIndex: 'PigeonholeEmpDec',
                langKey: 'PigeonholeEmp',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '归档日期',
                dataIndex: 'PigeonholeDt',
                langKey: 'PigeonholeDt',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '密级类型',
                dataIndex: 'ArcseCrettypeDec',
                langKey: 'ArcseCrettype',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '保密期限',
                dataIndex: 'ArcseCrettermDec',
                langKey: 'ArcseCretterm',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '受控文件',
                dataIndex: 'ControledFile',
                langKey: 'ControledFile',
                width: 100,
                sortable: true,
                hidden: true
            }, {
                header: '打印次数',
                dataIndex: 'PrintCount',
                langKey: 'PrintCount',
                width: 100,
                sortable: true,
                hidden: true
            }
            ]
        }
    }

}