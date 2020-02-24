using System;
using System.Collections.Generic;

namespace SUP.CustomForm.ServerGen
{
    [Serializable]
    public class TemplateInfo
    {
        private string _nameSpacePrefix;// 命名空间前缀;
        private string _className;   // 类名;
        private string _pform;
        private string _eform;
        private string _qform;
        private string _title;
        private string _tableMaster; // 主表名;
        private string _sqlList;     // 列表sql语句;
        private string _sqlMaster;   // 主表sql语句;
        private string _hasAsrGrid;  //有附件单据体
        private IList<TemplateDetailInfo> _detailInfoList = new List<TemplateDetailInfo>();// 明细表相关信息放在List中;
        private IList<CodeToNameInfo> _codeToNameList = new List<CodeToNameInfo>();// List中需要代码转名称的字段;
        private IList<CodeToNameInfo> _codeToNameGrid = new List<CodeToNameInfo>();// Grids中需要代码转名称的字段;
        private IList<string> _bitmapNameList = new List<string>();// 图片控件的名称放在List中;

        public string NameSpacePrefix
        {
            get { return _nameSpacePrefix; }
            set { _nameSpacePrefix = value; }
        }

        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        public string PForm
        {
            get { return _pform; }
            set { _pform = value; }
        }

        public string EForm
        {
            get { return _eform; }
            set { _eform = value; }
        }

        public string QForm
        {
            get { return _qform; }
            set { _qform = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string TableMaster
        {
            get { return _tableMaster; }
            set { _tableMaster = value; }
        }
        public string SqlList
        {
            get { return _sqlList; }
            set { _sqlList = value; }
        }
        public string SqlMaster
        {
            get { return _sqlMaster; }
            set { _sqlMaster = value; }
        }

        public string HasAsrGrid
        {
            get { return _hasAsrGrid; }
            set { _hasAsrGrid = value; }
        }

        public IList<TemplateDetailInfo> DetailInfoList
        {
            get { return _detailInfoList; }
            set { _detailInfoList = value; }
        }

        public IList<CodeToNameInfo> CodeToNameList
        {
            get { return _codeToNameList; }
            set { _codeToNameList = value; }
        }

        public IList<CodeToNameInfo> CodeToNameGrid
        {
            get { return _codeToNameGrid; }
            set { _codeToNameGrid = value; }
        }

        public IList<string> BitmapNameList
        {
            get { return _bitmapNameList; }
            set { _bitmapNameList = value; }
        }

    }

    [Serializable]
    public class TemplateDetailInfo
    {
        private string _sql; // sql语句;
        private string _name;// 明细表单grid的名字;
        private string _tableName;// 表名;
        private string _subtotal;//表体属性合集;
        private Dictionary<string, string> groupfield = new Dictionary<string, string>();  //分组小计的分组列

        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public string Subtotal
        {
            get { return _subtotal; }
            set { _subtotal = value; }
        }

        public Dictionary<string, string> Groupfield
        {
            get { return groupfield; }
            set { groupfield = value; }
        }
    }

    [Serializable]
    public class CodeToNameInfo
    {
        public string TableName { get; set; }

        public string CodeName { get; set; }

        public string HelpId { get; set; }

        public bool MultiSelect { get; set; }
    }
}
