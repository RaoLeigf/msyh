using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// PB按钮控件
    /// </summary>
    public class PbButtonInfo : PbBaseTextInfo
    {
        public PbButtonInfo()
        {
            ControlType = PbControlType.Button;
        }


        /*
        private string _text = string.Empty;

        /// <summary>
        /// 按钮的显示文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        public PbEvent<PbExpressionImp> ClickEvent
        {
            get { return _clickEvent; }
            set { _clickEvent = value; }
        }

        private PbEvent<PbExpressionImp> _clickEvent = new PbEvent<PbExpressionImp>();
        */
    }
}
