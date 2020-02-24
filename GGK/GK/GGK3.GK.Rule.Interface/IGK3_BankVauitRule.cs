#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Rule.Interface
    * 类 名 称：			IGK3_BankVauitRule
    * 文 件 名：			IGK3_BankVauitRule.cs
    * 创建时间：			2019/11/18 
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

using GGK3.GK.Model.Domain;

namespace GGK3.GK.Rule.Interface
{
	/// <summary>
	/// GK3_BankVauit业务逻辑层接口
	/// </summary>
    public partial interface IGK3_BankVauitRule : IEntRuleBase<GK3_BankVauitModel>
    {
		#region IGK3_BankVauitRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GK3_BankVauitModel> ExampleMethod<GK3_BankVauitModel>(string param)

		#endregion
    }
}
