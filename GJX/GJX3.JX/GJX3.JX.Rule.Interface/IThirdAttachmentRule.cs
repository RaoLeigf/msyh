#region Summary
/**************************************************************************************
    * 命名空间：			GJX3.JX.Rule.Interface
    * 类 名 称：			IThirdAttachmentRule
    * 文 件 名：			IThirdAttachmentRule.cs
    * 创建时间：			2019/10/9 
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

using GJX3.JX.Model.Domain;

namespace GJX3.JX.Rule.Interface
{
	/// <summary>
	/// ThirdAttachment业务逻辑层接口
	/// </summary>
    public partial interface IThirdAttachmentRule : IEntRuleBase<ThirdAttachmentModel>
    {
		#region IThirdAttachmentRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ThirdAttachmentModel> ExampleMethod<ThirdAttachmentModel>(string param)

		#endregion
    }
}
