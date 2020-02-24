#region Summary
/**************************************************************************************
    * 命名空间：			GXM3.XM.Rule.Interface
    * 类 名 称：			IXmReportDtlRule
    * 文 件 名：			IXmReportDtlRule.cs
    * 创建时间：			2020/1/17 
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Rule.Interface
{
	/// <summary>
	/// XmReportDtl业务逻辑层接口
	/// </summary>
    public partial interface IXmReportDtlRule : IEntRuleBase<XmReportDtlModel>
    {
		#region IXmReportDtlRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<XmReportDtlModel> ExampleMethod<XmReportDtlModel>(string param)

		#endregion
    }
}
