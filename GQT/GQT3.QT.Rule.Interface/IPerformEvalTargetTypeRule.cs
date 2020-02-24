#region Summary
/**************************************************************************************
    * 类 名 称：        IPerformEvalTargetTypeRule
    * 命名空间：        GQT3.QT.Rule.Interface
    * 文 件 名：        IPerformEvalTargetTypeRule.cs
    * 创建时间：        2018/10/15 
    * 作    者：        刘杭    
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
	/// PerformEvalTargetType业务逻辑层接口
	/// </summary>
    public partial interface IPerformEvalTargetTypeRule : IEntRuleBase<PerformEvalTargetTypeModel>
    {
		#region IPerformEvalTargetTypeRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<PerformEvalTargetTypeModel> ExampleMethod<PerformEvalTargetTypeModel>(string param)

		#endregion
    }
}
