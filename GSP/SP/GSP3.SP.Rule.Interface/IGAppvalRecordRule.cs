#region Summary
/**************************************************************************************
    * 命名空间：			GSP3.SP.Rule.Interface
    * 类 名 称：			IGAppvalRecordRule
    * 文 件 名：			IGAppvalRecordRule.cs
    * 创建时间：			2019/5/20 
    * 作    者：			李明    
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

using GSP3.SP.Model.Domain;

namespace GSP3.SP.Rule.Interface
{
	/// <summary>
	/// GAppvalRecord业务逻辑层接口
	/// </summary>
    public partial interface IGAppvalRecordRule : IEntRuleBase<GAppvalRecordModel>
    {
		#region IGAppvalRecordRule 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GAppvalRecordModel> ExampleMethod<GAppvalRecordModel>(string param)

		#endregion
    }
}
