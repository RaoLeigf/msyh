#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Rule.Interface
    * 类 名 称：			IQtCoverUpDataRule
    * 文 件 名：			IQtCoverUpDataRule.cs
    * 创建时间：			2019/10/29 
    * 作    者：			王冠冠    
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
	/// QtCoverUpData业务逻辑层接口
	/// </summary>
    public partial interface IQtCoverUpDataRule : IEntRuleBase<QtCoverUpDataModel>
    {
		#region IQtCoverUpDataRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtCoverUpDataModel> ExampleMethod<QtCoverUpDataModel>(string param)

		#endregion
    }
}
