#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectMstFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectMstFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;
using GXM3.XM.Model.Enums;

using NG3.WorkFlow.Interfaces;
using Enterprise3.Common.Base.Criterion;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectMst业务组装处理类
	/// </summary>
    public partial class ProjectMstApproveFacade : EntFacadeBase<ProjectMstApproveModel>, IProjectMstApproveFacade
    {
		//#region 类变量及属性
		///// <summary>
  //      /// ProjectMst业务逻辑处理对象
  //      /// </summary>
		//IProjectMstRule ProjectMstRule
  //      {
  //          get
  //          {          
  //              if (CurrentRule == null)
  //                  throw new NGAppException("InitializeObjectFail");

  //              return CurrentRule as IProjectMstRule;
  //          }
  //      }
		///// <summary>
  //      /// ProjectDtlImplPlan业务逻辑处理对象
  //      /// </summary>
		//IProjectDtlImplPlanRule ProjectDtlImplPlanRule { get; set; }
		///// <summary>
  //      /// ProjectDtlTextContent业务逻辑处理对象
  //      /// </summary>
		//IProjectDtlTextContentRule ProjectDtlTextContentRule { get; set; }
		///// <summary>
  //      /// ProjectDtlFundAppl业务逻辑处理对象
  //      /// </summary>
		//IProjectDtlFundApplRule ProjectDtlFundApplRule { get; set; }
		///// <summary>
  //      /// ProjectDtlBudgetDtl业务逻辑处理对象
  //      /// </summary>
		//IProjectDtlBudgetDtlRule ProjectDtlBudgetDtlRule { get; set; }
  //      /// <summary>
  //      /// ProjectDtlPurchaseDtl业务逻辑处理对象
  //      /// </summary>
  //      IProjectDtlPurchaseDtlRule ProjectDtlPurchaseDtlRule { get; set; }
  //      /// <summary>
  //      /// ProjectDtlPurDtl4SOF业务逻辑处理对象
  //      /// </summary>
  //      IProjectDtlPurDtl4SOFRule ProjectDtlPurDtl4SOFRule { get; set; }
  //      /// <summary>
  //      /// ProjectDtlPerformTarget业务逻辑处理对象
  //      /// </summary>
  //      IProjectDtlPerformTargetRule ProjectDtlPerformTargetRule { get; set; }
  //      #endregion

  //      #region 重载方法
  //      /// <summary>
  //      /// 分页获取全部集合
  //      /// </summary>
  //      /// <param name="pageNumber">页码(从1开始)</param>
  //      /// <param name="pageSize">每页大小(最大为200)</param>
  //      /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
  //      /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
  //      /// <returns>集合</returns>
  //      public override PagedResult<ProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
  //      {
  //          PagedResult<ProjectMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

  //          #region 列表Grid代码转名称
		//	RichHelpDac helpdac = new RichHelpDac();
  //          //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
  //          //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType", "");

  //          #endregion

  //          return pageResult;
  //      }

  //      /// <summary>
  //      /// 分页获取全部集合
  //      /// </summary>
  //      /// <param name="pageNumber">页码(从1开始)</param>
  //      /// <param name="pageSize">每页大小(最大为200)</param>
  //      /// <param name="nameSqlName">命名SQL名称</param>
  //      /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
  //      /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
  //      /// <returns>集合</returns>
  //      public override PagedResult<ProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
  //      {
  //          PagedResult<ProjectMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

  //          #region 列表Grid代码转名称
		//	RichHelpDac helpdac = new RichHelpDac();
  //          //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
  //          //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
  //          helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType", "");

  //          #endregion

  //          return pageResult;
  //      }

  //      /// <summary>
  //      /// 通过id，删除数据
  //      /// </summary>
  //      /// <param name="id">单主键id值</param>
  //      public override DeletedResult Delete<TValType>(TValType id)
  //      {
		//	ProjectDtlImplPlanRule.DeleteByForeignKey(id);
		//	ProjectDtlTextContentRule.DeleteByForeignKey(id);
		//	ProjectDtlFundApplRule.DeleteByForeignKey(id);
		//	ProjectDtlBudgetDtlRule.DeleteByForeignKey(id);

  //          ProjectDtlPurchaseDtlRule.DeleteByForeignKey(id);
  //          ProjectDtlPurDtl4SOFRule.DeleteByForeignKey(id);
  //          ProjectDtlPerformTargetRule.DeleteByForeignKey(id);

  //          return base.Delete(id);
  //      }

		///// <summary>
  //      /// 通过ids，删除数据
  //      /// </summary>
  //      /// <param name="ids">单主键id集合</param>
  //      public override DeletedResult Delete<TValType>(IList<TValType> ids)
  //      {
		//	ProjectDtlImplPlanRule.DeleteByForeignKey(ids);
		//	ProjectDtlTextContentRule.DeleteByForeignKey(ids);
		//	ProjectDtlFundApplRule.DeleteByForeignKey(ids);
		//	ProjectDtlBudgetDtlRule.DeleteByForeignKey(ids);

  //          ProjectDtlPurchaseDtlRule.DeleteByForeignKey(ids);
  //          ProjectDtlPurDtl4SOFRule.DeleteByForeignKey(ids);
  //          ProjectDtlPerformTargetRule.DeleteByForeignKey(ids);

  //          return base.Delete(ids);
  //      }


  //      /// <summary>
  //      /// 更改项目状态,项目状态更改成“单位备选”时,删除当前预算，并把对应项目的状态改为“单位备选”
  //      /// </summary>
  //      /// <param name="phid"></param>
  //      public void UpdateFProjStatus(long phid)
  //      {
  //          var model = base.Find(phid);
  //          model.Data.FProjStatus = 1;
  //          model.Data.FApproveStatus = "1";  //审批状态改为未上报
  //          CurrentRule.Update<Int64>(model.Data);

  //      }

  //      #endregion

  //      #region 实现 IProjectMstFacade 业务添加的成员

  //      ///// <summary>
  //      ///// 方法实例
  //      ///// </summary>
  //      ///// <returns></returns>
  //      //public IList<ProjectMstModel> ExampleMethod<ProjectMstModel>(string param)
  //      //{
  //      //    //编写代码
  //      //}

  //      /// <summary>
  //      /// 保存数据
  //      /// </summary>
  //      /// <param name="projectMstEntity"></param>
  //      /// <param name="projectDtlImplPlanEntities"></param>
  //      /// <param name="projectDtlTextContentEntities"></param>
  //      /// <param name="projectDtlFundApplEntities"></param>
  //      /// <param name="projectDtlBudgetDtlEntities"></param>
  //      /// <returns></returns>
  //      public SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities)
  //      {
  //          SavedResult<Int64> savedResult = base.Save<Int64>(projectMstEntity);
  //          if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
  //          {
		//		if (projectDtlImplPlanEntities.Count > 0)
		//		{
		//			ProjectDtlImplPlanRule.Save(projectDtlImplPlanEntities, savedResult.KeyCodes[0]);
		//		}
		//		if (projectDtlTextContentEntities.Count > 0)
		//		{
		//			ProjectDtlTextContentRule.Save(projectDtlTextContentEntities, savedResult.KeyCodes[0]);
		//		}
		//		if (projectDtlFundApplEntities.Count > 0)
		//		{
		//			ProjectDtlFundApplRule.Save(projectDtlFundApplEntities, savedResult.KeyCodes[0]);
		//		}
		//		if (projectDtlBudgetDtlEntities.Count > 0)
		//		{
		//			ProjectDtlBudgetDtlRule.Save(projectDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
		//		}
  //          }

		//	return savedResult;
  //      }

  //      /// <summary>
  //      /// 保存数据
  //      /// </summary>
		///// <param name="projectMstEntity"></param>
		///// <param name="projectDtlTextContentEntities"></param>
		///// <param name="projectDtlPurchaseDtlEntities"></param>
		///// <param name="projectDtlPurDtl4SOFEntities"></param>
		///// <param name="projectDtlPerformTargetEntities"></param>
		///// <param name="projectDtlFundApplEntities"></param>
		///// <param name="projectDtlBudgetDtlEntities"></param>
		///// <param name="projectDtlImplPlanEntities"></param>
  //      /// <returns></returns>
  //      public SavedResult<Int64> SaveProjectMst(ProjectMstModel projectMstEntity, List<ProjectDtlTextContentModel> projectDtlTextContentEntities, List<ProjectDtlPurchaseDtlModel> projectDtlPurchaseDtlEntities, List<ProjectDtlPurDtl4SOFModel> projectDtlPurDtl4SOFEntities, List<ProjectDtlPerformTargetModel> projectDtlPerformTargetEntities, List<ProjectDtlFundApplModel> projectDtlFundApplEntities, List<ProjectDtlBudgetDtlModel> projectDtlBudgetDtlEntities, List<ProjectDtlImplPlanModel> projectDtlImplPlanEntities)
  //      {
  //          SavedResult<Int64> savedResult = base.Save<Int64>(projectMstEntity);
  //          if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
  //          {
  //              if (projectDtlTextContentEntities.Count > 0)
  //              {
  //                  ProjectDtlTextContentRule.Save(projectDtlTextContentEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlPurchaseDtlEntities!=null && projectDtlPurchaseDtlEntities.Count > 0)
  //              {
  //                  ProjectDtlPurchaseDtlRule.Save(projectDtlPurchaseDtlEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlPurDtl4SOFEntities!=null && projectDtlPurDtl4SOFEntities.Count > 0)
  //              {
  //                  ProjectDtlPurDtl4SOFRule.Save(projectDtlPurDtl4SOFEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlPerformTargetEntities!=null && projectDtlPerformTargetEntities.Count > 0)
  //              {
  //                  ProjectDtlPerformTargetRule.Save(projectDtlPerformTargetEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlFundApplEntities.Count > 0)
  //              {
  //                  ProjectDtlFundApplRule.Save(projectDtlFundApplEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlBudgetDtlEntities.Count > 0)
  //              {
  //                  ProjectDtlBudgetDtlRule.Save(projectDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
  //              }
  //              if (projectDtlImplPlanEntities.Count > 0)
  //              {
  //                  ProjectDtlImplPlanRule.Save(projectDtlImplPlanEntities, savedResult.KeyCodes[0]);
  //              }
  //          }

  //          return savedResult;
  //      }
 




  //      #endregion



    }
}

