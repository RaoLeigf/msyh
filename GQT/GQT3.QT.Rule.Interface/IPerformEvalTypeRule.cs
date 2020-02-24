#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformEvalTypeRule
    * 命名空间：        GQT3.QT.Rule.Interface
    * 文 件 名：        IPerformEvalTypeRule.cs
    * 创建时间：        2018/10/16 
    * 作    者：        李长敏琛    
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

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Rule.Interface
{
	/// <summary>
	/// PerformEvalType业务逻辑层接口
	/// </summary>
    public partial interface IPerformEvalTypeRule : IEntRuleBase<PerformEvalTypeModel>
    {
		#region IPerformEvalTypeRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformEvalTypeModel> ExampleMethod<PerformEvalTypeModel>(string param)

		#endregion
    }
}
