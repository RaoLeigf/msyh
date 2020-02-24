using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB的标签控件
    /// </summary>
    public class PbLabelInfo : PbBaseTextInfo
    {
        public PbLabelInfo()
        {
            ControlType = PbControlType.Label;
        }

        private string _text = string.Empty;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
