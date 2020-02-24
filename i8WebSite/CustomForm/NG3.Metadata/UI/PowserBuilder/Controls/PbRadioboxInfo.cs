using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// 单选控件
    /// </summary>
    public class PbRadioboxInfo:PbBaseTextInfo
    {
        private IList<PbPairValueInfo> _pbPairValueInfos = new List<PbPairValueInfo>();

        public PbRadioboxInfo()
        {
            ControlType = PbControlType.Radiobox;
            //_pbPairValueInfos.Add(new PbPairValueInfo("是","1"));
            //_pbPairValueInfos.Add(new PbPairValueInfo("否", "0"));
        }

        public IList<PbPairValueInfo> PbPairValueInfos
        {
            get { return _pbPairValueInfos; }
            set { _pbPairValueInfos = value; }
        }
    }
}
