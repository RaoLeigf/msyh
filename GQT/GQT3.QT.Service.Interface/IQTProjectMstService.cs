#region Summary
/**************************************************************************************
    * 命名空间：			GQT3.QT.Service.Interface
    * 类 名 称：			IQTProjectMstService
    * 文 件 名：			IQTProjectMstService.cs
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
using GQT3.QT.Model.Extra;

namespace GQT3.QT.Service.Interface
{
	/// <summary>
	/// QTProjectMst服务组装层接口
	/// </summary>
    public partial interface IQTProjectMstService : IEntServiceBase<QTProjectMstModel>
    {
		#region IQTProjectMstService 业务添加的成员

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

        /// <summary>
        /// 通过外键值获取QTProjectDtlBudgetDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlBudgetDtlModel> FindQTProjectDtlBudgetDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlFundAppl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlFundApplModel> FindQTProjectDtlFundApplByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlImplPlan明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlImplPlanModel> FindQTProjectDtlImplPlanByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlPerformTarget明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlPerformTargetModel> FindQTProjectDtlPerformTargetByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlPurchaseDtl明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlPurchaseDtlModel> FindQTProjectDtlPurchaseDtlByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlPurDtl4SOF明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlPurDtl4SOFModel> FindQTProjectDtlPurDtl4SOFByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 通过外键值获取QTProjectDtlTextContent明细数据
        /// </summary>
        /// <param name="id">外键值</param>
        /// <returns></returns>
        FindedResults<QTProjectDtlTextContentModel> FindQTProjectDtlTextContentByForeignKey<TValType>(TValType id);

        /// <summary>
        /// 申报部门项目汇总表
        /// </summary>
        /// <returns>返回Json串</returns>
        List<QTProjectMstHZModel> GetQTProjectMstHZ(Dictionary<string, object> dic, int pageIndex, int pageSize , out int TotalItems);

        #endregion
    }
}
