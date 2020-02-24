using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB通用帮助控件
    /// </summary>
    public class PbDataHelpEditInfo : PbBaseTextInfo
    {
        public PbDataHelpEditInfo()
        {
            ControlType = PbControlType.DataHelpEdit;
        }

        private string _dataHelpId = string.Empty;

        /// <summary>
        /// 通用帮助编号
        /// </summary>
        public string DataHelpId
        {
            get { return _dataHelpId; }
            set { _dataHelpId = value; }
        }

        private string _sql = string.Empty;

        private string _displayColumn = string.Empty;
        private string _displayColumnName = string.Empty;

        private string _contentColumn = string.Empty;
        private string _contentColumnName = string.Empty;


        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        public string DisplayColumn
        {
            get { return _displayColumn; }
            set { _displayColumn = value; }
        }

        public string ContentColumn
        {
            get { return _contentColumn; }
            set { _contentColumn = value; }
        }

        public string DisplayColumnName
        {
            get { return _displayColumnName; }
            set { _displayColumnName = value; }
        }

        public string ContentColumnName
        {
            get { return _contentColumnName; }
            set { _contentColumnName = value; }
        }

        public bool IsSupportMultiSelect { get; set; }
    }
}
