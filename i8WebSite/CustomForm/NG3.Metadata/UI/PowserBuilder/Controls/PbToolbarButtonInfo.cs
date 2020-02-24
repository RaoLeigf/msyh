using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.UI.PowserBuilder.Events;
using NG3.Metadata.UI.PowserBuilder.Events.Implementation;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    /// <summary>
    /// 工具条按钮信息
    /// </summary>
    public class PbToolbarButtonInfo:PbBaseInfo
    {
        private PbEvent<PbBuildInImp> _clickEvent = new PbEvent<PbBuildInImp>();
        private bool _isDockLeft = false;

        private bool _isShowDisplayName = false;

        private PbEventImpType _pbEventImpType;

        public PbToolbarButtonInfo()
        {
            ControlType = PbControlType.ToolbarButton;
        }


        /// <summary>
        /// 工具条按钮单击事件
        /// </summary>
        public PbEvent<PbBuildInImp> ClickEvent
        {
            get { return _clickEvent; }
            set { _clickEvent = value; }
        }

        /// <summary>
        /// 是否在左边停靠，如果是否，就是右边停靠
        /// </summary>
        public bool IsDockLeft
        {
            get { return _isDockLeft; }
            set { _isDockLeft = value; }
        }

        /// <summary>
        /// 是否显示工具栏的显示名称
        /// </summary>
        public bool IsShowDisplayName
        {
            get { return _isShowDisplayName; }
            set { _isShowDisplayName = value; }
        }


         public PbEventImpType EventImpType
        {
            get { return _pbEventImpType; }
            set { _pbEventImpType = value; }
        }
    }
}
