#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectDtlPurchaseDtlFacade
    * 命名空间：        GXM3.XM.Facade.Interface
    * 文 件 名：        IProjectDtlPurchaseDtlFacade.cs
    * 创建时间：        2018/10/15 
    * 作    者：        李明    
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

using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade.Interface
{
	/// <summary>
	/// ProjectDtlPurchaseDtl业务组装层接口
	/// </summary>
    public partial interface IProjectDtlPurchaseDtlFacade : IEntFacadeBase<ProjectDtlPurchaseDtlModel>
    {
		#region IProjectDtlPurchaseDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlPurchaseDtlModel> ExampleMethod<ProjectDtlPurchaseDtlModel>(string param)

		#endregion
    }
}
