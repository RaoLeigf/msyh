#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Rule.Interface
    * 类 名 称：			IPaymentMstRule
    * 文 件 名：			IPaymentMstRule.cs
    * 创建时间：			2019/5/15 
    * 作    者：			吾丰明    
    * 说    明：        
---------------------------------------------------------------------------------------
    * 修改时间：        * 修改人：        *说明：
    *
***************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Enterprise3.NHORM.Interface.EntBase;

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Rule.Interface
{
	/// <summary>
	/// PaymentMst业务逻辑层接口
	/// </summary>
    public partial interface IPaymentMstRule : IEntRuleBase<PaymentMstModel>
    {
        #region IPaymentMstRule 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentMstModel> ExampleMethod<PaymentMstModel>(string param)


        /// <summary>
        /// 获取资金拨付主列表
        /// </summary>
        /// <param name="fPhid"></param>
        /// <returns></returns>
        DataTable GetPaymentList(long fPhid);

        /// <summary>
        /// 根据主表主键查找明细表数据
        /// </summary>
        /// <param name="mstPhid">主表主键</param>
        /// <returns></returns>
        IList<PaymentDtlModel> GetPaymentDtlsByMstPhid(long mstPhid);

        /// <summary>
        /// 根据单据主键与支付状态修改单据
        /// </summary>
        /// <param name="phid">单据主键</param>
        /// <param name="isPay">支付状态</param>
        void UpdatePaymentPay(long phid, int isPay);
        #endregion
    }
}
