#region Summary
/**************************************************************************************
    * 类 名 称：        BudgetDtlPurDtl4SOFFacade
    * 命名空间：        GYS3.YS.Facade
    * 文 件 名：        BudgetDtlPurDtl4SOFFacade.cs
    * 创建时间：        2018/10/22 
    * 作    者：        刘杭    
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

namespace GYS3.YS.Facade
{
	/// <summary>
	/// BudgetDtlPurDtl4SOF业务组装处理类
	/// </summary>
    public partial class BudgetDtlPurDtl4SOFFacade : EntFacadeBase<BudgetDtlPurDtl4SOFModel>, IBudgetDtlPurDtl4SOFFacade
    {
		#region 类变量及属性
		/// <summary>
        /// BudgetDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
		IBudgetDtlPurDtl4SOFRule BudgetDtlPurDtl4SOFRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IBudgetDtlPurDtl4SOFRule;
            }
        }
		/// <summary>
        /// BudgetDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
		IBudgetDtlPurchaseDtlRule BudgetDtlPurchaseDtlRule { get; set; }
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
        /// BudgetDtlImplPlan业务逻辑处理对象
        /// </summary>
		IBudgetDtlImplPlanRule BudgetDtlImplPlanRule { get; set; }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<BudgetDtlPurDtl4SOFModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<BudgetDtlPurDtl4SOFModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<BudgetDtlPurDtl4SOFModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<BudgetDtlPurDtl4SOFModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<BudgetDtlPurDtl4SOFModel>(findedResults.Data, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			BudgetDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlTextContentRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlFundApplRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);
			BudgetDtlImplPlanRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			BudgetDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlTextContentRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlFundApplRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);
			BudgetDtlImplPlanRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IBudgetDtlPurDtl4SOFFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<BudgetDtlPurDtl4SOFModel> ExampleMethod<BudgetDtlPurDtl4SOFModel>(string param)
        //{
        //    //编写代码
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="budgetDtlPurDtl4SOFEntity"></param>
		/// <param name="budgetDtlPurchaseDtlEntities"></param>
		/// <param name="budgetDtlTextContentEntities"></param>
		/// <param name="budgetDtlFundApplEntities"></param>
		/// <param name="budgetDtlBudgetDtlEntities"></param>
		/// <param name="budgetDtlPerformTargetEntities"></param>
		/// <param name="budgetDtlPurDtl4SOFEntities"></param>
		/// <param name="budgetDtlImplPlanEntities"></param>
        /// <returns></returns>
        /*public SavedResult<Int64> SaveBudgetDtlPurDtl4SOF(BudgetDtlPurDtl4SOFModel budgetDtlPurDtl4SOFEntity, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities)
        {
            SavedResult<Int64> savedResult = base.Save<Int64>(budgetDtlPurDtl4SOFEntity);
            if (savedResult.Status == ResponseStatus.Success && savedResult.KeyCodes.Count > 0)
            {
				if (budgetDtlPurchaseDtlEntities.Count > 0)
				{
					BudgetDtlPurchaseDtlRule.Save(budgetDtlPurchaseDtlEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlTextContentEntities.Count > 0)
				{
					BudgetDtlTextContentRule.Save(budgetDtlTextContentEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlFundApplEntities.Count > 0)
				{
					BudgetDtlFundApplRule.Save(budgetDtlFundApplEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlBudgetDtlEntities.Count > 0)
				{
					BudgetDtlBudgetDtlRule.Save(budgetDtlBudgetDtlEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlPerformTargetEntities.Count > 0)
				{
					BudgetDtlPerformTargetRule.Save(budgetDtlPerformTargetEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlPurDtl4SOFEntities.Count > 0)
				{
					BudgetDtlPurDtl4SOFRule.Save(budgetDtlPurDtl4SOFEntities, savedResult.KeyCodes[0]);
				}
				if (budgetDtlImplPlanEntities.Count > 0)
				{
					BudgetDtlImplPlanRule.Save(budgetDtlImplPlanEntities, savedResult.KeyCodes[0]);
				}
            }

			return savedResult;
        }*/

        #endregion
    }
}

