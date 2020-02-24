/**
* 合并Grid的数据列
* @param grid {Ext.Grid.Panel} 需要合并的Grid
* @param colIndexArray {Array} 需要合并列的Index(序号)数组；从0开始计数，序号也包含。
* @param isAllSome {Boolean} 是否2个tr的colIndexArray必须完成一样才能进行合并。true：完成一样；false：不完全一样
*/
function ExMergeGrid(grid, colIndexArray, isAllSome) {
    isAllSome = isAllSome == undefined ? true : isAllSome; // 默认为true

    // 1.是否含有数据
    var gridView = document.getElementById(grid.getView().getId() + '-body');
    if (gridView == null) {
        return;
    }

    // 2.获取Grid的所有tr
    var trArray = [];
    if (grid.layout.type == 'table') { // 若是table部署方式，获取的tr方式如下
        trArray = gridView.childNodes;
    } else {
        trArray = gridView.getElementsByTagName('tr');
    }

    // 3.进行合并操作
    if (isAllSome) { // 3.1 全部列合并：只有相邻tr所指定的td都相同才会进行合并
        var lastTr = trArray[0]; // 指向第一行
        // 1)遍历grid的tr，从第二个数据行开始
        for (var i = 1, trLength = trArray.length; i < trLength; i++) {
            var thisTr = trArray[i];
            var isPass = true; // 是否验证通过
            // 2)遍历需要合并的列
            for (var j = 0, colArrayLength = colIndexArray.length; j < colArrayLength; j++) {
                var colIndex = colIndexArray[j];
                // 3)比较2个td的列是否匹配，若不匹配，就把last指向当前列
                if (lastTr.childNodes[colIndex].innerText != thisTr.childNodes[colIndex].innerText) {
                    lastTr = thisTr;
                    isPass = false;
                    break;
                }
            }

            // 4)若colIndexArray验证通过，就把当前行合并到'合并行'
            if (isPass) {
                for (var j = 0, colArrayLength = colIndexArray.length; j < colArrayLength; j++) {
                    var colIndex = colIndexArray[j];
                    // 5)设置合并行的td rowspan属性
                    if (lastTr.childNodes[colIndex].hasAttribute('rowspan')) {
                        var rowspan = lastTr.childNodes[colIndex].getAttribute('rowspan') - 0;
                        rowspan++;
                        lastTr.childNodes[colIndex].setAttribute('rowspan', rowspan);
                    } else {
                        lastTr.childNodes[colIndex].setAttribute('rowspan', '2');
                    }
                    // lastTr.childNodes[colIndex].style['text-align'] = 'center';; // 水平居中
                    lastTr.childNodes[colIndex].style['vertical-align'] = 'middle';; // 纵向居中
                    thisTr.childNodes[colIndex].style.display = 'none';
                }
            }
        }
    } else { // 3.2 逐个列合并：每个列在前面列合并的前提下可分别合并
        // 1)遍历列的序号数组
        for (var i = 0, colArrayLength = colIndexArray.length; i < colArrayLength; i++) {
            var colIndex = colIndexArray[i];
            var lastTr = trArray[0]; // 合并tr，默认为第一行数据
            // 2)遍历grid的tr，从第二个数据行开始
            for (var j = 1, trLength = trArray.length; j < trLength; j++) {
                var thisTr = trArray[j];
                // 3)2个tr的td内容一样
                if (lastTr.childNodes[colIndex].innerText == thisTr.childNodes[colIndex].innerText) {
                    // 4)若前面的td未合并，后面的td都不进行合并操作
                    if (i > 0 && thisTr.childNodes[colIndexArray[i - 1]].style.display != 'none') {
                        lastTr = thisTr;
                        continue;
                    } else {
                        // 5)符合条件合并td
                        if (lastTr.childNodes[colIndex].hasAttribute('rowspan')) {
                            var rowspan = lastTr.childNodes[colIndex].getAttribute('rowspan') - 0;
                            rowspan++;
                            lastTr.childNodes[colIndex].setAttribute('rowspan', rowspan);
                        } else {
                            lastTr.childNodes[colIndex].setAttribute('rowspan', '2');
                        }
                        // lastTr.childNodes[colIndex].style['text-align'] = 'center';; // 水平居中
                        lastTr.childNodes[colIndex].style['vertical-align'] = 'middle';; // 纵向居中
                        thisTr.childNodes[colIndex].style.display = 'none'; // 当前行隐藏
                    }
                } else {
                    // 5)2个tr的td内容不一样
                    lastTr = thisTr;
                }
            }
        }
    }
}


//还原单元格
function ExsplitGrid(grid, colIndexArray) {
    // 1.是否含有数据
    var gridView = document.getElementById(grid.getView().getId() + '-body');
    if (gridView == null) {
        return;
    }

    // 2.获取Grid的所有tr
    var trArray = [];
    if (grid.layout.type == 'table') { // 若是table部署方式，获取的tr方式如下
        trArray = gridView.childNodes;
    } else {
        trArray = gridView.getElementsByTagName('tr');
    }

    for (var i = 0, trLength = trArray.length; i < trLength; i++) {
        var thisTr = trArray[i];
        for (var j = 0, colArrayLength = colIndexArray.length; j < colArrayLength; j++) {
            var colIndex = colIndexArray[j];
            thisTr.childNodes[colIndex].setAttribute('rowspan', '1');
            if (thisTr.childNodes[colIndex].style.display == 'none') {
                thisTr.childNodes[colIndex].style.display = "";
            }
        }
    }
}