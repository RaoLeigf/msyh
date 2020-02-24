#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Facade.Interface
    * 类 名 称：			IQTProjectMstFacade
    * 文 件 名：			IQTProjectMstFacade.cs
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
using System.Text;
using Enterprise3.Common.Model.Results;
using Enterprise3.NHORM.Interface.EntBase;

using GQT3.QT.Model.Domain;

namespace GQT3.QT.Facade.Interface
{
	/// <summary>
	/// QTProjectMst业务组装层接口
	/// </summary>
    public partial interface IQTProjectMstFacade : IEntFacadeBase<QTProjectMstModel>
    {
		#region IQTProjectMstFacade 业务添加的成员

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
        SavedResult<Int64> SaveQTProjectMst(QTProjectMstModel qTProjectMstEntity, List<QTProjectDtlBudgetDtlModel> qTProjectDtlBudgetDtlEntities, List<QTProjectDtlFundApplModel> qTProjectDtlFundApplEntities, List<QTProjectDtlImplPlanModel> qTProjectDtlImplPlanEntities, List<QTProjectDtlPerformTargetModel> qTProjectDtlPerformTargetEntities, List<QTProjectDtlPurchaseDtlModel> qTProjectDtlPurchaseDtlEntities, List<QTProjectDtlPurDtl4SOFModel> qTProjectDtlPurDtl4SOFEntities, List<QTProjectDtlTextContentModel> qTProjectDtlTextContentEntities);

		#endregion
    }
}
