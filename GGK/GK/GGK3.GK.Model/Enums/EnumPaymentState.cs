#region Summary
/**************************************************************************************
    * 类 名 称：        EnumYesNo
    * 命名空间：        GXM3.XM.Model.Enums
    * 文 件 名：        EnumYesNo.cs
    * 创建时间：        2018/8/29 
    * 作    者：        李明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGK3.GK.Model.Enums
{
    /// <summary>
    /// 支付状态: 0-待支付, 1-支付成功, 2-支付异常, 3-支付中
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