#region Summary
/**************************************************************************************
    * 类 名 称：        IAddinServiceRule
    * 命名空间：        NG3.Addin.Rule.Interface
    * 文 件 名：        IAddinServiceRule.cs
    * 创建时间：        2017/12/13 
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

using NG3.Addin.Model.Domain;

namespace NG3.Addin.Rule.Interface
{
	/// <summary>
	/// AddinService业务逻辑层接口
	/// </summary>
    public partial interface IAddinServiceRule : IEntRuleBase<AddinServiceModel>
    {
		#region IAddinServiceRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<AddinServiceModel> ExampleMethod<AddinServiceModel>(string param)

		#endregion
    }
}
