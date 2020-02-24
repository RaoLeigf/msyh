#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade
    * 类 名 称：			QTProjectMstFacade
    * 文 件 名：			QTProjectMstFacade.cs
    * 创建时间：			2019/9/4 
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GQT3.QT.Facade.Interface;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade
{
	/// <summary>
	/// QTProjectMst业务组装处理类
	/// </summary>
    public partial class QTProjectMstFacade : EntFacadeBase<QTProjectMstModel>, IQTProjectMstFacade
    {
		#region 类变量及属性
		/// <summary>
        /// QTProjectMst业务逻辑处理对象
        /// </summary>
		IQTProjectMstRule QTProjectMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IQTProjectMstRule;
            }
        }
		/// <summary>
        /// QTProjectDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IQTProjectDtlBudgetDtlRule QTProjectDtlBudgetDtlRule { get; set; }
		/// <summary>
        /// QTProjectDtlFundAppl业务逻辑处理对象
        /// </summary>
		IQTProjectDtlFundApplRule QTProjectDtlFundApplRule { get; set; }
		/// <summary>
        /// QTProjectDtlImplPlan业务逻辑处理对象
        /// </summary>
		IQTProjectDtlImplPlanRule QTProjectDtlImplPlanRule { get; set; }
		/// <summary>
        /// QTProjectDtlPerformTarget业务逻辑处理对象
        /// </summary>
		IQTProjectDtlPerformTargetRule QTProjectDtlPerformTargetRule { get; set; }
		/// <summary>
        /// QTProjectDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
		IQTProjectDtlPurchaseDtlRule QTProjectDtlPurchaseDtlRule { get; set; }
		/// <summary>
        /// QTProjectDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
		IQTProjectDtlPurDtl4SOFRule QTProjectDtlPurDtl4SOFRule { get; set; }
		/// <summary>
        /// QTProjectDtlTextContent业务逻辑处理对象
        /// </summary>
		IQTProjectDtlTextContentRule QTProjectDtlTextContentRule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public override FindedResults<QTProjectMstModel> Find(Dictionary<string, object> dicWhere, params string[] sorts)
        {
            FindedResults<QTProjectMstModel> Result = base.Find(dicWhere, sorts);
            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            helpdac.CodeToName<QTProjectMstModel>(Result.Data, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist2","");
            helpdac.CodeToName<QTProjectMstModel>(Result.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<QTProjectMstModel>(Result.Data, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<QTProjectMstModel>(Result.Data, "FDtlstage", "FDtlstage_EXName", "gh_DtlStage", "");
            //helpdac.CodeToName<QTProjectMstModel>(Result.Data, "FAccount", "FAccount_EXName", "gh_Account", "");
            #endregion
            return Result;
        }
        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<QTProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<QTProjectMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, dic, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist2", "");
			helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FDtlstage", "FDtlstage_EXName", "gh_DtlStage", "");
            helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            //helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 分页获取全部集合
        /// </summary>
        /// <param name="pageNumber">页码(从1开始)</param>
        /// <param name="pageSize">每页大小(最大为200)</param>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public override PagedResult<QTProjectMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, bool isUseInfoRight = false, params string[] sorts)
        {
            PagedResult<QTProjectMstModel> pageResult = base.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, isUseInfoRight, sorts);

            #region 列表Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "ys_orglist2", "");
			helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FDtlstage", "FDtlstage_EXName", "gh_DtlStage", "");
            helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            //helpdac.CodeToName<QTProjectMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			QTProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);
			QTProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			QTProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);
			QTProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IQTProjectMstFacade 业务添加的成员

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="qTProjectMstEntity"></param>
		/// <param name="qTProjectDtlBudgetDtlEntities"></param>
		/// <param name="qTProjectDtlFundApplEntities"></param>
		/// <param name="qTProjectDtlImplPlanEntities"></param>
		/// <param name="qTProjectDtlPerformTargetEntities"></param>
		/// <param name="qTProjectDtlPurchaseDtlEntities"></param>
		/// <param name="qTProjectDtlPurDtl4SOFEntities"></param>
		/// <param name="qTProjectDtlTextContentEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveQTProjectMst(QTProjectMstModel qTProjectMstEntity, List<QTProjectDtlBudgetDtlModel> qTProjectDtlBudgetDtlEntities, List<QTProjectDtlFundApplModel> qTProjectDtlFundApplEntities, List<QTProjectDtlImplPlanModel> qTProjectDtlImplPlanEntities, List<QTProjectDtlPerformTargetModel> qTProjectDtlPerformTargetEntities, List<QTProjectDtlPurchaseDtlModel> qTProjectDtlPurchaseDtlEntities, List<QTProjectDtlPurDtl4SOFModel> qTProjectDtlPurDtl4SOFEntities, List<QTProjectDtlTextContentModel> qTProjectDtlTextContentEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(qTProjectMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (qTProjectDtlBudgetDtlEntities.Count > 0)
				{
					QTProjectDtlBudgetDtlRule.Save(qTProjectDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlFundApplEntities.Count > 0)
				{
					QTProjectDtlFundApplRule.Save(qTProjectDtlFundApplEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlImplPlanEntities.Count > 0)
				{
					QTProjectDtlImplPlanRule.Save(qTProjectDtlImplPlanEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlPerformTargetEntities.Count > 0)
				{
					QTProjectDtlPerformTargetRule.Save(qTProjectDtlPerformTargetEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlPurchaseDtlEntities.Count > 0)
				{
					QTProjectDtlPurchaseDtlRule.Save(qTProjectDtlPurchaseDtlEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlPurDtl4SOFEntities.Count > 0)
				{
					QTProjectDtlPurDtl4SOFRule.Save(qTProjectDtlPurDtl4SOFEntities, savedResult.KeyCodes[0]);
				}
				if (qTProjectDtlTextContentEntities.Count > 0)
				{
					QTProjectDtlTextContentRule.Save(qTProjectDtlTextContentEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }

        #endregion
    }
}

