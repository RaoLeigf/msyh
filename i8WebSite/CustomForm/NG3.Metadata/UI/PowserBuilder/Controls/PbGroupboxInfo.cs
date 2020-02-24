using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// 分组框控件
    /// </summary>
    public class PbGroupboxInfo:PbBaseControlInfo
    {
        public PbGroupboxInfo()
        {
            ControlType = PbControlType.Gruopbox;
        }

        private string _text = string.Empty;

        /// <summary>
        /// 分组框的名称
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
