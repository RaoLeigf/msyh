#region Summary
/**************************************************************************************
    * 命名空间：			GYS3.YS.Rule.Interface
    * 类 名 称：			IJxTrackingRule
    * 文 件 名：			IJxTrackingRule.cs
    * 创建时间：			2019/10/17 
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

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Rule.Interface
{
	/// <summary>
	/// JxTracking业务逻辑层接口
	/// </summary>
    public partial interface IJxTrackingRule : IEntRuleBase<JxTrackingModel>
    {
		#region IJxTrackingRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<JxTrackingModel> ExampleMethod<JxTrackingModel>(string param)

		#endregion
    }
}
