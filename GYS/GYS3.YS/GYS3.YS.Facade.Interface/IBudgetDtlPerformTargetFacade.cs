#region Summary
/**************************************************************************************
    * 类 名 称：        IBudgetDtlPerformTargetFacade
    * 命名空间：        GYS3.YS.Facade.Interface
    * 文 件 名：        IBudgetDtlPerformTargetFacade.cs
    * 创建时间：        2018/10/16 
    * 作    者：        夏华军    
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
	/// BudgetDtlPerformTarget业务组装层接口
	/// </summary>
    public partial interface IBudgetDtlPerformTargetFacade : IEntFacadeBase<BudgetDtlPerformTargetModel>
    {
		#region IBudgetDtlPerformTargetFacade 业务添加的成员

		///// <summary>
        ///// 方法实例
        ///// </summary>
        ///// <returns></returns>
        //List<BudgetDtlPerformTargetModel> ExampleMethod<BudgetDtlPerformTargetModel>(string param)

        /// <summary>
        /// 保存数据
        /// </summary>
		/// <param name="budgetDtlPerformTargetEntity"></param>
		/// <param name="budgetDtlTextContentEntities"></param>
		/// <param name="budgetDtlFundApplEntities"></param>
		/// <param name="budgetDtlBudgetDtlEntities"></param>
		/// <param name="budgetDtlPerformTargetEntities"></param>
		/// <param name="budgetDtlImplPlanEntities"></param>
        /// <returns></returns>
        SavedResult<Int64> SaveBudgetDtlPerformTarget(BudgetDtlPerformTargetModel budgetDtlPerformTargetEntity, List<BudgetDtlTextContentModel> budgetDtlTextContentEntities, List<BudgetDtlFundApplModel> budgetDtlFundApplEntities, List<BudgetDtlBudgetDtlModel> budgetDtlBudgetDtlEntities, List<BudgetDtlPerformTargetModel> budgetDtlPerformTargetEntities, List<BudgetDtlImplPlanModel> budgetDtlImplPlanEntities);

		#endregion
    }
}
