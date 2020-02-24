
////价税合计编辑列表
Ext.define('Ext.Pmm.TaxAmountEditGrid', {
    extend: 'Ext.Gc.EditGrid',

    qtyCol: null,////数量列名称
    taxrateCol: null,////税率列名称
    taxCol: null,////税额列名称
    taxprcCol: null,////含税单价列名称（可以为空）
    untaxprcCol: null,////不含税单价列名称
    taxamtCol: null,////价税合计列名称
    untaxamtCol: null,////不含税金额列名称（外币）
    untaxamtdcCol: null,////本币不含税金额列名称
    PhidCurrType: null,////币种主键列（可以为空）
    ExchRate: null,////汇率列名称（可以为空）

    ////可以都为空
    UmConvCol: null,////换算分子列
    UmConvmCol: null,////换算分母列
    FzMsAmountCol: null,////辅助数量列
    FzMsPriceCol: null,////辅助单价列

    allowNegative: false,////是否允许录入负数

    initComponent: function () {
        var me = this;

        if (!me.features) {
            ////增加合计
            me.features = {
                ftype: 'summary',
                dock: 'bottom',
                showSummaryRow: true
            };
        }

        for (var i = 0; i < me.columns.length; i++) {
            var column = me.columns[i];

            if (i === 1) {
                column.summaryType = 'count';
                column.summaryRenderer = function (value, summaryData, dataIndex) {
                    return (Lang.TotalAmount || "合计") + "：";
                }
            }

            if (column.dataIndex === me.qtyCol
                || column.dataIndex === me.taxprcCol
                || column.dataIndex === me.untaxprcCol) {
                column.xtype = 'numbercolumn';
                column.format = '0.000000';
                column.align = 'right';

                if (column.editable !== false) {
                    column.editor = {
                        xtype: 'ngNumber',
                        decimalPrecision: 6,
                        maxValue: 1000000000, ////最大10亿
                        minValue: -1000000000
                    };
                }
                if (column.dataIndex === me.qtyCol) {
                    column.summaryType = 'sum';
                    column.ngFormat = 'fQty';
                }
                if (column.dataIndex === me.taxprcCol
                    || column.dataIndex === me.untaxprcCol) {
                    column.ngFormat = 'fPrc';
                }
            }

            if (column.dataIndex === me.taxCol
                || column.dataIndex === me.taxamtCol
                || column.dataIndex === me.untaxamtCol) {
                column.xtype = 'numbercolumn';
                column.format = '0.000000';
                column.ngFormat = 'fAmt';
                column.align = 'right';

                if (column.editable !== false) {
                    column.editor = {
                        xtype: 'ngNumber',
                        decimalPrecision: 6,
                        maxValue: 1000000000, ////最大10亿
                        minValue: -1000000000
                    };
                }
                column.summaryType = 'sum';
            }

            if (column.dataIndex === me.taxrateCol) {
                column.align = 'right';
                if (column.editable !== false) {
                    column.editor = {
                        xtype: 'ngRateNumber'
                    }
                };
                column.renderer = function (val, cell) {
                    return Ext.util.Format.number(val * 100, "0.00") + "%";
                }
            }

            if (column.dataIndex === me.ExchRate) {
                column.xtype = 'numbercolumn';
                column.format = '0.0000';
                column.align = 'right';
                if (column.editable !== false) {
                    column.editor = {
                        xtype: 'ngNumber',
                        decimalPrecision: 4
                    };
                }
            }
        }

        this.callParent();

        if (me.allowNegative === false) {
            ////数量金额字段不允许录入负数
            me.on('validateedit', function (editor, e, eOpts) {
                if (e.field == me.qtyCol || e.field == me.taxrateCol
                    || e.field == me.taxCol || e.field == me.taxprcCol || e.field == me.untaxprcCol
                    || e.field == me.taxamtCol || e.field == me.untaxamtCol) {
                    if (e.value < 0) {
                        e.cancel = true;
                    }
                }
            });
        }

        me.on('edit', function (editor, e, eOpts) {
            ////价税合计计算
            me.countTaxAmount(e.record, e.field);

            if (!Ext.isEmpty(me.UmConvCol) && !Ext.isEmpty(me.UmConvmCol)) {
                ////辅助单价及辅助数量的的相关计算
                var umConv = e.record.get(me.UmConvCol);
                var UmConvm = e.record.get(me.UmConvmCol);
                //确定换算因子比例
                var ratio = 0;
                if (umConv !== 0 && UmConvm !== 0) {
                    ratio = umConv / UmConvm;
                } else if (umConv !== 0) {
                    ratio = umConv;
                }
                if (ratio > 0) {
                    me.countFzAmountAndPrc(e.record, e.field, ratio);
                }
            }
        });



        //if (me.lockable) {
        //    if (me.lockedGrid.plugins) {
        //        for (var j = 0; j < me.lockedGrid.plugins.length; j++) {
        //            var plugin = me.lockedGrid.plugins[j];
        //            if (plugin.$className === "Ext.grid.plugin.CellEditing") {
        //                plugin.on('edit', function(editor, e, eOpts) {
        //                    me.countTaxAmount(e.record, e.field);
        //                });
        //            }
        //        }
        //    }

        //    if (me.normalGrid.plugins) {
        //        for (var j = 0; j < me.normalGrid.plugins.length; j++) {
        //            var plugin = me.normalGrid.plugins[j];
        //            if (plugin.$className === "Ext.grid.plugin.CellEditing") {
        //                plugin.on('edit', function(editor, e, eOpts) {
        //                    me.countTaxAmount(e.record, e.field);
        //                });
        //            }
        //        }
        //    }

        //} else {
        //    if (me.plugins) {
        //        for (var j = 0; j < me.plugins.length; j++) {
        //            var plugin = me.plugins[j];
        //            if (plugin.$className === "Ext.grid.plugin.CellEditing") {
        //                plugin.on('edit', function(editor, e, eOpts) {
        //                    me.countTaxAmount(e.record, e.field);
        //                });
        //            }
        //        }
        //    }
        //}
    },
    countTaxAmount: function (record, changeCol) {
        /////计算价税合计方法

        var me = this;

        var countTaxAmount = function (record, changeCol, qtyCol, taxrateCol, taxCol,
    taxprcCol, untaxprcCol, taxamtCol, untaxamtCol, ExchRate, untaxamtdcCol) {
            //var qtyCol = "";////数量列
            //var taxrateCol = ""; ////税率列
            //var taxCol = ""; ////税额列

            //var taxprcCol = ""; ////含税单价列（可以为空）
            //var untaxprcCol = ""; ////不含税单价列
            //var taxamtCol = ""; ////价税合计列
            //var untaxamtCol = ""; ////不含税金额列


            //var changeCol = "";////改变列

            ////由不含税金额计算税额、价税合计
            var countTaxamtFc = function (record) {
                var taxrate = record.get(taxrateCol);
                if (!Ext.isEmpty(taxprcCol)) {
                    record.set(taxprcCol, record.get(untaxprcCol) * (1 + taxrate));
                }
                record.set(taxamtCol, record.get(untaxamtCol) * (1 + taxrate));
                record.set(taxCol, record.get(taxamtCol) - record.get(untaxamtCol));
            };

            if (changeCol === qtyCol) {
                ////数量
                ////由不含税单价、数量计算金额、本币金额
                ////由不含税金额计算税额、价税合计
                var prc = record.get(untaxprcCol);
                var qty = record.get(qtyCol);
                record.set(untaxamtCol, prc * qty);
                if (!Ext.isEmpty(ExchRate)) {
                    var exchrate = record.get(ExchRate);
                    record.set(untaxamtdcCol, prc * qty * exchrate)
                } else {
                    record.set(untaxamtdcCol, prc * qty)
                }

                countTaxamtFc(record);
            } else if (changeCol === taxrateCol) {
                /////税率
                ////由金额计算税额、价税合计
                countTaxamtFc(record);
            } else if (changeCol === taxprcCol) {
                ////含税单价
                ////由含税单价计算不含税单价

                var taxrate = record.get(taxrateCol);
                var prc = record.get(taxprcCol) / (1 + taxrate);
                record.set(untaxprcCol, prc);

                ////由不含税单价、数量计算金额、本币金额
                var qty = record.get(qtyCol);
                record.set(untaxamtCol, prc * qty);
                if (!Ext.isEmpty(ExchRate)) {
                    var exchrate = record.get(ExchRate);
                    record.set(untaxamtdcCol, prc * qty * exchrate)
                } else {
                    record.set(untaxamtdcCol, prc * qty)
                }

                ////由不含税金额计算税额、价税合计
                countTaxamtFc(record);
            } else if (changeCol === untaxprcCol) {
                ////不含税单价
                ////由数量计算金额、本币金额
                ////由金额计算税额、价税合计
                var qty = record.get(qtyCol);
                var prc = record.get(untaxprcCol);
                record.set(untaxamtCol, prc * qty);
                if (!Ext.isEmpty(ExchRate)) {
                    var exchrate = record.get(ExchRate);
                    record.set(untaxamtdcCol, prc * qty * exchrate)
                } else {
                    record.set(untaxamtdcCol, prc * qty)
                }
                countTaxamtFc(record);
            } else if (changeCol === taxamtCol) {
                ////价税合计
                ////计算含税单价

                var qty = record.get(qtyCol);
                if (qty > 0) {
                    if (!Ext.isEmpty(taxprcCol)) {
                        record.set(taxprcCol, record.get(taxamtCol) / qty);
                    }
                }

                var taxrate = record.get(taxrateCol);


                ////由价税合计算不含税金额金额
                var untaxamt = record.get(taxamtCol) / (1 + taxrate);
                record.set(untaxamtCol, untaxamt);

                ////由金额计算本币金额
                if (!Ext.isEmpty(ExchRate)) {
                    var exchrate = record.get(ExchRate);
                    record.set(untaxamtdcCol, untaxamt * exchrate)
                } else {
                    record.set(untaxamtdcCol, untaxamt)
                }

                ////由不含税金额计算不含税单价
                if (qty > 0) {
                    record.set(untaxprcCol, record.get(untaxamtCol) / qty);
                }

                ////计算税额
                record.set(taxCol, record.get(taxamtCol) - record.get(untaxamtCol));

            } else if (changeCol === untaxamtCol) {
                ////金额
                ////计算不含税单价
                ////计算本币金额
                ////由金额计算税额、价税合计
                var qty = record.get(qtyCol);
                if (qty > 0) {
                    record.set(untaxprcCol, record.get(untaxamtCol) / qty);
                }
                if (!Ext.isEmpty(ExchRate)) {
                    record.set(untaxamtdcCol, record.get(untaxamtCol) * record.get(ExchRate));
                } else {
                    record.set(untaxamtdcCol, record.get(untaxamtCol));
                }
                countTaxamtFc(record);
            } else if (changeCol === ExchRate) {
                record.set(untaxamtdcCol, record.get(untaxamtCol) * record.get(ExchRate));
            }

        }

        countTaxAmount(record, changeCol, me.qtyCol, me.taxrateCol, me.taxCol,
            me.taxprcCol, me.untaxprcCol, me.taxamtCol, me.untaxamtCol, me.ExchRate, me.untaxamtdcCol);
    },

    countFzAmountAndPrc: function (record, changeCol, ratio) {
        var me = this;

        var countFzAmountAndPrc = function (record, changeCol, FzMsAmountCol, FzMsPriceCol, qtyCol,
            taxprcCol, untaxprcCol, untaxamtCol, taxamtCol) {

            //根据数量列设置辅助数量值
            if (changeCol === qtyCol) {
                if (!Ext.isEmpty(FzMsAmountCol)) {
                    var qty = record.get(qtyCol);
                    record.set(FzMsAmountCol, qty * ratio);
                }
            }
            //根据单价列设置辅助单价值
            if (changeCol === untaxprcCol) {
                if (!Ext.isEmpty(FzMsPriceCol)) {
                    var untaxprc = record.get(untaxprcCol);
                    record.set(FzMsPriceCol, untaxprc / ratio);
                }
            }
            //根据辅助数量列设置数量值
            if (changeCol === FzMsAmountCol) {
                var fzMsAmount = record.get(FzMsAmountCol);
                record.set(qtyCol, fzMsAmount / ratio);
                changeCol = qtyCol;
                me.countTaxAmount(record, changeCol);
            }
            //根据辅助单价列设置单价值
            if (changeCol === FzMsPriceCol) {
                var fzMsPrice = record.get(FzMsPriceCol);
                record.set(untaxprcCol, fzMsPrice * ratio);
                changeCol = untaxprcCol;
                me.countTaxAmount(record, changeCol);
            }

            if (changeCol === taxprcCol) {
                if (!Ext.isEmpty(FzMsPriceCol)) {
                    var taxrate = record.get(me.taxrateCol);
                    var untaxprc = record.get(taxprcCol) / (1 + taxrate);
                    record.set(FzMsPriceCol, untaxprc / ratio);
                }
            }

            //金额列变化，若单价改变，重设辅助单价值
            if (changeCol === untaxamtCol) {
                if (!Ext.isEmpty(FzMsPriceCol)) {
                    untaxprc = record.get(untaxprcCol);
                    record.set(FzMsPriceCol, untaxprc / ratio);
                }
            }

            //价税合计列变化，若单价改变，重设辅助单价值
            if (changeCol === taxamtCol) {
                if (!Ext.isEmpty(FzMsPriceCol)) {
                    untaxprc = record.get(untaxprcCol);
                    record.set(FzMsPriceCol, untaxprc / ratio);
                }
            }

        }

        countFzAmountAndPrc(record, changeCol, me.FzMsAmountCol, me.FzMsPriceCol, me.qtyCol,
            me.taxprcCol, me.untaxprcCol, me.untaxamtCol, me.taxamtCol);
    }
});