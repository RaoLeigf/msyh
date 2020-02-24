#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetDtlPerformTargetRule
    * 命名空间：        GYS3.YS.Rule.Interface
    * 文 件 名：        IBudgetDtlPerformTargetRule.cs
    * 创建时间：        2018/10/16 
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
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule.Interface
{
	/// <summary>
	/// BudgetDtlPerformTarget业务逻辑层接口
	/// </summary>
    public partial interface IBudgetDtlPerformTargetRule : IEntRuleBase<BudgetDtlPerformTargetModel>
    {
		#region IBudgetDtlPerformTargetRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetDtlPerformTargetModel> ExampleMethod<BudgetDtlPerformTargetModel>(string param)

		#endregion
    }
}
