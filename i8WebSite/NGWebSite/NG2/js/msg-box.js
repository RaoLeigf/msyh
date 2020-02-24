var NG2 = {
    Msg: {
       
        /*******************************
        ����Ի���,��ʾ��,��ģ̬,һ��ʱ����Զ���ʧ
        msg:�Ի�����Ϣ��
        hold:�Ի�����ʾ����ʱ��
        ********************************/
        Error: function (msg, hold) {
            NG2.Msg.Show('Error', msg, hold, 'alert-error');
        },

        /*******************************
        ��Ϣ�Ի���,��ʾ��,��ģ̬,һ��ʱ����Զ���ʧ
        msg:�Ի�����Ϣ��
        hold:�Ի�����ʾ����ʱ��
        ********************************/
        Info: function (msg, hold) {
            NG2.Msg.Show('Info', msg, hold, 'alert-information');
        },

        /*******************************
        ����Ի���,��ʾ��,��ģ̬,һ��ʱ����Զ���ʧ
        msg:�Ի�����Ϣ��
        hold:�Ի�����ʾ����ʱ��
        ********************************/
        Warning: function (msg, hold) {
            NG2.Msg.Show('Warning', msg, hold, 'alert-warn');
        },

        /*******************************
        �ɹ��Ի���,��ʾ��,��ģ̬,һ��ʱ����Զ���ʧ
        msg:�Ի�����Ϣ��
        hold:�Ի�����ʾ����ʱ��
        ********************************/
        Success: function (msg, hold) {
            NG2.Msg.Show('Sccess', msg, hold, 'alert-success');
        },

        /*******************************
        ȷ�϶Ի���,��ʾ��,ģ̬,�����Զ���ʧ
        ����ֵ yes/no
        msg:�Ի�����Ϣ��
        ********************************/
        Confirm: function (msg) {
            Ext.MessageBox.confirm('Confirm', msg);
        },

        /*******************************
        ������ĶԻ���,��ʾ��,ģ̬,�����Զ���ʧ
        ����ֵ �����value
        title:������
        msg:��ʾ����Ϣ
        ********************************/
        Prompt: function (title, msg) {
            Ext.MessageBox.prompt(title, msg, true,'alert-information');
        },

        /*******************************
        ����������ĶԻ���,��ʾ��,ģ̬,�����Զ���ʧ
        ����ֵ �����value
        title:������
        msg:��ʾ����Ϣ
        width:��Ϣ����
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
        ����ť�ĶԻ���,��ʾ��,ģ̬,�����Զ���ʧ
        ����ֵ ����İ�ť��Ӧ��ֵ
        title:������
        msg:��ʾ����Ϣ
        buttons:Ҫ��ʾ�İ�ť,��Ext.MessageBox.YESNOCANCEL,
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
        �������Ի���,��ʾ��,ģ̬,������ɺ��Զ���ʧ
        ����ֵ 
        title:������
        msg:��ʾ����Ϣ
        progressText:��������Ҫ��ʾ����Ϣ
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
        �ȴ��Ի���,��ʾ��,ģ̬,�����Զ���ʧ
        ����ֵ ����İ�ť��Ӧ��ֵ
        msg:��ʾ����Ϣ
        progressText:��������Ҫ��ʾ����Ϣ
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
        ��ʾ�Ի���,��ʾ��,��ģ̬,һ��ʱ����Զ���ʧ
        title:�Ի������
        msg:�Ի�����Ϣ��
        hold:�Ի�����ʾ����ʱ��
        icon:Ҫ��ʾ��ͼƬ
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