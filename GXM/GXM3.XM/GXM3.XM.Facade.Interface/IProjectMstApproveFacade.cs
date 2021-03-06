#region Summary
/**************************************************************************************
    * 类 名 称：        IProjectMstFacade
    * 命名空间：        GXM3.XM.Facade.Interface
    * 文 件 名：        IProjectMstFacade.cs
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
	/// ProjectMst业务组装层接口
	/// </summary>
    public partial interface IProjectMstApproveFacade : IEntFacadeBase<ProjectMstApproveModel>
    {
		//#region IProjectMstFacade 业务添加的成员

		/////// <summary>
  //      ///// 方法实例
  //      ///// </summary>
  //      ///// <returns></returns>
  //      //List<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)

  //      /// <summary>
  //      /// 保存数据
  //      /// </summary>
		///// <param name="projectMstEntity"></param>
		///// <param name="projectDtlImplPlanEntities"></param>
		///// <param name="projectDtlTextContentEntities"></param>
		///// <param name="projectDtlFundApplEntities"></param>
		///// <param name="projectDtlBudgetDtlEntities"></param>
  //      /// <returns></returns>
  //      SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities);

  //      /// <summary>
  //      /// 保存数据
  //      /// </summary>
  //      /// <param name="projectMstEntity"></param>
  //      /// <param name="projectDtlTextContentEntities"></param>
  //      /// <param name="projectDtlPurchaseDtlEntities"></param>
  //      /// <param name="projectDtlPurDtl4SOFEntities"></param>
  //      /// <param name="projectDtlPerformTargetEntities"></param>
  //      /// <param name="projectDtlFundApplEntities"></param>
  //      /// <param name="projectDtlBudgetDtlEntities"></param>
  //      /// <param name="projectDtlImplPlanEntities"></param>
  //      /// <returns></returns>
  //      SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlPurchaseDtlModel> projectDtlPurchaseDtlEntities, List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4SOFEntities, List<ProjectDtlPerformTargetModel> projectDtlPerformTargetEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities);


  //      /// <summary>
  //      /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
  //      /// </summary>
  //      /// <param name="phid"></param>
  //      void UpdateFProjStatus(long phid);

  //      #endregion
    }
}
