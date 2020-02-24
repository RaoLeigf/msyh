#region Summary
/**************************************************************************************
    * 类 名 称：        ProjectDtlBudgetDtlFacade
    * 命名空间：        GXM3.XM.Facade
    * 文 件 名：        ProjectDtlBudgetDtlFacade.cs
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

namespace GXM3.XM.Facade
{
	/// <summary>
	/// ProjectDtlBudgetDtl业务组装处理类
	/// </summary>
    public partial class ProjectDtlBudgetDtlFacade : EntFacadeBase<ProjectDtlBudgetDtlModel>, IProjectDtlBudgetDtlFacade
    {
		#region 类变量及属性
		/// <summary>
        /// ProjectDtlBudgetDtl业务逻辑处理对象
        /// </summary>
		IProjectDtlBudgetDtlRule ProjectDtlBudgetDtlRule
        {
            get
            {          
                if (CurrentRule == null)
                    throw new NGAppException("InitializeObjectFail");

                return CurrentRule as IProjectDtlBudgetDtlRule;
            }
        }
		#endregion

		#region 重载方法
		/// <summary>
        /// 通过外关联的单主键值，获取数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns>实体</returns>
        public override FindedResults<ProjectDtlBudgetDtlModel> FindByForeignKey<TValType>(TValType id, params string[] sorts)
        {
            FindedResults<ProjectDtlBudgetDtlModel> findedResults = base.FindByForeignKey(id, sorts);

            #region 明细Grid代码转名称
			RichHelpDac helpdac = new RichHelpDac();
            //helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "属性名", "注册的帮助标识"
            //helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "Code属性名", "Name属性名", "注册的帮助标识", "");
            helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FSourceOfFunds", "FSourceOfFunds_EXName", "GHSourceOfFunds", "");
            helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FPaymentMethod", "FPaymentMethod_EXName", "GHPaymentMethod", "");
            //helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
            helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FExpensesChannel", "FExpensesChannel_EXName", "GHExpensesChannel", "");
            helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FBudgetAccounts", "FBudgetAccounts_EXName", "GHBudgetAccounts", "");
            helpdac.CodeToName<ProjectDtlBudgetDtlModel>(findedResults.Data, "FQtZcgnfl", "FQtZcgnfl_EXName", "GHQtZcgnfl", "");
            #endregion

            return findedResults;
        }

        #endregion

        #region 实现 IProjectDtlBudgetDtlFacade 业务添加的成员

        ///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //public IList<ProjectDtlBudgetDtlModel> ExampleMethod<ProjectDtlBudgetDtlModel>(string param)
        //{
        //    //编写代码
        //}
        /// <summary>
        /// 预算根据明细表主键回填预算金额
        /// </summary>
        /// <param name="phid"></param>
        /// <param name="FAmount"></param>
        public void UpdateDtlFBudgetAmount(long[] phid, decimal[] FAmount)
        {
           // var list = new List<ProjectDtlBudgetDtlModel>();
            for (var i = 0; i < phid.Length; i++)
            {
                if (phid[i] == 0)
                {
                    continue;
                }
                var model = base.Find(phid[i]).Data;
                model.FBudgetAmount = FAmount[i];
                //model.PersistentState = p
                ProjectDtlBudgetDtlRule.Update<Int64>(model);
               // list.Add(model);
            }

            
        }
        /// <summary>
        /// 生成预算时回填明细
        /// </summary>
        /// <param name="oldxm3BudgetDtl"></param>
        public void UpdateBudgetDtlList(List<ProjectDtlBudgetDtlModel> oldxm3BudgetDtl)
        {
            foreach(var model in oldxm3BudgetDtl)
            {
                ProjectDtlBudgetDtlRule.Update<Int64>(model);
            }
            
        }

        #endregion
    }
}

