#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseDtlRule
    * 命名空间：        GYS3.YS.Rule.Interface
    * 文 件 名：        IExpenseDtlRule.cs
    * 创建时间：        2019/1/24 
    * 作    者：        董泉伟    
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule.Interface
{
	/// <summary>
	/// ExpenseDtl业务逻辑层接口
	/// </summary>
    public partial interface IExpenseDtlRule : IEntRuleBase<ExpenseDtlModel>
    {
		#region IExpenseDtlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseDtlModel> ExampleMethod<ExpenseDtlModel>(string param)

		#endregion
    }
}
