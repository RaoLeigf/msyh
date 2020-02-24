///工作流标志列专用
Ext.define('Ext.Gc3.WFColumn', {
    extend: 'Ext.grid.column.CheckColumn',
    alias: 'widget.gcWFColumn',
    align: 'center',
    constructor: function () {
        this.addEvents(
            'beforecheckchange'
        );
        this.scope = this;
        this.callParent(arguments);
        this.on('beforecheckchange', function () {
            return false;
        });

    },
    renderer: function (value, meta) {
        var me = this;

        var cssPrefix = Ext.baseCSSPrefix,
            cls = [cssPrefix + 'grid-checkcolumn'];

        if (this.disabled) {
            meta.tdCls += ' ' + this.disabledCls;
        }

        if (value == '2') {
            cls.push(cssPrefix + 'grid-checkcolumn-checked');
            return '<img class="' + cls.join(' ') + '" src="' + Ext.BLANK_IMAGE_URL + '"/>';
        } else if (value == '1') {
            cls.push('icon-partial-select');
            return '<img class="' + cls.join(' ') + '" src="' + Ext.BLANK_IMAGE_URL + '"/>';
        } else {
            return '<img class="' + cls.join(' ') + '" src="' + Ext.BLANK_IMAGE_URL + '"/>';
        }
    },
    printRenderer: function (val) {
        if (val == '2') {
            return '√';
        } else if (val == '1') {
            return '□';
        } else {
            return '';
        }
    }
});