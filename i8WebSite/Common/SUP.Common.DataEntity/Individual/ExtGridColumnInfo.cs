using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity.Individual
{
    public class ExtGridColumnInfoBase
    {
        private string _header = string.Empty;

        private int _width = 168;
        private bool _sortable = true;
        private bool _mustInput = false; //必输列
        private string _dataIndex = string.Empty;
        private ExtGridColumnEditorBase _editor; //= new ExtGridColumnEditorBase();//,

        public ExtGridColumnInfoBase(string fieldname, string header)
        {
            this._dataIndex = fieldname;
            this._header = header;
            //this._editor.xtype = xtype;
        }

        public string header
        {
            get { return _header; }
            set { _header = value; }
        }

        public int width
        {
            get { return _width; }
            set { _width = value; }
        }

        public bool sortable
        {
            get { return _sortable; }
            set { _sortable = value; }
        }

        public bool mustInput
        {
            get { return _mustInput; }
            set { _mustInput = value; }
        }

        public string dataIndex
        {
            get { return _dataIndex; }
            set { _dataIndex = value; }
        }

        public ExtGridColumnEditorBase editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public string FieldUIId { get; set; }//字段UI的id(表名.字段名),删除字段ui信息需要校验
    }

    //单据列表的帮助列信息,无需editor
    public class ExtGridHelpColumnInfo : ExtGridColumnInfoBase
    {
        public ExtGridHelpColumnInfo(string fieldname, string header)
            : base(fieldname, header)
        {

        }
        
        //帮助列的helpid
        public string helpid { get; set; }

    }

    //单据列表用到的下拉,无需editor
    public class ExtGridComboBoxColumnInfoForList : ExtGridColumnInfoBase
    {       
        public ExtGridComboBoxColumnInfoForList(string fieldname, string header)
            : base(fieldname, header)
        {

        }
        public string renderer
        {
            get;set;
        }

    }

    public class ExtGridComboBoxColumnInfo : ExtGridColumnInfoBase
    {
        
        public ExtGridComboBoxColumnInfo(string fieldname, string header) 
            : base(fieldname, header)
        {

        }
        public string renderer
        {
            get;set;

            //下面，打印有问题
            //get
            //{

            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(" function (val, cell, record, rowIndex, colIndex, gridstore) { ");
            //    sb.Append(" var col = this.columns[colIndex];");
            //    sb.Append(" var store = col.getEditor().getStore();");
            //    sb.Append(" var ret; ");
            //    sb.Append(" var index = store.find('code', val);");
            //    sb.Append(" var record = store.getAt(index);");
            //    sb.Append("  if (record) { ");
            //    sb.Append("     ret = record.data.name;");
            //    sb.Append("  } ");
            //    sb.Append(" return ret; }");

            //    return sb.ToString();
            //}          
        }

    }

    public class ExtGridDateTimeColumnInfo : ExtGridColumnInfoBase
    {
        public ExtGridDateTimeColumnInfo(string fieldname, string header)
            : base(fieldname, header)
        {

        }
        public string renderer
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" function (val) { ");
                sb.Append(" if (val) {");
                sb.Append(" var str = Ext.util.Format.date(val, 'Y-m-d H:i:s');");
                sb.Append(" return str;");
                sb.Append("  } ");
                sb.Append("  else {");
                sb.Append(" return '';");
                sb.Append("  }");              
                sb.Append("} ");
               

                return sb.ToString();
            }
        }

    }

    public class ExtGridDateColumnInfo : ExtGridColumnInfoBase
    {
        public ExtGridDateColumnInfo(string fieldname, string header)
            : base(fieldname, header)
        {

        }
        public string renderer
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" function (val) { ");
                sb.Append(" if (val) {");
                sb.Append(" var str = Ext.util.Format.date(val, 'Y-m-d');");
                sb.Append(" return str;");
                sb.Append("  } ");
                sb.Append("  else {");
                sb.Append(" return '';");
                sb.Append("  } ");
                sb.Append(" } ");
                return sb.ToString();
            }
        }

    }

    public class ExtGridNumberColumnInfo : ExtGridColumnInfoBase
    {
        private int _declen = 2;
        public ExtGridNumberColumnInfo(string fieldname, string header,int declen)
            : base(fieldname, header)
        {
            _declen = declen;
        }
      
        public string xtype
        {
            get { return "numbercolumn"; }
        }

        public string format
        {
            get
            {
                return "0.".PadRight(_declen+2,'0');
            } 
        }

        public string align
        {
            get { return "right"; }
        }        	
    }

    //百分比列
    public class ExtGridPerCentColumnInfo : ExtGridNumberColumnInfo
    {
        private int _declen = 4;
        public ExtGridPerCentColumnInfo(string fieldname, string header, int declen)
            : base(fieldname, header, declen)
        {
            _declen = declen;
        }
        public string renderer
        {
            get
            {
                string decFormat = "0.".PadRight(_declen, '0');//"0.00"
                StringBuilder sb = new StringBuilder();
                sb.Append(" function (val) { ");
                sb.Append(" if (isNaN(val)){");
                sb.Append(" return '';");
                sb.Append(" }");
                sb.Append("  return Ext.util.Format.number(String(val * 100), '" + decFormat + "') + '%'; ");            
                sb.Append(" } ");
                return sb.ToString();
            }
        }
    }
    public class ExtGridColumnEditorBase
    {
        private string _xtype = string.Empty;

        public string xtype
        {
            get { return _xtype; }
            set { _xtype = value; }
        }

        private bool _allowBlank = true;

        public bool allowBlank
        {
            get { return _allowBlank; }
            set { _allowBlank = value; }
        }
    }


    //帮助Editor,包括ngRichHelp和ngComboBox
    public class ExtGridColumnHelpEditor : ExtGridColumnEditorBase
    {         
        public string helpid { get; set; }
        public string valueField{ get; set; }
        public string displayField { get; set; }
        public bool isInGrid { get; set; }
        public bool ORMMode { get; set; }         
        public Listeners listeners { get; set; }

    }

    //ngComboBox帮助Editor,
    public class ExtGridComboBoxEditor : ExtGridColumnEditorBase
    {     
        public string valueField { get; set; }
        public string displayField { get; set; }    
        public string queryMode { get; set; }
        public IList<KeyValueEntity> data { get; set; }
    }

    public class ExtGridNumberEditor : ExtGridColumnEditorBase
    {
        private bool _showPercent = false;
        private double _step = 1;
        public int decimalPrecision { get; set; }
        public bool showPercent
        {
            get { return _showPercent; }
            set { _showPercent = value; }
        }

        public double step
        {
            get
            {
                return _step;
            }

            set
            {
                _step = value;
            }
        }

        public decimal maxValue { get; set; }
    }

    public class Listeners
    {
        public string helpselected { get; set; }
    }

    public class KeyValueEntity
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
