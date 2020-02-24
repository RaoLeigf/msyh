using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB单据体信息
    /// </summary>
    public class PbGridInfo:PbBaseControlInfo
    {
        public PbGridInfo()
        {
            ControlType = PbControlType.Grid;
        }

        private bool _isInTab = false;

        private string _tabId = string.Empty;

        private string _sql = string.Empty;

        private string _tableName = string.Empty;

        private string _parentId = string.Empty;

        private string _title = string.Empty;

        private string _sumrowstyle = string.Empty;

        private string _nosumrowstyle = string.Empty;

        IList<PbBaseTextInfo> _pbBaseTextInfos = new List<PbBaseTextInfo>();

        /// <summary>
        /// 是否在Tab页内
        /// </summary>
        public bool IsInTab
        {
            get { return _isInTab; }
            set { _isInTab = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// 对应的字段集合
        /// </summary>
        public IList<PbBaseTextInfo> PbBaseTextInfos
        {
            get { return _pbBaseTextInfos; }
            set { _pbBaseTextInfos = value; }
        }

        /// <summary>
        /// 对应的表体的ID(支持多级单据体的时候用，为空表示父亲是表头)
        /// </summary>
        public string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        /// <summary>
        /// 对应的Tab页编号
        /// </summary>
        public string TabId
        {
            get { return _tabId; }
            set { _tabId = value; }
        }

        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string SumRowStyle
        {
            get { return _sumrowstyle; }
            set { _sumrowstyle = value; }
        }

        public string NoSumRowStyle
        {
            get { return _nosumrowstyle; }
            set { _nosumrowstyle = value; }
        }
        
    }
}
