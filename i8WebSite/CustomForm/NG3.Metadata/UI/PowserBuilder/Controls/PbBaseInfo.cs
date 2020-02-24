using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Metadata.Core;

namespace NG3.Metadata.UI.PowserBuilder.Controls
{
    public class PbBaseInfo:MetadataGod
    {
        private PbControlType _pbControlType = PbControlType.Unknow;
        private string _description = string.Empty;
        private bool _visible;
        private string _name = string.Empty;
        private string _fullName = string.Empty;
        private string _tooltipName = string.Empty;

        private bool _isAbsoluteLayout = true;  //是否绝对布局

        /// <summary>
        /// PB的控件类型
        /// </summary>
        public PbControlType ControlType
        {
            get { return _pbControlType; }
            set { _pbControlType = value; }
        }

        /// <summary>
        /// 控件描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <summary>
        /// Tooltip显示的中文名称
        /// </summary>
        public string TooltipName
        {
            get { return _tooltipName; }
            set { _tooltipName = value; }
        }

        /// <summary>
        /// 是否绝对布局
        /// </summary>
        public bool IsAbsoluteLayout
        {
            get { return _isAbsoluteLayout; }
            set { _isAbsoluteLayout = value; }
        }
    }
}
