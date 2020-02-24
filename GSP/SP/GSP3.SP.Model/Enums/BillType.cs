using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSP3.SP.Model.Enums
{
    /// <summary>
    /// 单据类型
    /// </summary>
    public class BillType
    {
        /// <summary>
        /// 资金拨付单
        /// </summary>
        public static string FundsPay = "001";

        /// <summary>
        /// 支付单
        /// </summary>
        public static string PayMent = "002";

        /// <summary>
        /// 项目用款计划
        /// </summary>
        public static string Expense = "003"; 

        /// <summary>
        /// 项目年初的工作流(及预立项)(民生银行的项目立项)
        /// </summary>
        public static string BeginProject = "004";

        /// <summary>
        /// 项目年中的工作流（及项目立项）（民生银行的项目草案）
        /// </summary>
        public static string MiddleProject = "005";

        /// <summary>
        /// 年中新增(直接在预算点新增)
        /// </summary>
        public static string MiddleBudget = "006";

        #region//年中调整分三个审批流

        /// <summary>
        /// 年中预算新增的工作流
        /// </summary>
        public static string MiddleAddBudget = "007";

        /// <summary>
        /// 年中预算修改的工作流
        /// </summary>
        public static string MiddleUpdateBudget = "008";

        /// <summary>
        /// 年中预算新增的工作流
        /// </summary>
        public static string MiddleDtlBudget = "009";

        /// <summary>
        /// 收入预算的审批流
        /// </summary>
        public static string InComeBudget = "010";

        #endregion

    }
}
