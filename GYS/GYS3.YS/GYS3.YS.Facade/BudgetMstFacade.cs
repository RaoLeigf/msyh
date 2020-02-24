#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetMstFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        BudgetMstFacade.cs
    * 创建时间：        2018/8/30 
    * 作    者：        董泉伟    
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

using GYS3.YS.Facade.Interface;
using GYS3.YS.Rule.Interface;
using GYS3.YS.Model.Domain;
using GYS3.YS.Model.Enums;
using NG3.WorkFlow.Interfaces;

using Enterprise3.Common.Base.Criterion;
using Enterprise3.Common.Base.Helpers;
using GQT3.QT.Rule.Interface;
using GQT3.QT.Model.Domain;
using GXM3.XM.Rule.Interface;

using NG3.WorkFlow.Rule;
using Newtonsoft.Json.Linq;
using SUP.Common.Base;
using NG3.Data.Service;
using System.Data;
using GData3.Common.Utils;
using GXM3.XM.Model.Domain;
using GSP3.SP.Model.Domain;
using Enterprise3.WebApi.GYS3.YS.Model.Response;
using Enterprise3.WebApi.GYS3.YS.Model.Common;

namespace GYS3.YS.Facade
{
	/// <summary>
	/// BudgetMst业务组装处理类
	/// </summary>
    public partial class BudgetMstFacade : EntFacadeBase<BudgetMstModel>, IBudgetMstFacade, IWorkFlowPlugin
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetMst业务逻辑处理对象
        /// </summary>
		IBudgetMstRule BudgetMstRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IBudgetMstRule;
            }
        }
		/// <summary>
        /// BudgetDtlImplPlan业务逻辑处理对象
        /// </summary>
		IBudgetDtlImplPlanRule BudgetDtlImplPlanRule { get; set; }
		/// <summary>
        /// BudgetDtlTextContent业务逻辑处理对象
        /// </summary>
		IBudgetDtlTextContentRule BudgetDtlTextContentRule { get; set; }
		/// <summary>
        /// BudgetDtlFundAppl业务逻辑处理对象
        /// </summary>
		IBudgetDtlFundApplRule BudgetDtlFundApplRule { get; set; }
		/// <summary>
        /// BudgetDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IBudgetDtlBudgetDtlRule BudgetDtlBudgetDtlRule { get; set; }
        /// <summary>
        /// BudgetDtlPerformTarget业务逻辑处理对象
        /// </summary>
		IBudgetDtlPerformTargetRule BudgetDtlPerformTargetRule { get; set; }
        /// <summary>
        /// BudgetDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
		IBudgetDtlPurchaseDtlRule BudgetDtlPurchaseDtlRule { get; set; }
        /// <summary>
        /// BudgetDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
		IBudgetDtlPurDtl4SOFRule BudgetDtlPurDtl4SOFRule { get; set; }

        /// <summary>
        /// BudgetDtlPersonnelRule业务逻辑处理对象
        /// </summary>
        IBudgetDtlPersonnelRule BudgetDtlPersonnelRule { get; set; }

        /// <summary>
        /// BudgetDtlPersonNameRule业务逻辑处理对象
        /// </summary>
        IBudgetDtlPersonNameRule BudgetDtlPersonNameRule { get; set; }

        /// <summary>
        /// BudgetDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
        IJxTrackingRule JxTrackingRule { get; set; }

        IQtOrgDygxRule QtOrgDygxRule { get; set; }

        ICorrespondenceSettings2Rule CorrespondenceSettings2Rule { get; set; }

        IProjectMstRule ProjectMstRule { get; set; }

        IProjectDtlBudgetDtlRule ProjectDtlBudgetDtlRule { get; set; }

        IBudgetAccountsRule BudgetAccountsRule { get; set; }

        ICorrespondenceSettingsRule CorrespondenceSettingsRule { get; set; }

        IQTControlSetRule QTControlSetRule { get; set; }

        IExpenseMstRule ExpenseMstRule { get; set; }
        IQtAccountRule QtAccountRule { get; set; }
        #endregion

        #region 重载方法
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override FindedResult<BudgetMstModel> Find<TValType>(TValType id)
        {
            FindedResult<BudgetMstModel> Result = base.Find(id);
            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FApprover", "FApprover_EXName", "fg3_user");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType");
            //helpdac.CodeToName<BudgetMstModel>(Result.Data, "FAccount", "FAccount_EXName", "gh_Account");
            #endregion
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public override FindedResults<BudgetMstModel> Find(Dictionary<string, object> dicWhere, params string[] sorts)
        {
            FindedResults<BudgetMstModel> Result = base.Find(dicWhere, sorts);
            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist","");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            helpdac.CodeToName<BudgetMstModel>(Result.Data, "FPerformEvalType", "FPerformEvalType_EXName", "GHPerformEvalType","");
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
        public override PagedResult<BudgetMstModel> LoadWithPage(int pageNumber, int pageSize = 20, Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<BudgetMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHBudget");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
			//helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
			//helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
			helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
			helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
			helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");
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
        public PagedResult<BudgetMstModel> LoadWithPage(int pageNumber, int pageSize = 20, string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<BudgetMstModel> pageResult = base.FacadeHelper.LoadWithPage(pageNumber, pageSize, nameSqlName, dic, sorts);

            pageResult = AddNextApproveName(pageResult, "GHBudget");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FAccount", "FAccount_EXName", "gh_Account", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 获取不分页集合
        /// </summary>
        /// <param name="nameSqlName">命名SQL名称</param>
        /// <param name="dic">查询条件字典（建议使用Enterprise3.Common.Base.CreateCriteria进行创建）</param>
        /// <param name="sorts">排序({属性名 desc,属性名,属性名 asc})</param>
        /// <returns>集合</returns>
        public PagedResult<BudgetMstModel> LoadWithPageInfinity(string nameSqlName = "", Dictionary<string, object> dic = null, params string[] sorts)
        {
            PagedResult<BudgetMstModel> pageResult = base.FacadeHelper.LoadWithPageInfinity(nameSqlName, dic, false, sorts);

            pageResult = AddNextApproveName(pageResult, "GHBudget");

            #region 列表Grid代码转名称
            RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FExpenseCategory", "FExpenseCategory_EXName", "GHExpenseCategory", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FApprover", "FApprover_EXName", "fg3_user", "");
            helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "FPerformType", "FPerformType_EXName", "GHPerformEvalTargetTypeTree", "");
            #endregion

            return pageResult;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			BudgetDtlImplPlanRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlTextContentRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlFundApplRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
            BudgetDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(id);
            BudgetDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
            BudgetDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);
            JxTrackingRule.RuleHelper.DeleteByForeignKey(id);
            return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			BudgetDtlImplPlanRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlTextContentRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlFundApplRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
            BudgetDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(ids);
            BudgetDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
            BudgetDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);
            JxTrackingRule.RuleHelper.DeleteByForeignKey(ids);
            return base.Delete(ids);
        }
        #endregion

        #region 实现 IBudgetMstFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetMstModel> ExampleMethod<BudgetMstModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlImplPlanEntities"></param>
        /// <param name="budgetDtlTextContentEntities"></param>
        /// <param name="budgetDtlFundApplEntities"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <param name="budgetDtlPerformTargetEntities"></param>
        /// <param name="budgetDtlPurchaseDtlEntities"></param>
        /// <param name="budgetDtlPurDtl4SOFEntities"></param>
        /// <param name="budgetDtlPersonnels"></param>
        /// <param name="budgetDtlPersonNames"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlPersonnelModel> budgetDtlPersonnels, List<BudgetDtlPersonNameModel> budgetDtlPersonNames)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(budgetMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (budgetDtlImplPlanEntities != null && budgetDtlImplPlanEntities.Count > 0)
				{
					BudgetDtlImplPlanRule.Save(budgetDtlImplPlanEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlTextContentEntities != null && budgetDtlTextContentEntities.Count > 0)
				{
					BudgetDtlTextContentRule.Save(budgetDtlTextContentEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlFundApplEntities != null && budgetDtlFundApplEntities.Count > 0)
				{
					BudgetDtlFundApplRule.Save(budgetDtlFundApplEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlBudgetDtlEntities != null && budgetDtlBudgetDtlEntities.Count > 0)
				{
					BudgetDtlBudgetDtlRule.Save(budgetDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
				}
                if (budgetDtlPerformTargetEntities != null && budgetDtlPerformTargetEntities.Count > 0) {
                    for(var i = 0; i < budgetDtlPerformTargetEntities.Count(); i++)
                    {
                        budgetDtlPerformTargetEntities[i].MstPhId = savedResult.KeyCodes[0];
                    }
                    BudgetDtlPerformTargetRule.Save(budgetDtlPerformTargetEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlPurchaseDtlEntities != null && budgetDtlPurchaseDtlEntities.Count > 0)
                {
                    BudgetDtlPurchaseDtlRule.Save(budgetDtlPurchaseDtlEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlPurDtl4SOFEntities != null && budgetDtlPurDtl4SOFEntities.Count > 0)
                {
                    BudgetDtlPurDtl4SOFRule.Save(budgetDtlPurDtl4SOFEntities, savedResult.KeyCodes[0]);
                }
                if(budgetDtlPersonnels != null && budgetDtlPersonnels.Count > 0)
                {
                    BudgetDtlPersonnelRule.Save(budgetDtlPersonnels, savedResult.KeyCodes[0]);
                }
                if(budgetDtlPersonNames != null && budgetDtlPersonNames.Count > 0)
                {
                    BudgetDtlPersonNameRule.Save(budgetDtlPersonNames, savedResult.KeyCodes[0]);
                }
            }


            return savedResult;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="budgetMstEntity"></param>
        /// <param name="budgetDtlImplPlanEntities"></param>
        /// <param name="budgetDtlTextContentEntities"></param>
        /// <param name="budgetDtlFundApplEntities"></param>
        /// <param name="budgetDtlBudgetDtlEntities"></param>
        /// <param name="budgetDtlPerformTargetEntities"></param>
        /// <returns></returns>
        public SavedResult<Int64> SaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(budgetMstEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
                if (budgetDtlImplPlanEntities != null && budgetDtlImplPlanEntities.Count > 0)
                {
                    BudgetDtlImplPlanRule.Save(budgetDtlImplPlanEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlTextContentEntities != null && budgetDtlTextContentEntities.Count > 0)
                {
                    BudgetDtlTextContentRule.Save(budgetDtlTextContentEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlFundApplEntities != null && budgetDtlFundApplEntities.Count > 0)
                {
                    BudgetDtlFundApplRule.Save(budgetDtlFundApplEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlBudgetDtlEntities != null && budgetDtlBudgetDtlEntities.Count > 0)
                {
                    BudgetDtlBudgetDtlRule.Save(budgetDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
                }
                if (budgetDtlPerformTargetEntities != null && budgetDtlPerformTargetEntities.Count > 0)
                {
                    BudgetDtlPerformTargetRule.Save(budgetDtlPerformTargetEntities, savedResult.KeyCodes[0]);
                }
            }


            return savedResult;
        }

        /// <summary>
        /// 同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddData()
        {

            string result="";
            
            List<BudgetMstModel> BudgetMstList = new List<BudgetMstModel>();
            List<string> mstCodeList = new List<string>();

            //过滤相同代码的单据
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            new CreateCriteria(dicWhere)
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FType", "c"))
                .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));
            IList<BudgetMstModel> BudgetMstListc = BudgetMstRule.Find(dicWhere);
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2)
                .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<string>.Eq("FType", "z"))
                .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));
            IList<BudgetMstModel> BudgetMstListz = BudgetMstRule.Find(dicWhere2);

            for (var a = 0; a < BudgetMstListz.Count; a++)
            {
                if (!mstCodeList.Contains(BudgetMstListz[a].FProjCode))
                {
                    mstCodeList.Add(BudgetMstListz[a].FProjCode);
                    BudgetMstList.Add(BudgetMstListz[a]);
                }
            }
            for (var b = 0; b < BudgetMstListc.Count; b++)
            {
                if (!mstCodeList.Contains(BudgetMstListc[b].FProjCode))
                {
                    mstCodeList.Add(BudgetMstListc[b].FProjCode);
                    BudgetMstList.Add(BudgetMstListc[b]);
                }
            }

            //List<string> mstOrgList = new List<string>();
            //if (BudgetMstList.Count > 0)
            //{
            //    for (var j = 0; j < BudgetMstList.Count; j++)
            //    {
            //        if (!mstOrgList.Contains(BudgetMstList[j].FDeclarationUnit))
            //        {
            //            mstOrgList.Add(BudgetMstList[j].FDeclarationUnit);
            //        }
            //    }
            //}

            if (BudgetMstList.Count > 0)
            {
                List<string> AccountList = BudgetMstList.Where(x => !string.IsNullOrEmpty(x.FAccount)).Select(x => x.FAccount).Distinct().ToList();
                string userConn;
                string zbly_dm;
                foreach (var Account in AccountList)
                {
                    //连接串更改为从基础数据-账套中取  QtAccountRule
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                    var Accounts = QtAccountRule.Find(conndic);
                    if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                    {
                        userConn = Accounts[0].FConn;
                        zbly_dm = Accounts[0].Dm;
                        var BudgetMstList2 = BudgetMstList.FindAll(x => x.FAccount == Account);
                        List<string> valuesqlList = new List<string>();
                        List<DateTime?> DJRQList = new List<DateTime?>();
                        List<DateTime?> DT1List = new List<DateTime?>();
                        List<DateTime?> DT2List = new List<DateTime?>();
                        List<string> mstSqlList = new List<string>();
                        List<string> dtlSqlList = new List<string>();
                        int ID = BudgetMstRule.GetId(userConn);
                        for (var i = 0; i < BudgetMstList2.Count; i++)
                        {
                            if (BudgetMstList2[i].FSaveToOldG6h == 0)
                            {

                                string ZY;
                                string DWDM;
                                string DEF_STR7;
                                //Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                                //new CreateCriteria(dicWhere2)
                                //    .Add(ORMRestrictions<String>.Eq("Xmorg", BudgetMstList[i].FDeclarationUnit));
                                IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(BudgetMstList2[i].FDeclarationUnit);
                                if (OrgDygx.Count > 0)
                                {
                                    //BudgetMstList[i].FDeclarationUnit = OrgDygx[0].Oldorg;
                                    DWDM = OrgDygx[0].Oldorg;
                                }
                                else
                                {
                                    DWDM = BudgetMstList2[i].FDeclarationUnit;
                                }

                                //Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();
                                //new CreateCriteria(dicWhere3)
                                //    .Add(ORMRestrictions<String>.Eq("Xmorg", BudgetMstList[i].FBudgetDept));
                                //IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.Find(dicWhere3);
                                IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.findByXmorg(BudgetMstList2[i].FBudgetDept);
                                if (OrgDygx2.Count > 0)
                                {
                                    //BudgetMstList[i].FBudgetDept = OrgDygx2[0].Oldorg;
                                    ZY = OrgDygx2[0].Oldorg;
                                    DEF_STR7 = OrgDygx2[0].Oldorg;
                                }
                                else
                                {
                                    ZY = BudgetMstList2[i].FBudgetDept;
                                    DEF_STR7 = BudgetMstList2[i].FBudgetDept;
                                }
                                string type = BudgetMstList2[i].FType;//c/z
                                string DJH = ZY + type + BudgetMstList2[i].FProjCode + BudgetMstList2[i].FVerNo;
                                IList<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtlList = BudgetDtlBudgetDtlRule.FindByForeignKey(BudgetMstList2[i].PhId);
                                if (BudgetDtlBudgetDtlList.Count > 0)
                                {
                                    for (var j = 0; j < BudgetDtlBudgetDtlList.Count; j++)
                                    {
                                        ID += 1;

                                        /*string DJH;
                                        if (type == "c")
                                        {
                                            DJH = ZY + type + BudgetMstList[i].FProjCode + "0001";
                                        }
                                        else
                                        {
                                            DJH = ZY + type + BudgetMstList[i].FProjCode + "0002";
                                        }*/

                                        DateTime? DJRQ = BudgetMstList[i].FDateofDeclaration;
                                        DateTime? DT1 = BudgetMstList[i].FStartDate;
                                        DateTime? DT2 = BudgetMstList[i].FEndDate;
                                        //string DWDM = BudgetMstList[i].FDeclarationUnit;
                                        string YSKM_DM = BudgetDtlBudgetDtlList[j].FBudgetAccounts;
                                        string JFQD_DM = BudgetDtlBudgetDtlList[j].FSourceOfFunds;
                                        decimal PAY_JE;// = BudgetDtlBudgetDtlList[j].FAmount;
                                        if (BudgetMstList[i].FVerNo == "0002")
                                        {
                                            PAY_JE = BudgetDtlBudgetDtlList[j].FAmountAfterEdit;
                                        }
                                        else
                                        {
                                            PAY_JE = BudgetDtlBudgetDtlList[j].FAmount;
                                        }
                                        //string ZY = BudgetMstList[i].FBudgetDept;
                                        string DEF_STR1 = BudgetMstList[i].FProjCode;
                                        string DEF_STR4 = BudgetMstList[i].FExpenseCategory;
                                        int DEF_INT1 = int.Parse(BudgetMstList[i].FProjAttr);
                                        int DEF_INT2 = int.Parse(BudgetMstList[i].FDuration);
                                        string MXXM = BudgetDtlBudgetDtlList[j].FDtlCode;
                                        //string DEF_STR7 = BudgetMstList[i].FBudgetDept;
                                        string DEF_STR8 = "";
                                        if (BudgetDtlBudgetDtlList[j].FExpensesChannel != null && BudgetDtlBudgetDtlList[j].FExpensesChannel != "")
                                        {
                                            //IList<QtOrgDygxModel> OrgZCQD=new IList<QtOrgDygxModel>();
                                            IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(BudgetDtlBudgetDtlList[j].FExpensesChannel);
                                            if (OrgZCQD.Count > 0)
                                            {
                                                DEF_STR8 = OrgZCQD[0].Oldorg;
                                            }
                                            else
                                            {
                                                DEF_STR8 = BudgetDtlBudgetDtlList[j].FExpensesChannel;
                                            }
                                        }
                                        int year = int.Parse(BudgetMstList[i].FYear);
                                        int xmzt = 3;
                                        int int1;
                                        if (BudgetMstList[i].FIfPerformanceAppraisal == EnumYesNo.Yes)
                                        {
                                            int1 = 1;
                                        }
                                        else
                                        {
                                            int1 = 2;
                                        }
                                        int int2;
                                        if (BudgetMstList[i].FIfKeyEvaluation == EnumYesNo.Yes)
                                        {
                                            int2 = 1;
                                        }
                                        else
                                        {
                                            int2 = 2;
                                        }
                                        //允许预备费抵扣
                                        string ACCNO1;
                                        Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                                        new CreateCriteria(dicYSKM)
                                            .Add(ORMRestrictions<String>.Eq("KMDM", BudgetDtlBudgetDtlList[j].FBudgetAccounts));
                                        IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                                        if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                                        {
                                            ACCNO1 = "1";
                                        }
                                        else
                                        {
                                            ACCNO1 = "0";
                                        }
                                        string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                            + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                            + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                                        string dtlSql = "('jj','" + BudgetMstList[i].FProjCode + "','" + BudgetDtlBudgetDtlList[j].FDtlCode + "','" + BudgetDtlBudgetDtlList[j].FName + "','ys')";
                                        valuesqlList.Add(valuesql);
                                        DJRQList.Add(DJRQ);
                                        DT1List.Add(DT1);
                                        DT2List.Add(DT2);

                                        if (!dtlSqlList.Contains(dtlSql))
                                        {
                                            dtlSqlList.Add(dtlSql);
                                        }
                                        //result = BudgetMstRule.AddData(ID, DJH, DJRQ, DWDM, YSKM_DM, JFQD_DM, PAY_JE, ZY, DEF_STR1, DEF_STR4, DEF_INT1, DEF_INT2, MXXM, DEF_STR7, DEF_STR8, year, xmzt, int1, int2);
                                    }
                                }
                                string mstSql = "('" + BudgetMstList2[i].FProjCode + "','" + BudgetMstList2[i].FProjName + "','ys')";
                                if (!mstSqlList.Contains(mstSql))
                                {
                                    mstSqlList.Add(mstSql);
                                }

                                BudgetMstList2[i].FSaveToOldG6h = 1;
                                BudgetMstList2[i].PersistentState = PersistentState.Modified;
                            }
                        }
                        try
                        {
                            BudgetMstRule.AddData(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList, "ys",DT1List,DT2List);
                        }
                        catch (Exception e)
                        {
                            result = result + Account + ",";
                        }
                    }
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
                result = result + "同步失败";
            }
            else
            {
                base.Save<Int64>(BudgetMstList);
            }
            return result;
        }

        /// <summary>
        /// 根据预算单据主键集合同步数据到老G6H数据库
        /// </summary>
        /// <param name="phids">预算主键集合</param>
        /// <returns></returns>
        public string AddData2(List<long> phids)
        {

            string result = "";

            List<BudgetMstModel> BudgetMstList = new List<BudgetMstModel>();
            //List<string> mstCodeList = new List<string>();

            ////过滤相同代码的单据
            //Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //new CreateCriteria(dicWhere)
            //    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
            //    .Add(ORMRestrictions<string>.Eq("FType", "c"))
            //    .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));
            //IList<BudgetMstModel> BudgetMstListc = BudgetMstRule.Find(dicWhere);
            //Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            //new CreateCriteria(dicWhere2)
            //    .Add(ORMRestrictions<Int32>.Eq("FLifeCycle", 0))
            //    .Add(ORMRestrictions<string>.Eq("FType", "z"))
            //    .Add(ORMRestrictions<string>.Eq("FYear", ConfigHelper.GetString("DBG6H_Year")));
            //IList<BudgetMstModel> BudgetMstListz = BudgetMstRule.Find(dicWhere2);

            //for (var a = 0; a < BudgetMstListz.Count; a++)
            //{
            //    if (!mstCodeList.Contains(BudgetMstListz[a].FProjCode))
            //    {
            //        mstCodeList.Add(BudgetMstListz[a].FProjCode);
            //        BudgetMstList.Add(BudgetMstListz[a]);
            //    }
            //}
            //for (var b = 0; b < BudgetMstListc.Count; b++)
            //{
            //    if (!mstCodeList.Contains(BudgetMstListc[b].FProjCode))
            //    {
            //        mstCodeList.Add(BudgetMstListc[b].FProjCode);
            //        BudgetMstList.Add(BudgetMstListc[b]);
            //    }
            //}

            BudgetMstList = this.BudgetMstRule.Find(t => phids.Contains(t.PhId)).ToList();

            if (BudgetMstList != null && BudgetMstList.Count > 0)
            {
                List<string> AccountList = BudgetMstList.Where(x => !string.IsNullOrEmpty(x.FAccount)).Select(x => x.FAccount).Distinct().ToList();
                string userConn;
                string zbly_dm;
                foreach (var Account in AccountList)
                {
                    //连接串更改为从基础数据-账套中取  QtAccountRule
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                    var Accounts = QtAccountRule.Find(conndic);
                    if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                    {
                        userConn = Accounts[0].FConn;
                        zbly_dm = Accounts[0].Dm;
                        var BudgetMstList2 = BudgetMstList.FindAll(x => x.FAccount == Account);
                        List<string>[] valuesqlList = new List<string>[500];
                        List<DateTime?>[] DJRQList = new List<DateTime?>[500];
                        List<string> mstSqlList = new List<string>();
                        List<string> dtlSqlList = new List<string>();
                        List<string> mstCode = new List<string>();
                        List<string> dtlcodeList = new List<string>();
                        List<string> DJHs = new List<string>();
                        int ID = BudgetMstRule.GetId(userConn);
                        int k = 0;
                        for (var i = 0; i < BudgetMstList2.Count; i++)
                        {
                            if (BudgetMstList2[i].FSaveToOldG6h == 0)
                            {

                                string ZY;
                                string DWDM;
                                string DEF_STR7;
                                //Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                                //new CreateCriteria(dicWhere2)
                                //    .Add(ORMRestrictions<String>.Eq("Xmorg", BudgetMstList[i].FDeclarationUnit));
                                IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.findByXmorg(BudgetMstList2[i].FDeclarationUnit);
                                if (OrgDygx.Count > 0)
                                {
                                    //BudgetMstList[i].FDeclarationUnit = OrgDygx[0].Oldorg;
                                    DWDM = OrgDygx[0].Oldorg;
                                }
                                else
                                {
                                    DWDM = BudgetMstList2[i].FDeclarationUnit;
                                }

                                //Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();
                                //new CreateCriteria(dicWhere3)
                                //    .Add(ORMRestrictions<String>.Eq("Xmorg", BudgetMstList[i].FBudgetDept));
                                //IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.Find(dicWhere3);
                                IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.findByXmorg(BudgetMstList2[i].FBudgetDept);
                                if (OrgDygx2.Count > 0)
                                {
                                    //BudgetMstList[i].FBudgetDept = OrgDygx2[0].Oldorg;
                                    ZY = OrgDygx2[0].Oldorg;
                                    DEF_STR7 = OrgDygx2[0].Oldorg;
                                }
                                else
                                {
                                    ZY = BudgetMstList2[i].FBudgetDept;
                                    DEF_STR7 = BudgetMstList2[i].FBudgetDept;
                                }
                                string type = BudgetMstList2[i].FType;//c/z
                                string DJH = ZY + type + BudgetMstList2[i].FProjCode + BudgetMstList2[i].FVerNo;
                                DJHs.Add(DJH);
                                IList<BudgetDtlBudgetDtlModel> BudgetDtlBudgetDtlList = BudgetDtlBudgetDtlRule.FindByForeignKey(BudgetMstList2[i].PhId);
                                if (BudgetDtlBudgetDtlList.Count > 0)
                                {
                                    for (var j = 0; j < BudgetDtlBudgetDtlList.Count; j++)
                                    {
                                        ID += 1;

                                        /*string DJH;
                                        if (type == "c")
                                        {
                                            DJH = ZY + type + BudgetMstList[i].FProjCode + "0001";
                                        }
                                        else
                                        {
                                            DJH = ZY + type + BudgetMstList[i].FProjCode + "0002";
                                        }*/

                                        DateTime? DJRQ = BudgetMstList2[i].FDateofDeclaration;
                                        //string DWDM = BudgetMstList[i].FDeclarationUnit;
                                        string YSKM_DM = BudgetDtlBudgetDtlList[j].FBudgetAccounts;
                                        string JFQD_DM = BudgetDtlBudgetDtlList[j].FSourceOfFunds;
                                        decimal PAY_JE;// = BudgetDtlBudgetDtlList[j].FAmount;
                                        if (BudgetMstList2[i].FVerNo == "0002")
                                        {
                                            PAY_JE = BudgetDtlBudgetDtlList[j].FAmountAfterEdit;
                                        }
                                        else
                                        {
                                            PAY_JE = BudgetDtlBudgetDtlList[j].FAmount;
                                        }
                                        //string ZY = BudgetMstList[i].FBudgetDept;
                                        string DEF_STR1 = BudgetMstList2[i].FProjCode;
                                        string DEF_STR4 = BudgetMstList2[i].FExpenseCategory;
                                        int DEF_INT1 = int.Parse(BudgetMstList2[i].FProjAttr);
                                        int DEF_INT2 = int.Parse(BudgetMstList2[i].FDuration);
                                        string MXXM = BudgetDtlBudgetDtlList[j].FDtlCode;
                                        //string DEF_STR7 = BudgetMstList[i].FBudgetDept;
                                        string DEF_STR8 = "";
                                        if (BudgetDtlBudgetDtlList[j].FExpensesChannel != null && BudgetDtlBudgetDtlList[j].FExpensesChannel != "")
                                        {
                                            //IList<QtOrgDygxModel> OrgZCQD=new IList<QtOrgDygxModel>();
                                            IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(BudgetDtlBudgetDtlList[j].FExpensesChannel);
                                            if (OrgZCQD.Count > 0)
                                            {
                                                DEF_STR8 = OrgZCQD[0].Oldorg;
                                            }
                                            else
                                            {
                                                DEF_STR8 = BudgetDtlBudgetDtlList[j].FExpensesChannel;
                                            }
                                        }
                                        int year = int.Parse(BudgetMstList2[i].FYear);
                                        int xmzt = 3;
                                        int int1;
                                        if (BudgetMstList2[i].FIfPerformanceAppraisal == EnumYesNo.Yes)
                                        {
                                            int1 = 1;
                                        }
                                        else
                                        {
                                            int1 = 2;
                                        }
                                        int int2;
                                        if (BudgetMstList2[i].FIfKeyEvaluation == EnumYesNo.Yes)
                                        {
                                            int2 = 1;
                                        }
                                        else
                                        {
                                            int2 = 2;
                                        }
                                        //允许预备费抵扣
                                        string ACCNO1;
                                        Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                                        new CreateCriteria(dicYSKM)
                                            .Add(ORMRestrictions<String>.Eq("KMDM", BudgetDtlBudgetDtlList[j].FBudgetAccounts));
                                        IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                                        if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                                        {
                                            ACCNO1 = "1";
                                        }
                                        else
                                        {
                                            ACCNO1 = "0";
                                        }
                                        string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                            + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                            + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                                        string dtlSql = "('jj','" + BudgetMstList2[i].FProjCode + "','" + BudgetDtlBudgetDtlList[j].FDtlCode + "','" + BudgetDtlBudgetDtlList[j].FName + "','ys')";
                                        valuesqlList[i].Add(valuesql);
                                        DJRQList[i].Add(DJRQ);

                                        if (!dtlSqlList.Contains(dtlSql))
                                        {
                                            dtlSqlList.Add(dtlSql);
                                        }
                                        //result = BudgetMstRule.AddData(ID, DJH, DJRQ, DWDM, YSKM_DM, JFQD_DM, PAY_JE, ZY, DEF_STR1, DEF_STR4, DEF_INT1, DEF_INT2, MXXM, DEF_STR7, DEF_STR8, year, xmzt, int1, int2);
                                    }
                                }
                                string mstSql = "('" + BudgetMstList2[i].FProjCode + "','" + BudgetMstList2[i].FProjName + "','ys')";
                                if (!mstSqlList.Contains(mstSql))
                                {
                                    mstSqlList.Add(mstSql);
                                }

                                BudgetMstList2[i].FSaveToOldG6h = 1;
                                BudgetMstList2[i].PersistentState = PersistentState.Modified;
                            }
                        }
                        try
                        {
                            //BudgetMstRule.AddData(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList, "ys");
                            ProjectMstRule.ApproveAddData2(userConn, zbly_dm, valuesqlList, mstSqlList, dtlSqlList, DJRQList, "ys", mstCode, dtlcodeList, DJHs);
                        }
                        catch (Exception e)
                        {
                            result = result + Account + ",";
                        }
                    }
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
                result = result + "同步失败";
            }
            else
            {
                base.Save<Int64>(BudgetMstList);
            }
            return result;
        }


        /// <summary>
        /// 项目生成预算时同步数据到老G6H数据库
        /// </summary>
        /// <returns></returns>
        public string AddDataInSaveBudgetMst(BudgetMstModel budgetMstEntity, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities)
        {
            List<string> valuesqlList = new List<string>();
            List<DateTime?> DJRQList = new List<DateTime?>();
            List<DateTime?> DT1List = new List<DateTime?>();
            List<DateTime?> DT2List = new List<DateTime?>();
            //List<string> mstSqlList = new List<string>();
            List<string> dtlSqlList = new List<string>();
            List<string> dtlcodeList = new List<string>();
            string result = "";
            
            //连接串更改为从基础数据-账套中取  QtAccountRule
            if (!string.IsNullOrEmpty(budgetMstEntity.FAccount))
            {
                Dictionary<string, object> conndic = new Dictionary<string, object>();
                new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", budgetMstEntity.FAccount));
                var Accounts = QtAccountRule.Find(conndic);
                if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                {
                    try
                    {
                        string userConn = Accounts[0].FConn;
                        string zbly_dm = Accounts[0].Dm;
                        int ID = BudgetMstRule.GetId(userConn);
                        string DWDM;
                        string ZY;
                        string DEF_STR7;
                        //组织部门（项目库->老g6h）
                        Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2)
                            .Add(ORMRestrictions<String>.Eq("Xmorg", budgetMstEntity.FDeclarationUnit));
                        IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.Find(dicWhere2);
                        if (OrgDygx.Count > 0)
                        {
                            //budgetMstEntity.FDeclarationUnit = OrgDygx[0].Oldorg;
                            DWDM = OrgDygx[0].Oldorg;
                        }
                        else
                        {
                            DWDM = budgetMstEntity.FDeclarationUnit;
                        }
                        Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere3)
                            .Add(ORMRestrictions<String>.Eq("Xmorg", budgetMstEntity.FBudgetDept));
                        IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.Find(dicWhere3);
                        if (OrgDygx2.Count > 0)
                        {
                            //budgetMstEntity.FBudgetDept = OrgDygx2[0].Oldorg;
                            ZY = OrgDygx2[0].Oldorg;
                            DEF_STR7 = OrgDygx2[0].Oldorg;
                        }
                        else
                        {
                            ZY = budgetMstEntity.FBudgetDept;
                            DEF_STR7 = budgetMstEntity.FBudgetDept;
                        }
                        string type = budgetMstEntity.FType;//c/z
                                                            /*string DJH;
                                                            if (type == "c")
                                                            {
                                                                DJH = ZY + type + budgetMstEntity.FProjCode + "0001";
                                                            }
                                                            else
                                                            {
                                                                if (budgetMstEntity.FMidYearChange == "1")
                                                                {
                                                                    DJH = ZY + "c" + budgetMstEntity.FProjCode + "0002";
                                                                }
                                                                else
                                                                {
                                                                    DJH = ZY + type + budgetMstEntity.FProjCode + "0001";
                                                                }
                                                            }*/
                        string DJH = ZY + type + budgetMstEntity.FProjCode + budgetMstEntity.FVerNo;

                        for (var i = 0; i < budgetDtlBudgetDtlEntities.Count; i++)
                        {
                            ID += 1;
                            DateTime? DJRQ = budgetMstEntity.FDateofDeclaration;
                            DateTime? DT1 = budgetMstEntity.FStartDate;
                            DateTime? DT2 = budgetMstEntity.FEndDate;
                            //string DWDM = budgetMstEntity.FDeclarationUnit;
                            string YSKM_DM = budgetDtlBudgetDtlEntities[i].FBudgetAccounts;
                            string JFQD_DM = budgetDtlBudgetDtlEntities[i].FSourceOfFunds;
                            decimal PAY_JE;// = budgetDtlBudgetDtlEntities[i].FAmount;
                            if (budgetMstEntity.FVerNo == "0002")
                            {
                                PAY_JE = budgetDtlBudgetDtlEntities[i].FAmountAfterEdit;
                            }
                            else
                            {
                                PAY_JE = budgetDtlBudgetDtlEntities[i].FAmount;
                            }
                            //string ZY = budgetMstEntity.FBudgetDept;
                            string DEF_STR1 = budgetMstEntity.FProjCode;
                            string DEF_STR4 = budgetMstEntity.FExpenseCategory;
                            int DEF_INT1 = int.Parse(budgetMstEntity.FProjAttr);
                            int DEF_INT2 = int.Parse(budgetMstEntity.FDuration);
                            string MXXM = budgetDtlBudgetDtlEntities[i].FDtlCode;
                            //string DEF_STR7 = budgetMstEntity.FBudgetDept;
                            string DEF_STR8 = "";
                            if (budgetDtlBudgetDtlEntities[i].FExpensesChannel != null && budgetDtlBudgetDtlEntities[i].FExpensesChannel != "")
                            {
                                IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(budgetDtlBudgetDtlEntities[i].FExpensesChannel);
                                if (OrgZCQD.Count > 0)
                                {
                                    //budgetDtlBudgetDtlEntities[i].FExpensesChannel = OrgDygx[0].Oldorg;
                                    DEF_STR8 = OrgZCQD[0].Oldorg;
                                }
                                else
                                {
                                    DEF_STR8 = budgetDtlBudgetDtlEntities[i].FExpensesChannel;
                                }
                            }
                            //string DEF_STR8 = budgetDtlBudgetDtlEntities[i].FExpensesChannel;
                            int year = int.Parse(budgetMstEntity.FYear);
                            int xmzt = 3;
                            int int1;
                            if (budgetMstEntity.FIfPerformanceAppraisal == EnumYesNo.Yes)
                            {
                                int1 = 1;
                            }
                            else
                            {
                                int1 = 2;
                            }
                            int int2;
                            if (budgetMstEntity.FIfKeyEvaluation == EnumYesNo.Yes)
                            {
                                int2 = 1;
                            }
                            else
                            {
                                int2 = 2;
                            }
                            string ACCNO1;
                            Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                            new CreateCriteria(dicYSKM)
                                .Add(ORMRestrictions<String>.Eq("KMDM", budgetDtlBudgetDtlEntities[i].FBudgetAccounts));
                            IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                            if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                            {
                                ACCNO1 = "1";
                            }
                            else
                            {
                                ACCNO1 = "0";
                            }


                            string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                            + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                            + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                            string dtlSql = "('jj','" + budgetMstEntity.FProjCode + "','" + budgetDtlBudgetDtlEntities[i].FDtlCode + "','" + budgetDtlBudgetDtlEntities[i].FName + "','ys')";
                            valuesqlList.Add(valuesql);
                            DJRQList.Add(DJRQ);
                            DT1List.Add(DT1);
                            DT2List.Add(DT2);
                            if (!dtlSqlList.Contains(dtlSql))
                            {
                                dtlSqlList.Add(dtlSql);
                                dtlcodeList.Add(budgetDtlBudgetDtlEntities[i].FDtlCode);
                            }
                        }
                        string mstSql = "('" + budgetMstEntity.FProjCode + "','" + budgetMstEntity.FProjName + "','ys')";
                        //mstSqlList.Add(mstSql);

                        ProjectMstRule.ApproveAddData(userConn, zbly_dm, valuesqlList, mstSql, dtlSqlList, DJRQList, "ys", budgetMstEntity.FProjCode, dtlcodeList, DJH,DT1List,DT2List);
                        //BudgetMstRule.AddData
                    }
                    catch (Exception e)
                    {
                        result = result + budgetMstEntity.FProjCode + "同步失败";
                    }
                }
                else
                {
                    result = result + budgetMstEntity.FProjCode + "同步失败";
                }
            }
            else
            {
                result = result + budgetMstEntity.FProjCode + "同步失败";
            }
            return result;
        }

        /// <summary>
        /// 允许预备费抵扣
        /// </summary>
        /// <returns></returns>
        public string AddYBF(long id)
        {
            var result = "";
            BudgetMstModel budgetMstEntity = BudgetMstRule.Find(id);
            var FProjCode=budgetMstEntity.FProjCode;
            if (!string.IsNullOrEmpty(budgetMstEntity.FAccount))
            {
                //根据组织读取conn
                //Dictionary<string, object> conndic = new Dictionary<string, object>();
                //new CreateCriteria(conndic)
                //    .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                //    .Add(ORMRestrictions<string>.Eq("DefStr1", budgetMstEntity.FDeclarationUnit));
                //IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
                Dictionary<string, object> conndic = new Dictionary<string, object>();
                new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", budgetMstEntity.FAccount));
                var Accounts = QtAccountRule.Find(conndic);
                if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                {
                    string userConn = Accounts[0].FConn;
                    try
                    {
                        BudgetMstRule.AddYBF(userConn, FProjCode);
                        budgetMstEntity.FBillNO = "1";
                        budgetMstEntity.PersistentState = PersistentState.Modified;
                        BudgetMstRule.RuleHelper.Save<Int64>(budgetMstEntity, "");
                        result = "成功";
                    }
                    catch (Exception e)
                    {
                        result = "失败";
                    }
                }
                else
                {
                    result = "失败,请配置conn";
                }
            }
            else
            {
                result = "失败,请配置该单据的账套";
            }
            return result;
        }

        /// <summary>
        /// 获取实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="BudgetMsts"></param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> GetSJFSS(string userID, PagedResult<BudgetMstModel> BudgetMsts)
        {
            string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var CorrespondenceSettings = CorrespondenceSettingsRule.Find(dicWhereUnit);
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }
            DateTime? beforeDate=new DateTime(DateTime.Now.Year,1,1); //账务筛选时间
            var dicWheredate = new Dictionary<string, object>();
            new CreateCriteria(dicWheredate).Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZExpense"));
            var  setModels = QTControlSetRule.Find(dicWheredate);//QTControlSetModel
            if (setModels.Count > 0)
            {
                beforeDate = setModels[0].BEGINFDATE;
            }
            string userConn = "";
            string select_DM;
            string Date_Dm;
            //Dictionary<string, object> conndic = new Dictionary<string, object>();
            //new CreateCriteria(conndic)
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
            //    .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            //IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
            //if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            //{
            //    userConn = CorrespondenceSettings2s[0].DefStr2;
            //}
            List<string> AccountList = BudgetMsts.Results.ToList().Where(x=>!string.IsNullOrEmpty(x.FAccount)).Select(x => x.FAccount).Distinct().ToList();
            //判断是否做过用款计划
            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2)
                    .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                    .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstRule.Find(dicWhere2);

            DataTable dataTable = null;
            foreach (var Account in AccountList)
            {
                Dictionary<string, object> conndic = new Dictionary<string, object>();
                new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                var Accounts = QtAccountRule.Find(conndic);
                if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                {
                    userConn = Accounts[0].FConn;
                    DbHelper.Open(userConn);
                    if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        select_DM = @"SELECT zxyt, sum(nvl(j,0))-sum(nvl(d,0)) total FROM v_zw_pzhz WHERE ";
                        Date_Dm = " PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') GROUP BY zxyt";
                    }
                    else
                    {
                        select_DM = @"SELECT zxyt, sum(isnull(j,0))-sum(isnull(d,0))) total FROM v_zw_pzhz WHERE ";
                        Date_Dm = " PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "' GROUP BY zxyt";
                    }
                    dataTable = DbHelper.GetDataTable(userConn, select_DM + Date_Dm);
                    DbHelper.Close(userConn);
                    for (var i = 0; i < BudgetMsts.Results.Count; i++)
                    {
                        if (BudgetMsts.Results[i].FAccount == Account)
                        {
                            var ExpenseMstList2 = ExpenseMstList.ToList().FindAll(x => x.FProjcode == BudgetMsts.Results[i].FProjCode);
                            if (ExpenseMstList2.Count <= 0)
                            {
                                BudgetMsts.Results[i].FBillNO = BudgetMsts.Results[i].FBudgetAmount.ToString();
                            }

                            //dataTable = DbHelper.GetDataTable(userConn, select_DM + "'" + BudgetMsts.Results[i].FProjCode + "'" + Date_Dm);
                            if (dataTable.Rows.Count != 0)
                            {
                                for (var j = 0; j < dataTable.Rows.Count; j++)
                                {
                                    if (dataTable.Rows[j]["zxyt"] != null && dataTable.Rows[j]["zxyt"].ToString() == BudgetMsts.Results[i].FProjCode)
                                    {
                                        BudgetMsts.Results[i].FDeclarationDept = dataTable.Rows[j]["total"].ToString();

                                        BudgetMsts.Results[i].FBillNO = (double.Parse(BudgetMsts.Results[i].FBillNO) - double.Parse(dataTable.Rows[j]["total"].ToString())).ToString();//剩余可编报数减去账务实际发生数
                                    }
                                }
                            }

                        }

                    }
                }
            }
            return BudgetMsts;
        }

        /// <summary>
        /// 获取项目支出预算情况
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="BudgetMsts">预算对象集合</param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> GetSJFSS2(string userID, PagedResult<BudgetMstModel> BudgetMsts)
        {
            string FDeclarationUnit = "";//组织
            var dicWhereUnit = new Dictionary<string, object>();
            new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
                .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            var CorrespondenceSettings = CorrespondenceSettingsRule.Find(dicWhereUnit);
            if (CorrespondenceSettings.Count > 0)
            {
                FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            }
            DateTime? beforeDate = new DateTime(DateTime.Now.Year, 1, 1); //账务筛选时间
            var dicWheredate = new Dictionary<string, object>();
            new CreateCriteria(dicWheredate).Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZExpense"));
            var setModels = QTControlSetRule.Find(dicWheredate);//QTControlSetModel
            if (setModels.Count > 0)
            {
                beforeDate = setModels[0].BEGINFDATE;
            }
            string userConn = "";
            string select_DM;
            string Date_Dm;
            Dictionary<string, object> conndic = new Dictionary<string, object>();
            new CreateCriteria(conndic)
                .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
                .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
            if (CorrespondenceSettings2s.Count > 0 && CorrespondenceSettings2s[0].DefStr2 != null)
            {
                userConn = CorrespondenceSettings2s[0].DefStr2;
            }
            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
            {
                select_DM = @"SELECT zxyt, sum(nvl(j,0))-sum(nvl(d,0)) total FROM v_zw_pzhz WHERE ";
                Date_Dm = " PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') GROUP BY zxyt";
            }
            else
            {
                select_DM = @"SELECT zxyt, sum(isnull(j,0))-sum(isnull(d,0))) total FROM v_zw_pzhz WHERE";
                Date_Dm = " PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "' GROUP BY zxyt";
            }

            //找出用款计划的数据集合
            var dicWhere2 = new Dictionary<string, object>();
            new CreateCriteria(dicWhere2)
                //.Add(ORMRestrictions<string>.Eq("FProjcode", BudgetMsts.Results[i].FProjCode))
                .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
            IList<ExpenseMstModel> ExpenseMstList = ExpenseMstRule.Find(dicWhere2);
            IList<ExpenseMstModel> expenseByCode = new List<ExpenseMstModel>();
            DataTable dataTable = null;
            DbHelper.Open(userConn);
            dataTable = DbHelper.GetDataTable(userConn, select_DM + Date_Dm);//按zxyt把所有所有数据先查询出来，提高效率
            DbHelper.Close(userConn);
            for (var i = 0; i < BudgetMsts.Results.Count; i++)
            {
                BudgetMsts.Results[i].FSurplusAmount = decimal.Parse(BudgetMsts.Results[i].FBillNO.ToString());//给剩余可编报附上初始值
                //判断是否做过用款计划
                //var dicWhere2 = new Dictionary<string, object>();
                //new CreateCriteria(dicWhere2).Add(ORMRestrictions<string>.Eq("FProjcode", BudgetMsts.Results[i].FProjCode))
                //        .Add(ORMRestrictions<System.Int32>.Eq("FLifeCycle", 0))
                //        .Add(ORMRestrictions<System.Int32>.NotEq("FIfpurchase", 1));
                //IList<ExpenseMstModel> ExpenseMstList = ExpenseMstRule.Find(dicWhere2);
                if (ExpenseMstList != null && ExpenseMstList.Count > 0)
                {
                    expenseByCode = ExpenseMstList.ToList().FindAll(t => t.FProjcode == BudgetMsts.Results[i].FProjCode);
                }               
                if (expenseByCode == null || expenseByCode.Count <= 0)
                {
                    BudgetMsts.Results[i].FSurplusAmount = BudgetMsts.Results[i].FBudgetAmount;//剩余可编报金额
                }

                
                if (dataTable.Rows.Count != 0)
                {
                    for(var j =0; j< dataTable.Rows.Count; j++)
                    {
                        if(dataTable.Rows[j]["zxyt"] != null && dataTable.Rows[j]["zxyt"].ToString() == BudgetMsts.Results[i].FProjCode)
                        {
                            BudgetMsts.Results[i].FHappenAmount = decimal.Parse(dataTable.Rows[j]["total"].ToString());//实际已发生金额

                            BudgetMsts.Results[i].FSurplusAmount = BudgetMsts.Results[i].FSurplusAmount - decimal.Parse(dataTable.Rows[j]["total"].ToString());//剩余可编报数减去账务实际发生数
                        }
                    }
                }
            }
            
            return BudgetMsts;
        }

        /// <summary>
        /// 获取老g6h预算数据主表
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<BudgetMstModel> GetOldMstList(string userConn)
        {
            IList <BudgetMstModel> budgetMsts = null;
            
            budgetMsts = DCHelper.DataTable2List<BudgetMstModel>(BudgetMstRule.GetOldMstList(userConn));
            if (budgetMsts.Count > 0)
            {
                //得到组织对应关系
                Dictionary<string, object> dicOrgDygx = new Dictionary<string, object>();
                new CreateCriteria(dicOrgDygx)
                    .Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.Find(dicOrgDygx);
                //QtOrgDygxModel UnitDygx = new QtOrgDygxModel();
                foreach (BudgetMstModel mst in budgetMsts)
                {
                    mst.FType = mst.FProjCode.Substring(0, 1);
                    //申报组织，预算部门，申报部门转换
                    if (!string.IsNullOrEmpty(mst.FDeclarationUnit))
                    {
                        QtOrgDygxModel dygx1 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FDeclarationUnit);
                        if (dygx1 != null)
                        {
                            mst.FDeclarationUnit = dygx1.Xmorg;
                        }

                    }
                    if (!string.IsNullOrEmpty(mst.FBudgetDept))
                    {
                        QtOrgDygxModel dygx2 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FBudgetDept);
                        if (dygx2 != null)
                        {
                            mst.FBudgetDept = dygx2.Xmorg;
                        }
                    }
                    if (!string.IsNullOrEmpty(mst.FDeclarationDept))
                    {
                        QtOrgDygxModel dygx3 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FDeclarationDept);
                        if (dygx3 != null)
                        {
                            mst.FDeclarationDept = dygx3.Xmorg;
                        }
                    }
                }
            }
            
            return budgetMsts;
        }

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<BudgetDtlBudgetDtlModel> GetOldDtlList(string userConn)
        {
            return DCHelper.DataTable2List<BudgetDtlBudgetDtlModel>(BudgetMstRule.GetOldDtlList(userConn));
        }

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<BudgetDtlTextContentModel> GetOldTextList(string userConn)
        {
            return DCHelper.DataTable2List<BudgetDtlTextContentModel>(BudgetMstRule.GetOldTextList(userConn));
        }

        /// <summary>
        /// 获取老g6h预算数据主表XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<ProjectMstModel> GetOldMstListXM(string userConn)
        {
            IList<ProjectMstModel> Msts = null;

            Msts = DCHelper.DataTable2List<ProjectMstModel>(BudgetMstRule.GetOldMstList(userConn));
            if (Msts.Count > 0)
            {
                //得到组织对应关系
                Dictionary<string, object> dicOrgDygx = new Dictionary<string, object>();
                new CreateCriteria(dicOrgDygx)
                    .Add(ORMRestrictions<Int64>.NotEq("PhId", 0));
                IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.Find(dicOrgDygx);
                //QtOrgDygxModel UnitDygx = new QtOrgDygxModel();
                foreach (ProjectMstModel mst in Msts)
                {
                    mst.FType = mst.FProjCode.Substring(0, 1);
                    //申报组织，预算部门，申报部门转换
                    if (!string.IsNullOrEmpty(mst.FDeclarationUnit))
                    {
                        QtOrgDygxModel dygx1 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FDeclarationUnit);
                        if (dygx1 != null)
                        {
                            mst.FDeclarationUnit = dygx1.Xmorg;
                        }
                        
                    }
                    if (!string.IsNullOrEmpty(mst.FBudgetDept))
                    {
                        QtOrgDygxModel dygx2 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FBudgetDept);
                        if (dygx2 != null)
                        {
                            mst.FBudgetDept = dygx2.Xmorg;
                        }
                    }
                    if (!string.IsNullOrEmpty(mst.FDeclarationDept))
                    {
                        QtOrgDygxModel dygx3 = OrgDygx.ToList().Find(t => t.Oldorg == mst.FDeclarationDept);
                        if (dygx3 != null)
                        {
                            mst.FDeclarationDept = dygx3.Xmorg;
                        }
                    }
                }
            }

            return Msts;
        }

        /// <summary>
        /// 获取老g6h预算数据明细表(FQtZcgnfl存的是主单据代码FProjCode)XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<ProjectDtlBudgetDtlModel> GetOldDtlListXM(string userConn)
        {
            return DCHelper.DataTable2List<ProjectDtlBudgetDtlModel>(BudgetMstRule.GetOldDtlList(userConn));
        }

        /// <summary>
        /// 获取老g6h预算数据text表(FLTPerformGoal存的是主单据代码FProjCode)XM
        /// </summary>
        /// <param name="userConn"></param>
        /// <returns></returns>
        public IList<ProjectDtlTextContentModel> GetOldTextListXM(string userConn)
        {
            return DCHelper.DataTable2List<ProjectDtlTextContentModel>(BudgetMstRule.GetOldTextList(userConn));
        }

        #endregion

        #region 工作流接口
        /// <summary>
        /// 流程发起时调用（一般用于修改表单状态为送审中、或是维护表单已挂工作流）
        /// </summary>
        /// <param name="ec"></param>
        public void FlowStart(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            //更改状态为：审批中
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.IsPending).ToString();
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 在审批任务执行前调用，在这里判断是否允许执行审批操作（现在流程中没有判断杜绝多个审批节点执行，所以单据状态为已审批也允许再次审批）
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        ///  审批组件任务办理时调用（现在流程中没有判断杜绝多个审批节点执行，所以如果单据已审批就修改审批人、审批时间）
        /// </summary>
        /// <param name="ec"></param>
        public void Approve(WorkFlowExecutionContext ec)
        {
            //用 FlowEnd(), 在流程结束时进行操作(approve 方法 在进行审批节点后就会调用,可能存在多个审批节点)
            //long phid = Convert.ToInt64(ec.BillInfo.PK1);
            //var mst = base.Find(phid);
            ////更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            //    mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            //    mst.Data.FProjAttr = "3";
            //    mst.Data.FApproveDate = DateTime.Now;
            //    mst.Data.FApprover = base.UserID;
            //    CurrentRule.Update<Int64>(mst.Data);
            //}

            //审批最后增加待预算通过节点，故审批后就处理相关转老G6h   20190711 
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            BudgetMstModel budgetMst = base.Find(phid).Data;
            if (!string.IsNullOrEmpty(budgetMst.FAccount))
            {
                List<string> valuesqlList = new List<string>();
                List<DateTime?> DJRQList = new List<DateTime?>();
                List<DateTime?> DT1List = new List<DateTime?>();
                List<DateTime?> DT2List = new List<DateTime?>();
                //List<string> mstSqlList = new List<string>();
                List<string> dtlSqlList = new List<string>();
                List<string> dtlcodeList = new List<string>();
                string result = "";
                Dictionary<string, object> conndic = new Dictionary<string, object>();
                new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", budgetMst.FAccount));
                var Accounts = QtAccountRule.Find(conndic);
                if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                {
                    try
                    {
                        string userConn = Accounts[0].FConn;
                        string zbly_dm = Accounts[0].Dm;
                        int ID = BudgetMstRule.GetId(userConn);
                        string DWDM;
                        string ZY;
                        string DEF_STR7;
                        //组织部门（项目库->老g6h）
                        Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere2)
                            .Add(ORMRestrictions<String>.Eq("Xmorg", budgetMst.FDeclarationUnit));
                        IList<QtOrgDygxModel> OrgDygx = QtOrgDygxRule.Find(dicWhere2);
                        if (OrgDygx.Count > 0)
                        {
                            //budgetMstEntity.FDeclarationUnit = OrgDygx[0].Oldorg;
                            DWDM = OrgDygx[0].Oldorg;
                        }
                        else
                        {
                            DWDM = budgetMst.FDeclarationUnit;
                        }
                        Dictionary<string, object> dicWhere3 = new Dictionary<string, object>();
                        new CreateCriteria(dicWhere3)
                            .Add(ORMRestrictions<String>.Eq("Xmorg", budgetMst.FBudgetDept));
                        IList<QtOrgDygxModel> OrgDygx2 = QtOrgDygxRule.Find(dicWhere3);
                        if (OrgDygx2.Count > 0)
                        {
                            //budgetMstEntity.FBudgetDept = OrgDygx2[0].Oldorg;
                            ZY = OrgDygx2[0].Oldorg;
                            DEF_STR7 = OrgDygx2[0].Oldorg;
                        }
                        else
                        {
                            ZY = budgetMst.FBudgetDept;
                            DEF_STR7 = budgetMst.FBudgetDept;
                        }
                        string type = budgetMst.FType;//c/z
                        string DJH = ZY + type + budgetMst.FProjCode + budgetMst.FVerNo;

                        IList<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlList = BudgetDtlBudgetDtlRule.FindByForeignKey(budgetMst.PhId);
                        for (var i = 0; i < budgetDtlBudgetDtlList.Count; i++)
                        {
                            ID += 1;

                            DateTime? DJRQ = budgetMst.FDateofDeclaration;
                            DateTime? DT1 = budgetMst.FStartDate;
                            DateTime? DT2 = budgetMst.FEndDate;
                            //string DWDM = budgetMstEntity.FDeclarationUnit;
                            string YSKM_DM = budgetDtlBudgetDtlList[i].FBudgetAccounts;
                            string JFQD_DM = budgetDtlBudgetDtlList[i].FSourceOfFunds;
                            decimal PAY_JE;
                            if (budgetMst.FVerNo == "0002")
                            {
                                PAY_JE = budgetDtlBudgetDtlList[i].FAmountAfterEdit;
                            }
                            else
                            {
                                PAY_JE = budgetDtlBudgetDtlList[i].FAmount;
                            }
                            //string ZY = budgetMstEntity.FBudgetDept;
                            string DEF_STR1 = budgetMst.FProjCode;
                            string DEF_STR4 = budgetMst.FExpenseCategory;
                            int DEF_INT1 = int.Parse(budgetMst.FProjAttr);
                            int DEF_INT2 = int.Parse(budgetMst.FDuration);
                            string MXXM = budgetDtlBudgetDtlList[i].FDtlCode;
                            //string DEF_STR7 = budgetMstEntity.FBudgetDept;
                            string DEF_STR8 = "";
                            if (budgetDtlBudgetDtlList[i].FExpensesChannel != null && budgetDtlBudgetDtlList[i].FExpensesChannel != "")
                            {
                                IList<QtOrgDygxModel> OrgZCQD = QtOrgDygxRule.findByXmorg(budgetDtlBudgetDtlList[i].FExpensesChannel);
                                if (OrgZCQD.Count > 0)
                                {
                                    //budgetDtlBudgetDtlEntities[i].FExpensesChannel = OrgDygx[0].Oldorg;
                                    DEF_STR8 = OrgZCQD[0].Oldorg;
                                }
                                else
                                {
                                    DEF_STR8 = budgetDtlBudgetDtlList[i].FExpensesChannel;
                                }
                            }
                            //string DEF_STR8 = budgetDtlBudgetDtlEntities[i].FExpensesChannel;
                            int year = int.Parse(budgetMst.FYear);
                            int xmzt = 3;
                            int int1;
                            if (budgetMst.FIfPerformanceAppraisal == EnumYesNo.Yes)
                            {
                                int1 = 1;
                            }
                            else
                            {
                                int1 = 2;
                            }
                            int int2;
                            if (budgetMst.FIfKeyEvaluation == EnumYesNo.Yes)
                            {
                                int2 = 1;
                            }
                            else
                            {
                                int2 = 2;
                            }
                            string ACCNO1;
                            Dictionary<string, object> dicYSKM = new Dictionary<string, object>();
                            new CreateCriteria(dicYSKM)
                                .Add(ORMRestrictions<String>.Eq("KMDM", budgetDtlBudgetDtlList[i].FBudgetAccounts));
                            IList<BudgetAccountsModel> budgetAccounts = BudgetAccountsRule.Find(dicYSKM);
                            if (budgetAccounts.Count > 0 && budgetAccounts[0].HB == "1")
                            {
                                ACCNO1 = "1";
                            }
                            else
                            {
                                ACCNO1 = "0";
                            }


                            string valuesql = "(" + ID + ",'" + DJH + "','" + DWDM + "','" + YSKM_DM + "','" + JFQD_DM + "'," + PAY_JE + ",'" + ZY + "','"
                                            + DEF_STR1 + "','" + DEF_STR4 + "'," + DEF_INT1 + "," + DEF_INT2 + ",'" + MXXM + "','" + DEF_STR7 + "','" + DEF_STR8 + "',"
                                            + year + "," + xmzt + "," + int1 + "," + int2 + ",'" + ACCNO1 + "'";

                            string dtlSql = "('jj','" + budgetMst.FProjCode + "','" + budgetDtlBudgetDtlList[i].FDtlCode + "','" + budgetDtlBudgetDtlList[i].FName + "','ys')";
                            valuesqlList.Add(valuesql);
                            DJRQList.Add(DJRQ);
                            DT1List.Add(DT1);
                            DT2List.Add(DT2);
                            if (!dtlSqlList.Contains(dtlSql))
                            {
                                dtlSqlList.Add(dtlSql);
                                dtlcodeList.Add(budgetDtlBudgetDtlList[i].FDtlCode);
                            }
                        }
                        string mstSql = "('" + budgetMst.FProjCode + "','" + budgetMst.FProjName + "','ys')";

                        ProjectMstRule.ApproveAddData(userConn, zbly_dm, valuesqlList, mstSql, dtlSqlList, DJRQList, "ys", budgetMst.FProjCode, dtlcodeList, DJH,DT1List,DT2List);

                    }
                    catch (Exception e)
                    {
                        //result = result + budgetMst.FDeclarationUnit + "同步失败";
                    }
                }
            }
            if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            {
                mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
                //mst.Data.FProjAttr = "3";
                mst.Data.FApproveDate = DateTime.Now;
                mst.Data.FApprover = base.UserID;
                CurrentRule.Update<Int64>(mst.Data);
            }

        }

        /// <summary>
        /// 审批节点回退前判断是否允许取消审批
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public ApproveValidResult CheckCancelApproveValid(WorkFlowExecutionContext ec)
        {
            return NG3.WorkFlow.Interfaces.ApproveValidResult.Create(ApproveValidType.Yes, string.Empty);
        }

        /// <summary>
        /// 审批节点回退时调用进行单据取消审批操作
        /// </summary>
        /// <param name="ec"></param>
        public void CancelApprove(WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 流程被终止时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowAbort(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);
            //更新状态为待上报
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.ToBeRepored).ToString();
            CurrentRule.Update<Int64>(mst.Data);
        }

        /// <summary>
        /// 流程结束时调用
        /// </summary>
        /// <param name="ec"></param>
        public void FlowEnd(WorkFlowExecutionContext ec)
        {
            long phid = Convert.ToInt64(ec.BillInfo.PK1);
            var mst = base.Find(phid);

            
            //更新状态为已审批
            //if (mst.Data.FApproveStatus != Convert.ToInt32(EnumApproveStatus.Approved).ToString())
            //{
            mst.Data.FApproveStatus = Convert.ToInt32(EnumApproveStatus.Approved).ToString();
            mst.Data.FProjStatus = 8; //预算只有年中调整审批，故审批完时改为状态8，即 调整项目执行
            mst.Data.FApproveDate = DateTime.Now;
            mst.Data.FApprover = base.UserID;
            CurrentRule.Update<Int64>(mst.Data);
            //}
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 新增、编辑\审核类组件任务执行时调用,方法参数中包含组件id
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="ec"></param>
        public void EditUserTaskComplete(string compId, WorkFlowExecutionContext ec)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据转pdf所需的套打模块、及数据，用于APP端
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public BizToPdfEntity GetBizToPdfEntity(WorkFlowExecutionContext executionContext)
        {
            return null;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 获取单据附用（用于App端）
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<BizAttachment> GetBizAttachment(WorkFlowExecutionContext executionContext)
        {
            return new List<BizAttachment>();
        }

        /// <summary>
        /// app办理时如果单据有修改则调用该方法判断是否允许保存修改
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public ApproveValidResult CheckBizSaveByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return ApproveValidResult.DefaultValue;
        }

        /// <summary>
        /// app端办理时如果修改了单据内容则调用该方法进行单据保存。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public bool SaveBizDataByMobileApp(WorkFlowExecutionContext executionContext, Dictionary<string, string> jsonData)
        {
            return true;
            //throw new NotImplementedException();
        }
        #endregion

        #region 金格控件标签取值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arcID"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetKingGridTagRelateData(string type, long arcID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere = new Dictionary<string, object>();
           // new CreateCriteria(dicWhere).Add(ORMRestrictions<string>.Eq("Dylx", "02"));
            Dictionary<string, object> dicWhere1 = new Dictionary<string, object>();
            Dictionary<string, object> dicWhere2 = new Dictionary<string, object>();
            //new CreateCriteria(dicWhere1).Add(ORMRestrictions<string>.Eq("FDeclarationUnit", base.OrgID.ToString()));
            var KingGridTag = BudgetMstRule.RuleHelper.LoadWithPageInfinity("GYS3.YS.FindKingGridTag", dicWhere);
            var KingGridTag_YSSMS = KingGridTag.Results.Where(x => x.FProjAttr == "1").ToList(); //预算说明书标签
            var KingGridTag_TZSMS = KingGridTag.Results.Where(x => x.FProjAttr == "2").ToList();//调整说明书标签

            var resultYSKMCount = BudgetMstRule.RuleHelper.LoadWithPageInfinity("GYS3.YS.FindYSKM1", dicWhere1);
            var resultYSKM = resultYSKMCount.Results.Where(x => x.FDeclarationUnit == type).ToList();

            var resultTzYSKMCount = BudgetMstRule.RuleHelper.LoadWithPageInfinity("GYS3.YS.FindYSKMTZ", dicWhere1); //调整说明书取数
            var resultTzYSKM = resultTzYSKMCount.Results.Where(x => x.FDeclarationUnit == type).ToList();

            decimal sumSR = 0;//收入合计
            decimal sumZC = 0; //支出合计

            foreach (var item in resultYSKM)
            {
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "4")
                {
                    sumSR += item.FProjAmount;
                }
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "5")
                {
                    sumZC += item.FProjAmount;
                }
            }

            dic.Add("d_4_amout", String.Format("{0:N2}", sumSR) + "万元"); //收入合计
            dic.Add("d_5_amout", String.Format("{0:N2}", sumZC) + "万元" ); //支出合计
            dic.Add("d_surplus", String.Format("{0:N2}", (sumSR - sumZC)) + "万元");  //结余
            dic.Add("d_year", DateTime.Now.Year.ToString()); //年度


            decimal sumTzSR = 0;//调整收入合计
            decimal sumTzZC = 0; //调整支出合计
            decimal sumTzAddSR = 0;//调增收入合计
            decimal sumTzAddZC = 0; //调增支出合计
            decimal sumTzCutSR = 0;//调减收入合计
            decimal sumTzCutZC = 0; //调减支出合计

            foreach (var item in resultTzYSKM)
            {
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "4" && item.FProjAmount >= 0)
                {
                    sumTzAddSR += item.FProjAmount;
                }
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "5" && item.FProjAmount >= 0)
                {
                    sumTzAddZC += item.FProjAmount;
                }
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "4" && item.FProjAmount < 0)
                {
                    sumTzCutSR += item.FProjAmount;
                }
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 1) == "5" && item.FProjAmount < 0)
                {
                    sumTzCutZC += item.FProjAmount;
                }
            }
            sumTzSR = sumSR + sumTzAddSR + sumTzCutSR;
            sumTzZC = sumZC + sumTzAddZC + sumTzCutZC;
            dic.Add("d_4_add_amout", String.Format("{0:N2}", sumTzAddSR) + "万元");//收入预算总调增
            dic.Add("d_4_cut_amout", String.Format("{0:N2}", System.Math.Abs( sumTzCutSR)) + "万元");//收入预算总调减
            dic.Add("d_5_add_amout", String.Format("{0:N2}", sumTzAddZC) + "万元");//支出预算总调增
            dic.Add("d_5_cut_amout", String.Format("{0:N2}", System.Math.Abs(sumTzCutZC)) + "万元");//支出预算总调减
            dic.Add("d_4_tz_amout", String.Format("{0:N2}", sumTzSR ) + "万元"); //调整后收入合计
            dic.Add("d_5_tz_amout", String.Format("{0:N2}", sumTzZC) + "万元"); //调整后支出合计
            dic.Add("d_tz_surplus", String.Format("{0:N2}", (sumTzSR - sumTzZC)) + "万元");  //调整后结余



            if (resultYSKM.Count > 0)
            {
                dic.Add("d_unit", resultYSKM[0].FDeclarationUnit_EXName);  //单位
            }
            decimal FAmount = 0;
            string FData = "";
            var value = new List<BudgetMstModel>() ;
            foreach (var item in KingGridTag_YSSMS)
            {
                switch (item.FProjCode)  //金格标签代码
                {
                    case "d_501_amout":   //501职工活动支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "501");
                        dic.Add("d_501_amout_Percent", GetYskmPercent(FAmount, sumZC));  //501职工活动支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元"; 
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_amout":               //50101职工活动支出_ys
                        //value = resultYSKM.Results.Where(x => x.FProjCode == "50101").ToList();  // x.FProjCode 暂时为预算科目代码
                        //foreach (var km in value)
                        //{
                        //    FAmount += km.FProjAmount;   //FProjAmount 暂时为项目金额
                        //}
                        //FData = String.Format("{0:N2}", FAmount) + "万元";
                        FData = GetYskmAmount(resultYSKM, "50101");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_data":               //50101职工活动支出_项目_ys
                        //value = resultYSKM.Results.Where(x => x.FProjCode == "50101").ToList();  // x.FProjCode 暂时为预算科目代码
                        //foreach (var km in value)
                        //{
                        //    FData += km.FProjName + ",";   //FProjName 暂时为预算科目内容
                        //}
                        //if (FData.Length > 0)
                        //{
                        //    FData = FData.Substring(0, FData.Length - 1);
                        //}
                        FData = GetYskmData(resultYSKM, "50101");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_amout":               //50102文体活动费_ys
                        FData = GetYskmAmount(resultYSKM, "50102");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_data":               //50102文体活动费_项目_ys
                        FData = GetYskmData(resultYSKM, "50102");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_amout":               //50103宣传活动费_ys
                        FData = GetYskmAmount(resultYSKM, "50103");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_data":               //50103宣传活动费_项目_ys
                        FData = GetYskmData(resultYSKM, "50103");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_amout":               //50104其他活动支出_ys
                        FData = GetYskmAmount(resultYSKM, "50104");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_data":               //50104其他活动支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50104");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_502_amout":               //502维权支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "502");
                        dic.Add("d_502_amout_Percent", GetYskmPercent(FAmount, sumZC));  //502维权支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    
                    case "d_50201_amout":               //50201劳动关系协调费_ys
                        FData = GetYskmAmount(resultYSKM, "50201");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50201_data":               //50201劳动关系协调费_项目_ys
                        FData = GetYskmData(resultYSKM, "50201");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_amout":               //50202劳动保护费_ys
                        FData = GetYskmAmount(resultYSKM, "50202");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_data":               //50202劳动保护费_项目_ys
                        FData = GetYskmData(resultYSKM, "50202");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_amout":               //50203法律援助费_ys
                        FData = GetYskmAmount(resultYSKM, "50203");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_data":               //50203法律援助费_项目_ys
                        FData = GetYskmData(resultYSKM, "50203");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_amout":               //50204困难职工帮扶费_ys
                        FData = GetYskmAmount(resultYSKM, "50204");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_data":               //50204困难职工帮扶费_项目_ys
                        FData = GetYskmData(resultYSKM, "50204");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_amout":               //50205送温暖费_ys
                        FData = GetYskmAmount(resultYSKM, "50205");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_data":               //50205送温暖费_项目_ys
                        FData = GetYskmData(resultYSKM, "50205");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_amout":               //50206其他维权支出_ys
                        FData = GetYskmAmount(resultYSKM, "50206");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_data":               //50206其他维权支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50206");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_503_amout":               //503业务支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "503");
                        dic.Add("d_503_amout_Percent", GetYskmPercent(FAmount, sumZC));  //503业务支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_amout":               //50301培训费_ys
                        FData = GetYskmAmount(resultYSKM, "50301");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_data":               //50301培训费_项目_ys
                        FData = GetYskmData(resultYSKM, "50301");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_amout":               //50302会议费_ys
                        FData = GetYskmAmount(resultYSKM, "50302");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_data":               //50302会议费_项目_ys
                        FData = GetYskmData(resultYSKM, "50302");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_amout":               //50303外事费_ys
                        FData = GetYskmAmount(resultYSKM, "50303");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_data":               //50303外事费_项目_ys
                        FData = GetYskmData(resultYSKM, "50303");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_amout":               //50304专项业务费_ys
                        FData = GetYskmAmount(resultYSKM, "50304");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_data":               //50304专项业务费_项目_ys
                        FData = GetYskmData(resultYSKM, "50304");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_amout":               //50305其他业务支出_ys
                        FData = GetYskmAmount(resultYSKM, "50305");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_data":               //50305其他业务支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50305");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_504_amout":               //504行政支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "504");
                        dic.Add("d_504_amout_Percent", GetYskmPercent(FAmount, sumZC));  //504行政支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50401_amout":               //50401工资福利支出_ys
                        FData = GetYskmAmount(resultYSKM, "50401");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50401_data":               //50401工资福利支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50401");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_amout":               //50402商品和服务支出_ys
                        FData = GetYskmAmount(resultYSKM, "50402");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_data":               //50402商品和服务支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50402");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_amout":               //50403对个人和家庭的补助支出_ys
                        FData = GetYskmAmount(resultYSKM, "50403");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_data":               //50403对个人和家庭的补助支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50403");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_amout":               //50404其他行政支出_ys
                        FData = GetYskmAmount(resultYSKM, "50404");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_data":               //50404其他行政支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50404");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_505_amout":               //505资本性支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "505");
                        dic.Add("d_505_amout_Percent", GetYskmPercent(FAmount, sumZC));  //505资本性支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    
                    case "d_50501_amout":               //50501房屋建筑物购建_ys
                        FData = GetYskmAmount(resultYSKM, "50501");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50501_data":               //50501房屋建筑物购建_项目_ys
                        FData = GetYskmData(resultYSKM, "50501");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_amout":               //50502办公设备购置_ys
                        FData = GetYskmAmount(resultYSKM, "50502");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_data":               //50502办公设备购置_项目_ys
                        FData = GetYskmData(resultYSKM, "50502");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_amout":               //50503专用设备购置_ys
                        FData = GetYskmAmount(resultYSKM, "50503");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_data":               //50503专用设备购置_项目_ys
                        FData = GetYskmData(resultYSKM, "50503");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_amout":               //50504交通工具购置_ys
                        FData = GetYskmAmount(resultYSKM, "50504");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_data":               //50504交通工具购置_项目_ys
                        FData = GetYskmData(resultYSKM, "50504");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_amout":               //50505大型修缮_ys
                        FData = GetYskmAmount(resultYSKM, "50505");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_data":               //50505大型修缮_项目_ys
                        FData = GetYskmData(resultYSKM, "50505");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_amout":               //50506信息网络购建_ys
                        FData = GetYskmAmount(resultYSKM, "50506");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_data":               //50506信息网络购建_项目_ys
                        FData = GetYskmData(resultYSKM, "50506");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_amout":               //50507其他资本性支出_ys
                        FData = GetYskmAmount(resultYSKM, "50507");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_data":               //50507其他资本性支出_项目_ys
                        FData = GetYskmData(resultYSKM, "50507");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_506_amout":               //506补助下级支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "506");
                        dic.Add("d_506_amout_Percent", GetYskmPercent(FAmount, sumZC));  //506补助下级支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                  
                    case "d_50601_amout":               //50601回拨补助_ys
                        FData = GetYskmAmount(resultYSKM, "50601");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50601_data":               //50601回拨补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50601");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_amout":               //50602专项补助_ys
                        FData = GetYskmAmount(resultYSKM, "50602");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_data":               //50602专项补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50602");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_amout":               //50603超收补助_ys
                        FData = GetYskmAmount(resultYSKM, "50603");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_data":               //50603超收补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50603");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_amout":               //50604帮扶补助_ys
                        FData = GetYskmAmount(resultYSKM, "50604");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_data":               //50604帮扶补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50604");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_amout":               //50605送温暖补助_ys
                        FData = GetYskmAmount(resultYSKM, "50605");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_data":               //50605送温暖补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50605");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_amout":               //50606救灾补助_ys
                        FData = GetYskmAmount(resultYSKM, "50606");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_data":               //50606救灾补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50606");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_amout":               //50607其他补助_ys
                        FData = GetYskmAmount(resultYSKM, "50607");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_data":               //50607其他补助_项目_ys
                        FData = GetYskmData(resultYSKM, "50607");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_amout":               //507事业支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "507");
                        dic.Add("d_507_amout_Percent", GetYskmPercent(FAmount, sumZC));  //507事业支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_data":               //507事业支出_项目_ys
                        FData = GetYskmData(resultYSKM, "507");
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_508_amout":               //508其他支出_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "508");
                        dic.Add("d_508_amout_Percent", GetYskmPercent(FAmount, sumZC));  //508其他支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_508_data":               //508其他支出_项目_ys
                        FData = GetYskmData(resultYSKM, "508");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_amout":               //509预备费_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "509");
                        dic.Add("d_509_amout_Percent", GetYskmPercent(FAmount, sumZC));  //509预备费_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_data":               //509预备费_项目_ys
                        FData = GetYskmData(resultYSKM, "509");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_401_amout":               //401会费收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "401");
                        dic.Add("d_401_amout_Percent", GetYskmPercent(FAmount, sumSR));  //401会费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_402_amout":               //402拨缴经费收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "402");
                        dic.Add("d_402_amout_Percent", GetYskmPercent(FAmount, sumSR));  //402拨缴经费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_403_amout":               //403上级补助收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "403");
                        dic.Add("d_403_amout_Percent", GetYskmPercent(FAmount, sumSR));  //403上级补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40301_amout":               //40301回拨补助_ys
                        FData = GetYskmAmount(resultYSKM, "40301");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40302_amout":               //40302专项补助_ys
                        FData = GetYskmAmount(resultYSKM, "40302");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40303_amout":               //40303超收补助_ys
                        FData = GetYskmAmount(resultYSKM, "40303");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40304_amout":               //40304帮扶补助_ys
                        FData = GetYskmAmount(resultYSKM, "40304");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40305_amout":               //40305送温暖补助_ys
                        FData = GetYskmAmount(resultYSKM, "40305");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40306_amout":               //40306救灾补助_ys
                        FData = GetYskmAmount(resultYSKM, "40306");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40307_amout":               //40307其他补助_ys
                        FData = GetYskmAmount(resultYSKM, "40307");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_404_amout":               //404政府补助收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "404");
                        dic.Add("d_404_amout_Percent", GetYskmPercent(FAmount, sumSR));  //404政府补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_405_amout":               //405行政补助收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "405");
                        dic.Add("d_405_amout_Percent", GetYskmPercent(FAmount, sumSR));  //405行政补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_406_amout":               //406事业收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "406");
                        dic.Add("d_406_amout_Percent", GetYskmPercent(FAmount, sumSR));  //406事业收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_407_amout":               //407投资收益_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "407");
                        dic.Add("d_407_amout_Percent", GetYskmPercent(FAmount, sumSR));  //407投资收益__ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_408_amout":               //408其他收入_ys
                        FAmount = GetSumYskmAmount(resultYSKM, "408");
                        dic.Add("d_408_amout_Percent", GetYskmPercent(FAmount, sumSR));  //408其他收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    default:
                        break;
                }
            }


            //调整
            foreach (var item in KingGridTag_TZSMS)
            {
                switch (item.FProjCode)  //金格标签代码
                {
                    case "d_501_add_amout":   //501职工活动支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "501","add");
                        dic.Add("d_501_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //501职工活动支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_add_amout":               //50101职工活动支出_ys
                
                        FData = GetYskmAmount(resultTzYSKM, "50101","add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_add_data":               //50101职工活动支出_项目_ys
              
                        FData = GetYskmData(resultTzYSKM, "50101", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_add_amout":               //50102文体活动费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50102", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_add_data":               //50102文体活动费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50102", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_add_amout":               //50103宣传活动费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50103", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_add_data":               //50103宣传活动费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50103", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_add_amout":               //50104其他活动支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50104", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_add_data":               //50104其他活动支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50104", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_502_add_amout":               //502维权支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "502", "add");
                        dic.Add("d_502_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //502维权支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50201_add_amout":               //50201劳动关系协调费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50201", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50201_add_data":               //50201劳动关系协调费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50201", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_add_amout":               //50202劳动保护费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50202", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_add_data":               //50202劳动保护费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50202", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_add_amout":               //50203法律援助费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50203", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_add_data":               //50203法律援助费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50203", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_add_amout":               //50204困难职工帮扶费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50204", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_add_data":               //50204困难职工帮扶费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50204", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_add_amout":               //50205送温暖费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50205", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_add_data":               //50205送温暖费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50205", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_add_amout":               //50206其他维权支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50206", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_add_data":               //50206其他维权支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50206", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_503_add_amout":               //503业务支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "503", "add");
                        dic.Add("d_503_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //503业务支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_add_amout":               //50301培训费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50301", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_add_data":               //50301培训费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50301", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_add_amout":               //50302会议费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50302", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_add_data":               //50302会议费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50302", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_add_amout":               //50303外事费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50303", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_add_data":               //50303外事费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50303", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_add_amout":               //50304专项业务费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50304", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_add_data":               //50304专项业务费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50304", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_add_amout":               //50305其他业务支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50305");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_add_data":               //50305其他业务支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50305", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_504_add_amout":               //504行政支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "504", "add");
                        dic.Add("d_504_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //504行政支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50401_add_amout":               //50401工资福利支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50401", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50401_add_data":               //50401工资福利支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50401", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_add_amout":               //50402商品和服务支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50402", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_add_data":               //50402商品和服务支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50402", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_add_amout":               //50403对个人和家庭的补助支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50403", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_add_data":               //50403对个人和家庭的补助支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50403", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_add_amout":               //50404其他行政支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50404", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_add_data":               //50404其他行政支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50404", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_505_add_amout":               //505资本性支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "505", "add");
                        dic.Add("d_505_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //505资本性支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50501_add_amout":               //50501房屋建筑物购建_ys
                        FData = GetYskmAmount(resultTzYSKM, "50501", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50501_add_data":               //50501房屋建筑物购建_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50501", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_add_amout":               //50502办公设备购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50502", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_add_data":               //50502办公设备购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50502", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_add_amout":               //50503专用设备购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50503", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_add_data":               //50503专用设备购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50503", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_add_amout":               //50504交通工具购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50504", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_add_data":               //50504交通工具购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50504", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_add_amout":               //50505大型修缮_ys
                        FData = GetYskmAmount(resultTzYSKM, "50505", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_add_data":               //50505大型修缮_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50505", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_add_amout":               //50506信息网络购建_ys
                        FData = GetYskmAmount(resultTzYSKM, "50506", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_add_data":               //50506信息网络购建_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50506", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_add_amout":               //50507其他资本性支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50507", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_add_data":               //50507其他资本性支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50507", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_506_add_amout":               //506补助下级支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "506", "add");
                        dic.Add("d_506_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //506补助下级支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50601_add_amout":               //50601回拨补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50601", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50601_add_data":               //50601回拨补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50601", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_add_amout":               //50602专项补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50602", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_add_data":               //50602专项补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50602", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_add_amout":               //50603超收补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50603", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_add_data":               //50603超收补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50603", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_add_amout":               //50604帮扶补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50604", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_add_data":               //50604帮扶补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50604", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_add_amout":               //50605送温暖补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50605", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_add_data":               //50605送温暖补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50605", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_add_amout":               //50606救灾补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50606", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_add_data":               //50606救灾补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50606", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_add_amout":               //50607其他补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50607", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_add_data":               //50607其他补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50607", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_add_amout":               //507事业支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "507", "add");
                        dic.Add("d_507_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //507事业支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_add_data":               //507事业支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "507", "add");
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_508_add_amout":               //508其他支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "508", "add");
                        dic.Add("d_508_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //508其他支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_508_add_data":               //508其他支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "508", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_add_amout":               //509预备费_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "509", "add");
                        dic.Add("d_509_add_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //509预备费_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_add_data":               //509预备费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "509", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_501_cut_amout":   //501职工活动支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "501", "cut");
                        dic.Add("d_501_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //501职工活动支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_cut_amout":               //50101职工活动支出_ys

                        FData = GetYskmAmount(resultTzYSKM, "50101", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50101_cut_data":               //50101职工活动支出_项目_ys

                        FData = GetYskmData(resultTzYSKM, "50101", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_cut_amout":               //50102文体活动费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50102", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50102_cut_data":               //50102文体活动费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50102", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_cut_amout":               //50103宣传活动费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50103", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50103_cut_data":               //50103宣传活动费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50103", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_cut_amout":               //50104其他活动支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50104", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50104_cut_data":               //50104其他活动支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50104", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_502_cut_amout":               //502维权支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "502", "cut");
                        dic.Add("d_502_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //502维权支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50201_cut_amout":               //50201劳动关系协调费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50201", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50201_cut_data":               //50201劳动关系协调费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50201", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_cut_amout":               //50202劳动保护费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50202", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50202_cut_data":               //50202劳动保护费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50202", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_cut_amout":               //50203法律援助费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50203", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50203_cut_data":               //50203法律援助费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50203", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_cut_amout":               //50204困难职工帮扶费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50204", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50204_cut_data":               //50204困难职工帮扶费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50204", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_cut_amout":               //50205送温暖费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50205", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50205_cut_data":               //50205送温暖费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50205", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_cut_amout":               //50206其他维权支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50206", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50206_cut_data":               //50206其他维权支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50206", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_503_cut_amout":               //503业务支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "503", "cut");
                        dic.Add("d_503_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //503业务支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_cut_amout":               //50301培训费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50301", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50301_cut_data":               //50301培训费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50301", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_cut_amout":               //50302会议费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50302", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50302_cut_data":               //50302会议费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50302", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_cut_amout":               //50303外事费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50303", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50303_cut_data":               //50303外事费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50303", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_cut_amout":               //50304专项业务费_ys
                        FData = GetYskmAmount(resultTzYSKM, "50304", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50304_cut_data":               //50304专项业务费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50304", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_cut_amout":               //50305其他业务支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50305", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50305_cut_data":               //50305其他业务支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50305", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_504_cut_amout":               //504行政支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "504", "cut");
                        dic.Add("d_504_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //504行政支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50401_cut_amout":               //50401工资福利支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50401", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50401_cut_data":               //50401工资福利支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50401", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_cut_amout":               //50402商品和服务支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50402", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50402_cut_data":               //50402商品和服务支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50402", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_cut_amout":               //50403对个人和家庭的补助支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50403", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50403_cut_data":               //50403对个人和家庭的补助支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50403", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_cut_amout":               //50404其他行政支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50404", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50404_cut_data":               //50404其他行政支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50404", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_505_cut_amout":               //505资本性支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "505", "cut");
                        dic.Add("d_505_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //505资本性支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50501_cut_amout":               //50501房屋建筑物购建_ys
                        FData = GetYskmAmount(resultTzYSKM, "50501", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50501_cut_data":               //50501房屋建筑物购建_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50501", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_cut_amout":               //50502办公设备购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50502", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50502_cut_data":               //50502办公设备购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50502", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_cut_amout":               //50503专用设备购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50503", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50503_cut_data":               //50503专用设备购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50503", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_cut_amout":               //50504交通工具购置_ys
                        FData = GetYskmAmount(resultTzYSKM, "50504", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50504_cut_data":               //50504交通工具购置_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50504", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_cut_amout":               //50505大型修缮_ys
                        FData = GetYskmAmount(resultTzYSKM, "50505", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50505_cut_data":               //50505大型修缮_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50505", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_cut_amout":               //50506信息网络购建_ys
                        FData = GetYskmAmount(resultTzYSKM, "50506", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50506_cut_data":               //50506信息网络购建_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50506", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_cut_amout":               //50507其他资本性支出_ys
                        FData = GetYskmAmount(resultTzYSKM, "50507", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50507_cut_data":               //50507其他资本性支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50507", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_506_cut_amout":               //506补助下级支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "506", "cut");
                        dic.Add("d_506_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //506补助下级支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_50601_cut_amout":               //50601回拨补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50601", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50601_cut_data":               //50601回拨补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50601", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_cut_amout":               //50602专项补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50602", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50602_cut_data":               //50602专项补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50602", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_cut_amout":               //50603超收补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50603", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50603_cut_data":               //50603超收补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50603", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_cut_amout":               //50604帮扶补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50604", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50604_cut_data":               //50604帮扶补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50604", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_cut_amout":               //50605送温暖补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50605", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50605_cut_data":               //50605送温暖补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50605", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_cut_amout":               //50606救灾补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50606", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50606_cut_data":               //50606救灾补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50606", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_cut_amout":               //50607其他补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "50607", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_50607_cut_data":               //50607其他补助_项目_ys
                        FData = GetYskmData(resultTzYSKM, "50607", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_cut_amout":               //507事业支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "507", "cut");
                        dic.Add("d_507_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //507事业支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_507_cut_data":               //507事业支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "507", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;

                    case "d_508_cut_amout":               //508其他支出_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "508", "cut");
                        dic.Add("d_508_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //508其他支出_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_508_cut_data":               //508其他支出_项目_ys
                        FData = GetYskmData(resultTzYSKM, "508", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_cut_amout":               //509预备费_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "509", "cut");
                        dic.Add("d_509_cut_amout_Percent", GetYskmPercent(FAmount, sumTzZC));  //509预备费_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_509_cut_data":               //509预备费_项目_ys
                        FData = GetYskmData(resultTzYSKM, "509", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_401_add_amout":               //401会费收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "401", "add");
                        dic.Add("d_401_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //401会费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_402_add_amout":               //402拨缴经费收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "402", "add");
                        dic.Add("d_402_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //402拨缴经费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_403_add_amout":               //403上级补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "403", "add");
                        dic.Add("d_403_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //403上级补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40301_add_amout":               //40301回拨补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40301", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40302_add_amout":               //40302专项补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40302", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40303_add_amout":               //40303超收补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40303", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40304_add_amout":               //40304帮扶补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40304", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40305_add_amout":               //40305送温暖补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40305", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40306_add_amout":               //40306救灾补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40306", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40307_add_amout":               //40307其他补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40307", "add");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_404_add_amout":               //404政府补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "404", "add");
                        dic.Add("d_404_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //404政府补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_405_add_amout":               //405行政补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "405", "add");
                        dic.Add("d_405_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //405行政补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_406_add_amout":               //406事业收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "406", "add");
                        dic.Add("d_406_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //406事业收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_407_add_amout":               //407投资收益_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "407", "add");
                        dic.Add("d_407_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //407投资收益__ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_408_add_amout":               //408其他收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "408", "add");
                        dic.Add("d_408_add_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //408其他收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_401_cut_amout":               //401会费收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "401", "cut");
                        dic.Add("d_401_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //401会费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_402_cut_amout":               //402拨缴经费收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "402", "cut");
                        dic.Add("d_402_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //402拨缴经费收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_403_cut_amout":               //403上级补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "403", "cut");
                        dic.Add("d_403_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //403上级补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40301_cut_amout":               //40301回拨补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40301", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40302_cut_amout":               //40302专项补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40302", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40303_cut_amout":               //40303超收补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40303", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40304_cut_amout":               //40304帮扶补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40304", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40305_cut_amout":               //40305送温暖补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40305", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40306_cut_amout":               //40306救灾补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40306", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_40307_cut_amout":               //40307其他补助_ys
                        FData = GetYskmAmount(resultTzYSKM, "40307", "cut");
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_404_cut_amout":               //404政府补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "404", "cut");
                        dic.Add("d_404_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //404政府补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_405_cut_amout":               //405行政补助收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "405", "cut");
                        dic.Add("d_405_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //405行政补助收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_406_cut_amout":               //406事业收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "406", "cut");
                        dic.Add("d_406_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //406事业收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_407_cut_amout":               //407投资收益_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "407", "cut");
                        dic.Add("d_407_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //407投资收益__ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    case "d_408_cut_amout":               //408其他收入_ys
                        FAmount = GetSumYskmAmount(resultTzYSKM, "408", "cut");
                        dic.Add("d_408_cut_amout_Percent", GetYskmPercent(FAmount, sumTzSR));  //408其他收入_ysPercent
                        FData = String.Format("{0:N2}", FAmount) + "万元";
                        dic.Add(item.FProjCode, FData);
                        break;
                    default:
                        break;
                }
            }

            return dic;
        }

        /// <summary>
        /// XXX 预算科目_项目_ys
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public string GetYskmData(List<BudgetMstModel> resultYSKM, string FYskm)
        {
            string FData = "";
            var value = resultYSKM.Where(x => x.FProjCode == FYskm).ToList();  // x.FProjCode 暂时为预算科目代码
            foreach (var km in value)
            {
                FData += km.FProjName + String.Format("{0:N2}", System.Math.Abs(km.FProjAmount)) + "万元##%!!%##";   //FProjName 暂时为预算科目内容
            }
            if (FData.Length > 0)
            {
                FData = FData.Substring(0, FData.Length - 8);
            }
            return FData;
        }


        /// <summary>
        /// XXX 预算科目_项目_ys
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public string GetYskmData(List<BudgetMstModel> resultYSKM, string FYskm,string TZ)
        {
            string FData = "";
            var value = resultYSKM.Where(x => x.FProjCode == FYskm).ToList();  // x.FProjCode 暂时为预算科目代码
            foreach (var km in value)
            {
                if(TZ == "add")
                {
                    if(km.FProjAmount >= 0)
                    {
                        FData += km.FProjName + String.Format("{0:N2}", System.Math.Abs(km.FProjAmount)) + "万元##%!!%##";   //FProjName 暂时为预算科目内容
                    }
                }

                if (TZ == "cut")
                {
                    if (km.FProjAmount < 0)
                    {
                        FData += km.FProjName + String.Format("{0:N2}", System.Math.Abs(km.FProjAmount)) + "万元##%!!%##";   //FProjName 暂时为预算科目内容
                    }
                }
            }
            if (FData.Length > 0)
            {
                FData = FData.Substring(0, FData.Length - 8);
            }
            return FData;
        }

        /// <summary>
        /// XXX 预算科目_ys
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public string GetYskmAmount(List<BudgetMstModel> resultYSKM, string FYskm)
        {
            string FData = "";
            decimal FAmount = 0;
            var value = resultYSKM.Where(x => x.FProjCode == FYskm).ToList();  // x.FProjCode 暂时为预算科目代码
            foreach (var km in value)
            {
                FAmount += km.FProjAmount;   //FProjAmount 暂时为项目金额
            }
            FData = String.Format("{0:N2}", FAmount) + "万元";
            return FData;
        }

        /// <summary>
        /// XXX 预算科目_ys
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public string GetYskmAmount(List<BudgetMstModel> resultYSKM, string FYskm,string TZ)
        {
            string FData = "";
            decimal FAmount = 0;
            var value = resultYSKM.Where(x => x.FProjCode == FYskm).ToList();  // x.FProjCode 暂时为预算科目代码
            if(TZ == "add")
            {
                foreach (var km in value)
                {
                    if(km.FProjAmount >= 0)
                    {
                        FAmount += km.FProjAmount;   //FProjAmount 暂时为项目金额
                    }
                    
                }
            }

            if (TZ == "cut")
            {
                foreach (var km in value)
                {
                    if (km.FProjAmount < 0)
                    {
                        FAmount += km.FProjAmount;   //FProjAmount 暂时为项目金额
                    }

                }
            }

            FData = String.Format("{0:N2}", System.Math.Abs(FAmount)) + "万元";
            return FData;
        }

        /// <summary>
        /// XXX 预算科目_ys(一级科目)
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public decimal GetSumYskmAmount(List<BudgetMstModel> resultYSKM, string FYskm)
        {
            
            decimal FAmount = 0;
           
            foreach (var item in resultYSKM)
            {
                if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 3) == FYskm)
                {
                    FAmount += item.FProjAmount;
                }             
            }
            return FAmount;
        }

        /// <summary>
        /// XXX 预算科目_ys(一级科目)
        /// </summary>
        /// <param name="resultYSKM"></param>
        /// <param name="FYskm"></param>
        /// <returns></returns>
        public decimal GetSumYskmAmount(List<BudgetMstModel> resultYSKM, string FYskm,string TZ)
        {

            decimal FAmount = 0;

            if(TZ == "add")
            {
                foreach (var item in resultYSKM)
                {
                    if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 3) == FYskm && item.FProjAmount >= 0)
                    {
                        FAmount += item.FProjAmount;
                    }
                }
            }
            if (TZ == "cut")
            {
                foreach (var item in resultYSKM)
                {
                    if (item.FProjCode.Length > 0 && item.FProjCode.Substring(0, 3) == FYskm && item.FProjAmount < 0)
                    {
                        FAmount += item.FProjAmount;
                    }
                }
            }
            FAmount = System.Math.Abs(FAmount);
            return FAmount;
        }

        /// <summary>
        /// XXX 预算科目_ys%
        /// </summary>
        /// <param name="Numerator">分子</param>
        /// <param name="Denominator">分母</param>
        /// <returns></returns>
        public string GetYskmPercent(decimal Numerator, decimal Denominator)
        {
            string FData = "";
            if(Denominator == 0)
            {
                return "0%";
            }
            if (Numerator == 0)
            {
                return "0%";
            }
            FData = (Numerator / Denominator).ToString("0.00%");
            return FData;
        }

        #endregion

        #region 下一审批人
        /// <summary>
        /// 获取审批中的单据id
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public List<string> GetApproveList(PagedResult<BudgetMstModel> pageResult)
        {
            var idList = new List<string>();
            foreach (var item in pageResult.Results)
            {
                if (item.FApproveStatus == "2")
                {
                    idList.Add(item.PhId.ToString());
                }
            }
            return idList;
        }

        /// <summary>
        /// 增加单据中下一审批节点名称
        /// </summary>
        /// <param name="pageResult"></param>
        /// <param name="BizType"> 审批流业务类型</param>
        /// <returns></returns>
        public PagedResult<BudgetMstModel> AddNextApproveName(PagedResult<BudgetMstModel> pageResult, string BizType)
        {
            var approveListId = GetApproveList(pageResult);

            QTControlSetModel qTControlSet=new QTControlSetModel();
            Dictionary<string, object> dicWhereSet = new Dictionary<string, object>();
            new CreateCriteria(dicWhereSet)
               .Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZYsAmount"));

            IList<QTControlSetModel> SetResult = QTControlSetRule.Find(dicWhereSet);
            if (SetResult.Count > 0)
            {
                qTControlSet = SetResult[0];
            }
            if (qTControlSet.ControlOrNot == "1")
            {
                List<string> AccountList = pageResult.Results.ToList().Where(x => !string.IsNullOrEmpty(x.FAccount)).Select(x => x.FAccount).Distinct().ToList();
                string userConn = "";
                string select_DM;
                DataTable dataTable = null;
                foreach (var Account in AccountList)
                {
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                    var Accounts = QtAccountRule.Find(conndic);
                    if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                    {
                        userConn = Accounts[0].FConn;
                        if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            select_DM = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)),zxyt FROM v_zw_pzhz group by zxyt ";
                        }
                        else
                        {
                            select_DM = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)),zxyt FROM v_zw_pzhz group by zxyt ";
                        }
                        DbHelper.Open(userConn);
                        dataTable = DbHelper.GetDataTable(userConn, select_DM);
                        DbHelper.Close(userConn);
                        foreach (var item in pageResult.Results)
                        {
                            if (item.FAccount == Account)
                            {
                                //当取不到已使用金额时
                                item.UseAmount = 0;
                                item.RemainAmount = item.FProjAmount;
                                var drArr = dataTable.Select("zxyt='" + item.FProjCode + "'");
                                if (drArr.Length > 0)
                                {
                                    item.UseAmount = Convert.ToDecimal(drArr[0][0]);
                                    item.RemainAmount = item.FProjAmount - item.UseAmount;
                                }
                                
                            }
                        }
                    }
                }
            }
            if (approveListId.Count == 0)
            {
                foreach (var item in pageResult.Results)
                {
                    item.FNextApprove = "无";
                }
                return pageResult;
            }
            var Next_approve = WorkFlowHelper.GetFlowInfoByBizList(BizType, approveListId);

            foreach (var item in pageResult.Results)
            {
                if (item.FApproveStatus == "2")
                {
                    for (var i = 0; i < Next_approve.Count; i++)
                    {
                        if (Next_approve[i]["pk1"].ToString() == item.PhId.ToString() && Next_approve[i]["flow_status_name"].ToString() == "运行中")
                        {
                            item.FNextApprove = Next_approve[i]["curnodes"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    item.FNextApprove = "无";
                }
            }
            return pageResult;
        }
        #endregion


        /// <summary>
        /// 项目预算调整分析表
        /// </summary>
        /// <param name="dicWhere"></param>
        /// <returns></returns>
        public List<BudgetTZModel> GetBudgetTZList(string userID, Dictionary<string, object> dicWhere)
        {
            List<BudgetTZModel> BudgetTZList = new List<BudgetTZModel>();
            // Dictionary<string, object> dicWhere = new Dictionary<string, object>();
            //new CreateCriteria(dicWhere)
            //    .Add(ORMRestrictions<string>.Eq("FBudgetAccounts", ""));
            var fnum = 0;
            var MstData = BudgetMstRule.Find(dicWhere, new string[] { "FDeclarationUnit Desc", "FDeclarationDept Desc" }).ToList();

            var ProData = ProjectMstRule.Find(dicWhere, new string[] { "FDeclarationUnit Desc", "FDeclarationDept Desc" }).ToList();

            if (ProData.Count > 0)
            {
                #region 列表Grid代码转名称
                RichHelpDac helpdac = new RichHelpDac();
                //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
                //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
                helpdac.CodeToName<ProjectMstModel>(ProData, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
                helpdac.CodeToName<ProjectMstModel>(ProData, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
                helpdac.CodeToName<ProjectMstModel>(ProData, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
                #endregion
                foreach (var item in ProData)
                {
                    BudgetTZModel BudgetTZPro = new BudgetTZModel();
                    fnum++;
                    BudgetTZPro.FProjName = item.FProjName;
                    BudgetTZPro.FProjCode = item.FProjCode;
                    BudgetTZPro.FDeclarationUnit = item.FDeclarationUnit;
                    BudgetTZPro.FDeclarationUnit_EXName = item.FDeclarationUnit_EXName;
                    BudgetTZPro.FDeclarationDept = item.FDeclarationDept;
                    BudgetTZPro.FDeclarationDept_EXName = item.FDeclarationDept_EXName;
                    BudgetTZPro.FBudgetDept = item.FBudgetDept;
                    BudgetTZPro.FBudgetDept_EXName = item.FBudgetDept_EXName;
                    BudgetTZPro.FNum = fnum.ToString();
                    BudgetTZPro.FDtlName = "";
                    BudgetTZPro.FBudgetAccounts = "";
                    BudgetTZPro.FBudgetAccounts_EXName = "";
                    BudgetTZPro.FExpensesChannel = "";
                    BudgetTZPro.FExpensesChannel_EXName = "";
                    BudgetTZPro.FBudgetAmount = item.FBudgetAmount;
                    BudgetTZPro.FAmountAddEdit = 0;
                    BudgetTZPro.FAmountCutEdit = 0;
                    BudgetTZPro.FAmountAfterEdit = item.FProjAmount;
                    BudgetTZPro.FAmountAfterEditApprove = item.FProjAmount;
                    BudgetTZPro.UseAmount = 0;
                    BudgetTZPro.RemainAmount = 0;
                    BudgetTZPro.FUserPer = 0;
                    BudgetTZList.Add(BudgetTZPro);

                    var DtlData = ProjectDtlBudgetDtlRule.FindByForeignKey(item.PhId);
                    if (DtlData.Count > 0)
                    {
                        #region 明细Grid代码转名称
                        RichHelpDac helpdac1 = new RichHelpDac();
                        helpdac1.CodeToName<ProjectDtlBudgetDtlModel>(DtlData, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
                        helpdac1.CodeToName<ProjectDtlBudgetDtlModel>(DtlData, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
                        #endregion

                        foreach (var data in DtlData)
                        {
                            BudgetTZModel BudgetTZPro1 = new BudgetTZModel();
                            BudgetTZPro1.FProjName = item.FProjName;
                            BudgetTZPro1.FProjDtlCode = data.FDtlCode;
                            BudgetTZPro1.FDeclarationUnit = item.FDeclarationUnit;
                            BudgetTZPro1.FDeclarationUnit_EXName = item.FDeclarationUnit_EXName;
                            BudgetTZPro1.FDeclarationDept = item.FDeclarationDept;
                            BudgetTZPro1.FDeclarationDept_EXName = item.FDeclarationDept_EXName;
                            BudgetTZPro1.FBudgetDept = item.FBudgetDept;
                            BudgetTZPro1.FBudgetDept_EXName = item.FBudgetDept_EXName;
                            BudgetTZPro1.FNum = "";
                            BudgetTZPro1.FDtlName = data.FName;
                            BudgetTZPro1.FBudgetAccounts = data.FBudgetAccounts;
                            BudgetTZPro1.FBudgetAccounts_EXName = data.FBudgetAccounts_EXName;
                            BudgetTZPro1.FExpensesChannel = data.FExpensesChannel;
                            BudgetTZPro1.FExpensesChannel_EXName = data.FExpensesChannel_EXName;
                            BudgetTZPro1.FBudgetAmount = data.FAmount;
                            if (data.FAmountEdit >= 0)
                            {
                                BudgetTZPro1.FAmountAddEdit = data.FAmountEdit;
                                BudgetTZPro1.FAmountCutEdit = 0;
                            }
                            else
                            {
                                BudgetTZPro1.FAmountAddEdit = 0;
                                BudgetTZPro1.FAmountCutEdit = System.Math.Abs(data.FAmountEdit);
                            }

                            BudgetTZPro1.FAmountAfterEdit = data.FAmountAfterEdit;
                            BudgetTZPro1.FAmountAfterEditApprove = data.FAmountAfterEdit;
                            BudgetTZPro1.UseAmount = 0;
                            BudgetTZPro1.RemainAmount = data.FAmountAfterEdit - BudgetTZPro.UseAmount;
                            BudgetTZPro1.FUserPer = 0;
                            BudgetTZList.Add(BudgetTZPro1);
                        }

                    }

                }
            }


            if (MstData.Count > 0)
            {
                #region 列表Grid代码转名称
                RichHelpDac helpdac = new RichHelpDac();
                //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "属性名", "注册的帮助标识"
                //helpdac.CodeToName<BudgetMstModel>(pageResult.Results, "Code属性名", "Name属性名", "注册的帮助标识", "");
                helpdac.CodeToName<BudgetMstModel>(MstData, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist", "");
                helpdac.CodeToName<BudgetMstModel>(MstData, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode", "");
                helpdac.CodeToName<BudgetMstModel>(MstData, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist", "");
                #endregion
                foreach (var item in MstData)
                {
                    BudgetTZModel BudgetTZ = new  BudgetTZModel();
                    fnum++;
                    BudgetTZ.FProjName = item.FProjName;
                    BudgetTZ.FProjCode = item.FProjCode;
                    BudgetTZ.FDeclarationUnit = item.FDeclarationUnit;
                    BudgetTZ.FDeclarationUnit_EXName = item.FDeclarationUnit_EXName;
                    BudgetTZ.FDeclarationDept = item.FDeclarationDept;
                    BudgetTZ.FDeclarationDept_EXName = item.FDeclarationDept_EXName;
                    BudgetTZ.FBudgetDept = item.FBudgetDept;
                    BudgetTZ.FBudgetDept_EXName = item.FBudgetDept_EXName;
                    BudgetTZ.FNum = fnum.ToString();
                    BudgetTZ.FDtlName = "";
                    BudgetTZ.FBudgetAccounts = "";
                    BudgetTZ.FBudgetAccounts_EXName = "";
                    BudgetTZ.FExpensesChannel = "";
                    BudgetTZ.FExpensesChannel_EXName = "";
                    BudgetTZ.FBudgetAmount = item.FBudgetAmount;
                    BudgetTZ.FAmountAddEdit = 0;
                    BudgetTZ.FAmountCutEdit = 0;
                    BudgetTZ.FAmountAfterEdit = item.FProjAmount;
                    BudgetTZ.FAmountAfterEditApprove = item.FProjAmount;
                    BudgetTZ.UseAmount = 0;
                    BudgetTZ.RemainAmount = 0;
                    BudgetTZ.FUserPer = 0;
                    BudgetTZList.Add(BudgetTZ);

                    var DtlData = BudgetDtlBudgetDtlRule.FindByForeignKey(item.PhId);
                    if(DtlData.Count > 0)
                    {
                        #region 明细Grid代码转名称
                        RichHelpDac helpdac1 = new RichHelpDac();
                        helpdac1.CodeToName<BudgetDtlBudgetDtlModel>(DtlData, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
                        helpdac1.CodeToName<BudgetDtlBudgetDtlModel>(DtlData, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
                        #endregion
                        
                        foreach(var data in DtlData)
                        {
                            BudgetTZModel BudgetTZ1 = new BudgetTZModel();
                            BudgetTZ1.FProjName = item.FProjName;
                            BudgetTZ1.FProjDtlCode = data.FDtlCode;
                            BudgetTZ1.FDeclarationUnit = item.FDeclarationUnit;
                            BudgetTZ1.FDeclarationUnit_EXName = item.FDeclarationUnit_EXName;
                            BudgetTZ1.FDeclarationDept = item.FDeclarationDept;
                            BudgetTZ1.FDeclarationDept_EXName = item.FDeclarationDept_EXName;
                            BudgetTZ1.FBudgetDept = item.FBudgetDept;
                            BudgetTZ1.FBudgetDept_EXName = item.FBudgetDept_EXName;
                            BudgetTZ1.FNum = "";
                            BudgetTZ1.FDtlName = data.FName;
                            BudgetTZ1.FBudgetAccounts = data.FBudgetAccounts;
                            BudgetTZ1.FBudgetAccounts_EXName = data.FBudgetAccounts_EXName;
                            BudgetTZ1.FExpensesChannel = data.FExpensesChannel;
                            BudgetTZ1.FExpensesChannel_EXName = data.FExpensesChannel_EXName;
                            BudgetTZ1.FBudgetAmount = data.FAmount;
                            if(data.FAmountEdit >= 0)
                            {
                                BudgetTZ1.FAmountAddEdit = data.FAmountEdit;
                                BudgetTZ1.FAmountCutEdit = 0;
                            }
                            else
                            {
                                BudgetTZ1.FAmountAddEdit = 0;
                                BudgetTZ1.FAmountCutEdit =System.Math.Abs (data.FAmountEdit);
                            }
      
                            BudgetTZ1.FAmountAfterEdit = data.FAmountAfterEdit;
                            BudgetTZ1.FAmountAfterEditApprove = data.FAmountAfterEdit;
                            BudgetTZ1.UseAmount = 0;
                            BudgetTZ1.RemainAmount = data.FAmountAfterEdit - BudgetTZ.UseAmount;
                            BudgetTZ1.FUserPer = 0;
                            BudgetTZ1.FAccount = item.FAccount;
                            BudgetTZList.Add(BudgetTZ1);
                        }

                    }
                 
                }
            }



            BudgetTZList.OrderBy(u => u.FDeclarationUnit).ThenByDescending(u => u.FDeclarationDept);
            
            return GetSJFSSbyCode(userID, BudgetTZList);
            //return BudgetTZList;
        }



        /// <summary>
        /// 通过项目代码和操作员获取财务实际发生数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="BudgetTZList"></param>
        /// <returns></returns>
        public List<BudgetTZModel> GetSJFSSbyCode(string userID, List<BudgetTZModel> BudgetTZList)
        {
            
            
            //string FDeclarationUnit = "";
            //var dicWhereUnit = new Dictionary<string, object>();
            //new CreateCriteria(dicWhereUnit).Add(ORMRestrictions<string>.Eq("Dylx", "08"))
            //    .Add(ORMRestrictions<Int32>.Eq("DefInt1", 1)).Add(ORMRestrictions<string>.Eq("Dwdm", userID));
            //var CorrespondenceSettings = CorrespondenceSettingsRule.Find(dicWhereUnit);
            //if (CorrespondenceSettings.Count > 0)
            //{
            //    FDeclarationUnit = CorrespondenceSettings[0].Dydm;
            //}
            DateTime? beforeDate = new DateTime(DateTime.Now.Year, 1, 1); //账务筛选时间
            var dicWheredate = new Dictionary<string, object>();
            new CreateCriteria(dicWheredate).Add(ORMRestrictions<string>.Eq("BZ", "G6HBLKZExpense"));
            var setModels = QTControlSetRule.Find(dicWheredate);//QTControlSetModel
            if (setModels.Count > 0)
            {
                beforeDate = setModels[0].BEGINFDATE;
            }
            List<string> AccountList = BudgetTZList.Select(x => x.FAccount).Distinct().ToList();
            string userConn = "";
            string sql_zxyt;
            string sql_mxxm;
            //Dictionary<string, object> conndic = new Dictionary<string, object>();
            //new CreateCriteria(conndic)
            //    .Add(ORMRestrictions<string>.Eq("Dylx", "config"))
            //    .Add(ORMRestrictions<string>.Eq("DefStr1", FDeclarationUnit));
            //IList<CorrespondenceSettings2Model> CorrespondenceSettings2s = CorrespondenceSettings2Rule.RuleHelper.Find(conndic);
            DataTable dataTableZxyt = null;
            DataTable dataTableMxxm = null;
            
            //if (dataTable.Rows.Count != 0)
            //{
            //    result = dataTable.Rows[0][0].ToString();
            //}
            Decimal UseAmount = 0;

            foreach (var Account in AccountList)
            {
                if (!string.IsNullOrEmpty(Account))
                {
                    Dictionary<string, object> conndic = new Dictionary<string, object>();
                    new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                    var Accounts = QtAccountRule.Find(conndic);
                    if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                    {
                        userConn = Accounts[0].FConn;
                        if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            sql_zxyt = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)),zxyt FROM v_zw_pzhz WHERE (zxyt is not null or zxyt <> '' ) and PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by zxyt";
                            sql_mxxm = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)),mxxm FROM v_zw_pzhz WHERE (mxxm is not null or mxxm <> '' ) and  PZRQ < to_date('" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by mxxm";
                        }
                        else
                        {
                            sql_zxyt = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)),zxyt FROM v_zw_pzhz WHERE (zxyt is not null or zxyt <> '' ) and   PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "' group by zxyt ";
                            sql_mxxm = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)),mxxm FROM v_zw_pzhz WHERE (mxxm is not null or mxxm <> '' ) and  PZRQ < '" + Convert.ToDateTime(beforeDate).ToString("yyyy-MM-dd") + "' group by mxxm ";
                        }
                        DbHelper.Open(userConn);
                        dataTableZxyt = DbHelper.GetDataTable(userConn, sql_zxyt);
                        dataTableMxxm = DbHelper.GetDataTable(userConn, sql_mxxm);
                        DbHelper.Close(userConn);
                        var BudgetTZList2 = BudgetTZList.FindAll(x => x.FAccount == Account);
                        foreach (var item in BudgetTZList2)
                        {
                            UseAmount = 0;

                            if (!string.IsNullOrEmpty(item.FProjCode))
                            {
                                var drArr = dataTableZxyt.Select("zxyt='" + item.FProjCode + "'");
                                if (drArr.Length > 0)
                                {
                                    UseAmount = Convert.ToDecimal(drArr[0][0]);
                                }
                                item.UseAmount = UseAmount;
                                item.RemainAmount = item.FAmountAfterEdit - UseAmount;
                                if (item.FAmountAfterEdit != 0)
                                {
                                    item.FUserPer = (UseAmount * 100 / item.FAmountAfterEdit);
                                }
                                else
                                {
                                    item.FUserPer = 100;
                                }

                                if (item.FAmountAfterEdit >= item.FBudgetAmount)
                                {
                                    item.FAmountAddEdit = item.FAmountAfterEdit - item.FBudgetAmount;
                                }
                                else
                                {
                                    item.FAmountCutEdit = item.FBudgetAmount - item.FAmountAfterEdit;
                                }
                            }
                            if (!string.IsNullOrEmpty(item.FProjDtlCode))
                            {
                                var drArr = dataTableMxxm.Select("mxxm='" + item.FProjDtlCode + "'");
                                if (drArr.Length > 0)
                                {
                                    UseAmount = Convert.ToDecimal(drArr[0][0]);
                                }
                                item.UseAmount = UseAmount;
                                item.RemainAmount = item.FAmountAfterEdit - UseAmount;
                                if (item.FAmountAfterEdit != 0)
                                {
                                    item.FUserPer = (UseAmount * 100 / item.FAmountAfterEdit);
                                }
                                else
                                {
                                    item.FUserPer = 100;
                                }

                                //if (item.FAmountAfterEdit >= item.FBudgetAmount)
                                //{
                                //    item.FAmountAddEdit = item.FAmountAfterEdit - item.FBudgetAmount;
                                //}
                                //else
                                //{
                                //    item.FAmountCutEdit = item.FBudgetAmount - item.FAmountAfterEdit;
                                //}
                            }
                            
                        }
                    }

                    //else
                    //{
                    //   // return "0.00";
                    //}
                }
            }
            return BudgetTZList;
        }

        #region//预算工作流相关
        /// <summary>
        /// 修改项目审批状态
        /// </summary>
        /// <param name="recordModel">审批对象</param>
        /// <param name="fApproval">审批字段</param>
        /// <returns></returns>
        public SavedResult<long> UpdateBudget(GAppvalRecordModel recordModel, string fApproval)
        {
            if (recordModel.RefbillPhid == 0)
                return null;

            BudgetMstModel budgetMst = this.BudgetMstRule.Find(recordModel.RefbillPhid);

            budgetMst.FApproveStatus = fApproval;
            budgetMst.FApproveDate = DateTime.Now;
            budgetMst.FApprover = recordModel.OperaPhid;
            budgetMst.FApprover_EXName = recordModel.OperaName;
            budgetMst.PersistentState = PersistentState.Modified;

            return BudgetMstRule.Save<Int64>(budgetMst);
        }
        #endregion

        #region//项目预算调整分析表

        /// <summary>
        /// 根据年份与组织编码获取相应的项目预算调整分析表
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="orgCode">组织编码</param>
        /// <returns></returns>
        public List<BudgetAdjustAnalyseModel> GetBudgetAdjustAnalyseList(string year, string orgCode)
        {
            SqlDao sqlDao = new SqlDao();
            List<BudgetAdjustAnalyseModel> budgetAdjustAnalyses = new List<BudgetAdjustAnalyseModel>();
            budgetAdjustAnalyses = sqlDao.GetBudgetAdjustAnalyses(year, orgCode);

            if(budgetAdjustAnalyses != null && budgetAdjustAnalyses.Count > 0)
            {
                RichHelpDac helpdac = new RichHelpDac();
                helpdac.CodeToName<BudgetAdjustAnalyseModel>(budgetAdjustAnalyses, "FDeclarationUnit", "FDeclarationUnit_EXName", "sb_orglist","");
                helpdac.CodeToName<BudgetAdjustAnalyseModel>(budgetAdjustAnalyses, "FDeclarationDept", "FDeclarationDept_EXName", "dept4ocode","");
                helpdac.CodeToName<BudgetAdjustAnalyseModel>(budgetAdjustAnalyses, "FBudgetDept", "FBudgetDept_EXName", "ys_orglist","");
                helpdac.CodeToName<BudgetAdjustAnalyseModel>(budgetAdjustAnalyses, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");

                //所有预算对应的账套
                List<string> AccountList = budgetAdjustAnalyses.Select(x => x.FAccount).Distinct().ToList();
                //年度信息
                DateTime? startTime = new DateTime(int.Parse(year), 1, 1);
                DateTime? endTime = new DateTime(int.Parse(year) + 1, 1, 1);
                string userConn = "";
                string sql_zxyt;
                string sql_mxxm;

                DataTable dataTableZxyt = null;
                DataTable dataTableMxxm = null;

                Decimal UseAmount = 0;
                Decimal UseDtlAmount = 0;

                foreach (var Account in AccountList)
                {
                    if (!string.IsNullOrEmpty(Account))
                    {
                        Dictionary<string, object> conndic = new Dictionary<string, object>();
                        new CreateCriteria(conndic).Add(ORMRestrictions<string>.Eq("Dm", Account));
                        var Accounts = QtAccountRule.Find(conndic);
                        if (Accounts.Count > 0 && !string.IsNullOrEmpty(Accounts[0].FConn))
                        {
                            userConn = Accounts[0].FConn;
                            if (userConn.IndexOf("ConnectType=SqlClient", StringComparison.OrdinalIgnoreCase) < 0)
                            {
                                sql_zxyt = @"SELECT sum(nvl(pz.j,0))-sum(nvl(pz.d,0)) jd,pz.zxyt,lh.ORGID, lh.ORGCODE, lh.DWMC  
                                            FROM v_zw_pzhz pz
                                            LEFT JOIN z_lhdw lh ON lh.ORGCODE = pz.ORGCODE
                                            WHERE (pz.zxyt is not null or pz.zxyt <> '' ) and pz.PZRQ < to_date('" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') and  pz.PZRQ >= to_date('" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by pz.zxyt,lh.ORGID, lh.ORGCODE, lh.DWMC ";
                                sql_mxxm = @"SELECT sum(nvl(pz.j,0))-sum(nvl(pz.d,0)) jd,pz.mxxm,lh.ORGID, lh.ORGCODE, lh.DWMC  
                                            FROM v_zw_pzhz pz
                                            LEFT JOIN z_lhdw lh ON lh.ORGCODE = pz.ORGCODE
                                            WHERE (pz.mxxm is not null or pz.mxxm <> '' ) and pz.PZRQ < to_date('" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') and  pz.PZRQ >= to_date('" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by pz.mxxm,lh.ORGID, lh.ORGCODE, lh.DWMC ";
                                //sql_zxyt = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)),zxyt FROM v_zw_pzhz WHERE (zxyt is not null or zxyt <> '' ) and PZRQ < to_date('" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') and  PZRQ >= to_date('" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by zxyt";
                                //sql_mxxm = @"SELECT sum(nvl(j,0))-sum(nvl(d,0)),mxxm FROM v_zw_pzhz WHERE (mxxm is not null or mxxm <> '' ) and  PZRQ < to_date('" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') and  PZRQ >= to_date('" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "', 'yyyy/mm/dd hh24:mi:ss') group by mxxm";
                            }
                            else
                            {
                                sql_zxyt = @"SELECT sum(nvl(pz.j,0))-sum(nvl(pz.d,0)) jd,pz.zxyt,lh.ORGID, lh.ORGCODE, lh.DWMC  
                                            FROM v_zw_pzhz pz
                                            LEFT JOIN z_lhdw lh ON lh.ORGCODE = pz.ORGCODE
                                            WHERE (pz.zxyt is not null or pz.zxyt <> '' )  and  pz.PZRQ < '" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "' and  pz.PZRQ >= '" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "'  group by pz.zxyt,lh.ORGID, lh.ORGCODE, lh.DWMC ";
                                sql_mxxm = @"SELECT sum(nvl(pz.j,0))-sum(nvl(pz.d,0)) jd,pz.mxxm,lh.ORGID, lh.ORGCODE, lh.DWMC  
                                            FROM v_zw_pzhz pz
                                            LEFT JOIN z_lhdw lh ON lh.ORGCODE = pz.ORGCODE
                                            WHERE (pz.mxxm is not null or pz.mxxm <> '' )  and  pz.PZRQ < '" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "' and  pz.PZRQ >= '" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "' group by pz.mxxm,lh.ORGID, lh.ORGCODE, lh.DWMC ";
                                //sql_zxyt = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)),zxyt FROM v_zw_pzhz WHERE (zxyt is not null or zxyt <> '' ) and   PZRQ < '" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "' and  PZRQ >= '" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "' group by zxyt ";
                                //sql_mxxm = @"SELECT sum(isnull(j,0))-sum(isnull(d,0)),mxxm FROM v_zw_pzhz WHERE (mxxm is not null or mxxm <> '' ) and  PZRQ < '" + Convert.ToDateTime(endTime).ToString("yyyy-MM-dd") + "'and  PZRQ >= '" + Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + "' group by mxxm ";
                            }
                            DbHelper.Open(userConn);
                            dataTableZxyt = DbHelper.GetDataTable(userConn, sql_zxyt);
                            dataTableMxxm = DbHelper.GetDataTable(userConn, sql_mxxm);
                            DbHelper.Close(userConn);
                            var budgetAdjustAnalyses2 = budgetAdjustAnalyses.FindAll(x => x.FAccount == Account);
                            foreach (var item in budgetAdjustAnalyses2)
                            {
                                UseAmount = 0;
                                UseDtlAmount = 0;
                                if (!string.IsNullOrEmpty(item.FProjCode))
                                {
                                    var drArr = dataTableZxyt.Select("zxyt='" + item.FProjCode + "'");
                                    if (drArr.Length > 0)
                                    {
                                        UseAmount = Convert.ToDecimal(drArr[0]["jd"]);
                                        item.FAccountOrgId = drArr[0]["ORGID"].ToString();
                                        item.FAccountOrgCode = drArr[0]["ORGCODE"].ToString();
                                        item.FAccountOrgName = drArr[0]["DWMC"].ToString();
                                    }
                                }
                                item.FUseBudgetAmount = UseAmount;
                                if (!string.IsNullOrEmpty(item.FDtlCode))
                                {
                                    var drArr = dataTableMxxm.Select("mxxm='" + item.FDtlCode + "'");
                                    if (drArr.Length > 0)
                                    {
                                        UseDtlAmount = Convert.ToDecimal(drArr[0][0]);
                                        item.FAccountOrgId = drArr[0]["ORGID"].ToString();
                                        item.FAccountOrgCode = drArr[0]["ORGCODE"].ToString();
                                        item.FAccountOrgName = drArr[0]["DWMC"].ToString();
                                    }
                                }
                                item.FUseAmount = UseDtlAmount;
                            }
                        }
                    }
                }

                //找出所有不同的项目
                List<long> proPhIds = budgetAdjustAnalyses.ToList().Select(t => t.FProjPhId).Distinct().ToList();
                if(proPhIds != null && proPhIds.Count > 0)
                {
                    foreach(var phid in proPhIds)
                    {
                        BudgetAdjustAnalyseModel budgetAdjust = new BudgetAdjustAnalyseModel();
                        var adjusts = budgetAdjustAnalyses.ToList().FindAll(t=>t.FProjPhId  == phid);
                        if(adjusts != null && adjusts.Count > 0)
                        {
                            budgetAdjust.FYear = adjusts[0].FYear;
                            budgetAdjust.FProjCode = adjusts[0].FProjCode;
                            budgetAdjust.FProjName = adjusts[0].FProjName;
                            budgetAdjust.FDeclarationUnit = adjusts[0].FDeclarationUnit;
                            budgetAdjust.FDeclarationUnit_EXName = adjusts[0].FDeclarationUnit_EXName;
                            budgetAdjust.FDeclarationDept = adjusts[0].FDeclarationDept;
                            budgetAdjust.FDeclarationDept_EXName = adjusts[0].FDeclarationDept_EXName;
                            budgetAdjust.FStartDate = adjusts[0].FStartDate;
                            budgetAdjust.FEndDate = adjusts[0].FEndDate;
                            budgetAdjust.FDateofDeclaration = adjusts[0].FDateofDeclaration;
                            budgetAdjust.FBudgetDept = adjusts[0].FBudgetDept;
                            budgetAdjust.FBudgetDept_EXName = adjusts[0].FBudgetDept_EXName;
                            budgetAdjust.FAccount = adjusts[0].FAccount;
                            budgetAdjust.FBudgetAccounts = adjusts[0].FBudgetAccounts;
                            budgetAdjust.FBudgetAccounts_EXName = adjusts[0].FBudgetAccounts_EXName;
                            budgetAdjust.FBudgetAmount = adjusts[0].FBudgetAmount;
                            budgetAdjust.FUseBudgetAmount = adjusts[0].FUseBudgetAmount;
                            budgetAdjust.FNoUseBudgetAmount = adjusts[0].FNoUseBudgetAmount;
                            budgetAdjust.FZBudgetAmount = adjusts[0].FZBudgetAmount;
                            budgetAdjust.FJBudgetAmount = adjusts[0].FJBudgetAmount;
                            budgetAdjust.FBudgetAmountEdit = adjusts[0].FBudgetAmountEdit;
                            budgetAdjust.FBudgetAmountAfterEdit = adjusts[0].FBudgetAmountAfterEdit;
                            budgetAdjust.FBudgetRate = adjusts[0].FBudgetRate;
                            budgetAdjust.FNCBudgetRate = adjusts[0].FNCBudgetRate;
                            budgetAdjust.FZTBudgetRate = adjusts[0].FZTBudgetRate;
                            budgetAdjust.FAccountOrgId = adjusts[0].FAccountOrgId;
                            budgetAdjust.FAccountOrgCode = adjusts[0].FAccountOrgCode;
                            budgetAdjust.FAccountOrgName = adjusts[0].FAccountOrgName;
                            budgetAdjust.FLoanAmount = adjusts[0].FLoanAmount;
                            budgetAdjust.FSubmitAmount = adjusts[0].FSubmitAmount;
                            budgetAdjust.FDtlCode = "zzzheji";
                            budgetAdjust.FDtlName = "合计";
                            budgetAdjust.FAmount = adjusts.Sum(t=>t.FAmount);
                            budgetAdjust.FZAmount = adjusts.Sum(t => t.FZAmount);
                            budgetAdjust.FJAmount = adjusts.Sum(t => t.FJAmount);
                            budgetAdjust.FAmountEdit = adjusts.Sum(t => t.FAmountEdit);
                            budgetAdjust.FAmountAfterEdit = adjusts.Sum(t => t.FAmountAfterEdit);
                            budgetAdjust.FUseAmount = adjusts.Sum(t => t.FUseAmount);
                            budgetAdjust.FNoUseAmount = budgetAdjust.FAmountAfterEdit - budgetAdjust.FUseAmount;
                            budgetAdjust.FRate = budgetAdjust.FAmountAfterEdit == 0 ? 0 : budgetAdjust.FUseAmount / budgetAdjust.FAmountAfterEdit * 100;
                            budgetAdjust.FNCRate = budgetAdjust.FAmount == 0 ? 0 : budgetAdjust.FUseAmount / budgetAdjust.FAmount * 100;
                            budgetAdjustAnalyses.Add(budgetAdjust);
                        }
                    }
                }

                foreach(var adjust in budgetAdjustAnalyses)
                {                    
                    //项目的金额
                    adjust.FBudgetAmount = budgetAdjustAnalyses.FindAll(t => t.FProjPhId == adjust.FProjPhId && adjust.FDtlCode != "zzzheji").Sum(t => t.FAmount);
                    adjust.FZBudgetAmount = budgetAdjustAnalyses.FindAll(t => t.FProjPhId == adjust.FProjPhId && adjust.FDtlCode != "zzzheji").Sum(t => t.FZAmount);
                    adjust.FJBudgetAmount = budgetAdjustAnalyses.FindAll(t => t.FProjPhId == adjust.FProjPhId && adjust.FDtlCode != "zzzheji").Sum(t => t.FJAmount);
                    adjust.FBudgetAmountEdit = budgetAdjustAnalyses.FindAll(t => t.FProjPhId == adjust.FProjPhId && adjust.FDtlCode != "zzzheji").Sum(t => t.FAmountEdit);
                    adjust.FBudgetAmountAfterEdit = budgetAdjustAnalyses.FindAll(t => t.FProjPhId == adjust.FProjPhId && adjust.FDtlCode != "zzzheji").Sum(t => t.FAmountAfterEdit);
                    adjust.FNoUseBudgetAmount = adjust.FBudgetAmountAfterEdit - adjust.FUseBudgetAmount;
                    adjust.FBudgetRate = adjust.FBudgetAmountAfterEdit== 0 ?0: adjust.FUseBudgetAmount/ adjust.FBudgetAmountAfterEdit*100;
                    adjust.FNCBudgetRate = adjust.FBudgetAmount == 0 ? 0 : adjust.FUseBudgetAmount / adjust.FBudgetAmount * 100;
                }
                foreach (var adjust in budgetAdjustAnalyses)
                {
                    //明细的各类金额
                    if (!string.IsNullOrEmpty(adjust.FDtlCode) && !adjust.FDtlCode.Equals("zzzheji"))
                    {
                        adjust.FAmount = budgetAdjustAnalyses.FindAll(t => t.FDtlCode == adjust.FDtlCode).Sum(t => t.FAmount);
                        adjust.FZAmount = budgetAdjustAnalyses.FindAll(t => t.FDtlCode == adjust.FDtlCode).Sum(t => t.FZAmount);
                        adjust.FJAmount = budgetAdjustAnalyses.FindAll(t => t.FDtlCode == adjust.FDtlCode).Sum(t => t.FJAmount);
                        adjust.FAmountEdit = budgetAdjustAnalyses.FindAll(t => t.FDtlCode == adjust.FDtlCode).Sum(t => t.FAmountEdit);
                        adjust.FAmountAfterEdit = budgetAdjustAnalyses.FindAll(t => t.FDtlCode == adjust.FDtlCode).Sum(t => t.FAmountAfterEdit);
                    }
                    adjust.FNoUseAmount = adjust.FAmountAfterEdit - adjust.FUseAmount;
                    adjust.FRate = adjust.FAmountAfterEdit == 0 ? 0 : adjust.FUseAmount / adjust.FAmountAfterEdit*100;
                    adjust.FNCRate = adjust.FAmount == 0 ? 0 : adjust.FUseAmount / adjust.FAmount * 100;
                }
            }
            return budgetAdjustAnalyses;
        }
        #endregion
    }
}

