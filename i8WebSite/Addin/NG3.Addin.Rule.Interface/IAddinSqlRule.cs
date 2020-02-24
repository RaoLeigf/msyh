#region Summary
/**************************************************************************************
    * 类 名 称：        IAddinSqlRule
    * 命名空间：        NG3.Addin.Rule.Interface
    * 文 件 名：        IAddinSqlRule.cs
    * 创建时间：        2017/7/10 
    * 作    者：        韦忠吉    
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
	/// AddinSql业务逻辑层接口
	/// </summary>
    public partial interface IAddinSqlRule : IEntRuleBase<AddinSqlModel>
    {
		#region IAddinSqlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<AddinSqlModel> ExampleMethod<AddinSqlModel>(string param)

		#endregion
    }
}
