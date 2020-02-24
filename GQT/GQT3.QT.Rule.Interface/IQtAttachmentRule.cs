#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Rule.Interface
    * 类 名 称：			IQtAttachmentRule
    * 文 件 名：			IQtAttachmentRule.cs
    * 创建时间：			2019/6/17 
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
	/// QtAttachment业务逻辑层接口
	/// </summary>
    public partial interface IQtAttachmentRule : IEntRuleBase<QtAttachmentModel>
    {
		#region IQtAttachmentRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<QtAttachmentModel> ExampleMethod<QtAttachmentModel>(string param)

		#endregion
    }
}
