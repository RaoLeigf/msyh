#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Rule.Interface
    * 类 名 称：			IQtXmDistributeRule
    * 文 件 名：			IQtXmDistributeRule.cs
    * 创建时间：			2020/1/6 
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

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Rule.Interface
{
	/// <summary>
	/// QtXmDistribute业务逻辑层接口
	/// </summary>
    public partial interface IQtXmDistributeRule : IEntRuleBase<QtXmDistributeModel>
    {
		#region IQtXmDistributeRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtXmDistributeModel> ExampleMethod<QtXmDistributeModel>(string param)

		#endregion
    }
}
