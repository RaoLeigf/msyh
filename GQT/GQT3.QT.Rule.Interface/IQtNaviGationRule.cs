#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Rule.Interface
    * 类 名 称：			IQtNaviGationRule
    * 文 件 名：			IQtNaviGationRule.cs
    * 创建时间：			2019/11/14 
    * 作    者：			张宇    
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
	/// QtNaviGation业务逻辑层接口
	/// </summary>
    public partial interface IQtNaviGationRule : IEntRuleBase<QtNaviGationModel>
    {
		#region IQtNaviGationRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtNaviGationModel> ExampleMethod<QtNaviGationModel>(string param)

		#endregion
    }
}
