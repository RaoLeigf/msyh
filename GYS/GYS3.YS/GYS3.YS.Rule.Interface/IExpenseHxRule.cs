#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Rule.Interface
    * 类 名 称：			IExpenseHxRule
    * 文 件 名：			IExpenseHxRule.cs
    * 创建时间：			2019/5/5 
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule.Interface
{
	/// <summary>
	/// ExpenseHx业务逻辑层接口
	/// </summary>
    public partial interface IExpenseHxRule : IEntRuleBase<ExpenseHxModel>
    {
		#region IExpenseHxRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseHxModel> ExampleMethod<ExpenseHxModel>(string param)

		#endregion
    }
}
