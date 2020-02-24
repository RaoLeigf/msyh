using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXM3.XM.Model.Enums
{
    /// <summary>
    /// 项目状态：1|Alternative|单位备选;2|InBudget|纳入预算;3|Execute|项目执行;4|Adjust|项目调整;5|Pause|项目暂停;6|Terminated|项目终止;7|Closed|项目关闭
    /// </summary>
    public enum EnumProjStatus
    {
        /// <summary>
        /// 1-单位备选
        /// </summary>
        Alternative = 1,

        /// <summary>
        /// 2-纳入预算
        /// </summary>
        InBudget = 2,

        /// <summary>
        /// 3-项目执行
        /// </summary>
        Execute = 3,


        /// <summary>
        /// 4-项目调整
        /// </summary>
        Adjust = 4,

        /// <summary>
        /// 5-项目暂停
        /// </summary>
        Pause = 5,

        /// <summary>
        /// 6-项目终止
        /// </summary>
        Terminated = 6,

        /// <summary>
        /// 7-项目关闭
        /// </summary>
        Closed = 7
    }
}
