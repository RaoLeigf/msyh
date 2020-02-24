#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetDtlPurchaseDtlFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IBudgetDtlPurchaseDtlFacade.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GYS3.YS.Model.Domain;

namespace GYS3.YS.Facade.Interface
{
	/// <summary>
	/// BudgetDtlPurchaseDtl业务组装层接口
	/// </summary>
    public partial interface IBudgetDtlPurchaseDtlFacade : IEntFacadeBase<BudgetDtlPurchaseDtlModel>
    {
		#region IBudgetDtlPurchaseDtlFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetDtlPurchaseDtlModel> ExampleMethod<BudgetDtlPurchaseDtlModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="budgetDtlPurchaseDtlEntity"></param>
		/// <param name="budgetDtlPurchaseDtlEntities"></param>
		/// <param name="budgetDtlTextContentEntities"></param>
		/// <param name="budgetDtlFundApplEntities"></param>
		/// <param name="budgetDtlBudgetDtlEntities"></param>
		/// <param name="budgetDtlPerformTargetEntities"></param>
		/// <param name="budgetDtlPurDtl4SOFEntities"></param>
		/// <param name="budgetDtlImplPlanEntities"></param>
        /// <returns></returns>
        //SavedResult<Int64> SaveBudgetDtlPurchaseDtl(BudgetDtlPurchaseDtlModel budgetDtlPurchaseDtlEntity, List<BudgetDtlPurchaseDtlModel> budgetDtlPurchaseDtlEntities, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlPurDtl4SOFModel> budgetDtlPurDtl4SOFEntities, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities);

		#endregion
    }
}
