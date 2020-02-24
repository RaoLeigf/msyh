using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBK3.BK.Model.Enums
{
    /// <summary>
    /// ApprovalType(审批状态)
    /// </summary>
    public enum ApprovalType
    {
        /// <summary>
        /// 0-未发起审批
        /// </summary>
        not = 0,

		/// <summary>
		/// 1-审批中
		/// </summary>
		pend = 1,

        /// <summary>
        /// 2-未通过
        /// </summary>
        no = 2,

        /// <summary>
        /// 9-审批通过
        /// </summary>
        yes = 9
    }
}