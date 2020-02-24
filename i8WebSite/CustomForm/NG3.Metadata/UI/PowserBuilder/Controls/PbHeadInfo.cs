using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB表头控件
    /// </summary>
    public class PbHeadInfo : PbBaseControlInfo
    {
        public PbHeadInfo()
        {
            ControlType = PbControlType.Head;
        }

        private IList<PbBaseTextInfo> _pbColumns = new List<PbBaseTextInfo>();
        private IList<PbBaseControlInfo> _pbBaseControlInfos = new List<PbBaseControlInfo>();
        private IList<PbPictureboxInfo> _pbPictureboxInfos = new List<PbPictureboxInfo>();
        private Dictionary<string, string> _colrefdic = new Dictionary<string, string>();
        private IDictionary<string, string> _textareadic = new Dictionary<string, string>();
        private IDictionary<string, string> _colspandic = new Dictionary<string, string>();
        private IDictionary<string, string> _collapsedic = new Dictionary<string, string>();
        private IDictionary<string, string> _multiselectdic = new Dictionary<string, string>();
        private string _sql = string.Empty;
        private string _tableName = string.Empty;
        private bool _abslayout;
        private string _otid;
        private int _columnsPerRow = 4;
        private int _formLabelWidth = 80;

        //保存列标签和列单元格的关联关系
        public Dictionary<string, string> Colrefdic
        {
            get { return _colrefdic; }
            set { _colrefdic = value; }
        }

        //保存类型为textarea的字段
        public IDictionary<string, string> TextAreaDic
        {
            get { return _textareadic; }
            set { _textareadic = value; }
        }

        //保存所有列的列占位数
        public IDictionary<string, string> ColSpanDic
        {
            get { return _colspandic; }
            set { _colspandic = value; }
        }

        //保存所有fieldset的折叠属性
        public IDictionary<string, string> CollapseDic
        {
            get { return _collapsedic; }
            set { _collapsedic = value; }
        }

        //保存所有用复选框的列
        public IDictionary<string, string> MultiSelectDic
        {
            get { return _multiselectdic; }
            set { _multiselectdic = value; }
        }

        /// <summary>
        /// 表头其他控件信息(包括表体的Group)
        /// </summary>
        public IList<PbBaseControlInfo> PbBaseControlInfos
        {
            get { return _pbBaseControlInfos; }
            set { _pbBaseControlInfos = value; }
        }

        public bool Abslayout
        {
            get { return _abslayout; }
            set { _abslayout = value; }
        }

        public string Otid
        {
            get { return _otid; }
            set { _otid = value; }
        }

        public int ColumnsPerRow
        {
            get { return _columnsPerRow; }
            set { _columnsPerRow = value; }
        }

        public int FormLabelWidth
        {
            get { return _formLabelWidth; }
            set { _formLabelWidth = value; }
        }

        /// <summary>
        /// 表头列信息
        /// </summary>
        public IList<PbBaseTextInfo> PbColumns
        {
            get { return _pbColumns; }
            set { _pbColumns = value; }
        }

        /// <summary>
        /// 表头对应的SQL语句
        /// </summary>
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        /// <summary>
        /// 表头对应的表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 图片控件
        /// </summary>
        public IList<PbPictureboxInfo> PbPictureboxInfos
        {
            get { return _pbPictureboxInfos; }
            set { _pbPictureboxInfos = value; }
        }
       
    }
}
