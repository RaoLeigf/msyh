using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Model
{
    /// <summary>
    /// 支付状态， 0 待支付, 1 支付成功, 2 支付异常, 3 支付中
    /// </summary>
    public enum EnumPaymentState
    {
        /// <summary>
		/// 0-待支付
		/// </summary>
		PendingPayment = 0,

        /// <summary>
        /// 1-已付款
        /// </summary>
        Paid = 1,

        /// <summary>
        /// 2-支付异常
        /// </summary>
        AbnormalPayment = 2,

        /// <summary>
        /// 支付中
        /// </summary>
        DuringPayment = 3
    }
}
