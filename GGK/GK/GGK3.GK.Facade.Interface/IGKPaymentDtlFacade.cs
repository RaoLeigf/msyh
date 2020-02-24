#region Summary
/**************************************************************************************
    * 命名空间：			GGK3.GK.Facade.Interface
    * 类 名 称：			IGKPaymentDtlFacade
    * 文 件 名：			IGKPaymentDtlFacade.cs
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
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GGK3.GK.Model.Domain;

namespace GGK3.GK.Facade.Interface
{
	/// <summary>
	/// GKPaymentDtl业务组装层接口
	/// </summary>
    public partial interface IGKPaymentDtlFacade : IEntFacadeBase<GKPaymentDtlModel>
    {
		#region IGKPaymentDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<GKPaymentDtlModel> ExampleMethod<GKPaymentDtlModel>(string param)

		#endregion
    }
}
