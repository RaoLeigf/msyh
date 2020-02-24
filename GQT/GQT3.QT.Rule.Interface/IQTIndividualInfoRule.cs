#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Rule.Interface
    * 类 名 称：			IQTIndividualInfoRule
    * 文 件 名：			IQTIndividualInfoRule.cs
    * 创建时间：			2019/5/14 
    * 作    者：			董泉伟    
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
	/// QTIndividualInfo业务逻辑层接口
	/// </summary>
    public partial interface IQTIndividualInfoRule : IEntRuleBase<QTIndividualInfoModel>
    {
		#region IQTIndividualInfoRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QTIndividualInfoModel> ExampleMethod<QTIndividualInfoModel>(string param)

		#endregion
    }
}
