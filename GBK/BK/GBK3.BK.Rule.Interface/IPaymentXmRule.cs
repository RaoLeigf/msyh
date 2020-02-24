#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Rule.Interface
    * 类 名 称：			IPaymentXmRule
    * 文 件 名：			IPaymentXmRule.cs
    * 创建时间：			2019/5/23 
    * 作    者：			刘杭    
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
using Enterprise3.NHORM.Interface.EntBase;

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Rule.Interface
{
	/// <summary>
	/// PaymentXm业务逻辑层接口
	/// </summary>
    public partial interface IPaymentXmRule : IEntRuleBase<PaymentXmModel>
    {
		#region IPaymentXmRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentXmModel> ExampleMethod<PaymentXmModel>(string param)

		#endregion
    }
}
