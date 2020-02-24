using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Enums
{
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum Approval
    {
        /// <summary>
        /// 待送审
        /// </summary>
        Send = 0,
        /// <summary>
        /// 待审批
        /// </summary>
        Wait = 1,
        /// <summary>
        /// 未通过
        /// </summary>
        UnPass = 2,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 9
    }
}
