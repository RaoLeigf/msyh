#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectDtlImplPlanFacade
    * 命名空间：        GXM3.XM.Facade.Interface
    * 文 件 名：        IProjectDtlImplPlanFacade.cs
    * 创建时间：        2018/8/28 
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
	/// ProjectDtlImplPlan业务组装层接口
	/// </summary>
    public partial interface IProjectDtlImplPlanFacade : IEntFacadeBase<ProjectDtlImplPlanModel>
    {
		#region IProjectDtlImplPlanFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<ProjectDtlImplPlanModel> ExampleMethod<ProjectDtlImplPlanModel>(string param)


		#endregion
    }
}
