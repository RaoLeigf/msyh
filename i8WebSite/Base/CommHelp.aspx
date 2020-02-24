<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommHelp.aspx.cs" Inherits="Base.CommHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <NG2:SUPObject ID="SUPObject0" runat="server" ObjectType="TreeList">
    </NG2:SUPObject>
    <NG2:SUPObject ID="SUPObject1" runat="server" ObjectType="TreeList">
    </NG2:SUPObject>
    <NG2:SUPObject ID="SUPObject2" runat="server" ObjectType="TreeList">
    </NG2:SUPObject>
</body>
</html>
<NG2:SUPConfig ID="SUPConfig1" runat="server">
    <Templates>
    </Templates>
    <Script>
        var AF,AF2,AFTree,wcmp, checkColId = '', tb, pb, showBtn, delBtn, clearBtn, helpid = Ext.getUrlParam('id');
        var isMulti = helpObj.selectMode > 1;
        var codeArr = helpObj.codeField.split(',');
        var nameArr = helpObj.nameField.split(',');
        var vpitems = [
                    { border: false, region: 'center', id: 'center', xtype: 'panel' },
                    { border: false, region: 'south', height: 26, layout: 'fit', html: '<div id="bbar"/>' }
        ];

        if (helpObj.t0 != '') {
            vpitems.push({ border: false, region: 'west', id: 'left', split: true, autoScroll: true, collapseMode: 'mini', width: 200, xtype: 'panel' });
        }

        if (helpObj.t1 != '' || isMulti) {
            tb = new Ext.Toolbar({
                border: false,
                region: 'north',
                height: 26,
                items: ['->']
            });
            vpitems.push(tb);
        }

        if (isMulti) {
            vpitems.push({ border: false, region: 'east',id: 'right', split: true, autoScroll: true, width: 0, xtype: 'panel' });
        }
        
        var vp = new Ext.Viewport({
            layout: 'border',
            plugins: VMODE,
            renderTo: Ext.getBody(),
            items: vpitems
        });

        Ext.getCmp('center').body.dom.appendChild(SUPObject1);
        AF = $AF.create(SUPObject1);

        if (isMulti) {
            Ext.getCmp('right').body.dom.appendChild(SUPObject2);
            AF2 = $AF.create(SUPObject2);
        }

        if (helpObj.t0 != '') {
            Ext.getCmp('left').body.dom.appendChild(SUPObject0);
            AFTree = $AF.create(SUPObject0);
        }

        w.init = function (_wcmp) {
            wcmp = _wcmp;

            wcmp.setTitle(helpObj.title, helpObj.icon);

            wcmp.addButton({ text: '确定', handler: okHandler });
            wcmp.addButton({ text: '关闭', handler: function () {
                wcmp.hide();
            }
            });
            wcmp.doLayout();
        }

        w.dealAction = function () {
            loadData();
        }

        function loadData(params) {
            params = Ext.apply({ id: helpid, custom: AF.getCustom() }, params || {});
            Ext.apply(pb.load.params, params);
            pb.changePage(1);
        }

        function callback(lineid, AF) {
            var code = getCode(lineid, AF);
            var name = getName(lineid, AF);

            wcmp.hide();

            if (Ext.isFunction(wcmp.$params.callback)) {
                var callbackAF = isMulti ? AF2 : AF;
                wcmp.$params.callback.call(callbackAF, code, name, callbackAF);
            }
            else if (wcmp.$params.ctrl) {
                wcmp.$params.ctrl.value(code);
                wcmp.$params.ctrl.text(name);
            }
        }

        function getCode(lineid, AF) {
            var s = '', lineArr = Ext.isArray(lineid) ? lineid : lineid.split(',');
            for (var i = 0; i < lineArr.length; i++) {
                for (var j = 0; j < codeArr.length; j++) {
                    s += AF.GetCellData(lineArr[i], codeArr[j]);
                    if (j < codeArr.length - 1)
                        s += ',';
                }
                if (i < lineArr.length - 1)
                    s += '\\|';
            }
            return s;
        }

        function getName(lineid, AF) {
            var s = '', lineArr = Ext.isArray(lineid) ? lineid : lineid.split(',');
            for (var i = 0; i < lineArr.length; i++) {
                for (var j = 0; j < nameArr.length; j++) {
                    s += AF.GetCellText(lineArr[i], nameArr[j]);
                    if (j < nameArr.length - 1)
                        s += ',';
                }
                if (i < lineArr.length - 1)
                    s += '\\|';
            }
            return s;
        }

        function okHandler() {
            var obj = AF, lineid = AF.getCurrentRow();
            if (isMulti) {
                obj = AF2;
                lineid = AF2.FindAll(1);
            }
            if (lineid != '') {
                callback(lineid, obj);
            }
            else {
                AF.MessageBoxFloat('请选择一条记录!');
            }
        }

        function createMutliSelBtn(tb) {
            showBtn = tb.add({ id: 'showBtn', iconCls: 'icon-Package', enableToggle: true, handler: function () {
                if (this.pressed) {
                    Ext.getCmp('right').setWidth(vp.getWidth() / 2);
                }
                else {
                    Ext.getCmp('right').setWidth(0);
                }
                vp.doLayout();
            }
            });

            delBtn = tb.add({ id: 'delBtn', iconCls: 'icon-PackageDelete', text: '删除', handler: function () {
                if (!showBtn.pressed) {
                    showBtn.pressed = true;
                    showBtn.setButtonClass();
                    Ext.getCmp('right').setWidth(vp.getWidth() / 2);
                    vp.doLayout();
                }
                else {                    
                    var lineid = AF2.GetCurrentRow();
                    if (lineid.length > 0) {
                        var rs = lineid.split(',');
                        for (var i = 0; i < rs.length; i++) {
                            AF.SetCellData(getLineByKey(AF, AF2.GetRowKey(rs[i])), checkColId, 0);
                        }
                        AF2.DeleteCurrentRow();
                        updateShowBtn();
                    }
                }
            }
            });

            clearBtn = tb.add({ id: 'clearBtn', iconCls: 'icon-PackageGreen', text: '清空', handler: function () {
                Ext.Msg.confirm('清空?', '确定要清空所有选择的项吗?', function (btn) {
                    if (btn == 'yes') {
                        for (var i = 0; i < AF.getRows(); i++) {
                            AF.SetCellData(i, checkColId, 0);
                        }
                        AF2.DeleteRows(0, -1);
                        updateShowBtn();
                    }
                });
            }
            });
            tb.doLayout();
        }

        function updateShowBtn() {
            showBtn.setText('显示选择项[已选' + AF2.getRows() + '项]');
        }

        function getLineByKey(o, key) {
            for (var j = 0; j < o.GetRows(); j++) {
                if (key == o.GetRowKey(j)) {
                    return j;
                }
            }
            return -1;
        }

        AllReady = function () {
            AF.Build(helpObj.t2);

            pb = new Ext.ux.SupcanPager({
                renderTo: 'bbar',
                AF: AF,
                region: 'south',
                height: 26,
                sliderWidth: 150,
                load: {
                    url: '@/getList',
                    success: function (res) {
                        if (res.valid) {
                            AF.load(res.text);
                        }
                    }
                },
                autoLoad: false,
                displayInfo: true
            });

            if (helpObj.t0 != '') {
                AFTree.Build(helpObj.t0);
            }

            if (isMulti) {
                AF.setProp('multiRowSelectAble', true);
                AF2.Build(helpObj.t2);
                AF2.SetProp('SeparateBarStyle', false);
                AF2.setProp('multiRowSelectAble', true);
                createMutliSelBtn(tb);
                updateShowBtn();
                for (var i = 0; i < AF.getCols(); i++) {
                    if (AF.GetColProp(i, 'ishide') == '0') {
                        if (AF.GetColProp(i, 'isCheckboxOnly') == '0') {
                            //自动插入一个多选列
                            AF.InsertCol(i, { name: 'checked', isCheckboxOnly: true });
                        }
                        checkColId = AF.GetColName(i);
                        break;
                    }
                }

                pb.load.callback = function () {
                    for (var i = 0; i < AF.GetRows(); i++) {
                        var key = AF.GetRowKey(i);
                        for (var j = 0; j < AF2.GetRows(); j++) {
                            if (key == AF2.GetRowKey(j)) {
                                AF.SetCellData(i, checkColId, '1');
                            }
                        }
                    }
                }

                AF.OnClicked = function (rowid, colid) {
                    if (checkColId != '') {
                        var count = AF.getRows();
                        for (var i = 0; i < count; i++) {
                            var key = AF.GetRowKey(i);
                            if (AF.GetCellData(i, checkColId) == 1) {
                                var json = AF.toJson(AF.Export('asData', 'row=' + i) + "\r\n recordset");
                                if (getLineByKey(AF2, key) == -1) {
                                    AF2.InsertRows(0);
                                    AF2.SetRowCellData(0, json);
                                }
                            }
                            else {
                                for (var j = 0; j < AF2.GetRows(); j++) {
                                    if (key == AF2.GetRowKey(j)) {
                                        AF2.DeleteRows(j, 1);
                                    }
                                }
                            }
                        }
                    }
                    updateShowBtn();
                }
            }
            else {
                AF.setProp('multiRowSelectAble', false);

                AF.OnDblClicked = function (lineid) {
                    callback(lineid, AF);
                }
            }

            if (helpObj.t1 != '') {
                AF.OpenFreeformBar(helpObj.t1);
                tb.insert(0, { id: 'queryBtn', text: '查询', iconCls: 'icon-Magnifier', handler: function () {
                    loadData({ query: AF.getQuery() });
                }
                });
                tb.doLayout();
            }

            w.isReady = true;
        }
    </Script>
</NG2:SUPConfig>
