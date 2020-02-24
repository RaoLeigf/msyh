var NG2 = {
    Msg: {
       
        /*******************************
        错误对话框,提示性,非模态,一定时间后自动消失
        msg:对话框消息体
        hold:对话框显示持续时间
        ********************************/
        Error: function (msg, hold) {
            NG2.Msg.Show('Error', msg, hold, 'alert-error');
        },

        /*******************************
        消息对话框,提示性,非模态,一定时间后自动消失
        msg:对话框消息体
        hold:对话框显示持续时间
        ********************************/
        Info: function (msg, hold) {
            NG2.Msg.Show('Info', msg, hold, 'alert-information');
        },

        /*******************************
        警告对话框,提示性,非模态,一定时间后自动消失
        msg:对话框消息体
        hold:对话框显示持续时间
        ********************************/
        Warning: function (msg, hold) {
            NG2.Msg.Show('Warning', msg, hold, 'alert-warn');
        },

        /*******************************
        成功对话框,提示性,非模态,一定时间后自动消失
        msg:对话框消息体
        hold:对话框显示持续时间
        ********************************/
        Success: function (msg, hold) {
            NG2.Msg.Show('Sccess', msg, hold, 'alert-success');
        },

        /*******************************
        确认对话框,提示性,模态,不会自动消失
        返回值 yes/no
        msg:对话框消息体
        ********************************/
        Confirm: function (msg) {
            Ext.MessageBox.confirm('Confirm', msg);
        },

        /*******************************
        带输入的对话框,提示性,模态,不会自动消失
        返回值 输入的value
        title:标题栏
        msg:提示性信息
        ********************************/
        Prompt: function (title, msg) {
            Ext.MessageBox.prompt(title, msg, true,'alert-information');
        },

        /*******************************
        带多行输入的对话框,提示性,模态,不会自动消失
        返回值 输入的value
        title:标题栏
        msg:提示性信息
        width:消息框宽度
        ********************************/
        MultilinePrompt: function (title, msg, width) {
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                width: width,
                buttons: Ext.MessageBox.OKCANCEL,
                multiline: true,
                icon: 'alert-information'
            });
        },

        /*******************************
        带按钮的对话框,提示性,模态,不会自动消失
        返回值 点击的按钮对应的值
        title:标题栏
        msg:提示性信息
        buttons:要显示的按钮,如Ext.MessageBox.YESNOCANCEL,
        ********************************/
        ButtonDialog: function (title, msg, buttons) {
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                buttons: buttons,
                icon: 'alert-information'
            });
        },

        /*******************************
        进度条对话框,提示性,模态,处理完成后自动消失
        返回值 
        title:标题栏
        msg:提示性信息
        progressText:进度条上要显示的信息
        ********************************/
        ProgressDialog: function (title, msg, progressText) {
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                progressText: progressText,
                width: 300,
                progress: true,
                closable: false,
                icon: 'alert-information'
            });

            // this hideous block creates the bogus progress
            var f = function (v) {
                return function () {
                    if (v == 12) {
                        Ext.MessageBox.hide();
                        Ext.example.msg('Done', 'Your fake items were loaded!');
                    } 
                    else {
                        var i = v / 11;
                        Ext.MessageBox.updateProgress(i, Math.round(100 * i) + '% completed');
                    }
                };
            };
            for (var i = 1; i < 13; i++) {
                setTimeout(f(i), i * 500);
            }
        },

        /*******************************
        等待对话框,提示性,模态,不会自动消失
        返回值 点击的按钮对应的值
        msg:提示性信息
        progressText:进度条上要显示的信息
        ********************************/
        WaitDialog: function ( msg, progressText) {
            Ext.MessageBox.show({
                msg: msg,
                progressText: progressText,
                width: 300,
                wait: true,
                waitConfig: { interval: 200 },
                icon: 'alert-information'
            });
        },

        /*******************************
        提示对话框,提示性,非模态,一定时间后自动消失
        title:对话框标题
        msg:对话框消息体
        hold:对话框显示持续时间
        icon:要显示的图片
        ********************************/
        Show: function (title, msg, hold, icon) {
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                width: 300,
                icon: icon,
                modal: false
            });
            NG2.Msg.Hold(hold);
        },
        Hold: function Hold(hold) {
            var h = 5000;
            if (hold != undefined) {
                h = hold;
            }
            setTimeout(function () {
                Ext.MessageBox.hide();
            }, h);
        }
    }
};