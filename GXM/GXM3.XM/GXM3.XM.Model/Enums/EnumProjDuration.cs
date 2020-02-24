using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXM3.XM.Model.Enums
{
    /// <summary>
    /// 存续期限：1|OneOff|一次性项目;2|Frequent|经常性项目;3|CrossYear|跨年度项目
    /// </summary>
    public enum EnumProjDuration
    {
        /// <summary>
        /// 1-一次性项目
        /// </summary>
        OneOff = 1,

        /// <summary>
        /// 2-经常性项目
        /// </summary>
        Frequent = 2,

        /// <summary>
        /// 3-CrossYear
        /// </summary>
        CrossYear = 3
    }
}
