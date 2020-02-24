//统计Panel
Ext.define('Ext.Gc.CountPanel', {
    extend: 'Ext.form.Panel',
    region: 'north',
    autoHeight: true,
    frame: true,
    border: false,
    baseCls: 'my-panel-no-border',
    sumcolsPerRow: 1,
    chkcolsPerRow: 1,
    bodyStyle: 'padding:10px 10px 0px 10px',
    summerItems: '',
    checkItems: '',
    controItems: '',
    fieldDefaults: {
        labelWidth: 100,
        anchor: '100%',
        margin: '0 10 5 0',
        msgTarget: 'side'
    },
    defaults: {
        anchor: '100%'
    },
    initComponent: function () {
        var me = this;

        var items = me.createLayout();
        //if (items.length > 0)
        me.items = items;

        this.callParent(arguments);
    },
    createLayout: function () {
        var me = this;
        var sums = me.summerItems;
        var checks = me.checkItems;
        var contros = me.controItems;

        var arr = new Array();

        if (sums) {
            if (sums.length > 0)
                arr = me.createItems(sums, 'sum', arr);
        }
        if (checks) {
            if (checks.length > 0)
                arr = me.createItems(checks, 'chk', arr);
        }
        if (contros) {
            if (contros.length > 0)
                arr = me.createItems(contros, 'ctr', arr);
        }

        return arr;
    },
    createItems: function (data, tp, array) {
        var me = this;
        var rows, columnWith;
        if (tp == "sum") {
            rows = Math.ceil(data.length / me.sumcolsPerRow); //默认四列-统计项-改为默认一列
            me.columnsPerRow = me.sumcolsPerRow;
        } else {
            rows = Math.ceil(data.length / me.chkcolsPerRow); //默认一列-检查项
            me.columnsPerRow = me.chkcolsPerRow;
        }

        switch (me.columnsPerRow) {
            case 1:
                columnWith = 1;
                break;
            case 2:
                columnWith = 0.5;
                break;
            case 3:
                columnWith = 0.333;
                break;
            case 4:
                columnWith = 0.25;
                break;
            default:
                columnWith = 0.25;
        }


        for (var i = 0; i < rows; i++) {

            var objs = new Object();
            objs.xtype = 'container';
            objs.layout = 'column';
            objs.border = false;
            var arr = new Array();
            var m = 0;
            for (var k = (i == 0 ? i : i * me.columnsPerRow) ; k < data.length; k++) {
                m++;
                if (m > me.columnsPerRow)
                    break;


                var obj = new Object();

                obj.xtype = 'container';
                obj.columnWidth = columnWith;
                obj.layout = 'anchor';
                obj.border = false;

                if (tp != "sum") {
                    obj.margin = '10 0 0 0';
                }

                if (tp == "sum") {

                    obj.items = [
                        {
                            itemId: data[k].CNo,
                            xtype: 'ngNumber',
                            fieldLabel: data[k].CName,
                            //countPhId: data[k].PhId,
                            value: data[k].CValue,
                            align: 'right',
                            readOnly: true
                        }
                    ];

                } else {
                    var text;
                    if (tp == "chk") {
                        switch (data[k].CType) {
                            case "0":
                                text = CommLang.ImpTypeNotConstra || "不约束:";
                                break;
                            case "1":
                                text = CommLang.ImpTypeShow || "提示:";
                                break;
                            case "2":
                                text = CommLang.ImpTypeForbid || "禁止:";
                                break;
                        }
                        text += data[k].CName;

                        obj.items = [
                            {
                                xtype: 'label',
                                text: text
                            }];

                    }
                    else {

                        var text = data[k].CName;
                        if (data[k].CSum)
                            text += data[k].CSum;

                        obj.items = [
                            {
                                xtype: 'ngText',
                                itemId: data[k].CNo,
                                readOnly: true,
                                value: text
                            }];
                    }
                }

                arr.push(obj);
            }

            objs.items = arr;
            array.push(objs);
        }

        return array;
    },
    getCheckData: function () {
        //获取检查项在前台设置过后的数据[{PhId:3,Value:0}]
    }
});