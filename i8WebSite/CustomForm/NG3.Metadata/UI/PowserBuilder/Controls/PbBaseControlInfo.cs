using System.Collections.Generic;
using NG3.Metadata.Core;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB控件基础类
    /// </summary>
    public class PbBaseControlInfo : PbBaseInfo
    { 

        private int _xPos = -1;
        private int _yPos = -1;
        private PbExpressionImp _visibleExpressionImp = new PbExpressionImp(); 
        private long _textColor = 0;
        private PbExpressionImp _textColorExpressionImp = new PbExpressionImp();

        private int _height = -1;
        private int _width = -1;
        private int _maxLength = 0;

        private bool isTitle = false;
        private string subtotal = string.Empty;  //ini中的分组小计串
        private string levelsum = string.Empty;
        private Dictionary<string, string> sumdic = new Dictionary<string, string>();  //合计列
        private Dictionary<string, string> subdic = new Dictionary<string, string>();  //小计列
        private Dictionary<string, string> groupfield = new Dictionary<string, string>();  //分组小计的分组列 
        private Dictionary<string, string> groupcolsdic = new Dictionary<string, string>();  //多表头标签

        private int _span = 1;        
        private int _font = 9;
        private int _align = 0;  //0:左对齐，1:右对齐，2:中间对齐
        private bool _singletext = false;
        private bool _textarea = false;
        private int _colSpan = 1;
        private bool _multiSelect = false;

        private int _labelWidth = 0;
        private string _defaultvalue = string.Empty;
        private string _format = string.Empty;  //日期掩码
        private string _tag = string.Empty;
        private string _editMask = string.Empty;  //数值掩码
        private string _collapse = string.Empty;

        private long _color = 0;//列的字体颜色
        private long _backgroundColor = 0;//列的背景颜色
        private long _labelTextColor = 0;//标签的字体颜色

        //保存所有fieldset的折叠属性
        public string Collapse
        {
            get { return _collapse; }
            set { _collapse = value; }
        }

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

        public string EditMask
        {
            get { return _editMask; }
            set { _editMask = value; }
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public int LabelWidth
        {
            get { return _labelWidth; }
            set { _labelWidth = value; }
        }

        public bool SingleText
        {
            get { return _singletext; }
            set { _singletext = value; }
        }

        public bool TextArea
        {
            get { return _textarea; }
            set { _textarea = value; }
        }

        public int ColSpan
        {
            get { return _colSpan; }
            set { _colSpan = value; }
        }

        public bool MultiSelect
        {
            get { return _multiSelect; }
            set { _multiSelect = value; }
        }

        public string Subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }

        public string LevelSum
        {
            get { return levelsum; }
            set { levelsum = value; }
        }

        public Dictionary<string, string> Sumdic
        {
            get { return sumdic; }
            set { sumdic = value; }
        }

        public Dictionary<string, string> Subdic
        {
            get { return subdic; }
            set { subdic = value; }
        }

        public Dictionary<string, string> Groupfield
        {
            get { return groupfield; }
            set { groupfield = value; }
        }

        public Dictionary<string, string> Groupcolsdic
        {
            get { return groupcolsdic; }
            set { groupcolsdic = value; }
        }

        public bool IsTitle
        {
            get { return isTitle; }
            set { isTitle = value; }
        }

        /// <summary>
        /// 绝对坐标X轴
        /// </summary>
        public int XPos
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        /// <summary>
        /// 绝对坐标y轴
        /// </summary>
        public int YPos
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        /// <summary>
        /// 字体颜色(RGB的数字表示)
        /// </summary>
        public long TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// 可见性的表达式控制(表达式输出1或者0)
        /// </summary>
        public PbExpressionImp VisibleExpressionImp
        {
            get { return _visibleExpressionImp; }
            set { _visibleExpressionImp = value; }
        }

        /// <summary>
        /// 字体颜色的表达式控制(不同的表达式显示不同的字体)
        /// </summary>
        public PbExpressionImp TextColorExpressionImp
        {
            get { return _textColorExpressionImp; }
            set { _textColorExpressionImp = value; }
        }

        public int Span
        {
            get { return _span; }
            set { _span = value; }
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

        public long Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public long backgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public long LabelTextColor
        {
            get { return _labelTextColor; }
            set { _labelTextColor = value; }
        }
    }

}