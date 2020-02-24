#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetProcessCtrlRule
    * 命名空间：        GYS3.YS.Rule.Interface
    * 文 件 名：        IBudgetProcessCtrlRule.cs
    * 创建时间：        2018/9/10 
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
	/// BudgetProcessCtrl业务逻辑层接口
	/// </summary>
    public partial interface IBudgetProcessCtrlRule : IEntRuleBase<BudgetProcessCtrlModel>
    {
		#region IBudgetProcessCtrlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetProcessCtrlModel> ExampleMethod<BudgetProcessCtrlModel>(string param)

		#endregion
    }
}
