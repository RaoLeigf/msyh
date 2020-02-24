#region Summary
/**************************************************************************************
    * 命名空间：			GBK3.BK.Rule.Interface
    * 类 名 称：			IPaymentDtlRule
    * 文 件 名：			IPaymentDtlRule.cs
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
using System.Linq;
using System.Text;
using Enterprise3.NHORM.Interface.EntBase;

using GBK3.BK.Model.Domain;

namespace GBK3.BK.Rule.Interface
{
	/// <summary>
	/// PaymentDtl业务逻辑层接口
	/// </summary>
    public partial interface IPaymentDtlRule : IEntRuleBase<PaymentDtlModel>
    {
		#region IPaymentDtlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PaymentDtlModel> ExampleMethod<PaymentDtlModel>(string param)

		#endregion
    }
}
