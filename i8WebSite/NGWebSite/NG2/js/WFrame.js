///<reference path="../vswd-ext.js" />
var WFrame = {};
(function (WF) {
    WF.config = (function () {
        var pagePath = C_ROOT + 'SUP/Frame/',
        imgPath = C_ROOT + 'NG2/img/frame/';
        return {
            pagePath: pagePath,
            //登录处理页面
            loginUrl: pagePath + "Part_Login.aspx",
            logo: imgPath + 'logo.gif',
            headerImg: imgPath + 'header.gif',
            leftWidth: 200,
            rightWidth: 200,
            headHeight: 86
        };
    })();

    //#region HeadToolbar
    WF.HeadToolbar = Ext.extend(Ext.Toolbar, {
        defaults: { 
            width:55
        },

        initComponent: function () {
            this.items = [{
                iconCls: 'icon-Star',
                text: '快捷功能:',
                width:80
            },'->',{
                iconCls: 'icon-ArrowUp',
                text: '隐藏',
                handler: function(){
                    if(this.iconCls=='icon-ArrowDown'){
                        WF.Head.setHeight(WF.config.headHeight);
                         //this.setIconClass('icon-ArrowUp');
                         this.setIconCls('icon-ArrowUp')
                          this.setText("隐藏");
                    }
                    else{
                        WF.Head.setHeight(26);
                        //this.setIconClass('icon-ArrowDown')                        
                        this.setIconCls('icon-ArrowDown');
                        this.setText("显示");
                    }
                    
                    WF.viewport.doLayout();
                }
            },
            //{
            //    iconCls: 'icon-ChartLine',
            //    text: '年度'
            //},
            {
                iconCls: 'icon-Key',
                text: '修改密码',
                 width:80,
                handler: function(){
                    WF.ShowDialog({url:'Part_ChangePW.aspx',width:350,height:188,modal:true});
                }
            },
            //{
            //    iconCls: 'icon-CogEdit',
            //    text: '设置'
            //},{
            //    iconCls: 'icon-ArrowRefresh',
            //    text: '刷新'
            //},
            {
                iconCls: 'icon-Lock',
                text: '锁定',
                handler: function(){
                    WF.Login.open({lock:true});
                }
            },{
                iconCls: 'icon-UserGo',
                text: '注销',
                handler: function(){
                    Ext.Msg.confirm('注销?','确定要注销吗?',function(btn){
                        if(btn=='yes'){
                           location.href = "Login.aspx";
                           Ext.Msg.show({
                               closable:false,
                               modal: true,
                               msg: '正在注销, 请稍等...',
                               width:200
                           });
                        }
                    });
                }
            }];
            WF.HeadToolbar.superclass.initComponent.call(this);
        }        
    });
    //#endregion

    //#region Head
    WF.Head = (function () {
        var logo = new Ext.form.Label({
            width: 320,
            height:60,
            html: '<img src="' + WF.config.logo + '"/>'
        });
        var apps = new Ext.Container({
            flex: 1,
            html: ''
        });
        var himg = new Ext.Container({
            width: 480,
            height:60,
            html: '<img align=right src="' + WF.config.headerImg + '"/>'
        });
        return new Ext.Panel({
            region: 'north',
            layout: 'hbox',
            align: 'stretch',           
            style:'background-color:white',
            border: false,
            height: WF.config.headHeight,
            items: [logo, apps, himg],
            bbar: new WF.HeadToolbar()
        });
    })();
    //#endregion

    //#region Left
    WF.Left = (function () {

        return new Ext.Panel({            
            region: 'west',
            //title:'功能菜单',
            //border:false,
            layout: 'accordion',
            //plugins:VMODE,
            split: true,
            collapsible: true,
            //animCollapse: true,
            //collapseMode: 'mini',
            maxWidth: 400,
            width: WF.config.leftWidth
        });
    })();
    //#endregion

    //#region Center
    WF.Center = new Ext.TabPanel({
        region: 'center',
        activeTab: 0,             
        enableTabScroll: true,
        items: [{
            id: 'home',
            title: "首页",
            closeable:true,
            autoLoad:{
             url:'Index_Main.aspx',
             scope:this,
             scripts:true
            }
        }],
        listeners:{
            tabchange:function(me,tab){     
                             

//                 if(Ext.isIE == false)
//                 {
//                    try{
//                     if(tab.body.dom.firstChild)
//                     {
//                        //alert(tab.body.dom.firstChild.contentWindow.location.reload);
//                        var wind = tab.body.dom.firstChild.contentWindow
//                        wind.location.href =  wind.location.href;
//                        wind.location.reload();
//                     }
//                    }
//                    catch(e){}
//                 }
                               
                if(Ext.isIE6){
                    try{
                        if(tab.body.dom.firstChild)
                            setTimeout(tab.body.dom.firstChild.contentWindow.onresize(),200);
                    }
                    catch(e){}
                }                
            },           
          beforeremove: beforeRemoveTab
        }
    });

    function beforeRemoveTab(tab,panel)
    {
       //debugger;    

        WF.Checker.fireEvent('checkin', WF.Checker);//触发checkin事件

//       Ext.MessageBox.show({
//                          title:"操作确认",
//                          msg:"您确定要关闭<b>[" + panel.title +']</b>么？',
//                          buttons: Ext.MessageBox.YESNO,
//                          icon:Ext.MessageBox.QUESTION,
//                          fn:function(btn,text){
//                              if(btn == "yes")
//                              {                              
//                                   tab.un('beforeremove',beforeRemoveTab);

//                                   WF.Checker.fireEvent('checkin', WF.Checker);//触发checkin事件

//                                   tab.remove(panel);
//                                   tab.addListener('beforeremove',beforeRemoveTab,tab);

//                              }
//                          }
//                         });
                          
      //  return false;      
    };

    function beforeClosePanel(panel)
    {
  
       Ext.MessageBox.show({
                          title:"操作确认",
                          msg:"您确定要关闭<b>" + panel.title +'</b>么？',
                          buttons:Ext.MessageBox.YESNO,
                          icon:Ext.MessageBox.QUESTION,
                          fn:function(btn,text){
                              if(btn == "yes")
                              {
                                  //debugger;                                 
                                 var p = WF.Center.getActiveTab()
                                 WF.Center.un('beforeremove',beforeRemoveTab);

                                  if(panel.body.dom.firstChild.contentWindow.say)
                                 {
                                   panel.body.dom.firstChild.contentWindow.say();
                                 }

                                 WF.Center.remove(p);
                                 WF.Center.addListener('beforeremove',beforeRemoveTab,WF.Center);                      
                              }
                          }
                         }); 
        return false;      
    };


    WF.Center.openTab = function(title,url,params){
        var me = WF.Center;
        if(url){            
            var autoId = '';
            if(params) {
                for(var p in params){
                    var pv = params[p];
                    if(pv != null){
                        autoId+=p+(Ext.isObject(pv)?Ext.urlEncode(pv):pv+'').replace(/\W/g,'').substr(0,30);;
                    }
                }
            }
            autoId = url.replace(/[\/\.]/g, '') + autoId;
            
            var n = me.getComponent(autoId);
            if (!n) { //判断是否已经打开该面板
                
//                debugger;

//                var frame = document.createElement("IFRAME");
//                frame.id = "frame-" + autoId;
//                frame.frameborder = "0";
//                frame.height = "100%";
//                frame.width = "100%";
//                frame.src = $path(url);

//                var panel = new Ext.Panel({
//                    id: autoId,
//                    title: title,
//                    plugins: VMODE,
//                    closable: true,  //通过html载入目标页
//                    contentEl:frame,
//                    listeners:{
//                    //beforeclose:beforeClosePanel
//                    }
//                });

//                n = me.add(panel);

                n = me.add({
                    'id': autoId,
                    'title': title,
                    //plugins: VMODE,
                    closable: true,  //通过html载入目标页                   
                    html: '<iframe scrolling="auto" frameborder="0" width="100%" height="100%" src="' + $path(url) + '"></iframe>'
                 });


            }


            me.setActiveTab(autoId);

            n.body.dom.childNodes[0].contentWindow.lastArguments = params||{};
        }
    }
    //#endregion

    //#region Right
//    WF.Right = new Ext.Panel({
//        region: 'east',
//        split: true,
//        collapsible: true,
//        width: WF.config.rightWidth
//    });
    //#endregion

    //#region Taskbar
    WF.Taskbar = new Ext.panel.Panel({
        id:'Taskbar',
        title: '新中大软件',
        region: 'south',
        enableOverflow: true,
        border: false,
        changtitle:function(titlename)
        {
          this.setTitle(titlename);
        }
    })
    //#endregion

    //#region Login
    WF.Login = new Ext.Window({
        title: '登 录',
        autoScreen: true,
        iconCls: 'icon-Lock',
        //plugins:VMODE,
        closeAction: 'hide',
        initHidden: true,
        closable: false,
        maximizable: false,
        resizable: false,
        modal: true,
        width: 335,
        height: 188,
        layout: 'fit',
        url: WF.config.loginUrl,
        open: function (lastRequestOptions) {
            if (Ext.isEmpty(this.url)) return;           
            //显示窗口
            this.show();
            //this.disable();         
            this.waitIframe(this.url,
	        function () {
	            var dw = this.dw;
	            dw.win = this;
	            //this.buttons[0].setHandler(dw.loginHandler);
                //dw.loginSuccess = (dw.loginSuccess || function(){}).createSequence(WF.Login.loginSuccess,this);

                this.getDockedItems()[1].items.get(0).setHandler(dw.loginHandler);;//ext4
                dw.loginSuccess = Ext.Function.createSequence(dw.loginSuccess,WF.Login.loginSuccess,this);
                
	            this.on('hide', this.hideAction);
	        },
	        function () {
	            if (this.dw) {                
                    this.dw.lastRequestOptions = lastRequestOptions;
                    if(lastRequestOptions && lastRequestOptions.lock) this.dw.lockHandler();
	                this.dealAction();
	            }
	        }
        );
        },
        hideAction: function () {
        
        },
        dealAction: function () {
            //this.enable();
        },
        buttons: [{text: '登录',iconCls: 'icon-LockGo'}]
    });

    WF.Login.loginSuccess = function (json) {
        this.hide();
    }
    //#endregion

    WF.ShowDialog = function(opts){
        return $ShowDialog(opts);
    };
    
    WF.init = function () {        
        WF.viewport = new Ext.Viewport({
            layout: 'border',
            items: [WF.Head, WF.Left, WF.Center,WF.Taskbar]
        });
        window.MainTab = WF.Center;

        WF.Checker = new Ext.ux.Checker();
    }

    WF.active = function () {
        if ($user) {
            //说明已登录
            WF.Login.loginSuccess($user);
            //WF.Login.open();
        }
        else {
            WF.Login.open();
        }
    }

    WF.Checker = {};//new Ext.ux.Checker();

})(WFrame);

