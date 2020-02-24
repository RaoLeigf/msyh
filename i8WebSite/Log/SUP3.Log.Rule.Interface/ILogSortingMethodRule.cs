#region Summary
/**************************************************************************************
    * 类 名 称：        ILogSortingMethodRule
    * 命名空间：        SUP3.Log.Rule.Interface
    * 文 件 名：        ILogSortingMethodRule.cs
    * 创建时间：        2017/11/4 
    * 作    者：        洪鹏    
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

using SUP3.Log.Model.Domain;

namespace SUP3.Log.Rule.Interface
{
	/// <summary>
	/// LogSortingMethod业务逻辑层接口
	/// </summary>
    public partial interface ILogSortingMethodRule : IEntRuleBase<LogSortingMethodModel>
    {
		#region ILogSortingMethodRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<LogSortingMethodModel> ExampleMethod<LogSortingMethodModel>(string param)

		#endregion
    }
}
