using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB带字段绑定的控件基础类
    /// </summary>
    public class PbBaseTextInfo : PbBaseControlInfo
    {
        private string _leftText = string.Empty;
        private string _rightText = string.Empty;

        private PbColumnInfo _pbColumnInfo = new PbColumnInfo();

        private bool _isInGrid = false;
        private bool _isReadOnly = false;
        private PbExpressionImp _isReadOnlyExpressionImp = new PbExpressionImp();

        private bool _isMustInput = false;
        private PbExpressionImp _isMustInputExpressionImp = new PbExpressionImp();

        private bool _isProtect = false;

        private string _helpDataSource = string.Empty;
        private bool _isHelpDsMultiSelect = false;
        

        private PbBuildInImp _clickEvent = new PbBuildInImp();
        private PbBuildInImp _doubleClickEvent = new PbBuildInImp();
        private PbEvent<PbExpressionImp> _updateEvent = new PbEvent<PbExpressionImp>();
        //private string _summarytype = string.Empty;

        //public string SummaryType
        //{
        //    get { return _summarytype; }
        //    set { _summarytype = value; }
        //}

        /// <summary>
        /// 左边标签的文字显示
        /// </summary>
        public string LeftText
        {
            get { return _leftText; }
            set { _leftText = value; }
        }

        /// <summary>
        /// 右边输入框的文字显示
        /// </summary>
        public string RightText
        {
            get { return _rightText; }
            set { _rightText = value; }
        }

        /// <summary>
        /// 是否必输项目
        /// </summary>
        public bool IsMustInput
        {
            get { return _isMustInput; }
            set { _isMustInput = value; }
        }

        /// <summary>
        /// 是否保护项目
        /// </summary>
        public bool IsProtect
        {
            get { return _isProtect; }
            set { _isProtect = value; }
        }

        /// <summary>
        /// 帮助数据源(内置帮助和通用帮助都支持)
        /// </summary>
        public string HelpDataSource
        {
            get { return _helpDataSource; }
            set { _helpDataSource = value; }
        }

        /// <summary>
        /// 帮助是否可以多行选择(多行选择后，填入的时候用逗号分隔)
        /// </summary>
        public bool IsHelpDsMultiSelect
        {
            get { return _isHelpDsMultiSelect; }
            set { _isHelpDsMultiSelect = value; }
        }        

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }

        /// <summary>
        /// 只读对应的表达式(表达式返回1或者0，1表示只读，0表示不只读)
        /// </summary>
        public PbExpressionImp IsReadOnlyExpressionImp
        {
            get { return _isReadOnlyExpressionImp; }
            set { _isReadOnlyExpressionImp = value; }
        }

        /// <summary>
        /// 必输项对应的表达式(表达式返回1表示必输，0表示不必输)
        /// </summary>
        public PbExpressionImp IsMustInputExpressionImp
        {
            get { return _isMustInputExpressionImp; }
            set { _isMustInputExpressionImp = value; }
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        public PbBuildInImp ClickEvent
        {
            get { return _clickEvent; }
            set { _clickEvent = value; }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        public PbBuildInImp DoubleClickEvent
        {
            get { return _doubleClickEvent; }
            set { _doubleClickEvent = value; }
        }

        /// <summary>
        /// 更新后的事件
        /// </summary>
        public PbEvent<PbExpressionImp> UpdateEvent
        {
            get { return _updateEvent; }
            set { _updateEvent = value; }
        }

        /// <summary>
        /// 对应的数据库字段信息
        /// </summary>
        public PbColumnInfo ColumnInfo
        {
            get { return _pbColumnInfo; }
            set { _pbColumnInfo = value; }
        }
    }
}
