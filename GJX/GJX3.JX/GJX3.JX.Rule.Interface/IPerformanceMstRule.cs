#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformanceMstRule
    * 命名空间：        GJX3.JX.Rule.Interface
    * 文 件 名：        IPerformanceMstRule.cs
    * 创建时间：        2018/9/12 
    * 作    者：        吾丰明    
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

using GJX3.JX.Model.Domain;

namespace GJX3.JX.Rule.Interface
{
	/// <summary>
	/// PerformanceMst业务逻辑层接口
	/// </summary>
    public partial interface IPerformanceMstRule : IEntRuleBase<PerformanceMstModel>
    {
		#region IPerformanceMstRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformanceMstModel> ExampleMethod<PerformanceMstModel>(string param)

		#endregion
    }
}
