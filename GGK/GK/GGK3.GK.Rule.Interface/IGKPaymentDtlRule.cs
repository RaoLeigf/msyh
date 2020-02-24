#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Rule.Interface
    * 类 名 称：			IGKPaymentDtlRule
    * 文 件 名：			IGKPaymentDtlRule.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GGK3.GK.Model.Domain;

namespace GGK3.GK.Rule.Interface
{
	/// <summary>
	/// GKPaymentDtl业务逻辑层接口
	/// </summary>
    public partial interface IGKPaymentDtlRule : IEntRuleBase<GKPaymentDtlModel>
    {
		#region IGKPaymentDtlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GKPaymentDtlModel> ExampleMethod<GKPaymentDtlModel>(string param)

		#endregion
    }
}
