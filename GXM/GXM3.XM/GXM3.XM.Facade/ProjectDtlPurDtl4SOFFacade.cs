#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlPurDtl4SOFFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectDtlPurDtl4SOFFacade.cs
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
using SUP.Common.DataEntity;
using SUP.Common.DataAccess;
using Enterprise3.Common.ExceptionHandling.Exceptions;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Facade;

using GXM3.XM.Facade.Interface;
using GXM3.XM.Rule.Interface;
using GXM3.XM.Model.Domain;

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectDtlPurDtl4SOF业务组装处理类
	/// </summary>
    public partial class ProjectDtlPurDtl4SOFFacade : EntFacadeBase<ProjectDtlPurDtl4SOFModel>, IProjectDtlPurDtl4SOFFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlPurDtl4SOF业务逻辑处理对象
        /// </summary>
		IProjectDtlPurDtl4SOFRule ProjectDtlPurDtl4SOFRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlPurDtl4SOFRule;
            }
        }
		/// <summary>
        /// ProjectDtlTextContent业务逻辑处理对象
        /// </summary>
		IProjectDtlTextContentRule ProjectDtlTextContentRule { get; set; }
		/// <summary>
        /// ProjectDtlPurchaseDtl业务逻辑处理对象
        /// </summary>
		IProjectDtlPurchaseDtlRule ProjectDtlPurchaseDtlRule { get; set; }

		/// <summary>
        /// ProjectDtlPerformTarget业务逻辑处理对象
        /// </summary>
		IProjectDtlPerformTargetRule ProjectDtlPerformTargetRule { get; set; }
		/// <summary>
        /// ProjectDtlFundAppl业务逻辑处理对象
        /// </summary>
		IProjectDtlFundApplRule ProjectDtlFundApplRule { get; set; }
		/// <summary>
        /// ProjectDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IProjectDtlBudgetDtlRule ProjectDtlBudgetDtlRule { get; set; }
		/// <summary>
        /// ProjectDtlImplPlan业务逻辑处理对象
        /// </summary>
		IProjectDtlImplPlanRule ProjectDtlImplPlanRule { get; set; }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlPurDtl4SOFModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlPurDtl4SOFModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectDtlPurDtl4SOFModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectDtlPurDtl4SOFModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectDtlPurDtl4SOFModel>(findedResults.Data, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            #endregion

            return findedResults;
        }

        /// <summary>
        /// 通过id，删除数据
        /// </summary>
        /// <param name="id">单主键id值</param>
        public override DeletedResult Delete<TValType>(TValType id)
        {
			ProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(id);
			ProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(id);
			return base.Delete(id);
        }

		/// <summary>
        /// 通过ids，删除数据
        /// </summary>
        /// <param name="ids">单主键id集合</param>
        public override DeletedResult Delete<TValType>(IList<TValType> ids)
        {
			ProjectDtlTextContentRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlPurchaseDtlRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlPurDtl4SOFRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlPerformTargetRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlFundApplRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlBudgetDtlRule.RuleHelper.DeleteByForeignKey(ids);
			ProjectDtlImplPlanRule.RuleHelper.DeleteByForeignKey(ids);
			return base.Delete(ids);
        }
        #endregion

		#region 实现 IProjectDtlPurDtl4SOFFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlPurDtl4SOFModel> ExampleMethod<ProjectDtlPurDtl4SOFModel>(string param)
        //{
        //    //编写代码
        //}
        
        #endregion
    }
}

