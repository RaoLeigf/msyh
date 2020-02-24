Ext.define('Ext.Gc.KingGridTabPanel', {
    id: 'tabPanelKing',
    extend: 'Ext.ng.TabPanel',
    //id: 'tabs',
    activeTab: 0,
    region: 'center',
    autoScroll: true,
    overflowY: 'scroll',
    layout: 'border',
    //defaults: {                   //解决金格控件加载的问题
    //    hideMode: 'visibility'
    //},
    deferredRender: false,        //解决金格控件加载的问题
    listeners: {
        //解决金格控件加载的问题
        beforetabchange: function (tabpanel, nCard, oCard, eOpts) {
            document.body.focus(); //解决金格控件加载的问题
            if (oCard.hideMode == 'visibility') {
                oCard.body.dom.parentNode.style.height = '0px';
            }
            if (nCard.hideMode == 'visibility') {
                nCard.body.dom.parentNode.style.height = '100%';
            }
        },
        beforerender: function (view, opt) {
        }
    }
});