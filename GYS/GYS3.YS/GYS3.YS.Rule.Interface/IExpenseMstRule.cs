#region Summary
/**************************************************************************************
    * 类 名 称：        IExpenseMstRule
    * 命名空间：        GYS3.YS.Rule.Interface
    * 文 件 名：        IExpenseMstRule.cs
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
	/// ExpenseMst业务逻辑层接口
	/// </summary>
    public partial interface IExpenseMstRule : IEntRuleBase<ExpenseMstModel>
    {
		#region IExpenseMstRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ExpenseMstModel> ExampleMethod<ExpenseMstModel>(string param)

		#endregion
    }
}
