using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    public class PbToolbarButtonGroupInfo:MetadataGod
    {
        private IList<PbToolbarButtonInfo> _toolbarButtonInfos = new List<PbToolbarButtonInfo>();

        private bool _isSplit = false;

        /// <summary>
        /// 工具栏按钮按照list的顺序排列
        /// </summary>
        public IList<PbToolbarButtonInfo> ToolbarButtonInfos
        {
            get { return _toolbarButtonInfos; }
            set { _toolbarButtonInfos = value; }
        }

        /// <summary>
        /// 是否是工具栏的分隔符
        /// </summary>
        public bool IsSplit
        {
            get { return _isSplit; }
            set { _isSplit = value; }
        }
    }
}
