using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    public class PbTabInfo:PbBaseControlInfo
    {
        public PbTabInfo()
        {
            ControlType = PbControlType.Tab;
        }

        private IList<string> _gridIds = new List<string>();
        private IList<string> _report_views = new List<string>();
        private IList<string> _report_paras = new List<string>();
        private IList<string> _tabname = new List<string>();
        private string _region = string.Empty;

        /// <summary>
        /// tab的布局位置
        /// </summary>
        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        /// <summary>
        /// Tab页中对应的Grid编号集合，按照顺序
        /// </summary>
        public IList<string> GridIds
        {
            get { return _gridIds; }
            set { _gridIds = value; }
        }

        /// <summary>
        /// Tab页中对应的报表信息
        /// </summary>
        public IList<string> ReportViews
        {
            get { return _report_views; }
            set { _report_views = value; }
        }

        /// <summary>
        /// Tab页中对应的报表参数信息
        /// </summary>
        public IList<string> ReportParas
        {
            get { return _report_paras; }
            set { _report_paras = value; }
        }

        /// <summary>
        /// Tab页标签
        /// </summary>
        public IList<string> TabNames
        {
            get { return _tabname; }
            set { _tabname = value; }
        }
    }
}
