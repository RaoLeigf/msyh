using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXM3.XM.Model.Enums
{
    /// <summary>
    /// 审批状态：1|ToBeRepored|待上报;2|IsPending|审批中;3|Approved|已审批
    /// </summary>
    public enum EnumApproveStatus
    {
        /// <summary>
        /// 1-待上报
        /// </summary>
        ToBeRepored = 1,

        /// <summary>
        /// 2-审批中
        /// </summary>
        IsPending = 2,

        /// <summary>
        /// 3-已审批
        /// </summary>
        Approved = 3,

        /// <summary>
        /// 3-已退回
        /// </summary>
        NoApproved = 4
    }
}
