Ext.define('Ext.Gc.StarRating', {
    alias: "widget.StarRating",
    extend: 'Ext.Component',
    afterRender: function () {
        this.callParent();
    },
    width: 360,
    height: 30,
    padding: 0,
    readOnly: false,
    beanTotal: 0,//总金豆数
    avgScore: 0,//平均分
    hideBean:false,//隐藏金豆
    style: {
        color: '#000000',
        backgroundColor: '#FFFFFF'
    },
    initComponent: function () {
        var me = this;
        var date = new Date();
        var dateTime = date.getTime();
        var starId = me.id ? "star_" + me.id : "star" + dateTime;

        var beanTotal = me.beanTotal;
        var avgScore = me.avgScore;//平均分
        var avgStar = Math.round(avgScore);//平均得星（整数）
        var avgScoreShow = "";
        if (me.readOnly) {
            avgScoreShow = "平均分：" + avgScore;
        }

        var label = me.label ? me.label : '评分';
        var labelWidth = me.labelWidth ? me.labelWidth - 13 : 87;
        var labelAlign = me.lableAlign ? me.lableAlign : 'right';
        var thisWidth = me.width ? me.width : 130;
        var aMsg = [
			"较差|表现较差",
			"一般|表现一般",
			"中等|表现中等",
			"良好|表现良好",
			"优秀|表现优秀"
        ];
        if (me.readOnly) {
            var str = "平均得分：" + avgScore;
            switch (avgStar) {
                case 1:
                    str += "|表现较差";
                    break;
                case 2:
                    str += "|表现一般";
                    break;
                case 3:
                    str += "|表现中等";
                    break;
                case 4:
                    str += "|表现良好";
                    break;
                case 5:
                    str += "|表现优秀";
                    break;
                case 0:
                    str += "|暂无评价";
                    break;
            }
            aMsg = [
			    str,
			    str,
			    str,
			    str,
			    str
            ];
        }
        
        if (me.aMsg) {
            
        } else {
            me.aMsg = aMsg;
        }

        var html = "";
        if (me.hideBean) {
            html = '<div id="' + starId + '" class="star" style="width:' + thisWidth + 'px;">'
		    + '<span style="width:' + labelWidth + 'px;text-align:' + labelAlign + '" >' + label + '</span>'
		    + '<span>:</span>'
	        + '<ul>'
	        + '<li><a href="javascript:;">1</a></li>'
	        + '<li><a href="javascript:;">2</a></li>'
	        + '<li><a href="javascript:;">3</a></li>'
	        + '<li><a href="javascript:;">4</a></li>'
	        + '<li><a href="javascript:;">5</a></li>'
	        + '</ul>'
            + '<span>' + avgScoreShow + '&nbsp</span>'
	        + '<span></span>'
            + '<span></span>'
	        + '<p></p>'
		    + '</div>';
        } else {
            html = '<div id="' + starId + '" class="star" style="width:' + thisWidth + 'px;">'
		    + '<span style="width:' + labelWidth + 'px;text-align:' + labelAlign + '" >' + label + '</span>'
		    + '<span>:</span>'
	        + '<ul>'
	        + '<li><a href="javascript:;">1</a></li>'
	        + '<li><a href="javascript:;">2</a></li>'
	        + '<li><a href="javascript:;">3</a></li>'
	        + '<li><a href="javascript:;">4</a></li>'
	        + '<li><a href="javascript:;">5</a></li>'
	        + '</ul>'
            + '<span>' + avgScoreShow + '&nbsp</span>'
	        + '<span><img src="/i8/NG3Resource/js/PMS/PMS/YG/YgCraftInfo/img/bean.png" height="20" width="20" class="bean_img" /></span>'
            + '<span>' + beanTotal + '</span>'
	        + '<p></p>'
		    + '</div>';
        }
        
        this.html = html;
        this.listeners = {
            'boxready': function () {
                var oStar = document.getElementById(starId);
                var aLi = oStar.getElementsByTagName("li");
                var oUl = oStar.getElementsByTagName("ul")[0];
                var oSpan = oStar.getElementsByTagName("span")[1];
                var oP = oStar.getElementsByTagName("p")[0];
                var i = iScore = me.iStar = 0;
                var oAvg = oStar.getElementsByTagName("span")[2];
                var oBean = oStar.getElementsByTagName("span")[4];
                
                for (i = 1; i <= aLi.length; i++) {
                    aLi[i - 1].index = i;
                    //鼠标移过显示分数
                    aLi[i - 1].onmouseover = function () {
                        fnPoint(this.index);
                        //浮动层显示
                        oP.style.display = "block";
                        //计算浮动层位置
                        oP.style.left = oUl.offsetLeft + this.index * this.offsetWidth - 50 + "px";
                        //匹配浮动层文字内容
                        //oP.innerHTML = "<em><b>" + this.index + "</b> " + aMsg[this.index - 1].match(/(.+)\|/)[1] + "</em>" + aMsg[this.index - 1].match(/\|(.+)/)[1]
                        oP.innerHTML = "<em><b></b> " + me.aMsg[this.index - 1].match(/(.+)\|/)[1] + "</em>" + me.aMsg[this.index - 1].match(/\|(.+)/)[1]
                    };
                    //鼠标离开后恢复上次评分
                    aLi[i - 1].onmouseout = function () {
                        fnPoint();
                        //关闭浮动层
                        oP.style.display = "none"
                    };
                    //点击后进行评分处理
                    aLi[i - 1].onclick = function () {
                        if (me.readOnly)
                            return;

                        me.iStar = this.index;
                        oP.style.display = "none";
                        //oSpan.innerHTML = "<strong>" + (this.index) + " 分</strong> (" + aMsg[this.index - 1].match(/\|(.+)/)[1] + ")"
                        me.value = this.index;
                        oBean.textContent = this.index;
                    }
                }
                //评分处理
                function fnPoint(iArg) {
                    //分数赋值
                    iScore = iArg || me.iStar;
                    for (i = 0; i < aLi.length; i++) aLi[i].className = i < iScore ? "on" : "";
                }

                var setValue = function (score) {
                    me.iStar = Math.round(parseFloat(score));
                    fnPoint(me.iStar);
                    me.value = me.iStar;

                    if (me.readOnly) {
                        var str = "平均得分：" + score;
                        switch (me.iStar) {
                            case 1:
                                str += "|表现较差";
                                break;
                            case 2:
                                str += "|表现一般";
                                break;
                            case 3:
                                str += "|表现中等";
                                break;
                            case 4:
                                str += "|表现良好";
                                break;
                            case 5:
                                str += "|表现优秀";
                                break;
                            case 0:
                                str += "|暂无评价";
                                break;
                        }
                        me.aMsg = [
                            str,
                            str,
                            str,
                            str,
                            str
                        ];
                    }
                }

                var setAvgScoreShow = function (avgScore) {
                    oAvg.textContent = "平均分：" + avgScore;
                }

                var setBeanCount = function (count) {
                    if (me.readOnly)
                        oBean.textContent = parseFloat(oBean.textContent) + parseFloat(count);
                    else
                        oBean.textContent = parseFloat(count);
                }
                me.value = 0;
                me.setValue = setValue;
                me.setBeanCount = setBeanCount;
                me.setAvgScoreShow = setAvgScoreShow;

                if (me.readOnly) {
                    setValue(avgScore);
                }
            }
        }

        this.callParent();
    }
});