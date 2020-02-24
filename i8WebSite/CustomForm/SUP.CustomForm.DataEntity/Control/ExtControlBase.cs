using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.CustomForm.DataEntity.Control
{
    [Serializable]
    public class ExtControlBase
    {
        private string _id;
                
        private string _xtype;           //ui类型
        private string _cmpName;         //组件名
        private string _name;            //字段名       
        private string _fieldLabel;      //标签名
        private string _text;            //按钮上显示文字
        private bool _visible = true;    //是否显示
        private bool _mustInput = false; //是否必输项        
        private bool _protect = true;    //是否保护
        private int _font = 9;
        private int _align = 0;          //0:左对齐，1:右对齐，2:中间对齐
        private string _fieldStyle;      //字体样式
        private int _colspan = 1;        //跨的列数
        private string _iconCls;         //图标样式id
        private string _value;           //默认值
        private List<String> _expressions;  //表达式 
        private bool isTitle = false;
        private bool _singletext = false;
        private int _labelWidth = 0;     //标签宽度
        private int _maxLength = 0;      //文本输入最大长度
        private string _defaultvalue = string.Empty;
        private string _tag = string.Empty;  //文本框虚拟空值
        private string _format = "yyyy-mm-dd";   //日期掩码
        //private string _editMask = "###,###.000000";  //数值掩码
        private string _labelStyle = string.Empty;    //标签样式


        public ExtControlBase()
        {
 
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        //public string EditMask
        //{
        //    get { return _editMask; }
        //    set { _editMask = value; }
        //}

        public string DefaultValue
        {
            get { return _defaultvalue; }
            set { _defaultvalue = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public int LabelWidth
        {
            get { return _labelWidth; }
            set { _labelWidth = value; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        public bool SingleText
        {
            get { return _singletext; }
            set { _singletext = value; }
        }

        public bool IsTitle
        {
            get { return isTitle; }
            set { isTitle = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string XType
        {
            get { return _xtype; }
            set { _xtype = value; }
        }

        public string CmpName
        {
            get { return _cmpName; }
            set { _cmpName = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FieldLabel
        {
            get { return _fieldLabel; }
            set { _fieldLabel = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool MustInput
        {
            get { return _mustInput; }
            set { _mustInput = value; }
        }

        public bool Protect
        {
            get { return _protect; }
            set { _protect = value; }
        }

        public int Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public int Align
        {
            get { return _align; }
            set { _align = value; }
        }

        public string FieldStyle
        {
            get { return _fieldStyle; }
            set { _fieldStyle = value; }
        }

        public int ColSpan
        {
            get { return _colspan; }
            set { _colspan = value; }
        }

        public string IconCls
        {
            get { return _iconCls; }
            set { _iconCls = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    
        public string LabelStyle
        {
            get { return _labelStyle; }
            set { _labelStyle = value; }
        }

        
        public List<String> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }


        public int XPos { get; set; }
        public int YPos { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

    }


}
