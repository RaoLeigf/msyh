using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB下拉控件
    /// </summary>
    public class PbComboboxInfo:PbBaseTextInfo
    {

        public PbComboboxInfo()
        {
            ControlType = PbControlType.ComboBox;
        }

        private IList<PbPairValueInfo> _pbComboboxValueInfos = new List<PbPairValueInfo>();

        /// <summary>
        /// 下拉列表
        /// </summary>
        public IList<PbPairValueInfo> PbComboboxValueInfos
        {
            get { return _pbComboboxValueInfos; }
            set { _pbComboboxValueInfos = value; }
        }
    }
}
