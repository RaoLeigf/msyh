using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUP.Common.DataEntity
{
    /// <summary>
    /// 对应表fg_layoutlog
    ///[gid] [varchar](50) NOT NULL,
    ///[bustype] [varchar](100) NULL,
    ///[logid] [varchar](250)  NULL,
    ///[value][text]  NULL
    /// </summary>
    [Serializable]
    public class LayoutLogInfo
    {
        /// <summary>
        /// Gid
        /// </summary>
        public string Gid
        {
            get;
            set;
        }

        /// <summary>
        /// Bustype
        /// </summary>
        public string Bustype
        {
            get;
            set;
        }

        /// <summary>
        /// Logid
        /// </summary>
        public string Logid
        {
            get;
            set;
        }

        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        public string Pagesize
        {
            get;
            set;
        }
    }

    /// <summary>
    /// toolbar显示信息
    /// </summary>
    [Serializable]
    public class ToolBarInfo
    {
        public string PageID
        {
            get;
            set;
        }

        public string LogID
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
