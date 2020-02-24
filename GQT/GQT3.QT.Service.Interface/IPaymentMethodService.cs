#region Summary
/**************************************************************************************
    * 类 名 称：        IPaymentMethodService
    * 命名空间：        GQT3.QT.Service.Interface
    * 文 件 名：        IPaymentMethodService.cs
    * 创建时间：        2018/9/7 
    * 作    者：        夏华军    
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// PaymentMethod服务组装层接口
	/// </summary>
    public partial interface IPaymentMethodService : IEntServiceBase<PaymentMethodModel>
    {
        #region IPaymentMethodService 业务添加的成员

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        FindedResults<PaymentMethodModel> ExecuteDataCheck(ref List<PaymentMethodModel> budgetAccounts,string otype);
        #endregion
    }
}
